```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8875/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700H 2.30GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.302
  [Host]     : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3


```
| Method        | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|-------------- |----------:|----------:|----------:|----------:|------:|--------:|----------:|----------:|------------:|
| ClassCopy     | 0.6157 ns | 0.0517 ns | 0.0946 ns | 0.5979 ns |  1.02 |    0.22 |      91 B |         - |          NA |
| ByteStateCopy | 1.4193 ns | 0.0844 ns | 0.2354 ns | 1.3379 ns |  2.36 |    0.53 |     112 B |         - |          NA |
| IntStateCopy  | 1.1350 ns | 0.0624 ns | 0.0693 ns | 1.1108 ns |  1.89 |    0.30 |     115 B |         - |          NA |
