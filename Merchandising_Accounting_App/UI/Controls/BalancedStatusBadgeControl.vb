Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.ComponentModel
Imports System.Drawing
Imports System.Windows.Forms

Namespace UI.Controls

    ''' <summary>
    ''' Atomic reusable status badge indicating double-entry general ledger or reconciliation balancing state.
    ''' Adheres strictly to the 4-decimal-place double-entry equality invariant.
    ''' </summary>
    Public Class BalancedStatusBadgeControl

        Private _isBalanced As Boolean = True
        Private _balancedText As String = "BALANCED"
        Private _unbalancedText As String = "UNBALANCED"
        Private _varianceAmount As Decimal = 0D
        Private _showVariance As Boolean = False

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property IsBalanced As Boolean
            Get
                Return _isBalanced
            End Get
            Set(value As Boolean)
                _isBalanced = value
                UpdateBadgeDisplay()
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property BalancedText As String
            Get
                Return _balancedText
            End Get
            Set(value As String)
                _balancedText = value
                UpdateBadgeDisplay()
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property UnbalancedText As String
            Get
                Return _unbalancedText
            End Get
            Set(value As String)
                _unbalancedText = value
                UpdateBadgeDisplay()
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property VarianceAmount As Decimal
            Get
                Return _varianceAmount
            End Get
            Set(value As Decimal)
                _varianceAmount = value
                UpdateBadgeDisplay()
            End Set
        End Property

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property ShowVariance As Boolean
            Get
                Return _showVariance
            End Get
            Set(value As Boolean)
                _showVariance = value
                UpdateBadgeDisplay()
            End Set
        End Property

        Public Sub SetStatus(balanced As Boolean, Optional variance As Decimal = 0D)
            _isBalanced = balanced
            _varianceAmount = variance
            UpdateBadgeDisplay()
        End Sub

        Public Sub SetStatusFromDebitsAndCredits(totalDebits As Decimal, totalCredits As Decimal)
            Dim balanced = Math.Round(totalDebits, 4) = Math.Round(totalCredits, 4)
            Dim variance = Math.Abs(totalDebits - totalCredits)
            SetStatus(balanced, variance)
        End Sub

        Private Sub UpdateBadgeDisplay()
            If _isBalanced Then
                lblBadge.BackColor = Color.FromArgb(25, 135, 84) ' Success Green (#198754)
                lblBadge.Text = _balancedText
            Else
                lblBadge.BackColor = Color.FromArgb(220, 53, 69) ' Danger Red (#DC3545)
                If _showVariance AndAlso _varianceAmount > 0D Then
                    lblBadge.Text = $"{_unbalancedText} (Var: {_varianceAmount:C2})"
                Else
                    lblBadge.Text = _unbalancedText
                End If
            End If
        End Sub

    End Class

End Namespace
