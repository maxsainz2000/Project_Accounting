# MVP ERP Accounting System for Small-to-Mid Scale Merchandising Businesses

* **Author:** Max Sainz
* **Date:** 2026-11-16

## Overview

This document specifies the functional architecture and domain rules for the Minimum Viable Product (MVP) of an Enterprise Resource Planning (ERP) Accounting System. The system is designed for small-to-mid scale merchandising businesses, including wholesale distributors, import-to-retail merchants, and consumer goods distributors.

Basic bookkeeping tools and spreadsheets fail these businesses because physical warehouse movements occur independently of the General Ledger. This disconnect causes phantom inventory valuations, inaccurate margins, unrecorded shrinkage, and delayed month-end closes. 

This MVP resolves this problem by implementing an incorruptible, document-driven financial backbone. Every operational action (receiving stock, dispatching orders, inspecting returns, or issuing bills) automatically triggers balanced double-entry General Ledger postings in real time. The system enforces strict append-only record keeping, a hard block on negative inventory balances, forward-only inventory costing, single-warehouse operations, and single-currency accounting.

---

## Analysis

### 1. Domain Mechanics & Accounting Engine

#### Chart of Accounts (COA) Structure
The engine relies on a standardized, balanced Chart of Accounts configured for merchandising workflows:
* **Assets:**
  * `1010 Cash on Hand` / `1020 Operating Bank Account`
  * `1200 Accounts Receivable (Trade Debtors)`
  * `1300 Merchandise Inventory (On-Hand Asset)`
  * `1350 Quarantine Inventory (Physical Holding / Non-Valuated Interim Stock)`
* **Liabilities & Clearing:**
  * `2000 Accounts Payable (Trade Creditors)`
  * `2100 Goods Received Not Invoiced (GRNI Clearing)`
  * `2150 Customer Credit Memo Balances (Unapplied Customer Credits)`
* **Equity:**
  * `3000 Owner's Capital` / `3100 Retained Earnings`
  * `3900 Opening Balance Suspense`
* **Revenue & Contra-Revenue:**
  * `4000 Sales Revenue`
  * `4100 Sales Returns & Allowances (Contra-Revenue)`
* **Cost of Goods Sold (Direct):**
  * `5000 Cost of Goods Sold (COGS)`
* **Operating & Variance Expenses:**
  * `5100 Purchase Price Variance (PPV)`
  * `5200 Inbound Freight & Landed Cost Variance`
  * `5300 Inventory Shrinkage & Damaged Stock Expense`
  * `5400 Stocktake Revaluation Variance`

#### Deterministic Journal Entry Rules by Document
Operational documents execute balanced debits and credits automatically upon commitment:

1. **Procure-to-Pay (P2P):**
   * *Purchase Order (PO):* Commercial commitment only. Zero General Ledger entry.
   * *Goods Receipt Note (GRN):* Physical receipt of goods valued at the agreed PO unit price.
     * **Debit:** `1300 Merchandise Inventory` ($\text{Quantity} \times \text{PO Unit Price}$)
     * **Credit:** `2100 Goods Received Not Invoiced (GRNI)`
   * *Supplier Bill (AP Invoice):* Three-way match against GRN. If the billed price differs from the PO, or if separate freight is included:
     * **Debit:** `2100 GRNI` (clearing the exact PO valuation recorded on the GRN)
     * **Debit / Credit:** `5100 Purchase Price Variance` (difference between billed price and PO price)
     * **Debit:** `5200 Inbound Freight & Landed Cost Variance` (billed freight lines)
     * **Credit:** `2000 Accounts Payable` (total obligation owed to vendor)
   * *Payment Voucher:* Vendor settlement.
     * **Debit:** `2000 Accounts Payable`
     * **Credit:** `1020 Operating Bank Account`

2. **Order-to-Cash (O2C):**
   * *Sales Order (SO):* Customer commitment and stock reservation. Zero General Ledger entry.
   * *Sales Invoice & Delivery Note:* Posted simultaneously upon order fulfillment (guarded by the stock availability check).
     * **Debit:** `1200 Accounts Receivable` ($\text{Quantity} \times \text{Selling Price}$)
     * **Credit:** `4000 Sales Revenue`
     * **Debit:** `5000 Cost of Goods Sold` ($\text{Quantity} \times \text{Current Moving Average Cost}$)
     * **Credit:** `1300 Merchandise Inventory`
   * *Payment Receipt:* Customer settlement.
     * **Debit:** `1020 Operating Bank Account`
     * **Credit:** `1200 Accounts Receivable`

