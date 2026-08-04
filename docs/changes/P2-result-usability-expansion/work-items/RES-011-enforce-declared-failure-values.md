# RES-011: Enforce Declared Failure Values

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P2
- Rationale: The failure set is not actually closed when arbitrary numeric enum values can enter an active failure branch.
- Blocking prerequisite: completed RES-002 value-type migration.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-usability-expansion/README.md`
- Prerequisites: completed `P2-result-value-type-migration` and the active package architecture record.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- `TFailure` remains `where TFailure : struct, Enum`.
- Every active failure value must satisfy `Enum.IsDefined(typeof(TFailure), failureReason)`.
- A zero value is valid only when the enum declares a member with underlying value zero.
- A named composite value is valid; an unnamed `[Flags]` combination is invalid.
- Inactive failure storage may contain `default(TFailure)` but is never exposed as an active reason.
- Validation must compile on every core target without a new product dependency.

## 🎯 Outcome, scope, and non-goals

Every Result failure that can be observed by a consumer is one of the failure enum's declared values,
so consumers can enumerate the runtime failure set without an undocumented numeric remainder.

Centralize validation and apply it to valued and value-less constructors, factories, implicit
failure conversions, failure mapping, validation failure factories, recovery Results, collection
propagation, and integration deserialization boundaries as those APIs are introduced.

Do not validate inactive storage, require a zero member, prohibit duplicate enum names or values,
prohibit `[Flags]` enums, localize validation messages, or convert invalid values into Result
failures.

## 🔍 Current behavior and impact boundary

`Result<TValue, TFailure>` currently accepts any value whose static type is `TFailure`, including
casts such as `(TFailure)999`. C# also permits zero as an enum default even when the enum declares no
zero member. The generic enum constraint therefore closes the failure type but not its runtime value
set.

This work affects failure construction in the core package, focused tests, XML documentation, and
every later work package that can introduce a new failure value. It does not change success values or
the Uninitialized state.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Declared enum member | Construct, factory-create, or implicitly convert a valued failure | Creates failure with the exact value |
| BC-2 | Undefined numeric enum value | Use any valued failure creation path | Throws `ArgumentOutOfRangeException` naming `failureReason` |
| BC-3 | Enum declares zero | Create failure with zero | Creates a valid failure |
| BC-4 | Enum does not declare zero | Create failure with `(TFailure)0` | Throws `ArgumentOutOfRangeException` |
| BC-5 | Named composite member of a `[Flags]` enum | Create failure | Creates a valid failure |
| BC-6 | Unnamed combination of declared flags | Create failure | Throws `ArgumentOutOfRangeException` |
| BC-7 | Two enum names share one underlying value | Create failure with that value | Creates a valid failure because the underlying value is declared |
| BC-8 | Success or Uninitialized Result | Construct or inspect without an active failure | Performs no failure-value validation and preserves existing state behavior |
| BC-9 | Later callback produces an undefined failure | Map, Ensure, RecoverWith, or Bind through the applicable API | Rejects the invalid callback Result or failure value at its creation boundary |
| BC-10 | JSON contains an undefined numeric or string failure value | Deserialize through RES-009 | Throws `JsonException` with the underlying validation failure as context |

## 🛠️ Implementation contract

Add one internal generic validation boundary used by every core failure constructor or factory:

```csharp
internal static void ValidateFailureReason<TFailure>(TFailure failureReason)
    where TFailure : struct, Enum;
```

Use the portable `Enum.IsDefined(typeof(TFailure), failureReason)` API. When it returns false, throw
`ArgumentOutOfRangeException` with parameter name `failureReason`, the rejected value as
`ActualValue`, and one stable English message. Constructors validate before committing the Failure
state. Public factories and implicit conversions delegate to the validated constructor rather than
duplicating validation.

All later APIs that create failures must use the same validated construction path. APIs that merely
propagate an already initialized failure do not revalidate it. RES-009 translates invalid external
failure data into `JsonException` rather than exposing `ArgumentOutOfRangeException` as its public
wire-format error.

Do not use the newer generic `Enum.IsDefined<TEnum>` overload because it is unavailable on legacy
targets. A future cached validator is allowed only if it preserves identical behavior and does not
introduce a public API or dependency.

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1–BC-4 | Unit, `ResultFailureValidationTests.cs` | Exercise valued constructors, factories, conversions, zero-member and no-zero enums | Exact branch or `ArgumentOutOfRangeException` details | `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release` |
| BC-5–BC-7 | Unit, same file | Use named composite, unnamed flags, and duplicate-value enums | Exact accept/reject behavior | Same command |
| BC-8 | Regression, construction tests | Exercise success and default Result | No new validation and preserved state behavior | Same command |
| BC-9 | Unit, each introducing work package | Return undefined values from callbacks | Rejected at the single creation boundary | Same command |
| BC-10 | Contract, JSON integration tests | Deserialize numeric and string undefined failures | `JsonException` and no Result value | RES-009 integration test command |
| All | Multi-target build | Build Release | Portable validation compiles for every core target | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.02–0.04 person-months.
- Confidence: High.
- Assumptions: per-failure construction validation is acceptable without a benchmark gate.
- Excluded work: cached optimization, analyzer enforcement, enum source generation, and consumer migration.
- Actual effort on completion: Completed in the current implementation.
- Material variance and reason: None.

## 📋 Completion evidence

Focused construction tests verify declared values and reject undefined numeric and flags values.
The core package builds all nine targets with zero warnings.

## 🔄 Migration and rollback

Consumers that intentionally pass undefined numeric values will receive
`ArgumentOutOfRangeException` and must declare those values as enum members or stop using them. Before
publication, rollback removes centralized validation. After publication, relaxing or changing the
accepted value set is a public behavioral compatibility decision.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| `Enum.IsDefined` boxes on legacy targets | Very hot failure construction could allocate | Accept initially; optimize behind the internal boundary only from measured evidence |
| Adding a new enum member changes the accepted runtime value set | Version skew can affect serialized failures | Treat consumer enum versioning as part of its application contract |
