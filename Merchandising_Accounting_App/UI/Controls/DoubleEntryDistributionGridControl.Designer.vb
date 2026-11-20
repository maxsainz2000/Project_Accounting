Namespace UI.Controls

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class DoubleEntryDistributionGridControl
        Inherits System.Windows.Forms.UserControl

        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing AndAlso components IsNot Nothing Then
                    components.Dispose()
                End If
            Finally
                MyBase.Dispose(disposing)
            End Try
        End Sub

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Dim dgvCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Me.dgvLines = New System.Windows.Forms.DataGridView()
            Me.colLineId = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAccountCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAccountName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDebit = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colCredit = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colMemo = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.pnlFooter = New System.Windows.Forms.Panel()
            Me.flowSummary = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblDebits = New System.Windows.Forms.Label()
            Me.lblCredits = New System.Windows.Forms.Label()
            Me.badgeStatus = New Merchandising_Accounting_App.UI.Controls.BalancedStatusBadgeControl()
            CType(Me.dgvLines, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlFooter.SuspendLayout()
            Me.flowSummary.SuspendLayout()
            Me.SuspendLayout()
            '
            ' dgvLines
            '
            Me.dgvLines.AllowUserToAddRows = False
            Me.dgvLines.AllowUserToDeleteRows = False
            Me.dgvLines.AllowUserToResizeRows = False
            Me.dgvLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvLines.BackgroundColor = System.Drawing.Color.White
            Me.dgvLines.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            Me.dgvLines.ColumnHeadersDefaultCellStyle = dgvCellStyle1
            Me.dgvLines.ColumnHeadersHeight = 28
            Me.dgvLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvLines.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colLineId, Me.colAccountCode, Me.colAccountName, Me.colDebit, Me.colCredit, Me.colMemo})
            Me.dgvLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvLines.EnableHeadersVisualStyles = False
            Me.dgvLines.Location = New System.Drawing.Point(0, 0)
            Me.dgvLines.MultiSelect = False
            Me.dgvLines.Name = "dgvLines"
            Me.dgvLines.ReadOnly = True
            Me.dgvLines.RowHeadersVisible = False
            Me.dgvLines.RowTemplate.Height = 24
            Me.dgvLines.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvLines.Size = New System.Drawing.Size(700, 260)
            Me.dgvLines.TabIndex = 0
            '
            ' colLineId
            '
            Me.colLineId.DataPropertyName = "LineId"
            Me.colLineId.FillWeight = 40.0!
            Me.colLineId.HeaderText = "ID"
            Me.colLineId.MinimumWidth = 40
            Me.colLineId.Name = "colLineId"
            Me.colLineId.ReadOnly = True
            '
            ' colAccountCode
            '
            Me.colAccountCode.DataPropertyName = "AccountCode"
            Me.colAccountCode.FillWeight = 60.0!
            Me.colAccountCode.HeaderText = "Code"
            Me.colAccountCode.MinimumWidth = 60
            Me.colAccountCode.Name = "colAccountCode"
            Me.colAccountCode.ReadOnly = True
            '
            ' colAccountName
            '
            Me.colAccountName.DataPropertyName = "AccountName"
            Me.colAccountName.FillWeight = 160.0!
            Me.colAccountName.HeaderText = "Account Title"
            Me.colAccountName.MinimumWidth = 140
            Me.colAccountName.Name = "colAccountName"
            Me.colAccountName.ReadOnly = True
            '
            ' colDebit
            '
            Me.colDebit.DataPropertyName = "DebitAmount"
            dgvCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
            dgvCellStyle2.Format = "C2"
            Me.colDebit.DefaultCellStyle = dgvCellStyle2
            Me.colDebit.FillWeight = 80.0!
            Me.colDebit.HeaderText = "Debit ($)"
            Me.colDebit.MinimumWidth = 80
            Me.colDebit.Name = "colDebit"
            Me.colDebit.ReadOnly = True
            '
            ' colCredit
            '
            Me.colCredit.DataPropertyName = "CreditAmount"
            dgvCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight
            dgvCellStyle3.Format = "C2"
            Me.colCredit.DefaultCellStyle = dgvCellStyle3
            Me.colCredit.FillWeight = 80.0!
            Me.colCredit.HeaderText = "Credit ($)"
            Me.colCredit.MinimumWidth = 80
            Me.colCredit.Name = "colCredit"
            Me.colCredit.ReadOnly = True
            '
            ' colMemo
            '
            Me.colMemo.DataPropertyName = "LineMemo"
            Me.colMemo.FillWeight = 180.0!
            Me.colMemo.HeaderText = "Distribution Memo"
            Me.colMemo.MinimumWidth = 120
            Me.colMemo.Name = "colMemo"
            Me.colMemo.ReadOnly = True
            '
            ' pnlFooter
            '
            Me.pnlFooter.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlFooter.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlFooter.Controls.Add(Me.flowSummary)
            Me.pnlFooter.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlFooter.Location = New System.Drawing.Point(0, 260)
            Me.pnlFooter.Name = "pnlFooter"
            Me.pnlFooter.Size = New System.Drawing.Size(700, 34)
            Me.pnlFooter.TabIndex = 1
            '
            ' flowSummary
            '
            Me.flowSummary.Controls.Add(Me.lblDebits)
            Me.flowSummary.Controls.Add(Me.lblCredits)
            Me.flowSummary.Controls.Add(Me.badgeStatus)
            Me.flowSummary.Dock = System.Windows.Forms.DockStyle.Fill
            Me.flowSummary.Location = New System.Drawing.Point(0, 0)
            Me.flowSummary.Name = "flowSummary"
            Me.flowSummary.Padding = New System.Windows.Forms.Padding(8, 4, 8, 4)
            Me.flowSummary.Size = New System.Drawing.Size(698, 32)
            Me.flowSummary.TabIndex = 0
            '
            ' lblDebits
            '
            Me.lblDebits.AutoSize = True
            Me.lblDebits.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDebits.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblDebits.Location = New System.Drawing.Point(11, 8)
            Me.lblDebits.Margin = New System.Windows.Forms.Padding(3, 4, 16, 0)
            Me.lblDebits.Name = "lblDebits"
            Me.lblDebits.Size = New System.Drawing.Size(95, 15)
            Me.lblDebits.TabIndex = 0
            Me.lblDebits.Text = "Total Debits: $0.00"
            '
            ' lblCredits
            '
            Me.lblCredits.AutoSize = True
            Me.lblCredits.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCredits.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblCredits.Location = New System.Drawing.Point(125, 8)
            Me.lblCredits.Margin = New System.Windows.Forms.Padding(3, 4, 16, 0)
            Me.lblCredits.Name = "lblCredits"
            Me.lblCredits.Size = New System.Drawing.Size(98, 15)
            Me.lblCredits.TabIndex = 1
            Me.lblCredits.Text = "Total Credits: $0.00"
            '
            ' badgeStatus
            '
            Me.badgeStatus.BalancedText = "BALANCED"
            Me.badgeStatus.IsBalanced = True
            Me.badgeStatus.Location = New System.Drawing.Point(242, 6)
            Me.badgeStatus.Margin = New System.Windows.Forms.Padding(3, 2, 3, 2)
            Me.badgeStatus.Name = "badgeStatus"
            Me.badgeStatus.ShowVariance = False
            Me.badgeStatus.Size = New System.Drawing.Size(100, 22)
            Me.badgeStatus.TabIndex = 2
            Me.badgeStatus.UnbalancedText = "UNBALANCED"
            Me.badgeStatus.VarianceAmount = New Decimal(New Integer() {0, 0, 0, 0})
            '
            ' DoubleEntryDistributionGridControl
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.dgvLines)
            Me.Controls.Add(Me.pnlFooter)
            Me.Name = "DoubleEntryDistributionGridControl"
            Me.Size = New System.Drawing.Size(700, 294)
            CType(Me.dgvLines, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlFooter.ResumeLayout(False)
            Me.flowSummary.ResumeLayout(False)
            Me.flowSummary.PerformLayout()
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents dgvLines As System.Windows.Forms.DataGridView
        Friend WithEvents colLineId As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAccountCode As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAccountName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colDebit As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colCredit As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colMemo As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents pnlFooter As System.Windows.Forms.Panel
        Friend WithEvents flowSummary As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblDebits As System.Windows.Forms.Label
        Friend WithEvents lblCredits As System.Windows.Forms.Label
        Friend WithEvents badgeStatus As Merchandising_Accounting_App.UI.Controls.BalancedStatusBadgeControl

    End Class

End Namespace
