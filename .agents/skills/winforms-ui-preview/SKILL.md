---
name: winforms-ui-preview
description: >-
  Automated UI preview rendering, screenshot generation, and visual layout validation for the Merchandising ERP Windows Forms application. Use this skill whenever inspecting UI layout, rendering screenshots for visual verification, testing tab transitions, checking scrollbar and clipping invariants, or adding new preview scenarios for new forms or UserControls.
---

# Windows Forms UI Preview & Visual Verification Runbook

This skill documents the automated offscreen UI preview pipeline for the **Merchandising ERP Accounting Application** (`net10.0-windows` VB.NET).

It enables agents and developers to render, visually inspect, and regression-test every desktop view across multiple resolutions, secondary tabs, interactive states, and modal dialogs without requiring manual screenshots from the user.

---

## 1. Pipeline Architecture

```
[ Developer / Agent CLI ]
          │
          ▼
   preview_ui.ps1                 <-- Unified PowerShell CLI wrapper
          │
          ▼
Merchandising_Accounting_App.PreviewTool  <-- .NET 10 STA-threaded Offscreen Render Engine
          │
          ├──> MainShellForm (Host Container)
          │         └──> Child Dashboard Form (Dock.Fill)
          │
          ├──> Standalone Form / Modal Dialog
          │
          └──> Output to current_ui_screenshots/
                    ├── minimize/   (1280x720 Restored Desktop)
                    └── maximize/   (1920x1080 Maximized Full HD)
```

### Key Technical Characteristics
1. **Isolated STA Message Pump**: Runs on a background Single-Threaded Apartment (`STA`) thread with a 10-second watchdog join timeout (`staThread.Join(10000)`).
2. **Dual-Resolution Standard**:
   - **Restored Desktop (`1280×720`)**: Saved to `current_ui_screenshots/minimize/`. Strictly enforces `FormWindowState.Normal` and exact client size `(1280, 720)`.
   - **Maximized Full HD (`1920×1080`)**: Saved to `current_ui_screenshots/maximize/`. Proves responsive layout scaling across wide displays.
3. **Double Buffering & Layout Synchronization**: Triggers `PerformLayout()`, `Application.DoEvents()`, and an 80ms layout stabilization tick before invoking `Control.DrawToBitmap()`.

---

## 2. CLI Execution Quick Reference

Always execute from the workspace root (`c:\Users\maxsa\Documents\Project_Accounting`):

```powershell
# 1. Render all 36 captures across both resolutions (Standard & Maximized)
powershell -File .\preview_ui.ps1 --all --mode both

# 2. Render only maximized or only restored
powershell -File .\preview_ui.ps1 --all --mode max
powershell -File .\preview_ui.ps1 --all --mode restore

# 3. Render a single form or scenario by alias
powershell -File .\preview_ui.ps1 --form "General Ledger"
powershell -File .\preview_ui.ps1 --form Purchasing
powershell -File .\preview_ui.ps1 --form Sales
powershell -File .\preview_ui.ps1 --form Warehouse
powershell -File .\preview_ui.ps1 --form Reports

# 4. Filter by category
powershell -File .\preview_ui.ps1 --tabs     # All 8 secondary tab views
powershell -File .\preview_ui.ps1 --states   # All interactive states (row selection details)
powershell -File .\preview_ui.ps1 --dialogs  # Modal dialogs (FiscalPeriodForm)
```

---

## 3. How to Add New Views Flexibly

When introducing a new Form or decoupled `UserControl`, add its scenario definition to `RegisterScenarios()` in [Program.vb](../../../Merchandising_Accounting_App.PreviewTool/Program.vb).

### Pattern A: New Top-Level or Base Form
```visualbasic
AvailableScenarios.Add(New PreviewScenario(
    "CustomerManagementForm",                         ' Unique Scenario Key
    ScenarioType.BaseForm,                           ' BaseForm | SecondaryTab | InteractiveState | ModalDialog
    "CustomerManagementForm",                         ' Form/Type Name
    "Customer Management Dashboard",                 ' Scenario Display Name
    "Customer Management",                           ' Output PNG filename (without .png)
    "Customer Accounts & Credit Limits",             ' Shell Header Title
    "Accounts receivable customer directory & terms", ' Shell Header Subtitle
    New String() {"customers", "customer", "crm"},   ' CLI Aliases for --form
    Function() New UI.Customers.CustomerManagementForm() ' Factory Lambda
))
```

### Pattern B: New Tab or Sub-View (Secondary Tab)
```visualbasic
AvailableScenarios.Add(New PreviewScenario(
    "SalesDashboardForm_PriceLists",
    ScenarioType.SecondaryTab,
    "SalesDashboardForm",
    "SalesDashboardForm (Price Lists Tab)",
    "Sales - Price Lists",
    "Sales: Tiered Price Lists",
    "Customer volume tiers and promotional pricing",
    New String() {"sales_pricelists", "pricelists"},
    Function() New UI.Sales.SalesDashboardForm(),
    Sub(f As Form)
        ' SetupAction: Switch to the specific tab before capturing
        Dim sf = TryCast(f, UI.Sales.SalesDashboardForm)
        If sf IsNot Nothing Then
            Dim tc = sf.Controls.Find("tabSales", True).OfType(Of TabControl)().FirstOrDefault()
            If tc IsNot Nothing Then
                tc.SelectedTab = tc.TabPages("tabPriceLists")
            End If
        End If
    End Sub
))
```

