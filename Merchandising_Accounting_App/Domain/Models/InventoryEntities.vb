Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports Merchandising_Accounting_App.Domain

Namespace Domain.Models

    ''' <summary>
    ''' Represents a merchandised inventory item with stock counts, unit costs, and packaging multipliers.
    ''' </summary>
    Public Class Product
        Public Property ProductId As Integer
        Public Property SKU As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property BaseUnitName As String = String.Empty
        Public Property PackUnitName As String
        Public Property PackMultiplier As Integer = 1
        Public Property CurrentUnitCost As Decimal
        Public Property TotalQtyOnHand As Integer
        Public Property AllocatedQty As Integer = 0
        Public Property QuarantineQty As Integer = 0
        Public Property IsActive As Boolean = True

        ' Navigation Properties
        Public Overridable Property InventoryTransactions As ICollection(Of InventoryTransaction) = New List(Of InventoryTransaction)()
        Public Overridable Property AdjustmentLines As ICollection(Of InventoryAdjustmentLine) = New List(Of InventoryAdjustmentLine)()

        Public Sub New()
        End Sub

        Public Sub New(sku As String, name As String, baseUnitName As String, currentUnitCost As Decimal, Optional totalQtyOnHand As Integer = 0)
            Me.SKU = sku
            Me.Name = name
            Me.BaseUnitName = baseUnitName
            Me.CurrentUnitCost = currentUnitCost
            Me.TotalQtyOnHand = totalQtyOnHand
            Me.PackMultiplier = 1
            Me.IsActive = True
        End Sub
    End Class

    ''' <summary>
    ''' Append-only transaction audit entry recording physical stock and cost movements.
    ''' </summary>
    Public Class InventoryTransaction
        Public Property TransactionId As Integer
        Public Property ProductId As Integer
        Public Property SourceDocType As String = String.Empty
        Public Property SourceDocId As Integer
        Public Property OperationalEventDate As DateTime
        Public Property PostingDate As DateTime
        Public Property QtyChange As Integer
        Public Property UnitCost As Decimal
        Public Property RunningQty As Integer
        Public Property RunningUnitCost As Decimal

        ' Navigation Properties
        Public Overridable Property Product As Product

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Header document for physical stock adjustments, cycle counts, write-downs, and shrinkage.
    ''' </summary>
    Public Class InventoryAdjustment
        Public Property AdjustmentId As Integer
        Public Property DocumentNumber As String = String.Empty
        Public Property AdjustmentDate As DateTime
        Public Property PostingDate As DateTime
        Public Property ReasonCode As String = String.Empty
        Public Property Status As String = String.Empty

        ' Navigation Properties
        Public Overridable Property Lines As ICollection(Of InventoryAdjustmentLine) = New List(Of InventoryAdjustmentLine)()

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Item line detail for physical stock adjustment.
    ''' </summary>
    Public Class InventoryAdjustmentLine
        Public Property AdjustmentLineId As Integer
        Public Property AdjustmentId As Integer
        Public Property ProductId As Integer
        Public Property QtyChange As Integer
        Public Property UnitCost As Decimal
        Public Property TargetAccountId As Integer
        Public Property Notes As String = String.Empty

        ' Navigation Properties
        Public Overridable Property Adjustment As InventoryAdjustment
        Public Overridable Property Product As Product
        Public Overridable Property TargetAccount As Account

        Public Sub New()
        End Sub
    End Class

End Namespace
