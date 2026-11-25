# Workspace Rules: Merchandising ERP Accounting System

This workspace contains the source code, design specifications, and architecture for the **Merchandising Accounting ERP Application**, a desktop accounting system designed for small-to-mid scale merchandising businesses.

All agents, swarm workers, and subagents operating within this repository must adhere strictly to these engineering invariants and workflows.

---

## 1. Core Engineering Workflow: Karpathy Behavioral Invariants

The following four foundational principles govern all planning, code generation, refactoring, and review across every agent and subagent session:

**Tradeoff:** These guidelines bias toward caution over speed. For trivial tasks, use judgment.

### 1.1 Think Before Coding
- **Don't assume. Don't hide confusion. Surface tradeoffs.**
- Before implementing, state assumptions explicitly. If requirements or domain specifications are ambiguous, pause and ask rather than guessing.
- If multiple valid interpretations exist, present them clearly — never pick silently.
- If a simpler or existing architecture exists, say so. Push back against unnecessary complexity.
- If something is unclear, stop immediately. Name what is confusing and resolve the uncertainty.

### 1.2 Simplicity First
- **Minimum code that solves the problem. Nothing speculative.**
- No features, configuration knobs, or premature abstractions beyond what was specifically requested.
- No single-use wrappers or abstractions for one-off tasks.
- No "flexibility" or speculative hooks for theoretical future requirements.
- Never write error handling for impossible scenarios made unreachable by database constraints or type safety.
- Don't refactor things that aren't broken.
- If a 50-line solution solves the problem cleanly, rewrite any 200-line implementation.
- Ask: *"Would a senior engineer consider this overcomplicated?"* If yes, simplify.

### 1.3 Surgical Changes
- **Touch only what you must. Clean up only your own mess.**
- Do not "improve", restyle, reformat, or refactor adjacent, unrelated code. Match existing codebase conventions and style, even if you'd do it differently.
- If you notice unrelated dead code or bugs, document them — do not unilaterally delete or alter them.
- When changes create orphans (imports, variables, helpers), clean them up. Never delete pre-existing dead code unless explicitly requested.
- *The Golden Test:* Every changed line in the diff must trace directly to the user's specific request.

### 1.4 Goal-Driven Execution
- **Define success criteria. Loop until verified.**
- Convert every task into verifiable objectives before writing code:
  - *"Add validation"* $\rightarrow$ Write tests for invalid inputs, then make them pass.
  - *"Fix the bug"* $\rightarrow$ Write a test reproducing the failure, then make it pass.
  - *"Refactor X"* $\rightarrow$ Ensure tests pass before and after without behavioral regressions.
  - *"New feature"* $\rightarrow$ Define unit, integration, and UI acceptance criteria, implement, then verify green suite.
- For multi-step tasks, maintain an explicit step-by-step verification plan:
  ```
  1. [Step] -> verify: [check]
  2. [Step] -> verify: [check]
  3. [Step] -> verify: [check]
  ```
- Strong success criteria enable agents to loop independently. Weak criteria (*"make it work"*) require constant clarification.
- Loop independently until all verification criteria and hygiene invariants are satisfied.

---

## 2. Verification Gates & Workspace Governance

### 2.1 Mandatory Automated Verification Gates
Before signaling completion or ending any session, the following automated gates must be verified:
1. **Full Test Suite (`dotnet test`)**:
   - `Merchandising_Accounting_App.Tests` must execute with **100% passing tests** (zero test failures, zero regressions).
   - Domain engines (`LedgerPostingEngine`, `InventoryCostingEngine`) maintain $\ge 90\%$ branch coverage; application workflow services maintain $\ge 85\%$ statement coverage.
2. **Automated UI Previews (`preview_ui.ps1`)**:
   - Any modification or addition to UI controls, layouts, forms, or themes must execute:
     ```powershell
     powershell -File .\preview_ui.ps1 --all --mode both
     ```
   - Visual captures in `current_ui_screenshots/minimize/` (1280×720 restored) and `current_ui_screenshots/maximize/` (1920×1080) must be inspected for layout fidelity.
3. **Automated Workspace Hygiene (`WorkspaceHygieneTests.vb`)**:
   - Manifest completeness: all active `.vb` source files are cataloged in living specifications.
   - Documentation links: zero broken relative or file links across active documentation.
   - Root markdown rule: `GEMINI.md` is the sole authorized markdown document in the repository root.
   - Rule file size invariant: `GEMINI.md` file size must not exceed 24,000 bytes.

