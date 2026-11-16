Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Collections.Generic

Namespace UI.Warehouse

    Public Class WarehouseIntakeForm

        Private ReadOnly _repo As WarehouseMockRepository = WarehouseMockRepository.Instance
        Private _selectedPO As WarehousePOModel
        Private _selectedPOLine As WarehousePOLineModel
        Private _selectedRMA As WarehouseRMAModel

        Private Sub WarehouseIntakeForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            SetupOpenPOsGrid()
            SetupPOLinesGrid()
            SetupGRNJournalGrid()
            SetupQuarantineGrid()
            SetupRMAJournalGrid()

            PopulateIntakeProducts()
            RefreshDockReceivingView()
            RefreshQuarantineView()
        End Sub

        Private Sub SetupOpenPOsGrid()
            dgvOpenPOs.Columns.Clear()
            dgvOpenPOs.AutoGenerateColumns = False

            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "PO Number",
                .DataPropertyName = "DocumentNumber",
                .Width = 110
            })
            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Vendor",
                .DataPropertyName = "VendorName",
                .Width = 180
            })
            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Date",
                .DataPropertyName = "OrderDate",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "yyyy-MM-dd"},
                .Width = 90
            })
            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Status",
                .DataPropertyName = "Status",
                .Width = 100
            })
            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Total ($)",
                .DataPropertyName = "TotalAmount",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 90
            })
            dgvOpenPOs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Pending Qty",
                .DataPropertyName = "TotalPendingQty",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 90
            })
        End Sub

        Private Sub SetupPOLinesGrid()
            dgvPOLines.Columns.Clear()
            dgvPOLines.AutoGenerateColumns = False

            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "SKU",
                .DataPropertyName = "SKU",
                .Width = 100
            })
            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Product Name",
                .DataPropertyName = "ProductName",
                .Width = 180
            })
            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Ordered",
                .DataPropertyName = "OrderedQty",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 70
            })
            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Received",
                .DataPropertyName = "ReceivedQty",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 70
            })
            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Pending",
                .DataPropertyName = "PendingQty",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 70
            })
            dgvPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Unit Cost",
                .DataPropertyName = "POUnitPrice",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 85
            })
        End Sub

        Private Sub SetupGRNJournalGrid()
            dgvGRNJournal.Columns.Clear()
            dgvGRNJournal.AutoGenerateColumns = False

            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Doc #", .Width = 110})
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Posting Date", .Width = 90})
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Account", .Width = 75})
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Account Name", .Width = 220})
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Debit ($)",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 95
            })
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Credit ($)",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 95
            })
            dgvGRNJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Memo / Line Reference", .Width = 260})
        End Sub

        Private Sub SetupQuarantineGrid()
            dgvQuarantineItems.Columns.Clear()
            dgvQuarantineItems.AutoGenerateColumns = False

            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "RMA #", .DataPropertyName = "DocumentNumber", .Width = 100})
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Customer", .DataPropertyName = "CustomerName", .Width = 150})
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Orig Invoice", .DataPropertyName = "OriginalInvoiceNumber", .Width = 110})
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "SKU", .DataPropertyName = "SKU", .Width = 100})
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "In Quarantine",
                .DataPropertyName = "QuarantineQty",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 100
            })
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Restocked",
                .DataPropertyName = "RestockedQty",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 80
            })
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Scrapped",
                .DataPropertyName = "ScrappedQty",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 80
            })
            dgvQuarantineItems.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Status",
                .DataPropertyName = "Status",
                .Width = 100
            })
        End Sub

        Private Sub SetupRMAJournalGrid()
            dgvRMAJournal.Columns.Clear()
            dgvRMAJournal.AutoGenerateColumns = False

            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Credit Note #", .Width = 120})
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Posting Date", .Width = 90})
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Account", .Width = 75})
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Account Name", .Width = 210})
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Debit ($)",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 90
            })
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Credit ($)",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 90
            })
            dgvRMAJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Memo / Line Reference", .Width = 240})
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

                        lblCostImpactPreview.Text = $"Stock Impact: {prod.SKU} On-Hand: {curQty} -> {newQty} | Avg Cost: ${curCost:N2} -> ${blendedCost:N2} (Debit 1300 / Credit 2100)"
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

    End Class

End Namespace
