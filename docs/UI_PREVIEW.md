# Windows Forms Automated UI Preview & Screenshot Tooling

## 1. Executive Summary

In desktop application development using .NET 10 Windows Forms (`net10.0-windows`) and Visual Basic .NET, inspecting UI forms (such as `LedgerDashboardForm` or `MainShellForm`) typically requires manual user interaction, launching Visual Studio, or taking manual desktop screenshots.

This document presents the technical research, architectural design, and operational guide for an **automated, unattended WinForms UI preview and screenshot mechanism** implemented in `Merchandising_Accounting_App.PreviewTool`. The tooling renders forms offscreen on dedicated Single-Threaded Apartment (STA) threads, executes all runtime mock data bindings and layouts, and exports high-fidelity PNG screenshots into `current_ui_screenshots/` without requiring interactive user sessions or the Visual Studio IDE.

---

## 2. Technical Research & Findings

### 2.1 Limitations of the Visual Studio Designer

Historically, developers relied on the Visual Studio Windows Forms Designer surface to preview form layouts. However, for modern automated agent workflows and continuous integration (CI/CD), the VS Designer presents insurmountable barriers:

1. **Out-of-Process Architecture (`DesignToolsServer`):** Since .NET Core 3.1 and through .NET 10, the Visual Studio WinForms Designer executes in an out-of-process design-time server. It cannot be launched headlessly from the command line or scripted via simple CLI tooling.
2. **Absence of Headless Export Capabilities:** Visual Studio offers no built-in API or CLI command to export a form designer view to an image file (PNG/JPEG) without opening the graphical IDE.
3. **Design-Time vs. Runtime Data Binding:** The designer only executes `InitializeComponent()`. Crucial visual elements—such as Master Chart of Accounts grid rows, General Ledger journal entries, trial balance tallies, and KPI indicators—are populated dynamically during `Form.Load` or via ViewModel services. The VS designer displays empty grids and blank labels, failing to reflect true runtime presentation.

### 2.2 Comparison of Programmatic Capture Techniques

To render WinForms without user interaction, three programmatic approaches were evaluated:

| Technique | Description | Advantages | Disadvantages / Limitations |
| :--- | :--- | :--- | :--- |
| **Win32 `BitBlt` Screen Scraping** | Captures pixels directly from the screen Device Context (DC) via Win32 GDI. | High speed; captures exact desktop rendering including OS window frames. | **Fails in headless/offscreen environments.** Requires the window to be physically on-screen, top-most, and unobstructed. Occlusion or screen savers produce black or corrupted rectangles. |
| **Win32 `PrintWindow` (`PW_RENDERFULLCONTENT`)** | Sends a print message to the target `HWND` asking the Desktop Window Manager (DWM) to render its surface into a memory DC. | Works for partially occluded top-level windows. | Highly inconsistent across Windows DWM compositor versions. Fails for offscreen coordinates (e.g. coordinates outside active virtual displays) where DWM composition trees are unallocated. |
| **Managed `Control.DrawToBitmap` via GDI+** | Managed WinForms method that dispatches `WM_PRINT` / `WM_PRINTCLIENT` recursively down the control tree into a GDI+ `Bitmap`. | **Deterministic and fully offscreen.** Does not require physical monitor focus or on-screen positioning. Captures full control hierarchy, data grids, fonts, and colors. | Does not render non-client DWM title bars natively (which are OS-rendered). In standard WinForms, controls with complex child splitters require appropriate layout initialization. |

**Conclusion:** Managed `Control.DrawToBitmap` combined with an offscreen host harness is the only reliable, deterministic technique for unattended automation.

### 2.3 STA Threading & Message Loop Requirements

