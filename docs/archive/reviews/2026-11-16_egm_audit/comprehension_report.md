# Comprehension Test Report (Stage 6)

**Target:** `docs/designs/mvp_erp_accounting.md` (`FeatureRange: 1..296`, `DetailedRange: 297..497`)  
**Auditor:** `egm-comprehension` (Naive Reader Persona)  
**Execution Mode:** Consolidated Uninterrupted Full Flow

---

## What this proposes
The document proposes an MVP desktop ERP accounting application built in .NET 10 Windows Forms (Visual Basic .NET) using an embedded SQLite database for small-to-mid scale merchandising businesses. It replaces fragmented spreadsheet workflows with an integrated, document-driven architecture where warehouse operational events (receiving, sales fulfillment, and customer returns) immediately generate balanced double-entry General Ledger journal entries in real time. The core design enforces hard-stop zero negative inventory, perpetual moving weighted average costing, three-way matching via Goods Received Not Invoiced (GRNI) clearing with forward-only purchase/freight variance expensing, two-stage quarantine customer returns, and an append-only dual-date ledger.

## How the existing system works, per this document
According to the document, target businesses currently manage physical warehouse operations (receiving, picking, shipping, and returns) in standalone software or spreadsheets entirely disconnected from the General Ledger. Bookkeepers record batched summary journal entries days or weeks later from supplier bills and bank statements, allowing negative inventory balances, obscuring true product margins, and causing large month-end write-downs. Furthermore, existing bookkeeping tools permit backdated edits that retroactively alter closed accounting periods and trial balances, while customer returns often bypass quality inspection directly into active stock. In the existing codebase, the project consists solely of default empty Windows Forms template files (`Form1.vb`, `Form1.Designer.vb`, `Form1.resx`) without accounting domain models, persistence tables, or business services.

---

## Prioritized Findings (Blockers -> Major -> Nits)

### 1. Dual-Date Ledger vs. PeriodLockManager Rejection Contradiction - Severity: Blocker
* **Affected Scope**: Goals (Line 31), Period Locking Guard (Lines 242–245), and Component Responsibilities (Line 118).
* **Discrepancy Description & Rationale**:
  There is a direct contradiction between the stated purpose of the dual-date ledger and the validation rules enforced by `PeriodLockManager`.
  Line 31 establishes that the dual-date mechanism records both an immutable `System Posting Timestamp` (governing the open financial period and statutory Trial Balance) and an `Operational Event Date` (capturing physical occurrence time for operational event ordering), specifically to prevent backdated entries from altering closed periods while still capturing true historical operational dates.
  However, lines 242–245 state:
  > "`PeriodLockManager` validates both `SystemPostingTimestampUtc` and `OperationalEventDate` against `AccountingPeriods`. If an operational event falls into a period marked `IsClosed = 1`, the document posting is rejected. The user must either post into the current open period or have an authorized administrator reopen the period."
  
  If any document with an `OperationalEventDate` in a closed period is strictly rejected, then late-arriving physical events (e.g., dock receipts from the last day of a closed month entered on the first day of the new open month) cannot be recorded with their true physical occurrence date. The user would be forced to forge the `OperationalEventDate` into the open period, destroying operational ordering and defeating the documented rationale of dual dating. The specification must clarify whether late operational events are assigned to the current open fiscal period (`PeriodId`) while preserving their historical `OperationalEventDate`.

