Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.ComponentModel
Imports System.Linq
Imports System.Windows.Forms

Namespace UI.Controls

    ''' <summary>
    ''' Atomic reusable control displaying balanced double-entry general ledger distribution lines.
    ''' Enforces the zero horizontal scrollbar invariant and 4-decimal double-entry balancing discipline.
    ''' </summary>
    Public Class DoubleEntryDistributionGridControl

        Public Class DistributionLineItem
            Public Property LineId As Integer
            Public Property AccountCode As String = String.Empty
            Public Property AccountName As String = String.Empty
            Public Property DebitAmount As Decimal
            Public Property CreditAmount As Decimal
            Public Property LineMemo As String = String.Empty
        End Class

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property TotalDebits As Decimal = 0D

        <DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)>
        Public Property TotalCredits As Decimal = 0D

        Public ReadOnly Property IsBalanced As Boolean
            Get
                Return Math.Round(TotalDebits, 4) = Math.Round(TotalCredits, 4)
            End Get
        End Property

        Public Sub Clear()
            dgvLines.Rows.Clear()
            TotalDebits = 0D
            TotalCredits = 0D
            UpdateSummaryDisplay()
        End Sub

        Public Sub SetLines(lines As IEnumerable(Of DistributionLineItem))
            dgvLines.Rows.Clear()
            TotalDebits = 0D
            TotalCredits = 0D

            If lines IsNot Nothing Then
                For Each item In lines
                    Dim rowIdx = dgvLines.Rows.Add(
                        item.LineId,
                        item.AccountCode,
                        item.AccountName,
                        item.DebitAmount,
                        item.CreditAmount,
                        item.LineMemo
                    )
                    TotalDebits += item.DebitAmount
                    TotalCredits += item.CreditAmount
                Next
            End If

            UpdateSummaryDisplay()
        End Sub

        Private Sub UpdateSummaryDisplay()
            lblDebits.Text = $"Total Debits: {TotalDebits:C2}"
            lblCredits.Text = $"Total Credits: {TotalCredits:C2}"
            badgeStatus.SetStatusFromDebitsAndCredits(TotalDebits, TotalCredits)
        End Sub

    End Class

End Namespace