Windows Forms is inherently thread-affine and requires the Single-Threaded Apartment (STA) COM model:
- **STA Thread Affinity:** All UI controls must be instantiated, manipulated, and disposed on a thread configured with `Thread.SetApartmentState(ApartmentState.STA)`.
- **Message Pump Requirement (`Application.Run`):** Creating controls without an active message pump leaves Win32 window procedures unserviced. Window layout messages (`WM_CREATE`, `WM_SIZE`, `WM_LAYOUT`, `WM_PAINT`) and child control data-binding notifications will not process, leaving DataGridViews and SplitContainers unrendered.
- **Message Loop Termination Isolation:** Calling `Application.ExitThread()` terminates the message pump for the calling thread. In .NET WinForms, attempting to restart `Application.Run` on an already-terminated thread leads to message loop corruption. **Architecture Invariant:** Each form capture session executes on a dedicated, freshly spawned STA worker thread that joins and cleanly terminates upon completion.

### 2.4 Why Windows PowerShell 5.1 Fails on .NET 10

Windows PowerShell (`powershell.exe`, version 5.1) is hosted on the legacy .NET Framework 4.8 CLR (CLR 4.0). By contrast, `Merchandising_Accounting_App` is built on modern .NET 10 (CoreCLR).
- Attempting to load .NET 10 assemblies into Windows PowerShell 5.1 via `Add-Type` fails immediately with `BadImageFormatException` or assembly loader exceptions due to incompatible CLR runtimes.
- While PowerShell 7+ (`pwsh.exe`) runs on CoreCLR, it is an optional installation and not guaranteed to be present on standard Windows workstations.
- **Resolution:** A dedicated .NET 10 executable console tool (`Merchandising_Accounting_App.PreviewTool`) runs natively on .NET 10 CoreCLR. The root script `preview_ui.ps1` acts as a lightweight wrapper calling `dotnet run`, ensuring 100% compatibility across any PowerShell version.

---

## 3. Tool Architecture & Implementation

### 3.1 Solution Topology

The preview tooling is organized as an independent tooling project registered in the Visual Studio XML solution `Merchandising_Accounting_App.slnx`:

```
c:\Users\maxsa\Documents\Project_Accounting\
├── Merchandising_Accounting_App.slnx          <-- Solution registering app, PreviewTool & Tests
├── preview_ui.ps1                            <-- Root PowerShell CLI launcher script
├── current_ui_screenshots/                   <-- Standardized output folder for generated PNGs
│   ├── minimize/                             <-- 1280x720 Restored Desktop Viewport (17 PNGs)
│   ├── maximize/                             <-- 1920x1080 Maximized Full HD Viewport (17 PNGs)
│   ├── manual_screenshot_minimize/           <-- User-captured reference benchmarks (720p)
│   └── manual_screenshot_maximize/           <-- User-captured reference benchmarks (1080p)
├── Merchandising_Accounting_App/             <-- Core desktop accounting application
│   └── UI/
│       ├── Accounting/LedgerDashboardForm.vb
│       ├── Purchasing/PurchasingDashboardForm.vb
│       ├── Sales/SalesDashboardForm.vb
│       ├── Warehouse/WarehouseIntakeForm.vb
│       ├── Reporting/FinancialReportsForm.vb
│       └── MainShellForm.vb
├── Merchandising_Accounting_App.PreviewTool/ <-- Headless STA preview engine (.NET 10)
│   └── Program.vb                            <-- 17 scenarios, offscreen capture & dual-mode
└── Merchandising_Accounting_App.Tests/       <-- Automated test suite (15 tests)
    └── FormInstantiationTests.vb             <-- Layout, handle & dual-resolution tests
```
│       ├── Sales/SalesDashboardForm.vb
│       ├── Warehouse/WarehouseIntakeForm.vb
│       ├── Reporting/FinancialReportsForm.vb
│       └── MainShellForm.vb
└── Merchandising_Accounting_App.PreviewTool/  <-- Dedicated preview tool
    ├── Merchandising_Accounting_App.PreviewTool.vbproj
    └── Program.vb
