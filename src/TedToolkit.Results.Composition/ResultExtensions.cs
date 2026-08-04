// -----------------------------------------------------------------------
// <copyright file="ResultExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

// The approved public extension signatures pass Result values by value.
#pragma warning disable RCS1231

using System.Diagnostics.CodeAnalysis;

using static TedToolkit.Results.ResultValidation;

namespace TedToolkit.Results;

/// <summary>
/// Provides synchronous branch consumption and composition for Result values.
/// </summary>
public static class ResultExtensions
{
    /// <summary>
    /// Projects the active valued Result branch into a single value.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TResult">The value returned by either callback.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="onSuccess">The callback used for success.</param>
    /// <param name="onFailure">The callback used for failure.</param>
    /// <returns>The value returned by the callback for the active branch.</returns>
    /// <exception cref="ArgumentNullException">Either callback is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static TResult Match<TValue, TFailure, TResult>(
        this Result<TValue, TFailure> result,
        Func<TValue, TResult> onSuccess,
        Func<TFailure, TResult> onFailure)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(onSuccess, nameof(onSuccess));
        ResultValidation.ThrowIfNull(onFailure, nameof(onFailure));
        return result.IsSuccess ? onSuccess(result.Value) : onFailure(result.FailureReason);
    }

    /// <summary>
    /// Projects the active value-less Result branch into a single value.
    /// </summary>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TResult">The value returned by either callback.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="onSuccess">The callback used for success.</param>
    /// <param name="onFailure">The callback used for failure.</param>
    /// <returns>The value returned by the callback for the active branch.</returns>
    /// <exception cref="ArgumentNullException">Either callback is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static TResult Match<TFailure, TResult>(
        this Result<TFailure> result,
        Func<TResult> onSuccess,
        Func<TFailure, TResult> onFailure)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(onSuccess, nameof(onSuccess));
        ResultValidation.ThrowIfNull(onFailure, nameof(onFailure));
        return result.IsSuccess ? onSuccess() : onFailure(result.FailureReason);
    }

    /// <summary>
    /// Attempts to obtain the active success value.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="value">The success value, or <see langword="default"/> for failure.</param>
    /// <returns><see langword="true"/> when <paramref name="result"/> is successful.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static bool TryGetValue<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        [MaybeNullWhen(false)] out TValue value)
        where TFailure : struct, Enum
    {
        if (result.IsSuccess)
        {
            value = result.Value;
            return true;
        }

        value = default;
        return false;
    }

    /// <summary>
    /// Attempts to obtain the active failure reason from a valued Result.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="failureReason">The failure reason, or <see langword="default"/> for success.</param>
    /// <returns><see langword="true"/> when <paramref name="result"/> is failed.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static bool TryGetFailure<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        out TFailure failureReason)
        where TFailure : struct, Enum
    {
        if (result.IsFailure)
        {
            failureReason = result.FailureReason;
            return true;
        }

        failureReason = default;
        return false;
    }

    /// <summary>
    /// Attempts to obtain the active failure reason from a value-less Result.
    /// </summary>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="failureReason">The failure reason, or <see langword="default"/> for success.</param>
    /// <returns><see langword="true"/> when <paramref name="result"/> is failed.</returns>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static bool TryGetFailure<TFailure>(
        this Result<TFailure> result,
        out TFailure failureReason)
        where TFailure : struct, Enum
    {
        if (result.IsFailure)
        {
            failureReason = result.FailureReason;
            return true;
        }

        failureReason = default;
        return false;
    }

    /// <summary>
    /// Deconstructs a valued Result into its discriminator and branch payloads.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="isSuccess"><see langword="true"/> when the success branch is active.</param>
    /// <param name="value">The success value, or <see langword="default"/> for failure.</param>
    /// <param name="failureReason">The failure reason, or <see langword="default"/> for success.</param>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static void Deconstruct<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        out bool isSuccess,
        out TValue value,
        out TFailure failureReason)
        where TFailure : struct, Enum
    {
        isSuccess = result.IsSuccess;
        value = isSuccess ? result.Value : default!;
        failureReason = isSuccess ? default : result.FailureReason;
    }

    /// <summary>
    /// Deconstructs a value-less Result into its discriminator and failure payload.
    /// </summary>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="isSuccess"><see langword="true"/> when the success branch is active.</param>
    /// <param name="failureReason">The failure reason, or <see langword="default"/> for success.</param>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static void Deconstruct<TFailure>(
        this Result<TFailure> result,
        out bool isSuccess,
        out TFailure failureReason)
        where TFailure : struct, Enum
    {
        isSuccess = result.IsSuccess;
        failureReason = isSuccess ? default : result.FailureReason;
    }

    /// <summary>
    /// Transforms the active success value while preserving the failure type.
    /// </summary>
    /// <typeparam name="TValue">The source success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TNext">The mapped success value type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="mapper">The success-value mapper.</param>
    /// <returns>A mapped success, or the original failure.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static Result<TNext, TFailure> Map<TValue, TFailure, TNext>(
        this Result<TValue, TFailure> result,
        Func<TValue, TNext> mapper)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(mapper, nameof(mapper));
        return result.IsSuccess
            ? Result.Succeed<TNext, TFailure>(mapper(result.Value))
            : Result.Fail<TNext, TFailure>(result.FailureReason);
    }

    /// <summary>
    /// Chains a successful value into another Result with the same failure type.
    /// </summary>
    /// <typeparam name="TValue">The source success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TNext">The bound success value type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="binder">The callback that produces the next Result.</param>
    /// <returns>The bound Result for success, or the original failure.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">The source or callback Result is uninitialized.</exception>
    public static Result<TNext, TFailure> Bind<TValue, TFailure, TNext>(
        this Result<TValue, TFailure> result,
        Func<TValue, Result<TNext, TFailure>> binder)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(binder, nameof(binder));
        if (result.IsFailure)
        {
            return Result.Fail<TNext, TFailure>(result.FailureReason);
        }

        var bound = binder(result.Value);
        EnsureInitialized(bound.IsInitialized, "The binder returned an uninitialized result.");
        return bound;
    }

    /// <summary>
    /// Transforms the active failure reason while preserving the success value.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The source failure enum type.</typeparam>
    /// <typeparam name="TNextFailure">The mapped failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="mapper">The failure-reason mapper.</param>
    /// <returns>The original success, or a failure containing the mapped reason.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="mapper"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static Result<TValue, TNextFailure> MapFailure<TValue, TFailure, TNextFailure>(
        this Result<TValue, TFailure> result,
        Func<TFailure, TNextFailure> mapper)
        where TFailure : struct, Enum
        where TNextFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(mapper, nameof(mapper));
        return result.IsSuccess
            ? Result.Succeed<TValue, TNextFailure>(result.Value)
            : Result.Fail<TValue, TNextFailure>(mapper(result.FailureReason));
    }

    /// <summary>
    /// Validates a success value and uses a fixed failure when validation fails.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="predicate">The predicate a successful value must satisfy.</param>
    /// <param name="failureReason">The failure reason used when validation fails.</param>
    /// <returns>The original Result when validation is unnecessary or succeeds; otherwise, a failure.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="predicate"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static Result<TValue, TFailure> Ensure<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        Func<TValue, bool> predicate,
        TFailure failureReason)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(predicate, nameof(predicate));
        if (result.IsFailure || predicate(result.Value))
        {
            return result;
        }

        return Result.Fail<TValue, TFailure>(failureReason);
    }

    /// <summary>
    /// Validates a success value and computes a failure when validation fails.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="predicate">The predicate a successful value must satisfy.</param>
    /// <param name="failureFactory">The callback that produces a failure reason from the rejected value.</param>
    /// <returns>The original Result when validation is unnecessary or succeeds; otherwise, a failure.</returns>
    /// <exception cref="ArgumentNullException">Either callback is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static Result<TValue, TFailure> Ensure<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        Func<TValue, bool> predicate,
        Func<TValue, TFailure> failureFactory)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(predicate, nameof(predicate));
        ResultValidation.ThrowIfNull(failureFactory, nameof(failureFactory));
        if (result.IsFailure || predicate(result.Value))
        {
            return result;
        }

        return Result.Fail<TValue, TFailure>(failureFactory(result.Value));
    }

    /// <summary>
    /// Recovers a failure into a successful value.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="recovery">The callback that produces a value from the failure reason.</param>
    /// <returns>The original success, or a success containing the recovered value.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="recovery"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException"><paramref name="result"/> is uninitialized.</exception>
    public static Result<TValue, TFailure> Recover<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        Func<TFailure, TValue> recovery)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(recovery, nameof(recovery));
        return result.IsSuccess
            ? result
            : Result.Succeed<TValue, TFailure>(recovery(result.FailureReason));
    }

    /// <summary>
    /// Recovers a failure through another Result-producing operation.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="recovery">The callback that produces the recovered Result.</param>
    /// <returns>The original success, or the initialized Result returned by the callback.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="recovery"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">The source or callback Result is uninitialized.</exception>
    public static Result<TValue, TFailure> RecoverWith<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        Func<TFailure, Result<TValue, TFailure>> recovery)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(recovery, nameof(recovery));
        if (result.IsSuccess)
        {
            return result;
        }

        var recovered = recovery(result.FailureReason);
        EnsureInitialized(recovered.IsInitialized, "The recovery callback returned an uninitialized result.");
        return recovered;
    }

    /// <summary>
    /// Chains a value-less success into another value-less Result.
    /// </summary>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="binder">The callback that produces the next Result.</param>
    /// <returns>The bound Result for success, or the original failure.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">The source or callback Result is uninitialized.</exception>
    public static Result<TFailure> Bind<TFailure>(
        this Result<TFailure> result,
        Func<Result<TFailure>> binder)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(binder, nameof(binder));
        if (result.IsFailure)
        {
            return Result.Fail(result.FailureReason);
        }

        var bound = binder();
        EnsureInitialized(bound.IsInitialized, "The binder returned an uninitialized result.");
        return bound;
    }

    /// <summary>
    /// Chains a value-less success into a valued Result.
    /// </summary>
    /// <typeparam name="TValue">The bound success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="binder">The callback that produces the valued Result.</param>
    /// <returns>The bound Result for success, or the original failure.</returns>
    /// <exception cref="ArgumentNullException"><paramref name="binder"/> is <see langword="null"/>.</exception>
    /// <exception cref="InvalidOperationException">The source or callback Result is uninitialized.</exception>
    public static Result<TValue, TFailure> Bind<TValue, TFailure>(
        this Result<TFailure> result,
        Func<Result<TValue, TFailure>> binder)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(binder, nameof(binder));
        if (result.IsFailure)
        {
            return Result.Fail<TValue, TFailure>(result.FailureReason);
        }

        var bound = binder();
        EnsureInitialized(bound.IsInitialized, "The binder returned an uninitialized result.");
        return bound;
    }
}
#pragma warning restore RCS1231