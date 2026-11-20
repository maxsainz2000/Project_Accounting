# Goldfish Review Report: Initial Zoom Out Review (Stage 5)

**Target:** `docs/designs/mvp_erp_accounting.md` (`Lines 1..497`)  
**Standard:** `resources/document_standards/zoom-out-standard.md`  
**Execution Mode:** Consolidated Uninterrupted Full Flow

---

## Reviewer Status

| Reviewer | Status | Focus Lens |
| :--- | :--- | :--- |
| `egm-zoom-out` | completed | High-altitude simplicity, minimalism, YAGNI, and codebase reuse |
| `egm-inventive-catalyst` | completed | TRIZ contradiction elimination, false compromises, and operational trade-offs |

---

## Prioritized Findings (Blockers -> Major -> Nits)

### 1. Persistence Strategy Contradiction (EF Core 10 vs. Dapper + Raw SQL DDL) - Severity: Blocker
* **Reviewer Lens:** `egm-zoom-out`
* **Affected Scope:** Technical Plan - Section 1 (Line 98) vs. Detailed Implementation Plan (Lines 308–347)
* **Discrepancy Description**: The high-level architecture diagram explicitly defines the persistence layer as `Data & Storage Layer (SQLite / EF Core 10)`. In contrast, the implementation plan adds `Microsoft.Data.Sqlite` and `Dapper` as NuGet dependencies, specifies a hand-written raw SQL DDL script in `DatabaseBootstrap.vb`, creates a custom `DatabaseContext.vb` connection wrapper, and builds four hand-rolled repository classes.
* **Rationale & Recommendation**: If EF Core 10 is adopted, hand-writing DDL migration scripts, raw connection lifecycles, and boilerplate repository classes reinvents what `DbContext` and EF Core migrations already handle out of the box. Conversely, if Dapper / raw ADO.NET is chosen for minimal overhead, labeling the persistence layer as `EF Core 10` introduces conflicting technical guidance. The design must commit to a single, unified data access strategy without redundant wrapper layers.

### 2. Sequence Diagram Clashes with Pure Domain Abstraction Boundary - Severity: Blocker
* **Reviewer Lens:** `egm-zoom-out`
* **Affected Scope:** Technical Plan - Section 1 (Lines 105–107), Section 4 Sequence Diagram (Lines 204–209), and Implementation Plan (Lines 365–381)
* **Discrepancy Description**: The document presents contradictory views of domain responsibility and data flow. Sections 1, 2, and the implementation plan define `InventoryCostingEngine` and `LedgerPostingEngine` as "Pure Domain Computation Engines" that isolate business rules from data access. However, the sequence diagram depicts `StockEngine` directly executing SQL (`UPDATE Products`, `INSERT INTO InventoryTransactions`) and `PostEngine` directly writing `INSERT INTO JournalEntries` to the SQLite database. Concurrently, lines 333–342 assign these identical write operations to `ProductRepository` and `LedgerRepository`.
* **Rationale & Recommendation**: Domain calculation engines cannot be pure in-memory calculation units and simultaneously execute direct SQL mutations. The architecture must enforce a clean, unidirectional flow: the application service queries current state, invokes pure domain calculation functions, and persists resulting entities via the data access layer.

### 3. Surrender of Stock Reservation / ATP Decoupling in Sales Order Workflow - Severity: Major
* **Reviewer Lens:** `egm-inventive-catalyst`
* **Affected Scope:** Section 1 (CUJ 2, Lines 58–62), Section 3 (Relational Schema: `SalesOrders`, `Products`, Lines 143, 164–168), and Section 5 (Detailed Implementation: Lines 391, 424)
* **Discrepancy Description**: In `concepts_brief.md` (Lines 94–96), Contradiction 1 was explicitly resolved via "Decoupled Reservation & ATP Commitments", allowing Sales Orders to confirm orders against inbound Purchase Orders while withholding Delivery Notes. However, line 424 states that `SalesDashboardForm` will "prevent document submission if stock is insufficient" across both Sales Orders and Sales Invoices. Furthermore, the `Products` schema (Line 143) lacks `AllocatedQty` tracking.
* **Rationale & Recommendation**: Apply **Segmentation** and **Preliminary Action** (TRIZ Patterns 1 & 3). Segment inventory state on `Products` into `TotalQtyOnHand`, `AllocatedQty`, and `InboundExpectedQty` to calculate Net Available-to-Promise ($\text{ATP} = \text{TotalQtyOnHand} - \text{AllocatedQty} + \text{InboundExpectedQty}$). Confirming a `SalesOrder` executes a preliminary reservation (`AllocatedQty += OrderedQty`) with zero General Ledger impact. The hard validation block applies only to physical dispatch/invoice posting when $\text{FulfilledQty} > \text{TotalQtyOnHand}$.