```

### 3.2 Compiler & Typing Invariants

In strict adherence to repository guidelines (`GEMINI.md`), `Merchandising_Accounting_App.PreviewTool.vbproj` enforces:
- `<TargetFramework>net10.0-windows</TargetFramework>`
- `<OutputType>Exe</OutputType>`
- `<UseWindowsForms>true</UseWindowsForms>`
- `<OptionStrict>On</OptionStrict>`
- `<OptionExplicit>On</OptionExplicit>`
- `<OptionCompare>Binary</OptionCompare>`
- `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`

### 3.3 Offscreen Rendering Pipeline

The rendering workflow in `Program.vb` follows a 7-stage pipeline:

```
[CLI Request] (--form LedgerDashboardForm or --all)
      │
      ▼
[STA Thread Creation] (Thread.SetApartmentState = STA)
      │
      ▼
[Offscreen Host Initialization]
  - Standalone Mode: Form hosted inside offscreen borderless container (Location: -15000, -15000)
  - Shell Mode: Hosted inside MainShellForm (Size: 1500x850, Dock: Fill)
      │
      ▼
[Data-Binding & Lifecycle Induction]
  - Invokes Form.OnLoad to populate ViewModels, DataGridViews, and indicators
      │
      ▼
[Message Pump Synchronization]
  - Application.Run(formToRun) pumps Win32 messages
  - 350ms Timer allows layout passes and DoEvents synchronization
      │
      ▼
[GDI+ Bitmap Capture]
  - form.DrawToBitmap(bmp, bounds)
  - bmp.Save(outputPath, ImageFormat.Png)
      │
      ▼
[Clean Teardown]
  - Application.ExitThread() exits message loop cleanly with exit code 0
