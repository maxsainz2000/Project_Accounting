Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models

Namespace Services

    ''' <summary>
    ''' Statutory fiscal period guard and administrative period service.
    ''' Enforces immutability of closed accounting periods, validates statutory posting dates,
    ''' resolves posting dates for operational events occurring in closed periods, and manages period closing.
    ''' </summary>
    Public Class PeriodService

        Private ReadOnly _context As ErpDbContext

        ''' <summary>
        ''' Initializes a new instance of the <see cref="PeriodService"/> class with the specified database context.
        ''' </summary>
        Public Sub New(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            _context = context
        End Sub

        ''' <summary>
        ''' Initializes a new instance of the <see cref="PeriodService"/> class using the default database context.
        ''' </summary>
        Public Sub New()
            Me.New(New ErpDbContext())
        End Sub

        ''' <summary>
        ''' Validates that the specified statutory posting date maps to an active, open accounting period.
        ''' Throws a <see cref="ClosedPeriodPostingException"/> if the period is closed, sealed, or does not exist.
        ''' </summary>
        ''' <param name="postingDate">The statutory posting date to validate.</param>
        ''' <returns>The active <see cref="AccountingPeriod"/> covering the posting date.</returns>
        Public Function ValidatePostingDate(postingDate As DateTime) As AccountingPeriod
            Dim period = _context.AccountingPeriods _
                .FirstOrDefault(Function(p) postingDate.Date >= p.StartDate.Date AndAlso postingDate.Date <= p.EndDate.Date)

            If period Is Nothing Then
                Throw New ClosedPeriodPostingException(postingDate, $"No fiscal accounting period found covering statutory posting date {postingDate:yyyy-MM-dd}.")
            End If

            If period.IsClosed Then
                Throw New ClosedPeriodPostingException(postingDate, period.PeriodId, $"Fiscal period FY{period.FiscalYear}-P{period.PeriodNumber:D2} is permanently closed and sealed. Statutory posting date {postingDate:yyyy-MM-dd} is rejected.")
            End If

            Return period
        End Function

        ''' <summary>
        ''' Retrieves the current active, open statutory fiscal accounting period.
        ''' Returns the earliest open period ordered by FiscalYear and PeriodNumber.
        ''' Throws an <see cref="InvalidOperationException"/> if no open period exists.
        ''' </summary>
        Public Function GetActiveFiscalPeriod() As AccountingPeriod
            Dim activePeriod = _context.AccountingPeriods _
                .Where(Function(p) Not p.IsClosed) _
                .OrderBy(Function(p) p.FiscalYear) _
                .ThenBy(Function(p) p.PeriodNumber) _
                .FirstOrDefault()

            If activePeriod Is Nothing Then
                Throw New InvalidOperationException("No active open fiscal accounting period is currently configured in the general ledger.")
            End If

            Return activePeriod
        End Function

        ''' <summary>
        ''' Permanently seals and closes an accounting period, stamping it with UTC closure metadata
        ''' and preventing any subsequent operational or adjusting entries from being posted.
        ''' </summary>
        ''' <param name="periodId">The primary key identifier of the accounting period to seal.</param>
        Public Sub CloseFiscalPeriod(periodId As Integer)
            Dim period = _context.AccountingPeriods.FirstOrDefault(Function(p) p.PeriodId = periodId)
            If period Is Nothing Then
                Throw New InvalidOperationException($"Fiscal accounting period with ID {periodId} was not found.")
            End If

            If period.IsClosed Then
                ' Period is already sealed; operation is idempotent
                Return
            End If

            period.IsClosed = True
            period.ClosedAtUtc = DateTime.UtcNow
            _context.SaveChanges()
        End Sub

        ''' <summary>
        ''' Resolves the statutory posting date for an operational event according to statutory closed-period invariants.
        ''' If the event physically occurred during an active open period, the posting date matches the event date.
        ''' If the event physically occurred during a closed/sealed period, the statutory posting date is automatically
        ''' mapped to the first open day (StartDate) of the current active open fiscal period.
        ''' </summary>
        ''' <param name="eventDate">The physical operational event timestamp.</param>
        ''' <returns>The statutory posting date.</returns>
        Public Function ResolveStatutoryPostingDate(eventDate As DateTime) As DateTime
            Dim period = _context.AccountingPeriods _
                .FirstOrDefault(Function(p) eventDate.Date >= p.StartDate.Date AndAlso eventDate.Date <= p.EndDate.Date)

            If period IsNot Nothing AndAlso Not period.IsClosed Then
                ' Operational event occurred within an open period: statutory posting date equals operational event date
                Return eventDate
            End If

            ' Operational event occurred in a closed period (or outside configured periods):
            ' Map to the first open day of the current active period
            Dim activePeriod = GetActiveFiscalPeriod()
            Return activePeriod.StartDate.Date
        End Function

        ''' <summary>
        ''' Retrieves all configured accounting periods ordered chronologically.
        ''' </summary>
        Public Function GetAllPeriods() As List(Of AccountingPeriod)
            Return _context.AccountingPeriods _
                .OrderBy(Function(p) p.FiscalYear) _
                .ThenBy(Function(p) p.PeriodNumber) _
                .ToList()
        End Function

        ''' <summary>
        ''' Retrieves a fiscal accounting period by its unique identifier.
        ''' </summary>
        Public Function GetPeriodById(periodId As Integer) As AccountingPeriod
            Return _context.AccountingPeriods.FirstOrDefault(Function(p) p.PeriodId = periodId)
        End Function

        ''' <summary>
        ''' Creates and registers a new open fiscal accounting period.
        ''' </summary>
        Public Function CreatePeriod(fiscalYear As Integer, periodNumber As Integer, startDate As DateTime, endDate As DateTime) As AccountingPeriod
            Dim existing = _context.AccountingPeriods.FirstOrDefault(Function(p) p.FiscalYear = fiscalYear AndAlso p.PeriodNumber = periodNumber)
            If existing IsNot Nothing Then
                Throw New InvalidOperationException($"Fiscal period FY{fiscalYear}-P{periodNumber:D2} already exists.")
            End If

            Dim newPeriod As New AccountingPeriod(fiscalYear, periodNumber, startDate, endDate) With {
                .IsClosed = False,
                .ClosedAtUtc = Nothing
            }

            _context.AccountingPeriods.Add(newPeriod)
            _context.SaveChanges()
            Return newPeriod
        End Function

    End Class

End Namespace
