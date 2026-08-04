# RES-013: Add Shared Result State Contracts

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P2
- Rationale: Shared state contracts reduce repeated infrastructure while preserving the established consumer API.
- Blocking prerequisite: none; ADR-0005 is accepted.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-state-contracts/README.md`
- Prerequisites: `docs/adr/ADR-0005-share-result-state-contracts.md`.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- Core exports `IResultState` and `IResultState<TFailure>` as inspection-only contracts.
- Both Result structs implement the generic interface without changing fields, constructors, or state behavior.
- `TFailure` remains constrained by `where TFailure : struct, Enum` and remains strongly typed.
- Existing public Composition methods retain concrete Result receiver and return types; ordinary calls
  infer their existing generic arguments.
- Internal shared guards use `in TResult` with `where TResult : struct, IResultState`; existing value
  paths must not convert Result to an interface reference.
- Specialized JSON, collection-position, callback, and HTTP exception contracts remain observable.
- All nine core and Composition target frameworks remain supported.

## 🎯 Outcome, scope, and non-goals

Consumers can inspect either Result arity through shared state interfaces, while existing fluent
Composition calls remain concise and package implementations remove only genuinely identical guards.

Do not add `Value`, validation commands, callbacks, factories, or composition behavior to either
interface. Do not merge overloads whose success payload, callback, return shape, or exception context differs.

## 🔍 Current behavior and impact boundary

The core assembly currently exports only the static factory and two Result structs. Both structs
repeat the same private initialization guard. Composition repeats valued and value-less guard overloads,
and its public methods use concrete receivers that support inference and concrete return types.

The impact boundary is the core public type surface, both Result declarations, internal guards in core,
Composition and ASP.NET Core, core contract tests, and package documentation. JSON schema and public
Composition signatures remain unchanged.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Either initialized Result arity | Inspect through `IResultState` | Reports the same initialization and branch state as the concrete value |
| BC-2 | Either failed Result arity | Pass it to a helper accepting `IResultState<TFailure>` with inferred type arguments | Returns the typed failure reason without explicit generic arguments |
| BC-3 | Either successful Result arity | Read `FailureReason` through the generic interface | Throws the existing `InvalidOperationException` |
| BC-4 | Either default Result arity | Consume branch state through the interfaces or existing APIs | Remains uninitialized and rejects branch-dependent access |
| BC-5 | Existing concrete Composition call | Compile and execute without explicit type arguments | Preserves the concrete return type and existing behavior |
| BC-6 | Core, Composition, ASP.NET Core, JSON, or collection validation | Exercise initialized and uninitialized boundaries | Preserves the existing exception type, message context, and callback timing |
| BC-7 | Inspect core assembly public types | Reflect exported types | Exports exactly Result factory, two Result structs, and two state interfaces |

## 🛠️ Implementation contract

| Boundary or artifact | Required change | Compatibility or invariant |
| --- | --- | --- |
| Core interfaces | Add `IResultState` and `IResultState<TFailure>` with existing state/failure properties | Inspection only; complete English XML documentation |
| Result structs | Implement `IResultState<TFailure>` implicitly | No field, layout, constructor, or behavior change |
| Core validation | Consolidate the two private guards through one constrained generic internal helper | No interface-reference parameter and identical exception text |
| Composition validation | Consolidate identical arity-specific guards in the existing validation component | Public extension signatures and inference remain unchanged |
| ASP.NET Core validation | Share one constrained guard across MVC and minimal API adapters | Preserve `InvalidOperationException` behavior |
| JSON and collections | Retain or centralize only when specialized exception context is preserved | JSON continues throwing `JsonException`; collection messages retain position |
| Documentation/tests | Describe interfaces and protect inferred calls and exported types | English-only authored content under repository policy |

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1–BC-4, BC-7 | Contract, core tests | Use both arities through each interface and reflect exports | Properties, failure exceptions, and exact public types match | `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release` |
| BC-5–BC-6 | Unit/regression, Composition tests | Run inferred fluent calls and uninitialized boundary cases | Existing behavior passes unchanged | `dotnet run --project tests/TedToolkit.Results.Composition.Tests/TedToolkit.Results.Composition.Tests.csproj -c Release` |
| BC-6 | Integration, ASP.NET Core and JSON tests | Run adapter and converter suites | Exact existing exception contracts remain | Project-specific Release `dotnet run` commands |
| All | Multi-target build | Build the solution in Release | Zero warnings and errors | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.04–0.09 person-months.
- Confidence: Medium.
- Assumptions: generic interface inference succeeds from both concrete Result implementations and
  analyzer rules accept constrained `in` helpers on every target.
- Excluded work: downstream adoption, new public combinators, interface-returning Composition APIs,
  benchmarks, and package publication.
- Actual effort on completion: Completed in one implementation session, within the planning range.
- Material variance and reason: None.

## 📋 Completion evidence

- `dotnet build TedToolkit.Results.slnx -c Release`: passed for every target framework with zero
  warnings and zero errors.
- Focused interface contract tests: 4 passed.
- Focused exported-surface contract tests: 2 passed.
- Focused Composition guard tests: 12 passed across collection, synchronous binding, and async callback paths.
- ASP.NET Core tests: 11 passed.
- Full core suite: 44 passed and 2 unrelated enum-text casing assertions failed.
- Full Composition suite: 54 passed and 5 unrelated enum-text casing assertions failed.
- Full System.Text.Json suite: 12 passed and 1 unrelated enum-text casing assertion failed.
- The remaining failures expect `First` or `Second`, while the test enums declare `FIRST` and `SECOND`;
  this change does not alter enum formatting or serialization.

## 🔄 Migration and rollback

The change is additive and requires no consumer migration. Consumers opt into interface-typed storage
with the normal CLR boxing cost. Rollback removes the new interfaces and restores local guards before
publication; after publication, removing implemented public interfaces is a compatibility break.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Interface-based extension receiver boxes concrete Results | Allocation and type-shape regression | Prohibited by the implementation contract |
| Inferring `TFailure` from an implemented interface may vary by call shape | Some generic helpers could need explicit type arguments | Protect the approved helper call shape with a compile-time test |
| Specialized guard consolidation could erase context | Weaker diagnostics | Retain specialized helpers unless exception behavior is identical |