### 2.2 Mandatory Atomic Git Commit Protocol
1. **Zero Uncommitted Work Invariant**: AI agents must never conclude a turn, end a session, or report a task complete while leaving uncommitted source code, test files, preview images, or documentation in the working tree.
2. **Automated Commit Trigger**: Upon satisfying all verification gates (`dotnet test` 100% pass, `preview_ui.ps1` 100% pass, documentation synced, hygiene check clean), the agent MUST immediately stage all modified and untracked files and create a clean semantic commit (`feat: ...`, `fix: ...`, `docs: ...`, `refactor: ...`).
3. **Local Commit Safety**: Commits are created locally. Remote pushes (`git push`) remain strictly gated by explicit user instruction.

### 2.3 Workspace Boundary Rules & File Hygiene
1. **Output Destination Rule**: Never write new markdown documents, reports, or drafts to the workspace root directory. `GEMINI.md` is the only authorized markdown file in the workspace root. Direct temporary scratch files and conversational artifacts to conversation artifact directories, system architecture specifications to `docs/designs/`, and active execution plans to `docs/plans/`.
2. **Plan Archival Definition of Done**: When an execution or migration plan in `docs/plans/` is completed or merged, moving it to `docs/archive/plans/YYYY-MM-DD_<name>/` is strictly required. Active plans in `docs/plans/` must never retain a terminal status (`Completed` or `Merged`).
3. **Zero Archive Ingestion**: Coding agents must NEVER ingest or parse files inside `docs/archive/` (`docs/archive/designs/`, `docs/archive/reviews/`, `docs/archive/plans/`, or `docs/archive/baseline/`) during active development or bug fixes. These files represent superseded historical drafts.

### 2.4 Living Technical Specification Definition of Done (DoD)
1. **Authoritative Living Specifications**: [`docs/designs/mvp_erp_accounting.md`](docs/designs/mvp_erp_accounting.md) (Core ERP) and [`docs/designs/agri_erp_accounting.md`](docs/designs/agri_erp_accounting.md) (Agricultural Extension) are living technical specifications that evolve with the codebase.
2. **Mandatory Documentation Gate**: Every session modifying code must satisfy the following:
   - [ ] Synchronize `docs/designs/mvp_erp_accounting.md` and/or `docs/designs/agri_erp_accounting.md` (architecture, schema, Target File Manifest).
   - [ ] Synchronize `docs/README.md` test metrics and active indices.
   - [ ] Execute `dotnet test` ensuring `WorkspaceHygieneTests` passes all manifest and parity invariants.
   - [ ] Archive completed execution plans to `docs/archive/plans/YYYY-MM-DD_<name>/`.
   - [ ] Ensure `git status` has zero untracked files and working tree is clean.

---

## 3. Technology Stack & Compiler Safety Invariants

### 3.1 Technology Stack & Topology
- **Target Framework:** .NET 10.0 Windows Forms (`net10.0-windows`).
- **Language & Runtime:** Visual Basic .NET (`MyType: WindowsForms`).
- **Solution Model:** Microsoft Visual Studio 2026 XML solution (`Merchandising_Accounting_App.slnx`) at repository root.
- **ORM & Database:** Entity Framework Core 10 (`Microsoft.EntityFrameworkCore.Sqlite`) targeting embedded local SQLite (`merchandising_erp.db`) in WAL mode.
- **Deployment Scope:** Single-workstation standalone desktop application running on local disk storage. Multi-station distributed networking, shared SMB network paths, and external client-server databases are explicit non-goals for MVP.

### 3.2 Mandatory Compiler & Type Safety Invariants
- **Compiler Flags:**
  - `<OptionStrict>On</OptionStrict>` (Mandatory: late binding, implicit narrowing conversions, and loose typing are strictly prohibited).
  - `<OptionExplicit>On</OptionExplicit>` (Mandatory: all variables must be explicitly declared).
  - `<OptionCompare>Binary</OptionCompare>` (Mandatory: case-sensitive ordinal string comparisons).
  - `<TreatWarningsAsErrors>true</TreatWarningsAsErrors>` (Mandatory: all compiler warnings must be resolved; zero-warning builds required).
