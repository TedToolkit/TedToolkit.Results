# RES-004: Add Synchronous Composition

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P2
- Rationale: Transforming and chaining successful operations is the primary usability improvement over manual branching.
- Blocking prerequisite: RES-003.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-usability-expansion/README.md`
- Prerequisites: RES-003.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- All source and destination failure types remain enum value types.
- A callback executes only on its active branch and at most once.
- A propagated failure preserves the exact enum value without calling the success callback.
- Callback exceptions propagate unchanged and are never converted to failures.
- Existing construction, equality, conversion, and formatting contracts remain unchanged.
- An Uninitialized source or Result returned by a callback is rejected with `InvalidOperationException`.

## 🎯 Outcome, scope, and non-goals

Consumers can transform a success value, chain a Result-returning operation, or explicitly translate
one closed failure enum into another without manual `IsSuccess` branching.

Add `Map`, `Bind`, and `MapFailure` to `Result<TValue, TFailure>`. Do not add LINQ aliases,
side-effect helpers, asynchronous callbacks, exception capture, or multi-error accumulation.

## 🔍 Current behavior and impact boundary

The current generic Result exposes branch state and payload access only. Every multi-step operation
must manually unwrap a value and recreate a failure. This work affects the generic Result API,
focused composition tests, XML documentation, and pipeline examples.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Success and a mapper | Call `Map` | Invokes the mapper once and returns success with the mapped value |
| BC-2 | Failure and a mapper | Call `Map` | Does not invoke the mapper and returns the same failure value |
| BC-3 | Nullable mapped value | Call `Map` | Returns a valid success containing null |
| BC-4 | Success and a binder returning success | Call `Bind` | Invokes once and returns the bound success |
| BC-5 | Success and a binder returning failure | Call `Bind` | Invokes once and returns the bound failure |
| BC-6 | Failure and a binder | Call `Bind` | Does not invoke the binder and propagates the original failure |
| BC-7 | Binder returns an Uninitialized Result | Call `Bind` on success | Throws `InvalidOperationException` identifying an invalid callback result |
| BC-8 | Failure and a failure mapper | Call `MapFailure` | Invokes once and returns failure with the mapped enum |
| BC-9 | Success and a failure mapper | Call `MapFailure` | Does not invoke the mapper and preserves the success value |
| BC-10 | Any callback argument is null | Invoke the corresponding method | Throws `ArgumentNullException` before branch evaluation |
| BC-11 | An invoked callback throws | Invoke the corresponding method | Propagates the same exception instance |
| BC-12 | Source Result is Uninitialized | Invoke any composition method | Throws `InvalidOperationException` and invokes no callback |

## 🛠️ Implementation contract

| Boundary or artifact | Required change | Compatibility or invariant |
| --- | --- | --- |
| Success transformation | `public Result<TNext, TFailure> Map<TNext>(Func<TValue, TNext> mapper)` | Mapper executes only on success; nullable `TNext` is allowed when declared nullable |
| Result chaining | `public Result<TNext, TFailure> Bind<TNext>(Func<TValue, Result<TNext, TFailure>> binder)` | Binder executes only on success and must return an initialized Result |
| Failure transformation | `public Result<TValue, TNextFailure> MapFailure<TNextFailure>(Func<TFailure, TNextFailure> mapper) where TNextFailure : struct, Enum` | Mapper executes only on failure; success value identity is preserved |
| Argument validation | Validate delegate arguments eagerly | Behavior does not depend on the active branch |
| Runtime behavior | Do not catch callback exceptions | Expected failures remain values; defects remain exceptions |
| Initialization guard | Reject an Uninitialized source or bound Result | Default values never silently enter a pipeline |

Factories, rather than implicit conversions, should construct propagated branches internally so the
implementation stays unambiguous when `TValue` and `TFailure` happen to be the same enum type.

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1–BC-3 | Unit, `ResultMapTests.cs` | Map success, failure, and nullable value | Branch, payload, and callback count | `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release` |
| BC-4–BC-7 | Unit, `ResultBindTests.cs` | Bind all source/destination branch combinations and null | Exact returned branch and exception | Same command |
| BC-8, BC-9 | Unit, `ResultMapFailureTests.cs` | Translate one test enum into another | Exact mapped or preserved payload | Same command |
| BC-10–BC-12 | Unit, affected files | Pass null and throwing callbacks or a default Result | Exact exception type or identity | Same command |
| All | Multi-target build | Build Release | All generic signatures compile for every core target | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.03–0.06 person-months.
- Confidence: High.
- Assumptions: the methods live on the generic Result record and introduce no extension dependency.
- Excluded work: validation, recovery, async, collection, LINQ, and side-effect APIs.
- Actual effort on completion: Completed in the current implementation.
- Material variance and reason: None.

## 📋 Completion evidence

Core tests cover Map, Bind, and MapFailure for both branches, callback selection, exception
propagation, null callbacks, and Uninitialized values.

## 🔄 Migration and rollback

The API is additive. Rollback removes the new methods before publication. Existing consumers are not
required to migrate.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Delegate overload inference can be sensitive when value and failure types match | Some calls may need explicit type arguments | Compile an enum-as-value consumer case before implementation approval |
| Returning an Uninitialized Result from a binder breaks an active pipeline | A delayed default-state failure would be harder to diagnose | Fail at the Bind boundary with `InvalidOperationException` |
