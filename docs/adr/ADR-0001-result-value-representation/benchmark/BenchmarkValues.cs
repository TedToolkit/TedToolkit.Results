// -----------------------------------------------------------------------
// <copyright file="BenchmarkValues.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Representation.Benchmarks;

internal enum ByteFailure : byte
{
    First,
    Second,
}

public enum IntFailure
{
    First,
    Second,
}

internal readonly struct LargeValue
{
    public LargeValue(
        long first,
        long second,
        long third,
        long fourth,
        long fifth,
        long sixth,
        long seventh,
        long eighth)
    {
        First = first;
        Second = second;
        Third = third;
        Fourth = fourth;
        Fifth = fifth;
        Sixth = sixth;
        Seventh = seventh;
        Eighth = eighth;
    }

    public long First { get; }

    public long Second { get; }

    public long Third { get; }

    public long Fourth { get; }

    public long Fifth { get; }

    public long Sixth { get; }

    public long Seventh { get; }

    public long Eighth { get; }
}
