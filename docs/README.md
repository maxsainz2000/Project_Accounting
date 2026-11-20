# Merchandising ERP Accounting Application — Documentation Portal

Welcome to the master documentation portal for the **Merchandising ERP Accounting Application**, an audit-certified desktop accounting system built on .NET 10 Windows Forms (`net10.0-windows`), Visual Basic .NET, and SQLite.

This portal provides an authoritative map of project specifications, architectural guides, operational runbooks, and historical archives.

---

## 1. Active Documentation Index

The table below outlines the primary active documentation governing the project. All agents and developers must consult these living documents:

| Document | Primary Scope | Audience / Usage |
| :--- | :--- | :--- |
| [**`GEMINI.md`**](../GEMINI.md) | **Workspace Rules & Standards**<br>Mandatory compiler flags (`Option Strict On`), monetary (`Decimal`) and physical (`Integer`) invariants, four-layer architecture, UI responsiveness, and preview invariants. | All agents and human contributors before making any modifications. |
| [**`docs/designs/mvp_erp_accounting.md`**](designs/mvp_erp_accounting.md) | **Authoritative Technical Specification**<br>Single source of truth for double-entry GL rules, perpetual moving weighted average costing, dual-timestamp audit trails, zero-negative-stock guards, schema models, and service contracts. | Domain engine development, accounting workflow verification, schema migrations. |
| [**`docs/designs/SITEMAP.md`**](designs/SITEMAP.md) | **Design Documents Sitemap**<br>Mandatory integration registry required for automated EGM specification discovery and tracking. | Tooling, specification indexers, plugin auto-registration. |
| [**`docs/UI_PREVIEW.md`**](UI_PREVIEW.md) | **Automated UI Preview Architecture**<br>In-depth architectural design and research on offscreen STA rendering, offscreen GDI+ `Control.DrawToBitmap` mechanics, resolution standards, and CoreCLR headless execution. | UI engineering, automated screenshot tooling architecture. |
| [**`.agents/skills/winforms-ui-preview/SKILL.md`**](../.agents/skills/winforms-ui-preview/SKILL.md) | **UI Preview Operational Runbook**<br>CLI execution guide (`preview_ui.ps1`), alias mappings, dual-resolution capture procedures (1280×720 and 1920×1080), and visual validation checklist. | Agents performing visual inspection and UI layout verification. |

---

## 2. Historical & Superseded Archive (`docs/archive/`)

Historical drafts, pre-audit specifications, swarm execution plans, and audit reviews have been moved into the `docs/archive/` tree to preserve project lineage while preventing context bloat for coding agents.

> [!WARNING]
> **Archive Policy**: The files in `docs/archive/` are frozen historical records. **Do not ingest or consult archive files during active feature development or bug fixes.** All domain logic, schemas, and accounting behaviors must derive exclusively from the authoritative specification [`docs/designs/mvp_erp_accounting.md`](designs/mvp_erp_accounting.md).

### Archive Topology

```
docs/archive/
├── baseline/
│   └── initial_architecture.md                # Project initialization baseline & SDK configuration (2026-11-16)
├── designs/
│   └── 2026-11-16_pre_audit/
│       ├── concepts_brief.md                  # Pre-audit concepts brief & contradiction analysis
│       └── design_draft.md                    # Initial pre-audit system design draft
├── plans/
│   └── 2026-11-19_functional_core_swarm/
│       └── functional_core_transition_swarm_plan.md  # Multi-agent swarm implementation plan
└── reviews/
    └── 2026-11-16_egm_audit/
        ├── zoom_out_report.md                 # Stage 5 initial zoom-out audit report
        ├── comprehension_report.md            # Stage 6 specification comprehension audit report
        ├── critic_report.md                   # Stage 7 structural and domain critic review
        ├── intermediate_zoom_out_report.md    # Stage 8 intermediate consistency zoom-out report
        ├── implementation_readiness_report.md # Stage 9 code readiness & edge-case audit report
        └── consolidated_review_report.md      # Consolidated pre-implementation audit sign-off
```

---

## 3. Context Navigation Matrix & Agent Boundary Rules

To ensure rapid context acquisition and prevent context pollution:

| Task / Domain Area | Mandatory Reading | Prohibited / Out-of-Scope Reading |
| :--- | :--- | :--- |
| **Compiler & Project Rules** | [`GEMINI.md`](../GEMINI.md) | Historical drafts in `docs/archive/` |
| **Business Logic, GL, Costing, Schemas** | [`docs/designs/mvp_erp_accounting.md`](designs/mvp_erp_accounting.md) | [`docs/archive/designs/2026-11-16_pre_audit/*`](archive/designs/2026-11-16_pre_audit/) |
| **UI Form & UserControl Development** | [`GEMINI.md`](../GEMINI.md) §6, [`.agents/skills/winforms-ui-preview/SKILL.md`](../.agents/skills/winforms-ui-preview/SKILL.md) | Archive files |
| **UI Preview Tooling & Offscreen Engine** | [`docs/UI_PREVIEW.md`](UI_PREVIEW.md) | Pre-audit reports |
| **Test Verification & Quality Gates** | [`GEMINI.md`](../GEMINI.md) §3 & §5 | Pre-audit reports |

### Strict Boundary Invariants for Agents
1. **Never load `docs/archive/` for active tasks**: Archive files are historical snapshots. Reading them wastes token context and risks importing obsolete assumptions that were superseded during the audit.
2. **Single Source of Truth**: When any question arises regarding debit/credit distributions, inventory valuation, or API contracts, [`docs/designs/mvp_erp_accounting.md`](designs/mvp_erp_accounting.md) is the final authority.
3. **Always run verification**: Any modification to domain engines, services, or UI components must be verified using the automated test suite (`dotnet test`) and automated visual preview engine (`powershell -File .\preview_ui.ps1 --all --mode both`).

---

## 4. Quickstart Verification Commands

```powershell
# Run all unit and integration test fixtures (159 tests)
dotnet test

# Render offscreen UI preview screenshots (1280x720 and 1920x1080)
powershell -File .\preview_ui.ps1 --all --mode both

# Verify UI preview screenshots for a single dashboard form
powershell -File .\preview_ui.ps1 --form "General Ledger"
```
