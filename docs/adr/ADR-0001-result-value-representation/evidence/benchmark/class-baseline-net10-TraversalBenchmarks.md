```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8875/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700H 2.30GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.302
  [Host]     : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3


```
| Method         | Mean     | Error    | StdDev   | Median   | Ratio | RatioSD | Allocated | Alloc Ratio |
|--------------- |---------:|---------:|---------:|---------:|------:|--------:|----------:|------------:|
| ClassArray     | 373.8 ns |  7.52 ns |  7.39 ns | 374.2 ns |  1.00 |    0.03 |         - |          NA |
| ByteStateArray | 566.1 ns | 25.95 ns | 74.86 ns | 548.1 ns |  1.51 |    0.20 |         - |          NA |
| IntStateArray  | 512.8 ns | 28.95 ns | 82.11 ns | 485.3 ns |  1.37 |    0.22 |         - |          NA |
