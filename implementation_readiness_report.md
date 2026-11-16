# Implementation Readiness Report (Stage 9)

**Target:** `docs/designs/mvp_erp_accounting.md` (`DetailedRange: Lines 297..497`)  
**Standard:** `resources/document_standards/implementation-readiness-standard.md`  
**Execution Mode:** Consolidated Uninterrupted Full Flow

---

## Reviewer Status

| Reviewer | Status | Focus Lens |
| :--- | :--- | :--- |
| `egm-implementation-readiness` | completed | Pragmatic tech lead analysis, Zero-Decision Rule enforcement, actionability, and phasing |
| `egm-codebase-details` | completed | Codebase drift, non-existent references, target file lists, and test infrastructure |
| `egm-style` | completed | Copy editing, claim calibration, and fluff reduction across implementation tasks |

---

## Prioritized Findings (Blockers -> Major -> Nits)

### 1. Absence of Incremental Phasing and Buildable Checkpoints - Severity: Blocker
* **Reviewer Lens:** `egm-implementation-readiness`
* **Affected Scope:** Entire Detailed Implementation Plan (Lines 297–468)
* **Discrepancy Description**: The plan violates the Phasing & Checkpoints mandate of the Target Standard. Rather than partitioning work into incremental phases where each phase leaves the codebase buildable and testable, the document presents a static file catalog organized by architectural layer. An implementer cannot determine execution order, dependency sequencing, or incremental test gates.
* **Rationale & Guidance**: Restructure the plan into sequential phases, each with explicit deliverables and verification gates:
  - **Phase 1: Project Hardening & Solution Setup** (Solution creation, `.vbproj` strict typing, test project reference linking).
  - **Phase 2: Domain Entities, Enums & Computation Engines** (Pure invariant math, balance validation, unit conversion) + Phase 2 Unit Tests.
  - **Phase 3: Persistence Layer & Schema Bootstrapping** (DDL execution, COA seeding, connection lifecycle, repositories) + Integration Tests.
  - **Phase 4: Application Services & Document Workflows** (P2P, O2C, Returns, Reporting workflows with transactional boundaries).
  - **Phase 5: Presentation Layer (WinForms Shell & Dashboard Views)** (Navigation shell, operational input forms, data grid bindings).
  - **Phase 6: End-to-End CUJ Verification** (Full workflow testing and code coverage validation).

### 2. Non-Existent Solution Path at Repository Root & Omission of Solution File Modification - Severity: Blocker
* **Reviewer Lens:** `egm-implementation-readiness`, `egm-codebase-details`
* **Affected Scope:** Lines 444, 473, 479; `Merchandising_Accounting_App.slnx`
* **Discrepancy Description**: The verification commands specify `dotnet build Merchandising_Accounting_App.slnx` and `dotnet test Merchandising_Accounting_App.slnx`. However:
  1. `Merchandising_Accounting_App.slnx` does not exist at the repository root; it resides in `Merchandising_Accounting_App/`. Running `dotnet build` from root fails immediately with `MSB1009`.
  2. The existing solution file contains only `Merchandising_Accounting_App.vbproj`. The plan creates a sibling test project `Merchandising_Accounting_App.Tests/` (Line 444), but omits the solution file from the Target Files list. Without updating the solution file, `dotnet test` executes against the WinExe project only and runs 0 tests.
* **Rationale & Guidance**: Explicitly list `Merchandising_Accounting_App.slnx` under files to modify/move (moving it to repository root and binding both main and test projects), and ensure CLI verification commands point to the correct path.