3. **Two-Stage Customer Returns:**
   * *Stage 1 (Physical Intake):* Return Merchandise Authorization (RMA) / Return Receipt logs physical arrival into Quarantine. Warehouse clerk inspects goods and records item disposition ("Restockable" or "Damaged/Scrap"). Zero General Ledger entry.
   * *Stage 2 (Financial Resolution):* Accounting generates a Credit Note linked to the completed inspection record:
     * *Revenue Reversal:*
       * **Debit:** `4100 Sales Returns & Allowances` (invoiced selling price)
       * **Credit:** `1200 Accounts Receivable` (or `2150 Customer Credit Memo Balance` if original invoice was already paid)
     * *Stock Restock Line (Restockable items):*
       * **Debit:** `1300 Merchandise Inventory` ($\text{Restocked Qty} \times \text{Original Invoice Unit Cost}$)
       * **Credit:** `5000 Cost of Goods Sold`
     * *Stock Write-Off Line (Scrapped items):*
       * **Debit:** `5300 Inventory Shrinkage & Damaged Stock Expense` ($\text{Scrapped Qty} \times \text{Original Invoice Unit Cost}$)
       * **Credit:** `5000 Cost of Goods Sold`

---

### 2. Inventive Contradictions & Systematic Resolutions

```
+---------------------------------------------------------------------------------------------------------+
|                                    INVENTIVE RESOLUTION MATRIX                                          |
+---------------------------------------------------------------------------------------------------------+
| Contradiction 1: Hard-Stop Zero Negative Stock  VS  Sales Velocity & Urgency to Close                  |
| RESOLUTION: "Decoupled Reservation & ATP Commitments"                                                   |
|             (Order Confirmation != Physical Dispatch; Inbound POs display expected fulfillment dates)  |
+---------------------------------------------------------------------------------------------------------+
| Contradiction 2: Strict Append-Only Timestamp   VS  Human Operational Delay (Late Friday Shipments)     |
| RESOLUTION: "Dual-Date Ledger Architecture"                                                             |
|             (System Posting Timestamp for GL Integrity + Operational Event Date for Analytics)          |
+---------------------------------------------------------------------------------------------------------+
| Contradiction 3: Strict No-Cash Refunds Policy   VS  Customer Churn / Retention Friction                |
| RESOLUTION: "AP Settlement Offset Vouchers"                                                             |
|             (Credit Note remains default; Manager-Authorized AP Voucher enables formal disbursement)    |
+---------------------------------------------------------------------------------------------------------+
| Contradiction 4: Forward-Only Variance Dumping  VS  Accurate Realized Product Gross Margin Analysis     |
| RESOLUTION: "Managerial Profitability Decoupling"                                                       |
|             (Ledger remains pristine; Reporting Engine provides synthetic landed-cost margin analytics)|
+---------------------------------------------------------------------------------------------------------+
```

#### Contradiction 1: Hard-Stop Stock Availability vs. Sales Urgency
* **Trade-off:** Sales representatives lose deals if the system strictly prevents issuing an invoice when on-hand stock reads zero, even when goods are sitting on the receiving dock or inbound on a confirmed delivery truck.
* **Resolution (Decoupled Reservation & ATP Commitments):** Decouple commercial commitment from physical stock fulfillment. Sales reps can issue confirmed **Sales Orders** against **Available-to-Promise (ATP)** inbound pipeline (linking to open Purchase Orders with confirmed dock delivery dates). The sales order confirms pricing and commits the customer, while the system strictly disables generation of the official Sales Invoice and physical Delivery Note until the warehouse clerk posts the Goods Receipt.

#### Contradiction 2: Strict Append-Only Timestamps vs. Real-World Operational Delays
* **Trade-off:** A delivery truck arrives Friday afternoon at 4:45 PM and stock is physically unloaded, but the warehouse clerk enters the Goods Receipt on Monday morning at 9:00 AM. Using Monday's timestamp means late Friday outbound deliveries fail the hard-stop validation because the system considers stock unavailable until Monday.
* **Resolution (Dual-Date Ledger Architecture):** Every operational document records two distinct date fields:
  1. `System Posting Timestamp`: An immutable, system-clock timestamp assigned to the active open accounting period. This governs the General Ledger, Trial Balance, and financial period locks.
  2. `Operational Event Date`: A user-specified date capturing when the physical event occurred in the warehouse. 
  The inventory availability checker evaluates inventory based on operational event sequence within the open period, while financial postings follow the append-only ledger timeline.

#### Contradiction 3: Strict Credit-Only Returns vs. Customer Dispute Escalation
* **Trade-off:** The baseline requires all returns to issue credit memo balances with zero cash refunds. If an aggrieved customer terminates their account or returns defective items and demands repayment, refusing a cash refund creates legal or commercial conflict.
* **Resolution (Controlled AP Refund Disbursement Voucher):** The Credit Note remains the only document that reverses the sale and credits the customer's AR balance. Frontline clerks cannot handle cash at the counter. However, to handle formal account closures, the system provides an administrative document: **Customer Refund Disbursement Voucher**. This document debits `2150 Customer Credit Memo Balances` and credits `1020 Operating Bank Account`, requiring explicit manager authorization.

