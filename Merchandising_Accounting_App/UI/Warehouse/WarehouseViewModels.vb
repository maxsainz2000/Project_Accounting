Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Collections.Generic

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

    Public Class WarehouseMockRepository

        Private Shared _instance As WarehouseMockRepository
        Public Shared ReadOnly Property Instance As WarehouseMockRepository
            Get
                If _instance Is Nothing Then
                    _instance = New WarehouseMockRepository()
                End If
                Return _instance
            End Get
        End Property

        Public Property Products As New List(Of WarehouseProductModel)()
        Public Property PurchaseOrders As New List(Of WarehousePOModel)()
        Public Property GoodsReceiptNotes As New List(Of WarehouseGRNModel)()
        Public Property RMAList As New List(Of WarehouseRMAModel)()
        Public Property JournalHistory As New List(Of SimpleJournalEntry)()

        Private _nextGRNNumber As Integer = 101
        Private _nextRMANumber As Integer = 501

        Public Sub New()
            SeedInitialData()
        End Sub

        Private Sub SeedInitialData()
            ' Products catalog
            Products.Add(New WarehouseProductModel() With {
                .ProductId = 1,
                .SKU = "SKU-BEV-001",
                .Name = "Artisan Colombian Coffee Beans (1kg)",
                .BaseUnit = "Bag",
                .CurrentUnitCost = 14.50D,
                .TotalQtyOnHand = 120,
                .QuarantineQty = 5
            })

            Products.Add(New WarehouseProductModel() With {
                .ProductId = 2,
                .SKU = "SKU-BEV-002",
                .Name = "Organic Earl Grey Tea (500g)",
                .BaseUnit = "Tin",
                .CurrentUnitCost = 8.20D,
                .TotalQtyOnHand = 85,
                .QuarantineQty = 0
            })

            Products.Add(New WarehouseProductModel() With {
                .ProductId = 3,
                .SKU = "SKU-PKG-003",
                .Name = "Biodegradable Hot Cups (Pack of 50)",
                .BaseUnit = "Pack",
                .CurrentUnitCost = 4.10D,
                .TotalQtyOnHand = 300,
                .QuarantineQty = 12
            })

            Products.Add(New WarehouseProductModel() With {
                .ProductId = 4,
                .SKU = "SKU-SNK-004",
                .Name = "Almond Butter Crisps (12pk)",
                .BaseUnit = "Box",
                .CurrentUnitCost = 6.75D,
                .TotalQtyOnHand = 60,
                .QuarantineQty = 0
            })

            ' Purchase Orders
            Dim po1 As New WarehousePOModel() With {
                .POId = 1,
                .DocumentNumber = "PO-2026-0041",
                .VendorName = "Global Bean Importers Ltd.",
                .OrderDate = DateTime.Today.AddDays(-3),
                .Status = "Open"
            }
            po1.Lines.Add(New WarehousePOLineModel() With {
                .LineId = 1,
                .ProductId = 1,
                .SKU = "SKU-BEV-001",
                .ProductName = "Artisan Colombian Coffee Beans (1kg)",
                .OrderedQty = 100,
                .ReceivedQty = 0,
                .POUnitPrice = 15.00D
            })
            PurchaseOrders.Add(po1)

            Dim po2 As New WarehousePOModel() With {
                .POId = 2,
                .DocumentNumber = "PO-2026-0042",
                .VendorName = "EcoPackaging Supplies Co.",
                .OrderDate = DateTime.Today.AddDays(-2),
                .Status = "Open"
            }
            po2.Lines.Add(New WarehousePOLineModel() With {
                .LineId = 2,
                .ProductId = 3,
                .SKU = "SKU-PKG-003",
                .ProductName = "Biodegradable Hot Cups (Pack of 50)",
                .OrderedQty = 200,
                .ReceivedQty = 50,
                .POUnitPrice = 4.00D
            })
            PurchaseOrders.Add(po2)

            Dim po3 As New WarehousePOModel() With {
                .POId = 3,
                .DocumentNumber = "PO-2026-0043",
                .VendorName = "Ceylon Tea Traders Inc.",
                .OrderDate = DateTime.Today.AddDays(-1),
                .Status = "Open"
            }
            po3.Lines.Add(New WarehousePOLineModel() With {
                .LineId = 3,
                .ProductId = 2,
                .SKU = "SKU-BEV-002",
                .ProductName = "Organic Earl Grey Tea (500g)",
                .OrderedQty = 75,
                .ReceivedQty = 0,
                .POUnitPrice = 8.10D
            })
            PurchaseOrders.Add(po3)

            ' Existing GRN (historical)
            Dim grn0 As New WarehouseGRNModel() With {
                .GRNId = 1,
                .DocumentNumber = "GRN-2026-0099",
                .PONumber = "PO-2026-0042",
                .VendorName = "EcoPackaging Supplies Co.",
                .ReceiptDate = DateTime.Today.AddDays(-1),
                .PostingDate = DateTime.Today.AddDays(-1),
                .Status = "Posted"
            }
            grn0.Lines.Add(New WarehouseGRNLineModel() With {
                .LineId = 1,
                .ProductId = 3,
                .SKU = "SKU-PKG-003",
                .ProductName = "Biodegradable Hot Cups (Pack of 50)",
                .ReceivedQty = 50,
                .POUnitPrice = 4.00D
            })
            GoodsReceiptNotes.Add(grn0)

            ' RMA Returns in Quarantine
            RMAList.Add(New WarehouseRMAModel() With {
                .RMAId = 1,
                .DocumentNumber = "RMA-2026-0012",
                .CustomerName = "Apex Coffee Lounge",
                .OriginalInvoiceNumber = "INV-2026-0189",
                .ProductId = 1,
                .SKU = "SKU-BEV-001",
                .ProductName = "Artisan Colombian Coffee Beans (1kg)",
                .ReturnedQty = 5,
                .QuarantineQty = 5,
                .RestockedQty = 0,
                .ScrappedQty = 0,
                .OriginalUnitCost = 14.50D,
                .SellingUnitPrice = 24.00D,
                .ReturnDate = DateTime.Today.AddDays(-2),
                .Status = "Quarantine"
            })

            RMAList.Add(New WarehouseRMAModel() With {
                .RMAId = 2,
                .DocumentNumber = "RMA-2026-0015",
                .CustomerName = "Metro Catering Group",
                .OriginalInvoiceNumber = "INV-2026-0192",
                .ProductId = 3,
                .SKU = "SKU-PKG-003",
                .ProductName = "Biodegradable Hot Cups (Pack of 50)",
                .ReturnedQty = 12,
                .QuarantineQty = 12,
                .RestockedQty = 0,
                .ScrappedQty = 0,
                .OriginalUnitCost = 4.10D,
                .SellingUnitPrice = 7.50D,
                .ReturnDate = DateTime.Today.AddDays(-1),
                .Status = "Quarantine"
            })
        End Sub

        Public Function CreateGRN(po As WarehousePOModel, lineReceipts As Dictionary(Of Integer, Integer)) As WarehouseGRNModel
            If po Is Nothing Then Throw New ArgumentNullException(NameOf(po))
            If lineReceipts Is Nothing OrElse lineReceipts.Count = 0 Then
                Throw New InvalidOperationException("At least one line receipt quantity must be provided.")
            End If

            Dim grnDocNumber As String = $"GRN-2026-{_nextGRNNumber:D4}"
            _nextGRNNumber += 1

            Dim grn As New WarehouseGRNModel() With {
                .GRNId = GoodsReceiptNotes.Count + 1,
                .DocumentNumber = grnDocNumber,
                .PONumber = po.DocumentNumber,
                .VendorName = po.VendorName,
                .ReceiptDate = DateTime.Today,
                .PostingDate = DateTime.Today,
                .Status = "Posted"
            }

            Dim journalEntry As New SimpleJournalEntry() With {
                .DocumentNumber = grnDocNumber,
                .SourceDocType = "GoodsReceiptNote",
                .PostingDate = DateTime.Today,
                .Description = $"Inbound Dock Receipt from {po.VendorName} under {po.DocumentNumber}"
            }

            Dim totalValuation As Decimal = 0D

            For Each line In po.Lines
                If lineReceipts.ContainsKey(line.LineId) Then
                    Dim recQty As Integer = lineReceipts(line.LineId)
                    If recQty > 0 Then
                        Dim grnLine As New WarehouseGRNLineModel() With {
                            .LineId = grn.Lines.Count + 1,
                            .ProductId = line.ProductId,
                            .SKU = line.SKU,
                            .ProductName = line.ProductName,
                            .ReceivedQty = recQty,
                            .POUnitPrice = line.POUnitPrice
                        }
                        grn.Lines.Add(grnLine)

                        ' Update PO line received count
                        line.ReceivedQty += recQty

                        ' Update product inventory and perpetual moving average cost
                        Dim prod = Products.Find(Function(p) p.ProductId = line.ProductId)
                        If prod IsNot Nothing Then
                            Dim oldQty As Integer = prod.TotalQtyOnHand
                            Dim oldCost As Decimal = prod.CurrentUnitCost
                            Dim totalNewQty As Integer = oldQty + recQty

                            If totalNewQty > 0 Then
                                Dim totalValue As Decimal = (CDec(oldQty) * oldCost) + (CDec(recQty) * line.POUnitPrice)
                                prod.CurrentUnitCost = Math.Round(totalValue / CDec(totalNewQty), 4)
                            End If

                            prod.TotalQtyOnHand = totalNewQty
                        End If

                        totalValuation += grnLine.ExtendedCost
                    End If
                End If
            Next

            If grn.Lines.Count = 0 Then
                Throw New InvalidOperationException("No positive receiving quantities were specified.")
            End If

            ' Update PO overall status
            If po.TotalPendingQty = 0 Then
                po.Status = "Fully Received"
            Else
                po.Status = "Partially Received"
            End If

            ' Generate balanced GL journal lines:
            ' Debit 1300 Merchandise Inventory (Received Qty * PO Unit Price)
            ' Credit 2100 GRNI Clearing (Received Qty * PO Unit Price)
            journalEntry.Lines.Add(New SimpleJournalLine() With {
                .AccountCode = "1300",
                .AccountName = "Merchandise Inventory",
                .Debit = totalValuation,
                .Credit = 0D,
                .LineMemo = $"GRN {grnDocNumber} Stock Intake ({po.DocumentNumber})"
            })
            journalEntry.Lines.Add(New SimpleJournalLine() With {
                .AccountCode = "2100",
                .AccountName = "Goods Received Not Invoiced (GRNI)",
                .Debit = 0D,
                .Credit = totalValuation,
                .LineMemo = $"GRN {grnDocNumber} GRNI Accrual ({po.DocumentNumber})"
            })

            GoodsReceiptNotes.Insert(0, grn)
            JournalHistory.Insert(0, journalEntry)

            Return grn
        End Function

        Public Function IntakeRMAToQuarantine(customerName As String, invoiceNum As String, prod As WarehouseProductModel, returnedQty As Integer, originalCost As Decimal, sellingPrice As Decimal) As WarehouseRMAModel
            If prod Is Nothing Then Throw New ArgumentNullException(NameOf(prod))
            If returnedQty <= 0 Then Throw New ArgumentException("Returned quantity must be greater than zero.", NameOf(returnedQty))

            Dim rmaDocNum As String = $"RMA-2026-{_nextRMANumber:D4}"
            _nextRMANumber += 1

            Dim rma As New WarehouseRMAModel() With {
                .RMAId = RMAList.Count + 1,
                .DocumentNumber = rmaDocNum,
                .CustomerName = customerName,
                .OriginalInvoiceNumber = invoiceNum,
                .ProductId = prod.ProductId,
                .SKU = prod.SKU,
                .ProductName = prod.Name,
                .ReturnedQty = returnedQty,
                .QuarantineQty = returnedQty,
                .RestockedQty = 0,
                .ScrappedQty = 0,
                .OriginalUnitCost = originalCost,
                .SellingUnitPrice = sellingPrice,
                .ReturnDate = DateTime.Today,
                .Status = "Quarantine"
            }

            ' Increment Quarantine Qty on Product (Operational non-valuated quarantine - NO GL posting)
            prod.QuarantineQty += returnedQty

            RMAList.Insert(0, rma)
            Return rma
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

            Dim prod = Products.Find(Function(p) p.ProductId = rma.ProductId)
            If prod Is Nothing Then Throw New InvalidOperationException("Product record not found.")

            ' Decrement product quarantine stock
            prod.QuarantineQty = Math.Max(0, prod.QuarantineQty - totalInspected)
            rma.QuarantineQty -= totalInspected
            rma.RestockedQty += restockQty
            rma.ScrappedQty += scrapQty

            If rma.QuarantineQty = 0 Then
                rma.Status = "Completed"
            Else
                rma.Status = "Partially Inspected"
            End If

            ' Restock updates perpetual moving average cost and TotalQtyOnHand
            If restockQty > 0 Then
                Dim oldQty As Integer = prod.TotalQtyOnHand
                Dim oldCost As Decimal = prod.CurrentUnitCost
                Dim newQty As Integer = oldQty + restockQty
                If newQty > 0 Then
                    Dim blendedValue As Decimal = (CDec(oldQty) * oldCost) + (CDec(restockQty) * rma.OriginalUnitCost)
                    prod.CurrentUnitCost = Math.Round(blendedValue / CDec(newQty), 4)
                End If
                prod.TotalQtyOnHand = newQty
            End If

            ' Financial Credit Note Generation
            ' Credit Note Selling Price Total: totalInspected * SellingUnitPrice
            Dim totalRevenueReversal As Decimal = CDec(totalInspected) * rma.SellingUnitPrice
            Dim restockCostVal As Decimal = CDec(restockQty) * rma.OriginalUnitCost
            Dim scrapCostVal As Decimal = CDec(scrapQty) * rma.OriginalUnitCost
            Dim totalCogsReversal As Decimal = restockCostVal + scrapCostVal

            Dim journal As New SimpleJournalEntry() With {
                .DocumentNumber = $"CRN-{rma.DocumentNumber}",
                .SourceDocType = "CreditNote",
                .PostingDate = DateTime.Today,
                .Description = $"Inspection Disposition for {rma.DocumentNumber} ({rma.CustomerName}): Restock {restockQty}, Scrap {scrapQty}"
            }

            ' Revenue / AR reversal
            journal.Lines.Add(New SimpleJournalLine() With {
                .AccountCode = "4100",
                .AccountName = "Sales Returns & Allowances",
                .Debit = totalRevenueReversal,
                .Credit = 0D,
                .LineMemo = $"Sales Return Allowance for {rma.DocumentNumber}"
            })
            journal.Lines.Add(New SimpleJournalLine() With {
                .AccountCode = "1200",
                .AccountName = "Accounts Receivable",
                .Debit = 0D,
                .Credit = totalRevenueReversal,
                .LineMemo = $"Customer AR Credit Memo ({rma.CustomerName})"
            })

            ' Inventory / Scrap / COGS entries
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

            JournalHistory.Insert(0, journal)
            Return journal
        End Function

    End Class

End Namespace
