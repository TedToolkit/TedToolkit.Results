# RES-012: Move Transferable APIs to Composition

## 📌 Status

Completed

## 🚦 Delivery priority

- Priority: P2
- Rationale: The core/Composition boundary is a public contract concern for every DDD consumer.
- Blocking prerequisite: none; ADR-0004 is accepted.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-minimize-result-contract-surface/README.md`
- Prerequisites: `docs/adr/ADR-0004-minimize-result-contract-surface.md`.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- Core retains construction, factories, state/payload properties, conditional operators, conversion
  operators, record equality, safe `ToString`, and internal invariant validation.
- Core exposes no public method that can be transferred without losing approved C# language behavior.
- Composition retains namespace `TedToolkit.Results` and references only the core package.
- Every moved API preserves its existing generic constraints, callback timing, inactive payload,
  nullable-flow metadata, exception type, exception timing, and Uninitialized rejection.
- No public `IResult` interface or new combinator is introduced.
- All nine core and Composition target frameworks remain supported.

## 🎯 Outcome, scope, and non-goals

Consumers referencing Composition continue to call synchronous branch and pipeline APIs fluently,
while consumers referencing only core see a minimal Result value contract.

Move Match, TryGet, Deconstruct, Map, Bind, MapFailure, Ensure, Recover, and RecoverWith for both
applicable Result arities. Move their tests and XML documentation with them. Do not change factories,
properties, operators, conversions, equality, formatting, async/collection signatures, JSON, or HTTP behavior.

## 🔍 Current behavior and impact boundary

`Result<TValue, TFailure>` currently owns eleven transferable methods and
`Result<TFailure>` owns five. Their focused tests currently compile in the core test project.
Composition already owns Task and collection extensions in the same namespace, so it is the existing
distribution boundary for optional Result behavior.

This change affects both core Result source files, the Composition source/test projects, package
READMEs, the core public-surface contract test, and historical documentation whose current ownership
statement is superseded by ADR-0004. Integration packages continue using factories and properties only.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Core-only reference | Inspect either Result type | No transferable method is declared; construction, properties, operators, conversions, equality, and formatting remain available |
| BC-2 | Composition reference and valued success/failure | Call Match or TryGet extensions | Invokes or returns only the active branch with existing nullable metadata |
| BC-3 | Composition reference and either Result arity | Use deconstruction or positional switch syntax | Extension Deconstruct supplies the same discriminator and inactive default payloads |
| BC-4 | Valued Result | Call Map, Bind, or MapFailure | Preserves existing branch propagation and callback behavior |
| BC-5 | Valued Result | Call either Ensure overload | Preserves validation, failure creation, and short-circuit behavior |
| BC-6 | Valued Result | Call Recover or RecoverWith | Preserves recovery branches and callback behavior |
| BC-7 | Value-less Result | Call either Bind overload | Preserves value-less or valued binding and first failure |
| BC-8 | Null callback/source or callback exception | Invoke a moved extension | Preserves exact argument validation and exception propagation |
| BC-9 | Uninitialized source or callback Result | Invoke a moved extension | Throws `InvalidOperationException` at the existing boundary and invokes no inactive callback |
| BC-10 | Existing async, collection, JSON, ASP.NET, operator, conversion, equality, factory, or formatting consumer | Build and execute existing tests | Behavior remains unchanged |

## 🛠️ Implementation contract

Add `public static class ResultExtensions` to `TedToolkit.Results.Composition` in namespace
`TedToolkit.Results`. It owns these exact public extension families:

```csharp
TResult Match<TValue, TFailure, TResult>(
    this Result<TValue, TFailure> result,
    Func<TValue, TResult> onSuccess,
    Func<TFailure, TResult> onFailure)
    where TFailure : struct, Enum;

TResult Match<TFailure, TResult>(
    this Result<TFailure> result,
    Func<TResult> onSuccess,
    Func<TFailure, TResult> onFailure)
    where TFailure : struct, Enum;

bool TryGetValue<TValue, TFailure>(
    this Result<TValue, TFailure> result,
    [MaybeNullWhen(false)] out TValue value)
    where TFailure : struct, Enum;

bool TryGetFailure<TValue, TFailure>(
    this Result<TValue, TFailure> result,
    out TFailure failureReason)
    where TFailure : struct, Enum;

bool TryGetFailure<TFailure>(this Result<TFailure> result, out TFailure failureReason)
    where TFailure : struct, Enum;

void Deconstruct<TValue, TFailure>(
    this Result<TValue, TFailure> result,
    out bool isSuccess,
    [MaybeNull] out TValue value,
    out TFailure failureReason)
    where TFailure : struct, Enum;

void Deconstruct<TFailure>(
    this Result<TFailure> result,
    out bool isSuccess,
    out TFailure failureReason)
    where TFailure : struct, Enum;

Result<TNext, TFailure> Map<TValue, TFailure, TNext>(
    this Result<TValue, TFailure> result,
    Func<TValue, TNext> mapper)
    where TFailure : struct, Enum;

Result<TNext, TFailure> Bind<TValue, TFailure, TNext>(
    this Result<TValue, TFailure> result,
    Func<TValue, Result<TNext, TFailure>> binder)
    where TFailure : struct, Enum;

