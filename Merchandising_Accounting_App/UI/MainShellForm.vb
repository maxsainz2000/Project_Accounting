Namespace UI

    Public Class MainShellForm

        Private _currentChildForm As Form

        Public Sub LoadChildForm(childForm As Form, title As String, subtitle As String)
            If _currentChildForm IsNot Nothing Then
                _currentChildForm.Close()
                pnlContentHost.Controls.Remove(_currentChildForm)
                _currentChildForm.Dispose()
                _currentChildForm = Nothing
            End If

            _currentChildForm = childForm
            lblCurrentModule.Text = title
            lblModuleSubtitle.Text = subtitle

            childForm.TopLevel = False
            childForm.FormBorderStyle = FormBorderStyle.None
            childForm.Dock = DockStyle.Fill
            pnlContentHost.Controls.Add(childForm)
            pnlContentHost.Tag = childForm
            childForm.BringToFront()
            childForm.Show()
        End Sub

        Private Sub MainShellForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            ' Load Purchasing as initial view
            LoadChildForm(New UI.Purchasing.PurchasingDashboardForm(), "Purchasing & 3-Way Match Verification", "Purchase Orders, GRN Inbound Tracking & 3-Way Match Verification (GRNI Clearing, PPV & Freight Variances)")
        End Sub

        Private Sub btnPurchasing_Click(sender As Object, e As EventArgs) Handles btnPurchasing.Click
            LoadChildForm(New UI.Purchasing.PurchasingDashboardForm(), "Purchasing & 3-Way Match Verification", "Purchase Orders, GRN Inbound Tracking & 3-Way Match Verification (GRNI Clearing, PPV & Freight Variances)")
        End Sub

        Private Sub btnSales_Click(sender As Object, e As EventArgs) Handles btnSales.Click
            LoadChildForm(New UI.Sales.SalesDashboardForm(), "Sales Orders & Inventory Stock Reservation", "Customer Orders, Real-Time Stock Availability, Hard-Stop Inventory Guard, and GL Invoicing")
        End Sub

        Private Sub btnWarehouse_Click(sender As Object, e As EventArgs) Handles btnWarehouse.Click
            LoadChildForm(New UI.Warehouse.WarehouseIntakeForm(), "Warehouse Intake & RMA Quarantine Inspection", "Dock Intake (GRN & Moving Average Cost) | RMA Quarantine Intake & Inspection (Restock vs. Scrap)")
        End Sub

        Private Sub btnLedger_Click(sender As Object, e As EventArgs) Handles btnLedger.Click
            LoadChildForm(New UI.Accounting.LedgerDashboardForm(), "General Ledger Journal & Chart of Accounts", "Append-only double-entry general ledger, chart of accounts (1010-5500), and fiscal period guard")
        End Sub

        Private Sub btnFinancialReports_Click(sender As Object, e As EventArgs) Handles btnFinancialReports.Click
            LoadChildForm(New UI.Reporting.FinancialReportsForm(), "Financial Reporting & Trial Balance", "Trial balance verification, P&L income statement, and managerial product margin profitability")
        End Sub

    End Class

End Namespace
