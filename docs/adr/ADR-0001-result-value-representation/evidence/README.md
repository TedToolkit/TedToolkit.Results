# Evidence index: ADR-0001

- Decision question: Does later evidence require reconsidering the accepted typed state enum's underlying width?
- Source revision: No Git revision is available; the repository has no commits yet.
- Evidence owner: TedToolkit maintainers
- Captured date: 2026-08-03

| Evidence | Path or source | Purpose | Applicable TFM/environment | Provenance and confidence |
| --- | --- | --- | --- | --- |
| Benchmark source | `../benchmark/` | Compare class, byte-state struct, and int-state struct representations | net48, net8.0, net10.0 on Windows x64 | Authored experiment; Measured, Medium confidence |
| State-width reports | `benchmark/state-width-net48.md`, `benchmark/state-width-net8.md`, `benchmark/state-width-net10.md` | Compare byte/int throughput and native code size | .NET Framework 4.8.1, .NET 8.0.29, .NET 10.0.10 | BenchmarkDotNet; Measured, Medium confidence |
| Full state-width data | `benchmark/state-width-net48.json`, `benchmark/state-width-net8.json`, `benchmark/state-width-net10.json` | Preserve full statistics and benchmark metadata | Same runtimes and host | BenchmarkDotNet; Measured, High provenance |
| Class baseline | `benchmark/class-baseline-net10-*.md` and `benchmark/class-baseline-net10.log` | Compare construction allocation and class/struct trade-offs | .NET 10.0.10 on Windows x64 | BenchmarkDotNet; Measured, Medium confidence |
| Structure sizes | `benchmark/size-report.md` | Compare representative closed-generic byte/int layouts | .NET 10.0.10 on Windows x64 | `Unsafe.SizeOf<T>()`; Measured, High confidence for listed shapes |
| Run manifest | `benchmark/run-manifest.md` | Record commands, host, SDK, runtime, exclusions, and limitations | Measured host | Authored manifest; High provenance |

## Reproduction and retention

Run each target framework explicitly from the benchmark directory. The retained evidence includes
generated Markdown summaries, full JSON reports, selected disassembly, complete logs, the size
report, and the run manifest. Generated build output and the remaining
`BenchmarkDotNet.Artifacts` directory are excluded from the repository.
