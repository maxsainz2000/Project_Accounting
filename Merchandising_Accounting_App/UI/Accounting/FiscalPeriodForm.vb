Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms
Imports Merchandising_Accounting_App.Services

Namespace UI.Accounting

    Public Class FiscalPeriodForm

        Private _period As FiscalPeriodViewModel

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property CurrentPeriod As FiscalPeriodViewModel
            Get
                Return _period
            End Get
            Set(value As FiscalPeriodViewModel)
                _period = value
                UpdatePeriodDisplay()
            End Set
        End Property

        Public Sub New()
            InitializeComponent()
            Dim ledgerVm As New LedgerDashboardViewModel()
            _period = ledgerVm.CurrentPeriod
            UpdatePeriodDisplay()
        End Sub

        Public Sub New(period As FiscalPeriodViewModel)
            InitializeComponent()
            _period = period
            UpdatePeriodDisplay()
        End Sub

        Private Sub UpdatePeriodDisplay()
            If _period Is Nothing Then Return

            lblPeriodLabel.Text = $"Period: {_period.PeriodLabel}"
            lblDateRange.Text = $"Date Range: {_period.DateRangeText}"

            If _period.IsClosed Then
                lblStatusBadge.Text = "CLOSED - PERMANENTLY SEALED"
                lblStatusBadge.BackColor = Color.FromArgb(108, 117, 125)
                btnClosePeriod.Enabled = False
                btnClosePeriod.Text = "Period Already Closed"
            Else
                lblStatusBadge.Text = "OPEN - POSTINGS ACTIVE"
                lblStatusBadge.BackColor = Color.FromArgb(25, 135, 84)
                btnClosePeriod.Enabled = True
                btnClosePeriod.Text = "Seal & Close Fiscal Period"
            End If
        End Sub

        Private Sub btnClosePeriod_Click(sender As Object, e As EventArgs) Handles btnClosePeriod.Click
            If _period Is Nothing OrElse _period.IsClosed Then Return

            Dim confirmMsg As String = $"Are you sure you want to seal and close fiscal period {_period.PeriodLabel}?" & Environment.NewLine & Environment.NewLine &
                                       "Statutory statutory posting rules:" & Environment.NewLine &
                                       "• Closed periods are permanently immutable." & Environment.NewLine &
                                       "• No further operational or adjusting journal entries can be posted to this period."

            Dim result As DialogResult = MessageBox.Show(Me, confirmMsg, "Confirm Period Close Guard", MessageBoxButtons.YesNo, MessageBoxIcon.Warning)
            If result = DialogResult.Yes Then
                Try
                    Dim periodSvc As New PeriodService()
                    periodSvc.CloseFiscalPeriod(_period.PeriodId)
                    _period.IsClosed = True
                    _period.ClosedAtUtc = DateTime.UtcNow
                    UpdatePeriodDisplay()
                    MessageBox.Show(Me, $"Fiscal period {_period.PeriodLabel} is now permanently sealed.", "Fiscal Period Closed", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Catch ex As Exception
                    MessageBox.Show(Me, $"Failed to close fiscal period {_period.PeriodLabel}: {ex.Message}", "Period Close Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End Try
            End If
        End Sub

        Private Sub btnDismiss_Click(sender As Object, e As EventArgs) Handles btnDismiss.Click
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

    End Class

End Namespace
