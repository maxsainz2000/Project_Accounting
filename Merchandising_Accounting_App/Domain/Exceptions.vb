Option Strict On
Option Explicit On
Option Compare Binary

Imports System

Namespace Domain

    ''' <summary>
    ''' Raised when an operation attempts to allocate, sell, or dispatch stock beyond the available on-hand physical inventory.
    ''' </summary>
    Public Class InsufficientStockException
        Inherits Exception

        Public Property ProductId As Integer?
        Public Property RequestedQty As Integer?
        Public Property AvailableQty As Integer?

        Public Sub New()
            MyBase.New("Insufficient inventory on hand to fulfill the requested transaction.")
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        Public Sub New(productId As Integer, requestedQty As Integer, availableQty As Integer)
            MyBase.New($"Insufficient stock for Product ID {productId}. Requested: {requestedQty}, Available on hand: {availableQty}.")
            Me.ProductId = productId
            Me.RequestedQty = requestedQty
            Me.AvailableQty = availableQty
        End Sub

        Public Sub New(productId As Integer, requestedQty As Integer, availableQty As Integer, message As String)
            MyBase.New(message)
            Me.ProductId = productId
            Me.RequestedQty = requestedQty
            Me.AvailableQty = availableQty
        End Sub
    End Class

    ''' <summary>
    ''' Raised when a proposed journal entry fails the fundamental accounting equation (Sum of Debits <> Sum of Credits to 4 decimal places).
    ''' </summary>
    Public Class UnbalancedJournalEntryException
        Inherits Exception

        Public Property TotalDebits As Decimal?
        Public Property TotalCredits As Decimal?
        Public Property Imbalance As Decimal?

        Public Sub New()
            MyBase.New("General Ledger journal entry is unbalanced; debits must equal credits.")
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        Public Sub New(totalDebits As Decimal, totalCredits As Decimal)
            MyBase.New($"Journal entry debit/credit imbalance. Total Debits: {totalDebits:F4}, Total Credits: {totalCredits:F4}, Difference: {Math.Abs(totalDebits - totalCredits):F4}.")
            Me.TotalDebits = totalDebits
            Me.TotalCredits = totalCredits
            Me.Imbalance = Math.Abs(totalDebits - totalCredits)
        End Sub

        Public Sub New(totalDebits As Decimal, totalCredits As Decimal, message As String)
            MyBase.New(message)
            Me.TotalDebits = totalDebits
            Me.TotalCredits = totalCredits
            Me.Imbalance = Math.Abs(totalDebits - totalCredits)
        End Sub
    End Class

    ''' <summary>
    ''' Raised when an operational posting date maps to an accounting period that has already been closed and sealed.
    ''' </summary>
    Public Class ClosedPeriodPostingException
        Inherits Exception

        Public Property PostingDate As DateTime?
        Public Property PeriodId As Integer?
        Public Property FiscalYear As Integer?
        Public Property PeriodNumber As Integer?

        Public Sub New()
            MyBase.New("Posting to a closed or sealed accounting period is strictly prohibited.")
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        Public Sub New(postingDate As DateTime, message As String)
            MyBase.New(message)
            Me.PostingDate = postingDate
        End Sub

        Public Sub New(postingDate As DateTime, periodId As Integer, message As String)
            MyBase.New(message)
            Me.PostingDate = postingDate
            Me.PeriodId = periodId
        End Sub
    End Class

    ''' <summary>
    ''' Raised when a 3-way matching validation fails between Purchase Order, Goods Receipt Note, and Supplier Bill.
    ''' </summary>
    Public Class ThreeWayMatchException
        Inherits Exception

        Public Property POId As Integer?
        Public Property GRNId As Integer?
        Public Property BillId As Integer?
        Public Property BilledQty As Integer?
        Public Property ReceivedQty As Integer?

        Public Sub New()
            MyBase.New("Three-way match validation failed between Purchase Order, Goods Receipt Note, and Supplier Bill.")
        End Sub

        Public Sub New(message As String)
            MyBase.New(message)
        End Sub

        Public Sub New(message As String, innerException As Exception)
            MyBase.New(message, innerException)
        End Sub

        Public Sub New(billedQty As Integer, receivedQty As Integer, message As String)
            MyBase.New(message)
            Me.BilledQty = billedQty
            Me.ReceivedQty = receivedQty
        End Sub
    End Class

End Namespace
