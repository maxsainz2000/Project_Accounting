Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data.Repositories

    ''' <summary>
    ''' Repository for Master Chart of Accounts (COA) entities.
    ''' </summary>
    Public Class AccountRepository

        Private ReadOnly _context As ErpDbContext

        Public Sub New(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            _context = context
        End Sub

        Public Function GetById(accountId As Integer) As Account
            Return _context.Accounts.Find(accountId)
        End Function

        Public Async Function GetByIdAsync(accountId As Integer) As Task(Of Account)
            Return Await _context.Accounts.FindAsync(accountId)
        End Function

        Public Function GetByCode(code As String) As Account
            If String.IsNullOrWhiteSpace(code) Then Return Nothing
            Return _context.Accounts.FirstOrDefault(Function(a) a.Code = code)
        End Function

        Public Async Function GetByCodeAsync(code As String) As Task(Of Account)
            If String.IsNullOrWhiteSpace(code) Then Return Nothing
            Return Await _context.Accounts.FirstOrDefaultAsync(Function(a) a.Code = code)
        End Function

        Public Function GetAll() As List(Of Account)
            Return _context.Accounts.OrderBy(Function(a) a.Code).ToList()
        End Function

        Public Async Function GetAllAsync() As Task(Of List(Of Account))
            Return Await _context.Accounts.OrderBy(Function(a) a.Code).ToListAsync()
        End Function

        Public Function GetAllActive() As List(Of Account)
            Return _context.Accounts.Where(Function(a) a.IsActive).OrderBy(Function(a) a.Code).ToList()
        End Function

        Public Async Function GetAllActiveAsync() As Task(Of List(Of Account))
            Return Await _context.Accounts.Where(Function(a) a.IsActive).OrderBy(Function(a) a.Code).ToListAsync()
        End Function

        Public Function GetByType(accountType As AccountType) As List(Of Account)
            Return _context.Accounts.Where(Function(a) a.AccountType = accountType AndAlso a.IsActive).OrderBy(Function(a) a.Code).ToList()
        End Function

        Public Async Function GetByTypeAsync(accountType As AccountType) As Task(Of List(Of Account))
            Return Await _context.Accounts.Where(Function(a) a.AccountType = accountType AndAlso a.IsActive).OrderBy(Function(a) a.Code).ToListAsync()
        End Function

        Public Sub Add(account As Account)
            If account Is Nothing Then Throw New ArgumentNullException(NameOf(account))
            _context.Accounts.Add(account)
            _context.SaveChanges()
        End Sub

        Public Async Function AddAsync(account As Account) As Task
            If account Is Nothing Then Throw New ArgumentNullException(NameOf(account))
            Await _context.Accounts.AddAsync(account)
            Await _context.SaveChangesAsync()
        End Function

        Public Sub Update(account As Account)
            If account Is Nothing Then Throw New ArgumentNullException(NameOf(account))
            _context.Accounts.Update(account)
            _context.SaveChanges()
        End Sub

        Public Async Function UpdateAsync(account As Account) As Task
            If account Is Nothing Then Throw New ArgumentNullException(NameOf(account))
            _context.Accounts.Update(account)
            Await _context.SaveChangesAsync()
        End Function

    End Class

End Namespace
