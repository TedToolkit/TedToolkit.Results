# RES-003: Add Exhaustive Branch Consumption

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P2
- Rationale: Branch-safe consumption removes repeated access checks and makes switch expressions practical.
- Blocking prerequisite: RES-011.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-usability-expansion/README.md`
- Prerequisites: RES-011.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- Preserve `where TFailure : struct, Enum` and the existing success/failure invariant.
- Preserve the explicit Uninitialized state; every new branch-dependent API rejects it.
- Preserve every existing constructor, conversion, property, equality, exception, and formatting behavior.
- The branch discriminator, not a default inactive payload, determines which branch is active.
- Callback exceptions propagate unchanged; the Result type does not catch or convert them.
- Public APIs and runtime argument messages are English and compile for every current core target.

## 🎯 Outcome, scope, and non-goals

Consumers can use an initialized Result directly in positive or negated Boolean conditions and can
inspect or exhaustively project either branch without reading an inactive throwing property. Add
conditional operators, `IsFailure`, `Match`, `TryGetValue`, `TryGetFailure`, and three-component
`Deconstruct` to `Result<TValue, TFailure>`.

Do not add an implicit conversion to `bool`, `operator &`, `operator |`, asynchronous callbacks,
side-effect-only `Switch`, arbitrary failures, other new implicit conversions, or changes to the
existing property getters.

## 🔍 Current behavior and impact boundary

`src/TedToolkit.Results/Result{TValue,TFailure}.cs` exposes `IsSuccess`, `Value`, and
`FailureReason`. Consumers must manually test `IsSuccess`; reading the inactive property throws.
No current API supports `if (result)`, `if (!result)`, a positional pattern, or projection of both
branches into one output type.

This work affects only the generic Result public API, its focused tests, XML documentation, and
package examples.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Any success | Read `IsFailure` | Returns `false` |
| BC-2 | Any failure | Read `IsFailure` | Returns `true` |
| BC-3 | Success and two non-null Match callbacks | Call `Match` | Invokes only `onSuccess` once and returns its value |
| BC-4 | Failure and two non-null Match callbacks | Call `Match` | Invokes only `onFailure` once and returns its value |
| BC-5 | Either callback is null | Call `Match` on either branch | Throws `ArgumentNullException` before invoking a callback |
| BC-6 | Success, including a nullable success value | Call `TryGetValue` | Returns `true` and outputs the contained value |
| BC-7 | Failure | Call `TryGetValue` | Returns `false` and outputs `default` |
| BC-8 | Failure | Call `TryGetFailure` | Returns `true` and outputs the contained failure enum |
| BC-9 | Success | Call `TryGetFailure` | Returns `false` and outputs `default(TFailure)` |
| BC-10 | Success | Deconstruct into three values | Outputs `(true, value, default(TFailure))` |
| BC-11 | Failure | Deconstruct into three values | Outputs `(false, default(TValue), failureReason)` |
| BC-12 | Either branch | Use a positional switch with true and false arms | The matching arm observes only the active payload and the compiler accepts the public deconstruction pattern |
| BC-13 | Uninitialized Result | Read `IsFailure`, Match, use either Try method, or Deconstruct | Throws `InvalidOperationException` and invokes no callback |
| BC-14 | Successful initialized Result | Evaluate `if (result)` and `if (!result)` | Positive condition is true and negated condition is false |
| BC-15 | Failed initialized Result | Evaluate `if (result)` and `if (!result)` | Positive condition is false and negated condition is true |
| BC-16 | Uninitialized Result | Evaluate either conditional form | Throws `InvalidOperationException` and executes neither conditional body |
| BC-17 | Public Result API | Inspect its declared operators | Contains `true`, `false`, and `!`; contains no implicit bool, `&`, or `|` operator |

## 🛠️ Implementation contract

| Boundary or artifact | Required change | Compatibility or invariant |
| --- | --- | --- |
| Branch alias | Add `public bool IsFailure => !IsSuccess;` | Never stores independent state |
| Positive condition | Add `public static bool operator true(Result<TValue, TFailure> result)` | Returns `result.IsSuccess`; Uninitialized throws |
| Required complement | Add `public static bool operator false(Result<TValue, TFailure> result)` | Returns `result.IsFailure`; paired with `operator true` as required by C# |
| Negated condition | Add `public static bool operator !(Result<TValue, TFailure> result)` | Returns `result.IsFailure`; Uninitialized throws |
| Projection | Add `public TResult Match<TResult>(Func<TValue, TResult> onSuccess, Func<TFailure, TResult> onFailure)` | Eagerly validates both delegates and invokes exactly one |
| Safe value access | Add `public bool TryGetValue(out TValue? value)` with framework-compatible nullable metadata | Failure returns `default`; nullable success remains valid |
| Safe failure access | Add `public bool TryGetFailure(out TFailure failureReason)` | Success returns `default(TFailure)`; Boolean return disambiguates it from a real zero-valued enum |
| Positional pattern | Add `public void Deconstruct(out bool isSuccess, out TValue? value, out TFailure failureReason)` | `isSuccess` is the only discriminator; inactive payload is `default` |
| Initialization guard | Validate `IsInitialized` before every new branch-dependent operation | Default Result never masquerades as either branch |
| Documentation | Show `result switch { (true, var value, _) => ..., (false, _, var failure) => ... }` | Do not imply that payload defaults identify the branch |

If nullable annotations require conditional compilation or internal compatibility attributes on
legacy targets, they must not change the runtime signature between target frameworks.

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1, BC-2 | Unit, `ResultBranchConsumptionTests.cs` | Construct each branch and read aliases | Complementary Boolean state | `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release` |
| BC-3–BC-5 | Unit, same file | Use counting delegates, null delegates, and both branches | Return value, invocation count, and exception type | Same command |
| BC-6–BC-9 | Unit, same file | Call both Try methods for each branch | Boolean and exact out value | Same command |
| BC-10–BC-13 | Unit plus compile coverage, `ResultDeconstructionTests.cs` | Deconstruct and evaluate a positional switch, including default Result | Exact tuple components, selected arm, and default rejection | Same command |
| BC-14–BC-16 | Unit, `ResultConditionalOperatorTests.cs` | Execute positive and negated conditions for all three states | Exact body execution and default-state exception | Same command |
| BC-17 | API-shape unit, `ResultConditionalOperatorTests.cs` | Inspect declared public operators through reflection | Required three operators exist and excluded operators do not | Same command |
| All | Multi-target build | Build the solution in Release | No target-specific signature or analyzer failures | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.02–0.04 person-months.
- Confidence: High.
- Assumptions: nullable metadata can be expressed consistently across the existing target matrix.
- Excluded work: implicit bool conversion, logical Result combination, asynchronous matching,
  `Switch`, LINQ patterns, and constructor changes.
- Actual effort on completion: Completed in the current implementation.
- Material variance and reason: None.

## 📋 Completion evidence

Core tests cover Match, TryGet methods, deconstruction, conditional operators, inactive payloads,
null callbacks, and Uninitialized rejection.

## 🔄 Migration and rollback

The API is additive. Rollback removes the eight new members and their documentation before release.
After publication, removal requires a major-version compatibility decision.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| `default(TValue)` may be a valid success value | Payload inspection cannot determine the branch | Documentation and tests must make `isSuccess` authoritative |
| Nullable annotations differ on legacy targets | Consumer flow analysis may become inconsistent | Verify compiled reference metadata for representative modern and legacy targets |
| `if (result)` resembles a two-state Boolean although Result has an Uninitialized state | Consumers may assume default is failure | Both `operator true` and `operator !` delegate to validated branch properties and reject default |
| An implicit bool conversion would apply beyond conditional syntax and conventionally should not throw | Overload resolution and ordinary assignments could gain surprising behavior | Use `true`, `false`, and `!` operators only |
