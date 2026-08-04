# RES-008: Add Collection Composition

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P3
- Rationale: Collection helpers remove repetitive loops but are deferrable after single-Result composition.
- Blocking prerequisite: RES-004 and RES-006.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-usability-expansion/README.md`
- Prerequisites: RES-004, RES-006, `docs/adr/ADR-0003-isolate-optional-composition.md`.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- Collection composition is ordered, single-pass where possible, deterministic, and fail-fast.
- Only one enum failure is returned; failures are never accumulated or reordered.
- Enumeration and selector exceptions propagate unchanged.
- Empty input succeeds with an empty materialized value list or a value-less success.
- An Uninitialized element or selector result is invalid and stops enumeration immediately.

## 🎯 Outcome, scope, and non-goals

Consumers can convert ordered Result collections into one Result, traverse input through a
Result-returning selector, and combine value-less command Results without writing manual loops.

Add `Sequence`, `Traverse`, and `Combine`. Do not add failure accumulation, parallel execution,
asynchronous enumeration, lazy successful output, retry, or nullable-input filtering.

## 🔍 Current behavior and impact boundary

Neither Result arity currently provides collection support. Consumers must choose failure ordering,
materialization, and empty-input behavior independently. This work adds optional Composition
extension methods, focused tests, XML documentation, and examples; it does not change Result representation.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Empty valued Result sequence | Call `Sequence` | Returns success containing an empty read-only list |
| BC-2 | All valued Results succeed | Call `Sequence` | Returns success with values in input order |
| BC-3 | One or more valued Results fail | Call `Sequence` | Stops at and returns the first failure in enumeration order |
| BC-4 | Empty source collection and selector | Call `Traverse` | Returns success containing an empty read-only list; selector is not invoked |
| BC-5 | Source items all select to success | Call `Traverse` | Invokes once per item in order and returns ordered values |
| BC-6 | A source item selects to failure | Call `Traverse` | Stops after that item and returns its failure |
| BC-7 | Empty or all-success value-less Results | Call `Combine` | Returns value-less success |
| BC-8 | One or more value-less Results fail | Call `Combine` | Stops at and returns the first failure |
| BC-9 | Source or selector is null, or an element/selector Result is Uninitialized | Invoke the corresponding API | Throws `ArgumentNullException` for arguments or `InvalidOperationException` for an Uninitialized Result |
| BC-10 | Enumeration or selector throws | Invoke the corresponding API | Propagates the same exception instance and stops enumeration |

## 🛠️ Implementation contract

Add public static `ResultEnumerableExtensions` in the `TedToolkit.Results.Composition` package and
`TedToolkit.Results` namespace with exactly
these signatures:

```csharp
Result<IReadOnlyList<TValue>, TFailure> Sequence<TValue, TFailure>(
    this IEnumerable<Result<TValue, TFailure>> results)
    where TFailure : struct, Enum;

Result<IReadOnlyList<TValue>, TFailure> Traverse<TSource, TValue, TFailure>(
    this IEnumerable<TSource> source,
    Func<TSource, Result<TValue, TFailure>> selector)
    where TFailure : struct, Enum;

Result<TFailure> Combine<TFailure>(
    this IEnumerable<Result<TFailure>> results)
    where TFailure : struct, Enum;
```

Materialize successful valued output exactly once into a private `List<TValue>`, then wrap it in
`ReadOnlyCollection<TValue>` before constructing the successful Result. Return that wrapper through
the declared `IReadOnlyList<TValue>` API; never expose or retain a separately accessible mutable list.

Enumerate inputs at most once, preserve order, dispose enumerators, and stop immediately on the first
failure. Validate source and selector arguments before enumeration. Reject a default Uninitialized
Result element or selector result with `InvalidOperationException` including its zero-based input
position.

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1–BC-3 | Unit, `ResultEnumerableExtensionsTests.cs` | Use empty, all-success, and multiple-failure tracked enumerables | Ordered values, first failure, enumeration count | `dotnet run --project tests/TedToolkit.Results.Composition.Tests/TedToolkit.Results.Composition.Tests.csproj -c Release` |
| BC-4–BC-6 | Unit, `ResultTraverseTests.cs` | Traverse tracked inputs and stop at failure | Selector order/count and returned branch | Same command |
| BC-7, BC-8 | Unit, `ResultCombineTests.cs` | Combine value-less Results | Success or first failure | Same command |
| BC-9, BC-10 | Unit, affected files | Supply nulls and throwing enumerables/selectors | Exact exception and early disposal | Same command |
| All | Multi-target build | Build Release | Collection API compiles for every Composition target | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.04–0.08 person-months.
- Confidence: Medium.
- Assumptions: fail-fast behavior satisfies the closed single-failure model.
- Excluded work: accumulated failures, async enumeration, parallel traversal, and streaming output.
- Actual effort on completion: Completed in the current implementation.
- Material variance and reason: The approved DDD boundary moved the extensions from core to the optional Composition package.

## 📋 Completion evidence

The Composition project builds all nine targets with zero warnings. Collection tests cover empty and
ordered success, first-failure short-circuiting, value-less combination, and invalid inputs.

## 🔄 Migration and rollback

The extension API is additive. Rollback removes the helpers without changing Result records or
existing consumers.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Fail-fast hides later independent failures | Bulk validation may need all failures | Keep aggregation out of this model; use an application-specific collection as the success value if needed |
| `ReadOnlyCollection<T>` does not make mutable elements immutable | Consumers may mutate referenced element objects | Guarantee collection-shape immutability only; element mutability remains `TValue`'s contract |
