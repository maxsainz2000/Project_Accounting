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
    ''' Disposition line detail for customer return inspection.
    ''' </summary>
    Public Class ReturnDispositionLine
        Public Property ReturnLineId As Integer?
        Public Property ProductId As Integer
        Public Property RestockQty As Integer
        Public Property ScrapQty As Integer
        Public Property Notes As String = String.Empty

        Public Sub New()
        End Sub

        Public Sub New(productId As Integer, restockQty As Integer, scrapQty As Integer, Optional returnLineId As Integer? = Nothing)
            Me.ProductId = productId
            Me.RestockQty = restockQty
            Me.ScrapQty = scrapQty
            Me.ReturnLineId = returnLineId
        End Sub
    End Class

    ''' <summary>
    ''' Application workflow service orchestrating two-stage Customer Returns:
    ''' 1. Physical intake into non-valuated operational quarantine (QuarantineQty) with 0 General Ledger entries.
    ''' 2. Physical inspection disposition (restock vs. scrap), moving average cost recalculation,
    '''    Credit Note journal posting against Accounts Receivable, and optional refund disbursements.
    ''' </summary>
    Public Class ReturnsService

        Private ReadOnly _context As ErpDbContext
        Private ReadOnly _periodService As PeriodService
        Private ReadOnly _productRepo As ProductRepository
        Private ReadOnly _ledgerRepo As LedgerRepository
        Private ReadOnly _accountRepo As AccountRepository
        Private ReadOnly _docRepo As DocumentRepository

        Public Sub New(context As ErpDbContext, Optional periodService As PeriodService = Nothing)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            _context = context
            _periodService = If(periodService, New PeriodService(_context))
            _productRepo = New ProductRepository(_context)
            _ledgerRepo = New LedgerRepository(_context)
            _accountRepo = New AccountRepository(_context)
            _docRepo = New DocumentRepository(_context)
        End Sub

        Public Sub New()
            Me.New(New ErpDbContext())
        End Sub

        ''' <summary>
        ''' Receives customer returned goods into non-valuated operational quarantine.
        ''' Increments product QuarantineQty with ZERO (0) General Ledger entries.
        ''' </summary>
        ''' <param name="customerId">The customer identifier.</param>
        ''' <param name="invoiceId">The original Sales Invoice ID under which the goods were purchased.</param>
        ''' <param name="lines">Collection of customer return lines specifying returned quantities.</param>
        ''' <param name="returnDate">Optional physical return receipt date (defaults to today).</param>
        ''' <param name="documentNumber">Optional RMA document number (auto-generated if omitted).</param>
        ''' <returns>The created and tracked CustomerReturn voucher in 'Quarantine' status.</returns>
        Public Function ReceiveReturnToQuarantine(customerId As Integer, invoiceId As Integer, lines As IEnumerable(Of CustomerReturnLine), Optional returnDate As DateTime? = Nothing, Optional documentNumber As String = Nothing) As CustomerReturn
            If customerId <= 0 Then
                Throw New ArgumentException("Valid CustomerId must be specified.", NameOf(customerId))
            End If
            If invoiceId <= 0 Then
                Throw New ArgumentException("Valid original InvoiceId must be specified.", NameOf(invoiceId))
            End If
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Return lines cannot be null.")
            End If

            Dim lineList As List(Of CustomerReturnLine) = lines.ToList()
            If lineList.Count = 0 Then
                Throw New ArgumentException("Customer return must contain at least one line item.", NameOf(lines))
            End If

            Dim customer = _context.Customers.Find(customerId)
            If customer Is Nothing Then
                Throw New ArgumentException($"Customer with ID {customerId} was not found.", NameOf(customerId))
            End If

            Dim invoice = _docRepo.GetSalesInvoiceById(invoiceId)
            If invoice Is Nothing Then
                Throw New ArgumentException($"Sales Invoice with ID {invoiceId} was not found.", NameOf(invoiceId))
            End If
            If invoice.CustomerId <> customerId Then
                Throw New InvalidOperationException($"Sales Invoice {invoice.DocumentNumber} does not belong to Customer ID {customerId}.")
            End If

            For Each line In lineList
                If line.ProductId <= 0 Then
                    Throw New ArgumentException("Product ID must be greater than zero.", NameOf(lines))
                End If
                If line.ReturnedQty <= 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Returned quantity must be greater than zero.")
                End If

                Dim product = _productRepo.GetById(line.ProductId)
                If product Is Nothing Then
                    Throw New ArgumentException($"Product with ID {line.ProductId} was not found.", NameOf(lines))
                End If

                ' Infer selling price and original COGS unit cost from original invoice if not specified
                Dim invLine = invoice.Lines.FirstOrDefault(Function(l) l.ProductId = line.ProductId)
                If line.SellingUnitPrice <= 0D AndAlso invLine IsNot Nothing Then
                    line.SellingUnitPrice = invLine.UnitPrice
                End If
                If line.OriginalUnitCost <= 0D Then
                    If invLine IsNot Nothing AndAlso invLine.InvoicedCOGSUnitCost > 0D Then
                        line.OriginalUnitCost = invLine.InvoicedCOGSUnitCost
                    Else
                        line.OriginalUnitCost = product.CurrentUnitCost
                    End If
                End If
            Next

            Dim effectiveReturnDate As DateTime = If(returnDate.HasValue, returnDate.Value, DateTime.Today)
            Dim statutoryPostingDate As DateTime = _periodService.ResolveStatutoryPostingDate(effectiveReturnDate)
            Dim docNum As String = If(Not String.IsNullOrWhiteSpace(documentNumber), documentNumber, GenerateReturnNumber())

            Using tx = _context.Database.BeginTransaction()
                Dim retDoc As New CustomerReturn() With {
                    .CustomerId = customerId,
                    .Customer = customer,
                    .OriginalInvoiceId = invoiceId,
                    .OriginalInvoice = invoice,
                    .DocumentNumber = docNum,
                    .ReturnDate = effectiveReturnDate,
                    .PostingDate = statutoryPostingDate,
                    .Status = "Quarantine"
                }

                For Each line In lineList
                    ' Increment operational quarantine quantity on product (NO GL postings)
                    Dim product = _productRepo.GetById(line.ProductId)
                    product.QuarantineQty += line.ReturnedQty

                    Dim retLine As New CustomerReturnLine() With {
                        .ProductId = line.ProductId,
                        .ReturnedQty = line.ReturnedQty,
                        .RestockQty = 0,
                        .ScrapQty = 0,
                        .OriginalUnitCost = line.OriginalUnitCost,
                        .SellingUnitPrice = line.SellingUnitPrice,
                        .DispositionStatus = "Quarantine",
                        .CustomerReturn = retDoc
                    }
                    retDoc.Lines.Add(retLine)
                Next

                _context.CustomerReturns.Add(retDoc)
                _context.SaveChanges()
                tx.Commit()

                ' Crucial Invariant: 0 General Ledger entries generated during quarantine intake
                Return retDoc
            End Using
        End Function

        ''' <summary>
        ''' Processes physical return inspection dispositions (restock vs. scrap):
        ''' 1. Restock items: Debit 1300 Inventory, Credit 5000 COGS, recalculate perpetual moving average cost.
        ''' 2. Scrap items: Debit 5300 Inventory Shrinkage, Credit 5000 COGS.
        ''' 3. Credit Note: Debit 4100 Sales Returns &amp; Allowances, Credit 1200 AR (subledger credit).
        ''' </summary>
        ''' <param name="returnId">The CustomerReturn identifier.</param>
        ''' <param name="dispositions">Collection of disposition results detailing restocked and scrapped quantities per product.</param>
        ''' <param name="postingDate">Optional statutory General Ledger posting date.</param>
        ''' <returns>The inspected, settled, and GL-posted CustomerReturn entity.</returns>
        Public Function ProcessReturnInspection(returnId As Integer, dispositions As IEnumerable(Of ReturnDispositionLine), Optional postingDate As DateTime? = Nothing) As CustomerReturn
            If returnId <= 0 Then
                Throw New ArgumentException("Return ID must be greater than zero.", NameOf(returnId))
            End If
            If dispositions Is Nothing Then
                Throw New ArgumentNullException(NameOf(dispositions), "Inspection dispositions cannot be null.")
            End If

            Dim dispList As List(Of ReturnDispositionLine) = dispositions.ToList()
            If dispList.Count = 0 Then
                Throw New ArgumentException("Inspection must contain at least one disposition line.", NameOf(dispositions))
            End If

            Dim returnDoc = _docRepo.GetCustomerReturnById(returnId)
            If returnDoc Is Nothing Then
                Throw New ArgumentException($"Customer Return with ID {returnId} was not found.", NameOf(returnId))
            End If

            If String.Equals(returnDoc.Status, DocumentStatus.Posted.ToString(), StringComparison.OrdinalIgnoreCase) Then
                Throw New InvalidOperationException($"Customer Return {returnDoc.DocumentNumber} has already been inspected and posted.")
            End If

            Dim effectiveEventDate As DateTime = If(returnDoc.ReturnDate <> DateTime.MinValue, returnDoc.ReturnDate, DateTime.Today)
            Dim statutoryPostingDate As DateTime = If(postingDate.HasValue, postingDate.Value, _periodService.ResolveStatutoryPostingDate(effectiveEventDate))

            ' Validate statutory posting period
            Dim period = _periodService.ValidatePostingDate(statutoryPostingDate)

            Using tx = _context.Database.BeginTransaction()
                Dim customer = _context.Customers.Find(returnDoc.CustomerId)
                Dim totalReturnAllowance As Decimal = 0D

                For Each disp In dispList
                    If disp.RestockQty < 0 OrElse disp.ScrapQty < 0 Then
                        Throw New ArgumentOutOfRangeException(NameOf(dispositions), "Restock and Scrap quantities cannot be negative.")
                    End If

                    Dim totalInspected As Integer = disp.RestockQty + disp.ScrapQty
                    If totalInspected <= 0 Then
                        Throw New ArgumentException("Total inspected quantity (Restock + Scrap) must be greater than zero.", NameOf(dispositions))
                    End If

                    ' Match return line by ReturnLineId or ProductId
                    Dim retLine As CustomerReturnLine = Nothing
                    If disp.ReturnLineId.HasValue AndAlso disp.ReturnLineId.Value > 0 Then
                        retLine = returnDoc.Lines.FirstOrDefault(Function(l) l.ReturnLineId = disp.ReturnLineId.Value)
                    Else
                        retLine = returnDoc.Lines.FirstOrDefault(Function(l) l.ProductId = disp.ProductId)
                    End If

                    If retLine Is Nothing Then
                        Throw New ArgumentException($"Return line for Product ID {disp.ProductId} was not found on Return {returnDoc.DocumentNumber}.", NameOf(dispositions))
                    End If

                    If totalInspected > retLine.ReturnedQty Then
                        Throw New InvalidOperationException($"Cannot inspect {totalInspected} units. Only {retLine.ReturnedQty} units were returned in quarantine.")
                    End If

                    Dim product = _productRepo.GetById(retLine.ProductId)
                    If product Is Nothing Then
                        Throw New ArgumentException($"Product with ID {retLine.ProductId} was not found.", NameOf(dispositions))
                    End If

                    ' Decrement operational quarantine stock
                    product.QuarantineQty = Math.Max(0, product.QuarantineQty - totalInspected)

                    ' Restock processing
                    If disp.RestockQty > 0 Then
                        ' Recalculate perpetual moving average cost forward
                        Dim restockCost = InventoryCostingEngine.CalculateRestockMovingAverageCost(
                            product.TotalQtyOnHand,
                            product.CurrentUnitCost,
                            disp.RestockQty,
                            retLine.OriginalUnitCost)

                        product.CurrentUnitCost = restockCost
                        product.TotalQtyOnHand += disp.RestockQty

                        ' Append InventoryTransaction audit entry
                        Dim invTx As New InventoryTransaction() With {
                            .ProductId = product.ProductId,
                            .SourceDocType = "CustomerReturn",
                            .SourceDocId = returnDoc.ReturnId,
                            .OperationalEventDate = returnDoc.ReturnDate,
                            .PostingDate = statutoryPostingDate,
                            .QtyChange = disp.RestockQty,
                            .UnitCost = retLine.OriginalUnitCost,
                            .RunningQty = product.TotalQtyOnHand,
                            .RunningUnitCost = product.CurrentUnitCost
                        }
                        _context.InventoryTransactions.Add(invTx)
                    End If

                    ' Update return line disposition details
                    retLine.RestockQty = disp.RestockQty
                    retLine.ScrapQty = disp.ScrapQty
                    retLine.DispositionStatus = If(disp.ScrapQty > 0 AndAlso disp.RestockQty = 0,
                                                  ReturnDisposition.Scrap.ToString(),
                                                  If(disp.RestockQty > 0 AndAlso disp.ScrapQty = 0,
                                                     ReturnDisposition.Restock.ToString(),
                                                     "Mixed"))

                    totalReturnAllowance += Math.Round(CDec(totalInspected) * retLine.SellingUnitPrice, 4, MidpointRounding.AwayFromZero)
                Next

                ' Emit balanced Credit Note journal:
                ' Restock: Debit 1300 Inventory, Credit 5000 COGS
                ' Scrap: Debit 5300 Inventory Shrinkage, Credit 5000 COGS
                ' Allowance: Debit 4100 Sales Returns & Allowances, Credit 1200 AR
                returnDoc.PostingDate = statutoryPostingDate
                Dim journalEntry = LedgerPostingEngine.GenerateCreditNoteJournal(returnDoc, returnDoc.Lines, GetAccountLookup())
                journalEntry.PeriodId = period.PeriodId
                journalEntry.PostingDate = statutoryPostingDate
                journalEntry.OperationalEventDate = returnDoc.ReturnDate
                _context.JournalEntries.Add(journalEntry)

                ' Update customer AR subledger balance (credit memo decreases AR balance or creates customer credit)
                If customer IsNot Nothing Then
                    customer.CurrentARBalance -= totalReturnAllowance
                End If

                returnDoc.Status = DocumentStatus.Posted.ToString()

                _context.SaveChanges()
                tx.Commit()

                Return returnDoc
            End Using
        End Function

        ''' <summary>
        ''' Overload for ProcessReturnInspection accepting CustomerReturnLine instances.
        ''' </summary>
        Public Function ProcessReturnInspection(returnId As Integer, dispositions As IEnumerable(Of CustomerReturnLine), Optional postingDate As DateTime? = Nothing) As CustomerReturn
            If dispositions Is Nothing Then
                Throw New ArgumentNullException(NameOf(dispositions))
            End If

            Dim lines As New List(Of ReturnDispositionLine)()
            For Each line In dispositions
                lines.Add(New ReturnDispositionLine() With {
                    .ReturnLineId = If(line.ReturnLineId > 0, CType(line.ReturnLineId, Integer?), Nothing),
                    .ProductId = line.ProductId,
                    .RestockQty = line.RestockQty,
                    .ScrapQty = line.ScrapQty
                })
            Next

            Return ProcessReturnInspection(returnId, lines, postingDate)
        End Function

        ''' <summary>
        ''' Disburses a customer cash/bank refund clearing a customer's credit balance on Accounts Receivable:
        ''' Debit: 1200 Accounts Receivable
        ''' Credit: 1020 Operating Bank Account
        ''' </summary>
        Public Function DisburseCustomerRefund(customerId As Integer, returnId As Integer?, amount As Decimal, bankAccountId As Integer, Optional paymentDate As DateTime? = Nothing, Optional postingDate As DateTime? = Nothing, Optional voucherNumber As String = Nothing) As PaymentVoucher
            If customerId <= 0 Then
                Throw New ArgumentException("Valid CustomerId must be specified.", NameOf(customerId))
            End If
            If amount <= 0D Then
                Throw New ArgumentOutOfRangeException(NameOf(amount), "Refund disbursement amount must be greater than zero.")
            End If

            Dim customer = _context.Customers.Find(customerId)
            If customer Is Nothing Then
                Throw New ArgumentException($"Customer with ID {customerId} was not found.", NameOf(customerId))
            End If

            Dim effectivePaymentDate As DateTime = If(paymentDate.HasValue, paymentDate.Value, DateTime.Today)
            Dim statutoryPostingDate As DateTime = If(postingDate.HasValue, postingDate.Value, _periodService.ResolveStatutoryPostingDate(effectivePaymentDate))

            Dim period = _periodService.ValidatePostingDate(statutoryPostingDate)

            Dim resolvedBankAccountId As Integer = bankAccountId
            If resolvedBankAccountId <= 0 Then
                Dim defaultBank = _accountRepo.GetByCode(AccountCodes.OperatingBank.ToString())
                If defaultBank IsNot Nothing Then
                    resolvedBankAccountId = defaultBank.AccountId
                Else
                    resolvedBankAccountId = AccountCodes.OperatingBank
                End If
            End If

            Dim vNum As String = If(Not String.IsNullOrWhiteSpace(voucherNumber), voucherNumber, $"REF-{DateTime.Today:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpperInvariant()}")

            Using tx = _context.Database.BeginTransaction()
                Dim voucher As New PaymentVoucher() With {
                    .VoucherType = "CustomerRefund",
                    .CustomerId = customerId,
                    .TargetDocId = If(returnId.HasValue, returnId.Value, 0),
                    .VoucherNumber = vNum,
                    .PaymentDate = effectivePaymentDate,
                    .PostingDate = statutoryPostingDate,
                    .DisbursedAmount = amount,
                    .BankAccountId = resolvedBankAccountId
                }

                _context.PaymentVouchers.Add(voucher)
                _context.SaveChanges()

                ' Emit balanced General Ledger entry: Debit 1200 AR, Credit 1020 Operating Bank
                Dim journalEntry = LedgerPostingEngine.GeneratePaymentVoucherJournal(voucher, GetAccountLookup())
                journalEntry.PeriodId = period.PeriodId
                journalEntry.PostingDate = statutoryPostingDate
                journalEntry.OperationalEventDate = effectivePaymentDate
                _context.JournalEntries.Add(journalEntry)

                ' Update customer AR subledger balance (refunding credit balance brings AR back towards zero)
                customer.CurrentARBalance += amount

                _context.SaveChanges()
                tx.Commit()

                Return voucher
            End Using
        End Function

        ''' <summary>
        ''' Resolves account code to primary key ID in the current database.
        ''' </summary>
        Private Function GetAccountLookup() As Func(Of String, Integer)
            Return Function(code As String) As Integer
                Dim acct = _accountRepo.GetByCode(code)
                If acct IsNot Nothing Then
                    Return acct.AccountId
                End If
                Return Integer.Parse(code)
            End Function
        End Function

        Private Shared Function GenerateReturnNumber() As String
            Return $"RMA-{DateTime.Today:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpperInvariant()}"
        End Function

    End Class

End Namespace
