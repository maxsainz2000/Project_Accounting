Namespace UI.Controls

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class FiscalPeriodChipControl
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
            Me.pnlContainer = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblPeriod = New System.Windows.Forms.Label()
            Me.lblStatusPill = New System.Windows.Forms.Label()
            Me.lblDateRange = New System.Windows.Forms.Label()
            Me.pnlContainer.SuspendLayout()
            Me.SuspendLayout()
            '
            ' pnlContainer
            '
            Me.pnlContainer.AutoSize = True
            Me.pnlContainer.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.pnlContainer.Controls.Add(Me.lblPeriod)
            Me.pnlContainer.Controls.Add(Me.lblStatusPill)
            Me.pnlContainer.Controls.Add(Me.lblDateRange)
            Me.pnlContainer.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlContainer.Location = New System.Drawing.Point(0, 0)
            Me.pnlContainer.Name = "pnlContainer"
            Me.pnlContainer.Size = New System.Drawing.Size(350, 28)
            Me.pnlContainer.TabIndex = 0
            Me.pnlContainer.WrapContents = False
            '
            ' lblPeriod
            '
            Me.lblPeriod.AutoSize = True
            Me.lblPeriod.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPeriod.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblPeriod.Location = New System.Drawing.Point(3, 4)
            Me.lblPeriod.Margin = New System.Windows.Forms.Padding(3, 4, 3, 0)
            Me.lblPeriod.Name = "lblPeriod"
            Me.lblPeriod.Size = New System.Drawing.Size(90, 15)
            Me.lblPeriod.TabIndex = 0
            Me.lblPeriod.Text = "FY2026-P11"
            '
            ' lblStatusPill
            '
            Me.lblStatusPill.AutoSize = True
            Me.lblStatusPill.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblStatusPill.Font = New System.Drawing.Font("Segoe UI", 7.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblStatusPill.ForeColor = System.Drawing.Color.White
            Me.lblStatusPill.Location = New System.Drawing.Point(99, 3)
            Me.lblStatusPill.Margin = New System.Windows.Forms.Padding(3, 3, 3, 0)
            Me.lblStatusPill.Name = "lblStatusPill"
            Me.lblStatusPill.Padding = New System.Windows.Forms.Padding(6, 2, 6, 2)
            Me.lblStatusPill.Size = New System.Drawing.Size(46, 16)
            Me.lblStatusPill.TabIndex = 1
            Me.lblStatusPill.Text = "OPEN"
            Me.lblStatusPill.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            ' lblDateRange
            '
            Me.lblDateRange.AutoSize = True
            Me.lblDateRange.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDateRange.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblDateRange.Location = New System.Drawing.Point(151, 4)
            Me.lblDateRange.Margin = New System.Windows.Forms.Padding(3, 4, 3, 0)
            Me.lblDateRange.Name = "lblDateRange"
            Me.lblDateRange.Size = New System.Drawing.Size(130, 13)
            Me.lblDateRange.TabIndex = 2
            Me.lblDateRange.Text = "2026-11-01 to 2026-11-30"
            '
            ' FiscalPeriodChipControl
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.AutoSize = True
            Me.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.Controls.Add(Me.pnlContainer)
            Me.Name = "FiscalPeriodChipControl"
            Me.Size = New System.Drawing.Size(350, 28)
            Me.pnlContainer.ResumeLayout(False)
            Me.pnlContainer.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()
        End Sub

        Friend WithEvents pnlContainer As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblPeriod As System.Windows.Forms.Label
        Friend WithEvents lblStatusPill As System.Windows.Forms.Label
        Friend WithEvents lblDateRange As System.Windows.Forms.Label

    End Class

End Namespace
