# Use Platform Throw Helpers and Inline Hot Guards

## 📌 Status

Completed

- Change owner: TedToolkit maintainers
- Approval owner: User

## 🚦 Change priority

- Priority: P3
- Rationale: This is a requested, deferrable implementation-quality and hot-path optimization with no correctness outage.
- Directory: `docs/changes/P3-use-platform-throw-helpers-and-inline-guards/`

## 🎯 Intended outcome and scope

Use framework-provided argument throw helpers wherever every target supports them, retain explicit
compatibility only for legacy targets, and add aggressive-inlining hints only to short internal
guards on frequently executed paths. Preserve every public API, exception type, parameter name,
Result representation, package boundary, and target framework.

Non-goals are a public Result interface, new exception behavior, blanket inlining, benchmarks,
removing legacy targets, or changing validation policy.

## 🧾 Source intent and hard constraints

- Source request: Prefer C#/.NET-provided throw helpers and use AggressiveInlining where reasonable.
- User outcome: Reduce hand-written guard code and avoid unnecessary call overhead without making the code harder to maintain.
- External hard constraints: all nine core/Composition targets and both ASP.NET Core targets remain supported.

## 🧩 Governing principles and decisions

- Applicable principles and revision: none.
- Related ADRs and status: ADR-0001 and ADR-0003 remain unchanged.
- Related architecture records and revision: `docs/architecture/result-package-boundaries.md`, not yet committed.
- Resulting constraint: no public surface or package-boundary change.
- Reapproval trigger: any public API, exception contract, representation, or target matrix changes.

## 🧭 Planned approach

Update the portable null-guard helpers to delegate to `ArgumentNullException.ThrowIfNull` on
`net6.0` and later while retaining their legacy implementation. Mark those helpers and small
initialization/task guards with `MethodImplOptions.AggressiveInlining`. Leave enum validation,
serialization, mapping, async state machines, and exception construction cold paths unannotated.

## ✅ Completion criteria

- Modern targets use the platform null helper and legacy targets retain identical behavior.
- Only short internal guards receive inlining hints.
- All existing tests pass and the full solution builds with zero warnings and errors.

## ⏱️ Workload estimate

- Person-month basis: one developer working for one complete team-capacity month.
- Work-package range total: 0.01–0.02 person-months.
- Coordination, verification, migration, and rollout: 0.00–0.01 person-months.
- Contingency: 0.01 person-months for analyzer or legacy-target compatibility.
- Total planning range: 0.01–0.04 person-months.
- Confidence: High.
- Assumptions and excluded work: no benchmark gate and no public interface work.
- Re-estimation trigger and approval threshold: reapprove if public behavior or representation changes.

## 🚧 Plan blockers

None.

## 🗺️ Delivery map

| ID | Work package | Outcome | Priority and rationale | Estimate | Prerequisites | Status | Document |
| --- | --- | --- | --- | --- | --- | --- | --- |
| PERF-001 | Optimize portable guards | Platform throws and selective inlining without behavior change | P3 — implementation-quality optimization | 0.01–0.02 person-months | None | Implemented | `work-items/PERF-001-optimize-portable-guards.md` |

## ⚠️ Risks and coordination

| Item | Impact | Next action |
| --- | --- | --- |
| Excessive inlining can increase generated code size | Performance may regress | Annotate only tiny high-frequency guards; leave complex methods to the JIT |
| Platform helpers are unavailable on legacy targets | Legacy compilation could fail | Retain conditional portable fallback and build the full target matrix |
