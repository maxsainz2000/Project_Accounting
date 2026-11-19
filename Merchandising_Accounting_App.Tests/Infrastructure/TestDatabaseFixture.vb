Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports Microsoft.Data.Sqlite
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Data

Namespace Infrastructure

    ''' <summary>
    ''' Test database fixture that provides an isolated in-memory SQLite database instance
    ''' with initialized schema, pragmas, and seeded Master Chart of Accounts and fiscal periods.
    ''' Maintains an open SQLite connection for the duration of the fixture lifecycle to preserve in-memory state.
    ''' </summary>
    Public Class TestDatabaseFixture
        Implements IDisposable

        Private ReadOnly _connection As SqliteConnection
        Private ReadOnly _options As DbContextOptions(Of ErpDbContext)
        Private _disposed As Boolean = False

        Public Sub New()
            ' SQLite in-memory database requires keeping the connection open
            _connection = New SqliteConnection("DataSource=:memory:")
            _connection.Open()

            _options = New DbContextOptionsBuilder(Of ErpDbContext)() _
                .UseSqlite(_connection) _
                .Options

            Using context As New ErpDbContext(_options)
                DatabaseInitializer.Initialize(context)
            End Using
        End Sub

        Public Function CreateContext() As ErpDbContext
            If _disposed Then
                Throw New ObjectDisposedException(NameOf(TestDatabaseFixture))
            End If
            Return New ErpDbContext(_options)
        End Function

        Public ReadOnly Property Connection As SqliteConnection
            Get
                Return _connection
            End Get
        End Property

        Public Sub Dispose() Implements IDisposable.Dispose
            If Not _disposed Then
                _connection?.Dispose()
                _disposed = True
            End If
            GC.SuppressFinalize(Me)
        End Sub

    End Class

End Namespace
