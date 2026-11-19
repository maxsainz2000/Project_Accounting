Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq

Namespace UI.Sales

    ''' <summary>
    ''' Represents customer account profile and trade receivable exposure.
    ''' </summary>
    Public Class CustomerViewModel
        Public Property CustomerId As Integer
        Public Property Code As String
        Public Property Name As String
        Public Property CreditLimit As Decimal
        Public Property CurrentARBalance As Decimal

        Public ReadOnly Property AvailableCredit As Decimal
            Get
                Return CreditLimit - CurrentARBalance
            End Get
        End Property

        Public ReadOnly Property DisplayText As String
            Get
                Return $"{Code} - {Name}"
            End Get
        End Property

        Public ReadOnly Property CreditStatus As String
            Get
                If AvailableCredit <= 0D Then
                    Return "CREDIT LIMIT EXCEEDED"
                ElseIf AvailableCredit < (CreditLimit * 0.15D) Then
                    Return "NEAR CREDIT LIMIT"
                Else
                    Return "CREDIT APPROVED"
                End If
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Stock status enumeration for badges and guard checks.
    ''' </summary>
    Public Enum StockAvailabilityLevel
        InStock
        LowStock
        InsufficientStock
    End Enum

    ''' <summary>
    ''' Real-time product inventory item with perpetual moving average cost.
    ''' </summary>
    Public Class ProductStockViewModel
        Public Property ProductId As Integer
        Public Property SKU As String
        Public Property Name As String
        Public Property Category As String
        Public Property UnitPrice As Decimal
        Public Property MovingAvgCost As Decimal
        Public Property QtyOnHand As Integer
        Public Property QtyReserved As Integer
        Public Property ReorderLevel As Integer

        Public ReadOnly Property QtyAvailable As Integer
            Get
                Dim avail As Integer = QtyOnHand - QtyReserved
                Return If(avail > 0, avail, 0)
            End Get
        End Property

        Public ReadOnly Property AvailabilityLevel As StockAvailabilityLevel
            Get
                If QtyOnHand <= 0 Then
                    Return StockAvailabilityLevel.InsufficientStock
                ElseIf QtyOnHand <= ReorderLevel Then
                    Return StockAvailabilityLevel.LowStock
                Else
                    Return StockAvailabilityLevel.InStock
                End If
            End Get
        End Property

        Public ReadOnly Property StockBadgeText As String
            Get
                Select Case AvailabilityLevel
                    Case StockAvailabilityLevel.InsufficientStock
                        Return "OUT OF STOCK"
                    Case StockAvailabilityLevel.LowStock
                        Return "LOW STOCK"
                    Case Else
                        Return "IN STOCK"
                End Select
            End Get
        End Property

        Public ReadOnly Property DisplayText As String
            Get
                Return $"{SKU} - {Name} (Available: {QtyAvailable} units)"
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Represents an item line within an active sales order.
    ''' </summary>
    Public Class SalesOrderLineViewModel
        Public Property LineId As Integer
        Public Property ProductId As Integer
        Public Property SKU As String
        Public Property ProductName As String
        Public Property OrderedQty As Integer
        Public Property UnitPrice As Decimal
        Public Property MovingAvgCost As Decimal
        Public Property AvailableAtOrder As Integer

        Public ReadOnly Property LineTotal As Decimal
            Get
                Return CDec(OrderedQty) * UnitPrice
            End Get
        End Property

        Public ReadOnly Property StockBadgeText As String
            Get
                If OrderedQty > AvailableAtOrder Then
                    Return "INSUFFICIENT STOCK"
                ElseIf (AvailableAtOrder - OrderedQty) <= 15 Then
                    Return "LOW STOCK POST-ORDER"
                Else
                    Return "IN STOCK"
                End If
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Commercial customer order commitment header.
    ''' </summary>
    Public Class SalesOrderViewModel
        Public Property SOId As Integer
        Public Property DocumentNumber As String
        Public Property CustomerId As Integer
        Public Property CustomerName As String
        Public Property OrderDate As DateTime
        Public Property Status As String ' Draft, Confirmed, Invoiced, Cancelled
        Public Property Lines As New List(Of SalesOrderLineViewModel)()

        Public ReadOnly Property TotalAmount As Decimal
            Get
                Dim total As Decimal = 0D
                For Each line As SalesOrderLineViewModel In Lines
                    total += line.LineTotal
                Next
                Return total
            End Get
        End Property

        Public ReadOnly Property TotalQuantity As Integer
            Get
                Dim qty As Integer = 0
                For Each line As SalesOrderLineViewModel In Lines
                    qty += line.OrderedQty
                Next
                Return qty
            End Get
        End Property

        Public ReadOnly Property LineCount As Integer
            Get
                Return Lines.Count
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Line fulfillment item on a finalized sales invoice.
    ''' </summary>
    Public Class SalesInvoiceLineViewModel
        Public Property LineId As Integer
        Public Property ProductId As Integer
        Public Property SKU As String
        Public Property ProductName As String
        Public Property FulfilledQty As Integer
        Public Property UnitPrice As Decimal
        Public Property COGSUnitCost As Decimal

        Public ReadOnly Property LineTotal As Decimal
            Get
                Return CDec(FulfilledQty) * UnitPrice
            End Get
        End Property

        Public ReadOnly Property LineCOGS As Decimal
            Get
                Return CDec(FulfilledQty) * COGSUnitCost
            End Get
        End Property

        Public ReadOnly Property LineMargin As Decimal
            Get
                Return LineTotal - LineCOGS
            End Get
        End Property

        Public ReadOnly Property MarginPercentage As Decimal
            Get
                If LineTotal > 0D Then
                    Return Math.Round(((LineTotal - LineCOGS) / LineTotal) * 100D, 2)
                Else
                    Return 0D
                End If
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Combined sales billing and stock dispatch invoice document.
    ''' </summary>
    Public Class SalesInvoiceViewModel
        Public Property InvoiceId As Integer
        Public Property DocumentNumber As String
        Public Property SOId As Integer
        Public Property SODocumentNumber As String
        Public Property CustomerId As Integer
        Public Property CustomerName As String
        Public Property InvoiceDate As DateTime
        Public Property PostingDate As DateTime
        Public Property DueDate As DateTime
        Public Property Status As String ' Posted, Settled
        Public Property Lines As New List(Of SalesInvoiceLineViewModel)()

        Public ReadOnly Property TotalAmount As Decimal
            Get
                Dim total As Decimal = 0D
                For Each line As SalesInvoiceLineViewModel In Lines
                    total += line.LineTotal
                Next
                Return total
            End Get
        End Property

        Public ReadOnly Property TotalCOGS As Decimal
            Get
                Dim total As Decimal = 0D
                For Each line As SalesInvoiceLineViewModel In Lines
                    total += line.LineCOGS
                Next
                Return total
            End Get
        End Property

        Public ReadOnly Property GrossMargin As Decimal
            Get
                Return TotalAmount - TotalCOGS
            End Get
        End Property

        Public ReadOnly Property GrossMarginPct As Decimal
            Get
                If TotalAmount > 0D Then
                    Return Math.Round(((TotalAmount - TotalCOGS) / TotalAmount) * 100D, 2)
                Else
                    Return 0D
                End If
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Balanced double-entry general ledger line preview.
    ''' </summary>
    Public Class JournalEntryPreviewViewModel
        Public Property AccountCode As String
        Public Property AccountName As String
        Public Property DebitAmount As Decimal
        Public Property CreditAmount As Decimal
        Public Property Explanation As String
    End Class

    ''' <summary>
    ''' Result contract for real-time stock availability verification.
    ''' </summary>
    Public Class StockCheckResult
        Public Property IsPermitted As Boolean
        Public Property BadgeText As String
        Public Property BadgeColor As Color
        Public Property AvailableQty As Integer
        Public Property RequestedQty As Integer
        Public Property ShortfallQty As Integer
        Public Property WarningMessage As String
    End Class

    ''' <summary>
    ''' Execution outcome for Sales Invoice generation and GL posting.
    ''' </summary>
    Public Class InvoiceGenerationResult
        Public Property Success As Boolean
        Public Property ErrorMessage As String
        Public Property Invoice As SalesInvoiceViewModel
        Public Property JournalLines As New List(Of JournalEntryPreviewViewModel)()
    End Class

    ''' <summary>
    ''' In-memory mock data store simulating the database and Order-to-Cash service.
    ''' </summary>
    Public Class MockSalesDataService

        Private Shared _instance As MockSalesDataService
        Public Shared ReadOnly Property Instance As MockSalesDataService
            Get
                If _instance Is Nothing Then
                    _instance = New MockSalesDataService()
                End If
                Return _instance
            End Get
        End Property

        Public ReadOnly Property Customers As New List(Of CustomerViewModel)()
        Public ReadOnly Property Products As New List(Of ProductStockViewModel)()
        Public ReadOnly Property SalesOrders As New List(Of SalesOrderViewModel)()
        Public ReadOnly Property SalesInvoices As New List(Of SalesInvoiceViewModel)()

        Private _nextSOId As Integer = 100
        Private _nextInvoiceId As Integer = 200

        Private Sub New()
            InitializeSeedData()
        End Sub

        Private Sub InitializeSeedData()
            ' Initialize Sample Customers
            Customers.Add(New CustomerViewModel() With {
                .CustomerId = 1,
                .Code = "CUST-001",
                .Name = "Apex Wholesale Distributors Ltd.",
                .CreditLimit = 50000.0D,
                .CurrentARBalance = 14250.0D
            })
            Customers.Add(New CustomerViewModel() With {
                .CustomerId = 2,
                .Code = "CUST-002",
                .Name = "Beacon Commercial Mart Chain",
                .CreditLimit = 35000.0D,
                .CurrentARBalance = 6800.0D
            })
            Customers.Add(New CustomerViewModel() With {
                .CustomerId = 3,
                .Code = "CUST-003",
                .Name = "Crestview Gourmet Foods Inc.",
                .CreditLimit = 15000.0D,
                .CurrentARBalance = 15000.0D ' Exactly at credit limit
            })
            Customers.Add(New CustomerViewModel() With {
                .CustomerId = 4,
                .Code = "CUST-004",
                .Name = "Delta Express Mart",
                .CreditLimit = 10000.0D,
                .CurrentARBalance = 1200.0D
            })

            ' Initialize Sample Products (with inventory counts, reorder thresholds, and moving avg cost)
            Products.Add(New ProductStockViewModel() With {
                .ProductId = 101,
                .SKU = "SKU-BEV-001",
                .Name = "Arabica Reserve Whole Bean Coffee (1kg)",
                .Category = "Beverages",
                .UnitPrice = 24.5D,
                .MovingAvgCost = 14.8D,
                .QtyOnHand = 120,
                .QtyReserved = 20,
                .ReorderLevel = 25
            })
            Products.Add(New ProductStockViewModel() With {
                .ProductId = 102,
                .SKU = "SKU-BEV-002",
                .Name = "Single Origin Matcha Ceremonial Grade (250g)",
                .Category = "Beverages",
                .UnitPrice = 38.0D,
                .MovingAvgCost = 22.5D,
                .QtyOnHand = 15,
                .QtyReserved = 0,
                .ReorderLevel = 20 ' LOW STOCK
            })
            Products.Add(New ProductStockViewModel() With {
                .ProductId = 103,
                .SKU = "SKU-SNK-001",
                .Name = "Single Estate Dark Chocolate 72% (100g)",
                .Category = "Confectionery",
                .UnitPrice = 6.25D,
                .MovingAvgCost = 3.4D,
                .QtyOnHand = 0,
                .QtyReserved = 0,
                .ReorderLevel = 50 ' OUT OF STOCK (Hard-Stop demonstration)
            })
            Products.Add(New ProductStockViewModel() With {
                .ProductId = 104,
                .SKU = "SKU-BEV-003",
                .Name = "Cold Brew Sparkling Citrus Tonic (330ml)",
                .Category = "Beverages",
                .UnitPrice = 3.5D,
                .MovingAvgCost = 1.95D,
                .QtyOnHand = 350,
                .QtyReserved = 50,
                .ReorderLevel = 60
            })
            Products.Add(New ProductStockViewModel() With {
                .ProductId = 105,
                .SKU = "SKU-SNK-002",
                .Name = "Roasted Sea Salt & Truffle Cashews (500g)",
                .Category = "Snacks",
                .UnitPrice = 16.0D,
                .MovingAvgCost = 9.75D,
                .QtyOnHand = 8,
                .QtyReserved = 0,
                .ReorderLevel = 15 ' LOW STOCK
            })

            ' Initialize Seed Sales Orders
            Dim so1 As New SalesOrderViewModel() With {
                .SOId = 1,
                .DocumentNumber = "SO-2026-0001",
                .CustomerId = 1,
                .CustomerName = "Apex Wholesale Distributors Ltd.",
                .OrderDate = DateTime.Today.AddDays(-2),
                .Status = "Confirmed"
            }
            so1.Lines.Add(New SalesOrderLineViewModel() With {
                .LineId = 1,
                .ProductId = 101,
                .SKU = "SKU-BEV-001",
                .ProductName = "Arabica Reserve Whole Bean Coffee (1kg)",
                .OrderedQty = 20,
                .UnitPrice = 24.5D,
                .MovingAvgCost = 14.8D,
                .AvailableAtOrder = 140
            })
            so1.Lines.Add(New SalesOrderLineViewModel() With {
                .LineId = 2,
                .ProductId = 104,
                .SKU = "SKU-BEV-003",
                .ProductName = "Cold Brew Sparkling Citrus Tonic (330ml)",
                .OrderedQty = 50,
                .UnitPrice = 3.5D,
                .MovingAvgCost = 1.95D,
                .AvailableAtOrder = 400
            })
            SalesOrders.Add(so1)

            Dim so2 As New SalesOrderViewModel() With {
                .SOId = 2,
                .DocumentNumber = "SO-2026-0002",
                .CustomerId = 2,
                .CustomerName = "Beacon Commercial Mart Chain",
                .OrderDate = DateTime.Today.AddDays(-1),
                .Status = "Draft"
            }
            so2.Lines.Add(New SalesOrderLineViewModel() With {
                .LineId = 3,
                .ProductId = 102,
                .SKU = "SKU-BEV-002",
                .ProductName = "Single Origin Matcha Ceremonial Grade (250g)",
                .OrderedQty = 5,
                .UnitPrice = 38.0D,
                .MovingAvgCost = 22.5D,
                .AvailableAtOrder = 15
            })
            SalesOrders.Add(so2)
        End Sub

        ''' <summary>
        ''' Evaluates stock availability in real time against requested dispatch count.
        ''' </summary>
        Public Function EvaluateStockAvailability(product As ProductStockViewModel, requestedQty As Integer) As StockCheckResult
            Dim result As New StockCheckResult() With {
                .RequestedQty = requestedQty,
                .AvailableQty = If(product IsNot Nothing, product.QtyAvailable, 0)
            }

            If product Is Nothing Then
                result.IsPermitted = False
                result.BadgeText = "PRODUCT NOT FOUND"
                result.BadgeColor = Color.DarkGray
                result.WarningMessage = "Selected SKU could not be found in active product inventory."
                Return result
            End If

            If requestedQty <= 0 Then
                result.IsPermitted = False
                result.BadgeText = "INVALID QUANTITY"
                result.BadgeColor = Color.DarkGray
                result.WarningMessage = "Ordered quantity must be greater than zero."
                Return result
            End If

            If requestedQty > product.QtyOnHand Then
                result.IsPermitted = False
                result.ShortfallQty = requestedQty - product.QtyOnHand
                result.BadgeText = "HARD STOP: INSUFFICIENT STOCK"
                result.BadgeColor = Color.FromArgb(220, 53, 69) ' Crimson Red
                result.WarningMessage = $"CRITICAL HARD-STOP: Requested {requestedQty} units, but physical on-hand stock is only {product.QtyOnHand} units. Deficit: {result.ShortfallQty} units. Negative inventory is strictly prohibited by ERP accounting controls."
            ElseIf requestedQty > product.QtyAvailable Then
                result.IsPermitted = False
                result.ShortfallQty = requestedQty - product.QtyAvailable
                result.BadgeText = "STOCK RESERVED BY OTHER ORDERS"
                result.BadgeColor = Color.FromArgb(253, 126, 20) ' Orange
                result.WarningMessage = $"Reservation conflict: {product.QtyReserved} units are allocated to other confirmed orders. Only {product.QtyAvailable} units currently free to promise."
            ElseIf (product.QtyAvailable - requestedQty) <= product.ReorderLevel Then
                result.IsPermitted = True
                result.BadgeText = "WARNING: REORDER THRESHOLD"
                result.BadgeColor = Color.FromArgb(255, 193, 7) ' Warning Amber
                result.WarningMessage = $"Order acceptable, but remaining balance ({product.QtyAvailable - requestedQty} units) will drop below reorder threshold ({product.ReorderLevel} units)."
            Else
                result.IsPermitted = True
                result.BadgeText = "IN STOCK: READY"
                result.BadgeColor = Color.FromArgb(40, 167, 69) ' Success Green
                result.WarningMessage = $"Sufficient stock verified. Available: {product.QtyAvailable} units. Order quantity: {requestedQty} units."
            End If

            Return result
        End Function

        ''' <summary>
        ''' Registers a new sales order commitment.
        ''' </summary>
        Public Function RegisterSalesOrder(so As SalesOrderViewModel) As SalesOrderViewModel
            _nextSOId += 1
            so.SOId = _nextSOId
            If String.IsNullOrWhiteSpace(so.DocumentNumber) Then
                so.DocumentNumber = $"SO-2026-{_nextSOId:D4}"
            End If

            ' Reserve quantities if confirmed
            If String.Equals(so.Status, "Confirmed", StringComparison.Ordinal) Then
                For Each line As SalesOrderLineViewModel In so.Lines
                    Dim prod As ProductStockViewModel = Products.FirstOrDefault(Function(p) p.ProductId = line.ProductId)
                    If prod IsNot Nothing Then
                        prod.QtyReserved += line.OrderedQty
                    End If
                Next
            End If

            SalesOrders.Insert(0, so)
            Return so
        End Function

        ''' <summary>
        ''' Generates Sales Invoice and executes double-entry General Ledger postings.
        ''' Enforces the zero negative stock invariant with an atomic hard-stop block.
        ''' </summary>
        Public Function GenerateSalesInvoice(soId As Integer, invoiceDate As DateTime, postingDate As DateTime) As InvoiceGenerationResult
            Dim outcome As New InvoiceGenerationResult()

            Dim so As SalesOrderViewModel = SalesOrders.FirstOrDefault(Function(o) o.SOId = soId)
            If so Is Nothing Then
                outcome.Success = False
                outcome.ErrorMessage = $"Sales Order ID #{soId} was not found."
                Return outcome
            End If

            If String.Equals(so.Status, "Invoiced", StringComparison.Ordinal) Then
                outcome.Success = False
                outcome.ErrorMessage = $"Sales Order {so.DocumentNumber} is already fully invoiced."
                Return outcome
            End If

            If so.Lines.Count = 0 Then
                outcome.Success = False
                outcome.ErrorMessage = $"Sales Order {so.DocumentNumber} contains zero line items."
                Return outcome
            End If

            ' --- STEP 1: HARD-STOP STOCK VALIDATION GATE ---
            ' Check every line before modifying any state. Zero negative inventory rule.
            For Each line As SalesOrderLineViewModel In so.Lines
                Dim prod As ProductStockViewModel = Products.FirstOrDefault(Function(p) p.ProductId = line.ProductId)
                If prod Is Nothing Then
                    outcome.Success = False
                    outcome.ErrorMessage = $"Product '{line.SKU}' could not be located in product master."
                    Return outcome
                End If

                If line.OrderedQty > prod.QtyOnHand Then
                    outcome.Success = False
                    outcome.ErrorMessage = $"INSUFFICIENT STOCK HARD-STOP: Product '{prod.SKU}' ({prod.Name}) has only {prod.QtyOnHand} units on hand, but order requires {line.OrderedQty} units. Disallow negative inventory."
                    Return outcome
                End If
            Next

            ' --- STEP 2: ATOMIC INVENTORY DECREMENT & INVOICE GENERATION ---
            _nextInvoiceId += 1
            Dim inv As New SalesInvoiceViewModel() With {
                .InvoiceId = _nextInvoiceId,
                .DocumentNumber = $"INV-2026-{_nextInvoiceId:D4}",
                .SOId = so.SOId,
                .SODocumentNumber = so.DocumentNumber,
                .CustomerId = so.CustomerId,
                .CustomerName = so.CustomerName,
                .InvoiceDate = invoiceDate,
                .PostingDate = postingDate,
                .DueDate = invoiceDate.AddDays(30),
                .Status = "Posted"
            }

            Dim lineIndex As Integer = 0
            For Each soLine As SalesOrderLineViewModel In so.Lines
                lineIndex += 1
                Dim prod As ProductStockViewModel = Products.First(Function(p) p.ProductId = soLine.ProductId)

                ' Atomic decrement of physical stock
                prod.QtyOnHand -= soLine.OrderedQty
                ' Decrement reservation
                If prod.QtyReserved >= soLine.OrderedQty Then
                    prod.QtyReserved -= soLine.OrderedQty
                Else
                    prod.QtyReserved = 0
                End If

                Dim invLine As New SalesInvoiceLineViewModel() With {
                    .LineId = lineIndex,
                    .ProductId = prod.ProductId,
                    .SKU = prod.SKU,
                    .ProductName = prod.Name,
                    .FulfilledQty = soLine.OrderedQty,
                    .UnitPrice = soLine.UnitPrice,
                    .COGSUnitCost = prod.MovingAvgCost
                }
                inv.Lines.Add(invLine)
            Next

            ' Update Customer AR Exposure
            Dim customer As CustomerViewModel = Customers.FirstOrDefault(Function(c) c.CustomerId = so.CustomerId)
            If customer IsNot Nothing Then
                customer.CurrentARBalance += inv.TotalAmount
            End If

            ' Mark Sales Order as Invoiced
            so.Status = "Invoiced"

            ' Record Invoice
            SalesInvoices.Insert(0, inv)

            ' --- STEP 3: CONSTRUCT BALANCED DOUBLE-ENTRY GL PREVIEW ---
            ' Debit 1200 Accounts Receivable (Asset) = Total Sales Value
            outcome.JournalLines.Add(New JournalEntryPreviewViewModel() With {
                .AccountCode = "1200",
                .AccountName = "Accounts Receivable",
                .DebitAmount = inv.TotalAmount,
                .CreditAmount = 0D,
                .Explanation = $"Customer invoice billing for {inv.CustomerName} ({inv.DocumentNumber})"
            })

            ' Credit 4000 Sales Revenue (Revenue) = Total Sales Value
            outcome.JournalLines.Add(New JournalEntryPreviewViewModel() With {
                .AccountCode = "4000",
                .AccountName = "Sales Revenue",
                .DebitAmount = 0D,
                .CreditAmount = inv.TotalAmount,
                .Explanation = $"Revenue recognized from merchandise sales fulfillment ({inv.SODocumentNumber})"
            })

            ' Debit 5000 Cost of Goods Sold (Expense) = Perpetual COGS
            outcome.JournalLines.Add(New JournalEntryPreviewViewModel() With {
                .AccountCode = "5000",
                .AccountName = "Cost of Goods Sold (COGS)",
                .DebitAmount = inv.TotalCOGS,
                .CreditAmount = 0D,
                .Explanation = $"Moving average inventory cost recognized upon stock dispatch ({inv.DocumentNumber})"
            })

            ' Credit 1300 Merchandise Inventory (Asset) = Perpetual Inventory Reduction
            outcome.JournalLines.Add(New JournalEntryPreviewViewModel() With {
                .AccountCode = "1300",
                .AccountName = "Merchandise Inventory",
                .DebitAmount = 0D,
                .CreditAmount = inv.TotalCOGS,
                .Explanation = $"Relief of perpetual warehouse inventory asset balance ({inv.SODocumentNumber})"
            })

            outcome.Success = True
            outcome.Invoice = inv
            Return outcome
        End Function

    End Class

End Namespace
