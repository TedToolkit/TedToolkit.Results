// -----------------------------------------------------------------------
// <copyright file="Result{TValue,TFailure}.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace TedToolkit.Results;

/// <summary>
/// Represents an operation result containing either a success value or a failure reason.
/// </summary>
/// <typeparam name="TValue">The type of value produced by a successful operation.</typeparam>
/// <typeparam name="TFailure">The enum type describing why an operation failed.</typeparam>
/// <remarks>
/// Each result contains exactly one active branch. A successful result may contain
/// <see langword="null"/> when <typeparamref name="TValue"/> is nullable.
/// The default value is uninitialized and must not be consumed as either branch.
/// </remarks>
public readonly record struct Result<TValue, TFailure>
    where TFailure : struct, Enum
{
    private readonly TFailure _failureReason;

    private readonly ResultState _state;

    private readonly TValue? _value;

    /// <summary>
    /// Initializes a successful result with the specified value.
    /// </summary>
    /// <param name="value">
    /// The value produced by the operation. The value may be <see langword="null"/> when
    /// <typeparamref name="TValue"/> is nullable.
    /// </param>
    public Result(TValue value)
    {
        _failureReason = default;
        _state = ResultState.SUCCESS;
        _value = value;
    }

    /// <summary>
    /// Initializes a failed result with the specified reason.
    /// </summary>
    /// <param name="failureReason">The reason the operation failed.</param>
    public Result(TFailure failureReason)
    {
        ResultValidation.ValidateFailureReason(failureReason);
        _failureReason = failureReason;
        _state = ResultState.FAILURE;
        _value = default;
    }

    /// <summary>
    /// Gets a value indicating whether the result contains an active branch.
    /// </summary>
    /// <value>
    /// <see langword="true"/> when the result was constructed as a success or failure;
    /// otherwise, <see langword="false"/>.
    /// </value>
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
    /// <value><see langword="true"/> for a successful result; otherwise, <see langword="false"/>.</value>
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
    /// <value><see langword="true"/> for a failed result; otherwise, <see langword="false"/>.</value>
    /// <exception cref="InvalidOperationException">The result is uninitialized.</exception>
    public bool IsFailure
    {
        get
        {
            return !IsSuccess;
        }
    }

    /// <summary>
    /// Gets the value produced by a successful operation.
    /// </summary>
    /// <value>The value supplied when the successful result was created.</value>
    /// <exception cref="InvalidOperationException">The result represents a failure.</exception>
    public TValue Value
    {
        get
        {
            ResultValidation.ThrowIfUninitialized(IsInitialized);
            if (_state != ResultState.SUCCESS)
            {
                throw new InvalidOperationException("A failed result does not contain a success value.");
            }

            return _value!;
        }
    }

    /// <summary>
    /// Gets the reason an operation failed.
    /// </summary>
    /// <value>The reason supplied when the failed result was created.</value>
    /// <exception cref="InvalidOperationException">The result represents a success.</exception>
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
    /// Determines whether a result satisfies a positive conditional expression.
    /// </summary>
    /// <param name="result">The result to evaluate.</param>
    /// <returns><see langword="true"/> when <paramref name="result"/> is successful.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    [SuppressMessage(
        "Performance",
        "RCS1231",
        Justification = "The approved public operator contract passes Result by value.")]
    [SuppressMessage(
        "Design",
        "CA2225",
        Justification = "The IsSuccess property is the friendly alternative to conditional syntax.")]
    public static bool operator true(Result<TValue, TFailure> result)
    {
        return result.IsSuccess;
    }

    /// <summary>
    /// Determines whether a result fails a positive conditional expression.
    /// </summary>
    /// <param name="result">The result to evaluate.</param>
    /// <returns><see langword="true"/> when <paramref name="result"/> is failed.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    [SuppressMessage(
        "Performance",
        "RCS1231",
        Justification = "The approved public operator contract passes Result by value.")]
    public static bool operator false(Result<TValue, TFailure> result)
    {
        return result.IsFailure;
    }

    /// <summary>
    /// Negates the successful state of a result.
    /// </summary>
    /// <param name="result">The result to evaluate.</param>
    /// <returns><see langword="true"/> when <paramref name="result"/> is failed.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    [SuppressMessage(
        "Performance",
        "RCS1231",
        Justification = "The approved public operator contract passes Result by value.")]
    [SuppressMessage(
        "Design",
        "CA2225",
        Justification = "The IsFailure property is the friendly alternative to negated conditional syntax.")]
    public static bool operator !(Result<TValue, TFailure> result)
    {
        return result.IsFailure;
    }

    /// <summary>
    /// Converts a success value to a successful result.
    /// </summary>
    /// <param name="value">
    /// The value produced by the operation. The value may be <see langword="null"/> when
    /// <typeparamref name="TValue"/> is nullable.
    /// </param>
    /// <returns>A successful result containing <paramref name="value"/>.</returns>
    [SuppressMessage(
        "Design",
        "CA2225",
        Justification = "The non-generic Result type provides the equivalent factory API.")]
    public static implicit operator Result<TValue, TFailure>(TValue value)
    {
        return new(value);
    }

    /// <summary>
    /// Converts a failure reason to a failed result.
    /// </summary>
    /// <param name="failureReason">The reason the operation failed.</param>
    /// <returns>A failed result containing <paramref name="failureReason"/>.</returns>
    [SuppressMessage(
        "Design",
        "CA2225",
        Justification = "The non-generic Result type provides the equivalent factory API.")]
    public static implicit operator Result<TValue, TFailure>(TFailure failureReason)
    {
        return new(failureReason);
    }

    /// <summary>
    /// Converts a successful result to its contained value.
    /// </summary>
    /// <param name="result">The result whose value to read.</param>
    /// <returns>The success value contained in <paramref name="result"/>.</returns>
    /// <exception cref="InvalidOperationException">
    /// <paramref name="result"/> is uninitialized or represents a failure.
    /// </exception>
    [SuppressMessage(
        "Design",
        "CA2225",
        Justification = "The Value property provides the equivalent explicit API.")]
    [SuppressMessage(
        "Performance",
        "RCS1231",
        Justification = "User-defined conversion parameters cannot use ref-like modifiers without changing the public contract.")]
    public static explicit operator TValue(Result<TValue, TFailure> result)
    {
        return result.Value;
    }

    /// <summary>
    /// Returns diagnostic text identifying the active result branch.
    /// </summary>
    /// <returns>
    /// Text beginning with <c>✅</c> for a success or <c>❌</c> for a failure.
    /// </returns>
    /// <remarks>
    /// The returned text is intended only for logs and debugging. It is not a serialization,
    /// persistence, localization, or user-interface contract.
    /// </remarks>
    public override string ToString()
    {
        ResultValidation.ThrowIfUninitialized(IsInitialized);
        if (_state == ResultState.SUCCESS)
        {
            var value = _value is null ? "null" : _value.ToString() ?? "null";
            return $"✅ {value}";
        }

        return $"❌ {_failureReason.ToString()}";
    }
}