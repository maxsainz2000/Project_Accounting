Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports Merchandising_Accounting_App.Domain

Namespace Domain.Models

    ''' <summary>
    ''' Represents a commercial customer profile, credit ceiling, and accounts receivable balance.
    ''' </summary>
    Public Class Customer
        Public Property CustomerId As Integer
        Public Property Code As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property CreditLimit As Decimal
        Public Property CurrentARBalance As Decimal

        ' Navigation Properties
        Public Overridable Property SalesOrders As ICollection(Of SalesOrder) = New List(Of SalesOrder)()
        Public Overridable Property SalesInvoices As ICollection(Of SalesInvoice) = New List(Of SalesInvoice)()
        Public Overridable Property PaymentReceipts As ICollection(Of PaymentReceipt) = New List(Of PaymentReceipt)()
        Public Overridable Property CustomerReturns As ICollection(Of CustomerReturn) = New List(Of CustomerReturn)()

        Public Sub New()
        End Sub

        Public Sub New(code As String, name As String, creditLimit As Decimal, Optional arBalance As Decimal = 0D)
            Me.Code = code
            Me.Name = name
            Me.CreditLimit = creditLimit
            Me.CurrentARBalance = arBalance
        End Sub
    End Class

    ''' <summary>
    ''' Commercial customer order commitment.
    ''' </summary>
    Public Class SalesOrder
        Public Property SOId As Integer
        Public Property CustomerId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property OrderDate As DateTime
        Public Property Status As String = String.Empty

        ' Navigation Properties
        Public Overridable Property Customer As Customer
        Public Overridable Property Lines As ICollection(Of SOLine) = New List(Of SOLine)()
        Public Overridable Property SalesInvoices As ICollection(Of SalesInvoice) = New List(Of SalesInvoice)()

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Line item on a sales order.
    ''' </summary>
    Public Class SOLine
        Public Property SOLineId As Integer
        Public Property SOId As Integer
        Public Property ProductId As Integer
        Public Property OrderedQty As Integer
        Public Property UnitPrice As Decimal

        ' Navigation Properties
        Public Overridable Property SalesOrder As SalesOrder
        Public Overridable Property Product As Product
        Public Overridable Property InvoiceLines As ICollection(Of SalesInvoiceLine) = New List(Of SalesInvoiceLine)()

        Public Sub New()
        End Sub

        Public Sub New(productId As Integer, orderedQty As Integer, unitPrice As Decimal)
            Me.ProductId = productId
            Me.OrderedQty = orderedQty
            Me.UnitPrice = unitPrice
        End Sub
    End Class

    ''' <summary>
    ''' Combined sales billing and stock dispatch fulfillment document.
    ''' </summary>
    Public Class SalesInvoice
        Public Property InvoiceId As Integer
        Public Property SOId As Integer?
        Public Property CustomerId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property InvoiceDate As DateTime
        Public Property PostingDate As DateTime
        Public Property DueDate As DateTime
        Public Property TotalAmount As Decimal
        Public Property Status As String = String.Empty

        ' Navigation Properties
        Public Overridable Property Customer As Customer
        Public Overridable Property SalesOrder As SalesOrder
        Public Overridable Property Lines As ICollection(Of SalesInvoiceLine) = New List(Of SalesInvoiceLine)()
        Public Overridable Property PaymentReceipts As ICollection(Of PaymentReceipt) = New List(Of PaymentReceipt)()
        Public Overridable Property CustomerReturns As ICollection(Of CustomerReturn) = New List(Of CustomerReturn)()

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Line fulfillment on a sales invoice tracking selling price and moving average COGS unit cost.
    ''' </summary>
    Public Class SalesInvoiceLine
        Public Property InvoiceLineId As Integer
        Public Property InvoiceId As Integer
        Public Property SOLineId As Integer?
        Public Property ProductId As Integer
        Public Property FulfilledQty As Integer
        Public Property UnitPrice As Decimal
        Public Property InvoicedCOGSUnitCost As Decimal

        ' Navigation Properties
        Public Overridable Property SalesInvoice As SalesInvoice
        Public Overridable Property SOLine As SOLine
        Public Overridable Property Product As Product

        Public Sub New()
        End Sub

        Public Sub New(productId As Integer, fulfilledQty As Integer, unitPrice As Decimal, cogsUnitCost As Decimal)
            Me.ProductId = productId
            Me.FulfilledQty = fulfilledQty
            Me.UnitPrice = unitPrice
            Me.InvoicedCOGSUnitCost = cogsUnitCost
        End Sub
    End Class

    ''' <summary>
    ''' Customer payment receipt clearing accounts receivable.
    ''' </summary>
    Public Class PaymentReceipt
        Public Property ReceiptId As Integer
        Public Property CustomerId As Integer
        Public Property InvoiceId As Integer?
        Public Property ReceiptDate As DateTime
        Public Property PostingDate As DateTime
        Public Property AmountReceived As Decimal
        Public Property BankAccountId As Integer

        ' Navigation Properties
        Public Overridable Property Customer As Customer
        Public Overridable Property SalesInvoice As SalesInvoice
        Public Overridable Property BankAccount As Account

        Public Sub New()
        End Sub

        Public Sub New(customerId As Integer, amountReceived As Decimal, bankAccountId As Integer, Optional invoiceId As Integer? = Nothing)
            Me.CustomerId = customerId
            Me.AmountReceived = amountReceived
            Me.BankAccountId = bankAccountId
            Me.InvoiceId = invoiceId
            Me.ReceiptDate = DateTime.Now
            Me.PostingDate = DateTime.Now
        End Sub
    End Class

End Namespace