### 2. Broken GL Posting and Settlement Rules for Customer Returns - Severity: Blocker
* **Affected Scope**: CUJ 3 (Lines 66–67), Journal Posting Table Matrix (Lines 229–231), and Relational Schema Design (Lines 163, 177–179).
* **Discrepancy Description & Rationale**:
  The document specifies contradictory and mathematically broken GL posting rules for customer returns and refund disbursements:
  1. In the Journal Posting Table Matrix (Lines 229–230), Credit Notes are defined to credit `1200 Accounts Receivable`. Account `2150 Customer Credit Balances` is never credited.
  2. In line 231, Customer Refund Disbursements are defined to debit `2150 Customer Credit Balances` and credit `1020 Operating Bank Account`.
  3. If a Credit Note credits `1200 Accounts Receivable` and a subsequent cash refund debits `2150 Customer Credit Balances`, account `2150` is left with an unbacked debit balance, while `1200 Accounts Receivable` retains an unbacked credit balance. The ledger will never clear between AR and customer credits.
  4. CUJ 3 (Line 66) introduces an unexplained slash notation: `1200 Accounts Receivable / 2150 Credit Memo Balance Credit $150.00`, without defining the operational rule governing when AR is credited versus when Credit Memo Balance is credited (e.g., whether the invoice was unpaid vs. settled).
  5. Account `2150` is inconsistently named across the document: "Credit Memo Balance" (Line 66), "Customer Credit Balances" (Line 231), and `CreditMemoBalance` in table `Customers` (Line 163).

### 3. Ambiguous Accounting and Physical Status of `1350 Quarantine Inventory` - Severity: Major
* **Affected Scope**: Goals (Line 33), Journal Posting Table Matrix (Line 228), Alternatives Considered (Line 273), and Relational Schema (Lines 143–144, 172–176).
* **Discrepancy Description & Rationale**:
  The operational and accounting status of `1350 Quarantine Inventory` is contradictory.
  Lines 33 and 273 explicitly reference `1350 Quarantine Inventory` as an account code for customer returns. However, line 228 of the Journal Posting Table Matrix states that RMA dock intake has `*No Ledger Entry*` and is a `Physical quarantine log only`. Lines 229–230 then show that post-inspection restocked goods debit `1300 Merchandise Inventory` directly, while scrapped units debit `5300 Inventory Shrinkage Expense`.
  Furthermore, the schema introduces an integer column `QuarantineQty` on `Products` (Line 144) rather than tracking quarantine value in the General Ledger. It is completely unexplained whether `1350 Quarantine Inventory` exists as a GL account that receives debit/credit postings, or if quarantine tracking is solely an off-ledger quantity counter.

### 4. Missing Packaging Unit Linkage on Transactional Line Items - Severity: Major
* **Affected Scope**: Technical Plan Schema (Lines 145, 153, 155, 165, 167) and Domain Engine (Lines 95, 370).
* **Discrepancy Description & Rationale**:
  The document details packaging conversions via `UnitConversions (ConversionId PK, ProductId FK, PackUnitName, Multiplier INTEGER)` and highlights unit conversion in the domain engine (`ConvertPackagingToUnits`).
  However, none of the operational line item tables (`POLines`, `GRNLines`, `BillLines`, `SOLines`, `SalesInvoiceLines`) include a `UnitOfMeasure`, `PackUnitName`, or `ConversionId` foreign key. All lines strictly record raw integer counts (`OrderedQty`, `ReceivedQty`, `FulfilledQty`, `BilledQty`).
  The specification does not explain how packaging units are selected, displayed, or persisted on transactional documents, or whether unit conversion is purely an ephemeral calculation performed in the UI prior to writing base integer units.

### 5. Undefined Validation Rules and Trigger Conditions for ThreeWayMatchException - Severity: Major
* **Affected Scope**: Domain Business Exceptions (Line 362), Application Services (Lines 117, 387), and Automated Test Suite (Lines 460–463).
* **Discrepancy Description & Rationale**:
  The document introduces `ThreeWayMatchException` and describes `ThreeWayMatchingService` as validating PO, GRN, and Supplier Bill lines.
  However, the document also mandates that price variances are automatically expensed to `5100 Purchase Price Variance` and freight differences to `5200 Inbound Freight Variance`. The document never defines what failure criteria actually throw `ThreeWayMatchException`. For example, is it raised when `BilledQty > ReceivedQty`, when a GRN line has already been settled, or when a price difference exceeds an unstated tolerance threshold? The matching validation rules are undefined.

