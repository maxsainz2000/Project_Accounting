Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace UI.Controls

    ''' <summary>
    ''' Atomic reusable chip control displaying the statutory fiscal period label, date boundaries, and status.
    ''' </summary>
    Public Class FiscalPeriodChipControl

        Private _periodLabel As String = "FY2026-P11"
        Private _dateRangeText As String = "2026-11-01 to 2026-11-30"
        Private _isClosed As Boolean = False
        Private _showDates As Boolean = True

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property PeriodLabel As String
            Get
                Return _periodLabel
            End Get
            Set(value As String)
                _periodLabel = value
                lblPeriod.Text = value
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property DateRangeText As String
            Get
                Return _dateRangeText
            End Get
            Set(value As String)
                _dateRangeText = value
                lblDateRange.Text = value
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property IsClosed As Boolean
            Get
                Return _isClosed
            End Get
            Set(value As Boolean)
                _isClosed = value
                UpdateStatusDisplay()
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property ShowDates As Boolean
            Get
                Return _showDates
            End Get
            Set(value As Boolean)
                _showDates = value
                lblDateRange.Visible = value
            End Set
        End Property

        Public Sub SetPeriod(label As String, dateRange As String, closed As Boolean)
            _periodLabel = label
            _dateRangeText = dateRange
            _isClosed = closed
            lblPeriod.Text = label
            lblDateRange.Text = dateRange
            UpdateStatusDisplay()
        End Sub

        Private Sub UpdateStatusDisplay()
            If _isClosed Then
                lblStatusPill.BackColor = Color.FromArgb(108, 117, 125) ' Secondary Muted Gray (#6C757D)
                lblStatusPill.Text = "CLOSED"
            Else
                lblStatusPill.BackColor = Color.FromArgb(25, 135, 84) ' Success Green (#198754)
                lblStatusPill.Text = "OPEN"
            End If
        End Sub

    End Class

End Namespace
