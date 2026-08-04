// -----------------------------------------------------------------------
// <copyright file="CopyBenchmarks.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;
using BenchmarkDotNet.Attributes;

namespace TedToolkit.Results.Representation.Benchmarks;

[MemoryDiagnoser]
[DisassemblyDiagnoser(maxDepth: 2)]
public class CopyBenchmarks
{
    private static readonly LargeValue VALUE = new(1, 2, 3, 4, 5, 6, 7, 8);

    private readonly ByteStateResult<LargeValue, IntFailure> _byteResult = new(VALUE);
    private readonly Result<LargeValue, IntFailure> _classResult = new(VALUE);
    private readonly IntStateResult<LargeValue, IntFailure> _intResult = new(VALUE);

    [Benchmark(Baseline = true)]
    public long ClassCopy()
    {
        return Consume(_classResult);
    }

    [Benchmark]
    public long ByteStateCopy()
    {
        return Consume(_byteResult);
    }

    [Benchmark]
    public long IntStateCopy()
    {
        return Consume(_intResult);
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static long Consume(Result<LargeValue, IntFailure> result)
    {
        return result.Value.Eighth;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static long Consume(ByteStateResult<LargeValue, IntFailure> result)
    {
        return result.Value.Eighth;
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    private static long Consume(IntStateResult<LargeValue, IntFailure> result)
    {
        return result.Value.Eighth;
    }
}
