Namespace UI.Reporting

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class FinancialReportsForm
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
            Dim dgvCellStyle1 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle2 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle3 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle4 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle5 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()
            Dim dgvCellStyle6 As System.Windows.Forms.DataGridViewCellStyle = New System.Windows.Forms.DataGridViewCellStyle()

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

            ' Tab 2: Profit & Loss
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

            ' Tab 3: Product Margins
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

            Me.pnlHeader.SuspendLayout()
            Me.pnlActions.SuspendLayout()
            Me.tabReports.SuspendLayout()
            Me.tabTrialBalance.SuspendLayout()
            Me.pnlTbStatus.SuspendLayout()
            CType(Me.dgvTrialBalance, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabProfitLoss.SuspendLayout()
            Me.pnlPlSummary.SuspendLayout()
            CType(Me.dgvProfitLoss, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabProductMargins.SuspendLayout()
            Me.pnlPmSummary.SuspendLayout()
            CType(Me.dgvProductMargins, System.ComponentModel.ISupportInitialize).BeginInit()
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
            Me.lblSubtitle.Text = "Trial balance verification, P&L income statement, and managerial product margin profitability"
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
            Me.tabReports.Controls.Add(Me.tabProfitLoss)
            Me.tabReports.Controls.Add(Me.tabProductMargins)
            Me.tabReports.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tabReports.Location = New System.Drawing.Point(0, 85)
            Me.tabReports.Name = "tabReports"
            Me.tabReports.Padding = New System.Drawing.Point(16, 8)
            Me.tabReports.SelectedIndex = 0
            Me.tabReports.Size = New System.Drawing.Size(1200, 635)
            Me.tabReports.TabIndex = 1

            '
            ' tabTrialBalance
            '
            Me.tabTrialBalance.BackColor = System.Drawing.Color.White
            Me.tabTrialBalance.Controls.Add(Me.dgvTrialBalance)
            Me.tabTrialBalance.Controls.Add(Me.pnlTbStatus)
            Me.tabTrialBalance.Location = New System.Drawing.Point(4, 36)
            Me.tabTrialBalance.Name = "tabTrialBalance"
            Me.tabTrialBalance.Padding = New System.Windows.Forms.Padding(12)
            Me.tabTrialBalance.Size = New System.Drawing.Size(1192, 595)
            Me.tabTrialBalance.TabIndex = 0
            Me.tabTrialBalance.Text = "Trial Balance (Debits = Credits)"

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
            Me.lblTbTotalDebits.Text = "Total Debits: $47,100.00"

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
            Me.lblTbTotalCredits.Text = "Total Credits: $47,100.00"

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
            Me.dgvTrialBalance.Size = New System.Drawing.Size(1168, 523)
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
            ' tabProfitLoss
            '
            Me.tabProfitLoss.BackColor = System.Drawing.Color.White
            Me.tabProfitLoss.Controls.Add(Me.dgvProfitLoss)
            Me.tabProfitLoss.Controls.Add(Me.pnlPlSummary)
            Me.tabProfitLoss.Location = New System.Drawing.Point(4, 36)
            Me.tabProfitLoss.Name = "tabProfitLoss"
            Me.tabProfitLoss.Padding = New System.Windows.Forms.Padding(12)
            Me.tabProfitLoss.Size = New System.Drawing.Size(1192, 595)
            Me.tabProfitLoss.TabIndex = 1
            Me.tabProfitLoss.Text = "Profit & Loss / Income Statement"

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
            Me.lblPlNetRevenue.Size = New System.Drawing.Size(147, 16)
            Me.lblPlNetRevenue.TabIndex = 0
            Me.lblPlNetRevenue.Text = "Net Revenue: $450.00"

            '
            ' lblPlGrossProfit
            '
            Me.lblPlGrossProfit.AutoSize = True
            Me.lblPlGrossProfit.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPlGrossProfit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblPlGrossProfit.Location = New System.Drawing.Point(220, 14)
            Me.lblPlGrossProfit.Name = "lblPlGrossProfit"
            Me.lblPlGrossProfit.Size = New System.Drawing.Size(141, 16)
            Me.lblPlGrossProfit.TabIndex = 1
            Me.lblPlGrossProfit.Text = "Gross Profit: $150.00"

            '
            ' lblPlVariances
            '
            Me.lblPlVariances.AutoSize = True
            Me.lblPlVariances.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPlVariances.ForeColor = System.Drawing.Color.FromArgb(CType(CType(180, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(0, Byte), Integer))
            Me.lblPlVariances.Location = New System.Drawing.Point(420, 14)
            Me.lblPlVariances.Name = "lblPlVariances"
            Me.lblPlVariances.Size = New System.Drawing.Size(193, 16)
            Me.lblPlVariances.TabIndex = 2
            Me.lblPlVariances.Text = "Operating Variances: $170.00"

            '
            ' lblPlNetIncome
            '
            Me.lblPlNetIncome.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblPlNetIncome.AutoSize = True
            Me.lblPlNetIncome.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPlNetIncome.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblPlNetIncome.Location = New System.Drawing.Point(880, 14)
            Me.lblPlNetIncome.Name = "lblPlNetIncome"
            Me.lblPlNetIncome.Size = New System.Drawing.Size(240, 17)
            Me.lblPlNetIncome.TabIndex = 3
            Me.lblPlNetIncome.Text = "Net Operating Income: ($20.00)"

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
            dgvCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle3.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dgvProfitLoss.ColumnHeadersDefaultCellStyle = dgvCellStyle3
            Me.dgvProfitLoss.ColumnHeadersHeight = 30
            Me.dgvProfitLoss.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvProfitLoss.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPlCategory, Me.colPlDescription, Me.colPlAmount})
            dgvCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle4.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle4.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvProfitLoss.DefaultCellStyle = dgvCellStyle4
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
            Me.dgvProfitLoss.Size = New System.Drawing.Size(1168, 523)
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
            Me.colPlAmount.HeaderText = "Amount (USD)"
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
            Me.tabProductMargins.Location = New System.Drawing.Point(4, 36)
            Me.tabProductMargins.Name = "tabProductMargins"
            Me.tabProductMargins.Padding = New System.Windows.Forms.Padding(12)
            Me.tabProductMargins.Size = New System.Drawing.Size(1192, 595)
            Me.tabProductMargins.TabIndex = 2
            Me.tabProductMargins.Text = "Product Margins & Landed Profitability"

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
            Me.lblPmTotalUnits.Size = New System.Drawing.Size(155, 16)
            Me.lblPmTotalUnits.TabIndex = 0
            Me.lblPmTotalUnits.Text = "Total Units Sold: 125"

            '
            ' lblPmTotalProfit
            '
            Me.lblPmTotalProfit.AutoSize = True
            Me.lblPmTotalProfit.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPmTotalProfit.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblPmTotalProfit.Location = New System.Drawing.Point(235, 14)
            Me.lblPmTotalProfit.Name = "lblPmTotalProfit"
            Me.lblPmTotalProfit.Size = New System.Drawing.Size(211, 16)
            Me.lblPmTotalProfit.TabIndex = 1
            Me.lblPmTotalProfit.Text = "Total Net Landed Margin: $665.00"

            '
            ' lblPmOverallMargin
            '
            Me.lblPmOverallMargin.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblPmOverallMargin.AutoSize = True
            Me.lblPmOverallMargin.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPmOverallMargin.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblPmOverallMargin.Location = New System.Drawing.Point(880, 14)
            Me.lblPmOverallMargin.Name = "lblPmOverallMargin"
            Me.lblPmOverallMargin.Size = New System.Drawing.Size(225, 17)
            Me.lblPmOverallMargin.TabIndex = 2
            Me.lblPmOverallMargin.Text = "Weighted Landed Margin: 38.89%"

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
            dgvCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dgvProductMargins.ColumnHeadersDefaultCellStyle = dgvCellStyle5
            Me.dgvProductMargins.ColumnHeadersHeight = 30
            Me.dgvProductMargins.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvProductMargins.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colPmSku, Me.colPmName, Me.colPmUnits, Me.colPmRevenue, Me.colPmCogs, Me.colPmFreight, Me.colPmPpv, Me.colPmNetMargin, Me.colPmMarginPct})
            dgvCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle6.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle6.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle6.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvProductMargins.DefaultCellStyle = dgvCellStyle6
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
            Me.dgvProductMargins.Size = New System.Drawing.Size(1168, 523)
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
            Me.tabProfitLoss.ResumeLayout(False)
            Me.pnlPlSummary.ResumeLayout(False)
            Me.pnlPlSummary.PerformLayout()
            CType(Me.dgvProfitLoss, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabProductMargins.ResumeLayout(False)
            Me.pnlPmSummary.ResumeLayout(False)
            Me.pnlPmSummary.PerformLayout()
            CType(Me.dgvProductMargins, System.ComponentModel.ISupportInitialize).EndInit()
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

    End Class

End Namespace
