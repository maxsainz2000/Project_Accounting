# Merchandising ERP: Functional Core Transition (/swarm Runbook)

> **Execution Context:** This runbook is structured for direct execution by a swarm coordinator in an Antigravity `/swarm` session. Each phase defines exact `swarm{action: "add", ...}` tool calls, worker names, isolated workspace modes (`share` vs `inherit`), and verifiable completion criteria.

---

## 1. Architectural Context & Prerequisites

The presentation shell and all domain dashboards (`MainShellForm`, `LedgerDashboardForm`, `PurchasingDashboardForm`, `SalesDashboardForm`, `WarehouseIntakeForm`, `FinancialReportsForm`) are complete and verified across both `1280x720` (1000px content host) and `1920x1080` viewports, satisfying the **Zero Horizontal Scrollbar Invariant** via 34 automated preview screenshots.

**Goal:** Transition from in-memory UI mock repositories to the real SQLite Entity Framework Core 10 functional core specified in `docs/designs/mvp_erp_accounting.md`.

### Critical Workspace Invariants
- **Strict Typing:** `<OptionStrict>On</OptionStrict>` and `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>`.
- **Warning NU1903 Bypass:** To allow `Microsoft.EntityFrameworkCore.Sqlite` 10.0.0 without triggering a build break on transitive dependency `SQLitePCLRaw.lib.e_sqlite3`, `<WarningsNotAsErrors>NU1903</WarningsNotAsErrors>` must be declared in `Merchandising_Accounting_App.vbproj`.
- **Parallel Workspaces:** Because concurrent `dotnet build` calls in the same directory collide on file locks in `bin/` and `obj/`, author workers run with `workspace: "share"`. Merges occur at phase verification gates.

---

## 2. Swarm Topology

```mermaid
graph TD
    subgraph Phase 0: Pre-Flight Foundation Contract
        W0["Worker: preflight (inherit)<br/>(vbproj package setup, WarningsNotAsErrors, 17 Entity POCOs, Enums, Exceptions)"]
    end

    subgraph Phase 1: Foundational Core (3 Concurrent Workers)
        W1["Worker: engines (share)<br/>(InventoryCostingEngine, LedgerPostingEngine, Domain Unit Tests)"]
        W2["Worker: persistence (share)<br/>(ErpDbContext, EntityConfigurations, DatabaseInitializer, Repositories)"]
        W3["Worker: periodsvc (share)<br/>(PeriodService, ReportingService, Period & Reporting Tests)"]
        V1["Worker: verifier-1 (inherit)<br/>(Merge shares, run Gate 1 test suite)"]
    end

    subgraph Phase 2: Workflow Services (2 Concurrent Workers)
        W4["Worker: p2psvc (share)<br/>(ProcureToPayService, 3-Way Match, PPV/Freight Routing, P2P Tests)"]
        W5["Worker: o2csvc (share)<br/>(OrderToCashService, Stock Guard, ReturnsService Quarantine, O2C Tests)"]
        V2["Worker: verifier-2 (inherit)<br/>(Merge shares, run Gate 2 test suite)"]
    end

    subgraph Phase 3: UI Integration & Verification (2 Workers)
        W6["Worker: uibridge (share)<br/>(Replace Mocks, Bind Live Services, Live StatusStrip Connection)"]
        W7["Worker: verifier (inherit)<br/>(EndToEndCUJTests CUJ 1-5, Headless UI Preview Verification)"]
    end

    W0 --> W1
    W0 --> W2
    W0 --> W3
    W1 --> V1
    W2 --> V1
    W3 --> V1
    V1 --> W4
    V1 --> W5
    W4 --> V2
    W5 --> V2
    V2 --> W6
    W6 --> W7
```

---

## 3. Turnkey Execution Commands

### Swarm Initialization
```json
swarm{action: "title", title: "Merchandising ERP Functional Core Transition"}
swarm{action: "add", name: "monitor", brief: "You are this swarm's stall monitor. Load the swarm_monitor skill and follow it. Post only to break a stall."}
```

---

### Phase 0: Pre-Flight Foundation Contract

**Worker:** `preflight`  
**Workspace:** `inherit` (Establishes root project configuration and POCOs before branch forking)

