Namespace UI.Reporting

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class FinancialReportsForm
        Inherits System.Windows.Forms.Form

        'Form overrides dispose to clean up the component list.
        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing Then
                    If _viewModel IsNot Nothing Then
                        _viewModel.Dispose()
                    End If
                    If components IsNot Nothing Then
                        components.Dispose()
                    End If
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
            Dim dgvCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle7 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle8 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle9 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle10 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle11 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle12 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle13 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle14 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle15 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle16 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()

            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.lblSubtitle = New System.Windows.Forms.Label()
            Me.pnlActions = New System.Windows.Forms.FlowLayoutPanel()
            Me.btnExportCsv = New System.Windows.Forms.Button()
            Me.lblPeriodSelector = New System.Windows.Forms.Label()
            Me.cmbPeriod = New System.Windows.Forms.ComboBox()

            Me.tabReports = New System.Windows.Forms.TabControl()

            ' Tab 1: Trial Balance
            Me.tabTrialBalance = New System.Windows.Forms.TabPage()
            Me.pnlTbStatus = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblTbBalancedBadge = New System.Windows.Forms.Label()
            Me.lblTbDiff = New System.Windows.Forms.Label()
            Me.lblTbTotalCredits = New System.Windows.Forms.Label()
            Me.lblTbTotalDebits = New System.Windows.Forms.Label()
            Me.dgvTrialBalance = New System.Windows.Forms.DataGridView()
            Me.colTbCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colTbName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colTbType = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colTbNormal = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colTbDebit = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colTbCredit = New System.Windows.Forms.DataGridViewTextBoxColumn()

            ' Tab 2: Balance Sheet
            Me.tabBalanceSheet = New System.Windows.Forms.TabPage()
            Me.pnlBsSummary = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblBsTotalAssets = New System.Windows.Forms.Label()
            Me.lblBsTotalLiabilities = New System.Windows.Forms.Label()
            Me.lblBsNetAssets = New System.Windows.Forms.Label()
            Me.lblBsTotalEquity = New System.Windows.Forms.Label()
            Me.lblBsVariance = New System.Windows.Forms.Label()
            Me.lblBsBalancedBadge = New System.Windows.Forms.Label()
            Me.dgvBalanceSheet = New System.Windows.Forms.DataGridView()
            Me.colBsCategory = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colBsCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colBsName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colBsAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()

            ' Tab 3: Profit & Loss
            Me.tabProfitLoss = New System.Windows.Forms.TabPage()
            Me.pnlPlSummary = New System.Windows.Forms.Panel()
            Me.lblPlNetIncome = New System.Windows.Forms.Label()
            Me.lblPlVariances = New System.Windows.Forms.Label()
            Me.lblPlGrossProfit = New System.Windows.Forms.Label()
            Me.lblPlNetRevenue = New System.Windows.Forms.Label()
            Me.dgvProfitLoss = New System.Windows.Forms.DataGridView()
            Me.colPlCategory = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPlDescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPlAmount = New System.Windows.Forms.DataGridViewTextBoxColumn()

            ' Tab 4: Product Margins
            Me.tabProductMargins = New System.Windows.Forms.TabPage()
            Me.pnlPmSummary = New System.Windows.Forms.Panel()
            Me.lblPmOverallMargin = New System.Windows.Forms.Label()
            Me.lblPmTotalProfit = New System.Windows.Forms.Label()
            Me.lblPmTotalUnits = New System.Windows.Forms.Label()
            Me.dgvProductMargins = New System.Windows.Forms.DataGridView()
            Me.colPmSku = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPmName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPmUnits = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPmRevenue = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPmCogs = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPmFreight = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPmPpv = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPmNetMargin = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPmMarginPct = New System.Windows.Forms.DataGridViewTextBoxColumn()

            ' Tab 5: AR Aging
            Me.tabArAging = New System.Windows.Forms.TabPage()
            Me.pnlArSummary = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblArCurrent = New System.Windows.Forms.Label()
            Me.lblAr1To30 = New System.Windows.Forms.Label()
            Me.lblAr31To60 = New System.Windows.Forms.Label()
            Me.lblAr61To90 = New System.Windows.Forms.Label()
            Me.lblArOver90 = New System.Windows.Forms.Label()
            Me.lblArTotal = New System.Windows.Forms.Label()
            Me.dgvArAging = New System.Windows.Forms.DataGridView()
            Me.colArCustomerCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colArCustomerName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colArCurrent = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAr1To30 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAr31To60 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAr61To90 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colArOver90 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colArTotal = New System.Windows.Forms.DataGridViewTextBoxColumn()

            ' Tab 6: AP Aging
            Me.tabApAging = New System.Windows.Forms.TabPage()
            Me.pnlApSummary = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblApCurrent = New System.Windows.Forms.Label()
            Me.lblAp1To30 = New System.Windows.Forms.Label()
            Me.lblAp31To60 = New System.Windows.Forms.Label()
            Me.lblAp61To90 = New System.Windows.Forms.Label()
            Me.lblApOver90 = New System.Windows.Forms.Label()
            Me.lblApTotal = New System.Windows.Forms.Label()
            Me.dgvApAging = New System.Windows.Forms.DataGridView()
            Me.colApVendorCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colApVendorName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colApCurrent = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAp1To30 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAp31To60 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAp61To90 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colApOver90 = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colApTotal = New System.Windows.Forms.DataGridViewTextBoxColumn()

            ' Tab 7: Inventory Valuation
            Me.tabStockValuation = New System.Windows.Forms.TabPage()
            Me.pnlSvSummary = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblSvTotalUnits = New System.Windows.Forms.Label()
            Me.lblSvSubledgerValuation = New System.Windows.Forms.Label()
            Me.lblSvGlAccount1300 = New System.Windows.Forms.Label()
            Me.lblSvVariance = New System.Windows.Forms.Label()
            Me.lblSvReconciledBadge = New System.Windows.Forms.Label()
            Me.dgvStockValuation = New System.Windows.Forms.DataGridView()
            Me.colSvSku = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colSvName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colSvCategory = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colSvBaseUnit = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colSvQty = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colSvCost = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colSvValuation = New System.Windows.Forms.DataGridViewTextBoxColumn()

            ' Tab 8: GRNI Aging
            Me.tabGrniAging = New System.Windows.Forms.TabPage()
            Me.pnlGrniSummary = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblGrniOpenCount = New System.Windows.Forms.Label()
            Me.lblGrniUnbilledUnits = New System.Windows.Forms.Label()
            Me.lblGrniAccrualTotal = New System.Windows.Forms.Label()
            Me.lblGrniFlaggedCount = New System.Windows.Forms.Label()
            Me.lblGrniFlaggedAmount = New System.Windows.Forms.Label()
            Me.dgvGrniAging = New System.Windows.Forms.DataGridView()
            Me.colGrniDoc = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colGrniVendor = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colGrniItem = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colGrniReceived = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colGrniBilled = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colGrniUnbilled = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colGrniPoPrice = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colGrniAccrual = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colGrniDaysOpen = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colGrniStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()

            Me.pnlHeader.SuspendLayout()
            Me.pnlActions.SuspendLayout()
            Me.tabReports.SuspendLayout()
            Me.tabTrialBalance.SuspendLayout()
            Me.pnlTbStatus.SuspendLayout()
            CType(Me.dgvTrialBalance, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabBalanceSheet.SuspendLayout()
            Me.pnlBsSummary.SuspendLayout()
            CType(Me.dgvBalanceSheet, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabProfitLoss.SuspendLayout()
            Me.pnlPlSummary.SuspendLayout()
            CType(Me.dgvProfitLoss, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabProductMargins.SuspendLayout()
            Me.pnlPmSummary.SuspendLayout()
            CType(Me.dgvProductMargins, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabArAging.SuspendLayout()
            Me.pnlArSummary.SuspendLayout()
            CType(Me.dgvArAging, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabApAging.SuspendLayout()
            Me.pnlApSummary.SuspendLayout()
            CType(Me.dgvApAging, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabStockValuation.SuspendLayout()
            Me.pnlSvSummary.SuspendLayout()
            CType(Me.dgvStockValuation, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabGrniAging.SuspendLayout()
            Me.pnlGrniSummary.SuspendLayout()
            CType(Me.dgvGrniAging, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()

            '
            ' pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlHeader.Controls.Add(Me.pnlActions)
            Me.pnlHeader.Controls.Add(Me.lblSubtitle)
            Me.pnlHeader.Controls.Add(Me.lblTitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Padding = New System.Windows.Forms.Padding(16, 4, 16, 4)
            Me.pnlHeader.Size = New System.Drawing.Size(1200, 50)
            Me.pnlHeader.TabIndex = 0
            Me.lblTitle.Visible = False
            Me.lblSubtitle.Visible = False

            '
            ' lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 14.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblTitle.Location = New System.Drawing.Point(14, 12)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(415, 26)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Financial Statements & Analytical Reports"
            Me.lblTitle.UseMnemonic = False

            '
            ' lblSubtitle
            '
            Me.lblSubtitle.AutoSize = True
            Me.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblSubtitle.Location = New System.Drawing.Point(16, 42)
            Me.lblSubtitle.Name = "lblSubtitle"
            Me.lblSubtitle.Size = New System.Drawing.Size(530, 17)
            Me.lblSubtitle.TabIndex = 1
            Me.lblSubtitle.Text = "Trial balance verification, Balance Sheet, P&L, AR/AP aging, inventory valuation, and GRNI inquiry"
            Me.lblSubtitle.UseMnemonic = False

            '
            ' pnlActions
            '
            Me.pnlActions.AutoSize = True
            Me.pnlActions.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.pnlActions.BackColor = System.Drawing.Color.Transparent
            Me.pnlActions.Controls.Add(Me.lblPeriodSelector)
            Me.pnlActions.Controls.Add(Me.cmbPeriod)
            Me.pnlActions.Controls.Add(Me.btnExportCsv)
            Me.pnlActions.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlActions.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            Me.pnlActions.Location = New System.Drawing.Point(740, 4)
            Me.pnlActions.Name = "pnlActions"
            Me.pnlActions.Padding = New System.Windows.Forms.Padding(0, 8, 16, 4)
            Me.pnlActions.Size = New System.Drawing.Size(444, 42)
            Me.pnlActions.TabIndex = 2
            Me.pnlActions.WrapContents = False

            '
            ' lblPeriodSelector
            '
            Me.lblPeriodSelector.AutoSize = True
            Me.lblPeriodSelector.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPeriodSelector.Location = New System.Drawing.Point(4, 15)
            Me.lblPeriodSelector.Margin = New System.Windows.Forms.Padding(4, 7, 6, 4)
            Me.lblPeriodSelector.Name = "lblPeriodSelector"
            Me.lblPeriodSelector.Size = New System.Drawing.Size(73, 15)
            Me.lblPeriodSelector.TabIndex = 0
            Me.lblPeriodSelector.Text = "Fiscal Period:"

            '
            ' cmbPeriod
            '
            Me.cmbPeriod.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbPeriod.FormattingEnabled = True
            Me.cmbPeriod.Items.AddRange(New Object() {"FY2026-P11 [OPEN]", "FY2026-P10 [CLOSED]", "FY2026 Full YTD"})
            Me.cmbPeriod.Location = New System.Drawing.Point(87, 12)
            Me.cmbPeriod.Margin = New System.Windows.Forms.Padding(4, 4, 8, 4)
            Me.cmbPeriod.Name = "cmbPeriod"
            Me.cmbPeriod.Size = New System.Drawing.Size(200, 24)
            Me.cmbPeriod.TabIndex = 1

            '
            ' btnExportCsv
            '
            Me.btnExportCsv.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.btnExportCsv.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.btnExportCsv.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnExportCsv.Location = New System.Drawing.Point(299, 10)
            Me.btnExportCsv.Margin = New System.Windows.Forms.Padding(4, 2, 4, 4)
            Me.btnExportCsv.Name = "btnExportCsv"
            Me.btnExportCsv.Size = New System.Drawing.Size(115, 30)
            Me.btnExportCsv.TabIndex = 2
            Me.btnExportCsv.Text = "Export CSV"
            Me.btnExportCsv.UseMnemonic = False
            Me.btnExportCsv.UseVisualStyleBackColor = True

            '
            ' tabReports
            '
            Me.tabReports.Controls.Add(Me.tabTrialBalance)
            Me.tabReports.Controls.Add(Me.tabBalanceSheet)
            Me.tabReports.Controls.Add(Me.tabProfitLoss)
            Me.tabReports.Controls.Add(Me.tabProductMargins)
            Me.tabReports.Controls.Add(Me.tabArAging)
            Me.tabReports.Controls.Add(Me.tabApAging)
            Me.tabReports.Controls.Add(Me.tabStockValuation)
            Me.tabReports.Controls.Add(Me.tabGrniAging)
            Me.tabReports.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tabReports.Location = New System.Drawing.Point(0, 50)
            Me.tabReports.Multiline = True
            Me.tabReports.Name = "tabReports"
            Me.tabReports.Padding = New System.Drawing.Point(12, 6)
            Me.tabReports.SelectedIndex = 0
            Me.tabReports.Size = New System.Drawing.Size(1200, 670)
            Me.tabReports.TabIndex = 1

            '
            ' tabTrialBalance
            '
            Me.tabTrialBalance.BackColor = System.Drawing.Color.White
            Me.tabTrialBalance.Controls.Add(Me.dgvTrialBalance)
            Me.tabTrialBalance.Controls.Add(Me.pnlTbStatus)
            Me.tabTrialBalance.Location = New System.Drawing.Point(4, 30)
            Me.tabTrialBalance.Name = "tabTrialBalance"
            Me.tabTrialBalance.Padding = New System.Windows.Forms.Padding(12)
            Me.tabTrialBalance.Size = New System.Drawing.Size(1192, 636)
            Me.tabTrialBalance.TabIndex = 0
            Me.tabTrialBalance.Text = "Trial Balance"

            '
            ' pnlTbStatus
            '
            Me.pnlTbStatus.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlTbStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlTbStatus.Controls.Add(Me.lblTbTotalDebits)
            Me.pnlTbStatus.Controls.Add(Me.lblTbTotalCredits)
            Me.pnlTbStatus.Controls.Add(Me.lblTbDiff)
            Me.pnlTbStatus.Controls.Add(Me.lblTbBalancedBadge)
            Me.pnlTbStatus.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlTbStatus.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            Me.pnlTbStatus.Location = New System.Drawing.Point(12, 12)
            Me.pnlTbStatus.Name = "pnlTbStatus"
            Me.pnlTbStatus.Padding = New System.Windows.Forms.Padding(10, 8, 10, 8)
            Me.pnlTbStatus.Size = New System.Drawing.Size(1168, 48)
            Me.pnlTbStatus.TabIndex = 0
            Me.pnlTbStatus.AutoSize = True
            Me.pnlTbStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.pnlTbStatus.WrapContents = True

            '
            ' lblTbTotalDebits
            '
            Me.lblTbTotalDebits.AutoSize = True
            Me.lblTbTotalDebits.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTbTotalDebits.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblTbTotalDebits.Location = New System.Drawing.Point(14, 14)
            Me.lblTbTotalDebits.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblTbTotalDebits.Name = "lblTbTotalDebits"
            Me.lblTbTotalDebits.Size = New System.Drawing.Size(160, 17)
            Me.lblTbTotalDebits.TabIndex = 0
            Me.lblTbTotalDebits.Text = "Total Debits: $0.00"

            '
            ' lblTbTotalCredits
            '
            Me.lblTbTotalCredits.AutoSize = True
            Me.lblTbTotalCredits.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTbTotalCredits.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblTbTotalCredits.Location = New System.Drawing.Point(194, 14)
            Me.lblTbTotalCredits.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblTbTotalCredits.Name = "lblTbTotalCredits"
            Me.lblTbTotalCredits.Size = New System.Drawing.Size(164, 17)
            Me.lblTbTotalCredits.TabIndex = 1
            Me.lblTbTotalCredits.Text = "Total Credits: $0.00"

            '
            ' lblTbDiff
            '
            Me.lblTbDiff.AutoSize = True
            Me.lblTbDiff.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTbDiff.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblTbDiff.Location = New System.Drawing.Point(378, 14)
            Me.lblTbDiff.Margin = New System.Windows.Forms.Padding(4, 7, 20, 4)
            Me.lblTbDiff.Name = "lblTbDiff"
            Me.lblTbDiff.Size = New System.Drawing.Size(113, 16)
            Me.lblTbDiff.TabIndex = 2
            Me.lblTbDiff.Text = "Difference: $0.0000"

            '
            ' lblTbBalancedBadge
            '
            Me.lblTbBalancedBadge.AutoSize = True
            Me.lblTbBalancedBadge.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblTbBalancedBadge.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTbBalancedBadge.ForeColor = System.Drawing.Color.White
            Me.lblTbBalancedBadge.Location = New System.Drawing.Point(515, 8)
            Me.lblTbBalancedBadge.Margin = New System.Windows.Forms.Padding(4, 2, 8, 4)
            Me.lblTbBalancedBadge.Name = "lblTbBalancedBadge"
            Me.lblTbBalancedBadge.Padding = New System.Windows.Forms.Padding(10, 5, 10, 5)
            Me.lblTbBalancedBadge.TabIndex = 3
            Me.lblTbBalancedBadge.Text = "TRIAL BALANCE: BALANCED"
            Me.lblTbBalancedBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter

            '
            ' dgvTrialBalance
            '
            Me.dgvTrialBalance.AllowUserToAddRows = False
            Me.dgvTrialBalance.AllowUserToDeleteRows = False
            Me.dgvTrialBalance.AllowUserToResizeRows = False
            Me.dgvTrialBalance.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvTrialBalance.BackgroundColor = System.Drawing.Color.White
            Me.dgvTrialBalance.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvTrialBalance.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvTrialBalance.ColumnHeadersDefaultCellStyle = dgvCellStyle1
            Me.dgvTrialBalance.ColumnHeadersHeight = 32
            Me.dgvTrialBalance.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvTrialBalance.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colTbCode, Me.colTbName, Me.colTbType, Me.colTbNormal, Me.colTbDebit, Me.colTbCredit})
            dgvCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle2.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle2.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvTrialBalance.DefaultCellStyle = dgvCellStyle2
            Me.dgvTrialBalance.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvTrialBalance.EnableHeadersVisualStyles = False
            Me.dgvTrialBalance.Location = New System.Drawing.Point(12, 60)
            Me.dgvTrialBalance.MultiSelect = False
            Me.dgvTrialBalance.Name = "dgvTrialBalance"
            Me.dgvTrialBalance.ReadOnly = True
            Me.dgvTrialBalance.RowHeadersVisible = False
            Me.dgvTrialBalance.RowTemplate.Height = 26
            Me.dgvTrialBalance.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvTrialBalance.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvTrialBalance.Size = New System.Drawing.Size(1168, 564)
            Me.dgvTrialBalance.TabIndex = 1

            '
            ' colTbCode
            '
            Me.colTbCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colTbCode.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colTbCode.HeaderText = "Code"
            Me.colTbCode.MinimumWidth = 60
            Me.colTbCode.Name = "colTbCode"
            Me.colTbCode.ReadOnly = True
            Me.colTbCode.Width = 65

            '
            ' colTbName
            '
            Me.colTbName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colTbName.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colTbName.FillWeight = 160.0!
            Me.colTbName.HeaderText = "Account Title"
            Me.colTbName.MinimumWidth = 220
            Me.colTbName.Name = "colTbName"
            Me.colTbName.ReadOnly = True

            '
            ' colTbType
            '
            Me.colTbType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colTbType.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colTbType.HeaderText = "Type"
            Me.colTbType.MinimumWidth = 150
            Me.colTbType.Name = "colTbType"
            Me.colTbType.ReadOnly = True
            Me.colTbType.Width = 160

            '
            ' colTbNormal
            '
            Me.colTbNormal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colTbNormal.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colTbNormal.HeaderText = "Normal"
            Me.colTbNormal.MinimumWidth = 80
            Me.colTbNormal.Name = "colTbNormal"
            Me.colTbNormal.ReadOnly = True
            Me.colTbNormal.Width = 85

            '
            ' colTbDebit
            '
            Me.colTbDebit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colTbDebit.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colTbDebit.HeaderText = "Debit ($)"
            Me.colTbDebit.MinimumWidth = 120
            Me.colTbDebit.Name = "colTbDebit"
            Me.colTbDebit.ReadOnly = True
            Me.colTbDebit.Width = 125

            '
            ' colTbCredit
            '
            Me.colTbCredit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colTbCredit.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colTbCredit.HeaderText = "Credit ($)"
            Me.colTbCredit.MinimumWidth = 120
            Me.colTbCredit.Name = "colTbCredit"
            Me.colTbCredit.ReadOnly = True
            Me.colTbCredit.Width = 125

            '
            ' tabBalanceSheet
            '
            Me.tabBalanceSheet.BackColor = System.Drawing.Color.White
            Me.tabBalanceSheet.Controls.Add(Me.dgvBalanceSheet)
            Me.tabBalanceSheet.Controls.Add(Me.pnlBsSummary)
            Me.tabBalanceSheet.Location = New System.Drawing.Point(4, 30)
            Me.tabBalanceSheet.Name = "tabBalanceSheet"
            Me.tabBalanceSheet.Padding = New System.Windows.Forms.Padding(12)
            Me.tabBalanceSheet.Size = New System.Drawing.Size(1192, 636)
            Me.tabBalanceSheet.TabIndex = 1
            Me.tabBalanceSheet.Text = "Balance Sheet"

            '
            ' pnlBsSummary
            '
            Me.pnlBsSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlBsSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlBsSummary.Controls.Add(Me.lblBsTotalAssets)
            Me.pnlBsSummary.Controls.Add(Me.lblBsTotalLiabilities)
            Me.pnlBsSummary.Controls.Add(Me.lblBsNetAssets)
            Me.pnlBsSummary.Controls.Add(Me.lblBsTotalEquity)
            Me.pnlBsSummary.Controls.Add(Me.lblBsVariance)
            Me.pnlBsSummary.Controls.Add(Me.lblBsBalancedBadge)
            Me.pnlBsSummary.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlBsSummary.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            Me.pnlBsSummary.Location = New System.Drawing.Point(12, 12)
            Me.pnlBsSummary.Name = "pnlBsSummary"
            Me.pnlBsSummary.Padding = New System.Windows.Forms.Padding(10, 8, 10, 8)
            Me.pnlBsSummary.Size = New System.Drawing.Size(1168, 48)
            Me.pnlBsSummary.TabIndex = 0
            Me.pnlBsSummary.AutoSize = True
            Me.pnlBsSummary.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.pnlBsSummary.WrapContents = True

            '
            ' lblBsTotalAssets
            '
            Me.lblBsTotalAssets.AutoSize = True
            Me.lblBsTotalAssets.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblBsTotalAssets.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblBsTotalAssets.Location = New System.Drawing.Point(14, 14)
            Me.lblBsTotalAssets.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblBsTotalAssets.Name = "lblBsTotalAssets"
            Me.lblBsTotalAssets.Size = New System.Drawing.Size(155, 17)
            Me.lblBsTotalAssets.TabIndex = 0
            Me.lblBsTotalAssets.Text = "Total Assets: $0.00"

            '
            ' lblBsTotalLiabilities
            '
            Me.lblBsTotalLiabilities.AutoSize = True
            Me.lblBsTotalLiabilities.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblBsTotalLiabilities.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblBsTotalLiabilities.Location = New System.Drawing.Point(189, 14)
            Me.lblBsTotalLiabilities.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblBsTotalLiabilities.Name = "lblBsTotalLiabilities"
            Me.lblBsTotalLiabilities.Size = New System.Drawing.Size(175, 17)
            Me.lblBsTotalLiabilities.TabIndex = 1
            Me.lblBsTotalLiabilities.Text = "Total Liabilities: $0.00"

            '
            ' lblBsNetAssets
            '
            Me.lblBsNetAssets.AutoSize = True
            Me.lblBsNetAssets.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblBsNetAssets.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblBsNetAssets.Location = New System.Drawing.Point(384, 14)
            Me.lblBsNetAssets.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblBsNetAssets.Name = "lblBsNetAssets"
            Me.lblBsNetAssets.Size = New System.Drawing.Size(145, 17)
            Me.lblBsNetAssets.TabIndex = 2
            Me.lblBsNetAssets.Text = "Net Assets: $0.00"

            '
            ' lblBsTotalEquity
            '
            Me.lblBsTotalEquity.AutoSize = True
            Me.lblBsTotalEquity.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblBsTotalEquity.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblBsTotalEquity.Location = New System.Drawing.Point(549, 14)
            Me.lblBsTotalEquity.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblBsTotalEquity.Name = "lblBsTotalEquity"
            Me.lblBsTotalEquity.Size = New System.Drawing.Size(150, 17)
            Me.lblBsTotalEquity.TabIndex = 3
            Me.lblBsTotalEquity.Text = "Total Equity: $0.00"

            '
            ' lblBsVariance
            '
            Me.lblBsVariance.AutoSize = True
            Me.lblBsVariance.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblBsVariance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblBsVariance.Location = New System.Drawing.Point(719, 14)
            Me.lblBsVariance.Margin = New System.Windows.Forms.Padding(4, 7, 20, 4)
            Me.lblBsVariance.Name = "lblBsVariance"
            Me.lblBsVariance.Size = New System.Drawing.Size(113, 16)
            Me.lblBsVariance.TabIndex = 4
            Me.lblBsVariance.Text = "Variance: $0.0000"

            '
            ' lblBsBalancedBadge
            '
            Me.lblBsBalancedBadge.AutoSize = True
            Me.lblBsBalancedBadge.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblBsBalancedBadge.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblBsBalancedBadge.ForeColor = System.Drawing.Color.White
            Me.lblBsBalancedBadge.Location = New System.Drawing.Point(856, 8)
            Me.lblBsBalancedBadge.Margin = New System.Windows.Forms.Padding(4, 2, 8, 4)
            Me.lblBsBalancedBadge.Name = "lblBsBalancedBadge"
            Me.lblBsBalancedBadge.Padding = New System.Windows.Forms.Padding(10, 5, 10, 5)
            Me.lblBsBalancedBadge.TabIndex = 5
            Me.lblBsBalancedBadge.Text = "BALANCE SHEET: BALANCED"
            Me.lblBsBalancedBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter

            '
            ' dgvBalanceSheet
            '
            Me.dgvBalanceSheet.AllowUserToAddRows = False
            Me.dgvBalanceSheet.AllowUserToDeleteRows = False
            Me.dgvBalanceSheet.AllowUserToResizeRows = False
            Me.dgvBalanceSheet.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvBalanceSheet.BackgroundColor = System.Drawing.Color.White
            Me.dgvBalanceSheet.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvBalanceSheet.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle3.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvBalanceSheet.ColumnHeadersDefaultCellStyle = dgvCellStyle3
            Me.dgvBalanceSheet.ColumnHeadersHeight = 32
            Me.dgvBalanceSheet.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvBalanceSheet.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colBsCategory, Me.colBsCode, Me.colBsName, Me.colBsAmount})
            dgvCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle4.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle4.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvBalanceSheet.DefaultCellStyle = dgvCellStyle4
            Me.dgvBalanceSheet.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvBalanceSheet.EnableHeadersVisualStyles = False
            Me.dgvBalanceSheet.Location = New System.Drawing.Point(12, 60)
            Me.dgvBalanceSheet.MultiSelect = False
            Me.dgvBalanceSheet.Name = "dgvBalanceSheet"
            Me.dgvBalanceSheet.ReadOnly = True
            Me.dgvBalanceSheet.RowHeadersVisible = False
            Me.dgvBalanceSheet.RowTemplate.Height = 26
            Me.dgvBalanceSheet.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvBalanceSheet.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvBalanceSheet.Size = New System.Drawing.Size(1168, 564)
            Me.dgvBalanceSheet.TabIndex = 1

            '
            ' colBsCategory
            '
            Me.colBsCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colBsCategory.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colBsCategory.HeaderText = "Category"
            Me.colBsCategory.MinimumWidth = 100
            Me.colBsCategory.Name = "colBsCategory"
            Me.colBsCategory.ReadOnly = True
            Me.colBsCategory.Width = 120

            '
            ' colBsCode
            '
            Me.colBsCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colBsCode.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colBsCode.HeaderText = "Code"
            Me.colBsCode.MinimumWidth = 65
            Me.colBsCode.Name = "colBsCode"
            Me.colBsCode.ReadOnly = True
            Me.colBsCode.Width = 75

            '
            ' colBsName
            '
            Me.colBsName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colBsName.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colBsName.FillWeight = 160.0!
            Me.colBsName.HeaderText = "Account Title / Section"
            Me.colBsName.MinimumWidth = 220
            Me.colBsName.Name = "colBsName"
            Me.colBsName.ReadOnly = True

            '
            ' colBsAmount
            '
            Me.colBsAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colBsAmount.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colBsAmount.HeaderText = "Amount ($)"
            Me.colBsAmount.MinimumWidth = 110
            Me.colBsAmount.Name = "colBsAmount"
            Me.colBsAmount.ReadOnly = True
            Me.colBsAmount.Width = 130

            '
            ' tabProfitLoss
            '
            Me.tabProfitLoss.BackColor = System.Drawing.Color.White
            Me.tabProfitLoss.Controls.Add(Me.dgvProfitLoss)
            Me.tabProfitLoss.Controls.Add(Me.pnlPlSummary)
            Me.tabProfitLoss.Location = New System.Drawing.Point(4, 30)
            Me.tabProfitLoss.Name = "tabProfitLoss"
            Me.tabProfitLoss.Padding = New System.Windows.Forms.Padding(12)
            Me.tabProfitLoss.Size = New System.Drawing.Size(1192, 636)
            Me.tabProfitLoss.TabIndex = 2
            Me.tabProfitLoss.Text = "Income Statement"

            '
            ' pnlPlSummary
            '
            Me.pnlPlSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlPlSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlPlSummary.Controls.Add(Me.lblPlNetIncome)
            Me.pnlPlSummary.Controls.Add(Me.lblPlVariances)
            Me.pnlPlSummary.Controls.Add(Me.lblPlGrossProfit)
            Me.pnlPlSummary.Controls.Add(Me.lblPlNetRevenue)
            Me.pnlPlSummary.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlPlSummary.Location = New System.Drawing.Point(12, 12)
            Me.pnlPlSummary.Name = "pnlPlSummary"
            Me.pnlPlSummary.Padding = New System.Windows.Forms.Padding(10)
            Me.pnlPlSummary.Size = New System.Drawing.Size(1168, 48)
            Me.pnlPlSummary.TabIndex = 0

            '
            ' lblPlNetRevenue
            '
            Me.lblPlNetRevenue.AutoSize = True
            Me.lblPlNetRevenue.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPlNetRevenue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblPlNetRevenue.Location = New System.Drawing.Point(15, 14)
            Me.lblPlNetRevenue.Name = "lblPlNetRevenue"
            Me.lblPlNetRevenue.Size = New System.Drawing.Size(130, 16)
            Me.lblPlNetRevenue.TabIndex = 0
            Me.lblPlNetRevenue.Text = "Net Revenue: $0.00"

            '
            ' lblPlGrossProfit
            '
            Me.lblPlGrossProfit.AutoSize = True
            Me.lblPlGrossProfit.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPlGrossProfit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblPlGrossProfit.Location = New System.Drawing.Point(220, 14)
            Me.lblPlGrossProfit.Name = "lblPlGrossProfit"
            Me.lblPlGrossProfit.Size = New System.Drawing.Size(125, 16)
            Me.lblPlGrossProfit.TabIndex = 1
            Me.lblPlGrossProfit.Text = "Gross Profit: $0.00"

            '
            ' lblPlVariances
            '
            Me.lblPlVariances.AutoSize = True
            Me.lblPlVariances.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPlVariances.ForeColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.lblPlVariances.Location = New System.Drawing.Point(420, 14)
            Me.lblPlVariances.Name = "lblPlVariances"
            Me.lblPlVariances.Size = New System.Drawing.Size(175, 16)
            Me.lblPlVariances.TabIndex = 2
            Me.lblPlVariances.Text = "Operating Variances: $0.00"

            '
            ' lblPlNetIncome
            '
            Me.lblPlNetIncome.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblPlNetIncome.AutoSize = True
            Me.lblPlNetIncome.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPlNetIncome.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblPlNetIncome.Location = New System.Drawing.Point(880, 14)
            Me.lblPlNetIncome.Name = "lblPlNetIncome"
            Me.lblPlNetIncome.Size = New System.Drawing.Size(225, 17)
            Me.lblPlNetIncome.TabIndex = 3
            Me.lblPlNetIncome.Text = "Net Operating Income: $0.00"

            '
            ' dgvProfitLoss
            '
            Me.dgvProfitLoss.AllowUserToAddRows = False
            Me.dgvProfitLoss.AllowUserToDeleteRows = False
            Me.dgvProfitLoss.AllowUserToResizeRows = False
            Me.dgvProfitLoss.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvProfitLoss.BackgroundColor = System.Drawing.Color.White
            Me.dgvProfitLoss.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvProfitLoss.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dgvProfitLoss.ColumnHeadersDefaultCellStyle = dgvCellStyle5
            Me.dgvProfitLoss.ColumnHeadersHeight = 30
            Me.dgvProfitLoss.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvProfitLoss.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPlCategory, Me.colPlDescription, Me.colPlAmount})
            dgvCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle6.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle6.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle6.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvProfitLoss.DefaultCellStyle = dgvCellStyle6
            Me.dgvProfitLoss.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvProfitLoss.EnableHeadersVisualStyles = False
            Me.dgvProfitLoss.Location = New System.Drawing.Point(12, 60)
            Me.dgvProfitLoss.MultiSelect = False
            Me.dgvProfitLoss.Name = "dgvProfitLoss"
            Me.dgvProfitLoss.ReadOnly = True
            Me.dgvProfitLoss.RowHeadersVisible = False
            Me.dgvProfitLoss.RowTemplate.Height = 26
            Me.dgvProfitLoss.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvProfitLoss.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvProfitLoss.Size = New System.Drawing.Size(1168, 564)
            Me.dgvProfitLoss.TabIndex = 1

            '
            ' colPlCategory
            '
            Me.colPlCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPlCategory.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colPlCategory.HeaderText = "Category"
            Me.colPlCategory.MinimumWidth = 90
            Me.colPlCategory.Name = "colPlCategory"
            Me.colPlCategory.ReadOnly = True
            Me.colPlCategory.Width = 140

            '
            ' colPlDescription
            '
            Me.colPlDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colPlDescription.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colPlDescription.FillWeight = 160.0!
            Me.colPlDescription.HeaderText = "Line Item Description"
            Me.colPlDescription.MinimumWidth = 160
            Me.colPlDescription.Name = "colPlDescription"
            Me.colPlDescription.ReadOnly = True

            '
            ' colPlAmount
            '
            Me.colPlAmount.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPlAmount.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colPlAmount.HeaderText = "Amount ($)"
            Me.colPlAmount.MinimumWidth = 100
            Me.colPlAmount.Name = "colPlAmount"
            Me.colPlAmount.ReadOnly = True
            Me.colPlAmount.Width = 130

            '
            ' tabProductMargins
            '
            Me.tabProductMargins.BackColor = System.Drawing.Color.White
            Me.tabProductMargins.Controls.Add(Me.dgvProductMargins)
            Me.tabProductMargins.Controls.Add(Me.pnlPmSummary)
            Me.tabProductMargins.Location = New System.Drawing.Point(4, 30)
            Me.tabProductMargins.Name = "tabProductMargins"
            Me.tabProductMargins.Padding = New System.Windows.Forms.Padding(12)
            Me.tabProductMargins.Size = New System.Drawing.Size(1192, 636)
            Me.tabProductMargins.TabIndex = 3
            Me.tabProductMargins.Text = "Product Margins"

            '
            ' pnlPmSummary
            '
            Me.pnlPmSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlPmSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlPmSummary.Controls.Add(Me.lblPmOverallMargin)
            Me.pnlPmSummary.Controls.Add(Me.lblPmTotalProfit)
            Me.pnlPmSummary.Controls.Add(Me.lblPmTotalUnits)
            Me.pnlPmSummary.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlPmSummary.Location = New System.Drawing.Point(12, 12)
            Me.pnlPmSummary.Name = "pnlPmSummary"
            Me.pnlPmSummary.Padding = New System.Windows.Forms.Padding(10)
            Me.pnlPmSummary.Size = New System.Drawing.Size(1168, 48)
            Me.pnlPmSummary.TabIndex = 0

            '
            ' lblPmTotalUnits
            '
            Me.lblPmTotalUnits.AutoSize = True
            Me.lblPmTotalUnits.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPmTotalUnits.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblPmTotalUnits.Location = New System.Drawing.Point(15, 14)
            Me.lblPmTotalUnits.Name = "lblPmTotalUnits"
            Me.lblPmTotalUnits.Size = New System.Drawing.Size(140, 16)
            Me.lblPmTotalUnits.TabIndex = 0
            Me.lblPmTotalUnits.Text = "Total Units Sold: 0"

            '
            ' lblPmTotalProfit
            '
            Me.lblPmTotalProfit.AutoSize = True
            Me.lblPmTotalProfit.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPmTotalProfit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblPmTotalProfit.Location = New System.Drawing.Point(235, 14)
            Me.lblPmTotalProfit.Name = "lblPmTotalProfit"
            Me.lblPmTotalProfit.Size = New System.Drawing.Size(200, 16)
            Me.lblPmTotalProfit.TabIndex = 1
            Me.lblPmTotalProfit.Text = "Total Net Landed Margin: $0.00"

            '
            ' lblPmOverallMargin
            '
            Me.lblPmOverallMargin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblPmOverallMargin.AutoSize = True
            Me.lblPmOverallMargin.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPmOverallMargin.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblPmOverallMargin.Location = New System.Drawing.Point(880, 14)
            Me.lblPmOverallMargin.Name = "lblPmOverallMargin"
            Me.lblPmOverallMargin.Size = New System.Drawing.Size(220, 17)
            Me.lblPmOverallMargin.TabIndex = 2
            Me.lblPmOverallMargin.Text = "Weighted Landed Margin: 0.00%"

            '
            ' dgvProductMargins
            '
            Me.dgvProductMargins.AllowUserToAddRows = False
            Me.dgvProductMargins.AllowUserToDeleteRows = False
            Me.dgvProductMargins.AllowUserToResizeRows = False
            Me.dgvProductMargins.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvProductMargins.BackgroundColor = System.Drawing.Color.White
            Me.dgvProductMargins.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvProductMargins.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle7.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle7.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle7.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle7.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle7.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle7.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle7.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dgvProductMargins.ColumnHeadersDefaultCellStyle = dgvCellStyle7
            Me.dgvProductMargins.ColumnHeadersHeight = 30
            Me.dgvProductMargins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvProductMargins.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPmSku, Me.colPmName, Me.colPmUnits, Me.colPmRevenue, Me.colPmCogs, Me.colPmFreight, Me.colPmPpv, Me.colPmNetMargin, Me.colPmMarginPct})
            dgvCellStyle8.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle8.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle8.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle8.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle8.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle8.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle8.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvProductMargins.DefaultCellStyle = dgvCellStyle8
            Me.dgvProductMargins.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvProductMargins.EnableHeadersVisualStyles = False
            Me.dgvProductMargins.Location = New System.Drawing.Point(12, 60)
            Me.dgvProductMargins.MultiSelect = False
            Me.dgvProductMargins.Name = "dgvProductMargins"
            Me.dgvProductMargins.ReadOnly = True
            Me.dgvProductMargins.RowHeadersVisible = False
            Me.dgvProductMargins.RowTemplate.Height = 26
            Me.dgvProductMargins.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvProductMargins.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvProductMargins.Size = New System.Drawing.Size(1168, 564)
            Me.dgvProductMargins.TabIndex = 1

            '
            ' colPmSku
            '
            Me.colPmSku.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPmSku.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colPmSku.HeaderText = "SKU"
            Me.colPmSku.MinimumWidth = 85
            Me.colPmSku.Name = "colPmSku"
            Me.colPmSku.ReadOnly = True
            Me.colPmSku.Width = 95

            '
            ' colPmName
            '
            Me.colPmName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colPmName.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colPmName.FillWeight = 110.0!
            Me.colPmName.HeaderText = "Product Name"
            Me.colPmName.MinimumWidth = 140
            Me.colPmName.Name = "colPmName"
            Me.colPmName.ReadOnly = True

            '
            ' colPmUnits
            '
            Me.colPmUnits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPmUnits.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N0"}
            Me.colPmUnits.HeaderText = "Units"
            Me.colPmUnits.MinimumWidth = 70
            Me.colPmUnits.Name = "colPmUnits"
            Me.colPmUnits.ReadOnly = True
            Me.colPmUnits.Width = 75

            '
            ' colPmRevenue
            '
            Me.colPmRevenue.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPmRevenue.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colPmRevenue.HeaderText = "Revenue"
            Me.colPmRevenue.MinimumWidth = 95
            Me.colPmRevenue.Name = "colPmRevenue"
            Me.colPmRevenue.ReadOnly = True
            Me.colPmRevenue.Width = 100

            '
            ' colPmCogs
            '
            Me.colPmCogs.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPmCogs.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colPmCogs.HeaderText = "COGS"
            Me.colPmCogs.MinimumWidth = 95
            Me.colPmCogs.Name = "colPmCogs"
            Me.colPmCogs.ReadOnly = True
            Me.colPmCogs.Width = 100

            '
            ' colPmFreight
            '
            Me.colPmFreight.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPmFreight.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colPmFreight.HeaderText = "Freight"
            Me.colPmFreight.MinimumWidth = 85
            Me.colPmFreight.Name = "colPmFreight"
            Me.colPmFreight.ReadOnly = True
            Me.colPmFreight.Width = 90

            '
            ' colPmPpv
            '
            Me.colPmPpv.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPmPpv.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colPmPpv.HeaderText = "PPV"
            Me.colPmPpv.MinimumWidth = 85
            Me.colPmPpv.Name = "colPmPpv"
            Me.colPmPpv.ReadOnly = True
            Me.colPmPpv.Width = 90

            '
            ' colPmNetMargin
            '
            Me.colPmNetMargin.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPmNetMargin.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colPmNetMargin.HeaderText = "Net Margin"
            Me.colPmNetMargin.MinimumWidth = 95
            Me.colPmNetMargin.Name = "colPmNetMargin"
            Me.colPmNetMargin.ReadOnly = True
            Me.colPmNetMargin.Width = 105

            '
            ' colPmMarginPct
            '
            Me.colPmMarginPct.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPmMarginPct.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "0.0'%'"}
            Me.colPmMarginPct.HeaderText = "Margin %"
            Me.colPmMarginPct.MinimumWidth = 80
            Me.colPmMarginPct.Name = "colPmMarginPct"
            Me.colPmMarginPct.ReadOnly = True
            Me.colPmMarginPct.Width = 85

            '
            ' tabArAging
            '
            Me.tabArAging.BackColor = System.Drawing.Color.White
            Me.tabArAging.Controls.Add(Me.dgvArAging)
            Me.tabArAging.Controls.Add(Me.pnlArSummary)
            Me.tabArAging.Location = New System.Drawing.Point(4, 30)
            Me.tabArAging.Name = "tabArAging"
            Me.tabArAging.Padding = New System.Windows.Forms.Padding(12)
            Me.tabArAging.Size = New System.Drawing.Size(1192, 636)
            Me.tabArAging.TabIndex = 4
            Me.tabArAging.Text = "A/R Aging"

            '
            ' pnlArSummary
            '
            Me.pnlArSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlArSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlArSummary.Controls.Add(Me.lblArCurrent)
            Me.pnlArSummary.Controls.Add(Me.lblAr1To30)
            Me.pnlArSummary.Controls.Add(Me.lblAr31To60)
            Me.pnlArSummary.Controls.Add(Me.lblAr61To90)
            Me.pnlArSummary.Controls.Add(Me.lblArOver90)
            Me.pnlArSummary.Controls.Add(Me.lblArTotal)
            Me.pnlArSummary.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlArSummary.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            Me.pnlArSummary.Location = New System.Drawing.Point(12, 12)
            Me.pnlArSummary.Name = "pnlArSummary"
            Me.pnlArSummary.Padding = New System.Windows.Forms.Padding(10, 8, 10, 8)
            Me.pnlArSummary.Size = New System.Drawing.Size(1168, 48)
            Me.pnlArSummary.TabIndex = 0
            Me.pnlArSummary.AutoSize = True
            Me.pnlArSummary.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.pnlArSummary.WrapContents = True

            '
            ' lblArCurrent
            '
            Me.lblArCurrent.AutoSize = True
            Me.lblArCurrent.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblArCurrent.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblArCurrent.Location = New System.Drawing.Point(14, 14)
            Me.lblArCurrent.Margin = New System.Windows.Forms.Padding(4, 6, 12, 4)
            Me.lblArCurrent.Name = "lblArCurrent"
            Me.lblArCurrent.Size = New System.Drawing.Size(95, 15)
            Me.lblArCurrent.TabIndex = 0
            Me.lblArCurrent.Text = "Current: $0.00"

            '
            ' lblAr1To30
            '
            Me.lblAr1To30.AutoSize = True
            Me.lblAr1To30.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAr1To30.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblAr1To30.Location = New System.Drawing.Point(125, 14)
            Me.lblAr1To30.Margin = New System.Windows.Forms.Padding(4, 6, 12, 4)
            Me.lblAr1To30.Name = "lblAr1To30"
            Me.lblAr1To30.Size = New System.Drawing.Size(102, 15)
            Me.lblAr1To30.TabIndex = 1
            Me.lblAr1To30.Text = "1-30 Days: $0.00"

            '
            ' lblAr31To60
            '
            Me.lblAr31To60.AutoSize = True
            Me.lblAr31To60.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAr31To60.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblAr31To60.Location = New System.Drawing.Point(245, 14)
            Me.lblAr31To60.Margin = New System.Windows.Forms.Padding(4, 6, 12, 4)
            Me.lblAr31To60.Name = "lblAr31To60"
            Me.lblAr31To60.Size = New System.Drawing.Size(109, 15)
            Me.lblAr31To60.TabIndex = 2
            Me.lblAr31To60.Text = "31-60 Days: $0.00"

            '
            ' lblAr61To90
            '
            Me.lblAr61To90.AutoSize = True
            Me.lblAr61To90.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAr61To90.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblAr61To90.Location = New System.Drawing.Point(370, 14)
            Me.lblAr61To90.Margin = New System.Windows.Forms.Padding(4, 6, 12, 4)
            Me.lblAr61To90.Name = "lblAr61To90"
            Me.lblAr61To90.Size = New System.Drawing.Size(109, 15)
            Me.lblAr61To90.TabIndex = 3
            Me.lblAr61To90.Text = "61-90 Days: $0.00"

            '
            ' lblArOver90
            '
            Me.lblArOver90.AutoSize = True
            Me.lblArOver90.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblArOver90.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblArOver90.Location = New System.Drawing.Point(495, 14)
            Me.lblArOver90.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblArOver90.Name = "lblArOver90"
            Me.lblArOver90.Size = New System.Drawing.Size(95, 15)
            Me.lblArOver90.TabIndex = 4
            Me.lblArOver90.Text = "90+ Days: $0.00"

            '
            ' lblArTotal
            '
            Me.lblArTotal.AutoSize = True
            Me.lblArTotal.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblArTotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblArTotal.Location = New System.Drawing.Point(610, 14)
            Me.lblArTotal.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblArTotal.Name = "lblArTotal"
            Me.lblArTotal.Size = New System.Drawing.Size(125, 17)
            Me.lblArTotal.TabIndex = 5
            Me.lblArTotal.Text = "Total AR: $0.00"

            '
            ' dgvArAging
            '
            Me.dgvArAging.AllowUserToAddRows = False
            Me.dgvArAging.AllowUserToDeleteRows = False
            Me.dgvArAging.AllowUserToResizeRows = False
            Me.dgvArAging.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvArAging.BackgroundColor = System.Drawing.Color.White
            Me.dgvArAging.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvArAging.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle9.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle9.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle9.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle9.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle9.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle9.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle9.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvArAging.ColumnHeadersDefaultCellStyle = dgvCellStyle9
            Me.dgvArAging.ColumnHeadersHeight = 32
            Me.dgvArAging.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvArAging.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colArCustomerCode, Me.colArCustomerName, Me.colArCurrent, Me.colAr1To30, Me.colAr31To60, Me.colAr61To90, Me.colArOver90, Me.colArTotal})
            dgvCellStyle10.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle10.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle10.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle10.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle10.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle10.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle10.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvArAging.DefaultCellStyle = dgvCellStyle10
            Me.dgvArAging.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvArAging.EnableHeadersVisualStyles = False
            Me.dgvArAging.Location = New System.Drawing.Point(12, 60)
            Me.dgvArAging.MultiSelect = False
            Me.dgvArAging.Name = "dgvArAging"
            Me.dgvArAging.ReadOnly = True
            Me.dgvArAging.RowHeadersVisible = False
            Me.dgvArAging.RowTemplate.Height = 26
            Me.dgvArAging.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvArAging.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvArAging.Size = New System.Drawing.Size(1168, 564)
            Me.dgvArAging.TabIndex = 1

            '
            ' colArCustomerCode
            '
            Me.colArCustomerCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colArCustomerCode.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colArCustomerCode.HeaderText = "Customer Code"
            Me.colArCustomerCode.MinimumWidth = 90
            Me.colArCustomerCode.Name = "colArCustomerCode"
            Me.colArCustomerCode.ReadOnly = True
            Me.colArCustomerCode.Width = 110

            '
            ' colArCustomerName
            '
            Me.colArCustomerName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colArCustomerName.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colArCustomerName.FillWeight = 160.0!
            Me.colArCustomerName.HeaderText = "Customer Name"
            Me.colArCustomerName.MinimumWidth = 160
            Me.colArCustomerName.Name = "colArCustomerName"
            Me.colArCustomerName.ReadOnly = True

            '
            ' colArCurrent
            '
            Me.colArCurrent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colArCurrent.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colArCurrent.HeaderText = "Current ($)"
            Me.colArCurrent.MinimumWidth = 95
            Me.colArCurrent.Name = "colArCurrent"
            Me.colArCurrent.ReadOnly = True
            Me.colArCurrent.Width = 105

            '
            ' colAr1To30
            '
            Me.colAr1To30.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colAr1To30.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colAr1To30.HeaderText = "1-30 Days ($)"
            Me.colAr1To30.MinimumWidth = 95
            Me.colAr1To30.Name = "colAr1To30"
            Me.colAr1To30.ReadOnly = True
            Me.colAr1To30.Width = 105

            '
            ' colAr31To60
            '
            Me.colAr31To60.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colAr31To60.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colAr31To60.HeaderText = "31-60 Days ($)"
            Me.colAr31To60.MinimumWidth = 95
            Me.colAr31To60.Name = "colAr31To60"
            Me.colAr31To60.ReadOnly = True
            Me.colAr31To60.Width = 105

            '
            ' colAr61To90
            '
            Me.colAr61To90.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colAr61To90.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colAr61To90.HeaderText = "61-90 Days ($)"
            Me.colAr61To90.MinimumWidth = 95
            Me.colAr61To90.Name = "colAr61To90"
            Me.colAr61To90.ReadOnly = True
            Me.colAr61To90.Width = 105

            '
            ' colArOver90
            '
            Me.colArOver90.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colArOver90.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colArOver90.HeaderText = "90+ Days ($)"
            Me.colArOver90.MinimumWidth = 95
            Me.colArOver90.Name = "colArOver90"
            Me.colArOver90.ReadOnly = True
            Me.colArOver90.Width = 105

            '
            ' colArTotal
            '
            Me.colArTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colArTotal.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colArTotal.HeaderText = "Total Balance ($)"
            Me.colArTotal.MinimumWidth = 110
            Me.colArTotal.Name = "colArTotal"
            Me.colArTotal.ReadOnly = True
            Me.colArTotal.Width = 120

            '
            ' tabApAging
            '
            Me.tabApAging.BackColor = System.Drawing.Color.White
            Me.tabApAging.Controls.Add(Me.dgvApAging)
            Me.tabApAging.Controls.Add(Me.pnlApSummary)
            Me.tabApAging.Location = New System.Drawing.Point(4, 30)
            Me.tabApAging.Name = "tabApAging"
            Me.tabApAging.Padding = New System.Windows.Forms.Padding(12)
            Me.tabApAging.Size = New System.Drawing.Size(1192, 636)
            Me.tabApAging.TabIndex = 5
            Me.tabApAging.Text = "A/P Aging"

            '
            ' pnlApSummary
            '
            Me.pnlApSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlApSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlApSummary.Controls.Add(Me.lblApCurrent)
            Me.pnlApSummary.Controls.Add(Me.lblAp1To30)
            Me.pnlApSummary.Controls.Add(Me.lblAp31To60)
            Me.pnlApSummary.Controls.Add(Me.lblAp61To90)
            Me.pnlApSummary.Controls.Add(Me.lblApOver90)
            Me.pnlApSummary.Controls.Add(Me.lblApTotal)
            Me.pnlApSummary.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlApSummary.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            Me.pnlApSummary.Location = New System.Drawing.Point(12, 12)
            Me.pnlApSummary.Name = "pnlApSummary"
            Me.pnlApSummary.Padding = New System.Windows.Forms.Padding(10, 8, 10, 8)
            Me.pnlApSummary.Size = New System.Drawing.Size(1168, 48)
            Me.pnlApSummary.TabIndex = 0
            Me.pnlApSummary.AutoSize = True
            Me.pnlApSummary.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.pnlApSummary.WrapContents = True

            '
            ' lblApCurrent
            '
            Me.lblApCurrent.AutoSize = True
            Me.lblApCurrent.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblApCurrent.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblApCurrent.Location = New System.Drawing.Point(14, 14)
            Me.lblApCurrent.Margin = New System.Windows.Forms.Padding(4, 6, 12, 4)
            Me.lblApCurrent.Name = "lblApCurrent"
            Me.lblApCurrent.Size = New System.Drawing.Size(95, 15)
            Me.lblApCurrent.TabIndex = 0
            Me.lblApCurrent.Text = "Current: $0.00"

            '
            ' lblAp1To30
            '
            Me.lblAp1To30.AutoSize = True
            Me.lblAp1To30.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAp1To30.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblAp1To30.Location = New System.Drawing.Point(125, 14)
            Me.lblAp1To30.Margin = New System.Windows.Forms.Padding(4, 6, 12, 4)
            Me.lblAp1To30.Name = "lblAp1To30"
            Me.lblAp1To30.Size = New System.Drawing.Size(102, 15)
            Me.lblAp1To30.TabIndex = 1
            Me.lblAp1To30.Text = "1-30 Days: $0.00"

            '
            ' lblAp31To60
            '
            Me.lblAp31To60.AutoSize = True
            Me.lblAp31To60.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAp31To60.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblAp31To60.Location = New System.Drawing.Point(245, 14)
            Me.lblAp31To60.Margin = New System.Windows.Forms.Padding(4, 6, 12, 4)
            Me.lblAp31To60.Name = "lblAp31To60"
            Me.lblAp31To60.Size = New System.Drawing.Size(109, 15)
            Me.lblAp31To60.TabIndex = 2
            Me.lblAp31To60.Text = "31-60 Days: $0.00"

            '
            ' lblAp61To90
            '
            Me.lblAp61To90.AutoSize = True
            Me.lblAp61To90.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAp61To90.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblAp61To90.Location = New System.Drawing.Point(370, 14)
            Me.lblAp61To90.Margin = New System.Windows.Forms.Padding(4, 6, 12, 4)
            Me.lblAp61To90.Name = "lblAp61To90"
            Me.lblAp61To90.Size = New System.Drawing.Size(109, 15)
            Me.lblAp61To90.TabIndex = 3
            Me.lblAp61To90.Text = "61-90 Days: $0.00"

            '
            ' lblApOver90
            '
            Me.lblApOver90.AutoSize = True
            Me.lblApOver90.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblApOver90.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblApOver90.Location = New System.Drawing.Point(495, 14)
            Me.lblApOver90.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblApOver90.Name = "lblApOver90"
            Me.lblApOver90.Size = New System.Drawing.Size(95, 15)
            Me.lblApOver90.TabIndex = 4
            Me.lblApOver90.Text = "90+ Days: $0.00"

            '
            ' lblApTotal
            '
            Me.lblApTotal.AutoSize = True
            Me.lblApTotal.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblApTotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblApTotal.Location = New System.Drawing.Point(610, 14)
            Me.lblApTotal.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblApTotal.Name = "lblApTotal"
            Me.lblApTotal.Size = New System.Drawing.Size(125, 17)
            Me.lblApTotal.TabIndex = 5
            Me.lblApTotal.Text = "Total AP: $0.00"

            '
            ' dgvApAging
            '
            Me.dgvApAging.AllowUserToAddRows = False
            Me.dgvApAging.AllowUserToDeleteRows = False
            Me.dgvApAging.AllowUserToResizeRows = False
            Me.dgvApAging.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvApAging.BackgroundColor = System.Drawing.Color.White
            Me.dgvApAging.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvApAging.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle11.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle11.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle11.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle11.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle11.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle11.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle11.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvApAging.ColumnHeadersDefaultCellStyle = dgvCellStyle11
            Me.dgvApAging.ColumnHeadersHeight = 32
            Me.dgvApAging.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvApAging.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colApVendorCode, Me.colApVendorName, Me.colApCurrent, Me.colAp1To30, Me.colAp31To60, Me.colAp61To90, Me.colApOver90, Me.colApTotal})
            dgvCellStyle12.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle12.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle12.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle12.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle12.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle12.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle12.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvApAging.DefaultCellStyle = dgvCellStyle12
            Me.dgvApAging.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvApAging.EnableHeadersVisualStyles = False
            Me.dgvApAging.Location = New System.Drawing.Point(12, 60)
            Me.dgvApAging.MultiSelect = False
            Me.dgvApAging.Name = "dgvApAging"
            Me.dgvApAging.ReadOnly = True
            Me.dgvApAging.RowHeadersVisible = False
            Me.dgvApAging.RowTemplate.Height = 26
            Me.dgvApAging.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvApAging.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvApAging.Size = New System.Drawing.Size(1168, 564)
            Me.dgvApAging.TabIndex = 1

            '
            ' colApVendorCode
            '
            Me.colApVendorCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colApVendorCode.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colApVendorCode.HeaderText = "Vendor Code"
            Me.colApVendorCode.MinimumWidth = 90
            Me.colApVendorCode.Name = "colApVendorCode"
            Me.colApVendorCode.ReadOnly = True
            Me.colApVendorCode.Width = 110

            '
            ' colApVendorName
            '
            Me.colApVendorName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colApVendorName.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colApVendorName.FillWeight = 160.0!
            Me.colApVendorName.HeaderText = "Vendor Name"
            Me.colApVendorName.MinimumWidth = 160
            Me.colApVendorName.Name = "colApVendorName"
            Me.colApVendorName.ReadOnly = True

            '
            ' colApCurrent
            '
            Me.colApCurrent.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colApCurrent.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colApCurrent.HeaderText = "Current ($)"
            Me.colApCurrent.MinimumWidth = 95
            Me.colApCurrent.Name = "colApCurrent"
            Me.colApCurrent.ReadOnly = True
            Me.colApCurrent.Width = 105

            '
            ' colAp1To30
            '
            Me.colAp1To30.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colAp1To30.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colAp1To30.HeaderText = "1-30 Days ($)"
            Me.colAp1To30.MinimumWidth = 95
            Me.colAp1To30.Name = "colAp1To30"
            Me.colAp1To30.ReadOnly = True
            Me.colAp1To30.Width = 105

            '
            ' colAp31To60
            '
            Me.colAp31To60.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colAp31To60.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colAp31To60.HeaderText = "31-60 Days ($)"
            Me.colAp31To60.MinimumWidth = 95
            Me.colAp31To60.Name = "colAp31To60"
            Me.colAp31To60.ReadOnly = True
            Me.colAp31To60.Width = 105

            '
            ' colAp61To90
            '
            Me.colAp61To90.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colAp61To90.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colAp61To90.HeaderText = "61-90 Days ($)"
            Me.colAp61To90.MinimumWidth = 95
            Me.colAp61To90.Name = "colAp61To90"
            Me.colAp61To90.ReadOnly = True
            Me.colAp61To90.Width = 105

            '
            ' colApOver90
            '
            Me.colApOver90.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colApOver90.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colApOver90.HeaderText = "90+ Days ($)"
            Me.colApOver90.MinimumWidth = 95
            Me.colApOver90.Name = "colApOver90"
            Me.colApOver90.ReadOnly = True
            Me.colApOver90.Width = 105

            '
            ' colApTotal
            '
            Me.colApTotal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colApTotal.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colApTotal.HeaderText = "Total Balance ($)"
            Me.colApTotal.MinimumWidth = 110
            Me.colApTotal.Name = "colApTotal"
            Me.colApTotal.ReadOnly = True
            Me.colApTotal.Width = 120

            '
            ' tabStockValuation
            '
            Me.tabStockValuation.BackColor = System.Drawing.Color.White
            Me.tabStockValuation.Controls.Add(Me.dgvStockValuation)
            Me.tabStockValuation.Controls.Add(Me.pnlSvSummary)
            Me.tabStockValuation.Location = New System.Drawing.Point(4, 30)
            Me.tabStockValuation.Name = "tabStockValuation"
            Me.tabStockValuation.Padding = New System.Windows.Forms.Padding(12)
            Me.tabStockValuation.Size = New System.Drawing.Size(1192, 636)
            Me.tabStockValuation.TabIndex = 6
            Me.tabStockValuation.Text = "Inventory Valuation"

            '
            ' pnlSvSummary
            '
            Me.pnlSvSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlSvSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlSvSummary.Controls.Add(Me.lblSvTotalUnits)
            Me.pnlSvSummary.Controls.Add(Me.lblSvSubledgerValuation)
            Me.pnlSvSummary.Controls.Add(Me.lblSvGlAccount1300)
            Me.pnlSvSummary.Controls.Add(Me.lblSvVariance)
            Me.pnlSvSummary.Controls.Add(Me.lblSvReconciledBadge)
            Me.pnlSvSummary.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlSvSummary.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            Me.pnlSvSummary.Location = New System.Drawing.Point(12, 12)
            Me.pnlSvSummary.Name = "pnlSvSummary"
            Me.pnlSvSummary.Padding = New System.Windows.Forms.Padding(10, 8, 10, 8)
            Me.pnlSvSummary.Size = New System.Drawing.Size(1168, 48)
            Me.pnlSvSummary.TabIndex = 0
            Me.pnlSvSummary.AutoSize = True
            Me.pnlSvSummary.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.pnlSvSummary.WrapContents = True

            '
            ' lblSvTotalUnits
            '
            Me.lblSvTotalUnits.AutoSize = True
            Me.lblSvTotalUnits.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSvTotalUnits.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblSvTotalUnits.Location = New System.Drawing.Point(14, 14)
            Me.lblSvTotalUnits.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblSvTotalUnits.Name = "lblSvTotalUnits"
            Me.lblSvTotalUnits.Size = New System.Drawing.Size(100, 15)
            Me.lblSvTotalUnits.TabIndex = 0
            Me.lblSvTotalUnits.Text = "Total Units: 0"

            '
            ' lblSvSubledgerValuation
            '
            Me.lblSvSubledgerValuation.AutoSize = True
            Me.lblSvSubledgerValuation.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSvSubledgerValuation.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblSvSubledgerValuation.Location = New System.Drawing.Point(134, 14)
            Me.lblSvSubledgerValuation.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblSvSubledgerValuation.Name = "lblSvSubledgerValuation"
            Me.lblSvSubledgerValuation.Size = New System.Drawing.Size(160, 17)
            Me.lblSvSubledgerValuation.TabIndex = 1
            Me.lblSvSubledgerValuation.Text = "Stock Valuation: $0.00"

            '
            ' lblSvGlAccount1300
            '
            Me.lblSvGlAccount1300.AutoSize = True
            Me.lblSvGlAccount1300.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSvGlAccount1300.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblSvGlAccount1300.Location = New System.Drawing.Point(314, 14)
            Me.lblSvGlAccount1300.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblSvGlAccount1300.Name = "lblSvGlAccount1300"
            Me.lblSvGlAccount1300.Size = New System.Drawing.Size(140, 17)
            Me.lblSvGlAccount1300.TabIndex = 2
            Me.lblSvGlAccount1300.Text = "GL 1300: $0.00"

            '
            ' lblSvVariance
            '
            Me.lblSvVariance.AutoSize = True
            Me.lblSvVariance.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSvVariance.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblSvVariance.Location = New System.Drawing.Point(474, 14)
            Me.lblSvVariance.Margin = New System.Windows.Forms.Padding(4, 7, 20, 4)
            Me.lblSvVariance.Name = "lblSvVariance"
            Me.lblSvVariance.Size = New System.Drawing.Size(113, 16)
            Me.lblSvVariance.TabIndex = 3
            Me.lblSvVariance.Text = "Variance: $0.0000"

            '
            ' lblSvReconciledBadge
            '
            Me.lblSvReconciledBadge.AutoSize = True
            Me.lblSvReconciledBadge.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblSvReconciledBadge.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSvReconciledBadge.ForeColor = System.Drawing.Color.White
            Me.lblSvReconciledBadge.Location = New System.Drawing.Point(611, 8)
            Me.lblSvReconciledBadge.Margin = New System.Windows.Forms.Padding(4, 2, 8, 4)
            Me.lblSvReconciledBadge.Name = "lblSvReconciledBadge"
            Me.lblSvReconciledBadge.Padding = New System.Windows.Forms.Padding(10, 5, 10, 5)
            Me.lblSvReconciledBadge.TabIndex = 4
            Me.lblSvReconciledBadge.Text = "GL 1300: RECONCILED"
            Me.lblSvReconciledBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter

            '
            ' dgvStockValuation
            '
            Me.dgvStockValuation.AllowUserToAddRows = False
            Me.dgvStockValuation.AllowUserToDeleteRows = False
            Me.dgvStockValuation.AllowUserToResizeRows = False
            Me.dgvStockValuation.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvStockValuation.BackgroundColor = System.Drawing.Color.White
            Me.dgvStockValuation.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvStockValuation.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle13.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle13.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle13.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle13.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle13.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle13.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle13.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvStockValuation.ColumnHeadersDefaultCellStyle = dgvCellStyle13
            Me.dgvStockValuation.ColumnHeadersHeight = 32
            Me.dgvStockValuation.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvStockValuation.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colSvSku, Me.colSvName, Me.colSvCategory, Me.colSvBaseUnit, Me.colSvQty, Me.colSvCost, Me.colSvValuation})
            dgvCellStyle14.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle14.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle14.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle14.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle14.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle14.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle14.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvStockValuation.DefaultCellStyle = dgvCellStyle14
            Me.dgvStockValuation.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvStockValuation.EnableHeadersVisualStyles = False
            Me.dgvStockValuation.Location = New System.Drawing.Point(12, 60)
            Me.dgvStockValuation.MultiSelect = False
            Me.dgvStockValuation.Name = "dgvStockValuation"
            Me.dgvStockValuation.ReadOnly = True
            Me.dgvStockValuation.RowHeadersVisible = False
            Me.dgvStockValuation.RowTemplate.Height = 26
            Me.dgvStockValuation.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvStockValuation.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvStockValuation.Size = New System.Drawing.Size(1168, 564)
            Me.dgvStockValuation.TabIndex = 1

            '
            ' colSvSku
            '
            Me.colSvSku.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colSvSku.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colSvSku.HeaderText = "SKU"
            Me.colSvSku.MinimumWidth = 85
            Me.colSvSku.Name = "colSvSku"
            Me.colSvSku.ReadOnly = True
            Me.colSvSku.Width = 100

            '
            ' colSvName
            '
            Me.colSvName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colSvName.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colSvName.FillWeight = 160.0!
            Me.colSvName.HeaderText = "Product Name"
            Me.colSvName.MinimumWidth = 180
            Me.colSvName.Name = "colSvName"
            Me.colSvName.ReadOnly = True

            '
            ' colSvCategory
            '
            Me.colSvCategory.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colSvCategory.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colSvCategory.HeaderText = "Category"
            Me.colSvCategory.MinimumWidth = 100
            Me.colSvCategory.Name = "colSvCategory"
            Me.colSvCategory.ReadOnly = True
            Me.colSvCategory.Width = 120

            '
            ' colSvBaseUnit
            '
            Me.colSvBaseUnit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colSvBaseUnit.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colSvBaseUnit.HeaderText = "Base Unit"
            Me.colSvBaseUnit.MinimumWidth = 70
            Me.colSvBaseUnit.Name = "colSvBaseUnit"
            Me.colSvBaseUnit.ReadOnly = True
            Me.colSvBaseUnit.Width = 80

            '
            ' colSvQty
            '
            Me.colSvQty.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colSvQty.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N0"}
            Me.colSvQty.HeaderText = "On Hand"
            Me.colSvQty.MinimumWidth = 80
            Me.colSvQty.Name = "colSvQty"
            Me.colSvQty.ReadOnly = True
            Me.colSvQty.Width = 90

            '
            ' colSvCost
            '
            Me.colSvCost.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colSvCost.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N4"}
            Me.colSvCost.HeaderText = "Avg Cost ($)"
            Me.colSvCost.MinimumWidth = 100
            Me.colSvCost.Name = "colSvCost"
            Me.colSvCost.ReadOnly = True
            Me.colSvCost.Width = 120

            '
            ' colSvValuation
            '
            Me.colSvValuation.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colSvValuation.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colSvValuation.HeaderText = "Total Value ($)"
            Me.colSvValuation.MinimumWidth = 110
            Me.colSvValuation.Name = "colSvValuation"
            Me.colSvValuation.ReadOnly = True
            Me.colSvValuation.Width = 130

            '
            ' tabGrniAging
            '
            Me.tabGrniAging.BackColor = System.Drawing.Color.White
            Me.tabGrniAging.Controls.Add(Me.dgvGrniAging)
            Me.tabGrniAging.Controls.Add(Me.pnlGrniSummary)
            Me.tabGrniAging.Location = New System.Drawing.Point(4, 30)
            Me.tabGrniAging.Name = "tabGrniAging"
            Me.tabGrniAging.Padding = New System.Windows.Forms.Padding(12)
            Me.tabGrniAging.Size = New System.Drawing.Size(1192, 636)
            Me.tabGrniAging.TabIndex = 7
            Me.tabGrniAging.Text = "GRNI Aging"

            '
            ' pnlGrniSummary
            '
            Me.pnlGrniSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlGrniSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlGrniSummary.Controls.Add(Me.lblGrniOpenCount)
            Me.pnlGrniSummary.Controls.Add(Me.lblGrniUnbilledUnits)
            Me.pnlGrniSummary.Controls.Add(Me.lblGrniAccrualTotal)
            Me.pnlGrniSummary.Controls.Add(Me.lblGrniFlaggedCount)
            Me.pnlGrniSummary.Controls.Add(Me.lblGrniFlaggedAmount)
            Me.pnlGrniSummary.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlGrniSummary.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            Me.pnlGrniSummary.Location = New System.Drawing.Point(12, 12)
            Me.pnlGrniSummary.Name = "pnlGrniSummary"
            Me.pnlGrniSummary.Padding = New System.Windows.Forms.Padding(10, 8, 10, 8)
            Me.pnlGrniSummary.Size = New System.Drawing.Size(1168, 48)
            Me.pnlGrniSummary.TabIndex = 0
            Me.pnlGrniSummary.AutoSize = True
            Me.pnlGrniSummary.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.pnlGrniSummary.WrapContents = True

            '
            ' lblGrniOpenCount
            '
            Me.lblGrniOpenCount.AutoSize = True
            Me.lblGrniOpenCount.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblGrniOpenCount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblGrniOpenCount.Location = New System.Drawing.Point(14, 14)
            Me.lblGrniOpenCount.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblGrniOpenCount.Name = "lblGrniOpenCount"
            Me.lblGrniOpenCount.Size = New System.Drawing.Size(100, 15)
            Me.lblGrniOpenCount.TabIndex = 0
            Me.lblGrniOpenCount.Text = "Open Lines: 0"

            '
            ' lblGrniUnbilledUnits
            '
            Me.lblGrniUnbilledUnits.AutoSize = True
            Me.lblGrniUnbilledUnits.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblGrniUnbilledUnits.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblGrniUnbilledUnits.Location = New System.Drawing.Point(134, 14)
            Me.lblGrniUnbilledUnits.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblGrniUnbilledUnits.Name = "lblGrniUnbilledUnits"
            Me.lblGrniUnbilledUnits.Size = New System.Drawing.Size(110, 15)
            Me.lblGrniUnbilledUnits.TabIndex = 1
            Me.lblGrniUnbilledUnits.Text = "Unbilled Units: 0"

            '
            ' lblGrniAccrualTotal
            '
            Me.lblGrniAccrualTotal.AutoSize = True
            Me.lblGrniAccrualTotal.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblGrniAccrualTotal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblGrniAccrualTotal.Location = New System.Drawing.Point(264, 14)
            Me.lblGrniAccrualTotal.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblGrniAccrualTotal.Name = "lblGrniAccrualTotal"
            Me.lblGrniAccrualTotal.Size = New System.Drawing.Size(170, 17)
            Me.lblGrniAccrualTotal.TabIndex = 2
            Me.lblGrniAccrualTotal.Text = "Accrual (2100): $0.00"

            '
            ' lblGrniFlaggedCount
            '
            Me.lblGrniFlaggedCount.AutoSize = True
            Me.lblGrniFlaggedCount.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblGrniFlaggedCount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblGrniFlaggedCount.Location = New System.Drawing.Point(454, 14)
            Me.lblGrniFlaggedCount.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblGrniFlaggedCount.Name = "lblGrniFlaggedCount"
            Me.lblGrniFlaggedCount.Size = New System.Drawing.Size(120, 15)
            Me.lblGrniFlaggedCount.TabIndex = 3
            Me.lblGrniFlaggedCount.Text = "Flagged >30D: 0"

            '
            ' lblGrniFlaggedAmount
            '
            Me.lblGrniFlaggedAmount.AutoSize = True
            Me.lblGrniFlaggedAmount.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblGrniFlaggedAmount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblGrniFlaggedAmount.Location = New System.Drawing.Point(594, 14)
            Me.lblGrniFlaggedAmount.Margin = New System.Windows.Forms.Padding(4, 6, 16, 4)
            Me.lblGrniFlaggedAmount.Name = "lblGrniFlaggedAmount"
            Me.lblGrniFlaggedAmount.Size = New System.Drawing.Size(130, 15)
            Me.lblGrniFlaggedAmount.TabIndex = 4
            Me.lblGrniFlaggedAmount.Text = "Flagged Bal: $0.00"

            '
            ' dgvGrniAging
            '
            Me.dgvGrniAging.AllowUserToAddRows = False
            Me.dgvGrniAging.AllowUserToDeleteRows = False
            Me.dgvGrniAging.AllowUserToResizeRows = False
            Me.dgvGrniAging.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvGrniAging.BackgroundColor = System.Drawing.Color.White
            Me.dgvGrniAging.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvGrniAging.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle15.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle15.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle15.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle15.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle15.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle15.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle15.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvGrniAging.ColumnHeadersDefaultCellStyle = dgvCellStyle15
            Me.dgvGrniAging.ColumnHeadersHeight = 32
            Me.dgvGrniAging.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvGrniAging.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colGrniDoc, Me.colGrniVendor, Me.colGrniItem, Me.colGrniReceived, Me.colGrniBilled, Me.colGrniUnbilled, Me.colGrniPoPrice, Me.colGrniAccrual, Me.colGrniDaysOpen, Me.colGrniStatus})
            dgvCellStyle16.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle16.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle16.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle16.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle16.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle16.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle16.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvGrniAging.DefaultCellStyle = dgvCellStyle16
            Me.dgvGrniAging.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvGrniAging.EnableHeadersVisualStyles = False
            Me.dgvGrniAging.Location = New System.Drawing.Point(12, 60)
            Me.dgvGrniAging.MultiSelect = False
            Me.dgvGrniAging.Name = "dgvGrniAging"
            Me.dgvGrniAging.ReadOnly = True
            Me.dgvGrniAging.RowHeadersVisible = False
            Me.dgvGrniAging.RowTemplate.Height = 26
            Me.dgvGrniAging.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvGrniAging.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvGrniAging.Size = New System.Drawing.Size(1168, 564)
            Me.dgvGrniAging.TabIndex = 1

            '
            ' colGrniDoc
            '
            Me.colGrniDoc.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colGrniDoc.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colGrniDoc.HeaderText = "GRN #"
            Me.colGrniDoc.MinimumWidth = 85
            Me.colGrniDoc.Name = "colGrniDoc"
            Me.colGrniDoc.ReadOnly = True
            Me.colGrniDoc.Width = 95

            '
            ' colGrniVendor
            '
            Me.colGrniVendor.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colGrniVendor.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colGrniVendor.HeaderText = "Vendor"
            Me.colGrniVendor.MinimumWidth = 110
            Me.colGrniVendor.Name = "colGrniVendor"
            Me.colGrniVendor.ReadOnly = True
            Me.colGrniVendor.Width = 130

            '
            ' colGrniItem
            '
            Me.colGrniItem.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colGrniItem.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colGrniItem.FillWeight = 140.0!
            Me.colGrniItem.HeaderText = "Item / SKU"
            Me.colGrniItem.MinimumWidth = 140
            Me.colGrniItem.Name = "colGrniItem"
            Me.colGrniItem.ReadOnly = True

            '
            ' colGrniReceived
            '
            Me.colGrniReceived.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colGrniReceived.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N0"}
            Me.colGrniReceived.HeaderText = "Rcvd"
            Me.colGrniReceived.MinimumWidth = 60
            Me.colGrniReceived.Name = "colGrniReceived"
            Me.colGrniReceived.ReadOnly = True
            Me.colGrniReceived.Width = 70

            '
            ' colGrniBilled
            '
            Me.colGrniBilled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colGrniBilled.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N0"}
            Me.colGrniBilled.HeaderText = "Billed"
            Me.colGrniBilled.MinimumWidth = 60
            Me.colGrniBilled.Name = "colGrniBilled"
            Me.colGrniBilled.ReadOnly = True
            Me.colGrniBilled.Width = 70

            '
            ' colGrniUnbilled
            '
            Me.colGrniUnbilled.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colGrniUnbilled.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N0"}
            Me.colGrniUnbilled.HeaderText = "Unbilled"
            Me.colGrniUnbilled.MinimumWidth = 65
            Me.colGrniUnbilled.Name = "colGrniUnbilled"
            Me.colGrniUnbilled.ReadOnly = True
            Me.colGrniUnbilled.Width = 75

            '
            ' colGrniPoPrice
            '
            Me.colGrniPoPrice.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colGrniPoPrice.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colGrniPoPrice.HeaderText = "PO Price ($)"
            Me.colGrniPoPrice.MinimumWidth = 85
            Me.colGrniPoPrice.Name = "colGrniPoPrice"
            Me.colGrniPoPrice.ReadOnly = True
            Me.colGrniPoPrice.Width = 95

            '
            ' colGrniAccrual
            '
            Me.colGrniAccrual.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colGrniAccrual.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colGrniAccrual.HeaderText = "Accrual ($)"
            Me.colGrniAccrual.MinimumWidth = 95
            Me.colGrniAccrual.Name = "colGrniAccrual"
            Me.colGrniAccrual.ReadOnly = True
            Me.colGrniAccrual.Width = 105

            '
            ' colGrniDaysOpen
            '
            Me.colGrniDaysOpen.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colGrniDaysOpen.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N0"}
            Me.colGrniDaysOpen.HeaderText = "Days Open"
            Me.colGrniDaysOpen.MinimumWidth = 65
            Me.colGrniDaysOpen.Name = "colGrniDaysOpen"
            Me.colGrniDaysOpen.ReadOnly = True
            Me.colGrniDaysOpen.Width = 75

            '
            ' colGrniStatus
            '
            Me.colGrniStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colGrniStatus.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colGrniStatus.HeaderText = "Aging Status"
            Me.colGrniStatus.MinimumWidth = 85
            Me.colGrniStatus.Name = "colGrniStatus"
            Me.colGrniStatus.ReadOnly = True
            Me.colGrniStatus.Width = 95

            '
            ' FinancialReportsForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(1200, 720)
            Me.Controls.Add(Me.tabReports)
            Me.Controls.Add(Me.pnlHeader)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "FinancialReportsForm"
            Me.Text = "Financial Reports"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlActions.ResumeLayout(False)
            Me.pnlActions.PerformLayout()
            Me.tabReports.ResumeLayout(False)
            Me.tabTrialBalance.ResumeLayout(False)
            Me.pnlTbStatus.ResumeLayout(False)
            Me.pnlTbStatus.PerformLayout()
            CType(Me.dgvTrialBalance, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabBalanceSheet.ResumeLayout(False)
            Me.pnlBsSummary.ResumeLayout(False)
            Me.pnlBsSummary.PerformLayout()
            CType(Me.dgvBalanceSheet, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabProfitLoss.ResumeLayout(False)
            Me.pnlPlSummary.ResumeLayout(False)
            Me.pnlPlSummary.PerformLayout()
            CType(Me.dgvProfitLoss, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabProductMargins.ResumeLayout(False)
            Me.pnlPmSummary.ResumeLayout(False)
            Me.pnlPmSummary.PerformLayout()
            CType(Me.dgvProductMargins, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabArAging.ResumeLayout(False)
            Me.pnlArSummary.ResumeLayout(False)
            Me.pnlArSummary.PerformLayout()
            CType(Me.dgvArAging, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabApAging.ResumeLayout(False)
            Me.pnlApSummary.ResumeLayout(False)
            Me.pnlApSummary.PerformLayout()
            CType(Me.dgvApAging, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabStockValuation.ResumeLayout(False)
            Me.pnlSvSummary.ResumeLayout(False)
            Me.pnlSvSummary.PerformLayout()
            CType(Me.dgvStockValuation, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabGrniAging.ResumeLayout(False)
            Me.pnlGrniSummary.ResumeLayout(False)
            Me.pnlGrniSummary.PerformLayout()
            CType(Me.dgvGrniAging, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents lblSubtitle As System.Windows.Forms.Label
        Friend WithEvents pnlActions As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblPeriodSelector As System.Windows.Forms.Label
        Friend WithEvents cmbPeriod As System.Windows.Forms.ComboBox
        Friend WithEvents btnExportCsv As System.Windows.Forms.Button

        Friend WithEvents tabReports As System.Windows.Forms.TabControl

        ' Tab 1: Trial Balance
        Friend WithEvents tabTrialBalance As System.Windows.Forms.TabPage
        Friend WithEvents pnlTbStatus As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblTbTotalDebits As System.Windows.Forms.Label
        Friend WithEvents lblTbTotalCredits As System.Windows.Forms.Label
        Friend WithEvents lblTbDiff As System.Windows.Forms.Label
        Friend WithEvents lblTbBalancedBadge As System.Windows.Forms.Label
        Friend WithEvents dgvTrialBalance As System.Windows.Forms.DataGridView
        Friend WithEvents colTbCode As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colTbName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colTbType As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colTbNormal As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colTbDebit As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colTbCredit As System.Windows.Forms.DataGridViewTextBoxColumn

        ' Tab 2: Balance Sheet
        Friend WithEvents tabBalanceSheet As System.Windows.Forms.TabPage
        Friend WithEvents pnlBsSummary As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblBsTotalAssets As System.Windows.Forms.Label
        Friend WithEvents lblBsTotalLiabilities As System.Windows.Forms.Label
        Friend WithEvents lblBsNetAssets As System.Windows.Forms.Label
        Friend WithEvents lblBsTotalEquity As System.Windows.Forms.Label
        Friend WithEvents lblBsVariance As System.Windows.Forms.Label
        Friend WithEvents lblBsBalancedBadge As System.Windows.Forms.Label
        Friend WithEvents dgvBalanceSheet As System.Windows.Forms.DataGridView
        Friend WithEvents colBsCategory As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colBsCode As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colBsName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colBsAmount As System.Windows.Forms.DataGridViewTextBoxColumn

        ' Tab 3: Profit & Loss
        Friend WithEvents tabProfitLoss As System.Windows.Forms.TabPage
        Friend WithEvents pnlPlSummary As System.Windows.Forms.Panel
        Friend WithEvents lblPlNetRevenue As System.Windows.Forms.Label
        Friend WithEvents lblPlGrossProfit As System.Windows.Forms.Label
        Friend WithEvents lblPlVariances As System.Windows.Forms.Label
        Friend WithEvents lblPlNetIncome As System.Windows.Forms.Label
        Friend WithEvents dgvProfitLoss As System.Windows.Forms.DataGridView
        Friend WithEvents colPlCategory As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPlDescription As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPlAmount As System.Windows.Forms.DataGridViewTextBoxColumn

        ' Tab 4: Product Margins
        Friend WithEvents tabProductMargins As System.Windows.Forms.TabPage
        Friend WithEvents pnlPmSummary As System.Windows.Forms.Panel
        Friend WithEvents lblPmTotalUnits As System.Windows.Forms.Label
        Friend WithEvents lblPmTotalProfit As System.Windows.Forms.Label
        Friend WithEvents lblPmOverallMargin As System.Windows.Forms.Label
        Friend WithEvents dgvProductMargins As System.Windows.Forms.DataGridView
        Friend WithEvents colPmSku As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPmName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPmUnits As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPmRevenue As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPmCogs As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPmFreight As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPmPpv As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPmNetMargin As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPmMarginPct As System.Windows.Forms.DataGridViewTextBoxColumn

        ' Tab 5: AR Aging
        Friend WithEvents tabArAging As System.Windows.Forms.TabPage
        Friend WithEvents pnlArSummary As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblArCurrent As System.Windows.Forms.Label
        Friend WithEvents lblAr1To30 As System.Windows.Forms.Label
        Friend WithEvents lblAr31To60 As System.Windows.Forms.Label
        Friend WithEvents lblAr61To90 As System.Windows.Forms.Label
        Friend WithEvents lblArOver90 As System.Windows.Forms.Label
        Friend WithEvents lblArTotal As System.Windows.Forms.Label
        Friend WithEvents dgvArAging As System.Windows.Forms.DataGridView
        Friend WithEvents colArCustomerCode As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colArCustomerName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colArCurrent As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAr1To30 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAr31To60 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAr61To90 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colArOver90 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colArTotal As System.Windows.Forms.DataGridViewTextBoxColumn

        ' Tab 6: AP Aging
        Friend WithEvents tabApAging As System.Windows.Forms.TabPage
        Friend WithEvents pnlApSummary As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblApCurrent As System.Windows.Forms.Label
        Friend WithEvents lblAp1To30 As System.Windows.Forms.Label
        Friend WithEvents lblAp31To60 As System.Windows.Forms.Label
        Friend WithEvents lblAp61To90 As System.Windows.Forms.Label
        Friend WithEvents lblApOver90 As System.Windows.Forms.Label
        Friend WithEvents lblApTotal As System.Windows.Forms.Label
        Friend WithEvents dgvApAging As System.Windows.Forms.DataGridView
        Friend WithEvents colApVendorCode As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colApVendorName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colApCurrent As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAp1To30 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAp31To60 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAp61To90 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colApOver90 As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colApTotal As System.Windows.Forms.DataGridViewTextBoxColumn

        ' Tab 7: Inventory Valuation
        Friend WithEvents tabStockValuation As System.Windows.Forms.TabPage
        Friend WithEvents pnlSvSummary As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblSvTotalUnits As System.Windows.Forms.Label
        Friend WithEvents lblSvSubledgerValuation As System.Windows.Forms.Label
        Friend WithEvents lblSvGlAccount1300 As System.Windows.Forms.Label
        Friend WithEvents lblSvVariance As System.Windows.Forms.Label
        Friend WithEvents lblSvReconciledBadge As System.Windows.Forms.Label
        Friend WithEvents dgvStockValuation As System.Windows.Forms.DataGridView
        Friend WithEvents colSvSku As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colSvName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colSvCategory As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colSvBaseUnit As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colSvQty As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colSvCost As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colSvValuation As System.Windows.Forms.DataGridViewTextBoxColumn

        ' Tab 8: GRNI Aging
        Friend WithEvents tabGrniAging As System.Windows.Forms.TabPage
        Friend WithEvents pnlGrniSummary As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblGrniOpenCount As System.Windows.Forms.Label
        Friend WithEvents lblGrniUnbilledUnits As System.Windows.Forms.Label
        Friend WithEvents lblGrniAccrualTotal As System.Windows.Forms.Label
        Friend WithEvents lblGrniFlaggedCount As System.Windows.Forms.Label
        Friend WithEvents lblGrniFlaggedAmount As System.Windows.Forms.Label
        Friend WithEvents dgvGrniAging As System.Windows.Forms.DataGridView
        Friend WithEvents colGrniDoc As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colGrniVendor As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colGrniItem As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colGrniReceived As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colGrniBilled As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colGrniUnbilled As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colGrniPoPrice As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colGrniAccrual As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colGrniDaysOpen As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colGrniStatus As System.Windows.Forms.DataGridViewTextBoxColumn

    End Class

End Namespace