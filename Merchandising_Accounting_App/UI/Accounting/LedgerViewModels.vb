Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Collections.Generic
Imports System.Linq
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Data.Repositories
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models
Imports Merchandising_Accounting_App.Services

Namespace UI.Accounting

    Public Class AccountItemViewModel
        Public Property AccountId As Integer
        Public Property Code As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property AccountType As String = String.Empty
        Public Property NormalBalance As String = String.Empty
        Public Property Balance As Decimal
        Public Property IsActive As Boolean = True

        Public ReadOnly Property DisplayCodeAndName As String
            Get
                Return $"{Code} - {Name}"
            End Get
        End Property

        Public ReadOnly Property FormattedBalance As String
            Get
                Return Balance.ToString("C2")
            End Get
        End Property
    End Class

    Public Class JournalEntryLineViewModel
        Public Property LineId As Integer
        Public Property EntryId As Integer
        Public Property AccountId As Integer
        Public Property AccountCode As String = String.Empty
        Public Property AccountName As String = String.Empty
        Public Property DebitAmount As Decimal
        Public Property CreditAmount As Decimal
        Public Property LineMemo As String = String.Empty

        Public ReadOnly Property FormattedDebit As String
            Get
                If DebitAmount > 0D Then
                    Return DebitAmount.ToString("C2")
                Else
                    Return "-"
                End If
            End Get
        End Property

        Public ReadOnly Property FormattedCredit As String
            Get
                If CreditAmount > 0D Then
                    Return CreditAmount.ToString("C2")
                Else
                    Return "-"
                End If
            End Get
        End Property
    End Class

    Public Class JournalEntryViewModel
        Public Property EntryId As Integer
        Public Property PostingDate As DateTime
        Public Property OperationalEventDate As DateTime
        Public Property SourceDocType As String = String.Empty
        Public Property SourceDocNumber As String = String.Empty
        Public Property PeriodLabel As String = String.Empty
        Public Property Description As String = String.Empty
        Public Property CreatedBy As String = String.Empty
        Public Property TotalDebits As Decimal
        Public Property TotalCredits As Decimal
        Public Property Lines As New List(Of JournalEntryLineViewModel)()

        Public ReadOnly Property IsBalanced As Boolean
            Get
                Return Math.Round(TotalDebits, 4) = Math.Round(TotalCredits, 4)
            End Get
        End Property

        Public ReadOnly Property FormattedPostingDate As String
            Get
                Return PostingDate.ToString("yyyy-MM-dd")
            End Get
        End Property

        Public ReadOnly Property FormattedEventDate As String
            Get
                Return OperationalEventDate.ToString("yyyy-MM-dd")
            End Get
        End Property

        Public ReadOnly Property BalanceStatusText As String
            Get
                If IsBalanced Then
                    Return "Balanced (Debits = Credits)"
                Else
                    Dim diff As Decimal = Math.Abs(TotalDebits - TotalCredits)
                    Return $"UNBALANCED (Variance: {diff:C2})"
                End If
            End Get
        End Property
    End Class

    Public Class FiscalPeriodViewModel
        Public Property PeriodId As Integer
        Public Property FiscalYear As Integer
        Public Property PeriodNumber As Integer
        Public Property StartDate As DateTime
        Public Property EndDate As DateTime
        Public Property IsClosed As Boolean
        Public Property ClosedAtUtc As DateTime?

        Public ReadOnly Property PeriodLabel As String
            Get
                Return $"FY{FiscalYear}-P{PeriodNumber:D2}"
            End Get
        End Property

        Public ReadOnly Property StatusText As String
            Get
                If IsClosed Then
                    Return "CLOSED (Permanently Immutable)"
                Else
                    Return "OPEN (Statutory Postings Active)"
                End If
            End Get
        End Property

        Public ReadOnly Property DateRangeText As String
            Get
                Return $"{StartDate:yyyy-MM-dd} to {EndDate:yyyy-MM-dd}"
            End Get
        End Property
    End Class

    Public Class LedgerDashboardViewModel
        Implements IDisposable

        Private ReadOnly _ledgerRepo As LedgerRepository
        Private ReadOnly _periodService As PeriodService
        Private ReadOnly _context As ErpDbContext
        Private _disposed As Boolean = False

        Public Property Accounts As New List(Of AccountItemViewModel)()
        Public Property JournalEntries As New List(Of JournalEntryViewModel)()
        Public Property CurrentPeriod As FiscalPeriodViewModel

        Public Sub New(Optional ledgerRepo As LedgerRepository = Nothing, Optional periodService As PeriodService = Nothing, Optional context As ErpDbContext = Nothing)
            _context = If(context, New ErpDbContext())
            _ledgerRepo = If(ledgerRepo, New LedgerRepository(_context))
            _periodService = If(periodService, New PeriodService(_context))
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
            ' 1. Load Fiscal Period
            Try
                Dim activePeriod = _periodService.GetActiveFiscalPeriod()
                If activePeriod IsNot Nothing Then
                    CurrentPeriod = New FiscalPeriodViewModel() With {
                        .PeriodId = activePeriod.PeriodId,
                        .FiscalYear = activePeriod.FiscalYear,
                        .PeriodNumber = activePeriod.PeriodNumber,
                        .StartDate = activePeriod.StartDate,
                        .EndDate = activePeriod.EndDate,
                        .IsClosed = activePeriod.IsClosed,
                        .ClosedAtUtc = activePeriod.ClosedAtUtc
                    }
                Else
                    CurrentPeriod = Nothing
                End If
            Catch ex As Exception
                CurrentPeriod = Nothing
            End Try

            ' 2. Load Chart of Accounts & Balances from real general ledger entries
            Accounts.Clear()
            Try
                Dim accts = _context.Accounts.OrderBy(Function(a) a.Code).ToList()
                Dim journalLines = _context.JournalEntryLines.ToList()

                For Each acct In accts
                    Dim debits As Decimal = journalLines.Where(Function(l) l.AccountId = acct.AccountId).Sum(Function(l) l.DebitAmount)
                    Dim credits As Decimal = journalLines.Where(Function(l) l.AccountId = acct.AccountId).Sum(Function(l) l.CreditAmount)

                    Dim normalBal As String
                    Dim bal As Decimal
                    If acct.AccountType = AccountType.Asset OrElse acct.AccountType = AccountType.Expense OrElse acct.Code = "4100" Then
                        normalBal = "Debit"
                        bal = debits - credits
                    Else
                        normalBal = "Credit"
                        bal = credits - debits
                    End If

                    Dim typeDisplay As String = acct.AccountType.ToString()
                    If acct.Code = "4100" Then typeDisplay = "Revenue (Contra)"

                    Accounts.Add(New AccountItemViewModel() With {
                        .AccountId = acct.AccountId,
                        .Code = acct.Code,
                        .Name = acct.Name,
                        .AccountType = typeDisplay,
                        .NormalBalance = normalBal,
                        .Balance = bal,
                        .IsActive = acct.IsActive
                    })
                Next
            Catch ex As Exception
                ' Keep empty if database not initialized
            End Try

            ' 3. Load Journal Entries
            JournalEntries.Clear()
            Try
                Dim entries = _ledgerRepo.GetAll()
                For Each entry In entries
                    Dim totalDebits As Decimal = entry.Lines.Sum(Function(l) l.DebitAmount)
                    Dim totalCredits As Decimal = entry.Lines.Sum(Function(l) l.CreditAmount)

                    Dim jeVm As New JournalEntryViewModel() With {
                        .EntryId = entry.EntryId,
                        .PostingDate = entry.PostingDate,
                        .OperationalEventDate = entry.OperationalEventDate,
                        .SourceDocType = entry.SourceDocType,
                        .SourceDocNumber = $"{entry.SourceDocType} #{entry.SourceDocId}",
                        .PeriodLabel = If(entry.Period IsNot Nothing, $"FY{entry.Period.FiscalYear}-P{entry.Period.PeriodNumber:D2}", If(CurrentPeriod IsNot Nothing, CurrentPeriod.PeriodLabel, "")),
                        .Description = entry.Description,
                        .CreatedBy = entry.CreatedBy,
                        .TotalDebits = totalDebits,
                        .TotalCredits = totalCredits
                    }

                    For Each line In entry.Lines
                        jeVm.Lines.Add(New JournalEntryLineViewModel() With {
                            .LineId = line.LineId,
                            .EntryId = line.EntryId,
                            .AccountId = line.AccountId,
                            .AccountCode = If(line.Account IsNot Nothing, line.Account.Code, ""),
                            .AccountName = If(line.Account IsNot Nothing, line.Account.Name, ""),
                            .DebitAmount = line.DebitAmount,
                            .CreditAmount = line.CreditAmount,
                            .LineMemo = line.LineMemo
                        })
                    Next

                    JournalEntries.Add(jeVm)
                Next
            Catch ex As Exception
                ' Keep empty if database not initialized
            End Try
        End Sub
    End Class

End Namespace
