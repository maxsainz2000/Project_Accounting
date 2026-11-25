# Proposal & Implementation Plan: Visual Perception, Design Taste Heuristics & Strategic Red Team Reconciliation for `@plugin:anti-slop-desktop-design`

**Status:** Approved — Ready for Implementation (Architectural Review & Open Decisions Resolved)

---

## [Goal Description]

This plan resolves architectural misalignments verified across `@plugin:anti-slop-desktop-design`, `agri_erp_accounting_ui_prototype`, and `Project_Accounting` during live multi-view Chrome DevTools Protocol (CDP) audits, automated harness runs, and WinForms offscreen screenshot reviews: **`@web-visual-auditor` currently has zero visual perception, the 10 external design engineering resources are largely unembedded and unfiltered, `@audit-verifier` lacks semantic mismatch rules and safe feedback scoping, and the evaluation harness / WinForms parity checks allow critical layout failures to score 96–100/100 PASS.**

### Verified Core Problem Statement

1. **The Illusion of "Visual" Auditing (Worse Than Documented):**
   - [web-visual-auditor.md](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/agents/web-visual-auditor.md#L195-L206) and [skills/web-visual-auditor/SKILL.md](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/skills/web-visual-auditor/SKILL.md#L168-L181) instruct `@web-visual-auditor` to navigate via `/browser` and call `window.__auditAntiSlopDOM()`. In reality, `@web-visual-auditor` only has `tools: [read_file, run_command, search_web]`, `chrome-devtools-plugin` is `"enabled": false` in [config.json](file:///C:/Users/maxsa/.gemini/config/config.json#L11-L18), and [audit_dom.js](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/skills/web-visual-auditor/scripts/audit_dom.js#L9) is an anonymous IIFE rather than a `window` method.
   - Worse, running [eval_harness.py](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/eval/eval_harness.py#L595-L710) with `--visual` (without `--visual-report`) never touches a browser—it runs duplicated static `.tsx` regexes and awards free `15/15` scores for `webKeyboard` and `webArchitecture`, while [evaluate_design.ps1](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/eval/evaluate_design.ps1#L216-L228) is a `Write-Host` stub for web targets.
   - Live CDP + PNG inspection across all 6 views and 5 interactive states proved `audit_dom.js` misses inner `.overflow-x-auto` horizontal scrollbars (`GrowerSettlementView` `577px > 549px` at `720p`), below-fold registers and primary CTAs (`WeighbridgeIntakeView` Scale Ticket Register pushed below even the `1080p` fold at `top: 1072px` / `1238px`), clipped `<input>`/`<select>` elements, `68.5px` squeezed headers (`AgronomicSalesDispatchView`), unclicked `role="dialog"` modals (`SiloInventoryView`), dead `A1` contrast logic, and wrong `A3` threshold (`< 4.5` instead of `< 5.6`, missing `#B45309` at `4.51–5.02:1`), while emitting false positives (`A2` on `aria-hidden="true"` `·` separators; `D3` on `[class*="grid-cols-3"]`).

2. **Partial, Unfiltered Design Engineering Resources & Contrast Math Drift:**
   - Only fragments of *Anti UI Slop* (4-point Design Contract, 4-state table model) exist in [skills/anti-slop-desktop-design/SKILL.md](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/skills/anti-slop-desktop-design/SKILL.md#L14-L27), and none of the 10 external resources (*Anti UI Slop*, *Impeccable*, *Taste Skill*, *Emil Kowalski*, *Vercel Web Design Guidelines*, *Frontend Design*, *Hallmark*, *Make Interfaces Feel Better*, *UI UX Pro Max*, *Stageshow*) are integrated into [rubric.json](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/eval/rubric.json) or agent checklists with an enterprise desktop **Domain Precedence Filter** to reject conflicting consumer web tropes.
   - Documentation contrast numbers also drifted from exact WCAG relative luminance (`#17201B` on white is `16.68:1`, `#37433D` is `10.33:1`, `#526059` is `6.61:1`, `#143A2B` is `12.58:1`, `#92400E` is `7.09:1` on white / `6.37:1` on `#FEF3C7`, `#B45309` is `5.02:1` on white / `4.51:1` on `#FEF3C7`).

3. **Unsafe Red Team Reconciliation & WinForms Verification Parity Gaps:**
   - [audit-verifier.md](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/agents/audit-verifier.md#L45-L55) treats every Code vs. DOM contradiction as `--source code --outcome false_positive`. Meanwhile, [findings_feedback.jsonl](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/eval/findings_feedback.jsonl#L1) already holds 1 targetless `S1` entry, and `eval_harness.py` ignores `"source"` when tallying feedback—one more targetless `S1` false positive disables `S1` globally across web and WinForms.
   - `--visual-report` parsing in [eval_harness.py](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/eval/eval_harness.py#L444-L468) ignores `agent-json`, leaves `visualDimensionScores` empty, and fails to detect `[Severity: Blocker]` lines in `blockingDefects`.
   - On WinForms, `eval_harness.py --visual` scores `ScaleTicketForm.vb` **`96/100 PASS`** and `ScaleTicketForm.Designer.vb` **`100/100 PASS`** despite [ScaleTicketForm.png](file:///c:/Users/maxsa/Documents/Project_Accounting/current_ui_screenshots/minimize/ScaleTicketForm.png) showing a visible `720p` horizontal scrollbar, `pnlCalculated` inverted above `pnlInputs`, clipped labels, `17` child `.Font` overrides, `148px` stacked headers, and missing `TabIndex`/`&` mnemonics on inputs/buttons, while Phase 5 bypasses `@audit-verifier`.

### What This Plan Accomplishes

This plan upgrades Phases 2–5 across `@plugin:anti-slop-desktop-design` and `Project_Accounting` into complementary, non-overlapping subsystems:
- **`@slop-auditor` (Component-Closure Code & Token Audit):** Audits the full import closure (`App.tsx` shell, `index.html`, `index.css`, shared components, target view `.tsx`, or `.vb`+`.Designer.vb` pairs) for token discipline, keyboard handlers, type safety, state completeness, and latent layout traps.
- **`@web-visual-auditor` (Multi-State Headless CDP Capture + Multimodal Vision + Repaired DOM Telemetry):** Launches headless Chrome (`--remote-debugging-port=0`), traverses a State Manifest (views, `[role="tab"]`, `[role="dialog"]`), captures `1280×720`, `1920×1080`, and scrolled table bands into an OS `%TEMP%` directory (`tempfile.mkdtemp()`, deleted in `finally`), inspects PNGs via `read_file` vision against `skills/design-taste/SKILL.md`, and merges DOM measurements from the repaired `audit_dom.js`.
- **`@audit-verifier` (5-Category Mismatch Resolution Engine & Gate-Separated Scoring):** Applies the corrected 5-Category Matrix (fixing `Code Illusion`, adding `Scope / Viewport / State Divergence`, preventing Floor feedback poisoning), separates non-waivable **Floor Blockers** (`>= 85` Gate) from the **Craft Quality Grade (`A–D`)**, and outputs a 3-part Visual Evidence Tuple inside `ACTION_ENVELOPE.xml`.
- **WinForms & Harness Parity:** Synchronizes `eval/rubric.json`, `eval/eval_harness.py`, `eval/evaluate_design.ps1`, `Merchandising_Accounting_App.PreviewTool`, `FormInstantiationTests.vb`, and `WorkspaceHygieneTests.vb`.

---

## Architecture: Target State Machine

```text
                                  Single Command: /anti-slop <view_name>
                                                     │
                                                     ▼
                               ┌───────────────────────────────────────────┐
                               │  Phase 1: Target Ingestion & Scoping      │
                               │  • Resolve TSX import closure + CSS/App   │
                               │  • Build State Manifest (tabs & dialogs)  │
                               └─────────────────────┬─────────────────────┘
                                                     │
                                                     ▼
                               ┌───────────────────────────────────────────┐
                               │  Phase 2: DIVERGENT PARALLEL DUAL AUDIT   │
                               └──────────────┬────────────────────────────┘
                                              │
                     ┌────────────────────────┴────────────────────────┐
                     ▼                                                 ▼
     ┌───────────────────────────────┐                 ┌───────────────────────────────┐
     │  @slop-auditor                │                 │  @web-visual-auditor          │
     │  [Code Closure & Tokens]      │                 │  [CDP Capture + Vision + DOM] │
     ├───────────────────────────────┤                 ├───────────────────────────────┤
     │ • Full import-closure AST     │                 │ • cdp_audit.py (port=0, TEMP) │
     │ • 4/5-state & key handler chk │                 │ • 720p, 1080p, scroll & tabs  │
     │ • Latent responsive traps     │                 │ • Vision critique (read_file) │
     │ • Surface-type classification │                 │ • Repaired audit_dom.js       │
     │ • Zero screenshot access      │                 │ • Zero image leak + cleanup   │
     └───────────────┬───────────────┘                 └───────────────┬───────────────┘
                     │                                                 │
                     │ (Code Audit Manifest)                           │ (Visual + DOM + Craft)
                     └────────────────────────┬────────────────────────┘
                                              │
                                              ▼
                               ┌───────────────────────────────────────────┐
                               │  Phase 3: RED TEAM CROSS-VERIFICATION     │
                               │  (@audit-verifier)                        │
                               ├───────────────────────────────────────────┤
                               │ • 5-Category Mismatch Matrix              │
                               │ • Gate-Separated Floor (>=85) vs Craft    │
                               │ • Source/Target-scoped feedback logging   │
                               │ • 3-Part Visual Evidence in Envelope      │
                               └─────────────────────┬─────────────────────┘
                                                     │
                                                     ▼
                               ┌───────────────────────────────────────────┐
                               │  Phase 4 -> 5 -> 6 (Transpile, Telemetry, │
                               │  @winforms-visual-auditor -> Verifier,    │
                               │  STA Layout Tests, Spec Sync & Commit)    │
                               └───────────────────────────────────────────┘
```

---

## Resolved Architectural Decisions

1. **Defect Weighting (`Option B — Gate-Separated + Non-Waivable "Floor"`):**
   - **Pass/Fail Gate (`Composite Score >= 85` AND `0 Floor Blockers`):** Driven strictly by non-waivable **Floor** invariants: `100%` client width, zero `720p` outer/inner horizontal scrollbars, `24–28px` operational grid row heights (exempting `Prose Reference Matrix` surfaces), single-line atomic columns, zero clipped inputs/`<select>`/headers/cells, primary CTAs and key rows reachable cleanly, WCAG contrast (`>= 7.0:1` primary, `>= 4.5:1` secondary, `>= 5.6:1` financial variances), `:focus-visible` focus rings, `Esc`/Enter hooks, and non-blocking primary workflows. Floor rules **cannot** be waived in `findings_feedback.jsonl`.
   - **Qualitative Craft Quality (`Craft Grade A–D`):** Subjective craft/taste heuristics (*Impeccable* optical spacing, tinted neutrals, concentric radius harmony, domain-specific CTA copy, monospace chip accents) emit a separate **Craft Grade (`A–D`)** and prioritized **Craft Majors** into `ACTION_ENVELOPE.xml` so AI Studio refines polish without deadlocking WinForms compilation.

2. **Visual Evidence in `ACTION_ENVELOPE.xml` (3-Part Structured Tuple + Native Tailwind v4 Tokens):**
   - Every visual defect emits:
     1. **Source + Selector:** `<file:line> | <DOM selector>`
     2. **Viewport + Measured Proof:** `[1280×720 / 1920×1080, Box: x,y,w,h]` + exact measurement (`scrollWidth 577 > clientWidth 549`, `Commit button top=743 > 720`, `textW 310 > avail 165`)
     3. **Prototype-Native Fix:** Concrete Tailwind v4 classes matching [src/index.css](file:///c:/Users/maxsa/Documents/agri_erp_accounting_ui_prototype/src/index.css) and [skills/anti-slop-desktop-design/SKILL.md](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/skills/anti-slop-desktop-design/SKILL.md) (`text-[#17201B]`, `text-[#37433D]`, `text-[#92400E]`, `border-[#D2D8D5]`, `whitespace-nowrap`, `min-w-0`, `focus-visible:ring-2`).
   - Alien shadcn tokens (`text-muted-foreground`, `border-border/40`) and `uppercase tracking-wider` on dense table headers are **prohibited**.

3. **Storage of Design Principles (`skills/design-taste/SKILL.md` + Lean Floor in `rules/AGENTS.md`):**
   - Per [plugin/SKILL.md](file:///C:/Users/maxsa/.gemini/antigravity/builtin/skills/plugin/SKILL.md#L205-L211) and [agy-customizations/SKILL.md](file:///C:/Users/maxsa/.gemini/antigravity/builtin/skills/agy-customizations/SKILL.md#L85-L92), Antigravity plugins auto-load **`rules/AGENTS.md`** globally on every turn (including backend SQLite/EF Core work) and discard unknown top-level `plugin.json` fields, while skills use progressive disclosure.
   - Keep [rules/AGENTS.md](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/rules/AGENTS.md) lean as the global non-waivable Floor; package the 10-resource playbook, Domain Precedence Filter, and surface-type rubrics as `skills/design-taste/SKILL.md` (with `references/` if split), and instruct `@slop-auditor`, `@web-visual-auditor`, `@winforms-visual-auditor`, and `@audit-verifier` to load it via `read_file`.

4. **Modal vs. Split-Panel Boundary (`W4` / `WW4` — Scoped Confirmation/Diagnostic Exemption):**
   - **Primary Operational Workflows (Hard Blocker):** Weighbridge intake, settlements, purchasing 3-way match, and sales dispatch MUST NEVER use blocking modal overlays; they must use docked split panels or collapsible bottom drawers per [rules/AGENTS.md](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/rules/AGENTS.md).
   - **Secondary Confirmations & Read-Only Drilldowns (Scoped Exemption with 4 Guardrails):** Explicit irreversible confirmation dialogs (`FiscalPeriodForm`, `DisburseVendorPaymentDialog`) and secondary diagnostic overlays (`Clean-Out Reconcile`, `Trace Origin`—though docked split/bottom panes remain preferred during WinForms transpilation of `SiloInventoryControl`) do **not** fail `W4`/`WW4` solely for existing, **provided all 4 Floor guardrails pass**: (1) `Esc` dismisses and restores focus, (2) WAI-ARIA focus trap (web) or `AcceptButton`/`CancelButton` (WinForms), (3) zero clipped inputs or mid-token wrapping at `1280×720` (`SiloInventoryView` modals currently fail this), and (4) domain-specific primary CTA verbs (no generic `"Done"`). When `[role="dialog"]` is open, `audit_dom.js` scopes checks to the active dialog rather than background grids.

5. **`MainShellForm` Header Height Lock & Double-Header Elimination (2-Stage Resolution):**
   - **Stage 1 — Immediate Approach B `UserControl` Rule:** Transpiled `UserControl`s hosted inside `MainShellForm.pnlContentHost` MUST **never** create their own `pnlHeader` title/subtitle banner (eliminating the `148px` double header seen in [ScaleTicketForm.png](file:///c:/Users/maxsa/Documents/Project_Accounting/current_ui_screenshots/minimize/ScaleTicketForm.png) and recovering `60px` immediately). `MainShellForm.LoadContent(control, title, subtitle)` owns module title/subtitle rendering; child `UserControl`s start directly with at most a `32px` pinned summary/toolbar strip.
   - **Stage 2 — Dedicated Shell-Compaction Prerequisite PR:** [MainShellForm.vb](file:///c:/Users/maxsa/Documents/Project_Accounting/Merchandising_Accounting_App/UI/MainShellForm.vb#L24) and [BrandingAndBrandingStringsTests.vb](file:///c:/Users/maxsa/Documents/Project_Accounting/Merchandising_Accounting_App.Tests/BrandingAndBrandingStringsTests.vb#L81) currently lock `pnlHeader.Height >= 88` and `btn*.Height >= 52`, shrinking `pnlContentHost` to `606px` height at `1280×720`. Leave `BrandingAndBrandingStringsTests.vb` untouched during Phase 2–3 web prototype audits, then execute a dedicated shell-compaction commit prior to Phase 4/5 transpilation of dense agricultural views—compacting `pnlHeader` (`48px` or unified `32px` bar hosting `FiscalPeriodChipControl`) and sidebar buttons (`40px`), and intentionally updating `BrandingAndBrandingStringsTests.vb` in that same atomic commit.

---

## Proposed Changes

### Component 1: Centralized Design Taste Skill & Contrast Reconciliation

#### [NEW] `~/.gemini/config/plugins/anti-slop-desktop-design/skills/design-taste/SKILL.md`
Synthesize the 10 external design engineering resources through an explicit **Domain Precedence Filter** and **Surface Classification Matrix**:

| Resource | Keep / Adapt for Industrial Desktop ERP | Exclude / Override Conflict |
| :--- | :--- | :--- |
| **1. Anti UI Slop (`uizze.com`)** | Pre-ship Finish Gate, 4-point Design Contract, 5-state coverage (`loading`/`empty`/`error`/`success`/`disabled`). | Consumer marketing hero patterns. |
| **2. Impeccable (`pbakaus/impeccable`)** | **"Operate & Read" mode**: tight `1.125–1.2` type scale, tinted neutrals on `#F5F9F7`/`#EEF4F1` surfaces, heading spacing asymmetry, ban card-in-card nesting, `65–75ch` prose measure. | Display font pairings (violates `W2` `Segoe UI`), `clamp()` fluid sizing. |
| **3. Emil Kowalski (`emilkowalski/skills`)** | `/break-ui` stress testing (long grower/contract names, overflow checks, `min-w-0`), ban `transition-all`, ban `ease-in` overlays, `prefers-reduced-motion: reduce`. | `200–300ms` spring choreography on grids/tabs (`P2` requires `<= 16ms` / instant snap). |
| **4. Jakub Krehel (`make-interfaces-feel-better`)** | Concentric border radii ($R_{\text{outer}} = R_{\text{inner}} + \text{padding}$), optical vs. geometric label alignment, `text-wrap: balance`/`pretty`, `>= 40px` `::after` hit-areas on dense close buttons. | `44px` row heights, consumer spring `scale(0.96)` press feedback. |
| **5. Vercel (`web-design-guidelines`)** | Require `focus-visible:ring-*` whenever `outline-none` is used, WAI-ARIA dialog focus trap + `Esc` + focus return, URL-reflected tab/filter state, sticky headers. | Loose consumer padding. |
| **6. Hallmark (`Nutlope/hallmark`)** | Immutable non-waivable **Floor** vs. **Craft** tiering, zero hardcoded hexes outside `@theme`/tokens, `overflow-x: clip`, ban tracked uppercase table headers & middot metadata clutter, zero dead/competing primary buttons. | Randomized Theme Dealer palettes (palette locked to `#143A2B` / `#17201B` / `#F4F6F5`). |
| **7. Taste (`Leonxlnx/taste-skill`)** | Enterprise dial calibration (`DESIGN_VARIANCE: 1–2`, `MOTION_INTENSITY: 1`, `VISUAL_DENSITY: 9–10`), single primary accent `#143A2B`. | Editorial layouts, photography grids, high whitespace. |
| **8. UI UX Pro Max (`nextlevelbuilder`)** | Accessibility #1 priority hierarchy, **`>= 12px` minimum body/table text floor**, field-adjacent inline errors, sticky headers. | Multi-theme consumer SaaS styling. |
| **9. Frontend Design (`anthropics/skills`)** | Compact semantic color-role plan (4–6 named roles), domain-specific CTA verbs (ban generic `"Done"`). | Expressive Google Fonts, artistic asymmetry. |
| **10. Stageshow (`benjitaylor/skills`)** | Host-budget split column sizing inside the `720p` `1000px` content host (`min-w-0` budgets preventing `~456px` crushed side grids). | Bento grids, faux marketing mockups, animated cursors. |

Key rules encoded in `skills/design-taste/SKILL.md`:
- **Surface Classification:**
  1. *Operational Register* (`24px` rows, `26px` headers, `>= 12px` Sentence/Title Case untracked headers, `font-mono tabular-nums text-right`)
  2. *Master Document List* (`28px` rows, `28px` headers)
  3. *Form / Split Workspace* (pinned `32px` summary strip, `min-w-0` host budget, single primary `#143A2B` CTA + secondary outline/ghost actions)
  4. *Prose Reference Matrix* (e.g., `MVP -> Agri Spec Transition Map`: audited for `65–75ch` measure, `>= 7.0:1` AAA contrast, `text-wrap: pretty`, and monospace identifier chips rather than `24px` single-line row heights)
- **Exact Prototype-Native Tokens (Tailwind v4):** `text-[#17201B]` (`16.68:1`), `text-[#37433D]` (`10.33:1`), `text-[#526059]` (`6.61:1` muted non-critical only), `text-[#92400E]` (`7.09:1` on white / `6.37:1` on `#FEF3C7`), `text-[#166534]` (`7.13:1`), `text-[#991B1B]` (`8.31:1`), `border-[#D2D8D5]`.

#### [MODIFY] `~/.gemini/config/plugins/anti-slop-desktop-design/skills/anti-slop-desktop-design/SKILL.md`
- Reconcile Section 3 contrast table ratios to exact WCAG relative luminance values (`16.68:1`, `10.33:1`, `6.61:1`, `12.58:1`, `7.09:1` / `6.37:1`, `7.13:1`, `8.31:1`), align concentric radius rule with $R_{\text{outer}} = R_{\text{inner}} + \text{padding}$, and cross-link `skills/design-taste/SKILL.md`.

---

### Component 2: Standalone CDP Multi-State Runner, Repaired `audit_dom.js` & `@web-visual-auditor`

#### [NEW] `~/.gemini/config/plugins/anti-slop-desktop-design/skills/web-visual-auditor/scripts/cdp_audit.py`
- Deterministic Python CDP runner using local Chrome (`--headless=new --remote-debugging-port=0`) + `websockets`:
  - Accepts `--url`, optional `--state-manifest` (JSON specifying sidebar buttons, `[role="tab"]` selectors, and `[role="dialog"]` triggers) or auto-discovers top-level nav + `[role="tab"]` states.
  - Writes PNGs exclusively to `tempfile.mkdtemp()` in OS `%TEMP%` (never inside repo workspace, avoiding `brain/<subagent-id>` path assumptions and guaranteeing clean `git status`), and cleans up the temp directory in `finally` after `@web-visual-auditor` completes `read_file` inspection.
  - Captures `1280×720` above-the-fold, `1920×1080` above-the-fold, scrolled lower bands (revealing below-fold registers/footers), and each interactive tab/modal state, while executing `audit_dom.js` at **both** `1280×720` and `1920×1080`.

#### [MODIFY] `~/.gemini/config/plugins/anti-slop-desktop-design/skills/web-visual-auditor/scripts/audit_dom.js`
- Fix all verified blind spots and false positives:
  - Implement `A1` (`< 7.0:1` primary/table body text check).
  - Skip `aria-hidden="true"` decorative glyphs in `A2` (eliminating false `-4` penalties on middots).
  - Fix `A3` amber contrast threshold from `< 4.5` to `< 5.6` per `rubric.json`.
  - Detect inner `.overflow-x-auto` table scrollbars (`el.scrollWidth > el.clientWidth + 1` at `720p`), `thead tr` wrapping/height (`> 30px`), `<input>`/`<select>` value truncation (`scrollWidth > clientWidth`), missing `:focus-visible` ring styles alongside `outline: none`, and below-fold primary submit buttons / registers at `720p`.
  - Scope DOM scan to active `[role="dialog"]` when a modal is open (preventing background grids from triggering `D3`), exempt Prose Reference Matrices from `24–28px` `S1`, refine `D3` selector so 3-col input rows and silo grids don't misfire, detect `LedgerAndTransitionView`'s `<article>` journal deck under `D4` even when nested `<table>` tags exist, and align rule IDs to `WP3`, `WW1–WW4`.

#### [MODIFY] `~/.gemini/config/plugins/anti-slop-desktop-design/agents/web-visual-auditor.md` & `skills/web-visual-auditor/SKILL.md`
- Remove broken `/browser` and `window.__auditAntiSlopDOM()` instructions (existing `tools: [read_file, run_command, search_web]` already suffices).
- Mandate executing `cdp_audit.py` + inspecting generated `720p`/`1080p`/scrolled/state PNGs via `read_file` against `skills/design-taste/SKILL.md`.
- Output both the **Structural Floor Score** and **Craft Grade (`A–D`)** in `agent-json` (`"sources": ["dom", "vision"]` or separated `"dom"` / `"vision"` tags) without leaking image payloads or temp paths.

---

### Component 3: Component-Closure Code Auditor (`@slop-auditor`)

#### [MODIFY] `~/.gemini/config/plugins/anti-slop-desktop-design/agents/slop-auditor.md`
- Upgrade from single-file auditing to **target import-closure auditing**:
  - For React/Vite views: inspect `src/App.tsx`, `index.html`, `src/index.css`, imported shared components (e.g., `DoubleEntryDistributionGrid.tsx`, `FiscalPeriodChip.tsx`), and the target view `.tsx`.
  - For WinForms controls: inspect paired `.vb` + `.Designer.vb` + `.resx` together (plus `MainShellForm` host budget).
  - Add static checks for `outline-none` without `focus-visible:ring-*`, `transition-all`, Google Fonts / arbitrary font overrides (`W2`), child `.Font` assignments in `.Designer.vb`, missing per-input `TabIndex` / button `&` mnemonics, and missing 5-state coverage (`loading`/`empty`/`error`/`success`/`disabled`).

---

### Component 4: Red Team Reconciliation Engine (`@audit-verifier`)

#### [MODIFY] `~/.gemini/config/plugins/anti-slop-desktop-design/agents/audit-verifier.md`
Replace the draft 4-category rules with the verified **5-Category Mismatch Resolution Matrix**:

1. **Category 1 — The Code Illusion (Visual Reality Trumps Code):**
   - When code uses compact tokens (`py-1`, `h-7`) or omits overflow hints, yet vision/DOM shows tall wrapped rows (`> 30px`), clipped `<select>`/`<input>`, or inner scrollbars.
   - **Ruling:** `VERIFIED FLOOR DEFECT`.
   - **Directive:** **Strictly forbid** `max-h-7 overflow-hidden` (which silently chops data). Mandate splitting composite cells into discrete single-line atomic columns (`S2`/`WS2`), `whitespace-nowrap`, `min-w-0`, and fluid width reallocation.
2. **Category 2 — The Snapshot Illusion (Code Reality Catches Latent Traps):**
   - When a `1080p` screenshot looks clean, but code closure reveals `min-w-[1400px]`, `xl:grid-cols-12` compression at `1280px` (`~1009px` host), `outline-none` without `focus-visible`, or missing `Esc`/keyboard hooks.
   - **Ruling:** `VERIFIED LATENT DEFECT`.
3. **Category 3 — Verified CSS Cascade Override / Non-Floor False Positive:**
   - **Only** when live computed DOM + vision confirm a CSS override makes the rendered output compliant on a **non-Floor** rule (never waive Floor rules `D1`, `S1`, `S2`, `P1`, `A1–A3`, `VT1`, `VV1` on subjective grounds).
   - **Action:** Log exclusively via `python eval_harness.py --record-feedback --rule <ID> --source <code|dom|vision> --outcome false_positive --target <TargetName> --reason "<Measured DOM Proof>"`.
4. **Category 4 — Pure Visual Craft & Taste Defect:**
   - When structural Floor metrics pass, but vision inspection identifies craft violations against `skills/design-taste/SKILL.md` (tinted neutral mismatch, card-in-card nesting, tracked uppercase headers, competing solid primary buttons, generic `"Done"` CTA, `11px` copy).
   - **Ruling:** `CRAFT MAJOR` — lowers **Craft Grade (`A–D`)** and populates `ACTION_ENVELOPE.xml` remediation without falsely blocking Phase 4 compilation.
5. **Category 5 — Scope / Viewport / State Divergence (Not a Contradiction):**
   - When `@slop-auditor` reports a defect inside an unvisited modal/tab (`Clean-Out Reconcile`, `Outbound Bulk Grain Dispatch`), an outer shell file (`src/App.tsx`), or a `720p`-only breakpoint missing from a `1080p` landing shot, classify as `VERIFIED SCOPE/STATE-TRIGGERED DEFECT` and require `@web-visual-auditor` re-capture of that state if visual confirmation is missing.

---

### Component 5: Evaluation Harness, Rubric, Orchestrator & Manifest Synchronization

#### [MODIFY] `~/.gemini/config/plugins/anti-slop-desktop-design/eval/rubric.json`
- Unify `WP3` and `WW1–WW4` naming across `rubric.json`, `audit_dom.js`, `web-visual-auditor.md`, and `skills/web-visual-auditor/SKILL.md`.
- Partition `dimensions` into web vs. WinForms applicability so `.tsx` static evaluations cannot earn 40 free WinForms points (`P3`, `W1–W4`).
- Align `W3` check semantics (`Option Strict On` vs. control instantiation order) and add `craftGrade` metadata.

#### [MODIFY] `~/.gemini/config/plugins/anti-slop-desktop-design/eval/eval_harness.py` & `eval/evaluate_design.ps1`
- Accept `--source` / `-Source` choices `["code", "dom", "vision", "visual", "human"]`.
- Key `findings_feedback.jsonl` counts by `(rule, source, target)` (with non-waivable Floor protection) and populate `corroborated_rules` strictly from genuine visual/DOM evidence rather than duplicated static regexes.
- Remove fake regex `--visual` simulation for Web in `eval_harness.py` (require `--visual-report` or invoke `cdp_audit.py`); add `-VisualReport` and standalone `-TargetUrl` execution to `evaluate_design.ps1`.
- Fix `--visual-report` parsing to ingest `agent-json` directly, populate `visualDimensionScores`, avoid multi-word heading bleed into `visualFindings`, and recognize `[Severity: Blocker]` lines in `blockingDefects`.
- Fix WinForms false passes: inspect paired `.vb`+`.Designer.vb` closures, detect child `.Font = New ...` overrides under `W2`, check inputs/buttons specifically for `TabIndex` (`K1`) and `&` mnemonics (`K2`), subtract container `Padding` from the `980px` `P1` grid budget, remove `glob("*.png")[0]` fallback, and require target STA reflection test existence under `-VerifyGates`.

#### [MODIFY] `~/.gemini/config/plugins/anti-slop-desktop-design/agents/anti-slop-orchestrator.md`, `skills/anti-slop/SKILL.md`, `plugin.json` & `README.md`
- Update Phase 2 dispatch prompts to pass target import-closure paths + State Manifests, and update Phase 3 `ACTION_ENVELOPE.xml` constraints to emit Gate-Separated **Floor Blockers + Craft Grade (`A–D`)** with prototype-native Tailwind v4 tokens (`src/index.css`).
- Route Phase 5 `@winforms-visual-auditor` findings through `@audit-verifier` per [winforms-visual-auditor.md](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/agents/winforms-visual-auditor.md#L191).
- Register `skills/design-taste` in [plugin.json](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/plugin.json) and document the updated CDP + Taste + Red Team architecture in [README.md](file:///C:/Users/maxsa/.gemini/config/plugins/anti-slop-desktop-design/README.md).

---

### Component 6: WinForms Preview Telemetry, Test Parity & Control Remediation (`Project_Accounting`)

#### [MODIFY] `Merchandising_Accounting_App.PreviewTool/Program.vb`, `agents/winforms-visual-auditor.md`, `skills/winforms-visual-auditor/SKILL.md` & `docs/UI_PREVIEW.md`
- Emit companion JSON layout telemetry per capture (`HScrollBar.Visible`, `RowTemplate.Height`, `ColumnHeadersHeight`, `TextRenderer` clipping, child `.Font` override count, host viewport dimensions) alongside PNGs so `@winforms-visual-auditor` pairs vision inspection with exact WinForms measurements.
- Register `ScaleTicketForm`, `DoubleEntryDistributionGridControl`, `FiscalPeriodChipControl`, `BalancedStatusBadgeControl`, and `DisburseVendorPaymentDialog` in `InitializeScenarioRegistry()`, and reconcile scenario/test counts across `Program.vb`, `preview_ui.ps1`, and [docs/UI_PREVIEW.md](file:///c:/Users/maxsa/Documents/Project_Accounting/docs/UI_PREVIEW.md) (`18+` scenarios, `217+` tests).

#### [MODIFY] `Merchandising_Accounting_App.Tests/FormInstantiationTests.vb` & `WorkspaceHygieneTests.vb`
- Expand [FormInstantiationTests.vb](file:///c:/Users/maxsa/Documents/Project_Accounting/Merchandising_Accounting_App.Tests/FormInstantiationTests.vb) to test `ScaleTicketForm` (refactored to Approach B `UserControl`), `FiscalPeriodForm`, `DisburseVendorPaymentDialog`, and all 3 `UI/Controls` controls (`DoubleEntryDistributionGridControl`, `FiscalPeriodChipControl`, `BalancedStatusBadgeControl`) at `1280×720` and `1920×1080`, asserting `HorizontalScrollBar.Visible = False`, `RowTemplate.Height <= 28`, `ColumnHeadersHeight <= 28`, zero text truncation, and zero `.Font` overrides.
- Expand [WorkspaceHygieneTests.vb](file:///c:/Users/maxsa/Documents/Project_Accounting/Merchandising_Accounting_App.Tests/WorkspaceHygieneTests.vb) `ActiveDocumentationLinksInvariant` to scan [docs/UI_PREVIEW.md](file:///c:/Users/maxsa/Documents/Project_Accounting/docs/UI_PREVIEW.md) and `docs/designs/*.md`.
- Remediate [ScaleTicketForm.Designer.vb](file:///c:/Users/maxsa/Documents/Project_Accounting/Merchandising_Accounting_App/UI/Agricultural/ScaleTicketForm.Designer.vb) (Approach B `UserControl`, remove duplicate `pnlHeader`, fix inverted `pnlCalculated`/`pnlInputs` docking, reduce grid columns `<= 940px` inside padded host, remove `17` `.Font` overrides, fix `UseMnemonic`, wire `ProcessCmdKey` & `If Not Me.DesignMode Then`, distinguish primary CTA vs. secondary actions) and [DoubleEntryDistributionGridControl.Designer.vb](file:///c:/Users/maxsa/Documents/Project_Accounting/Merchandising_Accounting_App/UI/Controls/DoubleEntryDistributionGridControl.Designer.vb) (`120×24` `badgeStatus` clipping `"BALANCE"`, `'g'` descender clipping, blue `SystemColors.Highlight` `"ID"` header).
- Execute **Stage 2 Shell-Compaction PR** relaxing `pnlHeader.Height >= 88` / `btn*.Height >= 52` in [MainShellForm.vb](file:///c:/Users/maxsa/Documents/Project_Accounting/Merchandising_Accounting_App/UI/MainShellForm.vb#L24) and [BrandingAndBrandingStringsTests.vb](file:///c:/Users/maxsa/Documents/Project_Accounting/Merchandising_Accounting_App.Tests/BrandingAndBrandingStringsTests.vb#L81) prior to Phase 4/5 transpilation of multi-panel agricultural screens.

---

## Verification Plan

### Automated Verification
1. **Plugin Unit & Harness Regression Tests (Python):**
   - Test `cdp_audit.py` against local `vite preview` (`c:\Users\maxsa\Documents\agri_erp_accounting_ui_prototype`): verify `720p` + `1080p` + scrolled PNGs + all 6 views + interactive tabs/dialogs execute cleanly and OS `%TEMP%` directories are deleted on exit (both success and exception paths).
   - Test `audit_dom.js`: assert detection of `GrowerSettlementView` inner `.overflow-x-auto` overflow (`577 > 549`), `A1`, `A3` on `#B45309` (`< 5.6:1`), `<select>`/`<input>` clipping, `aria-hidden` exclusion in `A2`, and `[role="dialog"]` scoping.
   - Test `eval_harness.py` & `evaluate_design.ps1`: assert `(rule, source, target)` feedback isolation, Floor non-waivability, `--source visual`, `agent-json` `--visual-report` parsing, and failure of `ScaleTicketForm.Designer.vb` (`W2` child `.Font`, `K1`, `P1`) prior to remediation.
2. **End-to-End Live Web Audit Test:**
   - Invoke `@web-visual-auditor` and `@slop-auditor` on `WeighbridgeIntakeView` (`https://grainledger-erp-agricultural-merchandising-accoun.ai.studio/` + local closure) and pass both into `@audit-verifier`.
   - Assert: `ACTION_ENVELOPE.xml` contains separated Floor Blockers + Craft Grade (`A–D`), 3-part Visual Evidence Tuples, prototype-native Tailwind v4 tokens, Category 5 Scope rulings where applicable, and zero leaked screenshot image payloads.
3. **WinForms Build, Preview Telemetry & Hygiene Tests:**
   - Run `dotnet test Merchandising_Accounting_App.slnx` (100% pass across `WorkspaceHygieneTests`, `FormInstantiationTests`, and domain/service suites).
   - Run `powershell -File .\preview_ui.ps1 --all --mode both` and verify JSON telemetry + rendered PNGs show zero scrollbars and zero clipped controls.

### Manual Verification
1. Execute `/anti-slop WeighbridgeIntakeView --audit-only` end-to-end and inspect the generated `ACTION_ENVELOPE.xml` in chat + disk.
2. Visually inspect updated `ScaleTicketForm` (transpiled `UserControl`) and `DoubleEntryDistributionGridControl` PNGs in `current_ui_screenshots/minimize/` and `maximize/` after Phase 5 remediation.
