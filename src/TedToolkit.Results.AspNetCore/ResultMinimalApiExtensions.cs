// -----------------------------------------------------------------------
// <copyright file="ResultMinimalApiExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;

using static TedToolkit.Results.AspNetCore.ResultValidation;

namespace TedToolkit.Results.AspNetCore;

#pragma warning disable RCS1231 // The approved public API passes Result structs by value.

/// <summary>
/// Converts Results to ASP.NET Core minimal API results.
/// </summary>
public static class ResultMinimalApiExtensions
{
    /// <summary>
    /// Converts a valued Result to HTTP 200 on success or a consumer-mapped result on failure.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The Result to convert.</param>
    /// <param name="failureMapper">Maps the active failure to an application-owned response.</param>
    /// <returns>The selected minimal API result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="failureMapper"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="result"/> is uninitialized, or <paramref name="failureMapper"/> returns <see langword="null"/>.
    /// </exception>
    public static IResult ToHttpResult<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        Func<TFailure, IResult> failureMapper)
        where TFailure : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(failureMapper);

        EnsureInitialized(result.IsInitialized);
        if (result.IsSuccess)
        {
            return Microsoft.AspNetCore.Http.Results.Ok(result.Value);
        }

        return failureMapper(result.FailureReason)
            ?? throw new InvalidOperationException("The failure mapper returned null.");
    }

    /// <summary>
    /// Converts a value-less Result to HTTP 204 on success or a consumer-mapped result on failure.
    /// </summary>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The Result to convert.</param>
    /// <param name="failureMapper">Maps the active failure to an application-owned response.</param>
    /// <returns>The selected minimal API result.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="failureMapper"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="result"/> is uninitialized, or <paramref name="failureMapper"/> returns <see langword="null"/>.
    /// </exception>
    public static IResult ToHttpResult<TFailure>(
        this Result<TFailure> result,
        Func<TFailure, IResult> failureMapper)
        where TFailure : struct, Enum
    {
        ArgumentNullException.ThrowIfNull(failureMapper);

        EnsureInitialized(result.IsInitialized);
        if (result.IsSuccess)
        {
            return Microsoft.AspNetCore.Http.Results.NoContent();
        }

        return failureMapper(result.FailureReason)
            ?? throw new InvalidOperationException("The failure mapper returned null.");
    }
}
#pragma warning restore RCS1231