```json
swarm{
  "action": "add",
  "name": "preflight",
  "workspace": "inherit",
  "brief": "You own establishing the foundational domain contracts and package dependencies for Merchandising ERP.\n\nTASKS:\n1. In Merchandising_Accounting_App/Merchandising_Accounting_App.vbproj:\n   - Add <PackageReference Include=\"Microsoft.EntityFrameworkCore.Sqlite\" Version=\"10.0.0\" />\n   - Add <WarningsNotAsErrors>NU1903</WarningsNotAsErrors> (to bypass SQLitePCLRaw vulnerability halt under TreatWarningsAsErrors)\n   - Add <Import Include=\"Microsoft.EntityFrameworkCore\" />\n2. In Merchandising_Accounting_App/Domain/Enums.vb:\n   - Define AccountType, DocumentStatus (Draft, Posted, Settled, Cancelled), ReturnDisposition (Restock, Scrap), AdjustmentReason.\n3. In Merchandising_Accounting_App/Domain/Exceptions.vb:\n   - Define InsufficientStockException, UnbalancedJournalEntryException, ClosedPeriodPostingException, ThreeWayMatchException.\n4. In Merchandising_Accounting_App/Domain/Models/ (17 POCOs matching docs/designs/mvp_erp_accounting.md lines 151-179):\n   - GeneralLedgerEntities.vb: Account, AccountingPeriod, JournalEntry, JournalEntryLine\n   - InventoryEntities.vb: Product, InventoryTransaction, InventoryAdjustment, InventoryAdjustmentLine\n   - ProcurementEntities.vb: Vendor, PurchaseOrder, POLine, GoodsReceiptNote, GRNLine, SupplierBill, BillLine, PaymentVoucher\n   - SalesEntities.vb: Customer, SalesOrder, SOLine, SalesInvoice, SalesInvoiceLine, PaymentReceipt\n   - ReturnsEntities.vb: CustomerReturn, CustomerReturnLine\n\nCOMPLETION CRITERIA:\nRun 'dotnet build Merchandising_Accounting_App.slnx -c Release'. Must compile with 0 errors and 0 warnings under Option Strict On and TreatWarningsAsErrors. Once passing, commit your changes to git master and post: '@coordinator [done] Foundation contracts and POCOs ready with clean build'."
}
```

*Gate 0:* On `[done]`, Coordinator executes:
```json
swarm{action: "kill", name: "preflight"}
```

---

### Phase 1: Foundational Core (3 Parallel Workers + Verifier)

Coordinator executes all 3 `add` calls in one turn:

#### Worker: `engines`
```json
swarm{
  "action": "add",
  "name": "engines",
  "workspace": "share",
  "brief": "You own pure domain engines and calculation accuracy. Do not touch UI or database files.\n\nTASKS:\n1. In Merchandising_Accounting_App/Domain/Engines/InventoryCostingEngine.vb:\n   - Implement ValidateSufficientStock(currentQty As Integer, requiredQty As Integer) As Boolean\n   - Implement perpetual moving average costing: NewCost = ((CurrentQty * CurrentCost) + (ReceivedQty * UnitPrice)) / (CurrentQty + ReceivedQty)\n   - Implement restocked return costing: RestockCost = ((CurrentQty * CurrentCost) + (RestockedQty * OriginalCost)) / (CurrentQty + RestockedQty)\n   - Implement carton conversion: ConvertPackagingToUnits(packQty As Integer, multiplier As Integer) As Integer\n2. In Merchandising_Accounting_App/Domain/Engines/LedgerPostingEngine.vb:\n   - Implement double-entry balance validator: Math.Round(debits, 4) == Math.Round(credits, 4)\n   - Implement penny-rounding allocation to Account 5500 Cash Rounding & Roundoff Variance if |variance| <= 0.02\n   - Implement deterministic journal entry generators for GRN, Supplier Bill, Sales Invoice, Credit Note, Payment Voucher, Payment Receipt\n3. In Merchandising_Accounting_App.Tests/Domain/:\n   - Author InventoryCostingEngineTests.vb and LedgerPostingEngineTests.vb (target >= 90% branch coverage)\n\nCOMPLETION CRITERIA:\nRun 'dotnet test Merchandising_Accounting_App.slnx -c Release --filter \"FullyQualifiedName~Domain\"'. All tests must pass with 0 warnings. Commit changes in your workspace and post: '@coordinator [done] Pure domain engines and unit tests complete'."
}
```

