Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models
Imports Merchandising_Accounting_App.Tests.Infrastructure
Imports Merchandising_Accounting_App.Services

Namespace Services

    <TestClass>
    Public Class ReportingServiceTests

        ''' <summary>
        ''' Helper to create and post a balanced journal entry directly into the context.
        ''' </summary>
        Private Function PostJournalEntry(context As ErpDbContext, postingDate As DateTime, sourceDocType As String, docNumber As String, lines As IEnumerable(Of Tuple(Of String, Decimal, Decimal, String))) As JournalEntry
            Dim period = context.AccountingPeriods.FirstOrDefault(Function(p) postingDate.Date >= p.StartDate.Date AndAlso postingDate.Date <= p.EndDate.Date)
            Dim periodId As Integer = If(period IsNot Nothing, period.PeriodId, 1)

            Dim entry As New JournalEntry() With {
                .PostingDate = postingDate,
                .OperationalEventDate = postingDate,
                .SourceDocType = sourceDocType,
                .SourceDocId = 1,
                .PeriodId = periodId,
                .Description = $"Posting for {docNumber}",
                .CreatedBy = "test_user"
            }

            For Each line In lines
                Dim acct = context.Accounts.First(Function(a) a.Code = line.Item1)
                entry.Lines.Add(New JournalEntryLine() With {
                    .AccountId = acct.AccountId,
                    .DebitAmount = line.Item2,
                    .CreditAmount = line.Item3,
                    .LineMemo = line.Item4
                })
            Next

            context.JournalEntries.Add(entry)
            context.SaveChanges()
            Return entry
        End Function

        <TestMethod>
        Public Sub TrialBalance_ZeroPostings_IsBalancedWithZeroTotals()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)

                    Dim tb = reportingSvc.GetTrialBalance()

                    Assert.IsNotNull(tb)
                    Assert.IsTrue(tb.IsBalanced)
                    Assert.AreEqual(0D, tb.TotalDebits)
                    Assert.AreEqual(0D, tb.TotalCredits)
                    Assert.AreEqual(0D, tb.ImbalanceVariance)
                    Assert.IsTrue(tb.Rows.Count >= 14) ' All default COA accounts present
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub TrialBalance_WithMultipleBalancedPostings_EnforcesDebitsEqualCredits()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)
                    Dim postingDate As New DateTime(2026, 11, 5)

                    ' JE 1: Capital injection: Cash (1010) Dr $10,000 / Owner's Capital (3010) Cr $10,000
                    PostJournalEntry(context, postingDate, "CapitalInjection", "CAP-001", {
                        Tuple.Create("1010", 10000.0D, 0.0D, "Capital funding"),
                        Tuple.Create("3010", 0.0D, 10000.0D, "Owner capital")
                    })

                    ' JE 2: GRN Dock Receiving: Inventory (1300) Dr $2,000 / GRNI (2100) Cr $2,000
                    PostJournalEntry(context, postingDate, "GRN", "GRN-001", {
                        Tuple.Create("1300", 2000.0D, 0.0D, "Stock receipt 200 units"),
                        Tuple.Create("2100", 0.0D, 2000.0D, "GRNI clearing")
                    })

                    ' JE 3: Supplier Bill: GRNI (2100) Dr $2,000 / AP (2000) Cr $2,000
                    PostJournalEntry(context, postingDate, "SupplierBill", "BILL-001", {
                        Tuple.Create("2100", 2000.0D, 0.0D, "Clear GRNI"),
                        Tuple.Create("2000", 0.0D, 2000.0D, "Trade AP payable")
                    })

                    ' JE 4: Sales Invoice: AR (1200) Dr $1,500 / Revenue (4000) Cr $1,500
                    '                     COGS (5000) Dr $800 / Inventory (1300) Cr $800
                    PostJournalEntry(context, postingDate, "SalesInvoice", "INV-001", {
                        Tuple.Create("1200", 1500.0D, 0.0D, "Customer receivable"),
                        Tuple.Create("4000", 0.0D, 1500.0D, "Gross sales revenue"),
                        Tuple.Create("5000", 800.0D, 0.0D, "Cost of goods sold"),
                        Tuple.Create("1300", 0.0D, 800.0D, "Stock dispatch reduction")
                    })

                    Dim tb = reportingSvc.GetTrialBalance(postingDate)

                    Assert.IsNotNull(tb)
                    Assert.IsTrue(tb.IsBalanced, $"Trial balance must be balanced. Debits: {tb.TotalDebits}, Credits: {tb.TotalCredits}")
                    Assert.AreEqual(0D, tb.ImbalanceVariance)

                    ' Total debits and credits must be equal
                    Assert.AreEqual(tb.TotalDebits, tb.TotalCredits)

                    ' Verify individual accounts net balances
                    Dim cashRow = tb.Rows.First(Function(r) r.AccountCode = "1010")
                    Assert.AreEqual(10000.0D, cashRow.DebitAmount)
                    Assert.AreEqual(0.0D, cashRow.CreditAmount)

                    Dim invRow = tb.Rows.First(Function(r) r.AccountCode = "1300")
                    Assert.AreEqual(1200.0D, invRow.DebitAmount) ' 2000 - 800 = 1200

                    Dim grniRow = tb.Rows.First(Function(r) r.AccountCode = "2100")
                    Assert.AreEqual(0.0D, grniRow.DebitAmount)
                    Assert.AreEqual(0.0D, grniRow.CreditAmount) ' 2000 - 2000 = 0

                    Dim apRow = tb.Rows.First(Function(r) r.AccountCode = "2000")
                    Assert.AreEqual(2000.0D, apRow.CreditAmount)

                    Dim capitalRow = tb.Rows.First(Function(r) r.AccountCode = "3010")
                    Assert.AreEqual(10000.0D, capitalRow.CreditAmount)

                    Dim revRow = tb.Rows.First(Function(r) r.AccountCode = "4000")
                    Assert.AreEqual(1500.0D, revRow.CreditAmount)

                    Dim cogsRow = tb.Rows.First(Function(r) r.AccountCode = "5000")
                    Assert.AreEqual(800.0D, cogsRow.DebitAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub BalanceSheet_FundamentalEquation_AssetsEqualLiabilitiesPlusEquity()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)
                    Dim postingDate As New DateTime(2026, 11, 10)

                    ' Seed initial postings:
                    ' 1010 Cash Dr $15,000 / 3010 Capital Cr $15,000
                    PostJournalEntry(context, postingDate, "Funding", "F-01", {
                        Tuple.Create("1010", 15000.0D, 0.0D, "Funding"),
                        Tuple.Create("3010", 0.0D, 15000.0D, "Capital")
                    })

                    ' 1300 Inventory Dr $5,000 / 2000 AP Cr $5,000
                    PostJournalEntry(context, postingDate, "Purchases", "P-01", {
                        Tuple.Create("1300", 5000.0D, 0.0D, "Stock"),
                        Tuple.Create("2000", 0.0D, 5000.0D, "Payable")
                    })

                    ' Sales: AR Dr $3,000 / Revenue Cr $3,000
                    '        COGS Dr $1,800 / Inventory Cr $1,800
                    PostJournalEntry(context, postingDate, "Sales", "S-01", {
                        Tuple.Create("1200", 3000.0D, 0.0D, "Receivable"),
                        Tuple.Create("4000", 0.0D, 3000.0D, "Revenue"),
                        Tuple.Create("5000", 1800.0D, 0.0D, "COGS"),
                        Tuple.Create("1300", 0.0D, 1800.0D, "Inventory Out")
                    })

                    ' Operating variances: Freight Dr $150 / AP Cr $150
                    PostJournalEntry(context, postingDate, "FreightBill", "FB-01", {
                        Tuple.Create("5200", 150.0D, 0.0D, "Freight variance"),
                        Tuple.Create("2000", 0.0D, 150.0D, "Payable carrier")
                    })

                    Dim bs = reportingSvc.GetBalanceSheet(postingDate)

                    Assert.IsNotNull(bs)
                    Assert.IsTrue(bs.IsBalanced, $"Balance sheet must satisfy Assets = Liabilities + Equity. Variance: {bs.ImbalanceVariance}")

                    ' Assets: Cash $15,000 + Inventory ($5,000 - $1,800 = $3,200) + AR $3,000 = $21,200
                    Assert.AreEqual(21200.0D, bs.TotalAssets)

                    ' Liabilities: AP $5,000 + $150 = $5,150
                    Assert.AreEqual(5150.0D, bs.TotalLiabilities)

                    ' Equity Accounts Total: Capital $15,000
                    Assert.AreEqual(15000.0D, bs.EquityAccountsTotal)

                    ' Current Net Income: Revenue $3,000 - COGS $1,800 - Freight $150 = $1,050
                    Assert.AreEqual(1050.0D, bs.CurrentNetIncome)

                    ' Total Equity: $15,000 + $1,050 = $16,050
                    Assert.AreEqual(16050.0D, bs.TotalEquity)

                    ' Total Liabilities & Equity: $5,150 + $16,050 = $21,200
                    Assert.AreEqual(21200.0D, bs.TotalLiabilitiesAndEquity)

                    Assert.AreEqual(bs.TotalAssets, bs.TotalLiabilitiesAndEquity)
                    Assert.AreEqual(0D, bs.ImbalanceVariance)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub IncomeStatement_CalculatesGrossProfitAndOperatingIncome()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)
                    Dim postingDate As New DateTime(2026, 11, 12)

                    ' Sales Revenue $600 / AR $600
                    PostJournalEntry(context, postingDate, "Sales", "INV-1", {
                        Tuple.Create("1200", 600.0D, 0.0D, "AR"),
                        Tuple.Create("4000", 0.0D, 600.0D, "Sales Revenue")
                    })

                    ' Sales Returns $150 / AR $150
                    PostJournalEntry(context, postingDate, "Return", "RET-1", {
                        Tuple.Create("4100", 150.0D, 0.0D, "Returns"),
                        Tuple.Create("1200", 0.0D, 150.0D, "AR Credit")
                    })

                    ' COGS $300 / Inventory $300
                    PostJournalEntry(context, postingDate, "COGS", "CG-1", {
                        Tuple.Create("5000", 300.0D, 0.0D, "COGS"),
                        Tuple.Create("1300", 0.0D, 300.0D, "Stock Out")
                    })

                    ' Variances: PPV $100, Freight $50, Shrinkage $20 / Cash $170
                    PostJournalEntry(context, postingDate, "Expenses", "EXP-1", {
                        Tuple.Create("5100", 100.0D, 0.0D, "PPV"),
                        Tuple.Create("5200", 50.0D, 0.0D, "Freight"),
                        Tuple.Create("5300", 20.0D, 0.0D, "Shrinkage"),
                        Tuple.Create("1010", 0.0D, 170.0D, "Cash Out")
                    })

                    Dim isReport = reportingSvc.GetIncomeStatement(New DateTime(2026, 11, 1), New DateTime(2026, 11, 30))

                    Assert.IsNotNull(isReport)
                    Assert.AreEqual(600.0D, isReport.GrossSalesRevenue)
                    Assert.AreEqual(150.0D, isReport.SalesReturnsAndAllowances)
                    Assert.AreEqual(450.0D, isReport.NetSalesRevenue)
                    Assert.AreEqual(300.0D, isReport.CostOfGoodsSold)
                    Assert.AreEqual(150.0D, isReport.GrossProfitMargin)
                    Assert.AreEqual(170.0D, isReport.TotalOperatingExpenses)
                    Assert.AreEqual(-20.0D, isReport.NetOperatingIncome)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ProductProfitabilityBridge_ReconcilesRevenueToNetLandedMargin()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)

                    ' Seed Customer & Vendor
                    Dim cust As New Customer() With {.Code = "CUST-001", .Name = "Bean Cafe"}
                    Dim vend As New Vendor() With {.Code = "VEND-001", .Name = "Coffee Importers Ltd"}
                    context.Customers.Add(cust)
                    context.Vendors.Add(vend)
                    context.SaveChanges()

                    ' Seed Product
                    Dim prod As New Product() With {
                        .SKU = "SKU-A001",
                        .Name = "Organic Arabica Coffee Beans 1kg",
                        .BaseUnitName = "Bag",
                        .PackMultiplier = 1,
                        .CurrentUnitCost = 7.5D,
                        .TotalQtyOnHand = 100,
                        .IsActive = True
                    }
                    context.Products.Add(prod)
                    context.SaveChanges()

                    ' Seed POLine & GRNLine for PPV tracking
                    Dim po As New PurchaseOrder() With {.VendorId = vend.VendorId, .DocumentNumber = "PO-001", .OrderDate = New DateTime(2026, 11, 1), .TotalAmount = 400.0D}
                    context.PurchaseOrders.Add(po)
                    context.SaveChanges()

                    Dim poLine As New POLine(prod.ProductId, 40, 10.0D) With {.POId = po.POId}
                    context.POLines.Add(poLine)
                    context.SaveChanges()

                    Dim grn As New GoodsReceiptNote() With {.POId = po.POId, .DocumentNumber = "GRN-001", .ReceiptDate = New DateTime(2026, 11, 2), .PostingDate = New DateTime(2026, 11, 2), .Status = "Posted"}
                    context.GoodsReceiptNotes.Add(grn)
                    context.SaveChanges()

                    Dim grnLine As New GRNLine(poLine.POLineId, prod.ProductId, 40, 10.0D) With {.GRNId = grn.GRNId}
                    context.GRNLines.Add(grnLine)
                    context.SaveChanges()

                    ' Seed Posted Sales Invoice: 40 units fulfilled @ $15.00/unit, COGS $7.50/unit
                    Dim invoice As New SalesInvoice() With {
                        .CustomerId = cust.CustomerId,
                        .DocumentNumber = "INV-2026-0001",
                        .InvoiceDate = New DateTime(2026, 11, 8),
                        .PostingDate = New DateTime(2026, 11, 8),
                        .DueDate = New DateTime(2026, 12, 8),
                        .TotalAmount = 600.0D,
                        .Status = "Posted"
                    }
                    context.SalesInvoices.Add(invoice)
                    context.SaveChanges()

                    Dim invLine As New SalesInvoiceLine() With {
                        .InvoiceId = invoice.InvoiceId,
                        .ProductId = prod.ProductId,
                        .FulfilledQty = 40,
                        .UnitPrice = 15.0D,
                        .InvoicedCOGSUnitCost = 7.5D
                    }
                    context.SalesInvoiceLines.Add(invLine)
                    context.SaveChanges()

                    ' Seed Posted Supplier Bill: Invoiced price $12.50 (PPV = ($12.50 - $10.00) * 40 = $100.00), Freight $50.00
                    Dim bill As New SupplierBill() With {
                        .VendorId = vend.VendorId,
                        .DocumentNumber = "BILL-2026-0001",
                        .BillDate = New DateTime(2026, 11, 5),
                        .PostingDate = New DateTime(2026, 11, 5),
                        .DueDate = New DateTime(2026, 12, 5),
                        .TotalAmount = 550.0D,
                        .Status = "Posted"
                    }
                    context.SupplierBills.Add(bill)
                    context.SaveChanges()

                    Dim billLine As New BillLine() With {
                        .BillId = bill.BillId,
                        .GRNLineId = grnLine.GRNLineId,
                        .ProductId = prod.ProductId,
                        .BilledQty = 40,
                        .BilledUnitPrice = 12.5D,
                        .LineTotal = 500.0D,
                        .FreightAmount = 50.0D
                    }
                    context.BillLines.Add(billLine)
                    context.SaveChanges()

                    Dim bridge = reportingSvc.GetProductProfitabilityBridge(New DateTime(2026, 11, 1), New DateTime(2026, 11, 30))

                    Assert.IsNotNull(bridge)
                    Dim row = bridge.Rows.FirstOrDefault(Function(r) r.ProductSku = "SKU-A001")
                    Assert.IsNotNull(row)

                    Assert.AreEqual(40, row.UnitsSold)
                    Assert.AreEqual(600.0D, row.InvoicedRevenue)
                    Assert.AreEqual(300.0D, row.CostOfGoodsSold)
                    Assert.AreEqual(50.0D, row.InboundFreightVariance)
                    Assert.AreEqual(100.0D, row.PurchasePriceVariance)
                    ' Net Landed Margin = 600 - 300 - 50 - 100 = 150
                    Assert.AreEqual(150.0D, row.NetMarginAmount)
                    ' Margin Percentage = 150 / 600 * 100 = 25.0%
                    Assert.AreEqual(25.0D, row.MarginPercentage)

                    Assert.AreEqual(40, bridge.TotalUnitsSold)
                    Assert.AreEqual(600.0D, bridge.TotalRevenue)
                    Assert.AreEqual(300.0D, bridge.TotalCogs)
                    Assert.AreEqual(50.0D, bridge.TotalFreightVariance)
                    Assert.AreEqual(100.0D, bridge.TotalPurchasePriceVariance)
                    Assert.AreEqual(150.0D, bridge.TotalNetMargin)
                    Assert.AreEqual(25.0D, bridge.OverallMarginPercentage)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub CompiledQueries_DirectInvocation_ExecutesSuccessfully()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    ' Test direct execution of each compiled query delegate
                    Dim accounts = ReportingService.CompiledTrialBalanceAccountsQuery(context).ToList()
                    Assert.IsNotNull(accounts)
                    Assert.IsTrue(accounts.Count >= 14)

                    Dim lines = ReportingService.CompiledJournalLinesAsOfDateQuery(context, DateTime.MaxValue).ToList()
                    Assert.IsNotNull(lines)

                    Dim dateLines = ReportingService.CompiledJournalLinesDateRangeQuery(context, DateTime.MinValue, DateTime.MaxValue).ToList()
                    Assert.IsNotNull(dateLines)

                    Dim salesLines = ReportingService.CompiledSalesInvoiceLinesQuery(context).ToList()
                    Assert.IsNotNull(salesLines)

                    Dim billLines = ReportingService.CompiledSupplierBillLinesQuery(context).ToList()
                    Assert.IsNotNull(billLines)

                    Dim products = ReportingService.CompiledActiveProductsQuery(context).ToList()
                    Assert.IsNotNull(products)
                End Using
            End Using
        End Sub

    End Class

End Namespace
