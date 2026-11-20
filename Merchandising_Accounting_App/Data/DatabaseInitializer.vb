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
Imports Merchandising_Accounting_App.Services

Namespace Data

    ''' <summary>
    ''' Initializes the SQLite database, establishes performance and concurrency pragmas (WAL, NORMAL, foreign keys, busy timeout),
    ''' ensures schema creation, and seeds foundational Master Chart of Accounts (COA) and active fiscal periods.
    ''' </summary>
    Public Class DatabaseInitializer

        ''' <summary>
        ''' Gets the authoritative local SQLite database file path anchored to Application.StartupPath.
        ''' </summary>
        Public Shared Function GetDefaultDatabasePath() As String
            Dim basePath = If(Not String.IsNullOrEmpty(System.Windows.Forms.Application.StartupPath),
                              System.Windows.Forms.Application.StartupPath,
                              AppContext.BaseDirectory)
            Return System.IO.Path.Combine(basePath, "merchandising_erp.db")
        End Function

        ''' <summary>
        ''' Builds a standard SQLite connection string enforcing Foreign Keys and busy timeout.
        ''' </summary>
        Public Shared Function BuildConnectionString(Optional dbPath As String = Nothing) As String
            Dim resolvedPath = If(String.IsNullOrWhiteSpace(dbPath), GetDefaultDatabasePath(), dbPath)
            Return $"Data Source={resolvedPath};Foreign Keys=True;Default Timeout=3;"
        End Function

        ''' <summary>
        ''' Bootstraps the local SQLite database file, establishes WAL pragmas, creates schema,
        ''' seeds foundational master data (COA &amp; active period), and populates initial operational data.
        ''' </summary>
        Public Shared Sub InitializeDatabase(Optional dbPath As String = Nothing)
            Dim connString = BuildConnectionString(dbPath)
            Dim optionsBuilder As New DbContextOptionsBuilder(Of ErpDbContext)()
            optionsBuilder.UseSqlite(connString)
            Using context As New ErpDbContext(optionsBuilder.Options)
                Initialize(context)
                SeedOperationalSampleData(context)
            End Using
        End Sub

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

        ''' <summary>
        ''' Seeds initial commercial master data (Products, Vendors, Customers) and end-to-end operational
        ''' transactions (PO, GRN, 3-Way Match, Sales Invoice, Customer Return with Quarantine Inspection, Settlements)
        ''' ensuring all UI modules bind to live database records with double-entry integrity.
        ''' </summary>
        Public Shared Sub SeedOperationalSampleData(context As ErpDbContext)
            If context Is Nothing Then Throw New ArgumentNullException(NameOf(context))
            If context.Products.Any() Then
                Return
            End If

            ' 1. Seed Products
            Dim p1 As New Product("SKU-BEV-001", "Artisan Colombian Coffee Beans (1kg)", "kg", 15.00D, 0)
            Dim p2 As New Product("SKU-BEV-002", "Organic Earl Grey Tea (500g)", "pk", 8.10D, 0)
            Dim p3 As New Product("SKU-PKG-003", "Biodegradable Hot Cups (Pack of 50)", "pk", 4.00D, 0)
            Dim p4 As New Product("SKU-SNK-004", "Almond Butter Crisps (12pk)", "bx", 6.50D, 0)
            context.Products.AddRange(p1, p2, p3, p4)

            ' 2. Seed Vendors
            Dim v1 As New Vendor("VEND-101", "Global Bean Importers Ltd.", 30, 0D)
            Dim v2 As New Vendor("VEND-102", "EcoPackaging Supplies Co.", 15, 0D)
            Dim v3 As New Vendor("VEND-103", "Ceylon Tea Traders Inc.", 45, 0D)
            Dim v4 As New Vendor("VEND-104", "Pacific Freight & Logistics", 15, 0D)
            context.Vendors.AddRange(v1, v2, v3, v4)

            ' 3. Seed Customers
            Dim c1 As New Customer("CUST-001", "Apex Wholesale Distributors Ltd.", 50000.0D, 0D)
            Dim c2 As New Customer("CUST-002", "Beacon Commercial Mart Chain", 35000.0D, 0D)
            Dim c3 As New Customer("CUST-003", "Crestview Gourmet Foods Inc.", 15000.0D, 0D)
            Dim c4 As New Customer("CUST-004", "Delta Express Mart", 10000.0D, 0D)
            context.Customers.AddRange(c1, c2, c3, c4)

            context.SaveChanges()

            ' 4. Seed Workflow Transactions via Domain Workflow Services
            Dim p2pService As New ProcureToPayService(context)
            Dim o2cService As New OrderToCashService(context)
            Dim returnsService As New ReturnsService(context)

            ' CUJ 1: Inbound Receiving Dock GRN + 3-Way Match
            ' PO 1: Vendor 1, 100 units SKU-BEV-001 @ $15.00
            Dim po1Lines As New List(Of POLine) From {
                New POLine(p1.ProductId, 100, 15.00D)
            }
            Dim po1 = p2pService.CreatePurchaseOrder(v1.VendorId, po1Lines, New DateTime(2026, 11, 2), "PO-2026-0041")

            ' GRN 1: Received 50 units
            Dim grn1Lines As New List(Of GRNLine) From {
                New GRNLine(po1.Lines.First().POLineId, p1.ProductId, 50, 15.00D)
            }
            Dim grn1 = p2pService.ReceiveGoods(po1.POId, grn1Lines, New DateTime(2026, 11, 3), "GRN-2026-0088", New DateTime(2026, 11, 3))

            ' Supplier Bill 1: 50 units @ $16.20 with $45 inbound freight -> Unfavorable PPV +$60.00
            Dim bill1Lines As New List(Of BillLine) From {
                New BillLine() With {
                    .GRNLineId = grn1.Lines.First().GRNLineId,
                    .ProductId = p1.ProductId,
                    .BilledQty = 50,
                    .BilledUnitPrice = 16.20D,
                    .LineTotal = 50 * 16.20D,
                    .FreightAmount = 45.00D
                }
            }
            Dim bill1 = p2pService.MatchSupplierBill(v1.VendorId, grn1.GRNId, bill1Lines, "BILL-2026-1092", New DateTime(2026, 11, 5), New DateTime(2026, 11, 5))

            ' PO 2: Vendor 2, 200 units SKU-PKG-003 @ $4.00
            Dim po2Lines As New List(Of POLine) From {
                New POLine(p3.ProductId, 200, 4.00D)
            }
            Dim po2 = p2pService.CreatePurchaseOrder(v2.VendorId, po2Lines, New DateTime(2026, 11, 3), "PO-2026-0042")

            ' GRN 2: Received 200 units
            Dim grn2Lines As New List(Of GRNLine) From {
                New GRNLine(po2.Lines.First().POLineId, p3.ProductId, 200, 4.00D)
            }
            Dim grn2 = p2pService.ReceiveGoods(po2.POId, grn2Lines, New DateTime(2026, 11, 4), "GRN-2026-0099", New DateTime(2026, 11, 4))

            ' Supplier Bill 2: Exact Match (200 units @ $4.00, $0 freight)
            Dim bill2Lines As New List(Of BillLine) From {
                New BillLine() With {
                    .GRNLineId = grn2.Lines.First().GRNLineId,
                    .ProductId = p3.ProductId,
                    .BilledQty = 200,
                    .BilledUnitPrice = 4.00D,
                    .LineTotal = 800.00D,
                    .FreightAmount = 0.00D
                }
            }
            Dim bill2 = p2pService.MatchSupplierBill(v2.VendorId, grn2.GRNId, bill2Lines, "BILL-2026-1088", New DateTime(2026, 11, 6), New DateTime(2026, 11, 6))

            ' PO 3: Vendor 3, 75 units SKU-BEV-002 @ $8.10
            Dim po3Lines As New List(Of POLine) From {
                New POLine(p2.ProductId, 75, 8.10D)
            }
            Dim po3 = p2pService.CreatePurchaseOrder(v3.VendorId, po3Lines, New DateTime(2026, 11, 5), "PO-2026-0043")

            ' GRN 3: Received 75 units (Pending Bill)
            Dim grn3Lines As New List(Of GRNLine) From {
                New GRNLine(po3.Lines.First().POLineId, p2.ProductId, 75, 8.10D)
            }
            Dim grn3 = p2pService.ReceiveGoods(po3.POId, grn3Lines, New DateTime(2026, 11, 7), "GRN-2026-0104", New DateTime(2026, 11, 7))

            ' CUJ 2: Outbound Sales Order & Invoice Fulfillment
            ' SO 1: Customer 1, 40 units SKU-BEV-001 @ $24.50
            Dim so1Lines As New List(Of SOLine) From {
                New SOLine(p1.ProductId, 40, 24.50D)
            }
            Dim so1 = o2cService.CreateSalesOrder(c1.CustomerId, so1Lines, New DateTime(2026, 11, 7), "SO-2026-0081")
            Dim inv1 = o2cService.FulfillSalesInvoice(so1.SOId, New DateTime(2026, 11, 8))

            ' SO 2: Customer 2, 50 units SKU-PKG-003 @ $7.50
            Dim so2Lines As New List(Of SOLine) From {
                New SOLine(p3.ProductId, 50, 7.50D)
            }
            Dim so2 = o2cService.CreateSalesOrder(c2.CustomerId, so2Lines, New DateTime(2026, 11, 9), "SO-2026-0082")

            ' CUJ 3: Customer Return with Quarantine Inspection (10 units returned: 8 restocked, 2 scrapped)
            Dim rmaLines As New List(Of CustomerReturnLine) From {
                New CustomerReturnLine(p1.ProductId, 10, 0, 0, p1.CurrentUnitCost, 24.50D)
            }
            Dim ret1 = returnsService.ReceiveReturnToQuarantine(c1.CustomerId, inv1.InvoiceId, rmaLines, New DateTime(2026, 11, 10), "RMA-2026-0012")
            Dim rmaRetLine = ret1.Lines.First()
            Dim inspectionDispositions As New List(Of ReturnDispositionLine) From {
                New ReturnDispositionLine(p1.ProductId, 8, 2, rmaRetLine.ReturnLineId)
            }
            returnsService.ProcessReturnInspection(ret1.ReturnId, inspectionDispositions, New DateTime(2026, 11, 10))

            ' Settlements
            Dim bankAccount = context.Accounts.FirstOrDefault(Function(a) a.Code = "1020")
            If bankAccount IsNot Nothing Then
                ' Payment Voucher: Settle Bill 2 ($800)
                p2pService.DisbursePaymentVoucher(v2.VendorId, bill2.BillId, 800.00D, bankAccount.AccountId, New DateTime(2026, 11, 12), New DateTime(2026, 11, 12), "PV-2026-0033")

                ' Payment Receipt: Customer 1 pays $450 towards invoice
                o2cService.RecordPaymentReceipt(c1.CustomerId, inv1.InvoiceId, 450.00D, bankAccount.AccountId, New DateTime(2026, 11, 14), New DateTime(2026, 11, 14))
            End If
        End Sub

    End Class

End Namespace
