Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms

Namespace UI.Accounting

    Public Class LedgerDashboardForm

        Friend ReadOnly _viewModel As New LedgerDashboardViewModel()
        Private _selectedEntry As JournalEntryViewModel

        Private Sub LedgerDashboardForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            InitializePeriodIndicator()
            InitializeAccountTypeFilter()
            InitializeDocTypeFilter()
            RefreshAccountsGrid()
            RefreshJournalGrid()
            LedgerDashboardForm_Resize(Nothing, EventArgs.Empty)
        End Sub

        Private Sub InitializePeriodIndicator()
            If _viewModel.CurrentPeriod IsNot Nothing Then
                lblPeriodName.Text = _viewModel.CurrentPeriod.PeriodLabel
                lblPeriodDates.Text = _viewModel.CurrentPeriod.DateRangeText
                If _viewModel.CurrentPeriod.IsClosed Then
                    lblPeriodBadge.Text = "CLOSED - SEALED"
                    lblPeriodBadge.BackColor = Color.FromArgb(220, 53, 69)
                Else
                    lblPeriodBadge.Text = "OPEN - ACTIVE"
                    lblPeriodBadge.BackColor = Color.FromArgb(25, 135, 84)
                End If
            End If
        End Sub

        Private Sub InitializeAccountTypeFilter()
            If cmbAccountTypeFilter.Items.Count > 0 Then
                cmbAccountTypeFilter.SelectedIndex = 0
            End If
        End Sub

        Private Sub InitializeDocTypeFilter()
            If cmbDocTypeFilter.Items.Count > 0 Then
                cmbDocTypeFilter.SelectedIndex = 0
            End If
        End Sub

        Private Sub RefreshAccountsGrid()
            Dim filterType As String = CStr(cmbAccountTypeFilter.SelectedItem)
            Dim searchKeyword As String = txtSearchCoa.Text.Trim()

            Dim filtered = _viewModel.Accounts.AsEnumerable()

            If Not String.IsNullOrEmpty(filterType) AndAlso Not filterType.Equals("All Types", StringComparison.Ordinal) Then
                filtered = filtered.Where(Function(a) a.AccountType.StartsWith(filterType, StringComparison.Ordinal))
            End If

            If Not String.IsNullOrEmpty(searchKeyword) Then
                filtered = filtered.Where(Function(a) a.Code.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase) OrElse
                                                      a.Name.Contains(searchKeyword, StringComparison.OrdinalIgnoreCase))
            End If

            Dim list = filtered.ToList()
            dgvAccounts.Rows.Clear()

            For Each acct In list
                Dim rowIdx As Integer = dgvAccounts.Rows.Add(acct.Code, acct.Name, acct.AccountType, acct.NormalBalance, acct.FormattedBalance)
                If acct.Balance < 0D Then
                    dgvAccounts.Rows(rowIdx).Cells(4).Style.ForeColor = Color.DarkRed
                End If
            Next

            lblCoaCount.Text = $"Showing {list.Count} of {_viewModel.Accounts.Count} Chart Accounts | Active: {_viewModel.Accounts.Where(Function(a) a.IsActive).Count()}"
        End Sub

        Private Sub RefreshJournalGrid()
            Dim filterDoc As String = CStr(cmbDocTypeFilter.SelectedItem)

            Dim filtered = _viewModel.JournalEntries.AsEnumerable()

            If Not String.IsNullOrEmpty(filterDoc) AndAlso Not filterDoc.Equals("All Documents", StringComparison.Ordinal) Then
                filtered = filtered.Where(Function(j) j.SourceDocType.Equals(filterDoc, StringComparison.Ordinal))
            End If

            Dim list = filtered.ToList()
            dgvJournalEntries.Rows.Clear()

            For Each entry In list
                Dim statusText As String = entry.BalanceStatusText
                Dim rowIdx As Integer = dgvJournalEntries.Rows.Add(
                    entry.EntryId,
                    entry.FormattedPostingDate,
                    entry.FormattedEventDate,
                    entry.SourceDocType,
                    entry.SourceDocNumber,
                    entry.Description,
                    entry.TotalDebits.ToString("C2"),
                    entry.TotalCredits.ToString("C2"),
                    statusText
                )
                dgvJournalEntries.Rows(rowIdx).Tag = entry

                If Not entry.IsBalanced Then
                    dgvJournalEntries.Rows(rowIdx).Cells(8).Style.ForeColor = Color.Red
                    dgvJournalEntries.Rows(rowIdx).Cells(8).Style.Font = New Font(dgvJournalEntries.Font, FontStyle.Bold)
                Else
                    dgvJournalEntries.Rows(rowIdx).Cells(8).Style.ForeColor = Color.DarkGreen
                End If
            Next

            If dgvJournalEntries.Rows.Count > 0 Then
                dgvJournalEntries.Rows(0).Selected = True
                Dim firstEntry As JournalEntryViewModel = TryCast(dgvJournalEntries.Rows(0).Tag, JournalEntryViewModel)
                SelectJournalEntry(firstEntry)
            Else
                SelectJournalEntry(Nothing)
            End If
        End Sub

        Private Sub SelectJournalEntry(entry As JournalEntryViewModel)
            _selectedEntry = entry
            dgvJournalLines.Rows.Clear()

            If entry Is Nothing Then
                lblSelectedEntryInfo.Text = "No entry selected"
                lblDebitsSum.Text = "Debits: $0.00"
                lblCreditsSum.Text = "Credits: $0.00"
                lblBalancedBadge.Text = "-"
                lblBalancedBadge.BackColor = Color.Gray
                Return
            End If

            lblSelectedEntryInfo.Text = $"Entry #{entry.EntryId}"

            Dim totalDebits As Decimal = 0D
            Dim totalCredits As Decimal = 0D

            For Each line In entry.Lines
                dgvJournalLines.Rows.Add(
                    line.LineId,
                    line.AccountCode,
                    line.AccountName,
                    line.FormattedDebit,
                    line.FormattedCredit,
                    line.LineMemo
                )
                totalDebits += line.DebitAmount
                totalCredits += line.CreditAmount
            Next

            lblDebitsSum.Text = $"Debits: {totalDebits:C2}"
            lblCreditsSum.Text = $"Credits: {totalCredits:C2}"

            If Math.Round(totalDebits, 4) = Math.Round(totalCredits, 4) Then
                lblBalancedBadge.Text = "BALANCED"
                lblBalancedBadge.BackColor = Color.FromArgb(25, 135, 84)
            Else
                lblBalancedBadge.Text = "UNBALANCED"
                lblBalancedBadge.BackColor = Color.FromArgb(220, 53, 69)
            End If
        End Sub

        Private Sub dgvJournalEntries_SelectionChanged(sender As Object, e As EventArgs) Handles dgvJournalEntries.SelectionChanged
            If dgvJournalEntries.SelectedRows.Count > 0 Then
                Dim selectedRow As DataGridViewRow = dgvJournalEntries.SelectedRows(0)
                Dim entry As JournalEntryViewModel = TryCast(selectedRow.Tag, JournalEntryViewModel)
                SelectJournalEntry(entry)
            End If
        End Sub

        Private Sub cmbAccountTypeFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAccountTypeFilter.SelectedIndexChanged
            RefreshAccountsGrid()
        End Sub

        Private Sub txtSearchCoa_TextChanged(sender As Object, e As EventArgs) Handles txtSearchCoa.TextChanged
            RefreshAccountsGrid()
        End Sub

        Private Sub cmbDocTypeFilter_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbDocTypeFilter.SelectedIndexChanged
            RefreshJournalGrid()
        End Sub

        Private Sub btnRefresh_Click(sender As Object, e As EventArgs) Handles btnRefresh.Click
            _viewModel.LoadData()
            InitializePeriodIndicator()
            RefreshAccountsGrid()
            RefreshJournalGrid()
        End Sub

        Private Sub btnPeriodAction_Click(sender As Object, e As EventArgs) Handles btnPeriodAction.Click
            Using dlg As New FiscalPeriodForm(_viewModel.CurrentPeriod)
                dlg.ShowDialog(Me)
            End Using
            _viewModel.LoadData()
            InitializePeriodIndicator()
            RefreshJournalGrid()
        End Sub

        Private Sub LedgerDashboardForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
            If splitMain IsNot Nothing AndAlso splitMain.Width > 0 Then
                Dim targetDistance As Integer
                If splitMain.Width <= 1100 Then
                    ' Standard 720p: allocate 290px to Panel 1 (COA) matching splitMain.Panel1MinSize, giving Panel 2 706px and grid width 674px
                    targetDistance = 290
                    Dim minDistance As Integer = splitMain.Panel1MinSize
                    Dim maxDistance As Integer = splitMain.Width - splitMain.Panel2MinSize - splitMain.SplitterWidth
                    If maxDistance >= minDistance Then
                        targetDistance = Math.Clamp(targetDistance, minDistance, maxDistance)
                    Else
                        targetDistance = minDistance
                    End If
                    If colDescription IsNot Nothing Then
                        colDescription.MinimumWidth = 2
                    End If
                Else
                    ' Maximized 1080p: allocate ~32% (clamped 420-480px)
                    targetDistance = CInt(Math.Floor(splitMain.Width * 0.32))
                    Dim minDistance As Integer = Math.Max(splitMain.Panel1MinSize, 420)
                    Dim maxDistance As Integer = Math.Min(480, splitMain.Width - splitMain.Panel2MinSize - splitMain.SplitterWidth)
                    If maxDistance >= minDistance Then
                        targetDistance = Math.Clamp(targetDistance, minDistance, maxDistance)
                    Else
                        targetDistance = minDistance
                    End If
                    If colDescription IsNot Nothing Then
                        colDescription.MinimumWidth = 90
                    End If
                End If

                If splitMain.SplitterDistance <> targetDistance Then
                    splitMain.SplitterDistance = targetDistance
                End If
            End If

            If lblPeriodDates IsNot Nothing Then
                ' Hide period date range on restored/compact widths to prevent lblTitle ellipsis truncation
                lblPeriodDates.Visible = (Me.Width >= 1400)
            End If
        End Sub

    End Class

End Namespace
