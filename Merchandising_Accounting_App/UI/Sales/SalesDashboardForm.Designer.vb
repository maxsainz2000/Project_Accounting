Namespace UI.Sales

    <Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
    Partial Class SalesDashboardForm
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

        Friend WithEvents pnlHeader As System.Windows.Forms.Panel
        Friend WithEvents lblHeaderTitle As System.Windows.Forms.Label
        Friend WithEvents lblHeaderSubtitle As System.Windows.Forms.Label
        Friend WithEvents pnlKpiContainer As System.Windows.Forms.Panel
        Friend WithEvents pnlKpiOrders As System.Windows.Forms.Panel
        Friend WithEvents lblKpiOrdersValue As System.Windows.Forms.Label
        Friend WithEvents lblKpiOrdersTitle As System.Windows.Forms.Label
        Friend WithEvents pnlKpiInvoices As System.Windows.Forms.Panel
        Friend WithEvents lblKpiInvoicesValue As System.Windows.Forms.Label
        Friend WithEvents lblKpiInvoicesTitle As System.Windows.Forms.Label
        Friend WithEvents pnlKpiStockGuard As System.Windows.Forms.Panel
        Friend WithEvents lblKpiStockGuardValue As System.Windows.Forms.Label
        Friend WithEvents lblKpiStockGuardTitle As System.Windows.Forms.Label
        Friend WithEvents pnlKpiLowStock As System.Windows.Forms.Panel
        Friend WithEvents lblKpiLowStockValue As System.Windows.Forms.Label
        Friend WithEvents lblKpiLowStockTitle As System.Windows.Forms.Label

        Friend WithEvents tabMain As System.Windows.Forms.TabControl
        Friend WithEvents tabOrders As System.Windows.Forms.TabPage
        Friend WithEvents tabInvoices As System.Windows.Forms.TabPage
        Friend WithEvents tabStockMonitor As System.Windows.Forms.TabPage

        ' Tab 1: Sales Orders Controls
        Friend WithEvents splitOrders As System.Windows.Forms.SplitContainer
        Friend WithEvents grpCustomer As System.Windows.Forms.GroupBox
        Friend WithEvents tblCustomerCredit As System.Windows.Forms.TableLayoutPanel
        Friend WithEvents lblCustomerPrompt As System.Windows.Forms.Label
        Friend WithEvents cmbCustomer As System.Windows.Forms.ComboBox
        Friend WithEvents lblCreditLimitTitle As System.Windows.Forms.Label
        Friend WithEvents lblCreditLimitVal As System.Windows.Forms.Label
        Friend WithEvents lblCurrentARTitle As System.Windows.Forms.Label
        Friend WithEvents lblCurrentARVal As System.Windows.Forms.Label
        Friend WithEvents lblAvailCreditTitle As System.Windows.Forms.Label
        Friend WithEvents lblAvailCreditVal As System.Windows.Forms.Label
        Friend WithEvents lblCustomerBadge As System.Windows.Forms.Label

        Friend WithEvents grpAddItem As System.Windows.Forms.GroupBox
        Friend WithEvents lblProductPrompt As System.Windows.Forms.Label
        Friend WithEvents cmbProduct As System.Windows.Forms.ComboBox
        Friend WithEvents lblQtyPrompt As System.Windows.Forms.Label
        Friend WithEvents numQuantity As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblPricePrompt As System.Windows.Forms.Label
        Friend WithEvents numUnitPrice As System.Windows.Forms.NumericUpDown
        Friend WithEvents lblStockBadgeItem As System.Windows.Forms.Label
        Friend WithEvents lblItemStockInfo As System.Windows.Forms.Label
        Friend WithEvents btnAddLineItem As System.Windows.Forms.Button
        Friend WithEvents lblStockWarningNotice As System.Windows.Forms.Label

        Friend WithEvents grpOrderLines As System.Windows.Forms.GroupBox
        Friend WithEvents dgvCurrentLines As System.Windows.Forms.DataGridView
        Friend WithEvents pnlOrderLineActions As System.Windows.Forms.Panel
        Friend WithEvents btnRemoveLine As System.Windows.Forms.Button
        Friend WithEvents btnClearOrderForm As System.Windows.Forms.Button
        Friend WithEvents btnSaveOrderDraft As System.Windows.Forms.Button
        Friend WithEvents btnConfirmOrder As System.Windows.Forms.Button
        Friend WithEvents lblOrderTotalDisplay As System.Windows.Forms.Label

        Friend WithEvents grpExistingOrders As System.Windows.Forms.GroupBox
        Friend WithEvents dgvExistingOrders As System.Windows.Forms.DataGridView
        Friend WithEvents pnlOrderActions As System.Windows.Forms.Panel
        Friend WithEvents btnGenerateInvoiceFromSO As System.Windows.Forms.Button
        Friend WithEvents lblSelectedOrderSummary As System.Windows.Forms.Label
        Friend WithEvents grpSelectedOrderLines As System.Windows.Forms.GroupBox
        Friend WithEvents dgvSelectedOrderLines As System.Windows.Forms.DataGridView

        ' Tab 2: Sales Invoices Controls
        Friend WithEvents splitInvoices As System.Windows.Forms.SplitContainer
        Friend WithEvents grpInvoicesList As System.Windows.Forms.GroupBox
        Friend WithEvents dgvInvoices As System.Windows.Forms.DataGridView
        Friend WithEvents splitInvoiceDetails As System.Windows.Forms.SplitContainer
        Friend WithEvents grpInvoiceLines As System.Windows.Forms.GroupBox
        Friend WithEvents dgvInvoiceLines As System.Windows.Forms.DataGridView
        Friend WithEvents grpJournalPreview As System.Windows.Forms.GroupBox
        Friend WithEvents dgvJournalPreview As System.Windows.Forms.DataGridView
        Friend WithEvents lblGlNotice As System.Windows.Forms.Label

        ' Tab 3: Stock Monitor Controls
        Friend WithEvents splitStock As System.Windows.Forms.SplitContainer
        Friend WithEvents grpAllProducts As System.Windows.Forms.GroupBox
        Friend WithEvents dgvProductsCatalog As System.Windows.Forms.DataGridView
        Friend WithEvents grpStockSimulation As System.Windows.Forms.GroupBox
        Friend WithEvents lblSimPrompt As System.Windows.Forms.Label
        Friend WithEvents cmbSimProduct As System.Windows.Forms.ComboBox
        Friend WithEvents lblSimQtyPrompt As System.Windows.Forms.Label
        Friend WithEvents numSimQty As System.Windows.Forms.NumericUpDown
        Friend WithEvents btnRunSim As System.Windows.Forms.Button
        Friend WithEvents pnlSimBadgeContainer As System.Windows.Forms.Panel
        Friend WithEvents lblSimBadge As System.Windows.Forms.Label
        Friend WithEvents lblSimExplanation As System.Windows.Forms.Label
        Friend WithEvents lblStockGuardPolicy As System.Windows.Forms.Label

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.pnlHeader = New System.Windows.Forms.Panel()
            Me.pnlKpiContainer = New System.Windows.Forms.Panel()
            Me.pnlKpiStockGuard = New System.Windows.Forms.Panel()
            Me.lblKpiStockGuardValue = New System.Windows.Forms.Label()
            Me.lblKpiStockGuardTitle = New System.Windows.Forms.Label()
            Me.pnlKpiInvoices = New System.Windows.Forms.Panel()
            Me.lblKpiInvoicesValue = New System.Windows.Forms.Label()
            Me.lblKpiInvoicesTitle = New System.Windows.Forms.Label()
            Me.pnlKpiOrders = New System.Windows.Forms.Panel()
            Me.lblKpiOrdersValue = New System.Windows.Forms.Label()
            Me.lblKpiOrdersTitle = New System.Windows.Forms.Label()
            Me.pnlKpiLowStock = New System.Windows.Forms.Panel()
            Me.lblKpiLowStockValue = New System.Windows.Forms.Label()
            Me.lblKpiLowStockTitle = New System.Windows.Forms.Label()
            Me.lblHeaderSubtitle = New System.Windows.Forms.Label()
            Me.lblHeaderTitle = New System.Windows.Forms.Label()
            Me.tabMain = New System.Windows.Forms.TabControl()
            Me.tabOrders = New System.Windows.Forms.TabPage()
            Me.splitOrders = New System.Windows.Forms.SplitContainer()
            Me.grpOrderLines = New System.Windows.Forms.GroupBox()
            Me.dgvCurrentLines = New System.Windows.Forms.DataGridView()
            Me.pnlOrderLineActions = New System.Windows.Forms.Panel()
            Me.btnConfirmOrder = New System.Windows.Forms.Button()
            Me.btnSaveOrderDraft = New System.Windows.Forms.Button()
            Me.btnClearOrderForm = New System.Windows.Forms.Button()
            Me.btnRemoveLine = New System.Windows.Forms.Button()
            Me.lblOrderTotalDisplay = New System.Windows.Forms.Label()
            Me.grpAddItem = New System.Windows.Forms.GroupBox()
            Me.lblStockWarningNotice = New System.Windows.Forms.Label()
            Me.btnAddLineItem = New System.Windows.Forms.Button()
            Me.lblStockBadgeItem = New System.Windows.Forms.Label()
            Me.lblItemStockInfo = New System.Windows.Forms.Label()
            Me.numUnitPrice = New System.Windows.Forms.NumericUpDown()
            Me.lblPricePrompt = New System.Windows.Forms.Label()
            Me.numQuantity = New System.Windows.Forms.NumericUpDown()
            Me.lblQtyPrompt = New System.Windows.Forms.Label()
            Me.cmbProduct = New System.Windows.Forms.ComboBox()
            Me.lblProductPrompt = New System.Windows.Forms.Label()
            Me.grpCustomer = New System.Windows.Forms.GroupBox()
            Me.tblCustomerCredit = New System.Windows.Forms.TableLayoutPanel()
            Me.lblCustomerBadge = New System.Windows.Forms.Label()
            Me.lblAvailCreditVal = New System.Windows.Forms.Label()
            Me.lblAvailCreditTitle = New System.Windows.Forms.Label()
            Me.lblCurrentARVal = New System.Windows.Forms.Label()
            Me.lblCurrentARTitle = New System.Windows.Forms.Label()
            Me.lblCreditLimitVal = New System.Windows.Forms.Label()
            Me.lblCreditLimitTitle = New System.Windows.Forms.Label()
            Me.cmbCustomer = New System.Windows.Forms.ComboBox()
            Me.lblCustomerPrompt = New System.Windows.Forms.Label()
            Me.grpSelectedOrderLines = New System.Windows.Forms.GroupBox()
            Me.dgvSelectedOrderLines = New System.Windows.Forms.DataGridView()
            Me.pnlOrderActions = New System.Windows.Forms.Panel()
            Me.lblSelectedOrderSummary = New System.Windows.Forms.Label()
            Me.btnGenerateInvoiceFromSO = New System.Windows.Forms.Button()
            Me.grpExistingOrders = New System.Windows.Forms.GroupBox()
            Me.dgvExistingOrders = New System.Windows.Forms.DataGridView()
            Me.tabInvoices = New System.Windows.Forms.TabPage()
            Me.splitInvoices = New System.Windows.Forms.SplitContainer()
            Me.grpInvoicesList = New System.Windows.Forms.GroupBox()
            Me.dgvInvoices = New System.Windows.Forms.DataGridView()
            Me.splitInvoiceDetails = New System.Windows.Forms.SplitContainer()
            Me.grpInvoiceLines = New System.Windows.Forms.GroupBox()
            Me.dgvInvoiceLines = New System.Windows.Forms.DataGridView()
            Me.grpJournalPreview = New System.Windows.Forms.GroupBox()
            Me.dgvJournalPreview = New System.Windows.Forms.DataGridView()
            Me.lblGlNotice = New System.Windows.Forms.Label()
            Me.tabStockMonitor = New System.Windows.Forms.TabPage()
            Me.splitStock = New System.Windows.Forms.SplitContainer()
            Me.grpAllProducts = New System.Windows.Forms.GroupBox()
            Me.dgvProductsCatalog = New System.Windows.Forms.DataGridView()
            Me.grpStockSimulation = New System.Windows.Forms.GroupBox()
            Me.lblStockGuardPolicy = New System.Windows.Forms.Label()
            Me.lblSimExplanation = New System.Windows.Forms.Label()
            Me.pnlSimBadgeContainer = New System.Windows.Forms.Panel()
            Me.lblSimBadge = New System.Windows.Forms.Label()
            Me.btnRunSim = New System.Windows.Forms.Button()
            Me.numSimQty = New System.Windows.Forms.NumericUpDown()
            Me.lblSimQtyPrompt = New System.Windows.Forms.Label()
            Me.cmbSimProduct = New System.Windows.Forms.ComboBox()
            Me.lblSimPrompt = New System.Windows.Forms.Label()
            Me.pnlHeader.SuspendLayout()
            Me.pnlKpiContainer.SuspendLayout()
            Me.pnlKpiStockGuard.SuspendLayout()
            Me.pnlKpiInvoices.SuspendLayout()
            Me.pnlKpiOrders.SuspendLayout()
            Me.pnlKpiLowStock.SuspendLayout()
            Me.tabMain.SuspendLayout()
            Me.tabOrders.SuspendLayout()
            CType(Me.splitOrders, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitOrders.Panel1.SuspendLayout()
            Me.splitOrders.Panel2.SuspendLayout()
            Me.splitOrders.SuspendLayout()
            Me.grpOrderLines.SuspendLayout()
            CType(Me.dgvCurrentLines, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlOrderLineActions.SuspendLayout()
            Me.grpAddItem.SuspendLayout()
            CType(Me.numUnitPrice, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.numQuantity, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpCustomer.SuspendLayout()
            Me.tblCustomerCredit.SuspendLayout()
            Me.grpSelectedOrderLines.SuspendLayout()
            CType(Me.dgvSelectedOrderLines, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.pnlOrderActions.SuspendLayout()
            Me.grpExistingOrders.SuspendLayout()
            CType(Me.dgvExistingOrders, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabInvoices.SuspendLayout()
            CType(Me.splitInvoices, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitInvoices.Panel1.SuspendLayout()
            Me.splitInvoices.Panel2.SuspendLayout()
            Me.splitInvoices.SuspendLayout()
            Me.grpInvoicesList.SuspendLayout()
            CType(Me.dgvInvoices, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.splitInvoiceDetails, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitInvoiceDetails.Panel1.SuspendLayout()
            Me.splitInvoiceDetails.Panel2.SuspendLayout()
            Me.splitInvoiceDetails.SuspendLayout()
            Me.grpInvoiceLines.SuspendLayout()
            CType(Me.dgvInvoiceLines, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpJournalPreview.SuspendLayout()
            CType(Me.dgvJournalPreview, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.tabStockMonitor.SuspendLayout()
            CType(Me.splitStock, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.splitStock.Panel1.SuspendLayout()
            Me.splitStock.Panel2.SuspendLayout()
            Me.splitStock.SuspendLayout()
            Me.grpAllProducts.SuspendLayout()
            CType(Me.dgvProductsCatalog, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.grpStockSimulation.SuspendLayout()
            Me.pnlSimBadgeContainer.SuspendLayout()
            CType(Me.numSimQty, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'pnlHeader
            '
            Me.pnlHeader.BackColor = System.Drawing.Color.White
            Me.pnlHeader.Controls.Add(Me.pnlKpiContainer)
            Me.pnlHeader.Controls.Add(Me.lblHeaderSubtitle)
            Me.pnlHeader.Controls.Add(Me.lblHeaderTitle)
            Me.pnlHeader.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlHeader.Location = New System.Drawing.Point(0, 0)
            Me.pnlHeader.Name = "pnlHeader"
            Me.pnlHeader.Padding = New System.Windows.Forms.Padding(16, 6, 16, 6)
            Me.pnlHeader.Size = New System.Drawing.Size(1200, 74)
            Me.pnlHeader.TabIndex = 0
            '
            'pnlKpiContainer
            '
            Me.pnlKpiContainer.Controls.Add(Me.pnlKpiLowStock)
            Me.pnlKpiContainer.Controls.Add(Me.pnlKpiStockGuard)
            Me.pnlKpiContainer.Controls.Add(Me.pnlKpiInvoices)
            Me.pnlKpiContainer.Controls.Add(Me.pnlKpiOrders)
            Me.pnlKpiContainer.Dock = System.Windows.Forms.DockStyle.Right
            Me.pnlKpiContainer.Location = New System.Drawing.Point(420, 6)
            Me.pnlKpiContainer.Name = "pnlKpiContainer"
            Me.pnlKpiContainer.Size = New System.Drawing.Size(770, 62)
            Me.pnlKpiContainer.TabIndex = 2
            '
            'pnlKpiLowStock
            '
            Me.pnlKpiLowStock.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(248, Byte), Integer), CType(CType(235, Byte), Integer))
            Me.pnlKpiLowStock.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlKpiLowStock.Controls.Add(Me.lblKpiLowStockValue)
            Me.pnlKpiLowStock.Controls.Add(Me.lblKpiLowStockTitle)
            Me.pnlKpiLowStock.Location = New System.Drawing.Point(574, 1)
            Me.pnlKpiLowStock.Name = "pnlKpiLowStock"
            Me.pnlKpiLowStock.Size = New System.Drawing.Size(185, 60)
            Me.pnlKpiLowStock.TabIndex = 3
            '
            'lblKpiLowStockValue
            '
            Me.lblKpiLowStockValue.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblKpiLowStockValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(217, Byte), Integer), CType(CType(119, Byte), Integer), CType(CType(6, Byte), Integer))
            Me.lblKpiLowStockValue.Location = New System.Drawing.Point(4, 25)
            Me.lblKpiLowStockValue.Name = "lblKpiLowStockValue"
            Me.lblKpiLowStockValue.Size = New System.Drawing.Size(175, 30)
            Me.lblKpiLowStockValue.TabIndex = 1
            Me.lblKpiLowStockValue.Text = "0 Items"
            Me.lblKpiLowStockValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblKpiLowStockTitle
            '
            Me.lblKpiLowStockTitle.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblKpiLowStockTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblKpiLowStockTitle.Location = New System.Drawing.Point(4, 4)
            Me.lblKpiLowStockTitle.Name = "lblKpiLowStockTitle"
            Me.lblKpiLowStockTitle.Size = New System.Drawing.Size(175, 18)
            Me.lblKpiLowStockTitle.TabIndex = 0
            Me.lblKpiLowStockTitle.Text = "LOW STOCK ALERTS"
            Me.lblKpiLowStockTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'pnlKpiStockGuard
            '
            Me.pnlKpiStockGuard.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(247, Byte), Integer), CType(CType(238, Byte), Integer))
            Me.pnlKpiStockGuard.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlKpiStockGuard.Controls.Add(Me.lblKpiStockGuardValue)
            Me.pnlKpiStockGuard.Controls.Add(Me.lblKpiStockGuardTitle)
            Me.pnlKpiStockGuard.Location = New System.Drawing.Point(384, 1)
            Me.pnlKpiStockGuard.Name = "pnlKpiStockGuard"
            Me.pnlKpiStockGuard.Size = New System.Drawing.Size(185, 60)
            Me.pnlKpiStockGuard.TabIndex = 2
            '
            'lblKpiStockGuardValue
            '
            Me.lblKpiStockGuardValue.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblKpiStockGuardValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblKpiStockGuardValue.Location = New System.Drawing.Point(4, 25)
            Me.lblKpiStockGuardValue.Name = "lblKpiStockGuardValue"
            Me.lblKpiStockGuardValue.Size = New System.Drawing.Size(175, 30)
            Me.lblKpiStockGuardValue.TabIndex = 1
            Me.lblKpiStockGuardValue.Text = "ENFORCING ZERO <= 0"
            Me.lblKpiStockGuardValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblKpiStockGuardTitle
            '
            Me.lblKpiStockGuardTitle.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblKpiStockGuardTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblKpiStockGuardTitle.Location = New System.Drawing.Point(4, 4)
            Me.lblKpiStockGuardTitle.Name = "lblKpiStockGuardTitle"
            Me.lblKpiStockGuardTitle.Size = New System.Drawing.Size(175, 18)
            Me.lblKpiStockGuardTitle.TabIndex = 0
            Me.lblKpiStockGuardTitle.Text = "STOCK GUARD INVARIANT"
            Me.lblKpiStockGuardTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'pnlKpiInvoices
            '
            Me.pnlKpiInvoices.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlKpiInvoices.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlKpiInvoices.Controls.Add(Me.lblKpiInvoicesValue)
            Me.pnlKpiInvoices.Controls.Add(Me.lblKpiInvoicesTitle)
            Me.pnlKpiInvoices.Location = New System.Drawing.Point(194, 1)
            Me.pnlKpiInvoices.Name = "pnlKpiInvoices"
            Me.pnlKpiInvoices.Size = New System.Drawing.Size(185, 60)
            Me.pnlKpiInvoices.TabIndex = 1
            '
            'lblKpiInvoicesValue
            '
            Me.lblKpiInvoicesValue.Font = New System.Drawing.Font("Segoe UI", 11.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblKpiInvoicesValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.lblKpiInvoicesValue.Location = New System.Drawing.Point(4, 25)
            Me.lblKpiInvoicesValue.Name = "lblKpiInvoicesValue"
            Me.lblKpiInvoicesValue.Size = New System.Drawing.Size(175, 30)
            Me.lblKpiInvoicesValue.TabIndex = 1
            Me.lblKpiInvoicesValue.Text = "$0.00"
            Me.lblKpiInvoicesValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblKpiInvoicesTitle
            '
            Me.lblKpiInvoicesTitle.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblKpiInvoicesTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblKpiInvoicesTitle.Location = New System.Drawing.Point(4, 4)
            Me.lblKpiInvoicesTitle.Name = "lblKpiInvoicesTitle"
            Me.lblKpiInvoicesTitle.Size = New System.Drawing.Size(175, 18)
            Me.lblKpiInvoicesTitle.TabIndex = 0
            Me.lblKpiInvoicesTitle.Text = "POSTED INVOICES MTD"
            Me.lblKpiInvoicesTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'pnlKpiOrders
            '
            Me.pnlKpiOrders.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlKpiOrders.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlKpiOrders.Controls.Add(Me.lblKpiOrdersValue)
            Me.pnlKpiOrders.Controls.Add(Me.lblKpiOrdersTitle)
            Me.pnlKpiOrders.Location = New System.Drawing.Point(4, 1)
            Me.pnlKpiOrders.Name = "pnlKpiOrders"
            Me.pnlKpiOrders.Size = New System.Drawing.Size(185, 60)
            Me.pnlKpiOrders.TabIndex = 0
            '
            'lblKpiOrdersValue
            '
            Me.lblKpiOrdersValue.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblKpiOrdersValue.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblKpiOrdersValue.Location = New System.Drawing.Point(4, 25)
            Me.lblKpiOrdersValue.Name = "lblKpiOrdersValue"
            Me.lblKpiOrdersValue.Size = New System.Drawing.Size(175, 30)
            Me.lblKpiOrdersValue.TabIndex = 1
            Me.lblKpiOrdersValue.Text = "2 Orders ($665.00)"
            Me.lblKpiOrdersValue.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblKpiOrdersTitle
            '
            Me.lblKpiOrdersTitle.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblKpiOrdersTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblKpiOrdersTitle.Location = New System.Drawing.Point(4, 4)
            Me.lblKpiOrdersTitle.Name = "lblKpiOrdersTitle"
            Me.lblKpiOrdersTitle.Size = New System.Drawing.Size(175, 18)
            Me.lblKpiOrdersTitle.TabIndex = 0
            Me.lblKpiOrdersTitle.Text = "ACTIVE SALES ORDERS"
            Me.lblKpiOrdersTitle.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'lblHeaderSubtitle
            '
            Me.lblHeaderSubtitle.Visible = False
            '
            'lblHeaderTitle
            '
            Me.lblHeaderTitle.Visible = False
            '
            'tabMain
            '
            Me.tabMain.Controls.Add(Me.tabOrders)
            Me.tabMain.Controls.Add(Me.tabInvoices)
            Me.tabMain.Controls.Add(Me.tabStockMonitor)
            Me.tabMain.Dock = System.Windows.Forms.DockStyle.Fill
            Me.tabMain.Location = New System.Drawing.Point(0, 52)
            Me.tabMain.Name = "tabMain"
            Me.tabMain.SelectedIndex = 0
            Me.tabMain.Size = New System.Drawing.Size(1200, 668)
            Me.tabMain.TabIndex = 1
            '
            'tabOrders
            '
            Me.tabOrders.Controls.Add(Me.splitOrders)
            Me.tabOrders.Location = New System.Drawing.Point(4, 26)
            Me.tabOrders.Name = "tabOrders"
            Me.tabOrders.Padding = New System.Windows.Forms.Padding(8)
            Me.tabOrders.Size = New System.Drawing.Size(1192, 605)
            Me.tabOrders.TabIndex = 0
            Me.tabOrders.Text = "Sales Orders & Entry"
            Me.tabOrders.UseVisualStyleBackColor = True
            '
            'splitOrders
            '
            Me.splitOrders.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitOrders.FixedPanel = System.Windows.Forms.FixedPanel.None
            Me.splitOrders.Location = New System.Drawing.Point(8, 8)
            Me.splitOrders.Name = "splitOrders"
            '
            'splitOrders.Panel1
            '
            Me.splitOrders.Panel1.AutoScroll = True
            Me.splitOrders.Panel1.Controls.Add(Me.grpOrderLines)
            Me.splitOrders.Panel1.Controls.Add(Me.grpAddItem)
            Me.splitOrders.Panel1.Controls.Add(Me.grpCustomer)
            Me.splitOrders.Panel1MinSize = 540
            '
            'splitOrders.Panel2
            '
            Me.splitOrders.Panel2.Controls.Add(Me.grpSelectedOrderLines)
            Me.splitOrders.Panel2.Controls.Add(Me.pnlOrderActions)
            Me.splitOrders.Panel2.Controls.Add(Me.grpExistingOrders)
            Me.splitOrders.Panel2MinSize = 400
            Me.splitOrders.Size = New System.Drawing.Size(1176, 589)
            Me.splitOrders.SplitterDistance = 560
            Me.splitOrders.TabIndex = 0
            '
            'grpOrderLines
            '
            Me.grpOrderLines.Controls.Add(Me.dgvCurrentLines)
            Me.grpOrderLines.Controls.Add(Me.pnlOrderLineActions)
            Me.grpOrderLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpOrderLines.Location = New System.Drawing.Point(0, 338)
            Me.grpOrderLines.Name = "grpOrderLines"
            Me.grpOrderLines.Padding = New System.Windows.Forms.Padding(8, 26, 8, 8)
            Me.grpOrderLines.Size = New System.Drawing.Size(530, 251)
            Me.grpOrderLines.TabIndex = 2
            Me.grpOrderLines.TabStop = False
            Me.grpOrderLines.Text = "3. Active Order Items & Commitment"
            '
            'dgvCurrentLines
            '
            Me.dgvCurrentLines.AllowUserToAddRows = False
            Me.dgvCurrentLines.AllowUserToDeleteRows = False
            Me.dgvCurrentLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvCurrentLines.BackgroundColor = System.Drawing.Color.White
            Me.dgvCurrentLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvCurrentLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvCurrentLines.Location = New System.Drawing.Point(8, 26)
            Me.dgvCurrentLines.MultiSelect = False
            Me.dgvCurrentLines.Name = "dgvCurrentLines"
            Me.dgvCurrentLines.ReadOnly = True
            Me.dgvCurrentLines.RowHeadersVisible = False
            Me.dgvCurrentLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvCurrentLines.Size = New System.Drawing.Size(524, 169)
            Me.dgvCurrentLines.TabIndex = 0
            '
            'pnlOrderLineActions
            '
            Me.pnlOrderLineActions.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.pnlOrderLineActions.Controls.Add(Me.btnConfirmOrder)
            Me.pnlOrderLineActions.Controls.Add(Me.btnSaveOrderDraft)
            Me.pnlOrderLineActions.Controls.Add(Me.btnClearOrderForm)
            Me.pnlOrderLineActions.Controls.Add(Me.btnRemoveLine)
            Me.pnlOrderLineActions.Controls.Add(Me.lblOrderTotalDisplay)
            Me.pnlOrderLineActions.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.pnlOrderLineActions.Location = New System.Drawing.Point(3, 189)
            Me.pnlOrderLineActions.Name = "pnlOrderLineActions"
            Me.pnlOrderLineActions.Size = New System.Drawing.Size(524, 82)
            Me.pnlOrderLineActions.TabIndex = 1
            '
            'btnConfirmOrder
            '
            Me.btnConfirmOrder.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.btnConfirmOrder.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnConfirmOrder.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnConfirmOrder.ForeColor = System.Drawing.Color.White
            Me.btnConfirmOrder.Location = New System.Drawing.Point(368, 40)
            Me.btnConfirmOrder.Name = "btnConfirmOrder"
            Me.btnConfirmOrder.Size = New System.Drawing.Size(150, 36)
            Me.btnConfirmOrder.TabIndex = 4
            Me.btnConfirmOrder.Text = "Confirm Order"
            Me.btnConfirmOrder.UseVisualStyleBackColor = False
            '
            'btnSaveOrderDraft
            '
            Me.btnSaveOrderDraft.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.btnSaveOrderDraft.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnSaveOrderDraft.ForeColor = System.Drawing.Color.White
            Me.btnSaveOrderDraft.Location = New System.Drawing.Point(232, 40)
            Me.btnSaveOrderDraft.Name = "btnSaveOrderDraft"
            Me.btnSaveOrderDraft.Size = New System.Drawing.Size(130, 36)
            Me.btnSaveOrderDraft.TabIndex = 3
            Me.btnSaveOrderDraft.Text = "Save Draft"
            Me.btnSaveOrderDraft.UseVisualStyleBackColor = False
            '
            'btnClearOrderForm
            '
            Me.btnClearOrderForm.Location = New System.Drawing.Point(120, 40)
            Me.btnClearOrderForm.Name = "btnClearOrderForm"
            Me.btnClearOrderForm.Size = New System.Drawing.Size(106, 36)
            Me.btnClearOrderForm.TabIndex = 2
            Me.btnClearOrderForm.Text = "Clear"
            Me.btnClearOrderForm.UseVisualStyleBackColor = True
            '
            'btnRemoveLine
            '
            Me.btnRemoveLine.Location = New System.Drawing.Point(8, 40)
            Me.btnRemoveLine.Name = "btnRemoveLine"
            Me.btnRemoveLine.Size = New System.Drawing.Size(106, 36)
            Me.btnRemoveLine.TabIndex = 1
            Me.btnRemoveLine.Text = "Remove Item"
            Me.btnRemoveLine.UseVisualStyleBackColor = True
            '
            'lblOrderTotalDisplay
            '
            Me.lblOrderTotalDisplay.Font = New System.Drawing.Font("Segoe UI", 11.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblOrderTotalDisplay.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblOrderTotalDisplay.Location = New System.Drawing.Point(8, 8)
            Me.lblOrderTotalDisplay.Name = "lblOrderTotalDisplay"
            Me.lblOrderTotalDisplay.Size = New System.Drawing.Size(510, 24)
            Me.lblOrderTotalDisplay.TabIndex = 0
            Me.lblOrderTotalDisplay.Text = "Order Grand Total: $0.00 (0 items)"
            Me.lblOrderTotalDisplay.TextAlign = System.Drawing.ContentAlignment.MiddleRight
            '
            'grpAddItem
            '
            Me.grpAddItem.Controls.Add(Me.lblStockWarningNotice)
            Me.grpAddItem.Controls.Add(Me.btnAddLineItem)
            Me.grpAddItem.Controls.Add(Me.lblStockBadgeItem)
            Me.grpAddItem.Controls.Add(Me.lblItemStockInfo)
            Me.grpAddItem.Controls.Add(Me.numUnitPrice)
            Me.grpAddItem.Controls.Add(Me.lblPricePrompt)
            Me.grpAddItem.Controls.Add(Me.numQuantity)
            Me.grpAddItem.Controls.Add(Me.lblQtyPrompt)
            Me.grpAddItem.Controls.Add(Me.cmbProduct)
            Me.grpAddItem.Controls.Add(Me.lblProductPrompt)
            Me.grpAddItem.Dock = System.Windows.Forms.DockStyle.Top
            Me.grpAddItem.Location = New System.Drawing.Point(0, 126)
            Me.grpAddItem.Name = "grpAddItem"
            Me.grpAddItem.Padding = New System.Windows.Forms.Padding(12, 24, 12, 8)
            Me.grpAddItem.Size = New System.Drawing.Size(530, 218)
            Me.grpAddItem.TabIndex = 1
            Me.grpAddItem.TabStop = False
            Me.grpAddItem.Text = "2. Add Line Item & Real-Time Stock Check"
            '
            'lblStockWarningNotice
            '
            Me.lblStockWarningNotice.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblStockWarningNotice.BackColor = System.Drawing.Color.FromArgb(CType(CType(255, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(205, Byte), Integer))
            Me.lblStockWarningNotice.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblStockWarningNotice.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblStockWarningNotice.ForeColor = System.Drawing.Color.FromArgb(CType(CType(133, Byte), Integer), CType(CType(100, Byte), Integer), CType(CType(4, Byte), Integer))
            Me.lblStockWarningNotice.Location = New System.Drawing.Point(12, 156)
            Me.lblStockWarningNotice.Name = "lblStockWarningNotice"
            Me.lblStockWarningNotice.Padding = New System.Windows.Forms.Padding(4)
            Me.lblStockWarningNotice.Size = New System.Drawing.Size(506, 52)
            Me.lblStockWarningNotice.TabIndex = 9
            Me.lblStockWarningNotice.Text = "Stock guard status message"
            '
            'btnAddLineItem
            '
            Me.btnAddLineItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnAddLineItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.btnAddLineItem.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnAddLineItem.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnAddLineItem.ForeColor = System.Drawing.Color.White
            Me.btnAddLineItem.Location = New System.Drawing.Point(315, 112)
            Me.btnAddLineItem.Name = "btnAddLineItem"
            Me.btnAddLineItem.Size = New System.Drawing.Size(203, 36)
            Me.btnAddLineItem.TabIndex = 8
            Me.btnAddLineItem.Text = "+ Add Line"
            Me.btnAddLineItem.UseVisualStyleBackColor = False
            '
            'lblStockBadgeItem
            '
            Me.lblStockBadgeItem.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblStockBadgeItem.AutoSize = True
            Me.lblStockBadgeItem.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblStockBadgeItem.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblStockBadgeItem.ForeColor = System.Drawing.Color.White
            Me.lblStockBadgeItem.Location = New System.Drawing.Point(360, 72)
            Me.lblStockBadgeItem.Name = "lblStockBadgeItem"
            Me.lblStockBadgeItem.Padding = New System.Windows.Forms.Padding(6, 3, 6, 3)
            Me.lblStockBadgeItem.Size = New System.Drawing.Size(70, 20)
            Me.lblStockBadgeItem.TabIndex = 7
            Me.lblStockBadgeItem.Text = "IN STOCK"
            Me.lblStockBadgeItem.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.lblStockBadgeItem.UseMnemonic = False
            '
            'lblItemStockInfo
            '
            Me.lblItemStockInfo.AutoSize = True
            Me.lblItemStockInfo.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblItemStockInfo.Location = New System.Drawing.Point(12, 76)
            Me.lblItemStockInfo.Name = "lblItemStockInfo"
            Me.lblItemStockInfo.Size = New System.Drawing.Size(248, 17)
            Me.lblItemStockInfo.TabIndex = 6
            Me.lblItemStockInfo.Text = "On Hand: 0 | Reserved: 0 | Free to Promise: 0"
            '
            'numUnitPrice
            '
            Me.numUnitPrice.DecimalPlaces = 2
            Me.numUnitPrice.Location = New System.Drawing.Point(180, 122)
            Me.numUnitPrice.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numUnitPrice.Name = "numUnitPrice"
            Me.numUnitPrice.Size = New System.Drawing.Size(120, 26)
            Me.numUnitPrice.TabIndex = 5
            Me.numUnitPrice.ThousandsSeparator = True
            '
            'lblPricePrompt
            '
            Me.lblPricePrompt.AutoSize = True
            Me.lblPricePrompt.Location = New System.Drawing.Point(180, 102)
            Me.lblPricePrompt.Name = "lblPricePrompt"
            Me.lblPricePrompt.Size = New System.Drawing.Size(92, 17)
            Me.lblPricePrompt.TabIndex = 4
            Me.lblPricePrompt.Text = "Unit Price ($):"
            '
            'numQuantity
            '
            Me.numQuantity.Location = New System.Drawing.Point(12, 122)
            Me.numQuantity.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numQuantity.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numQuantity.Name = "numQuantity"
            Me.numQuantity.Size = New System.Drawing.Size(140, 26)
            Me.numQuantity.TabIndex = 3
            Me.numQuantity.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'lblQtyPrompt
            '
            Me.lblQtyPrompt.AutoSize = True
            Me.lblQtyPrompt.Location = New System.Drawing.Point(12, 102)
            Me.lblQtyPrompt.Name = "lblQtyPrompt"
            Me.lblQtyPrompt.Size = New System.Drawing.Size(107, 17)
            Me.lblQtyPrompt.TabIndex = 2
            Me.lblQtyPrompt.Text = "Order Qty (Units):"
            '
            'cmbProduct
            '
            Me.cmbProduct.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmbProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbProduct.FormattingEnabled = True
            Me.cmbProduct.Location = New System.Drawing.Point(12, 44)
            Me.cmbProduct.Name = "cmbProduct"
            Me.cmbProduct.Size = New System.Drawing.Size(506, 25)
            Me.cmbProduct.TabIndex = 1
            '
            'lblProductPrompt
            '
            Me.lblProductPrompt.AutoSize = True
            Me.lblProductPrompt.Location = New System.Drawing.Point(12, 24)
            Me.lblProductPrompt.Name = "lblProductPrompt"
            Me.lblProductPrompt.Size = New System.Drawing.Size(130, 17)
            Me.lblProductPrompt.TabIndex = 0
            Me.lblProductPrompt.Text = "Select Product / SKU:"
            '
            'grpCustomer
            '
            Me.grpCustomer.Controls.Add(Me.tblCustomerCredit)
            Me.grpCustomer.Controls.Add(Me.cmbCustomer)
            Me.grpCustomer.Controls.Add(Me.lblCustomerPrompt)
            Me.grpCustomer.Dock = System.Windows.Forms.DockStyle.Top
            Me.grpCustomer.Location = New System.Drawing.Point(0, 0)
            Me.grpCustomer.Name = "grpCustomer"
            Me.grpCustomer.Padding = New System.Windows.Forms.Padding(12, 24, 12, 8)
            Me.grpCustomer.Size = New System.Drawing.Size(530, 135)
            Me.grpCustomer.TabIndex = 0
            Me.grpCustomer.TabStop = False
            Me.grpCustomer.Text = "1. Customer Selection & AR Credit Status"
            '
            'tblCustomerCredit
            '
            Me.tblCustomerCredit.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.tblCustomerCredit.ColumnCount = 4
            Me.tblCustomerCredit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.0!))
            Me.tblCustomerCredit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.0!))
            Me.tblCustomerCredit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 26.0!))
            Me.tblCustomerCredit.ColumnStyles.Add(New System.Windows.Forms.ColumnStyle(System.Windows.Forms.SizeType.Percent, 22.0!))
            Me.tblCustomerCredit.Controls.Add(Me.lblCreditLimitTitle, 0, 0)
            Me.tblCustomerCredit.Controls.Add(Me.lblCreditLimitVal, 0, 1)
            Me.tblCustomerCredit.Controls.Add(Me.lblCurrentARTitle, 1, 0)
            Me.tblCustomerCredit.Controls.Add(Me.lblCurrentARVal, 1, 1)
            Me.tblCustomerCredit.Controls.Add(Me.lblAvailCreditTitle, 2, 0)
            Me.tblCustomerCredit.Controls.Add(Me.lblAvailCreditVal, 2, 1)
            Me.tblCustomerCredit.Controls.Add(Me.lblCustomerBadge, 3, 0)
            Me.tblCustomerCredit.Location = New System.Drawing.Point(12, 75)
            Me.tblCustomerCredit.Name = "tblCustomerCredit"
            Me.tblCustomerCredit.RowCount = 2
            Me.tblCustomerCredit.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 20.0!))
            Me.tblCustomerCredit.RowStyles.Add(New System.Windows.Forms.RowStyle(System.Windows.Forms.SizeType.Absolute, 28.0!))
            Me.tblCustomerCredit.SetRowSpan(Me.lblCustomerBadge, 2)
            Me.tblCustomerCredit.Size = New System.Drawing.Size(506, 48)
            Me.tblCustomerCredit.TabIndex = 2
            '
            'lblCustomerBadge
            '
            Me.lblCustomerBadge.BackColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblCustomerBadge.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblCustomerBadge.Font = New System.Drawing.Font("Segoe UI", 8.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCustomerBadge.ForeColor = System.Drawing.Color.White
            Me.lblCustomerBadge.Location = New System.Drawing.Point(397, 0)
            Me.lblCustomerBadge.Margin = New System.Windows.Forms.Padding(3, 4, 3, 4)
            Me.lblCustomerBadge.Name = "lblCustomerBadge"
            Me.lblCustomerBadge.Size = New System.Drawing.Size(106, 40)
            Me.lblCustomerBadge.TabIndex = 6
            Me.lblCustomerBadge.Text = "CREDIT APPROVED"
            Me.lblCustomerBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            Me.lblCustomerBadge.UseMnemonic = False
            '
            'lblAvailCreditVal
            '
            Me.lblAvailCreditVal.AutoEllipsis = True
            Me.lblAvailCreditVal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblAvailCreditVal.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAvailCreditVal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(40, Byte), Integer), CType(CType(167, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblAvailCreditVal.Location = New System.Drawing.Point(265, 20)
            Me.lblAvailCreditVal.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.lblAvailCreditVal.Name = "lblAvailCreditVal"
            Me.lblAvailCreditVal.Size = New System.Drawing.Size(126, 28)
            Me.lblAvailCreditVal.TabIndex = 5
            Me.lblAvailCreditVal.Text = "$0.00"
            Me.lblAvailCreditVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblAvailCreditTitle
            '
            Me.lblAvailCreditTitle.AutoEllipsis = True
            Me.lblAvailCreditTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblAvailCreditTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblAvailCreditTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblAvailCreditTitle.Location = New System.Drawing.Point(265, 0)
            Me.lblAvailCreditTitle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.lblAvailCreditTitle.Name = "lblAvailCreditTitle"
            Me.lblAvailCreditTitle.Size = New System.Drawing.Size(126, 20)
            Me.lblAvailCreditTitle.TabIndex = 2
            Me.lblAvailCreditTitle.Text = "Avail. Credit:"
            Me.lblAvailCreditTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'lblCurrentARVal
            '
            Me.lblCurrentARVal.AutoEllipsis = True
            Me.lblCurrentARVal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblCurrentARVal.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCurrentARVal.ForeColor = System.Drawing.Color.FromArgb(CType(CType(220, Byte), Integer), CType(CType(53, Byte), Integer), CType(CType(69, Byte), Integer))
            Me.lblCurrentARVal.Location = New System.Drawing.Point(134, 20)
            Me.lblCurrentARVal.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.lblCurrentARVal.Name = "lblCurrentARVal"
            Me.lblCurrentARVal.Size = New System.Drawing.Size(125, 28)
            Me.lblCurrentARVal.TabIndex = 4
            Me.lblCurrentARVal.Text = "$0.00"
            Me.lblCurrentARVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblCurrentARTitle
            '
            Me.lblCurrentARTitle.AutoEllipsis = True
            Me.lblCurrentARTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblCurrentARTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCurrentARTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblCurrentARTitle.Location = New System.Drawing.Point(134, 0)
            Me.lblCurrentARTitle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.lblCurrentARTitle.Name = "lblCurrentARTitle"
            Me.lblCurrentARTitle.Size = New System.Drawing.Size(125, 20)
            Me.lblCurrentARTitle.TabIndex = 1
            Me.lblCurrentARTitle.Text = "Current AR:"
            Me.lblCurrentARTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'lblCreditLimitVal
            '
            Me.lblCreditLimitVal.AutoEllipsis = True
            Me.lblCreditLimitVal.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblCreditLimitVal.Font = New System.Drawing.Font("Segoe UI", 9.0!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCreditLimitVal.Location = New System.Drawing.Point(3, 20)
            Me.lblCreditLimitVal.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.lblCreditLimitVal.Name = "lblCreditLimitVal"
            Me.lblCreditLimitVal.Size = New System.Drawing.Size(125, 28)
            Me.lblCreditLimitVal.TabIndex = 3
            Me.lblCreditLimitVal.Text = "$0.00"
            Me.lblCreditLimitVal.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'lblCreditLimitTitle
            '
            Me.lblCreditLimitTitle.AutoEllipsis = True
            Me.lblCreditLimitTitle.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblCreditLimitTitle.Font = New System.Drawing.Font("Segoe UI", 8.5!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblCreditLimitTitle.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblCreditLimitTitle.Location = New System.Drawing.Point(3, 0)
            Me.lblCreditLimitTitle.Margin = New System.Windows.Forms.Padding(3, 0, 3, 0)
            Me.lblCreditLimitTitle.Name = "lblCreditLimitTitle"
            Me.lblCreditLimitTitle.Size = New System.Drawing.Size(125, 20)
            Me.lblCreditLimitTitle.TabIndex = 0
            Me.lblCreditLimitTitle.Text = "Credit Limit:"
            Me.lblCreditLimitTitle.TextAlign = System.Drawing.ContentAlignment.BottomLeft
            '
            'cmbCustomer
            '
            Me.cmbCustomer.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.cmbCustomer.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbCustomer.FormattingEnabled = True
            Me.cmbCustomer.Location = New System.Drawing.Point(12, 44)
            Me.cmbCustomer.Name = "cmbCustomer"
            Me.cmbCustomer.Size = New System.Drawing.Size(506, 25)
            Me.cmbCustomer.TabIndex = 1
            '
            'lblCustomerPrompt
            '
            Me.lblCustomerPrompt.AutoSize = True
            Me.lblCustomerPrompt.Location = New System.Drawing.Point(12, 24)
            Me.lblCustomerPrompt.Name = "lblCustomerPrompt"
            Me.lblCustomerPrompt.Size = New System.Drawing.Size(157, 17)
            Me.lblCustomerPrompt.TabIndex = 0
            Me.lblCustomerPrompt.Text = "Select Customer Account:"
            '
            'grpSelectedOrderLines
            '
            Me.grpSelectedOrderLines.Controls.Add(Me.dgvSelectedOrderLines)
            Me.grpSelectedOrderLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpSelectedOrderLines.Location = New System.Drawing.Point(0, 375)
            Me.grpSelectedOrderLines.Name = "grpSelectedOrderLines"
            Me.grpSelectedOrderLines.Size = New System.Drawing.Size(642, 214)
            Me.grpSelectedOrderLines.TabIndex = 2
            Me.grpSelectedOrderLines.TabStop = False
            Me.grpSelectedOrderLines.Text = "Order Items Detail View"
            '
            'dgvSelectedOrderLines
            '
            Me.dgvSelectedOrderLines.AllowUserToAddRows = False
            Me.dgvSelectedOrderLines.AllowUserToDeleteRows = False
            Me.dgvSelectedOrderLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvSelectedOrderLines.BackgroundColor = System.Drawing.Color.White
            Me.dgvSelectedOrderLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvSelectedOrderLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvSelectedOrderLines.Location = New System.Drawing.Point(3, 20)
            Me.dgvSelectedOrderLines.MultiSelect = False
            Me.dgvSelectedOrderLines.Name = "dgvSelectedOrderLines"
            Me.dgvSelectedOrderLines.ReadOnly = True
            Me.dgvSelectedOrderLines.RowHeadersVisible = False
            Me.dgvSelectedOrderLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvSelectedOrderLines.Size = New System.Drawing.Size(636, 191)
            Me.dgvSelectedOrderLines.TabIndex = 0
            '
            'pnlOrderActions
            '
            Me.pnlOrderActions.BackColor = System.Drawing.Color.FromArgb(CType(CType(240, Byte), Integer), CType(CType(244, Byte), Integer), CType(CType(255, Byte), Integer))
            Me.pnlOrderActions.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.pnlOrderActions.Controls.Add(Me.lblSelectedOrderSummary)
            Me.pnlOrderActions.Controls.Add(Me.btnGenerateInvoiceFromSO)
            Me.pnlOrderActions.Dock = System.Windows.Forms.DockStyle.Top
            Me.pnlOrderActions.Location = New System.Drawing.Point(0, 305)
            Me.pnlOrderActions.Name = "pnlOrderActions"
            Me.pnlOrderActions.Padding = New System.Windows.Forms.Padding(8)
            Me.pnlOrderActions.Size = New System.Drawing.Size(642, 78)
            Me.pnlOrderActions.TabIndex = 1
            '
            'lblSelectedOrderSummary
            '
            Me.lblSelectedOrderSummary.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.lblSelectedOrderSummary.Location = New System.Drawing.Point(8, 4)
            Me.lblSelectedOrderSummary.Name = "lblSelectedOrderSummary"
            Me.lblSelectedOrderSummary.Size = New System.Drawing.Size(620, 20)
            Me.lblSelectedOrderSummary.TabIndex = 1
            Me.lblSelectedOrderSummary.Text = "Selected Order: None"
            Me.lblSelectedOrderSummary.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'btnGenerateInvoiceFromSO
            '
            Me.btnGenerateInvoiceFromSO.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
            Me.btnGenerateInvoiceFromSO.BackColor = System.Drawing.Color.FromArgb(CType(CType(13, Byte), Integer), CType(CType(110, Byte), Integer), CType(CType(253, Byte), Integer))
            Me.btnGenerateInvoiceFromSO.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnGenerateInvoiceFromSO.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnGenerateInvoiceFromSO.ForeColor = System.Drawing.Color.White
            Me.btnGenerateInvoiceFromSO.Location = New System.Drawing.Point(8, 28)
            Me.btnGenerateInvoiceFromSO.Name = "btnGenerateInvoiceFromSO"
            Me.btnGenerateInvoiceFromSO.Size = New System.Drawing.Size(430, 42)
            Me.btnGenerateInvoiceFromSO.TabIndex = 0
            Me.btnGenerateInvoiceFromSO.Text = "⚡ Fulfill & Generate Sales Invoice"
            Me.btnGenerateInvoiceFromSO.UseMnemonic = False
            Me.btnGenerateInvoiceFromSO.UseVisualStyleBackColor = False
            '
            'grpExistingOrders
            '
            Me.grpExistingOrders.Controls.Add(Me.dgvExistingOrders)
            Me.grpExistingOrders.Dock = System.Windows.Forms.DockStyle.Top
            Me.grpExistingOrders.Location = New System.Drawing.Point(0, 0)
            Me.grpExistingOrders.Name = "grpExistingOrders"
            Me.grpExistingOrders.Size = New System.Drawing.Size(642, 305)
            Me.grpExistingOrders.TabIndex = 0
            Me.grpExistingOrders.TabStop = False
            Me.grpExistingOrders.Text = "Commercial Sales Orders"
            '
            'dgvExistingOrders
            '
            Me.dgvExistingOrders.AllowUserToAddRows = False
            Me.dgvExistingOrders.AllowUserToDeleteRows = False
            Me.dgvExistingOrders.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvExistingOrders.BackgroundColor = System.Drawing.Color.White
            Me.dgvExistingOrders.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvExistingOrders.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvExistingOrders.Location = New System.Drawing.Point(3, 20)
            Me.dgvExistingOrders.MultiSelect = False
            Me.dgvExistingOrders.Name = "dgvExistingOrders"
            Me.dgvExistingOrders.ReadOnly = True
            Me.dgvExistingOrders.RowHeadersVisible = False
            Me.dgvExistingOrders.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvExistingOrders.Size = New System.Drawing.Size(636, 282)
            Me.dgvExistingOrders.TabIndex = 0
            '
            'tabInvoices
            '
            Me.tabInvoices.Controls.Add(Me.splitInvoices)
            Me.tabInvoices.Location = New System.Drawing.Point(4, 26)
            Me.tabInvoices.Name = "tabInvoices"
            Me.tabInvoices.Padding = New System.Windows.Forms.Padding(8)
            Me.tabInvoices.Size = New System.Drawing.Size(1192, 605)
            Me.tabInvoices.TabIndex = 1
            Me.tabInvoices.Text = "Sales Invoices & GL Postings"
            Me.tabInvoices.UseVisualStyleBackColor = True
            '
            'splitInvoices
            '
            Me.splitInvoices.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitInvoices.Location = New System.Drawing.Point(8, 8)
            Me.splitInvoices.Name = "splitInvoices"
            Me.splitInvoices.Orientation = System.Windows.Forms.Orientation.Horizontal
            '
            'splitInvoices.Panel1
            '
            Me.splitInvoices.Panel1.Controls.Add(Me.grpInvoicesList)
            '
            'splitInvoices.Panel2
            '
            Me.splitInvoices.Panel2.Controls.Add(Me.splitInvoiceDetails)
            Me.splitInvoices.Size = New System.Drawing.Size(1176, 589)
            Me.splitInvoices.SplitterDistance = 270
            Me.splitInvoices.TabIndex = 0
            '
            'grpInvoicesList
            '
            Me.grpInvoicesList.Controls.Add(Me.dgvInvoices)
            Me.grpInvoicesList.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpInvoicesList.Location = New System.Drawing.Point(0, 0)
            Me.grpInvoicesList.Name = "grpInvoicesList"
            Me.grpInvoicesList.Size = New System.Drawing.Size(1176, 270)
            Me.grpInvoicesList.TabIndex = 0
            Me.grpInvoicesList.TabStop = False
            Me.grpInvoicesList.Text = "Posted Sales Invoices (Combined Billing & Inventory Relief)"
            '
            'dgvInvoices
            '
            Me.dgvInvoices.AllowUserToAddRows = False
            Me.dgvInvoices.AllowUserToDeleteRows = False
            Me.dgvInvoices.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvInvoices.BackgroundColor = System.Drawing.Color.White
            Me.dgvInvoices.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvInvoices.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvInvoices.Location = New System.Drawing.Point(3, 20)
            Me.dgvInvoices.MultiSelect = False
            Me.dgvInvoices.Name = "dgvInvoices"
            Me.dgvInvoices.ReadOnly = True
            Me.dgvInvoices.RowHeadersVisible = False
            Me.dgvInvoices.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvInvoices.Size = New System.Drawing.Size(1170, 247)
            Me.dgvInvoices.TabIndex = 0
            '
            'splitInvoiceDetails
            '
            Me.splitInvoiceDetails.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitInvoiceDetails.Location = New System.Drawing.Point(0, 0)
            Me.splitInvoiceDetails.Name = "splitInvoiceDetails"
            '
            'splitInvoiceDetails.Panel1
            '
            Me.splitInvoiceDetails.Panel1.Controls.Add(Me.grpInvoiceLines)
            '
            'splitInvoiceDetails.Panel2
            '
            Me.splitInvoiceDetails.Panel2.Controls.Add(Me.grpJournalPreview)
            Me.splitInvoiceDetails.Size = New System.Drawing.Size(1176, 315)
            Me.splitInvoiceDetails.SplitterDistance = 580
            Me.splitInvoiceDetails.TabIndex = 0
            '
            'grpInvoiceLines
            '
            Me.grpInvoiceLines.Controls.Add(Me.dgvInvoiceLines)
            Me.grpInvoiceLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpInvoiceLines.Location = New System.Drawing.Point(0, 0)
            Me.grpInvoiceLines.Name = "grpInvoiceLines"
            Me.grpInvoiceLines.Size = New System.Drawing.Size(580, 315)
            Me.grpInvoiceLines.TabIndex = 0
            Me.grpInvoiceLines.TabStop = False
            Me.grpInvoiceLines.Text = "Fulfillment Item Breakdown (Revenue vs COGS)"
            '
            'dgvInvoiceLines
            '
            Me.dgvInvoiceLines.AllowUserToAddRows = False
            Me.dgvInvoiceLines.AllowUserToDeleteRows = False
            Me.dgvInvoiceLines.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvInvoiceLines.BackgroundColor = System.Drawing.Color.White
            Me.dgvInvoiceLines.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvInvoiceLines.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvInvoiceLines.Location = New System.Drawing.Point(3, 20)
            Me.dgvInvoiceLines.MultiSelect = False
            Me.dgvInvoiceLines.Name = "dgvInvoiceLines"
            Me.dgvInvoiceLines.ReadOnly = True
            Me.dgvInvoiceLines.RowHeadersVisible = False
            Me.dgvInvoiceLines.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvInvoiceLines.Size = New System.Drawing.Size(574, 292)
            Me.dgvInvoiceLines.TabIndex = 0
            '
            'grpJournalPreview
            '
            Me.grpJournalPreview.Controls.Add(Me.dgvJournalPreview)
            Me.grpJournalPreview.Controls.Add(Me.lblGlNotice)
            Me.grpJournalPreview.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpJournalPreview.Location = New System.Drawing.Point(0, 0)
            Me.grpJournalPreview.Name = "grpJournalPreview"
            Me.grpJournalPreview.Size = New System.Drawing.Size(592, 315)
            Me.grpJournalPreview.TabIndex = 0
            Me.grpJournalPreview.TabStop = False
            Me.grpJournalPreview.Text = "Double-Entry General Ledger Verification (Debits = Credits)"
            '
            'dgvJournalPreview
            '
            Me.dgvJournalPreview.AllowUserToAddRows = False
            Me.dgvJournalPreview.AllowUserToDeleteRows = False
            Me.dgvJournalPreview.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvJournalPreview.BackgroundColor = System.Drawing.Color.White
            Me.dgvJournalPreview.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvJournalPreview.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvJournalPreview.Location = New System.Drawing.Point(3, 20)
            Me.dgvJournalPreview.MultiSelect = False
            Me.dgvJournalPreview.Name = "dgvJournalPreview"
            Me.dgvJournalPreview.ReadOnly = True
            Me.dgvJournalPreview.RowHeadersVisible = False
            Me.dgvJournalPreview.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvJournalPreview.Size = New System.Drawing.Size(586, 257)
            Me.dgvJournalPreview.TabIndex = 0
            '
            'lblGlNotice
            '
            Me.lblGlNotice.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.lblGlNotice.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.lblGlNotice.ForeColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblGlNotice.Location = New System.Drawing.Point(3, 277)
            Me.lblGlNotice.Name = "lblGlNotice"
            Me.lblGlNotice.Padding = New System.Windows.Forms.Padding(6, 4, 6, 4)
            Me.lblGlNotice.Size = New System.Drawing.Size(586, 35)
            Me.lblGlNotice.TabIndex = 1
            Me.lblGlNotice.Text = "Append-only GL ledger posting: 1200 Accounts Receivable / 4000 Sales Revenue / 5000 COGS / 1300 Inventory."
            Me.lblGlNotice.TextAlign = System.Drawing.ContentAlignment.MiddleLeft
            '
            'tabStockMonitor
            '
            Me.tabStockMonitor.Controls.Add(Me.splitStock)
            Me.tabStockMonitor.Location = New System.Drawing.Point(4, 26)
            Me.tabStockMonitor.Name = "tabStockMonitor"
            Me.tabStockMonitor.Padding = New System.Windows.Forms.Padding(8)
            Me.tabStockMonitor.Size = New System.Drawing.Size(1192, 605)
            Me.tabStockMonitor.TabIndex = 2
            Me.tabStockMonitor.Text = "Live Stock Monitor & Invariant Gate"
            Me.tabStockMonitor.UseVisualStyleBackColor = True
            '
            'splitStock
            '
            Me.splitStock.Dock = System.Windows.Forms.DockStyle.Fill
            Me.splitStock.FixedPanel = System.Windows.Forms.FixedPanel.Panel2
            Me.splitStock.Location = New System.Drawing.Point(8, 8)
            Me.splitStock.Name = "splitStock"
            '
            'splitStock.Panel1
            '
            Me.splitStock.Panel1.Controls.Add(Me.grpAllProducts)
            '
            'splitStock.Panel2
            '
            Me.splitStock.Panel2.Controls.Add(Me.grpStockSimulation)
            Me.splitStock.Panel2MinSize = 380
            Me.splitStock.Size = New System.Drawing.Size(1176, 589)
            Me.splitStock.SplitterDistance = 750
            Me.splitStock.TabIndex = 0
            '
            'grpAllProducts
            '
            Me.grpAllProducts.Controls.Add(Me.dgvProductsCatalog)
            Me.grpAllProducts.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpAllProducts.Location = New System.Drawing.Point(0, 0)
            Me.grpAllProducts.Name = "grpAllProducts"
            Me.grpAllProducts.Size = New System.Drawing.Size(750, 589)
            Me.grpAllProducts.TabIndex = 0
            Me.grpAllProducts.TabStop = False
            Me.grpAllProducts.Text = "Perpetual Inventory Stock Balances & Real-Time Availability Badges"
            '
            'dgvProductsCatalog
            '
            Me.dgvProductsCatalog.AllowUserToAddRows = False
            Me.dgvProductsCatalog.AllowUserToDeleteRows = False
            Me.dgvProductsCatalog.AutoSizeColumnsMode = System.Windows.Forms.DataGridViewAutoSizeColumnsMode.Fill
            Me.dgvProductsCatalog.BackgroundColor = System.Drawing.Color.White
            Me.dgvProductsCatalog.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize
            Me.dgvProductsCatalog.Dock = System.Windows.Forms.DockStyle.Fill
            Me.dgvProductsCatalog.Location = New System.Drawing.Point(3, 20)
            Me.dgvProductsCatalog.MultiSelect = False
            Me.dgvProductsCatalog.Name = "dgvProductsCatalog"
            Me.dgvProductsCatalog.ReadOnly = True
            Me.dgvProductsCatalog.RowHeadersVisible = False
            Me.dgvProductsCatalog.SelectionMode = System.Windows.Forms.DataGridViewSelectionMode.FullRowSelect
            Me.dgvProductsCatalog.Size = New System.Drawing.Size(744, 566)
            Me.dgvProductsCatalog.TabIndex = 0
            '
            'grpStockSimulation
            '
            Me.grpStockSimulation.Controls.Add(Me.lblStockGuardPolicy)
            Me.grpStockSimulation.Controls.Add(Me.lblSimExplanation)
            Me.grpStockSimulation.Controls.Add(Me.pnlSimBadgeContainer)
            Me.grpStockSimulation.Controls.Add(Me.btnRunSim)
            Me.grpStockSimulation.Controls.Add(Me.numSimQty)
            Me.grpStockSimulation.Controls.Add(Me.lblSimQtyPrompt)
            Me.grpStockSimulation.Controls.Add(Me.cmbSimProduct)
            Me.grpStockSimulation.Controls.Add(Me.lblSimPrompt)
            Me.grpStockSimulation.Dock = System.Windows.Forms.DockStyle.Fill
            Me.grpStockSimulation.Location = New System.Drawing.Point(0, 0)
            Me.grpStockSimulation.Name = "grpStockSimulation"
            Me.grpStockSimulation.Padding = New System.Windows.Forms.Padding(12)
            Me.grpStockSimulation.Size = New System.Drawing.Size(422, 589)
            Me.grpStockSimulation.TabIndex = 0
            Me.grpStockSimulation.TabStop = False
            Me.grpStockSimulation.Text = "Hard-Stop Invariant Simulation Gate"
            '
            'lblStockGuardPolicy
            '
            Me.lblStockGuardPolicy.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.lblStockGuardPolicy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblStockGuardPolicy.Dock = System.Windows.Forms.DockStyle.Bottom
            Me.lblStockGuardPolicy.ForeColor = System.Drawing.Color.FromArgb(CType(CType(73, Byte), Integer), CType(CType(80, Byte), Integer), CType(CType(87, Byte), Integer))
            Me.lblStockGuardPolicy.Location = New System.Drawing.Point(12, 459)
            Me.lblStockGuardPolicy.Name = "lblStockGuardPolicy"
            Me.lblStockGuardPolicy.Padding = New System.Windows.Forms.Padding(8)
            Me.lblStockGuardPolicy.Size = New System.Drawing.Size(398, 118)
            Me.lblStockGuardPolicy.TabIndex = 7
            Me.lblStockGuardPolicy.Text = "INVARIANT ENFORCEMENT RULES:" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "- Physical on-hand stock must never fall below" &
    " zero (CHECK TotalQtyOnHand >= 0)." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "- OrderToCashService throws InsufficientS" &
    "tockException when OrderQty > QtyOnHand." & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "- Disallowed dispatch blocks Sales I" &
    "nvoice generation atomically."
            '
            'lblSimExplanation
            '
            Me.lblSimExplanation.BackColor = System.Drawing.Color.White
            Me.lblSimExplanation.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
            Me.lblSimExplanation.ForeColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.lblSimExplanation.Location = New System.Drawing.Point(15, 260)
            Me.lblSimExplanation.Name = "lblSimExplanation"
            Me.lblSimExplanation.Padding = New System.Windows.Forms.Padding(8)
            Me.lblSimExplanation.Size = New System.Drawing.Size(392, 120)
            Me.lblSimExplanation.TabIndex = 6
            Me.lblSimExplanation.Text = "Select a product and quantity to verify the real-time stock guard."
            '
            'pnlSimBadgeContainer
            '
            Me.pnlSimBadgeContainer.Controls.Add(Me.lblSimBadge)
            Me.pnlSimBadgeContainer.Location = New System.Drawing.Point(15, 204)
            Me.pnlSimBadgeContainer.Name = "pnlSimBadgeContainer"
            Me.pnlSimBadgeContainer.Size = New System.Drawing.Size(392, 46)
            Me.pnlSimBadgeContainer.TabIndex = 5
            '
            'lblSimBadge
            '
            Me.lblSimBadge.BackColor = System.Drawing.Color.FromArgb(CType(CType(108, Byte), Integer), CType(CType(117, Byte), Integer), CType(CType(125, Byte), Integer))
            Me.lblSimBadge.Dock = System.Windows.Forms.DockStyle.Fill
            Me.lblSimBadge.Font = New System.Drawing.Font("Segoe UI", 10.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.lblSimBadge.ForeColor = System.Drawing.Color.White
            Me.lblSimBadge.Location = New System.Drawing.Point(0, 0)
            Me.lblSimBadge.Name = "lblSimBadge"
            Me.lblSimBadge.Size = New System.Drawing.Size(392, 46)
            Me.lblSimBadge.TabIndex = 0
            Me.lblSimBadge.Text = "READY TO SIMULATE"
            Me.lblSimBadge.TextAlign = System.Drawing.ContentAlignment.MiddleCenter
            '
            'btnRunSim
            '
            Me.btnRunSim.BackColor = System.Drawing.Color.FromArgb(CType(CType(33, Byte), Integer), CType(CType(37, Byte), Integer), CType(CType(41, Byte), Integer))
            Me.btnRunSim.FlatStyle = System.Windows.Forms.FlatStyle.Flat
            Me.btnRunSim.Font = New System.Drawing.Font("Segoe UI", 9.5!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
            Me.btnRunSim.ForeColor = System.Drawing.Color.White
            Me.btnRunSim.Location = New System.Drawing.Point(15, 150)
            Me.btnRunSim.Name = "btnRunSim"
            Me.btnRunSim.Size = New System.Drawing.Size(392, 40)
            Me.btnRunSim.TabIndex = 4
            Me.btnRunSim.Text = "Evaluate Stock Guard Invariant"
            Me.btnRunSim.UseVisualStyleBackColor = False
            '
            'numSimQty
            '
            Me.numSimQty.Location = New System.Drawing.Point(15, 110)
            Me.numSimQty.Maximum = New Decimal(New Integer() {100000, 0, 0, 0})
            Me.numSimQty.Minimum = New Decimal(New Integer() {1, 0, 0, 0})
            Me.numSimQty.Name = "numSimQty"
            Me.numSimQty.Size = New System.Drawing.Size(392, 24)
            Me.numSimQty.TabIndex = 3
            Me.numSimQty.Value = New Decimal(New Integer() {1, 0, 0, 0})
            '
            'lblSimQtyPrompt
            '
            Me.lblSimQtyPrompt.AutoSize = True
            Me.lblSimQtyPrompt.Location = New System.Drawing.Point(12, 90)
            Me.lblSimQtyPrompt.Name = "lblSimQtyPrompt"
            Me.lblSimQtyPrompt.Size = New System.Drawing.Size(185, 17)
            Me.lblSimQtyPrompt.TabIndex = 2
            Me.lblSimQtyPrompt.Text = "Simulate Dispatch Quantity:"
            '
            'cmbSimProduct
            '
            Me.cmbSimProduct.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
            Me.cmbSimProduct.FormattingEnabled = True
            Me.cmbSimProduct.Location = New System.Drawing.Point(15, 52)
            Me.cmbSimProduct.Name = "cmbSimProduct"
            Me.cmbSimProduct.Size = New System.Drawing.Size(392, 25)
            Me.cmbSimProduct.TabIndex = 1
            '
            'lblSimPrompt
            '
            Me.lblSimPrompt.AutoSize = True
            Me.lblSimPrompt.Location = New System.Drawing.Point(12, 32)
            Me.lblSimPrompt.Name = "lblSimPrompt"
            Me.lblSimPrompt.Size = New System.Drawing.Size(161, 17)
            Me.lblSimPrompt.TabIndex = 0
            Me.lblSimPrompt.Text = "Select Product to Test:"
            '
            'SalesDashboardForm
            '
            Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(248, Byte), Integer), CType(CType(249, Byte), Integer), CType(CType(250, Byte), Integer))
            Me.ClientSize = New System.Drawing.Size(1200, 720)
            Me.Controls.Add(Me.tabMain)
            Me.Controls.Add(Me.pnlHeader)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
            Me.Name = "SalesDashboardForm"
            Me.Text = "Sales Dashboard & Order Fulfillment"
            Me.pnlHeader.ResumeLayout(False)
            Me.pnlHeader.PerformLayout()
            Me.pnlKpiContainer.ResumeLayout(False)
            Me.pnlKpiStockGuard.ResumeLayout(False)
            Me.pnlKpiInvoices.ResumeLayout(False)
            Me.pnlKpiOrders.ResumeLayout(False)
            Me.pnlKpiLowStock.ResumeLayout(False)
            Me.tabMain.ResumeLayout(False)
            Me.tabOrders.ResumeLayout(False)
            Me.splitOrders.Panel1.ResumeLayout(False)
            Me.splitOrders.Panel2.ResumeLayout(False)
            CType(Me.splitOrders, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitOrders.ResumeLayout(False)
            Me.grpOrderLines.ResumeLayout(False)
            CType(Me.dgvCurrentLines, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlOrderLineActions.ResumeLayout(False)
            Me.grpAddItem.ResumeLayout(False)
            Me.grpAddItem.PerformLayout()
            CType(Me.numUnitPrice, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.numQuantity, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tblCustomerCredit.ResumeLayout(False)
            Me.tblCustomerCredit.PerformLayout()
            Me.grpCustomer.ResumeLayout(False)
            Me.grpCustomer.PerformLayout()
            Me.grpSelectedOrderLines.ResumeLayout(False)
            CType(Me.dgvSelectedOrderLines, System.ComponentModel.ISupportInitialize).EndInit()
            Me.pnlOrderActions.ResumeLayout(False)
            Me.grpExistingOrders.ResumeLayout(False)
            CType(Me.dgvExistingOrders, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabInvoices.ResumeLayout(False)
            Me.splitInvoices.Panel1.ResumeLayout(False)
            Me.splitInvoices.Panel2.ResumeLayout(False)
            CType(Me.splitInvoices, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitInvoices.ResumeLayout(False)
            Me.grpInvoicesList.ResumeLayout(False)
            CType(Me.dgvInvoices, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitInvoiceDetails.Panel1.ResumeLayout(False)
            Me.splitInvoiceDetails.Panel2.ResumeLayout(False)
            CType(Me.splitInvoiceDetails, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitInvoiceDetails.ResumeLayout(False)
            Me.grpInvoiceLines.ResumeLayout(False)
            CType(Me.dgvInvoiceLines, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpJournalPreview.ResumeLayout(False)
            CType(Me.dgvJournalPreview, System.ComponentModel.ISupportInitialize).EndInit()
            Me.tabStockMonitor.ResumeLayout(False)
            Me.splitStock.Panel1.ResumeLayout(False)
            Me.splitStock.Panel2.ResumeLayout(False)
            CType(Me.splitStock, System.ComponentModel.ISupportInitialize).EndInit()
            Me.splitStock.ResumeLayout(False)
            Me.grpAllProducts.ResumeLayout(False)
            CType(Me.dgvProductsCatalog, System.ComponentModel.ISupportInitialize).EndInit()
            Me.grpStockSimulation.ResumeLayout(False)
            Me.grpStockSimulation.PerformLayout()
            Me.pnlSimBadgeContainer.ResumeLayout(False)
            CType(Me.numSimQty, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)

        End Sub

    End Class

End Namespace
