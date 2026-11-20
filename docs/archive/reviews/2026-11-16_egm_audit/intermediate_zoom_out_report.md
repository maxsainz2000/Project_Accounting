# Intermediate Zoom Out Review Report (Stage 8)

**Target:** `docs/designs/mvp_erp_accounting.md` (`full file: Lines 1..497`)  
**Standard:** `resources/document_standards/zoom-out-standard.md`  
**Reviewer:** `egm-zoom-out` (Skeptical Architect Persona)  
**Execution Mode:** Consolidated Uninterrupted Full Flow

---

## Reviewer Status

| Reviewer | Status | Focus Lens |
| :--- | :--- | :--- |
| `egm-zoom-out` | completed | High-altitude simplicity, YAGNI enforcement, complexity risk pruning, and scope creep protection |

---

## Executive Summary & High-Altitude Assessment

Following the Comprehension (Stage 6) and Critic (Stage 7) reviews, the design document is at risk of scope explosion and architectural over-engineering. Recommendations from downstream reviewers—such as introducing an IPC/HTTP daemon to host SQLite for multi-station setups, adding multi-table stock counting sheet modules, and building multi-document credit allocation clearing engines—threaten to convert a clean, single-warehouse desktop MVP into an over-engineered distributed system.

This review stress-tests high-altitude simplicity, pruning speculative bloat while ensuring the core invariants (balanced ledger, perpetual costing, zero negative stock) remain rock-solid.

---

## Prioritized Findings (Blockers -> Major -> Nits)

### 1. The Distributed IPC/HTTP Daemon Trap (Multi-Station Over-Engineering) - Severity: Blocker
* **Affected Scope:** System Architecture Overview (Lines 74–75), Concurrency Boundaries (Lines 235–237), Alternatives Considered (Lines 275–279), and Critic Finding 2.
* **Simplicity Rationale**: In response to concurrency questions, Critic Finding 2 proposed: "If multi-workstation concurrency is required across physical stations, specify an application service daemon with an IPC/HTTP boundary hosting the SQLite database." This is an extreme YAGNI violation and architectural dead-end. Transforming an embedded desktop application into an IPC/HTTP client-server distributed application to host an SQLite database adds network serialization, connection failure modes, daemon lifecycle management, and API versioning.
* **Recommendation**: 
  - Explicitly declare in Non-Goals that the MVP is restricted to a single dedicated PC workstation (or sequential operator handoffs). Multi-station concurrent operations across physical desks are an explicit Non-Goal for Day-1 MVP.
  - If multi-user concurrent networking is ever required in Phase 2, the established platform solution is swapping the persistence provider to a true client-server RDBMS (PostgreSQL / SQL Server)—not hand-rolling a custom IPC/HTTP daemon around SQLite.

### 2. Relational Schema Over-Engineering & Speculative Table Proliferation - Severity: Blocker
* **Affected Scope:** Relational Schema Specification (Lines 130–181), Implementation Plan (Line 320), and Critic Finding 6.
* **Simplicity Rationale**: The design currently specifies 27 relational tables for a single-warehouse, desktop-based MVP. Proposing to add `InventoryAdjustments` and `InventoryAdjustmentLines` pushes the schema to 29 tables, creating 29 entity classes, 29 DDL table scripts, massive join overhead, and extensive boilerplate mapping code.
* **Recommendation**: Prune the schema down to essential MVP entities (~16–18 tables):
  1. **Collapse Customer Returns (Eliminate 3 Tables):** Consolidate `RMAReceipts`, `RMALines`, `ReturnInspections`, and `InspectionLines` into a single pair: `CustomerReturns` and `CustomerReturnLines` (containing `ReturnedQty`, `RestockQty`, `ScrapQty`, and `InspectionStatus`).
  2. **Eliminate `CustomerRefundVouchers` Table (Eliminate 1 Table):** Settle refund disbursements using the standard `PaymentVouchers` table with a discriminator (`VoucherType = 'CustomerRefund'`) or an optional `CreditNoteId` foreign key.
  3. **Streamline Stock Discrepancies (Avoid 2 New Tables):** For stock shrinkage and count corrections (`5300` / `5400`), do not build a multi-table stocktake count sheet system. A single lightweight `InventoryAdjustments` table updating `TotalQtyOnHand` and emitting a balanced journal entry handles write-offs and physical count adjustments cleanly.
  4. **Eliminate `UnitConversions` Table (Eliminate 1 Table):** Storing `PackMultiplier INTEGER` and `PackUnitName TEXT` directly on `Products` eliminates the join table entirely while respecting the non-goal forbidding fractional units.