### 4. Persistence Boundary Invariant Surrendered to Unprotected Application TOCTOU Pre-Check - Severity: Major
* **Reviewer Lens:** `egm-inventive-catalyst`
* **Affected Scope:** Section 3 (Relational Schema: `Products`, Line 143), Section 4 (Sequence Diagram, Lines 197–204), and Section 5 (`InventoryCostingEngine.vb`, Line 368)
* **Discrepancy Description**: The design declares non-negative inventory as an absolute system invariant, but delegates enforcement exclusively to application-level code (`ValidateStockAvailable`) executed prior to opening the database transaction (Lines 197–202). Concurrently, the persistence schema for `Products` omits a declarative database-level `CHECK (TotalQtyOnHand >= 0)` constraint.
* **Rationale & Recommendation**: Apply **Beforehand Cushioning** and **Self-Service** (TRIZ Patterns 4 & 9). Cushion the persistence layer by embedding `CHECK (TotalQtyOnHand >= 0)` directly in the SQLite DDL and executing stock decrements via atomic conditional updates: `UPDATE Products SET TotalQtyOnHand = TotalQtyOnHand - @Qty WHERE ProductId = @Id AND TotalQtyOnHand >= @Qty;`. The database autonomously protects the invariant regardless of caller behavior.

### 5. Phantom Application Services between Architecture Diagram and Implementation Plan - Severity: Major
* **Reviewer Lens:** `egm-zoom-out`
* **Affected Scope:** Technical Plan - Section 1 (Lines 86–91), Section 4 Sequence Diagram (Lines 191–212), and Implementation Plan (Lines 384–404)
* **Discrepancy Description**: The architecture diagram and sequence diagram designate `DocumentWorkflowManager`, `ThreeWayMatchingService`, and `PeriodLockManager` as central application orchestrators. However, in the Detailed Implementation Plan, these three components vanish completely. Instead, the plan introduces an entirely different set of workflow services: `ProcureToPayService`, `OrderToCashService`, `ReturnsService`, and `ReportingService`.
* **Rationale & Recommendation**: Having `DocMgr (DocumentWorkflowManager)` anchor the core sequence diagram while omitting it from the target implementation plan leaves implementers without guidance on where document dispatching, period guards, and matching validation actually reside. Consolidate these fragmented abstractions: workflow services (P2P, O2C, Returns) should handle period checks and matching rules directly within their transactional boundaries.

### 6. Schema Over-Engineering & Speculative Table Proliferation (27 Tables for MVP) - Severity: Major
* **Reviewer Lens:** `egm-zoom-out`
* **Affected Scope:** Technical Plan - Section 3 (Lines 130–181) and Implementation Plan (Line 320)
* **Discrepancy Description**: Specifying 27 relational database tables for a single-warehouse, desktop-based MVP violates YAGNI. The returns workflow alone introduces five separate normalized tables: `RMAReceipts`, `RMALines`, `ReturnInspections`, `InspectionLines`, and `CustomerCreditNotes` (plus `CustomerRefundVouchers`).
* **Rationale & Recommendation**: Splitting physical return intake and inspection into separate header/line pairs (`RMAReceipts` vs. `ReturnInspections`) creates unnecessary join overhead and boilerplate mapping code. For Day-1 MVP, physical arrival and inspection disposition (`RestockQty` vs. `ScrapQty`) can be tracked on a single Return document/lines entity. Furthermore, introducing a dedicated `CustomerRefundVouchers` table duplicates the schema structure of `PaymentVouchers`. Trim the schema down to essential operational entities.

