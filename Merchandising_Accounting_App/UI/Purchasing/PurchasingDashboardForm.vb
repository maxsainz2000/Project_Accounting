Option Strict On
Option Explicit On
Option Compare Binary

Imports System.Collections.Generic

Namespace UI.Purchasing

    Public Class PurchasingDashboardForm

        Friend ReadOnly _repo As New PurchasingDataService()
        Private _selectedMatch As ThreeWayMatchModel
        Private _selectedPO As PurchasingPOModel

        Private Sub PurchasingDashboardForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            SetupThreeWayMatchGrid()
            SetupThreeWayJournalGrid()
            SetupPurchaseOrdersGrid()
            SetupPOLinesGrid()
            SetupGRNsGrid()

            PopulateDropdowns()
            RefreshThreeWayMatchView()
            RefreshPurchaseOrdersView()
            RefreshGRNsView()
        End Sub

        Private Sub SetupThreeWayMatchGrid()
            dgvThreeWayMatch.Columns.Clear()
            dgvThreeWayMatch.AutoGenerateColumns = False
            dgvThreeWayMatch.ScrollBars = ScrollBars.Both
            dgvThreeWayMatch.ColumnHeadersDefaultCellStyle.WrapMode = DataGridViewTriState.True
            dgvThreeWayMatch.ColumnHeadersHeight = 36

            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Match Status", .DataPropertyName = "MatchStatus", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 130, .MinimumWidth = 120})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Vendor", .DataPropertyName = "VendorName", .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill, .FillWeight = 160.0!, .MinimumWidth = 180})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "PO #", .DataPropertyName = "PONumber", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 110, .MinimumWidth = 105})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "PO Qty", .DataPropertyName = "POQty", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight}, .Width = 70, .MinimumWidth = 65})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "PO Price", .DataPropertyName = "POUnitPrice", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}, .Width = 85, .MinimumWidth = 80})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "GRN #", .DataPropertyName = "GRNNumber", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 120, .MinimumWidth = 115})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "GRN Qty", .DataPropertyName = "GRNQty", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight}, .Width = 70, .MinimumWidth = 65})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Bill #", .DataPropertyName = "BillNumber", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 120, .MinimumWidth = 115})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Bill Qty", .DataPropertyName = "BillQty", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight}, .Width = 70, .MinimumWidth = 65})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Billed Price", .DataPropertyName = "BilledUnitPrice", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}, .Width = 85, .MinimumWidth = 80})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Freight ($)", .DataPropertyName = "InboundFreight", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}, .Width = 85, .MinimumWidth = 80})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "PPV ($)", .DataPropertyName = "PurchasePriceVariance", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}, .Width = 80, .MinimumWidth = 75})
            dgvThreeWayMatch.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Net Payable ($)", .DataPropertyName = "NetTotalPayable", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight}, .Width = 110, .MinimumWidth = 100})

            For Each col As DataGridViewColumn In dgvThreeWayMatch.Columns
                col.SortMode = DataGridViewColumnSortMode.NotSortable
            Next
        End Sub

        Private Sub tabPurchasing_SelectedIndexChanged(sender As Object, e As EventArgs) Handles tabPurchasing.SelectedIndexChanged
            If tabPurchasing.SelectedTab Is tabPurchaseOrders Then
                dgvPurchaseOrders.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
                dgvPurchasingPOLines.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
            ElseIf tabPurchasing.SelectedTab Is tabGRNList Then
                dgvGRNs.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader)
            ElseIf tabPurchasing.SelectedTab Is tabThreeWayMatch Then
                dgvThreeWayMatch.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.None)
            End If
        End Sub

        Private Sub SetupThreeWayJournalGrid()
            dgvThreeWayJournal.Columns.Clear()
            dgvThreeWayJournal.AutoGenerateColumns = False
            dgvThreeWayJournal.ScrollBars = ScrollBars.Both

            dgvThreeWayJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Bill Doc #", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 145, .MinimumWidth = 135})
            dgvThreeWayJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Posting Date", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 90, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}})
            dgvThreeWayJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Account", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 95, .MinimumWidth = 85, .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleCenter}})
            dgvThreeWayJournal.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Account Name", .AutoSizeMode = DataGridViewAutoSizeColumnMode.None, .Width = 240, .MinimumWidth = 180})
            dgvThreeWayJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Debit ($)",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 95,
                .MinimumWidth = 90
            })
            dgvThreeWayJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Credit ($)",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.None,
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 95,
                .MinimumWidth = 90
            })
            dgvThreeWayJournal.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Memo / Clearing Breakdown",
                .AutoSizeMode = DataGridViewAutoSizeColumnMode.Fill,
                .MinimumWidth = 180
            })
        End Sub

        Private Sub SetupPurchaseOrdersGrid()
            dgvPurchaseOrders.Columns.Clear()
            dgvPurchaseOrders.AutoGenerateColumns = False

            dgvPurchaseOrders.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "PO #", .DataPropertyName = "DocumentNumber", .Width = 110})
            dgvPurchaseOrders.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Vendor", .DataPropertyName = "VendorName", .Width = 200})
            dgvPurchaseOrders.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Order Date",
                .DataPropertyName = "OrderDate",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "yyyy-MM-dd"},
                .Width = 95
            })
            dgvPurchaseOrders.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Status", .DataPropertyName = "Status", .Width = 110})
            dgvPurchaseOrders.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Total Qty",
                .DataPropertyName = "TotalOrderedQty",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 85
            })
            dgvPurchaseOrders.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Total ($)",
                .DataPropertyName = "TotalAmount",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 100
            })
        End Sub

        Private Sub SetupPOLinesGrid()
            dgvPurchasingPOLines.Columns.Clear()
            dgvPurchasingPOLines.AutoGenerateColumns = False

            dgvPurchasingPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "SKU", .DataPropertyName = "SKU", .Width = 110})
            dgvPurchasingPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Product Name", .DataPropertyName = "ProductName", .Width = 250})
            dgvPurchasingPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Ordered Qty",
                .DataPropertyName = "OrderedQty",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 100
            })
            dgvPurchasingPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "PO Unit Price",
                .DataPropertyName = "UnitPrice",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 100
            })
            dgvPurchasingPOLines.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Extended Total",
                .DataPropertyName = "LineTotal",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 120
            })
        End Sub

        Private Sub SetupGRNsGrid()
            dgvGRNs.Columns.Clear()
            dgvGRNs.AutoGenerateColumns = False

            dgvGRNs.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "GRN #", .DataPropertyName = "DocumentNumber", .Width = 110})
            dgvGRNs.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "PO #", .DataPropertyName = "PONumber", .Width = 110})
            dgvGRNs.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Vendor", .DataPropertyName = "VendorName", .Width = 200})
            dgvGRNs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Receipt Date",
                .DataPropertyName = "ReceiptDate",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "yyyy-MM-dd"},
                .Width = 100
            })
            dgvGRNs.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "SKU", .DataPropertyName = "SKU", .Width = 110})
            dgvGRNs.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Product Name", .DataPropertyName = "ProductName", .Width = 220})
            dgvGRNs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Received Qty",
                .DataPropertyName = "ReceivedQty",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 95
            })
            dgvGRNs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "PO Unit Price",
                .DataPropertyName = "POUnitPrice",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 95
            })
            dgvGRNs.Columns.Add(New DataGridViewTextBoxColumn() With {
                .HeaderText = "Total Valuation",
                .DataPropertyName = "TotalValuation",
                .DefaultCellStyle = New DataGridViewCellStyle() With {.Format = "C2", .Alignment = DataGridViewContentAlignment.MiddleRight},
                .Width = 110
            })
            dgvGRNs.Columns.Add(New DataGridViewTextBoxColumn() With {.HeaderText = "Status", .DataPropertyName = "Status", .Width = 100})
        End Sub

        Private Sub PopulateDropdowns()
            ' Vendors
            cmbNewPOVendor.Items.Clear()
            For Each v In _repo.Vendors
                cmbNewPOVendor.Items.Add(v)
            Next
            cmbNewPOVendor.DisplayMember = "DisplayText"
            If cmbNewPOVendor.Items.Count > 0 Then cmbNewPOVendor.SelectedIndex = 0

            ' Products
            cmbNewPOProduct.Items.Clear()
            For Each p In _repo.CatalogProducts
                cmbNewPOProduct.Items.Add(p)
            Next
            cmbNewPOProduct.DisplayMember = "DisplayText"
            If cmbNewPOProduct.Items.Count > 0 Then cmbNewPOProduct.SelectedIndex = 0
        End Sub

        Private Sub RefreshThreeWayMatchView()
            Dim matches As New List(Of ThreeWayMatchModel)(_repo.ThreeWayMatches)
            dgvThreeWayMatch.DataSource = Nothing
            dgvThreeWayMatch.DataSource = matches
            For Each c As DataGridViewColumn In dgvThreeWayMatch.Columns
                c.SortMode = DataGridViewColumnSortMode.NotSortable
            Next

            If matches.Count > 0 Then
                dgvThreeWayMatch.Rows(0).Selected = True
                _selectedMatch = matches(0)
                BindMatchDetails(_selectedMatch)
            Else
                _selectedMatch = Nothing
            End If

            RefreshThreeWayJournalGrid()
        End Sub

        Private Sub BindMatchDetails(match As ThreeWayMatchModel)
            If match Is Nothing Then Return

            lblSelectedMatch.Text = $"Selected Match: PO {match.PONumber} | GRN {match.GRNNumber} | Vendor: {match.VendorName} | Product: {match.SKU}"

            If match.IsSettled Then
                txtBillNumber.Text = match.BillNumber
                txtBillNumber.Enabled = False
                numBillQty.Value = CDec(match.BillQty)
                numBillQty.Enabled = False
                numBilledUnitPrice.Value = match.BilledUnitPrice
                numBilledUnitPrice.Enabled = False
                numFreightAmount.Value = match.InboundFreight
                numFreightAmount.Enabled = False
                btnPostThreeWayMatch.Enabled = False
                btnPostThreeWayMatch.Text = "Match Settled & Billed"
            Else
                txtBillNumber.Text = If(String.IsNullOrWhiteSpace(match.BillNumber), "BILL-2026-1095", match.BillNumber)
                txtBillNumber.Enabled = True
                numBillQty.Maximum = 100000D
                numBillQty.Value = CDec(If(match.BillQty > 0, match.BillQty, match.GRNQty))
                numBillQty.Enabled = True
                numBilledUnitPrice.Value = If(match.BilledUnitPrice > 0D, match.BilledUnitPrice, match.POUnitPrice)
                numBilledUnitPrice.Enabled = True
                numFreightAmount.Value = match.InboundFreight
                numFreightAmount.Enabled = True
                btnPostThreeWayMatch.Enabled = True
                btnPostThreeWayMatch.Text = "Commit Supplier Bill & Post 3-Way Match Journal"
            End If

            UpdateLiveVarianceCalculation()
        End Sub

        Private Sub UpdateLiveVarianceCalculation()
            If _selectedMatch Is Nothing Then Return

            Dim billQ As Integer = Convert.ToInt32(numBillQty.Value)
            Dim billedP As Decimal = numBilledUnitPrice.Value
            Dim freight As Decimal = numFreightAmount.Value
            Dim poP As Decimal = _selectedMatch.POUnitPrice

            Dim ppv As Decimal = (billedP - poP) * CDec(billQ)
            Dim grniClear As Decimal = poP * CDec(billQ)
            Dim totalPayable As Decimal = (CDec(billQ) * billedP) + freight

            Dim ppvText As String
            If ppv > 0D Then
                ppvText = $"+${ppv:N2} (Unfavorable: Debit 5100 PPV)"
            ElseIf ppv < 0D Then
                ppvText = $"-${Math.Abs(ppv):N2} (Favorable: Credit 5100 PPV)"
            Else
                ppvText = "$0.00 (Exact Price Match)"
            End If

            lblVariancePreview.Text = $"Clearing 2100 GRNI: ${grniClear:N2} | PPV Variance: {ppvText} | Inbound Freight: ${freight:N2} (Acct 5200) | Net AP Due (2000): ${totalPayable:N2}"
        End Sub

        Private Sub RefreshThreeWayJournalGrid()
            dgvThreeWayJournal.Rows.Clear()
            For Each entry In _repo.JournalHistory
                If entry.SourceDocType = "SupplierBill" Then
                    For Each line In entry.Lines
                        dgvThreeWayJournal.Rows.Add(
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

        Private Sub RefreshPurchaseOrdersView()
            Dim poList As New List(Of PurchasingPOModel)(_repo.PurchaseOrders)
            dgvPurchaseOrders.DataSource = Nothing
            dgvPurchaseOrders.DataSource = poList

            If poList.Count > 0 Then
                dgvPurchaseOrders.Rows(0).Selected = True
                _selectedPO = poList(0)
                BindPOLines(_selectedPO)
            Else
                _selectedPO = Nothing
                dgvPurchasingPOLines.DataSource = Nothing
            End If
        End Sub

        Private Sub BindPOLines(po As PurchasingPOModel)
            If po Is Nothing Then
                dgvPurchasingPOLines.DataSource = Nothing
                Return
            End If

            Dim lines As New List(Of PurchasingPOLineModel)(po.Lines)
            dgvPurchasingPOLines.DataSource = Nothing
            dgvPurchasingPOLines.DataSource = lines
        End Sub

        Private Sub RefreshGRNsView()
            Dim grnList As New List(Of PurchasingGRNModel)(_repo.GoodsReceiptNotes)
            dgvGRNs.DataSource = Nothing
            dgvGRNs.DataSource = grnList
        End Sub

        ' Event Handlers
        Private Sub dgvThreeWayMatch_SelectionChanged(sender As Object, e As EventArgs) Handles dgvThreeWayMatch.SelectionChanged
            If dgvThreeWayMatch.SelectedRows.Count > 0 Then
                _selectedMatch = TryCast(dgvThreeWayMatch.SelectedRows(0).DataBoundItem, ThreeWayMatchModel)
                BindMatchDetails(_selectedMatch)
            End If
        End Sub

        Private Sub numBillQty_ValueChanged(sender As Object, e As EventArgs) Handles numBillQty.ValueChanged
            UpdateLiveVarianceCalculation()
        End Sub

        Private Sub numBilledUnitPrice_ValueChanged(sender As Object, e As EventArgs) Handles numBilledUnitPrice.ValueChanged
            UpdateLiveVarianceCalculation()
        End Sub

        Private Sub numFreightAmount_ValueChanged(sender As Object, e As EventArgs) Handles numFreightAmount.ValueChanged
            UpdateLiveVarianceCalculation()
        End Sub

        Private Sub btnPostThreeWayMatch_Click(sender As Object, e As EventArgs) Handles btnPostThreeWayMatch.Click
            If _selectedMatch Is Nothing Then
                MessageBox.Show("Please select a 3-way match record to reconcile.", "Selection Required", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim billNum As String = txtBillNumber.Text.Trim()
            If String.IsNullOrWhiteSpace(billNum) Then
                MessageBox.Show("Please provide a Supplier Bill / Invoice document number.", "Bill Number Required", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Dim billQty As Integer = Convert.ToInt32(numBillQty.Value)
            Dim billedPrice As Decimal = numBilledUnitPrice.Value
            Dim freight As Decimal = numFreightAmount.Value

            Try
                Dim journal = _repo.PostThreeWayMatch(_selectedMatch, billNum, billQty, billedPrice, freight)

                MessageBox.Show($"Supplier Bill matched and reconciled successfully!{Environment.NewLine}" &
                                $"Document: {billNum}{Environment.NewLine}" &
                                $"Status: {_selectedMatch.MatchStatus}{Environment.NewLine}" &
                                $"Purchase Price Variance (Acct 5100): ${_selectedMatch.PurchasePriceVariance:N2}{Environment.NewLine}" &
                                $"Inbound Freight (Acct 5200): ${freight:N2}{Environment.NewLine}" &
                                $"Total Credited to Accounts Payable (Acct 2000): ${_selectedMatch.NetTotalPayable:N2}{Environment.NewLine}" &
                                $"Double-Entry Verification: Debits = ${journal.TotalDebits:N2}, Credits = ${journal.TotalCredits:N2} (Balanced)",
                                "3-Way Match Clearing Posted", MessageBoxButtons.OK, MessageBoxIcon.Information)

                RefreshThreeWayMatchView()
                PopulateDropdowns()
            Catch ex As Exception
                MessageBox.Show($"Error recording supplier bill: {ex.Message}", "Match Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

        Private Sub dgvPurchaseOrders_SelectionChanged(sender As Object, e As EventArgs) Handles dgvPurchaseOrders.SelectionChanged
            If dgvPurchaseOrders.SelectedRows.Count > 0 Then
                _selectedPO = TryCast(dgvPurchaseOrders.SelectedRows(0).DataBoundItem, PurchasingPOModel)
                BindPOLines(_selectedPO)
            End If
        End Sub

        Private Sub cmbNewPOProduct_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmbNewPOProduct.SelectedIndexChanged
            Dim p = TryCast(cmbNewPOProduct.SelectedItem, PurchasingProductItem)
            If p IsNot Nothing Then
                numNewPOUnitPrice.Value = p.StandardCost
            End If
        End Sub

        Private Sub btnCreatePO_Click(sender As Object, e As EventArgs) Handles btnCreatePO.Click
            Dim v = TryCast(cmbNewPOVendor.SelectedItem, PurchasingVendorModel)
            Dim p = TryCast(cmbNewPOProduct.SelectedItem, PurchasingProductItem)
            Dim qty As Integer = Convert.ToInt32(numNewPOQty.Value)
            Dim price As Decimal = numNewPOUnitPrice.Value

            If v Is Nothing OrElse p Is Nothing Then
                MessageBox.Show("Please select a vendor and a product.", "Missing Information", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            If qty <= 0 Then
                MessageBox.Show("Quantity must be greater than zero.", "Invalid Quantity", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                Return
            End If

            Try
                Dim lines As New List(Of PurchasingPOLineModel)()
                lines.Add(New PurchasingPOLineModel() With {
                    .LineId = 1,
                    .ProductId = p.ProductId,
                    .SKU = p.SKU,
                    .ProductName = p.Name,
                    .OrderedQty = qty,
                    .UnitPrice = price
                })

                Dim po = _repo.CreatePurchaseOrder(v, lines)

                MessageBox.Show($"Purchase Order created successfully!{Environment.NewLine}" &
                                $"PO Document: {po.DocumentNumber}{Environment.NewLine}" &
                                $"Vendor: {v.Name}{Environment.NewLine}" &
                                $"Item: {p.SKU} x {qty} @ ${price:N2}{Environment.NewLine}" &
                                $"Total PO Commitment: ${po.TotalAmount:N2}",
                                "Purchase Order Approved", MessageBoxButtons.OK, MessageBoxIcon.Information)

                RefreshPurchaseOrdersView()
            Catch ex As Exception
                MessageBox.Show($"Error creating Purchase Order: {ex.Message}", "PO Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        End Sub

    End Class

End Namespace
