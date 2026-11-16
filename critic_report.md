# Critic Review Report (Stage 7)

**Target:** `docs/designs/mvp_erp_accounting.md` (`FeatureRange: 1..296`)  
**Standards:** `resources/document_standards/feature-spec-standard.md`, Style Mandates  
**Execution Mode:** Consolidated Uninterrupted Full Flow

---

## Reviewer Status

| Reviewer | Status | Focus Lens |
| :--- | :--- | :--- |
| `egm-design` | completed | System architecture, scalability, consistency invariants, and failure modes |
| `egm-structural` | completed | Heading hierarchy, template compliance, diagram syntax, and layout rules |
| `egm-style` | completed | Technical copy editing, conciseness, claim calibration, and fluff reduction |

---

## Prioritized Findings (Blockers -> Major -> Nits)

### 1. TOCTOU Race Condition on Inventory Validation & Missing DB Constraints - Severity: Blocker
* **Reviewer Lens:** `egm-design`
* **Affected Scope:** Lines 143–144, 188–214, 235–237 (`InventoryCostingEngine` & Concurrency Model)
* **Discrepancy Description**: Stock validation (`ValidateStockAvailable`) is evaluated before acquiring the write lock and beginning the database transaction (Lines 197–202), while read queries execute concurrently via WAL mode. Two simultaneous sales orders for the same SKU can pass validation against an un-decremented balance and sequentially drive inventory negative. Furthermore, the `Products` table DDL omits a database-level `CHECK (TotalQtyOnHand >= 0)` constraint.
* **Rationale & Recommendation**: Move stock validation inside the transaction boundary under the write lock. Enforce a declarative `CHECK (TotalQtyOnHand >= 0)` constraint in SQLite DDL, and execute stock decrements via atomic conditional updates: `UPDATE Products SET TotalQtyOnHand = TotalQtyOnHand - @Qty WHERE ProductId = @Id AND TotalQtyOnHand >= @Qty;`.

### 2. Architectural Mismatch Between Multi-Role Operations and In-Process SQLite Locking - Severity: Blocker
* **Reviewer Lens:** `egm-design`
* **Affected Scope:** Lines 73–75, 235–237, 275–279 (System Architecture & Concurrency Model)
* **Discrepancy Description**: The specification describes multi-role workflows (dock receiving, order fulfillment, billing, ledger closing) across distinct warehouse and office personas. However, the persistence layer specifies embedded SQLite with an in-process .NET `SemaphoreSlim(1, 1)` serial lock. An in-process lock cannot synchronize across multiple PCs connecting over an SMB network share, where SQLite WAL mode is unsupported and causes database corruption.
* **Rationale & Recommendation**: Explicitly define the deployment topology. If the MVP is strictly restricted to a single physical workstation host, document the physical deployment constraint, single-process boundaries, and sequential operator handoffs. If multi-workstation concurrency is required across physical stations, specify an application service daemon with an IPC/HTTP boundary hosting the SQLite database.

### 3. Missing Mandatory Risks & Mitigations Section - Severity: Blocker
* **Reviewer Lens:** `egm-design`, `egm-structural`
* **Affected Scope:** Lines 280–282 (Document Structure)
* **Discrepancy Description**: The Feature Specification Standard mandates that specifications explicitly identify potential failure modes, operational risks, scalability boundaries, and mitigations. Lines 1–296 completely lack a `## Risks & Mitigations` section, omitting upstream failure modes (dock blockades, invoice latency, break-bulk divisibility errors, unbilled GRNI liabilities).
* **Rationale & Recommendation**: Add a dedicated `## Risks & Mitigations` section detailing concrete failure modes, concurrency boundaries, client power loss protections, and aging GRNI/quarantine mitigations.

### 4. GL vs. Inventory Valuation Desynchronization on Restock Returns - Severity: Major
* **Reviewer Lens:** `egm-design`
* **Affected Scope:** Lines 63–67, 122–124, 229–230 (`InventoryCostingEngine` & Returns Valuation)
* **Discrepancy Description**: Restocked returns debit `1300 Merchandise Inventory` and credit `5000 COGS` at the original sales invoice unit cost. However, the moving weighted average costing formula is defined solely for inbound purchase receipts (`PO Unit Price`). If newer purchases were received at different unit costs between sale and return, simply incrementing `TotalQtyOnHand` without mathematically blending returned units causes GL Account `1300` to permanently desynchronize from the inventory subledger valuation ($\text{TotalQtyOnHand} \times \text{CurrentUnitCost}$).
* **Rationale & Recommendation**: Formally specify the weighted average formula for restocked returns in `InventoryCostingEngine`:
  $$\text{New Average Cost} = \frac{(\text{Current Qty} \times \text{Current Unit Cost}) + (\text{Restocked Qty} \times \text{Original Invoice Unit Cost})}{\text{Current Qty} + \text{Restocked Qty}}$$

