# RES-014: Remove Result State Interfaces

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P2
- Rationale: Remove an unjustified public abstraction before it becomes a published compatibility obligation.
- Blocking prerequisite: none; ADR-0006 is accepted.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-remove-result-state-contracts/README.md`
- Prerequisites: `docs/adr/ADR-0006-keep-concrete-result-contracts.md`.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- Core exports only the static Result factory and both concrete Result structs.
- Both Result arities remain `readonly record struct` with the existing fields and three-state behavior.
- Internal guards receive Boolean initialization state and preserve exact exception type and context.
- Public Composition receivers, return types, inference, callback timing, and exceptions do not change.
- JSON continues throwing `JsonException`; collections retain position; HTTP retains conversion context.
- Authored source, tests, comments, runtime text, and documentation remain English-only.
- All existing target frameworks remain supported.

## 🎯 Outcome, scope, and non-goals

Remove both public Result state interfaces and every active dependency on them while leaving concrete
Result behavior unchanged. Do not add a replacement public abstraction or change representation.

## 🔍 Current behavior and impact boundary

Core currently exports five types because both Result structs implement `IResultState<TFailure>`.
Core, Composition, and ASP.NET Core validation helpers use `where TResult : struct, IResultState`.
The interface-specific core tests and README describe polymorphic use and boxing.

The impact boundary is both interface files, Result declarations, package-local validation helpers,
the exact exported-type test, interface-focused tests, core README, architecture records, and change records.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Core assembly | Reflect exported types | Exactly the static factory and two concrete Result generic types are exported |
| BC-2 | Either initialized or default Result arity | Read state and branch members | Existing values and `InvalidOperationException` behavior remain unchanged |
| BC-3 | Existing synchronous or async Composition call | Compile and execute | Concrete inference, result shape, callback timing, and validation behavior remain unchanged |
| BC-4 | Collection containing an uninitialized Result | Sequence or traverse | Existing position-bearing exception remains |
| BC-5 | Default Result passed to HTTP or JSON adapter | Convert or serialize | Existing HTTP `InvalidOperationException` and JSON `JsonException` remain |
| BC-6 | Active source and documentation | Search for `IResultState` | No active contract or source reference remains outside superseded historical records |

## 🛠️ Implementation contract

| Boundary or artifact | Required change | Compatibility or invariant |
| --- | --- | --- |
| Core public API | Delete both interfaces and remove implementation clauses | Exact three exported types |
| Core guard | Accept `bool isInitialized` | Preserve core exception text |
| Composition guards | Accept Boolean state and existing contextual parameters | Preserve callback and collection messages |
| ASP.NET Core guard | Accept Boolean state | Preserve HTTP conversion message |
| Tests and README | Remove interface-focused behavior and documentation | Retain concrete API coverage |
| Architecture | Supersede ADR-0005 with ADR-0006 | Preserve decision history |

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1–BC-2 | Contract/unit, core tests | Reflect exports and run construction/default tests | Three exports and unchanged state behavior | Core Release `dotnet run` with focused filters |
| BC-3–BC-4 | Unit, Composition tests | Run binding, async callback, and collection guard tests | Existing behavior and messages | Composition Release `dotnet run` with focused filters |
| BC-5 | Integration, ASP.NET Core and JSON tests | Run adapter suites | Existing exception types and behavior | Project-specific Release `dotnet run` commands |
| BC-6 | Static inspection | Search active code and docs | Only superseded/historical records mention interfaces | `rg -n "IResultState" src tests README.md docs/architecture docs/adr` |
| All | Multi-target build | Build solution in Release | Zero warnings and errors | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.03–0.07 person-months.
- Confidence: High.
- Assumptions: no published interface-bearing package and no external consumer migration.
- Excluded work: enum-text casing, new benchmarks, class representation, and package publication.
- Actual effort on completion: Completed in one implementation session, within the planning range.
- Material variance and reason: None.

## 📋 Completion evidence

- `dotnet build TedToolkit.Results.slnx -c Release`: passed for every target framework with zero
  warnings and zero errors.
- Core public-surface contract: 2 focused tests passed and exactly three types are exported.
- Composition validation: 12 focused collection, binding, and async callback tests passed.
- ASP.NET Core integration: 11 tests passed.
- Active source, tests, root README, and architecture contain no `IResultState` reference; both interface
  source files and the focused interface test file are absent.
- Full suites retain the existing enum-name casing mismatch: core 40/42 passed, Composition 54/59
  passed, and System.Text.Json 12/13 passed.

## 🔄 Migration and rollback

No migration is required before publication. If already published, removing implemented interfaces is
binary and source breaking and requires a major release. Rollback restores ADR-0005 and RES-013.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Unpublished-status assumption is wrong | Interface removal becomes breaking | Confirm package publication state before release |
| Historical records are mistaken for active guidance | Reviewers may follow superseded design | Keep explicit Superseded status and links |
