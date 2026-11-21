Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Drawing
Imports System.IO
Imports System.Windows.Forms

Namespace UI.Reporting

    Public Class FinancialReportsForm

        Friend ReadOnly _viewModel As New FinancialReportsViewModel()

        Private Sub FinancialReportsForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            If cmbPeriod.Items.Count > 0 Then
                cmbPeriod.SelectedIndex = 0
            Else
                RefreshAllReports()
            End If
        End Sub

        Private Sub RefreshAllReports()
            Dim asOfDate As DateTime? = Nothing
            Dim startDate As DateTime? = Nothing
            Dim endDate As DateTime? = Nothing

            If cmbPeriod.SelectedItem IsNot Nothing Then
                Dim selectedText As String = cmbPeriod.SelectedItem.ToString()
                If selectedText.Contains("P11") Then
                    startDate = New DateTime(2026, 11, 1)
                    endDate = New DateTime(2026, 11, 30, 23, 59, 59)
                    asOfDate = endDate
                ElseIf selectedText.Contains("P10") Then
                    startDate = New DateTime(2026, 10, 1)
                    endDate = New DateTime(2026, 10, 31, 23, 59, 59)
                    asOfDate = endDate
                ElseIf selectedText.Contains("YTD") Then
                    startDate = New DateTime(2026, 1, 1)
                    endDate = New DateTime(2026, 12, 31, 23, 59, 59)
                    asOfDate = endDate
                End If
            End If

            Try
                _viewModel.LoadReports(asOfDate, startDate, endDate)
            Catch ex As Exception
                MessageBox.Show(Me, $"Failed to load financial reports: {ex.Message}", "Reporting Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
            LoadTrialBalanceGrid()
            LoadBalanceSheetGrid()
            LoadProfitLossGrid()
            LoadProductMarginsGrid()
            LoadArAgingGrid()
            LoadApAgingGrid()
            LoadStockValuationGrid()
            LoadGrniAgingGrid()
        End Sub

        Private Sub LoadTrialBalanceGrid()
            dgvTrialBalance.Rows.Clear()

            For Each row In _viewModel.TrialBalanceRows
                Dim rowIdx As Integer = dgvTrialBalance.Rows.Add(
                    row.AccountCode,
                    row.AccountName,
                    row.AccountType,
                    row.NormalBalance,
                    row.FormattedDebit,
                    row.FormattedCredit
                )

                If row.AccountType.StartsWith("Revenue", StringComparison.Ordinal) OrElse
                   row.AccountType.Equals("Liability", StringComparison.Ordinal) OrElse
                   row.AccountType.Equals("Equity", StringComparison.Ordinal) Then
                    dgvTrialBalance.Rows(rowIdx).Cells(5).Style.Font = New Font(dgvTrialBalance.Font, FontStyle.Regular)
                End If
            Next

            lblTbTotalDebits.Text = $"Total Debits: {_viewModel.TotalTrialBalanceDebits:C2}"
            lblTbTotalCredits.Text = $"Total Credits: {_viewModel.TotalTrialBalanceCredits:C2}"

            Dim diff As Decimal = Math.Abs(_viewModel.TotalTrialBalanceDebits - _viewModel.TotalTrialBalanceCredits)
            lblTbDiff.Text = $"Difference: {diff:C4}"

            If _viewModel.IsTrialBalanceBalanced Then
                lblTbBalancedBadge.Text = "TRIAL BALANCE: BALANCED"
                lblTbBalancedBadge.BackColor = Color.FromArgb(25, 135, 84)
            Else
                lblTbBalancedBadge.Text = "TRIAL BALANCE: UNBALANCED"
                lblTbBalancedBadge.BackColor = Color.FromArgb(220, 53, 69)
            End If
        End Sub

        Private Sub LoadProfitLossGrid()
            dgvProfitLoss.Rows.Clear()

            For Each row In _viewModel.IncomeStatementRows
                Dim rowIdx As Integer = dgvProfitLoss.Rows.Add(
                    row.Category,
                    row.LineDescription,
                    row.FormattedAmount
                )

                Dim gridRow As DataGridViewRow = dgvProfitLoss.Rows(rowIdx)

                If row.IsTotal Then
                    gridRow.DefaultCellStyle.Font = New Font(dgvProfitLoss.Font.FontFamily, 9.5F, FontStyle.Bold)
                    gridRow.DefaultCellStyle.BackColor = Color.FromArgb(235, 240, 250)
                    If row.Amount < 0D Then
                        gridRow.Cells(2).Style.ForeColor = Color.FromArgb(220, 53, 69)
                    Else
                        gridRow.Cells(2).Style.ForeColor = Color.FromArgb(25, 135, 84)
                    End If
                ElseIf row.IsSubtotal Then
                    gridRow.DefaultCellStyle.Font = New Font(dgvProfitLoss.Font.FontFamily, 9.0F, FontStyle.Bold)
                    gridRow.DefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250)
                End If
            Next
        End Sub

        Private Sub LoadProductMarginsGrid()
            dgvProductMargins.Rows.Clear()

            Dim totalUnits As Integer = 0
            Dim totalRevenue As Decimal = 0D
            Dim totalProfit As Decimal = 0D

            For Each row In _viewModel.ProductMarginRows
                totalUnits += row.UnitsSold
                totalRevenue += row.InvoicedRevenue
                totalProfit += row.NetMarginAmount

                Dim rowIdx As Integer = dgvProductMargins.Rows.Add(
                    row.ProductSku,
                    row.ProductName,
                    row.UnitsSold,
                    row.FormattedRevenue,
                    row.FormattedCogs,
                    row.FormattedFreight,
                    row.FormattedPpv,
                    row.FormattedMargin,
                    row.FormattedPercentage
                )

                If row.NetMarginAmount > 0D Then
                    dgvProductMargins.Rows(rowIdx).Cells(7).Style.ForeColor = Color.FromArgb(25, 135, 84)
                ElseIf row.NetMarginAmount < 0D Then
                    dgvProductMargins.Rows(rowIdx).Cells(7).Style.ForeColor = Color.FromArgb(220, 53, 69)
                End If
            Next

            lblPmTotalUnits.Text = $"Total Units Sold: {totalUnits}"
            lblPmTotalProfit.Text = $"Total Net Landed Margin: {totalProfit:C2}"

            If totalRevenue > 0D Then
                Dim overallPct As Decimal = Math.Round((totalProfit / totalRevenue) * 100D, 2)
                lblPmOverallMargin.Text = $"Weighted Landed Margin: {overallPct:F2}%"
            End If
        End Sub

        Private Sub LoadBalanceSheetGrid()
            dgvBalanceSheet.Rows.Clear()

            For Each row In _viewModel.BalanceSheetRows
                Dim rowIdx As Integer = dgvBalanceSheet.Rows.Add(
                    row.Category,
                    row.AccountCode,
                    row.AccountName,
                    row.FormattedAmount
                )

                Dim gridRow As DataGridViewRow = dgvBalanceSheet.Rows(rowIdx)

                If row.IsTotal Then
                    gridRow.DefaultCellStyle.Font = New Font(dgvBalanceSheet.Font.FontFamily, 9.5F, FontStyle.Bold)
                    gridRow.DefaultCellStyle.BackColor = Color.FromArgb(235, 240, 250)
                ElseIf row.IsSubtotal Then
                    gridRow.DefaultCellStyle.Font = New Font(dgvBalanceSheet.Font.FontFamily, 9.0F, FontStyle.Bold)
                    gridRow.DefaultCellStyle.BackColor = Color.FromArgb(248, 249, 250)
                End If
            Next

            If _viewModel.BalanceSheetReport IsNot Nothing Then
                lblBsTotalAssets.Text = $"Total Assets: {_viewModel.BalanceSheetReport.TotalAssets:C2}"
                lblBsTotalLiabilities.Text = $"Total Liabilities: {_viewModel.BalanceSheetReport.TotalLiabilities:C2}"
                lblBsNetAssets.Text = $"Net Assets: {_viewModel.BalanceSheetReport.NetAssets:C2}"
                lblBsTotalEquity.Text = $"Total Equity: {_viewModel.BalanceSheetReport.TotalEquity:C2}"
                lblBsVariance.Text = $"Variance: {_viewModel.BalanceSheetReport.NetAssetsToEquityVariance:C4}"

                If _viewModel.BalanceSheetReport.IsBalanced Then
                    lblBsBalancedBadge.Text = "BALANCE SHEET: BALANCED"
                    lblBsBalancedBadge.BackColor = Color.FromArgb(25, 135, 84)
                Else
                    lblBsBalancedBadge.Text = "BALANCE SHEET: UNBALANCED"
                    lblBsBalancedBadge.BackColor = Color.FromArgb(220, 53, 69)
                End If
            End If
        End Sub

        Private Sub LoadArAgingGrid()
            dgvArAging.Rows.Clear()

            For Each row In _viewModel.ArAgingRows
                Dim rowIdx As Integer = dgvArAging.Rows.Add(
                    row.CustomerCode,
                    row.CustomerName,
                    row.FormattedCurrent,
                    row.FormattedDays1To30,
                    row.FormattedDays31To60,
                    row.FormattedDays61To90,
                    row.FormattedDaysOver90,
                    row.FormattedTotal
                )

                If row.DaysOver90 > 0D Then
                    dgvArAging.Rows(rowIdx).Cells(6).Style.ForeColor = Color.FromArgb(220, 53, 69)
                End If
            Next

            If _viewModel.ArAgingReport IsNot Nothing Then
                lblArCurrent.Text = $"Current: {_viewModel.ArAgingReport.TotalCurrent:C2}"
                lblAr1To30.Text = $"1-30 Days: {_viewModel.ArAgingReport.TotalDays1To30:C2}"
                lblAr31To60.Text = $"31-60 Days: {_viewModel.ArAgingReport.TotalDays31To60:C2}"
                lblAr61To90.Text = $"61-90 Days: {_viewModel.ArAgingReport.TotalDays61To90:C2}"
                lblArOver90.Text = $"90+ Days: {_viewModel.ArAgingReport.TotalDaysOver90:C2}"
                lblArTotal.Text = $"Total AR: {_viewModel.ArAgingReport.GrandTotal:C2}"

                If _viewModel.ArAgingReport.TotalDaysOver90 > 0D Then
                    lblArOver90.ForeColor = Color.FromArgb(220, 53, 69)
                Else
                    lblArOver90.ForeColor = Color.FromArgb(33, 37, 41)
                End If
            End If
        End Sub

        Private Sub LoadApAgingGrid()
            dgvApAging.Rows.Clear()

            For Each row In _viewModel.ApAgingRows
                Dim rowIdx As Integer = dgvApAging.Rows.Add(
                    row.VendorCode,
                    row.VendorName,
                    row.FormattedCurrent,
                    row.FormattedDays1To30,
                    row.FormattedDays31To60,
                    row.FormattedDays61To90,
                    row.FormattedDaysOver90,
                    row.FormattedTotal
                )

                If row.DaysOver90 > 0D Then
                    dgvApAging.Rows(rowIdx).Cells(6).Style.ForeColor = Color.FromArgb(220, 53, 69)
                End If
            Next

            If _viewModel.ApAgingReport IsNot Nothing Then
                lblApCurrent.Text = $"Current: {_viewModel.ApAgingReport.TotalCurrent:C2}"
                lblAp1To30.Text = $"1-30 Days: {_viewModel.ApAgingReport.TotalDays1To30:C2}"
                lblAp31To60.Text = $"31-60 Days: {_viewModel.ApAgingReport.TotalDays31To60:C2}"
                lblAp61To90.Text = $"61-90 Days: {_viewModel.ApAgingReport.TotalDays61To90:C2}"
                lblApOver90.Text = $"90+ Days: {_viewModel.ApAgingReport.TotalDaysOver90:C2}"
                lblApTotal.Text = $"Total AP: {_viewModel.ApAgingReport.GrandTotal:C2}"

                If _viewModel.ApAgingReport.TotalDaysOver90 > 0D Then
                    lblApOver90.ForeColor = Color.FromArgb(220, 53, 69)
                Else
                    lblApOver90.ForeColor = Color.FromArgb(33, 37, 41)
                End If
            End If
        End Sub

        Private Sub LoadStockValuationGrid()
            dgvStockValuation.Rows.Clear()

            For Each row In _viewModel.StockValuationRows
                dgvStockValuation.Rows.Add(
                    row.Sku,
                    row.ProductName,
                    row.Category,
                    row.BaseUnit,
                    row.FormattedQty,
                    row.FormattedCost,
                    row.FormattedValuation
                )
            Next

            If _viewModel.StockValuationReport IsNot Nothing Then
                lblSvTotalUnits.Text = $"Total Units: {_viewModel.StockValuationReport.TotalStockQuantity:N0}"
                lblSvSubledgerValuation.Text = $"Stock Valuation: {_viewModel.StockValuationReport.TotalValuation:C2}"
                lblSvGlAccount1300.Text = $"GL 1300: {_viewModel.StockValuationReport.GlAccount1300Balance:C2}"
                lblSvVariance.Text = $"Variance: {_viewModel.StockValuationReport.ReconciliationVariance:C4}"

                If _viewModel.StockValuationReport.IsReconciled Then
                    lblSvReconciledBadge.Text = "GL 1300: RECONCILED"
                    lblSvReconciledBadge.BackColor = Color.FromArgb(25, 135, 84)
                Else
                    lblSvReconciledBadge.Text = "GL 1300: VARIANCE"
                    lblSvReconciledBadge.BackColor = Color.FromArgb(220, 53, 69)
                End If
            End If
        End Sub

        Private Sub LoadGrniAgingGrid()
            dgvGrniAging.Rows.Clear()

            For Each row In _viewModel.GrniAgingRows
                Dim rowIdx As Integer = dgvGrniAging.Rows.Add(
                    row.GrnNumber,
                    row.VendorName,
                    row.ItemDescription,
                    row.ReceivedQty,
                    row.BilledQty,
                    row.UnbilledQty,
                    row.FormattedPoPrice,
                    row.FormattedAccrual,
                    row.DaysOpen,
                    row.StatusText
                )

                If row.FlaggedOver30Days Then
                    dgvGrniAging.Rows(rowIdx).Cells(9).Style.BackColor = Color.FromArgb(255, 235, 235)
                    dgvGrniAging.Rows(rowIdx).Cells(9).Style.ForeColor = Color.FromArgb(220, 53, 69)
                    dgvGrniAging.Rows(rowIdx).Cells(9).Style.Font = New Font(dgvGrniAging.Font, FontStyle.Bold)
                End If
            Next

            If _viewModel.GrniAgingReport IsNot Nothing Then
                lblGrniOpenCount.Text = $"Open Lines: {_viewModel.GrniAgingRows.Count}"
                lblGrniUnbilledUnits.Text = $"Unbilled Units: {_viewModel.GrniAgingReport.TotalUnbilledQty:N0}"
                lblGrniAccrualTotal.Text = $"Accrual (2100): {_viewModel.GrniAgingReport.TotalAccrualBalance:C2}"
                lblGrniFlaggedCount.Text = $"Flagged >30D: {_viewModel.GrniAgingReport.TotalFlaggedOver30Days}"
                lblGrniFlaggedAmount.Text = $"Flagged Bal: {_viewModel.GrniAgingReport.TotalFlaggedAmount:C2}"

                If _viewModel.GrniAgingReport.TotalFlaggedOver30Days > 0 Then
                    lblGrniFlaggedCount.ForeColor = Color.FromArgb(220, 53, 69)
                    lblGrniFlaggedAmount.ForeColor = Color.FromArgb(220, 53, 69)
                Else
                    lblGrniFlaggedCount.ForeColor = Color.FromArgb(25, 135, 84)
                    lblGrniFlaggedAmount.ForeColor = Color.FromArgb(25, 135, 84)
                End If
            End If
        End Sub

        Private Sub btnExportCsv_Click(sender As Object, e As EventArgs) Handles btnExportCsv.Click
            Dim defaultFileName As String = "ReportExport.csv"
            Dim csvContent As String = String.Empty

            If tabReports.SelectedTab Is tabTrialBalance Then
                defaultFileName = "TrialBalance_FY2026-P11.csv"
                csvContent = _viewModel.ExportTrialBalanceToCsv()
            ElseIf tabReports.SelectedTab Is tabBalanceSheet Then
                defaultFileName = "BalanceSheet_FY2026-P11.csv"
                csvContent = _viewModel.ExportBalanceSheetToCsv()
            ElseIf tabReports.SelectedTab Is tabProfitLoss Then
                defaultFileName = "IncomeStatement_FY2026-P11.csv"
                csvContent = _viewModel.ExportIncomeStatementToCsv()
            ElseIf tabReports.SelectedTab Is tabProductMargins Then
                defaultFileName = "ProductMargins_FY2026-P11.csv"
                csvContent = _viewModel.ExportProductMarginsToCsv()
            ElseIf tabReports.SelectedTab Is tabArAging Then
                defaultFileName = "ARAging_FY2026-P11.csv"
                csvContent = _viewModel.ExportArAgingToCsv()
            ElseIf tabReports.SelectedTab Is tabApAging Then
                defaultFileName = "APAging_FY2026-P11.csv"
                csvContent = _viewModel.ExportApAgingToCsv()
            ElseIf tabReports.SelectedTab Is tabStockValuation Then
                defaultFileName = "StockValuation_FY2026-P11.csv"
                csvContent = _viewModel.ExportInventoryValuationToCsv()
            ElseIf tabReports.SelectedTab Is tabGrniAging Then
                defaultFileName = "GrniAging_FY2026-P11.csv"
                csvContent = _viewModel.ExportGrniAgingToCsv()
            End If

            Using sfd As New SaveFileDialog()
                sfd.FileName = defaultFileName
                sfd.Filter = "CSV Files (*.csv)|*.csv|All Files (*.*)|*.*"
                sfd.Title = "Export Report to CSV"

                If sfd.ShowDialog(Me) = DialogResult.OK Then
                    Try
                        File.WriteAllText(sfd.FileName, csvContent, System.Text.Encoding.UTF8)
                        MessageBox.Show(Me, $"Successfully exported report to:{Environment.NewLine}{sfd.FileName}", "CSV Export Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Catch ex As Exception
                        MessageBox.Show(Me, $"Export failed: {ex.Message}", "Export Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    End Try
                End If
            End Using
        End Sub

        Private Sub cmbPeriod_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbPeriod.SelectedIndexChanged
            RefreshAllReports()
        End Sub

    End Class

End Namespace
