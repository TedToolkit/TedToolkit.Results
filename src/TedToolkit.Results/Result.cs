// -----------------------------------------------------------------------
// <copyright file="Result.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results;

/// <summary>
/// Creates operation results through factory methods.
/// </summary>
public static class Result
{
    /// <summary>
    /// Creates a successful Result without a success value.
    /// </summary>
    /// <typeparam name="TFailure">The enum type describing why an operation failed.</typeparam>
    /// <returns>An initialized successful Result.</returns>
    public static Result<TFailure> Succeed<TFailure>()
        where TFailure : struct, Enum
    {
        return Result<TFailure>.CreateSuccess();
    }

    /// <summary>
    /// Creates a failed Result without a success value.
    /// </summary>
    /// <typeparam name="TFailure">The enum type describing why an operation failed.</typeparam>
    /// <param name="failureReason">The reason the operation failed.</param>
    /// <returns>An initialized failed Result.</returns>
    public static Result<TFailure> Fail<TFailure>(TFailure failureReason)
        where TFailure : struct, Enum
    {
        return new(failureReason);
    }

    /// <summary>
    /// Creates a successful result containing the specified value.
    /// </summary>
    /// <typeparam name="TValue">The type of value produced by a successful operation.</typeparam>
    /// <typeparam name="TFailure">The enum type describing why an operation failed.</typeparam>
    /// <param name="value">
    /// The value produced by the operation. The value may be <see langword="null"/> when
    /// <typeparamref name="TValue"/> is nullable.
    /// </param>
    /// <returns>A successful result containing <paramref name="value"/>.</returns>
    public static Result<TValue, TFailure> Succeed<TValue, TFailure>(TValue value)
        where TFailure : struct, Enum
    {
        return new(value);
    }

    /// <summary>
    /// Creates a failed result containing the specified reason.
    /// </summary>
    /// <typeparam name="TValue">The type of value produced by a successful operation.</typeparam>
    /// <typeparam name="TFailure">The enum type describing why an operation failed.</typeparam>
    /// <param name="failureReason">The reason the operation failed.</param>
    /// <returns>A failed result containing <paramref name="failureReason"/>.</returns>
    public static Result<TValue, TFailure> Fail<TValue, TFailure>(TFailure failureReason)
        where TFailure : struct, Enum
    {
        return new(failureReason);
    }
}