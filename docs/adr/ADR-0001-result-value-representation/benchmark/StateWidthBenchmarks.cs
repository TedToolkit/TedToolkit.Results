// -----------------------------------------------------------------------
// <copyright file="StateWidthBenchmarks.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Exporters.Json;

namespace TedToolkit.Results.Representation.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 1)]
[JsonExporterAttribute.Full]
public class StateWidthBenchmarks
{
    private ByteStateResult<int, ByteFailure>[] _byteStateByteFailure = null!;
    private ByteStateResult<int, IntFailure>[] _byteStateIntFailure = null!;
    private IntStateResult<int, ByteFailure>[] _intStateByteFailure = null!;
    private IntStateResult<int, IntFailure>[] _intStateIntFailure = null!;

    [Params(1024, 65536)]
    public int Length { get; set; }

    [GlobalSetup]
    public void Setup()
    {
        _byteStateByteFailure = new ByteStateResult<int, ByteFailure>[Length];
        _byteStateIntFailure = new ByteStateResult<int, IntFailure>[Length];
        _intStateByteFailure = new IntStateResult<int, ByteFailure>[Length];
        _intStateIntFailure = new IntStateResult<int, IntFailure>[Length];

        for (var index = 0; index < Length; index++)
        {
            if (index % 2 == 0)
            {
                _byteStateByteFailure[index] = new(index);
                _byteStateIntFailure[index] = new(index);
                _intStateByteFailure[index] = new(index);
                _intStateIntFailure[index] = new(index);
            }
            else
            {
                _byteStateByteFailure[index] = new(ByteFailure.Second);
                _byteStateIntFailure[index] = new(IntFailure.Second);
                _intStateByteFailure[index] = new(ByteFailure.Second);
                _intStateIntFailure[index] = new(IntFailure.Second);
            }
        }
    }

    [Benchmark(Baseline = true)]
    public int ByteStateWithByteFailure()
    {
        var sum = 0;
        for (var index = 0; index < _byteStateByteFailure.Length; index++)
        {
            ref readonly var result = ref _byteStateByteFailure[index];
            sum += result.IsSuccess ? result.Value : (int)result.FailureReason;
        }

        return sum;
    }

    [Benchmark]
    public int IntStateWithByteFailure()
    {
        var sum = 0;
        for (var index = 0; index < _intStateByteFailure.Length; index++)
        {
            ref readonly var result = ref _intStateByteFailure[index];
            sum += result.IsSuccess ? result.Value : (int)result.FailureReason;
        }

        return sum;
    }

    [Benchmark]
    public int ByteStateWithIntFailure()
    {
        var sum = 0;
        for (var index = 0; index < _byteStateIntFailure.Length; index++)
        {
            ref readonly var result = ref _byteStateIntFailure[index];
            sum += result.IsSuccess ? result.Value : (int)result.FailureReason;
        }

        return sum;
    }

    [Benchmark]
    public int IntStateWithIntFailure()
    {
        var sum = 0;
        for (var index = 0; index < _intStateIntFailure.Length; index++)
        {
            ref readonly var result = ref _intStateIntFailure[index];
            sum += result.IsSuccess ? result.Value : (int)result.FailureReason;
        }

        return sum;
    }
}
