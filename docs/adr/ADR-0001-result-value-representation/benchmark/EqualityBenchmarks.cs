// -----------------------------------------------------------------------
// <copyright file="EqualityBenchmarks.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using BenchmarkDotNet.Attributes;

namespace TedToolkit.Results.Representation.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 2)]
public class EqualityBenchmarks
{
    private readonly ByteStateResult<int, IntFailure> _byteFirst = new(42);
    private readonly ByteStateResult<int, IntFailure> _byteSecond = new(42);
    private readonly Result<int, IntFailure> _classFirst = new(42);
    private readonly Result<int, IntFailure> _classSecond = new(42);
    private readonly IntStateResult<int, IntFailure> _intFirst = new(42);
    private readonly IntStateResult<int, IntFailure> _intSecond = new(42);

    [Benchmark(Baseline = true)]
    public bool ClassEquality()
    {
        return _classFirst == _classSecond;
    }

    [Benchmark]
    public bool ByteStateEquality()
    {
        return _byteFirst == _byteSecond;
    }

    [Benchmark]
    public bool IntStateEquality()
    {
        return _intFirst == _intSecond;
    }
}
