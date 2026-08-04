# Add Shared Result State Contracts

## 📌 Status

Completed

- Change owner: TedToolkit maintainers
- Approval owner: User

## 🚦 Change priority

- Priority: P2
- Rationale: This planned public contract improvement enables shared Result infrastructure and removes repeated guards without addressing an outage.
- Directory: `docs/changes/P2-result-state-contracts/`

## 🎯 Intended outcome and scope

Expose type-safe state contracts shared by valued and value-less Results, then use them selectively
inside package implementations where semantics, inference, and exception behavior remain unchanged.

Scope includes the two public interfaces, implicit implementation by both Result structs, public API
and inference contract tests, package-level guard consolidation, architecture and package documentation,
and full regression verification. Non-goals are replacing concrete Composition signatures, merging
payload-dependent overloads, adding interface composition methods, changing exception contracts,
changing Result layout, or changing target frameworks.

## 🧾 Source intent and hard constraints

- Source request: Create non-generic and failure-generic Result state interfaces and use them where
  callers do not need explicit type arguments.
- User or business outcome: Shared Result infrastructure becomes easier to write while fluent consumer
  syntax stays concise.
- External hard constraints: None.

## 🧩 Governing principles and decisions

- Applicable principles and revision: none.
- Related ADRs and status: `docs/adr/ADR-0005-share-result-state-contracts.md` (Accepted, not yet committed).
- Related architecture records and revision: `docs/architecture/result-package-boundaries.md` (Active, not yet committed).
- Reapproval trigger: public Composition signatures become interface-based, a Result value is boxed on
  an existing path, payload types become untyped, exception behavior changes, or the target matrix changes.

The repository has no Git commit, so no governing revision can be pinned. The user approved this
direction on 2026-08-04; pin the records after the initial repository commit.

## 🧭 Planned approach

Deliver one coherent work package. Add the interface hierarchy to core and implement it on both Result
structs. Preserve existing concrete public extensions and inferred calls. Replace duplicated internal
arity overloads with constrained generic guards only where both arities use the same exception contract;
retain specialized JSON and collection validation when their observable exception differs.

## ✅ Completion criteria

- Both Result arities implement `IResultState<TFailure>` and expose the existing properties through it.
- Consumer helper calls infer `TFailure` from either concrete Result without explicit type arguments.
- Existing Composition calls compile unchanged and retain concrete Result return types.
- Core, Composition, and ASP.NET Core no longer duplicate equivalent valued/value-less guard overloads.
- JSON and collection-specific exception behavior remains unchanged.
- All affected tests and the full Release build pass with zero warnings and errors.

## ⏱️ Workload estimate

- Person-month basis: one experienced library developer working for one complete team-capacity month.
- Work-package range total: 0.04–0.09 person-months.
- Coordination, verification, migration, and rollout: 0.01–0.03 person-months.
- Contingency: 0.01–0.03 person-months for cross-target inference, analyzer, or constrained-call issues.
- Total planning range: 0.06–0.15 person-months.
- Confidence: Medium.
- Assumptions and excluded work: packages are not yet published; downstream adoption, new combinators,
  and an interface-oriented parallel Composition API are excluded.
- Re-estimation trigger and approval threshold: reapprove above 0.15 person-months or if inferred,
  non-boxing implementation requires a public signature change.

## 🚧 Plan blockers

None.

## 🗺️ Delivery map

| ID | Work package | Outcome | Priority and rationale | Estimate | Prerequisites | Status | Document |
| --- | --- | --- | --- | --- | --- | --- | --- |
| RES-013 | Add shared Result state contracts | Both arities share typed state contracts while inferred concrete Composition calls remain intact | P2 — improves contract reuse without widening behavior | 0.04–0.09 person-months | ADR-0005 | Implemented | `work-items/RES-013-add-result-state-contracts.md` |

## 📋 Completion evidence

- Added both public interfaces and implemented the generic contract on both Result structs.
- Added four focused interface behavior tests and updated the exact exported-type contract; all six
  focused core tests pass.
- Consolidated core, synchronous, asynchronous, collection, MVC, and minimal API guards through
  inferred constrained generic calls. JSON retains its specialized `JsonException` guard.
- Focused Composition collection, binding, and async callback guard tests pass (12 tests total).
- ASP.NET Core tests pass (11 tests). The complete Release solution build succeeds for every target
  framework with zero warnings and zero errors.
- Full core, Composition, and System.Text.Json suites retain eight unrelated pre-existing failures in
  which tests expect title-cased enum text while `TestFailure` declares `FIRST` and `SECOND`. No changed
  implementation participates in that casing behavior.

## ⚠️ Risks and coordination

| Item | Impact | Next action |
| --- | --- | --- |
| Interface-typed parameters box Result structs | Existing allocation-free paths could regress | Keep public extensions concrete and constrain internal helpers by struct type |
| Generic abstraction can weaken inference | Callers could need explicit type arguments | Add compile-time consumer tests using inferred calls |
| Core public surface expands | Existing boundary tests fail until updated | Protect the exact five exported contract types |