#### Worker: `persistence`
```json
swarm{
  "action": "add",
  "name": "persistence",
  "workspace": "share",
  "brief": "You own the EF Core SQLite persistence layer and repositories. Do not touch UI or domain calculation engines.\n\nTASKS:\n1. In Merchandising_Accounting_App/Data/ErpDbContext.vb:\n   - Configure SQLite connection targeting merchandising_erp.db.\n   - Declare DbSet for all 17 tables; invoke modelBuilder.ApplyConfigurationsFromAssembly.\n2. In Merchandising_Accounting_App/Data/Configurations/:\n   - Decoupled IEntityTypeConfiguration(Of T) for Product, JournalEntry, Procurement, Sales.\n   - Enforce HasPrecision(18, 4) on all currency properties.\n   - Declare SQLite table constraint: CHECK (TotalQtyOnHand >= 0) on Products.\n3. In Merchandising_Accounting_App/Data/DatabaseInitializer.vb:\n   - Apply SQLite pragmas: PRAGMA journal_mode = WAL; PRAGMA synchronous = NORMAL; PRAGMA foreign_keys = ON; PRAGMA busy_timeout = 3000;\n   - Call context.Database.EnsureCreated();\n   - Seed Master Chart of Accounts (1010 through 5500) and initial open period FY2026-P11.\n4. In Merchandising_Accounting_App/Data/Repositories/:\n   - ProductRepository.vb: Implement atomic conditional decrement: UPDATE Products SET TotalQtyOnHand = TotalQtyOnHand - @Qty WHERE ProductId = @Id AND TotalQtyOnHand >= @Qty;\n   - LedgerRepository.vb: Append-only insert (reject update/delete).\n   - AccountRepository.vb, AccountingPeriodRepository.vb, DocumentRepository.vb.\n5. In Merchandising_Accounting_App.Tests/Infrastructure/TestDatabaseFixture.vb:\n   - In-memory SQLite connection (DataSource=:memory:) for integration tests.\n\nCOMPLETION CRITERIA:\nRun 'dotnet test Merchandising_Accounting_App.slnx -c Release --filter \"FullyQualifiedName~Infrastructure\"'. Must pass 100%. Commit in your workspace and post: '@coordinator [done] EF Core persistence, WAL pragmas, and repositories complete'."
}
```

#### Worker: `periodsvc`
```json
swarm{
  "action": "add",
  "name": "periodsvc",
  "workspace": "share",
  "brief": "You own the statutory fiscal period guard and financial reporting queries. Do not touch UI or operational workflows.\n\nTASKS:\n1. In Merchandising_Accounting_App/Services/PeriodService.vb:\n   - Implement ValidatePostingDate(postingDate As DateTime): throws ClosedPeriodPostingException if period IsClosed = True.\n   - Implement GetActiveFiscalPeriod() As AccountingPeriod.\n   - Implement CloseFiscalPeriod(periodId As Integer): seals period with ClosedAtUtc.\n   - Implement ResolveStatutoryPostingDate(eventDate As DateTime): maps closed period events to first open day of current period while preserving OperationalEventDate.\n2. In Merchandising_Accounting_App/Services/ReportingService.vb:\n   - Implement compiled queries for Trial Balance (sum Debits == sum Credits), Balance Sheet (Assets = Liabilities + Equity), Income Statement, and Product Margin & Profitability Bridge.\n3. In Merchandising_Accounting_App.Tests/Services/:\n   - Author PeriodServiceTests.vb and ReportingServiceTests.vb.\n\nCOMPLETION CRITERIA:\nRun 'dotnet test Merchandising_Accounting_App.slnx -c Release --filter \"FullyQualifiedName~Period|FullyQualifiedName~Reporting\"'. Must pass 100%. Commit in your workspace and post: '@coordinator [done] Period and Reporting services complete'."
}
```

*Gate 1:* Merge workspace branches into `master`. Coordinator runs:
```json
swarm{
  "action": "add",
  "name": "verifier-1",
  "workspace": "inherit",
  "brief": "You are Gate 1 Verifier. Run 'dotnet test Merchandising_Accounting_App.slnx -c Release'. Assert 0 build errors, 0 warnings, and 100% test pass rate across Domain, Infrastructure, Period, and Reporting test suites. Post: '@coordinator [done] Gate 1 verified successfully'."
}
```
On `[done]`, Coordinator kills `engines`, `persistence`, `periodsvc`, and `verifier-1`.

