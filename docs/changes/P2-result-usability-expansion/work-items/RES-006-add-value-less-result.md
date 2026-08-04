# RES-006: Add Value-less Result

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P2
- Rationale: Commands need explicit success and failure without inventing a dummy success value.
- Blocking prerequisite: RES-003.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-usability-expansion/README.md`
- Prerequisites: RES-003.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- The new public type is `readonly record struct Result<TFailure>` with `where TFailure : struct, Enum`.
- It models successful completion without a success payload or one enum failure reason.
- It follows the existing explicit Uninitialized state, record equality, branch exception,
  conversion, and diagnostic conventions.
- Every active failure reason follows RES-011 declared-value validation.
- It does not introduce a public `Unit` type or permit arbitrary failure payloads.

## 🎯 Outcome, scope, and non-goals

Command-like operations can return a Result without using `bool`, `object`, or a synthetic Unit value.
Add `Result<TFailure>`, corresponding non-generic factories, Boolean conditional operators, branch
consumption, equality, formatting, and basic chaining into valued or value-less Results.

Do not redesign `Result<TValue, TFailure>`, add success metadata, introduce a Unit type, or implicitly
convert between the two Result arities.

## 🔍 Current behavior and impact boundary

The package only exposes `Result<TValue, TFailure>`, so a command must choose a meaningless
`TValue`. This affects domain modeling and prevents a natural `Combine` operation for commands.
The work adds one core public type and extends the static factory, tests, XML documentation, and
package README.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | No input | Factory-create success | `IsInitialized` and `IsSuccess` are true and `IsFailure` is false |
| BC-2 | Enum reason | Construct, factory-create, or implicitly convert failure | `IsFailure` is true and `FailureReason` returns the exact reason |
| BC-3 | Success | Read `FailureReason` | Throws `InvalidOperationException` |
| BC-4 | Equal or unequal branches and reasons | Compare records | Equality follows branch and failure value |
| BC-5 | Success or failure | Call `Match` | Invokes exactly the active callback and returns its output |
| BC-6 | Success or failure | Call `TryGetFailure` | Follows the valued Result contract |
| BC-7 | Success or failure | Deconstruct | Outputs `(true, default)` or `(false, failureReason)` |
| BC-8 | Success and a value-less binder | Call `Bind` | Invokes and returns the bound Result |
| BC-9 | Success and a valued binder | Call `Bind<TValue>` | Invokes and returns the valued Result |
| BC-10 | Failure and either binder | Call `Bind` | Does not invoke and propagates the same failure |
| BC-11 | Success or failure | Call `ToString` | Returns `✅` for success or `❌ <reason>` for failure |
| BC-12 | `default(Result<TFailure>)` | Query or consume it | `IsInitialized` returns false; every branch-dependent API and `ToString` throws `InvalidOperationException` |
| BC-13 | Equal defaults or default versus initialized | Compare records | Defaults compare equal to one another and unequal to every initialized Result |
| BC-14 | Initialized success or failure | Evaluate `if (result)` and `if (!result)` | Follows `IsSuccess` and `IsFailure` exactly |
| BC-15 | Uninitialized value-less Result | Evaluate either conditional form | Throws `InvalidOperationException` |

## 🛠️ Implementation contract

| Boundary or artifact | Required change | Compatibility or invariant |
| --- | --- | --- |
| Public type | Add `public readonly record struct Result<TFailure> where TFailure : struct, Enum` | Explicit Uninitialized, Success, and Failure states; success has no payload |
| Construction | Public failure constructor and non-public success-state construction used by the factory | `new Result<TFailure>()` remains the Uninitialized default and never creates success |
| Static factories | Add `Result.Succeed<TFailure>()` and `Result.Fail<TFailure>(TFailure failureReason)` | Named factories are the canonical documented creation path |
| Conversion | Add implicit conversion from `TFailure` to `Result<TFailure>` | No reverse or cross-arity implicit conversion |
| Consumption | Add `IsInitialized`, `IsSuccess`, `IsFailure`, throwing `FailureReason`, `Match`, `TryGetFailure`, and two-component `Deconstruct` | Boolean discriminator is authoritative only for initialized values |
| Conditions | Add paired `operator true`/`operator false` and `operator !`; do not add implicit bool, `&`, or `|` | Matches the valued Result contract and rejects Uninitialized |
| Composition | Add value-less `Bind(Func<Result<TFailure>>)` and valued `Bind<TValue>(Func<Result<TValue, TFailure>>)` | Binder runs only on success and must return an initialized Result |
| Diagnostics | Format success as `✅` and failure as `❌ <enum text>` | Diagnostic only, not serialization or UI text |

The new one-arity type is selected instead of `UnitResult<TFailure>` to keep one Result vocabulary,
and instead of a public Unit value to avoid forcing a placeholder into consumer APIs.

The complete public surface added by this work package is:

```csharp
public readonly record struct Result<TFailure>
    where TFailure : struct, Enum
{
    public Result(TFailure failureReason);

    public bool IsInitialized { get; }
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public TFailure FailureReason { get; }

    public TResult Match<TResult>(
        Func<TResult> onSuccess,
        Func<TFailure, TResult> onFailure);

    public bool TryGetFailure(out TFailure failureReason);
    public void Deconstruct(out bool isSuccess, out TFailure failureReason);

    public Result<TFailure> Bind(Func<Result<TFailure>> binder);
    public Result<TValue, TFailure> Bind<TValue>(
        Func<Result<TValue, TFailure>> binder);

    public static implicit operator Result<TFailure>(TFailure failureReason);
    public static bool operator true(Result<TFailure> result);
    public static bool operator false(Result<TFailure> result);
    public static bool operator !(Result<TFailure> result);

    public override string ToString();
}