### 3. Omission of `Application.myapp` Causing Build Failure on `Form1` Deletion - Severity: Blocker
* **Reviewer Lens:** `egm-codebase-details`
* **Affected Scope:** Lines 412–415; `Application.myapp` (Lines 1–10); `Application.Designer.vb` (Lines 30–33)
* **Discrepancy Description**: Line 412 instructs deleting `Form1.vb`, `Form1.Designer.vb`, and `Form1.resx`. In VB Windows Forms SDK projects using `<MyType>WindowsForms</MyType>`, the startup form is governed by `Application.myapp` (`<MainForm>Form1</MainForm>`), which auto-generates `Me.MainForm = Form1` in `Application.Designer.vb`. Deleting `Form1` without modifying `Application.myapp` emits compiler error `BC30451: 'Form1' is not declared` and halts the build. Line 414 vaguely instructs to "update StartupObject or My.MyApplication", leaving an ambiguous choice to the implementer.
* **Rationale & Guidance**: Explicitly list `Merchandising_Accounting_App/My Project/Application.myapp` under files to modify, replacing `<MainForm>Form1</MainForm>` with `<MainForm>MainShellForm</MainForm>`.

### 4. Omission of Concrete SQLite DDL, Column Types, and Seed Data - Severity: Blocker
* **Reviewer Lens:** `egm-implementation-readiness`
* **Affected Scope:** Section 2, `Merchandising_Accounting_App/Data/DatabaseBootstrap.vb` (Lines 318–322)
* **Discrepancy Description**: `DatabaseBootstrap.vb` is tasked with executing DDL to create 27 tables and seeding the Chart of Accounts and initial fiscal period. However, no SQL DDL script or seed dataset is provided. SQLite lacks native `DECIMAL(18,4)` and enum types. The implementer must guess whether currency is stored as `TEXT`, `INTEGER` cents, or `REAL`, whether foreign keys cascade or restrict, what check constraints exist (`CHECK (TotalQtyOnHand >= 0)`), and what exact account codes and equity accounts (e.g., `3000 Retained Earnings`) must be seeded.
* **Rationale & Guidance**: Document exact SQL DDL scripts and seed rows directly in the plan or DDL migration resources. Specify exact column types (e.g., `DECIMAL(18,4)` mapped to `TEXT` or scaled `INTEGER`), table indexes, and full Chart of Accounts seed entries (code, name, type, normal balance).

### 5. Contradictory Persistence Stack and Unpinned Package Versions - Severity: Blocker
* **Reviewer Lens:** `egm-implementation-readiness`
* **Affected Scope:** Section 1 (Line 308) vs. Architecture Diagram (Line 98)
* **Discrepancy Description**: Line 98 specifies `Data & Storage Layer (SQLite / EF Core 10)`, while line 308 instructs adding `Microsoft.Data.Sqlite (v9.0+)` and `Dapper (v2.1+)` with hand-rolled ADO.NET repositories. Leaving both EF Core 10 and Dapper in the document forces the developer to make an architectural choice on ORM vs. micro-ORM. Floating version bounds (`v9.0+`, `v2.1+`) violate deterministic build standards.
* **Rationale & Guidance**: Reconcile the architecture diagram and implementation plan to a single standard. If Dapper + `Microsoft.Data.Sqlite` is the target, remove EF Core from line 98 and pin exact NuGet versions (e.g., `Microsoft.Data.Sqlite` Version `9.0.2`, `Dapper` Version `2.1.35`). Alternatively, if EF Core 10 is adopted, standardize on `Microsoft.EntityFrameworkCore.Sqlite` and remove hand-rolled SQL DDL and boilerplate repository classes.

### 6. Missing Windows Forms Designer and Resource Files in Target Write Scope - Severity: Major
* **Reviewer Lens:** `egm-implementation-readiness`, `egm-codebase-details`
* **Affected Scope:** Section 5, User Interface Forms (Lines 407–441)
* **Discrepancy Description**: The file list includes `MainShellForm.vb` and `MainShellForm.Designer.vb`, but lists only the bare `.vb` file for `PurchasingDashboardForm.vb`, `SalesDashboardForm.vb`, `WarehouseIntakeForm.vb`, `LedgerDashboardForm.vb`, and `FinancialReportsForm.vb`. In Windows Forms .NET, forms require companion `.Designer.vb` and `.resx` files. In a Phase 5 write allowlist, creating these companion files would violate write bounds.
* **Rationale & Guidance**: Update the Target File list to explicitly enumerate all companion files (`*.Designer.vb` and `*.resx`) for all six forms.