---

### Phase 2: Workflow Services (2 Parallel Workers + Verifier)

Coordinator executes both `add` calls in one turn:

#### Worker: `p2psvc`
```json
swarm{
  "action": "add",
  "name": "p2psvc",
  "workspace": "share",
  "brief": "You own the Procure-to-Pay workflow service. Do not touch UI or Sales workflows.\n\nTASKS:\n1. In Merchandising_Accounting_App/Services/ProcureToPayService.vb:\n   - Implement CreatePurchaseOrder(vendorId, lines) As PurchaseOrder\n   - Implement ReceiveGoods(poId, receiptLines, eventDate) As GoodsReceiptNote: atomic stock increment, moving average cost recalculation via InventoryCostingEngine, journal generation (Debit 1300 Inventory, Credit 2100 GRNI).\n   - Implement MatchSupplierBill(vendorId, grnId, billLines, billDocNum, invoiceDate): validate billed qty <= received qty; route price variances to 5100 PPV and uncapitalized freight to 5200 Inbound Freight Variance; credit 2000 AP.\n   - Implement DisbursePaymentVoucher(vendorId, billId, amount, bankAccountId) As PaymentVoucher: Debit 2000 AP, Credit 1020 Operating Bank Account.\n2. In Merchandising_Accounting_App.Tests/Services/ProcureToPayServiceTests.vb:\n   - Author comprehensive workflow tests achieving >= 85% statement coverage.\n\nCOMPLETION CRITERIA:\nRun 'dotnet test Merchandising_Accounting_App.slnx -c Release --filter \"FullyQualifiedName~ProcureToPay\"'. Must pass 100%. Commit in your workspace and post: '@coordinator [done] ProcureToPay workflow service complete'."
}
```

#### Worker: `o2csvc`
```json
swarm{
  "action": "add",
  "name": "o2csvc",
  "workspace": "share",
  "brief": "You own the Order-to-Cash and Customer Returns workflow services. Do not touch UI or Procurement workflows.\n\nTASKS:\n1. In Merchandising_Accounting_App/Services/OrderToCashService.vb:\n   - Implement CreateSalesOrder(customerId, lines) As SalesOrder\n   - Implement FulfillSalesInvoice(soId, lines, postingDate) As SalesInvoice:\n     * Validate stock pre-check via InventoryCostingEngine.ValidateSufficientStock.\n     * Execute atomic conditional update via ProductRepository. Throw InsufficientStockException on shortfall.\n     * Emit balanced journal: Debit 1200 AR, Credit 4000 Sales Revenue, Debit 5000 COGS, Credit 1300 Inventory.\n   - Implement RecordPaymentReceipt(customerId, invoiceId, amount, bankAccountId) As PaymentReceipt: Debit 1020 Bank, Credit 1200 AR.\n2. In Merchandising_Accounting_App/Services/ReturnsService.vb:\n   - Implement ReceiveReturnToQuarantine(customerId, invoiceId, lines): increment QuarantineQty with 0 general ledger entries.\n   - Implement ProcessReturnInspection(returnId, dispositions):\n     * Restock items: Debit 1300 Inventory, Credit 5000 COGS, recalculate moving average cost.\n     * Scrap items: Debit 5300 Inventory Shrinkage, Credit 5000 COGS.\n     * Credit Note: Debit 4100 Sales Returns & Allowances, Credit 1200 AR.\n3. In Merchandising_Accounting_App.Tests/Services/:\n   - Author OrderToCashServiceTests.vb and ReturnsServiceTests.vb (target >= 85% statement coverage).\n\nCOMPLETION CRITERIA:\nRun 'dotnet test Merchandising_Accounting_App.slnx -c Release --filter \"FullyQualifiedName~OrderToCash|FullyQualifiedName~Returns\"'. Must pass 100%. Commit in your workspace and post: '@coordinator [done] OrderToCash and Returns services complete'."
}
```

