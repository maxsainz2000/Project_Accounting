Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Data
Imports System.Linq
Imports System.Threading.Tasks
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models

Namespace Data

    ''' <summary>
    ''' Initializes the SQLite database, establishes performance and concurrency pragmas (WAL, NORMAL, foreign keys, busy timeout),
    ''' ensures schema creation, and seeds foundational Master Chart of Accounts (COA) and active fiscal periods.
    ''' </summary>
    Public Class DatabaseInitializer

        Public Shared Sub ApplyPragmas(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If

            context.Database.ExecuteSqlRaw("PRAGMA journal_mode = WAL;")
            context.Database.ExecuteSqlRaw("PRAGMA synchronous = NORMAL;")
            context.Database.ExecuteSqlRaw("PRAGMA foreign_keys = ON;")
            context.Database.ExecuteSqlRaw("PRAGMA busy_timeout = 3000;")
        End Sub

        Public Shared Async Function ApplyPragmasAsync(context As ErpDbContext) As Task
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If

            Await context.Database.ExecuteSqlRawAsync("PRAGMA journal_mode = WAL;")
            Await context.Database.ExecuteSqlRawAsync("PRAGMA synchronous = NORMAL;")
            Await context.Database.ExecuteSqlRawAsync("PRAGMA foreign_keys = ON;")
            Await context.Database.ExecuteSqlRawAsync("PRAGMA busy_timeout = 3000;")
        End Function

        Public Shared Sub Initialize(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If

            ' Ensure connection is open so connection-scoped pragmas stick
            If context.Database.GetDbConnection().State <> ConnectionState.Open Then
                context.Database.OpenConnection()
            End If

            ApplyPragmas(context)
            context.Database.EnsureCreated()
            SeedMasterData(context)
        End Sub

        Public Shared Async Function InitializeAsync(context As ErpDbContext) As Task
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If

            If context.Database.GetDbConnection().State <> ConnectionState.Open Then
                Await context.Database.OpenConnectionAsync()
            End If

            Await ApplyPragmasAsync(context)
            Await context.Database.EnsureCreatedAsync()
            Await SeedMasterDataAsync(context)
        End Function

        Public Shared Sub SeedMasterData(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If

            SeedChartOfAccounts(context)
            SeedAccountingPeriods(context)
            context.SaveChanges()
        End Sub

        Public Shared Async Function SeedMasterDataAsync(context As ErpDbContext) As Task
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If

            SeedChartOfAccounts(context)
            SeedAccountingPeriods(context)
            Await context.SaveChangesAsync()
        End Function

        Private Shared Sub SeedChartOfAccounts(context As ErpDbContext)
            If context.Accounts.Any() Then
                Return
            End If

            Dim defaultAccounts As New List(Of Account) From {
                New Account("1010", "Cash on Hand", AccountType.Asset),
                New Account("1020", "Operating Bank Account", AccountType.Asset),
                New Account("1200", "Accounts Receivable", AccountType.Asset),
                New Account("1300", "Merchandise Inventory", AccountType.Asset),
                New Account("2000", "Accounts Payable", AccountType.Liability),
                New Account("2100", "Goods Received Not Invoiced (GRNI)", AccountType.Liability),
                New Account("3000", "Retained Earnings", AccountType.Equity),
                New Account("3010", "Owner's Capital", AccountType.Equity),
                New Account("4000", "Sales Revenue", AccountType.Revenue),
                New Account("4100", "Sales Returns & Allowances", AccountType.Revenue),
                New Account("5000", "Cost of Goods Sold (COGS)", AccountType.Expense),
                New Account("5100", "Purchase Price Variance (PPV)", AccountType.Expense),
                New Account("5200", "Inbound Freight Variance", AccountType.Expense),
                New Account("5300", "Inventory Shrinkage & Damaged Stock", AccountType.Expense),
                New Account("5400", "Stocktake Revaluation Variance", AccountType.Expense),
                New Account("5500", "Cash Rounding & Roundoff Variance", AccountType.Expense)
            }

            context.Accounts.AddRange(defaultAccounts)
        End Sub

        Private Shared Sub SeedAccountingPeriods(context As ErpDbContext)
            If context.AccountingPeriods.Any() Then
                Return
            End If

            ' Initial open period FY2026-P11 (November 2026)
            Dim initialPeriod As New AccountingPeriod(2026, 11, New DateTime(2026, 11, 1, 0, 0, 0), New DateTime(2026, 11, 30, 23, 59, 59)) With {
                .IsClosed = False,
                .ClosedAtUtc = Nothing
            }

            context.AccountingPeriods.Add(initialPeriod)
        End Sub

    End Class

End Namespace
