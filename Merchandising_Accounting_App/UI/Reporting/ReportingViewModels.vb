Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Text
Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Services

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
        Implements IDisposable

        Private ReadOnly _reportingService As ReportingService
        Private ReadOnly _context As ErpDbContext
        Private _disposed As Boolean = False

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

        Public Property ErrorMessage As String = String.Empty

        Public ReadOnly Property HasError As Boolean
            Get
                Return Not String.IsNullOrEmpty(ErrorMessage)
            End Get
        End Property

        Public Sub New(Optional reportingService As ReportingService = Nothing, Optional context As ErpDbContext = Nothing)
            _context = If(context, New ErpDbContext())
            _reportingService = If(reportingService, New ReportingService(_context))
            LoadReports()
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            If Not _disposed Then
                _context?.Dispose()
                _disposed = True
            End If
            GC.SuppressFinalize(Me)
        End Sub

        Public Sub LoadReports(Optional asOfDate As DateTime? = Nothing, Optional startDate As DateTime? = Nothing, Optional endDate As DateTime? = Nothing)
            ErrorMessage = String.Empty
            Try
                ' 1. Trial Balance
                Dim tbReport = _reportingService.GetTrialBalance(asOfDate)
                TrialBalanceRows.Clear()
                For Each r In tbReport.Rows
                    TrialBalanceRows.Add(New TrialBalanceRowViewModel() With {
                        .AccountCode = r.AccountCode,
                        .AccountName = r.AccountName,
                        .AccountType = r.AccountType.ToString(),
                        .DebitAmount = r.DebitAmount,
                        .CreditAmount = r.CreditAmount
                    })
                Next

                ' 2. Income Statement
                Dim isReport = _reportingService.GetIncomeStatement(startDate, endDate)
                IncomeStatementRows.Clear()
                For Each r In isReport.Rows
                    IncomeStatementRows.Add(New IncomeStatementRowViewModel() With {
                        .Category = r.Category,
                        .LineDescription = r.LineDescription,
                        .Amount = r.Amount,
                        .IsSubtotal = r.IsSubtotal,
                        .IsTotal = r.IsTotal
                    })
                Next

                ' 3. Product Profitability Bridge
                Dim pmReport = _reportingService.GetProductProfitabilityBridge(startDate, endDate)
                ProductMarginRows.Clear()
                For Each r In pmReport.Rows
                    ProductMarginRows.Add(New ProductProfitabilityRowViewModel() With {
                        .ProductSku = r.ProductSku,
                        .ProductName = r.ProductName,
                        .UnitsSold = r.UnitsSold,
                        .InvoicedRevenue = r.InvoicedRevenue,
                        .CostOfGoodsSold = r.CostOfGoodsSold,
                        .InboundFreightVariance = r.InboundFreightVariance,
                        .PurchasePriceVariance = r.PurchasePriceVariance,
                        .NetMarginAmount = r.NetMarginAmount,
                        .MarginPercentage = r.MarginPercentage
                    })
                Next

            Catch ex As Exception
                ErrorMessage = $"Reporting query error: {ex.Message}"
                TrialBalanceRows.Clear()
                IncomeStatementRows.Clear()
                ProductMarginRows.Clear()
                Throw
            End Try
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
