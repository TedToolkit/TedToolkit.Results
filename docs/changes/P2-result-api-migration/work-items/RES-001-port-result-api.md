# RES-001: Port Result API

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P2
- Rationale: The package needs its first planned reusable behavior without an outage or deadline.
- Blocking prerequisite: none.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-api-migration/README.md`
- Prerequisites: none.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- The public namespace is `TedToolkit.Results`.
- `TFailure` is restricted to enum value types.
- A result contains exactly one active branch; success may contain a nullable value.
- The API must compile on every target framework configured by `AlmostAllFrameworks.props`.
- Authored source, tests, messages, identifiers, comments, XML documentation, examples, and
  repository documentation must not contain Chinese text.
- Locale-specific presentation is outside the package; consumer values remain ordinary Unicode data.
- Tests use TUnit, awaited assertions, English XML summaries, and the documented `dotnet run` entry point.

## 🎯 Outcome, scope, and non-goals

Deliver the approved generic result record and non-generic factory with branch access, conversions,
record equality, diagnostic formatting, documentation, and deterministic unit coverage.

Do not add functional combinators, async helpers, serialization contracts, localization resources,
new dependencies, or Roboticplus consumer changes.

## 🔍 Current behavior and impact boundary

The new package is currently an empty scaffold with one assembly identity test. The source behavior
exists as two uncommitted files and nine tests under
`C:/Work/Codes/Roboticplus.BevelCutting`. This work affects only the production package, its test
project, contributor guidance, and package documentation.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | A success value, including nullable values | Construct a generic result | `IsSuccess` is true, `Value` returns the value, and `FailureReason` throws |
| BC-2 | An enum failure value | Construct a generic result | `IsSuccess` is false, `FailureReason` returns the enum, and `Value` throws |
| BC-3 | A success value or failure enum | Apply implicit conversion | The corresponding result branch is created |
| BC-4 | A success, failure, or null result reference | Apply explicit value conversion | Success returns its value; failure throws `InvalidOperationException`; null throws `ArgumentNullException` |
| BC-5 | A success value or failure enum | Use `Result.Succeed` or `Result.Fail` | The corresponding valid result is returned |
| BC-6 | Success with text, null, or a value whose `ToString` returns null | Call `ToString` | Returns `✅ <text>` and uses `null` as the fallback |
| BC-7 | A failure enum | Call `ToString` | Returns `❌ <enum text>` |
| BC-8 | Results with equal or unequal branch contents | Compare records | Equality follows branch and contained value semantics |
| BC-9 | The complete package | Build Release | All configured target frameworks compile and package successfully |

## 🛠️ Implementation contract

| Boundary or artifact | Required change | Compatibility or invariant |
| --- | --- | --- |
| `Result<TValue, TFailure>` | Add sealed record, constructors, branch properties, conversions, and formatting | Exactly one active branch; nullable success is valid |
| Non-generic `Result` | Add typed success and failure factories | Factory behavior matches constructors |
| Runtime errors | Use English `InvalidOperationException` messages and portable explicit null checks | Exception types match the source contract on every target |
| Tests | Replace the scaffold-only test with focused behavior tests | Fast, deterministic, parallel-safe TUnit tests |
| Documentation | Replace scaffold wording with installation, behavior, and usage guidance | English-only authored content |

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1, BC-2, BC-8 | Unit, `ResultConstructionTests.cs` | Construct success and failure results | Branch state, access, exceptions, and equality | `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release` |
| BC-3, BC-4 | Unit, `ResultConversionTests.cs` | Apply implicit and explicit conversions | Branch state, value, and exception types | Same focused test command |
| BC-5 | Unit, `ResultFactoryTests.cs` | Invoke both factories | Correct branch and payload | Same focused test command |
| BC-6, BC-7 | Unit, `ResultFormattingTests.cs` | Format each branch and null fallback | Exact diagnostic text | Same focused test command |
| BC-9 | Build/package | Build solution in Release | Zero warnings/errors and generated package | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.03–0.06 person-months.
- Confidence: High.
- Assumptions: existing project structure, target list, and TUnit setup remain valid.
- Excluded work: downstream migration, publishing, and additional result APIs.
- Actual effort on completion: 0.03 person-months.
- Material variance and reason: none; implementation remained within the approved range.

## 📋 Completion evidence

- `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release`:
  19 tests passed, 0 failed.
- `dotnet build TedToolkit.Results.slnx -c Release`: succeeded for all nine target frameworks with
  0 warnings and 0 errors; generated `TedToolkit.Results.1.0.0.nupkg`.
- `dotnet build Build/Build.csproj -c Release`: succeeded with 0 warnings and 0 errors.
- Repository-authored file scan with `rg --pcre2 '\p{Han}'`: no Han characters found.
- Package inspection confirmed the package README, XML documentation, and assemblies for all nine
  configured target frameworks.
- The initial multi-target build reported enum boxing in diagnostic formatting on legacy targets.
  Calling the enum's `ToString()` explicitly preserved output and removed the diagnostic.

## 🔄 Migration and rollback

This adds the first public API to a new package, so there is no existing TedToolkit binary contract
to migrate. Rollback removes the new API, focused tests, and updated package documentation while
leaving the scaffold intact. Roboticplus consumers are unchanged.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Record equality is part of the public semantics | Future class-shape changes can become breaking | Protect equality with tests |
| Diagnostic Emoji output is exact behavior | Consumers might misuse it as a protocol | Document it as diagnostics only |
