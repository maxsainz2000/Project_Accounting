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
    ''' General Ledger repository managing financial journal entries and debit/credit distributions.
    ''' Strictly enforces the append-only invariant: updates and deletions are expressly prohibited.
    ''' </summary>
    Public Class LedgerRepository

        Private ReadOnly _context As ErpDbContext

        Public Sub New(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            _context = context
        End Sub

        Public Sub Add(entry As JournalEntry)
            If entry Is Nothing Then Throw New ArgumentNullException(NameOf(entry))
            _context.JournalEntries.Add(entry)
            _context.SaveChanges()
        End Sub

        Public Async Function AddAsync(entry As JournalEntry) As Task
            If entry Is Nothing Then Throw New ArgumentNullException(NameOf(entry))
            Await _context.JournalEntries.AddAsync(entry)
            Await _context.SaveChangesAsync()
        End Function

        Public Function GetById(entryId As Integer) As JournalEntry
            Return _context.JournalEntries _
                .Include(Function(j) j.Lines) _
                    .ThenInclude(Function(l) l.Account) _
                .Include(Function(j) j.Period) _
                .FirstOrDefault(Function(j) j.EntryId = entryId)
        End Function

        Public Async Function GetByIdAsync(entryId As Integer) As Task(Of JournalEntry)
            Return Await _context.JournalEntries _
                .Include(Function(j) j.Lines) _
                    .ThenInclude(Function(l) l.Account) _
                .Include(Function(j) j.Period) _
                .FirstOrDefaultAsync(Function(j) j.EntryId = entryId)
        End Function

        Public Function GetByPeriod(periodId As Integer) As List(Of JournalEntry)
            Return _context.JournalEntries _
                .Include(Function(j) j.Lines) _
                    .ThenInclude(Function(l) l.Account) _
                .Where(Function(j) j.PeriodId = periodId) _
                .OrderBy(Function(j) j.PostingDate) _
                .ThenBy(Function(j) j.EntryId) _
                .ToList()
        End Function

        Public Async Function GetByPeriodAsync(periodId As Integer) As Task(Of List(Of JournalEntry))
            Return Await _context.JournalEntries _
                .Include(Function(j) j.Lines) _
                    .ThenInclude(Function(l) l.Account) _
                .Where(Function(j) j.PeriodId = periodId) _
                .OrderBy(Function(j) j.PostingDate) _
                .ThenBy(Function(j) j.EntryId) _
                .ToListAsync()
        End Function

        Public Function GetByDateRange(startDate As DateTime, endDate As DateTime) As List(Of JournalEntry)
            Return _context.JournalEntries _
                .Include(Function(j) j.Lines) _
                    .ThenInclude(Function(l) l.Account) _
                .Where(Function(j) j.PostingDate >= startDate AndAlso j.PostingDate <= endDate) _
                .OrderBy(Function(j) j.PostingDate) _
                .ThenBy(Function(j) j.EntryId) _
                .ToList()
        End Function

        Public Async Function GetByDateRangeAsync(startDate As DateTime, endDate As DateTime) As Task(Of List(Of JournalEntry))
            Return Await _context.JournalEntries _
                .Include(Function(j) j.Lines) _
                    .ThenInclude(Function(l) l.Account) _
                .Where(Function(j) j.PostingDate >= startDate AndAlso j.PostingDate <= endDate) _
                .OrderBy(Function(j) j.PostingDate) _
                .ThenBy(Function(j) j.EntryId) _
                .ToListAsync()
        End Function

        Public Function GetAll() As List(Of JournalEntry)
            Return _context.JournalEntries _
                .Include(Function(j) j.Lines) _
                    .ThenInclude(Function(l) l.Account) _
                .OrderByDescending(Function(j) j.PostingDate) _
                .ThenByDescending(Function(j) j.EntryId) _
                .ToList()
        End Function

        Public Async Function GetAllAsync() As Task(Of List(Of JournalEntry))
            Return Await _context.JournalEntries _
                .Include(Function(j) j.Lines) _
                    .ThenInclude(Function(l) l.Account) _
                .OrderByDescending(Function(j) j.PostingDate) _
                .ThenByDescending(Function(j) j.EntryId) _
                .ToListAsync()
        End Function

        ''' <summary>
        ''' Rejected: General Ledger entries are immutable and append-only.
        ''' </summary>
        Public Sub Update(entry As JournalEntry)
            Throw New InvalidOperationException("General Ledger journal entries are strictly append-only; update operations are prohibited.")
        End Sub

        ''' <summary>
        ''' Rejected: General Ledger entries are immutable and append-only.
        ''' </summary>
        Public Function UpdateAsync(entry As JournalEntry) As Task
            Throw New InvalidOperationException("General Ledger journal entries are strictly append-only; update operations are prohibited.")
        End Function

        ''' <summary>
        ''' Rejected: General Ledger entries are immutable and append-only.
        ''' </summary>
        Public Sub Delete(entry As JournalEntry)
            Throw New InvalidOperationException("General Ledger journal entries are strictly append-only; delete operations are prohibited.")
        End Sub

        ''' <summary>
        ''' Rejected: General Ledger entries are immutable and append-only.
        ''' </summary>
        Public Sub Delete(entryId As Integer)
            Throw New InvalidOperationException("General Ledger journal entries are strictly append-only; delete operations are prohibited.")
        End Sub

        ''' <summary>
        ''' Rejected: General Ledger entries are immutable and append-only.
        ''' </summary>
        Public Function DeleteAsync(entryId As Integer) As Task
            Throw New InvalidOperationException("General Ledger journal entries are strictly append-only; delete operations are prohibited.")
        End Function

    End Class

End Namespace
