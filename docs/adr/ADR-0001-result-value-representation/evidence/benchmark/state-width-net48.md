```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8875/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700H 2.30GHz, 1 CPU, 20 logical and 14 physical cores
  [Host]     : .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256
  DefaultJob : .NET Framework 4.8.1 (4.8.9337.0), X64 RyuJIT VectorSize=256


```
| Method                   | Length | Mean       | Error      | StdDev     | Median     | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|------------------------- |------- |-----------:|-----------:|-----------:|-----------:|------:|--------:|----------:|----------:|------------:|
| **ByteStateWithByteFailure** | **1024**   |   **1.821 μs** |  **0.0371 μs** |  **0.1015 μs** |   **1.791 μs** |  **1.00** |    **0.08** |     **297 B** |         **-** |          **NA** |
| IntStateWithByteFailure  | 1024   |   2.145 μs |  0.1125 μs |  0.3192 μs |   2.076 μs |  1.18 |    0.19 |     300 B |         - |          NA |
| ByteStateWithIntFailure  | 1024   |   2.483 μs |  0.1543 μs |  0.4302 μs |   2.349 μs |  1.37 |    0.25 |     287 B |         - |          NA |
| IntStateWithIntFailure   | 1024   |   2.366 μs |  0.1410 μs |  0.4001 μs |   2.319 μs |  1.30 |    0.23 |     286 B |         - |          NA |
|                          |        |            |            |            |            |       |         |           |           |             |
| **ByteStateWithByteFailure** | **65536**  | **223.993 μs** | **28.9466 μs** | **85.3498 μs** | **187.604 μs** |  **1.13** |    **0.57** |     **297 B** |         **-** |          **NA** |
| IntStateWithByteFailure  | 65536  | 124.607 μs |  3.1960 μs |  9.1184 μs | 121.912 μs |  0.63 |    0.20 |     300 B |         - |          NA |
| ByteStateWithIntFailure  | 65536  | 118.059 μs |  2.7973 μs |  7.5626 μs | 115.781 μs |  0.59 |    0.19 |     287 B |         - |          NA |
| IntStateWithIntFailure   | 65536  | 140.013 μs |  6.8188 μs | 18.7809 μs | 136.130 μs |  0.70 |    0.24 |     286 B |         - |          NA |