### 7. Omission of Payment, Receipt, and Refund Journal Generation in `LedgerPostingEngine` - Severity: Major
* **Reviewer Lens:** `egm-implementation-readiness`, `egm-codebase-details`
* **Affected Scope:** Section 3, `Merchandising_Accounting_App/Domain/Engines/LedgerPostingEngine.vb` (Lines 373–381)
* **Discrepancy Description**: The posting table matrix explicitly defines transactions for Vendor Payment Vouchers (Debit 2000 / Credit 1020), Customer Payment Receipts (Debit 1020 / Credit 1200), and Customer Refund Disbursements (Debit 2150 / Credit 1020). However, `LedgerPostingEngine.vb` in Section 3 only specifies methods for GRN, Supplier Bill, Sales Invoice, and Credit Note, providing zero method signatures for payments, receipts, and refund disbursements.
* **Rationale & Guidance**: Add explicit method signatures and return types to `LedgerPostingEngine.vb` for:
  - `GeneratePaymentVoucherJournal(voucher As PaymentVoucher) As JournalEntry`
  - `GeneratePaymentReceiptJournal(receipt As PaymentReceipt) As JournalEntry`
  - `GenerateCustomerRefundJournal(refund As CustomerRefundVoucher) As JournalEntry`

### 8. Ambiguous Repository Method Signatures and Missing Transaction Propagation Pattern - Severity: Major
* **Reviewer Lens:** `egm-implementation-readiness`, `egm-codebase-details`
* **Affected Scope:** Section 2, Persistence & Repositories (Lines 323–347)
* **Discrepancy Description**: Repositories are described only with high-level summaries. The plan does not define repository interfaces, method names, parameter types, or return types. The plan also omits defining how active database transactions (`SqliteTransaction`) are passed or shared across multiple repository calls when committing a multi-entity document workflow.
* **Rationale & Guidance**: Specify explicit method contracts for each repository and define the transaction propagation mechanism (e.g., methods accepting an optional `transaction As SqliteTransaction = Nothing`, or a Unit of Work pattern managed by `DatabaseContext`).

### 9. Disappearance of `PeriodLockManager` / `PeriodService` from Service Layer and Repositories - Severity: Major
* **Reviewer Lens:** `egm-implementation-readiness`, `egm-codebase-details`
* **Affected Scope:** Section 4, Application Services (Lines 383–404) vs. Architecture (Lines 89, 118, 340, 434)
* **Discrepancy Description**: The architecture highlights `PeriodLockManager (PeriodService)` as a core workflow service guarding the open/closed period boundary. However, Section 4 completely omits `PeriodService.vb` from the list of files to create, and none of the repositories expose methods to query, create, open, or close records in `AccountingPeriods`.
* **Rationale & Guidance**: Add `Merchandising_Accounting_App/Services/PeriodService.vb` to the Application Services list and `AccountingPeriodRepository.vb` (or explicit methods in `LedgerRepository`) defining period administration and validation contracts.

### 10. Unresolved TOCTOU Concurrency Hazard and Restock Costing in `InventoryCostingEngine` - Severity: Major
* **Reviewer Lens:** `egm-implementation-readiness`, `egm-codebase-details`
* **Affected Scope:** Section 3, `InventoryCostingEngine.vb` (Lines 365–372)
* **Discrepancy Description**: Line 368 defines `ValidateStockAvailable(productId, requiredQty)` on `InventoryCostingEngine`. A pure domain engine should not take entity IDs or perform database lookups. More critically, checking stock before entering the write transaction leaves a TOCTOU race condition open. Additionally, `CalculateNewMovingAverageCost` only defines the formula for PO intake, completely omitting how moving average unit cost is updated when inspected customer returns are restocked to inventory.
* **Rationale & Guidance**: Redefine `InventoryCostingEngine` methods as pure functions: `ValidateSufficientStock(currentQty As Integer, requiredQty As Integer) As Boolean`. Specify the atomic conditional update in `ProductRepository`: `UPDATE Products SET TotalQtyOnHand = TotalQtyOnHand - @Qty WHERE ProductId = @Id AND TotalQtyOnHand >= @Qty;` throwing `InsufficientStockException` if rows affected equals 0. Add `CalculateRestockMovingAverageCost(currentQty, currentCost, restockedQty, originalInvoiceCost)` to `InventoryCostingEngine`.

