// -----------------------------------------------------------------------
// <copyright file="SizeReport.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Results.Representation.Benchmarks;

internal static class SizeReport
{
    public static void WriteToConsole()
    {
        Write<int, ByteFailure>();
        Write<int, IntFailure>();
        Write<object, ByteFailure>();
        Write<object, IntFailure>();
        Write<LargeValue, ByteFailure>();
        Write<LargeValue, IntFailure>();
    }

    private static void Write<TValue, TFailure>()
        where TFailure : struct, Enum
    {
        Console.WriteLine(
            $"TValue={typeof(TValue).Name}, TFailure={typeof(TFailure).Name}, " +
            $"ByteState={Unsafe.SizeOf<ByteStateResult<TValue, TFailure>>()}, " +
            $"IntState={Unsafe.SizeOf<IntStateResult<TValue, TFailure>>()}");
    }
}