### 5. Broken Credit Memo Ledger Mechanics and Missing Credit Application Workflow - Severity: Major
* **Reviewer Lens:** `egm-design`
* **Affected Scope:** Lines 162–180, 218–232 (Customer Credit Ledger & Returns Financial Resolution)
* **Discrepancy Description**: In the posting matrix, Credit Notes are hardcoded to credit `1200 Accounts Receivable`. If an invoice was already settled, crediting AR creates an anomalous negative trade receivable rather than crediting `2150 Customer Credit Balances`. While `CustomerRefundVouchers` debits `2150`, there is no posting rule to credit `2150` on return, and zero workflow support for applying customer credits against future invoices.
* **Rationale & Recommendation**: Define conditional posting logic: credit `1200 AR` if offsetting an open invoice, or credit `2150 Customer Credit Balances` if issued as an unapplied credit memo. Add a credit application workflow (Debit `2150`, Credit `1200`) to settle future invoices with customer credit balances.

### 6. Omission of Physical Stock Adjustments from Schema and Posting Mechanics - Severity: Major
* **Reviewer Lens:** `egm-design`
* **Affected Scope:** Lines 130–181, 217–232 (Schema & Ledger Posting)
* **Discrepancy Description**: The Chart of Accounts specifies `5300 Inventory Shrinkage & Damaged Stock Expense` and `5400 Stocktake Revaluation Variance`, but the relational schema contains no tables for stock adjustments (`InventoryAdjustments`, `StockCountSheets`), and Section 4 contains zero posting rules for count adjustments.
* **Rationale & Recommendation**: Add `InventoryAdjustments` and `InventoryAdjustmentLines` tables. Define deterministic journal posting rules for inventory write-downs (Debit `5300` / Credit `1300`) and stock count surpluses (Debit `1300` / Credit `5400`).

### 7. Unresolved Open Questions Prior to Implementation Kickoff - Severity: Major
* **Reviewer Lens:** `egm-design`
* **Affected Scope:** Lines 46–50 (`## Open Questions`)
* **Discrepancy Description**: Partial dock receipts and unbilled GRNI aging mechanisms remain open questions. In a double-entry merchandising system, leaving partial PO closures undefined risks stranded commitments, and failing to define unbilled GRNI resolution leaves liabilities trapped in clearing accounts.
* **Rationale & Recommendation**: Formulate explicit architectural resolutions: (1) closing a partially fulfilled PO transitions the document to `Closed - Short`, releasing commitments without a GL entry; (2) implement a 30-day GRNI aging review with an authorized adjustment document to clear abandoned balances against `5100 PPV`.

### 8. Violation of Alternatives Rule: Personal Attribution Phrasing - Severity: Major
* **Reviewer Lens:** `egm-structural`
* **Affected Scope:** Lines 253, 258, 263, 268, 273, 278 (`## Alternatives Considered`)
* **Discrepancy Description**: The Target Standard strictly stipulates: "Rejection rationales must state concrete technical or scope trade-offs and must not attribute a rejection to a maintainer without linking their issue, discussion, or PR." All six alternatives begin with `Rejected by author: ...`.
* **Rationale & Recommendation**: Remove "Rejected by author:" across all six alternatives and state the technical trade-offs directly.

### 9. Mermaid Diagram Syntax Vulnerabilities - Severity: Major
* **Reviewer Lens:** `egm-structural`
* **Affected Scope:** Lines 99, 191
* **Discrepancy Description**: (1) Node `DB[(Embedded SQLite DB: ...)]` uses cylinder delimiters with unquoted text containing colons, commas, and `<br/>` tags. (2) Sequence diagram alias `participant DB as SQLite Transaction (merchandising_erp.db)` contains unquoted parentheses. Strict parsers fail on these unquoted delimiters.
* **Rationale & Recommendation**: Quote cylinder and alias strings: `DB[("Embedded SQLite DB: ...")]` and `participant DB as "SQLite Transaction (merchandising_erp.db)"`.

### 10. Uncalibrated Security Claim: "Tamper-Evident" Append-Only Ledgers - Severity: Major
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Line 285 (`Non-Functional Requirements > Security & Integrity`)
* **Discrepancy Description**: `CALIBRATE`. The specification labels the ledgers "Tamper-Evident", but only restricts application-level queries. Tamper evidence requires cryptographic chaining (Merkle trees/block hashing). Direct database edits remain undetected.
* **Rationale & Recommendation**: `CALIBRATE` heading and text to "Append-Only Application Ledgers".

### 11. Jargon and Contradictory Architectural Terminology - Severity: Major
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Line 74 (`1. System Architecture Overview`)
* **Discrepancy Description**: `CALIBRATE`. Describing a single `.vbproj` directory organization as "strict single-project clean architecture" combines contradictory software architecture terms into buzzword phrasing.
* **Rationale & Recommendation**: `CALIBRATE` to: "The application enforces a layered architecture within a single project, organized into Presentation, Service, Domain, and Persistence namespaces."

