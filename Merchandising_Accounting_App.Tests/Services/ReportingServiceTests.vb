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
Imports Merchandising_Accounting_App.UI.Reporting

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

        <TestMethod>
        Public Sub BalanceSheet_ReconcilesNetAssetsToTotalEquity()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)
                    Dim postingDate As New DateTime(2026, 11, 15)

                    ' Post capital funding: Dr 1010 Cash $20,000 / Cr 3010 Owner's Capital $20,000
                    PostJournalEntry(context, postingDate, "Funding", "F-01", {
                        Tuple.Create("1010", 20000.0D, 0.0D, "Funding"),
                        Tuple.Create("3010", 0.0D, 20000.0D, "Capital")
                    })

                    ' Post inventory buy on credit: Dr 1300 Inventory $5,000 / Cr 2000 AP $5,000
                    PostJournalEntry(context, postingDate, "Purchase", "P-01", {
                        Tuple.Create("1300", 5000.0D, 0.0D, "Inventory buy"),
                        Tuple.Create("2000", 0.0D, 5000.0D, "Accounts payable")
                    })

                    ' Post sales with profit: Dr 1200 AR $3,000 / Cr 4000 Sales $3,000
                    '                         Dr 5000 COGS $1,800 / Cr 1300 Inventory $1,800
                    ' Net income = 3,000 - 1,800 = 1,200
                    PostJournalEntry(context, postingDate, "Sale", "S-01", {
                        Tuple.Create("1200", 3000.0D, 0.0D, "Receivable"),
                        Tuple.Create("4000", 0.0D, 3000.0D, "Sales"),
                        Tuple.Create("5000", 1800.0D, 0.0D, "COGS"),
                        Tuple.Create("1300", 0.0D, 1800.0D, "Inventory relief")
                    })

                    Dim bs = reportingSvc.GetBalanceSheet(postingDate)

                    Assert.IsNotNull(bs)
                    Assert.IsTrue(bs.IsBalanced, "Assets must equal Liabilities + Equity")
                    ' Assets: Cash 20,000 + Inventory (5,000 - 1,800 = 3,200) + AR 3,000 = 26,200
                    Assert.AreEqual(26200.0D, bs.TotalAssets)
                    ' Liabilities: AP 5,000
                    Assert.AreEqual(5000.0D, bs.TotalLiabilities)
                    ' Equity Accounts: Capital 20,000
                    Assert.AreEqual(20000.0D, bs.EquityAccountsTotal)
                    ' Net Income: 1,200
                    Assert.AreEqual(1200.0D, bs.CurrentNetIncome)
                    ' Total Equity: 21,200
                    Assert.AreEqual(21200.0D, bs.TotalEquity)
                    ' Net Assets = Total Assets - Total Liabilities = 26,200 - 5,000 = 21,200
                    Assert.AreEqual(21200.0D, bs.NetAssets)
                    ' Net Assets vs Equity Variance = 0
                    Assert.AreEqual(0.0D, bs.NetAssetsToEquityVariance)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub AccountsReceivableAging_BucketingAndPayments_AccuratelyAgesBalances()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)
                    Dim asOfDate As New DateTime(2026, 11, 20)

                    Dim cust As New Customer("CUST-AGING", "Acme Retail Co.", 50000.0D, 0D)
                    context.Customers.Add(cust)
                    context.SaveChanges()

                    ' 1. Current Invoice (DueDate in future: 2026-11-30 -> daysPastDue = -10 <= 0): $1,000
                    Dim invCurrent As New SalesInvoice() With {
                        .CustomerId = cust.CustomerId,
                        .DocumentNumber = "INV-CURR",
                        .InvoiceDate = New DateTime(2026, 11, 15),
                        .PostingDate = New DateTime(2026, 11, 15),
                        .DueDate = New DateTime(2026, 11, 30),
                        .TotalAmount = 1000.0D,
                        .Status = "Posted"
                    }
                    context.SalesInvoices.Add(invCurrent)

                    ' 2. 1-30 Days Overdue Invoice (DueDate: 2026-11-05 -> daysPastDue = 15): $2,000 with $500 partial payment -> Open $1,500
                    Dim inv1To30 As New SalesInvoice() With {
                        .CustomerId = cust.CustomerId,
                        .DocumentNumber = "INV-1TO30",
                        .InvoiceDate = New DateTime(2026, 10, 6),
                        .PostingDate = New DateTime(2026, 10, 6),
                        .DueDate = New DateTime(2026, 11, 5),
                        .TotalAmount = 2000.0D,
                        .Status = "Posted"
                    }
                    context.SalesInvoices.Add(inv1To30)
                    context.SaveChanges()

                    Dim receipt As New PaymentReceipt() With {
                        .CustomerId = cust.CustomerId,
                        .InvoiceId = inv1To30.InvoiceId,
                        .ReceiptDate = New DateTime(2026, 11, 10),
                        .PostingDate = New DateTime(2026, 11, 10),
                        .AmountReceived = 500.0D,
                        .BankAccountId = 1
                    }
                    context.PaymentReceipts.Add(receipt)

                    ' 3. 31-60 Days Overdue Invoice (DueDate: 2026-10-06 -> daysPastDue = 45): $3,000
                    Dim inv31To60 As New SalesInvoice() With {
                        .CustomerId = cust.CustomerId,
                        .DocumentNumber = "INV-31TO60",
                        .InvoiceDate = New DateTime(2026, 9, 6),
                        .PostingDate = New DateTime(2026, 9, 6),
                        .DueDate = New DateTime(2026, 10, 6),
                        .TotalAmount = 3000.0D,
                        .Status = "Posted"
                    }
                    context.SalesInvoices.Add(inv31To60)

                    ' 4. 61-90 Days Overdue Invoice (DueDate: 2026-9-06 -> daysPastDue = 75): $4,000
                    Dim inv61To90 As New SalesInvoice() With {
                        .CustomerId = cust.CustomerId,
                        .DocumentNumber = "INV-61TO90",
                        .InvoiceDate = New DateTime(2026, 8, 7),
                        .PostingDate = New DateTime(2026, 8, 7),
                        .DueDate = New DateTime(2026, 9, 6),
                        .TotalAmount = 4000.0D,
                        .Status = "Posted"
                    }
                    context.SalesInvoices.Add(inv61To90)

                    ' 5. Over 90 Days Overdue Invoice (DueDate: 2026-7-08 -> daysPastDue = 135): $5,000
                    Dim invOver90 As New SalesInvoice() With {
                        .CustomerId = cust.CustomerId,
                        .DocumentNumber = "INV-OVER90",
                        .InvoiceDate = New DateTime(2026, 6, 8),
                        .PostingDate = New DateTime(2026, 6, 8),
                        .DueDate = New DateTime(2026, 7, 8),
                        .TotalAmount = 5000.0D,
                        .Status = "Posted"
                    }
                    context.SalesInvoices.Add(invOver90)
                    context.SaveChanges()

                    Dim arReport = reportingSvc.GetAccountsReceivableAging(asOfDate)

                    Assert.IsNotNull(arReport)
                    Assert.AreEqual(1, arReport.Rows.Count)

                    Dim row = arReport.Rows(0)
                    Assert.AreEqual("CUST-AGING", row.CustomerCode)
                    Assert.AreEqual(1000.0D, row.CurrentAmount)
                    Assert.AreEqual(1500.0D, row.Days1To30)
                    Assert.AreEqual(3000.0D, row.Days31To60)
                    Assert.AreEqual(4000.0D, row.Days61To90)
                    Assert.AreEqual(5000.0D, row.DaysOver90)
                    Assert.AreEqual(14500.0D, row.TotalOutstanding)

                    ' Verify report summary totals
                    Assert.AreEqual(1000.0D, arReport.TotalCurrent)
                    Assert.AreEqual(1500.0D, arReport.TotalDays1To30)
                    Assert.AreEqual(3000.0D, arReport.TotalDays31To60)
                    Assert.AreEqual(4000.0D, arReport.TotalDays61To90)
                    Assert.AreEqual(5000.0D, arReport.TotalDaysOver90)
                    Assert.AreEqual(14500.0D, arReport.GrandTotal)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub AccountsReceivableAging_WithCustomerReturnCredit_ReducesOpenAgingBalance()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)
                    Dim asOfDate As New DateTime(2026, 11, 25)

                    Dim cust As New Customer("CUST-RET", "Return Customer", 10000.0D, 0D)
                    context.Customers.Add(cust)

                    Dim prod As New Product("SKU-RET", "Return Item", "Ea", 10.0D, 100)
                    context.Products.Add(prod)
                    context.SaveChanges()

                    ' Invoice: 10 units @ $50 = $500 (Due 2026-11-10 -> 15 days overdue on 2026-11-25)
                    Dim inv As New SalesInvoice() With {
                        .CustomerId = cust.CustomerId,
                        .DocumentNumber = "INV-RET-01",
                        .InvoiceDate = New DateTime(2026, 10, 11),
                        .PostingDate = New DateTime(2026, 10, 11),
                        .DueDate = New DateTime(2026, 11, 10),
                        .TotalAmount = 500.0D,
                        .Status = "Posted"
                    }
                    context.SalesInvoices.Add(inv)
                    context.SaveChanges()

                    ' Customer Return: 4 units returned @ $50 = $200 credit
                    Dim retDoc As New CustomerReturn() With {
                        .CustomerId = cust.CustomerId,
                        .OriginalInvoiceId = inv.InvoiceId,
                        .DocumentNumber = "RMA-001",
                        .ReturnDate = New DateTime(2026, 11, 18),
                        .PostingDate = New DateTime(2026, 11, 18),
                        .Status = "Posted"
                    }
                    context.CustomerReturns.Add(retDoc)
                    context.SaveChanges()

                    Dim retLine As New CustomerReturnLine() With {
                        .ReturnId = retDoc.ReturnId,
                        .ProductId = prod.ProductId,
                        .ReturnedQty = 4,
                        .RestockQty = 4,
                        .ScrapQty = 0,
                        .OriginalUnitCost = 10.0D,
                        .SellingUnitPrice = 50.0D,
                        .DispositionStatus = "Restock"
                    }
                    context.CustomerReturnLines.Add(retLine)
                    context.SaveChanges()

                    Dim arReport = reportingSvc.GetAccountsReceivableAging(asOfDate)

                    Assert.IsNotNull(arReport)
                    Assert.AreEqual(1, arReport.Rows.Count)
                    ' Open balance: 500 - 200 = 300 in 1-30 bucket
                    Dim row = arReport.Rows(0)
                    Assert.AreEqual(300.0D, row.Days1To30)
                    Assert.AreEqual(300.0D, row.TotalOutstanding)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub AccountsPayableAging_BucketingAndVouchers_AccuratelyAgesLiabilities()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)
                    Dim asOfDate As New DateTime(2026, 11, 20)

                    Dim vend As New Vendor("VEND-AGING", "Global Logistics Supplies", 30, 0D)
                    context.Vendors.Add(vend)
                    context.SaveChanges()

                    ' 1. Current Bill (DueDate in future: 2026-11-30): $800
                    Dim billCurrent As New SupplierBill() With {
                        .VendorId = vend.VendorId,
                        .DocumentNumber = "BILL-CURR",
                        .BillDate = New DateTime(2026, 11, 1),
                        .PostingDate = New DateTime(2026, 11, 1),
                        .DueDate = New DateTime(2026, 11, 30),
                        .TotalAmount = 800.0D,
                        .Status = "Posted"
                    }
                    context.SupplierBills.Add(billCurrent)

                    ' 2. 1-30 Days Overdue Bill (DueDate: 2026-11-05 -> 15 days past due): $1,200 with $200 voucher -> Open $1,000
                    Dim bill1To30 As New SupplierBill() With {
                        .VendorId = vend.VendorId,
                        .DocumentNumber = "BILL-1TO30",
                        .BillDate = New DateTime(2026, 10, 6),
                        .PostingDate = New DateTime(2026, 10, 6),
                        .DueDate = New DateTime(2026, 11, 5),
                        .TotalAmount = 1200.0D,
                        .Status = "Posted"
                    }
                    context.SupplierBills.Add(bill1To30)
                    context.SaveChanges()

                    Dim voucher As New PaymentVoucher() With {
                        .VendorId = vend.VendorId,
                        .VoucherType = "VendorPayment",
                        .TargetDocId = bill1To30.BillId,
                        .VoucherNumber = "VOUCH-01",
                        .PaymentDate = New DateTime(2026, 11, 8),
                        .PostingDate = New DateTime(2026, 11, 8),
                        .DisbursedAmount = 200.0D,
                        .BankAccountId = 1
                    }
                    context.PaymentVouchers.Add(voucher)

                    ' 3. 31-60 Days Overdue Bill (DueDate: 2026-10-06 -> 45 days past due): $1,500
                    Dim bill31To60 As New SupplierBill() With {
                        .VendorId = vend.VendorId,
                        .DocumentNumber = "BILL-31TO60",
                        .BillDate = New DateTime(2026, 9, 6),
                        .PostingDate = New DateTime(2026, 9, 6),
                        .DueDate = New DateTime(2026, 10, 6),
                        .TotalAmount = 1500.0D,
                        .Status = "Posted"
                    }
                    context.SupplierBills.Add(bill31To60)

                    ' 4. 61-90 Days Overdue Bill (DueDate: 2026-9-06 -> 75 days past due): $2,000
                    Dim bill61To90 As New SupplierBill() With {
                        .VendorId = vend.VendorId,
                        .DocumentNumber = "BILL-61TO90",
                        .BillDate = New DateTime(2026, 8, 7),
                        .PostingDate = New DateTime(2026, 8, 7),
                        .DueDate = New DateTime(2026, 9, 6),
                        .TotalAmount = 2000.0D,
                        .Status = "Posted"
                    }
                    context.SupplierBills.Add(bill61To90)

                    ' 5. Over 90 Days Overdue Bill (DueDate: 2026-7-08 -> 135 days past due): $2,500
                    Dim billOver90 As New SupplierBill() With {
                        .VendorId = vend.VendorId,
                        .DocumentNumber = "BILL-OVER90",
                        .BillDate = New DateTime(2026, 6, 8),
                        .PostingDate = New DateTime(2026, 6, 8),
                        .DueDate = New DateTime(2026, 7, 8),
                        .TotalAmount = 2500.0D,
                        .Status = "Posted"
                    }
                    context.SupplierBills.Add(billOver90)
                    context.SaveChanges()

                    Dim apReport = reportingSvc.GetAccountsPayableAging(asOfDate)

                    Assert.IsNotNull(apReport)
                    Assert.AreEqual(1, apReport.Rows.Count)

                    Dim row = apReport.Rows(0)
                    Assert.AreEqual("VEND-AGING", row.VendorCode)
                    Assert.AreEqual(800.0D, row.CurrentAmount)
                    Assert.AreEqual(1000.0D, row.Days1To30)
                    Assert.AreEqual(1500.0D, row.Days31To60)
                    Assert.AreEqual(2000.0D, row.Days61To90)
                    Assert.AreEqual(2500.0D, row.DaysOver90)
                    Assert.AreEqual(7800.0D, row.TotalOutstanding)

                    ' Verify summary totals
                    Assert.AreEqual(800.0D, apReport.TotalCurrent)
                    Assert.AreEqual(1000.0D, apReport.TotalDays1To30)
                    Assert.AreEqual(1500.0D, apReport.TotalDays31To60)
                    Assert.AreEqual(2000.0D, apReport.TotalDays61To90)
                    Assert.AreEqual(2500.0D, apReport.TotalDaysOver90)
                    Assert.AreEqual(7800.0D, apReport.GrandTotal)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub InventoryValuation_ReconcilesWithGlAccount1300()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)
                    Dim asOfDate As New DateTime(2026, 11, 20)

                    ' Create active products
                    Dim prodA As New Product("SKU-VAL-A", "Premium Espresso Beans", "Bag", 10.0D, 100)
                    Dim prodB As New Product("SKU-VAL-B", "Artisan Filter Roast", "Box", 20.0D, 50)
                    context.Products.AddRange(prodA, prodB)
                    context.SaveChanges()

                    ' Post balanced journal entry establishing $2,000 inventory asset balance on GL 1300
                    PostJournalEntry(context, asOfDate, "GRN", "GRN-VAL", {
                        Tuple.Create("1300", 2000.0D, 0.0D, "Inventory valuation intake"),
                        Tuple.Create("2100", 0.0D, 2000.0D, "GRNI clearing")
                    })

                    Dim report = reportingSvc.GetInventoryValuationReport(asOfDate)

                    Assert.IsNotNull(report)
                    ' Total units = 100 + 50 = 150
                    Assert.AreEqual(150, report.TotalStockQuantity)
                    ' Total subledger valuation = (100 * 10) + (50 * 20) = 2,000.00
                    Assert.AreEqual(2000.0D, report.TotalValuation)
                    ' Reconciles to GL Account 1300 balance of $2,000.00
                    Assert.AreEqual(2000.0D, report.GlAccount1300Balance)
                    Assert.AreEqual(0.0D, report.ReconciliationVariance)
                    Assert.IsTrue(report.IsReconciled)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub GrniAging_UnbilledAndPartiallyBilledReceipts_FlagsOver30Days()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)
                    Dim asOfDate As New DateTime(2026, 11, 20)

                    Dim vend As New Vendor("VEND-GRNI", "Direct Wholesale Importers", 30, 0D)
                    context.Vendors.Add(vend)

                    Dim prodA As New Product("SKU-GRNI-A", "Bulk Raw Beans", "Sack", 5.0D, 100)
                    Dim prodB As New Product("SKU-GRNI-B", "Packaging Cartons", "Pack", 8.0D, 50)
                    context.Products.AddRange(prodA, prodB)
                    context.SaveChanges()

                    Dim po As New PurchaseOrder() With {.VendorId = vend.VendorId, .DocumentNumber = "PO-GRNI", .OrderDate = New DateTime(2026, 10, 1), .TotalAmount = 900.0D}
                    context.PurchaseOrders.Add(po)
                    context.SaveChanges()

                    Dim poLineA As New POLine(prodA.ProductId, 100, 5.0D) With {.POId = po.POId}
                    Dim poLineB As New POLine(prodB.ProductId, 50, 8.0D) With {.POId = po.POId}
                    context.POLines.AddRange(poLineA, poLineB)
                    context.SaveChanges()

                    ' GRN 1: Received 10 days ago (2026-11-10 -> DaysOpen = 10 <= 30)
                    ' 100 units @ $5.00. Partially billed 40 units -> Unbilled 60 units = $300.00 accrual (Not flagged)
                    Dim grn1 As New GoodsReceiptNote() With {
                        .POId = po.POId,
                        .DocumentNumber = "GRN-RECENT",
                        .ReceiptDate = New DateTime(2026, 11, 10),
                        .PostingDate = New DateTime(2026, 11, 10),
                        .Status = "Posted"
                    }
                    context.GoodsReceiptNotes.Add(grn1)
                    context.SaveChanges()

                    Dim grnLine1 As New GRNLine(poLineA.POLineId, prodA.ProductId, 100, 5.0D) With {.GRNId = grn1.GRNId}
                    context.GRNLines.Add(grnLine1)
                    context.SaveChanges()

                    ' Bill against GRN 1 for 40 units
                    Dim bill As New SupplierBill() With {
                        .VendorId = vend.VendorId,
                        .DocumentNumber = "BILL-PARTIAL",
                        .BillDate = New DateTime(2026, 11, 15),
                        .PostingDate = New DateTime(2026, 11, 15),
                        .TotalAmount = 200.0D,
                        .Status = "Posted"
                    }
                    context.SupplierBills.Add(bill)
                    context.SaveChanges()

                    Dim billLine As New BillLine() With {
                        .BillId = bill.BillId,
                        .GRNLineId = grnLine1.GRNLineId,
                        .ProductId = prodA.ProductId,
                        .BilledQty = 40,
                        .BilledUnitPrice = 5.0D,
                        .LineTotal = 200.0D
                    }
                    context.BillLines.Add(billLine)

                    ' GRN 2: Received 45 days ago (2026-10-06 -> DaysOpen = 45 > 30)
                    ' 50 units @ $8.00. Unbilled 50 units = $400.00 accrual (FLAGGED > 30D)
                    Dim grn2 As New GoodsReceiptNote() With {
                        .POId = po.POId,
                        .DocumentNumber = "GRN-STALE",
                        .ReceiptDate = New DateTime(2026, 10, 6),
                        .PostingDate = New DateTime(2026, 10, 6),
                        .Status = "Posted"
                    }
                    context.GoodsReceiptNotes.Add(grn2)
                    context.SaveChanges()

                    Dim grnLine2 As New GRNLine(poLineB.POLineId, prodB.ProductId, 50, 8.0D) With {.GRNId = grn2.GRNId}
                    context.GRNLines.Add(grnLine2)
                    context.SaveChanges()

                    Dim report = reportingSvc.GetGrniAgingReport(asOfDate)

                    Assert.IsNotNull(report)
                    Assert.AreEqual(2, report.Rows.Count)

                    ' GRN 1: unbilled 60 units, $300 accrual, days open 10, not flagged
                    Dim row1 = report.Rows.First(Function(r) r.GrnNumber = "GRN-RECENT")
                    Assert.AreEqual(100, row1.ReceivedQty)
                    Assert.AreEqual(40, row1.BilledQty)
                    Assert.AreEqual(60, row1.UnbilledQty)
                    Assert.AreEqual(300.0D, row1.AccrualBalance)
                    Assert.AreEqual(10, row1.DaysOpen)
                    Assert.IsFalse(row1.FlaggedOver30Days)

                    ' GRN 2: unbilled 50 units, $400 accrual, days open 45, FLAGGED
                    Dim row2 = report.Rows.First(Function(r) r.GrnNumber = "GRN-STALE")
                    Assert.AreEqual(50, row2.ReceivedQty)
                    Assert.AreEqual(0, row2.BilledQty)
                    Assert.AreEqual(50, row2.UnbilledQty)
                    Assert.AreEqual(400.0D, row2.AccrualBalance)
                    Assert.AreEqual(45, row2.DaysOpen)
                    Assert.IsTrue(row2.FlaggedOver30Days)

                    ' Report totals
                    Assert.AreEqual(150, report.TotalReceivedQty)
                    Assert.AreEqual(40, report.TotalBilledQty)
                    Assert.AreEqual(110, report.TotalUnbilledQty)
                    Assert.AreEqual(700.0D, report.TotalAccrualBalance)
                    Assert.AreEqual(1, report.TotalFlaggedOver30Days)
                    Assert.AreEqual(400.0D, report.TotalFlaggedAmount)
                End Using
            End Using
        End Sub

        <TestMethod>
        Public Sub ReportingViewModel_CsvExports_ContainExpectedHeadersAndData()
            Using fixture As New TestDatabaseFixture()
                Using context As ErpDbContext = fixture.CreateContext()
                    Dim reportingSvc As New ReportingService(context)
                    Dim vm As New FinancialReportsViewModel(reportingSvc, context)

                    ' Test Balance Sheet CSV export
                    Dim bsCsv = vm.ExportBalanceSheetToCsv()
                    Assert.IsNotNull(bsCsv)
                    Assert.IsTrue(bsCsv.Contains("Category,Account Code,Account Name,Amount"))
                    Assert.IsTrue(bsCsv.Contains("Net Assets"))

                    ' Test AR Aging CSV export
                    Dim arCsv = vm.ExportArAgingToCsv()
                    Assert.IsNotNull(arCsv)
                    Assert.IsTrue(arCsv.Contains("Customer Code,Customer Name,Current,1-30 Days,31-60 Days,61-90 Days,90+ Days,Total Outstanding"))

                    ' Test AP Aging CSV export
                    Dim apCsv = vm.ExportApAgingToCsv()
                    Assert.IsNotNull(apCsv)
                    Assert.IsTrue(apCsv.Contains("Vendor Code,Vendor Name,Current,1-30 Days,31-60 Days,61-90 Days,90+ Days,Total Outstanding"))

                    ' Test Inventory Valuation CSV export
                    Dim svCsv = vm.ExportInventoryValuationToCsv()
                    Assert.IsNotNull(svCsv)
                    Assert.IsTrue(svCsv.Contains("SKU,Product Name,Category,Base Unit,Qty On Hand,Moving Avg Cost,Total Valuation"))
                    Assert.IsTrue(svCsv.Contains("GL Account 1300 Balance"))

                    ' Test GRNI Aging CSV export
                    Dim grniCsv = vm.ExportGrniAgingToCsv()
                    Assert.IsNotNull(grniCsv)
                    Assert.IsTrue(grniCsv.Contains("GRN Number,Receipt Date,Vendor,Item Description,Received Qty,Billed Qty,Unbilled Qty,PO Price,Accrual Balance,Days Open,Aging Status"))
                End Using
            End Using
        End Sub

    End Class

End Namespace
