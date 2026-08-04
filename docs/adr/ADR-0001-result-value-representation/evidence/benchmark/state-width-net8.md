```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8875/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700H 2.30GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.302
  [Host]     : .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 8.0.29 (8.0.29, 8.0.2926.32403), X64 RyuJIT x86-64-v3


```
| Method                   | Length | Mean        | Error       | StdDev       | Median      | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|------------------------- |------- |------------:|------------:|-------------:|------------:|------:|--------:|----------:|----------:|------------:|
| **ByteStateWithByteFailure** | **1024**   |  **1,043.1 ns** |    **50.06 ns** |    **145.25 ns** |  **1,039.8 ns** |  **1.02** |    **0.20** |     **228 B** |         **-** |          **NA** |
| IntStateWithByteFailure  | 1024   |    925.4 ns |    34.51 ns |     96.78 ns |    891.6 ns |  0.90 |    0.16 |     231 B |         - |          NA |
| ByteStateWithIntFailure  | 1024   |    873.8 ns |    20.64 ns |     59.54 ns |    853.1 ns |  0.85 |    0.14 |     231 B |         - |          NA |
| IntStateWithIntFailure   | 1024   |    888.9 ns |    21.73 ns |     62.00 ns |    879.1 ns |  0.87 |    0.14 |     230 B |         - |          NA |
|                          |        |             |             |              |             |       |         |           |           |             |
| **ByteStateWithByteFailure** | **65536**  | **59,703.5 ns** | **3,568.61 ns** | **10,353.19 ns** | **55,906.9 ns** |  **1.03** |    **0.24** |     **226 B** |         **-** |          **NA** |
| IntStateWithByteFailure  | 65536  | 64,399.5 ns | 3,516.55 ns | 10,257.95 ns | 61,933.4 ns |  1.11 |    0.24 |     229 B |         - |          NA |
| ByteStateWithIntFailure  | 65536  | 56,235.0 ns | 1,675.10 ns |  4,751.97 ns | 55,581.3 ns |  0.97 |    0.17 |     229 B |         - |          NA |
| IntStateWithIntFailure   | 65536  | 50,100.0 ns |   994.21 ns |  1,547.87 ns | 49,997.7 ns |  0.86 |    0.13 |     228 B |         - |          NA |
