// -----------------------------------------------------------------------
// <copyright file="TraversalBenchmarks.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using BenchmarkDotNet.Attributes;

namespace TedToolkit.Results.Representation.Benchmarks;

[MemoryDiagnoser]
public class TraversalBenchmarks
{
    private const int LENGTH = 1024;

    private readonly ByteStateResult<int, IntFailure>[] _byteResults = CreateByteResults();
    private readonly Result<int, IntFailure>[] _classResults = CreateClassResults();
    private readonly IntStateResult<int, IntFailure>[] _intResults = CreateIntResults();

    [Benchmark(Baseline = true)]
    public int ClassArray()
    {
        var sum = 0;
        foreach (var result in _classResults)
        {
            sum += result.IsSuccess ? result.Value : (int)result.FailureReason;
        }

        return sum;
    }

    [Benchmark]
    public int ByteStateArray()
    {
        var sum = 0;
        foreach (var result in _byteResults)
        {
            sum += result.IsSuccess ? result.Value : (int)result.FailureReason;
        }

        return sum;
    }

    [Benchmark]
    public int IntStateArray()
    {
        var sum = 0;
        foreach (var result in _intResults)
        {
            sum += result.IsSuccess ? result.Value : (int)result.FailureReason;
        }

        return sum;
    }

    private static ByteStateResult<int, IntFailure>[] CreateByteResults()
    {
        var results = new ByteStateResult<int, IntFailure>[LENGTH];
        for (var index = 0; index < results.Length; index++)
        {
            results[index] = index % 2 == 0 ? new(index) : new(IntFailure.Second);
        }

        return results;
    }

    private static Result<int, IntFailure>[] CreateClassResults()
    {
        var results = new Result<int, IntFailure>[LENGTH];
        for (var index = 0; index < results.Length; index++)
        {
            results[index] = index % 2 == 0 ? new(index) : new(IntFailure.Second);
        }

        return results;
    }

    private static IntStateResult<int, IntFailure>[] CreateIntResults()
    {
        var results = new IntStateResult<int, IntFailure>[LENGTH];
        for (var index = 0; index < results.Length; index++)
        {
            results[index] = index % 2 == 0 ? new(index) : new(IntFailure.Second);
        }

        return results;
    }
}
