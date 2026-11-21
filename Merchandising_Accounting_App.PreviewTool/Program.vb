Option Strict On
Option Explicit On
Option Compare Binary

Imports System
Imports System.Collections.Generic
Imports System.Drawing
Imports System.Drawing.Imaging
Imports System.IO
Imports System.Linq
Imports System.Threading
Imports System.Windows.Forms

Namespace Merchandising_Accounting_App.PreviewTool

    Public Enum ScenarioType
        BaseForm
        SecondaryTab
        InteractiveState
        ModalDialog
    End Enum

    Public Class PreviewScenario
        Public Property Key As String
        Public Property ScenarioType As ScenarioType
        Public Property FormKey As String
        Public Property DisplayName As String
        Public Property FriendlyName As String
        Public Property Title As String
        Public Property Subtitle As String
        Public Property Aliases As List(Of String)
        Public Property Factory As Func(Of Form)
        Public Property SetupAction As Action(Of Form)
        Public Property TargetSize As Size?
        Public Property IsDialog As Boolean = False

        Public Sub New(key As String, scenarioType As ScenarioType, formKey As String, displayName As String, friendlyName As String, title As String, subtitle As String, aliases As IEnumerable(Of String), factory As Func(Of Form), Optional setupAction As Action(Of Form) = Nothing, Optional isDialog As Boolean = False, Optional targetSize As Size? = Nothing)
            Me.Key = key
            Me.ScenarioType = scenarioType
            Me.FormKey = formKey
            Me.DisplayName = displayName
            Me.FriendlyName = friendlyName
            Me.Title = title
            Me.Subtitle = subtitle
            Me.Aliases = New List(Of String)(aliases)
            Me.Factory = factory
            Me.SetupAction = setupAction
            Me.IsDialog = isDialog
            Me.TargetSize = targetSize
        End Sub
    End Class

    Public Module Program

        Private ReadOnly AvailableScenarios As New List(Of PreviewScenario)()
        Public ReadOnly RestoredSize As New Size(1280, 720)
        Public ReadOnly MaximizedSize As New Size(1920, 1080)

        Public Enum RenderMode
            Both
            Restored
            Maximized
        End Enum

        Private Sub InitializeScenarioRegistry()
            AvailableScenarios.Clear()

            ' =========================================================================
            ' A. Base Dashboards & Shell (6 Scenarios)
            ' =========================================================================
            AvailableScenarios.Add(New PreviewScenario(
                "LedgerDashboardForm",
                ScenarioType.BaseForm,
                "LedgerDashboardForm",
                "LedgerDashboardForm",
                "General Ledger",
                "General Ledger Journal & Chart of Accounts",
                "Append-only double-entry general ledger, chart of accounts (1010-5500), and fiscal period guard",
                New String() {"ledger", "ledgerdashboard", "ledgerdashboardform", "generalledger", "general ledger", "gl"},
                Function() New UI.Accounting.LedgerDashboardForm()
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "PurchasingDashboardForm",
                ScenarioType.BaseForm,
                "PurchasingDashboardForm",
                "PurchasingDashboardForm",
                "Purchasing",
                "Purchasing & 3-Way Match Verification",
                "Purchase Orders, GRN Inbound Tracking & 3-Way Match Verification (GRNI Clearing, PPV & Freight Variances)",
                New String() {"purchasing", "purchasingdashboard", "purchasingdashboardform", "po"},
                Function() New UI.Purchasing.PurchasingDashboardForm()
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "SalesDashboardForm",
                ScenarioType.BaseForm,
                "SalesDashboardForm",
                "SalesDashboardForm",
                "Sales",
                "Sales Orders & Inventory Stock Reservation",
                "Customer Orders, Real-Time Stock Availability, Hard-Stop Inventory Guard, and GL Invoicing",
                New String() {"sales", "salesdashboard", "salesdashboardform", "so"},
                Function() New UI.Sales.SalesDashboardForm()
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "WarehouseIntakeForm",
                ScenarioType.BaseForm,
                "WarehouseIntakeForm",
                "WarehouseIntakeForm",
                "Warehouse",
                "Warehouse Intake & RMA Quarantine Inspection",
                "Dock Intake (GRN & Moving Average Cost) | RMA Quarantine Intake & Inspection (Restock vs. Scrap)",
                New String() {"warehouse", "warehouseintake", "warehouseintakeform", "dock", "rma"},
                Function() New UI.Warehouse.WarehouseIntakeForm()
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "FinancialReportsForm",
                ScenarioType.BaseForm,
                "FinancialReportsForm",
                "FinancialReportsForm",
                "Financial Reports",
                "Financial Reporting & Trial Balance",
                "Trial balance verification, P&L income statement, and managerial product margin profitability",
                New String() {"financialreports", "financialreportsform", "financial reports", "reports", "reporting", "trialbalance"},
                Function() New UI.Reporting.FinancialReportsForm()
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "MainShellForm",
                ScenarioType.BaseForm,
                "MainShellForm",
                "MainShellForm",
                "MainShellForm",
                "Merchandising ERP Accounting System",
                "Desktop Accounting Shell",
                New String() {"mainshell", "mainshellform", "shell"},
                Function() New UI.MainShellForm()
            ))

            ' =========================================================================
            ' B. Secondary Tab Scenarios (7 Scenarios — Eliminating All Tab Blindspots)
            ' =========================================================================
            AvailableScenarios.Add(New PreviewScenario(
                "PurchasingDashboardForm_PurchaseOrders",
                ScenarioType.SecondaryTab,
                "PurchasingDashboardForm",
                "PurchasingDashboardForm (Purchase Orders Tab)",
                "Purchasing - Purchase Orders",
                "Purchasing: Purchase Orders Management",
                "Vendor Purchase Orders, Approval Workflow & Line-Item Commitments",
                New String() {"purchasing_pos", "purchasing_purchaseorders", "purchaseorders"},
                Function() New UI.Purchasing.PurchasingDashboardForm(),
                Sub(f As Form)
                    Dim pf As UI.Purchasing.PurchasingDashboardForm = TryCast(f, UI.Purchasing.PurchasingDashboardForm)
                    If pf IsNot Nothing Then
                        Dim tc As TabControl = pf.Controls.Find("tabPurchasing", True).OfType(Of TabControl)().FirstOrDefault()
                        If tc IsNot Nothing Then
                            Dim tp As TabPage = tc.TabPages("tabPurchaseOrders")
                            If tp IsNot Nothing Then tc.SelectedTab = tp
                        End If
                    End If
                End Sub
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "PurchasingDashboardForm_GRNList",
                ScenarioType.SecondaryTab,
                "PurchasingDashboardForm",
                "PurchasingDashboardForm (GRN List Tab)",
                "Purchasing - Goods Receipt Notes",
                "Purchasing: Inbound Goods Receipt Notes (GRN)",
                "Inbound GRN Receipts, PO Price Reconciliation & Pending Supplier Bills",
                New String() {"purchasing_grn", "purchasing_grns", "grnlist"},
                Function() New UI.Purchasing.PurchasingDashboardForm(),
                Sub(f As Form)
                    Dim pf As UI.Purchasing.PurchasingDashboardForm = TryCast(f, UI.Purchasing.PurchasingDashboardForm)
                    If pf IsNot Nothing Then
                        Dim tc As TabControl = pf.Controls.Find("tabPurchasing", True).OfType(Of TabControl)().FirstOrDefault()
                        If tc IsNot Nothing Then
                            Dim tp As TabPage = tc.TabPages("tabGRNList")
                            If tp IsNot Nothing Then tc.SelectedTab = tp
                        End If
                    End If
                End Sub
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "SalesDashboardForm_Invoices",
                ScenarioType.SecondaryTab,
                "SalesDashboardForm",
                "SalesDashboardForm (Invoices Tab)",
                "Sales - Invoices",
                "Sales: Invoices & Accounts Receivable",
                "Sales Invoices, AR Subledger Tracking & Balanced 4-Line General Ledger Posting Preview",
                New String() {"sales_invoices", "salesinvoices", "invoices"},
                Function() New UI.Sales.SalesDashboardForm(),
                Sub(f As Form)
                    Dim sf As UI.Sales.SalesDashboardForm = TryCast(f, UI.Sales.SalesDashboardForm)
                    If sf IsNot Nothing Then
                        Dim tc As TabControl = sf.Controls.Find("tabMain", True).OfType(Of TabControl)().FirstOrDefault()
                        If tc IsNot Nothing Then
                            Dim tp As TabPage = tc.TabPages("tabInvoices")
                            If tp IsNot Nothing Then tc.SelectedTab = tp
                        End If
                    End If
                End Sub
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "SalesDashboardForm_StockMonitor",
                ScenarioType.SecondaryTab,
                "SalesDashboardForm",
                "SalesDashboardForm (Stock Availability Tab)",
                "Sales - Stock Availability",
                "Sales: Real-Time Stock Availability Guard",
                "Full Product Catalog Availability, Stock Badges & Non-Negative Inventory Policy Verification",
                New String() {"sales_stock", "sales_stockmonitor", "stockmonitor"},
                Function() New UI.Sales.SalesDashboardForm(),
                Sub(f As Form)
                    Dim sf As UI.Sales.SalesDashboardForm = TryCast(f, UI.Sales.SalesDashboardForm)
                    If sf IsNot Nothing Then
                        Dim tc As TabControl = sf.Controls.Find("tabMain", True).OfType(Of TabControl)().FirstOrDefault()
                        If tc IsNot Nothing Then
                            Dim tp As TabPage = tc.TabPages("tabStockMonitor")
                            If tp IsNot Nothing Then tc.SelectedTab = tp
                        End If
                    End If
                End Sub
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "WarehouseIntakeForm_Quarantine",
                ScenarioType.SecondaryTab,
                "WarehouseIntakeForm",
                "WarehouseIntakeForm (RMA Quarantine Tab)",
                "Warehouse - RMA Quarantine",
                "Warehouse: RMA Quarantine Intake & Inspection",
                "Non-Valuated Operational Quarantine & Stage-2 Disposition (Restock to 1300 vs Scrap to 5300)",
                New String() {"warehouse_quarantine", "quarantine", "rma_quarantine"},
                Function() New UI.Warehouse.WarehouseIntakeForm(),
                Sub(f As Form)
                    Dim wf As UI.Warehouse.WarehouseIntakeForm = TryCast(f, UI.Warehouse.WarehouseIntakeForm)
                    If wf IsNot Nothing Then
                        Dim tc As TabControl = wf.Controls.Find("tabMain", True).OfType(Of TabControl)().FirstOrDefault()
                        If tc IsNot Nothing Then
                            Dim tp As TabPage = tc.TabPages("tabQuarantine")
                            If tp IsNot Nothing Then tc.SelectedTab = tp
                        End If
                    End If
                End Sub
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "WarehouseIntakeForm_Adjustments",
                ScenarioType.SecondaryTab,
                "WarehouseIntakeForm",
                "WarehouseIntakeForm (Stock Adjustments Tab)",
                "Warehouse - Stock Adjustments",
                "Warehouse: Physical Inventory Adjustments & Shrinkage",
                "Physical Count Variance Entry, GL Double-Entry Distribution Preview & Audit Logging",
                New String() {"warehouse_adjustments", "stock_adjustments", "adjustments"},
                Function() New UI.Warehouse.WarehouseIntakeForm(),
                Sub(f As Form)
                    Dim wf As UI.Warehouse.WarehouseIntakeForm = TryCast(f, UI.Warehouse.WarehouseIntakeForm)
                    If wf IsNot Nothing Then
                        Dim tc As TabControl = wf.Controls.Find("tabMain", True).OfType(Of TabControl)().FirstOrDefault()
                        If tc IsNot Nothing Then
                            Dim tp As TabPage = tc.TabPages("tabStockAdjustments")
                            If tp IsNot Nothing Then tc.SelectedTab = tp
                        End If
                    End If
                End Sub
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "FinancialReportsForm_ProfitLoss",
                ScenarioType.SecondaryTab,
                "FinancialReportsForm",
                "FinancialReportsForm (Profit & Loss Tab)",
                "Financial Reports - Profit & Loss",
                "Financial Reports: Statement of Profit or Loss (Income Statement)",
                "Classified Revenue, COGS, Gross Margin, PPV, Freight Variance, Damaged Stock & Net Profit",
                New String() {"reports_pnl", "reports_profitloss", "profitloss", "pnl"},
                Function() New UI.Reporting.FinancialReportsForm(),
                Sub(f As Form)
                    Dim rf As UI.Reporting.FinancialReportsForm = TryCast(f, UI.Reporting.FinancialReportsForm)
                    If rf IsNot Nothing Then
                        Dim tc As TabControl = rf.Controls.Find("tabReports", True).OfType(Of TabControl)().FirstOrDefault()
                        If tc IsNot Nothing Then
                            Dim tp As TabPage = tc.TabPages("tabProfitLoss")
                            If tp IsNot Nothing Then tc.SelectedTab = tp
                        End If
                    End If
                End Sub
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "FinancialReportsForm_ProductMargins",
                ScenarioType.SecondaryTab,
                "FinancialReportsForm",
                "FinancialReportsForm (Product Margins Tab)",
                "Financial Reports - Product Margins",
                "Financial Reports: Managerial Product Margin Profitability",
                "SKU Margin Analysis: Units Sold, Revenue, COGS, Inbound Freight, PPV Impact & Net Margin %",
                New String() {"reports_margins", "reports_productmargins", "productmargins", "margins"},
                Function() New UI.Reporting.FinancialReportsForm(),
                Sub(f As Form)
                    Dim rf As UI.Reporting.FinancialReportsForm = TryCast(f, UI.Reporting.FinancialReportsForm)
                    If rf IsNot Nothing Then
                        Dim tc As TabControl = rf.Controls.Find("tabReports", True).OfType(Of TabControl)().FirstOrDefault()
                        If tc IsNot Nothing Then
                            Dim tp As TabPage = tc.TabPages("tabProductMargins")
                            If tp IsNot Nothing Then tc.SelectedTab = tp
                        End If
                    End If
                End Sub
            ))

            ' =========================================================================
            ' C. Key Interactive State Scenarios (3 Scenarios)
            ' =========================================================================
            AvailableScenarios.Add(New PreviewScenario(
                "LedgerDashboardForm_EntryDetails",
                ScenarioType.InteractiveState,
                "LedgerDashboardForm",
                "LedgerDashboardForm (Supplier Bill 4-Line Distribution State)",
                "General Ledger - Distribution Lines",
                "General Ledger: Multi-Line Journal Distribution",
                "Entry 102 Distribution Lines: GRNI Clearing, PPV Unfavorable, Freight & Accounts Payable",
                New String() {"gl_distribution", "gl_details", "ledger_details"},
                Function() New UI.Accounting.LedgerDashboardForm(),
                Sub(f As Form)
                    Dim lf As UI.Accounting.LedgerDashboardForm = TryCast(f, UI.Accounting.LedgerDashboardForm)
                    If lf IsNot Nothing Then
                        Dim dgv As DataGridView = lf.Controls.Find("dgvJournalEntries", True).OfType(Of DataGridView)().FirstOrDefault()
                        If dgv IsNot Nothing AndAlso dgv.Rows.Count > 1 Then
                            dgv.ClearSelection()
                            dgv.Rows(1).Selected = True
                            dgv.CurrentCell = dgv.Rows(1).Cells(0)
                        End If
                    End If
                End Sub
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "PurchasingDashboardForm_ThreeWayMatch_PPV",
                ScenarioType.InteractiveState,
                "PurchasingDashboardForm",
                "PurchasingDashboardForm (3-Way Match PPV & Freight Reconciliation State)",
                "Purchasing - 3-Way Match PPV",
                "Purchasing: 3-Way Match Live Variance Reconciliation",
                "Active Match Row 1 Selection: Unfavorable PPV (+$60.00), Inbound Freight ($45.00) & Net Payable ($855.00)",
                New String() {"purchasing_ppv", "threewaymatch_ppv", "3way_ppv"},
                Function() New UI.Purchasing.PurchasingDashboardForm(),
                Sub(f As Form)
                    Dim pf As UI.Purchasing.PurchasingDashboardForm = TryCast(f, UI.Purchasing.PurchasingDashboardForm)
                    If pf IsNot Nothing Then
                        Dim dgv As DataGridView = pf.Controls.Find("dgvThreeWayMatch", True).OfType(Of DataGridView)().FirstOrDefault()
                        If dgv IsNot Nothing AndAlso dgv.Rows.Count > 1 Then
                            dgv.ClearSelection()
                            dgv.Rows(1).Selected = True
                            dgv.CurrentCell = dgv.Rows(1).Cells(0)
                        End If
                    End If
                End Sub
            ))

            AvailableScenarios.Add(New PreviewScenario(
                "SalesDashboardForm_StockSimulation",
                ScenarioType.InteractiveState,
                "SalesDashboardForm",
                "SalesDashboardForm (Stock Invariant Blocked Simulation State)",
                "Sales - Stock Simulation",
                "Sales: Hard-Stop Zero Negative Inventory Guard Simulation",
                "Simulation Execution: Over-dispatch triggers InsufficientStockException and red invariant guard banner",
                New String() {"sales_simulation", "stock_simulation", "hardstop_simulation"},
                Function() New UI.Sales.SalesDashboardForm(),
                Sub(f As Form)
                    Dim sf As UI.Sales.SalesDashboardForm = TryCast(f, UI.Sales.SalesDashboardForm)
                    If sf IsNot Nothing Then
                        Dim tc As TabControl = sf.Controls.Find("tabMain", True).OfType(Of TabControl)().FirstOrDefault()
                        If tc IsNot Nothing Then
                            Dim tp As TabPage = tc.TabPages("tabStockMonitor")
                            If tp IsNot Nothing Then tc.SelectedTab = tp
                        End If
                        Dim cmb As ComboBox = sf.Controls.Find("cmbSimProduct", True).OfType(Of ComboBox)().FirstOrDefault()
                        Dim num As NumericUpDown = sf.Controls.Find("numSimQty", True).OfType(Of NumericUpDown)().FirstOrDefault()
                        Dim btn As Button = sf.Controls.Find("btnRunSim", True).OfType(Of Button)().FirstOrDefault()
                        If cmb IsNot Nothing AndAlso cmb.Items.Count > 0 Then cmb.SelectedIndex = 0
                        If num IsNot Nothing Then num.Value = 500D
                        If btn IsNot Nothing Then btn.PerformClick()
                    End If
                End Sub
            ))

            ' =========================================================================
            ' D. Modal Dialog Scenarios (1 Scenario)
            ' =========================================================================
            AvailableScenarios.Add(New PreviewScenario(
                "FiscalPeriodForm",
                ScenarioType.ModalDialog,
                "FiscalPeriodForm",
                "FiscalPeriodForm (Modal Dialog)",
                "Fiscal Period Guard",
                "Fiscal Period Status & Month-End Close Guard",
                "Statutory posting date immutability, open fiscal periods & permanent close seal policy",
                New String() {"fiscalperiod", "fiscalperiodform", "period", "periodguard", "periodclose", "dialog"},
                Function() New UI.Accounting.FiscalPeriodForm(),
                Nothing,
                True,
                New Size(540, 440)
            ))
        End Sub

        <STAThread()>
        Public Function Main(args() As String) As Integer
            ' Ensure local database is initialized with master data and WAL pragmas for previews
            Try
                Global.Merchandising_Accounting_App.Data.DatabaseInitializer.InitializeDatabase()
            Catch ex As Exception
                Console.WriteLine($"[Warning] Database bootstrap in PreviewTool: {ex.Message}")
            End Try

            InitializeScenarioRegistry()

            ' Parse Arguments
            Dim requestedName As String = Nothing
            Dim filterAll As Boolean = False
            Dim filterTabs As Boolean = False
            Dim filterStates As Boolean = False
            Dim filterDialogs As Boolean = False
            Dim useShell As Boolean = True
            Dim mode As RenderMode = RenderMode.Maximized
            Dim outputDirectory As String = "current_ui_screenshots"
            Dim delayMilliseconds As Integer = 350
            Dim showHelp As Boolean = False
            Dim customResolution As Size? = Nothing

            Dim i As Integer = 0
            While i < args.Length
                Dim arg As String = args(i).Trim()
                Dim lowerArg As String = arg.ToLowerInvariant()

                If lowerArg = "--help" OrElse lowerArg = "-help" OrElse lowerArg = "/help" OrElse lowerArg = "-h" OrElse lowerArg = "/?" Then
                    showHelp = True
                ElseIf lowerArg = "--all" OrElse lowerArg = "-all" OrElse lowerArg = "/all" OrElse lowerArg = "-a" Then
                    filterAll = True
                ElseIf lowerArg = "--tabs" OrElse lowerArg = "-tabs" Then
                    filterTabs = True
                ElseIf lowerArg = "--states" OrElse lowerArg = "-states" OrElse lowerArg = "--interactive" OrElse lowerArg = "-interactive" Then
                    filterStates = True
                ElseIf lowerArg = "--dialogs" OrElse lowerArg = "-dialogs" OrElse lowerArg = "--modals" OrElse lowerArg = "-modals" Then
                    filterDialogs = True
                ElseIf lowerArg = "--shell" OrElse lowerArg = "-shell" OrElse lowerArg = "/shell" OrElse lowerArg = "-s" Then
                    useShell = True
                ElseIf lowerArg = "--standalone" OrElse lowerArg = "-standalone" OrElse lowerArg = "/standalone" Then
                    useShell = False
                ElseIf lowerArg = "--resolution" OrElse lowerArg = "-resolution" OrElse lowerArg = "-r" Then
                    If i + 1 < args.Length Then
                        i += 1
                        Dim resStr As String = args(i).Trim()
                        Dim parts As String() = resStr.ToLowerInvariant().Split("x"c)
                        If parts.Length = 2 Then
                            Dim w As Integer
                            Dim h As Integer
                            If Integer.TryParse(parts(0), w) AndAlso Integer.TryParse(parts(1), h) Then
                                customResolution = New Size(w, h)
                            End If
                        End If
                    End If
                ElseIf lowerArg.StartsWith("--resolution=", StringComparison.OrdinalIgnoreCase) Then
                    Dim resStr As String = arg.Substring(arg.IndexOf("="c) + 1).Trim()
                    Dim parts As String() = resStr.ToLowerInvariant().Split("x"c)
                    If parts.Length = 2 Then
                        Dim w As Integer
                        Dim h As Integer
                        If Integer.TryParse(parts(0), w) AndAlso Integer.TryParse(parts(1), h) Then
                            customResolution = New Size(w, h)
                        End If
                    End If
                ElseIf lowerArg = "--mode" OrElse lowerArg = "-mode" OrElse lowerArg = "--state" OrElse lowerArg = "-state" OrElse lowerArg = "-m" Then
                    If i + 1 < args.Length Then
                        i += 1
                        Dim modeVal As String = args(i).Trim().ToLowerInvariant()
                        If modeVal = "min" OrElse modeVal = "minimize" OrElse modeVal = "restore" OrElse modeVal = "restored" OrElse modeVal = "normal" Then
                            mode = RenderMode.Restored
                        ElseIf modeVal = "max" OrElse modeVal = "maximize" OrElse modeVal = "maximized" Then
                            mode = RenderMode.Maximized
                        Else
                            mode = RenderMode.Both
                        End If
                    End If
                ElseIf lowerArg.StartsWith("--mode=", StringComparison.OrdinalIgnoreCase) OrElse lowerArg.StartsWith("--state=", StringComparison.OrdinalIgnoreCase) Then
                    Dim modeVal As String = arg.Substring(arg.IndexOf("="c) + 1).Trim().ToLowerInvariant()
                    If modeVal = "min" OrElse modeVal = "minimize" OrElse modeVal = "restore" OrElse modeVal = "restored" OrElse modeVal = "normal" Then
                        mode = RenderMode.Restored
                    ElseIf modeVal = "max" OrElse modeVal = "maximize" OrElse modeVal = "maximized" Then
                        mode = RenderMode.Maximized
                    Else
                        mode = RenderMode.Both
                    End If
                ElseIf lowerArg = "--form" OrElse lowerArg = "-form" OrElse lowerArg = "/form" OrElse lowerArg = "-f" Then
                    If i + 1 < args.Length Then
                        i += 1
                        requestedName = args(i).Trim()
                    Else
                        Console.Error.WriteLine("[ERROR] Missing value for --form parameter.")
                        Return 1
                    End If
                ElseIf lowerArg.StartsWith("--form=", StringComparison.OrdinalIgnoreCase) OrElse lowerArg.StartsWith("-form=", StringComparison.OrdinalIgnoreCase) Then
                    requestedName = arg.Substring(arg.IndexOf("="c) + 1).Trim()
                ElseIf lowerArg = "--output" OrElse lowerArg = "-output" OrElse lowerArg = "/output" OrElse lowerArg = "-o" Then
                    If i + 1 < args.Length Then
                        i += 1
                        outputDirectory = args(i).Trim()
                    Else
                        Console.Error.WriteLine("[ERROR] Missing value for --output parameter.")
                        Return 1
                    End If
                ElseIf lowerArg.StartsWith("--output=", StringComparison.OrdinalIgnoreCase) OrElse lowerArg.StartsWith("-output=", StringComparison.OrdinalIgnoreCase) Then
                    outputDirectory = arg.Substring(arg.IndexOf("="c) + 1).Trim()
                ElseIf lowerArg = "--delay" OrElse lowerArg = "-delay" OrElse lowerArg = "/delay" Then
                    If i + 1 < args.Length AndAlso Integer.TryParse(args(i + 1), delayMilliseconds) Then
                        i += 1
                    End If
                ElseIf Not arg.StartsWith("-"c) AndAlso Not arg.StartsWith("/"c) AndAlso String.IsNullOrEmpty(requestedName) Then
                    requestedName = arg
                End If
                i += 1
            End While

            If showHelp Then
                PrintUsage()
                Return 0
            End If

            ' Resolve Scenarios to render
            Dim targets As New List(Of PreviewScenario)()
            If filterDialogs Then
                targets.AddRange(AvailableScenarios.Where(Function(s) s.ScenarioType = ScenarioType.ModalDialog))
            ElseIf filterStates Then
                targets.AddRange(AvailableScenarios.Where(Function(s) s.ScenarioType = ScenarioType.InteractiveState))
            ElseIf filterTabs Then
                targets.AddRange(AvailableScenarios.Where(Function(s) s.ScenarioType = ScenarioType.BaseForm OrElse s.ScenarioType = ScenarioType.SecondaryTab))
            ElseIf Not String.IsNullOrEmpty(requestedName) Then
                targets.AddRange(ResolveScenarios(requestedName))
                If targets.Count = 0 Then
                    Console.Error.WriteLine($"[ERROR] Unknown form or scenario '{requestedName}'.")
                    Console.Error.WriteLine("Available scenarios:")
                    For Each item In AvailableScenarios
                        Console.Error.WriteLine($"  - [{item.ScenarioType}] {item.Key} / {item.FriendlyName} (aliases: {String.Join(", ", item.Aliases)})")
                    Next
                    Return 1
                End If
            Else
                targets.AddRange(AvailableScenarios)
            End If

            ' Ensure output directory exists
            Dim fullOutputDir As String = Path.GetFullPath(outputDirectory)
            Dim minOutputDir As String = Path.Combine(fullOutputDir, "minimize")
            Dim maxOutputDir As String = Path.Combine(fullOutputDir, "maximize")

            If Not Directory.Exists(fullOutputDir) Then Directory.CreateDirectory(fullOutputDir)
            If Not Directory.Exists(minOutputDir) Then Directory.CreateDirectory(minOutputDir)
            If Not Directory.Exists(maxOutputDir) Then Directory.CreateDirectory(maxOutputDir)

            Console.WriteLine("================================================================================")
            Console.WriteLine(" WinForms Automated UI Preview Tool (.NET 10 Windows Forms)")
            Console.WriteLine("================================================================================")
            Console.WriteLine($" Output directory : {fullOutputDir}")
            Console.WriteLine($" Render Mode      : {mode}")
            Console.WriteLine($" Shell container  : {If(useShell, "Enabled (hosted in MainShellForm)", "Disabled (standalone form)")}")
            Console.WriteLine($" Target Scenarios : {targets.Count} scenario(s)")
            Console.WriteLine("--------------------------------------------------------------------------------")

            ' Initialize WinForms Application configuration
            Try
                Dim fontName As String = "Segoe UI"
                Using testFont As New Font("Segoe UI Variable Text", 9.5F, FontStyle.Regular, GraphicsUnit.Point)
                    If testFont.Name.Equals("Segoe UI Variable Text", StringComparison.OrdinalIgnoreCase) Then
                        fontName = "Segoe UI Variable Text"
                    End If
                End Using
                Application.SetColorMode(SystemColorMode.System)
                Application.SetHighDpiMode(HighDpiMode.PerMonitorV2)
                Application.EnableVisualStyles()
                Application.SetCompatibleTextRenderingDefault(False)
                Application.SetDefaultFont(New Font(fontName, 9.5F, FontStyle.Regular, GraphicsUnit.Point))
            Catch
                ' Ignore if already initialized
            End Try

            Dim totalSuccess As Integer = 0
            Dim totalFailed As Integer = 0

            For Each scenario In targets
                Console.WriteLine()
                Console.WriteLine($"[{scenario.ScenarioType}] Rendering: {scenario.FriendlyName} ({scenario.Key})")

                Dim restoredTargetSize As Size = RestoredSize
                Dim maximizedTargetSize As Size = MaximizedSize

                If customResolution.HasValue Then
                    restoredTargetSize = customResolution.Value
                    maximizedTargetSize = customResolution.Value
                ElseIf scenario.IsDialog AndAlso scenario.TargetSize.HasValue Then
                    restoredTargetSize = scenario.TargetSize.Value
                    maximizedTargetSize = scenario.TargetSize.Value
                End If

                ' 1. Restored Capture (1280x720)
                If mode = RenderMode.Restored OrElse mode = RenderMode.Both Then
                    Dim minFileName As String = $"{scenario.FriendlyName}.png"
                    Dim minFullPath As String = Path.Combine(minOutputDir, minFileName)
                    Console.Write($"  -> [Restored {restoredTargetSize.Width}x{restoredTargetSize.Height}] {minFileName} ... ")
                    Dim okMin As Boolean = RenderScenarioOnStaThread(scenario, minFullPath, restoredTargetSize, useShell, delayMilliseconds)
                    If okMin Then
                        Console.WriteLine("OK")
                        ' Also copy/save primary key filename in root output dir if in restored mode
                        If mode = RenderMode.Restored Then
                            Dim rootFileName As String = $"{scenario.Key}.png"
                            Dim rootFullPath As String = Path.Combine(fullOutputDir, rootFileName)
                            SafeCopyFile(minFullPath, rootFullPath)
                            If useShell AndAlso Not scenario.IsDialog Then
                                Dim shellPath As String = Path.Combine(fullOutputDir, $"{scenario.Key}_Shell.png")
                                SafeCopyFile(minFullPath, shellPath)
                            End If
                        End If
                        totalSuccess += 1
                    Else
                        Console.WriteLine("FAILED")
                        totalFailed += 1
                    End If
                End If

                ' 2. Maximized Capture (1920x1018)
                If mode = RenderMode.Maximized OrElse mode = RenderMode.Both Then
                    Dim maxFileName As String = $"{scenario.FriendlyName}.png"
                    Dim maxFullPath As String = Path.Combine(maxOutputDir, maxFileName)
                    Console.Write($"  -> [Maximized {maximizedTargetSize.Width}x{maximizedTargetSize.Height}] {maxFileName} ... ")
                    Dim okMax As Boolean = RenderScenarioOnStaThread(scenario, maxFullPath, maximizedTargetSize, useShell, delayMilliseconds)
                    If okMax Then
                        Console.WriteLine("OK")
                        ' Copy/save primary key filename in root output dir
                        Dim rootFileName As String = $"{scenario.Key}.png"
                        Dim rootFullPath As String = Path.Combine(fullOutputDir, rootFileName)
                        SafeCopyFile(maxFullPath, rootFullPath)
                        If useShell AndAlso Not scenario.IsDialog Then
                            Dim shellPath As String = Path.Combine(fullOutputDir, $"{scenario.Key}_Shell.png")
                            SafeCopyFile(maxFullPath, shellPath)
                        End If
                        totalSuccess += 1
                    Else
                        Console.WriteLine("FAILED")
                        totalFailed += 1
                    End If
                End If
            Next

            Console.WriteLine()
            Console.WriteLine("================================================================================")
            Console.WriteLine($" Finished. Success: {totalSuccess}, Failed: {totalFailed}")
            Console.WriteLine("================================================================================")

            Return If(totalFailed > 0, 1, 0)
        End Function

        Private Sub SafeCopyFile(sourcePath As String, destPath As String)
            For attempt As Integer = 1 To 10
                Try
                    File.Copy(sourcePath, destPath, True)
                    Return
                Catch ex As IOException When attempt < 10
                    Thread.Sleep(50)
                End Try
            Next
        End Sub

        Private Function ResolveScenarios(nameOrAlias As String) As List(Of PreviewScenario)
            Dim results As New List(Of PreviewScenario)()
            Dim cleanName As String = nameOrAlias.Trim().ToLowerInvariant()

            ' 1. Identify if the requested name matches any BaseForm or its aliases
            Dim matchingFormKeys As New HashSet(Of String)(StringComparer.OrdinalIgnoreCase)
            For Each s In AvailableScenarios
                If s.ScenarioType = ScenarioType.BaseForm Then
                    If s.FormKey.Equals(nameOrAlias, StringComparison.OrdinalIgnoreCase) OrElse
                       s.FormKey.ToLowerInvariant() = cleanName OrElse
                       s.FriendlyName.Equals(nameOrAlias, StringComparison.OrdinalIgnoreCase) OrElse
                       s.FriendlyName.ToLowerInvariant() = cleanName OrElse
                       s.DisplayName.Equals(nameOrAlias, StringComparison.OrdinalIgnoreCase) OrElse
                       s.DisplayName.ToLowerInvariant() = cleanName Then
                        matchingFormKeys.Add(s.FormKey)
                    Else
                        For Each al In s.Aliases
                            If al.Equals(cleanName, StringComparison.OrdinalIgnoreCase) Then
                                matchingFormKeys.Add(s.FormKey)
                                Exit For
                            End If
                        Next
                    End If
                End If
            Next

            ' If a module was matched, return all scenarios belonging to that FormKey
            If matchingFormKeys.Count > 0 Then
                For Each s In AvailableScenarios
                    If matchingFormKeys.Contains(s.FormKey) Then
                        results.Add(s)
                    End If
                Next
                Return results
            End If

            ' 2. Otherwise, match individual scenario keys, friendly names, or aliases
            For Each s In AvailableScenarios
                If s.FormKey.Equals(nameOrAlias, StringComparison.OrdinalIgnoreCase) OrElse
                   s.FormKey.ToLowerInvariant() = cleanName OrElse
                   s.Key.Equals(nameOrAlias, StringComparison.OrdinalIgnoreCase) OrElse
                   s.Key.ToLowerInvariant() = cleanName OrElse
                   s.FriendlyName.Equals(nameOrAlias, StringComparison.OrdinalIgnoreCase) OrElse
                   s.FriendlyName.ToLowerInvariant() = cleanName OrElse
                   s.DisplayName.Equals(nameOrAlias, StringComparison.OrdinalIgnoreCase) OrElse
                   s.DisplayName.ToLowerInvariant() = cleanName Then
                    results.Add(s)
                Else
                    For Each al In s.Aliases
                        If al.Equals(cleanName, StringComparison.OrdinalIgnoreCase) Then
                            results.Add(s)
                            Exit For
                        End If
                    Next
                End If
            Next

            Return results
        End Function

        Private Function RenderScenarioOnStaThread(scenario As PreviewScenario, outputPath As String, targetSize As Size, useShell As Boolean, delayMs As Integer) As Boolean
            Dim capturedSuccess As Boolean = False
            Dim capturedException As Exception = Nothing

            Dim staThread As New Thread(Sub()
                Try
                    Dim formToRun As Form = Nothing
                    Dim timer As New System.Windows.Forms.Timer()
                    timer.Interval = Math.Max(150, delayMs)

                    If scenario.IsDialog Then
                        ' Modal Dialog: rendered headlessly offscreen at fixed client size without ShowDialog()
                        Dim dialogForm As Form = scenario.Factory.Invoke()
                        dialogForm.WindowState = FormWindowState.Normal
                        dialogForm.StartPosition = FormStartPosition.Manual
                        dialogForm.Location = New Point(-15000, -15000)
                        dialogForm.ShowInTaskbar = False
                        dialogForm.ClientSize = targetSize

                        AddHandler timer.Tick, Sub(s, ev)
                            timer.Stop()
                            timer.Dispose()
                            Try
                                dialogForm.WindowState = FormWindowState.Normal
                                dialogForm.ClientSize = targetSize
                                scenario.SetupAction?.Invoke(dialogForm)
                                dialogForm.PerformLayout()
                                Application.DoEvents()
                                Thread.Sleep(80)
                                Application.DoEvents()
                                dialogForm.Refresh()

                                Using bmp As New Bitmap(targetSize.Width, targetSize.Height)
                                    dialogForm.DrawToBitmap(bmp, New Rectangle(0, 0, targetSize.Width, targetSize.Height))
                                    bmp.Save(outputPath, ImageFormat.Png)
                                End Using
                                capturedSuccess = True
                            Catch ex As Exception
                                capturedException = ex
                            Finally
                                Application.ExitThread()
                            End Try
                        End Sub

                        formToRun = dialogForm
                    ElseIf scenario.Key.Equals("MainShellForm", StringComparison.OrdinalIgnoreCase) Then
                        Dim targetForm As Form = scenario.Factory.Invoke()
                        targetForm.WindowState = FormWindowState.Normal
                        targetForm.StartPosition = FormStartPosition.Manual
                        targetForm.Location = New Point(-15000, -15000)
                        targetForm.ShowInTaskbar = False
                        targetForm.FormBorderStyle = FormBorderStyle.None
                        targetForm.ClientSize = targetSize

                        AddHandler timer.Tick, Sub(s, ev)
                            timer.Stop()
                            timer.Dispose()
                            Try
                                targetForm.WindowState = FormWindowState.Normal
                                targetForm.ClientSize = targetSize
                                scenario.SetupAction?.Invoke(targetForm)
                                targetForm.PerformLayout()
                                Application.DoEvents()
                                Thread.Sleep(80)
                                Application.DoEvents()
                                targetForm.Refresh()

                                Using bmp As New Bitmap(targetSize.Width, targetSize.Height)
                                    targetForm.DrawToBitmap(bmp, New Rectangle(0, 0, targetSize.Width, targetSize.Height))
                                    bmp.Save(outputPath, ImageFormat.Png)
                                End Using
                                capturedSuccess = True
                            Catch ex As Exception
                                capturedException = ex
                            Finally
                                Application.ExitThread()
                            End Try
                        End Sub

                        formToRun = targetForm
                    ElseIf useShell Then
                        ' Hosted in MainShellForm desktop container
                        Dim shellForm As New UI.MainShellForm()
                        shellForm.WindowState = FormWindowState.Normal
                        shellForm.AutoLoadDefaultModule = False
                        shellForm.StartPosition = FormStartPosition.Manual
                        shellForm.Location = New Point(-15000, -15000)
                        shellForm.ShowInTaskbar = False
                        shellForm.FormBorderStyle = FormBorderStyle.None
                        shellForm.ClientSize = targetSize

                        Dim childForm As Form = scenario.Factory.Invoke()

                        AddHandler timer.Tick, Sub(s, ev)
                            timer.Stop()
                            timer.Dispose()
                            Try
                                shellForm.WindowState = FormWindowState.Normal
                                shellForm.ClientSize = targetSize
                                shellForm.LoadChildForm(childForm, scenario.Title, scenario.Subtitle)
                                scenario.SetupAction?.Invoke(childForm)

                                shellForm.PerformLayout()
                                childForm.PerformLayout()
                                Application.DoEvents()
                                Thread.Sleep(80)
                                Application.DoEvents()
                                shellForm.Refresh()

                                Using bmp As New Bitmap(targetSize.Width, targetSize.Height)
                                    shellForm.DrawToBitmap(bmp, New Rectangle(0, 0, targetSize.Width, targetSize.Height))
                                    bmp.Save(outputPath, ImageFormat.Png)
                                End Using
                                capturedSuccess = True
                            Catch ex As Exception
                                capturedException = ex
                            Finally
                                Application.ExitThread()
                            End Try
                        End Sub

                        formToRun = shellForm
                    Else
                        ' Standalone target dashboard form hosted in offscreen preview container
                        Dim hostForm As New Form()
                        hostForm.WindowState = FormWindowState.Normal
                        hostForm.FormBorderStyle = FormBorderStyle.None
                        hostForm.StartPosition = FormStartPosition.Manual
                        hostForm.Location = New Point(-15000, -15000)
                        hostForm.ShowInTaskbar = False
                        hostForm.ClientSize = targetSize

                        Dim targetForm As Form = scenario.Factory.Invoke()

                        AddHandler timer.Tick, Sub(s, ev)
                            timer.Stop()
                            timer.Dispose()
                            Try
                                hostForm.WindowState = FormWindowState.Normal
                                hostForm.ClientSize = targetSize
                                targetForm.TopLevel = False
                                targetForm.FormBorderStyle = FormBorderStyle.None
                                targetForm.Dock = DockStyle.Fill
                                hostForm.Controls.Add(targetForm)
                                targetForm.Show()

                                scenario.SetupAction?.Invoke(targetForm)
                                hostForm.PerformLayout()
                                targetForm.PerformLayout()
                                Application.DoEvents()
                                Thread.Sleep(80)
                                Application.DoEvents()
                                targetForm.Refresh()

                                Using bmp As New Bitmap(targetSize.Width, targetSize.Height)
                                    hostForm.DrawToBitmap(bmp, New Rectangle(0, 0, targetSize.Width, targetSize.Height))
                                    bmp.Save(outputPath, ImageFormat.Png)
                                End Using
                                capturedSuccess = True
                            Catch ex As Exception
                                capturedException = ex
                            Finally
                                Application.ExitThread()
                            End Try
                        End Sub

                        formToRun = hostForm
                    End If

                    timer.Start()
                    Application.Run(formToRun)
                Catch ex As Exception
                    If Not capturedSuccess Then
                        capturedException = ex
                    End If
                End Try
            End Sub)

            staThread.SetApartmentState(ApartmentState.STA)
            staThread.Start()

            ' Thread Watchdog: Wait up to 10 seconds to prevent any thread hangs
            If Not staThread.Join(10000) Then
                staThread.Interrupt()
                Console.Error.WriteLine()
                Console.Error.WriteLine($"[ERROR] Watchdog timeout (10000ms) exceeded while rendering {scenario.DisplayName}")
                Return False
            End If

            If capturedException IsNot Nothing Then
                Console.Error.WriteLine()
                Console.Error.WriteLine($"[ERROR] Exception while rendering {scenario.DisplayName}: {capturedException.Message}")
                Console.Error.WriteLine(capturedException.StackTrace)
                If capturedException.InnerException IsNot Nothing Then
                    Console.Error.WriteLine($"Inner: {capturedException.InnerException.Message}")
                    Console.Error.WriteLine(capturedException.InnerException.StackTrace)
                End If
                Return False
            End If

            Return capturedSuccess
        End Function

        Private Sub PrintUsage()
            Console.WriteLine("Merchandising Accounting ERP - Windows Forms UI Preview Tool")
            Console.WriteLine("Renders WinForms headlessly to PNG offscreen on STA thread.")
            Console.WriteLine()
            Console.WriteLine("Usage:")
            Console.WriteLine("  dotnet run --project Merchandising_Accounting_App.PreviewTool -- [options]")
            Console.WriteLine("  .\preview_ui.ps1 [options]")
            Console.WriteLine()
            Console.WriteLine("Options:")
            Console.WriteLine("  --all                       Render all 17 scenarios (default)")
            Console.WriteLine("  --tabs                      Render all base forms and secondary tab views")
            Console.WriteLine("  --states, --interactive     Render interactive state scenarios (PPV variance, distributions, simulation)")
            Console.WriteLine("  --dialogs, --modals         Render modal dialog scenarios (FiscalPeriodForm)")
            Console.WriteLine("  --form <Name>               Render scenarios for specified form (e.g. Ledger, Purchasing, Sales, Warehouse)")
            Console.WriteLine("  --mode <restore|max|both>   Target render mode (default: max)")
            Console.WriteLine("  --resolution <WxH>          Custom viewport resolution (e.g. 1920x1080, 1280x720)")
            Console.WriteLine("  --shell                     Render form inside MainShellForm desktop container (default: true)")
            Console.WriteLine("  --standalone                Render standalone dashboard form without shell container")
            Console.WriteLine("  --output <Dir>              Target screenshot directory (default: current_ui_screenshots)")
            Console.WriteLine("  --delay <ms>                Wait time in milliseconds after Shown before capture (default: 350)")
            Console.WriteLine("  --help, -h                  Show this help message")
            Console.WriteLine()
            Console.WriteLine("Available Scenarios:")
            For Each s In AvailableScenarios
                Console.WriteLine($"  [{s.ScenarioType,-16}] {s.Key,-40} -> {s.FriendlyName}")
            Next
            Console.WriteLine()
            Console.WriteLine("Examples:")
            Console.WriteLine("  .\preview_ui.ps1 --all")
            Console.WriteLine("  .\preview_ui.ps1 --tabs")
            Console.WriteLine("  .\preview_ui.ps1 --states")
            Console.WriteLine("  .\preview_ui.ps1 --dialogs")
            Console.WriteLine("  .\preview_ui.ps1 --form Purchasing")
            Console.WriteLine("  .\preview_ui.ps1 --form 'General Ledger' --mode both")
        End Sub

    End Module

End Namespace
