# Migrate Result to an explicit-state value type

## 📌 Status

Completed

- Change owner: TedToolkit maintainers
- Approval owner: TedToolkit maintainers

## 🚦 Change priority

- Priority: P2
- Rationale: Establish the intended public value representation before stable consumers depend on the current class binary shape.
- Directory: `docs/changes/P2-result-value-type-migration/`

## 🎯 Intended outcome and scope

Change `Result<TValue, TFailure>` to an immutable record struct that avoids an ordinary separate
Result allocation and explicitly distinguishes uninitialized, successful, and failed values.
Preserve initialized branch behavior. Remove the null Result-reference contract because the public
type becomes a value type. Do not add combinators, serialization behavior, dependencies to the
product project, or a maintained benchmark suite.

## 🧾 Source intent and hard constraints

- Source request, issue, or brief: User-approved design and benchmark decision rule from 2026-08-03.
- User or business outcome: Use value representation without allowing default values to masquerade as a branch.
- External hard constraints: All configured package target frameworks must continue to build.

## 🧩 Governing principles and decisions

- Applicable principles and revision: None; the repository has no committed revision.
- Related ADRs and status: `docs/adr/ADR-0001-result-value-representation/README.md`.
- Related architecture records and revision: None.
- Reapproval trigger: Benchmark evidence contradicts the approved selection rule or requires a different state model.

## 🧭 Planned approach

Apply the benchmark-supported typed `ResultState` enum with its normal `int` underlying representation to
`readonly record struct Result<TValue, TFailure>`, expose initialization state, reject
uninitialized branch operations, revise conversion and documentation contracts, and verify all
initialized behavior plus the new default state. Retained benchmarks record the accepted trade-off:
int favors the common modern-runtime int-enum path and generated code, while byte only reduces a
narrow measured closed-generic layout.

## ✅ Completion criteria

ADR-0001 records the accepted typed-enum state-width decision; all new default-state and existing
initialized-result tests pass; every configured target framework builds without warnings; package
documentation describes the value type and its default state.

## ⏱️ Workload estimate

- Person-month basis: One full-time engineer working for one calendar month.
- Work-package range total: 0.02–0.04 person-months.
- Coordination, verification, migration, and rollout: 0.005–0.01 person-months.
- Contingency: 0.005–0.01 person-months for benchmark or legacy-target diagnostics.
- Total planning range: 0.03–0.06 person-months.
- Confidence: High.
- Assumptions and excluded work: No published consumer migration or maintained performance suite.
- Re-estimation trigger and approval threshold: A new public API family or downstream migration is required.

## 🚧 Plan blockers

None.

## 🗺️ Delivery map

| ID | Work package | Outcome | Priority and rationale | Estimate | Prerequisites | Status | Document |
| --- | --- | --- | --- | --- | --- | --- | --- |
| RES-002 | Migrate Result to an explicit-state value type | Allocation-free Result with safe default semantics | P2 — establish public shape before adoption | 0.02–0.04 person-months | Accepted ADR-0001 | Implemented | `work-items/RES-002-migrate-result-value-type.md` |

## ⚠️ Risks and coordination

| Item | Impact | Next action |
| --- | --- | --- |
| Class-to-struct migration breaks binary and some source compatibility | Existing consumers must recompile and revise null handling | Complete before stable publication or use a breaking release |
| Large `TValue` values increase copy cost | Value representation may regress some workloads | Preserve large-value copy evidence and document the trade-off |
