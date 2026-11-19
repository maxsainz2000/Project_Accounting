Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports Merchandising_Accounting_App.Domain

Namespace Domain.Models

    ''' <summary>
    ''' Customer return document recording physical quarantine intake and inspection disposition.
    ''' </summary>
    Public Class CustomerReturn
        Public Property ReturnId As Integer
        Public Property CustomerId As Integer
        Public Property OriginalInvoiceId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property ReturnDate As DateTime
        Public Property PostingDate As DateTime
        Public Property Status As String = String.Empty

        ' Navigation Properties
        Public Overridable Property Customer As Customer
        Public Overridable Property OriginalInvoice As SalesInvoice
        Public Overridable Property Lines As ICollection(Of CustomerReturnLine) = New List(Of CustomerReturnLine)()

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Item line detail of returned goods recording restock vs. scrap inspection disposition.
    ''' </summary>
    Public Class CustomerReturnLine
        Public Property ReturnLineId As Integer
        Public Property ReturnId As Integer
        Public Property ProductId As Integer
        Public Property ReturnedQty As Integer
        Public Property RestockQty As Integer
        Public Property ScrapQty As Integer
        Public Property OriginalUnitCost As Decimal
        Public Property SellingUnitPrice As Decimal
        Public Property DispositionStatus As String = String.Empty

        ' Navigation Properties
        Public Overridable Property CustomerReturn As CustomerReturn
        Public Overridable Property Product As Product

        Public Sub New()
        End Sub

        Public Sub New(productId As Integer, returnedQty As Integer, restockQty As Integer, scrapQty As Integer, originalUnitCost As Decimal, sellingUnitPrice As Decimal)
            Me.ProductId = productId
            Me.ReturnedQty = returnedQty
            Me.RestockQty = restockQty
            Me.ScrapQty = scrapQty
            Me.OriginalUnitCost = originalUnitCost
            Me.SellingUnitPrice = sellingUnitPrice
            Me.DispositionStatus = If(scrapQty > 0 AndAlso restockQty = 0, ReturnDisposition.Scrap.ToString(), ReturnDisposition.Restock.ToString())
        End Sub
    End Class

End Namespace
