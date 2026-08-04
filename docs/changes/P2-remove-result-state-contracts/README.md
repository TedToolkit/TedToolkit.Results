# Remove Result State Interfaces

## 📌 Status

Completed

- Change owner: TedToolkit maintainers
- Approval owner: User

## 🚦 Change priority

- Priority: P2
- Rationale: This planned public API correction removes unjustified abstraction before publication.
- Directory: `docs/changes/P2-remove-result-state-contracts/`

## 🎯 Intended outcome and scope

Return the core package to three concrete public Result types and preserve shared internal validation
without public interfaces. Scope includes interface removal, struct declaration cleanup, Boolean guard
inputs, public-surface tests, package documentation, and regression verification.

Non-goals are changing the `readonly record struct` representation, adding a public guard, changing
Composition signatures, changing exception behavior, addressing unrelated enum-text casing failures,
or changing target frameworks.

## 🧾 Source intent and hard constraints

- Source request: Remove the two state interfaces and apply the simpler concrete Result design.
- User or business outcome: Reduce total public and conceptual complexity while retaining existing Result usage.
- External hard constraints: None.

## 🧩 Governing principles and decisions

- Applicable principles and revision: none.
- Related ADRs and status: `docs/adr/ADR-0006-keep-concrete-result-contracts.md` (Accepted, not yet committed).
- Related architecture records and revision: `docs/architecture/result-package-boundaries.md` (Active, not yet committed).
- Reapproval trigger: a public interface or guard remains necessary, struct representation changes,
  exception behavior changes, or a public Composition signature changes.

The repository has no Git commit, so no governing revision can be pinned. The user approved the
direction on 2026-08-04; pin the records after the initial repository commit.

## 🧭 Planned approach

Deliver one work package. First restore the exact three-type public-surface contract test, then remove
the interfaces and interface-specific tests. Replace constrained Result guard parameters with Boolean
state parameters in core, Composition, and ASP.NET Core while preserving specialized contexts.

## ✅ Completion criteria

- Core exports exactly `Result`, `Result<TFailure>`, and `Result<TValue, TFailure>`.
- Neither Result struct implements a public Result interface.
- No `IResultState` source, test, package documentation, or active architecture contract remains.
- All existing Result construction, state, composition, JSON, collection, and HTTP behavior is unchanged.
- The full Release solution builds for every target with zero warnings and errors.

## ⏱️ Workload estimate

- Person-month basis: one experienced library developer working for one complete team-capacity month.
- Work-package range total: 0.03–0.07 person-months.
- Coordination, verification, migration, and rollout: 0.01–0.02 person-months.
- Contingency: 0.01–0.03 person-months for cross-target analyzer or validation-timing regressions.
- Total planning range: 0.05–0.12 person-months.
- Confidence: High.
- Assumptions and excluded work: packages are not published; enum-text casing, new benchmarks, and
  downstream migration are excluded.
- Re-estimation trigger and approval threshold: reapprove above 0.12 person-months or if removal
  requires a public replacement abstraction.

## 🚧 Plan blockers

None.

## 🗺️ Delivery map

| ID | Work package | Outcome | Priority and rationale | Estimate | Prerequisites | Status | Document |
| --- | --- | --- | --- | --- | --- | --- | --- |
| RES-014 | Remove Result state interfaces | Core returns to concrete contracts with internal Boolean guards | P2 — simplifies public API before publication | 0.03–0.07 person-months | ADR-0006 | Implemented | `work-items/RES-014-remove-result-state-interfaces.md` |

## 📋 Completion evidence

- Deleted both public interface files, their implementation clauses, focused tests, and package README guidance.
- Restored the exact three-export core contract; both focused public-surface tests pass.
- Core, Composition, and ASP.NET Core shared guards now receive Boolean initialization state while
  retaining their existing exception context.
- Focused Composition guard tests pass (12 tests), and ASP.NET Core tests pass (11 tests).
- The complete Release solution build passes for every target framework with zero warnings and errors.
- Full core, Composition, and System.Text.Json suites retain the same eight unrelated enum-text casing
  failures documented before this correction; the passing counts are 40, 54, and 12 respectively.

## ⚠️ Risks and coordination

| Item | Impact | Next action |
| --- | --- | --- |
| Validation refactor changes exception context | Consumer diagnostics regress | Keep existing helper-specific messages and focused tests |
| Interface artifacts remain in documentation | Consumers see a stale contract | Search all active source, tests, README, architecture, and ADR references |
