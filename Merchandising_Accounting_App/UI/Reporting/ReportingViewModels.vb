Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Collections.Generic
Imports System.Text

Namespace UI.Reporting

    Public Class TrialBalanceRowViewModel
        Public Property AccountCode As String = String.Empty
        Public Property AccountName As String = String.Empty
        Public Property AccountType As String = String.Empty
        Public Property DebitAmount As Decimal
        Public Property CreditAmount As Decimal

        Public ReadOnly Property NormalBalance As String
            Get
                If AccountType.StartsWith("Revenue", StringComparison.Ordinal) OrElse
                   AccountType.Equals("Liability", StringComparison.Ordinal) OrElse
                   AccountType.Equals("Equity", StringComparison.Ordinal) Then
                    Return "Credit"
                Else
                    Return "Debit"
                End If
            End Get
        End Property

        Public ReadOnly Property FormattedDebit As String
            Get
                If DebitAmount > 0D Then
                    Return DebitAmount.ToString("C2")
                Else
                    Return "-"
                End If
            End Get
        End Property

        Public ReadOnly Property FormattedCredit As String
            Get
                If CreditAmount > 0D Then
                    Return CreditAmount.ToString("C2")
                Else
                    Return "-"
                End If
            End Get
        End Property
    End Class

    Public Class IncomeStatementRowViewModel
        Public Property Category As String = String.Empty
        Public Property LineDescription As String = String.Empty
        Public Property Amount As Decimal
        Public Property IsSubtotal As Boolean
        Public Property IsTotal As Boolean

        Public ReadOnly Property FormattedAmount As String
            Get
                If Amount < 0D Then
                    Return $"({Math.Abs(Amount):C2})"
                Else
                    Return Amount.ToString("C2")
                End If
            End Get
        End Property
    End Class

    Public Class ProductProfitabilityRowViewModel
        Public Property ProductSku As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property UnitsSold As Integer
        Public Property InvoicedRevenue As Decimal
        Public Property CostOfGoodsSold As Decimal
        Public Property InboundFreightVariance As Decimal
        Public Property PurchasePriceVariance As Decimal
        Public Property NetMarginAmount As Decimal
        Public Property MarginPercentage As Decimal

        Public ReadOnly Property FormattedRevenue As String
            Get
                Return InvoicedRevenue.ToString("C2")
            End Get
        End Property

        Public ReadOnly Property FormattedCogs As String
            Get
                Return CostOfGoodsSold.ToString("C2")
            End Get
        End Property

        Public ReadOnly Property FormattedFreight As String
            Get
                Return InboundFreightVariance.ToString("C2")
            End Get
        End Property

        Public ReadOnly Property FormattedPpv As String
            Get
                Return PurchasePriceVariance.ToString("C2")
            End Get
        End Property

        Public ReadOnly Property FormattedMargin As String
            Get
                Return NetMarginAmount.ToString("C2")
            End Get
        End Property

        Public ReadOnly Property FormattedPercentage As String
            Get
                Return $"{MarginPercentage:F2}%"
            End Get
        End Property
    End Class

    Public Class FinancialReportsViewModel
        Public Property TrialBalanceRows As New List(Of TrialBalanceRowViewModel)()
        Public Property IncomeStatementRows As New List(Of IncomeStatementRowViewModel)()
        Public Property ProductMarginRows As New List(Of ProductProfitabilityRowViewModel)()

        Public ReadOnly Property TotalTrialBalanceDebits As Decimal
            Get
                Dim sum As Decimal = 0D
                For Each r In TrialBalanceRows
                    sum += r.DebitAmount
                Next
                Return sum
            End Get
        End Property

        Public ReadOnly Property TotalTrialBalanceCredits As Decimal
            Get
                Dim sum As Decimal = 0D
                For Each r In TrialBalanceRows
                    sum += r.CreditAmount
                Next
                Return sum
            End Get
        End Property

        Public ReadOnly Property IsTrialBalanceBalanced As Boolean
            Get
                Return Math.Round(TotalTrialBalanceDebits, 4) = Math.Round(TotalTrialBalanceCredits, 4)
            End Get
        End Property

        Public Sub New()
            LoadMockData()
        End Sub

        Private Sub LoadMockData()
            ' Trial Balance mock dataset - strictly balanced to 4 decimal places
            TrialBalanceRows = New List(Of TrialBalanceRowViewModel) From {
                New TrialBalanceRowViewModel() With {.AccountCode = "1010", .AccountName = "Cash on Hand", .AccountType = "Asset", .DebitAmount = 2500.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "1020", .AccountName = "Operating Bank Account", .AccountType = "Asset", .DebitAmount = 43300.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "1200", .AccountName = "Accounts Receivable", .AccountType = "Asset", .DebitAmount = 0.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "1300", .AccountName = "Merchandise Inventory", .AccountType = "Asset", .DebitAmount = 680.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "2000", .AccountName = "Accounts Payable", .AccountType = "Liability", .DebitAmount = 0.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "2100", .AccountName = "Goods Received Not Invoiced (GRNI)", .AccountType = "Liability", .DebitAmount = 0.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "3000", .AccountName = "Retained Earnings", .AccountType = "Equity", .DebitAmount = 0.0D, .CreditAmount = 42500.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "3010", .AccountName = "Owner's Capital", .AccountType = "Equity", .DebitAmount = 0.0D, .CreditAmount = 4000.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "4000", .AccountName = "Sales Revenue", .AccountType = "Revenue", .DebitAmount = 0.0D, .CreditAmount = 600.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "4100", .AccountName = "Sales Returns & Allowances", .AccountType = "Revenue (Contra)", .DebitAmount = 150.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "5000", .AccountName = "Cost of Goods Sold (COGS)", .AccountType = "Expense", .DebitAmount = 300.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "5100", .AccountName = "Purchase Price Variance (PPV)", .AccountType = "Expense", .DebitAmount = 100.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "5200", .AccountName = "Inbound Freight Variance", .AccountType = "Expense", .DebitAmount = 50.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "5300", .AccountName = "Inventory Shrinkage & Damaged Stock", .AccountType = "Expense", .DebitAmount = 20.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "5400", .AccountName = "Stocktake Revaluation Variance", .AccountType = "Expense", .DebitAmount = 0.0D, .CreditAmount = 0.0D},
                New TrialBalanceRowViewModel() With {.AccountCode = "5500", .AccountName = "Cash Rounding & Roundoff Variance", .AccountType = "Expense", .DebitAmount = 0.0D, .CreditAmount = 0.0D}
            }

            ' Profit & Loss / Income Statement dataset
            IncomeStatementRows = New List(Of IncomeStatementRowViewModel) From {
                New IncomeStatementRowViewModel() With {.Category = "Operating Revenue", .LineDescription = "4000 Gross Sales Revenue", .Amount = 600.0D, .IsSubtotal = False, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Operating Revenue", .LineDescription = "4100 Less: Sales Returns & Allowances", .Amount = -150.0D, .IsSubtotal = False, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Operating Revenue", .LineDescription = "NET SALES REVENUE", .Amount = 450.0D, .IsSubtotal = True, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Cost of Sales", .LineDescription = "5000 Cost of Goods Sold (COGS)", .Amount = 300.0D, .IsSubtotal = False, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Cost of Sales", .LineDescription = "GROSS PROFIT MARGIN", .Amount = 150.0D, .IsSubtotal = True, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Operating Variances & Expenses", .LineDescription = "5100 Purchase Price Variance (PPV)", .Amount = 100.0D, .IsSubtotal = False, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Operating Variances & Expenses", .LineDescription = "5200 Inbound Freight Variance", .Amount = 50.0D, .IsSubtotal = False, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Operating Variances & Expenses", .LineDescription = "5300 Inventory Shrinkage & Damaged Stock", .Amount = 20.0D, .IsSubtotal = False, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Operating Variances & Expenses", .LineDescription = "5400 Stocktake Revaluation Variance", .Amount = 0.0D, .IsSubtotal = False, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Operating Variances & Expenses", .LineDescription = "5500 Cash Rounding & Roundoff Variance", .Amount = 0.0D, .IsSubtotal = False, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Operating Variances & Expenses", .LineDescription = "TOTAL OPERATING VARIANCES", .Amount = 170.0D, .IsSubtotal = True, .IsTotal = False},
                New IncomeStatementRowViewModel() With {.Category = "Net Income", .LineDescription = "NET OPERATING INCOME / (LOSS)", .Amount = -20.0D, .IsSubtotal = False, .IsTotal = True}
            }

            ' Product Margins & Profitability dataset (Managerial analytical view)
            ProductMarginRows = New List(Of ProductProfitabilityRowViewModel) From {
                New ProductProfitabilityRowViewModel() With {
                    .ProductSku = "SKU-A001",
                    .ProductName = "Organic Arabica Coffee Beans 1kg",
                    .UnitsSold = 40,
                    .InvoicedRevenue = 600.0D,
                    .CostOfGoodsSold = 300.0D,
                    .InboundFreightVariance = 50.0D,
                    .PurchasePriceVariance = 100.0D,
                    .NetMarginAmount = 150.0D,
                    .MarginPercentage = 25.0D
                },
                New ProductProfitabilityRowViewModel() With {
                    .ProductSku = "SKU-B002",
                    .ProductName = "Artisan Ceramic Dripper V60",
                    .UnitsSold = 25,
                    .InvoicedRevenue = 750.0D,
                    .CostOfGoodsSold = 375.0D,
                    .InboundFreightVariance = 25.0D,
                    .PurchasePriceVariance = 0.0D,
                    .NetMarginAmount = 350.0D,
                    .MarginPercentage = 46.67D
                },
                New ProductProfitabilityRowViewModel() With {
                    .ProductSku = "SKU-C003",
                    .ProductName = "Unbleached Cone Coffee Filters (100pk)",
                    .UnitsSold = 60,
                    .InvoicedRevenue = 360.0D,
                    .CostOfGoodsSold = 180.0D,
                    .InboundFreightVariance = 15.0D,
                    .PurchasePriceVariance = 0.0D,
                    .NetMarginAmount = 165.0D,
                    .MarginPercentage = 45.83D
                }
            }
        End Sub

        Public Function ExportTrialBalanceToCsv() As String
            Dim sb As New StringBuilder()
            sb.AppendLine("Account Code,Account Name,Account Type,Normal,Debit,Credit")
            For Each row In TrialBalanceRows
                sb.AppendLine($"""{row.AccountCode}"",""{row.AccountName}"",""{row.AccountType}"",""{row.NormalBalance}"",{row.DebitAmount:F2},{row.CreditAmount:F2}")
            Next
            sb.AppendLine($"""TOTALS"","""","""","""",{TotalTrialBalanceDebits:F2},{TotalTrialBalanceCredits:F2}")
            Return sb.ToString()
        End Function

        Public Function ExportIncomeStatementToCsv() As String
            Dim sb As New StringBuilder()
            sb.AppendLine("Category,Line Description,Amount")
            For Each row In IncomeStatementRows
                sb.AppendLine($"""{row.Category}"",""{row.LineDescription}"",{row.Amount:F2}")
            Next
            Return sb.ToString()
        End Function

        Public Function ExportProductMarginsToCsv() As String
            Dim sb As New StringBuilder()
            sb.AppendLine("SKU,Product Name,Units Sold,Invoiced Revenue,COGS,Inbound Freight,PPV,Net Landed Margin,Margin %")
            For Each row In ProductMarginRows
                sb.AppendLine($"""{row.ProductSku}"",""{row.ProductName}"",{row.UnitsSold},{row.InvoicedRevenue:F2},{row.CostOfGoodsSold:F2},{row.InboundFreightVariance:F2},{row.PurchasePriceVariance:F2},{row.NetMarginAmount:F2},{row.MarginPercentage:F2}%")
            Next
            Return sb.ToString()
        End Function
    End Class

End Namespace
