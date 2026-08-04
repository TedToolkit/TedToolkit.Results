# RES-002: Migrate Result to an explicit-state value type

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P2
- Rationale: Establish the intended public shape before package adoption.
- Blocking prerequisite: none; ADR-0001 is Accepted.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-value-type-migration/README.md`
- Prerequisites: Accepted `docs/adr/ADR-0001-result-value-representation/README.md`.
- Applicable principles: None.

## 🧩 Explicit governing constraints

- Use `readonly record struct` with a typed internal enum for explicit uninitialized, success, and failure states; keep the enum's normal `int` underlying representation.
- The default value is uninitialized and never appears successful or failed.
- Expose a non-throwing initialization query; branch-dependent APIs reject uninitialized values.
- Preserve all initialized Result behavior, including nullable success values and record equality.
- Remove the reference-null conversion contract and do not add product dependencies.
- Build every target configured by `AlmostAllFrameworks.props`.

## 🎯 Outcome, scope, and non-goals

Deliver an immutable value Result with safe default semantics and the accepted typed-enum state width.
Update directly affected tests and package documentation. Do not add new Result operations,
serialization contracts, or retain the ADR benchmark as a default CI workload.

## 🔍 Current behavior and impact boundary

`Result<TValue, TFailure>` is currently a sealed record class. Every non-null instance is initialized,
and explicit value conversion checks for a null Result reference. The change affects the generic
public type, its factories and conversions, equality shape, documentation, tests, and binary
compatibility. The non-generic factory signatures remain source-compatible for ordinary calls.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Constructed success, including nullable value | Read state and branches | Initialized and successful; Value returns payload; FailureReason throws |
| BC-2 | Constructed failure enum | Read state and branches | Initialized and failed; FailureReason returns reason; Value throws |
| BC-3 | Default Result | Read `IsInitialized` | Returns false without throwing |
| BC-4 | Default Result | Read `IsSuccess`, Value, or FailureReason | Throws `InvalidOperationException` and does not masquerade as a branch |
| BC-5 | Default Result | Convert explicitly or call `ToString` | Throws `InvalidOperationException` |
| BC-6 | Initialized Result | Use conversions, factories, formatting, or equality | Existing observable behavior is preserved |
| BC-7 | Equal or different default/initialized values | Compare records | Equal defaults compare equal; default differs from every initialized result |
| BC-8 | Generic Result type | Inspect and build | It is a readonly value type and all target frameworks compile |

## 🛠️ Implementation contract

| Boundary or artifact | Required change | Compatibility or invariant |
| --- | --- | --- |
| Generic Result declaration | Change to `readonly record struct` | Public binary breaking change is deliberate |
| Internal representation | Add a typed three-state `ResultState` enum | Zero is Uninitialized; one is Success; two is Failure; Result code never compares raw integers |
| Initialization API | Add `IsInitialized` | Safe on default and initialized values |
| Branch APIs | Validate explicit state | Existing wrong-branch messages remain; uninitialized uses a distinct English message |
| Explicit value conversion | Remove reference-null checks and delegate to Value | Default conversion throws `InvalidOperationException` |
| Documentation and tests | Describe and verify value/default semantics | No implementation-detail assertion of private state width |

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1, BC-2, BC-3, BC-4 | Unit, `ResultConstructionTests.cs` | Construct or default Result and read APIs | Exact state, values, and exception types | `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release` |
| BC-5, BC-6 | Unit, conversion and formatting tests | Convert and format Result values | Preserved initialized behavior and rejected default | Same command |
| BC-7 | Unit, `ResultEqualityTests.cs` | Compare default and initialized values | Record value equality follows state and payload | Same command |
| BC-8 | Contract/build | Reflect type shape and build solution | Readonly value type; zero warnings/errors for all TFMs | Same command, then `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.02–0.04 person-months.
- Confidence: High.
- Assumptions: No downstream migration; the int-backed enum's performance is accepted unless a review trigger produces contrary evidence.
- Excluded work: Publishing, consumer migration, and maintained continuous benchmarks.
- Actual effort on completion: 0.02 person-months.
- Material variance and reason: none; implementation remained within the approved range.

## 📋 Completion evidence

- `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release`:
  83 tests passed, 0 failed in the latest regression run.
- Downstream regression projects passed: Composition 15, SystemTextJson 13, and AspNetCore 11;
  122 tests passed overall with 0 failures.
- `dotnet build TedToolkit.Results.slnx -c Release`: succeeded for all nine target frameworks with
  0 warnings and 0 errors; generated all four package artifacts.
- Reflection coverage confirms the public generic Result is a readonly value type.
- Default-state coverage confirms initialization queries are non-throwing while branch access,
  conversion, and formatting reject uninitialized values.
- ADR-0001 retains BenchmarkDotNet 0.15.8 Markdown, full JSON, disassembly, and logs for net48,
  net8.0, and net10.0, plus the representative closed-generic size report.

## 🔄 Migration and rollback

Consumers must recompile and replace null Result-reference handling with nullable value wrappers or
explicit default-state handling. Rollback restores the sealed record declaration, null conversion
guard, and previous documentation and tests.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Large value payload copies | Some call patterns may be slower than the record class | Preserve benchmark evidence and document scope |
| Default values enter through arrays, fields, and `out` paths | Unchecked consumers may encounter new exceptions | Expose `IsInitialized` and document the contract |
