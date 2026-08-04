```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8875/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700H 2.30GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.302
  [Host]     : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3


```
| Method            | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|------------------ |----------:|----------:|----------:|----------:|------:|--------:|----------:|----------:|------------:|
| ClassEquality     | 0.3950 ns | 0.0436 ns | 0.0910 ns | 0.3744 ns |  1.05 |    0.34 |      68 B |         - |          NA |
| ByteStateEquality | 0.6749 ns | 0.1029 ns | 0.3035 ns | 0.7068 ns |  1.80 |    0.91 |      47 B |         - |          NA |
| IntStateEquality  | 0.2339 ns | 0.0741 ns | 0.2184 ns | 0.1766 ns |  0.62 |    0.61 |      45 B |         - |          NA |
