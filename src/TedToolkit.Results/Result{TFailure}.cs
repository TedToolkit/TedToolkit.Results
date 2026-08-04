// -----------------------------------------------------------------------
// <copyright file="Result{TFailure}.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace TedToolkit.Results;

/// <summary>
/// Represents command completion as either success without a value or an enum failure reason.
/// </summary>
/// <typeparam name="TFailure">The enum type describing why an operation failed.</typeparam>
/// <remarks>The default value is uninitialized and must not be consumed as either branch.</remarks>
public readonly record struct Result<TFailure>
    where TFailure : struct, Enum
{
    private readonly TFailure _failureReason;

    private readonly ResultState _state;

    /// <summary>
    /// Initializes a failed result with the specified reason.
    /// </summary>
    /// <param name="failureReason">The reason the operation failed.</param>
    /// <exception cref="ArgumentOutOfRangeException">
    /// <paramref name="failureReason"/> is not a declared enum value.
    /// </exception>
    public Result(TFailure failureReason)
    {
        ResultValidation.ValidateFailureReason(failureReason);
        _failureReason = failureReason;
        _state = ResultState.FAILURE;
    }

    private Result(ResultState state)
    {
        _failureReason = default;
        _state = state;
    }

    /// <summary>
    /// Gets a value indicating whether the result contains an active branch.
    /// </summary>
    public bool IsInitialized
    {
        get
        {
            return _state != ResultState.UNINITIALIZED;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the operation succeeded.
    /// </summary>
    /// <exception cref="InvalidOperationException">The result is uninitialized.</exception>
    public bool IsSuccess
    {
        get
        {
            ResultValidation.ThrowIfUninitialized(IsInitialized);
            return _state == ResultState.SUCCESS;
        }
    }

    /// <summary>
    /// Gets a value indicating whether the operation failed.
    /// </summary>
    /// <exception cref="InvalidOperationException">The result is uninitialized.</exception>
    public bool IsFailure
    {
        get
        {
            return !IsSuccess;
        }
    }

    /// <summary>
    /// Gets the reason the operation failed.
    /// </summary>
    /// <exception cref="InvalidOperationException">The result is uninitialized or successful.</exception>
    public TFailure FailureReason
    {
        get
        {
            ResultValidation.ThrowIfUninitialized(IsInitialized);
            if (_state != ResultState.FAILURE)
            {
                throw new InvalidOperationException("A successful result does not contain a failure reason.");
            }

            return _failureReason;
        }
    }

    /// <summary>
    /// Converts a failure reason into a value-less failed Result.
    /// </summary>
    /// <param name="failureReason">The reason the operation failed.</param>
    /// <returns>A failed Result containing the reason.</returns>
    [SuppressMessage(
        "Design",
        "CA2225",
        Justification = "The non-generic Result type provides the equivalent factory API.")]
    public static implicit operator Result<TFailure>(TFailure failureReason)
    {
        return new(failureReason);
    }

    /// <summary>
    /// Determines whether a value-less Result satisfies a positive condition.
    /// </summary>
    /// <param name="result">The Result to evaluate.</param>
    /// <returns><see langword="true"/> when successful.</returns>
    [SuppressMessage("Performance", "RCS1231", Justification = "The approved operator passes Result by value.")]
    [SuppressMessage("Design", "CA2225", Justification = "IsSuccess is the friendly alternative.")]
    public static bool operator true(Result<TFailure> result)
    {
        return result.IsSuccess;
    }

    /// <summary>
    /// Determines whether a value-less Result fails a positive condition.
    /// </summary>
    /// <param name="result">The Result to evaluate.</param>
    /// <returns><see langword="true"/> when failed.</returns>
    [SuppressMessage("Performance", "RCS1231", Justification = "The approved operator passes Result by value.")]
    public static bool operator false(Result<TFailure> result)
    {
        return result.IsFailure;
    }

    /// <summary>
    /// Negates the successful state of a value-less Result.
    /// </summary>
    /// <param name="result">The Result to evaluate.</param>
    /// <returns><see langword="true"/> when failed.</returns>
    [SuppressMessage("Performance", "RCS1231", Justification = "The approved operator passes Result by value.")]
    [SuppressMessage("Design", "CA2225", Justification = "IsFailure is the friendly alternative.")]
    public static bool operator !(Result<TFailure> result)
    {
        return result.IsFailure;
    }

    /// <summary>
    /// Returns diagnostic text identifying the active branch.
    /// </summary>
    /// <returns><c>✅</c> for success or text beginning with <c>❌</c> for failure.</returns>
    public override string ToString()
    {
        ResultValidation.ThrowIfUninitialized(IsInitialized);
        return _state == ResultState.SUCCESS ? "✅" : $"❌ {_failureReason.ToString()}";
    }

    /// <summary>
    /// Creates the internal initialized success state used by the public factory.
    /// </summary>
    /// <returns>An initialized successful Result.</returns>
    internal static Result<TFailure> CreateSuccess()
    {
        return new(ResultState.SUCCESS);
    }
}