- **Monetary Representation:**
  - Every financial figure, unit cost, price, currency balance, and journal line debit/credit **must strictly utilize the 128-bit `Decimal` type**.
  - Never use floating-point types (`Double`, `Single`, `Float`) for monetary calculations.
- **Physical Stock Representation:**
  - Every stock count, on-hand quantity, carton multiplier, and unit conversion **must strictly utilize `Integer`**. Fractional or floating-point stock counts are strictly forbidden.
- **Rounding Discipline:**
  - Journal balancing enforces equality to 4 decimal places: `Math.Round(debits, 4) == Math.Round(credits, 4)`.
  - Fractional penny rounding differences generated during unit conversions route to Account `5500 Cash Rounding & Roundoff Variance` if $|variance| \le 0.02$.

---

## 4. Authoritative Architecture, Living Specifications & Domain Topology

Detailed business logic, ledger mechanics, and UI layout rules are maintained in authoritative living specifications to eliminate bloat from agent workflows. Coding agents must adhere strictly to the following authority topology:

| Task / Domain Area | Authoritative Source | Key Rules & Enforcements | Prohibited / Out-of-Scope Reading |
| :--- | :--- | :--- | :--- |
| **Workspace Workflow & Standards** | `GEMINI.md` | Karpathy invariants (§1), verification gates & git commit protocol (§2), compiler invariants (§3). | Any file in `docs/archive/` |
| **Core ERP Accounting & GL Rules** | [`docs/designs/mvp_erp_accounting.md`](docs/designs/mvp_erp_accounting.md) | Double-entry append-only ledger, perpetual moving-average costing, zero negative stock (`CHECK (TotalQtyOnHand >= 0)`), 3-way matching (GRNI clearing, PPV `5100`), 2-stage returns quarantine, immutable closed fiscal periods, and 25-table core schema. | `docs/archive/designs/*`, `docs/archive/reviews/*` |
| **Agricultural Extension Operations** | [`docs/designs/agri_erp_accounting.md`](docs/designs/agri_erp_accounting.md) | Country grain elevator intake, rapid weighbridge scale ticketing, two-stage provisional intake, commingled silo storage, drying service revenue, and bilateral contra-clearing. | `docs/archive/designs/*`, `docs/archive/reviews/*` |
| **Desktop UI & Anti-Slop System** | [`docs/UI_PREVIEW.md`](docs/UI_PREVIEW.md) & [`.agents/skills/winforms-ui-preview/SKILL.md`](.agents/skills/winforms-ui-preview/SKILL.md) | Anti-slop invariants: 100% viewport filling, zero horizontal scrollbars at 1280×720 (content width 1000px), 24–28px row heights, single-line columns, Segoe UI typography, decoupled UserControls (`.vb`, `.Designer.vb`, `.resx`). | Historical audit reports |
| **Master Documentation Portal** | [`docs/README.md`](docs/README.md) | System documentation index, architecture mapping, and test metrics parity. | Historical drafts |

### 4.1 Strict Authority Hierarchy
- Any ambiguity in core merchandising data models, GL posting engines, moving average costing, or base schema resolves to [`docs/designs/mvp_erp_accounting.md`](docs/designs/mvp_erp_accounting.md).
- Any ambiguity in grain scale ticketing, agricultural contracts, silo bin lot tracking, drying revenue, or bilateral grower contra clearing resolves to [`docs/designs/agri_erp_accounting.md`](docs/designs/agri_erp_accounting.md).
- If code comments or external descriptions conflict with the authoritative living specification, the specification prevails.

### 4.2 Architectural Layering & UI Standards Reference
1. **Four-Layer Architecture:** Code is strictly partitioned into `[UI / Presentation]`, `[Application Services]`, `[Domain Layer]`, and `[Persistence Layer]`. Forms and UserControls remain thin presentation wrappers delegating to services and pure domain engines.
2. **Windows Forms File Discipline:** Every Form and UserControl maintains companion files: `.vb` (events & logic), `.Designer.vb` (layout), and `.resx` (resources).
3. **Desktop Anti-Slop & UI Previews:** Follow guidelines in [`docs/UI_PREVIEW.md`](docs/UI_PREVIEW.md) and [`.agents/skills/winforms-ui-preview/SKILL.md`](.agents/skills/winforms-ui-preview/SKILL.md). All views must render cleanly at restored desktop resolution (1280×720, content host width: 1000px) with zero horizontal scrollbars on primary data tables.
