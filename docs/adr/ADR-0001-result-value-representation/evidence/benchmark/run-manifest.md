# ADR-0001 benchmark run manifest

- Captured date: 2026-08-03
- Source revision: No Git revision is available; the repository has no commits yet.
- BenchmarkDotNet: 0.15.8
- SDK: .NET SDK 10.0.302
- Operating system: Windows 11 10.0.26200.8875, x64
- Processor: 12th Gen Intel Core i7-12700H, 14 physical cores, 20 logical processors
- Benchmark process GC: Concurrent Workstation
- Power plan: BenchmarkDotNet selected High performance during each run
- Competing benchmark workload: None; target-framework runs were sequential

## Commands

Run from `docs/adr/ADR-0001-result-value-representation/benchmark/`:

```powershell
dotnet run -c Release -f net10.0 -- --filter "*StateWidthBenchmarks*" --artifacts "<temp>/TedToolkit.Results.ADR0001.net10"
dotnet run -c Release -f net8.0 -- --filter "*StateWidthBenchmarks*" --artifacts "<temp>/TedToolkit.Results.ADR0001.net8"
dotnet run -c Release -f net48 -- --filter "*StateWidthBenchmarks*" --artifacts "<temp>/TedToolkit.Results.ADR0001.net48"
dotnet run -c Release -f net10.0 -- --sizes
```

An earlier full net10 run used:

```powershell
dotnet run -c Release -f net10.0 -- --filter "*"
```

## Runtime matrix

| Target | Runtime | JIT |
| --- | --- | --- |
| `net48` | .NET Framework 4.8.1 (4.8.9337.0) | x64 RyuJIT |
| `net8.0` | .NET 8.0.29 | x64 RyuJIT x86-64-v3 |
| `net10.0` | .NET 10.0.10 | x64 RyuJIT x86-64-v3 |

## Retained evidence

Each state-width target retains a GitHub Markdown report, full JSON report, selected assembly
listing, and complete BenchmarkDotNet log. The class baseline retains the five generated Markdown
reports and the successful 18-benchmark net10 log.

## Exclusions and limitations

- An initial net10 attempt was excluded because an outer command timeout orphaned the generated
  benchmark process and caused a subsequent file lock; it produced no valid measurements.
- Results apply to one Windows x64 Intel host. They do not establish ARM64 behavior.
- Several array measurements were multimodal or high variance. The ADR therefore treats the data as
  directional and records that no discriminator width wins every runtime and workload.
- Absolute times below one nanosecond in the full-run construction/access microbenchmarks are near
  the harness resolution; allocation and generated-code evidence are retained, but those timings are
  not used alone to select the state width.