Result<TValue, TNextFailure> MapFailure<TValue, TFailure, TNextFailure>(
    this Result<TValue, TFailure> result,
    Func<TFailure, TNextFailure> mapper)
    where TFailure : struct, Enum
    where TNextFailure : struct, Enum;

Result<TValue, TFailure> Ensure<TValue, TFailure>(
    this Result<TValue, TFailure> result,
    Func<TValue, bool> predicate,
    TFailure failureReason)
    where TFailure : struct, Enum;

Result<TValue, TFailure> Ensure<TValue, TFailure>(
    this Result<TValue, TFailure> result,
    Func<TValue, bool> predicate,
    Func<TValue, TFailure> failureFactory)
    where TFailure : struct, Enum;

Result<TValue, TFailure> Recover<TValue, TFailure>(
    this Result<TValue, TFailure> result,
    Func<TFailure, TValue> recovery)
    where TFailure : struct, Enum;

Result<TValue, TFailure> RecoverWith<TValue, TFailure>(
    this Result<TValue, TFailure> result,
    Func<TFailure, Result<TValue, TFailure>> recovery)
    where TFailure : struct, Enum;

Result<TFailure> Bind<TFailure>(
    this Result<TFailure> result,
    Func<Result<TFailure>> binder)
    where TFailure : struct, Enum;

Result<TValue, TFailure> Bind<TValue, TFailure>(
    this Result<TFailure> result,
    Func<Result<TValue, TFailure>> binder)
    where TFailure : struct, Enum;
```

Implement extensions only through the core public contract and static factories. Do not expose
core internals to Composition. Add portable nullable attributes to Composition only when required
by its legacy targets. Preserve selective AggressiveInlining only for small internal guards.

Remove the corresponding instance methods and their XML documentation from both core Result types.
Do not remove or rename constructors, properties, factories, operators, conversions, `ToString`, or
internal `CreateSuccess` and invariant helpers.

Move focused behavior tests from `TedToolkit.Results.Tests` to
`TedToolkit.Results.Composition.Tests`; split mixed formatting/binding tests so formatting remains
owned by core. Extend core contract tests to reject all moved method names. Add a Composition
compile/runtime case proving extension-based deconstruction and positional switch syntax.

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1 | Contract, core tests | Reflect declared public members and exercise retained language API | Moved names absent; retained members work | `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release` |
| BC-2–BC-9 | Unit/contract, Composition tests | Run moved tests against extension methods, including default/null/throw cases | Exact existing behavior and syntax | `dotnet run --project tests/TedToolkit.Results.Composition.Tests/TedToolkit.Results.Composition.Tests.csproj -c Release` |
| BC-10 | Regression, JSON and ASP.NET tests | Run both integration suites | Existing contracts pass | Their documented `dotnet run` commands |
| All | Multi-target build/package | Build the solution in Release | Zero warnings/errors and valid package assets | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.05–0.10 person-months.
- Confidence: Medium.
- Assumptions: extension deconstruction is supported across the compiler target set and packages are
  not yet published from this repository.
- Excluded work: public interfaces, downstream repository migration, benchmarks, and package publication.
- Actual effort on completion: Completed in one implementation session, within the planning range.
- Material variance and reason: None.

## 📋 Completion evidence

- Added `ResultExtensions` to Composition with all 16 approved signatures and legacy nullable
  attribute polyfills.
- Removed all transferable instance methods and obsolete nullable polyfills from core.
- Moved branch-consumption and pipeline tests to Composition, split value-less formatting back into
  core, and added value-less null-validation coverage.
- Verified extension deconstruction through assignment and positional switch syntax.
- Core tests: 42 passed. Composition tests: 59 passed. System.Text.Json tests: 13 passed. ASP.NET Core
  tests: 11 passed.
- Full Release build: succeeded across all target frameworks with zero warnings and zero errors.
- Deliberate migration note: ordinary inferred fluent calls remain unchanged. Calls that previously
  supplied only an instance method's generic arguments now require inference or a typed delegate,
  because C# does not support partially explicit generic arguments on extension methods. This was an
  anticipated risk and does not alter runtime behavior.

## 🔄 Migration and rollback

Source consumers add `TedToolkit.Results.Composition`; ordinary inferred fluent syntax remains
unchanged because the namespace is unchanged. Partially explicit generic calls use inference or typed
delegates. Existing binaries must recompile, and published packages require a major-version decision.
Rollback restores the instance methods, core tests, and ADR-0003 boundary before publication; do not
publish duplicate instance and extension APIs under the same signatures.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Extension generic inference differs for partially explicit calls | Those calls require inference or a typed delegate | Documented in the Composition README; covered by typed null-delegate tests |
| Deconstruction extension discovery can vary by syntax | Previously supported patterns could stop compiling | Closed: assignment deconstruction and positional switch expressions pass |
| Removed binary members break precompiled consumers | Runtime binding failure | Coordinate a major version and require recompilation before publication |
| Moving nullable annotations can change metadata | Flow analysis could regress on legacy or modern consumers | Preserve attributes and build representative target metadata |
