# Consolidated Goldfish Review Report: EGM Quality Audit

**Target Document:** [`docs/designs/mvp_erp_accounting.md`](file:///c:/Users/maxsa/Documents/Project_Accounting/docs/designs/mvp_erp_accounting.md) (`Lines 1..497`)  
**Goldfish Review Context:** [`concepts_brief.md`](file:///c:/Users/maxsa/Documents/Project_Accounting/concepts_brief.md), [`initial_architecture.md`](file:///c:/Users/maxsa/Documents/Project_Accounting/initial_architecture.md)  
**Execution Mode:** Consolidated Report with All Review Rubrics (Uninterrupted Full Flow)  
**Date of Audit:** 2026-11-16  

---

## 1. Executive Summary & Review Overview

An uninterrupted, five-stage multi-perspective review was executed on `docs/designs/mvp_erp_accounting.md` adhering to the Elephant-Goldfish Model (EGM) Goldfish Review Protocol. Each review stage was conducted by specialized, stateless auditor subagents operating in isolated contexts.

Across the five review stages:
1. **Stage 5 (Initial Zoom Out)**: `egm-zoom-out` and `egm-inventive-catalyst` evaluated high-altitude architecture, YAGNI boundaries, and TRIZ technical contradictions.
2. **Stage 6 (Comprehension Test)**: `egm-comprehension` evaluated the target document's self-containment, clarity, and unstated assumptions with zero external context.
3. **Stage 7 (Critic Review)**: `egm-design`, `egm-structural`, and `egm-style` stress-tested failure modes, template compliance, and technical copy precision across lines 1–296.
4. **Stage 8 (Intermediate Zoom Out)**: `egm-zoom-out` re-audited the complete architecture, pruning complexity creep, speculative multi-tier IPC daemons, and schema bloat introduced by downstream reviews.
5. **Stage 9 (Implementation Readiness Test)**: `egm-implementation-readiness`, `egm-codebase-details`, and `egm-style` evaluated the actionability, phasing, Zero-Decision Rule compliance, and codebase fidelity of lines 297–497.

### Stage Completion Matrix

| Stage | Review Stage Name | Reviewer Personas | Scope | Individual Report Artifact |
| :--- | :--- | :--- | :--- | :--- |
| **Stage 5** | Initial Zoom Out Review | `egm-zoom-out`, `egm-inventive-catalyst` | Full File (Lines 1–497) | [`zoom_out_report.md`](file:///c:/Users/maxsa/Documents/Project_Accounting/zoom_out_report.md) |
| **Stage 6** | Comprehension Test | `egm-comprehension` | Full File (Lines 1–497) | [`comprehension_report.md`](file:///c:/Users/maxsa/Documents/Project_Accounting/comprehension_report.md) |
| **Stage 7** | Critic Review | `egm-design`, `egm-structural`, `egm-style` | FeatureRange (Lines 1–296) | [`critic_report.md`](file:///c:/Users/maxsa/Documents/Project_Accounting/critic_report.md) |
| **Stage 8** | Intermediate Zoom Out | `egm-zoom-out` | Full File (Lines 1–497) | [`intermediate_zoom_out_report.md`](file:///c:/Users/maxsa/Documents/Project_Accounting/intermediate_zoom_out_report.md) |
| **Stage 9** | Implementation Readiness | `egm-implementation-readiness`, `egm-codebase-details`, `egm-style` | DetailedRange (Lines 297–497) | [`implementation_readiness_report.md`](file:///c:/Users/maxsa/Documents/Project_Accounting/implementation_readiness_report.md) |

---

## 2. Consolidated Prioritized Action Items

Findings across all five stages have been deduplicated, cross-referenced, and synthesized into 11 consolidated technical action items grouped by severity.

```
+---------------------------------------------------------------------------------------------------------+
|                                  FINDINGS DISTRIBUTION BY SEVERITY                                      |
+---------------------------------------------------------------------------------------------------------+
|  🔴 BLOCKERS: 5 Action Themes (Architecture, Invariant Protection, Technology Stack, Solution Setup)   |
|  🟡 MAJORS:   4 Action Themes (Schema Pruning, Return Mechanics, Closed Periods, Readiness Phasing)     |
|  🟢 NITS:     2 Action Themes (Test Infrastructure & Method Contracts, Editorial Copy Calibration)      |
+---------------------------------------------------------------------------------------------------------+
```

---

### 🔴 Blockers (Must Be Resolved Before Implementation)

#### ACTION-1: Resolve Persistence Stack Contradiction & Standardize on a Unified Data Layer
* **Affected Areas:** Architecture Diagram (Line 98), Implementation Plan (Lines 308, 318–347), Zoom Out Stage 5 (Finding 1), Zoom Out Stage 8 (Finding 3), Implementation Readiness Stage 9 (Finding 5).
* **Defect:** Section 1 specifies `Data & Storage Layer (SQLite / EF Core 10)`, while Section 5 mandates installing `Microsoft.Data.Sqlite (v9.0+)` and `Dapper (v2.1+)`, creating hand-rolled SQL DDL scripts (`DatabaseBootstrap.vb`), and building raw repository classes. This introduces an architectural contradiction: developers cannot determine whether to use EF Core migrations and `DbContext` or raw Dapper SQL.
* **Remediation:** 
  - Standardize on **EF Core 10 with SQLite (`Microsoft.EntityFrameworkCore.Sqlite`)**.
  - Remove all references to Dapper and raw ADO.NET connection wrappers.
  - Rely on `DbContext` for automated SQLite table migrations, change tracking, and transactional Unit-of-Work, eliminating hundreds of lines of boilerplate repository and manual DDL bootstrap code.

#### ACTION-2: Fix TOCTOU Concurrency Flaw & Enforce Database-Level Non-Negative Stock Invariant
* **Affected Areas:** Schema (Line 143), Sequence Diagram (Lines 197–204), Domain Engine (Lines 368), Zoom Out Stage 5 (Findings 2, 4), Critic Stage 7 (Finding 1), Zoom Out Stage 8 (Finding 6), Implementation Readiness Stage 9 (Finding 10).
* **Defect:** Stock validation (`ValidateStockAvailable`) is performed in application memory before beginning the database transaction. Concurrently, read queries execute unblocked via WAL mode. Two simultaneous sales orders for the same SKU can pass validation against an un-decremented balance and sequentially decrement inventory below zero. Furthermore, the `Products` table DDL lacks a declarative `CHECK (TotalQtyOnHand >= 0)` constraint.
* **Remediation:**
  - Embed `CHECK (TotalQtyOnHand >= 0)` directly in the `Products` table schema.
  - Move stock verification inside the serializable database transaction under the write lock.
  - Execute stock decrements via atomic conditional SQL updates:
    ```sql
    UPDATE Products 
    SET TotalQtyOnHand = TotalQtyOnHand - @Qty 
    WHERE ProductId = @Id AND TotalQtyOnHand >= @Qty;
    ```
  - Verify that affected rows equal 1; throw `InsufficientStockException` if 0 rows are updated.

#### ACTION-3: Reject Distributed Daemon & Enforce Single-Workstation Topology in Non-Goals
* **Affected Areas:** Architecture Overview (Lines 74–75), Concurrency Boundaries (Lines 235–237), Alternatives Considered (Lines 275–279), Critic Stage 7 (Finding 2), Zoom Out Stage 8 (Finding 1).
* **Defect:** Critic Review flagged that an in-process `SemaphoreSlim(1, 1)` fails multi-station operations over network shares (SMB) and proposed an IPC/HTTP daemon hosting SQLite. Stage 8 identified this as extreme premature optimization and an architectural dead-end for an embedded desktop MVP.
* **Remediation:**
  - Explicitly state in **Non-Goals** and **Constraints** that the MVP is strictly a single-user workstation desktop application (or sequential operator handoffs on a single physical host). Multi-station distributed networking is an explicit Non-Goal.
  - Discard custom `SemaphoreSlim(1, 1)` and `TransactionQueueService` abstractions. Standard SQLite configured with WAL mode (`PRAGMA journal_mode=WAL;`) and a busy timeout (`PRAGMA busy_timeout=3000;`) provides zero-deadlock local desktop concurrency.

#### ACTION-4: Fix Solution Path at Repository Root, Register Test Projects, and Modify `Application.myapp`
* **Affected Areas:** Configuration (Lines 303–314), File Modifications (Lines 412–415), Verification Plan (Lines 473, 479), Codebase Details Stage 9 (Findings 1, 2).
* **Defect:** 
  1. `Merchandising_Accounting_App.slnx` does not exist at repository root; it resides in `Merchandising_Accounting_App/`. Running `dotnet build Merchandising_Accounting_App.slnx` fails with `MSB1009`.
  2. The existing solution file contains only `Merchandising_Accounting_App.vbproj`. The plan creates a sibling test project `Merchandising_Accounting_App.Tests/` but omits the solution file from the modification list.
  3. Line 412 instructs deleting `Form1.vb`, `Form1.Designer.vb`, and `Form1.resx`. In VB Windows Forms SDK projects, deleting `Form1` without modifying `Application.myapp` (`<MainForm>Form1</MainForm>`) causes compile error `BC30451: 'Form1' is not declared`.
* **Remediation:**
  - Relocate `Merchandising_Accounting_App.slnx` to the repository root.
  - Register `Merchandising_Accounting_App.vbproj` and `Merchandising_Accounting_App.Tests.vbproj` into `Merchandising_Accounting_App.slnx`.
  - Add `Merchandising_Accounting_App/My Project/Application.myapp` to the Target Files to Modify list, replacing `<MainForm>Form1</MainForm>` with `<MainForm>MainShellForm</MainForm>`.

#### ACTION-5: Restructure Implementation Plan into Incremental Phasing with Buildable Checkpoints
* **Affected Areas:** Detailed Implementation Plan (Lines 297–468), Implementation Readiness Stage 9 (Findings 1, 3, 4).
* **Defect:** The implementation plan presents a static file catalog organized by architectural layer rather than incremental phases. It violates the Zero-Decision Rule by omitting execution sequencing, concrete SQLite DDL column types (e.g. mapping `DECIMAL(18,4)`), and Chart of Accounts seed datasets.
* **Remediation:** Partition the implementation plan into six sequential phases, each with verifiable completion gates:
  - **Phase 1: Project Hardening & Solution Setup** (Solution root migration, strict typing, package dependencies, test project creation).
  - **Phase 2: Domain Entities & Computation Engines** (Entities, Enums, Exceptions, pure math for `LedgerPostingEngine` and `InventoryCostingEngine`) + Phase 2 Unit Tests.
  - **Phase 3: Persistence Layer & Database Bootstrapping** (`DbContext`, EF Core migrations, full COA seed rows, repository contracts) + DB Integration Tests.
  - **Phase 4: Application Services & Document Workflows** (`ProcureToPayService`, `OrderToCashService`, `ReturnsService`, `PeriodService`, `ReportingService` with transactional boundaries).
  - **Phase 5: Presentation Layer (WinForms Shell & Dashboard Views)** (`MainShellForm` container, companion `.Designer.vb` and `.resx` files, dashboard data bindings).
  - **Phase 6: End-to-End CUJ Verification & Coverage Gate** (Full automated workflow tests verifying 90% domain coverage and 85% service coverage).

---

### 🟡 Majors (Must Be Addressed in Design Refinement)

#### ACTION-6: Ruthlessly Prune Schema from 27 to 17 Tables & Consolidate Customer Returns
* **Affected Areas:** Relational Schema (Lines 130–181), Implementation Plan (Line 320), Zoom Out Stage 5 (Finding 4), Critic Stage 7 (Finding 6), Zoom Out Stage 8 (Finding 2).
* **Defect:** 27 relational tables for a single-warehouse desktop MVP is excessive. Splitting customer return intake and inspection into separate header/line pairs across two distinct stages (`RMAReceipts`, `RMALines`, `ReturnInspections`, `InspectionLines`) adds join overhead and boilerplate code. Packaging conversions via `UnitConversions` violate the non-goal against variable conversions. Dedicated `CustomerRefundVouchers` duplicates `PaymentVouchers`.
* **Remediation:** Prune down to 17 core tables:
  1. **Consolidate Returns:** Replace `RMAReceipts`, `RMALines`, `ReturnInspections`, and `InspectionLines` with a single pair: `CustomerReturns (ReturnId PK, CustomerId FK, DocumentNumber, ReturnDate, OriginalInvoiceId FK, Status)` and `CustomerReturnLines (LineId PK, ReturnId FK, ProductId FK, ReturnedQty, RestockQty, ScrapQty, DispositionStatus)`.
  2. **Eliminate `CustomerRefundVouchers`:** Use `PaymentVouchers` with a `VoucherType` discriminator or optional `CreditNoteId` foreign key.
  3. **Inline Packaging Units:** Store `PackMultiplier INTEGER` and `PackUnitName TEXT` directly on `Products`, eliminating the `UnitConversions` table.
  4. **Lightweight Stock Adjustments:** Add a single `InventoryAdjustments` table for shrinkage and count adjustments rather than a full stocktake sheet subsystem.

#### ACTION-7: Correct Customer Returns Ledger Posting & Moving Average Restock Valuation
* **Affected Areas:** CUJ 3 (Lines 63–67), Journal Posting Table Matrix (Lines 229–232), Relational Schema (Lines 163, 177–179), Comprehension Stage 6 (Findings 2, 3), Critic Stage 7 (Findings 4, 5), Zoom Out Stage 8 (Findings 4, 8), Codebase Details Stage 9 (Finding 12).
* **Defect:** 
  1. Credit Notes credit `1200 AR`, while Refund Disbursements debit `2150 Customer Credit Balances`, leaving both accounts unbacked.
  2. Account `1350 Quarantine Inventory` is listed as an Asset account but receives zero journal entries.
  3. Restocked returns debit `1300 Inventory` at the original invoice unit cost without mathematically blending into `Products.CurrentUnitCost`, causing GL Account 1300 to permanently desynchronize from subledger inventory valuation.
* **Remediation:**
  - Eliminate Account `2150` from the primary posting flow. Credit Notes always credit `1200 Accounts Receivable` (creating a credit balance on the customer's AR subledger). Subsequent sales invoices naturally absorb this credit balance. Cash/bank refund disbursements debit `1200 AR` (restoring it to zero) and credit `1020 Bank`.
  - Remove `1350 Quarantine Inventory` from the Chart of Accounts. Physical quarantine is an operational quantity counter (`QuarantineQty` on `Products`), not a GL asset.
  - Add restock weighted average costing to `InventoryCostingEngine`:
    $$\text{New Average Cost} = \frac{(\text{Current Qty} \times \text{Current Cost}) + (\text{Restocked Qty} \times \text{Original Invoice Cost})}{\text{Current Qty} + \text{Restocked Qty}}$$

#### ACTION-8: Clarify Closed Period Invariant & Operational Event Date Semantics
* **Affected Areas:** Goals (Line 31), Period Locking Guard (Lines 242–245), Comprehension Stage 6 (Finding 1), Zoom Out Stage 8 (Finding 7), Implementation Readiness Stage 9 (Finding 9).
* **Defect:** Line 31 defines dual dates to prevent altering closed periods while recording true physical dates. However, lines 242–245 state that `PeriodLockManager` rejects any entry where `OperationalEventDate` falls in a closed period unless an administrator reopens the period. Reopening closed periods destroys historical immutability, while rejecting late entries destroys operational event sequencing.
* **Remediation:**
  - The statutory General Ledger posting date (`PostingDate`) **must always map to the current open fiscal period**. Closed periods are permanently immutable and cannot be altered.
  - The physical event date (`OperationalEventDate`) is captured purely as informational metadata on operational documents.
  - If an operational receipt physically occurred in a prior closed month, its financial journal is posted to the *first open day of the current open period*.
  - Explicitly add `PeriodService.vb` to the Service Layer and expose period administration methods in `LedgerRepository`.

#### ACTION-9: Add Mandatory "Risks & Mitigations" Section, Resolve Open Questions, and Fix Alternatives
* **Affected Areas:** Open Questions (Lines 46–50), Alternatives Considered (Lines 253, 258, 263, 268, 273, 278), Non-Functional Requirements (Lines 280–282), Critic Stage 7 (Findings 3, 7, 8), Structural Stage 7 (Findings 1, 2).
* **Defect:**
  1. The document completely omits a dedicated `## Risks & Mitigations` section, violating the Feature Specification Standard.
  2. Open Questions regarding partial PO fulfillment and 30-day unbilled GRNI aging remain unresolved.
  3. Rejection rationales in Alternatives Considered begin with `Rejected by author: ...`, violating the Alternatives Rule attribution constraint.
* **Remediation:**
  - Add a dedicated `## Risks & Mitigations` section detailing failure modes (dock entry latency, power loss during writes, invoice pricing discrepancies, unbilled GRNI liabilities) and concrete architectural mitigations.
  - Formally resolve Open Questions in the design: (1) closing a partially fulfilled PO transitions it to `Closed - Short`, releasing commitments without a GL entry; (2) implement a 30-day GRNI aging review with an authorized adjustment document clearing abandoned balances to `5100 PPV`.
  - Remove all "Rejected by author:" text and state concrete technical and scope trade-offs directly.

---

### 🟢 Nits (Clean-Up, Precision & Consistency Improvements)

#### ACTION-10: Complete Engine Method Signatures, WinForms File Allowlist & Test Infrastructure
* **Affected Areas:** Detailed Implementation Plan (Lines 365–468), Comprehension Stage 6 (Findings 4, 5, 6, 7), Codebase Details Stage 9 (Findings 3, 4, 5, 7, 8, 11), Implementation Readiness Stage 9 (Findings 6, 7, 10).
* **Defect:** 
  1. `LedgerPostingEngine` omits method signatures for Payment Vouchers, Payment Receipts, and Refund Vouchers.
  2. WinForms forms in Section 5 omit companion `.Designer.vb` and `.resx` files.
  3. Automated Test Suite omits test infrastructure files (`TestDatabaseFixture.vb`) and concrete test cases.
  4. Trigger conditions for `ThreeWayMatchException` are undefined.
  5. Concept ambiguity exists between Delivery Notes and Sales Invoices.
* **Remediation:**
  - Add complete method contracts to `LedgerPostingEngine` for payment vouchers, customer receipts, and refunds.
  - Explicitly enumerate all companion files (`*.Designer.vb`, `*.resx`) for all forms in the Target Files list.
  - Add `TestDatabaseFixture.vb` to test project infrastructure, detailing in-memory SQLite initialization and seed teardown.
  - Define `ThreeWayMatchException` triggers: throw if `BilledQty > ReceivedQty`, if vendor IDs mismatch, or if a target `GRNLine` is already settled.
  - Clarify that `SalesInvoice` is the combined commercial/fulfillment document, and "Delivery Note" is a printable packing slip.

#### ACTION-11: Technical Copy Editing, Syntax Quotes & Promotional Fluff Stripping
* **Affected Areas:** Overview, Problem, Goals, Technical Plan, NFRs, Verification (Lines 11, 13, 24, 29, 34, 65, 74, 99, 119, 121, 191, 240, 257, 278, 285, 308, 309, 314, 321, 371, 398, 403, 409, 415, 420, 425, 429, 452, 457, 462, 467, 489), Critic Stage 7 (Findings 9–13, 15, 17–26), Style Stage 7, Style Stage 9 (Findings 1–5).
* **Defect:**
  1. Unquoted Mermaid syntax delimiters in cylinder nodes and sequence aliases.
  2. Uncalibrated claims: "tamper-evident ledgers" (application append-only, not cryptographic), "incorruptible" (hyperbole), "microsecond reads" (sub-millisecond), "statutory financial statements" (standard/internal), "absolute type safety" (explicit typing).
  3. Promotional adjectives and informal slang: "clean shell", "high-visibility", "streamlined", "stock math", "frontline interface", "hard-stop invariant", "pristine stock", "pure engine".
  4. Redundant dependency: `System.ComponentModel.Annotations` on .NET 10.
* **Remediation:**
  - Enclose Mermaid labels in quotes: `DB[("Embedded SQLite DB: ...")]` and `participant DB as "SQLite Transaction (merchandising_erp.db)"`.
  - Apply `CALIBRATE` actions: "Append-Only Application Ledgers", "Constraint-enforcing system", "Low-latency sub-millisecond reads", "Standard financial statements", "Explicit typing".
  - Replace marketing fluff and slang with dry engineering terminology.
  - Remove `System.ComponentModel.Annotations` from NuGet references.

---

## 3. Comprehensive Traceability Matrix

This matrix maps every raw finding from all five review stage reports directly to the consolidated action items.

| Stage & Finding ID | Source Auditor | Reported Finding Name | Raw Severity | Mapped Action Item |
| :--- | :--- | :--- | :--- | :--- |
| **S5-F01** | `egm-zoom-out` | Persistence Strategy Contradiction (EF Core vs. Dapper) | Blocker | **ACTION-1** |
| **S5-F02** | `egm-zoom-out` | Sequence Diagram Clashes with Pure Domain Boundary | Blocker | **ACTION-2** |
| **S5-F03** | `egm-inventive-catalyst` | Surrender of Stock Reservation / ATP Decoupling in SO | Major | **ACTION-6** |
| **S5-F04** | `egm-inventive-catalyst` | Persistence Invariant Surrendered to Unprotected TOCTOU | Major | **ACTION-2** |
| **S5-F05** | `egm-zoom-out` | Phantom Application Services between Diag and Plan | Major | **ACTION-8**, **ACTION-10** |
| **S5-F06** | `egm-zoom-out` | Schema Over-Engineering (27 Tables for MVP) | Major | **ACTION-6** |
| **S5-F07** | `egm-inventive-catalyst` | Multi-Station Facility Concurrency Bottleneck | Major | **ACTION-3** |
| **S5-F08** | `egm-zoom-out` | Premature Concurrency & Locking for Single-User App | Major | **ACTION-3** |
| **S5-F09** | `egm-zoom-out` | Invariant Contradiction: Speculative Fractional Packaging | Major | **ACTION-6** |
| **S5-F10** | `egm-zoom-out` | Altitude Drops into Implementation Weeds / Signatures | Nit | **ACTION-11** |
| **S5-F11** | `egm-inventive-catalyst` | Dead Asset Account in General Ledger (`1350 Quarantine`) | Nit | **ACTION-7** |
| **S6-F01** | `egm-comprehension` | Dual-Date Ledger vs. PeriodLockManager Rejection | Blocker | **ACTION-8** |
| **S6-F02** | `egm-comprehension` | Broken GL Posting and Settlement Rules for Returns | Blocker | **ACTION-7** |
| **S6-F03** | `egm-comprehension` | Ambiguous Status of `1350 Quarantine Inventory` | Major | **ACTION-7** |
| **S6-F04** | `egm-comprehension` | Missing Packaging Unit Linkage on Transactional Lines | Major | **ACTION-6** |
| **S6-F05** | `egm-comprehension` | Undefined Rules for ThreeWayMatchException | Major | **ACTION-10** |
| **S6-F06** | `egm-comprehension` | Ambiguity Between Sales Invoices and Delivery Notes | Major | **ACTION-10** |
| **S6-F07** | `egm-comprehension` | Missing Master Chart of Accounts / Equity Accounts | Major | **ACTION-5** |
| **S6-F08** | `egm-comprehension` | Inconsistent Document Status Lifecycle Terminology | Nit | **ACTION-11** |
| **S6-F09** | `egm-comprehension` | Integer Units vs. Fractional Packaging Variance | Nit | **ACTION-6**, **ACTION-11** |
| **S6-F10** | `egm-comprehension` | Missing Implementation Scope for Statement of Cash Flows | Nit | **ACTION-5** |
| **S6-F11** | `egm-comprehension` | Unqualified BankAccountId Foreign Key References | Nit | **ACTION-5** |
| **S7-F01** | `egm-design` | TOCTOU Race Condition on Inventory Validation | Blocker | **ACTION-2** |
| **S7-F02** | `egm-design` | Mismatch Between Multi-Role Operations and In-Process Lock | Blocker | **ACTION-3** |
| **S7-F03** | `egm-design` / `structural` | Missing Mandatory Risks & Mitigations Section | Blocker | **ACTION-9** |
| **S7-F04** | `egm-design` | GL vs. Inventory Valuation Desync on Restock Returns | Major | **ACTION-7** |
| **S7-F05** | `egm-design` | Broken Credit Memo Ledger Mechanics & Missing Workflow | Major | **ACTION-7** |
| **S7-F06** | `egm-design` | Omission of Physical Stock Adjustments from Schema | Major | **ACTION-6** |
| **S7-F07** | `egm-design` | Unresolved Open Questions Prior to Kickoff | Major | **ACTION-9** |
| **S7-F08** | `egm-structural` | Violation of Alternatives Rule: Personal Attribution | Major | **ACTION-9** |
| **S7-F09** | `egm-structural` | Mermaid Diagram Syntax Vulnerabilities | Major | **ACTION-11** |
| **S7-F10** | `egm-style` | Uncalibrated Security Claim: "Tamper-Evident" Ledgers | Major | **ACTION-11** |
| **S7-F11** | `egm-style` | Jargon: "Single-Project Clean Architecture" | Major | **ACTION-11** |
| **S7-F12** | `egm-style` | Marketing Hyperbole: "Incorruptible" | Major | **ACTION-11** |
| **S7-F13** | `egm-style` | Uncalibrated Latency Metric: "Microsecond Read" | Major | **ACTION-11** |
| **S7-F14** | `egm-design` | Rigid Inbound Freight Invoice Attribution | Nit | **ACTION-10** |
| **S7-F15** | `egm-design` | Improper Variance Account Mapping for Penny-Rounding | Nit | **ACTION-11** |
| **S7-F16** | `egm-structural` | Malformed Schema Layout (ASCII Box vs. Markdown/DDL) | Nit | **ACTION-5** |
| **S7-F17** | `egm-structural` | Inconsistent Section Dividers and Delimiter Style | Nit | **ACTION-11** |
| **S7-F18** | `egm-structural` | Asymmetric Bullet Formatting in Component Responsibilities | Nit | **ACTION-11** |
| **S7-F19** | `egm-style` | Synonym Redundancy: "Immediately in real time" | Nit | **ACTION-11** |
| **S7-F20** | `egm-style` | Informal Phrasing: "Leaving the business blind" | Nit | **ACTION-11** |
| **S7-F21** | `egm-style` | Subjective Modifiers: "Massive write-downs" | Nit | **ACTION-11** |
| **S7-F22** | `egm-style` | Syntactic Repetition: "Preventing... preventing" | Nit | **ACTION-11** |
| **S7-F23** | `egm-style` | Promotional Adjective: "Instant statements" | Nit | **ACTION-11** |
| **S7-F24** | `egm-style` | Non-Technical Vocabulary: "Pristine stock" | Nit | **ACTION-11** |
| **S7-F25** | `egm-style` | Subjective Filler: "Fast queries", "Pure engine" | Nit | **ACTION-11** |
| **S7-F26** | `egm-style` | Hyperbolic Modifier: "Massive complexity" | Nit | **ACTION-11** |
| **S8-F01** | `egm-zoom-out` | The Distributed IPC/HTTP Daemon Trap | Blocker | **ACTION-3** |
| **S8-F02** | `egm-zoom-out` | Relational Schema Over-Engineering (27 Tables) | Blocker | **ACTION-6** |
| **S8-F03** | `egm-zoom-out` | Persistence Identity Crisis: EF Core 10 vs. Dapper | Blocker | **ACTION-1** |
| **S8-F04** | `egm-zoom-out` | Over-Complicating Customer Returns & Credit Settlement | Major | **ACTION-7** |
| **S8-F05** | `egm-zoom-out` | Speculative ATP Pipeline vs. Simple Hard-Stop Invariant | Major | **ACTION-6** |
| **S8-F06** | `egm-zoom-out` | Premature Concurrency & Locking for Local SQLite | Major | **ACTION-3** |
| **S8-F07** | `egm-zoom-out` | Dual-Date Complexity vs. Immutable Closed Periods | Major | **ACTION-8** |
| **S8-F08** | `egm-zoom-out` | Dead General Ledger Account: `1350 Quarantine Inventory` | Nit | **ACTION-7** |
| **S8-F09** | `egm-zoom-out` | Delivery Notes vs. Sales Invoices Scope Duplication | Nit | **ACTION-10** |
| **S8-F10** | `egm-zoom-out` | Code-Level Weeds in High-Altitude Architecture | Nit | **ACTION-11** |
| **S9-F01** | `egm-implementation-readiness` | Absence of Incremental Phasing and Buildable Checkpoints | Blocker | **ACTION-5** |
| **S9-F02** | `egm-codebase-details` | Non-Existent Solution Path & Omitted Test Registration | Blocker | **ACTION-4** |
| **S9-F03** | `egm-codebase-details` | Omission of `Application.myapp` Causing Build Failure | Blocker | **ACTION-4** |
| **S9-F04** | `egm-implementation-readiness` | Omission of Concrete DDL, Column Types, and Seed Data | Blocker | **ACTION-5** |
| **S9-F05** | `egm-implementation-readiness` | Contradictory Persistence Stack & Unpinned Versions | Blocker | **ACTION-1** |
| **S9-F06** | `egm-codebase-details` | Missing WinForms Designer/Resx Files in Target Scope | Major | **ACTION-10** |
| **S9-F07** | `egm-implementation-readiness` | Omission of Payment/Receipt Journals in PostingEngine | Major | **ACTION-7**, **ACTION-10** |
| **S9-F08** | `egm-implementation-readiness` | Ambiguous Repo Signatures & Transaction Propagation | Major | **ACTION-1**, **ACTION-10** |
| **S9-F09** | `egm-implementation-readiness` | Disappearance of `PeriodService` from Service Layer | Major | **ACTION-8** |
| **S9-F10** | `egm-implementation-readiness` | Unresolved TOCTOU Concurrency Hazard in CostingEngine | Major | **ACTION-2** |
| **S9-F11** | `egm-codebase-details` | Undefined Rules and Triggers for ThreeWayMatchException | Major | **ACTION-10** |
| **S9-F12** | `egm-codebase-details` | Neglect of Test Infrastructure in Test Suite Plan | Major | **ACTION-10** |
| **S9-F13** | `egm-implementation-readiness` | Abstract Test Plan Lacking Concrete Method Signatures | Major | **ACTION-10** |
| **S9-F14** | `egm-style` | Uncalibrated Invariant Claims in Test Rationales | Major | **ACTION-11** |
| **S9-F15** | `egm-style` | Scope Inflation: "Statutory statements", "Absolute safety" | Major | **ACTION-11** |
| **S9-F16** | `egm-implementation-readiness` | Monolithic Entity and Document Repository Structures | Nit | **ACTION-6** |
| **S9-F17** | `egm-codebase-details` | Redundant NuGet Dependency `System.ComponentModel` | Nit | **ACTION-11** |
| **S9-F18** | `egm-codebase-details` | Unstated Runtime Storage Path for SQLite Database File | Nit | **ACTION-11** |
| **S9-F19** | `egm-codebase-details` | Inconsistent Main Form Naming (MainNavigation vs. Shell) | Nit | **ACTION-11** |
| **S9-F20** | `egm-codebase-details` | Omission of Restock Return Cost Recalculation Method | Nit | **ACTION-7** |
| **S9-F21** | `egm-style` | Subjective Quality Descriptors in UI Specs | Nit | **ACTION-11** |
| **S9-F22** | `egm-style` | Corporate Buzzwords and Informal Colloquialisms | Nit | **ACTION-11** |
| **S9-F23** | `egm-style` | Synonym Redundancy and Buzzword Density | Nit | **ACTION-11** |

---

## 4. Final Review Verdict & Next Steps

* **Overall Audit Status:** **FAIL - Remediations Required**
* **Gate Verdict:** The design document provides a sound domain accounting model, but cannot proceed directly to execution due to persistence stack contradictions, concurrency race conditions, missing buildable solution configurations, and lack of implementation readiness phasing.
* **Handoff Guidance:**
  1. Use `/egm-draft` or direct document remediation to apply the 11 consolidated action items directly to `docs/designs/mvp_erp_accounting.md`.
  2. Prune the schema to 17 tables, standardize on EF Core 10 SQLite, embed database check constraints, and restructure the Detailed Implementation Plan into 6 incremental phases.
  3. Once revised, the document will achieve full Zero-Decision Rule compliance and ready-to-code implementation state.
