Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports Merchandising_Accounting_App.Domain

Namespace Domain.Models

    ''' <summary>
    ''' Represents a Master Chart of Accounts (COA) entry.
    ''' </summary>
    Public Class Account
        Public Property AccountId As Integer
        Public Property Code As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property AccountType As AccountType
        Public Property IsActive As Boolean = True

        ' Navigation Properties
        Public Overridable Property JournalEntryLines As ICollection(Of JournalEntryLine) = New List(Of JournalEntryLine)()

        Public Sub New()
        End Sub

        Public Sub New(code As String, name As String, accountType As AccountType, Optional isActive As Boolean = True)
            Me.Code = code
            Me.Name = name
            Me.AccountType = accountType
            Me.IsActive = isActive
        End Sub
    End Class

    ''' <summary>
    ''' Represents a monthly statutory fiscal accounting period.
    ''' </summary>
    Public Class AccountingPeriod
        Public Property PeriodId As Integer
        Public Property FiscalYear As Integer
        Public Property PeriodNumber As Integer
        Public Property StartDate As DateTime
        Public Property EndDate As DateTime
        Public Property IsClosed As Boolean
        Public Property ClosedAtUtc As DateTime?

        ' Navigation Properties
        Public Overridable Property JournalEntries As ICollection(Of JournalEntry) = New List(Of JournalEntry)()

        Public Sub New()
        End Sub

        Public Sub New(fiscalYear As Integer, periodNumber As Integer, startDate As DateTime, endDate As DateTime)
            Me.FiscalYear = fiscalYear
            Me.PeriodNumber = periodNumber
            Me.StartDate = startDate
            Me.EndDate = endDate
            Me.IsClosed = False
            Me.ClosedAtUtc = Nothing
        End Sub
    End Class

    ''' <summary>
    ''' Immutable double-entry financial journal entry header.
    ''' </summary>
    Public Class JournalEntry
        Public Property EntryId As Integer
        Public Property SourceDocType As String = String.Empty
        Public Property SourceDocId As Integer
        Public Property PostingDate As DateTime
        Public Property OperationalEventDate As DateTime
        Public Property PeriodId As Integer
        Public Property Description As String = String.Empty
        Public Property CreatedBy As String = String.Empty

        ' Navigation Properties
        Public Overridable Property Period As AccountingPeriod
        Public Overridable Property Lines As ICollection(Of JournalEntryLine) = New List(Of JournalEntryLine)()

        Public Sub New()
        End Sub
    End Class

    ''' <summary>
    ''' Financial debit or credit distribution line belonging to a journal entry.
    ''' </summary>
    Public Class JournalEntryLine
        Public Property LineId As Integer
        Public Property EntryId As Integer
        Public Property AccountId As Integer
        Public Property DebitAmount As Decimal
        Public Property CreditAmount As Decimal
        Public Property LineMemo As String = String.Empty

        ' Navigation Properties
        Public Overridable Property JournalEntry As JournalEntry
        Public Overridable Property Account As Account

        Public Sub New()
        End Sub

        Public Sub New(accountId As Integer, debit As Decimal, credit As Decimal, Optional memo As String = "")
            Me.AccountId = accountId
            Me.DebitAmount = debit
            Me.CreditAmount = credit
            Me.LineMemo = If(memo, String.Empty)
        End Sub
    End Class

End Namespace