public static class Result
{
    public static Result<TFailure> Succeed<TFailure>()
        where TFailure : struct, Enum;

    public static Result<TFailure> Fail<TFailure>(TFailure failureReason)
        where TFailure : struct, Enum;
}
```

The declarations above extend the existing non-generic `Result` factory; they do not require a new
type declaration or source split. Internally, `Result<TFailure>` exposes an
`internal static CreateSuccess()` factory to the assembly; the public parameterless struct value
remains Uninitialized. No public constructor or conversion creates value-less success.

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1–BC-3, BC-12 | Unit, `ValueLessResultConstructionTests.cs` | Factory-create both branches and inspect default | State, reason, and exception | `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release` |
| BC-4, BC-13 | Unit, `ValueLessResultEqualityTests.cs` | Compare representative and default records | Exact equality semantics | Same command |
| BC-5–BC-7 | Unit, `ValueLessResultConsumptionTests.cs` | Match, Try, deconstruct | Callback counts and outputs | Same command |
| BC-14, BC-15 | Unit, `ValueLessResultConditionalOperatorTests.cs` | Evaluate both conditional forms for every state | Exact body execution and default rejection | Same command |
| BC-8–BC-10 | Unit, `ValueLessResultBindTests.cs` | Bind each branch into both arities | Exact branch propagation and invocation counts | Same command |
| BC-11 | Unit, `ValueLessResultFormattingTests.cs` | Format both branches | Exact diagnostic text | Same command |
| All | Multi-target build | Build and pack Release | New arity and factories appear on all core targets | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.04–0.07 person-months.
- Confidence: Medium.
- Assumptions: the one-arity naming is acceptable alongside the non-generic static `Result` factory.
- Excluded work: arbitrary failure types, Unit, cross-arity conversions, and async composition.
- Actual effort on completion: Completed in the current implementation.
- Material variance and reason: None.

## 📋 Completion evidence

Core tests cover value-less construction, conversion, consumption, binding, formatting, equality,
conditional operators, and Uninitialized behavior across the approved API.

## 🔄 Migration and rollback

The type and factory overloads are additive. Rollback removes the new arity before publication.
After consumers publish signatures containing `Result<TFailure>`, removal is breaking.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Many .NET Result libraries use one generic parameter for the success value | Consumers may initially misread `Result<TFailure>` | Make the enum constraint and command examples prominent in the package README |
| A parameterless struct expression produces Uninitialized rather than success | Consumers may assume `new Result<TFailure>()` succeeds | Make `Result.Succeed<TFailure>()` the only public success creation path and document default rejection |
