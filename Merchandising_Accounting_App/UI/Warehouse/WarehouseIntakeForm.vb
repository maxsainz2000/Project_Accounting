Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Collections.Generic
Imports Merchandising_Accounting_App.Domain.Models

Namespace UI.Warehouse

    Public Class WarehouseIntakeForm

        Friend ReadOnly _repo As New WarehouseDataService()
        Private _selectedPO As WarehousePOModel
        Private _selectedPOLine As WarehousePOLineModel
        Private _selectedRMA As WarehouseRMAModel
        Private ReadOnly _pendingAdjustments As New List(Of WarehouseAdjustmentLineModel)()

        Private Sub WarehouseIntakeForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            SetupOpenPOsGrid()
            SetupPOLinesGrid()
            SetupGRNJournalGrid()
            SetupQuarantineGrid()
            SetupRMAJournalGrid()
            SetupStockAdjustmentsGrid()

            PopulateIntakeProducts()
            PopulateAdjustmentProducts()
            RefreshDockReceivingView()
            RefreshQuarantineView()
            RefreshAdjustmentsView()
            WarehouseIntakeForm_Resize(Nothing, EventArgs.Empty)
        End Sub

        Private Sub SetupOpenPOsGrid()
            dgvOpenPOs.Columns.Clear()
            dgvOpenPOs.AutoGenerateColumns = False
            dgvOpenPOs.ScrollBars = ScrollBars.Both

            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "PO Number",
                .DataPropertyName = "DocumentNumber",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .Width = 95,
                .MinimumWidth = 85
            })
            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Vendor",
                .DataPropertyName = "VendorName",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                .FillWeight = 100.0!,
                .MinimumWidth = 80
            })
            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Date",
                .DataPropertyName = "OrderDate",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "yyyy-MM-dd", .Alignment = DataGridViewContentAlignment.MiddleCenter},
                .Width = 75,
                .MinimumWidth = 65
            })
            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Status",
                .DataPropertyName = "Status",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .Width = 60,
                .MinimumWidth = 50,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
            })
            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Total ($)",
                .DataPropertyName = "TotalAmount",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 70,
                .MinimumWidth = 60
            })
            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Pending Qty",
                .DataPropertyName = "TotalPendingQty",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 60,
                .MinimumWidth = 50
            })
        End Sub

        Private Sub SetupPOLinesGrid()
            dgvPOLines.Columns.Clear()
            dgvPOLines.AutoGenerateColumns = False
            dgvPOLines.ScrollBars = ScrollBars.Both

            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "SKU",
                .DataPropertyName = "SKU",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .Width = 90,
                .MinimumWidth = 75
            })
            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Product Name",
                .DataPropertyName = "ProductName",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                .FillWeight = 120.0!,
                .MinimumWidth = 90
            })
            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Ordered",
                .DataPropertyName = "OrderedQty",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 60,
                .MinimumWidth = 50
            })
            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Received",
                .DataPropertyName = "ReceivedQty",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 60,
                .MinimumWidth = 50
            })
            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Pending",
                .DataPropertyName = "PendingQty",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 60,
                .MinimumWidth = 50
            })
            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Unit Cost",
                .DataPropertyName = "POUnitPrice",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 75,
                .MinimumWidth = 65
            })
        End Sub

        Private Sub SetupGRNJournalGrid()
            dgvGRNJournal.Columns.Clear()
            dgvGRNJournal.AutoGenerateColumns = False
            dgvGRNJournal.ScrollBars = ScrollBars.Both

            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Doc #", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 115, .MinimumWidth = 100})
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Posting Date", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 90, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}})
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Account", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 95, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}})
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Account Name", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 240, .MinimumWidth = 180})
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Debit ($)",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 100,
                .MinimumWidth = 95
            })
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Credit ($)",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 100,
                .MinimumWidth = 95
            })
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Memo / Line Reference", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .MinimumWidth = 160})
        End Sub

        Private Sub SetupQuarantineGrid()
            dgvQuarantineItems.Columns.Clear()
            dgvQuarantineItems.AutoGenerateColumns = False
            dgvQuarantineItems.ScrollBars = ScrollBars.Both

            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "RMA #", .DataPropertyName = "DocumentNumber", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 80, .MinimumWidth = 70})
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Customer",
                .DataPropertyName = "CustomerName",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                .FillWeight = 110.0!,
                .MinimumWidth = 85
            })
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Orig Invoice", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .DataPropertyName = "OriginalInvoiceNumber", .Width = 75, .MinimumWidth = 65})
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "SKU", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .DataPropertyName = "SKU", .Width = 70, .MinimumWidth = 60})
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "In Quarantine",
                .DataPropertyName = "QuarantineQty",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 65,
                .MinimumWidth = 55
            })
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Restocked",
                .DataPropertyName = "RestockedQty",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 55,
                .MinimumWidth = 45
            })
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Scrapped",
                .DataPropertyName = "ScrappedQty",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 55,
                .MinimumWidth = 45
            })
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Status",
                .DataPropertyName = "Status",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .Width = 65,
                .MinimumWidth = 55,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}
            })
        End Sub

        Private Sub SetupRMAJournalGrid()
            dgvRMAJournal.Columns.Clear()
            dgvRMAJournal.AutoGenerateColumns = False
            dgvRMAJournal.ScrollBars = ScrollBars.Both

            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Credit Note #", .DataPropertyName = "DocumentNumber", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 85, .MinimumWidth = 70})
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Posting Date", .DataPropertyName = "PostingDate", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 70, .MinimumWidth = 60, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "yyyy-MM-dd", .Alignment = DataGridViewContentAlignment.MiddleCenter}})
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Account", .DataPropertyName = "AccountCode", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 65, .MinimumWidth = 55, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}})
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Account Name", .DataPropertyName = "AccountName", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 130, .MinimumWidth = 100})
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Debit ($)",
                .DataPropertyName = "Debit",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 65,
                .MinimumWidth = 55
            })
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Credit ($)",
                .DataPropertyName = "Credit",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 65,
                .MinimumWidth = 55
            })
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Memo / Line Reference", .DataPropertyName = "LineMemo", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .FillWeight = 110.0!, .MinimumWidth = 85})
        End Sub

        Private Sub PopulateIntakeProducts()
            cmbIntakeProduct.Items.Clear()
            For Each prod In _repo.Products
                cmbIntakeProduct.Items.Add(prod)
            Next
            cmbIntakeProduct.DisplayMember = "DisplayText"
            If cmbIntakeProduct.Items.Count > 0 Then
                cmbIntakeProduct.SelectedIndex = 0
            End If
        End Sub

        Private Sub RefreshDockReceivingView()
            Dim poList As New List(Of WarehousePOModel)(_repo.PurchaseOrders)
            dgvOpenPOs.DataSource = Nothing
            dgvOpenPOs.DataSource = poList

            If poList.Count > 0 Then
                dgvOpenPOs.Rows(0).Selected = True
                _selectedPO = poList(0)
                BindPOLines(_selectedPO)
            Else
                dgvPOLines.DataSource = Nothing
                _selectedPO = Nothing
                _selectedPOLine = Nothing
            End If

            RefreshGRNJournalGrid()
        End Sub

        Private Sub BindPOLines(po As WarehousePOModel)
            If po Is Nothing Then
                dgvPOLines.DataSource = Nothing
                Return
            End If

            Dim lines As New List(Of WarehousePOLineModel)(po.Lines)
            dgvPOLines.DataSource = Nothing
            dgvPOLines.DataSource = lines

            If lines.Count > 0 Then
                dgvPOLines.Rows(0).Selected = True
                _selectedPOLine = lines(0)
                UpdateReceiveQtyControls()
            Else
                _selectedPOLine = Nothing
            End If
        End Sub

        Private Sub UpdateReceiveQtyControls()
            If _selectedPOLine IsNot Nothing Then
                Dim pending As Integer = _selectedPOLine.PendingQty
                If pending > 0 Then
                    numReceiveQty.Maximum = CDec(pending)
                    numReceiveQty.Value = CDec(pending)
                    numReceiveQty.Enabled = True
                    btnReceiveGRN.Enabled = True

                    Dim prod = _repo.Products.Find(Function(p) p.ProductId = _selectedPOLine.ProductId)
                    If prod IsNot Nothing Then
                        Dim curQty As Integer = prod.TotalQtyOnHand
                        Dim curCost As Decimal = prod.CurrentUnitCost
                        Dim recQty As Integer = pending
                        Dim newQty As Integer = curQty + recQty
                        Dim blendedCost As Decimal = Math.Round(((CDec(curQty) * curCost) + (CDec(recQty) * _selectedPOLine.POUnitPrice)) / CDec(newQty), 4)

                        lblCostImpactPreview.Text = $"Stock Impact: {prod.SKU} Qty: {curQty} -> {newQty} | Cost: ${curCost:N2} -> ${blendedCost:N2} (Dr 1300/Cr 2100)"
                    End If
                Else
                    numReceiveQty.Maximum = 0D
                    numReceiveQty.Value = 0D
                    numReceiveQty.Enabled = False
                    btnReceiveGRN.Enabled = False
                    lblCostImpactPreview.Text = "Line already fully received."
                End If
            End If
        End Sub

        Private Sub RefreshGRNJournalGrid()
            dgvGRNJournal.Rows.Clear()
            For Each entry In _repo.JournalHistory
                If entry.SourceDocType = "GoodsReceiptNote" Then
                    For Each line In entry.Lines
                        dgvGRNJournal.Rows.Add(
                            entry.DocumentNumber,
                            entry.PostingDate.ToString("yyyy-MM-dd"),
                            line.AccountCode,
                            line.AccountName,
                            If(line.Debit > 0D, line.Debit.ToString("C2"), "-"),
                            If(line.Credit > 0D, line.Credit.ToString("C2"), "-"),
                            line.LineMemo
                        )
                    Next
                End If
            Next
        End Sub

        Private Sub RefreshQuarantineView()
            Dim rmaList As New List(Of WarehouseRMAModel)(_repo.RMAList)
            dgvQuarantineItems.DataSource = Nothing
            dgvQuarantineItems.DataSource = rmaList

            If rmaList.Count > 0 Then
                dgvQuarantineItems.Rows(0).Selected = True
                _selectedRMA = rmaList(0)
                UpdateInspectionControls()
            Else
                _selectedRMA = Nothing
                lblSelectedRMA.Text = "Selected RMA: None"
            End If

            RefreshRMAJournalGrid()
        End Sub

        Private Sub UpdateInspectionControls()
            If _selectedRMA IsNot Nothing Then
                lblSelectedRMA.Text = $"Selected RMA: {_selectedRMA.DocumentNumber} | Customer: {_selectedRMA.CustomerName} | In Quarantine: {_selectedRMA.QuarantineQty} units"

                Dim maxQ As Integer = _selectedRMA.QuarantineQty
                numRestockQty.Maximum = CDec(maxQ)
                numScrapQty.Maximum = CDec(maxQ)

                If maxQ > 0 Then
                    numRestockQty.Value = CDec(maxQ)
                    numScrapQty.Value = 0D
                    numRestockQty.Enabled = True
                    numScrapQty.Enabled = True
                    btnConfirmInspection.Enabled = True
                    UpdateInspectionSummary()
                Else
                    numRestockQty.Value = 0D
                    numScrapQty.Value = 0D
                    numRestockQty.Enabled = False
                    numScrapQty.Enabled = False
                    btnConfirmInspection.Enabled = False
                    lblInspectionSummary.Text = "RMA fully inspected. All units disposed."
                End If
            End If
        End Sub

        Private Sub UpdateInspectionSummary()
            If _selectedRMA Is Nothing Then Return

            Dim restock As Integer = Convert.ToInt32(numRestockQty.Value)
            Dim scrap As Integer = Convert.ToInt32(numScrapQty.Value)
            Dim total As Integer = restock + scrap

            Dim arCredit As Decimal = CDec(total) * _selectedRMA.SellingUnitPrice
            Dim restockVal As Decimal = CDec(restock) * _selectedRMA.OriginalUnitCost
            Dim scrapVal As Decimal = CDec(scrap) * _selectedRMA.OriginalUnitCost

            lblInspectionSummary.Text = $"AR Credit (4100/1200): ${arCredit:N2} | Restock to 1300: ${restockVal:N2} ({restock} units) | Scrap to 5300: ${scrapVal:N2} ({scrap} units)"
        End Sub

        Private Sub RefreshRMAJournalGrid()
            dgvRMAJournal.Rows.Clear()
            For Each entry In _repo.JournalHistory
                If entry.SourceDocType = "CreditNote" Then
                    For Each line In entry.Lines
                        dgvRMAJournal.Rows.Add(
                            entry.DocumentNumber,
                            entry.PostingDate.ToString("yyyy-MM-dd"),
                            line.AccountCode,
                            line.AccountName,
                            If(line.Debit > 0D, line.Debit.ToString("C2"), "-"),
                            If(line.Credit > 0D, line.Credit.ToString("C2"), "-"),
                            line.LineMemo
                        )
                    Next
                End If
            Next
        End Sub

        ' Event Handlers
        Private Sub dgvOpenPOs_SelectionChanged(sender As Object, e As EventArgs) Handles dgvOpenPOs.SelectionChanged
            If dgvOpenPOs.SelectedRows.Count > 0 Then
                _selectedPO = TryCast(dgvOpenPOs.SelectedRows(0).DataBoundItem, WarehousePOModel)
                BindPOLines(_selectedPO)
            End If
        End Sub

        Private Sub dgvPOLines_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPOLines.SelectionChanged
            If dgvPOLines.SelectedRows.Count > 0 Then
                _selectedPOLine = TryCast(dgvPOLines.SelectedRows(0).DataBoundItem, WarehousePOLineModel)
                UpdateReceiveQtyControls()
            End If
        End Sub

        Private Sub btnReceiveGRN_Click(sender As Object, e As EventArgs) Handles btnReceiveGRN.Click
            If _selectedPO Is Nothing OrElse _selectedPOLine Is Nothing Then
                MessageBox.Show("Please select an open Purchase Order and line item to receive.", "Intake Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim recQty As Integer = Convert.ToInt32(numReceiveQty.Value)
            If recQty <= 0 Then
                MessageBox.Show("Receiving quantity must be greater than zero.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim dict As New Dictionary(Of Integer, Integer)()
            dict.Add(_selectedPOLine.LineId, recQty)

            Try
                Dim grn = _repo.CreateGRN(_selectedPO, dict)

                MessageBox.Show($"Dock receipt committed successfully!{Environment.NewLine}" &
                                $"Generated GRN: {grn.DocumentNumber}{Environment.NewLine}" &
                                $"Received: {recQty} units of {_selectedPOLine.SKU}{Environment.NewLine}" &
                                $"Total Valuation: ${grn.TotalValuation:N2}{Environment.NewLine}" &
                                $"Balanced GL Journal posted: Debit 1300 Inventory / Credit 2100 GRNI.",
                                "Goods Receipt Confirmed", MessageBoxButtons.OK, MessageBoxIcon.Information)

                RefreshDockReceivingView()
                PopulateIntakeProducts()
            Catch ex As Exception
                MessageBox.Show($"Error receiving goods: {ex.Message}", "Receiving Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub cmbIntakeProduct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbIntakeProduct.SelectedIndexChanged
            Dim prod = TryCast(cmbIntakeProduct.SelectedItem, WarehouseProductModel)
            If prod IsNot Nothing Then
                numIntakeOrigCost.Value = prod.CurrentUnitCost
                numIntakeSellPrice.Value = Math.Round(prod.CurrentUnitCost * 1.5D, 2)
            End If
        End Sub

        Private Sub btnIntakeToQuarantine_Click(sender As Object, e As EventArgs) Handles btnIntakeToQuarantine.Click
            Dim customer As String = txtIntakeCustomer.Text.Trim()
            Dim invoice As String = txtIntakeInvoice.Text.Trim()
            Dim prod = TryCast(cmbIntakeProduct.SelectedItem, WarehouseProductModel)
            Dim qty As Integer = Convert.ToInt32(numIntakeQty.Value)
            Dim cost As Decimal = numIntakeOrigCost.Value
            Dim price As Decimal = numIntakeSellPrice.Value

            If String.IsNullOrWhiteSpace(customer) OrElse String.IsNullOrWhiteSpace(invoice) Then
                MessageBox.Show("Customer name and original sales invoice number are required.", "Required Fields", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If prod Is Nothing Then
                MessageBox.Show("Please select a product for return intake.", "Product Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                Dim rma = _repo.IntakeRMAToQuarantine(customer, invoice, prod, qty, cost, price)

                MessageBox.Show($"RMA Return Voucher logged to Quarantine!{Environment.NewLine}" &
                                $"RMA Number: {rma.DocumentNumber}{Environment.NewLine}" &
                                $"Quarantine Count: +{qty} units on SKU {prod.SKU}{Environment.NewLine}" &
                                $"Notice: Inventory is held in operational non-valuated quarantine.{Environment.NewLine}" &
                                $"No General Ledger journal created until Stage 2 inspection.",
                                "Quarantine Intake Completed", MessageBoxButtons.OK, MessageBoxIcon.Information)

                RefreshQuarantineView()
                PopulateIntakeProducts()
            Catch ex As Exception
                MessageBox.Show($"Error intaking RMA: {ex.Message}", "Intake Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub dgvQuarantineItems_SelectionChanged(sender As Object, e As EventArgs) Handles dgvQuarantineItems.SelectionChanged
            If dgvQuarantineItems.SelectedRows.Count > 0 Then
                _selectedRMA = TryCast(dgvQuarantineItems.SelectedRows(0).DataBoundItem, WarehouseRMAModel)
                UpdateInspectionControls()
            End If
        End Sub

        Private Sub numRestockQty_ValueChanged(sender As Object, e As EventArgs) Handles numRestockQty.ValueChanged
            If _selectedRMA IsNot Nothing Then
                Dim restock As Integer = Convert.ToInt32(numRestockQty.Value)
                Dim scrap As Integer = Convert.ToInt32(numScrapQty.Value)
                Dim maxQ As Integer = _selectedRMA.QuarantineQty

                If restock + scrap > maxQ Then
                    numScrapQty.Value = CDec(Math.Max(0, maxQ - restock))
                End If
                UpdateInspectionSummary()
            End If
        End Sub

        Private Sub numScrapQty_ValueChanged(sender As Object, e As EventArgs) Handles numScrapQty.ValueChanged
            If _selectedRMA IsNot Nothing Then
                Dim restock As Integer = Convert.ToInt32(numRestockQty.Value)
                Dim scrap As Integer = Convert.ToInt32(numScrapQty.Value)
                Dim maxQ As Integer = _selectedRMA.QuarantineQty

                If restock + scrap > maxQ Then
                    numRestockQty.Value = CDec(Math.Max(0, maxQ - scrap))
                End If
                UpdateInspectionSummary()
            End If
        End Sub

        Private Sub btnConfirmInspection_Click(sender As Object, e As EventArgs) Handles btnConfirmInspection.Click
            If _selectedRMA Is Nothing Then
                MessageBox.Show("Please select an RMA item from quarantine to inspect.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim restock As Integer = Convert.ToInt32(numRestockQty.Value)
            Dim scrap As Integer = Convert.ToInt32(numScrapQty.Value)
            Dim total As Integer = restock + scrap

            If total <= 0 Then
                MessageBox.Show("Inspection requires disposing at least 1 unit to Restock or Scrap.", "Inspection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If total > _selectedRMA.QuarantineQty Then
                MessageBox.Show($"Total inspected ({total}) exceeds remaining quarantine units ({_selectedRMA.QuarantineQty}).", "Invalid Count", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                Dim journal = _repo.InspectRMA(_selectedRMA, restock, scrap)

                MessageBox.Show($"Inspection Disposition Committed!{Environment.NewLine}" &
                                $"Credit Note Journal: {journal.DocumentNumber}{Environment.NewLine}" &
                                $"Restocked to Active Inventory: {restock} units (Perpetual Cost Recalculated){Environment.NewLine}" &
                                $"Scrapped to Damaged Stock: {scrap} units (Expense Acct 5300){Environment.NewLine}" &
                                $"Double-Entry Balanced: Total Debits = ${journal.TotalDebits:N2}, Total Credits = ${journal.TotalCredits:N2}",
                                "Inspection & Credit Note Posted", MessageBoxButtons.OK, MessageBoxIcon.Information)

                RefreshQuarantineView()
                PopulateIntakeProducts()
            Catch ex As Exception
                MessageBox.Show($"Error posting inspection: {ex.Message}", "Inspection Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub WarehouseIntakeForm_Resize(sender As Object, e As EventArgs) Handles Me.Resize
            If pnlPOLines IsNot Nothing AndAlso splitDock IsNot Nothing AndAlso splitDock.Panel1.Width > 0 Then
                ' Balanced 50% split for Open POs and PO Lines panels
                Dim targetW As Integer = CInt(Math.Floor(splitDock.Panel1.Width * 0.5))
                pnlPOLines.Width = Math.Max(460, targetW)
            End If
            If pnlReceiveActions IsNot Nothing AndAlso btnReceiveGRN IsNot Nothing Then
                btnReceiveGRN.Left = 12
                btnReceiveGRN.Width = Math.Max(200, pnlReceiveActions.ClientSize.Width - 24)
            End If
        End Sub

        Private Sub tabMain_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabMain.SelectedIndexChanged
            ' Keep explicit column widths to prevent blind auto-resize triggering horizontal scrollbars at 1280x720
        End Sub

        Private Sub SetupStockAdjustmentsGrid()
            dgvAdjLines.Columns.Clear()
            dgvAdjLines.AutoGenerateColumns = False
            dgvAdjLines.ScrollBars = ScrollBars.Both

            dgvAdjLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "SKU",
                .DataPropertyName = "SKU",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .Width = 85,
                .MinimumWidth = 75
            })
            dgvAdjLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Product Name",
                .DataPropertyName = "ProductName",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                .FillWeight = 140.0!,
                .MinimumWidth = 90
            })
            dgvAdjLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "On-Hand",
                .DataPropertyName = "TotalQtyOnHand",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 60,
                .MinimumWidth = 55
            })
            dgvAdjLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Variance (+/-)",
                .DataPropertyName = "VarianceQty",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 65,
                .MinimumWidth = 60
            })
            dgvAdjLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Reason",
                .DataPropertyName = "ReasonCode",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .Width = 85,
                .MinimumWidth = 75
            })
            dgvAdjLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Unit Cost",
                .DataPropertyName = "UnitCost",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 70,
                .MinimumWidth = 65
            })
            dgvAdjLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Impact ($)",
                .DataPropertyName = "TotalImpact",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 75,
                .MinimumWidth = 70
            })
            dgvAdjLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Notes",
                .DataPropertyName = "Notes",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                .FillWeight = 100.0!,
                .MinimumWidth = 80
            })
        End Sub

        Private Sub PopulateAdjustmentProducts()
            cmbAdjProduct.DataSource = Nothing
            cmbAdjProduct.DisplayMember = "DisplayText"
            cmbAdjProduct.ValueMember = "ProductId"
            cmbAdjProduct.DataSource = _repo.Products.ToList()

            cmbAdjReason.Items.Clear()
            cmbAdjReason.Items.Add("Shrinkage")
            cmbAdjReason.Items.Add("Damage")
            cmbAdjReason.Items.Add("CountSurplus")
            cmbAdjReason.Items.Add("CountDeficit")
            If cmbAdjReason.Items.Count > 0 Then
                cmbAdjReason.SelectedIndex = 0
            End If
        End Sub

        Private Sub RefreshAdjustmentsView()
            dgvAdjLines.DataSource = Nothing
            dgvAdjLines.DataSource = _pendingAdjustments.ToList()
            UpdateAdjustmentJournalPreview()
        End Sub

        Private Sub cmbAdjProduct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAdjProduct.SelectedIndexChanged
            Dim prod = TryCast(cmbAdjProduct.SelectedItem, WarehouseProductModel)
            If prod IsNot Nothing Then
                lblAdjOnHand.Text = $"On Hand: {prod.TotalQtyOnHand} {prod.BaseUnit}"
                lblAdjUnitCost.Text = $"Unit Cost: {prod.CurrentUnitCost:C2}"
            Else
                lblAdjOnHand.Text = "On Hand: 0 Units"
                lblAdjUnitCost.Text = "Unit Cost: $0.00"
            End If
            UpdateAdjustmentJournalPreview()
        End Sub

        Private Sub cmbAdjReason_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbAdjReason.SelectedIndexChanged
            Dim reason = If(cmbAdjReason.SelectedItem IsNot Nothing, cmbAdjReason.SelectedItem.ToString(), "")
            If String.Equals(reason, "CountSurplus", StringComparison.OrdinalIgnoreCase) Then
                If numAdjVariance.Value < 0 Then numAdjVariance.Value = Math.Abs(numAdjVariance.Value)
            ElseIf String.Equals(reason, "Shrinkage", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(reason, "Damage", StringComparison.OrdinalIgnoreCase) OrElse
                   String.Equals(reason, "CountDeficit", StringComparison.OrdinalIgnoreCase) Then
                If numAdjVariance.Value > 0 Then numAdjVariance.Value = -numAdjVariance.Value
            End If
            UpdateAdjustmentJournalPreview()
        End Sub

        Private Sub numAdjVariance_ValueChanged(sender As Object, e As EventArgs) Handles numAdjVariance.ValueChanged
            UpdateAdjustmentJournalPreview()
        End Sub

        Private Sub btnAddAdjLine_Click(sender As Object, e As EventArgs) Handles btnAddAdjLine.Click
            Dim prod = TryCast(cmbAdjProduct.SelectedItem, WarehouseProductModel)
            If prod Is Nothing Then
                MessageBox.Show("Please select a product to adjust.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim variance As Integer = CInt(numAdjVariance.Value)
            If variance = 0 Then
                MessageBox.Show("Please enter a non-zero count variance quantity.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If variance < 0 AndAlso Math.Abs(variance) > prod.TotalQtyOnHand Then
                MessageBox.Show($"Cannot adjust stock for SKU {prod.SKU}. Requested reduction ({Math.Abs(variance)}) exceeds on-hand stock ({prod.TotalQtyOnHand}).",
                                "Zero Negative Stock Violation", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim reasonStr As String = If(cmbAdjReason.SelectedItem IsNot Nothing, cmbAdjReason.SelectedItem.ToString(), "Shrinkage")
            Dim impactVal As Decimal = Math.Round(Math.Abs(CDec(variance)) * prod.CurrentUnitCost, 4, MidpointRounding.AwayFromZero)

            _pendingAdjustments.Add(New WarehouseAdjustmentLineModel() With {
                .ProductId = prod.ProductId,
                .SKU = prod.SKU,
                .ProductName = prod.Name,
                .TotalQtyOnHand = prod.TotalQtyOnHand,
                .VarianceQty = variance,
                .ReasonCode = reasonStr,
                .UnitCost = prod.CurrentUnitCost,
                .TotalImpact = impactVal,
                .Notes = txtAdjNotes.Text.Trim()
            })

            numAdjVariance.Value = 0
            txtAdjNotes.Clear()
            RefreshAdjustmentsView()
        End Sub

        Private Sub btnClearAdjLines_Click(sender As Object, e As EventArgs) Handles btnClearAdjLines.Click
            _pendingAdjustments.Clear()
            RefreshAdjustmentsView()
        End Sub

        Private Sub btnRemoveAdjLine_Click(sender As Object, e As EventArgs) Handles btnRemoveAdjLine.Click
            If dgvAdjLines.SelectedRows.Count > 0 Then
                Dim selectedItem = TryCast(dgvAdjLines.SelectedRows(0).DataBoundItem, WarehouseAdjustmentLineModel)
                If selectedItem IsNot Nothing Then
                    _pendingAdjustments.Remove(selectedItem)
                    RefreshAdjustmentsView()
                End If
            End If
        End Sub

        Private Sub btnSubmitAdjustment_Click(sender As Object, e As EventArgs) Handles btnSubmitAdjustment.Click
            Try
                Dim linesToSubmit As New List(Of InventoryAdjustmentLine)()

                If _pendingAdjustments.Count > 0 Then
                    For Each p In _pendingAdjustments
                        linesToSubmit.Add(New InventoryAdjustmentLine() With {
                            .ProductId = p.ProductId,
                            .QtyChange = p.VarianceQty,
                            .UnitCost = p.UnitCost,
                            .Notes = p.Notes
                        })
                    Next
                ElseIf cmbAdjProduct.SelectedItem IsNot Nothing AndAlso numAdjVariance.Value <> 0 Then
                    Dim prod = TryCast(cmbAdjProduct.SelectedItem, WarehouseProductModel)
                    If prod IsNot Nothing Then
                        Dim variance As Integer = CInt(numAdjVariance.Value)
                        If variance < 0 AndAlso Math.Abs(variance) > prod.TotalQtyOnHand Then
                            MessageBox.Show($"Cannot adjust stock for SKU {prod.SKU}. Requested reduction ({Math.Abs(variance)}) exceeds on-hand quantity ({prod.TotalQtyOnHand}).",
                                            "Validation Error", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                            Return
                        End If
                        linesToSubmit.Add(New InventoryAdjustmentLine() With {
                            .ProductId = prod.ProductId,
                            .QtyChange = variance,
                            .UnitCost = prod.CurrentUnitCost,
                            .Notes = txtAdjNotes.Text.Trim()
                        })
                    End If
                End If

                If linesToSubmit.Count = 0 Then
                    MessageBox.Show("Please add at least one line with non-zero count variance to adjust.",
                                    "No Adjustments Entered", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    Return
                End If

                Dim reason As String = If(cmbAdjReason.SelectedItem IsNot Nothing, cmbAdjReason.SelectedItem.ToString(), "PhysicalCount")
                Dim adj = _repo.RecordAdjustment(Nothing, reason, linesToSubmit, DateTime.Today, "Warehouse Physical Stock Adjustment")

                MessageBox.Show($"Inventory adjustment {adj.DocumentNumber} posted successfully! General Ledger updated.",
                                "Adjustment Posted", MessageBoxButtons.OK, MessageBoxIcon.Information)

                _pendingAdjustments.Clear()
                RefreshAdjustmentsView()
                PopulateAdjustmentProducts()
                RefreshDockReceivingView()
                numAdjVariance.Value = 0
                txtAdjNotes.Clear()
                UpdateAdjustmentJournalPreview()
            Catch ex As Exception
                MessageBox.Show($"Error recording inventory adjustment: {ex.Message}", "Adjustment Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub UpdateAdjustmentJournalPreview()
            Dim linesToPreview As New List(Of WarehouseAdjustmentLineModel)()

            If _pendingAdjustments.Count > 0 Then
                linesToPreview.AddRange(_pendingAdjustments)
            ElseIf cmbAdjProduct.SelectedItem IsNot Nothing AndAlso numAdjVariance.Value <> 0 Then
                Dim prod = TryCast(cmbAdjProduct.SelectedItem, WarehouseProductModel)
                If prod IsNot Nothing Then
                    Dim variance As Integer = CInt(numAdjVariance.Value)
                    Dim reasonStr As String = If(cmbAdjReason.SelectedItem IsNot Nothing, cmbAdjReason.SelectedItem.ToString(), "Shrinkage")
                    Dim impactVal As Decimal = Math.Round(Math.Abs(CDec(variance)) * prod.CurrentUnitCost, 4, MidpointRounding.AwayFromZero)
                    linesToPreview.Add(New WarehouseAdjustmentLineModel() With {
                        .ProductId = prod.ProductId,
                        .SKU = prod.SKU,
                        .ProductName = prod.Name,
                        .TotalQtyOnHand = prod.TotalQtyOnHand,
                        .VarianceQty = variance,
                        .ReasonCode = reasonStr,
                        .UnitCost = prod.CurrentUnitCost,
                        .TotalImpact = impactVal,
                        .Notes = txtAdjNotes.Text.Trim()
                    })
                End If
            End If

            If linesToPreview.Count = 0 Then
                ctrlAdjJournal.Clear()
                lblAdjStatus.Text = "Add items or enter count variance to preview double-entry postings."
                lblAdjStatus.ForeColor = System.Drawing.Color.FromArgb(108, 117, 125)
                Return
            End If

            Dim distributionLines As New List(Of UI.Controls.DoubleEntryDistributionGridControl.DistributionLineItem)()
            Dim lineIdx As Integer = 1

            For Each item In linesToPreview
                If item.VarianceQty = 0 Then Continue For

                Dim valuation As Decimal = Math.Round(Math.Abs(CDec(item.VarianceQty)) * item.UnitCost, 4, MidpointRounding.AwayFromZero)

                If item.VarianceQty < 0 Then
                    Dim debitAccountCode As String = "5300"
                    Dim debitAccountName As String = "Inventory Shrinkage & Damaged Stock"
                    If String.Equals(item.ReasonCode, "Damage", StringComparison.OrdinalIgnoreCase) Then
                        debitAccountName = "Damaged Stock Expense"
                    End If

                    distributionLines.Add(New UI.Controls.DoubleEntryDistributionGridControl.DistributionLineItem() With {
                        .LineId = lineIdx,
                        .AccountCode = debitAccountCode,
                        .AccountName = debitAccountName,
                        .DebitAmount = valuation,
                        .CreditAmount = 0D,
                        .LineMemo = $"Write-down {Math.Abs(item.VarianceQty)} units ({item.SKU}) - {item.ReasonCode}"
                    })
                    lineIdx += 1

                    distributionLines.Add(New UI.Controls.DoubleEntryDistributionGridControl.DistributionLineItem() With {
                        .LineId = lineIdx,
                        .AccountCode = "1300",
                        .AccountName = "Merchandise Inventory",
                        .DebitAmount = 0D,
                        .CreditAmount = valuation,
                        .LineMemo = $"Inventory reduction {Math.Abs(item.VarianceQty)} units ({item.SKU})"
                    })
                    lineIdx += 1
                Else
                    distributionLines.Add(New UI.Controls.DoubleEntryDistributionGridControl.DistributionLineItem() With {
                        .LineId = lineIdx,
                        .AccountCode = "1300",
                        .AccountName = "Merchandise Inventory",
                        .DebitAmount = valuation,
                        .CreditAmount = 0D,
                        .LineMemo = $"Inventory surplus count {item.VarianceQty} units ({item.SKU})"
                    })
                    lineIdx += 1

                    distributionLines.Add(New UI.Controls.DoubleEntryDistributionGridControl.DistributionLineItem() With {
                        .LineId = lineIdx,
                        .AccountCode = "5400",
                        .AccountName = "Stocktake Variance Gain",
                        .DebitAmount = 0D,
                        .CreditAmount = valuation,
                        .LineMemo = $"Stock count gain {item.VarianceQty} units ({item.SKU})"
                    })
                    lineIdx += 1
                End If
            Next

            ctrlAdjJournal.SetLines(distributionLines)
            lblAdjStatus.Text = $"Ready to post: {linesToPreview.Count} line(s) totaling {ctrlAdjJournal.TotalDebits:C2} in physical adjustment."
            lblAdjStatus.ForeColor = System.Drawing.Color.FromArgb(25, 135, 84)
        End Sub

    End Class

End Namespace
