// -----------------------------------------------------------------------
// <copyright file="AccessBenchmarks.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using BenchmarkDotNet.Attributes;

namespace TedToolkit.Results.Representation.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 2)]
public class AccessBenchmarks
{
    private readonly ByteStateResult<int, IntFailure> _byteFailure = new(IntFailure.Second);
    private readonly ByteStateResult<int, IntFailure> _byteSuccess = new(42);
    private readonly Result<int, IntFailure> _classFailure = new(IntFailure.Second);
    private readonly Result<int, IntFailure> _classSuccess = new(42);
    private readonly IntStateResult<int, IntFailure> _intFailure = new(IntFailure.Second);
    private readonly IntStateResult<int, IntFailure> _intSuccess = new(42);

    [Benchmark(Baseline = true)]
    public int ClassRead()
    {
        return _classSuccess.IsSuccess ? _classSuccess.Value : (int)_classFailure.FailureReason;
    }

    [Benchmark]
    public int ByteStateRead()
    {
        return _byteSuccess.IsSuccess ? _byteSuccess.Value : (int)_byteFailure.FailureReason;
    }

    [Benchmark]
    public int IntStateRead()
    {
        return _intSuccess.IsSuccess ? _intSuccess.Value : (int)_intFailure.FailureReason;
    }
}