### 6. Ambiguity Between Sales Invoices and Delivery Notes - Severity: Major
* **Affected Scope**: Goals (Lines 28, 29), CUJ 2 (Line 60), Architecture Overview (Line 122), and Relational Schema (Lines 166–168).
* **Discrepancy Description & Rationale**:
  The document repeatedly references "Delivery Notes", "sales invoices and delivery notes", and "Sales Invoice / Delivery Note" as distinct concepts or dual documents.
  However, the schema contains no `DeliveryNotes` or `DeliveryNoteLines` tables, and the posting workflow (Lines 196–213) combines stock decrement and revenue recognition into a single `SalesInvoice` posting. The document does not clarify whether Delivery Note is an independent document entity omitted from the schema, a printed warehouse dispatch voucher generated from a Sales Invoice, or merely synonymous with Sales Invoice.

### 7. Missing Master Chart of Accounts and Missing Equity / Retained Earnings Accounts - Severity: Major
* **Affected Scope**: Relational Schema (Line 135) and Persistence Infrastructure (Line 320).
* **Discrepancy Description & Rationale**:
  Line 320 specifies that `DatabaseBootstrap` "Seeds the standard Chart of Accounts (COA: 1010 through 5400)". However, there is no master Chart of Accounts listing in the document:
  - Account `1010` is referenced in line 320 as the lower bound of the COA, but is never named or defined anywhere.
  - Equity accounts (standard 3000-series) are entirely missing from the document, despite `AccountType` defining `Equity` (Line 135) and the core requirements demanding a Balance Sheet (Line 34). A double-entry accounting system cannot produce a balanced Balance Sheet or close accounting periods without defined Equity / Retained Earnings accounts.

### 8. Inconsistent Document Status Lifecycle Terminology - Severity: Nit
* **Affected Scope**: Component Responsibilities (Line 114) vs. Domain Enums (Line 352) vs. Document Workflow Manager (Lines 87, 346).
* **Discrepancy Description & Rationale**:
  Document status terminology is inconsistent across sections:
  - Line 114 specifies statuses as `(Draft, Posted, Voided)`.
  - Line 352 defines `DocumentStatus` as `(Draft, Posted, Settled, Cancelled)`.
  - Lines 87 and 346 describe the lifecycle as `(Draft -> Posted -> Settled)`.
  The document alternates between `Voided` and `Cancelled`, and does not describe what operational action moves a posted document into `Settled`.

### 9. Discrepancy Between Integer Units and Fractional Packaging Variance - Severity: Nit
* **Affected Scope**: Non-Goals (Line 41) vs. Concurrency & Precision Boundaries (Line 241).
* **Discrepancy Description & Rationale**:
  Line 41 explicitly designates "Variable / Catch-Weight Units" as a Non-Goal and specifies that tracking is strictly anchored to atomic integer units with integer packaging multipliers. In contrast, line 241 discusses rounding variances arising from "fractional unit packaging conversions" and routing them to `5400 Stocktake Revaluation Variance`. Furthermore, account 5400 is designated for "Stocktake Revaluation", but physical inventory stocktaking is neither modeled in the schema nor supported in the MVP.

### 10. Missing Implementation Scope for Statement of Cash Flows - Severity: Nit
* **Affected Scope**: Goals (Line 34) vs. Implementation Plan (Lines 401–403, 439).
* **Discrepancy Description & Rationale**:
  Line 34 includes the "Cash Flow" statement as a core goal under Financial and Operational Reporting. However, Cash Flow reporting is omitted from both `ReportingService.vb` (Line 401) and `FinancialReportsForm.vb` (Line 439), leaving it unclear whether Cash Flow reporting is in scope for the MVP.

### 11. Unqualified BankAccountId Foreign Key References - Severity: Nit
* **Affected Scope**: Relational Schema (Lines 160, 169, 179).
* **Discrepancy Description & Rationale**:
  Tables `PaymentVouchers`, `PaymentReceipts`, and `CustomerRefundVouchers` include foreign keys named `BankAccountId FK` or `BankAccountId`. However, there is no `BankAccounts` table in the schema. The document should explicitly clarify that `BankAccountId` references `Accounts(AccountId)` where the account type is Asset (specifically `1020 Operating Bank Account`).