### 3. Persistence Layer Identity Crisis: EF Core 10 vs. Dapper + Hand-Rolled Repositories - Severity: Blocker
* **Affected Scope:** Technical Plan Section 1 (Line 98), Detailed Implementation Plan (Lines 308–347), and Zoom Out Stage 5 Finding 1.
* **Simplicity Rationale**: Section 1 defines the storage layer as `Data & Storage Layer (SQLite / EF Core 10)`, while Section 5 mandates installing `Microsoft.Data.Sqlite` + `Dapper`, writing hand-rolled raw SQL DDL scripts (`DatabaseBootstrap.vb`), hand-rolling a custom connection wrapper with `SemaphoreSlim(1, 1)` (`DatabaseContext.vb`), and building four boilerplate repository classes.
* **Recommendation**: Standardize on **EF Core 10 with SQLite**. It eliminates hundreds of lines of hand-written SQL DDL scripts, manual transaction managers, and repetitive repository boilerplate, maximizing speed of delivery and codebase minimalism.

### 4. Over-Complicating Customer Returns & Credit Settlement - Severity: Major
* **Affected Scope:** Section 1 (CUJ 3, Lines 63–67), Journal Posting Table Matrix (Lines 229–232), Relational Schema (Lines 163, 177–179), and Comprehension Finding 2 / Critic Finding 5.
* **Simplicity Rationale**: Critic recommended introducing a full credit application workflow, conditional posting branches, and cross-account clearing mechanisms between `1200 AR` and `2150 Customer Credit Balances`. This over-engineers standard ledger mechanics.
* **Recommendation**: In standard small-business accounting (e.g., QuickBooks, Xero), customer credit memos post directly to `1200 Accounts Receivable` as an unapplied credit balance (a credit balance on the customer's AR subledger). Future sales invoices naturally absorb this credit balance. If a formal bank refund is disbursed upon account closure, the Payment Voucher debits `1200 Accounts Receivable` (bringing the balance back to zero) and credits `1020 Bank Account`. This requires zero new clearing tables, zero credit application workflows, and keeps the ledger 100% balanced.

### 5. Speculative Available-to-Promise (ATP) Pipeline vs. Simple Hard-Stop Invariant - Severity: Major
* **Affected Scope:** Section 1 (CUJ 2, Line 61), Relational Schema (Line 143), and Zoom Out Stage 5 Finding 3.
* **Simplicity Rationale**: Building a dynamic Available-to-Promise (ATP) pipeline engine—calculating date-based fulfillment schedules across open POs and reserved SO lines—is an advanced supply-chain capability far beyond Day-1 MVP requirements.
* **Recommendation**: 
  - Keep Sales Orders as commercial commitments with zero General Ledger entry.
  - If stock reservation is needed prior to fulfillment, a simple scalar counter `AllocatedQty` on `Products` is sufficient ($\text{AvailableToOrder} = \text{TotalQtyOnHand} - \text{AllocatedQty}$).
  - The hard-stop validation block applies strictly at fulfillment time: a `SalesInvoice` cannot be posted if $\text{FulfilledQty} > \text{TotalQtyOnHand}$.
  - Explicitly defer multi-date inbound ATP scheduling to Phase 2.

### 6. Premature Concurrency & Locking Abstractions for Local SQLite - Severity: Major
* **Affected Scope:** Concurrency Boundaries (Lines 235–237), Implementation Plan (Lines 313, 325), and Critic Finding 1.
* **Simplicity Rationale**: The document introduces an application-level serial write queue (`SemaphoreSlim(1, 1)` or `TransactionQueueService`) to avoid `SQLITE_BUSY` errors. In a single-process desktop application where database operations are initiated from user interface actions, custom semaphore queueing is premature optimization and introduces potential deadlock hazards.
* **Recommendation**: Apply Critic Finding 1: Enforce a declarative `CHECK (TotalQtyOnHand >= 0)` constraint in SQLite DDL, set `PRAGMA busy_timeout = 3000;`, and execute stock decrements atomically:
  ```sql
  UPDATE Products 
  SET TotalQtyOnHand = TotalQtyOnHand - @Qty 
  WHERE ProductId = @Id AND TotalQtyOnHand >= @Qty;
  ```
  This single conditional SQL update, combined with SQLite's native WAL transaction locking and busy timeout, completely eliminates the need for custom application-level semaphores or queue services.

### 7. Dual-Date Ledger Complexity vs. Immutable Closed Period Invariant - Severity: Major
* **Affected Scope:** Goals (Line 31), Period Locking Guard (Lines 242–245), and Comprehension Finding 1.
* **Simplicity Rationale**: The design specifies a complex "Dual-Date" mechanism to accommodate late operational entries, yet states that `PeriodLockManager` rejects any posting where `OperationalEventDate` falls into a closed period unless an administrator reopens the period. Reopening closed periods destroys historical immutability, while rejecting the entry destroys operational sequencing.
* **Recommendation**:
  1. The General Ledger posting date (`PostingDate`) must always map to the current open accounting period. No entry can ever alter a closed period.
  2. The physical event date (`OperationalEventDate`) is purely informational metadata on the operational document.
  3. If an operational receipt physically occurred in a prior closed month, its financial journal is posted to the first open day of the current open period. Closed periods remain permanently immutable, no periods are reopened, and no complex dual-period reconciliation engine is required.

### 8. Dead General Ledger Account: `1350 Quarantine Inventory` - Severity: Nit
* **Affected Scope:** Goals (Line 33), Relational Schema (Line 135), Journal Posting Table Matrix (Lines 228–230), and Stage 5 Finding 11.
* **Simplicity Rationale**: `1350 Quarantine Inventory` is listed as an Asset account in the Chart of Accounts, yet the posting matrix explicitly specifies that RMA dock intake has zero ledger entry, and post-inspection restocks debit `1300 Merchandise Inventory` directly while scraps debit `5300 Shrinkage`. Account `1350` is completely dead—it never receives a single debit or credit.
* **Recommendation**: Remove `1350 Quarantine Inventory` from the financial Chart of Accounts. Physical quarantine is an operational status (`QuarantineQty` on `Products`), not a General Ledger asset.

### 9. Delivery Notes vs. Sales Invoices Scope Duplication - Severity: Nit
* **Affected Scope:** Goals (Lines 28, 29), Architecture Overview (Line 122), and Comprehension Finding 6.
* **Simplicity Rationale**: Building separate Delivery Note and Sales Invoice workflows with interim fulfillment accounts (e.g., Unbilled Shipments) is unnecessary scope bloat for a single-warehouse merchandising business.
* **Recommendation**: Explicitly declare that `SalesInvoice` is the combined commercial and fulfillment document for the MVP. When a Sales Invoice is posted, it decrements stock, books COGS, and recognizes revenue in one atomic transaction. A "Delivery Note" is simply a printable warehouse packing/dispatch slip generated from the posted `SalesInvoice`, not a separate ledger document.

### 10. Removal of Code-Level Weeds from High-Altitude Specification - Severity: Nit
* **Affected Scope:** Section 5 (Lines 240–241) and Detailed Implementation Plan (Lines 365–381).
* **Simplicity Rationale**: The document drops altitude into implementation trivia, detailing helper method names (`ValidateStockAvailable`, `CalculateNewMovingAverageCost`, `ConvertPackagingToUnits`), exact code expressions (`Math.Round(debitSum, 4) == Math.Round(creditSum, 4)`), and specific repository parameter signatures.
* **Recommendation**: Remove concrete method signatures and code expressions from the architectural design. The architecture should specify module responsibilities, domain invariants, and transactional guarantees, leaving method naming and parameter lists to the implementation phase.
