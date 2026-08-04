// -----------------------------------------------------------------------
// <copyright file="ResultValidation.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;

namespace TedToolkit.Results;

/// <summary>
/// Validates internal Result invariants shared by construction paths.
/// </summary>
internal static class ResultValidation
{
    /// <summary>
    /// Throws when a Result does not contain an active branch.
    /// </summary>
    /// <param name="isInitialized">Whether the Result contains an active branch.</param>
    /// <exception cref="InvalidOperationException"><paramref name="isInitialized"/> is <see langword="false"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfUninitialized(bool isInitialized)
    {
        if (isInitialized)
        {
            return;
        }

        throw new InvalidOperationException("An uninitialized result does not contain an active branch.");
    }

    /// <summary>
    /// Verifies that a failure reason is declared by its enum type.
    /// </summary>
    /// <typeparam name="TFailure">The enum type describing why an operation failed.</typeparam>
    /// <param name="failureReason">The failure reason to validate.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="failureReason"/> is not a declared enum value.
    /// </exception>
    [SuppressMessage(
        "Usage",
        "CA2263",
        Justification = "The generic overload is unavailable on the package's legacy target frameworks.")]
    internal static void ValidateFailureReason<TFailure>(TFailure failureReason)
        where TFailure : struct, Enum
    {
        if (Enum.IsDefined(typeof(TFailure), failureReason))
        {
            return;
        }

        throw new ArgumentOutOfRangeException(
            nameof(failureReason),
            failureReason,
            "The failure reason must be a declared enum value.");
    }
}