#### Contradiction 4: Forward-Only Variance Expensing vs. SKU-Level Margin Visibility
* **Trade-off:** Expensing late-arriving freight and supplier price variances directly to `5100 PPV` and `5200 Freight Expense` preserves the integrity of past inventory valuations and prevents retroactive changes to the Trial Balance. However, it blinds the business owner to the true landed margin of specific SKUs on standard financial statements.
* **Resolution (Managerial Margin Decoupling):** Maintain a two-layer financial view:
  1. *Statutory General Ledger Layer:* Strictly append-only and forward-only. Variances are expensed directly in the current open period. Audited financial statements remain stable.
  2. *Managerial Analytical Layer:* An analytical report matches posted PPV lines and freight invoices back to their parent Purchase Order line items. The **Product Profitability Report** displays both the standard gross trading margin (invoice price less base unit cost) and the attributed landed variance per SKU, giving true economic profitability without altering General Ledger balances.

---

### 3. Secondary Operational Risks & Mitigations

| Risk | Cause | Operational Impact | Enforced System Mitigation |
| :--- | :--- | :--- | :--- |
| **Warehouse Dock Blockade** | Stock arrives physically, but the clerk delays entering the GRN. Sales cannot fulfill orders. | Frontline workers bypass the system or dispatch goods off-the-books. | **Fast-Track Dock Intake:** One-step barcode receipt mode that updates on-hand stock and posts GRNI instantly. |
| **Supplier Invoice Latency** | Goods arrive and sell in Month 1; supplier bill arrives in Month 2 with a 15% price surcharge. | Month 1 shows an artificially high gross margin; Month 2 absorbs an unexpected PPV expense spike. | **Managerial Margin Decoupling:** General Ledger reflects reality in Month 2; managerial analytics link the cost back to the original order. |
| **Break-Bulk Divisibility Errors** | Selling unpackaged items in irregular increments from bulk cases. | Fractional inventory balances and division-by-zero rounding errors. | **Rigid Integer Multiplier Invariant:** All stock is tracked at the atomic Base Unit (e.g., Piece). Pack units are strictly whole-number multiples. |
| **Quarantine Backlog** | Returned items sit in quarantine uninspected for weeks. | Physical space is blocked, customer awaits credit note, stock sits unvalued. | **Quarantine Aging SLA Alerts:** System flags items in quarantine older than 48 hours to the warehouse manager. |

---

### 4. The Ideal Final Result (IFR)

The system achieves financial and operational balance:
1. Operational clerks execute daily physical tasks using simple terminology without exposure to debit/credit mechanics.
2. Every document transaction automatically produces balanced double-entry General Ledger postings.
3. Inventory quantities never drop below zero, eliminating undefined costing calculations.
4. Historical financial statements remain permanently immutable because backdated valuation edits are strictly prevented.
5. True economic margins remain visible through managerial reporting without corrupting General Ledger integrity.

---

## Goals vs. Non-Goals

### In-Scope Goals (Day-1 MVP)
* **Document-Driven Double-Entry Ledger:** Automatic General Ledger postings triggered by operational documents (Sales Invoice, Supplier Bill, Goods Receipt, Payment Voucher, Credit Note, Stock Adjustment).
* **Perpetual Moving Weighted Average Costing:** Real-time inventory asset updates computed on non-negative integer stock quantities.
* **Hard-Stop Zero Negative Inventory:** System-level validation that blocks order dispatch and invoice generation if available stock is insufficient.
* **Strict Append-Only Posting:** Prohibiting backdated adjustments that alter past periods or past valuations; immutable closed periods.
* **Forward-Only Variance Accounting:** Direct expensing of supplier invoice price discrepancies and late freight charges to `5100 PPV` and `5200 Freight Expense`.
* **Two-Stage Customer Returns:** Separation of physical intake (RMA to Quarantine with inspection categorization) from financial resolution (Credit Note).
* **Customer Credit Balance Ledger:** Unapplied credit memos applied to future invoices, with an authorized bank disbursement option for account terminations.
* **Three-Way Matching:** Linking Purchase Orders, Goods Receipt Notes, and Supplier Bills using a dedicated `2100 GRNI` clearing account.
* **Rigid Integer Unit of Measure (UoM) Hierarchy:** Immutable Base Unit per SKU, with packaging units defined strictly as integer multipliers.
* **Single Central Warehouse:** All stock balances tracked within a single physical facility.
* **Single Currency:** All transactions, bank accounts, and ledgers maintained in a single local base currency.
* **Core Financial & Operational Deliverables:** Trial Balance, Income Statement (P&L), Balance Sheet, Cash Flow Summary, AR/AP Aging Schedules, Inventory Valuation Report, and Product Profitability Report.

