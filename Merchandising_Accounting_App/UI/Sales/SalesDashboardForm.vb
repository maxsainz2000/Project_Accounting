Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Linq
Imports System.Windows.Forms

Namespace UI.Sales

    Public Class SalesDashboardForm

        Friend ReadOnly _dataService As New SalesDataService()
        Private _currentDraftOrder As SalesOrderViewModel
        Private _selectedExistingOrder As SalesOrderViewModel

        Private Sub SalesDashboardForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            InitializeDraftOrder()
            SetupGrids()
            LoadCustomers()
            LoadProducts()
            RefreshExistingOrdersGrid()
            RefreshInvoicesGrid()
            RefreshProductsCatalogGrid()
            UpdateKpiCards()
            SalesDashboardForm_Resize(Nothing, EventArgs.Empty)
        End Sub

        Private Sub SalesDashboardForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
            If splitOrders IsNot Nothing AndAlso splitOrders.Width > 0 Then
                Dim targetDistance As Integer = CInt(Math.Floor(splitOrders.Width * 0.49))
                Dim minDistance As Integer = Math.Max(splitOrders.Panel1MinSize, 520)
                Dim maxDistance As Integer = splitOrders.Width - splitOrders.Panel2MinSize - splitOrders.SplitterWidth
                If maxDistance >= minDistance Then
                    targetDistance = Math.Clamp(targetDistance, minDistance, maxDistance)
                    If splitOrders.SplitterDistance <> targetDistance Then
                        splitOrders.SplitterDistance = targetDistance
                    End If
                End If
            End If
            If pnlOrderActions IsNot Nothing AndAlso btnGenerateInvoiceFromSO IsNot Nothing Then
                btnGenerateInvoiceFromSO.Left = 8
                btnGenerateInvoiceFromSO.Width = Math.Max(320, pnlOrderActions.ClientSize.Width - 16)
            End If
        End Sub

        Private Sub tabMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabMain.SelectedIndexChanged
            If tabMain.SelectedTab Is tabInvoices Then
                dgvInvoices.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
                dgvInvoiceLines.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
                dgvJournalPreview.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
            ElseIf tabMain.SelectedTab Is tabStockMonitor Then
                dgvProductsCatalog.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
            End If
        End Sub

        Private Sub InitializeDraftOrder()
            _currentDraftOrder = New SalesOrderViewModel() With {
                .OrderDate = DateTime.Today,
                .Status = "Draft"
            }
            RefreshCurrentDraftGrid()
        End Sub

        Private Sub SetupGrids()
            ' Setup dgvCurrentLines
            dgvCurrentLines.AutoGenerateColumns = False
            dgvCurrentLines.ScrollBars = ScrollBars.Both
            dgvCurrentLines.Columns.Clear()
            dgvCurrentLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "SKU", .HeaderText = "SKU", .FillWeight = 20.0!, .MinimumWidth = 90})
            dgvCurrentLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "ProductName", .HeaderText = "Product Name", .FillWeight = 50.0!, .MinimumWidth = 240})
            dgvCurrentLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "OrderedQty", .HeaderText = "Qty", .FillWeight = 12.0!, .MinimumWidth = 60, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvCurrentLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "UnitPrice", .HeaderText = "Price", .FillWeight = 15.0!, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvCurrentLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "LineTotal", .HeaderText = "Total", .FillWeight = 18.0!, .MinimumWidth = 95, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})

            ' Setup dgvExistingOrders
            dgvExistingOrders.AutoGenerateColumns = False
            dgvExistingOrders.ScrollBars = ScrollBars.Both
            dgvExistingOrders.Columns.Clear()
            dgvExistingOrders.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "DocumentNumber", .HeaderText = "Order #", .FillWeight = 20.0!, .MinimumWidth = 95})
            dgvExistingOrders.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "CustomerName", .HeaderText = "Customer", .FillWeight = 35.0!, .MinimumWidth = 140})
            dgvExistingOrders.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "OrderDate", .HeaderText = "Date", .FillWeight = 18.0!, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "yyyy-MM-dd", .Alignment = DataGridViewContentAlignment.MiddleCenter}})
            dgvExistingOrders.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "TotalAmount", .HeaderText = "Total Amount", .FillWeight = 20.0!, .MinimumWidth = 100, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvExistingOrders.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "Status", .HeaderText = "Status", .FillWeight = 17.0!, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}})

            ' Setup dgvSelectedOrderLines
            dgvSelectedOrderLines.AutoGenerateColumns = False
            dgvSelectedOrderLines.ScrollBars = ScrollBars.Both
            dgvSelectedOrderLines.Columns.Clear()
            dgvSelectedOrderLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "SKU", .HeaderText = "SKU", .FillWeight = 20.0!, .MinimumWidth = 90})
            dgvSelectedOrderLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "ProductName", .HeaderText = "Product", .FillWeight = 50.0!, .MinimumWidth = 240})
            dgvSelectedOrderLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "OrderedQty", .HeaderText = "Qty", .FillWeight = 12.0!, .MinimumWidth = 60, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvSelectedOrderLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "UnitPrice", .HeaderText = "Unit Price", .FillWeight = 15.0!, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvSelectedOrderLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "LineTotal", .HeaderText = "Total", .FillWeight = 18.0!, .MinimumWidth = 95, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvSelectedOrderLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "StockBadgeText", .HeaderText = "Stock Check", .FillWeight = 25.0!, .MinimumWidth = 100, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}})

            ' Setup dgvInvoices
            dgvInvoices.AutoGenerateColumns = False
            dgvInvoices.ScrollBars = ScrollBars.Both
            dgvInvoices.Columns.Clear()
            dgvInvoices.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "DocumentNumber", .HeaderText = "Invoice #", .FillWeight = 15.0!, .MinimumWidth = 95})
            dgvInvoices.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "SODocumentNumber", .HeaderText = "SO Ref", .FillWeight = 15.0!, .MinimumWidth = 85})
            dgvInvoices.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "CustomerName", .HeaderText = "Customer", .FillWeight = 30.0!, .MinimumWidth = 140})
            dgvInvoices.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "InvoiceDate", .HeaderText = "Invoice Date", .FillWeight = 15.0!, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "yyyy-MM-dd", .Alignment = DataGridViewContentAlignment.MiddleCenter}})
            dgvInvoices.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "TotalAmount", .HeaderText = "Invoiced Amount", .FillWeight = 18.0!, .MinimumWidth = 105, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvInvoices.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "TotalCOGS", .HeaderText = "Total COGS", .FillWeight = 16.0!, .MinimumWidth = 100, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvInvoices.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "GrossMargin", .HeaderText = "Gross Margin", .FillWeight = 16.0!, .MinimumWidth = 100, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvInvoices.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "GrossMarginPct", .HeaderText = "Margin %", .FillWeight = 13.0!, .MinimumWidth = 75, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "0.0'%'", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvInvoices.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "Status", .HeaderText = "GL Status", .FillWeight = 14.0!, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}})

            ' Setup dgvInvoiceLines
            dgvInvoiceLines.AutoGenerateColumns = False
            dgvInvoiceLines.ScrollBars = ScrollBars.Both
            dgvInvoiceLines.Columns.Clear()
            dgvInvoiceLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "SKU", .HeaderText = "SKU", .FillWeight = 18.0!, .MinimumWidth = 90})
            dgvInvoiceLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "ProductName", .HeaderText = "Product Name", .FillWeight = 50.0!, .MinimumWidth = 240})
            dgvInvoiceLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "FulfilledQty", .HeaderText = "Fulfilled Qty", .FillWeight = 12.0!, .MinimumWidth = 65, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvInvoiceLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "UnitPrice", .HeaderText = "Selling Price", .FillWeight = 15.0!, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvInvoiceLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "COGSUnitCost", .HeaderText = "Avg Unit Cost", .FillWeight = 15.0!, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvInvoiceLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "LineTotal", .HeaderText = "Revenue", .FillWeight = 16.0!, .MinimumWidth = 95, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvInvoiceLines.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "LineMargin", .HeaderText = "Margin", .FillWeight = 15.0!, .MinimumWidth = 90, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})

            ' Setup dgvJournalPreview
            dgvJournalPreview.AutoGenerateColumns = False
            dgvJournalPreview.ScrollBars = ScrollBars.Both
            dgvJournalPreview.Columns.Clear()
            dgvJournalPreview.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "AccountCode", .HeaderText = "Account", .FillWeight = 12.0!, .MinimumWidth = 65, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}})
            dgvJournalPreview.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "AccountName", .HeaderText = "Account Title", .FillWeight = 28.0!, .MinimumWidth = 150})
            dgvJournalPreview.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "DebitAmount", .HeaderText = "Debit ($)", .FillWeight = 18.0!, .MinimumWidth = 95, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvJournalPreview.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "CreditAmount", .HeaderText = "Credit ($)", .FillWeight = 18.0!, .MinimumWidth = 95, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvJournalPreview.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "Explanation", .HeaderText = "Posting Explanation", .FillWeight = 34.0!, .MinimumWidth = 160})

            ' Setup dgvProductsCatalog
            dgvProductsCatalog.AutoGenerateColumns = False
            dgvProductsCatalog.ScrollBars = ScrollBars.Both
            dgvProductsCatalog.Columns.Clear()
            dgvProductsCatalog.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "SKU", .HeaderText = "SKU", .FillWeight = 15.0!, .MinimumWidth = 90})
            dgvProductsCatalog.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "Name", .HeaderText = "Product Description", .FillWeight = 45.0!, .MinimumWidth = 240})
            dgvProductsCatalog.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "Category", .HeaderText = "Category", .FillWeight = 15.0!, .MinimumWidth = 90})
            dgvProductsCatalog.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "QtyOnHand", .HeaderText = "On Hand", .FillWeight = 12.0!, .MinimumWidth = 70, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvProductsCatalog.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "QtyReserved", .HeaderText = "Reserved", .FillWeight = 12.0!, .MinimumWidth = 70, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvProductsCatalog.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "QtyAvailable", .HeaderText = "Free Qty", .FillWeight = 12.0!, .MinimumWidth = 70, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvProductsCatalog.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "ReorderLevel", .HeaderText = "Min Level", .FillWeight = 12.0!, .MinimumWidth = 70, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvProductsCatalog.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "MovingAvgCost", .HeaderText = "Avg Cost", .FillWeight = 14.0!, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvProductsCatalog.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "UnitPrice", .HeaderText = "List Price", .FillWeight = 14.0!, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}})
            dgvProductsCatalog.Columns.Add(New DataGridViewTextBoxColumn() With {.DataPropertyName = "StockBadgeText", .HeaderText = "Availability Status", .FillWeight = 20.0!, .MinimumWidth = 110, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}})
        End Sub

        Private Sub LoadCustomers()
            cmbCustomer.DisplayMember = "DisplayText"
            cmbCustomer.ValueMember = "CustomerId"
            cmbCustomer.DataSource = _dataService.Customers.ToList()
            If cmbCustomer.Items.Count > 0 Then
                cmbCustomer.SelectedIndex = 0
                UpdateCustomerInfo()
            End If
        End Sub

        Private Sub LoadProducts()
            cmbProduct.DisplayMember = "DisplayText"
            cmbProduct.ValueMember = "ProductId"
            cmbProduct.DataSource = _dataService.Products.ToList()

            cmbSimProduct.DisplayMember = "DisplayText"
            cmbSimProduct.ValueMember = "ProductId"
            cmbSimProduct.DataSource = _dataService.Products.ToList()

            If cmbProduct.Items.Count > 0 Then
                cmbProduct.SelectedIndex = 0
                UpdateProductInfo()
            End If
        End Sub

        Private Sub UpdateCustomerInfo()
            Dim cust As CustomerViewModel = TryCast(cmbCustomer.SelectedItem, CustomerViewModel)
            If cust IsNot Nothing Then
                lblCreditLimitVal.Text = cust.CreditLimit.ToString("C2")
                lblCurrentARVal.Text = cust.CurrentARBalance.ToString("C2")
                lblAvailCreditVal.Text = cust.AvailableCredit.ToString("C2")

                If cust.AvailableCredit <= 0D Then
                    lblCustomerBadge.Text = "CREDIT EXCEEDED"
                    lblCustomerBadge.BackColor = Color.FromArgb(220, 53, 69) ' Crimson Red
                ElseIf cust.AvailableCredit < (cust.CreditLimit * 0.15D) Then
                    lblCustomerBadge.Text = "NEAR CREDIT LIMIT"
                    lblCustomerBadge.BackColor = Color.FromArgb(253, 126, 20) ' Orange
                Else
                    lblCustomerBadge.Text = "CREDIT APPROVED"
                    lblCustomerBadge.BackColor = Color.FromArgb(40, 167, 69) ' Green
                End If
            End If
        End Sub

        Private Sub UpdateProductInfo()
            Dim prod As ProductStockViewModel = TryCast(cmbProduct.SelectedItem, ProductStockViewModel)
            If prod IsNot Nothing Then
                lblItemStockInfo.Text = $"On Hand: {prod.QtyOnHand} | Reserved: {prod.QtyReserved} | Free: {prod.QtyAvailable}"
                numUnitPrice.Value = prod.UnitPrice

                Dim reqQty As Integer = CInt(numQuantity.Value)
                Dim check As StockCheckResult = _dataService.EvaluateStockAvailability(prod, reqQty)

                lblStockBadgeItem.Text = check.BadgeText
                lblStockBadgeItem.BackColor = check.BadgeColor
                lblStockWarningNotice.Text = check.WarningMessage

                If Not check.IsPermitted Then
                    lblStockWarningNotice.BackColor = Color.FromArgb(248, 215, 218)
                    lblStockWarningNotice.ForeColor = Color.FromArgb(114, 28, 36)
                Else
                    lblStockWarningNotice.BackColor = Color.FromArgb(209, 231, 221)
                    lblStockWarningNotice.ForeColor = Color.FromArgb(15, 81, 50)
                End If
            End If
        End Sub

        Private Sub cmbCustomer_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbCustomer.SelectedIndexChanged
            UpdateCustomerInfo()
        End Sub

        Private Sub cmbProduct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbProduct.SelectedIndexChanged
            UpdateProductInfo()
        End Sub

        Private Sub numQuantity_ValueChanged(sender As Object, e As EventArgs) Handles numQuantity.ValueChanged
            UpdateProductInfo()
        End Sub

        Private Sub btnAddLineItem_Click(sender As Object, e As EventArgs) Handles btnAddLineItem.Click
            Dim prod As ProductStockViewModel = TryCast(cmbProduct.SelectedItem, ProductStockViewModel)
            If prod Is Nothing Then
                MessageBox.Show("Please select an item to add to the sales order.", "No Product Selected", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim qty As Integer = CInt(numQuantity.Value)
            Dim price As Decimal = numUnitPrice.Value

            ' Check if already in order lines
            Dim existingLine As SalesOrderLineViewModel = _currentDraftOrder.Lines.FirstOrDefault(Function(l) l.ProductId = prod.ProductId)
            Dim totalRequested As Integer = qty + If(existingLine IsNot Nothing, existingLine.OrderedQty, 0)

            ' Hard-stop zero negative stock gate test
            Dim check As StockCheckResult = _dataService.EvaluateStockAvailability(prod, totalRequested)
            If Not check.IsPermitted Then
                Dim dialogRes As DialogResult = MessageBox.Show(
                    $"{check.WarningMessage}{Environment.NewLine}{Environment.NewLine}Would you like to register this line anyway as a Draft back-order? (Note: Sales Invoice dispatch will be blocked by the Hard-Stop Invariant until fresh inventory is received via GRN.)",
                    "Stock Availability Notice",
                    MessageBoxButtons.YesNo,
                    MessageBoxIcon.Warning)

                If dialogRes <> DialogResult.Yes Then
                    Return
                End If
            End If

            If existingLine IsNot Nothing Then
                existingLine.OrderedQty = totalRequested
                existingLine.UnitPrice = price
                existingLine.AvailableAtOrder = prod.QtyAvailable
            Else
                Dim newLine As New SalesOrderLineViewModel() With {
                    .LineId = _currentDraftOrder.Lines.Count + 1,
                    .ProductId = prod.ProductId,
                    .SKU = prod.SKU,
                    .ProductName = prod.Name,
                    .OrderedQty = qty,
                    .UnitPrice = price,
                    .MovingAvgCost = prod.MovingAvgCost,
                    .AvailableAtOrder = prod.QtyAvailable
                }
                _currentDraftOrder.Lines.Add(newLine)
            End If

            RefreshCurrentDraftGrid()
        End Sub

        Private Sub btnRemoveLine_Click(sender As Object, e As EventArgs) Handles btnRemoveLine.Click
            If dgvCurrentLines.SelectedRows.Count > 0 Then
                Dim selectedLine As SalesOrderLineViewModel = TryCast(dgvCurrentLines.SelectedRows(0).DataBoundItem, SalesOrderLineViewModel)
                If selectedLine IsNot Nothing Then
                    _currentDraftOrder.Lines.Remove(selectedLine)
                    RefreshCurrentDraftGrid()
                End If
            End If
        End Sub

        Private Sub btnClearOrderForm_Click(sender As Object, e As EventArgs) Handles btnClearOrderForm.Click
            InitializeDraftOrder()
        End Sub

        Private Sub RefreshCurrentDraftGrid()
            dgvCurrentLines.DataSource = Nothing
            dgvCurrentLines.DataSource = _currentDraftOrder.Lines.ToList()
            lblOrderTotalDisplay.Text = $"Order Grand Total: {_currentDraftOrder.TotalAmount:C2} ({_currentDraftOrder.TotalQuantity} units)"
        End Sub

        Private Sub btnSaveOrderDraft_Click(sender As Object, e As EventArgs) Handles btnSaveOrderDraft.Click
            CommitOrderSubmission("Draft")
        End Sub

        Private Sub btnConfirmOrder_Click(sender As Object, e As EventArgs) Handles btnConfirmOrder.Click
            CommitOrderSubmission("Confirmed")
        End Sub

        Private Sub CommitOrderSubmission(status As String)
            Dim cust As CustomerViewModel = TryCast(cmbCustomer.SelectedItem, CustomerViewModel)
            If cust Is Nothing Then
                MessageBox.Show("Please choose a valid customer account.", "Customer Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If _currentDraftOrder.Lines.Count = 0 Then
                MessageBox.Show("The sales order must have at least one line item.", "Line Items Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            _currentDraftOrder.CustomerId = cust.CustomerId
            _currentDraftOrder.CustomerName = cust.Name
            _currentDraftOrder.Status = status
            _currentDraftOrder.OrderDate = DateTime.Today

            Dim registeredSO As SalesOrderViewModel = _dataService.RegisterSalesOrder(_currentDraftOrder)

            MessageBox.Show(
                $"Sales Order {registeredSO.DocumentNumber} successfully saved with status '{status}'.{Environment.NewLine}Customer: {registeredSO.CustomerName}{Environment.NewLine}Total: {registeredSO.TotalAmount.ToString("C2")}",
                "Order Saved",
                MessageBoxButtons.OK,
                MessageBoxIcon.Information)

            InitializeDraftOrder()
            RefreshExistingOrdersGrid()
            RefreshProductsCatalogGrid()
            UpdateKpiCards()
        End Sub

        Private Sub RefreshExistingOrdersGrid()
            dgvExistingOrders.DataSource = Nothing
            dgvExistingOrders.DataSource = _dataService.SalesOrders.ToList()

            If dgvExistingOrders.Rows.Count > 0 Then
                dgvExistingOrders.Rows(0).Selected = True
                UpdateSelectedOrderDetails()
            End If
        End Sub

        Private Sub dgvExistingOrders_SelectionChanged(sender As Object, e As EventArgs) Handles dgvExistingOrders.SelectionChanged
            UpdateSelectedOrderDetails()
        End Sub

        Private Sub UpdateSelectedOrderDetails()
            If dgvExistingOrders.SelectedRows.Count > 0 Then
                _selectedExistingOrder = TryCast(dgvExistingOrders.SelectedRows(0).DataBoundItem, SalesOrderViewModel)
                If _selectedExistingOrder IsNot Nothing Then
                    lblSelectedOrderSummary.Text = $"Selected Order: {_selectedExistingOrder.DocumentNumber} ({_selectedExistingOrder.CustomerName}){Environment.NewLine}Status: {_selectedExistingOrder.Status} | Amount: {_selectedExistingOrder.TotalAmount.ToString("C2")}"

                    dgvSelectedOrderLines.DataSource = Nothing
                    dgvSelectedOrderLines.DataSource = _selectedExistingOrder.Lines.ToList()

                    Dim isInvoiced As Boolean = String.Equals(_selectedExistingOrder.Status, "Invoiced", StringComparison.Ordinal)
                    btnGenerateInvoiceFromSO.Enabled = Not isInvoiced
                    If isInvoiced Then
                        btnGenerateInvoiceFromSO.Text = "✓ Already Invoiced & Fulfilled"
                        btnGenerateInvoiceFromSO.BackColor = Color.FromArgb(108, 117, 125)
                    Else
                        btnGenerateInvoiceFromSO.Text = "⚡ Fulfill & Generate Sales Invoice"
                        btnGenerateInvoiceFromSO.BackColor = Color.FromArgb(13, 110, 253)
                    End If
                End If
            Else
                _selectedExistingOrder = Nothing
                lblSelectedOrderSummary.Text = "Selected Order: None"
                dgvSelectedOrderLines.DataSource = Nothing
                btnGenerateInvoiceFromSO.Enabled = False
            End If
        End Sub

        Private Sub btnGenerateInvoiceFromSO_Click(sender As Object, e As EventArgs) Handles btnGenerateInvoiceFromSO.Click
            If _selectedExistingOrder Is Nothing Then
                MessageBox.Show("Please select an active Sales Order to fulfill.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim result As InvoiceGenerationResult = _dataService.GenerateSalesInvoice(
                _selectedExistingOrder.SOId,
                DateTime.Today,
                DateTime.Today)

            If Not result.Success Then
                MessageBox.Show(
                    $"FULFILLMENT REJECTED BY STOCK GUARD:{Environment.NewLine}{Environment.NewLine}{result.ErrorMessage}{Environment.NewLine}{Environment.NewLine}Accounting Rule Enforced: The ERP system strictly blocks dispatches that would cause physical on-hand stock to drop below zero.",
                    "Hard-Stop Invariant Violation",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Error)
                Return
            End If

            ' Display GL Double-Entry Posting Confirmation
            Dim glSummary As String = $"Sales Invoice {result.Invoice.DocumentNumber} posted successfully!{Environment.NewLine}{Environment.NewLine}" &
                $"--- AUTOMATIC DOUBLE-ENTRY POSTINGS ---{Environment.NewLine}" &
                $"Debit  1200 Accounts Receivable: {result.Invoice.TotalAmount.ToString("C2")}{Environment.NewLine}" &
                $"Credit 4000 Sales Revenue:       {result.Invoice.TotalAmount.ToString("C2")}{Environment.NewLine}" &
                $"Debit  5000 Cost of Goods Sold:  {result.Invoice.TotalCOGS.ToString("C2")}{Environment.NewLine}" &
                $"Credit 1300 Merchandise Inventory:{result.Invoice.TotalCOGS.ToString("C2")}{Environment.NewLine}{Environment.NewLine}" &
                $"Realized Gross Margin: {result.Invoice.GrossMargin.ToString("C2")} ({result.Invoice.GrossMarginPct:F2}%)"

            MessageBox.Show(glSummary, "Sales Invoice & GL Journal Posted", MessageBoxButtons.OK, MessageBoxIcon.Information)

            RefreshExistingOrdersGrid()
            RefreshInvoicesGrid()
            RefreshProductsCatalogGrid()
            UpdateCustomerInfo()
            UpdateKpiCards()

            ' Select invoices tab and focus newly created invoice
            tabMain.SelectedTab = tabInvoices
            SelectLatestInvoice()
        End Sub

        Private Sub RefreshInvoicesGrid()
            dgvInvoices.DataSource = Nothing
            dgvInvoices.DataSource = _dataService.SalesInvoices.ToList()

            If dgvInvoices.Rows.Count > 0 Then
                dgvInvoices.Rows(0).Selected = True
                UpdateSelectedInvoiceDetails()
            End If
        End Sub

        Private Sub SelectLatestInvoice()
            If dgvInvoices.Rows.Count > 0 Then
                dgvInvoices.Rows(0).Selected = True
                UpdateSelectedInvoiceDetails()
            End If
        End Sub

        Private Sub dgvInvoices_SelectionChanged(sender As Object, e As EventArgs) Handles dgvInvoices.SelectionChanged
            UpdateSelectedInvoiceDetails()
        End Sub

        Private Sub UpdateSelectedInvoiceDetails()
            If dgvInvoices.SelectedRows.Count > 0 Then
                Dim inv As SalesInvoiceViewModel = TryCast(dgvInvoices.SelectedRows(0).DataBoundItem, SalesInvoiceViewModel)
                If inv IsNot Nothing Then
                    dgvInvoiceLines.DataSource = Nothing
                    dgvInvoiceLines.DataSource = inv.Lines.ToList()

                    ' Build Balanced GL Preview Lines for Selected Invoice
                    Dim glLines As New List(Of JournalEntryPreviewViewModel)()
                    glLines.Add(New JournalEntryPreviewViewModel() With {
                        .AccountCode = "1200",
                        .AccountName = "Accounts Receivable",
                        .DebitAmount = inv.TotalAmount,
                        .CreditAmount = 0D,
                        .Explanation = $"Customer billable receivable for {inv.CustomerName}"
                    })
                    glLines.Add(New JournalEntryPreviewViewModel() With {
                        .AccountCode = "4000",
                        .AccountName = "Sales Revenue",
                        .DebitAmount = 0D,
                        .CreditAmount = inv.TotalAmount,
                        .Explanation = $"Revenue recognized upon delivery dispatch ({inv.DocumentNumber})"
                    })
                    glLines.Add(New JournalEntryPreviewViewModel() With {
                        .AccountCode = "5000",
                        .AccountName = "Cost of Goods Sold (COGS)",
                        .DebitAmount = inv.TotalCOGS,
                        .CreditAmount = 0D,
                        .Explanation = $"Relief of perpetual merchandise at moving average cost"
                    })
                    glLines.Add(New JournalEntryPreviewViewModel() With {
                        .AccountCode = "1300",
                        .AccountName = "Merchandise Inventory",
                        .DebitAmount = 0D,
                        .CreditAmount = inv.TotalCOGS,
                        .Explanation = $"Physical inventory relieved from warehouse stock"
                    })

                    dgvJournalPreview.DataSource = Nothing
                    dgvJournalPreview.DataSource = glLines

                    lblGlNotice.Text = $"Posting Verified: Debits {inv.TotalAmount + inv.TotalCOGS:C2} = Credits {inv.TotalAmount + inv.TotalCOGS:C2} | Difference: $0.0000"
                End If
            Else
                dgvInvoiceLines.DataSource = Nothing
                dgvJournalPreview.DataSource = Nothing
                lblGlNotice.Text = "No invoice selected."
            End If
        End Sub

        Private Sub RefreshProductsCatalogGrid()
            dgvProductsCatalog.DataSource = Nothing
            dgvProductsCatalog.DataSource = _dataService.Products.ToList()
            HighlightProductGridBadges()
        End Sub

        Private Sub HighlightProductGridBadges()
            For Each row As DataGridViewRow In dgvProductsCatalog.Rows
                Dim prod As ProductStockViewModel = TryCast(row.DataBoundItem, ProductStockViewModel)
                If prod IsNot Nothing Then
                    Dim statusCell As DataGridViewCell = row.Cells(9) ' Status column
                    Select Case prod.AvailabilityLevel
                        Case StockAvailabilityLevel.InsufficientStock
                            statusCell.Style.BackColor = Color.FromArgb(248, 215, 218)
                            statusCell.Style.ForeColor = Color.FromArgb(114, 28, 36)
                        Case StockAvailabilityLevel.LowStock
                            statusCell.Style.BackColor = Color.FromArgb(255, 243, 205)
                            statusCell.Style.ForeColor = Color.FromArgb(133, 100, 4)
                        Case Else
                            statusCell.Style.BackColor = Color.FromArgb(209, 231, 221)
                            statusCell.Style.ForeColor = Color.FromArgb(15, 81, 50)
                    End Select
                End If
            Next
        End Sub

        Private Sub btnRunSim_Click(sender As Object, e As EventArgs) Handles btnRunSim.Click
            Dim prod As ProductStockViewModel = TryCast(cmbSimProduct.SelectedItem, ProductStockViewModel)
            If prod Is Nothing Then
                MessageBox.Show("Please select a product to simulate.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim qty As Integer = CInt(numSimQty.Value)
            Dim check As StockCheckResult = _dataService.EvaluateStockAvailability(prod, qty)

            lblSimBadge.Text = check.BadgeText
            lblSimBadge.BackColor = check.BadgeColor

            If check.IsPermitted Then
                lblSimExplanation.ForeColor = Color.FromArgb(15, 81, 50)
                lblSimExplanation.Text = $"VALIDATION PASSED:{Environment.NewLine}" &
                    $"Physical On Hand: {prod.QtyOnHand} units{Environment.NewLine}" &
                    $"Simulated Dispatch: {qty} units{Environment.NewLine}" &
                    $"Post-fulfillment Remaining: {prod.QtyOnHand - qty} units{Environment.NewLine}" &
                    $"Outcome: Sales Invoice generation is permitted. Negative inventory prevented."
            Else
                lblSimExplanation.ForeColor = Color.FromArgb(114, 28, 36)
                lblSimExplanation.Text = $"HARD-STOP INVARIANT BLOCKED:{Environment.NewLine}" &
                    $"Physical On Hand: {prod.QtyOnHand} units{Environment.NewLine}" &
                    $"Requested Dispatch: {qty} units{Environment.NewLine}" &
                    $"Shortfall / Deficit: {check.ShortfallQty} units{Environment.NewLine}" &
                    $"Outcome: OrderToCashService throws InsufficientStockException. SQLite CHECK (TotalQtyOnHand >= 0) blocks dispatch atomically."
            End If
        End Sub

        Private Sub UpdateKpiCards()
            Dim totalOrdersVal As Decimal = 0D
            Dim openOrdersCount As Integer = 0
            For Each so As SalesOrderViewModel In _dataService.SalesOrders
                If Not String.Equals(so.Status, "Invoiced", StringComparison.Ordinal) Then
                    totalOrdersVal += so.TotalAmount
                    openOrdersCount += 1
                End If
            Next
            lblKpiOrdersValue.Text = $"{openOrdersCount} Orders ({totalOrdersVal:C2})"

            Dim totalInvoicedVal As Decimal = 0D
            For Each inv As SalesInvoiceViewModel In _dataService.SalesInvoices
                totalInvoicedVal += inv.TotalAmount
            Next
            lblKpiInvoicesValue.Text = totalInvoicedVal.ToString("C2")

            Dim lowStockCount As Integer = 0
            For Each prod In _dataService.Products
                If prod.QtyAvailable <= 10 Then
                    lowStockCount += 1
                End If
            Next
            If lblKpiLowStockValue IsNot Nothing Then
                lblKpiLowStockValue.Text = $"{lowStockCount} Items"
            End If
        End Sub

    End Class

End Namespace
