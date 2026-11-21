# Implementation Plan: Reconciling Living Authoritative Specification with Physical Codebase & Governance Invariants

*   **Plan Name:** `2026-11-21_specification_drift_reconciliation_plan`
*   **Target Specification:** `docs/designs/mvp_erp_accounting.md`
*   **Active Execution Plan File:** `docs/plans/2026-11-21_specification_drift_reconciliation_plan.md`
*   **Governance Authority:** `GEMINI.md` Rule 1.3 & Rule 1.5
*   **Status:** Completed

---

## Goal Description

An audit of `2026-11-21_DRIFT_REPORT.md` confirmed that the living technical specification (`docs/designs/mvp_erp_accounting.md`) has drifted from physical codebase reality. A previous plan execution (commit `3ad2ed3`) updated the Master Manifest table to 75 files but inadvertently inserted a reference to a nonexistent account (`4900 Inventory Adjustment Gain`) in Section 2, documented a single-product scalar adjustment helper instead of the batch multi-line document API, and left the Phase 1–6 target file checklists un-synchronized.

Furthermore, a forensic deep-dive revealed additional gaps:
1. **Phase 1 Target Files Omission**: 6 tooling and solution files (`PreviewTool.vbproj`, `Program.vb`, `preview_ui.ps1`, `tools/check_workspace_hygiene.ps1`, `tools/check_stop_hygiene.ps1`, `.agents/hooks.json`) are missing from Phase 1.
2. **Journal Posting Table Matrix Omission**: Section 4 omits statutory accounting entries for `GRNIAdjustment` (clearing stale unbilled receipts to Account `5100 PPV`) and append-only Reversing Journals.
3. **Application Services Contracts**: Section 2 and Phase 4 omit customer refund disbursements, quarantine intake, fiscal period closure, statutory posting date resolution, and inventory valuation reporting.
4. **Phased Checklist Parity**: Phases 3, 4, 5, and 6 omit entity configurations, service tests, UserControls, modal dialogs, ViewModels, and test fixtures.
5. **Metadata Stagnation**: Header status is still labeled `Reviewed (Post-EGM Remediation)` and dated `2026-11-16`, despite full MVP completion.

This plan executes the complete 1:1 synchronization of `docs/designs/mvp_erp_accounting.md` without altering working production code, preserving all 217 passing unit/integration tests and 18 passing UI preview scenarios.

---

## User Review Required

> [!IMPORTANT]
> **Zero Code Modifications**: This plan does NOT modify application binaries, database schema, or test fixtures. The physical implementation is 100% operational with 217 passing tests, 0 compiler warnings, and 36 rendered preview screenshots. All changes are strictly confined to the authoritative living technical specification (`docs/designs/mvp_erp_accounting.md`).

> [!NOTE]
> **Account Resolution**: The specification will be updated to permanently reference Account `5400 Stocktake Revaluation Variance` for inventory surpluses, aligning with lines 167, 277, and 297 of the specification and `LedgerPostingEngine.vb`. Phantom Account `4900` will be eradicated.

---

## Open Questions

None. The exact class definitions, method signatures, test counts, and account numbers have been verified against the physical codebase.

---

## Proposed Changes

### Documentation Layer (`docs/designs/`)

#### [MODIFY] `docs/designs/mvp_erp_accounting.md`

##### Workstream 1: Living Specification Metadata & Section 2 Contracts
*   **Lines 4–5**: Update document header metadata to reflect living, implemented status.
    ```markdown
    *   **Author:** Max Sainz
    *   **Status:** Living Specification (Implemented & Reconciled)
    *   **Date:** 2026-11-21
    ```
*   **Lines 133–141**: Reconcile Application & Workflow Services contracts:
    - Purge `4900 Inventory Adjustment Gain`; replace with `5400 Stocktake Revaluation Variance`.
    - Document multi-line batch adjustment signature: `RecordInventoryAdjustment(documentNumber, reason, lines As IEnumerable(Of InventoryAdjustmentLine), eventDate, user)`.
    - Document `ProcureToPayService` stale GRNI clearing (`ClearStaleGRNI`) and PO short closure (`ClosePurchaseOrderShort`).
    - Document `OrderToCashService` credit limit enforcement (`CreditLimitExceededException`), SO cancellation, and SO short closure.
    - Document `ReturnsService` quarantine intake (`ReceiveReturnToQuarantine`) and customer refund disbursements (`DisburseCustomerRefund`).
    - Document `PeriodService` period closure (`CloseFiscalPeriod`) and statutory posting date resolution (`ResolveStatutoryPostingDate`).
    - Document `ReportingService` inventory valuation report (`GetInventoryValuationReport`) and GRNI aging report (`GetGrniAgingReport`).