### Pattern C: New UserControl View (Decoupled Architecture)
When rendering a self-contained `UserControl`, wrap it inside an auto-docking host form:
```visualbasic
AvailableScenarios.Add(New PreviewScenario(
    "SalesOrderEntryControl",
    ScenarioType.BaseForm,
    "SalesOrderEntryControl",
    "Sales Order Entry Control",
    "Sales - Order Entry View",
    "Sales: Order Intake & Commitments",
    "Line-item entry with instant inventory invariant check",
    New String() {"orderentry", "so_entry"},
    Function()
        Dim host As New Form()
        Dim ctl As New UI.Sales.Controls.SalesOrderEntryControl()
        ctl.Dock = DockStyle.Fill
        host.Controls.Add(ctl)
        Return host
    End Function
))
```

### Pattern D: Interactive / Row-Selection State
```visualbasic
AvailableScenarios.Add(New PreviewScenario(
    "CustomerManagementForm_SelectedLedger",
    ScenarioType.InteractiveState,
    "CustomerManagementForm",
    "Customer Management (Selected Customer AR Ledger)",
    "Customer Management - AR Ledger",
    "Customer: Subledger Details",
    "Row selection displaying aging breakdown and posted invoices",
    New String() {"customer_ar", "customer_ledger"},
    Function() New UI.Customers.CustomerManagementForm(),
    Sub(f As Form)
        Dim cf = TryCast(f, UI.Customers.CustomerManagementForm)
        If cf IsNot Nothing Then
            Dim grid = cf.Controls.Find("dgvCustomers", True).OfType(Of DataGridView)().FirstOrDefault()
            If grid IsNot Nothing AndAlso grid.Rows.Count > 0 Then
                grid.Rows(0).Selected = True
                ' Fire row enter or selection event to trigger detail panel rendering
            End If
        End If
    End Sub
))
```

---

## 4. Visual Layout & Invariant Guidelines

Whenever creating or modifying views, maintain these core UI invariants:

1. **Dual-Resolution Clean Rendering**:
   - Every view must look fully polished at **both 1280×720 (Restored) and 1920×1080 (Maximized)**.
   - At 1280×720, total available content width inside `MainShellForm` is **1000px** ($1280\text{px} - 280\text{px}\text{ sidebar}$).
2. **Zero Horizontal Scrollbars Invariant**:
   - Primary `DataGridView` controls must NEVER trigger horizontal scrollbars at 1280×720.
   - Sum of fixed columns + minimum width of expandable `Fill` columns must remain **$\le \text{Inner Grid Client Width} - 20\text{px}$** (accounting for vertical scrollbar and borders).
   - Programmatic verification: `Assert.IsFalse(hScroll.Visible)` and `Assert.AreEqual(TotalColumns, dgv.DisplayedColumnCount(False))`.
3. **Visual Studio 2026 Designer Compatibility**:
   - In `.vbproj`: Always keep `<ApplicationDefaultFont>Segoe UI, 9.5pt</ApplicationDefaultFont>`.
   - Never put multi-word variable font strings like `Segoe UI Variable Text, 9.5pt` into `.vbproj` (it crashes VS `DesignToolsServer` `FontConverter`).
   - In `.Designer.vb`: Never declare form-level `Me.Font = ...`. Allow controls to inherit ambient font.
   - Standardize `Me.AutoScaleDimensions = New System.Drawing.SizeF(7.0!, 17.0!)` and `Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font`.
4. **Three-Companion File Discipline**:
   - Every Form or UserControl must have its 3 companion files:
     - `ViewName.vb` (code-behind logic)
     - `ViewName.Designer.vb` (designer-generated layout)
     - `ViewName.resx` (XML resources)

---

## 5. Verification Checklist

Before concluding any UI modification task:
1. `dotnet build Merchandising_Accounting_App.slnx -c Debug` $\to$ **0 Warnings, 0 Errors** under `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`.
2. `dotnet test` $\to$ All automated tests pass (including `FormInstantiationTests`).
3. `powershell -File .\preview_ui.ps1 --all --mode both` $\to$ All captures succeed.
4. Verify image dimensions and hash divergence:
   ```powershell
   # Confirm distinct hashes between minimize/ and maximize/
   Get-ChildItem "current_ui_screenshots/minimize/*.png" | ForEach-Object {
       $hMin = (Get-FileHash $_.FullName).Hash
       $hMax = (Get-FileHash (Join-Path "current_ui_screenshots/maximize" $_.Name)).Hash
       if ($hMin -eq $hMax -and $_.Name -ne "Fiscal Period Guard.png") {
           Write-Error "Duplicate image detected: $($_.Name)"
       }
   }
   ```
