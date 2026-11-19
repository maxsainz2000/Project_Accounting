Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Engines
Imports Merchandising_Accounting_App.Domain.Models

Namespace Services

    ''' <summary>
    ''' Procure-to-Pay (P2P) workflow service orchestrating purchase orders, physical dock receiving (GRN),
    ''' 3-way matching of supplier bills with variance routing (PPV and freight), and accounts payable cash disbursements.
    ''' </summary>
    Public Class ProcureToPayService

        Private ReadOnly _context As ErpDbContext
        Private ReadOnly _periodService As PeriodService

        ''' <summary>
        ''' Initializes a new instance of <see cref="ProcureToPayService"/> with the specified DbContext and PeriodService.
        ''' </summary>
        Public Sub New(context As ErpDbContext, periodService As PeriodService)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            If periodService Is Nothing Then
                Throw New ArgumentNullException(NameOf(periodService))
            End If
            _context = context
            _periodService = periodService
        End Sub

        ''' <summary>
        ''' Initializes a new instance of <see cref="ProcureToPayService"/> with the specified DbContext.
        ''' </summary>
        Public Sub New(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            _context = context
            _periodService = New PeriodService(context)
        End Sub

        ''' <summary>
        ''' Initializes a new instance of <see cref="ProcureToPayService"/> with default DbContext.
        ''' </summary>
        Public Sub New()
            Me.New(New ErpDbContext())
        End Sub

        ' =========================================================================
        ' 1. Purchase Order Creation
        ' =========================================================================

        ''' <summary>
        ''' Creates an approved Purchase Order commitment for the specified vendor and line items.
        ''' </summary>
        ''' <param name="vendorId">The primary key of the vendor.</param>
        ''' <param name="lines">Collection of purchase order line items.</param>
        ''' <param name="orderDate">Optional order date; defaults to today if omitted.</param>
        ''' <param name="documentNumber">Optional document number; auto-generated if omitted.</param>
        ''' <returns>The persisted <see cref="PurchaseOrder"/>.</returns>
        Public Function CreatePurchaseOrder(vendorId As Integer,
                                           lines As IEnumerable(Of POLine),
                                           Optional orderDate As DateTime? = Nothing,
                                           Optional documentNumber As String = Nothing) As PurchaseOrder
            If vendorId <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(vendorId), "Vendor ID must be greater than zero.")
            End If
            If lines Is Nothing Then
                Throw New ArgumentNullException(NameOf(lines), "Purchase order lines cannot be null.")
            End If

            Dim lineList As List(Of POLine) = lines.ToList()
            If lineList.Count = 0 Then
                Throw New ArgumentException("Purchase order must contain at least one line item.", NameOf(lines))
            End If

            Dim vendor = _context.Vendors.Find(vendorId)
            If vendor Is Nothing Then
                Throw New ArgumentException($"Vendor with ID {vendorId} does not exist.", NameOf(vendorId))
            End If

            Dim effectiveOrderDate As DateTime = If(orderDate.HasValue, orderDate.Value, DateTime.Today)
            Dim docNum As String = If(String.IsNullOrWhiteSpace(documentNumber),
                                     $"PO-{effectiveOrderDate:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpperInvariant()}",
                                     documentNumber.Trim())

            Dim totalAmount As Decimal = 0D
            For Each line In lineList
                If line.OrderedQty <= 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Ordered quantity must be greater than zero.")
                End If
                If line.UnitPrice < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(lines), "Unit price cannot be negative.")
                End If

                Dim product = _context.Products.Find(line.ProductId)
                If product Is Nothing Then
                    Throw New ArgumentException($"Product with ID {line.ProductId} does not exist.", NameOf(lines))
                End If

                totalAmount += Math.Round(CDec(line.OrderedQty) * line.UnitPrice, 4, MidpointRounding.AwayFromZero)
            Next

            Dim po As New PurchaseOrder() With {
                .VendorId = vendorId,
                .DocumentNumber = docNum,
                .OrderDate = effectiveOrderDate,
                .TotalAmount = totalAmount,
                .Status = "Approved"
            }

            For Each line In lineList
                line.PurchaseOrder = po
                po.Lines.Add(line)
            Next

            _context.PurchaseOrders.Add(po)
            _context.SaveChanges()

            Return po
        End Function

        ' =========================================================================
        ' 2. Goods Receipt Note (GRN) Receiving Dock Intake
        ' =========================================================================

        ''' <summary>
        ''' Records physical intake on the warehouse receiving dock under a Goods Receipt Note (GRN).
        ''' Atomically increments physical stock on hand, updates the moving weighted average cost forward,
        ''' writes append-only inventory transaction records, and posts a balanced journal entry
        ''' (Debit 1300 Merchandise Inventory, Credit 2100 GRNI Clearing).
        ''' </summary>
        ''' <param name="poId">The primary key of the purchase order being received.</param>
        ''' <param name="receiptLines">Collection of received items and quantities.</param>
        ''' <param name="eventDate">The physical dock receiving timestamp.</param>
        ''' <param name="documentNumber">Optional GRN document number; auto-generated if omitted.</param>
        ''' <param name="postingDate">Optional explicit posting date; resolved via PeriodService if omitted.</param>
        ''' <returns>The persisted <see cref="GoodsReceiptNote"/>.</returns>
        Public Function ReceiveGoods(poId As Integer,
                                     receiptLines As IEnumerable(Of GRNLine),
                                     eventDate As DateTime,
                                     Optional documentNumber As String = Nothing,
                                     Optional postingDate As DateTime? = Nothing) As GoodsReceiptNote
            If poId <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(poId), "Purchase order ID must be greater than zero.")
            End If
            If receiptLines Is Nothing Then
                Throw New ArgumentNullException(NameOf(receiptLines), "Receipt lines cannot be null.")
            End If

            Dim receiptLineList As List(Of GRNLine) = receiptLines.ToList()
            If receiptLineList.Count = 0 Then
                Throw New ArgumentException("Goods Receipt Note must contain at least one line item.", NameOf(receiptLines))
            End If

            Dim po = _context.PurchaseOrders _
                .Include(Function(p) p.Lines) _
                .FirstOrDefault(Function(p) p.POId = poId)

            If po Is Nothing Then
                Throw New ArgumentException($"Purchase order with ID {poId} does not exist.", NameOf(poId))
            End If

            ' Resolve statutory posting date and validate fiscal period
            Dim statutoryPostingDate As DateTime = If(postingDate.HasValue, postingDate.Value, _periodService.ResolveStatutoryPostingDate(eventDate))
            Dim period As AccountingPeriod = _periodService.ValidatePostingDate(statutoryPostingDate)

            Dim docNum As String = If(String.IsNullOrWhiteSpace(documentNumber),
                                     $"GRN-{eventDate:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpperInvariant()}",
                                     documentNumber.Trim())

            Dim isOuterTransaction As Boolean = _context.Database.CurrentTransaction IsNot Nothing
            Dim tx = If(isOuterTransaction, Nothing, _context.Database.BeginTransaction())

            Try
                Dim grn As New GoodsReceiptNote() With {
                    .POId = poId,
                    .DocumentNumber = docNum,
                    .ReceiptDate = eventDate,
                    .PostingDate = statutoryPostingDate,
                    .Status = DocumentStatus.Posted.ToString()
                }

                Dim invTxList As New List(Of InventoryTransaction)()

                For Each rLine In receiptLineList
                    If rLine.ReceivedQty <= 0 Then
                        Throw New ArgumentOutOfRangeException(NameOf(receiptLines), "Received quantity must be strictly greater than zero.")
                    End If

                    ' Match POLine
                    Dim poLine As POLine = Nothing
                    If rLine.POLineId > 0 Then
                        poLine = po.Lines.FirstOrDefault(Function(pl) pl.POLineId = rLine.POLineId)
                    ElseIf rLine.ProductId > 0 Then
                        poLine = po.Lines.FirstOrDefault(Function(pl) pl.ProductId = rLine.ProductId)
                    End If

                    If poLine Is Nothing Then
                        Throw New ArgumentException($"Line item for Product ID {rLine.ProductId} does not belong to Purchase Order {poId}.", NameOf(receiptLines))
                    End If

                    rLine.POLineId = poLine.POLineId
                    rLine.ProductId = poLine.ProductId
                    If rLine.POUnitPrice <= 0D Then
                        rLine.POUnitPrice = poLine.UnitPrice
                    End If

                    Dim product = _context.Products.Find(rLine.ProductId)
                    If product Is Nothing Then
                        Throw New InvalidOperationException($"Product with ID {rLine.ProductId} was not found in catalog.")
                    End If

                    ' Recalculate perpetual moving weighted average cost
                    Dim currentQty As Integer = product.TotalQtyOnHand
                    Dim currentCost As Decimal = product.CurrentUnitCost
                    Dim newCost As Decimal = InventoryCostingEngine.CalculateNewMovingAverageCost(currentQty, currentCost, rLine.ReceivedQty, rLine.POUnitPrice)

                    ' Atomic physical stock increment and forward cost update
                    product.TotalQtyOnHand += rLine.ReceivedQty
                    product.CurrentUnitCost = newCost

                    ' Prepare audit inventory transaction
                    Dim invTx As New InventoryTransaction() With {
                        .ProductId = product.ProductId,
                        .SourceDocType = "GoodsReceiptNote",
                        .OperationalEventDate = eventDate,
                        .PostingDate = statutoryPostingDate,
                        .QtyChange = rLine.ReceivedQty,
                        .UnitCost = rLine.POUnitPrice,
                        .RunningQty = product.TotalQtyOnHand,
                        .RunningUnitCost = newCost
                    }
                    invTxList.Add(invTx)

                    rLine.GoodsReceiptNote = grn
                    grn.Lines.Add(rLine)
                Next

                _context.GoodsReceiptNotes.Add(grn)
                _context.SaveChanges()

                ' Link generated GRNId to audit transactions
                For Each txItem In invTxList
                    txItem.SourceDocId = grn.GRNId
                    _context.InventoryTransactions.Add(txItem)
                Next

                ' Generate balanced double-entry General Ledger journal
                Dim accountLookup = GetAccountLookup()
                Dim journalEntry As JournalEntry = LedgerPostingEngine.GenerateGRNJournal(grn, grn.Lines, accountLookup)
                journalEntry.SourceDocId = grn.GRNId
                journalEntry.PeriodId = period.PeriodId
                _context.JournalEntries.Add(journalEntry)

                ' Update Purchase Order fulfillment status
                UpdatePurchaseOrderStatus(po, receiptLineList)

                _context.SaveChanges()

                If tx IsNot Nothing Then
                    tx.Commit()
                End If

                Return grn
            Catch
                If tx IsNot Nothing Then
                    tx.Rollback()
                End If
                Throw
            Finally
                tx?.Dispose()
            End Try
        End Function

        ' =========================================================================
        ' 3. Supplier Bill 3-Way Matching & AP Accrual
        ' =========================================================================

        ''' <summary>
        ''' Executes a 3-way match between Purchase Order, Goods Receipt Note, and Supplier Bill.
        ''' Validates that billed quantity does not exceed received quantity.
        ''' Routes price variances to Account 5100 PPV and uncapitalized freight charges to Account 5200 Inbound Freight Variance.
        ''' Credits Account 2000 Accounts Payable and increments the vendor's outstanding trade payable balance.
        ''' </summary>
        ''' <param name="vendorId">The primary key of the billing vendor.</param>
        ''' <param name="grnId">The primary key of the Goods Receipt Note being invoiced.</param>
        ''' <param name="billLines">Collection of invoiced line items and charges.</param>
        ''' <param name="billDocNum">The supplier's invoice/bill document number.</param>
        ''' <param name="invoiceDate">The supplier bill invoice date.</param>
        ''' <param name="postingDate">Optional statutory posting date; resolved via PeriodService if omitted.</param>
        ''' <param name="dueDate">Optional invoice due date; calculated from vendor payment terms if omitted.</param>
        ''' <returns>The persisted <see cref="SupplierBill"/>.</returns>
        Public Function MatchSupplierBill(vendorId As Integer,
                                          grnId As Integer,
                                          billLines As IEnumerable(Of BillLine),
                                          billDocNum As String,
                                          invoiceDate As DateTime,
                                          Optional postingDate As DateTime? = Nothing,
                                          Optional dueDate As DateTime? = Nothing) As SupplierBill
            If vendorId <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(vendorId), "Vendor ID must be greater than zero.")
            End If
            If grnId <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(grnId), "GRN ID must be greater than zero.")
            End If
            If String.IsNullOrWhiteSpace(billDocNum) Then
                Throw New ArgumentException("Supplier bill document number cannot be empty.", NameOf(billDocNum))
            End If
            If billLines Is Nothing Then
                Throw New ArgumentNullException(NameOf(billLines), "Bill lines cannot be null.")
            End If

            Dim billLineList As List(Of BillLine) = billLines.ToList()
            If billLineList.Count = 0 Then
                Throw New ArgumentException("Supplier bill must contain at least one line item.", NameOf(billLines))
            End If

            Dim vendor = _context.Vendors.Find(vendorId)
            If vendor Is Nothing Then
                Throw New ArgumentException($"Vendor with ID {vendorId} does not exist.", NameOf(vendorId))
            End If

            Dim grn = _context.GoodsReceiptNotes _
                .Include(Function(g) g.Lines) _
                .Include(Function(g) g.PurchaseOrder) _
                .FirstOrDefault(Function(g) g.GRNId = grnId)

            If grn Is Nothing Then
                Throw New ArgumentException($"Goods Receipt Note with ID {grnId} does not exist.", NameOf(grnId))
            End If

            ' Validate 3-way match: vendor on bill must match vendor on PO
            If grn.PurchaseOrder IsNot Nothing AndAlso grn.PurchaseOrder.VendorId <> vendorId Then
                Throw New ThreeWayMatchException($"Three-way match vendor mismatch: PO vendor {grn.PurchaseOrder.VendorId} does not match bill vendor {vendorId}.")
            End If

            ' Retrieve prior billed quantities across all existing bills for this GRN
            Dim grnLineIds = grn.Lines.Select(Function(gl) gl.GRNLineId).ToList()
            Dim priorBillLines = _context.BillLines _
                .Where(Function(bl) bl.GRNLineId.HasValue AndAlso grnLineIds.Contains(bl.GRNLineId.Value)) _
                .ToList()

            ' Validate 3-way matching rules per line
            For Each bLine In billLineList
                If bLine.BilledQty < 0 Then
                    Throw New ArgumentOutOfRangeException(NameOf(billLines), "Billed quantity cannot be negative.")
                End If
                If bLine.BilledUnitPrice < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(billLines), "Billed unit price cannot be negative.")
                End If
                If bLine.FreightAmount < 0D Then
                    Throw New ArgumentOutOfRangeException(NameOf(billLines), "Freight amount cannot be negative.")
                End If

                If bLine.BilledQty > 0 Then
                    Dim grnLine As GRNLine = Nothing
                    If bLine.GRNLineId.HasValue Then
                        grnLine = grn.Lines.FirstOrDefault(Function(gl) gl.GRNLineId = bLine.GRNLineId.Value)
                        If grnLine Is Nothing Then
                            Throw New ThreeWayMatchException($"GRN line ID {bLine.GRNLineId.Value} does not belong to Goods Receipt Note {grnId}.")
                        End If
                    ElseIf bLine.ProductId.HasValue Then
                        grnLine = grn.Lines.FirstOrDefault(Function(gl) gl.ProductId = bLine.ProductId.Value)
                        If grnLine Is Nothing Then
                            Throw New ThreeWayMatchException($"Product ID {bLine.ProductId.Value} was not received under Goods Receipt Note {grnId}.")
                        End If
                        bLine.GRNLineId = grnLine.GRNLineId
                    Else
                        Throw New ThreeWayMatchException("Bill line must identify either GRNLineId or ProductId.")
                    End If

                    bLine.GRNLine = grnLine
                    bLine.ProductId = grnLine.ProductId

                    ' Enforce: total billed quantity <= received quantity
                    Dim previouslyBilled As Integer = priorBillLines _
                        .Where(Function(p) p.GRNLineId.HasValue AndAlso p.GRNLineId.Value = grnLine.GRNLineId) _
                        .Sum(Function(p) p.BilledQty)

                    Dim currentTotalBilled As Integer = previouslyBilled + bLine.BilledQty
                    If currentTotalBilled > grnLine.ReceivedQty Then
                        Throw New ThreeWayMatchException(currentTotalBilled, grnLine.ReceivedQty,
                            $"Three-way match failed for GRN line {grnLine.GRNLineId}: total billed quantity {currentTotalBilled} exceeds received quantity {grnLine.ReceivedQty}.")
                    End If

                    If bLine.LineTotal = 0D Then
                        bLine.LineTotal = Math.Round(CDec(bLine.BilledQty) * bLine.BilledUnitPrice, 4, MidpointRounding.AwayFromZero)
                    End If
                Else
                    ' Pure freight / service line
                    If bLine.LineTotal = 0D AndAlso bLine.FreightAmount > 0D Then
                        bLine.LineTotal = bLine.FreightAmount
                    ElseIf bLine.FreightAmount = 0D AndAlso bLine.LineTotal > 0D Then
                        bLine.FreightAmount = bLine.LineTotal
                    End If
                End If
            Next

            ' Resolve statutory posting date and validate fiscal period
            Dim statutoryPostingDate As DateTime = If(postingDate.HasValue, postingDate.Value, _periodService.ResolveStatutoryPostingDate(invoiceDate))
            Dim period As AccountingPeriod = _periodService.ValidatePostingDate(statutoryPostingDate)

            Dim effectiveDueDate As DateTime = If(dueDate.HasValue, dueDate.Value, invoiceDate.AddDays(If(vendor.PaymentTermsDays > 0, vendor.PaymentTermsDays, 30)))

            Dim isOuterTransaction As Boolean = _context.Database.CurrentTransaction IsNot Nothing
            Dim tx = If(isOuterTransaction, Nothing, _context.Database.BeginTransaction())

            Try
                Dim bill As New SupplierBill() With {
                    .VendorId = vendorId,
                    .DocumentNumber = billDocNum.Trim(),
                    .BillDate = invoiceDate,
                    .PostingDate = statutoryPostingDate,
                    .DueDate = effectiveDueDate,
                    .Status = DocumentStatus.Posted.ToString()
                }

                For Each bLine In billLineList
                    bLine.SupplierBill = bill
                    bill.Lines.Add(bLine)
                Next

                _context.SupplierBills.Add(bill)
                _context.SaveChanges()

                ' Generate balanced journal entry
                Dim accountLookup = GetAccountLookup()
                Dim journalEntry As JournalEntry = LedgerPostingEngine.GenerateSupplierBillJournal(bill, bill.Lines, grn.Lines, accountLookup)
                journalEntry.SourceDocId = bill.BillId
                journalEntry.PeriodId = period.PeriodId

                ' Set total amount exactly equal to AP credit line
                Dim apAccountId As Integer = accountLookup(AccountCodes.AccountsPayable.ToString())
                Dim apCreditLine = journalEntry.Lines.FirstOrDefault(Function(l) l.AccountId = apAccountId)
                Dim totalBillAmount As Decimal = If(apCreditLine IsNot Nothing, apCreditLine.CreditAmount, bill.Lines.Sum(Function(l) l.LineTotal + l.FreightAmount))
                bill.TotalAmount = totalBillAmount

                _context.JournalEntries.Add(journalEntry)

                ' Increment vendor outstanding trade payable balance
                vendor.CurrentPayableBalance += bill.TotalAmount

                ' Update GRN status if all received items are now fully billed
                UpdateGRNStatus(grn, billLineList, priorBillLines)

                _context.SaveChanges()

                If tx IsNot Nothing Then
                    tx.Commit()
                End If

                Return bill
            Catch
                If tx IsNot Nothing Then
                    tx.Rollback()
                End If
                Throw
            Finally
                tx?.Dispose()
            End Try
        End Function

        ' =========================================================================
        ' 4. Accounts Payable Cash Disbursement (Payment Voucher)
        ' =========================================================================

        ''' <summary>
        ''' Disburses a cash payment voucher settling an outstanding supplier bill obligation.
        ''' Validates payment amount, decrements the vendor's payable balance, checks settlement status,
        ''' and posts a balanced journal entry (Debit 2000 Accounts Payable, Credit 1020 Operating Bank Account).
        ''' </summary>
        ''' <param name="vendorId">The primary key of the vendor receiving payment.</param>
        ''' <param name="billId">The primary key of the supplier bill being settled.</param>
        ''' <param name="amount">The cash disbursement amount.</param>
        ''' <param name="bankAccountId">The bank account ID or COA code (1020) funding the disbursement.</param>
        ''' <param name="paymentDate">Optional physical payment date; defaults to today if omitted.</param>
        ''' <param name="postingDate">Optional statutory posting date; resolved via PeriodService if omitted.</param>
        ''' <param name="voucherNumber">Optional voucher number; auto-generated if omitted.</param>
        ''' <returns>The persisted <see cref="PaymentVoucher"/>.</returns>
        Public Function DisbursePaymentVoucher(vendorId As Integer,
                                              billId As Integer,
                                              amount As Decimal,
                                              bankAccountId As Integer,
                                              Optional paymentDate As DateTime? = Nothing,
                                              Optional postingDate As DateTime? = Nothing,
                                              Optional voucherNumber As String = Nothing) As PaymentVoucher
            If vendorId <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(vendorId), "Vendor ID must be greater than zero.")
            End If
            If billId <= 0 Then
                Throw New ArgumentOutOfRangeException(NameOf(billId), "Bill ID must be greater than zero.")
            End If
            If amount <= 0D Then
                Throw New ArgumentOutOfRangeException(NameOf(amount), "Disbursement amount must be strictly greater than zero.")
            End If

            Dim vendor = _context.Vendors.Find(vendorId)
            If vendor Is Nothing Then
                Throw New ArgumentException($"Vendor with ID {vendorId} does not exist.", NameOf(vendorId))
            End If

            Dim bill = _context.SupplierBills.Find(billId)
            If bill Is Nothing Then
                Throw New ArgumentException($"Supplier bill with ID {billId} does not exist.", NameOf(billId))
            End If

            If bill.VendorId <> vendorId Then
                Throw New ArgumentException($"Supplier bill {bill.DocumentNumber} belongs to Vendor ID {bill.VendorId}, not specified Vendor ID {vendorId}.", NameOf(vendorId))
            End If

            If String.Equals(bill.Status, DocumentStatus.Settled.ToString(), StringComparison.OrdinalIgnoreCase) Then
                Throw New InvalidOperationException($"Supplier bill {bill.DocumentNumber} is already settled in full.")
            End If

            ' Resolve bank account
            Dim bankAccount = _context.Accounts.FirstOrDefault(Function(a) a.AccountId = bankAccountId OrElse a.Code = bankAccountId.ToString())
            If bankAccount Is Nothing Then
                Throw New ArgumentException($"Bank account ID or code '{bankAccountId}' was not found in Chart of Accounts.", NameOf(bankAccountId))
            End If

            ' Check existing disbursements against this bill
            Dim priorDisbursed As Decimal = _context.PaymentVouchers _
                .Where(Function(v) v.TargetDocId = billId AndAlso v.VoucherType = "VendorPayment") _
                .Sum(Function(v) v.DisbursedAmount)

            Dim remainingBalance As Decimal = bill.TotalAmount - priorDisbursed
            If amount > remainingBalance + 0.0001D Then
                Throw New ArgumentException($"Disbursement amount ${amount:N2} exceeds remaining payable balance of ${remainingBalance:N2} on bill {bill.DocumentNumber}.", NameOf(amount))
            End If

            Dim effectivePaymentDate As DateTime = If(paymentDate.HasValue, paymentDate.Value, DateTime.Today)
            Dim statutoryPostingDate As DateTime = If(postingDate.HasValue, postingDate.Value, _periodService.ResolveStatutoryPostingDate(effectivePaymentDate))
            Dim period As AccountingPeriod = _periodService.ValidatePostingDate(statutoryPostingDate)

            Dim docNum As String = If(String.IsNullOrWhiteSpace(voucherNumber),
                                     $"PV-{effectivePaymentDate:yyyyMMdd}-{Guid.NewGuid().ToString().Substring(0, 8).ToUpperInvariant()}",
                                     voucherNumber.Trim())

            Dim isOuterTransaction As Boolean = _context.Database.CurrentTransaction IsNot Nothing
            Dim tx = If(isOuterTransaction, Nothing, _context.Database.BeginTransaction())

            Try
                Dim voucher As New PaymentVoucher() With {
                    .VoucherType = "VendorPayment",
                    .VendorId = vendorId,
                    .TargetDocId = billId,
                    .VoucherNumber = docNum,
                    .PaymentDate = effectivePaymentDate,
                    .PostingDate = statutoryPostingDate,
                    .DisbursedAmount = amount,
                    .BankAccountId = bankAccount.AccountId
                }

                _context.PaymentVouchers.Add(voucher)
                _context.SaveChanges()

                ' Generate balanced journal entry
                Dim accountLookup = GetAccountLookup()
                Dim journalEntry As JournalEntry = LedgerPostingEngine.GeneratePaymentVoucherJournal(voucher, accountLookup)
                journalEntry.SourceDocId = voucher.VoucherId
                journalEntry.PeriodId = period.PeriodId
                _context.JournalEntries.Add(journalEntry)

                ' Decrement vendor payable balance
                vendor.CurrentPayableBalance -= amount

                ' Check settlement
                Dim totalDisbursed As Decimal = priorDisbursed + amount
                If totalDisbursed >= bill.TotalAmount - 0.0001D Then
                    bill.Status = DocumentStatus.Settled.ToString()
                End If

                _context.SaveChanges()

                If tx IsNot Nothing Then
                    tx.Commit()
                End If

                Return voucher
            Catch
                If tx IsNot Nothing Then
                    tx.Rollback()
                End If
                Throw
            Finally
                tx?.Dispose()
            End Try
        End Function

        ' =========================================================================
        ' Helper Query Methods
        ' =========================================================================

        Public Function GetPurchaseOrder(poId As Integer) As PurchaseOrder
            Return _context.PurchaseOrders _
                .Include(Function(p) p.Vendor) _
                .Include(Function(p) p.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(p) p.POId = poId)
        End Function

        Public Function GetGoodsReceiptNote(grnId As Integer) As GoodsReceiptNote
            Return _context.GoodsReceiptNotes _
                .Include(Function(g) g.PurchaseOrder) _
                .Include(Function(g) g.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(g) g.GRNId = grnId)
        End Function

        Public Function GetSupplierBill(billId As Integer) As SupplierBill
            Return _context.SupplierBills _
                .Include(Function(b) b.Vendor) _
                .Include(Function(b) b.Lines) _
                    .ThenInclude(Function(l) l.Product) _
                .FirstOrDefault(Function(b) b.BillId = billId)
        End Function

        Public Function GetPaymentVoucher(voucherId As Integer) As PaymentVoucher
            Return _context.PaymentVouchers _
                .Include(Function(v) v.Vendor) _
                .Include(Function(v) v.BankAccount) _
                .FirstOrDefault(Function(v) v.VoucherId = voucherId)
        End Function

        ' =========================================================================
        ' Private Internal Helpers
        ' =========================================================================

        Private Function GetAccountLookup() As Func(Of String, Integer)
            Dim map = _context.Accounts.ToDictionary(Function(a) a.Code, Function(a) a.AccountId)

            Return Function(code As String) As Integer
                If map.ContainsKey(code) Then
                    Return map(code)
                End If
                Dim parsed As Integer
                If Integer.TryParse(code, parsed) Then
                    Return parsed
                End If
                Throw New InvalidOperationException($"Account code '{code}' could not be resolved.")
            End Function
        End Function

        Private Sub UpdatePurchaseOrderStatus(po As PurchaseOrder, currentReceiptLines As List(Of GRNLine))
            Dim allPoLinesReceived As Boolean = True

            For Each pLine In po.Lines
                Dim priorReceivedQty As Integer = _context.GRNLines _
                    .Where(Function(gl) gl.POLineId = pLine.POLineId) _
                    .Sum(Function(gl) gl.ReceivedQty)

                If priorReceivedQty < pLine.OrderedQty Then
                    allPoLinesReceived = False
                End If
            Next

            If allPoLinesReceived Then
                po.Status = "Fully Received"
            Else
                po.Status = "Partially Received"
            End If
        End Sub

        Private Sub UpdateGRNStatus(grn As GoodsReceiptNote, currentBillLines As List(Of BillLine), priorBillLines As List(Of BillLine))
            Dim allGrnLinesBilled As Boolean = True

            For Each gLine In grn.Lines
                Dim priorBilled As Integer = priorBillLines _
                    .Where(Function(p) p.GRNLineId.HasValue AndAlso p.GRNLineId.Value = gLine.GRNLineId) _
                    .Sum(Function(p) p.BilledQty)

                Dim currentBilled As Integer = currentBillLines _
                    .Where(Function(b) b.GRNLineId.HasValue AndAlso b.GRNLineId.Value = gLine.GRNLineId) _
                    .Sum(Function(b) b.BilledQty)

                If (priorBilled + currentBilled) < gLine.ReceivedQty Then
                    allGrnLinesBilled = False
                End If
            Next

            If allGrnLinesBilled Then
                grn.Status = DocumentStatus.Settled.ToString()
            End If
        End Sub

    End Class

End Namespace