### Out-of-Scope Non-Goals (Phase 2 Deferrals)
* **Manufacturing & Assembly:** No Bill of Materials (BOM), Work-in-Progress (WIP) accounting, or raw-material-to-finished-goods transformations.
* **Multi-Warehouse & Multi-Bin Routing:** No multi-facility transfers, in-transit inventory tracking, or bin-level warehouse path optimization.
* **Multi-Currency & Foreign Exchange:** No realized/unrealized foreign exchange gain/loss ledgers or foreign currency bank accounts.
* **Variable / Catch-Weight Conversions:** No floating-ratio packaging units (e.g., meat or produce weighed per carton).
* **Countertop Cash Refunds:** No direct cash drawer disbursements at order counters without formal AP authorization.
* **Retroactive Inventory Revaluation:** No post-hoc recalculation of past inventory layer costs or past COGS.
* **Automated Direct Bank Feeds:** No direct bank API polling or automated bank reconciliation algorithms.

---

## Alternatives Weighed

1. **Periodic Inventory Accounting (vs. Perpetual Inventory)**
   * *Status:* Rejected by author.
   * *Rationale:* Periodic inventory relies on physical period-end counts to compute COGS indirectly ($\text{Beginning} + \text{Purchases} - \text{Ending}$). This leaves business owners blind to real-time gross margins, hides intra-period stock shrinkage, and fails to provide sales reps with accurate stock availability.

2. **FIFO (First-In, First-Out) Cost Layering (vs. Moving Weighted Average)**
   * *Status:* Rejected by author.
   * *Rationale:* FIFO requires maintaining distinct cost queues (batches/lots) for every purchase receipt and consuming them sequentially during sales. For small-to-mid scale merchants distributing standard packaged goods, this adds significant data complexity and friction when handling returns without delivering meaningful accuracy gains over Moving Weighted Average.

3. **Permitting Negative Inventory with Provisional Costing**
   * *Status:* Rejected by author.
   * *Rationale:* Allowing stock counts to go negative creates mathematical anomalies (division by negative quantities or zero) and distorts inventory asset balances. Correcting these balances after the fact requires retroactive recalculations that alter previously reported financial statements.

4. **Retroactive Landed Cost Capitalization to Remaining Inventory**
   * *Status:* Rejected by author.
   * *Rationale:* If freight charges arrive after 60% of a shipment has been sold, allocating the entire freight charge to the remaining 40% of units artificially inflates their unit asset value. Alternatively, retroactively restating the COGS for already-sold units changes past Trial Balances. Forward-only expensing directly to `5200 Freight Expense` maintains financial integrity.

5. **Single-Stage Direct Restock Returns**
   * *Status:* Rejected by author.
   * *Rationale:* Automatically restocking returned customer goods into salable inventory upon issuing a credit note risks putting damaged or expired merchandise back into active stock, leading to duplicate customer complaints. A physical inspection stage in a quarantine location is mandatory.

6. **Day-1 Multi-Currency Support**
   * *Status:* Rejected by author.
   * *Rationale:* Handling foreign currency purchases requires tracking base vs. transaction currencies, recording unrealized exchange variances at period-end, and booking realized exchange gains/losses upon settlement. Small-to-mid merchants manage this adequately by recording overseas purchases in local currency using the landed bank conversion rate.

---

## Key Decisions & Staging for Next Steps

The following concrete decisions are established to guide formal technical specification in `/egm-draft`:

1. **Stock Invariant:** Hard-stop zero negative inventory enforced at document validation time. Invoices and Delivery Notes cannot post without sufficient on-hand stock.
2. **Costing Engine:** Moving Weighted Average forward-only calculation anchored strictly to positive base unit quantities.
3. **Ledger Posting Policy:** Append-only ledger posting. Dual-date tracking (`System Posting Timestamp` for General Ledger integrity; `Operational Event Date` for operational sequence).
4. **Variance Handling:** Supplier bill price differences and late freight charges post directly to `5100 PPV` and `5200 Freight Expense` in the current open period.
5. **Returns Architecture:** Two-stage workflow consisting of physical receipt into Quarantine (RMA) followed by inspection categorization and financial Credit Note generation.
6. **Unit of Measure:** Atomic integer Base Unit per SKU with rigid whole-number packaging multipliers.
7. **Document Lifecycle:** Three discrete states: `Draft` (editable, no ledger impact), `Posted` (immutable, automatic balanced ledger entries), and `Settled/Reversed` (linked offsetting document).
