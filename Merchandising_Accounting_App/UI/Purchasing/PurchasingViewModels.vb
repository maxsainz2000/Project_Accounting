Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain.Models
Imports Merchandising_Accounting_App.Services

Namespace UI.Purchasing

    Public Class PurchasingVendorModel
        Public Property VendorId As Integer
        Public Property Code As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property PaymentTermsDays As Integer = 30
        Public Property CurrentPayableBalance As Decimal

        Public ReadOnly Property DisplayText As String
            Get
                Return $"{Code} - {Name} (Terms: Net-{PaymentTermsDays}, AP Balance: ${CurrentPayableBalance:N2})"
            End Get
        End Property
    End Class

    Public Class PurchasingProductItem
        Public Property ProductId As Integer
        Public Property SKU As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property StandardCost As Decimal

        Public ReadOnly Property DisplayText As String
            Get
                Return $"{SKU} - {Name} (Std Cost: ${StandardCost:N2})"
            End Get
        End Property
    End Class

    Public Class PurchasingPOLineModel
        Public Property LineId As Integer
        Public Property ProductId As Integer
        Public Property SKU As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property OrderedQty As Integer
        Public Property UnitPrice As Decimal

        Public ReadOnly Property LineTotal As Decimal
            Get
                Return CDec(OrderedQty) * UnitPrice
            End Get
        End Property
    End Class

    Public Class PurchasingPOModel
        Public Property POId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property VendorId As Integer
        Public Property VendorName As String = String.Empty
        Public Property OrderDate As DateTime
        Public Property Status As String = "Approved" ' Approved, Partially Received, Fully Received, Closed
        Public Property Lines As New List(Of PurchasingPOLineModel)()

        Public ReadOnly Property TotalAmount As Decimal
            Get
                Dim sumVal As Decimal = 0D
                For Each line In Lines
                    sumVal += line.LineTotal
                Next
                Return sumVal
            End Get
        End Property

        Public ReadOnly Property TotalOrderedQty As Integer
            Get
                Dim total As Integer = 0
                For Each line In Lines
                    total += line.OrderedQty
                Next
                Return total
            End Get
        End Property
    End Class

    Public Class PurchasingGRNModel
        Public Property GRNId As Integer
        Public Property GRNLineId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property PONumber As String = String.Empty
        Public Property VendorName As String = String.Empty
        Public Property ReceiptDate As DateTime
        Public Property SKU As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property ReceivedQty As Integer
        Public Property POUnitPrice As Decimal
        Public Property Status As String = "Pending Bill"

        Public ReadOnly Property TotalValuation As Decimal
            Get
                Return CDec(ReceivedQty) * POUnitPrice
            End Get
        End Property
    End Class

    Public Class ThreeWayMatchModel
        Public Property MatchId As Integer
        Public Property VendorId As Integer
        Public Property GRNId As Integer
        Public Property GRNLineId As Integer
        Public Property BillId As Integer
        Public Property PONumber As String = String.Empty
        Public Property GRNNumber As String = String.Empty
        Public Property VendorName As String = String.Empty
        Public Property SKU As String = String.Empty
        Public Property ProductName As String = String.Empty

        ' 1. Purchase Order
        Public Property POQty As Integer
        Public Property POUnitPrice As Decimal

        Public ReadOnly Property POTotal As Decimal
            Get
                Return CDec(POQty) * POUnitPrice
            End Get
        End Property

        ' 2. Goods Receipt Note
        Public Property GRNQty As Integer

        Public ReadOnly Property GRNValuation As Decimal
            Get
                Return CDec(GRNQty) * POUnitPrice
            End Get
        End Property

        ' 3. Supplier Bill
        Public Property BillNumber As String = String.Empty
        Public Property BillQty As Integer
        Public Property BilledUnitPrice As Decimal
        Public Property InboundFreight As Decimal

        Public ReadOnly Property BilledSubtotal As Decimal
            Get
                Return CDec(BillQty) * BilledUnitPrice
            End Get
        End Property

        Public ReadOnly Property NetTotalPayable As Decimal
            Get
                Return BilledSubtotal + InboundFreight
            End Get
        End Property

        ' Variances
        Public ReadOnly Property QtyVariance As Integer
            Get
                Return BillQty - GRNQty
            End Get
        End Property

        Public ReadOnly Property PurchasePriceVariance As Decimal
            Get
                Return (BilledUnitPrice - POUnitPrice) * CDec(BillQty)
            End Get
        End Property

        Public Property MatchStatus As String = "Pending Bill"
        Public Property IsSettled As Boolean = False
    End Class

    Public Class PurchasingJournalLine
        Public Property AccountCode As String = String.Empty
        Public Property AccountName As String = String.Empty
        Public Property Debit As Decimal
        Public Property Credit As Decimal
        Public Property LineMemo As String = String.Empty
    End Class

    Public Class PurchasingJournalEntry
        Public Property DocumentNumber As String = String.Empty
        Public Property SourceDocType As String = String.Empty
        Public Property PostingDate As DateTime
        Public Property Description As String = String.Empty
        Public Property Lines As New List(Of PurchasingJournalLine)()

        Public ReadOnly Property TotalDebits As Decimal
            Get
                Dim d As Decimal = 0D
                For Each l In Lines
                    d += l.Debit
                Next
                Return Math.Round(d, 4)
            End Get
        End Property

        Public ReadOnly Property TotalCredits As Decimal
            Get
                Dim c As Decimal = 0D
                For Each l In Lines
                    c += l.Credit
                Next
                Return Math.Round(c, 4)
            End Get
        End Property

        Public ReadOnly Property IsBalanced As Boolean
            Get
                Return TotalDebits = TotalCredits
            End Get
        End Property
    End Class

    Public Class PurchasingDataService
        Implements IDisposable

        Private ReadOnly _p2pService As ProcureToPayService
        Private ReadOnly _context As ErpDbContext
        Private _disposed As Boolean = False

        Public Property Vendors As New List(Of PurchasingVendorModel)()
        Public Property CatalogProducts As New List(Of PurchasingProductItem)()
        Public Property PurchaseOrders As New List(Of PurchasingPOModel)()
        Public Property GoodsReceiptNotes As New List(Of PurchasingGRNModel)()
        Public Property ThreeWayMatches As New List(Of ThreeWayMatchModel)()
        Public Property JournalHistory As New List(Of PurchasingJournalEntry)()

        Public Sub New(Optional p2pService As ProcureToPayService = Nothing, Optional context As ErpDbContext = Nothing)
            _context = If(context, New ErpDbContext())
            _p2pService = If(p2pService, New ProcureToPayService(_context))
            LoadData()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            If Not _disposed Then
                _context?.Dispose()
                _disposed = True
            End If
            GC.SuppressFinalize(Me)
        End Sub

        Public Sub LoadData()
            Try
                ' 1. Vendors
                Vendors.Clear()
                For Each v In _context.Vendors.OrderBy(Function(x) x.Name).ToList()
                    Vendors.Add(New PurchasingVendorModel() With {
                        .VendorId = v.VendorId,
                        .Code = v.Code,
                        .Name = v.Name,
                        .PaymentTermsDays = v.PaymentTermsDays,
                        .CurrentPayableBalance = v.CurrentPayableBalance
                    })
                Next

                ' 2. Catalog Products
                CatalogProducts.Clear()
                For Each p In _context.Products.Where(Function(x) x.IsActive).OrderBy(Function(x) x.SKU).ToList()
                    CatalogProducts.Add(New PurchasingProductItem() With {
                        .ProductId = p.ProductId,
                        .SKU = p.SKU,
                        .Name = p.Name,
                        .StandardCost = p.CurrentUnitCost
                    })
                Next

                ' 3. Purchase Orders
                PurchaseOrders.Clear()
                Dim pos = _context.PurchaseOrders _
                    .Include(Function(p) p.Vendor) _
                    .Include(Function(p) p.Lines).ThenInclude(Function(l) l.Product) _
                    .OrderByDescending(Function(p) p.OrderDate) _
                    .ToList()

                For Each po In pos
                    Dim poModel As New PurchasingPOModel() With {
                        .POId = po.POId,
                        .DocumentNumber = po.DocumentNumber,
                        .VendorId = po.VendorId,
                        .VendorName = If(po.Vendor IsNot Nothing, po.Vendor.Name, ""),
                        .OrderDate = po.OrderDate,
                        .Status = po.Status
                    }
                    For Each l In po.Lines
                        poModel.Lines.Add(New PurchasingPOLineModel() With {
                            .LineId = l.POLineId,
                            .ProductId = l.ProductId,
                            .SKU = If(l.Product IsNot Nothing, l.Product.SKU, ""),
                            .ProductName = If(l.Product IsNot Nothing, l.Product.Name, ""),
                            .OrderedQty = l.OrderedQty,
                            .UnitPrice = l.UnitPrice
                        })
                    Next
                    PurchaseOrders.Add(poModel)
                Next

                ' 4. Goods Receipt Notes
                GoodsReceiptNotes.Clear()
                Dim grns = _context.GoodsReceiptNotes _
                    .Include(Function(g) g.PurchaseOrder).ThenInclude(Function(p) p.Vendor) _
                    .Include(Function(g) g.Lines).ThenInclude(Function(l) l.Product) _
                    .OrderByDescending(Function(g) g.ReceiptDate) _
                    .ToList()

                For Each grn In grns
                    For Each l In grn.Lines
                        GoodsReceiptNotes.Add(New PurchasingGRNModel() With {
                            .GRNId = grn.GRNId,
                            .GRNLineId = l.GRNLineId,
                            .DocumentNumber = grn.DocumentNumber,
                            .PONumber = If(grn.PurchaseOrder IsNot Nothing, grn.PurchaseOrder.DocumentNumber, ""),
                            .VendorName = If(grn.PurchaseOrder IsNot Nothing AndAlso grn.PurchaseOrder.Vendor IsNot Nothing, grn.PurchaseOrder.Vendor.Name, ""),
                            .ReceiptDate = grn.ReceiptDate,
                            .SKU = If(l.Product IsNot Nothing, l.Product.SKU, ""),
                            .ProductName = If(l.Product IsNot Nothing, l.Product.Name, ""),
                            .ReceivedQty = l.ReceivedQty,
                            .POUnitPrice = l.POUnitPrice,
                            .Status = grn.Status
                        })
                    Next
                Next

                ' 5. Three-Way Matches
                ThreeWayMatches.Clear()
                Dim billLines = _context.BillLines _
                    .Include(Function(b) b.SupplierBill).ThenInclude(Function(sb) sb.Vendor) _
                    .Include(Function(b) b.Product) _
                    .ToList()

                Dim grnLines = _context.GRNLines _
                    .Include(Function(gl) gl.GoodsReceiptNote).ThenInclude(Function(g) g.PurchaseOrder).ThenInclude(Function(p) p.Vendor) _
                    .Include(Function(gl) gl.POLine) _
                    .Include(Function(gl) gl.Product) _
                    .OrderBy(Function(gl) gl.GRNLineId) _
                    .ToList()

                Dim matchIdCounter As Integer = 1
                For Each gl In grnLines
                    Dim po = If(gl.GoodsReceiptNote IsNot Nothing, gl.GoodsReceiptNote.PurchaseOrder, Nothing)
                    Dim vendor = If(po IsNot Nothing, po.Vendor, Nothing)
                    Dim matchedBillLine = billLines.FirstOrDefault(Function(bl) bl.GRNLineId.HasValue AndAlso bl.GRNLineId.Value = gl.GRNLineId)

                    Dim matchModel As New ThreeWayMatchModel() With {
                        .MatchId = matchIdCounter,
                        .VendorId = If(vendor IsNot Nothing, vendor.VendorId, 0),
                        .GRNId = gl.GRNId,
                        .GRNLineId = gl.GRNLineId,
                        .PONumber = If(po IsNot Nothing, po.DocumentNumber, ""),
                        .GRNNumber = If(gl.GoodsReceiptNote IsNot Nothing, gl.GoodsReceiptNote.DocumentNumber, ""),
                        .VendorName = If(vendor IsNot Nothing, vendor.Name, ""),
                        .SKU = If(gl.Product IsNot Nothing, gl.Product.SKU, ""),
                        .ProductName = If(gl.Product IsNot Nothing, gl.Product.Name, ""),
                        .POQty = If(gl.POLine IsNot Nothing, gl.POLine.OrderedQty, gl.ReceivedQty),
                        .POUnitPrice = gl.POUnitPrice,
                        .GRNQty = gl.ReceivedQty
                    }

                    If matchedBillLine IsNot Nothing AndAlso matchedBillLine.SupplierBill IsNot Nothing Then
                        matchModel.BillId = matchedBillLine.SupplierBill.BillId
                        matchModel.BillNumber = matchedBillLine.SupplierBill.DocumentNumber
                        matchModel.BillQty = matchedBillLine.BilledQty
                        matchModel.BilledUnitPrice = matchedBillLine.BilledUnitPrice
                        matchModel.InboundFreight = matchedBillLine.FreightAmount
                        matchModel.IsSettled = True

                        Dim ppv As Decimal = matchModel.PurchasePriceVariance
                        If ppv > 0D Then
                            matchModel.MatchStatus = $"PPV Unfavorable (+${ppv:N2})"
                        ElseIf ppv < 0D Then
                            matchModel.MatchStatus = $"PPV Favorable (-${Math.Abs(ppv):N2})"
                        Else
                            matchModel.MatchStatus = "Exact Match"
                        End If
                    Else
                        matchModel.BillNumber = ""
                        matchModel.BillQty = 0
                        matchModel.BilledUnitPrice = 0D
                        matchModel.InboundFreight = 0D
                        matchModel.MatchStatus = "Pending Bill"
                        matchModel.IsSettled = False
                    End If

                    ThreeWayMatches.Add(matchModel)
                    matchIdCounter += 1
                Next

                ' 6. Journal History (Supplier Bills and GRNs)
                JournalHistory.Clear()
                Dim jeList = _context.JournalEntries _
                    .Include(Function(j) j.Lines).ThenInclude(Function(l) l.Account) _
                    .Where(Function(j) j.SourceDocType = "SupplierBill" OrElse j.SourceDocType = "Supplier Bill" OrElse j.SourceDocType = "GoodsReceiptNote" OrElse j.SourceDocType = "Goods Receipt Note") _
                    .OrderByDescending(Function(j) j.PostingDate) _
                    .ThenByDescending(Function(j) j.EntryId) _
                    .ToList()

                For Each entry In jeList
                    Dim pje As New PurchasingJournalEntry() With {
                        .DocumentNumber = $"{entry.SourceDocType} #{entry.SourceDocId}",
                        .SourceDocType = "SupplierBill",
                        .PostingDate = entry.PostingDate,
                        .Description = entry.Description
                    }
                    For Each l In entry.Lines
                        pje.Lines.Add(New PurchasingJournalLine() With {
                            .AccountCode = If(l.Account IsNot Nothing, l.Account.Code, ""),
                            .AccountName = If(l.Account IsNot Nothing, l.Account.Name, ""),
                            .Debit = l.DebitAmount,
                            .Credit = l.CreditAmount,
                            .LineMemo = l.LineMemo
                        })
                    Next
                    JournalHistory.Add(pje)
                Next

            Catch ex As Exception
                ' Graceful handle if database not yet initialized
            End Try
        End Sub

        Public Function CreatePurchaseOrder(vendor As PurchasingVendorModel, lines As List(Of PurchasingPOLineModel)) As PurchasingPOModel
            If vendor Is Nothing Then Throw New ArgumentNullException(NameOf(vendor))
            If lines Is Nothing OrElse lines.Count = 0 Then
                Throw New ArgumentException("PO must have at least one line item.")
            End If

            Dim poLines As New List(Of POLine)()
            For Each l In lines
                poLines.Add(New POLine(l.ProductId, l.OrderedQty, l.UnitPrice))
            Next

            Dim po = _p2pService.CreatePurchaseOrder(vendor.VendorId, poLines, DateTime.Today)
            LoadData()

            Return PurchaseOrders.FirstOrDefault(Function(p) p.POId = po.POId)
        End Function

        Public Function PostThreeWayMatch(match As ThreeWayMatchModel, billNumber As String, billQty As Integer, billedPrice As Decimal, inboundFreight As Decimal) As PurchasingJournalEntry
            If match Is Nothing Then Throw New ArgumentNullException(NameOf(match))
            If String.IsNullOrWhiteSpace(billNumber) Then
                Throw New ArgumentException("Supplier Bill Number is required.", NameOf(billNumber))
            End If
            If billQty <= 0 Then
                Throw New ArgumentException("Billed quantity must be greater than zero.", NameOf(billQty))
            End If
            If billedPrice < 0D Then
                Throw New ArgumentException("Billed unit price cannot be negative.", NameOf(billedPrice))
            End If
            If inboundFreight < 0D Then
                Throw New ArgumentException("Inbound freight cannot be negative.", NameOf(inboundFreight))
            End If

            Dim grnLineId As Integer = match.GRNLineId
            If grnLineId <= 0 Then
                Dim gl = _context.GRNLines.FirstOrDefault(Function(l) l.GRNId = match.GRNId)
                If gl IsNot Nothing Then grnLineId = gl.GRNLineId
            End If

            Dim billLines As New List(Of BillLine) From {
                New BillLine() With {
                    .GRNLineId = grnLineId,
                    .BilledQty = billQty,
                    .BilledUnitPrice = billedPrice,
                    .LineTotal = CDec(billQty) * billedPrice,
                    .FreightAmount = inboundFreight
                }
            }

            Dim bill = _p2pService.MatchSupplierBill(match.VendorId, match.GRNId, billLines, billNumber.Trim(), DateTime.Today, DateTime.Today)
            LoadData()

            Dim journal = JournalHistory.FirstOrDefault()
            If journal Is Nothing Then
                journal = New PurchasingJournalEntry() With {
                    .DocumentNumber = billNumber,
                    .SourceDocType = "SupplierBill",
                    .PostingDate = DateTime.Today,
                    .Description = $"3-Way Match Clearing for {match.VendorName} (Bill {billNumber})"
                }
            End If
            Return journal
        End Function

        Public Function ClearStaleGRNI(grnLineId As Integer, reason As String) As GRNIAdjustment
            Dim adjustment = _p2pService.ClearStaleGRNI(grnLineId, reason, DateTime.Today)
            LoadData()
            Return adjustment
        End Function

        Public Function ClosePurchaseOrderShort(poId As Integer) As PurchaseOrder
            Dim po = _p2pService.ClosePurchaseOrderShort(poId)
            LoadData()
            Return po
        End Function

        Public Function DisburseVendorPayment(vendorId As Integer, billId As Integer, amount As Decimal, bankAccountId As Integer, Optional paymentDate As DateTime? = Nothing) As PaymentVoucher
            Dim voucher = _p2pService.DisbursePaymentVoucher(vendorId, billId, amount, bankAccountId, paymentDate)
            LoadData()
            Return voucher
        End Function

        Public Function GetOpenSupplierBills() As List(Of OpenSupplierBillItem)
            Dim results As New List(Of OpenSupplierBillItem)()
            Try
                Dim bills = _context.SupplierBills _
                    .Include(Function(b) b.Vendor) _
                    .Where(Function(b) b.Status <> "Settled") _
                    .OrderByDescending(Function(b) b.BillDate) _
                    .ToList()

                For Each b In bills
                    Dim priorDisbursed As Decimal = _context.PaymentVouchers _
                        .Where(Function(v) v.TargetDocId = b.BillId AndAlso v.VoucherType = "VendorPayment") _
                        .Sum(Function(v) v.DisbursedAmount)

                    Dim remaining As Decimal = b.TotalAmount - priorDisbursed
                    If remaining > 0.0001D Then
                        results.Add(New OpenSupplierBillItem() With {
                            .BillId = b.BillId,
                            .VendorId = b.VendorId,
                            .VendorName = If(b.Vendor IsNot Nothing, b.Vendor.Name, $"Vendor #{b.VendorId}"),
                            .DocumentNumber = b.DocumentNumber,
                            .BillDate = b.BillDate,
                            .DueDate = b.DueDate,
                            .TotalAmount = b.TotalAmount,
                            .DisbursedAmount = priorDisbursed,
                            .RemainingBalance = remaining,
                            .Status = b.Status
                        })
                    End If
                Next
            Catch
            End Try
            Return results
        End Function

        Public Function GetBankAccounts() As List(Of Account)
            Try
                Return _context.Accounts _
                    .Where(Function(a) a.IsActive AndAlso (a.Code = "1010" OrElse a.Code = "1020")) _
                    .OrderBy(Function(a) a.Code) _
                    .ToList()
            Catch
                Return New List(Of Account)()
            End Try
        End Function

    End Class

    Public Class OpenSupplierBillItem
        Public Property BillId As Integer
        Public Property VendorId As Integer
        Public Property VendorName As String = String.Empty
        Public Property DocumentNumber As String = String.Empty
        Public Property BillDate As DateTime
        Public Property DueDate As DateTime
        Public Property TotalAmount As Decimal
        Public Property DisbursedAmount As Decimal
        Public Property RemainingBalance As Decimal
        Public Property Status As String = String.Empty

        Public ReadOnly Property DisplayText As String
            Get
                Return $"{DocumentNumber} - {VendorName} (Due: {DueDate:yyyy-MM-dd}, Balance: ${RemainingBalance:N2})"
            End Get
        End Property
    End Class

End Namespace
