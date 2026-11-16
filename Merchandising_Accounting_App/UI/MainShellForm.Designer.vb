Namespace UI

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class MainShellForm
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
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

        'Required by the Windows Form Designer
        Private components As System.ComponentModel.IContainer

        'NOTE: The following procedure is required by the Windows Form Designer
        'It can be modified using the Windows Form Designer.  
        'Do not modify it using the code editor.
        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlSidebar = New System.Windows.Forms.Panel()
            Me.btnFinancialReports = New System.Windows.Forms.Button()
            Me.btnLedger = New System.Windows.Forms.Button()
            Me.btnWarehouse = New System.Windows.Forms.Button()
            Me.btnSales = New System.Windows.Forms.Button()
            Me.btnPurchasing = New System.Windows.Forms.Button()
            Me.pnlLogo = New System.Windows.Forms.Panel()
            Me.lblAppTitle = New System.Windows.Forms.Label()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblModuleSubtitle = New System.Windows.Forms.Label()
            Me.lblCurrentModule = New System.Windows.Forms.Label()
            Me.statusStrip = New System.Windows.Forms.StatusStrip()
            Me.tsLblDbStatus = New System.Windows.Forms.ToolStripStatusLabel()
            Me.tsLblSpacer = New System.Windows.Forms.ToolStripStatusLabel()
            Me.tsLblFiscalPeriod = New System.Windows.Forms.ToolStripStatusLabel()
            Me.tsLblSystemStatus = New System.Windows.Forms.ToolStripStatusLabel()
            Me.pnlContentHost = New System.Windows.Forms.Panel()
            Me.pnlSidebar.SuspendLayout()
            Me.pnlLogo.SuspendLayout()
            Me.pnlHeader.SuspendLayout()
            Me.statusStrip.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlSidebar
            '
            Me.pnlSidebar.BackColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.pnlSidebar.Controls.Add(Me.btnFinancialReports)
            Me.pnlSidebar.Controls.Add(Me.btnLedger)
            Me.pnlSidebar.Controls.Add(Me.btnWarehouse)
            Me.pnlSidebar.Controls.Add(Me.btnSales)
            Me.pnlSidebar.Controls.Add(Me.btnPurchasing)
            Me.pnlSidebar.Controls.Add(Me.pnlLogo)
            Me.pnlSidebar.Dock = System.Windows.Forms.DockStyle.Left
            Me.pnlSidebar.Location = New System.Drawing.Point(0, 0)
            Me.pnlSidebar.Name = "pnlSidebar"
            Me.pnlSidebar.Size = New System.Drawing.Size(270, 720)
            Me.pnlSidebar.TabIndex = 0
            '
            'btnFinancialReports
            '
            Me.btnFinancialReports.Dock = System.Windows.Forms.DockStyle.Top
            Me.btnFinancialReports.FlatAppearance.BorderSize = 0
            Me.btnFinancialReports.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnFinancialReports.Font = New System.Drawing.Font("Segoe UI Variable Text", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.btnFinancialReports.ForeColor = System.Drawing.Color.WhiteSmoke
            Me.btnFinancialReports.Location = New System.Drawing.Point(0, 288)
            Me.btnFinancialReports.Name = "btnFinancialReports"
            Me.btnFinancialReports.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
            Me.btnFinancialReports.Size = New System.Drawing.Size(270, 52)
            Me.btnFinancialReports.TabIndex = 5
            Me.btnFinancialReports.Text = "Financial Reports"
            Me.btnFinancialReports.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnFinancialReports.UseVisualStyleBackColor = True
            '
            'btnLedger
            '
            Me.btnLedger.Dock = System.Windows.Forms.DockStyle.Top
            Me.btnLedger.FlatAppearance.BorderSize = 0
            Me.btnLedger.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnLedger.Font = New System.Drawing.Font("Segoe UI Variable Text", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.btnLedger.ForeColor = System.Drawing.Color.WhiteSmoke
            Me.btnLedger.Location = New System.Drawing.Point(0, 236)
            Me.btnLedger.Name = "btnLedger"
            Me.btnLedger.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
            Me.btnLedger.Size = New System.Drawing.Size(270, 52)
            Me.btnLedger.TabIndex = 4
            Me.btnLedger.Text = "General Ledger && COA"
            Me.btnLedger.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnLedger.UseVisualStyleBackColor = True
            '
            'btnWarehouse
            '
            Me.btnWarehouse.Dock = System.Windows.Forms.DockStyle.Top
            Me.btnWarehouse.FlatAppearance.BorderSize = 0
            Me.btnWarehouse.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnWarehouse.Font = New System.Drawing.Font("Segoe UI Variable Text", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.btnWarehouse.ForeColor = System.Drawing.Color.WhiteSmoke
            Me.btnWarehouse.Location = New System.Drawing.Point(0, 184)
            Me.btnWarehouse.Name = "btnWarehouse"
            Me.btnWarehouse.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
            Me.btnWarehouse.Size = New System.Drawing.Size(270, 52)
            Me.btnWarehouse.TabIndex = 3
            Me.btnWarehouse.Text = "Warehouse && Intake"
            Me.btnWarehouse.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnWarehouse.UseVisualStyleBackColor = True
            '
            'btnSales
            '
            Me.btnSales.Dock = System.Windows.Forms.DockStyle.Top
            Me.btnSales.FlatAppearance.BorderSize = 0
            Me.btnSales.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSales.Font = New System.Drawing.Font("Segoe UI Variable Text", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.btnSales.ForeColor = System.Drawing.Color.WhiteSmoke
            Me.btnSales.Location = New System.Drawing.Point(0, 132)
            Me.btnSales.Name = "btnSales"
            Me.btnSales.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
            Me.btnSales.Size = New System.Drawing.Size(270, 52)
            Me.btnSales.TabIndex = 2
            Me.btnSales.Text = "Sales && Fulfillment"
            Me.btnSales.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnSales.UseVisualStyleBackColor = True
            '
            'btnPurchasing
            '
            Me.btnPurchasing.Dock = System.Windows.Forms.DockStyle.Top
            Me.btnPurchasing.FlatAppearance.BorderSize = 0
            Me.btnPurchasing.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnPurchasing.Font = New System.Drawing.Font("Segoe UI Variable Text", 10.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.btnPurchasing.ForeColor = System.Drawing.Color.WhiteSmoke
            Me.btnPurchasing.Location = New System.Drawing.Point(0, 80)
            Me.btnPurchasing.Name = "btnPurchasing"
            Me.btnPurchasing.Padding = New System.Windows.Forms.Padding(18, 0, 0, 0)
            Me.btnPurchasing.Size = New System.Drawing.Size(270, 52)
            Me.btnPurchasing.TabIndex = 1
            Me.btnPurchasing.Text = "Purchasing && 3-Way Match"
            Me.btnPurchasing.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.btnPurchasing.UseVisualStyleBackColor = True
            '
            'pnlLogo
            '
            Me.pnlLogo.Controls.Add(Me.lblAppTitle)
            Me.pnlLogo.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlLogo.Location = New System.Drawing.Point(0, 0)
            Me.pnlLogo.Name = "pnlLogo"
            Me.pnlLogo.Size = New System.Drawing.Size(270, 80)
            Me.pnlLogo.TabIndex = 0
            '
            'lblAppTitle
            '
            Me.lblAppTitle.AutoSize = True
            Me.lblAppTitle.Font = New System.Drawing.Font("Segoe UI Variable Display", 12.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.lblAppTitle.ForeColor = System.Drawing.Color.White
            Me.lblAppTitle.Location = New System.Drawing.Point(18, 25)
            Me.lblAppTitle.Name = "lblAppTitle"
            Me.lblAppTitle.Size = New System.Drawing.Size(188, 27)
            Me.lblAppTitle.TabIndex = 0
            Me.lblAppTitle.Text = "Merchandising ERP"
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.White
            Me.pnlHeader.Controls.Add(Me.lblModuleSubtitle)
            Me.pnlHeader.Controls.Add(Me.lblCurrentModule)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(270, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Padding = New System.Windows.Forms.Padding(20, 0, 0, 0)
            Me.pnlHeader.Size = New System.Drawing.Size(1010, 64)
            Me.pnlHeader.TabIndex = 1
            '
            'lblModuleSubtitle
            '
            Me.lblModuleSubtitle.AutoSize = True
            Me.lblModuleSubtitle.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblModuleSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblModuleSubtitle.Location = New System.Drawing.Point(20, 36)
            Me.lblModuleSubtitle.Name = "lblModuleSubtitle"
            Me.lblModuleSubtitle.Size = New System.Drawing.Size(320, 20)
            Me.lblModuleSubtitle.TabIndex = 1
            Me.lblModuleSubtitle.Text = "ERP Module Overview and Status Control Panel"
            '
            'lblCurrentModule
            '
            Me.lblCurrentModule.AutoSize = True
            Me.lblCurrentModule.Font = New System.Drawing.Font("Segoe UI Variable Display", 13.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.lblCurrentModule.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblCurrentModule.Location = New System.Drawing.Point(20, 8)
            Me.lblCurrentModule.Name = "lblCurrentModule"
            Me.lblCurrentModule.Size = New System.Drawing.Size(250, 30)
            Me.lblCurrentModule.TabIndex = 0
            Me.lblCurrentModule.Text = "Welcome to Operations"
            '
            'statusStrip
            '
            Me.statusStrip.ImageScalingSize = New System.Drawing.Size(20, 20)
            Me.statusStrip.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.tsLblDbStatus, Me.tsLblSpacer, Me.tsLblFiscalPeriod, Me.tsLblSystemStatus})
            Me.statusStrip.Location = New System.Drawing.Point(270, 694)
            Me.statusStrip.Name = "statusStrip"
            Me.statusStrip.Size = New System.Drawing.Size(1010, 26)
            Me.statusStrip.TabIndex = 2
            Me.statusStrip.Text = "StatusStrip1"
            '
            'tsLblDbStatus
            '
            Me.tsLblDbStatus.Name = "tsLblDbStatus"
            Me.tsLblDbStatus.Size = New System.Drawing.Size(298, 20)
            Me.tsLblDbStatus.Text = "Database: merchandising_erp.db (Mock Mode)"
            '
            'tsLblSpacer
            '
            Me.tsLblSpacer.Name = "tsLblSpacer"
            Me.tsLblSpacer.Size = New System.Drawing.Size(407, 20)
            Me.tsLblSpacer.Spring = True
            '
            'tsLblFiscalPeriod
            '
            Me.tsLblFiscalPeriod.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.tsLblFiscalPeriod.ForeColor = System.Drawing.Color.DarkGreen
            Me.tsLblFiscalPeriod.Name = "tsLblFiscalPeriod"
            Me.tsLblFiscalPeriod.Size = New System.Drawing.Size(175, 20)
            Me.tsLblFiscalPeriod.Text = "Fiscal Period: 2026-11 [OPEN]"
            '
            'tsLblSystemStatus
            '
            Me.tsLblSystemStatus.Name = "tsLblSystemStatus"
            Me.tsLblSystemStatus.Size = New System.Drawing.Size(115, 20)
            Me.tsLblSystemStatus.Text = "Option Strict: ON"
            '
            'pnlContentHost
            '
            Me.pnlContentHost.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlContentHost.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlContentHost.Location = New System.Drawing.Point(270, 64)
            Me.pnlContentHost.Name = "pnlContentHost"
            Me.pnlContentHost.Size = New System.Drawing.Size(1010, 630)
            Me.pnlContentHost.TabIndex = 3
            '
            'MainShellForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.ClientSize = New System.Drawing.Size(1280, 720)
            Me.Controls.Add(Me.pnlContentHost)
            Me.Controls.Add(Me.statusStrip)
            Me.Controls.Add(Me.pnlHeader)
            Me.Controls.Add(Me.pnlSidebar)
            Me.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.MinimumSize = New System.Drawing.Size(1100, 680)
            Me.Name = "MainShellForm"
            Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
            Me.Text = "Merchandising ERP - Accounting & Operations"
            Me.pnlSidebar.ResumeLayout(False)
            Me.pnlLogo.ResumeLayout(False)
            Me.pnlLogo.PerformLayout()
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.statusStrip.ResumeLayout(False)
            Me.statusStrip.PerformLayout()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub

        Friend WithEvents pnlSidebar As System.Windows.Forms.Panel
        Friend WithEvents pnlLogo As System.Windows.Forms.Panel
        Friend WithEvents lblAppTitle As System.Windows.Forms.Label
        Friend WithEvents btnPurchasing As System.Windows.Forms.Button
        Friend WithEvents btnSales As System.Windows.Forms.Button
        Friend WithEvents btnWarehouse As System.Windows.Forms.Button
        Friend WithEvents btnLedger As System.Windows.Forms.Button
        Friend WithEvents btnFinancialReports As System.Windows.Forms.Button
        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblCurrentModule As System.Windows.Forms.Label
        Friend WithEvents lblModuleSubtitle As System.Windows.Forms.Label
        Friend WithEvents statusStrip As System.Windows.Forms.StatusStrip
        Friend WithEvents tsLblDbStatus As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents tsLblSpacer As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents tsLblFiscalPeriod As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents tsLblSystemStatus As System.Windows.Forms.ToolStripStatusLabel
        Friend WithEvents pnlContentHost As System.Windows.Forms.Panel

    End Class

End Namespace
