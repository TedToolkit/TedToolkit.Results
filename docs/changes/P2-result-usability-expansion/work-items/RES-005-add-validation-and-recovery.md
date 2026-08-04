# RES-005: Add Validation and Recovery

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P2
- Rationale: Validation and explicit recovery cover common pipeline branches without expanding the failure model.
- Blocking prerequisite: RES-004.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-usability-expansion/README.md`
- Prerequisites: RES-004.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- Validation failures use the existing closed `TFailure` enum and never create messages or exceptions.
- Recovery is explicit at the call site and executes only for an existing failure.
- Callbacks are eagerly null-validated, execute at most once, and propagate exceptions unchanged.
- A callback returning a Result must return an initialized instance.

## 🎯 Outcome, scope, and non-goals

Consumers can turn an unacceptable success value into a failure and can recover a failure into a
value or another Result while preserving the original branch when no callback should execute.

Add `Ensure`, `Recover`, and `RecoverWith`. Do not add retry loops, logging, side effects, exception
capture, error accumulation, or a fallback that hides which failure was handled.

## 🔍 Current behavior and impact boundary

After RES-003, Map and Bind can transform and chain Results, but common predicate validation requires
a custom binder and recovery requires manual matching. This work affects the generic Result record,
focused tests, XML documentation, and examples only.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Success satisfying a predicate | Call `Ensure` | Preserves the same successful value |
| BC-2 | Success failing a predicate and a fixed reason | Call `Ensure` | Returns failure with the supplied reason |
| BC-3 | Success failing a predicate and a reason factory | Call `Ensure` | Invokes the factory once with the value and returns its reason |
| BC-4 | Existing failure | Call either `Ensure` overload | Invokes neither predicate nor factory and preserves the original failure |
| BC-5 | Failure and a value recovery callback | Call `Recover` | Invokes once and returns success with the recovered value |
| BC-6 | Success and a value recovery callback | Call `Recover` | Does not invoke and preserves the success value |
| BC-7 | Failure and a Result recovery callback | Call `RecoverWith` | Invokes once and returns the callback Result |
| BC-8 | Success and a Result recovery callback | Call `RecoverWith` | Does not invoke and preserves the success value |
| BC-9 | `RecoverWith` callback returns an Uninitialized Result | Recover a failure | Throws `InvalidOperationException` |
| BC-10 | Any required delegate is null | Call the corresponding API | Throws `ArgumentNullException` before branch evaluation |
| BC-11 | Any invoked callback throws | Call the corresponding API | Propagates the same exception instance |
| BC-12 | Source Result is Uninitialized | Call any validation or recovery API | Throws `InvalidOperationException` and invokes no callback |

## 🛠️ Implementation contract

| Boundary or artifact | Required change | Compatibility or invariant |
| --- | --- | --- |
| Fixed validation | `Ensure(Func<TValue, bool> predicate, TFailure failureReason)` | Failure reason is used only when a successful value fails the predicate |
| Computed validation | `Ensure(Func<TValue, bool> predicate, Func<TValue, TFailure> failureFactory)` | Factory executes only after the predicate returns false |
| Value recovery | `Recover(Func<TFailure, TValue> recovery)` | Failure becomes success; existing success is preserved |
| Result recovery | `RecoverWith(Func<TFailure, Result<TValue, TFailure>> recovery)` | Callback may remain failed or succeed, but must return an initialized Result |
| Exception boundary | Eagerly validate delegates and do not catch callback exceptions | Programmer faults are not modeled as enum failures |

Return types are `Result<TValue, TFailure>` for every method. Distinct `Recover` and `RecoverWith`
names avoid overload ambiguity when `TValue` itself is a Result type.

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1–BC-4 | Unit, `ResultEnsureTests.cs` | Exercise both Ensure overloads and both branches | Exact branch, reason, value identity, and callback counts | `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release` |
| BC-5–BC-9 | Unit, `ResultRecoveryTests.cs` | Recover to value, success Result, failure Result, and null | Exact returned branch and exception | Same command |
| BC-10–BC-12 | Unit, affected files | Pass null/throwing callbacks or an Uninitialized source | Exact exception behavior | Same command |
| All | Multi-target build | Build Release | All overloads compile without ambiguity on every target | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.02–0.04 person-months.
- Confidence: High.
- Assumptions: no retry or policy abstraction is required.
- Excluded work: async recovery, logging, exception mapping, and accumulated validation errors.
- Actual effort on completion: Completed in the current implementation.
- Material variance and reason: None.

## 📋 Completion evidence

Core tests cover Ensure, Recover, and RecoverWith for both branches, callback selection, null
callbacks, and Uninitialized values.

## 🔄 Migration and rollback

The API is additive and requires no migration. Remove the new methods and tests to roll back before
publication.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Recovery can hide failures if used indiscriminately | Domain failures may be unintentionally discarded | Examples must show explicit handling of the supplied failure reason |
| Ensure can grow into a validation framework | The core API could lose focus | Keep one failure per Result and exclude aggregation and message construction |
