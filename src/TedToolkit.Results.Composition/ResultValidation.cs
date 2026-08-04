// -----------------------------------------------------------------------
// <copyright file="ResultValidation.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Runtime.CompilerServices;

namespace TedToolkit.Results;

/// <summary>
/// Validates references and Result invariants used by optional composition operations.
/// </summary>
internal static class ResultValidation
{
    /// <summary>
    /// Throws when a required Result does not contain an active branch.
    /// </summary>
    /// <param name="isInitialized">Whether the source or callback Result contains an active branch.</param>
    /// <exception cref="InvalidOperationException"><paramref name="isInitialized"/> is <see langword="false"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ValidateInitialized(bool isInitialized)
    {
        if (isInitialized)
        {
            return;
        }

        throw new InvalidOperationException("The source or callback returned an uninitialized result.");
    }

    /// <summary>
    /// Throws with collection position context when a Result does not contain an active branch.
    /// </summary>
    /// <param name="isInitialized">Whether the Result contains an active branch.</param>
    /// <param name="position">The zero-based position of the Result in the source sequence.</param>
    /// <exception cref="InvalidOperationException"><paramref name="isInitialized"/> is <see langword="false"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ValidateInitialized(bool isInitialized, int position)
    {
        if (isInitialized)
        {
            return;
        }

        throw new InvalidOperationException(
            $"The Result at position {position.ToString(CultureInfo.InvariantCulture)} is uninitialized.");
    }

    /// <summary>
    /// Throws with operation-specific context when a callback Result does not contain an active branch.
    /// </summary>
    /// <param name="isInitialized">Whether the callback Result contains an active branch.</param>
    /// <param name="message">The exception message describing the callback boundary.</param>
    /// <exception cref="InvalidOperationException"><paramref name="isInitialized"/> is <see langword="false"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void EnsureInitialized(bool isInitialized, string message)
    {
        if (isInitialized)
        {
            return;
        }

        throw new InvalidOperationException(message);
    }

    /// <summary>
    /// Throws when a required callback or source reference is null.
    /// </summary>
    /// <param name="value">The reference to validate.</param>
    /// <param name="parameterName">The public parameter name.</param>
    /// <exception cref="ArgumentNullException"><paramref name="value"/> is <see langword="null"/>.</exception>
    [SuppressMessage(
        "Usage",
        "CA1510",
        Justification = "The portable helper supports target frameworks without ThrowIfNull.")]
    [SuppressMessage(
        "Roslynator",
        "RCS1255",
        Justification = "The portable helper supports target frameworks without ThrowIfNull.")]
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void ThrowIfNull([NotNull] object? value, string parameterName)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(value, parameterName);
#else
        if (value is not null)
        {
            return;
        }

        throw new ArgumentNullException(parameterName);
#endif
    }
}