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

        Public ReadOnly Property NetAssets As Decimal
            Get
                Return TotalAssets - TotalLiabilities
            End Get
        End Property

        Public ReadOnly Property NetAssetsToEquityVariance As Decimal
            Get
                Return Math.Abs(NetAssets - TotalEquity)
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

    ' =========================================================================
    ' Operational & Aging Report DTOs
    ' =========================================================================

    Public Class ArAgingLineItem
        Public Property CustomerId As Integer
        Public Property CustomerCode As String = String.Empty
        Public Property CustomerName As String = String.Empty
        Public Property CurrentAmount As Decimal
        Public Property Days1To30 As Decimal
        Public Property Days31To60 As Decimal
        Public Property Days61To90 As Decimal
        Public Property DaysOver90 As Decimal

        Public ReadOnly Property Current As Decimal
            Get
                Return CurrentAmount
            End Get
        End Property

        Public ReadOnly Property Over90Days As Decimal
            Get
                Return DaysOver90
            End Get
        End Property

        Public ReadOnly Property TotalOutstanding As Decimal
            Get
                Return CurrentAmount + Days1To30 + Days31To60 + Days61To90 + DaysOver90
            End Get
        End Property
    End Class

    Public Class AccountsReceivableAgingReport
        Public Property AsOfDate As DateTime
        Public Property Rows As New List(Of ArAgingLineItem)()
        Public Property TotalCurrent As Decimal
        Public Property TotalDays1To30 As Decimal
        Public Property TotalDays31To60 As Decimal
        Public Property TotalDays61To90 As Decimal
        Public Property TotalDaysOver90 As Decimal
        Public Property GrandTotal As Decimal
    End Class

    Public Class ApAgingLineItem
        Public Property VendorId As Integer
        Public Property VendorCode As String = String.Empty
        Public Property VendorName As String = String.Empty
        Public Property CurrentAmount As Decimal
        Public Property Days1To30 As Decimal
        Public Property Days31To60 As Decimal
        Public Property Days61To90 As Decimal
        Public Property DaysOver90 As Decimal

        Public ReadOnly Property Current As Decimal
            Get
                Return CurrentAmount
            End Get
        End Property

        Public ReadOnly Property Over90Days As Decimal
            Get
                Return DaysOver90
            End Get
        End Property

        Public ReadOnly Property TotalOutstanding As Decimal
            Get
                Return CurrentAmount + Days1To30 + Days31To60 + Days61To90 + DaysOver90
            End Get
        End Property
    End Class

    Public Class AccountsPayableAgingReport
        Public Property AsOfDate As DateTime
        Public Property Rows As New List(Of ApAgingLineItem)()
        Public Property TotalCurrent As Decimal
        Public Property TotalDays1To30 As Decimal
        Public Property TotalDays31To60 As Decimal
        Public Property TotalDays61To90 As Decimal
        Public Property TotalDaysOver90 As Decimal
        Public Property GrandTotal As Decimal
    End Class

    Public Class InventoryValuationLineItem
        Public Property ProductId As Integer
        Public Property Sku As String = String.Empty
        Public Property Name As String = String.Empty
        Public Property Category As String = String.Empty
        Public Property BaseUnit As String = String.Empty
        Public Property QtyOnHand As Integer
        Public Property MovingAvgCost As Decimal
        Public Property TotalValuation As Decimal
    End Class

    Public Class InventoryValuationReport
        Public Property AsOfDate As DateTime
        Public Property Rows As New List(Of InventoryValuationLineItem)()
        Public Property TotalStockQuantity As Integer
        Public Property TotalValuation As Decimal
        Public Property GlAccount1300Balance As Decimal

        Public ReadOnly Property ReconciliationVariance As Decimal
            Get
                Return TotalValuation - GlAccount1300Balance
            End Get
        End Property

        Public ReadOnly Property IsReconciled As Boolean
            Get
                Return Math.Abs(TotalValuation - GlAccount1300Balance) <= 0.02D
            End Get
        End Property
    End Class

    Public Class GrniAgingLineItem
        Public Property GrnNumber As String = String.Empty
        Public Property GrnDate As DateTime
        Public Property Vendor As String = String.Empty
        Public Property Item As String = String.Empty
        Public Property ReceivedQty As Integer
        Public Property BilledQty As Integer
        Public Property UnbilledQty As Integer
        Public Property PoPrice As Decimal
        Public Property AccrualBalance As Decimal
        Public Property DaysOpen As Integer
        Public Property FlaggedOver30Days As Boolean

        Public Property VendorName As String
            Get
                Return Vendor
            End Get
            Set(value As String)
                Vendor = value
            End Set
        End Property

        Public Property ItemDescription As String
            Get
                Return Item
            End Get
            Set(value As String)
                Item = value
            End Set
        End Property
    End Class

    Public Class GrniAgingReport
        Public Property AsOfDate As DateTime
        Public Property Rows As New List(Of GrniAgingLineItem)()
        Public Property TotalReceivedQty As Integer
        Public Property TotalBilledQty As Integer
        Public Property TotalUnbilledQty As Integer
        Public Property TotalAccrualBalance As Decimal
        Public Property TotalFlaggedOver30Days As Integer
        Public Property TotalFlaggedAmount As Decimal
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

        ' -------------------------------------------------------------------------
        ' 5. Accounts Receivable Aging Report
        ' -------------------------------------------------------------------------

        ''' <summary>
        ''' Generates the Accounts Receivable Aging report as of a specified cutoff date.
        ''' Buckets open customer balances into: Current, 1-30, 31-60, 61-90, and 90+ days past due.
        ''' </summary>
        Public Function GetAccountsReceivableAging(Optional asOfDate As DateTime? = Nothing) As AccountsReceivableAgingReport
            Dim cutoffDate As DateTime = If(asOfDate.HasValue, asOfDate.Value, DateTime.MaxValue)
            Dim report As New AccountsReceivableAgingReport() With {
                .AsOfDate = cutoffDate
            }

            Dim customers = _context.Customers.OrderBy(Function(c) c.Code).ToList()
            Dim invoices = _context.SalesInvoices _
                .Where(Function(i) i.PostingDate <= cutoffDate AndAlso i.Status <> "Draft" AndAlso i.Status <> "Cancelled") _
                .ToList()

            Dim invoiceIds = invoices.Select(Function(i) i.InvoiceId).ToList()

            Dim receipts = _context.PaymentReceipts _
                .Where(Function(r) r.PostingDate <= cutoffDate) _
                .ToList()

            Dim receiptsByInvoice = receipts _
                .Where(Function(r) r.InvoiceId.HasValue AndAlso invoiceIds.Contains(r.InvoiceId.Value)) _
                .GroupBy(Function(r) r.InvoiceId.Value) _
                .ToDictionary(Function(g) g.Key, Function(g) g.Sum(Function(r) r.AmountReceived))

            ' Fetch customer returns posted up to cutoff date
            Dim returns = _context.CustomerReturns _
                .Include(Function(cr) cr.Lines) _
                .Where(Function(cr) cr.PostingDate <= cutoffDate AndAlso cr.Status = "Posted") _
                .ToList()

            Dim returnCreditsByInvoice As New Dictionary(Of Integer, Decimal)()
            For Each ret In returns
                Dim allowance As Decimal = 0D
                For Each line In ret.Lines
                    Dim qty As Integer = line.RestockQty + line.ScrapQty
                    If qty > 0 Then
                        allowance += Math.Round(CDec(qty) * line.SellingUnitPrice, 4, MidpointRounding.AwayFromZero)
                    End If
                Next
                If returnCreditsByInvoice.ContainsKey(ret.OriginalInvoiceId) Then
                    returnCreditsByInvoice(ret.OriginalInvoiceId) += allowance
                Else
                    returnCreditsByInvoice(ret.OriginalInvoiceId) = allowance
                End If
            Next

            Dim invoicesByCustomer = invoices.GroupBy(Function(i) i.CustomerId).ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            For Each cust In customers
                Dim currentAmt As Decimal = 0D
                Dim days1To30Amt As Decimal = 0D
                Dim days31To60Amt As Decimal = 0D
                Dim days61To90Amt As Decimal = 0D
                Dim daysOver90Amt As Decimal = 0D

                If invoicesByCustomer.ContainsKey(cust.CustomerId) Then
                    For Each inv In invoicesByCustomer(cust.CustomerId)
                        Dim paidAmt As Decimal = 0D
                        If receiptsByInvoice.ContainsKey(inv.InvoiceId) Then
                            paidAmt = receiptsByInvoice(inv.InvoiceId)
                        End If

                        Dim creditAmt As Decimal = 0D
                        If returnCreditsByInvoice.ContainsKey(inv.InvoiceId) Then
                            creditAmt = returnCreditsByInvoice(inv.InvoiceId)
                        End If

                        Dim openAmt As Decimal = inv.TotalAmount - paidAmt - creditAmt
                        If openAmt > 0.0001D Then
                            Dim effectiveDueDate As DateTime = If(inv.DueDate <> DateTime.MinValue, inv.DueDate, inv.PostingDate.AddDays(30))
                            Dim daysPastDue As Integer = (cutoffDate.Date - effectiveDueDate.Date).Days

                            If daysPastDue <= 0 Then
                                currentAmt += openAmt
                            ElseIf daysPastDue <= 30 Then
                                days1To30Amt += openAmt
                            ElseIf daysPastDue <= 60 Then
                                days31To60Amt += openAmt
                            ElseIf daysPastDue <= 90 Then
                                days61To90Amt += openAmt
                            Else
                                daysOver90Amt += openAmt
                            End If
                        End If
                    Next
                End If

                Dim totalCustOutstanding As Decimal = currentAmt + days1To30Amt + days31To60Amt + days61To90Amt + daysOver90Amt
                If totalCustOutstanding <> 0D Then
                    Dim row As New ArAgingLineItem() With {
                        .CustomerId = cust.CustomerId,
                        .CustomerCode = cust.Code,
                        .CustomerName = cust.Name,
                        .CurrentAmount = currentAmt,
                        .Days1To30 = days1To30Amt,
                        .Days31To60 = days31To60Amt,
                        .Days61To90 = days61To90Amt,
                        .DaysOver90 = daysOver90Amt
                    }
                    report.Rows.Add(row)
                    report.TotalCurrent += currentAmt
                    report.TotalDays1To30 += days1To30Amt
                    report.TotalDays31To60 += days31To60Amt
                    report.TotalDays61To90 += days61To90Amt
                    report.TotalDaysOver90 += daysOver90Amt
                    report.GrandTotal += totalCustOutstanding
                End If
            Next

            Return report
        End Function

        ' -------------------------------------------------------------------------
        ' 6. Accounts Payable Aging Report
        ' -------------------------------------------------------------------------

        ''' <summary>
        ''' Generates the Accounts Payable Aging report as of a specified cutoff date.
        ''' Buckets open supplier obligations into: Current, 1-30, 31-60, 61-90, and 90+ days past due.
        ''' </summary>
        Public Function GetAccountsPayableAging(Optional asOfDate As DateTime? = Nothing) As AccountsPayableAgingReport
            Dim cutoffDate As DateTime = If(asOfDate.HasValue, asOfDate.Value, DateTime.MaxValue)
            Dim report As New AccountsPayableAgingReport() With {
                .AsOfDate = cutoffDate
            }

            Dim vendors = _context.Vendors.OrderBy(Function(v) v.Code).ToList()
            Dim bills = _context.SupplierBills _
                .Where(Function(b) b.PostingDate <= cutoffDate AndAlso b.Status <> "Draft" AndAlso b.Status <> "Cancelled") _
                .ToList()

            Dim billIds = bills.Select(Function(b) b.BillId).ToList()

            Dim vouchers = _context.PaymentVouchers _
                .Where(Function(v) v.PostingDate <= cutoffDate AndAlso v.VoucherType = "VendorPayment" AndAlso billIds.Contains(v.TargetDocId)) _
                .GroupBy(Function(v) v.TargetDocId) _
                .ToDictionary(Function(g) g.Key, Function(g) g.Sum(Function(v) v.DisbursedAmount))

            Dim billsByVendor = bills.GroupBy(Function(b) b.VendorId).ToDictionary(Function(g) g.Key, Function(g) g.ToList())

            For Each vend In vendors
                Dim currentAmt As Decimal = 0D
                Dim days1To30Amt As Decimal = 0D
                Dim days31To60Amt As Decimal = 0D
                Dim days61To90Amt As Decimal = 0D
                Dim daysOver90Amt As Decimal = 0D

                If billsByVendor.ContainsKey(vend.VendorId) Then
                    For Each bill In billsByVendor(vend.VendorId)
                        Dim disbursedAmt As Decimal = 0D
                        If vouchers.ContainsKey(bill.BillId) Then
                            disbursedAmt = vouchers(bill.BillId)
                        End If

                        Dim openAmt As Decimal = bill.TotalAmount - disbursedAmt
                        If openAmt > 0.0001D Then
                            Dim termDays As Integer = If(vend.PaymentTermsDays > 0, vend.PaymentTermsDays, 30)
                            Dim effectiveDueDate As DateTime = If(bill.DueDate <> DateTime.MinValue, bill.DueDate, bill.PostingDate.AddDays(termDays))
                            Dim daysPastDue As Integer = (cutoffDate.Date - effectiveDueDate.Date).Days

                            If daysPastDue <= 0 Then
                                currentAmt += openAmt
                            ElseIf daysPastDue <= 30 Then
                                days1To30Amt += openAmt
                            ElseIf daysPastDue <= 60 Then
                                days31To60Amt += openAmt
                            ElseIf daysPastDue <= 90 Then
                                days61To90Amt += openAmt
                            Else
                                daysOver90Amt += openAmt
                            End If
                        End If
                    Next
                End If

                Dim totalVendOutstanding As Decimal = currentAmt + days1To30Amt + days31To60Amt + days61To90Amt + daysOver90Amt
                If totalVendOutstanding <> 0D Then
                    Dim row As New ApAgingLineItem() With {
                        .VendorId = vend.VendorId,
                        .VendorCode = vend.Code,
                        .VendorName = vend.Name,
                        .CurrentAmount = currentAmt,
                        .Days1To30 = days1To30Amt,
                        .Days31To60 = days31To60Amt,
                        .Days61To90 = days61To90Amt,
                        .DaysOver90 = daysOver90Amt
                    }
                    report.Rows.Add(row)
                    report.TotalCurrent += currentAmt
                    report.TotalDays1To30 += days1To30Amt
                    report.TotalDays31To60 += days31To60Amt
                    report.TotalDays61To90 += days61To90Amt
                    report.TotalDaysOver90 += daysOver90Amt
                    report.GrandTotal += totalVendOutstanding
                End If
            Next

            Return report
        End Function

        ' -------------------------------------------------------------------------
        ' 7. Inventory Stock Valuation Report (Reconciling to GL Account 1300)
        ' -------------------------------------------------------------------------

        ''' <summary>
        ''' Generates the perpetual moving average inventory valuation report.
        ''' Evaluates SKU-level on-hand quantities, moving average costs, and reconciles total valuation against GL Account 1300.
        ''' </summary>
        Public Function GetInventoryValuationReport(Optional asOfDate As DateTime? = Nothing) As InventoryValuationReport
            Dim cutoffDate As DateTime = If(asOfDate.HasValue, asOfDate.Value, DateTime.MaxValue)
            Dim report As New InventoryValuationReport() With {
                .AsOfDate = cutoffDate
            }

            Dim products = _context.Products.Where(Function(p) p.IsActive).OrderBy(Function(p) p.SKU).ToList()

            Dim transactions = _context.InventoryTransactions _
                .Where(Function(t) t.PostingDate <= cutoffDate) _
                .OrderBy(Function(t) t.TransactionId) _
                .ToList()

            Dim txByProduct = transactions.GroupBy(Function(t) t.ProductId).ToDictionary(Function(g) g.Key, Function(g) g.Last())

            For Each prod In products
                Dim qty As Integer
                Dim cost As Decimal

                If txByProduct.ContainsKey(prod.ProductId) Then
                    Dim lastTx = txByProduct(prod.ProductId)
                    qty = lastTx.RunningQty
                    cost = lastTx.RunningUnitCost
                Else
                    qty = prod.TotalQtyOnHand
                    cost = prod.CurrentUnitCost
                End If

                Dim val As Decimal = Math.Round(CDec(qty) * cost, 4, MidpointRounding.AwayFromZero)

                Dim row As New InventoryValuationLineItem() With {
                    .ProductId = prod.ProductId,
                    .Sku = prod.SKU,
                    .Name = prod.Name,
                    .Category = If(String.IsNullOrEmpty(prod.PackUnitName), "Merchandise", prod.PackUnitName),
                    .BaseUnit = prod.BaseUnitName,
                    .QtyOnHand = qty,
                    .MovingAvgCost = cost,
                    .TotalValuation = val
                }

                report.Rows.Add(row)
                report.TotalStockQuantity += qty
                report.TotalValuation += val
            Next

            ' Calculate GL Account 1300 balance as of cutoff date
            Dim acct1300 = _context.Accounts.FirstOrDefault(Function(a) a.Code = "1300")
            If acct1300 IsNot Nothing Then
                Dim lines1300 = _context.JournalEntryLines _
                    .Where(Function(l) l.AccountId = acct1300.AccountId AndAlso l.JournalEntry.PostingDate <= cutoffDate) _
                    .ToList()

                Dim debits As Decimal = lines1300.Sum(Function(l) l.DebitAmount)
                Dim credits As Decimal = lines1300.Sum(Function(l) l.CreditAmount)
                report.GlAccount1300Balance = debits - credits
            End If

            Return report
        End Function

        ' -------------------------------------------------------------------------
        ' 8. Goods Received Not Invoiced (GRNI) Aging Report
        ' -------------------------------------------------------------------------

        ''' <summary>
        ''' Generates the Goods Received Not Invoiced (GRNI) aging inquiry report.
        ''' Tracks unbilled purchase dock receipts valued at PO price and flags items unbilled past 30 days.
        ''' </summary>
        Public Function GetGrniAgingReport(Optional asOfDate As DateTime? = Nothing) As GrniAgingReport
            Dim cutoffDate As DateTime = If(asOfDate.HasValue, asOfDate.Value, DateTime.MaxValue)
            Dim report As New GrniAgingReport() With {
                .AsOfDate = cutoffDate
            }

            Dim grnLines = _context.GRNLines _
                .Include(Function(gl) gl.GoodsReceiptNote) _
                    .ThenInclude(Function(grn) grn.PurchaseOrder) _
                        .ThenInclude(Function(po) po.Vendor) _
                .Include(Function(gl) gl.Product) _
                .Where(Function(gl) gl.GoodsReceiptNote.PostingDate <= cutoffDate AndAlso gl.GoodsReceiptNote.Status = "Posted") _
                .OrderBy(Function(gl) gl.GoodsReceiptNote.DocumentNumber) _
                .ThenBy(Function(gl) gl.GRNLineId) _
                .ToList()

            Dim grnLineIds = grnLines.Select(Function(gl) gl.GRNLineId).ToList()
            Dim billLines = _context.BillLines _
                .Include(Function(bl) bl.SupplierBill) _
                .Where(Function(bl) bl.GRNLineId.HasValue AndAlso grnLineIds.Contains(bl.GRNLineId.Value) AndAlso bl.SupplierBill.PostingDate <= cutoffDate AndAlso bl.SupplierBill.Status <> "Cancelled") _
                .ToList()

            Dim billedByGrnLine = billLines _
                .GroupBy(Function(bl) bl.GRNLineId.Value) _
                .ToDictionary(Function(g) g.Key, Function(g) g.Sum(Function(bl) bl.BilledQty))

            For Each gl In grnLines
                Dim billedQty As Integer = 0
                If billedByGrnLine.ContainsKey(gl.GRNLineId) Then
                    billedQty = billedByGrnLine(gl.GRNLineId)
                End If

                Dim unbilledQty As Integer = gl.ReceivedQty - billedQty
                If unbilledQty > 0 Then
                    Dim accrual As Decimal = Math.Round(CDec(unbilledQty) * gl.POUnitPrice, 4, MidpointRounding.AwayFromZero)
                    Dim receiptDate As DateTime = gl.GoodsReceiptNote.ReceiptDate
                    Dim daysOpen As Integer = Math.Max(0, (cutoffDate.Date - receiptDate.Date).Days)
                    Dim isOver30 As Boolean = (daysOpen > 30)

                    Dim vendName As String = "Unknown Vendor"
                    If gl.GoodsReceiptNote.PurchaseOrder IsNot Nothing AndAlso gl.GoodsReceiptNote.PurchaseOrder.Vendor IsNot Nothing Then
                        vendName = gl.GoodsReceiptNote.PurchaseOrder.Vendor.Name
                    End If

                    Dim itemDesc As String = If(gl.Product IsNot Nothing, $"{gl.Product.SKU} - {gl.Product.Name}", $"Product #{gl.ProductId}")

                    Dim row As New GrniAgingLineItem() With {
                        .GrnNumber = gl.GoodsReceiptNote.DocumentNumber,
                        .GrnDate = receiptDate,
                        .Vendor = vendName,
                        .Item = itemDesc,
                        .ReceivedQty = gl.ReceivedQty,
                        .BilledQty = billedQty,
                        .UnbilledQty = unbilledQty,
                        .PoPrice = gl.POUnitPrice,
                        .AccrualBalance = accrual,
                        .DaysOpen = daysOpen,
                        .FlaggedOver30Days = isOver30
                    }

                    report.Rows.Add(row)
                    report.TotalReceivedQty += gl.ReceivedQty
                    report.TotalBilledQty += billedQty
                    report.TotalUnbilledQty += unbilledQty
                    report.TotalAccrualBalance += accrual

                    If isOver30 Then
                        report.TotalFlaggedOver30Days += 1
                        report.TotalFlaggedAmount += accrual
                    End If
                End If
            Next

            Return report
        End Function

    End Class

End Namespace