##### Workstream 2: Section 4 Journal Posting Table Matrix
*   **Lines 280–298**: Insert missing statutory accounting entries into the Journal Posting Table Matrix:
    ```markdown
    | **GRNI Clearing Adjustment** | `2100 GRNI Clearing` | `5100 Purchase Price Variance` | $\text{Cleared Qty} \times \text{PO Unit Price}$ (Aging unbilled liability write-off) |
    | **Reversing Journal Entry** | Opposite of Target Debit Account | Opposite of Target Credit Account | Strict append-only inversion of target journal lines to correct posting errors |
    ```

##### Workstream 3: Phased Execution Strategy Checklists (Phases 1 through 6)
*   **Phase 1 (Lines 492–509)**:
    - Add to Target Files: `PreviewTool.vbproj`, `Program.vb`, `preview_ui.ps1`, `check_workspace_hygiene.ps1`, `check_stop_hygiene.ps1`, `.agents/hooks.json`.
    - Add Actionable Steps 7 & 8 documenting automated headless UI preview runner and workspace hygiene scripts/hooks.
*   **Phase 2 (Lines 516–546)**:
    - Add `CreditLimitExceededException` to Step 2.
    - Add pure costing functions: `EnsureSufficientStock`, `ConvertUnitsToPackaging`, `CalculateInventoryValuation` to Step 3.
    - Add `IsBalanced`, `AllocatePennyRounding`, `GenerateGRNIAdjustmentJournal`, `GenerateReversingJournal` to Step 4.
*   **Phase 3 (Lines 554–577)**:
    - Add to Target Files: `GeneralLedgerConfiguration.vb`, `InventoryConfiguration.vb`, `ProcurementConfiguration.vb`, `SalesConfiguration.vb`, `ReturnsConfiguration.vb`, and `PersistenceInfrastructureTests.vb`.
    - Update Step 1 to document modular `IEntityTypeConfiguration(Of T)` classes discovered via `modelBuilder.ApplyConfigurationsFromAssembly`.
*   **Phase 4 (Lines 585–600)**:
    - Add to Target Files: `InventoryService.vb`, `InventoryServiceTests.vb`, `PeriodServiceTests.vb`, `ReportingServiceTests.vb`, `SalesDashboardSettlementTests.vb`.
    - Add Step for `InventoryService` batch multi-line processing (deficits to `5300`, surpluses to `5400`).
    - Document PO/SO short-closure, SO cancellation, stale GRNI clearing, customer refund disbursements, and GRNI aging reports.
*   **Phase 5 (Lines 608–620)**:
    - Add to Target Files: `BalancedStatusBadgeControl`, `FiscalPeriodChipControl`, `DoubleEntryDistributionGridControl`, `FiscalPeriodForm`, `DisburseVendorPaymentDialog`, and 5 ViewModels (`MainShellViewModel`, `PurchasingDashboardViewModel`, `SalesDashboardViewModel`, `WarehouseIntakeViewModel`, `LedgerDashboardViewModel`).
    - Add Step 7 documenting decoupled UserControls and dual-resolution layout compliance.
*   **Phase 6 (Lines 628–636)**:
    - Add to Target Files: `BrandingAndBrandingStringsTests.vb`, `DecimalCurrencyFormattingTests.vb`, `PersistenceInfrastructureTests.vb`, `TestDatabaseFixture.vb`, `WorkspaceHygieneTests.vb`.
    - Add Step 6 documenting workspace hygiene, manifest parity, and visual layout validation.

---

## Verification Plan

### Automated Tests
1. **Comprehensive Test Suite**:
   ```powershell
   dotnet test Merchandising_Accounting_App.slnx -c Release --no-build
   ```
   *Expected outcome: 217 passed (100%), 0 failed, 0 skipped.*

2. **Workspace Hygiene & Parity Invariant**:
   ```powershell
   powershell -File .\tools\check_workspace_hygiene.ps1
   ```
   *Expected outcome: All repository invariants, compiler flags, and manifest checks pass with 0 errors.*

3. **UI Preview Offscreen Engine**:
   ```powershell
   powershell -File .\preview_ui.ps1 --all --mode both
   ```
   *Expected outcome: 36 bitmaps rendered with 0 failures.*

### Manual Verification
1. Inspect `docs/designs/mvp_erp_accounting.md` to ensure:
   - Account `4900` does not appear anywhere in the document (`grep` / search returns 0 occurrences).
   - Every file in the Master Manifest table (lines 390–478) is accounted for in the Phase 1–6 checklists.
   - Section 4 Journal Posting Table Matrix includes `GRNIAdjustment` and reversing entries.

---

## Governance & Archival Protocol (Post-Approval)

Per **GEMINI.md Rule 1.4 & Rule 1.5**:
1. Upon completing edits and passing all verification checks, move `docs/plans/2026-11-21_specification_drift_reconciliation_plan.md` to `docs/archive/plans/2026-11-21_specification_drift_reconciliation/2026-11-21_specification_drift_reconciliation_plan.md`.
2. Stage all modifications and commit locally:
   ```powershell
   git add docs/designs/mvp_erp_accounting.md docs/archive/plans/2026-11-21_specification_drift_reconciliation/
   git commit -m "docs(spec): reconcile living specification with physical implementation and governance invariants"
   ```
