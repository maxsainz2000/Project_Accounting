Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain.Models
Imports Merchandising_Accounting_App.Services

Namespace UI.Warehouse

    Public Class WarehouseProductModel
        Public Property ProductId As Integer
        Public Property SKU As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property BaseUnit As String = "Units"
        Public Property CurrentUnitCost As Decimal
        Public Property TotalQtyOnHand As Integer
        Public Property QuarantineQty As Integer

        Public ReadOnly Property DisplayText As String
            Get
                Return $"{SKU} - {Name} (On Hand: {TotalQtyOnHand}, Quarantine: {QuarantineQty}, Cost: ${CurrentUnitCost:N2})"
            End Get
        End Property
    End Class

    Public Class WarehousePOLineModel
        Public Property LineId As Integer
        Public Property ProductId As Integer
        Public Property SKU As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property OrderedQty As Integer
        Public Property ReceivedQty As Integer
        Public Property POUnitPrice As Decimal

        Public ReadOnly Property PendingQty As Integer
            Get
                Return Math.Max(0, OrderedQty - ReceivedQty)
            End Get
        End Property

        Public ReadOnly Property LineTotal As Decimal
            Get
                Return CDec(OrderedQty) * POUnitPrice
            End Get
        End Property
    End Class

    Public Class WarehousePOModel
        Public Property POId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property VendorName As String = String.Empty
        Public Property OrderDate As DateTime
        Public Property Status As String = "Open"
        Public Property Lines As New List(Of WarehousePOLineModel)()

        Public ReadOnly Property TotalAmount As Decimal
            Get
                Dim sumVal As Decimal = 0D
                For Each line In Lines
                    sumVal += line.LineTotal
                Next
                Return sumVal
            End Get
        End Property

        Public ReadOnly Property TotalPendingQty As Integer
            Get
                Dim pending As Integer = 0
                For Each line In Lines
                    pending += line.PendingQty
                Next
                Return pending
            End Get
        End Property
    End Class

    Public Class WarehouseGRNLineModel
        Public Property LineId As Integer
        Public Property ProductId As Integer
        Public Property SKU As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property ReceivedQty As Integer
        Public Property POUnitPrice As Decimal

        Public ReadOnly Property ExtendedCost As Decimal
            Get
                Return CDec(ReceivedQty) * POUnitPrice
            End Get
        End Property
    End Class

    Public Class WarehouseGRNModel
        Public Property GRNId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property PONumber As String = String.Empty
        Public Property VendorName As String = String.Empty
        Public Property ReceiptDate As DateTime
        Public Property PostingDate As DateTime
        Public Property Status As String = "Posted"
        Public Property Lines As New List(Of WarehouseGRNLineModel)()

        Public ReadOnly Property TotalReceivedQty As Integer
            Get
                Dim total As Integer = 0
                For Each l In Lines
                    total += l.ReceivedQty
                Next
                Return total
            End Get
        End Property

        Public ReadOnly Property TotalValuation As Decimal
            Get
                Dim sumVal As Decimal = 0D
                For Each l In Lines
                    sumVal += l.ExtendedCost
                Next
                Return sumVal
            End Get
        End Property
    End Class

    Public Class WarehouseRMAModel
        Public Property RMAId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property CustomerName As String = String.Empty
        Public Property OriginalInvoiceNumber As String = String.Empty
        Public Property ProductId As Integer
        Public Property SKU As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property ReturnedQty As Integer
        Public Property QuarantineQty As Integer
        Public Property RestockedQty As Integer
        Public Property ScrappedQty As Integer
        Public Property OriginalUnitCost As Decimal
        Public Property SellingUnitPrice As Decimal
        Public Property ReturnDate As DateTime
        Public Property Status As String = "Quarantine" ' "Quarantine", "Partially Inspected", "Completed"
    End Class

    Public Class SimpleJournalLine
        Public Property AccountCode As String = String.Empty
        Public Property AccountName As String = String.Empty
        Public Property Debit As Decimal
        Public Property Credit As Decimal
        Public Property LineMemo As String = String.Empty
    End Class

    Public Class SimpleJournalEntry
        Public Property DocumentNumber As String = String.Empty
        Public Property SourceDocType As String = String.Empty
        Public Property PostingDate As DateTime
        Public Property Description As String = String.Empty
        Public Property Lines As New List(Of SimpleJournalLine)()

        Public ReadOnly Property TotalDebits As Decimal
            Get
                Dim d As Decimal = 0D
                For Each line In Lines
                    d += line.Debit
                Next
                Return Math.Round(d, 4)
            End Get
        End Property

        Public ReadOnly Property TotalCredits As Decimal
            Get
                Dim c As Decimal = 0D
                For Each line In Lines
                    c += line.Credit
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

    Public Class WarehouseDataService
        Implements IDisposable

        Private ReadOnly _p2pService As ProcureToPayService
        Private ReadOnly _returnsService As ReturnsService
        Private ReadOnly _context As ErpDbContext
        Private _disposed As Boolean = False

        Public Property Products As New List(Of WarehouseProductModel)()
        Public Property PurchaseOrders As New List(Of WarehousePOModel)()
        Public Property GoodsReceiptNotes As New List(Of WarehouseGRNModel)()
        Public Property RMAList As New List(Of WarehouseRMAModel)()
        Public Property JournalHistory As New List(Of SimpleJournalEntry)()

        Public Sub New(Optional p2pService As ProcureToPayService = Nothing, Optional returnsService As ReturnsService = Nothing, Optional context As ErpDbContext = Nothing)
            _context = If(context, New ErpDbContext())
            _p2pService = If(p2pService, New ProcureToPayService(_context))
            _returnsService = If(returnsService, New ReturnsService(_context))
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
                ' 1. Products
                Products.Clear()
                For Each p In _context.Products.Where(Function(x) x.IsActive).OrderBy(Function(x) x.SKU).ToList()
                    Products.Add(New WarehouseProductModel() With {
                        .ProductId = p.ProductId,
                        .SKU = p.SKU,
                        .Name = p.Name,
                        .BaseUnit = p.BaseUnitName,
                        .CurrentUnitCost = p.CurrentUnitCost,
                        .TotalQtyOnHand = p.TotalQtyOnHand,
                        .QuarantineQty = p.QuarantineQty
                    })
                Next

                ' 2. Purchase Orders
                PurchaseOrders.Clear()
                Dim poEntities = _context.PurchaseOrders _
                    .Include(Function(p) p.Vendor) _
                    .Include(Function(p) p.Lines).ThenInclude(Function(l) l.Product) _
                    .OrderByDescending(Function(p) p.OrderDate) _
                    .ToList()

                For Each po In poEntities
                    Dim poModel As New WarehousePOModel() With {
                        .POId = po.POId,
                        .DocumentNumber = po.DocumentNumber,
                        .VendorName = If(po.Vendor IsNot Nothing, po.Vendor.Name, ""),
                        .OrderDate = po.OrderDate,
                        .Status = po.Status
                    }

                    For Each l In po.Lines
                        Dim recQty As Integer = _context.GRNLines.Where(Function(gl) gl.POLineId = l.POLineId).Sum(Function(gl) gl.ReceivedQty)
                        poModel.Lines.Add(New WarehousePOLineModel() With {
                            .LineId = l.POLineId,
                            .ProductId = l.ProductId,
                            .SKU = If(l.Product IsNot Nothing, l.Product.SKU, ""),
                            .ProductName = If(l.Product IsNot Nothing, l.Product.Name, ""),
                            .OrderedQty = l.OrderedQty,
                            .ReceivedQty = recQty,
                            .POUnitPrice = l.UnitPrice
                        })
                    Next
                    PurchaseOrders.Add(poModel)
                Next

                ' 3. Goods Receipt Notes
                GoodsReceiptNotes.Clear()
                Dim grnEntities = _context.GoodsReceiptNotes _
                    .Include(Function(g) g.PurchaseOrder).ThenInclude(Function(p) p.Vendor) _
                    .Include(Function(g) g.Lines).ThenInclude(Function(l) l.Product) _
                    .OrderByDescending(Function(g) g.ReceiptDate) _
                    .ToList()

                For Each grn In grnEntities
                    Dim grnModel As New WarehouseGRNModel() With {
                        .GRNId = grn.GRNId,
                        .DocumentNumber = grn.DocumentNumber,
                        .PONumber = If(grn.PurchaseOrder IsNot Nothing, grn.PurchaseOrder.DocumentNumber, ""),
                        .VendorName = If(grn.PurchaseOrder IsNot Nothing AndAlso grn.PurchaseOrder.Vendor IsNot Nothing, grn.PurchaseOrder.Vendor.Name, ""),
                        .ReceiptDate = grn.ReceiptDate,
                        .PostingDate = grn.PostingDate,
                        .Status = grn.Status
                    }

                    For Each l In grn.Lines
                        grnModel.Lines.Add(New WarehouseGRNLineModel() With {
                            .LineId = l.GRNLineId,
                            .ProductId = l.ProductId,
                            .SKU = If(l.Product IsNot Nothing, l.Product.SKU, ""),
                            .ProductName = If(l.Product IsNot Nothing, l.Product.Name, ""),
                            .ReceivedQty = l.ReceivedQty,
                            .POUnitPrice = l.POUnitPrice
                        })
                    Next
                    GoodsReceiptNotes.Add(grnModel)
                Next

                ' 4. RMA List
                RMAList.Clear()
                Dim rmaEntities = _context.CustomerReturns _
                    .Include(Function(r) r.Customer) _
                    .Include(Function(r) r.OriginalInvoice) _
                    .Include(Function(r) r.Lines).ThenInclude(Function(l) l.Product) _
                    .OrderByDescending(Function(r) r.ReturnDate) _
                    .ToList()

                For Each r In rmaEntities
                    For Each l In r.Lines
                        Dim remainingQuarantine As Integer = Math.Max(0, l.ReturnedQty - l.RestockQty - l.ScrapQty)
                        RMAList.Add(New WarehouseRMAModel() With {
                            .RMAId = r.ReturnId,
                            .DocumentNumber = r.DocumentNumber,
                            .CustomerName = If(r.Customer IsNot Nothing, r.Customer.Name, ""),
                            .OriginalInvoiceNumber = If(r.OriginalInvoice IsNot Nothing, r.OriginalInvoice.DocumentNumber, ""),
                            .ProductId = l.ProductId,
                            .SKU = If(l.Product IsNot Nothing, l.Product.SKU, ""),
                            .ProductName = If(l.Product IsNot Nothing, l.Product.Name, ""),
                            .ReturnedQty = l.ReturnedQty,
                            .QuarantineQty = remainingQuarantine,
                            .RestockedQty = l.RestockQty,
                            .ScrappedQty = l.ScrapQty,
                            .OriginalUnitCost = l.OriginalUnitCost,
                            .SellingUnitPrice = l.SellingUnitPrice,
                            .ReturnDate = r.ReturnDate,
                            .Status = r.Status
                        })
                    Next
                Next

                ' 5. Journal History (GRN & RMA entries)
                JournalHistory.Clear()
                Dim jeList = _context.JournalEntries _
                    .Include(Function(j) j.Lines).ThenInclude(Function(l) l.Account) _
                    .Where(Function(j) j.SourceDocType = "GoodsReceiptNote" OrElse j.SourceDocType = "Goods Receipt Note" OrElse j.SourceDocType = "CustomerReturn" OrElse j.SourceDocType = "CreditNote" OrElse j.SourceDocType = "Credit Note") _
                    .OrderByDescending(Function(j) j.PostingDate) _
                    .ThenByDescending(Function(j) j.EntryId) _
                    .ToList()

                For Each entry In jeList
                    Dim sje As New SimpleJournalEntry() With {
                        .DocumentNumber = $"{entry.SourceDocType} #{entry.SourceDocId}",
                        .SourceDocType = entry.SourceDocType,
                        .PostingDate = entry.PostingDate,
                        .Description = entry.Description
                    }
                    For Each l In entry.Lines
                        sje.Lines.Add(New SimpleJournalLine() With {
                            .AccountCode = If(l.Account IsNot Nothing, l.Account.Code, ""),
                            .AccountName = If(l.Account IsNot Nothing, l.Account.Name, ""),
                            .Debit = l.DebitAmount,
                            .Credit = l.CreditAmount,
                            .LineMemo = l.LineMemo
                        })
                    Next
                    JournalHistory.Add(sje)
                Next

            Catch ex As Exception
                ' Graceful handle if DB is uninitialized
            End Try
        End Sub

        Public Function CreateGRN(po As WarehousePOModel, lineReceipts As Dictionary(Of Integer, Integer)) As WarehouseGRNModel
            If po Is Nothing Then Throw New ArgumentNullException(NameOf(po))
            If lineReceipts Is Nothing OrElse lineReceipts.Count = 0 Then
                Throw New InvalidOperationException("At least one line receipt quantity must be provided.")
            End If

            Dim receiptLines As New List(Of GRNLine)()
            For Each kvp In lineReceipts
                If kvp.Value > 0 Then
                    Dim poLine = po.Lines.FirstOrDefault(Function(l) l.LineId = kvp.Key)
                    If poLine IsNot Nothing Then
                        receiptLines.Add(New GRNLine() With {
                            .POLineId = poLine.LineId,
                            .ProductId = poLine.ProductId,
                            .ReceivedQty = kvp.Value,
                            .POUnitPrice = poLine.POUnitPrice
                        })
                    End If
                End If
            Next

            If receiptLines.Count = 0 Then
                Throw New InvalidOperationException("No positive receiving quantities were specified.")
            End If

            Dim grnEntity = _p2pService.ReceiveGoods(po.POId, receiptLines, DateTime.Today)
            LoadData()

            Dim result As New WarehouseGRNModel() With {
                .GRNId = grnEntity.GRNId,
                .DocumentNumber = grnEntity.DocumentNumber,
                .PONumber = po.DocumentNumber,
                .VendorName = po.VendorName,
                .ReceiptDate = grnEntity.ReceiptDate,
                .PostingDate = grnEntity.PostingDate,
                .Status = grnEntity.Status
            }
            For Each rl In receiptLines
                result.Lines.Add(New WarehouseGRNLineModel() With {
                    .ProductId = rl.ProductId,
                    .ReceivedQty = rl.ReceivedQty,
                    .POUnitPrice = rl.POUnitPrice
                })
            Next

            Return result
        End Function

        Public Function IntakeRMAToQuarantine(customerName As String, invoiceNum As String, prod As WarehouseProductModel, returnedQty As Integer, originalCost As Decimal, sellingPrice As Decimal) As WarehouseRMAModel
            If prod Is Nothing Then Throw New ArgumentNullException(NameOf(prod))
            If returnedQty <= 0 Then Throw New ArgumentException("Returned quantity must be greater than zero.", NameOf(returnedQty))

            Dim invoice = _context.SalesInvoices.Include(Function(i) i.Lines).FirstOrDefault(Function(i) i.DocumentNumber.ToLower() = invoiceNum.ToLower())
            Dim customer As Customer = Nothing

            If invoice IsNot Nothing Then
                customer = _context.Customers.Find(invoice.CustomerId)
            Else
                customer = _context.Customers.FirstOrDefault(Function(c) c.Name.ToLower() = customerName.ToLower())
                If customer Is Nothing Then
                    customer = _context.Customers.FirstOrDefault()
                End If
                If customer IsNot Nothing Then
                    invoice = _context.SalesInvoices.Include(Function(i) i.Lines).FirstOrDefault(Function(i) i.CustomerId = customer.CustomerId)
                End If
                If invoice Is Nothing Then
                    invoice = _context.SalesInvoices.Include(Function(i) i.Lines).FirstOrDefault()
                End If
            End If

            If invoice Is Nothing OrElse customer Is Nothing Then
                Throw New InvalidOperationException("No valid sales invoice found to reference for customer return.")
            End If

            Dim retLines As New List(Of CustomerReturnLine)()
            retLines.Add(New CustomerReturnLine() With {
                .ProductId = prod.ProductId,
                .ReturnedQty = returnedQty,
                .OriginalUnitCost = originalCost,
                .SellingUnitPrice = sellingPrice
            })

            Dim rma = _returnsService.ReceiveReturnToQuarantine(customer.CustomerId, invoice.InvoiceId, retLines)
            LoadData()

            Dim model = RMAList.FirstOrDefault(Function(r) r.RMAId = rma.ReturnId)
            If model IsNot Nothing Then Return model

            Return New WarehouseRMAModel() With {
                .RMAId = rma.ReturnId,
                .DocumentNumber = rma.DocumentNumber,
                .CustomerName = If(rma.Customer IsNot Nothing, rma.Customer.Name, customerName),
                .OriginalInvoiceNumber = invoice.DocumentNumber,
                .ProductId = prod.ProductId,
                .SKU = prod.SKU,
                .ProductName = prod.Name,
                .ReturnedQty = returnedQty,
                .QuarantineQty = returnedQty,
                .RestockedQty = 0,
                .ScrappedQty = 0,
                .OriginalUnitCost = originalCost,
                .SellingUnitPrice = sellingPrice,
                .ReturnDate = rma.ReturnDate,
                .Status = rma.Status
            }
        End Function

        Public Function InspectRMA(rma As WarehouseRMAModel, restockQty As Integer, scrapQty As Integer) As SimpleJournalEntry
            If rma Is Nothing Then Throw New ArgumentNullException(NameOf(rma))
            If restockQty < 0 OrElse scrapQty < 0 Then
                Throw New ArgumentException("Disposition quantities cannot be negative.")
            End If
            Dim totalInspected As Integer = restockQty + scrapQty
            If totalInspected <= 0 Then
                Throw New ArgumentException("Must inspect at least 1 unit.")
            End If
            If totalInspected > rma.QuarantineQty Then
                Throw New InvalidOperationException($"Cannot inspect {totalInspected} units. Only {rma.QuarantineQty} units remaining in quarantine.")
            End If

            Dim dispLines As New List(Of ReturnDispositionLine)()
            dispLines.Add(New ReturnDispositionLine() With {
                .ProductId = rma.ProductId,
                .RestockQty = restockQty,
                .ScrapQty = scrapQty
            })

            Dim inspectedReturn = _returnsService.ProcessReturnInspection(rma.RMAId, dispLines)
            LoadData()

            ' Find journal entry for this return inspection
            Dim createdJe = JournalHistory.FirstOrDefault(Function(j) j.SourceDocType = "CustomerReturn" OrElse j.SourceDocType = "CreditNote")
            If createdJe IsNot Nothing Then
                Return createdJe
            End If

            Dim totalRevenueReversal As Decimal = CDec(totalInspected) * rma.SellingUnitPrice
            Dim restockCostVal As Decimal = CDec(restockQty) * rma.OriginalUnitCost
            Dim scrapCostVal As Decimal = CDec(scrapQty) * rma.OriginalUnitCost
            Dim totalCogsReversal As Decimal = restockCostVal + scrapCostVal

            Dim journal As New SimpleJournalEntry() With {
                .DocumentNumber = $"CRN-{inspectedReturn.DocumentNumber}",
                .SourceDocType = "CreditNote",
                .PostingDate = DateTime.Today,
                .Description = $"Inspection Disposition for {inspectedReturn.DocumentNumber}: Restock {restockQty}, Scrap {scrapQty}"
            }
            journal.Lines.Add(New SimpleJournalLine() With {
                .AccountCode = "4100",
                .AccountName = "Sales Returns & Allowances",
                .Debit = totalRevenueReversal,
                .Credit = 0D,
                .LineMemo = $"Sales Return Allowance for {inspectedReturn.DocumentNumber}"
            })
            journal.Lines.Add(New SimpleJournalLine() With {
                .AccountCode = "1200",
                .AccountName = "Accounts Receivable",
                .Debit = 0D,
                .Credit = totalRevenueReversal,
                .LineMemo = $"Customer AR Credit Memo ({rma.CustomerName})"
            })
            If restockQty > 0 Then
                journal.Lines.Add(New SimpleJournalLine() With {
                    .AccountCode = "1300",
                    .AccountName = "Merchandise Inventory",
                    .Debit = restockCostVal,
                    .Credit = 0D,
                    .LineMemo = $"Restock {restockQty} units at ${rma.OriginalUnitCost:N2}"
                })
            End If
            If scrapQty > 0 Then
                journal.Lines.Add(New SimpleJournalLine() With {
                    .AccountCode = "5300",
                    .AccountName = "Inventory Shrinkage & Damaged Stock",
                    .Debit = scrapCostVal,
                    .Credit = 0D,
                    .LineMemo = $"Scrap {scrapQty} damaged units at ${rma.OriginalUnitCost:N2}"
                })
            End If
            journal.Lines.Add(New SimpleJournalLine() With {
                .AccountCode = "5000",
                .AccountName = "Cost of Goods Sold (COGS)",
                .Debit = 0D,
                .Credit = totalCogsReversal,
                .LineMemo = $"Reverse COGS for {totalInspected} returned units"
            })

            Return journal
        End Function

    End Class

End Namespace
