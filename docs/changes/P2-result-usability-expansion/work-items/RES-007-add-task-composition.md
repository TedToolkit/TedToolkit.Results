# RES-007: Add Task Composition

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P2
- Rationale: Application services commonly return Task-based Results and need the same branch guarantees as synchronous pipelines.
- Blocking prerequisite: RES-004, RES-005, and RES-006.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-usability-expansion/README.md`
- Prerequisites: RES-004, RES-005, RES-006, `docs/adr/ADR-0003-isolate-optional-composition.md`.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- Async APIs preserve the approved synchronous branch, null, failure, and exception semantics.
- Cancellation and callback exceptions propagate through the returned Task and never become enum failures.
- The initial async surface uses `Task`, not `ValueTask` or `IAsyncEnumerable`.
- No API blocks synchronously, captures a required synchronization context, or calls `.Result`/`.Wait()`.
- The APIs live in optional `TedToolkit.Results.Composition`; the core assembly exposes no Task extension type.
- Uninitialized source or callback Results are rejected and never propagated as an active pipeline state.

## 🎯 Outcome, scope, and non-goals

Consumers can map, bind, match, validate, and recover Results with Task-returning callbacks and can
continue a pipeline whose previous stage is `Task<Result<...>>`.

Add a focused `ResultAsyncExtensions` surface for both Result arities and Task-wrapped Results. Do
not add ValueTask, async-enumerable, parallel execution, retry, timeout, logging, or exception-to-
failure conversion APIs.

## 🔍 Current behavior and impact boundary

After the synchronous work packages, consumers still need an `await`, a local Result variable, and
manual continuation for every asynchronous stage. This work affects the optional Composition public
API, Task-focused tests, XML documentation, and async examples. It does not change either Result record.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Successful valued Result | `MapAsync` or `BindAsync` with an async callback | Awaits one callback and returns its mapped or bound branch |
| BC-2 | Failed valued Result | `MapAsync` or `BindAsync` | Does not invoke the callback and propagates the failure |
| BC-3 | Either valued branch | `MatchAsync` | Awaits exactly the active callback and returns its output |
| BC-4 | Successful valued Result | `EnsureAsync` | Evaluates the async predicate and preserves success or returns the supplied failure |
| BC-5 | Failed valued Result | `RecoverAsync` or `RecoverWithAsync` | Awaits once and returns the recovered success or Result |
| BC-6 | Successful value-less Result | `BindAsync` into either Result arity | Awaits once and returns the bound Result |
| BC-7 | Failed value-less Result | `BindAsync` | Does not invoke and propagates the failure |
| BC-8 | `Task<Result<...>>` source | Chain two or more async extension calls | Awaits the source once per stage and preserves short-circuit behavior |
| BC-9 | Source Task is canceled or faulted | Await any extension | Propagates cancellation or the original failure exception |
| BC-10 | Invoked callback is canceled or faulted | Await any extension | Propagates cancellation or the original callback exception |
| BC-11 | Result-producing callback completes with an Uninitialized Result | Await Bind or RecoverWith | Throws `InvalidOperationException` through the returned Task |
| BC-12 | Delegate or Task argument is null | Invoke the API | Throws `ArgumentNullException` according to documented async timing |
| BC-13 | Direct or awaited source Result is Uninitialized | Await any extension | Throws `InvalidOperationException` and invokes no callback |
| BC-14 | An invoked callback returns a null Task | Await any extension | Throws `InvalidOperationException` identifying an invalid callback task |

## 🛠️ Implementation contract

Create public static `ResultAsyncExtensions` in the `TedToolkit.Results.Composition` package and
`TedToolkit.Results` namespace with exactly the
following valued-Result operation families. Each shown direct-Result overload also has the displayed
Task-source overload; no callback-shape convenience overloads are added.

```csharp
public static Task<Result<TNext, TFailure>> MapAsync<TValue, TFailure, TNext>(
    this Result<TValue, TFailure> result,
    Func<TValue, CancellationToken, Task<TNext>> mapper,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TNext, TFailure>> MapAsync<TValue, TFailure, TNext>(
    this Task<Result<TValue, TFailure>> resultTask,
    Func<TValue, CancellationToken, Task<TNext>> mapper,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TNext, TFailure>> BindAsync<TValue, TFailure, TNext>(
    this Result<TValue, TFailure> result,
    Func<TValue, CancellationToken, Task<Result<TNext, TFailure>>> binder,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TNext, TFailure>> BindAsync<TValue, TFailure, TNext>(
    this Task<Result<TValue, TFailure>> resultTask,
    Func<TValue, CancellationToken, Task<Result<TNext, TFailure>>> binder,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<TResult> MatchAsync<TValue, TFailure, TResult>(
    this Result<TValue, TFailure> result,
    Func<TValue, CancellationToken, Task<TResult>> onSuccess,
    Func<TFailure, CancellationToken, Task<TResult>> onFailure,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<TResult> MatchAsync<TValue, TFailure, TResult>(
    this Task<Result<TValue, TFailure>> resultTask,
    Func<TValue, CancellationToken, Task<TResult>> onSuccess,
    Func<TFailure, CancellationToken, Task<TResult>> onFailure,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TValue, TFailure>> EnsureAsync<TValue, TFailure>(
    this Result<TValue, TFailure> result,
    Func<TValue, CancellationToken, Task<bool>> predicate,
    TFailure failureReason,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TValue, TFailure>> EnsureAsync<TValue, TFailure>(
    this Task<Result<TValue, TFailure>> resultTask,
    Func<TValue, CancellationToken, Task<bool>> predicate,
    TFailure failureReason,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TValue, TFailure>> RecoverAsync<TValue, TFailure>(
    this Result<TValue, TFailure> result,
    Func<TFailure, CancellationToken, Task<TValue>> recovery,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TValue, TFailure>> RecoverAsync<TValue, TFailure>(
    this Task<Result<TValue, TFailure>> resultTask,
    Func<TFailure, CancellationToken, Task<TValue>> recovery,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TValue, TFailure>> RecoverWithAsync<TValue, TFailure>(
    this Result<TValue, TFailure> result,
    Func<TFailure, CancellationToken, Task<Result<TValue, TFailure>>> recovery,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TValue, TFailure>> RecoverWithAsync<TValue, TFailure>(
    this Task<Result<TValue, TFailure>> resultTask,
    Func<TFailure, CancellationToken, Task<Result<TValue, TFailure>>> recovery,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;
```

Provide exactly these value-less operation families:

```csharp
public static Task<Result<TFailure>> BindAsync<TFailure>(
    this Result<TFailure> result,
    Func<CancellationToken, Task<Result<TFailure>>> binder,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TFailure>> BindAsync<TFailure>(
    this Task<Result<TFailure>> resultTask,
    Func<CancellationToken, Task<Result<TFailure>>> binder,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TValue, TFailure>> BindAsync<TValue, TFailure>(
    this Result<TFailure> result,
    Func<CancellationToken, Task<Result<TValue, TFailure>>> binder,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<Result<TValue, TFailure>> BindAsync<TValue, TFailure>(
    this Task<Result<TFailure>> resultTask,
    Func<CancellationToken, Task<Result<TValue, TFailure>>> binder,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<TResult> MatchAsync<TFailure, TResult>(
    this Result<TFailure> result,
    Func<CancellationToken, Task<TResult>> onSuccess,
    Func<TFailure, CancellationToken, Task<TResult>> onFailure,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;

public static Task<TResult> MatchAsync<TFailure, TResult>(
    this Task<Result<TFailure>> resultTask,
    Func<CancellationToken, Task<TResult>> onSuccess,
    Func<TFailure, CancellationToken, Task<TResult>> onFailure,
    CancellationToken cancellationToken = default)
    where TFailure : struct, Enum;
```

Every method accepts an optional final `CancellationToken cancellationToken = default` and passes it
to the invoked callback. A short-circuited branch does not independently throw merely because the
token is already canceled. Do not add callback overloads without a CancellationToken in this first
surface; a lambda can ignore the supplied token explicitly.

Task-wrapped overloads must await the source with `ConfigureAwait(false)`, then apply the same branch
logic as the direct Result overload. They must
reject a source Task that completes with an Uninitialized Result using `InvalidOperationException`.

Null Task/delegate arguments and Uninitialized direct sources are validated synchronously before a
Task is returned. Faults and cancellation from non-null source or callback Tasks are observed when
the returned Task is awaited. An invoked callback that returns a null Task or a Result-producing
callback that returns an Uninitialized Result fails with `InvalidOperationException`.

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1–BC-5 | Unit, `ResultAsyncCompositionTests.cs` | Exercise each valued operation on both branches | Result branch, payload, callback count, and token identity | `dotnet run --project tests/TedToolkit.Results.Composition.Tests/TedToolkit.Results.Composition.Tests.csproj -c Release` |
| BC-6, BC-7 | Unit, `ValueLessResultAsyncCompositionTests.cs` | Bind value-less branches into both arities | Exact propagation and invocation count | Same command |
| BC-8 | Unit, `TaskResultPipelineTests.cs` | Chain at least three stages with a middle failure | Later callbacks do not execute | Same command |
| BC-9, BC-10 | Unit, affected files | Use canceled and faulted source/callback Tasks | Await observes cancellation or same exception instance | Same command |
| BC-11–BC-14 | Unit, affected files | Return default Result/null Task, supply null Task/delegate, or await a default source | Exact documented exception and timing | Same command |
| All | Multi-target build | Build Release | Task APIs compile for every Composition target | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.05–0.09 person-months.
- Confidence: Medium.
- Assumptions: Task and CancellationToken are sufficient for the first async surface.
- Excluded work: ValueTask, async streams, concurrency, retry, timeout, and exception mapping.
- Actual effort on completion: Completed in the current implementation.
- Material variance and reason: The approved DDD boundary moved the extensions from core to the optional Composition package.

## 📋 Completion evidence

The Composition project builds all nine targets with zero warnings. Its test suite covers direct and
Task-wrapped sources, both Result arities, callback selection, cancellation, and invalid inputs.

## 🔄 Migration and rollback

The extension API is additive. Rollback removes the extension class before publication without
changing either Result record.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Async overload count can dominate IntelliSense | Discoverability of the core sync surface may decline | Keep all methods in one clearly named extension class and exclude convenience permutations |
| Cancellation timing differs between active and short-circuited branches | Consumers may expect eager cancellation | Document that cancellation belongs to invoked work, while existing failures short-circuit |
