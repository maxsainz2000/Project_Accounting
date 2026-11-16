Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Collections.Generic

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

    Public Class PurchasingMockRepository

        Private Shared _instance As PurchasingMockRepository
        Public Shared ReadOnly Property Instance As PurchasingMockRepository
            Get
                If _instance Is Nothing Then
                    _instance = New PurchasingMockRepository()
                End If
                Return _instance
            End Get
        End Property

        Public Property Vendors As New List(Of PurchasingVendorModel)()
        Public Property CatalogProducts As New List(Of PurchasingProductItem)()
        Public Property PurchaseOrders As New List(Of PurchasingPOModel)()
        Public Property GoodsReceiptNotes As New List(Of PurchasingGRNModel)()
        Public Property ThreeWayMatches As New List(Of ThreeWayMatchModel)()
        Public Property JournalHistory As New List(Of PurchasingJournalEntry)()

        Private _nextPONumber As Integer = 50
        Private _nextBillNumber As Integer = 201

        Public Sub New()
            SeedInitialData()
        End Sub

        Private Sub SeedInitialData()
            ' Vendors
            Vendors.Add(New PurchasingVendorModel() With {
                .VendorId = 1,
                .Code = "VEND-101",
                .Name = "Global Bean Importers Ltd.",
                .PaymentTermsDays = 30,
                .CurrentPayableBalance = 15200.00D
            })
            Vendors.Add(New PurchasingVendorModel() With {
                .VendorId = 2,
                .Code = "VEND-102",
                .Name = "EcoPackaging Supplies Co.",
                .PaymentTermsDays = 15,
                .CurrentPayableBalance = 3450.50D
            })
            Vendors.Add(New PurchasingVendorModel() With {
                .VendorId = 3,
                .Code = "VEND-103",
                .Name = "Ceylon Tea Traders Inc.",
                .PaymentTermsDays = 45,
                .CurrentPayableBalance = 6800.00D
            })
            Vendors.Add(New PurchasingVendorModel() With {
                .VendorId = 4,
                .Code = "VEND-104",
                .Name = "Pacific Freight & Logistics",
                .PaymentTermsDays = 15,
                .CurrentPayableBalance = 1250.00D
            })

            ' Products
            CatalogProducts.Add(New PurchasingProductItem() With {
                .ProductId = 1,
                .SKU = "SKU-BEV-001",
                .Name = "Artisan Colombian Coffee Beans (1kg)",
                .StandardCost = 15.00D
            })
            CatalogProducts.Add(New PurchasingProductItem() With {
                .ProductId = 2,
                .SKU = "SKU-BEV-002",
                .Name = "Organic Earl Grey Tea (500g)",
                .StandardCost = 8.10D
            })
            CatalogProducts.Add(New PurchasingProductItem() With {
                .ProductId = 3,
                .SKU = "SKU-PKG-003",
                .Name = "Biodegradable Hot Cups (Pack of 50)",
                .StandardCost = 4.00D
            })
            CatalogProducts.Add(New PurchasingProductItem() With {
                .ProductId = 4,
                .SKU = "SKU-SNK-004",
                .Name = "Almond Butter Crisps (12pk)",
                .StandardCost = 6.50D
            })

            ' Purchase Orders
            Dim po1 As New PurchasingPOModel() With {
                .POId = 1,
                .DocumentNumber = "PO-2026-0041",
                .VendorId = 1,
                .VendorName = "Global Bean Importers Ltd.",
                .OrderDate = DateTime.Today.AddDays(-5),
                .Status = "Partially Received"
            }
            po1.Lines.Add(New PurchasingPOLineModel() With {
                .LineId = 1,
                .ProductId = 1,
                .SKU = "SKU-BEV-001",
                .ProductName = "Artisan Colombian Coffee Beans (1kg)",
                .OrderedQty = 100,
                .UnitPrice = 15.00D
            })
            PurchaseOrders.Add(po1)

            Dim po2 As New PurchasingPOModel() With {
                .POId = 2,
                .DocumentNumber = "PO-2026-0042",
                .VendorId = 2,
                .VendorName = "EcoPackaging Supplies Co.",
                .OrderDate = DateTime.Today.AddDays(-4),
                .Status = "Fully Received"
            }
            po2.Lines.Add(New PurchasingPOLineModel() With {
                .LineId = 2,
                .ProductId = 3,
                .SKU = "SKU-PKG-003",
                .ProductName = "Biodegradable Hot Cups (Pack of 50)",
                .OrderedQty = 200,
                .UnitPrice = 4.00D
            })
            PurchaseOrders.Add(po2)

            Dim po3 As New PurchasingPOModel() With {
                .POId = 3,
                .DocumentNumber = "PO-2026-0043",
                .VendorId = 3,
                .VendorName = "Ceylon Tea Traders Inc.",
                .OrderDate = DateTime.Today.AddDays(-2),
                .Status = "Approved"
            }
            po3.Lines.Add(New PurchasingPOLineModel() With {
                .LineId = 3,
                .ProductId = 2,
                .SKU = "SKU-BEV-002",
                .ProductName = "Organic Earl Grey Tea (500g)",
                .OrderedQty = 75,
                .UnitPrice = 8.10D
            })
            PurchaseOrders.Add(po3)

            ' GRNs
            GoodsReceiptNotes.Add(New PurchasingGRNModel() With {
                .GRNId = 1,
                .DocumentNumber = "GRN-2026-0088",
                .PONumber = "PO-2026-0041",
                .VendorName = "Global Bean Importers Ltd.",
                .ReceiptDate = DateTime.Today.AddDays(-3),
                .SKU = "SKU-BEV-001",
                .ProductName = "Artisan Colombian Coffee Beans (1kg)",
                .ReceivedQty = 50,
                .POUnitPrice = 15.00D,
                .Status = "Pending Bill"
            })

            GoodsReceiptNotes.Add(New PurchasingGRNModel() With {
                .GRNId = 2,
                .DocumentNumber = "GRN-2026-0099",
                .PONumber = "PO-2026-0042",
                .VendorName = "EcoPackaging Supplies Co.",
                .ReceiptDate = DateTime.Today.AddDays(-2),
                .SKU = "SKU-PKG-003",
                .ProductName = "Biodegradable Hot Cups (Pack of 50)",
                .ReceivedQty = 200,
                .POUnitPrice = 4.00D,
                .Status = "Pending Bill"
            })

            ' 3-Way Match Seed Items
            ' 1. Exact Match Scenario
            ThreeWayMatches.Add(New ThreeWayMatchModel() With {
                .MatchId = 1,
                .PONumber = "PO-2026-0042",
                .GRNNumber = "GRN-2026-0099",
                .VendorName = "EcoPackaging Supplies Co.",
                .SKU = "SKU-PKG-003",
                .ProductName = "Biodegradable Hot Cups (Pack of 50)",
                .POQty = 200,
                .POUnitPrice = 4.00D,
                .GRNQty = 200,
                .BillNumber = "BILL-2026-1088",
                .BillQty = 200,
                .BilledUnitPrice = 4.00D,
                .InboundFreight = 0.00D,
                .MatchStatus = "Exact Match",
                .IsSettled = True
            })

            ' 2. Price Variance (PPV Unfavorable) + Freight
            ThreeWayMatches.Add(New ThreeWayMatchModel() With {
                .MatchId = 2,
                .PONumber = "PO-2026-0041",
                .GRNNumber = "GRN-2026-0088",
                .VendorName = "Global Bean Importers Ltd.",
                .SKU = "SKU-BEV-001",
                .ProductName = "Artisan Colombian Coffee Beans (1kg)",
                .POQty = 100,
                .POUnitPrice = 15.00D,
                .GRNQty = 50,
                .BillNumber = "BILL-2026-1092",
                .BillQty = 50,
                .BilledUnitPrice = 16.20D, ' +$1.20/unit variance
                .InboundFreight = 45.00D,
                .MatchStatus = "PPV Unfavorable (+$60.00)",
                .IsSettled = False
            })

            ' 3. Pending 3-Way Match (Ready for supplier bill entry)
            ThreeWayMatches.Add(New ThreeWayMatchModel() With {
                .MatchId = 3,
                .PONumber = "PO-2026-0043",
                .GRNNumber = "GRN-2026-0104",
                .VendorName = "Ceylon Tea Traders Inc.",
                .SKU = "SKU-BEV-002",
                .ProductName = "Organic Earl Grey Tea (500g)",
                .POQty = 75,
                .POUnitPrice = 8.10D,
                .GRNQty = 75,
                .BillNumber = "",
                .BillQty = 0,
                .BilledUnitPrice = 0.00D,
                .InboundFreight = 0.00D,
                .MatchStatus = "Pending Bill",
                .IsSettled = False
            })
        End Sub

        Public Function CreatePurchaseOrder(vendor As PurchasingVendorModel, lines As List(Of PurchasingPOLineModel)) As PurchasingPOModel
            If vendor Is Nothing Then Throw New ArgumentNullException(NameOf(vendor))
            If lines Is Nothing OrElse lines.Count = 0 Then
                Throw New ArgumentException("PO must have at least one line item.")
            End If

            Dim poDocNum As String = $"PO-2026-{_nextPONumber:D4}"
            _nextPONumber += 1

            Dim po As New PurchasingPOModel() With {
                .POId = PurchaseOrders.Count + 1,
                .DocumentNumber = poDocNum,
                .VendorId = vendor.VendorId,
                .VendorName = vendor.Name,
                .OrderDate = DateTime.Today,
                .Status = "Approved"
            }

            For Each l In lines
                po.Lines.Add(l)
            Next

            PurchaseOrders.Insert(0, po)
            Return po
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

            match.BillNumber = billNumber.Trim()
            match.BillQty = billQty
            match.BilledUnitPrice = billedPrice
            match.InboundFreight = inboundFreight
            match.IsSettled = True

            Dim ppv As Decimal = match.PurchasePriceVariance
            If ppv > 0D Then
                match.MatchStatus = $"PPV Unfavorable (+${ppv:N2})"
            ElseIf ppv < 0D Then
                match.MatchStatus = $"PPV Favorable (-${Math.Abs(ppv):N2})"
            Else
                match.MatchStatus = "Exact Match"
            End If

            ' 3-Way Match Balanced General Ledger Posting
            ' 1. Clear GRNI at PO price: Debit 2100 GRNI = (BillQty * POUnitPrice)
            ' 2. Price Variance: Debit 5100 PPV if Unfavorable, or Credit 5100 PPV if Favorable
            ' 3. Inbound Freight: Debit 5200 Inbound Freight Variance = InboundFreight
            ' 4. Accounts Payable: Credit 2000 Accounts Payable = (BillQty * BilledUnitPrice) + InboundFreight

            Dim grniAmount As Decimal = CDec(billQty) * match.POUnitPrice
            Dim apAmount As Decimal = (CDec(billQty) * billedPrice) + inboundFreight

            Dim journal As New PurchasingJournalEntry() With {
                .DocumentNumber = billNumber,
                .SourceDocType = "SupplierBill",
                .PostingDate = DateTime.Today,
                .Description = $"3-Way Match Clearing for {match.VendorName} (PO {match.PONumber}, GRN {match.GRNNumber}, Bill {billNumber})"
            }

            ' 2100 GRNI Clearing
            journal.Lines.Add(New PurchasingJournalLine() With {
                .AccountCode = "2100",
                .AccountName = "Goods Received Not Invoiced (GRNI)",
                .Debit = grniAmount,
                .Credit = 0D,
                .LineMemo = $"Clear GRNI at PO Unit Price ${match.POUnitPrice:N2} x {billQty}"
            })

            ' 5100 PPV (Debit if Billed > PO, Credit if Billed < PO)
            If ppv > 0D Then
                journal.Lines.Add(New PurchasingJournalLine() With {
                    .AccountCode = "5100",
                    .AccountName = "Purchase Price Variance (PPV)",
                    .Debit = ppv,
                    .Credit = 0D,
                    .LineMemo = $"Unfavorable price variance on {match.SKU} (${billedPrice:N2} vs ${match.POUnitPrice:N2})"
                })
            ElseIf ppv < 0D Then
                journal.Lines.Add(New PurchasingJournalLine() With {
                    .AccountCode = "5100",
                    .AccountName = "Purchase Price Variance (PPV)",
                    .Debit = 0D,
                    .Credit = Math.Abs(ppv),
                    .LineMemo = $"Favorable price variance on {match.SKU} (${billedPrice:N2} vs ${match.POUnitPrice:N2})"
                })
            End If

            ' 5200 Inbound Freight
            If inboundFreight > 0D Then
                journal.Lines.Add(New PurchasingJournalLine() With {
                    .AccountCode = "5200",
                    .AccountName = "Inbound Freight Variance",
                    .Debit = inboundFreight,
                    .Credit = 0D,
                    .LineMemo = $"Inbound carrier freight charge on {billNumber}"
                })
            End If

            ' 2000 Accounts Payable
            journal.Lines.Add(New PurchasingJournalLine() With {
                .AccountCode = "2000",
                .AccountName = "Accounts Payable",
                .Debit = 0D,
                .Credit = apAmount,
                .LineMemo = $"Supplier liability to {match.VendorName} due on Net-30"
            })

            ' Update vendor balance
            Dim v = Vendors.Find(Function(vend) vend.Name.Equals(match.VendorName, StringComparison.OrdinalIgnoreCase))
            If v IsNot Nothing Then
                v.CurrentPayableBalance += apAmount
            End If

            JournalHistory.Insert(0, journal)
            Return journal
        End Function

    End Class

End Namespace