*Gate 2:* Merge workspace branches into `master`. Coordinator runs `verifier-2` (`workspace: "inherit"`):
```json
swarm{
  "action": "add",
  "name": "verifier-2",
  "workspace": "inherit",
  "brief": "You are Gate 2 Verifier. Run 'dotnet test Merchandising_Accounting_App.slnx -c Release'. Assert 0 build errors, 0 warnings, and 100% test pass rate across all services. Post: '@coordinator [done] Gate 2 verified successfully'."
}
```
On `[done]`, Coordinator kills `p2psvc`, `o2csvc`, and `verifier-2`.

---

### Phase 3: Integration, UI ViewModels Bridge & E2E Verification

#### Worker: `uibridge`
```json
swarm{
  "action": "add",
  "name": "uibridge",
  "workspace": "share",
  "brief": "You own refactoring UI ViewModels and forms to bind to real Application Services and Repositories, eliminating mock stores while maintaining zero visual layout regression.\n\nTASKS:\n1. In UI/Accounting/LedgerDashboardForm.vb & LedgerViewModels.vb: Bind to LedgerRepository and PeriodService. Replace MockLedgerStore.\n2. In UI/Purchasing/PurchasingDashboardForm.vb & PurchasingViewModels.vb: Bind to ProcureToPayService. Replace PurchasingMockRepository.\n3. In UI/Sales/SalesDashboardForm.vb & SalesViewModels.vb: Bind to OrderToCashService. Replace MockSalesDataService.\n4. In UI/Warehouse/WarehouseIntakeForm.vb & WarehouseViewModels.vb: Bind to ProcureToPayService and ReturnsService. Replace WarehouseMockRepository.\n5. In UI/Reporting/FinancialReportsForm.vb & ReportingViewModels.vb: Bind to ReportingService. Replace MockReportingStore.\n6. In UI/MainShellForm.vb:\n   - Initialize database in MainShellForm_Load via DatabaseInitializer.InitializeDatabase().\n   - Update status strip: tsLblDbStatus.Text = \"Database: merchandising_erp.db (Connected - WAL)\"; tsLblFiscalPeriod.Text = $\"Active Period: {activePeriod.FiscalYear}-P{activePeriod.PeriodNumber:D2}\".\n\nCOMPLETION CRITERIA:\nRun 'dotnet build Merchandising_Accounting_App.slnx -c Release'. 0 errors, 0 warnings. Commit in your workspace and post: '@coordinator [done] UI ViewModels connected to live services'."
}
```

*Merge `uibridge` into `master`.*

#### Worker: `verifier`
```json
swarm{
  "action": "add",
  "name": "verifier",
  "workspace": "inherit",
  "brief": "You own authoring statutory accounting End-to-End CUJ integration tests and running visual regression previews.\n\nTASKS:\n1. In Merchandising_Accounting_App.Tests/Integration/EndToEndCUJTests.vb, author:\n   - CUJ 1: Dock Intake (PO -> GRN -> stock increase, 1300 debit, 2100 credit, moving average cost).\n   - CUJ 2: Sales Stock Guard (attempt oversell -> assert InsufficientStockException; post valid invoice -> assert stock decrement & double-entry GL).\n   - CUJ 3: Two-Stage Customer Return (quarantine receipt with 0 GL -> inspection 8 restock / 2 scrap -> Credit Note, 5300 shrinkage debit, 1300 restock revaluation).\n   - CUJ 4: 3-Way Match with Variances (GRNI clearing, 5100 PPV debit, 5200 Inbound Freight debit, 2000 AP credit).\n   - CUJ 5: Period Lock (posting in sealed period throws ClosedPeriodPostingException).\n2. Run automated test suite: 'dotnet test Merchandising_Accounting_App.slnx -c Release'.\n3. Run visual preview regression: 'powershell -File .\\preview_ui.ps1 --all --mode both'.\n   - Verify all 34 screenshots render without errors, no horizontal scrollbars at 1280x720, and zero visual clipping.\n\nCOMPLETION CRITERIA:\nAll tests pass (0 failures), preview script succeeds. Post: '@coordinator [done] Full E2E CUJ and visual preview regression verified'."
}
```

---

## 4. Final Sign-off

When `verifier` posts `[done]`:
1. Coordinator verifies the test and preview outputs.
2. Posts `[done] Merchandising ERP Functional Core Transition completed successfully.` with no `@mention` so `monitor` silences.
3. Reports completion to the user.
