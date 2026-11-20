Imports Merchandising_Accounting_App.Data
Imports Merchandising_Accounting_App.Services

Namespace UI

    Public Class MainShellForm

        Private _currentContent As Control

        <System.ComponentModel.DesignerSerializationVisibility(System.ComponentModel.DesignerSerializationVisibility.Hidden)>
        Public Property AutoLoadDefaultModule As Boolean = True

        Public Sub LoadContent(contentControl As Control, title As String, subtitle As String)
            If _currentContent IsNot Nothing Then
                pnlContentHost.Controls.Remove(_currentContent)
                _currentContent.Dispose()
                _currentContent = Nothing
            End If

            _currentContent = contentControl
            lblCurrentModule.Text = title
            lblModuleSubtitle.Text = subtitle
            lblModuleSubtitle.Top = lblCurrentModule.Bottom + 4
            pnlHeader.Height = Math.Max(88, lblModuleSubtitle.Bottom + 12)

            contentControl.Dock = DockStyle.Fill
            contentControl.Font = Me.Font
            pnlContentHost.Controls.Add(contentControl)
            pnlContentHost.Tag = contentControl
            contentControl.BringToFront()
            contentControl.Show()
        End Sub

        Public Sub LoadChildForm(childForm As Form, title As String, subtitle As String)
            childForm.TopLevel = False
            childForm.FormBorderStyle = FormBorderStyle.None
            LoadContent(childForm, title, subtitle)
        End Sub

        Private Sub MainShellForm_Load(sender As Object, e As EventArgs) Handles Me.Load
            Try
                DatabaseInitializer.InitializeDatabase()
                Dim periodService As New PeriodService()
                Dim activePeriod = periodService.GetActiveFiscalPeriod()
                tsLblDbStatus.Text = "Database: merchandising_erp.db (Connected - WAL)"
                tsLblDbStatus.ForeColor = System.Drawing.Color.Empty
                If activePeriod IsNot Nothing Then
                    tsLblFiscalPeriod.Text = $"Active Period: {activePeriod.FiscalYear}-P{activePeriod.PeriodNumber:D2}"
                Else
                    tsLblFiscalPeriod.Text = "Active Period: None (Open Period Required)"
                End If
            Catch ex As Exception
                tsLblDbStatus.Text = $"Database Error: {ex.Message}"
                tsLblDbStatus.ForeColor = System.Drawing.Color.Firebrick
                tsLblFiscalPeriod.Text = "Active Period: Unresolved (Database Failure)"
            End Try

            ' Load Purchasing as initial view if enabled and no child form is already loaded
            If AutoLoadDefaultModule AndAlso _currentContent Is Nothing Then
                LoadChildForm(New UI.Purchasing.PurchasingDashboardForm(), "Purchasing & 3-Way Match Verification", "Purchase Orders, GRN Inbound Tracking & 3-Way Match Verification (GRNI Clearing, PPV & Freight Variances)")
            End If
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
