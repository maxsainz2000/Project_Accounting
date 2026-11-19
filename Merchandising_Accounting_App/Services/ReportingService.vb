Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Linq
Imports Microsoft.EntityFrameworkCore
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Domain
Imports Merchandising_Accounting_App.Domain.Models

Namespace Services

    ' =========================================================================
    ' Report DTO Models
    ' =========================================================================

    Public Class TrialBalanceRow
        Public Property AccountId As Integer
        Public Property AccountCode As String = String.Empty
        Public Property AccountName As String = String.Empty
        Public Property AccountType As AccountType
        Public Property NormalBalance As String = String.Empty
        Public Property DebitAmount As Decimal
        Public Property CreditAmount As Decimal
    End Class

    Public Class TrialBalanceReport
        Public Property AsOfDate As DateTime
        Public Property Rows As New List(Of TrialBalanceRow)()
        Public Property TotalDebits As Decimal
        Public Property TotalCredits As Decimal

        Public ReadOnly Property IsBalanced As Boolean
            Get
                Return Math.Round(TotalDebits, 4) = Math.Round(TotalCredits, 4)
            End Get
        End Property

        Public ReadOnly Property ImbalanceVariance As Decimal
            Get
                Return Math.Abs(TotalDebits - TotalCredits)
            End Get
        End Property
    End Class

    Public Class BalanceSheetRow
        Public Property AccountId As Integer
        Public Property AccountCode As String = String.Empty
        Public Property AccountName As String = String.Empty
        Public Property Amount As Decimal
    End Class

    Public Class BalanceSheetReport
        Public Property AsOfDate As DateTime
        Public Property AssetRows As New List(Of BalanceSheetRow)()
        Public Property LiabilityRows As New List(Of BalanceSheetRow)()
        Public Property EquityRows As New List(Of BalanceSheetRow)()
        Public Property TotalAssets As Decimal
        Public Property TotalLiabilities As Decimal
        Public Property EquityAccountsTotal As Decimal
        Public Property CurrentNetIncome As Decimal
        Public Property TotalEquity As Decimal
        Public Property TotalLiabilitiesAndEquity As Decimal

        Public ReadOnly Property IsBalanced As Boolean
            Get
                Return Math.Round(TotalAssets, 4) = Math.Round(TotalLiabilitiesAndEquity, 4)
            End Get
        End Property

        Public ReadOnly Property ImbalanceVariance As Decimal
            Get
                Return Math.Abs(TotalAssets - TotalLiabilitiesAndEquity)
            End Get
        End Property
    End Class

    Public Class IncomeStatementRow
        Public Property Category As String = String.Empty
        Public Property AccountCode As String = String.Empty
        Public Property LineDescription As String = String.Empty
        Public Property Amount As Decimal
        Public Property IsSubtotal As Boolean
        Public Property IsTotal As Boolean
    End Class

    Public Class IncomeStatementReport
        Public Property StartDate As DateTime
        Public Property EndDate As DateTime
        Public Property Rows As New List(Of IncomeStatementRow)()
        Public Property GrossSalesRevenue As Decimal
        Public Property SalesReturnsAndAllowances As Decimal
        Public Property NetSalesRevenue As Decimal
        Public Property CostOfGoodsSold As Decimal
        Public Property GrossProfitMargin As Decimal
        Public Property TotalOperatingExpenses As Decimal
        Public Property NetOperatingIncome As Decimal
    End Class

    Public Class ProductProfitabilityRow
        Public Property ProductId As Integer
        Public Property ProductSku As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property UnitsSold As Integer
        Public Property InvoicedRevenue As Decimal
        Public Property CostOfGoodsSold As Decimal
        Public Property InboundFreightVariance As Decimal
        Public Property PurchasePriceVariance As Decimal
        Public Property NetMarginAmount As Decimal
        Public Property MarginPercentage As Decimal
    End Class

    Public Class ProductProfitabilityReport
        Public Property StartDate As DateTime
        Public Property EndDate As DateTime
        Public Property Rows As New List(Of ProductProfitabilityRow)()
        Public Property TotalUnitsSold As Integer
        Public Property TotalRevenue As Decimal
        Public Property TotalCogs As Decimal
        Public Property TotalFreightVariance As Decimal
        Public Property TotalPurchasePriceVariance As Decimal
        Public Property TotalNetMargin As Decimal

        Public ReadOnly Property OverallMarginPercentage As Decimal
            Get
                If TotalRevenue > 0D Then
                    Return Math.Round((TotalNetMargin / TotalRevenue) * 100D, 2)
                Else
                    Return 0D
                End If
            End Get
        End Property
    End Class

    Public Class AccountDto
        Public Property AccountId As Integer
        Public Property Code As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property AccountType As AccountType
        Public Property IsActive As Boolean
    End Class

    Public Class ProductDto
        Public Property ProductId As Integer
        Public Property SKU As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property CurrentUnitCost As Decimal
        Public Property TotalQtyOnHand As Integer
        Public Property IsActive As Boolean
    End Class

    Public Class JournalLinePostingDto
        Public Property AccountId As Integer
        Public Property DebitAmount As Decimal
        Public Property CreditAmount As Decimal
    End Class

    Public Class ProductSalesLineDto
        Public Property ProductId As Integer
        Public Property FulfilledQty As Integer
        Public Property UnitPrice As Decimal
        Public Property InvoicedCOGSUnitCost As Decimal
        Public Property PostingDate As DateTime
    End Class

    Public Class ProductBillLineDto
        Public Property ProductId As Integer
        Public Property BilledQty As Integer
        Public Property BilledUnitPrice As Decimal
        Public Property POUnitPrice As Decimal
        Public Property FreightAmount As Decimal
        Public Property PostingDate As DateTime
    End Class

    ' =========================================================================
    ' Reporting Service Implementation
    ' =========================================================================

    ''' <summary>
    ''' Financial reporting engine providing compiled queries and statutory statement generation
    ''' for Trial Balance, Balance Sheet, Income Statement, and Product Margin & Profitability Bridge.
    ''' </summary>
    Public Class ReportingService

        Private ReadOnly _context As ErpDbContext

        ' -------------------------------------------------------------------------
        ' EF Core Compiled Queries
        ' -------------------------------------------------------------------------

        ''' <summary>
        ''' Compiled query: Active accounts for Trial Balance.
        ''' </summary>
        Public Shared ReadOnly CompiledTrialBalanceAccountsQuery As Func(Of ErpDbContext, IEnumerable(Of AccountDto)) =
            EF.CompileQuery(Function(ctx As ErpDbContext) _
                ctx.Accounts _
                    .Where(Function(a) a.IsActive) _
                    .Select(Function(a) New AccountDto With {
                        .AccountId = a.AccountId,
                        .Code = a.Code,
                        .Name = a.Name,
                        .AccountType = a.AccountType,
                        .IsActive = a.IsActive
                    })
            )

        ''' <summary>
        ''' Compiled query: Journal entry lines posted on or before a specified statutory cutoff date.
        ''' </summary>
        Public Shared ReadOnly CompiledJournalLinesAsOfDateQuery As Func(Of ErpDbContext, DateTime, IEnumerable(Of JournalLinePostingDto)) =
            EF.CompileQuery(Function(ctx As ErpDbContext, asOfDate As DateTime) _
                ctx.JournalEntryLines _
                    .Where(Function(l) l.JournalEntry.PostingDate <= asOfDate) _
                    .Select(Function(l) New JournalLinePostingDto With {
                        .AccountId = l.AccountId,
                        .DebitAmount = l.DebitAmount,
                        .CreditAmount = l.CreditAmount
                    })
            )

        ''' <summary>
        ''' Compiled query: Journal entry lines posted within an inclusive date range for Income Statement reporting.
        ''' </summary>
        Public Shared ReadOnly CompiledJournalLinesDateRangeQuery As Func(Of ErpDbContext, DateTime, DateTime, IEnumerable(Of JournalLinePostingDto)) =
            EF.CompileQuery(Function(ctx As ErpDbContext, startDate As DateTime, endDate As DateTime) _
                ctx.JournalEntryLines _
                    .Where(Function(l) l.JournalEntry.PostingDate >= startDate AndAlso l.JournalEntry.PostingDate <= endDate) _
                    .Select(Function(l) New JournalLinePostingDto With {
                        .AccountId = l.AccountId,
                        .DebitAmount = l.DebitAmount,
                        .CreditAmount = l.CreditAmount
                    })
            )

        ''' <summary>
        ''' Compiled query: Sales invoice lines fulfilled from posted sales invoices for Product Margin analytics.
        ''' </summary>
        Public Shared ReadOnly CompiledSalesInvoiceLinesQuery As Func(Of ErpDbContext, IEnumerable(Of ProductSalesLineDto)) =
            EF.CompileQuery(Function(ctx As ErpDbContext) _
                ctx.SalesInvoiceLines _
                    .Where(Function(sl) sl.SalesInvoice.Status = "Posted") _
                    .Select(Function(sl) New ProductSalesLineDto With {
                        .ProductId = sl.ProductId,
                        .FulfilledQty = sl.FulfilledQty,
                        .UnitPrice = sl.UnitPrice,
                        .InvoicedCOGSUnitCost = sl.InvoicedCOGSUnitCost,
                        .PostingDate = sl.SalesInvoice.PostingDate
                    })
            )

        ''' <summary>
        ''' Compiled query: Supplier bill lines from posted supplier bills for Landed Freight & PPV variance allocation.
        ''' </summary>
        Public Shared ReadOnly CompiledSupplierBillLinesQuery As Func(Of ErpDbContext, IEnumerable(Of ProductBillLineDto)) =
            EF.CompileQuery(Function(ctx As ErpDbContext) _
                ctx.BillLines _
                    .Where(Function(bl) bl.SupplierBill.Status = "Posted" AndAlso bl.ProductId.HasValue) _
                    .Select(Function(bl) New ProductBillLineDto With {
                        .ProductId = bl.ProductId.Value,
                        .BilledQty = bl.BilledQty,
                        .BilledUnitPrice = bl.BilledUnitPrice,
                        .POUnitPrice = If(bl.GRNLine IsNot Nothing, bl.GRNLine.POUnitPrice, 0D),
                        .FreightAmount = bl.FreightAmount,
                        .PostingDate = bl.SupplierBill.PostingDate
                    })
            )

        ''' <summary>
        ''' Compiled query: Active products catalog for profitability bridge analysis.
        ''' </summary>
        Public Shared ReadOnly CompiledActiveProductsQuery As Func(Of ErpDbContext, IEnumerable(Of ProductDto)) =
            EF.CompileQuery(Function(ctx As ErpDbContext) _
                ctx.Products _
                    .Where(Function(p) p.IsActive) _
                    .Select(Function(p) New ProductDto With {
                        .ProductId = p.ProductId,
                        .SKU = p.SKU,
                        .Name = p.Name,
                        .CurrentUnitCost = p.CurrentUnitCost,
                        .TotalQtyOnHand = p.TotalQtyOnHand,
                        .IsActive = p.IsActive
                    })
            )

        ' -------------------------------------------------------------------------
        ' Constructors
        ' -------------------------------------------------------------------------

        Public Sub New(context As ErpDbContext)
            If context Is Nothing Then
                Throw New ArgumentNullException(NameOf(context))
            End If
            _context = context
        End Sub

        Public Sub New()
            Me.New(New ErpDbContext())
        End Sub

        ' -------------------------------------------------------------------------
        ' 1. Trial Balance Report (Sum Debits == Sum Credits)
        ' -------------------------------------------------------------------------

        ''' <summary>
        ''' Generates the General Ledger Trial Balance report as of a statutory cutoff date.
        ''' Enforces the fundamental accounting invariant: Sum of Debits == Sum of Credits to 4 decimal places.
        ''' </summary>
        ''' <param name="asOfDate">Optional statutory cutoff date; defaults to maximum date if omitted.</param>
        ''' <returns>The balanced <see cref="TrialBalanceReport"/>.</returns>
        Public Function GetTrialBalance(Optional asOfDate As DateTime? = Nothing) As TrialBalanceReport
            Dim cutoffDate As DateTime = If(asOfDate.HasValue, asOfDate.Value, DateTime.MaxValue)
            Dim report As New TrialBalanceReport() With {
                .AsOfDate = cutoffDate
            }

            ' Execute compiled query to retrieve journal lines up to the cutoff date
            Dim lines As List(Of JournalLinePostingDto) = CompiledJournalLinesAsOfDateQuery(_context, cutoffDate).ToList()
            Dim accounts As List(Of Account) = _context.Accounts.Where(Function(a) a.IsActive).OrderBy(Function(a) a.Code).ToList()

            Dim linesByAccount = lines.GroupBy(Function(l) l.AccountId).ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            For Each account In accounts
                Dim totalDebits As Decimal = 0D
                Dim totalCredits As Decimal = 0D

                If linesByAccount.ContainsKey(account.AccountId) Then
                    For Each line In linesByAccount(account.AccountId)
                        totalDebits += line.DebitAmount
                        totalCredits += line.CreditAmount
                    Next
                End If

                ' Calculate net balance
                Dim netBalance As Decimal = totalDebits - totalCredits
                Dim debitAmt As Decimal = 0D
                Dim creditAmt As Decimal = 0D

                If netBalance > 0D Then
                    debitAmt = netBalance
                ElseIf netBalance < 0D Then
                    creditAmt = Math.Abs(netBalance)
                End If

                Dim normalBal As String
                If account.AccountType = AccountType.Liability OrElse
                   account.AccountType = AccountType.Equity OrElse
                   account.AccountType = AccountType.Revenue Then
                    normalBal = "Credit"
                Else
                    normalBal = "Debit"
                End If

                ' Special case: 4100 Sales Returns & Allowances is Revenue Contra with Normal Debit
                If account.Code = "4100" Then
                    normalBal = "Debit"
                End If

                Dim row As New TrialBalanceRow() With {
                    .AccountId = account.AccountId,
                    .AccountCode = account.Code,
                    .AccountName = account.Name,
                    .AccountType = account.AccountType,
                    .NormalBalance = normalBal,
                    .DebitAmount = debitAmt,
                    .CreditAmount = creditAmt
                }

                report.Rows.Add(row)
                report.TotalDebits += debitAmt
                report.TotalCredits += creditAmt
            Next

            Return report
        End Function

        ' -------------------------------------------------------------------------
        ' 2. Balance Sheet Report (Assets = Liabilities + Equity)
        ' -------------------------------------------------------------------------

        ''' <summary>
        ''' Generates the statutory Balance Sheet report as of the specified cutoff date.
        ''' Preserves the core accounting equation: Assets = Liabilities + Equity (including current period Net Income).
        ''' </summary>
        ''' <param name="asOfDate">Optional statutory cutoff date; defaults to maximum date if omitted.</param>
        ''' <returns>The balanced <see cref="BalanceSheetReport"/>.</returns>
        Public Function GetBalanceSheet(Optional asOfDate As DateTime? = Nothing) As BalanceSheetReport
            Dim cutoffDate As DateTime = If(asOfDate.HasValue, asOfDate.Value, DateTime.MaxValue)
            Dim report As New BalanceSheetReport() With {
                .AsOfDate = cutoffDate
            }

            Dim lines As List(Of JournalLinePostingDto) = CompiledJournalLinesAsOfDateQuery(_context, cutoffDate).ToList()
            Dim accounts As List(Of Account) = _context.Accounts.Where(Function(a) a.IsActive).OrderBy(Function(a) a.Code).ToList()

            Dim linesByAccount = lines.GroupBy(Function(l) l.AccountId).ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            Dim totalAssetSum As Decimal = 0D
            Dim totalLiabilitySum As Decimal = 0D
            Dim equityAccountsSum As Decimal = 0D

            Dim cumulativeRevenue As Decimal = 0D
            Dim cumulativeExpenses As Decimal = 0D

            For Each account In accounts
                Dim totalDebits As Decimal = 0D
                Dim totalCredits As Decimal = 0D

                If linesByAccount.ContainsKey(account.AccountId) Then
                    For Each line In linesByAccount(account.AccountId)
                        totalDebits += line.DebitAmount
                        totalCredits += line.CreditAmount
                    Next
                End If

                Select Case account.AccountType
                    Case AccountType.Asset
                        ' Assets have normal debit balance: Balance = Debits - Credits
                        Dim balance As Decimal = totalDebits - totalCredits
                        report.AssetRows.Add(New BalanceSheetRow() With {
                            .AccountId = account.AccountId,
                            .AccountCode = account.Code,
                            .AccountName = account.Name,
                            .Amount = balance
                        })
                        totalAssetSum += balance

                    Case AccountType.Liability
                        ' Liabilities have normal credit balance: Balance = Credits - Debits
                        Dim balance As Decimal = totalCredits - totalDebits
                        report.LiabilityRows.Add(New BalanceSheetRow() With {
                            .AccountId = account.AccountId,
                            .AccountCode = account.Code,
                            .AccountName = account.Name,
                            .Amount = balance
                        })
                        totalLiabilitySum += balance

                    Case AccountType.Equity
                        ' Equity accounts have normal credit balance: Balance = Credits - Debits
                        Dim balance As Decimal = totalCredits - totalDebits
                        report.EquityRows.Add(New BalanceSheetRow() With {
                            .AccountId = account.AccountId,
                            .AccountCode = account.Code,
                            .AccountName = account.Name,
                            .Amount = balance
                        })
                        equityAccountsSum += balance

                    Case AccountType.Revenue
                        If account.Code = "4100" Then
                            ' Contra revenue (Sales Returns & Allowances): debit reduces revenue
                            cumulativeRevenue -= (totalDebits - totalCredits)
                        Else
                            ' Standard revenue: credits - debits
                            cumulativeRevenue += (totalCredits - totalDebits)
                        End If

                    Case AccountType.Expense
                        ' Expenses have normal debit balance: debits - credits
                        cumulativeExpenses += (totalDebits - totalCredits)
                End Select
            Next

            ' Calculate Current Net Income: Net Revenue - Total Expenses
            Dim currentNetIncome As Decimal = cumulativeRevenue - cumulativeExpenses

            report.TotalAssets = totalAssetSum
            report.TotalLiabilities = totalLiabilitySum
            report.EquityAccountsTotal = equityAccountsSum
            report.CurrentNetIncome = currentNetIncome
            report.TotalEquity = equityAccountsSum + currentNetIncome
            report.TotalLiabilitiesAndEquity = totalLiabilitySum + report.TotalEquity

            Return report
        End Function

        ' -------------------------------------------------------------------------
        ' 3. Income Statement / Profit & Loss Report
        ' -------------------------------------------------------------------------

        ''' <summary>
        ''' Generates the statutory Income Statement (Profit & Loss) for a specified date range.
        ''' Breaks down Gross Sales, Returns, Net Sales, Cost of Goods Sold, Gross Profit, and Operating Variances.
        ''' </summary>
        Public Function GetIncomeStatement(Optional startDate As DateTime? = Nothing, Optional endDate As DateTime? = Nothing) As IncomeStatementReport
            Dim startRange As DateTime = If(startDate.HasValue, startDate.Value, DateTime.MinValue)
            Dim endRange As DateTime = If(endDate.HasValue, endDate.Value, DateTime.MaxValue)

            Dim report As New IncomeStatementReport() With {
                .StartDate = startRange,
                .EndDate = endRange
            }

            Dim lines As List(Of JournalLinePostingDto) = CompiledJournalLinesDateRangeQuery(_context, startRange, endRange).ToList()
            Dim accounts As List(Of Account) = _context.Accounts.Where(Function(a) a.IsActive).OrderBy(Function(a) a.Code).ToList()

            Dim linesByAccount = lines.GroupBy(Function(l) l.AccountId).ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            Dim grossSales As Decimal = 0D
            Dim salesReturns As Decimal = 0D
            Dim cogs As Decimal = 0D
            Dim totalVariances As Decimal = 0D

            Dim operatingVarianceRows As New List(Of IncomeStatementRow)()

            For Each account In accounts
                Dim totalDebits As Decimal = 0D
                Dim totalCredits As Decimal = 0D

                If linesByAccount.ContainsKey(account.AccountId) Then
                    For Each line In linesByAccount(account.AccountId)
                        totalDebits += line.DebitAmount
                        totalCredits += line.CreditAmount
                    Next
                End If

                Select Case account.Code
                    Case "4000"
                        grossSales = (totalCredits - totalDebits)
                    Case "4100"
                        salesReturns = (totalDebits - totalCredits)
                    Case "5000"
                        cogs = (totalDebits - totalCredits)
                    Case "5100", "5200", "5300", "5400", "5500"
                        Dim varExpense As Decimal = (totalDebits - totalCredits)
                        operatingVarianceRows.Add(New IncomeStatementRow() With {
                            .Category = "Operating Variances & Expenses",
                            .AccountCode = account.Code,
                            .LineDescription = $"{account.Code} {account.Name}",
                            .Amount = varExpense
                        })
                        totalVariances += varExpense
                End Select
            Next

            Dim netSales As Decimal = grossSales - salesReturns
            Dim grossProfit As Decimal = netSales - cogs
            Dim netIncome As Decimal = grossProfit - totalVariances

            report.GrossSalesRevenue = grossSales
            report.SalesReturnsAndAllowances = salesReturns
            report.NetSalesRevenue = netSales
            report.CostOfGoodsSold = cogs
            report.GrossProfitMargin = grossProfit
            report.TotalOperatingExpenses = totalVariances
            report.NetOperatingIncome = netIncome

            ' Populate structured statement rows
            report.Rows.Add(New IncomeStatementRow() With {.Category = "Operating Revenue", .AccountCode = "4000", .LineDescription = "4000 Gross Sales Revenue", .Amount = grossSales})
            report.Rows.Add(New IncomeStatementRow() With {.Category = "Operating Revenue", .AccountCode = "4100", .LineDescription = "4100 Less: Sales Returns & Allowances", .Amount = -salesReturns})
            report.Rows.Add(New IncomeStatementRow() With {.Category = "Operating Revenue", .LineDescription = "NET SALES REVENUE", .Amount = netSales, .IsSubtotal = True})

            report.Rows.Add(New IncomeStatementRow() With {.Category = "Cost of Sales", .AccountCode = "5000", .LineDescription = "5000 Cost of Goods Sold (COGS)", .Amount = cogs})
            report.Rows.Add(New IncomeStatementRow() With {.Category = "Cost of Sales", .LineDescription = "GROSS PROFIT MARGIN", .Amount = grossProfit, .IsSubtotal = True})

            For Each vRow In operatingVarianceRows
                report.Rows.Add(vRow)
            Next

            report.Rows.Add(New IncomeStatementRow() With {.Category = "Operating Variances & Expenses", .LineDescription = "TOTAL OPERATING VARIANCES", .Amount = totalVariances, .IsSubtotal = True})
            report.Rows.Add(New IncomeStatementRow() With {.Category = "Net Income", .LineDescription = "NET OPERATING INCOME / (LOSS)", .Amount = netIncome, .IsTotal = True})

            Return report
        End Function

        ' -------------------------------------------------------------------------
        ' 4. Product Margin & Profitability Bridge
        ' -------------------------------------------------------------------------

        ''' <summary>
        ''' Generates the managerial Product Margin & Profitability Bridge.
        ''' Reconciles SKU-level Invoiced Revenue, COGS, Inbound Freight Variances, and PPV variances to compute Net Landed Margin.
        ''' </summary>
        Public Function GetProductProfitabilityBridge(Optional startDate As DateTime? = Nothing, Optional endDate As DateTime? = Nothing) As ProductProfitabilityReport
            Dim startRange As DateTime = If(startDate.HasValue, startDate.Value, DateTime.MinValue)
            Dim endRange As DateTime = If(endDate.HasValue, endDate.Value, DateTime.MaxValue)

            Dim report As New ProductProfitabilityReport() With {
                .StartDate = startRange,
                .EndDate = endRange
            }

            Dim products = CompiledActiveProductsQuery(_context).OrderBy(Function(p) p.SKU).ToList()
            Dim salesLines = CompiledSalesInvoiceLinesQuery(_context) _
                .Where(Function(sl) sl.PostingDate >= startRange AndAlso sl.PostingDate <= endRange) _
                .ToList()

            Dim billLines = CompiledSupplierBillLinesQuery(_context) _
                .Where(Function(bl) bl.PostingDate >= startRange AndAlso bl.PostingDate <= endRange) _
                .ToList()

            Dim salesByProduct = salesLines.GroupBy(Function(sl) sl.ProductId).ToDictionary(Function(g) g.Key, Function(g) g.ToList())
            Dim billByProduct = billLines.GroupBy(Function(bl) bl.ProductId).ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            For Each prod In products
                Dim unitsSold As Integer = 0
                Dim invoicedRevenue As Decimal = 0D
                Dim cogsAmount As Decimal = 0D
                Dim freightAmount As Decimal = 0D
                Dim ppvAmount As Decimal = 0D

                If salesByProduct.ContainsKey(prod.ProductId) Then
                    For Each sl In salesByProduct(prod.ProductId)
                        unitsSold += sl.FulfilledQty
                        invoicedRevenue += (sl.FulfilledQty * sl.UnitPrice)
                        cogsAmount += (sl.FulfilledQty * sl.InvoicedCOGSUnitCost)
                    Next
                End If

                If billByProduct.ContainsKey(prod.ProductId) Then
                    For Each bl In billByProduct(prod.ProductId)
                        freightAmount += bl.FreightAmount
                        If bl.POUnitPrice > 0D Then
                            Dim unitDiff As Decimal = (bl.BilledUnitPrice - bl.POUnitPrice)
                            ppvAmount += (unitDiff * bl.BilledQty)
                        End If
                    Next
                End If

                Dim netMargin As Decimal = invoicedRevenue - cogsAmount - freightAmount - ppvAmount
                Dim marginPct As Decimal = 0D
                If invoicedRevenue > 0D Then
                    marginPct = Math.Round((netMargin / invoicedRevenue) * 100D, 2)
                End If

                Dim row As New ProductProfitabilityRow() With {
                    .ProductId = prod.ProductId,
                    .ProductSku = prod.SKU,
                    .ProductName = prod.Name,
                    .UnitsSold = unitsSold,
                    .InvoicedRevenue = invoicedRevenue,
                    .CostOfGoodsSold = cogsAmount,
                    .InboundFreightVariance = freightAmount,
                    .PurchasePriceVariance = ppvAmount,
                    .NetMarginAmount = netMargin,
                    .MarginPercentage = marginPct
                }

                report.Rows.Add(row)
                report.TotalUnitsSold += unitsSold
                report.TotalRevenue += invoicedRevenue
                report.TotalCogs += cogsAmount
                report.TotalFreightVariance += freightAmount
                report.TotalPurchasePriceVariance += ppvAmount
                report.TotalNetMargin += netMargin
            Next

            Return report
        End Function

    End Class

End Namespace