### 7. Multi-Station Facility Concurrency Bottlenecked by In-Process Serialized SQLite - Severity: Major
* **Reviewer Lens:** `egm-inventive-catalyst`
* **Affected Scope:** Section 1 (Lines 74–108), Section 5 (Lines 234–237), and Section 6 (Alternative 6, Lines 275–278)
* **Discrepancy Description**: In Alternative 6, the design rejects a client-server RDBMS to eliminate deployment complexity, opting for embedded SQLite with an in-process `SemaphoreSlim(1, 1)` serial lock (Lines 235–236). This accepts an unexamined operational bottleneck: either all physical warehouse roles (dock receiving, counter sales, accounting office) must physically queue up and take turns on a single computer workstation, or if the `.db` file is hosted on a shared network drive (SMB), the in-process semaphore fails cross-machine synchronization, risking SQLite WAL network file-locking crashes and database corruption.
* **Rationale & Recommendation**: Apply **Intermediary** or **Extraction** (TRIZ Patterns 8 & 2) if multi-station deployment is required, or explicitly declare in Non-Goals and Assumptions that the MVP is strictly restricted to a single physical workstation host.

### 8. Premature Concurrency & Locking Abstractions for a Single-User Desktop App - Severity: Major
* **Reviewer Lens:** `egm-zoom-out`
* **Affected Scope:** Technical Plan - Section 5 (Lines 235–237) and Implementation Plan (Lines 313, 325)
* **Discrepancy Description**: Wrapping SQLite writes in an application-level serial lock (`SemaphoreSlim(1, 1)`) or a dedicated `TransactionQueueService` to prevent `SQLITE_BUSY` contention is premature optimization for a local desktop Windows Forms application executing actions on the UI thread message loop.
* **Rationale & Recommendation**: Without multi-threaded background writers competing for database locks, introducing custom semaphore management adds unnecessary complexity and deadlock risk. Standard SQLite transactions configured with WAL mode and a busy timeout (`PRAGMA busy_timeout = 3000;`) are more than adequate for local desktop concurrency.

### 9. Invariant Contradiction: Speculative Fractional Packaging Conversions & Variance Routing - Severity: Major
* **Reviewer Lens:** `egm-zoom-out`
* **Affected Scope:** Technical Plan - Section 5 (Line 241) vs. Non-Goals (Line 41) & Concept Brief (Line 141)
* **Discrepancy Description**: Line 241 specifies that "Any penny-rounding variance generated from fractional unit packaging conversions is routed to `5400 Stocktake Revaluation Variance` if |variance| <= 0.02, or halts with a validation error if greater." This directly contradicts the core non-goal stated in line 41 ("No fractional or weight-variable conversion ratios; inventory tracking is strictly anchored to atomic integer Base Units with integer packaging multipliers").
* **Rationale & Recommendation**: Because pack units are strictly whole-number multipliers of the base unit, fractional packaging conversions cannot occur. Eliminate the speculative penny-rounding tolerance rule and variance routing to preserve domain clarity.

### 10. Altitude Drops into Implementation Weeds and Method Signatures - Severity: Nit
* **Reviewer Lens:** `egm-zoom-out`
* **Affected Scope:** Detailed Implementation Plan - Section 3 (Lines 365–381) and Section 5 (Line 240)
* **Discrepancy Description**: The design document specifies exact helper method names and parameters (`ValidateStockAvailable`, `CalculateNewMovingAverageCost`, `ConvertPackagingToUnits`, `GenerateGRNJournal`, etc.) as well as specific code expressions (`Math.Round(debitSum, 4) == Math.Round(creditSum, 4)`).
* **Rationale & Recommendation**: An architectural specification should focus on module boundaries, transactional guarantees, and domain invariant rules, leaving concrete helper method signatures to implementation.

### 11. Dead Asset Account in General Ledger (`1350 Quarantine Inventory`) - Severity: Nit
* **Reviewer Lens:** `egm-inventive-catalyst`
* **Affected Scope:** Section 3 (Accounts Schema & Posting Table Matrix, Lines 135, 228–231) and Section 1 (Goal 6, Line 33)
* **Discrepancy Description**: The design designates `1350 Quarantine Inventory` as an Asset account in the Chart of Accounts, yet explicitly enforces that physical intake into quarantine has zero financial/ledger impact (Line 228), and Stage 2 Credit Notes post directly between `5000 COGS` and `1300 Inventory` / `5300 Shrinkage` (Lines 229–230). Account `1350` is a dead ledger account that never receives a debit or credit.
* **Rationale & Recommendation**: Decouple physical tracking entirely from the General Ledger by eliminating `1350` from the financial Chart of Accounts (relying solely on operational `QuarantineQty` on `Products` and `ReturnInspections`), or elevate quarantine to an active interim clearing asset with explicit debit/credit postings upon RMA receipt and inspection clearance.
