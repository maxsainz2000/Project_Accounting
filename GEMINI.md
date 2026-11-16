# Workspace Rules: Merchandising ERP Accounting System

This workspace contains the source code, design specifications, and architecture for the **Merchandising Accounting ERP Application**, a desktop accounting system designed for small-to-mid scale merchandising businesses.

All agents, swarm workers, and tools operating within this repository must adhere strictly to the following rules and standards.

---

## 1. Authoritative Specification

* **Design Source of Truth:** `docs/designs/mvp_erp_accounting.md` is the authoritative, audit-certified technical specification for this project.
* Any ambiguity in data models, workflow transitions, accounting entries, or file paths must be resolved by adhering to `docs/designs/mvp_erp_accounting.md`.
* Architectural baselines and compiler flags are documented in `initial_architecture.md`.

---

## 2. Technology Stack & Project Topology

* **Target Framework:** .NET 10.0 Windows Forms (`net10.0-windows`).
* **Language & Runtime:** Visual Basic .NET (`MyType: WindowsForms`).
* **Solution Model:** Microsoft Visual Studio 2026 XML solution (`Merchandising_Accounting_App.slnx`) located at the repository root.
* **ORM & Database:** Entity Framework Core 10 (`Microsoft.EntityFrameworkCore.Sqlite`) targeting an embedded local SQLite file (`merchandising_erp.db`).
* **Deployment Scope:** Single-workstation standalone desktop application running on local disk storage. Multi-station distributed networking, shared SMB network paths, and external client-server databases are explicit non-goals for MVP.

---

## 3. Strict Compiler & Type Safety Invariants

* **Mandatory Compiler Flags:**
  * `<OptionStrict>On</OptionStrict>` (Mandatory: late binding, implicit narrowing conversions, and loose typing are strictly prohibited).
  * `<OptionExplicit>On</OptionExplicit>` (Mandatory: all variables must be explicitly declared).
  * `<OptionCompare>Binary</OptionCompare>` (Mandatory: case-sensitive ordinal string comparisons).
  * `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` (Mandatory: all compiler warnings must be resolved; zero-warning builds required).
* **Monetary Representation:**
  * Every financial figure, unit cost, price, currency balance, and journal line debit/credit **must strictly utilize the 128-bit `Decimal` type**.
  * Never use floating-point types (`Double`, `Single`, `Float`) for monetary calculations.
* **Physical Stock Representation:**
  * Every stock count, on-hand quantity, carton multiplier, and unit conversion **must strictly utilize `Integer`**. Fractional or floating-point stock counts are strictly forbidden.
* **Rounding Discipline:**
  * Journal balancing enforces equality to 4 decimal places: `Math.Round(debits, 4) == Math.Round(credits, 4)`.
  * Fractional penny rounding differences generated during unit conversions are routed to Account `5500 Cash Rounding & Roundoff Variance` if $|variance| \le 0.02$.

---

## 4. Invariable Accounting & Domain Rules

Every operational action in the application must preserve double-entry and perpetual inventory integrity:

1. **Hard-Stop Zero Negative Stock:**
   * The application must never allow physical inventory on hand to drop below zero.
   * Sales Invoices and delivery dispatches must be blocked if available stock is insufficient (`InsufficientStockException`).
   * Enforced at the SQLite database layer via declarative constraint `CHECK (TotalQtyOnHand >= 0)` on the `Products` table and atomic conditional updates:
     ```sql
     UPDATE Products 
     SET TotalQtyOnHand = TotalQtyOnHand - @Qty 
     WHERE ProductId = @Id AND TotalQtyOnHand >= @Qty;
     ```
