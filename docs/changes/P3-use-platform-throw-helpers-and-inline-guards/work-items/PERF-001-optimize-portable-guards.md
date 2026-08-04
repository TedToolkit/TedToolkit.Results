# PERF-001: Optimize Portable Guards

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P3
- Rationale: Reduce avoidable custom guard execution while preserving behavior.
- Blocking prerequisite: none.

## 🔗 Delivery context

- Parent change: `docs/changes/P3-use-platform-throw-helpers-and-inline-guards/README.md`
- Prerequisites: none.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- Public APIs, exceptions, parameter names, package dependencies, value layout, and target matrices do not change.
- Platform throw helpers are used only on targets that provide them.
- AggressiveInlining is limited to small synchronous internal guards; complex and cold methods remain JIT-directed.
- Repository-authored comments and documentation remain English.

## 🎯 Outcome, scope, and non-goals

Modern targets delegate null validation to the platform, legacy targets retain the portable fallback,
and frequently invoked small guards carry explicit inlining hints. No user-visible Result behavior changes.

## 🔍 Current behavior and impact boundary

Core and Composition each contain a portable `ThrowIfNull` implementation. Result, Composition,
JSON, and ASP.NET Core packages also contain small initialization guards called from public hot paths.
Existing tests already characterize their observable exception and branch behavior.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Modern target and null callback/source | Invoke a guarded API | Throws `ArgumentNullException` with the existing parameter name |
| BC-2 | Legacy target compilation | Build all target frameworks | Portable fallback compiles without platform-only API references |
| BC-3 | Initialized or default Result | Exercise existing APIs | Branch results and `InvalidOperationException` behavior remain unchanged |
| BC-4 | Guard source inspection | Inspect optimization attributes | Only selected small internal guards are marked AggressiveInlining |

## 🛠️ Implementation contract

| Boundary or artifact | Required change | Compatibility or invariant |
| --- | --- | --- |
| Core and Composition null helpers | Call `ArgumentNullException.ThrowIfNull` under `NET6_0_OR_GREATER`; retain legacy fallback | Same exception type and parameter name |
| Small initialization and callback-task guards | Add `MethodImplOptions.AggressiveInlining` | Internal implementation only |
| Enum and payload logic | No inlining hint or behavioral rewrite | Complex/cold paths remain unchanged |

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1, BC-3 | Regression, existing four test projects | Run existing behavior suites | All exception and branch tests pass | Documented `dotnet run` commands |
| BC-2 | Multi-target build | Build the solution in Release | Zero warnings and errors | `dotnet build TedToolkit.Results.slnx -c Release` |
| BC-4 | Source inspection | Search MethodImpl attributes | Attributes occur only on approved guard methods | `rg` inspection |

## ⏱️ Workload estimate

- Planning range: 0.01–0.02 person-months.
- Confidence: High.
- Assumptions: existing behavior tests sufficiently characterize this internal refactor.
- Excluded work: benchmarks, public interfaces, state representation changes, and new target frameworks.
- Actual effort on completion: Completed in the current implementation.
- Material variance and reason: None.

## 📋 Completion evidence

- `dotnet build TedToolkit.Results.slnx -c Release`: succeeded with zero warnings and errors across every target.
- Core tests: 83 passed.
- Composition tests: 15 passed.
- System.Text.Json tests: 13 passed.
- ASP.NET Core tests: 11 passed.
- Source inspection confirms platform null helpers on modern targets, legacy fallbacks, and eleven
  AggressiveInlining annotations limited to internal guard methods.

## 🔄 Migration and rollback

No migration is required. Rollback removes the internal attributes and restores the portable helper body.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| AggressiveInlining remains a JIT hint | Runtime may still decline to inline | Accept; do not promise a measured gain without benchmarks |
