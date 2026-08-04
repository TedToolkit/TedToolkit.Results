// -----------------------------------------------------------------------
// <copyright file="ResultValidation.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Runtime.CompilerServices;

namespace TedToolkit.Results.AspNetCore;

/// <summary>
/// Validates Result invariants shared by ASP.NET Core adapters.
/// </summary>
internal static class ResultValidation
{
    /// <summary>
    /// Throws when a Result cannot be converted because it has no active branch.
    /// </summary>
    /// <param name="isInitialized">Whether the Result contains an active branch.</param>
    /// <exception cref="InvalidOperationException"><paramref name="isInitialized"/> is <see langword="false"/>.</exception>
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    internal static void EnsureInitialized(bool isInitialized)
    {
        if (isInitialized)
        {
            return;
        }

        throw new InvalidOperationException("An uninitialized Result cannot be converted to an HTTP response.");
    }
}