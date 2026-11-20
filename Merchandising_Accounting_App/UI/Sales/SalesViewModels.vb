Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Drawing
Imports System.Linq
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models
Imports Merchandising_Accounting_App.Services

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
    ''' Order-to-Cash data service binding to OrderToCashService and live Entity Framework repositories.
    ''' </summary>
    Public Class SalesDataService
        Implements IDisposable

        Private ReadOnly _o2cService As OrderToCashService
        Private ReadOnly _context As ErpDbContext
        Private _disposed As Boolean = False

        Public Property Customers As New List(Of CustomerViewModel)()
        Public Property Products As New List(Of ProductStockViewModel)()
        Public Property SalesOrders As New List(Of SalesOrderViewModel)()
        Public Property SalesInvoices As New List(Of SalesInvoiceViewModel)()

        Public Sub New(Optional o2cService As OrderToCashService = Nothing, Optional context As ErpDbContext = Nothing)
            _context = If(context, New ErpDbContext())
            _o2cService = If(o2cService, New OrderToCashService(_context))
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
                ' 1. Customers
                Customers.Clear()
                For Each c In _context.Customers.OrderBy(Function(x) x.Name).ToList()
                    Customers.Add(New CustomerViewModel() With {
                        .CustomerId = c.CustomerId,
                        .Code = c.Code,
                        .Name = c.Name,
                        .CreditLimit = c.CreditLimit,
                        .CurrentARBalance = c.CurrentARBalance
                    })
                Next

                ' 2. Products
                Products.Clear()
                For Each p In _context.Products.Where(Function(x) x.IsActive).OrderBy(Function(x) x.SKU).ToList()
                    Dim estPrice As Decimal = If(p.CurrentUnitCost > 0D, Math.Round(p.CurrentUnitCost * 1.6D, 2), 20.00D)
                    Products.Add(New ProductStockViewModel() With {
                        .ProductId = p.ProductId,
                        .SKU = p.SKU,
                        .Name = p.Name,
                        .Category = If(p.SKU.Contains("BEV"), "Beverages", If(p.SKU.Contains("PKG"), "Packaging", "Snacks")),
                        .UnitPrice = estPrice,
                        .MovingAvgCost = p.CurrentUnitCost,
                        .QtyOnHand = p.TotalQtyOnHand,
                        .QtyReserved = p.AllocatedQty,
                        .ReorderLevel = 25
                    })
                Next

                ' 3. Sales Orders
                SalesOrders.Clear()
                Dim soEntities = _context.SalesOrders _
                    .Include(Function(s) s.Customer) _
                    .Include(Function(s) s.Lines).ThenInclude(Function(l) l.Product) _
                    .OrderByDescending(Function(s) s.OrderDate) _
                    .ToList()

                For Each so In soEntities
                    Dim soVm As New SalesOrderViewModel() With {
                        .SOId = so.SOId,
                        .DocumentNumber = so.DocumentNumber,
                        .CustomerId = so.CustomerId,
                        .CustomerName = If(so.Customer IsNot Nothing, so.Customer.Name, ""),
                        .OrderDate = so.OrderDate,
                        .Status = so.Status
                    }

                    For Each l In so.Lines
                        soVm.Lines.Add(New SalesOrderLineViewModel() With {
                            .LineId = l.SOLineId,
                            .ProductId = l.ProductId,
                            .SKU = If(l.Product IsNot Nothing, l.Product.SKU, ""),
                            .ProductName = If(l.Product IsNot Nothing, l.Product.Name, ""),
                            .OrderedQty = l.OrderedQty,
                            .UnitPrice = l.UnitPrice,
                            .MovingAvgCost = If(l.Product IsNot Nothing, l.Product.CurrentUnitCost, 0D),
                            .AvailableAtOrder = If(l.Product IsNot Nothing, l.Product.TotalQtyOnHand, 0)
                        })
                    Next
                    SalesOrders.Add(soVm)
                Next

                ' 4. Sales Invoices
                SalesInvoices.Clear()
                Dim invEntities = _context.SalesInvoices _
                    .Include(Function(i) i.Customer) _
                    .Include(Function(i) i.SalesOrder) _
                    .Include(Function(i) i.Lines).ThenInclude(Function(l) l.Product) _
                    .OrderByDescending(Function(i) i.PostingDate) _
                    .ToList()

                For Each inv In invEntities
                    Dim invVm As New SalesInvoiceViewModel() With {
                        .InvoiceId = inv.InvoiceId,
                        .DocumentNumber = inv.DocumentNumber,
                        .SOId = If(inv.SOId.HasValue, inv.SOId.Value, 0),
                        .SODocumentNumber = If(inv.SalesOrder IsNot Nothing, inv.SalesOrder.DocumentNumber, ""),
                        .CustomerId = inv.CustomerId,
                        .CustomerName = If(inv.Customer IsNot Nothing, inv.Customer.Name, ""),
                        .InvoiceDate = inv.InvoiceDate,
                        .PostingDate = inv.PostingDate,
                        .DueDate = inv.DueDate,
                        .Status = inv.Status
                    }

                    For Each l In inv.Lines
                        invVm.Lines.Add(New SalesInvoiceLineViewModel() With {
                            .LineId = l.InvoiceLineId,
                            .ProductId = l.ProductId,
                            .SKU = If(l.Product IsNot Nothing, l.Product.SKU, ""),
                            .ProductName = If(l.Product IsNot Nothing, l.Product.Name, ""),
                            .FulfilledQty = l.FulfilledQty,
                            .UnitPrice = l.UnitPrice,
                            .COGSUnitCost = l.InvoicedCOGSUnitCost
                        })
                    Next
                    SalesInvoices.Add(invVm)
                Next

            Catch ex As Exception
                ' Gracefully handle if DB is uninitialized
            End Try
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
                result.BadgeColor = Color.FromArgb(220, 53, 69)
                result.WarningMessage = $"CRITICAL HARD-STOP: Requested {requestedQty} units, but physical on-hand stock is only {product.QtyOnHand} units. Deficit: {result.ShortfallQty} units. Negative inventory is strictly prohibited by ERP accounting controls."
            ElseIf requestedQty > product.QtyAvailable Then
                result.IsPermitted = False
                result.ShortfallQty = requestedQty - product.QtyAvailable
                result.BadgeText = "STOCK RESERVED BY OTHER ORDERS"
                result.BadgeColor = Color.FromArgb(253, 126, 20)
                result.WarningMessage = $"Reservation conflict: {product.QtyReserved} units are allocated to other confirmed orders. Only {product.QtyAvailable} units currently free to promise."
            ElseIf (product.QtyAvailable - requestedQty) <= product.ReorderLevel Then
                result.IsPermitted = True
                result.BadgeText = "WARNING: REORDER THRESHOLD"
                result.BadgeColor = Color.FromArgb(255, 193, 7)
                result.WarningMessage = $"Order acceptable, but remaining balance ({product.QtyAvailable - requestedQty} units) will drop below reorder threshold ({product.ReorderLevel} units)."
            Else
                result.IsPermitted = True
                result.BadgeText = "IN STOCK: READY"
                result.BadgeColor = Color.FromArgb(40, 167, 69)
                result.WarningMessage = $"Sufficient stock verified. Available: {product.QtyAvailable} units. Order quantity: {requestedQty} units."
            End If

            Return result
        End Function

        ''' <summary>
        ''' Registers a new sales order commitment.
        ''' </summary>
        Public Function RegisterSalesOrder(so As SalesOrderViewModel) As SalesOrderViewModel
            If so Is Nothing Then Throw New ArgumentNullException(NameOf(so))

            Dim lines As New List(Of SOLine)()
            For Each l In so.Lines
                lines.Add(New SOLine(l.ProductId, l.OrderedQty, l.UnitPrice))
            Next

            Dim createdSO = _o2cService.CreateSalesOrder(so.CustomerId, lines, so.OrderDate, so.DocumentNumber)
            LoadData()

            Dim result = SalesOrders.FirstOrDefault(Function(s) s.SOId = createdSO.SOId)
            Return If(result, so)
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

            ' Hard-stop stock pre-check
            For Each line As SalesOrderLineViewModel In so.Lines
                Dim prod As ProductStockViewModel = Products.FirstOrDefault(Function(p) p.ProductId = line.ProductId)
                If prod IsNot Nothing AndAlso line.OrderedQty > prod.QtyOnHand Then
                    outcome.Success = False
                    outcome.ErrorMessage = $"INSUFFICIENT STOCK HARD-STOP: Product '{prod.SKU}' ({prod.Name}) has only {prod.QtyOnHand} units on hand, but order requires {line.OrderedQty} units. Disallow negative inventory."
                    Return outcome
                End If
            Next

            Try
                Dim inv = _o2cService.FulfillSalesInvoice(soId, postingDate)
                LoadData()

                Dim invVm = SalesInvoices.FirstOrDefault(Function(i) i.InvoiceId = inv.InvoiceId)
                If invVm Is Nothing Then
                    invVm = New SalesInvoiceViewModel() With {
                        .InvoiceId = inv.InvoiceId,
                        .DocumentNumber = inv.DocumentNumber,
                        .SOId = so.SOId,
                        .SODocumentNumber = so.DocumentNumber,
                        .CustomerId = so.CustomerId,
                        .CustomerName = so.CustomerName,
                        .InvoiceDate = inv.InvoiceDate,
                        .PostingDate = inv.PostingDate,
                        .DueDate = inv.DueDate,
                        .Status = inv.Status
                    }
                End If

                ' Construct balanced double-entry preview
                outcome.JournalLines.Add(New JournalEntryPreviewViewModel() With {
                    .AccountCode = "1200",
                    .AccountName = "Accounts Receivable",
                    .DebitAmount = inv.TotalAmount,
                    .CreditAmount = 0D,
                    .Explanation = $"Customer invoice billing for {invVm.CustomerName} ({inv.DocumentNumber})"
                })
                outcome.JournalLines.Add(New JournalEntryPreviewViewModel() With {
                    .AccountCode = "4000",
                    .AccountName = "Sales Revenue",
                    .DebitAmount = 0D,
                    .CreditAmount = inv.TotalAmount,
                    .Explanation = $"Revenue recognized from merchandise sales fulfillment ({so.DocumentNumber})"
                })
                outcome.JournalLines.Add(New JournalEntryPreviewViewModel() With {
                    .AccountCode = "5000",
                    .AccountName = "Cost of Goods Sold (COGS)",
                    .DebitAmount = invVm.TotalCOGS,
                    .CreditAmount = 0D,
                    .Explanation = $"Moving average inventory cost recognized upon stock dispatch ({inv.DocumentNumber})"
                })
                outcome.JournalLines.Add(New JournalEntryPreviewViewModel() With {
                    .AccountCode = "1300",
                    .AccountName = "Merchandise Inventory",
                    .DebitAmount = 0D,
                    .CreditAmount = invVm.TotalCOGS,
                    .Explanation = $"Relief of perpetual warehouse inventory asset balance ({so.DocumentNumber})"
                })

                outcome.Success = True
                outcome.Invoice = invVm
                Return outcome

            Catch ex As InsufficientStockException
                outcome.Success = False
                outcome.ErrorMessage = $"INSUFFICIENT STOCK HARD-STOP: {ex.Message}"
                Return outcome
            Catch ex As Exception
                outcome.Success = False
                outcome.ErrorMessage = ex.Message
                Return outcome
            End Try
        End Function

    End Class

End Namespace
