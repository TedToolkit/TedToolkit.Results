```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8875/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700H 2.30GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.302
  [Host]     : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3


```
| Method           | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Gen0   | Code Size | Allocated | Alloc Ratio |
|----------------- |----------:|----------:|----------:|----------:|------:|--------:|-------:|----------:|----------:|------------:|
| ClassSuccess     | 6.5100 ns | 0.4187 ns | 1.2279 ns | 6.9177 ns | 1.050 |    0.34 | 0.0025 |      35 B |      32 B |        1.00 |
| ByteStateSuccess | 0.2730 ns | 0.0809 ns | 0.2386 ns | 0.2208 ns | 0.044 |    0.04 |      - |      19 B |         - |        0.00 |
| IntStateSuccess  | 0.0166 ns | 0.0165 ns | 0.0392 ns | 0.0000 ns | 0.003 |    0.01 |      - |      24 B |         - |        0.00 |
| ClassFailure     | 4.1719 ns | 0.1572 ns | 0.3735 ns | 4.1508 ns | 0.673 |    0.18 | 0.0025 |      31 B |      32 B |        1.00 |
| ByteStateFailure | 0.2481 ns | 0.0430 ns | 0.0617 ns | 0.2394 ns | 0.040 |    0.01 |      - |      19 B |         - |        0.00 |
| IntStateFailure  | 0.0347 ns | 0.0296 ns | 0.0479 ns | 0.0133 ns | 0.006 |    0.01 |      - |      22 B |         - |        0.00 |
