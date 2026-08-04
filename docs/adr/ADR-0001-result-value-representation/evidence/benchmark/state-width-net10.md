```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8875/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700H 2.30GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.302
  [Host]     : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3


```
| Method                   | Length | Mean        | Error       | StdDev       | Median      | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|------------------------- |------- |------------:|------------:|-------------:|------------:|------:|--------:|----------:|----------:|------------:|
| **ByteStateWithByteFailure** | **1024**   |    **723.7 ns** |    **31.38 ns** |     **88.00 ns** |    **714.0 ns** |  **1.01** |    **0.17** |     **216 B** |         **-** |          **NA** |
| IntStateWithByteFailure  | 1024   |    602.0 ns |    25.73 ns |     75.88 ns |    589.0 ns |  0.84 |    0.14 |     128 B |         - |          NA |
| ByteStateWithIntFailure  | 1024   |    755.8 ns |    51.43 ns |    147.55 ns |    708.9 ns |  1.06 |    0.24 |     223 B |         - |          NA |
| IntStateWithIntFailure   | 1024   |    663.0 ns |    22.39 ns |     64.23 ns |    649.2 ns |  0.93 |    0.14 |     127 B |         - |          NA |
|                          |        |             |             |              |             |       |         |           |           |             |
| **ByteStateWithByteFailure** | **65536**  | **56,678.1 ns** | **3,440.19 ns** | **10,143.49 ns** | **54,399.1 ns** |  **1.03** |    **0.25** |     **220 B** |         **-** |          **NA** |
| IntStateWithByteFailure  | 65536  | 59,346.8 ns | 3,923.06 ns | 11,567.23 ns | 56,916.3 ns |  1.08 |    0.28 |     128 B |         - |          NA |
| ByteStateWithIntFailure  | 65536  | 70,952.6 ns | 4,625.56 ns | 13,638.57 ns | 71,378.1 ns |  1.29 |    0.33 |     223 B |         - |          NA |
| IntStateWithIntFailure   | 65536  | 53,395.1 ns | 3,367.52 ns |  9,929.20 ns | 53,758.0 ns |  0.97 |    0.24 |     127 B |         - |          NA |
