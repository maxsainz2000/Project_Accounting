Option Strict On
Option Explicit On
Option Compare Binary

Namespace UI.Accounting

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class FiscalPeriodForm
        Inherits System.Windows.Forms.Form

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
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblSubtitle = New System.Windows.Forms.Label()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.pnlStatus = New System.Windows.Forms.Panel()
            Me.lblTxSummary = New System.Windows.Forms.Label()
            Me.lblDateRange = New System.Windows.Forms.Label()
            Me.lblPeriodLabel = New System.Windows.Forms.Label()
            Me.lblStatusBadge = New System.Windows.Forms.Label()
            Me.lblStatusTitle = New System.Windows.Forms.Label()
            Me.pnlCompliance = New System.Windows.Forms.Panel()
            Me.lblPolicyDetails = New System.Windows.Forms.Label()
            Me.lblPolicyHeader = New System.Windows.Forms.Label()
            Me.pnlBottom = New System.Windows.Forms.Panel()
            Me.btnClosePeriod = New System.Windows.Forms.Button()
            Me.btnDismiss = New System.Windows.Forms.Button()
            Me.pnlHeader.SuspendLayout()
            Me.pnlStatus.SuspendLayout()
            Me.pnlCompliance.SuspendLayout()
            Me.pnlBottom.SuspendLayout()
            Me.SuspendLayout()
            '
            ' pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlHeader.Controls.Add(Me.lblSubtitle)
            Me.pnlHeader.Controls.Add(Me.lblTitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Size = New System.Drawing.Size(540, 72)
            Me.pnlHeader.TabIndex = 0
            '
            ' lblSubtitle
            '
            Me.lblSubtitle.AutoSize = True
            Me.lblSubtitle.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblSubtitle.Location = New System.Drawing.Point(16, 38)
            Me.lblSubtitle.Name = "lblSubtitle"
            Me.lblSubtitle.Size = New System.Drawing.Size(370, 15)
            Me.lblSubtitle.TabIndex = 1
            Me.lblSubtitle.Text = "Statutory fiscal period immutability and statutory posting controls"
            '
            ' lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblTitle.Location = New System.Drawing.Point(16, 12)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(209, 21)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Fiscal Period Status Guard"
            '
            ' pnlStatus
            '
            Me.pnlStatus.BackColor = System.Drawing.Color.White
            Me.pnlStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlStatus.Controls.Add(Me.lblTxSummary)
            Me.pnlStatus.Controls.Add(Me.lblDateRange)
            Me.pnlStatus.Controls.Add(Me.lblPeriodLabel)
            Me.pnlStatus.Controls.Add(Me.lblStatusBadge)
            Me.pnlStatus.Controls.Add(Me.lblStatusTitle)
            Me.pnlStatus.Location = New System.Drawing.Point(16, 86)
            Me.pnlStatus.Name = "pnlStatus"
            Me.pnlStatus.Size = New System.Drawing.Size(508, 116)
            Me.pnlStatus.TabIndex = 1
            '
            ' lblTxSummary
            '
            Me.lblTxSummary.AutoSize = True
            Me.lblTxSummary.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTxSummary.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
            Me.lblTxSummary.Location = New System.Drawing.Point(16, 88)
            Me.lblTxSummary.Name = "lblTxSummary"
            Me.lblTxSummary.Size = New System.Drawing.Size(242, 15)
            Me.lblTxSummary.TabIndex = 4
            Me.lblTxSummary.Text = "Active Transactions: 4 Posted Journal Entries"
            '
            ' lblDateRange
            '
            Me.lblDateRange.AutoSize = True
            Me.lblDateRange.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDateRange.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
            Me.lblDateRange.Location = New System.Drawing.Point(16, 66)
            Me.lblDateRange.Name = "lblDateRange"
            Me.lblDateRange.Size = New System.Drawing.Size(217, 15)
            Me.lblDateRange.TabIndex = 3
            Me.lblDateRange.Text = "Date Range: 2026-11-01 to 2026-11-30"
            '
            ' lblPeriodLabel
            '
            Me.lblPeriodLabel.AutoSize = True
            Me.lblPeriodLabel.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPeriodLabel.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblPeriodLabel.Location = New System.Drawing.Point(16, 44)
            Me.lblPeriodLabel.Name = "lblPeriodLabel"
            Me.lblPeriodLabel.Size = New System.Drawing.Size(125, 17)
            Me.lblPeriodLabel.TabIndex = 2
            Me.lblPeriodLabel.Text = "Period: FY2026-P11"
            '
            ' lblStatusBadge
            '
            Me.lblStatusBadge.AutoSize = True
            Me.lblStatusBadge.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblStatusBadge.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblStatusBadge.ForeColor = System.Drawing.Color.White
            Me.lblStatusBadge.Location = New System.Drawing.Point(160, 10)
            Me.lblStatusBadge.Name = "lblStatusBadge"
            Me.lblStatusBadge.Padding = New System.Windows.Forms.Padding(8, 3, 8, 3)
            Me.lblStatusBadge.Size = New System.Drawing.Size(167, 21)
            Me.lblStatusBadge.TabIndex = 1
            Me.lblStatusBadge.Text = "OPEN - POSTINGS ACTIVE"
            Me.lblStatusBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            ' lblStatusTitle
            '
            Me.lblStatusTitle.AutoSize = True
            Me.lblStatusTitle.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblStatusTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblStatusTitle.Location = New System.Drawing.Point(16, 12)
            Me.lblStatusTitle.Name = "lblStatusTitle"
            Me.lblStatusTitle.Size = New System.Drawing.Size(133, 15)
            Me.lblStatusTitle.TabIndex = 0
            Me.lblStatusTitle.Text = "Current Period Status:"
            '
            ' pnlCompliance
            '
            Me.pnlCompliance.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlCompliance.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlCompliance.Controls.Add(Me.lblPolicyDetails)
            Me.pnlCompliance.Controls.Add(Me.lblPolicyHeader)
            Me.pnlCompliance.Location = New System.Drawing.Point(16, 214)
            Me.pnlCompliance.Name = "pnlCompliance"
            Me.pnlCompliance.Size = New System.Drawing.Size(508, 154)
            Me.pnlCompliance.TabIndex = 2
            '
            ' lblPolicyDetails
            '
            Me.lblPolicyDetails.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPolicyDetails.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
            Me.lblPolicyDetails.Location = New System.Drawing.Point(16, 36)
            Me.lblPolicyDetails.Name = "lblPolicyDetails"
            Me.lblPolicyDetails.Size = New System.Drawing.Size(476, 106)
            Me.lblPolicyDetails.TabIndex = 1
            Me.lblPolicyDetails.Text = "• Posting dates are strictly enforced to map to open fiscal periods." & Global.Microsoft.VisualBasic.Constants.vbCrLf & "• Closed periods are permanently sealed and immutable per audit compliance standards." & Global.Microsoft.VisualBasic.Constants.vbCrLf & "• Out-of-period physical receipts must receive statutory posting on the first open day of the current period." & Global.Microsoft.VisualBasic.Constants.vbCrLf & "• Unbalanced journal lines or retroactive updates to closed periods are strictly prohibited."
            '
            ' lblPolicyHeader
            '
            Me.lblPolicyHeader.AutoSize = True
            Me.lblPolicyHeader.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPolicyHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblPolicyHeader.Location = New System.Drawing.Point(16, 12)
            Me.lblPolicyHeader.Name = "lblPolicyHeader"
            Me.lblPolicyHeader.Size = New System.Drawing.Size(284, 15)
            Me.lblPolicyHeader.TabIndex = 0
            Me.lblPolicyHeader.Text = "Statutory Accounting & Audit Compliance Policy:"
            '
            ' pnlBottom
            '
            Me.pnlBottom.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlBottom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlBottom.Controls.Add(Me.btnClosePeriod)
            Me.pnlBottom.Controls.Add(Me.btnDismiss)
            Me.pnlBottom.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBottom.Location = New System.Drawing.Point(0, 384)
            Me.pnlBottom.Name = "pnlBottom"
            Me.pnlBottom.Size = New System.Drawing.Size(540, 56)
            Me.pnlBottom.TabIndex = 3
            '
            ' btnClosePeriod
            '
            Me.btnClosePeriod.BackColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.btnClosePeriod.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClosePeriod.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnClosePeriod.ForeColor = System.Drawing.Color.White
            Me.btnClosePeriod.Location = New System.Drawing.Point(16, 10)
            Me.btnClosePeriod.Name = "btnClosePeriod"
            Me.btnClosePeriod.Size = New System.Drawing.Size(200, 34)
            Me.btnClosePeriod.TabIndex = 0
            Me.btnClosePeriod.Text = "Seal & Close Fiscal Period"
            Me.btnClosePeriod.UseVisualStyleBackColor = False
            '
            ' btnDismiss
            '
            Me.btnDismiss.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnDismiss.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.btnDismiss.DialogResult = System.Windows.Forms.DialogResult.OK
            Me.btnDismiss.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnDismiss.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnDismiss.ForeColor = System.Drawing.Color.White
            Me.btnDismiss.Location = New System.Drawing.Point(420, 10)
            Me.btnDismiss.Name = "btnDismiss"
            Me.btnDismiss.Size = New System.Drawing.Size(104, 34)
            Me.btnDismiss.TabIndex = 1
            Me.btnDismiss.Text = "Close"
            Me.btnDismiss.UseVisualStyleBackColor = False
            '
            ' FiscalPeriodForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(540, 440)
            Me.Controls.Add(Me.pnlBottom)
            Me.Controls.Add(Me.pnlCompliance)
            Me.Controls.Add(Me.pnlStatus)
            Me.Controls.Add(Me.pnlHeader)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "FiscalPeriodForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent
            Me.Text = "Fiscal Period Status & Month-End Close Guard"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlStatus.ResumeLayout(False)
            Me.pnlStatus.PerformLayout()
            Me.pnlCompliance.ResumeLayout(False)
            Me.pnlCompliance.PerformLayout()
            Me.pnlBottom.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents lblSubtitle As System.Windows.Forms.Label
        Friend WithEvents pnlStatus As System.Windows.Forms.Panel
        Friend WithEvents lblStatusTitle As System.Windows.Forms.Label
        Friend WithEvents lblStatusBadge As System.Windows.Forms.Label
        Friend WithEvents lblPeriodLabel As System.Windows.Forms.Label
        Friend WithEvents lblDateRange As System.Windows.Forms.Label
        Friend WithEvents lblTxSummary As System.Windows.Forms.Label
        Friend WithEvents pnlCompliance As System.Windows.Forms.Panel
        Friend WithEvents lblPolicyHeader As System.Windows.Forms.Label
        Friend WithEvents lblPolicyDetails As System.Windows.Forms.Label
        Friend WithEvents pnlBottom As System.Windows.Forms.Panel
        Friend WithEvents btnClosePeriod As System.Windows.Forms.Button
        Friend WithEvents btnDismiss As System.Windows.Forms.Button

    End Class

End Namespace
