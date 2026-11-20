Namespace UI.Accounting

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class LedgerDashboardForm
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

            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.lblTitle = New System.Windows.Forms.Label()
            Me.lblSubtitle = New System.Windows.Forms.Label()
            Me.pnlPeriodStatus = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblPeriodHeader = New System.Windows.Forms.Label()
            Me.lblPeriodName = New System.Windows.Forms.Label()
            Me.lblPeriodDates = New System.Windows.Forms.Label()
            Me.lblPeriodBadge = New System.Windows.Forms.Label()
            Me.btnPeriodAction = New System.Windows.Forms.Button()

            Me.splitMain = New System.Windows.Forms.SplitContainer()

            ' Left: Chart of Accounts
            Me.grpAccounts = New System.Windows.Forms.GroupBox()
            Me.pnlCoaFilter = New System.Windows.Forms.Panel()
            Me.lblFilterCoa = New System.Windows.Forms.Label()
            Me.cmbAccountTypeFilter = New System.Windows.Forms.ComboBox()
            Me.txtSearchCoa = New System.Windows.Forms.TextBox()
            Me.dgvAccounts = New System.Windows.Forms.DataGridView()
            Me.colAccountCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAccountName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colAccountType = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colNormalBal = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colBalance = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.pnlCoaSummary = New System.Windows.Forms.Panel()
            Me.lblCoaCount = New System.Windows.Forms.Label()

            ' Right: Journal Entries & Lines
            Me.splitJournal = New System.Windows.Forms.SplitContainer()
            Me.grpJournalEntries = New System.Windows.Forms.GroupBox()
            Me.pnlJournalFilter = New System.Windows.Forms.Panel()
            Me.lblDocFilter = New System.Windows.Forms.Label()
            Me.cmbDocTypeFilter = New System.Windows.Forms.ComboBox()
            Me.btnRefresh = New System.Windows.Forms.Button()
            Me.dgvJournalEntries = New System.Windows.Forms.DataGridView()
            Me.colEntryId = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colPostingDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colEventDate = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDocType = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDocNumber = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDescription = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colTotalDebits = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colTotalCredits = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colBalanceStatus = New System.Windows.Forms.DataGridViewTextBoxColumn()

            Me.grpJournalLines = New System.Windows.Forms.GroupBox()
            Me.dgvJournalLines = New System.Windows.Forms.DataGridView()
            Me.colLineId = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colLineAcctCode = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colLineAcctName = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colDebit = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colCredit = New System.Windows.Forms.DataGridViewTextBoxColumn()
            Me.colMemo = New System.Windows.Forms.DataGridViewTextBoxColumn()

            Me.pnlLinesSummary = New System.Windows.Forms.FlowLayoutPanel()
            Me.lblSelectedEntryInfo = New System.Windows.Forms.Label()
            Me.lblDebitsSum = New System.Windows.Forms.Label()
            Me.lblCreditsSum = New System.Windows.Forms.Label()
            Me.lblBalancedBadge = New System.Windows.Forms.Label()
            Me.lblAuditNotice = New System.Windows.Forms.Label()

            Me.pnlHeader.SuspendLayout()
            Me.pnlPeriodStatus.SuspendLayout()
            CType(Me.splitMain, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitMain.Panel1.SuspendLayout()
            Me.splitMain.Panel2.SuspendLayout()
            Me.splitMain.SuspendLayout()
            Me.grpAccounts.SuspendLayout()
            Me.pnlCoaFilter.SuspendLayout()
            CType(Me.dgvAccounts, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlCoaSummary.SuspendLayout()
            CType(Me.splitJournal, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitJournal.Panel1.SuspendLayout()
            Me.splitJournal.Panel2.SuspendLayout()
            Me.splitJournal.SuspendLayout()
            Me.grpJournalEntries.SuspendLayout()
            Me.pnlJournalFilter.SuspendLayout()
            CType(Me.dgvJournalEntries, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpJournalLines.SuspendLayout()
            CType(Me.dgvJournalLines, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlLinesSummary.SuspendLayout()
            Me.SuspendLayout()

            '
            ' pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(245, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlHeader.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlHeader.Controls.Add(Me.lblTitle)
            Me.pnlHeader.Controls.Add(Me.pnlPeriodStatus)
            Me.pnlHeader.Controls.Add(Me.lblSubtitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Padding = New System.Windows.Forms.Padding(16, 4, 16, 4)
            Me.pnlHeader.Size = New System.Drawing.Size(1200, 48)
            Me.pnlHeader.TabIndex = 0
            '
            ' lblTitle
            '
            Me.lblTitle.AutoEllipsis = True
            Me.lblTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 10.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(50, Byte), Integer), CType(CType(60, Byte), Integer), CType(CType(75, Byte), Integer))
            Me.lblTitle.Location = New System.Drawing.Point(16, 4)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(600, 38)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Chart of Accounts Master & Double-Entry General Ledger Registry"
            Me.lblTitle.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            Me.lblTitle.UseMnemonic = False
            Me.lblTitle.Visible = True
            '
            ' lblSubtitle
            '
            Me.lblSubtitle.Visible = False
            '
            ' pnlPeriodStatus
            '
            Me.pnlPeriodStatus.AutoSize = True
            Me.pnlPeriodStatus.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink
            Me.pnlPeriodStatus.BackColor = System.Drawing.Color.White
            Me.pnlPeriodStatus.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlPeriodStatus.Controls.Add(Me.btnPeriodAction)
            Me.pnlPeriodStatus.Controls.Add(Me.lblPeriodBadge)
            Me.pnlPeriodStatus.Controls.Add(Me.lblPeriodDates)
            Me.pnlPeriodStatus.Controls.Add(Me.lblPeriodName)
            Me.pnlPeriodStatus.Controls.Add(Me.lblPeriodHeader)
            Me.pnlPeriodStatus.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlPeriodStatus.FlowDirection = System.Windows.Forms.FlowDirection.RightToLeft
            Me.pnlPeriodStatus.Location = New System.Drawing.Point(620, 6)
            Me.pnlPeriodStatus.Name = "pnlPeriodStatus"
            Me.pnlPeriodStatus.Padding = New System.Windows.Forms.Padding(6, 4, 6, 4)
            Me.pnlPeriodStatus.Size = New System.Drawing.Size(564, 42)
            Me.pnlPeriodStatus.TabIndex = 2
            Me.pnlPeriodStatus.WrapContents = False
            '
            ' btnPeriodAction
            '
            Me.btnPeriodAction.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.btnPeriodAction.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.btnPeriodAction.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnPeriodAction.Margin = New System.Windows.Forms.Padding(4, 2, 4, 2)
            Me.btnPeriodAction.Name = "btnPeriodAction"
            Me.btnPeriodAction.Size = New System.Drawing.Size(130, 32)
            Me.btnPeriodAction.TabIndex = 4
            Me.btnPeriodAction.Text = "Period Info / Close"
            Me.btnPeriodAction.UseMnemonic = False
            Me.btnPeriodAction.UseVisualStyleBackColor = True
            '
            ' lblPeriodBadge
            '
            Me.lblPeriodBadge.AutoSize = True
            Me.lblPeriodBadge.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblPeriodBadge.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPeriodBadge.ForeColor = System.Drawing.Color.White
            Me.lblPeriodBadge.Margin = New System.Windows.Forms.Padding(6, 3, 4, 2)
            Me.lblPeriodBadge.Name = "lblPeriodBadge"
            Me.lblPeriodBadge.Padding = New System.Windows.Forms.Padding(6, 3, 6, 3)
            Me.lblPeriodBadge.Size = New System.Drawing.Size(100, 23)
            Me.lblPeriodBadge.TabIndex = 3
            Me.lblPeriodBadge.Text = "OPEN - ACTIVE"
            Me.lblPeriodBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            ' lblPeriodDates
            '
            Me.lblPeriodDates.AutoSize = True
            Me.lblPeriodDates.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPeriodDates.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
            Me.lblPeriodDates.Margin = New System.Windows.Forms.Padding(4, 7, 4, 2)
            Me.lblPeriodDates.Name = "lblPeriodDates"
            Me.lblPeriodDates.Size = New System.Drawing.Size(127, 13)
            Me.lblPeriodDates.TabIndex = 2
            Me.lblPeriodDates.Text = "2026-11-01 to 2026-11-30"
            '
            ' lblPeriodName
            '
            Me.lblPeriodName.AutoSize = True
            Me.lblPeriodName.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPeriodName.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblPeriodName.Margin = New System.Windows.Forms.Padding(4, 6, 2, 2)
            Me.lblPeriodName.Name = "lblPeriodName"
            Me.lblPeriodName.Size = New System.Drawing.Size(73, 16)
            Me.lblPeriodName.TabIndex = 1
            Me.lblPeriodName.Text = "FY2026-P11"
            '
            ' lblPeriodHeader
            '
            Me.lblPeriodHeader.AutoSize = True
            Me.lblPeriodHeader.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblPeriodHeader.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblPeriodHeader.Margin = New System.Windows.Forms.Padding(4, 7, 2, 2)
            Me.lblPeriodHeader.Name = "lblPeriodHeader"
            Me.lblPeriodHeader.Size = New System.Drawing.Size(84, 13)
            Me.lblPeriodHeader.TabIndex = 0
            Me.lblPeriodHeader.Text = "FISCAL PERIOD:"
            '
            ' splitMain
            '
            Me.splitMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitMain.Location = New System.Drawing.Point(0, 48)
            Me.splitMain.Name = "splitMain"
            Me.splitMain.Panel1MinSize = 290
            Me.splitMain.Panel2MinSize = 400
            '
            ' splitMain.Panel1
            '
            Me.splitMain.Panel1.Controls.Add(Me.grpAccounts)
            Me.splitMain.Panel1.Padding = New System.Windows.Forms.Padding(8)
            '
            ' splitMain.Panel2
            '
            Me.splitMain.Panel2.Controls.Add(Me.splitJournal)
            Me.splitMain.Panel2.Padding = New System.Windows.Forms.Padding(8)
            Me.splitMain.Size = New System.Drawing.Size(1200, 664)
            Me.splitMain.SplitterDistance = 290
            Me.splitMain.TabIndex = 1

            '
            ' grpAccounts
            '
            Me.grpAccounts.Controls.Add(Me.dgvAccounts)
            Me.grpAccounts.Controls.Add(Me.pnlCoaSummary)
            Me.grpAccounts.Controls.Add(Me.pnlCoaFilter)
            Me.grpAccounts.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpAccounts.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpAccounts.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.grpAccounts.Location = New System.Drawing.Point(8, 8)
            Me.grpAccounts.Name = "grpAccounts"
            Me.grpAccounts.Padding = New System.Windows.Forms.Padding(8)
            Me.grpAccounts.Size = New System.Drawing.Size(434, 619)
            Me.grpAccounts.TabIndex = 0
            Me.grpAccounts.TabStop = False
            Me.grpAccounts.Text = "Master Chart of Accounts (COA: 1010-5500)"
            '
            ' pnlCoaFilter
            '
            Me.pnlCoaFilter.Controls.Add(Me.txtSearchCoa)
            Me.pnlCoaFilter.Controls.Add(Me.cmbAccountTypeFilter)
            Me.pnlCoaFilter.Controls.Add(Me.lblFilterCoa)
            Me.pnlCoaFilter.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlCoaFilter.Location = New System.Drawing.Point(8, 25)
            Me.pnlCoaFilter.Name = "pnlCoaFilter"
            Me.pnlCoaFilter.Size = New System.Drawing.Size(418, 36)
            Me.pnlCoaFilter.TabIndex = 0
            '
            ' lblFilterCoa
            '
            Me.lblFilterCoa.AutoSize = True
            Me.lblFilterCoa.Location = New System.Drawing.Point(4, 10)
            Me.lblFilterCoa.Name = "lblFilterCoa"
            Me.lblFilterCoa.Size = New System.Drawing.Size(35, 15)
            Me.lblFilterCoa.TabIndex = 0
            Me.lblFilterCoa.Text = "Filter:"
            '
            ' cmbAccountTypeFilter
            '
            Me.cmbAccountTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbAccountTypeFilter.FormattingEnabled = True
            Me.cmbAccountTypeFilter.Items.AddRange(New Object() {"All Types", "Asset", "Liability", "Equity", "Revenue", "Expense"})
            Me.cmbAccountTypeFilter.Location = New System.Drawing.Point(68, 6)
            Me.cmbAccountTypeFilter.Name = "cmbAccountTypeFilter"
            Me.cmbAccountTypeFilter.Size = New System.Drawing.Size(150, 27)
            Me.cmbAccountTypeFilter.TabIndex = 1
            '
            ' txtSearchCoa
            '
            Me.txtSearchCoa.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.txtSearchCoa.Location = New System.Drawing.Point(228, 6)
            Me.txtSearchCoa.Name = "txtSearchCoa"
            Me.txtSearchCoa.PlaceholderText = "Search by code or name..."
            Me.txtSearchCoa.Size = New System.Drawing.Size(205, 27)
            Me.txtSearchCoa.TabIndex = 2
            '
            ' dgvAccounts
            '
            Me.dgvAccounts.AllowUserToAddRows = False
            Me.dgvAccounts.AllowUserToDeleteRows = False
            Me.dgvAccounts.AllowUserToResizeRows = False
            Me.dgvAccounts.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvAccounts.BackgroundColor = System.Drawing.Color.White
            Me.dgvAccounts.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvAccounts.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle1.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle1.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle1.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle1.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle1.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle1.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle1.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvAccounts.ColumnHeadersDefaultCellStyle = dgvCellStyle1
            Me.dgvAccounts.ColumnHeadersHeight = 32
            Me.dgvAccounts.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvAccounts.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colAccountCode, Me.colAccountName, Me.colAccountType, Me.colNormalBal, Me.colBalance})
            dgvCellStyle2.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle2.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle2.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle2.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle2.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle2.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle2.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvAccounts.DefaultCellStyle = dgvCellStyle2
            Me.dgvAccounts.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAccounts.EnableHeadersVisualStyles = False
            Me.dgvAccounts.Location = New System.Drawing.Point(8, 61)
            Me.dgvAccounts.MultiSelect = False
            Me.dgvAccounts.Name = "dgvAccounts"
            Me.dgvAccounts.ReadOnly = True
            Me.dgvAccounts.RowHeadersVisible = False
            Me.dgvAccounts.RowTemplate.Height = 24
            Me.dgvAccounts.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvAccounts.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvAccounts.Size = New System.Drawing.Size(418, 520)
            Me.dgvAccounts.TabIndex = 1
            '
            ' colAccountCode
            '
            Me.colAccountCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colAccountCode.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colAccountCode.HeaderText = "Code"
            Me.colAccountCode.MinimumWidth = 50
            Me.colAccountCode.Name = "colAccountCode"
            Me.colAccountCode.ReadOnly = True
            Me.colAccountCode.Width = 55
            '
            ' colAccountName
            '
            Me.colAccountName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colAccountName.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colAccountName.FillWeight = 140.0!
            Me.colAccountName.HeaderText = "Account Name"
            Me.colAccountName.MinimumWidth = 140
            Me.colAccountName.Name = "colAccountName"
            Me.colAccountName.ReadOnly = True
            '
            ' colAccountType
            '
            Me.colAccountType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colAccountType.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colAccountType.HeaderText = "Type"
            Me.colAccountType.MinimumWidth = 70
            Me.colAccountType.Name = "colAccountType"
            Me.colAccountType.ReadOnly = True
            Me.colAccountType.Width = 80
            '
            ' colNormalBal
            '
            Me.colNormalBal.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colNormalBal.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colNormalBal.HeaderText = "Normal"
            Me.colNormalBal.MinimumWidth = 60
            Me.colNormalBal.Name = "colNormalBal"
            Me.colNormalBal.ReadOnly = True
            Me.colNormalBal.Width = 65
            '
            ' colBalance
            '
            Me.colBalance.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colBalance.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colBalance.HeaderText = "Balance"
            Me.colBalance.MinimumWidth = 90
            Me.colBalance.Name = "colBalance"
            Me.colBalance.ReadOnly = True
            Me.colBalance.Width = 100
            '
            ' pnlCoaSummary
            '
            Me.pnlCoaSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlCoaSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlCoaSummary.Controls.Add(Me.lblCoaCount)
            Me.pnlCoaSummary.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlCoaSummary.Location = New System.Drawing.Point(8, 581)
            Me.pnlCoaSummary.Name = "pnlCoaSummary"
            Me.pnlCoaSummary.Padding = New System.Windows.Forms.Padding(6)
            Me.pnlCoaSummary.Size = New System.Drawing.Size(418, 30)
            Me.pnlCoaSummary.TabIndex = 2
            '
            ' lblCoaCount
            '
            Me.lblCoaCount.AutoSize = True
            Me.lblCoaCount.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCoaCount.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
            Me.lblCoaCount.Location = New System.Drawing.Point(6, 6)
            Me.lblCoaCount.Name = "lblCoaCount"
            Me.lblCoaCount.Size = New System.Drawing.Size(180, 15)
            Me.lblCoaCount.TabIndex = 0
            Me.lblCoaCount.Text = "Total Chart Accounts: 16 | Active: 16"

            '
            ' splitJournal
            '
            Me.splitJournal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitJournal.Location = New System.Drawing.Point(8, 8)
            Me.splitJournal.Name = "splitJournal"
            Me.splitJournal.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            ' splitJournal.Panel1
            '
            Me.splitJournal.Panel1.Controls.Add(Me.grpJournalEntries)
            '
            ' splitJournal.Panel2
            '
            Me.splitJournal.Panel2.Controls.Add(Me.grpJournalLines)
            Me.splitJournal.Size = New System.Drawing.Size(730, 619)
            Me.splitJournal.SplitterDistance = 330
            Me.splitJournal.TabIndex = 0

            '
            ' grpJournalEntries
            '
            Me.grpJournalEntries.Controls.Add(Me.dgvJournalEntries)
            Me.grpJournalEntries.Controls.Add(Me.pnlJournalFilter)
            Me.grpJournalEntries.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpJournalEntries.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpJournalEntries.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.grpJournalEntries.Location = New System.Drawing.Point(0, 0)
            Me.grpJournalEntries.Name = "grpJournalEntries"
            Me.grpJournalEntries.Padding = New System.Windows.Forms.Padding(8)
            Me.grpJournalEntries.Size = New System.Drawing.Size(730, 330)
            Me.grpJournalEntries.TabIndex = 0
            Me.grpJournalEntries.TabStop = False
            Me.grpJournalEntries.Text = "General Journal Entry Headers (Double-Entry Balanced)"
            '
            ' pnlJournalFilter
            '
            Me.pnlJournalFilter.Controls.Add(Me.btnRefresh)
            Me.pnlJournalFilter.Controls.Add(Me.cmbDocTypeFilter)
            Me.pnlJournalFilter.Controls.Add(Me.lblDocFilter)
            Me.pnlJournalFilter.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlJournalFilter.Location = New System.Drawing.Point(8, 25)
            Me.pnlJournalFilter.Name = "pnlJournalFilter"
            Me.pnlJournalFilter.Size = New System.Drawing.Size(714, 34)
            Me.pnlJournalFilter.TabIndex = 0
            '
            ' lblDocFilter
            '
            Me.lblDocFilter.AutoSize = True
            Me.lblDocFilter.Location = New System.Drawing.Point(4, 9)
            Me.lblDocFilter.Name = "lblDocFilter"
            Me.lblDocFilter.Size = New System.Drawing.Size(107, 15)
            Me.lblDocFilter.TabIndex = 0
            Me.lblDocFilter.Text = "Source Document:"
            '
            ' cmbDocTypeFilter
            '
            Me.cmbDocTypeFilter.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbDocTypeFilter.FormattingEnabled = True
            Me.cmbDocTypeFilter.Items.AddRange(New Object() {"All Documents", "Goods Receipt Note", "Supplier Bill", "Sales Invoice", "Credit Note", "Payment Receipt", "Payment Voucher"})
            Me.cmbDocTypeFilter.Location = New System.Drawing.Point(185, 5)
            Me.cmbDocTypeFilter.Name = "cmbDocTypeFilter"
            Me.cmbDocTypeFilter.Size = New System.Drawing.Size(200, 27)
            Me.cmbDocTypeFilter.TabIndex = 1
            '
            ' btnRefresh
            '
            Me.btnRefresh.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnRefresh.FlatStyle = System.Windows.Forms.FlatStyle.System
            Me.btnRefresh.Location = New System.Drawing.Point(630, 4)
            Me.btnRefresh.Name = "btnRefresh"
            Me.btnRefresh.Size = New System.Drawing.Size(80, 26)
            Me.btnRefresh.TabIndex = 2
            Me.btnRefresh.Text = "Refresh"
            Me.btnRefresh.UseVisualStyleBackColor = True
            '
            ' dgvJournalEntries
            '
            Me.dgvJournalEntries.AllowUserToAddRows = False
            Me.dgvJournalEntries.AllowUserToDeleteRows = False
            Me.dgvJournalEntries.AllowUserToResizeRows = False
            Me.dgvJournalEntries.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvJournalEntries.BackgroundColor = System.Drawing.Color.White
            Me.dgvJournalEntries.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvJournalEntries.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle3.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle3.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle3.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle3.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle3.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle3.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle3.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvJournalEntries.ColumnHeadersDefaultCellStyle = dgvCellStyle3
            Me.dgvJournalEntries.ColumnHeadersHeight = 34
            Me.dgvJournalEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvJournalEntries.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colEntryId, Me.colPostingDate, Me.colEventDate, Me.colDocType, Me.colDocNumber, Me.colDescription, Me.colTotalDebits, Me.colTotalCredits, Me.colBalanceStatus})
            dgvCellStyle4.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle4.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle4.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle4.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle4.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle4.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle4.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvJournalEntries.DefaultCellStyle = dgvCellStyle4
            Me.dgvJournalEntries.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvJournalEntries.EnableHeadersVisualStyles = False
            Me.dgvJournalEntries.Location = New System.Drawing.Point(8, 59)
            Me.dgvJournalEntries.MultiSelect = False
            Me.dgvJournalEntries.Name = "dgvJournalEntries"
            Me.dgvJournalEntries.ReadOnly = True
            Me.dgvJournalEntries.RowHeadersVisible = False
            Me.dgvJournalEntries.RowTemplate.Height = 24
            Me.dgvJournalEntries.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvJournalEntries.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvJournalEntries.Size = New System.Drawing.Size(714, 263)
            Me.dgvJournalEntries.TabIndex = 1
            '
            ' colEntryId
            '
            Me.colEntryId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colEntryId.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colEntryId.HeaderText = "ID"
            Me.colEntryId.MinimumWidth = 35
            Me.colEntryId.Name = "colEntryId"
            Me.colEntryId.ReadOnly = True
            Me.colEntryId.Width = 38
            '
            ' colPostingDate
            '
            Me.colPostingDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colPostingDate.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colPostingDate.HeaderText = "Posting Date"
            Me.colPostingDate.MinimumWidth = 70
            Me.colPostingDate.Name = "colPostingDate"
            Me.colPostingDate.ReadOnly = True
            Me.colPostingDate.Width = 75
            '
            ' colEventDate
            '
            Me.colEventDate.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colEventDate.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colEventDate.HeaderText = "Event Date"
            Me.colEventDate.MinimumWidth = 70
            Me.colEventDate.Name = "colEventDate"
            Me.colEventDate.ReadOnly = True
            Me.colEventDate.Width = 75
            '
            ' colDocType
            '
            Me.colDocType.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colDocType.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colDocType.HeaderText = "Doc Type"
            Me.colDocType.MinimumWidth = 85
            Me.colDocType.Name = "colDocType"
            Me.colDocType.ReadOnly = True
            Me.colDocType.Width = 95
            '
            ' colDocNumber
            '
            Me.colDocNumber.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colDocNumber.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colDocNumber.HeaderText = "Doc #"
            Me.colDocNumber.MinimumWidth = 80
            Me.colDocNumber.Name = "colDocNumber"
            Me.colDocNumber.ReadOnly = True
            Me.colDocNumber.Width = 90
            '
            ' colDescription
            '
            Me.colDescription.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colDescription.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colDescription.FillWeight = 200.0!
            Me.colDescription.HeaderText = "Description"
            Me.colDescription.MinimumWidth = 2
            Me.colDescription.Name = "colDescription"
            Me.colDescription.ReadOnly = True
            '
            ' colTotalDebits
            '
            Me.colTotalDebits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colTotalDebits.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colTotalDebits.HeaderText = "Debits"
            Me.colTotalDebits.MinimumWidth = 75
            Me.colTotalDebits.Name = "colTotalDebits"
            Me.colTotalDebits.ReadOnly = True
            Me.colTotalDebits.Width = 80
            '
            ' colTotalCredits
            '
            Me.colTotalCredits.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colTotalCredits.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colTotalCredits.HeaderText = "Credits"
            Me.colTotalCredits.MinimumWidth = 75
            Me.colTotalCredits.Name = "colTotalCredits"
            Me.colTotalCredits.ReadOnly = True
            Me.colTotalCredits.Width = 80
            '
            ' colBalanceStatus
            '
            Me.colBalanceStatus.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colBalanceStatus.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colBalanceStatus.HeaderText = "Status"
            Me.colBalanceStatus.MinimumWidth = 65
            Me.colBalanceStatus.Name = "colBalanceStatus"
            Me.colBalanceStatus.ReadOnly = True
            Me.colBalanceStatus.Width = 72

            '
            '
            ' grpJournalLines
            '
            Me.grpJournalLines.Controls.Add(Me.dgvJournalLines)
            Me.grpJournalLines.Controls.Add(Me.pnlLinesSummary)
            Me.grpJournalLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpJournalLines.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.grpJournalLines.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.grpJournalLines.Location = New System.Drawing.Point(0, 0)
            Me.grpJournalLines.Name = "grpJournalLines"
            Me.grpJournalLines.Padding = New System.Windows.Forms.Padding(8)
            Me.grpJournalLines.Size = New System.Drawing.Size(730, 285)
            Me.grpJournalLines.TabIndex = 0
            Me.grpJournalLines.TabStop = False
            Me.grpJournalLines.Text = "Double-Entry Distribution Lines"

            '
            ' dgvJournalLines
            '
            Me.dgvJournalLines.AllowUserToAddRows = False
            Me.dgvJournalLines.AllowUserToDeleteRows = False
            Me.dgvJournalLines.AllowUserToResizeRows = False
            Me.dgvJournalLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvJournalLines.BackgroundColor = System.Drawing.Color.White
            Me.dgvJournalLines.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
            Me.dgvJournalLines.ColumnHeadersBorderStyle = System.Windows.Forms.DataGridViewHeaderBorderStyle.Single
            dgvCellStyle5.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle5.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle5.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle5.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle5.SelectionBackColor = System.Drawing.SystemColors.Highlight
            dgvCellStyle5.SelectionForeColor = System.Drawing.SystemColors.HighlightText
            dgvCellStyle5.WrapMode = System.Windows.Forms.DataGridViewTriState.[True]
            Me.dgvJournalLines.ColumnHeadersDefaultCellStyle = dgvCellStyle5
            Me.dgvJournalLines.ColumnHeadersHeight = 28
            Me.dgvJournalLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.DisableResizing
            Me.dgvJournalLines.Columns.AddRange(New System.Windows.Forms.DataGridViewColumn() {Me.colLineId, Me.colLineAcctCode, Me.colLineAcctName, Me.colDebit, Me.colCredit, Me.colMemo})
            dgvCellStyle6.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft
            dgvCellStyle6.BackColor = System.Drawing.SystemColors.Window
            dgvCellStyle6.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            dgvCellStyle6.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            dgvCellStyle6.SelectionBackColor = System.Drawing.Color.FromArgb(CType(CType(225, Byte), Integer), CType(CType(235, Byte), Integer), CType(CType(245, Byte), Integer))
            dgvCellStyle6.SelectionForeColor = System.Drawing.Color.Black
            dgvCellStyle6.WrapMode = System.Windows.Forms.DataGridViewTriState.[False]
            Me.dgvJournalLines.DefaultCellStyle = dgvCellStyle6
            Me.dgvJournalLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvJournalLines.EnableHeadersVisualStyles = False
            Me.dgvJournalLines.Location = New System.Drawing.Point(8, 25)
            Me.dgvJournalLines.MultiSelect = False
            Me.dgvJournalLines.Name = "dgvJournalLines"
            Me.dgvJournalLines.ReadOnly = True
            Me.dgvJournalLines.RowHeadersVisible = False
            Me.dgvJournalLines.RowTemplate.Height = 24
            Me.dgvJournalLines.ScrollBars = System.Windows.Forms.ScrollBars.Both
            Me.dgvJournalLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvJournalLines.Size = New System.Drawing.Size(714, 217)
            Me.dgvJournalLines.TabIndex = 0
            '
            ' colLineId
            '
            Me.colLineId.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colLineId.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colLineId.HeaderText = "#"
            Me.colLineId.MinimumWidth = 30
            Me.colLineId.Name = "colLineId"
            Me.colLineId.ReadOnly = True
            Me.colLineId.Width = 35
            '
            ' colLineAcctCode
            '
            Me.colLineAcctCode.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colLineAcctCode.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleCenter}
            Me.colLineAcctCode.HeaderText = "Code"
            Me.colLineAcctCode.MinimumWidth = 45
            Me.colLineAcctCode.Name = "colLineAcctCode"
            Me.colLineAcctCode.ReadOnly = True
            Me.colLineAcctCode.Width = 50
            '
            ' colLineAcctName
            '
            Me.colLineAcctName.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colLineAcctName.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colLineAcctName.FillWeight = 40.0!
            Me.colLineAcctName.HeaderText = "Account Name"
            Me.colLineAcctName.MinimumWidth = 140
            Me.colLineAcctName.Name = "colLineAcctName"
            Me.colLineAcctName.ReadOnly = True
            '
            ' colDebit
            '
            Me.colDebit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colDebit.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colDebit.HeaderText = "Debit Amount"
            Me.colDebit.MinimumWidth = 75
            Me.colDebit.Name = "colDebit"
            Me.colDebit.ReadOnly = True
            Me.colDebit.Width = 80
            '
            ' colCredit
            '
            Me.colCredit.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.None
            Me.colCredit.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleRight, .Format = "N2"}
            Me.colCredit.HeaderText = "Credit Amount"
            Me.colCredit.MinimumWidth = 75
            Me.colCredit.Name = "colCredit"
            Me.colCredit.ReadOnly = True
            Me.colCredit.Width = 80
            '
            ' colMemo
            '
            Me.colMemo.AutoSizeMode = System.Windows.Forms.DataGridViewAutoSizeColumnMode.Fill
            Me.colMemo.DefaultCellStyle = New System.Windows.Forms.DataGridViewCellStyle() With {.Alignment = System.Windows.Forms.DataGridViewContentAlignment.MiddleLeft}
            Me.colMemo.FillWeight = 60.0!
            Me.colMemo.HeaderText = "Line Memo"
            Me.colMemo.MinimumWidth = 150
            Me.colMemo.Name = "colMemo"
            Me.colMemo.ReadOnly = True
            '
            ' pnlLinesSummary
            '
            Me.pnlLinesSummary.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlLinesSummary.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlLinesSummary.Controls.Add(Me.lblSelectedEntryInfo)
            Me.pnlLinesSummary.Controls.Add(Me.lblDebitsSum)
            Me.pnlLinesSummary.Controls.Add(Me.lblCreditsSum)
            Me.pnlLinesSummary.Controls.Add(Me.lblBalancedBadge)
            Me.pnlLinesSummary.Controls.Add(Me.lblAuditNotice)
            Me.pnlLinesSummary.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlLinesSummary.FlowDirection = System.Windows.Forms.FlowDirection.LeftToRight
            Me.pnlLinesSummary.Location = New System.Drawing.Point(8, 241)
            Me.pnlLinesSummary.Name = "pnlLinesSummary"
            Me.pnlLinesSummary.Padding = New System.Windows.Forms.Padding(6, 4, 6, 4)
            Me.pnlLinesSummary.Size = New System.Drawing.Size(714, 36)
            Me.pnlLinesSummary.TabIndex = 1
            Me.pnlLinesSummary.WrapContents = False
            '
            ' lblSelectedEntryInfo
            '
            Me.lblSelectedEntryInfo.AutoSize = True
            Me.lblSelectedEntryInfo.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSelectedEntryInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
            Me.lblSelectedEntryInfo.Location = New System.Drawing.Point(6, 6)
            Me.lblSelectedEntryInfo.Margin = New System.Windows.Forms.Padding(4, 7, 8, 2)
            Me.lblSelectedEntryInfo.Name = "lblSelectedEntryInfo"
            Me.lblSelectedEntryInfo.Size = New System.Drawing.Size(80, 14)
            Me.lblSelectedEntryInfo.TabIndex = 0
            Me.lblSelectedEntryInfo.Text = "Selected: #101"
            '
            ' lblDebitsSum
            '
            Me.lblDebitsSum.AutoSize = True
            Me.lblDebitsSum.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblDebitsSum.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblDebitsSum.Location = New System.Drawing.Point(126, 6)
            Me.lblDebitsSum.Margin = New System.Windows.Forms.Padding(4, 7, 8, 2)
            Me.lblDebitsSum.Name = "lblDebitsSum"
            Me.lblDebitsSum.Size = New System.Drawing.Size(105, 15)
            Me.lblDebitsSum.TabIndex = 1
            Me.lblDebitsSum.Text = "Debits: $1,000.00"
            '
            ' lblCreditsSum
            '
            Me.lblCreditsSum.AutoSize = True
            Me.lblCreditsSum.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCreditsSum.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblCreditsSum.Location = New System.Drawing.Point(251, 6)
            Me.lblCreditsSum.Margin = New System.Windows.Forms.Padding(4, 7, 8, 2)
            Me.lblCreditsSum.Name = "lblCreditsSum"
            Me.lblCreditsSum.Size = New System.Drawing.Size(109, 15)
            Me.lblCreditsSum.TabIndex = 2
            Me.lblCreditsSum.Text = "Credits: $1,000.00"
            '
            ' lblBalancedBadge
            '
            Me.lblBalancedBadge.AutoSize = True
            Me.lblBalancedBadge.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblBalancedBadge.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblBalancedBadge.ForeColor = System.Drawing.Color.White
            Me.lblBalancedBadge.Location = New System.Drawing.Point(380, 4)
            Me.lblBalancedBadge.Margin = New System.Windows.Forms.Padding(8, 2, 8, 2)
            Me.lblBalancedBadge.MinimumSize = New System.Drawing.Size(90, 22)
            Me.lblBalancedBadge.Name = "lblBalancedBadge"
            Me.lblBalancedBadge.Padding = New System.Windows.Forms.Padding(8, 3, 8, 3)
            Me.lblBalancedBadge.Size = New System.Drawing.Size(90, 22)
            Me.lblBalancedBadge.TabIndex = 3
            Me.lblBalancedBadge.Text = "BALANCED"
            Me.lblBalancedBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            ' lblAuditNotice
            '
            Me.lblAuditNotice.AutoSize = True
            Me.lblAuditNotice.Font = New System.Drawing.Font("Segoe UI", 7.5!, System.Drawing.FontStyle.Italic, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAuditNotice.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblAuditNotice.Location = New System.Drawing.Point(477, 4)
            Me.lblAuditNotice.Margin = New System.Windows.Forms.Padding(4, 6, 4, 4)
            Me.lblAuditNotice.Name = "lblAuditNotice"
            Me.lblAuditNotice.Size = New System.Drawing.Size(142, 12)
            Me.lblAuditNotice.TabIndex = 4
            Me.lblAuditNotice.Text = "Append-only double-entry ledger"
            Me.lblAuditNotice.Visible = False

            '
            ' LedgerDashboardForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.White
            Me.ClientSize = New System.Drawing.Size(1200, 720)
            Me.Controls.Add(Me.splitMain)
            Me.Controls.Add(Me.pnlHeader)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "LedgerDashboardForm"
            Me.Text = "General Ledger & COA"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlPeriodStatus.ResumeLayout(False)
            Me.pnlPeriodStatus.PerformLayout()
            Me.splitMain.Panel1.ResumeLayout(False)
            Me.splitMain.Panel2.ResumeLayout(False)
            CType(Me.splitMain, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitMain.ResumeLayout(False)
            Me.grpAccounts.ResumeLayout(False)
            Me.pnlCoaFilter.ResumeLayout(False)
            Me.pnlCoaFilter.PerformLayout()
            CType(Me.dgvAccounts, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlCoaSummary.ResumeLayout(False)
            Me.pnlCoaSummary.PerformLayout()
            Me.splitJournal.Panel1.ResumeLayout(False)
            Me.splitJournal.Panel2.ResumeLayout(False)
            CType(Me.splitJournal, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitJournal.ResumeLayout(False)
            Me.grpJournalEntries.ResumeLayout(False)
            Me.pnlJournalFilter.ResumeLayout(False)
            Me.pnlJournalFilter.PerformLayout()
            CType(Me.dgvJournalEntries, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpJournalLines.ResumeLayout(False)
            CType(Me.dgvJournalLines, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlLinesSummary.ResumeLayout(False)
            Me.pnlLinesSummary.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents lblSubtitle As System.Windows.Forms.Label
        Friend WithEvents pnlPeriodStatus As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblPeriodHeader As System.Windows.Forms.Label
        Friend WithEvents lblPeriodName As System.Windows.Forms.Label
        Friend WithEvents lblPeriodDates As System.Windows.Forms.Label
        Friend WithEvents lblPeriodBadge As System.Windows.Forms.Label
        Friend WithEvents btnPeriodAction As System.Windows.Forms.Button

        Friend WithEvents splitMain As System.Windows.Forms.SplitContainer
        Friend WithEvents grpAccounts As System.Windows.Forms.GroupBox
        Friend WithEvents pnlCoaFilter As System.Windows.Forms.Panel
        Friend WithEvents lblFilterCoa As System.Windows.Forms.Label
        Friend WithEvents cmbAccountTypeFilter As System.Windows.Forms.ComboBox
        Friend WithEvents txtSearchCoa As System.Windows.Forms.TextBox
        Friend WithEvents dgvAccounts As System.Windows.Forms.DataGridView
        Friend WithEvents colAccountCode As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAccountName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colAccountType As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colNormalBal As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colBalance As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents pnlCoaSummary As System.Windows.Forms.Panel
        Friend WithEvents lblCoaCount As System.Windows.Forms.Label

        Friend WithEvents splitJournal As System.Windows.Forms.SplitContainer
        Friend WithEvents grpJournalEntries As System.Windows.Forms.GroupBox
        Friend WithEvents pnlJournalFilter As System.Windows.Forms.Panel
        Friend WithEvents lblDocFilter As System.Windows.Forms.Label
        Friend WithEvents cmbDocTypeFilter As System.Windows.Forms.ComboBox
        Friend WithEvents btnRefresh As System.Windows.Forms.Button
        Friend WithEvents dgvJournalEntries As System.Windows.Forms.DataGridView
        Friend WithEvents colEntryId As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colPostingDate As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colEventDate As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colDocType As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colDocNumber As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colDescription As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colTotalDebits As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colTotalCredits As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colBalanceStatus As System.Windows.Forms.DataGridViewTextBoxColumn

        Friend WithEvents grpJournalLines As System.Windows.Forms.GroupBox
        Friend WithEvents dgvJournalLines As System.Windows.Forms.DataGridView
        Friend WithEvents colLineId As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colLineAcctCode As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colLineAcctName As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colDebit As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colCredit As System.Windows.Forms.DataGridViewTextBoxColumn
        Friend WithEvents colMemo As System.Windows.Forms.DataGridViewTextBoxColumn

        Friend WithEvents pnlLinesSummary As System.Windows.Forms.FlowLayoutPanel
        Friend WithEvents lblSelectedEntryInfo As System.Windows.Forms.Label
        Friend WithEvents lblDebitsSum As System.Windows.Forms.Label
        Friend WithEvents lblCreditsSum As System.Windows.Forms.Label
        Friend WithEvents lblBalancedBadge As System.Windows.Forms.Label
        Friend WithEvents lblAuditNotice As System.Windows.Forms.Label

    End Class

End Namespace
