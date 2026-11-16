Option Strict On
Option Explicit On
Option Compare Binary

Namespace UI.Purchasing

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class PurchasingDashboardForm
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
            Me.tabPurchasing = New System.Windows.Forms.TabControl()
            Me.tabThreeWayMatch = New System.Windows.Forms.TabPage()
            Me.splitThreeWay = New System.Windows.Forms.SplitContainer()
            Me.grpMatchGrid = New System.Windows.Forms.GroupBox()
            Me.dgvThreeWayMatch = New System.Windows.Forms.DataGridView()
            Me.pnlBillEntry = New System.Windows.Forms.Panel()
            Me.grpBillEntry = New System.Windows.Forms.GroupBox()
            Me.btnPostThreeWayMatch = New System.Windows.Forms.Button()
            Me.lblVariancePreview = New System.Windows.Forms.Label()
            Me.numFreightAmount = New System.Windows.Forms.NumericUpDown()
            Me.lblFreightAmount = New System.Windows.Forms.Label()
            Me.numBilledUnitPrice = New System.Windows.Forms.NumericUpDown()
            Me.lblBilledUnitPrice = New System.Windows.Forms.Label()
            Me.numBillQty = New System.Windows.Forms.NumericUpDown()
            Me.lblBillQty = New System.Windows.Forms.Label()
            Me.txtBillNumber = New System.Windows.Forms.TextBox()
            Me.lblBillNumber = New System.Windows.Forms.Label()
            Me.lblSelectedMatch = New System.Windows.Forms.Label()
            Me.grpThreeWayJournal = New System.Windows.Forms.GroupBox()
            Me.dgvThreeWayJournal = New System.Windows.Forms.DataGridView()
            Me.tabPurchaseOrders = New System.Windows.Forms.TabPage()
            Me.splitPOs = New System.Windows.Forms.SplitContainer()
            Me.grpPOList = New System.Windows.Forms.GroupBox()
            Me.dgvPurchaseOrders = New System.Windows.Forms.DataGridView()
            Me.grpPOLines = New System.Windows.Forms.GroupBox()
            Me.dgvPurchasingPOLines = New System.Windows.Forms.DataGridView()
            Me.grpCreatePO = New System.Windows.Forms.GroupBox()
            Me.btnCreatePO = New System.Windows.Forms.Button()
            Me.numNewPOUnitPrice = New System.Windows.Forms.NumericUpDown()
            Me.lblNewPOPrice = New System.Windows.Forms.Label()
            Me.numNewPOQty = New System.Windows.Forms.NumericUpDown()
            Me.lblNewPOQty = New System.Windows.Forms.Label()
            Me.cmbNewPOProduct = New System.Windows.Forms.ComboBox()
            Me.lblNewPOProduct = New System.Windows.Forms.Label()
            Me.cmbNewPOVendor = New System.Windows.Forms.ComboBox()
            Me.lblNewPOVendor = New System.Windows.Forms.Label()
            Me.tabGRNList = New System.Windows.Forms.TabPage()
            Me.grpGRNs = New System.Windows.Forms.GroupBox()
            Me.dgvGRNs = New System.Windows.Forms.DataGridView()
            Me.pnlGRNNotice = New System.Windows.Forms.Panel()
            Me.lblGRNNotice = New System.Windows.Forms.Label()
            Me.pnlHeader.SuspendLayout()
            Me.tabPurchasing.SuspendLayout()
            Me.tabThreeWayMatch.SuspendLayout()
            CType(Me.splitThreeWay, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitThreeWay.Panel1.SuspendLayout()
            Me.splitThreeWay.Panel2.SuspendLayout()
            Me.splitThreeWay.SuspendLayout()
            Me.grpMatchGrid.SuspendLayout()
            CType(Me.dgvThreeWayMatch, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlBillEntry.SuspendLayout()
            Me.grpBillEntry.SuspendLayout()
            CType(Me.numFreightAmount, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numBilledUnitPrice, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numBillQty, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpThreeWayJournal.SuspendLayout()
            CType(Me.dgvThreeWayJournal, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabPurchaseOrders.SuspendLayout()
            CType(Me.splitPOs, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitPOs.Panel1.SuspendLayout()
            Me.splitPOs.Panel2.SuspendLayout()
            Me.splitPOs.SuspendLayout()
            Me.grpPOList.SuspendLayout()
            CType(Me.dgvPurchaseOrders, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpPOLines.SuspendLayout()
            CType(Me.dgvPurchasingPOLines, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpCreatePO.SuspendLayout()
            CType(Me.numNewPOUnitPrice, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numNewPOQty, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabGRNList.SuspendLayout()
            Me.grpGRNs.SuspendLayout()
            CType(Me.dgvGRNs, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlGRNNotice.SuspendLayout()
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
            Me.pnlHeader.Size = New System.Drawing.Size(1040, 0)
            Me.pnlHeader.TabIndex = 0
            Me.pnlHeader.Visible = False
            '
            'lblSubtitle
            '
            Me.lblSubtitle.AutoSize = True
            Me.lblSubtitle.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblSubtitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblSubtitle.Location = New System.Drawing.Point(16, 33)
            Me.lblSubtitle.Name = "lblSubtitle"
            Me.lblSubtitle.Size = New System.Drawing.Size(730, 20)
            Me.lblSubtitle.TabIndex = 1
            Me.lblSubtitle.Text = "Purchase Orders, GRN Inbound Tracking & 3-Way Match Verification (GRNI Clearing, PPV & Freight Variances)"
            '
            'lblTitle
            '
            Me.lblTitle.AutoSize = True
            Me.lblTitle.Font = New System.Drawing.Font("Segoe UI Variable Display", 11.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.lblTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblTitle.Location = New System.Drawing.Point(16, 8)
            Me.lblTitle.Name = "lblTitle"
            Me.lblTitle.Size = New System.Drawing.Size(430, 27)
            Me.lblTitle.TabIndex = 0
            Me.lblTitle.Text = "Purchasing Dashboard & 3-Way Match Clearing"
            '
            'tabPurchasing
            '
            Me.tabPurchasing.Controls.Add(Me.tabThreeWayMatch)
            Me.tabPurchasing.Controls.Add(Me.tabPurchaseOrders)
            Me.tabPurchasing.Controls.Add(Me.tabGRNList)
            Me.tabPurchasing.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tabPurchasing.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.tabPurchasing.Location = New System.Drawing.Point(0, 0)
            Me.tabPurchasing.Name = "tabPurchasing"
            Me.tabPurchasing.SelectedIndex = 0
            Me.tabPurchasing.Size = New System.Drawing.Size(1040, 680)
            Me.tabPurchasing.TabIndex = 1
            '
            'tabThreeWayMatch
            '
            Me.tabThreeWayMatch.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.tabThreeWayMatch.Controls.Add(Me.splitThreeWay)
            Me.tabThreeWayMatch.Location = New System.Drawing.Point(4, 30)
            Me.tabThreeWayMatch.Name = "tabThreeWayMatch"
            Me.tabThreeWayMatch.Padding = New System.Windows.Forms.Padding(12)
            Me.tabThreeWayMatch.Size = New System.Drawing.Size(1032, 586)
            Me.tabThreeWayMatch.TabIndex = 0
            Me.tabThreeWayMatch.Text = "3-Way Match & Reconciliation"
            '
            'splitThreeWay
            '
            Me.splitThreeWay.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitThreeWay.Location = New System.Drawing.Point(12, 12)
            Me.splitThreeWay.Name = "splitThreeWay"
            Me.splitThreeWay.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitThreeWay.Panel1
            '
            Me.splitThreeWay.Panel1.Controls.Add(Me.grpMatchGrid)
            Me.splitThreeWay.Panel1.Controls.Add(Me.pnlBillEntry)
            '
            'splitThreeWay.Panel2
            '
            Me.splitThreeWay.Panel2.Controls.Add(Me.grpThreeWayJournal)
            Me.splitThreeWay.Size = New System.Drawing.Size(1008, 620)
            Me.splitThreeWay.SplitterDistance = 330
            Me.splitThreeWay.TabIndex = 0
            '
            'grpMatchGrid
            '
            Me.grpMatchGrid.Controls.Add(Me.dgvThreeWayMatch)
            Me.grpMatchGrid.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpMatchGrid.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.grpMatchGrid.Location = New System.Drawing.Point(0, 0)
            Me.grpMatchGrid.Name = "grpMatchGrid"
            Me.grpMatchGrid.Padding = New System.Windows.Forms.Padding(8, 28, 8, 8)
            Me.grpMatchGrid.Size = New System.Drawing.Size(1008, 170)
            Me.grpMatchGrid.TabIndex = 0
            Me.grpMatchGrid.TabStop = False
            Me.grpMatchGrid.Text = "3-Way Comparison: Purchase Order vs. Goods Receipt (GRN) vs. Supplier Bill"
            '
            'dgvThreeWayMatch
            '
            Me.dgvThreeWayMatch.AllowUserToAddRows = False
            Me.dgvThreeWayMatch.AllowUserToDeleteRows = False
            Me.dgvThreeWayMatch.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvThreeWayMatch.BackgroundColor = System.Drawing.Color.White
            Me.dgvThreeWayMatch.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvThreeWayMatch.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvThreeWayMatch.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.dgvThreeWayMatch.Location = New System.Drawing.Point(8, 28)
            Me.dgvThreeWayMatch.MultiSelect = False
            Me.dgvThreeWayMatch.Name = "dgvThreeWayMatch"
            Me.dgvThreeWayMatch.ReadOnly = True
            Me.dgvThreeWayMatch.RowHeadersVisible = False
            Me.dgvThreeWayMatch.RowHeadersWidth = 51
            Me.dgvThreeWayMatch.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvThreeWayMatch.Size = New System.Drawing.Size(1002, 193)
            Me.dgvThreeWayMatch.TabIndex = 0
            '
            'pnlBillEntry
            '
            Me.pnlBillEntry.Controls.Add(Me.grpBillEntry)
            Me.pnlBillEntry.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlBillEntry.Location = New System.Drawing.Point(0, 170)
            Me.pnlBillEntry.Name = "pnlBillEntry"
            Me.pnlBillEntry.Padding = New System.Windows.Forms.Padding(0, 6, 0, 0)
            Me.pnlBillEntry.Size = New System.Drawing.Size(1008, 160)
            Me.pnlBillEntry.TabIndex = 1
            '
            'grpBillEntry
            '
            Me.grpBillEntry.BackColor = System.Drawing.Color.White
            Me.grpBillEntry.Controls.Add(Me.btnPostThreeWayMatch)
            Me.grpBillEntry.Controls.Add(Me.lblVariancePreview)
            Me.grpBillEntry.Controls.Add(Me.numFreightAmount)
            Me.grpBillEntry.Controls.Add(Me.lblFreightAmount)
            Me.grpBillEntry.Controls.Add(Me.numBilledUnitPrice)
            Me.grpBillEntry.Controls.Add(Me.lblBilledUnitPrice)
            Me.grpBillEntry.Controls.Add(Me.numBillQty)
            Me.grpBillEntry.Controls.Add(Me.lblBillQty)
            Me.grpBillEntry.Controls.Add(Me.txtBillNumber)
            Me.grpBillEntry.Controls.Add(Me.lblBillNumber)
            Me.grpBillEntry.Controls.Add(Me.lblSelectedMatch)
            Me.grpBillEntry.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpBillEntry.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.grpBillEntry.Location = New System.Drawing.Point(0, 6)
            Me.grpBillEntry.Name = "grpBillEntry"
            Me.grpBillEntry.Padding = New System.Windows.Forms.Padding(12, 26, 12, 8)
            Me.grpBillEntry.Size = New System.Drawing.Size(1008, 154)
            Me.grpBillEntry.TabIndex = 0
            Me.grpBillEntry.TabStop = False
            Me.grpBillEntry.Text = "Record Supplier Bill && Reconcile 3-Way Match"
            '
            'btnPostThreeWayMatch
            '
            Me.btnPostThreeWayMatch.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnPostThreeWayMatch.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.btnPostThreeWayMatch.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnPostThreeWayMatch.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.btnPostThreeWayMatch.ForeColor = System.Drawing.Color.White
            Me.btnPostThreeWayMatch.Location = New System.Drawing.Point(680, 50)
            Me.btnPostThreeWayMatch.Name = "btnPostThreeWayMatch"
            Me.btnPostThreeWayMatch.Size = New System.Drawing.Size(310, 48)
            Me.btnPostThreeWayMatch.TabIndex = 10
            Me.btnPostThreeWayMatch.Text = "Commit Supplier Bill && Post Journal"
            Me.btnPostThreeWayMatch.UseVisualStyleBackColor = False
            '
            'lblVariancePreview
            '
            Me.lblVariancePreview.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblVariancePreview.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(248, Byte), Integer))
            Me.lblVariancePreview.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblVariancePreview.Font = New System.Drawing.Font("Segoe UI Variable Text", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblVariancePreview.ForeColor = System.Drawing.Color.FromArgb(CType(CType(10, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(202, Byte), Integer))
            Me.lblVariancePreview.Location = New System.Drawing.Point(14, 106)
            Me.lblVariancePreview.Name = "lblVariancePreview"
            Me.lblVariancePreview.Padding = New System.Windows.Forms.Padding(6, 4, 6, 4)
            Me.lblVariancePreview.Size = New System.Drawing.Size(976, 32)
            Me.lblVariancePreview.TabIndex = 9
            Me.lblVariancePreview.Text = "PPV Variance: $0.00 | Inbound Freight: $0.00 | Net Accounts Payable (2000): $0.00"
            '
            'numFreightAmount
            '
            Me.numFreightAmount.DecimalPlaces = 2
            Me.numFreightAmount.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.numFreightAmount.Location = New System.Drawing.Point(470, 68)
            Me.numFreightAmount.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numFreightAmount.Name = "numFreightAmount"
            Me.numFreightAmount.Size = New System.Drawing.Size(120, 27)
            Me.numFreightAmount.TabIndex = 8
            '
            'lblFreightAmount
            '
            Me.lblFreightAmount.AutoSize = True
            Me.lblFreightAmount.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblFreightAmount.Location = New System.Drawing.Point(470, 48)
            Me.lblFreightAmount.Name = "lblFreightAmount"
            Me.lblFreightAmount.Size = New System.Drawing.Size(149, 20)
            Me.lblFreightAmount.TabIndex = 7
            Me.lblFreightAmount.Text = "Carrier Freight (5200):"
            '
            'numBilledUnitPrice
            '
            Me.numBilledUnitPrice.DecimalPlaces = 2
            Me.numBilledUnitPrice.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.numBilledUnitPrice.Location = New System.Drawing.Point(325, 68)
            Me.numBilledUnitPrice.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numBilledUnitPrice.Name = "numBilledUnitPrice"
            Me.numBilledUnitPrice.Size = New System.Drawing.Size(125, 27)
            Me.numBilledUnitPrice.TabIndex = 6
            '
            'lblBilledUnitPrice
            '
            Me.lblBilledUnitPrice.AutoSize = True
            Me.lblBilledUnitPrice.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblBilledUnitPrice.Location = New System.Drawing.Point(325, 48)
            Me.lblBilledUnitPrice.Name = "lblBilledUnitPrice"
            Me.lblBilledUnitPrice.Size = New System.Drawing.Size(117, 20)
            Me.lblBilledUnitPrice.TabIndex = 5
            Me.lblBilledUnitPrice.Text = "Billed Unit Price:"
            '
            'numBillQty
            '
            Me.numBillQty.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.numBillQty.Location = New System.Drawing.Point(190, 68)
            Me.numBillQty.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numBillQty.Name = "numBillQty"
            Me.numBillQty.Size = New System.Drawing.Size(115, 27)
            Me.numBillQty.TabIndex = 4
            '
            'lblBillQty
            '
            Me.lblBillQty.AutoSize = True
            Me.lblBillQty.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblBillQty.Location = New System.Drawing.Point(190, 48)
            Me.lblBillQty.Name = "lblBillQty"
            Me.lblBillQty.Size = New System.Drawing.Size(110, 20)
            Me.lblBillQty.TabIndex = 3
            Me.lblBillQty.Text = "Billed Quantity:"
            '
            'txtBillNumber
            '
            Me.txtBillNumber.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.txtBillNumber.Location = New System.Drawing.Point(14, 68)
            Me.txtBillNumber.Name = "txtBillNumber"
            Me.txtBillNumber.Size = New System.Drawing.Size(155, 27)
            Me.txtBillNumber.TabIndex = 2
            Me.txtBillNumber.Text = "BILL-2026-1099"
            '
            'lblBillNumber
            '
            Me.lblBillNumber.AutoSize = True
            Me.lblBillNumber.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblBillNumber.Location = New System.Drawing.Point(14, 48)
            Me.lblBillNumber.Name = "lblBillNumber"
            Me.lblBillNumber.Size = New System.Drawing.Size(146, 20)
            Me.lblBillNumber.TabIndex = 1
            Me.lblBillNumber.Text = "Supplier Bill/Inv Doc#:"
            '
            'lblSelectedMatch
            '
            Me.lblSelectedMatch.AutoSize = True
            Me.lblSelectedMatch.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.lblSelectedMatch.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblSelectedMatch.Location = New System.Drawing.Point(14, 24)
            Me.lblSelectedMatch.Name = "lblSelectedMatch"
            Me.lblSelectedMatch.Size = New System.Drawing.Size(550, 20)
            Me.lblSelectedMatch.TabIndex = 0
            Me.lblSelectedMatch.Text = "Selected Match: PO / GRN"
            '
            'grpThreeWayJournal
            '
            Me.grpThreeWayJournal.Controls.Add(Me.dgvThreeWayJournal)
            Me.grpThreeWayJournal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpThreeWayJournal.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.grpThreeWayJournal.Location = New System.Drawing.Point(0, 0)
            Me.grpThreeWayJournal.Name = "grpThreeWayJournal"
            Me.grpThreeWayJournal.Padding = New System.Windows.Forms.Padding(8, 28, 8, 8)
            Me.grpThreeWayJournal.Size = New System.Drawing.Size(1008, 188)
            Me.grpThreeWayJournal.TabIndex = 0
            Me.grpThreeWayJournal.TabStop = False
            Me.grpThreeWayJournal.Text = "3-Way Match General Ledger Journal History (Debits = Credits Clearing)"
            '
            'dgvThreeWayJournal
            '
            Me.dgvThreeWayJournal.AllowUserToAddRows = False
            Me.dgvThreeWayJournal.AllowUserToDeleteRows = False
            Me.dgvThreeWayJournal.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvThreeWayJournal.BackgroundColor = System.Drawing.Color.White
            Me.dgvThreeWayJournal.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvThreeWayJournal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvThreeWayJournal.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.dgvThreeWayJournal.Location = New System.Drawing.Point(8, 28)
            Me.dgvThreeWayJournal.MultiSelect = False
            Me.dgvThreeWayJournal.Name = "dgvThreeWayJournal"
            Me.dgvThreeWayJournal.ReadOnly = True
            Me.dgvThreeWayJournal.RowHeadersVisible = False
            Me.dgvThreeWayJournal.RowHeadersWidth = 51
            Me.dgvThreeWayJournal.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvThreeWayJournal.Size = New System.Drawing.Size(1002, 161)
            Me.dgvThreeWayJournal.TabIndex = 0
            '
            'tabPurchaseOrders
            '
            Me.tabPurchaseOrders.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.tabPurchaseOrders.Controls.Add(Me.splitPOs)
            Me.tabPurchaseOrders.Controls.Add(Me.grpCreatePO)
            Me.tabPurchaseOrders.Location = New System.Drawing.Point(4, 30)
            Me.tabPurchaseOrders.Name = "tabPurchaseOrders"
            Me.tabPurchaseOrders.Padding = New System.Windows.Forms.Padding(12)
            Me.tabPurchaseOrders.Size = New System.Drawing.Size(1032, 586)
            Me.tabPurchaseOrders.TabIndex = 1
            Me.tabPurchaseOrders.Text = "Purchase Orders"
            '
            'splitPOs
            '
            Me.splitPOs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitPOs.Location = New System.Drawing.Point(12, 112)
            Me.splitPOs.Name = "splitPOs"
            Me.splitPOs.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitPOs.Panel1
            '
            Me.splitPOs.Panel1.Controls.Add(Me.grpPOList)
            '
            'splitPOs.Panel2
            '
            Me.splitPOs.Panel2.Controls.Add(Me.grpPOLines)
            Me.splitPOs.Size = New System.Drawing.Size(1008, 462)
            Me.splitPOs.SplitterDistance = 240
            Me.splitPOs.TabIndex = 1
            '
            'grpPOList
            '
            Me.grpPOList.Controls.Add(Me.dgvPurchaseOrders)
            Me.grpPOList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpPOList.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.grpPOList.Location = New System.Drawing.Point(0, 0)
            Me.grpPOList.Name = "grpPOList"
            Me.grpPOList.Size = New System.Drawing.Size(1008, 240)
            Me.grpPOList.TabIndex = 0
            Me.grpPOList.TabStop = False
            Me.grpPOList.Text = "Purchase Orders Catalog"
            '
            'dgvPurchaseOrders
            '
            Me.dgvPurchaseOrders.AllowUserToAddRows = False
            Me.dgvPurchaseOrders.AllowUserToDeleteRows = False
            Me.dgvPurchaseOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvPurchaseOrders.BackgroundColor = System.Drawing.Color.White
            Me.dgvPurchaseOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvPurchaseOrders.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvPurchaseOrders.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.dgvPurchaseOrders.Location = New System.Drawing.Point(3, 24)
            Me.dgvPurchaseOrders.MultiSelect = False
            Me.dgvPurchaseOrders.Name = "dgvPurchaseOrders"
            Me.dgvPurchaseOrders.ReadOnly = True
            Me.dgvPurchaseOrders.RowHeadersVisible = False
            Me.dgvPurchaseOrders.RowHeadersWidth = 51
            Me.dgvPurchaseOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvPurchaseOrders.Size = New System.Drawing.Size(1002, 213)
            Me.dgvPurchaseOrders.TabIndex = 0
            '
            'grpPOLines
            '
            Me.grpPOLines.Controls.Add(Me.dgvPurchasingPOLines)
            Me.grpPOLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpPOLines.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.grpPOLines.Location = New System.Drawing.Point(0, 0)
            Me.grpPOLines.Name = "grpPOLines"
            Me.grpPOLines.Size = New System.Drawing.Size(1008, 218)
            Me.grpPOLines.TabIndex = 0
            Me.grpPOLines.TabStop = False
            Me.grpPOLines.Text = "PO Line Details"
            '
            'dgvPurchasingPOLines
            '
            Me.dgvPurchasingPOLines.AllowUserToAddRows = False
            Me.dgvPurchasingPOLines.AllowUserToDeleteRows = False
            Me.dgvPurchasingPOLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvPurchasingPOLines.BackgroundColor = System.Drawing.Color.White
            Me.dgvPurchasingPOLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvPurchasingPOLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvPurchasingPOLines.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.dgvPurchasingPOLines.Location = New System.Drawing.Point(3, 24)
            Me.dgvPurchasingPOLines.MultiSelect = False
            Me.dgvPurchasingPOLines.Name = "dgvPurchasingPOLines"
            Me.dgvPurchasingPOLines.ReadOnly = True
            Me.dgvPurchasingPOLines.RowHeadersVisible = False
            Me.dgvPurchasingPOLines.RowHeadersWidth = 51
            Me.dgvPurchasingPOLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvPurchasingPOLines.Size = New System.Drawing.Size(1002, 191)
            Me.dgvPurchasingPOLines.TabIndex = 0
            '
            'grpCreatePO
            '
            Me.grpCreatePO.BackColor = System.Drawing.Color.White
            Me.grpCreatePO.Controls.Add(Me.btnCreatePO)
            Me.grpCreatePO.Controls.Add(Me.numNewPOUnitPrice)
            Me.grpCreatePO.Controls.Add(Me.lblNewPOPrice)
            Me.grpCreatePO.Controls.Add(Me.numNewPOQty)
            Me.grpCreatePO.Controls.Add(Me.lblNewPOQty)
            Me.grpCreatePO.Controls.Add(Me.cmbNewPOProduct)
            Me.grpCreatePO.Controls.Add(Me.lblNewPOProduct)
            Me.grpCreatePO.Controls.Add(Me.cmbNewPOVendor)
            Me.grpCreatePO.Controls.Add(Me.lblNewPOVendor)
            Me.grpCreatePO.Dock = System.Windows.Forms.DockStyle.Top
            Me.grpCreatePO.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.grpCreatePO.Location = New System.Drawing.Point(12, 12)
            Me.grpCreatePO.Name = "grpCreatePO"
            Me.grpCreatePO.Size = New System.Drawing.Size(1008, 100)
            Me.grpCreatePO.TabIndex = 0
            Me.grpCreatePO.TabStop = False
            Me.grpCreatePO.Text = "Create New Purchase Order"
            '
            'btnCreatePO
            '
            Me.btnCreatePO.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnCreatePO.BackColor = System.Drawing.Color.FromArgb(CType(CType(25, Byte), Integer), CType(CType(135, Byte), Integer), CType(CType(84, Byte), Integer))
            Me.btnCreatePO.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnCreatePO.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.btnCreatePO.ForeColor = System.Drawing.Color.White
            Me.btnCreatePO.Location = New System.Drawing.Point(825, 38)
            Me.btnCreatePO.Name = "btnCreatePO"
            Me.btnCreatePO.Size = New System.Drawing.Size(165, 38)
            Me.btnCreatePO.TabIndex = 8
            Me.btnCreatePO.Text = "Issue Purchase Order"
            Me.btnCreatePO.UseVisualStyleBackColor = False
            '
            'numNewPOUnitPrice
            '
            Me.numNewPOUnitPrice.DecimalPlaces = 2
            Me.numNewPOUnitPrice.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.numNewPOUnitPrice.Location = New System.Drawing.Point(685, 45)
            Me.numNewPOUnitPrice.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numNewPOUnitPrice.Name = "numNewPOUnitPrice"
            Me.numNewPOUnitPrice.Size = New System.Drawing.Size(120, 27)
            Me.numNewPOUnitPrice.TabIndex = 7
            '
            'lblNewPOPrice
            '
            Me.lblNewPOPrice.AutoSize = True
            Me.lblNewPOPrice.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblNewPOPrice.Location = New System.Drawing.Point(685, 23)
            Me.lblNewPOPrice.Name = "lblNewPOPrice"
            Me.lblNewPOPrice.Size = New System.Drawing.Size(107, 20)
            Me.lblNewPOPrice.TabIndex = 6
            Me.lblNewPOPrice.Text = "Unit Price ($/ea):"
            '
            'numNewPOQty
            '
            Me.numNewPOQty.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.numNewPOQty.Location = New System.Drawing.Point(565, 45)
            Me.numNewPOQty.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numNewPOQty.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numNewPOQty.Name = "numNewPOQty"
            Me.numNewPOQty.Size = New System.Drawing.Size(105, 27)
            Me.numNewPOQty.TabIndex = 5
            Me.numNewPOQty.Value = New Decimal(New Integer() {50, 0, 0, 0})
            '
            'lblNewPOQty
            '
            Me.lblNewPOQty.AutoSize = True
            Me.lblNewPOQty.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblNewPOQty.Location = New System.Drawing.Point(565, 23)
            Me.lblNewPOQty.Name = "lblNewPOQty"
            Me.lblNewPOQty.Size = New System.Drawing.Size(89, 20)
            Me.lblNewPOQty.TabIndex = 4
            Me.lblNewPOQty.Text = "Ordered Qty:"
            '
            'cmbNewPOProduct
            '
            Me.cmbNewPOProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbNewPOProduct.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.cmbNewPOProduct.FormattingEnabled = True
            Me.cmbNewPOProduct.Location = New System.Drawing.Point(280, 45)
            Me.cmbNewPOProduct.Name = "cmbNewPOProduct"
            Me.cmbNewPOProduct.Size = New System.Drawing.Size(265, 28)
            Me.cmbNewPOProduct.TabIndex = 3
            '
            'lblNewPOProduct
            '
            Me.lblNewPOProduct.AutoSize = True
            Me.lblNewPOProduct.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblNewPOProduct.Location = New System.Drawing.Point(280, 23)
            Me.lblNewPOProduct.Name = "lblNewPOProduct"
            Me.lblNewPOProduct.Size = New System.Drawing.Size(105, 20)
            Me.lblNewPOProduct.TabIndex = 2
            Me.lblNewPOProduct.Text = "Target Product:"
            '
            'cmbNewPOVendor
            '
            Me.cmbNewPOVendor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbNewPOVendor.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.cmbNewPOVendor.FormattingEnabled = True
            Me.cmbNewPOVendor.Location = New System.Drawing.Point(12, 45)
            Me.cmbNewPOVendor.Name = "cmbNewPOVendor"
            Me.cmbNewPOVendor.Size = New System.Drawing.Size(250, 28)
            Me.cmbNewPOVendor.TabIndex = 1
            '
            'lblNewPOVendor
            '
            Me.lblNewPOVendor.AutoSize = True
            Me.lblNewPOVendor.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblNewPOVendor.Location = New System.Drawing.Point(12, 23)
            Me.lblNewPOVendor.Name = "lblNewPOVendor"
            Me.lblNewPOVendor.Size = New System.Drawing.Size(117, 20)
            Me.lblNewPOVendor.TabIndex = 0
            Me.lblNewPOVendor.Text = "Selected Vendor:"
            '
            'tabGRNList
            '
            Me.tabGRNList.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.tabGRNList.Controls.Add(Me.grpGRNs)
            Me.tabGRNList.Controls.Add(Me.pnlGRNNotice)
            Me.tabGRNList.Location = New System.Drawing.Point(4, 30)
            Me.tabGRNList.Name = "tabGRNList"
            Me.tabGRNList.Padding = New System.Windows.Forms.Padding(12)
            Me.tabGRNList.Size = New System.Drawing.Size(1032, 586)
            Me.tabGRNList.TabIndex = 2
            Me.tabGRNList.Text = "Goods Receipt Notes (GRN)"
            '
            'grpGRNs
            '
            Me.grpGRNs.Controls.Add(Me.dgvGRNs)
            Me.grpGRNs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpGRNs.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point)
            Me.grpGRNs.Location = New System.Drawing.Point(12, 60)
            Me.grpGRNs.Name = "grpGRNs"
            Me.grpGRNs.Size = New System.Drawing.Size(1008, 514)
            Me.grpGRNs.TabIndex = 0
            Me.grpGRNs.TabStop = False
            Me.grpGRNs.Text = "Dock Received Goods Vouchers (GRNI Clearing Backlog)"
            '
            'dgvGRNs
            '
            Me.dgvGRNs.AllowUserToAddRows = False
            Me.dgvGRNs.AllowUserToDeleteRows = False
            Me.dgvGRNs.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvGRNs.BackgroundColor = System.Drawing.Color.White
            Me.dgvGRNs.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvGRNs.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvGRNs.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.dgvGRNs.Location = New System.Drawing.Point(3, 24)
            Me.dgvGRNs.MultiSelect = False
            Me.dgvGRNs.Name = "dgvGRNs"
            Me.dgvGRNs.ReadOnly = True
            Me.dgvGRNs.RowHeadersVisible = False
            Me.dgvGRNs.RowHeadersWidth = 51
            Me.dgvGRNs.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvGRNs.Size = New System.Drawing.Size(1002, 487)
            Me.dgvGRNs.TabIndex = 0
            '
            'pnlGRNNotice
            '
            Me.pnlGRNNotice.BackColor = System.Drawing.Color.FromArgb(CType(CType(236, Byte), Integer), CType(CType(242, Byte), Integer), CType(CType(248, Byte), Integer))
            Me.pnlGRNNotice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlGRNNotice.Controls.Add(Me.lblGRNNotice)
            Me.pnlGRNNotice.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlGRNNotice.Location = New System.Drawing.Point(12, 12)
            Me.pnlGRNNotice.Name = "pnlGRNNotice"
            Me.pnlGRNNotice.Padding = New System.Windows.Forms.Padding(10)
            Me.pnlGRNNotice.Size = New System.Drawing.Size(1008, 48)
            Me.pnlGRNNotice.TabIndex = 1
            '
            'lblGRNNotice
            '
            Me.lblGRNNotice.AutoSize = True
            Me.lblGRNNotice.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.lblGRNNotice.ForeColor = System.Drawing.Color.FromArgb(CType(CType(10, Byte), Integer), CType(CType(88, Byte), Integer), CType(CType(202, Byte), Integer))
            Me.lblGRNNotice.Location = New System.Drawing.Point(8, 12)
            Me.lblGRNNotice.Name = "lblGRNNotice"
            Me.lblGRNNotice.Size = New System.Drawing.Size(950, 20)
            Me.lblGRNNotice.TabIndex = 0
            Me.lblGRNNotice.Text = "Audit Invariant: Goods receipts credit Account 2100 GRNI at PO price. Unbilled GRNs remain open until supplier bills are matched and cleared."
            '
            'PurchasingDashboardForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(9.0!, 21.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Inherit
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(1040, 680)
            Me.Controls.Add(Me.tabPurchasing)
            Me.Controls.Add(Me.pnlHeader)
            Me.Font = New System.Drawing.Font("Segoe UI Variable Text", 9.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point)
            Me.Name = "PurchasingDashboardForm"
            Me.Text = "Purchasing Dashboard & 3-Way Match Verification"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.tabPurchasing.ResumeLayout(False)
            Me.tabThreeWayMatch.ResumeLayout(False)
            Me.splitThreeWay.Panel1.ResumeLayout(False)
            Me.splitThreeWay.Panel2.ResumeLayout(False)
            CType(Me.splitThreeWay, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitThreeWay.ResumeLayout(False)
            Me.grpMatchGrid.ResumeLayout(False)
            CType(Me.dgvThreeWayMatch, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlBillEntry.ResumeLayout(False)
            Me.grpBillEntry.ResumeLayout(False)
            Me.grpBillEntry.PerformLayout()
            CType(Me.numFreightAmount, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numBilledUnitPrice, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numBillQty, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpThreeWayJournal.ResumeLayout(False)
            CType(Me.dgvThreeWayJournal, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabPurchaseOrders.ResumeLayout(False)
            Me.splitPOs.Panel1.ResumeLayout(False)
            Me.splitPOs.Panel2.ResumeLayout(False)
            CType(Me.splitPOs, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitPOs.ResumeLayout(False)
            Me.grpPOList.ResumeLayout(False)
            CType(Me.dgvPurchaseOrders, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpPOLines.ResumeLayout(False)
            CType(Me.dgvPurchasingPOLines, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpCreatePO.ResumeLayout(False)
            Me.grpCreatePO.PerformLayout()
            CType(Me.numNewPOUnitPrice, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numNewPOQty, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabGRNList.ResumeLayout(False)
            Me.grpGRNs.ResumeLayout(False)
            CType(Me.dgvGRNs, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlGRNNotice.ResumeLayout(False)
            Me.pnlGRNNotice.PerformLayout()
            Me.ResumeLayout(False)

        End Sub

        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblTitle As System.Windows.Forms.Label
        Friend WithEvents lblSubtitle As System.Windows.Forms.Label
        Friend WithEvents tabPurchasing As System.Windows.Forms.TabControl
        Friend WithEvents tabThreeWayMatch As System.Windows.Forms.TabPage
        Friend WithEvents splitThreeWay As System.Windows.Forms.SplitContainer
        Friend WithEvents grpMatchGrid As System.Windows.Forms.GroupBox
        Friend WithEvents dgvThreeWayMatch As System.Windows.Forms.DataGridView
        Friend WithEvents pnlBillEntry As System.Windows.Forms.Panel
        Friend WithEvents grpBillEntry As System.Windows.Forms.GroupBox
        Friend WithEvents lblSelectedMatch As System.Windows.Forms.Label
        Friend WithEvents lblBillNumber As System.Windows.Forms.Label
        Friend WithEvents txtBillNumber As System.Windows.Forms.TextBox
        Friend WithEvents lblBillQty As System.Windows.Forms.Label
        Friend WithEvents numBillQty As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblBilledUnitPrice As System.Windows.Forms.Label
        Friend WithEvents numBilledUnitPrice As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblFreightAmount As System.Windows.Forms.Label
        Friend WithEvents numFreightAmount As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblVariancePreview As System.Windows.Forms.Label
        Friend WithEvents btnPostThreeWayMatch As System.Windows.Forms.Button
        Friend WithEvents grpThreeWayJournal As System.Windows.Forms.GroupBox
        Friend WithEvents dgvThreeWayJournal As System.Windows.Forms.DataGridView
        Friend WithEvents tabPurchaseOrders As System.Windows.Forms.TabPage
        Friend WithEvents splitPOs As System.Windows.Forms.SplitContainer
        Friend WithEvents grpPOList As System.Windows.Forms.GroupBox
        Friend WithEvents dgvPurchaseOrders As System.Windows.Forms.DataGridView
        Friend WithEvents grpPOLines As System.Windows.Forms.GroupBox
        Friend WithEvents dgvPurchasingPOLines As System.Windows.Forms.DataGridView
        Friend WithEvents grpCreatePO As System.Windows.Forms.GroupBox
        Friend WithEvents lblNewPOVendor As System.Windows.Forms.Label
        Friend WithEvents cmbNewPOVendor As System.Windows.Forms.ComboBox
        Friend WithEvents lblNewPOProduct As System.Windows.Forms.Label
        Friend WithEvents cmbNewPOProduct As System.Windows.Forms.ComboBox
        Friend WithEvents lblNewPOQty As System.Windows.Forms.Label
        Friend WithEvents numNewPOQty As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblNewPOPrice As System.Windows.Forms.Label
        Friend WithEvents numNewPOUnitPrice As System.Windows.Forms.NumericUpDown
        Friend WithEvents btnCreatePO As System.Windows.Forms.Button
        Friend WithEvents tabGRNList As System.Windows.Forms.TabPage
        Friend WithEvents grpGRNs As System.Windows.Forms.GroupBox
        Friend WithEvents dgvGRNs As System.Windows.Forms.DataGridView
        Friend WithEvents pnlGRNNotice As System.Windows.Forms.Panel
        Friend WithEvents lblGRNNotice As System.Windows.Forms.Label

    End Class

End Namespace
