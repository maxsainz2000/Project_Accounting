Option Strict On
Option Explicit On
Option Compare Binary

Namespace UI.Warehouse

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class WarehouseIntakeForm
        Inherits System.Windows.Forms.Form

        <System.Diagnostics.DebuggerNonUserCode()>
        Protected Overrides Sub Dispose(ByVal disposing As Boolean)
            Try
                If disposing Then
                    If _repo IsNot Nothing Then
                        _repo.Dispose()
                    End If
                    If components IsNot Nothing Then
                        components.Dispose()
                    End If
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
            Me.tabMain = New System.Windows.Forms.TabControl()
            Me.tabDockReceiving = New System.Windows.Forms.TabPage()
            Me.pnlDockMain = New System.Windows.Forms.Panel()
            Me.splitDock = New System.Windows.Forms.SplitContainer()
            Me.grpOpenPOs = New System.Windows.Forms.GroupBox()
            Me.dgvOpenPOs = New System.Windows.Forms.DataGridView()
            Me.pnlPOLines = New System.Windows.Forms.Panel()
            Me.grpPOLines = New System.Windows.Forms.GroupBox()
            Me.dgvPOLines = New System.Windows.Forms.DataGridView()
            Me.pnlReceiveActions = New System.Windows.Forms.Panel()
            Me.lblCostImpactPreview = New System.Windows.Forms.Label()
            Me.btnReceiveGRN = New System.Windows.Forms.Button()
            Me.numReceiveQty = New System.Windows.Forms.NumericUpDown()
            Me.lblReceiveQty = New System.Windows.Forms.Label()
            Me.grpGRNJournal = New System.Windows.Forms.GroupBox()
            Me.dgvGRNJournal = New System.Windows.Forms.DataGridView()
            Me.tabQuarantine = New System.Windows.Forms.TabPage()
            Me.splitRMA = New System.Windows.Forms.SplitContainer()
            Me.grpRMAIntake = New System.Windows.Forms.GroupBox()
            Me.pnlIntakeForm = New System.Windows.Forms.Panel()
            Me.lblIntakeNotice = New System.Windows.Forms.Label()
            Me.btnIntakeToQuarantine = New System.Windows.Forms.Button()
            Me.numIntakeSellPrice = New System.Windows.Forms.NumericUpDown()
            Me.lblIntakeSellPrice = New System.Windows.Forms.Label()
            Me.numIntakeOrigCost = New System.Windows.Forms.NumericUpDown()
            Me.lblIntakeOrigCost = New System.Windows.Forms.Label()
            Me.numIntakeQty = New System.Windows.Forms.NumericUpDown()
            Me.lblIntakeQty = New System.Windows.Forms.Label()
            Me.cmbIntakeProduct = New System.Windows.Forms.ComboBox()
            Me.lblIntakeProduct = New System.Windows.Forms.Label()
            Me.txtIntakeInvoice = New System.Windows.Forms.TextBox()
            Me.lblIntakeInvoice = New System.Windows.Forms.Label()
            Me.txtIntakeCustomer = New System.Windows.Forms.TextBox()
            Me.lblIntakeCustomer = New System.Windows.Forms.Label()
            Me.grpInspection = New System.Windows.Forms.GroupBox()
            Me.dgvQuarantineItems = New System.Windows.Forms.DataGridView()
            Me.pnlInspectionActions = New System.Windows.Forms.Panel()
            Me.lblInspectionSummary = New System.Windows.Forms.Label()
            Me.btnConfirmInspection = New System.Windows.Forms.Button()
            Me.numScrapQty = New System.Windows.Forms.NumericUpDown()
            Me.lblScrapQty = New System.Windows.Forms.Label()
            Me.numRestockQty = New System.Windows.Forms.NumericUpDown()
            Me.lblRestockQty = New System.Windows.Forms.Label()
            Me.lblSelectedRMA = New System.Windows.Forms.Label()
            Me.grpRMAJournal = New System.Windows.Forms.GroupBox()
            Me.dgvRMAJournal = New System.Windows.Forms.DataGridView()
            Me.tabStockAdjustments = New System.Windows.Forms.TabPage()
            Me.pnlAdjMain = New System.Windows.Forms.Panel()
            Me.splitStockAdjustments = New System.Windows.Forms.SplitContainer()
            Me.grpAdjustmentEntry = New System.Windows.Forms.GroupBox()
            Me.lblAdjProduct = New System.Windows.Forms.Label()
            Me.cmbAdjProduct = New System.Windows.Forms.ComboBox()
            Me.lblAdjOnHand = New System.Windows.Forms.Label()
            Me.lblAdjUnitCost = New System.Windows.Forms.Label()
            Me.lblAdjReason = New System.Windows.Forms.Label()
            Me.cmbAdjReason = New System.Windows.Forms.ComboBox()
            Me.lblAdjVariance = New System.Windows.Forms.Label()
            Me.numAdjVariance = New System.Windows.Forms.NumericUpDown()
            Me.lblAdjNotes = New System.Windows.Forms.Label()
            Me.txtAdjNotes = New System.Windows.Forms.TextBox()
            Me.btnAddAdjLine = New System.Windows.Forms.Button()
            Me.btnClearAdjLines = New System.Windows.Forms.Button()
            Me.grpAdjLines = New System.Windows.Forms.GroupBox()
            Me.dgvAdjLines = New System.Windows.Forms.DataGridView()
            Me.pnlAdjLineButtons = New System.Windows.Forms.Panel()
            Me.btnRemoveAdjLine = New System.Windows.Forms.Button()
            Me.grpAdjJournal = New System.Windows.Forms.GroupBox()
            Me.ctrlAdjJournal = New Merchandising_Accounting_App.UI.Controls.DoubleEntryDistributionGridControl()
            Me.pnlAdjSubmit = New System.Windows.Forms.Panel()
            Me.lblAdjStatus = New System.Windows.Forms.Label()
            Me.btnSubmitAdjustment = New System.Windows.Forms.Button()
            Me.pnlHeader.SuspendLayout()
            Me.tabMain.SuspendLayout()
            Me.tabDockReceiving.SuspendLayout()
            Me.pnlDockMain.SuspendLayout()
            CType(Me.splitDock, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitDock.Panel1.SuspendLayout()
            Me.splitDock.Panel2.SuspendLayout()
            Me.splitDock.SuspendLayout()
            Me.grpOpenPOs.SuspendLayout()
            CType(Me.dgvOpenPOs, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlPOLines.SuspendLayout()
            Me.grpPOLines.SuspendLayout()
            CType(Me.dgvPOLines, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlReceiveActions.SuspendLayout()
            CType(Me.numReceiveQty, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpGRNJournal.SuspendLayout()
            CType(Me.dgvGRNJournal, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabQuarantine.SuspendLayout()
            CType(Me.splitRMA, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitRMA.Panel1.SuspendLayout()
            Me.splitRMA.Panel2.SuspendLayout()
            Me.splitRMA.SuspendLayout()
            Me.grpRMAIntake.SuspendLayout()
            Me.pnlIntakeForm.SuspendLayout()
            CType(Me.numIntakeSellPrice, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numIntakeOrigCost, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numIntakeQty, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpInspection.SuspendLayout()
            CType(Me.dgvQuarantineItems, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlInspectionActions.SuspendLayout()
            CType(Me.numScrapQty, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numRestockQty, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpRMAJournal.SuspendLayout()
            CType(Me.dgvRMAJournal, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabStockAdjustments.SuspendLayout()
            Me.pnlAdjMain.SuspendLayout()
            CType(Me.splitStockAdjustments, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitStockAdjustments.Panel1.SuspendLayout()
            Me.splitStockAdjustments.Panel2.SuspendLayout()
            Me.splitStockAdjustments.SuspendLayout()
            Me.grpAdjustmentEntry.SuspendLayout()
            CType(Me.numAdjVariance, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpAdjLines.SuspendLayout()
            CType(Me.dgvAdjLines, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlAdjLineButtons.SuspendLayout()
            Me.grpAdjJournal.SuspendLayout()
            Me.pnlAdjSubmit.SuspendLayout()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(246, Byte), Integer))
            Me.pnlHeader.Controls.Add(Me.lblSubtitle)
            Me.pnlHeader.Controls.Add(Me.lblTitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Padding = New System.Windows.Forms.Padding(16, 10, 16, 10)
            Me.pnlHeader.Size = New System.Drawing.Size(1024, 0)
            Me.pnlHeader.TabIndex = 0
            Me.pnlHeader.Visible = False
            '
            'lblSubtitle
            '
            Me.lblSubtitle.AutoSize = True
            Me.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblSubtitle.Location = New System.Drawing.Point(16, 33)
            Me.lblSubtitle.Name = "lblSubtitle"
            Me.lblSubtitle.Size = New System.Drawing.Size(650, 20)
            Me.lblSubtitle.TabIndex = 1
            Me.lblSubtitle.Text = "Dock Intake (GRN & Moving Average Cost) | RMA Quarantine Intake & Inspection (Restock vs. Scrap)"
            Me.lblSubtitle.UseMnemonic = False
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Font = New System.Drawing.Font("Segoe UI", 11.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblTitle.Location = New System.Drawing.Point(16, 8)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(435, 27)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Warehouse Intake & RMA Quarantine Inspection"
            Me.lblTitle.UseMnemonic = False
            '
            'tabMain
            '
            Me.tabMain.Controls.Add(Me.tabDockReceiving)
            Me.tabMain.Controls.Add(Me.tabQuarantine)
            Me.tabMain.Controls.Add(Me.tabStockAdjustments)
            Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tabMain.Location = New System.Drawing.Point(0, 0)
            Me.tabMain.Name = "tabMain"
            Me.tabMain.SelectedIndex = 0
            Me.tabMain.Size = New System.Drawing.Size(1024, 670)
            Me.tabMain.TabIndex = 1
            '
            'tabDockReceiving
            '
            Me.tabDockReceiving.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.tabDockReceiving.Controls.Add(Me.pnlDockMain)
            Me.tabDockReceiving.Location = New System.Drawing.Point(4, 30)
            Me.tabDockReceiving.Name = "tabDockReceiving"
            Me.tabDockReceiving.Padding = New System.Windows.Forms.Padding(12)
            Me.tabDockReceiving.Size = New System.Drawing.Size(1016, 576)
            Me.tabDockReceiving.TabIndex = 0
            Me.tabDockReceiving.Text = "Dock Intake (PO -> GRN)"
            '
            'pnlDockMain
            '
            Me.pnlDockMain.Controls.Add(Me.splitDock)
            Me.pnlDockMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlDockMain.Location = New System.Drawing.Point(12, 12)
            Me.pnlDockMain.Name = "pnlDockMain"
            Me.pnlDockMain.Size = New System.Drawing.Size(992, 552)
            Me.pnlDockMain.TabIndex = 0
            '
            'splitDock
            '
            Me.splitDock.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitDock.Location = New System.Drawing.Point(0, 0)
            Me.splitDock.Name = "splitDock"
            Me.splitDock.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitDock.Panel1
            '
            Me.splitDock.Panel1.Controls.Add(Me.grpOpenPOs)
            Me.splitDock.Panel1.Controls.Add(Me.pnlPOLines)
            '
            'splitDock.Panel2
            '
            Me.splitDock.Panel2.Controls.Add(Me.grpGRNJournal)
            Me.splitDock.Size = New System.Drawing.Size(992, 552)
            Me.splitDock.SplitterDistance = 330
            Me.splitDock.TabIndex = 0
            '
            'grpOpenPOs
            '
            Me.grpOpenPOs.Controls.Add(Me.dgvOpenPOs)
            Me.grpOpenPOs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpOpenPOs.Location = New System.Drawing.Point(0, 0)
            Me.grpOpenPOs.Name = "grpOpenPOs"
            Me.grpOpenPOs.Padding = New System.Windows.Forms.Padding(8, 28, 8, 8)
            Me.grpOpenPOs.Size = New System.Drawing.Size(496, 330)
            Me.grpOpenPOs.TabIndex = 0
            Me.grpOpenPOs.TabStop = False
            Me.grpOpenPOs.Text = "1. Select Open Purchase Order"
            '
            'dgvOpenPOs
            '
            Me.dgvOpenPOs.AllowUserToAddRows = False
            Me.dgvOpenPOs.AllowUserToDeleteRows = False
            Me.dgvOpenPOs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvOpenPOs.BackgroundColor = System.Drawing.Color.White
            Me.dgvOpenPOs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvOpenPOs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvOpenPOs.Location = New System.Drawing.Point(8, 28)
            Me.dgvOpenPOs.MultiSelect = False
            Me.dgvOpenPOs.Name = "dgvOpenPOs"
            Me.dgvOpenPOs.ReadOnly = True
            Me.dgvOpenPOs.RowHeadersVisible = False
            Me.dgvOpenPOs.RowHeadersWidth = 51
            Me.dgvOpenPOs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvOpenPOs.Size = New System.Drawing.Size(480, 294)
            Me.dgvOpenPOs.TabIndex = 0
            '
            'pnlPOLines
            '
            Me.pnlPOLines.Controls.Add(Me.grpPOLines)
            Me.pnlPOLines.Controls.Add(Me.pnlReceiveActions)
            Me.pnlPOLines.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlPOLines.Location = New System.Drawing.Point(496, 0)
            Me.pnlPOLines.Name = "pnlPOLines"
            Me.pnlPOLines.Size = New System.Drawing.Size(540, 330)
            Me.pnlPOLines.TabIndex = 1
            '
            'grpPOLines
            '
            Me.grpPOLines.Controls.Add(Me.dgvPOLines)
            Me.grpPOLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpPOLines.Location = New System.Drawing.Point(0, 0)
            Me.grpPOLines.Name = "grpPOLines"
            Me.grpPOLines.Padding = New System.Windows.Forms.Padding(8, 28, 8, 8)
            Me.grpPOLines.Size = New System.Drawing.Size(496, 210)
            Me.grpPOLines.TabIndex = 0
            Me.grpPOLines.TabStop = False
            Me.grpPOLines.Text = "2. PO Items To Receive"
            '
            'dgvPOLines
            '
            Me.dgvPOLines.AllowUserToAddRows = False
            Me.dgvPOLines.AllowUserToDeleteRows = False
            Me.dgvPOLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvPOLines.BackgroundColor = System.Drawing.Color.White
            Me.dgvPOLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvPOLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvPOLines.Location = New System.Drawing.Point(8, 28)
            Me.dgvPOLines.MultiSelect = False
            Me.dgvPOLines.Name = "dgvPOLines"
            Me.dgvPOLines.ReadOnly = True
            Me.dgvPOLines.RowHeadersVisible = False
            Me.dgvPOLines.RowHeadersWidth = 51
            Me.dgvPOLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvPOLines.Size = New System.Drawing.Size(480, 174)
            Me.dgvPOLines.TabIndex = 0
            '
            'pnlReceiveActions
            '
            Me.pnlReceiveActions.BackColor = System.Drawing.Color.White
            Me.pnlReceiveActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlReceiveActions.Controls.Add(Me.lblCostImpactPreview)
            Me.pnlReceiveActions.Controls.Add(Me.btnReceiveGRN)
            Me.pnlReceiveActions.Controls.Add(Me.numReceiveQty)
            Me.pnlReceiveActions.Controls.Add(Me.lblReceiveQty)
            Me.pnlReceiveActions.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlReceiveActions.Location = New System.Drawing.Point(0, 210)
            Me.pnlReceiveActions.Name = "pnlReceiveActions"
            Me.pnlReceiveActions.Size = New System.Drawing.Size(496, 120)
            Me.pnlReceiveActions.TabIndex = 1
            '
            'lblCostImpactPreview
            '
            Me.lblCostImpactPreview.AutoSize = True
            Me.lblCostImpactPreview.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCostImpactPreview.ForeColor = System.Drawing.Color.FromArgb(CType(CType(30, Byte), Integer), CType(CType(126, Byte), Integer), CType(CType(52, Byte), Integer))
            Me.lblCostImpactPreview.Location = New System.Drawing.Point(12, 42)
            Me.lblCostImpactPreview.Name = "lblCostImpactPreview"
            Me.lblCostImpactPreview.Size = New System.Drawing.Size(434, 15)
            Me.lblCostImpactPreview.TabIndex = 3
            Me.lblCostImpactPreview.Text = "Moving Average Cost updates forward upon dock intake (Debit 1300 / Credit 2100)"
            '
            'btnReceiveGRN
            '
            Me.btnReceiveGRN.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnReceiveGRN.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.btnReceiveGRN.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnReceiveGRN.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnReceiveGRN.ForeColor = System.Drawing.Color.White
            Me.btnReceiveGRN.Location = New System.Drawing.Point(12, 70)
            Me.btnReceiveGRN.Name = "btnReceiveGRN"
            Me.btnReceiveGRN.Size = New System.Drawing.Size(470, 36)
            Me.btnReceiveGRN.TabIndex = 2
            Me.btnReceiveGRN.Text = "3. Confirm Dock Receipt & Post GRN"
            Me.btnReceiveGRN.UseMnemonic = False
            Me.btnReceiveGRN.UseVisualStyleBackColor = False
            '
            'numReceiveQty
            '
            Me.numReceiveQty.Location = New System.Drawing.Point(170, 12)
            Me.numReceiveQty.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numReceiveQty.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numReceiveQty.Name = "numReceiveQty"
            Me.numReceiveQty.Size = New System.Drawing.Size(120, 26)
            Me.numReceiveQty.TabIndex = 1
            Me.numReceiveQty.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'lblReceiveQty
            '
            Me.lblReceiveQty.AutoSize = True
            Me.lblReceiveQty.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblReceiveQty.Location = New System.Drawing.Point(12, 14)
            Me.lblReceiveQty.Name = "lblReceiveQty"
            Me.lblReceiveQty.Size = New System.Drawing.Size(117, 21)
            Me.lblReceiveQty.TabIndex = 0
            Me.lblReceiveQty.Text = "Receiving Qty:"
            '
            'grpGRNJournal
            '
            Me.grpGRNJournal.Controls.Add(Me.dgvGRNJournal)
            Me.grpGRNJournal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpGRNJournal.Location = New System.Drawing.Point(0, 0)
            Me.grpGRNJournal.Name = "grpGRNJournal"
            Me.grpGRNJournal.Padding = New System.Windows.Forms.Padding(8, 28, 8, 8)
            Me.grpGRNJournal.Size = New System.Drawing.Size(992, 218)
            Me.grpGRNJournal.TabIndex = 0
            Me.grpGRNJournal.TabStop = False
            Me.grpGRNJournal.Text = "Goods Receipt Note (GRN) Double-Entry General Ledger Posting History"
            '
            'dgvGRNJournal
            '
            Me.dgvGRNJournal.AllowUserToAddRows = False
            Me.dgvGRNJournal.AllowUserToDeleteRows = False
            Me.dgvGRNJournal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvGRNJournal.BackgroundColor = System.Drawing.Color.White
            Me.dgvGRNJournal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvGRNJournal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvGRNJournal.Location = New System.Drawing.Point(8, 28)
            Me.dgvGRNJournal.MultiSelect = False
            Me.dgvGRNJournal.Name = "dgvGRNJournal"
            Me.dgvGRNJournal.ReadOnly = True
            Me.dgvGRNJournal.RowHeadersVisible = False
            Me.dgvGRNJournal.RowHeadersWidth = 51
            Me.dgvGRNJournal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvGRNJournal.Size = New System.Drawing.Size(986, 191)
            Me.dgvGRNJournal.TabIndex = 0
            '
            'tabQuarantine
            '
            Me.tabQuarantine.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.tabQuarantine.Controls.Add(Me.splitRMA)
            Me.tabQuarantine.Location = New System.Drawing.Point(4, 30)
            Me.tabQuarantine.Name = "tabQuarantine"
            Me.tabQuarantine.Padding = New System.Windows.Forms.Padding(12)
            Me.tabQuarantine.Size = New System.Drawing.Size(1016, 576)
            Me.tabQuarantine.TabIndex = 1
            Me.tabQuarantine.Text = "RMA Quarantine & Inspection"
            '
            'splitRMA
            '
            Me.splitRMA.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitRMA.Location = New System.Drawing.Point(12, 12)
            Me.splitRMA.Name = "splitRMA"
            '
            'splitRMA.Panel1
            '
            Me.splitRMA.Panel1.Controls.Add(Me.grpRMAIntake)
            '
            'splitRMA.Panel2
            '
            Me.splitRMA.Panel2.Controls.Add(Me.grpInspection)
            Me.splitRMA.Panel2.Controls.Add(Me.grpRMAJournal)
            Me.splitRMA.Size = New System.Drawing.Size(992, 552)
            Me.splitRMA.SplitterDistance = 380
            Me.splitRMA.TabIndex = 0
            '
            'grpRMAIntake
            '
            Me.grpRMAIntake.Controls.Add(Me.pnlIntakeForm)
            Me.grpRMAIntake.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpRMAIntake.Location = New System.Drawing.Point(0, 0)
            Me.grpRMAIntake.Name = "grpRMAIntake"
            Me.grpRMAIntake.Size = New System.Drawing.Size(380, 552)
            Me.grpRMAIntake.TabIndex = 0
            Me.grpRMAIntake.TabStop = False
            Me.grpRMAIntake.Text = "Stage 1: RMA Intake to Quarantine"
            '
            'pnlIntakeForm
            '
            Me.pnlIntakeForm.AutoScroll = True
            Me.pnlIntakeForm.Controls.Add(Me.lblIntakeNotice)
            Me.pnlIntakeForm.Controls.Add(Me.btnIntakeToQuarantine)
            Me.pnlIntakeForm.Controls.Add(Me.numIntakeSellPrice)
            Me.pnlIntakeForm.Controls.Add(Me.lblIntakeSellPrice)
            Me.pnlIntakeForm.Controls.Add(Me.numIntakeOrigCost)
            Me.pnlIntakeForm.Controls.Add(Me.lblIntakeOrigCost)
            Me.pnlIntakeForm.Controls.Add(Me.numIntakeQty)
            Me.pnlIntakeForm.Controls.Add(Me.lblIntakeQty)
            Me.pnlIntakeForm.Controls.Add(Me.cmbIntakeProduct)
            Me.pnlIntakeForm.Controls.Add(Me.lblIntakeProduct)
            Me.pnlIntakeForm.Controls.Add(Me.txtIntakeInvoice)
            Me.pnlIntakeForm.Controls.Add(Me.lblIntakeInvoice)
            Me.pnlIntakeForm.Controls.Add(Me.txtIntakeCustomer)
            Me.pnlIntakeForm.Controls.Add(Me.lblIntakeCustomer)
            Me.pnlIntakeForm.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlIntakeForm.Location = New System.Drawing.Point(3, 24)
            Me.pnlIntakeForm.Name = "pnlIntakeForm"
            Me.pnlIntakeForm.Padding = New System.Windows.Forms.Padding(12)
            Me.pnlIntakeForm.Size = New System.Drawing.Size(374, 525)
            Me.pnlIntakeForm.TabIndex = 0
            '
            'lblIntakeNotice
            '
            Me.lblIntakeNotice.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(205, Byte), Integer))
            Me.lblIntakeNotice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblIntakeNotice.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblIntakeNotice.ForeColor = System.Drawing.Color.FromArgb(CType(CType(133, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(4, Byte), Integer))
            Me.lblIntakeNotice.Location = New System.Drawing.Point(12, 385)
            Me.lblIntakeNotice.Name = "lblIntakeNotice"
            Me.lblIntakeNotice.Padding = New System.Windows.Forms.Padding(8)
            Me.lblIntakeNotice.Size = New System.Drawing.Size(345, 75)
            Me.lblIntakeNotice.TabIndex = 13
            Me.lblIntakeNotice.Text = "Notice: Stage 1 intakes goods into operational non-valuated quarantine (QuarantineQty). No General Ledger entries are created until Stage 2 inspection."
            '
            'btnIntakeToQuarantine
            '
            Me.btnIntakeToQuarantine.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.btnIntakeToQuarantine.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnIntakeToQuarantine.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnIntakeToQuarantine.ForeColor = System.Drawing.Color.White
            Me.btnIntakeToQuarantine.Location = New System.Drawing.Point(12, 338)
            Me.btnIntakeToQuarantine.Name = "btnIntakeToQuarantine"
            Me.btnIntakeToQuarantine.Size = New System.Drawing.Size(345, 38)
            Me.btnIntakeToQuarantine.TabIndex = 12
            Me.btnIntakeToQuarantine.Text = "Log Inbound RMA to Quarantine"
            Me.btnIntakeToQuarantine.UseVisualStyleBackColor = False
            '
            'numIntakeSellPrice
            '
            Me.numIntakeSellPrice.DecimalPlaces = 2
            Me.numIntakeSellPrice.Location = New System.Drawing.Point(12, 298)
            Me.numIntakeSellPrice.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numIntakeSellPrice.Name = "numIntakeSellPrice"
            Me.numIntakeSellPrice.Size = New System.Drawing.Size(345, 27)
            Me.numIntakeSellPrice.TabIndex = 11
            Me.numIntakeSellPrice.Value = New Decimal(New Integer() {2500, 0, 0, 131072})
            '
            'lblIntakeSellPrice
            '
            Me.lblIntakeSellPrice.AutoSize = True
            Me.lblIntakeSellPrice.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblIntakeSellPrice.Location = New System.Drawing.Point(12, 276)
            Me.lblIntakeSellPrice.Name = "lblIntakeSellPrice"
            Me.lblIntakeSellPrice.Size = New System.Drawing.Size(188, 20)
            Me.lblIntakeSellPrice.TabIndex = 10
            Me.lblIntakeSellPrice.Text = "Original Selling Unit Price ($):"
            '
            'numIntakeOrigCost
            '
            Me.numIntakeOrigCost.DecimalPlaces = 2
            Me.numIntakeOrigCost.Location = New System.Drawing.Point(12, 242)
            Me.numIntakeOrigCost.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numIntakeOrigCost.Name = "numIntakeOrigCost"
            Me.numIntakeOrigCost.Size = New System.Drawing.Size(345, 27)
            Me.numIntakeOrigCost.TabIndex = 9
            Me.numIntakeOrigCost.Value = New Decimal(New Integer() {1500, 0, 0, 131072})
            '
            'lblIntakeOrigCost
            '
            Me.lblIntakeOrigCost.AutoSize = True
            Me.lblIntakeOrigCost.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblIntakeOrigCost.Location = New System.Drawing.Point(12, 220)
            Me.lblIntakeOrigCost.Name = "lblIntakeOrigCost"
            Me.lblIntakeOrigCost.Size = New System.Drawing.Size(193, 20)
            Me.lblIntakeOrigCost.TabIndex = 8
            Me.lblIntakeOrigCost.Text = "Original Invoiced Cost ($/ea):"
            '
            'numIntakeQty
            '
            Me.numIntakeQty.Location = New System.Drawing.Point(12, 186)
            Me.numIntakeQty.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
            Me.numIntakeQty.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numIntakeQty.Name = "numIntakeQty"
            Me.numIntakeQty.Size = New System.Drawing.Size(345, 27)
            Me.numIntakeQty.TabIndex = 7
            Me.numIntakeQty.Value = New Decimal(New Integer() {5, 0, 0, 0})
            '
            'lblIntakeQty
            '
            Me.lblIntakeQty.AutoSize = True
            Me.lblIntakeQty.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblIntakeQty.Location = New System.Drawing.Point(12, 164)
            Me.lblIntakeQty.Name = "lblIntakeQty"
            Me.lblIntakeQty.Size = New System.Drawing.Size(161, 20)
            Me.lblIntakeQty.TabIndex = 6
            Me.lblIntakeQty.Text = "Physical Returned Qty:"
            '
            'cmbIntakeProduct
            '
            Me.cmbIntakeProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbIntakeProduct.FormattingEnabled = True
            Me.cmbIntakeProduct.Location = New System.Drawing.Point(12, 130)
            Me.cmbIntakeProduct.Name = "cmbIntakeProduct"
            Me.cmbIntakeProduct.Size = New System.Drawing.Size(345, 28)
            Me.cmbIntakeProduct.TabIndex = 5
            '
            'lblIntakeProduct
            '
            Me.lblIntakeProduct.AutoSize = True
            Me.lblIntakeProduct.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblIntakeProduct.Location = New System.Drawing.Point(12, 108)
            Me.lblIntakeProduct.Name = "lblIntakeProduct"
            Me.lblIntakeProduct.Size = New System.Drawing.Size(107, 20)
            Me.lblIntakeProduct.TabIndex = 4
            Me.lblIntakeProduct.Text = "Product (SKU):"
            '
            'txtIntakeInvoice
            '
            Me.txtIntakeInvoice.Location = New System.Drawing.Point(12, 74)
            Me.txtIntakeInvoice.Name = "txtIntakeInvoice"
            Me.txtIntakeInvoice.Size = New System.Drawing.Size(345, 27)
            Me.txtIntakeInvoice.TabIndex = 3
            Me.txtIntakeInvoice.Text = "INV-2026-0205"
            '
            'lblIntakeInvoice
            '
            Me.lblIntakeInvoice.AutoSize = True
            Me.lblIntakeInvoice.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblIntakeInvoice.Location = New System.Drawing.Point(12, 52)
            Me.lblIntakeInvoice.Name = "lblIntakeInvoice"
            Me.lblIntakeInvoice.Size = New System.Drawing.Size(176, 20)
            Me.lblIntakeInvoice.TabIndex = 2
            Me.lblIntakeInvoice.Text = "Original Sales Invoice #:"
            '
            'txtIntakeCustomer
            '
            Me.txtIntakeCustomer.Location = New System.Drawing.Point(12, 22)
            Me.txtIntakeCustomer.Name = "txtIntakeCustomer"
            Me.txtIntakeCustomer.Size = New System.Drawing.Size(345, 27)
            Me.txtIntakeCustomer.TabIndex = 1
            Me.txtIntakeCustomer.Text = "Riverside Cafe & Roastery"
            '
            'lblIntakeCustomer
            '
            Me.lblIntakeCustomer.AutoSize = True
            Me.lblIntakeCustomer.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblIntakeCustomer.Location = New System.Drawing.Point(12, 0)
            Me.lblIntakeCustomer.Name = "lblIntakeCustomer"
            Me.lblIntakeCustomer.Size = New System.Drawing.Size(126, 20)
            Me.lblIntakeCustomer.TabIndex = 0
            Me.lblIntakeCustomer.Text = "Customer Name:"
            '
            'grpInspection
            '
            Me.grpInspection.Controls.Add(Me.dgvQuarantineItems)
            Me.grpInspection.Controls.Add(Me.pnlInspectionActions)
            Me.grpInspection.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpInspection.Location = New System.Drawing.Point(0, 0)
            Me.grpInspection.Name = "grpInspection"
            Me.grpInspection.Padding = New System.Windows.Forms.Padding(8, 28, 8, 8)
            Me.grpInspection.Size = New System.Drawing.Size(608, 350)
            Me.grpInspection.TabIndex = 0
            Me.grpInspection.TabStop = False
            Me.grpInspection.Text = "Stage 2: Quarantine Inspection & Disposition (Restock vs. Scrap)"
            '
            'dgvQuarantineItems
            '
            Me.dgvQuarantineItems.AllowUserToAddRows = False
            Me.dgvQuarantineItems.AllowUserToDeleteRows = False
            Me.dgvQuarantineItems.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvQuarantineItems.BackgroundColor = System.Drawing.Color.White
            Me.dgvQuarantineItems.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvQuarantineItems.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvQuarantineItems.Location = New System.Drawing.Point(8, 28)
            Me.dgvQuarantineItems.MultiSelect = False
            Me.dgvQuarantineItems.Name = "dgvQuarantineItems"
            Me.dgvQuarantineItems.ReadOnly = True
            Me.dgvQuarantineItems.RowHeadersVisible = False
            Me.dgvQuarantineItems.RowHeadersWidth = 51
            Me.dgvQuarantineItems.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvQuarantineItems.Size = New System.Drawing.Size(602, 196)
            Me.dgvQuarantineItems.TabIndex = 0
            '
            'pnlInspectionActions
            '
            Me.pnlInspectionActions.BackColor = System.Drawing.Color.White
            Me.pnlInspectionActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlInspectionActions.Controls.Add(Me.lblInspectionSummary)
            Me.pnlInspectionActions.Controls.Add(Me.btnConfirmInspection)
            Me.pnlInspectionActions.Controls.Add(Me.numScrapQty)
            Me.pnlInspectionActions.Controls.Add(Me.lblScrapQty)
            Me.pnlInspectionActions.Controls.Add(Me.numRestockQty)
            Me.pnlInspectionActions.Controls.Add(Me.lblRestockQty)
            Me.pnlInspectionActions.Controls.Add(Me.lblSelectedRMA)
            Me.pnlInspectionActions.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlInspectionActions.Location = New System.Drawing.Point(3, 220)
            Me.pnlInspectionActions.Name = "pnlInspectionActions"
            Me.pnlInspectionActions.Size = New System.Drawing.Size(602, 127)
            Me.pnlInspectionActions.TabIndex = 1
            '
            'lblInspectionSummary
            '
            Me.lblInspectionSummary.AutoSize = True
            Me.lblInspectionSummary.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblInspectionSummary.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblInspectionSummary.Location = New System.Drawing.Point(12, 60)
            Me.lblInspectionSummary.Name = "lblInspectionSummary"
            Me.lblInspectionSummary.Size = New System.Drawing.Size(490, 19)
            Me.lblInspectionSummary.TabIndex = 6
            Me.lblInspectionSummary.Text = "Disposition will credit customer AR and route Restock -> 1300 / Scrap -> 5300"
            '
            'btnConfirmInspection
            '
            Me.btnConfirmInspection.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.btnConfirmInspection.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnConfirmInspection.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnConfirmInspection.ForeColor = System.Drawing.Color.White
            Me.btnConfirmInspection.Location = New System.Drawing.Point(12, 85)
            Me.btnConfirmInspection.Name = "btnConfirmInspection"
            Me.btnConfirmInspection.Size = New System.Drawing.Size(575, 32)
            Me.btnConfirmInspection.TabIndex = 5
            Me.btnConfirmInspection.Text = "Confirm Inspection & Post Credit Note Journal"
            Me.btnConfirmInspection.UseMnemonic = False
            Me.btnConfirmInspection.UseVisualStyleBackColor = False
            '
            'numScrapQty
            '
            Me.numScrapQty.Location = New System.Drawing.Point(495, 28)
            Me.numScrapQty.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
            Me.numScrapQty.Name = "numScrapQty"
            Me.numScrapQty.Size = New System.Drawing.Size(92, 28)
            Me.numScrapQty.TabIndex = 4
            '
            'lblScrapQty
            '
            Me.lblScrapQty.AutoSize = True
            Me.lblScrapQty.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblScrapQty.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblScrapQty.Location = New System.Drawing.Point(320, 31)
            Me.lblScrapQty.Name = "lblScrapQty"
            Me.lblScrapQty.Size = New System.Drawing.Size(167, 20)
            Me.lblScrapQty.TabIndex = 3
            Me.lblScrapQty.Text = "Scrap Qty (Acct 5300):"
            '
            'numRestockQty
            '
            Me.numRestockQty.Location = New System.Drawing.Point(205, 28)
            Me.numRestockQty.Maximum = New Decimal(New Integer() {10000, 0, 0, 0})
            Me.numRestockQty.Name = "numRestockQty"
            Me.numRestockQty.Size = New System.Drawing.Size(92, 28)
            Me.numRestockQty.TabIndex = 2
            '
            'lblRestockQty
            '
            Me.lblRestockQty.AutoSize = True
            Me.lblRestockQty.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblRestockQty.ForeColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.lblRestockQty.Location = New System.Drawing.Point(12, 31)
            Me.lblRestockQty.Name = "lblRestockQty"
            Me.lblRestockQty.Size = New System.Drawing.Size(185, 20)
            Me.lblRestockQty.TabIndex = 1
            Me.lblRestockQty.Text = "Restock Qty (Acct 1300):"
            '
            'lblSelectedRMA
            '
            Me.lblSelectedRMA.AutoSize = True
            Me.lblSelectedRMA.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSelectedRMA.Location = New System.Drawing.Point(12, 6)
            Me.lblSelectedRMA.Name = "lblSelectedRMA"
            Me.lblSelectedRMA.Size = New System.Drawing.Size(176, 20)
            Me.lblSelectedRMA.TabIndex = 0
            Me.lblSelectedRMA.Text = "Selected RMA: None"
            '
            'grpRMAJournal
            '
            Me.grpRMAJournal.Controls.Add(Me.dgvRMAJournal)
            Me.grpRMAJournal.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.grpRMAJournal.Location = New System.Drawing.Point(0, 350)
            Me.grpRMAJournal.Name = "grpRMAJournal"
            Me.grpRMAJournal.Padding = New System.Windows.Forms.Padding(8, 28, 8, 8)
            Me.grpRMAJournal.Size = New System.Drawing.Size(608, 202)
            Me.grpRMAJournal.TabIndex = 1
            Me.grpRMAJournal.TabStop = False
            Me.grpRMAJournal.Text = "Customer Credit Note (CRN) Journal Entries"
            '
            'dgvRMAJournal
            '
            Me.dgvRMAJournal.AllowUserToAddRows = False
            Me.dgvRMAJournal.AllowUserToDeleteRows = False
            Me.dgvRMAJournal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvRMAJournal.BackgroundColor = System.Drawing.Color.White
            Me.dgvRMAJournal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvRMAJournal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvRMAJournal.Location = New System.Drawing.Point(8, 28)
            Me.dgvRMAJournal.MultiSelect = False
            Me.dgvRMAJournal.Name = "dgvRMAJournal"
            Me.dgvRMAJournal.ReadOnly = True
            Me.dgvRMAJournal.RowHeadersVisible = False
            Me.dgvRMAJournal.RowHeadersWidth = 51
            Me.dgvRMAJournal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvRMAJournal.Size = New System.Drawing.Size(602, 175)
            Me.dgvRMAJournal.TabIndex = 0
            '
            'tabStockAdjustments
            '
            Me.tabStockAdjustments.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.tabStockAdjustments.Controls.Add(Me.pnlAdjMain)
            Me.tabStockAdjustments.Location = New System.Drawing.Point(4, 30)
            Me.tabStockAdjustments.Name = "tabStockAdjustments"
            Me.tabStockAdjustments.Padding = New System.Windows.Forms.Padding(12)
            Me.tabStockAdjustments.Size = New System.Drawing.Size(1016, 576)
            Me.tabStockAdjustments.TabIndex = 2
            Me.tabStockAdjustments.Text = "Stock Adjustments & Cycle Count"
            '
            'pnlAdjMain
            '
            Me.pnlAdjMain.Controls.Add(Me.splitStockAdjustments)
            Me.pnlAdjMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.pnlAdjMain.Location = New System.Drawing.Point(12, 12)
            Me.pnlAdjMain.Name = "pnlAdjMain"
            Me.pnlAdjMain.Size = New System.Drawing.Size(992, 552)
            Me.pnlAdjMain.TabIndex = 0
            '
            'splitStockAdjustments
            '
            Me.splitStockAdjustments.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitStockAdjustments.Location = New System.Drawing.Point(0, 0)
            Me.splitStockAdjustments.Name = "splitStockAdjustments"
            Me.splitStockAdjustments.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitStockAdjustments.Panel1
            '
            Me.splitStockAdjustments.Panel1.Controls.Add(Me.grpAdjLines)
            Me.splitStockAdjustments.Panel1.Controls.Add(Me.grpAdjustmentEntry)
            '
            'splitStockAdjustments.Panel2
            '
            Me.splitStockAdjustments.Panel2.Controls.Add(Me.grpAdjJournal)
            Me.splitStockAdjustments.Size = New System.Drawing.Size(992, 552)
            Me.splitStockAdjustments.SplitterDistance = 320
            Me.splitStockAdjustments.TabIndex = 0
            '
            'grpAdjustmentEntry
            '
            Me.grpAdjustmentEntry.Controls.Add(Me.btnClearAdjLines)
            Me.grpAdjustmentEntry.Controls.Add(Me.btnAddAdjLine)
            Me.grpAdjustmentEntry.Controls.Add(Me.txtAdjNotes)
            Me.grpAdjustmentEntry.Controls.Add(Me.lblAdjNotes)
            Me.grpAdjustmentEntry.Controls.Add(Me.numAdjVariance)
            Me.grpAdjustmentEntry.Controls.Add(Me.lblAdjVariance)
            Me.grpAdjustmentEntry.Controls.Add(Me.cmbAdjReason)
            Me.grpAdjustmentEntry.Controls.Add(Me.lblAdjReason)
            Me.grpAdjustmentEntry.Controls.Add(Me.lblAdjUnitCost)
            Me.grpAdjustmentEntry.Controls.Add(Me.lblAdjOnHand)
            Me.grpAdjustmentEntry.Controls.Add(Me.cmbAdjProduct)
            Me.grpAdjustmentEntry.Controls.Add(Me.lblAdjProduct)
            Me.grpAdjustmentEntry.Dock = System.Windows.Forms.DockStyle.Left
            Me.grpAdjustmentEntry.Location = New System.Drawing.Point(0, 0)
            Me.grpAdjustmentEntry.Name = "grpAdjustmentEntry"
            Me.grpAdjustmentEntry.Padding = New System.Windows.Forms.Padding(12, 24, 12, 12)
            Me.grpAdjustmentEntry.Size = New System.Drawing.Size(330, 320)
            Me.grpAdjustmentEntry.TabIndex = 0
            Me.grpAdjustmentEntry.TabStop = False
            Me.grpAdjustmentEntry.Text = "1. Physical Count & Adjustment Input"
            '
            'lblAdjProduct
            '
            Me.lblAdjProduct.AutoSize = True
            Me.lblAdjProduct.Location = New System.Drawing.Point(12, 24)
            Me.lblAdjProduct.Name = "lblAdjProduct"
            Me.lblAdjProduct.Size = New System.Drawing.Size(142, 20)
            Me.lblAdjProduct.TabIndex = 0
            Me.lblAdjProduct.Text = "Select Product (SKU):"
            '
            'cmbAdjProduct
            '
            Me.cmbAdjProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbAdjProduct.FormattingEnabled = True
            Me.cmbAdjProduct.Location = New System.Drawing.Point(12, 46)
            Me.cmbAdjProduct.Name = "cmbAdjProduct"
            Me.cmbAdjProduct.Size = New System.Drawing.Size(305, 28)
            Me.cmbAdjProduct.TabIndex = 1
            '
            'lblAdjOnHand
            '
            Me.lblAdjOnHand.AutoSize = True
            Me.lblAdjOnHand.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAdjOnHand.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblAdjOnHand.Location = New System.Drawing.Point(12, 78)
            Me.lblAdjOnHand.Name = "lblAdjOnHand"
            Me.lblAdjOnHand.Size = New System.Drawing.Size(116, 20)
            Me.lblAdjOnHand.TabIndex = 2
            Me.lblAdjOnHand.Text = "On Hand: 0 Units"
            '
            'lblAdjUnitCost
            '
            Me.lblAdjUnitCost.AutoSize = True
            Me.lblAdjUnitCost.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblAdjUnitCost.Location = New System.Drawing.Point(170, 78)
            Me.lblAdjUnitCost.Name = "lblAdjUnitCost"
            Me.lblAdjUnitCost.Size = New System.Drawing.Size(118, 20)
            Me.lblAdjUnitCost.TabIndex = 3
            Me.lblAdjUnitCost.Text = "Unit Cost: $0.00"
            '
            'lblAdjReason
            '
            Me.lblAdjReason.AutoSize = True
            Me.lblAdjReason.Location = New System.Drawing.Point(12, 105)
            Me.lblAdjReason.Name = "lblAdjReason"
            Me.lblAdjReason.Size = New System.Drawing.Size(123, 20)
            Me.lblAdjReason.TabIndex = 4
            Me.lblAdjReason.Text = "Adjustment Reason:"
            '
            'cmbAdjReason
            '
            Me.cmbAdjReason.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbAdjReason.FormattingEnabled = True
            Me.cmbAdjReason.Location = New System.Drawing.Point(12, 127)
            Me.cmbAdjReason.Name = "cmbAdjReason"
            Me.cmbAdjReason.Size = New System.Drawing.Size(305, 28)
            Me.cmbAdjReason.TabIndex = 5
            '
            'lblAdjVariance
            '
            Me.lblAdjVariance.AutoSize = True
            Me.lblAdjVariance.Location = New System.Drawing.Point(12, 160)
            Me.lblAdjVariance.Name = "lblAdjVariance"
            Me.lblAdjVariance.Size = New System.Drawing.Size(193, 20)
            Me.lblAdjVariance.TabIndex = 6
            Me.lblAdjVariance.Text = "Count Variance (+/- Qty):"
            '
            'numAdjVariance
            '
            Me.numAdjVariance.Location = New System.Drawing.Point(12, 182)
            Me.numAdjVariance.Maximum = New Decimal(New Integer() {99999, 0, 0, 0})
            Me.numAdjVariance.Minimum = New Decimal(New Integer() {99999, 0, 0, -2147483648})
            Me.numAdjVariance.Name = "numAdjVariance"
            Me.numAdjVariance.Size = New System.Drawing.Size(145, 27)
            Me.numAdjVariance.TabIndex = 7
            Me.numAdjVariance.TextAlign = System.Windows.Forms.HorizontalAlignment.Right
            '
            'lblAdjNotes
            '
            Me.lblAdjNotes.AutoSize = True
            Me.lblAdjNotes.Location = New System.Drawing.Point(12, 215)
            Me.lblAdjNotes.Name = "lblAdjNotes"
            Me.lblAdjNotes.Size = New System.Drawing.Size(142, 20)
            Me.lblAdjNotes.TabIndex = 8
            Me.lblAdjNotes.Text = "Notes / Reason Detail:"
            '
            'txtAdjNotes
            '
            Me.txtAdjNotes.Location = New System.Drawing.Point(12, 237)
            Me.txtAdjNotes.Name = "txtAdjNotes"
            Me.txtAdjNotes.Size = New System.Drawing.Size(305, 27)
            Me.txtAdjNotes.TabIndex = 9
            '
            'btnAddAdjLine
            '
            Me.btnAddAdjLine.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.btnAddAdjLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnAddAdjLine.ForeColor = System.Drawing.Color.White
            Me.btnAddAdjLine.Location = New System.Drawing.Point(12, 272)
            Me.btnAddAdjLine.Name = "btnAddAdjLine"
            Me.btnAddAdjLine.Size = New System.Drawing.Size(145, 34)
            Me.btnAddAdjLine.TabIndex = 10
            Me.btnAddAdjLine.Text = "+ Add Line"
            Me.btnAddAdjLine.UseVisualStyleBackColor = False
            '
            'btnClearAdjLines
            '
            Me.btnClearAdjLines.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnClearAdjLines.Location = New System.Drawing.Point(170, 272)
            Me.btnClearAdjLines.Name = "btnClearAdjLines"
            Me.btnClearAdjLines.Size = New System.Drawing.Size(145, 34)
            Me.btnClearAdjLines.TabIndex = 11
            Me.btnClearAdjLines.Text = "Clear All"
            Me.btnClearAdjLines.UseVisualStyleBackColor = True
            '
            'grpAdjLines
            '
            Me.grpAdjLines.Controls.Add(Me.dgvAdjLines)
            Me.grpAdjLines.Controls.Add(Me.pnlAdjLineButtons)
            Me.grpAdjLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpAdjLines.Location = New System.Drawing.Point(330, 0)
            Me.grpAdjLines.Name = "grpAdjLines"
            Me.grpAdjLines.Padding = New System.Windows.Forms.Padding(8, 24, 8, 8)
            Me.grpAdjLines.Size = New System.Drawing.Size(662, 320)
            Me.grpAdjLines.TabIndex = 1
            Me.grpAdjLines.TabStop = False
            Me.grpAdjLines.Text = "2. Pending Stock Adjustment Lines"
            '
            'dgvAdjLines
            '
            Me.dgvAdjLines.AllowUserToAddRows = False
            Me.dgvAdjLines.AllowUserToDeleteRows = False
            Me.dgvAdjLines.BackgroundColor = System.Drawing.Color.White
            Me.dgvAdjLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvAdjLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvAdjLines.Location = New System.Drawing.Point(8, 24)
            Me.dgvAdjLines.MultiSelect = False
            Me.dgvAdjLines.Name = "dgvAdjLines"
            Me.dgvAdjLines.ReadOnly = True
            Me.dgvAdjLines.RowHeadersVisible = False
            Me.dgvAdjLines.RowHeadersWidth = 51
            Me.dgvAdjLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvAdjLines.Size = New System.Drawing.Size(646, 252)
            Me.dgvAdjLines.TabIndex = 0
            '
            'pnlAdjLineButtons
            '
            Me.pnlAdjLineButtons.Controls.Add(Me.btnRemoveAdjLine)
            Me.pnlAdjLineButtons.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlAdjLineButtons.Location = New System.Drawing.Point(8, 276)
            Me.pnlAdjLineButtons.Name = "pnlAdjLineButtons"
            Me.pnlAdjLineButtons.Size = New System.Drawing.Size(646, 36)
            Me.pnlAdjLineButtons.TabIndex = 1
            '
            'btnRemoveAdjLine
            '
            Me.btnRemoveAdjLine.Dock = System.Windows.Forms.DockStyle.Right
            Me.btnRemoveAdjLine.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnRemoveAdjLine.Location = New System.Drawing.Point(506, 0)
            Me.btnRemoveAdjLine.Name = "btnRemoveAdjLine"
            Me.btnRemoveAdjLine.Size = New System.Drawing.Size(140, 36)
            Me.btnRemoveAdjLine.TabIndex = 0
            Me.btnRemoveAdjLine.Text = "Remove Line"
            Me.btnRemoveAdjLine.UseVisualStyleBackColor = True
            '
            'grpAdjJournal
            '
            Me.grpAdjJournal.Controls.Add(Me.ctrlAdjJournal)
            Me.grpAdjJournal.Controls.Add(Me.pnlAdjSubmit)
            Me.grpAdjJournal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpAdjJournal.Location = New System.Drawing.Point(0, 0)
            Me.grpAdjJournal.Name = "grpAdjJournal"
            Me.grpAdjJournal.Padding = New System.Windows.Forms.Padding(8, 24, 8, 8)
            Me.grpAdjJournal.Size = New System.Drawing.Size(992, 228)
            Me.grpAdjJournal.TabIndex = 0
            Me.grpAdjJournal.TabStop = False
            Me.grpAdjJournal.Text = "3. Live Double-Entry General Ledger Impact Preview"
            '
            'ctrlAdjJournal
            '
            Me.ctrlAdjJournal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.ctrlAdjJournal.Location = New System.Drawing.Point(8, 24)
            Me.ctrlAdjJournal.Name = "ctrlAdjJournal"
            Me.ctrlAdjJournal.Size = New System.Drawing.Size(976, 150)
            Me.ctrlAdjJournal.TabIndex = 0
            '
            'pnlAdjSubmit
            '
            Me.pnlAdjSubmit.Controls.Add(Me.lblAdjStatus)
            Me.pnlAdjSubmit.Controls.Add(Me.btnSubmitAdjustment)
            Me.pnlAdjSubmit.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlAdjSubmit.Location = New System.Drawing.Point(8, 174)
            Me.pnlAdjSubmit.Name = "pnlAdjSubmit"
            Me.pnlAdjSubmit.Padding = New System.Windows.Forms.Padding(0, 4, 0, 0)
            Me.pnlAdjSubmit.Size = New System.Drawing.Size(976, 46)
            Me.pnlAdjSubmit.TabIndex = 1
            '
            'lblAdjStatus
            '
            Me.lblAdjStatus.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblAdjStatus.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblAdjStatus.Location = New System.Drawing.Point(0, 4)
            Me.lblAdjStatus.Name = "lblAdjStatus"
            Me.lblAdjStatus.Size = New System.Drawing.Size(656, 42)
            Me.lblAdjStatus.TabIndex = 0
            Me.lblAdjStatus.Text = "Add items above to preview double-entry postings."
            Me.lblAdjStatus.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnSubmitAdjustment
            '
            Me.btnSubmitAdjustment.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.btnSubmitAdjustment.Dock = System.Windows.Forms.DockStyle.Right
            Me.btnSubmitAdjustment.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSubmitAdjustment.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnSubmitAdjustment.ForeColor = System.Drawing.Color.White
            Me.btnSubmitAdjustment.Location = New System.Drawing.Point(656, 4)
            Me.btnSubmitAdjustment.Name = "btnSubmitAdjustment"
            Me.btnSubmitAdjustment.Size = New System.Drawing.Size(320, 42)
            Me.btnSubmitAdjustment.TabIndex = 1
            Me.btnSubmitAdjustment.Text = "Post Adjustment & Update Ledger"
            Me.btnSubmitAdjustment.UseVisualStyleBackColor = False
            '
            'WarehouseIntakeForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(1200, 720)
            Me.Controls.Add(Me.tabMain)
            Me.Controls.Add(Me.pnlHeader)
            Me.Name = "WarehouseIntakeForm"
            Me.Text = "Warehouse Dock Intake & RMA Inspection"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.tabMain.ResumeLayout(False)
            Me.tabDockReceiving.ResumeLayout(False)
            Me.pnlDockMain.ResumeLayout(False)
            Me.splitDock.Panel1.ResumeLayout(False)
            Me.splitDock.Panel2.ResumeLayout(False)
            CType(Me.splitDock, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitDock.ResumeLayout(False)
            Me.grpOpenPOs.ResumeLayout(False)
            CType(Me.dgvOpenPOs, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlPOLines.ResumeLayout(False)
            Me.grpPOLines.ResumeLayout(False)
            CType(Me.dgvPOLines, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlReceiveActions.ResumeLayout(False)
            Me.pnlReceiveActions.PerformLayout()
            CType(Me.numReceiveQty, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpGRNJournal.ResumeLayout(False)
            CType(Me.dgvGRNJournal, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabQuarantine.ResumeLayout(False)
            Me.splitRMA.Panel1.ResumeLayout(False)
            Me.splitRMA.Panel2.ResumeLayout(False)
            CType(Me.splitRMA, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitRMA.ResumeLayout(False)
            Me.grpRMAIntake.ResumeLayout(False)
            Me.pnlIntakeForm.ResumeLayout(False)
            Me.pnlIntakeForm.PerformLayout()
            CType(Me.numIntakeSellPrice, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numIntakeOrigCost, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numIntakeQty, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpInspection.ResumeLayout(False)
            CType(Me.dgvQuarantineItems, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlInspectionActions.ResumeLayout(False)
            Me.pnlInspectionActions.PerformLayout()
            CType(Me.numScrapQty, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numRestockQty, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpRMAJournal.ResumeLayout(False)
            CType(Me.dgvRMAJournal, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabStockAdjustments.ResumeLayout(False)
            Me.pnlAdjMain.ResumeLayout(False)
            Me.splitStockAdjustments.Panel1.ResumeLayout(False)
            Me.splitStockAdjustments.Panel2.ResumeLayout(False)
            CType(Me.splitStockAdjustments, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitStockAdjustments.ResumeLayout(False)
            Me.grpAdjustmentEntry.ResumeLayout(False)
            Me.grpAdjustmentEntry.PerformLayout()
            CType(Me.numAdjVariance, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpAdjLines.ResumeLayout(False)
            CType(Me.dgvAdjLines, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlAdjLineButtons.ResumeLayout(False)
            Me.grpAdjJournal.ResumeLayout(False)
            Me.pnlAdjSubmit.ResumeLayout(False)
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents lblSubtitle As System.Windows.Forms.Label
        Friend WithEvents tabMain As System.Windows.Forms.TabControl
        Friend WithEvents tabDockReceiving As System.Windows.Forms.TabPage
        Friend WithEvents tabQuarantine As System.Windows.Forms.TabPage
        Friend WithEvents tabStockAdjustments As System.Windows.Forms.TabPage
        Friend WithEvents pnlAdjMain As System.Windows.Forms.Panel
        Friend WithEvents splitStockAdjustments As System.Windows.Forms.SplitContainer
        Friend WithEvents grpAdjustmentEntry As System.Windows.Forms.GroupBox
        Friend WithEvents lblAdjProduct As System.Windows.Forms.Label
        Friend WithEvents cmbAdjProduct As System.Windows.Forms.ComboBox
        Friend WithEvents lblAdjOnHand As System.Windows.Forms.Label
        Friend WithEvents lblAdjUnitCost As System.Windows.Forms.Label
        Friend WithEvents lblAdjReason As System.Windows.Forms.Label
        Friend WithEvents cmbAdjReason As System.Windows.Forms.ComboBox
        Friend WithEvents lblAdjVariance As System.Windows.Forms.Label
        Friend WithEvents numAdjVariance As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblAdjNotes As System.Windows.Forms.Label
        Friend WithEvents txtAdjNotes As System.Windows.Forms.TextBox
        Friend WithEvents btnAddAdjLine As System.Windows.Forms.Button
        Friend WithEvents btnClearAdjLines As System.Windows.Forms.Button
        Friend WithEvents grpAdjLines As System.Windows.Forms.GroupBox
        Friend WithEvents dgvAdjLines As System.Windows.Forms.DataGridView
        Friend WithEvents pnlAdjLineButtons As System.Windows.Forms.Panel
        Friend WithEvents btnRemoveAdjLine As System.Windows.Forms.Button
        Friend WithEvents grpAdjJournal As System.Windows.Forms.GroupBox
        Friend WithEvents ctrlAdjJournal As Merchandising_Accounting_App.UI.Controls.DoubleEntryDistributionGridControl
        Friend WithEvents pnlAdjSubmit As System.Windows.Forms.Panel
        Friend WithEvents lblAdjStatus As System.Windows.Forms.Label
        Friend WithEvents btnSubmitAdjustment As System.Windows.Forms.Button
        Friend WithEvents pnlDockMain As System.Windows.Forms.Panel
        Friend WithEvents splitDock As System.Windows.Forms.SplitContainer
        Friend WithEvents grpOpenPOs As System.Windows.Forms.GroupBox
        Friend WithEvents dgvOpenPOs As System.Windows.Forms.DataGridView
        Friend WithEvents pnlPOLines As System.Windows.Forms.Panel
        Friend WithEvents grpPOLines As System.Windows.Forms.GroupBox
        Friend WithEvents dgvPOLines As System.Windows.Forms.DataGridView
        Friend WithEvents pnlReceiveActions As System.Windows.Forms.Panel
        Friend WithEvents lblReceiveQty As System.Windows.Forms.Label
        Friend WithEvents numReceiveQty As System.Windows.Forms.NumericUpDown
        Friend WithEvents btnReceiveGRN As System.Windows.Forms.Button
        Friend WithEvents lblCostImpactPreview As System.Windows.Forms.Label
        Friend WithEvents grpGRNJournal As System.Windows.Forms.GroupBox
        Friend WithEvents dgvGRNJournal As System.Windows.Forms.DataGridView
        Friend WithEvents splitRMA As System.Windows.Forms.SplitContainer
        Friend WithEvents grpRMAIntake As System.Windows.Forms.GroupBox
        Friend WithEvents pnlIntakeForm As System.Windows.Forms.Panel
        Friend WithEvents lblIntakeCustomer As System.Windows.Forms.Label
        Friend WithEvents txtIntakeCustomer As System.Windows.Forms.TextBox
        Friend WithEvents lblIntakeInvoice As System.Windows.Forms.Label
        Friend WithEvents txtIntakeInvoice As System.Windows.Forms.TextBox
        Friend WithEvents lblIntakeProduct As System.Windows.Forms.Label
        Friend WithEvents cmbIntakeProduct As System.Windows.Forms.ComboBox
        Friend WithEvents lblIntakeQty As System.Windows.Forms.Label
        Friend WithEvents numIntakeQty As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblIntakeOrigCost As System.Windows.Forms.Label
        Friend WithEvents numIntakeOrigCost As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblIntakeSellPrice As System.Windows.Forms.Label
        Friend WithEvents numIntakeSellPrice As System.Windows.Forms.NumericUpDown
        Friend WithEvents btnIntakeToQuarantine As System.Windows.Forms.Button
        Friend WithEvents lblIntakeNotice As System.Windows.Forms.Label
        Friend WithEvents grpInspection As System.Windows.Forms.GroupBox
        Friend WithEvents dgvQuarantineItems As System.Windows.Forms.DataGridView
        Friend WithEvents pnlInspectionActions As System.Windows.Forms.Panel
        Friend WithEvents lblSelectedRMA As System.Windows.Forms.Label
        Friend WithEvents lblRestockQty As System.Windows.Forms.Label
        Friend WithEvents numRestockQty As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblScrapQty As System.Windows.Forms.Label
        Friend WithEvents numScrapQty As System.Windows.Forms.NumericUpDown
        Friend WithEvents btnConfirmInspection As System.Windows.Forms.Button
        Friend WithEvents lblInspectionSummary As System.Windows.Forms.Label
        Friend WithEvents grpRMAJournal As System.Windows.Forms.GroupBox
        Friend WithEvents dgvRMAJournal As System.Windows.Forms.DataGridView

    End Class

End Namespace
