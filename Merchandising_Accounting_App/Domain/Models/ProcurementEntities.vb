Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports Merchandising_Accounting_App.Domain

Namespace Domain.Models

    ''' <summary>
    ''' Represents a merchandise supplier profile and outstanding accounts payable balance.
    ''' </summary>
    Public Class Vendor
        Public Property VendorId As Integer
        Public Property Code As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property PaymentTermsDays As Integer
        Public Property CurrentPayableBalance As Decimal

        ' Navigation Properties
        Public Overridable Property PurchaseOrders As ICollection(Of PurchaseOrder) = New List(Of PurchaseOrder)()
        Public Overridable Property SupplierBills As ICollection(Of SupplierBill) = New List(Of SupplierBill)()
        Public Overridable Property PaymentVouchers As ICollection(Of PaymentVoucher) = New List(Of PaymentVoucher)()

        Public Sub New()
        End Sub

        Public Sub New(code As String, name As String, paymentTermsDays As Integer, Optional payableBalance As Decimal = 0D)
            Me.Code = code
            Me.Name = name
            Me.PaymentTermsDays = paymentTermsDays
            Me.CurrentPayableBalance = payableBalance
        End Sub
    End Class

    ''' <summary>
    ''' Inbound purchase order commitment to a vendor.
    ''' </summary>
    Public Class PurchaseOrder
        Public Property POId As Integer
        Public Property VendorId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property Status As String = String.Empty
        Public Property OrderDate As DateTime
        Public Property TotalAmount As Decimal

        ' Navigation Properties
        Public Overridable Property Vendor As Vendor
        Public Overridable Property Lines As ICollection(Of POLine) = New List(Of POLine)()
        Public Overridable Property GoodsReceiptNotes As ICollection(Of GoodsReceiptNote) = New List(Of GoodsReceiptNote)()

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Line item on a purchase order.
    ''' </summary>
    Public Class POLine
        Public Property POLineId As Integer
        Public Property POId As Integer
        Public Property ProductId As Integer
        Public Property OrderedQty As Integer
        Public Property UnitPrice As Decimal

        ' Navigation Properties
        Public Overridable Property PurchaseOrder As PurchaseOrder
        Public Overridable Property Product As Product
        Public Overridable Property GRNLines As ICollection(Of GRNLine) = New List(Of GRNLine)()

        Public Sub New()
        End Sub

        Public Sub New(productId As Integer, orderedQty As Integer, unitPrice As Decimal)
            Me.ProductId = productId
            Me.OrderedQty = orderedQty
            Me.UnitPrice = unitPrice
        End Sub
    End Class

    ''' <summary>
    ''' Goods Receipt Note (GRN) representing physical receiving dock intake.
    ''' </summary>
    Public Class GoodsReceiptNote
        Public Property GRNId As Integer
        Public Property POId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property ReceiptDate As DateTime
        Public Property PostingDate As DateTime
        Public Property Status As String = String.Empty

        ' Navigation Properties
        Public Overridable Property PurchaseOrder As PurchaseOrder
        Public Overridable Property Lines As ICollection(Of GRNLine) = New List(Of GRNLine)()

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Line item detail for received goods receipt valued at PO purchase price.
    ''' </summary>
    Public Class GRNLine
        Public Property GRNLineId As Integer
        Public Property GRNId As Integer
        Public Property POLineId As Integer
        Public Property ProductId As Integer
        Public Property ReceivedQty As Integer
        Public Property POUnitPrice As Decimal

        ' Navigation Properties
        Public Overridable Property GoodsReceiptNote As GoodsReceiptNote
        Public Overridable Property POLine As POLine
        Public Overridable Property Product As Product
        Public Overridable Property BillLines As ICollection(Of BillLine) = New List(Of BillLine)()
        Public Overridable Property GRNIAdjustments As ICollection(Of GRNIAdjustment) = New List(Of GRNIAdjustment)()

        Public Sub New()
        End Sub

        Public Sub New(poLineId As Integer, productId As Integer, receivedQty As Integer, poUnitPrice As Decimal)
            Me.POLineId = poLineId
            Me.ProductId = productId
            Me.ReceivedQty = receivedQty
            Me.POUnitPrice = poUnitPrice
        End Sub
    End Class

    ''' <summary>
    ''' Invoiced supplier claim against open Goods Receipt Notes.
    ''' </summary>
    Public Class SupplierBill
        Public Property BillId As Integer
        Public Property VendorId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property BillDate As DateTime
        Public Property PostingDate As DateTime
        Public Property DueDate As DateTime
        Public Property TotalAmount As Decimal
        Public Property Status As String = String.Empty

        ' Navigation Properties
        Public Overridable Property Vendor As Vendor
        Public Overridable Property Lines As ICollection(Of BillLine) = New List(Of BillLine)()

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Billed line item matching a GRN line or freight/service charge.
    ''' </summary>
    Public Class BillLine
        Public Property BillLineId As Integer
        Public Property BillId As Integer
        Public Property GRNLineId As Integer?
        Public Property ProductId As Integer?
        Public Property BilledQty As Integer
        Public Property BilledUnitPrice As Decimal
        Public Property LineTotal As Decimal
        Public Property FreightAmount As Decimal
        Public Property AccountId As Integer?

        ' Navigation Properties
        Public Overridable Property SupplierBill As SupplierBill
        Public Overridable Property GRNLine As GRNLine
        Public Overridable Property Product As Product
        Public Overridable Property Account As Account

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Cash disbursement voucher settling accounts payable obligations or customer refunds.
    ''' </summary>
    Public Class PaymentVoucher
        Public Property VoucherId As Integer
        Public Property VoucherType As String = String.Empty
        Public Property VendorId As Integer?
        Public Property CustomerId As Integer?
        Public Property TargetDocId As Integer
        Public Property VoucherNumber As String = String.Empty
        Public Property PaymentDate As DateTime
        Public Property PostingDate As DateTime
        Public Property DisbursedAmount As Decimal
        Public Property BankAccountId As Integer

        ' Navigation Properties
        Public Overridable Property Vendor As Vendor
        Public Overridable Property Customer As Customer
        Public Overridable Property BankAccount As Account

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Represents a write-off or clearing adjustment for stale/unbilled Goods Receipt Note (GRN) accruals.
    ''' </summary>
    Public Class GRNIAdjustment
        Public Property AdjustmentId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property GoodsReceiptLineId As Integer
        Public Property AdjustmentDate As DateTime
        Public Property PostingDate As DateTime
        Public Property ClearedQty As Integer
        Public Property ClearedAmount As Decimal
        Public Property Reason As String = String.Empty
        Public Property Status As String = String.Empty
        Public Property CreatedAt As DateTime = DateTime.UtcNow

        ' Navigation Properties
        Public Overridable Property GoodsReceiptLine As GRNLine

        Public Sub New()
        End Sub

        Public Sub New(goodsReceiptLineId As Integer, clearedQty As Integer, clearedAmount As Decimal, reason As String, documentNumber As String, adjustmentDate As DateTime, postingDate As DateTime, Optional status As String = "Posted")
            Me.GoodsReceiptLineId = goodsReceiptLineId
            Me.ClearedQty = clearedQty
            Me.ClearedAmount = clearedAmount
            Me.Reason = reason
            Me.DocumentNumber = documentNumber
            Me.AdjustmentDate = adjustmentDate
            Me.PostingDate = postingDate
            Me.Status = status
            Me.CreatedAt = DateTime.UtcNow
        End Sub
    End Class

End Namespace
