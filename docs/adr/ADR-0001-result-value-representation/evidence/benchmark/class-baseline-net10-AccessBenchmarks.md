```

BenchmarkDotNet v0.15.8, Windows 11 (10.0.26200.8875/25H2/2025Update/HudsonValley2)
12th Gen Intel Core i7-12700H 2.30GHz, 1 CPU, 20 logical and 14 physical cores
.NET SDK 10.0.302
  [Host]     : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3
  DefaultJob : .NET 10.0.10 (10.0.10, 10.0.1026.32716), X64 RyuJIT x86-64-v3


```
| Method        | Mean      | Error     | StdDev    | Median    | Ratio | RatioSD | Code Size | Allocated | Alloc Ratio |
|-------------- |----------:|----------:|----------:|----------:|------:|--------:|----------:|----------:|------------:|
| ClassRead     | 0.1008 ns | 0.0390 ns | 0.1145 ns | 0.0690 ns |     ? |       ? |      26 B |         - |           ? |
| ByteStateRead | 0.3337 ns | 0.0455 ns | 0.0638 ns | 0.3399 ns |     ? |       ? |     125 B |         - |           ? |
| IntStateRead  | 0.0655 ns | 0.0340 ns | 0.0781 ns | 0.0520 ns |     ? |       ? |     124 B |         - |           ? |
