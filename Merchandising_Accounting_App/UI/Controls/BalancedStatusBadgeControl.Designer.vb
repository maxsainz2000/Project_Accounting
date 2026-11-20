Namespace UI.Controls

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class BalancedStatusBadgeControl
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
            Me.lblBadge = New System.Windows.Forms.Label()
            Me.SuspendLayout()
            '
            ' lblBadge
            '
            Me.lblBadge.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblBadge.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblBadge.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblBadge.ForeColor = System.Drawing.Color.White
            Me.lblBadge.Location = New System.Drawing.Point(0, 0)
            Me.lblBadge.Name = "lblBadge"
            Me.lblBadge.Padding = New System.Windows.Forms.Padding(8, 3, 8, 3)
            Me.lblBadge.Size = New System.Drawing.Size(120, 24)
            Me.lblBadge.TabIndex = 0
            Me.lblBadge.Text = "BALANCED"
            Me.lblBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            ' BalancedStatusBadgeControl
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.Controls.Add(Me.lblBadge)
            Me.Name = "BalancedStatusBadgeControl"
            Me.Size = New System.Drawing.Size(120, 24)
            Me.ResumeLayout(False)
        End Sub

        Friend WithEvents lblBadge As System.Windows.Forms.Label

    End Class

End Namespace