2. **Perpetual Moving Weighted Average Costing:**
   * Unit costs update forward-only upon physical Goods Receipt (GRN) and restocked returns:
     $$\text{Purchase Cost} = \frac{(\text{Current Qty} \times \text{Current Cost}) + (\text{Received Qty} \times \text{PO Unit Price})}{\text{Current Qty} + \text{Received Qty}}$$
     $$\text{Restock Cost} = \frac{(\text{Current Qty} \times \text{Current Cost}) + (\text{Restocked Qty} \times \text{Original Invoice Cost})}{\text{Current Qty} + \text{Restocked Qty}}$$
   * Never execute retroactive revaluations that alter past closed periods or past Cost of Goods Sold entries.
3. **Append-Only Double-Entry General Ledger:**
   * Every operational document (GRN, Supplier Bill, Sales Invoice, Credit Note, Payment Voucher) generates balanced debit/credit lines.
   * Tables `JournalEntries` and `JournalEntryLines` are strictly append-only. `UPDATE` and `DELETE` operations on posted records are prohibited.
   * Errors are corrected exclusively via explicit reversing documents.
4. **Immutable Closed Fiscal Periods:**
   * The statutory posting date (`PostingDate`) must map to the current open fiscal period. Closed periods are permanently sealed.
   * Late physical events physically occurring in a closed period receive a `PostingDate` on the first open day of the current open period, while recording the physical event time in `OperationalEventDate`.
5. **Two-Stage Customer Returns:**
   * Returns arrive in non-valuated operational quarantine (`QuarantineQty`) first.
   * Physical inspection determines item disposition (`Restock` vs. `Scrap`).
   * Financial Credit Notes credit `1200 Accounts Receivable` (subledger credit balance) and post restocked items to `1300 Merchandise Inventory` (with moving average cost recalculation) or scrapped items to `5300 Inventory Shrinkage & Damaged Stock Expense`.
6. **Three-Way Matching & GRNI Clearing:**
   * Stock receipts credit `2100 Goods Received Not Invoiced (GRNI)` at purchase order price.
   * Supplier bill price variances route to `5100 Purchase Price Variance (PPV)`, and uncapitalized carrier freight lines route to `5200 Inbound Freight Variance`.

---

## 5. Architectural Layering & Code Conventions

Code must be strictly partitioned into four distinct layers within the `Merchandising_Accounting_App` project:

```
[ UI / Presentation ]   --> MainShellForm, Dashboard Forms, ViewModels, Designers (.Designer.vb, .resx)
          │
          ▼
[ Application Services ] --> ProcureToPayService, OrderToCashService, ReturnsService, ReportingService, PeriodService
          │
          ▼
[ Domain Layer ]        --> Pure Engines (LedgerPostingEngine, InventoryCostingEngine), Entities, Enums, Exceptions
          │
          ▼
[ Persistence Layer ]   --> ErpDbContext, Repositories, Migrations, SQLite WAL & Pragmas
```

* **No Business Logic in UI Event Handlers:**
  * Form event handlers (`Button_Click`, `CellContentClick`, `SelectedIndexChanged`) must remain thin presentation wrappers.
  * Forms must never execute SQL queries, create DbContext instances directly, or instantiate raw journal entries. All actions must delegate to Application Services or Domain Engines.
* **Windows Forms File Discipline:**
  * Visual layout and component initialization belong in `FormName.Designer.vb`.
  * Visual event handlers and data-binding logic belong in `FormName.vb`.
  * Embedded resources and string manifests belong in `FormName.resx`.
  * When adding a new form, all three companion files must be created.
* **Modern Windows Forms Application Defaults:**
  * Typography: `Segoe UI Variable Text` at 9.5pt.
  * DPI Mode: `HighDpiMode.PerMonitorV2`.
  * Color Mode: `SystemColorMode.System` (respecting Windows OS light/dark mode).
  * Configured in `ApplicationEvents.vb` under `ApplyApplicationDefaults`.
* **Testing Hygiene:**
  * All unit and integration test fixtures reside in `Merchandising_Accounting_App.Tests`.
  * Domain engines (`LedgerPostingEngine`, `InventoryCostingEngine`) must maintain $\ge 90\%$ branch coverage.
  * Application workflow services must maintain $\ge 85\%$ statement coverage.
