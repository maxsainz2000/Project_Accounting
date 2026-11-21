Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Data.Repositories
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Engines
Imports Merchandising_Accounting_App.Domain.Models

Namespace Services

    ''' <summary>
    ''' Application workflow service managing inventory adjustments, cycle count variances,
    ''' stock write-downs/shrinkage, and surplus gains with atomic database updates and balanced GL postings.
    ''' </summary>
    Public Class InventoryService

        Private ReadOnly _context As ErpDbContext
        Private ReadOnly _periodService As PeriodService
        Private ReadOnly _productRepo As ProductRepository
        Private ReadOnly _accountRepo As AccountRepository

        Public Sub New(context As ErpDbContext, Optional periodService As PeriodService = Nothing)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            _context = context
            _periodService = If(periodService, New PeriodService(_context))
            _productRepo = New ProductRepository(_context)
            _accountRepo = New AccountRepository(_context)
        End Sub

        Public Sub New()
            Me.New(New ErpDbContext())
        End Sub

        ''' <summary>
        ''' Records and posts an inventory adjustment (shrinkage, damage, count surplus, or count deficit).
        ''' Enforces zero negative stock for shrinkage, executes atomic conditional raw SQL updates,
        ''' generates balanced General Ledger journal entries, and records append-only inventory transaction audit logs.
        ''' </summary>
        ''' <param name="documentNumber">Optional document number (auto-generated if omitted).</param>
        ''' <param name="reason">The adjustment reason (Shrinkage, Damage, CountSurplus, CountDeficit).</param>
        ''' <param name="lines">Collection of adjustment lines detailing product, quantity change, unit cost, and notes.</param>
        ''' <param name="adjustmentDate">Optional operational date of physical count or write-down.</param>
        ''' <param name="notes">Optional overall adjustment memo.</param>
        ''' <param name="postingDate">Optional explicit posting date (auto-shifts if omitted or in closed period).</param>
        ''' <returns>The persisted and posted InventoryAdjustment entity.</returns>
        Public Function RecordInventoryAdjustment(documentNumber As String,
                                                  reason As String,
                                                  lines As IEnumerable(Of InventoryAdjustmentLine),
                                                  Optional adjustmentDate As DateTime? = Nothing,
                                                  Optional notes As String = Nothing,
                                                  Optional postingDate As DateTime? = Nothing) As InventoryAdjustment
            If String.IsNullOrWhiteSpace(reason) Then
                Throw New ArgumentException("Adjustment reason must be specified.", NameOf(reason))
            End If
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Adjustment lines cannot be null.")
            End If

            Dim lineList As List(Of InventoryAdjustmentLine) = lines.ToList()
            If lineList.Count = 0 Then
                Throw New ArgumentException("Inventory adjustment must contain at least one line item.", NameOf(lines))
            End If

            If lineList.All(Function(l) l.QtyChange = 0) Then
                Throw New ArgumentException("Inventory adjustment must contain at least one non-zero quantity line item.", NameOf(lines))
            End If

            ' 1. Resolve dates and validate fiscal period
            Dim effectiveAdjustmentDate As DateTime = If(adjustmentDate.HasValue, adjustmentDate.Value, DateTime.Today)
            Dim statutoryPostingDate As DateTime = If(postingDate.HasValue, postingDate.Value, _periodService.ResolveStatutoryPostingDate(effectiveAdjustmentDate))
            Dim period As AccountingPeriod = _periodService.ValidatePostingDate(statutoryPostingDate)

            ' 2. Pre-validate products, stock availability, and assign perpetual unit costs
            For Each line In lineList
                If line.ProductId <= 0 Then
                    Throw New ArgumentException("Product ID must be greater than zero.", NameOf(lines))
                End If

                Dim product = _productRepo.GetById(line.ProductId)
                If product Is Nothing Then
                    Throw New ArgumentException($"Product with ID {line.ProductId} was not found.", NameOf(lines))
                End If

                If line.UnitCost <= 0D Then
                    line.UnitCost = product.CurrentUnitCost
                End If

                If line.QtyChange < 0 Then
                    Dim shrinkageQty As Integer = Math.Abs(line.QtyChange)
                    If product.TotalQtyOnHand < shrinkageQty Then
                        Throw New InsufficientStockException(line.ProductId, shrinkageQty, product.TotalQtyOnHand,
                            $"Cannot adjust stock for Product ID {line.ProductId} ({product.SKU}). Requested reduction: {shrinkageQty}, Available on hand: {product.TotalQtyOnHand}.")
                    End If
                End If
            Next

            Dim docNum As String = If(Not String.IsNullOrWhiteSpace(documentNumber), documentNumber.Trim(), GenerateAdjustmentNumber())

            Using tx = _context.Database.BeginTransaction()
                ' 3. Execute atomic database updates
                For Each line In lineList
                    If line.QtyChange < 0 Then
                        Dim qtyToDeduct As Integer = Math.Abs(line.QtyChange)
                        Dim decremented = _productRepo.DecrementStock(line.ProductId, qtyToDeduct)
                        If Not decremented Then
                            Dim currentProduct = _productRepo.GetById(line.ProductId)
                            Dim currentQty = If(currentProduct IsNot Nothing, currentProduct.TotalQtyOnHand, 0)
                            Throw New InsufficientStockException(line.ProductId, qtyToDeduct, currentQty)
                        End If
                    ElseIf line.QtyChange > 0 Then
                        _productRepo.IncrementStock(line.ProductId, line.QtyChange)
                    End If
                Next

                ' 4. Create and persist InventoryAdjustment header
                Dim adjustment As New InventoryAdjustment() With {
                    .DocumentNumber = docNum,
                    .AdjustmentDate = effectiveAdjustmentDate,
                    .PostingDate = statutoryPostingDate,
                    .ReasonCode = reason,
                    .Status = DocumentStatus.Posted.ToString()
                }

                Dim shrinkageAccountId As Integer = GetAccountLookup()(AccountCodes.InventoryShrinkage.ToString())
                Dim surplusAccountId As Integer = GetAccountLookup()(AccountCodes.StocktakeVariance.ToString())

                For Each line In lineList
                    Dim targetAcctId As Integer = line.TargetAccountId
                    If targetAcctId <= 0 Then
                        targetAcctId = If(line.QtyChange < 0, shrinkageAccountId, surplusAccountId)
                    End If

                    Dim adjLine As New InventoryAdjustmentLine() With {
                        .Adjustment = adjustment,
                        .ProductId = line.ProductId,
                        .QtyChange = line.QtyChange,
                        .UnitCost = line.UnitCost,
                        .TargetAccountId = targetAcctId,
                        .Notes = If(Not String.IsNullOrWhiteSpace(line.Notes), line.Notes, notes)
                    }
                    adjustment.Lines.Add(adjLine)
                Next

                _context.InventoryAdjustments.Add(adjustment)
                _context.SaveChanges()

                ' 5. Record append-only InventoryTransaction audit records
                For Each adjLine In adjustment.Lines
                    If adjLine.QtyChange = 0 Then Continue For

                    Dim reloadedProduct = _productRepo.GetById(adjLine.ProductId)
                    Dim invTx As New InventoryTransaction() With {
                        .ProductId = adjLine.ProductId,
                        .SourceDocType = "InventoryAdjustment",
                        .SourceDocId = adjustment.AdjustmentId,
                        .OperationalEventDate = effectiveAdjustmentDate,
                        .PostingDate = statutoryPostingDate,
                        .QtyChange = adjLine.QtyChange,
                        .UnitCost = adjLine.UnitCost,
                        .RunningQty = reloadedProduct.TotalQtyOnHand,
                        .RunningUnitCost = reloadedProduct.CurrentUnitCost
                    }
                    _context.InventoryTransactions.Add(invTx)
                Next

                ' 6. Emit balanced GL journal entry via LedgerPostingEngine
                Dim journalEntry = LedgerPostingEngine.GenerateInventoryAdjustmentJournal(adjustment, adjustment.Lines, GetAccountLookup())
                journalEntry.PeriodId = period.PeriodId
                journalEntry.PostingDate = statutoryPostingDate
                journalEntry.OperationalEventDate = effectiveAdjustmentDate
                _context.JournalEntries.Add(journalEntry)

                _context.SaveChanges()
                tx.Commit()

                Return adjustment
            End Using
        End Function

        Public Function GetAdjustmentById(adjustmentId As Integer) As InventoryAdjustment
            Return _context.InventoryAdjustments _
                .Include(Function(a) a.Lines).ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(a) a.AdjustmentId = adjustmentId)
        End Function

        Public Function GetAllAdjustments() As List(Of InventoryAdjustment)
            Return _context.InventoryAdjustments _
                .Include(Function(a) a.Lines).ThenInclude(Function(l) l.Product) _
                .OrderByDescending(Function(a) a.AdjustmentDate) _
                .ToList()
        End Function

        Private Function GetAccountLookup() As Func(Of String, Integer)
            Return Function(code As String) As Integer
                Dim acct = _accountRepo.GetByCode(code)
                If acct IsNot Nothing Then
                    Return acct.AccountId
                End If
                Return Integer.Parse(code)
            End Function
        End Function

        Private Shared Function GenerateAdjustmentNumber() As String
            Return $"ADJ-{DateTime.Today:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpperInvariant()}"
        End Function

    End Class

End Namespace