### 12. Marketing Hyperbole & Unprovable System Claim - Severity: Major
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Line 24 (`The Problem`)
* **Discrepancy Description**: `CALIBRATE`. Describing the solution as an "incorruptible, document-driven financial system" introduces unprovable promotional hyperbole.
* **Rationale & Recommendation**: `CALIBRATE` by replacing "incorruptible" with factual technical attributes: "The solution is a constraint-enforcing, document-driven financial system...".

### 13. Uncalibrated Latency Metric Claim - Severity: Major
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Line 278 (`Alternative 6`)
* **Discrepancy Description**: `CALIBRATE`. Claiming SQLite provides "microsecond local read performance" outruns empirical verification for complex join queries.
* **Rationale & Recommendation**: `CALIBRATE` to "low-latency, sub-millisecond local read performance" or tie directly to the $<200\text{ ms}$ query SLA.

### 14. Rigid Inbound Freight Invoice Attribution - Severity: Nit
* **Reviewer Lens:** `egm-design`
* **Affected Scope:** Lines 156–158, 221–223
* **Discrepancy Description**: Freight is modeled strictly as a line-level column on merchandise supplier bills tied to a `GRNLineId`. In practice, freight is often billed as an independent lump sum by third-party logistics carriers.
* **Rationale & Recommendation**: Allow `SupplierBills` to accept non-inventory freight lines referencing a PO/GRN without requiring a merchandise `GRNLineId`.

### 15. Improper Variance Account Mapping for Penny-Rounding - Severity: Nit
* **Reviewer Lens:** `egm-design`
* **Affected Scope:** Lines 240–241
* **Discrepancy Description**: Currency rounding variances are routed to `5400 Stocktake Revaluation Variance`, distorting physical inventory revaluation metrics.
* **Rationale & Recommendation**: Route fractional penny rounding to an administrative expense account (e.g., `5500 Cash Rounding & Roundoff Variance`).

### 16. Malformed Schema Layout - Severity: Nit
* **Reviewer Lens:** `egm-structural`
* **Affected Scope:** Lines 130–181
* **Discrepancy Description**: Schema specification is wrapped in an untyped code block with ASCII art boxes instead of structured Markdown tables or formal SQL DDL blocks.
* **Rationale & Recommendation**: Format schema models into Markdown specification tables or formal SQL DDL blocks.

### 17. Inconsistent Section Dividers and Delimiter Style - Severity: Nit
* **Reviewer Lens:** `egm-structural`
* **Affected Scope:** Lines 7, 68, 246, 280, 295
* **Discrepancy Description**: Document uses 80-character hyphen rules (`---...---`) intermittently instead of standard Markdown `---`.
* **Rationale & Recommendation**: Standardize on `---` across all major section breaks.

### 18. Asymmetric Bullet Formatting in Component Responsibilities - Severity: Nit
* **Reviewer Lens:** `egm-structural`
* **Affected Scope:** Lines 112–126
* **Discrepancy Description**: Inconsistent naming pattern and code backtick styling across bullet items.
* **Rationale & Recommendation**: Standardize naming pattern and code-span syntax uniformly across items.

### 19. Synonym Redundancy - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Line 13
* **Discrepancy Description**: "posts balanced double-entry accounting records immediately in real time" is redundant.
* **Rationale & Recommendation**: Simplify to "immediately" or "in real time".

### 20. Informal / Emotive Phrasing - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Line 11
* **Discrepancy Description**: "leaving the business blind to true stock availability..." is journalistic.
* **Rationale & Recommendation**: Rephrase to: "preventing real-time visibility into stock availability and product margins."

### 21. Subjective Modifiers - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Lines 19, 21
* **Discrepancy Description**: "massive write-downs" and "Flawed Returns Management".
* **Rationale & Recommendation**: Replace with "substantial write-downs" and "Uncontrolled Returns Processing".

### 22. Syntactic Repetition - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Line 29
* **Discrepancy Description**: Stacking "preventing... preventing" within a single sentence.
* **Rationale & Recommendation**: Streamline phrasing to remove repetition.

### 23. Promotional Adjective in Goal Definition - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Line 34
* **Discrepancy Description**: `CALIBRATE`. "Deliver instant standard financial statements".
* **Rationale & Recommendation**: `CALIBRATE` to "Deliver on-demand standard financial statements...".

### 24. Non-Technical / Subjective Vocabulary - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Line 65
* **Discrepancy Description**: Describing returned inventory as "pristine (restockable)".
* **Rationale & Recommendation**: Replace "pristine" with "undamaged" or "conforming".

### 25. Subjective Filler Adjectives - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Lines 119, 121
* **Discrepancy Description**: "fast, read-only queries" and "Pure accounting engine".
* **Rationale & Recommendation**: Calibrate to "Executes read-only queries..." and "Domain accounting engine".

### 26. Hyperbolic Modifier - Severity: Nit
* **Reviewer Lens:** `egm-style`
* **Affected Scope:** Line 257
* **Discrepancy Description**: "creates massive schema and query complexity".
* **Rationale & Recommendation**: Replace "massive" with "substantial" or "significant".
