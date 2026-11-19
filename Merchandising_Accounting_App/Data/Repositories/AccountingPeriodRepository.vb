Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data.Repositories

    ''' <summary>
    ''' Repository for monthly statutory fiscal accounting periods and period locking status.
    ''' </summary>
    Public Class AccountingPeriodRepository

        Private ReadOnly _context As ErpDbContext

        Public Sub New(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            _context = context
        End Sub

        Public Function GetById(periodId As Integer) As AccountingPeriod
            Return _context.AccountingPeriods.Find(periodId)
        End Function

        Public Async Function GetByIdAsync(periodId As Integer) As Task(Of AccountingPeriod)
            Return Await _context.AccountingPeriods.FindAsync(periodId)
        End Function

        ''' <summary>
        ''' Retrieves the earliest or current active (unclosed) accounting period.
        ''' </summary>
        Public Function GetActivePeriod() As AccountingPeriod
            Return _context.AccountingPeriods _
                .Where(Function(p) Not p.IsClosed) _
                .OrderBy(Function(p) p.FiscalYear) _
                .ThenBy(Function(p) p.PeriodNumber) _
                .FirstOrDefault()
        End Function

        Public Async Function GetActivePeriodAsync() As Task(Of AccountingPeriod)
            Return Await _context.AccountingPeriods _
                .Where(Function(p) Not p.IsClosed) _
                .OrderBy(Function(p) p.FiscalYear) _
                .ThenBy(Function(p) p.PeriodNumber) _
                .FirstOrDefaultAsync()
        End Function

        ''' <summary>
        ''' Finds the accounting period covering the specified calendar date.
        ''' </summary>
        Public Function GetPeriodForDate(targetDate As DateTime) As AccountingPeriod
            Return _context.AccountingPeriods _
                .FirstOrDefault(Function(p) p.StartDate <= targetDate AndAlso p.EndDate >= targetDate)
        End Function

        Public Async Function GetPeriodForDateAsync(targetDate As DateTime) As Task(Of AccountingPeriod)
            Return Await _context.AccountingPeriods _
                .FirstOrDefaultAsync(Function(p) p.StartDate <= targetDate AndAlso p.EndDate >= targetDate)
        End Function

        Public Function GetByYearAndPeriod(fiscalYear As Integer, periodNumber As Integer) As AccountingPeriod
            Return _context.AccountingPeriods _
                .FirstOrDefault(Function(p) p.FiscalYear = fiscalYear AndAlso p.PeriodNumber = periodNumber)
        End Function

        Public Async Function GetByYearAndPeriodAsync(fiscalYear As Integer, periodNumber As Integer) As Task(Of AccountingPeriod)
            Return Await _context.AccountingPeriods _
                .FirstOrDefaultAsync(Function(p) p.FiscalYear = fiscalYear AndAlso p.PeriodNumber = periodNumber)
        End Function

        Public Function GetAll() As List(Of AccountingPeriod)
            Return _context.AccountingPeriods _
                .OrderBy(Function(p) p.FiscalYear) _
                .ThenBy(Function(p) p.PeriodNumber) _
                .ToList()
        End Function

        Public Async Function GetAllAsync() As Task(Of List(Of AccountingPeriod))
            Return Await _context.AccountingPeriods _
                .OrderBy(Function(p) p.FiscalYear) _
                .ThenBy(Function(p) p.PeriodNumber) _
                .ToListAsync()
        End Function

        Public Sub Add(period As AccountingPeriod)
            If period Is Nothing Then Throw New ArgumentNullException(NameOf(period))
            _context.AccountingPeriods.Add(period)
            _context.SaveChanges()
        End Sub

        Public Async Function AddAsync(period As AccountingPeriod) As Task
            If period Is Nothing Then Throw New ArgumentNullException(NameOf(period))
            Await _context.AccountingPeriods.AddAsync(period)
            Await _context.SaveChangesAsync()
        End Function

        Public Sub Update(period As AccountingPeriod)
            If period Is Nothing Then Throw New ArgumentNullException(NameOf(period))
            _context.AccountingPeriods.Update(period)
            _context.SaveChanges()
        End Sub

        Public Async Function UpdateAsync(period As AccountingPeriod) As Task
            If period Is Nothing Then Throw New ArgumentNullException(NameOf(period))
            _context.AccountingPeriods.Update(period)
            Await _context.SaveChangesAsync()
        End Function

    End Class

End Namespace