### 11. Undefined Validation Rules and Trigger Conditions for `ThreeWayMatchException` - Severity: Major
* **Reviewer Lens:** `egm-codebase-details`
* **Affected Scope:** Lines 362, 387, 461
* **Discrepancy Description**: `ThreeWayMatchException` is listed as a domain exception, but price and freight variances are automatically routed to `5100 PPV` and `5200 Freight Variance`. The document never specifies what failure condition triggers `ThreeWayMatchException` (e.g., `BilledQty > ReceivedQty`, vendor mismatch, or matching against already-settled GRN lines).
* **Rationale & Guidance**: Explicitly define the trigger conditions: throw `ThreeWayMatchException` if `BilledQty > ReceivedQty`, if vendor IDs mismatch, or if a target `GRNLine` is already settled.

### 12. Neglect of Test Infrastructure in Automated Test Suite Plan - Severity: Major
* **Reviewer Lens:** `egm-codebase-details`
* **Affected Scope:** Section 6, Automated Test Suite (Lines 444–468)
* **Discrepancy Description**: The plan defines integration test suites verifying document workflows, GL debit/credit entries, and database persistence, but lists zero test infrastructure files (e.g., `TestDatabaseFixture.vb`, `BaseRepositoryTest.vb`, or SQLite in-memory factory helpers).
* **Rationale & Guidance**: Document test infrastructure files (`TestDatabaseFixture.vb`) establishing how in-memory or temporary SQLite test databases are initialized, seeded with schema/COA, and disposed between test executions.

### 13. Abstract Test Plan Lacking Concrete Method Signatures and Scenarios - Severity: Major
* **Reviewer Lens:** `egm-implementation-readiness`
* **Affected Scope:** Section 6, Automated Test Suite (Lines 442–468)
* **Discrepancy Description**: The test plan outlines broad topics without defining specific test cases, fixture names, inputs, or assertions. Missing entirely are test files and scenarios for `OrderToCashServiceTests.vb` (fulfillment stock block and customer payment receipts), `PeriodLockManagerTests.vb` (closed period posting rejection), and database bootstrap schema verification.
* **Rationale & Guidance**: Expand the test plan to enumerate concrete test fixtures with specific test case names, numeric inputs, and assertions (e.g., `InventoryCostingEngineTests`: `CalculateNewMovingAverageCost_WithExistingStock_CalculatesWeightedAverage(currentQty=100, currentCost=10.00, receivedQty=50, poPrice=13.00, expectedCost=11.00)`).

### 14. Uncalibrated Invariant Claims in Test and Workflow Rationales - Severity: Major
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Lines 398, 452, 462, 467
* **Discrepancy Description**: `CALIBRATE`. Claims like "Guarantees that customer returns undergo physical inspection", "no unbalanced journal entries can ever be created", and "Guarantees returns accounting prevents inventory contamination" are uncalibrated and dramatic.
* **Rationale & Guidance**: `CALIBRATE` to verifiable behaviors: "Enforces recorded inspection dispositions prior to credit note generation or restock", "Verifies that `LedgerPostingEngine` rejects out-of-balance entries", and "Verifies segregation of quarantined, restocked, and scrapped units during returns processing."

### 15. Scope Inflation and Regulatory Hyperbole - Severity: Major
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Lines 309, 403
* **Discrepancy Description**: `CALIBRATE`. "Provides statutory financial statements" implies legally mandated external regulatory reporting for an SME MVP. "Absolute type safety" is hyperbolic for `<OptionStrict>On</OptionStrict>`.
* **Rationale & Guidance**: `CALIBRATE` "statutory financial statements" to "standard financial statements". Replace "absolute type safety" with "explicit typing".

