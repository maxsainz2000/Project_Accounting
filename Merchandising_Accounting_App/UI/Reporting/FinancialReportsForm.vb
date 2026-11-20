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
            LoadProfitLossGrid()
            LoadProductMarginsGrid()
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

        Private Sub btnExportCsv_Click(sender As Object, e As EventArgs) Handles btnExportCsv.Click
            Dim defaultFileName As String = "ReportExport.csv"
            Dim csvContent As String = String.Empty

            If tabReports.SelectedTab Is tabTrialBalance Then
                defaultFileName = "TrialBalance_FY2026-P11.csv"
                csvContent = _viewModel.ExportTrialBalanceToCsv()
            ElseIf tabReports.SelectedTab Is tabProfitLoss Then
                defaultFileName = "IncomeStatement_FY2026-P11.csv"
                csvContent = _viewModel.ExportIncomeStatementToCsv()
            ElseIf tabReports.SelectedTab Is tabProductMargins Then
                defaultFileName = "ProductMargins_FY2026-P11.csv"
                csvContent = _viewModel.ExportProductMarginsToCsv()
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
