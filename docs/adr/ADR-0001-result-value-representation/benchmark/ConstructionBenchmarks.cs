// -----------------------------------------------------------------------
// <copyright file="ConstructionBenchmarks.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using BenchmarkDotNet.Attributes;

namespace TedToolkit.Results.Representation.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 2)]
public class ConstructionBenchmarks
{
    [Benchmark(Baseline = true)]
    public Result<int, IntFailure> ClassSuccess()
    {
        return new(42);
    }

    [Benchmark]
    public ByteStateResult<int, IntFailure> ByteStateSuccess()
    {
        return new(42);
    }

    [Benchmark]
    public IntStateResult<int, IntFailure> IntStateSuccess()
    {
        return new(42);
    }

    [Benchmark]
    public Result<int, IntFailure> ClassFailure()
    {
        return new(IntFailure.Second);
    }

    [Benchmark]
    public ByteStateResult<int, IntFailure> ByteStateFailure()
    {
        return new(IntFailure.Second);
    }

    [Benchmark]
    public IntStateResult<int, IntFailure> IntStateFailure()
    {
        return new(IntFailure.Second);
    }
}
