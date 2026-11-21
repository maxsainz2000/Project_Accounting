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

    Public Class BalanceSheetRowViewModel
        Public Property Category As String = String.Empty
        Public Property AccountCode As String = String.Empty
        Public Property AccountName As String = String.Empty
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

    Public Class ArAgingRowViewModel
        Public Property CustomerCode As String = String.Empty
        Public Property CustomerName As String = String.Empty
        Public Property CurrentAmount As Decimal
        Public Property Days1To30 As Decimal
        Public Property Days31To60 As Decimal
        Public Property Days61To90 As Decimal
        Public Property DaysOver90 As Decimal
        Public Property TotalOutstanding As Decimal

        Public ReadOnly Property FormattedCurrent As String
            Get
                Return If(CurrentAmount <> 0D, CurrentAmount.ToString("C2"), "-")
            End Get
        End Property

        Public ReadOnly Property FormattedDays1To30 As String
            Get
                Return If(Days1To30 <> 0D, Days1To30.ToString("C2"), "-")
            End Get
        End Property

        Public ReadOnly Property FormattedDays31To60 As String
            Get
                Return If(Days31To60 <> 0D, Days31To60.ToString("C2"), "-")
            End Get
        End Property

        Public ReadOnly Property FormattedDays61To90 As String
            Get
                Return If(Days61To90 <> 0D, Days61To90.ToString("C2"), "-")
            End Get
        End Property

        Public ReadOnly Property FormattedDaysOver90 As String
            Get
                Return If(DaysOver90 <> 0D, DaysOver90.ToString("C2"), "-")
            End Get
        End Property

        Public ReadOnly Property FormattedTotal As String
            Get
                Return TotalOutstanding.ToString("C2")
            End Get
        End Property
    End Class

    Public Class ApAgingRowViewModel
        Public Property VendorCode As String = String.Empty
        Public Property VendorName As String = String.Empty
        Public Property CurrentAmount As Decimal
        Public Property Days1To30 As Decimal
        Public Property Days31To60 As Decimal
        Public Property Days61To90 As Decimal
        Public Property DaysOver90 As Decimal
        Public Property TotalOutstanding As Decimal

        Public ReadOnly Property FormattedCurrent As String
            Get
                Return If(CurrentAmount <> 0D, CurrentAmount.ToString("C2"), "-")
            End Get
        End Property

        Public ReadOnly Property FormattedDays1To30 As String
            Get
                Return If(Days1To30 <> 0D, Days1To30.ToString("C2"), "-")
            End Get
        End Property

        Public ReadOnly Property FormattedDays31To60 As String
            Get
                Return If(Days31To60 <> 0D, Days31To60.ToString("C2"), "-")
            End Get
        End Property

        Public ReadOnly Property FormattedDays61To90 As String
            Get
                Return If(Days61To90 <> 0D, Days61To90.ToString("C2"), "-")
            End Get
        End Property

        Public ReadOnly Property FormattedDaysOver90 As String
            Get
                Return If(DaysOver90 <> 0D, DaysOver90.ToString("C2"), "-")
            End Get
        End Property

        Public ReadOnly Property FormattedTotal As String
            Get
                Return TotalOutstanding.ToString("C2")
            End Get
        End Property
    End Class

    Public Class InventoryValuationRowViewModel
        Public Property Sku As String = String.Empty
        Public Property ProductName As String = String.Empty
        Public Property Category As String = String.Empty
        Public Property BaseUnit As String = String.Empty
        Public Property QtyOnHand As Integer
        Public Property MovingAvgCost As Decimal
        Public Property TotalValuation As Decimal

        Public ReadOnly Property FormattedQty As String
            Get
                Return QtyOnHand.ToString("N0")
            End Get
        End Property

        Public ReadOnly Property FormattedCost As String
            Get
                Return MovingAvgCost.ToString("C4")
            End Get
        End Property

        Public ReadOnly Property FormattedValuation As String
            Get
                Return TotalValuation.ToString("C2")
            End Get
        End Property
    End Class

    Public Class GrniAgingRowViewModel
        Public Property GrnNumber As String = String.Empty
        Public Property GrnDate As DateTime
        Public Property VendorName As String = String.Empty
        Public Property ItemDescription As String = String.Empty
        Public Property ReceivedQty As Integer
        Public Property BilledQty As Integer
        Public Property UnbilledQty As Integer
        Public Property PoPrice As Decimal
        Public Property AccrualBalance As Decimal
        Public Property DaysOpen As Integer
        Public Property FlaggedOver30Days As Boolean

        Public ReadOnly Property FormattedDate As String
            Get
                Return GrnDate.ToString("yyyy-MM-dd")
            End Get
        End Property

        Public ReadOnly Property FormattedPoPrice As String
            Get
                Return PoPrice.ToString("C2")
            End Get
        End Property

        Public ReadOnly Property FormattedAccrual As String
            Get
                Return AccrualBalance.ToString("C2")
            End Get
        End Property

        Public ReadOnly Property StatusText As String
            Get
                If FlaggedOver30Days Then
                    Return "FLAGGED (>30D)"
                Else
                    Return "Current (<30D)"
                End If
            End Get
        End Property
    End Class

    Public Class FinancialReportsViewModel
        Implements IDisposable

        Private ReadOnly _reportingService As ReportingService
        Private ReadOnly _context As ErpDbContext
        Private _disposed As Boolean = False

        Public Property TrialBalanceRows As New List(Of TrialBalanceRowViewModel)()
        Public Property BalanceSheetRows As New List(Of BalanceSheetRowViewModel)()
        Public Property BalanceSheetReport As BalanceSheetReport
        Public Property IncomeStatementRows As New List(Of IncomeStatementRowViewModel)()
        Public Property ProductMarginRows As New List(Of ProductProfitabilityRowViewModel)()
        Public Property ArAgingRows As New List(Of ArAgingRowViewModel)()
        Public Property ArAgingReport As AccountsReceivableAgingReport
        Public Property ApAgingRows As New List(Of ApAgingRowViewModel)()
        Public Property ApAgingReport As AccountsPayableAgingReport
        Public Property StockValuationRows As New List(Of InventoryValuationRowViewModel)()
        Public Property StockValuationReport As InventoryValuationReport
        Public Property GrniAgingRows As New List(Of GrniAgingRowViewModel)()
        Public Property GrniAgingReport As GrniAgingReport

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

                ' 2. Balance Sheet
                Dim bsReport = _reportingService.GetBalanceSheet(asOfDate)
                BalanceSheetReport = bsReport
                BalanceSheetRows.Clear()

                ' Assets Section
                For Each r In bsReport.AssetRows
                    BalanceSheetRows.Add(New BalanceSheetRowViewModel() With {
                        .Category = "Assets",
                        .AccountCode = r.AccountCode,
                        .AccountName = r.AccountName,
                        .Amount = r.Amount
                    })
                Next
                BalanceSheetRows.Add(New BalanceSheetRowViewModel() With {
                    .Category = "Assets",
                    .AccountName = "TOTAL ASSETS",
                    .Amount = bsReport.TotalAssets,
                    .IsSubtotal = True
                })

                ' Liabilities Section
                For Each r In bsReport.LiabilityRows
                    BalanceSheetRows.Add(New BalanceSheetRowViewModel() With {
                        .Category = "Liabilities",
                        .AccountCode = r.AccountCode,
                        .AccountName = r.AccountName,
                        .Amount = r.Amount
                    })
                Next
                BalanceSheetRows.Add(New BalanceSheetRowViewModel() With {
                    .Category = "Liabilities",
                    .AccountName = "TOTAL LIABILITIES",
                    .Amount = bsReport.TotalLiabilities,
                    .IsSubtotal = True
                })

                ' Equity Section
                For Each r In bsReport.EquityRows
                    BalanceSheetRows.Add(New BalanceSheetRowViewModel() With {
                        .Category = "Equity",
                        .AccountCode = r.AccountCode,
                        .AccountName = r.AccountName,
                        .Amount = r.Amount
                    })
                Next
                BalanceSheetRows.Add(New BalanceSheetRowViewModel() With {
                    .Category = "Equity",
                    .AccountName = "Current Period Net Income / (Loss)",
                    .Amount = bsReport.CurrentNetIncome
                })
                BalanceSheetRows.Add(New BalanceSheetRowViewModel() With {
                    .Category = "Equity",
                    .AccountName = "TOTAL EQUITY",
                    .Amount = bsReport.TotalEquity,
                    .IsSubtotal = True
                })
                BalanceSheetRows.Add(New BalanceSheetRowViewModel() With {
                    .Category = "Reconciliation",
                    .AccountName = "TOTAL LIABILITIES & EQUITY",
                    .Amount = bsReport.TotalLiabilitiesAndEquity,
                    .IsTotal = True
                })

                ' 3. Income Statement
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

                ' 4. Product Profitability Bridge
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

                ' 5. Accounts Receivable Aging
                Dim arReport = _reportingService.GetAccountsReceivableAging(asOfDate)
                ArAgingReport = arReport
                ArAgingRows.Clear()
                For Each r In arReport.Rows
                    ArAgingRows.Add(New ArAgingRowViewModel() With {
                        .CustomerCode = r.CustomerCode,
                        .CustomerName = r.CustomerName,
                        .CurrentAmount = r.CurrentAmount,
                        .Days1To30 = r.Days1To30,
                        .Days31To60 = r.Days31To60,
                        .Days61To90 = r.Days61To90,
                        .DaysOver90 = r.DaysOver90,
                        .TotalOutstanding = r.TotalOutstanding
                    })
                Next

                ' 6. Accounts Payable Aging
                Dim apReport = _reportingService.GetAccountsPayableAging(asOfDate)
                ApAgingReport = apReport
                ApAgingRows.Clear()
                For Each r In apReport.Rows
                    ApAgingRows.Add(New ApAgingRowViewModel() With {
                        .VendorCode = r.VendorCode,
                        .VendorName = r.VendorName,
                        .CurrentAmount = r.CurrentAmount,
                        .Days1To30 = r.Days1To30,
                        .Days31To60 = r.Days31To60,
                        .Days61To90 = r.Days61To90,
                        .DaysOver90 = r.DaysOver90,
                        .TotalOutstanding = r.TotalOutstanding
                    })
                Next

                ' 7. Inventory Stock Valuation
                Dim svReport = _reportingService.GetInventoryValuationReport(asOfDate)
                StockValuationReport = svReport
                StockValuationRows.Clear()
                For Each r In svReport.Rows
                    StockValuationRows.Add(New InventoryValuationRowViewModel() With {
                        .Sku = r.Sku,
                        .ProductName = r.Name,
                        .Category = r.Category,
                        .BaseUnit = r.BaseUnit,
                        .QtyOnHand = r.QtyOnHand,
                        .MovingAvgCost = r.MovingAvgCost,
                        .TotalValuation = r.TotalValuation
                    })
                Next

                ' 8. GRNI Aging
                Dim grniReport = _reportingService.GetGrniAgingReport(asOfDate)
                GrniAgingReport = grniReport
                GrniAgingRows.Clear()
                For Each r In grniReport.Rows
                    GrniAgingRows.Add(New GrniAgingRowViewModel() With {
                        .GrnNumber = r.GrnNumber,
                        .GrnDate = r.GrnDate,
                        .VendorName = r.Vendor,
                        .ItemDescription = r.Item,
                        .ReceivedQty = r.ReceivedQty,
                        .BilledQty = r.BilledQty,
                        .UnbilledQty = r.UnbilledQty,
                        .PoPrice = r.PoPrice,
                        .AccrualBalance = r.AccrualBalance,
                        .DaysOpen = r.DaysOpen,
                        .FlaggedOver30Days = r.FlaggedOver30Days
                    })
                Next

            Catch ex As Exception
                ErrorMessage = $"Reporting query error: {ex.Message}"
                TrialBalanceRows.Clear()
                BalanceSheetRows.Clear()
                IncomeStatementRows.Clear()
                ProductMarginRows.Clear()
                ArAgingRows.Clear()
                ApAgingRows.Clear()
                StockValuationRows.Clear()
                GrniAgingRows.Clear()
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

        Public Function ExportBalanceSheetToCsv() As String
            Dim sb As New StringBuilder()
            Dim q As Char = ChrW(34)
            sb.AppendLine("Category,Account Code,Account Name,Amount")
            For Each row In BalanceSheetRows
                sb.AppendLine($"{q}{row.Category}{q},{q}{row.AccountCode}{q},{q}{row.AccountName}{q},{row.Amount:F2}")
            Next
            If BalanceSheetReport IsNot Nothing Then
                sb.AppendLine($"{q}Reconciliation{q},,{q}Net Assets (Assets - Liab){q},{BalanceSheetReport.NetAssets:F2}")
                sb.AppendLine($"{q}Reconciliation{q},,{q}Total Equity{q},{BalanceSheetReport.TotalEquity:F2}")
                sb.AppendLine($"{q}Reconciliation{q},,{q}Net Assets vs Equity Variance{q},{BalanceSheetReport.NetAssetsToEquityVariance:F4}")
            End If
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

        Public Function ExportArAgingToCsv() As String
            Dim sb As New StringBuilder()
            Dim q As Char = ChrW(34)
            sb.AppendLine("Customer Code,Customer Name,Current,1-30 Days,31-60 Days,61-90 Days,90+ Days,Total Outstanding")
            For Each row In ArAgingRows
                sb.AppendLine($"{q}{row.CustomerCode}{q},{q}{row.CustomerName}{q},{row.CurrentAmount:F2},{row.Days1To30:F2},{row.Days31To60:F2},{row.Days61To90:F2},{row.DaysOver90:F2},{row.TotalOutstanding:F2}")
            Next
            If ArAgingReport IsNot Nothing Then
                sb.AppendLine($"{q}TOTALS{q},,{ArAgingReport.TotalCurrent:F2},{ArAgingReport.TotalDays1To30:F2},{ArAgingReport.TotalDays31To60:F2},{ArAgingReport.TotalDays61To90:F2},{ArAgingReport.TotalDaysOver90:F2},{ArAgingReport.GrandTotal:F2}")
            End If
            Return sb.ToString()
        End Function

        Public Function ExportApAgingToCsv() As String
            Dim sb As New StringBuilder()
            Dim q As Char = ChrW(34)
            sb.AppendLine("Vendor Code,Vendor Name,Current,1-30 Days,31-60 Days,61-90 Days,90+ Days,Total Outstanding")
            For Each row In ApAgingRows
                sb.AppendLine($"{q}{row.VendorCode}{q},{q}{row.VendorName}{q},{row.CurrentAmount:F2},{row.Days1To30:F2},{row.Days31To60:F2},{row.Days61To90:F2},{row.DaysOver90:F2},{row.TotalOutstanding:F2}")
            Next
            If ApAgingReport IsNot Nothing Then
                sb.AppendLine($"{q}TOTALS{q},,{ApAgingReport.TotalCurrent:F2},{ApAgingReport.TotalDays1To30:F2},{ApAgingReport.TotalDays31To60:F2},{ApAgingReport.TotalDays61To90:F2},{ApAgingReport.TotalDaysOver90:F2},{ApAgingReport.GrandTotal:F2}")
            End If
            Return sb.ToString()
        End Function

        Public Function ExportInventoryValuationToCsv() As String
            Dim sb As New StringBuilder()
            Dim q As Char = ChrW(34)
            sb.AppendLine("SKU,Product Name,Category,Base Unit,Qty On Hand,Moving Avg Cost,Total Valuation")
            For Each row In StockValuationRows
                sb.AppendLine($"{q}{row.Sku}{q},{q}{row.ProductName}{q},{q}{row.Category}{q},{q}{row.BaseUnit}{q},{row.QtyOnHand},{row.MovingAvgCost:F4},{row.TotalValuation:F2}")
            Next
            If StockValuationReport IsNot Nothing Then
                sb.AppendLine($"{q}TOTALS{q},,,,{StockValuationReport.TotalStockQuantity},,{StockValuationReport.TotalValuation:F2}")
                sb.AppendLine($"{q}RECONCILIATION{q},,{q}GL Account 1300 Balance{q},,,{StockValuationReport.GlAccount1300Balance:F2}")
                sb.AppendLine($"{q}RECONCILIATION{q},,{q}Inventory vs GL 1300 Variance{q},,,{StockValuationReport.ReconciliationVariance:F4}")
            End If
            Return sb.ToString()
        End Function

        Public Function ExportGrniAgingToCsv() As String
            Dim sb As New StringBuilder()
            Dim q As Char = ChrW(34)
            sb.AppendLine("GRN Number,Receipt Date,Vendor,Item Description,Received Qty,Billed Qty,Unbilled Qty,PO Price,Accrual Balance,Days Open,Aging Status")
            For Each row In GrniAgingRows
                sb.AppendLine($"{q}{row.GrnNumber}{q},{q}{row.FormattedDate}{q},{q}{row.VendorName}{q},{q}{row.ItemDescription}{q},{row.ReceivedQty},{row.BilledQty},{row.UnbilledQty},{row.PoPrice:F2},{row.AccrualBalance:F2},{row.DaysOpen},{q}{row.StatusText}{q}")
            Next
            If GrniAgingReport IsNot Nothing Then
                sb.AppendLine($"{q}TOTALS{q},,,,{GrniAgingReport.TotalReceivedQty},{GrniAgingReport.TotalBilledQty},{GrniAgingReport.TotalUnbilledQty},,{GrniAgingReport.TotalAccrualBalance:F2},,")
                sb.AppendLine($"{q}SUMMARY{q},,{q}Flagged Over 30 Days Count{q},,,,{GrniAgingReport.TotalFlaggedOver30Days},{q}Flagged Amount{q},{GrniAgingReport.TotalFlaggedAmount:F2},,")
            End If
            Return sb.ToString()
        End Function
    End Class

End Namespace