### 16. Monolithic Entity and Document Repository Structures - Severity: Nit
* **Reviewer Lens:** `egm-implementation-readiness`
* **Affected Scope:** Lines 343, 355 (`DocumentRepository.vb`, `Entities.vb`)
* **Discrepancy Description**: Placing all 27 database entities into a single file `Entities.vb` and packing CRUD operations for all P2P, O2C, and Returns documents into a single `DocumentRepository.vb` produces unwieldy monolithic source files.
* **Rationale & Guidance**: Split `Entities.vb` into namespace-aligned files under `Domain/Models/` (e.g., `GeneralLedgerEntities.vb`, `InventoryEntities.vb`, `ProcurementEntities.vb`, `SalesEntities.vb`), and break `DocumentRepository` into focused repositories.

### 17. Redundant NuGet Dependency `System.ComponentModel.Annotations` on .NET 10 - Severity: Nit
* **Reviewer Lens:** `egm-codebase-details`
* **Affected Scope:** Line 308; `Merchandising_Accounting_App.vbproj` (Line 5)
* **Discrepancy Description**: In .NET 10 (`net10.0-windows`), `System.ComponentModel.Annotations` is part of the shared framework (`Microsoft.NETCore.App`). Adding a separate package reference is redundant and may trigger build warnings that fail the build under `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`.
* **Rationale & Guidance**: Remove `System.ComponentModel.Annotations` from NuGet package additions.

### 18. Unstated Runtime Storage Path for SQLite Database File - Severity: Nit
* **Reviewer Lens:** `egm-codebase-details`
* **Affected Scope:** Lines 293, 313, 320
* **Discrepancy Description**: The database filename is stated as `merchandising_erp.db`, but its runtime directory path is not specified. Relative file paths without an explicit directory resolve against the current working directory, which changes if launched from CLI or shortcuts.
* **Rationale & Guidance**: Specify explicit storage path (e.g., `Path.Combine(Application.StartupPath, "merchandising_erp.db")` or `%LocalAppData%\MerchandisingAccountingApp`).

### 19. Inconsistent Main Form Naming Between Overview and Implementation Plan - Severity: Nit
* **Reviewer Lens:** `egm-codebase-details`
* **Affected Scope:** Line 113 vs. Line 407
* **Discrepancy Description**: Section 2 names the top-level form `MainNavigationForm`, whereas Section 5 creates `MainShellForm.vb`.
* **Rationale & Guidance**: Reconcile top-level form naming to `MainShellForm`.

### 20. Omission of Restocked Customer Return Moving Average Recalculation Method - Severity: Nit
* **Reviewer Lens:** `egm-codebase-details`
* **Affected Scope:** Lines 369, 380
* **Discrepancy Description**: Line 369 defines `CalculateNewMovingAverageCost` for purchase receipts, but lacks a method to blend restocked return units at original invoice unit cost into `Products.CurrentUnitCost`.
* **Rationale & Guidance**: Add `CalculateRestockMovingAverageCost` to `InventoryCostingEngine.vb`.

### 21. Subjective Quality Descriptors and Marketing Fluff in UI Specs - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Lines 409, 429–430
* **Discrepancy Description**: "Clean shell", "High-visibility intake screen", and "Streamlined operational interface without accounting jargon".
* **Rationale & Guidance**: Use dry architectural descriptions: "Provides a top-level container form...", "Form for dock clerks to record Goods Receipts...", "Operational interface for physical receiving and RMA intake."

### 22. Corporate Buzzwords and Informal Colloquialisms - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Lines 314, 371, 420, 425, 457, 489
* **Discrepancy Description**: "Modern typography", "pre-flight database validation", "stock math", "Frontline interface", "hard-stop zero negative stock invariant", and "hard validation block".
* **Rationale & Guidance**: Replace with precise technical terminology ("standardized font rendering", "inventory valuation algorithms", "User interface for purchasing staff", "non-negative stock enforcement").

### 23. Synonym Redundancy and Buzzword Density - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Lines 321, 415
* **Discrepancy Description**: "Automated zero-configuration database setup" and "Eliminates dead code and unused files".
* **Rationale & Guidance**: Simplify to "Initializes database schema and seed data..." and "Removes default template files...".
