# ADR-0001: Result value representation

- Status: Accepted
- Date: 2026-08-03
- Decision owner: TedToolkit maintainers
- Decision scope: The in-memory representation of `Result<TValue, TFailure>` across all supported target frameworks.
- Applicable principles: None.
- Related change or work packages: `docs/changes/P2-result-value-type-migration/`
- Supersedes: None
- Superseded by: None

## 📌 Decision at a glance

Use a typed internal three-state enum with the normal `int` underlying representation for the
allocation-free `readonly record struct`.

## 🧭 Context and decision question

The current `sealed record` gives every non-null instance a valid success or failure branch, but
normally allocates a separate object. A value representation removes that allocation but introduces
the unavoidable all-zero `default(Result<TValue, TFailure>)` value. The approved representation uses
an explicit `Uninitialized`, `Success`, or `Failure` state so default cannot masquerade as failure.

The remaining representation choice was whether the private three-state enum should use a `byte` or
the normal `int` underlying representation. BenchmarkDotNet 0.15.8 measurements on .NET Framework
4.8.1, .NET 8.0.29, and .NET 10.0.10 show that neither width wins every workload. The normal `int`
representation is selected for the common int-backed failure-enum path and materially smaller .NET
10 generated code, while accepting a narrow layout advantage for byte-backed state.

## 🎯 Decision drivers and constraints

| Type | Driver or constraint | Evidence or source | Priority |
| --- | --- | --- | --- |
| Hard constraint | `default(Result<...>)` is explicitly uninitialized and never masquerades as success or failure | Approved source request | Must |
| Hard constraint | Initialized success, failure, nullable-value, conversion, formatting, and equality behavior remains intact | Existing tests and `RES-001` | Must |
| Hard constraint | The representation compiles for all package target frameworks | `AlmostAllFrameworks.props` | Must |
| Hard constraint | Use a typed state enum with the normal `int` underlying representation | User decisions on 2026-08-03 | Must |
| Decision driver | Avoid a separate Result object allocation in ordinary value flows | BenchmarkDotNet MemoryDiagnoser | High |
| Decision driver | Minimize representative closed-generic structure sizes | `Unsafe.SizeOf<T>()` report | High |
| Decision driver | Avoid a statistically meaningful hot-path throughput regression | BenchmarkDotNet reports | High |
| Decision driver | Understand discriminator load and branch code generation | BenchmarkDotNet DisassemblyDiagnoser | Medium |

## 🔎 Options and evidence

| Option | Evidence and confidence | Meets drivers | Decisive trade-off | Outcome |
| --- | --- | --- | --- | --- |
| Current `sealed record` | Existing implementation; High | Valid instances and compatibility, but separate allocation | Does not meet the approved allocation-free representation | Rejected |
| `readonly record struct` with byte state | Measured on three runtimes; Medium | Explicit three-state model; reduces `Result<int, ByteFailure>` from 12 to 8 bytes | Regresses representative modern-runtime int-enum traversal and emits larger .NET 10 code | Rejected |
| `readonly record struct` with a typed int-backed state enum | Measured on three runtimes; Medium | Explicit three-state model without magic numeric state checks; favors common int-backed enum workloads | Uses 4 more bytes for the narrow int-value/byte-failure layout and does not win every legacy-runtime case | Selected |

## ✅ Decision

Use an internal `ResultState` enum with the normal `int` underlying representation.
`ResultState.Uninitialized` is zero, `ResultState.Success` is one, and `ResultState.Failure` is two.
Result implementations store and compare the enum rather than raw integers. This choice prioritizes
the normal int-backed failure-enum case on supported modern runtimes; it is not a claim that `int`
is universally faster on every runtime or closed generic layout.

## 💡 Why this decision now

Changing a public generic type between class and struct is difficult to reverse after package
publication. The measured class baseline allocated 32 bytes per constructed Result, while both
struct candidates allocated no managed memory in the construction benchmark. For the discriminator,
the int-backed candidate reduced .NET 10 code size from approximately 220 bytes to 127–128 bytes in
the focused traversal methods and favored the representative int-backed enum path on .NET 8 and
.NET 10. The byte-backed candidate's only measured layout reduction was 12 to 8 bytes for an int
value combined with a byte-backed failure enum.

## 🔗 Evidence and links

- Evidence index: `evidence/README.md`
- Benchmark project: `benchmark/TedToolkit.Results.Representation.Benchmarks.csproj`
- Benchmark catalog: `../Benchmark.slnx`
- Run manifest: `evidence/benchmark/run-manifest.md`
- Size report: `evidence/benchmark/size-report.md`
- Retained state-width reports:
  - `evidence/benchmark/state-width-net48.md`
  - `evidence/benchmark/state-width-net8.md`
  - `evidence/benchmark/state-width-net10.md`
- Reproduction commands:
  - `dotnet run -c Release -f net48 -- --filter "*"`
  - `dotnet run -c Release -f net8.0 -- --filter "*"`
  - `dotnet run -c Release -f net10.0 -- --filter "*"`

## ⚖️ Consequences and accepted trade-offs

The selected value representation removes ordinary Result object allocations and makes arrays store
results inline. Passing results copies their fields, so large `TValue` instantiations may cost more
than copying a class reference. `default(Result<...>)` becomes a documented uninitialized state.
Changing class to struct is a source and binary compatibility break, including removal of the null
Result-reference conversion contract.

The int-backed state enum makes the measured `Result<int, ByteFailure>` layout 12 bytes instead of
8 bytes. The selected candidate also does not win every .NET Framework workload; those measurements
were noisy and sometimes favored byte. These costs are accepted in exchange for the common
int-backed enum behavior and materially smaller .NET 10 generated code. Large `TValue` values still
increase copy costs regardless of discriminator width.

## 🛠️ Implementation handoff

The related change must use `readonly record struct`, store an explicit typed three-state enum with
the normal `int` underlying representation, expose `IsInitialized`, reject uninitialized branch
reads, and update tests and documentation.

## 🔄 Rollout, rollback, and exit

Apply the representation before publishing a stable binary contract. Rollback restores the sealed
record and its null-reference conversion behavior. If already published, release this change only
under the repository's breaking-version policy.

## 📅 Follow-ups and review triggers

| Item | Owner | Due date or objective trigger | Status |
| --- | --- | --- | --- |
| Run and retain representative benchmarks | TedToolkit maintainers | Before accepting the first representation | Completed |
| Reassess the state width | TedToolkit maintainers | Representative evidence shows material consumer impact from the int-backed enum | Open |