```

---

## 4. Usage Guide

### 4.1 CLI Commands via PowerShell Launcher

The primary recommended command to preview forms is the root PowerShell script `preview_ui.ps1`:

#### Preview General Ledger Dashboard (Default / Standalone)
```powershell
.\preview_ui.ps1 --form LedgerDashboardForm
```
*Generates:* `current_ui_screenshots/LedgerDashboardForm.png`

#### Preview General Ledger Hosted Inside Desktop Shell
```powershell
.\preview_ui.ps1 --form LedgerDashboardForm --shell
```
*Generates:* `current_ui_screenshots/LedgerDashboardForm_Shell.png`

#### Render All 34 UI Captures (Dual Resolution)
```powershell
powershell -File .\preview_ui.ps1 --all --mode both
```
*Renders 17 scenarios at 1280×720 into `current_ui_screenshots/minimize/` and 17 scenarios at 1920×1080 into `current_ui_screenshots/maximize/`.*

#### Render Restored or Maximized Mode Only
```powershell
powershell -File .\preview_ui.ps1 --all --mode restore   # Generates 1280x720 only in minimize/
powershell -File .\preview_ui.ps1 --all --mode max       # Generates 1920x1080 only in maximize/
```

#### Category Filters
```powershell
powershell -File .\preview_ui.ps1 --tabs     # All 7 secondary tab views
powershell -File .\preview_ui.ps1 --states   # All 3 interactive states (row-selection detail lines)
powershell -File .\preview_ui.ps1 --dialogs  # Modal dialogs (FiscalPeriodForm)
```

#### Render a Single Module or Scenario by Alias
```powershell
powershell -File .\preview_ui.ps1 --form "General Ledger" --mode both
powershell -File .\preview_ui.ps1 --form purchasing --mode both
powershell -File .\preview_ui.ps1 --form sales --mode both
powershell -File .\preview_ui.ps1 --form warehouse --mode both
powershell -File .\preview_ui.ps1 --form reports --mode both
powershell -File .\preview_ui.ps1 --form fiscalperiod
```

### 4.2 Direct Execution via `dotnet run`

Alternatively, execute using the .NET CLI without PowerShell:

```shell
dotnet run --project Merchandising_Accounting_App.PreviewTool -- --all --mode both
dotnet run --project Merchandising_Accounting_App.PreviewTool -- --form "General Ledger"
dotnet run --project Merchandising_Accounting_App.PreviewTool -- --tabs
```

### 4.3 Full Scenario Catalog (17 Distinct Targets)

| Category | Key | Module / Output File | Supported Aliases |
| :--- | :--- | :--- | :--- |
| **Base Form** | `LedgerDashboardForm` | `General Ledger.png` | `ledger`, `gl`, `generalledger` |
| **Base Form** | `PurchasingDashboardForm` | `Purchasing.png` | `purchasing`, `po`, `purchasingdashboard` |
| **Base Form** | `SalesDashboardForm` | `Sales.png` | `sales`, `so`, `salesdashboard` |
| **Base Form** | `WarehouseIntakeForm` | `Warehouse.png` | `warehouse`, `dock`, `rma` |
| **Base Form** | `FinancialReportsForm` | `Financial Reports.png` | `reports`, `trialbalance`, `reporting` |
| **Base Form** | `MainShellForm` | `MainShellForm.png` | `mainshell`, `shell` |
| **Secondary Tab** | `PurchasingDashboardForm_PurchaseOrders` | `Purchasing - Purchase Orders.png` | `purchasing_pos`, `purchaseorders` |
| **Secondary Tab** | `PurchasingDashboardForm_GRNList` | `Purchasing - Goods Receipt Notes.png` | `purchasing_grn`, `grnlist` |
| **Secondary Tab** | `SalesDashboardForm_Invoices` | `Sales - Invoices.png` | `sales_invoices`, `invoices` |
| **Secondary Tab** | `SalesDashboardForm_StockMonitor` | `Sales - Stock Availability.png` | `sales_stock`, `stockmonitor` |
| **Secondary Tab** | `WarehouseIntakeForm_Quarantine` | `Warehouse - RMA Quarantine.png` | `warehouse_quarantine`, `rma_quarantine` |
| **Secondary Tab** | `FinancialReportsForm_ProfitLoss` | `Financial Reports - Profit & Loss.png` | `reports_pnl`, `profitloss` |
| **Secondary Tab** | `FinancialReportsForm_ProductMargins` | `Financial Reports - Product Margins.png` | `reports_margins`, `productmargins` |
| **Interactive** | `LedgerDashboardForm_EntryDetails` | `General Ledger - Distribution Lines.png` | `ledger_details`, `entrydetails` |
| **Interactive** | `PurchasingDashboardForm_ThreeWayMatch_PPV` | `Purchasing - 3-Way Match PPV.png` | `purchasing_ppv`, `threewaymatch` |
| **Interactive** | `SalesDashboardForm_StockSimulation` | `Sales - Stock Simulation.png` | `sales_simulation`, `stocksimulation` |
| **Modal Dialog** | `FiscalPeriodForm` | `Fiscal Period Guard.png` | `fiscalperiod`, `perioddialog` |

---

## 5. Verification & Quality Assurance

To verify that the preview tool adheres to all project quality gates:

1. **Zero-Warning Build:**
   ```powershell
   dotnet build Merchandising_Accounting_App.slnx -c Debug
   ```
   *Result:* 0 Errors, 0 Warnings under `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` and `<OptionStrict>On</OptionStrict>`.

2. **Automated Unit & Layout Tests:**
   ```powershell
   dotnet test
   ```
   *Result:* 15/15 tests passing, including programmatic assertions verifying that `HorizontalScrollBar.Visible = False` and `DisplayedColumnCount = 9/9` on `dgvJournalEntries` and `dgvJournalLines` at 1280×720.

3. **Automated Headless Preview Capture:**
   ```powershell
   powershell -File .\preview_ui.ps1 --all --mode both
   ```
   *Result:* Exits with code 0; renders all 34 captures across `current_ui_screenshots/minimize/` and `current_ui_screenshots/maximize/`.

4. **Cryptographic Image Independence:**
   All 16 shell-hosted views exhibit completely distinct SHA-256 hashes between `minimize/` and `maximize/`, confirming true responsive layout execution rather than static image resizing.

