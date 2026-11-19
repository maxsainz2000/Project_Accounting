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
    ''' Application workflow service orchestrating the Order-to-Cash cycle:
    ''' commercial Sales Orders, Sales Invoice fulfillment (with atomic conditional stock decrement and GL posting),
    ''' and Customer Payment Receipts clearing Accounts Receivable.
    ''' </summary>
    Public Class OrderToCashService

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
        ''' Creates and persists a commercial Sales Order commitment.
        ''' </summary>
        ''' <param name="customerId">The customer identifier.</param>
        ''' <param name="lines">Collection of sales order lines specifying product, ordered quantity, and unit price.</param>
        ''' <param name="orderDate">Optional order date (defaults to current date).</param>
        ''' <param name="documentNumber">Optional document number (auto-generated if omitted).</param>
        ''' <returns>The created and tracked SalesOrder entity.</returns>
        Public Function CreateSalesOrder(customerId As Integer, lines As IEnumerable(Of SOLine), Optional orderDate As DateTime? = Nothing, Optional documentNumber As String = Nothing) As SalesOrder
            If customerId <= 0 Then
                Throw New ArgumentException("Valid CustomerId must be specified.", NameOf(customerId))
            End If
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Sales order lines cannot be null.")
            End If

            Dim lineList As List(Of SOLine) = lines.ToList()
            If lineList.Count = 0 Then
                Throw New ArgumentException("Sales order must contain at least one line item.", NameOf(lines))
            End If

            Dim customer = _context.Customers.Find(customerId)
            If customer Is Nothing Then
                Throw New ArgumentException($"Customer with ID {customerId} was not found.", NameOf(customerId))
            End If

            For Each line In lineList
                If line.ProductId <= 0 Then
                    Throw New ArgumentException("Product ID must be greater than zero.", NameOf(lines))
                End If
                If line.OrderedQty <= 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Ordered quantity must be greater than zero.")
                End If
                If line.UnitPrice < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Unit price cannot be negative.")
                End If

                Dim product = _productRepo.GetById(line.ProductId)
                If product Is Nothing Then
                    Throw New ArgumentException($"Product with ID {line.ProductId} was not found.", NameOf(lines))
                End If
            Next

            Dim effectiveOrderDate As DateTime = If(orderDate.HasValue, orderDate.Value, DateTime.Today)
            Dim docNum As String = If(Not String.IsNullOrWhiteSpace(documentNumber), documentNumber, GenerateSalesOrderNumber())

            Dim salesOrder As New SalesOrder() With {
                .CustomerId = customerId,
                .Customer = customer,
                .DocumentNumber = docNum,
                .OrderDate = effectiveOrderDate,
                .Status = DocumentStatus.Draft.ToString()
            }

            For Each line In lineList
                Dim soLine As New SOLine() With {
                    .ProductId = line.ProductId,
                    .OrderedQty = line.OrderedQty,
                    .UnitPrice = line.UnitPrice,
                    .SalesOrder = salesOrder
                }
                salesOrder.Lines.Add(soLine)
            Next

            _context.SalesOrders.Add(salesOrder)
            _context.SaveChanges()

            Return salesOrder
        End Function

        ''' <summary>
        ''' Fulfills all lines of an existing Sales Order as a Sales Invoice.
        ''' </summary>
        Public Function FulfillSalesInvoice(soId As Integer, postingDate As DateTime) As SalesInvoice
            If soId <= 0 Then
                Throw New ArgumentException("Sales Order ID must be greater than zero.", NameOf(soId))
            End If

            Dim so = _docRepo.GetSalesOrderById(soId)
            If so Is Nothing Then
                Throw New ArgumentException($"Sales Order with ID {soId} was not found.", NameOf(soId))
            End If

            If so.Lines.Count = 0 Then
                Throw New InvalidOperationException($"Sales Order {so.DocumentNumber} contains no lines to fulfill.")
            End If

            Dim invoiceLines As New List(Of SalesInvoiceLine)()
            For Each soLine In so.Lines
                invoiceLines.Add(New SalesInvoiceLine() With {
                    .SOLineId = soLine.SOLineId,
                    .ProductId = soLine.ProductId,
                    .FulfilledQty = soLine.OrderedQty,
                    .UnitPrice = soLine.UnitPrice
                })
            Next

            Return FulfillSalesInvoice(soId, invoiceLines, postingDate, so.CustomerId)
        End Function

        ''' <summary>
        ''' Fulfills a Sales Invoice for a given Sales Order or customer:
        ''' 1. Validates stock pre-check via InventoryCostingEngine.ValidateSufficientStock.
        ''' 2. Executes atomic conditional update via ProductRepository. Throw InsufficientStockException on shortfall.
        ''' 3. Emits balanced journal: Debit 1200 AR, Credit 4000 Sales Revenue, Debit 5000 COGS, Credit 1300 Inventory.
        ''' </summary>
        ''' <param name="soId">The Sales Order ID, or 0 if a direct invoice without a pre-existing SO.</param>
        ''' <param name="lines">Fulfillment line items.</param>
        ''' <param name="postingDate">Statutory General Ledger posting date.</param>
        ''' <param name="customerId">Optional customer identifier (inferred from SO if omitted).</param>
        ''' <param name="invoiceDate">Optional physical invoice date (defaults to posting date).</param>
        ''' <param name="documentNumber">Optional invoice document number (auto-generated if omitted).</param>
        ''' <returns>The posted and balanced SalesInvoice entity.</returns>
        Public Function FulfillSalesInvoice(soId As Integer, lines As IEnumerable(Of SalesInvoiceLine), postingDate As DateTime, Optional customerId As Integer = 0, Optional invoiceDate As DateTime? = Nothing, Optional documentNumber As String = Nothing) As SalesInvoice
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Fulfillment lines cannot be null.")
            End If

            Dim lineList As List(Of SalesInvoiceLine) = lines.ToList()
            If lineList.Count = 0 Then
                Throw New ArgumentException("Sales invoice must contain at least one line item.", NameOf(lines))
            End If

            Dim resolvedCustomerId As Integer = customerId
            Dim salesOrder As SalesOrder = Nothing

            If soId > 0 Then
                salesOrder = _docRepo.GetSalesOrderById(soId)
                If salesOrder Is Nothing Then
                    Throw New ArgumentException($"Sales Order with ID {soId} was not found.", NameOf(soId))
                End If
                If resolvedCustomerId <= 0 Then
                    resolvedCustomerId = salesOrder.CustomerId
                End If
            End If

            If resolvedCustomerId <= 0 Then
                Throw New ArgumentException("A valid Customer ID must be specified directly or associated with the Sales Order.", NameOf(customerId))
            End If

            Dim customer = _context.Customers.Find(resolvedCustomerId)
            If customer Is Nothing Then
                Throw New ArgumentException($"Customer with ID {resolvedCustomerId} was not found.", NameOf(customerId))
            End If

            ' 1. Validate statutory posting period
            Dim period = _periodService.ValidatePostingDate(postingDate)

            ' 2. Validate line input & stock pre-check via InventoryCostingEngine.ValidateSufficientStock
            For Each line In lineList
                If line.ProductId <= 0 Then
                    Throw New ArgumentException("Product ID must be greater than zero.", NameOf(lines))
                End If
                If line.FulfilledQty <= 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Fulfilled quantity must be greater than zero.")
                End If
                If line.UnitPrice < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Unit price cannot be negative.")
                End If

                Dim product = _productRepo.GetById(line.ProductId)
                If product Is Nothing Then
                    Throw New ArgumentException($"Product with ID {line.ProductId} was not found.", NameOf(lines))
                End If

                If Not InventoryCostingEngine.ValidateSufficientStock(product.TotalQtyOnHand, line.FulfilledQty) Then
                    Throw New InsufficientStockException(line.ProductId, line.FulfilledQty, product.TotalQtyOnHand)
                End If
            Next

            Dim effectiveInvoiceDate As DateTime = If(invoiceDate.HasValue, invoiceDate.Value, postingDate)
            Dim docNum As String = If(Not String.IsNullOrWhiteSpace(documentNumber), documentNumber, GenerateInvoiceNumber())

            ' 3. Execute atomic transaction
            Using tx = _context.Database.BeginTransaction()
                ' Step A: Atomic conditional stock decrement via ProductRepository
                For Each line In lineList
                    Dim product = _productRepo.GetById(line.ProductId)
                    Dim success As Boolean = _productRepo.DecrementStock(line.ProductId, line.FulfilledQty)
                    If Not success Then
                        ' Atomic conditional update failed (concurrent depletion or insufficient stock)
                        Dim currentProduct = _productRepo.GetById(line.ProductId)
                        Dim currentQty As Integer = If(currentProduct IsNot Nothing, currentProduct.TotalQtyOnHand, 0)
                        Throw New InsufficientStockException(line.ProductId, line.FulfilledQty, currentQty)
                    End If

                    ' Record current perpetual moving average COGS cost
                    line.InvoicedCOGSUnitCost = product.CurrentUnitCost
                Next

                ' Step B: Calculate total invoice amount
                Dim totalAmount As Decimal = lineList.Sum(Function(l) Math.Round(CDec(l.FulfilledQty) * l.UnitPrice, 4, MidpointRounding.AwayFromZero))

                ' Step C: Create and persist SalesInvoice
                Dim invoice As New SalesInvoice() With {
                    .SOId = If(soId > 0, CType(soId, Integer?), Nothing),
                    .CustomerId = resolvedCustomerId,
                    .DocumentNumber = docNum,
                    .InvoiceDate = effectiveInvoiceDate,
                    .PostingDate = postingDate,
                    .DueDate = postingDate.AddDays(30),
                    .TotalAmount = totalAmount,
                    .Status = DocumentStatus.Posted.ToString()
                }

                For Each line In lineList
                    Dim invoiceLine As New SalesInvoiceLine() With {
                        .ProductId = line.ProductId,
                        .SOLineId = line.SOLineId,
                        .FulfilledQty = line.FulfilledQty,
                        .UnitPrice = line.UnitPrice,
                        .InvoicedCOGSUnitCost = line.InvoicedCOGSUnitCost,
                        .SalesInvoice = invoice
                    }
                    invoice.Lines.Add(invoiceLine)
                Next

                _context.SalesInvoices.Add(invoice)
                _context.SaveChanges()

                ' Step D: Append InventoryTransactions
                For Each invLine In invoice.Lines
                    Dim currentProduct = _productRepo.GetById(invLine.ProductId)
                    Dim invTx As New InventoryTransaction() With {
                        .ProductId = invLine.ProductId,
                        .SourceDocType = "SalesInvoice",
                        .SourceDocId = invoice.InvoiceId,
                        .OperationalEventDate = invoice.InvoiceDate,
                        .PostingDate = postingDate,
                        .QtyChange = -invLine.FulfilledQty,
                        .UnitCost = invLine.InvoicedCOGSUnitCost,
                        .RunningQty = currentProduct.TotalQtyOnHand,
                        .RunningUnitCost = currentProduct.CurrentUnitCost
                    }
                    _context.InventoryTransactions.Add(invTx)
                Next

                ' Step E: Emit balanced journal entry: Debit 1200 AR, Credit 4000 Sales Revenue, Debit 5000 COGS, Credit 1300 Inventory
                Dim journalEntry = LedgerPostingEngine.GenerateSalesInvoiceJournal(invoice, invoice.Lines, GetAccountLookup())
                journalEntry.PeriodId = period.PeriodId
                _context.JournalEntries.Add(journalEntry)

                ' Step F: Update customer AR subledger balance
                customer.CurrentARBalance += totalAmount

                ' Step G: If backed by SalesOrder, update status
                If salesOrder IsNot Nothing Then
                    salesOrder.Status = DocumentStatus.Settled.ToString()
                End If

                _context.SaveChanges()
                tx.Commit()

                Return invoice
            End Using
        End Function

        ''' <summary>
        ''' Records a Customer Payment Receipt clearing Accounts Receivable:
        ''' Debit: 1020 Operating Bank Account
        ''' Credit: 1200 Accounts Receivable
        ''' </summary>
        ''' <param name="customerId">The customer identifier.</param>
        ''' <param name="invoiceId">Optional Sales Invoice ID being settled (can be null for unapplied receipts).</param>
        ''' <param name="amount">Payment received amount.</param>
        ''' <param name="bankAccountId">Bank asset account identifier (defaults to 1020 Operating Bank if 0).</param>
        ''' <param name="receiptDate">Optional receipt event date.</param>
        ''' <param name="postingDate">Optional statutory posting date (defaults to receipt date or today).</param>
        ''' <returns>The created and posted PaymentReceipt entity.</returns>
        Public Function RecordPaymentReceipt(customerId As Integer, invoiceId As Integer?, amount As Decimal, bankAccountId As Integer, Optional receiptDate As DateTime? = Nothing, Optional postingDate As DateTime? = Nothing) As PaymentReceipt
            If customerId <= 0 Then
                Throw New ArgumentException("Valid CustomerId must be specified.", NameOf(customerId))
            End If
            If amount <= 0D Then
                Throw New ArgumentOutOfRangeException(NameOf(amount), "Payment receipt amount must be strictly greater than zero.")
            End If

            Dim customer = _context.Customers.Find(customerId)
            If customer Is Nothing Then
                Throw New ArgumentException($"Customer with ID {customerId} was not found.", NameOf(customerId))
            End If

            Dim invoice As SalesInvoice = Nothing
            If invoiceId.HasValue AndAlso invoiceId.Value > 0 Then
                invoice = _context.SalesInvoices.Find(invoiceId.Value)
                If invoice Is Nothing Then
                    Throw New ArgumentException($"Sales Invoice with ID {invoiceId.Value} was not found.", NameOf(invoiceId))
                End If
                If invoice.CustomerId <> customerId Then
                    Throw New InvalidOperationException($"Sales Invoice {invoice.DocumentNumber} does not belong to Customer ID {customerId}.")
                End If
            End If

            Dim effectiveReceiptDate As DateTime = If(receiptDate.HasValue, receiptDate.Value, DateTime.Today)
            Dim effectivePostingDate As DateTime = If(postingDate.HasValue, postingDate.Value, effectiveReceiptDate)

            ' Validate statutory posting period
            Dim period = _periodService.ValidatePostingDate(effectivePostingDate)

            ' Resolve bank account
            Dim resolvedBankAccountId As Integer = bankAccountId
            If resolvedBankAccountId <= 0 Then
                Dim defaultBank = _accountRepo.GetByCode(AccountCodes.OperatingBank.ToString())
                If defaultBank IsNot Nothing Then
                    resolvedBankAccountId = defaultBank.AccountId
                Else
                    resolvedBankAccountId = AccountCodes.OperatingBank
                End If
            End If

            Using tx = _context.Database.BeginTransaction()
                Dim receipt As New PaymentReceipt(customerId, amount, resolvedBankAccountId, invoiceId) With {
                    .ReceiptDate = effectiveReceiptDate,
                    .PostingDate = effectivePostingDate
                }

                _context.PaymentReceipts.Add(receipt)
                _context.SaveChanges()

                ' Emit balanced General Ledger entry: Debit 1020 Operating Bank, Credit 1200 AR
                Dim journalEntry = LedgerPostingEngine.GeneratePaymentReceiptJournal(receipt, GetAccountLookup())
                journalEntry.PeriodId = period.PeriodId
                _context.JournalEntries.Add(journalEntry)

                ' Update Customer subledger AR balance
                customer.CurrentARBalance -= amount

                ' Check invoice settlement
                If invoice IsNot Nothing Then
                    Dim targetInvoiceId As Integer = invoice.InvoiceId
                    Dim totalReceiptsForInvoice As Decimal = _context.PaymentReceipts _
                        .Where(Function(r) r.InvoiceId.HasValue AndAlso r.InvoiceId.Value = targetInvoiceId) _
                        .Sum(Function(r) r.AmountReceived)

                    If totalReceiptsForInvoice >= invoice.TotalAmount Then
                        invoice.Status = DocumentStatus.Settled.ToString()
                    End If
                End If

                _context.SaveChanges()
                tx.Commit()

                Return receipt
            End Using
        End Function

        ''' <summary>
        ''' Overload for RecordPaymentReceipt accepting an Integer invoiceId (0 indicates unapplied receipt).
        ''' </summary>
        Public Function RecordPaymentReceipt(customerId As Integer, invoiceId As Integer, amount As Decimal, bankAccountId As Integer, Optional receiptDate As DateTime? = Nothing, Optional postingDate As DateTime? = Nothing) As PaymentReceipt
            Dim nullableInvoiceId As Integer? = If(invoiceId > 0, CType(invoiceId, Integer?), Nothing)
            Return RecordPaymentReceipt(customerId, nullableInvoiceId, amount, bankAccountId, receiptDate, postingDate)
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

        Private Shared Function GenerateSalesOrderNumber() As String
            Return $"SO-{DateTime.Today:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpperInvariant()}"
        End Function

        Private Shared Function GenerateInvoiceNumber() As String
            Return $"INV-{DateTime.Today:yyyyMMdd}-{Guid.NewGuid().ToString("N").Substring(0, 4).ToUpperInvariant()}"
        End Function

    End Class

End Namespace
