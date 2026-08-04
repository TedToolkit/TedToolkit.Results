// -----------------------------------------------------------------------
// <copyright file="ResultAsyncExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

// The approved public signatures pass Results and cancellation tokens by value, while the private
// async wrappers deliberately preserve synchronous validation and ConfigureAwait(false) behavior.
using System.Runtime.CompilerServices;

using static TedToolkit.Results.ResultValidation;

namespace TedToolkit.Results;

#pragma warning disable RCS1231 // The approved public API passes Result structs and cancellation tokens by value.

/// <summary>
/// Provides Task-based composition for valued and value-less Results.
/// </summary>
public static class ResultAsyncExtensions
{
    /// <summary>
    /// Maps a successful Result through an asynchronous callback.
    /// </summary>
    /// <typeparam name="TValue">The source value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TNext">The mapped value type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="mapper">The asynchronous success mapper.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the mapped Result.</returns>
    public static Task<Result<TNext, TFailure>> MapAsync<TValue, TFailure, TNext>(
        this Result<TValue, TFailure> result,
        Func<TValue, CancellationToken, Task<TNext>> mapper,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(mapper, nameof(mapper));
        ValidateInitialized(result.IsInitialized);
        return MapCoreAsync(result, mapper, cancellationToken);
    }

    /// <summary>
    /// Maps a Task-wrapped successful Result through an asynchronous callback.
    /// </summary>
    /// <typeparam name="TValue">The source value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TNext">The mapped value type.</typeparam>
    /// <param name="resultTask">The source Result Task.</param>
    /// <param name="mapper">The asynchronous success mapper.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the mapped Result.</returns>
    public static Task<Result<TNext, TFailure>> MapAsync<TValue, TFailure, TNext>(
        this Task<Result<TValue, TFailure>> resultTask,
        Func<TValue, CancellationToken, Task<TNext>> mapper,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(resultTask, nameof(resultTask));
        ResultValidation.ThrowIfNull(mapper, nameof(mapper));
        return MapTaskCoreAsync(resultTask, mapper, cancellationToken);
    }

    /// <summary>
    /// Binds a successful Result through an asynchronous Result callback.
    /// </summary>
    /// <typeparam name="TValue">The source value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TNext">The bound value type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="binder">The asynchronous binder.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the bound Result.</returns>
    public static Task<Result<TNext, TFailure>> BindAsync<TValue, TFailure, TNext>(
        this Result<TValue, TFailure> result,
        Func<TValue, CancellationToken, Task<Result<TNext, TFailure>>> binder,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(binder, nameof(binder));
        ValidateInitialized(result.IsInitialized);
        return BindCoreAsync(result, binder, cancellationToken);
    }

    /// <summary>
    /// Binds a Task-wrapped successful Result through an asynchronous Result callback.
    /// </summary>
    /// <typeparam name="TValue">The source value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TNext">The bound value type.</typeparam>
    /// <param name="resultTask">The source Result Task.</param>
    /// <param name="binder">The asynchronous binder.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the bound Result.</returns>
    public static Task<Result<TNext, TFailure>> BindAsync<TValue, TFailure, TNext>(
        this Task<Result<TValue, TFailure>> resultTask,
        Func<TValue, CancellationToken, Task<Result<TNext, TFailure>>> binder,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(resultTask, nameof(resultTask));
        ResultValidation.ThrowIfNull(binder, nameof(binder));
        return BindTaskCoreAsync(resultTask, binder, cancellationToken);
    }

    /// <summary>
    /// Matches a Result through asynchronous branch callbacks.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TResult">The callback result type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="onSuccess">The success callback.</param>
    /// <param name="onFailure">The failure callback.</param>
    /// <param name="cancellationToken">The token passed to the active callback.</param>
    /// <returns>A Task producing the active callback value.</returns>
    public static Task<TResult> MatchAsync<TValue, TFailure, TResult>(
        this Result<TValue, TFailure> result,
        Func<TValue, CancellationToken, Task<TResult>> onSuccess,
        Func<TFailure, CancellationToken, Task<TResult>> onFailure,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(onSuccess, nameof(onSuccess));
        ResultValidation.ThrowIfNull(onFailure, nameof(onFailure));
        ValidateInitialized(result.IsInitialized);
        return MatchCoreAsync(result, onSuccess, onFailure, cancellationToken);
    }

    /// <summary>
    /// Matches a Task-wrapped Result through asynchronous branch callbacks.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TResult">The callback result type.</typeparam>
    /// <param name="resultTask">The source Result Task.</param>
    /// <param name="onSuccess">The success callback.</param>
    /// <param name="onFailure">The failure callback.</param>
    /// <param name="cancellationToken">The token passed to the active callback.</param>
    /// <returns>A Task producing the active callback value.</returns>
    public static Task<TResult> MatchAsync<TValue, TFailure, TResult>(
        this Task<Result<TValue, TFailure>> resultTask,
        Func<TValue, CancellationToken, Task<TResult>> onSuccess,
        Func<TFailure, CancellationToken, Task<TResult>> onFailure,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(resultTask, nameof(resultTask));
        ResultValidation.ThrowIfNull(onSuccess, nameof(onSuccess));
        ResultValidation.ThrowIfNull(onFailure, nameof(onFailure));
        return MatchTaskCoreAsync(resultTask, onSuccess, onFailure, cancellationToken);
    }

    /// <summary>
    /// Validates a successful Result through an asynchronous predicate.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="predicate">The asynchronous predicate.</param>
    /// <param name="failureReason">The reason used when validation fails.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the validated Result.</returns>
    public static Task<Result<TValue, TFailure>> EnsureAsync<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        Func<TValue, CancellationToken, Task<bool>> predicate,
        TFailure failureReason,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(predicate, nameof(predicate));
        ValidateInitialized(result.IsInitialized);
        return EnsureCoreAsync(result, predicate, failureReason, cancellationToken);
    }

    /// <summary>
    /// Validates a Task-wrapped successful Result through an asynchronous predicate.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="resultTask">The source Result Task.</param>
    /// <param name="predicate">The asynchronous predicate.</param>
    /// <param name="failureReason">The reason used when validation fails.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the validated Result.</returns>
    public static Task<Result<TValue, TFailure>> EnsureAsync<TValue, TFailure>(
        this Task<Result<TValue, TFailure>> resultTask,
        Func<TValue, CancellationToken, Task<bool>> predicate,
        TFailure failureReason,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(resultTask, nameof(resultTask));
        ResultValidation.ThrowIfNull(predicate, nameof(predicate));
        return EnsureTaskCoreAsync(resultTask, predicate, failureReason, cancellationToken);
    }

    /// <summary>
    /// Recovers a failed Result through an asynchronous value callback.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="recovery">The asynchronous recovery callback.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the recovered Result.</returns>
    public static Task<Result<TValue, TFailure>> RecoverAsync<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        Func<TFailure, CancellationToken, Task<TValue>> recovery,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(recovery, nameof(recovery));
        ValidateInitialized(result.IsInitialized);
        return RecoverCoreAsync(result, recovery, cancellationToken);
    }

    /// <summary>
    /// Recovers a Task-wrapped failed Result through an asynchronous value callback.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="resultTask">The source Result Task.</param>
    /// <param name="recovery">The asynchronous recovery callback.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the recovered Result.</returns>
    public static Task<Result<TValue, TFailure>> RecoverAsync<TValue, TFailure>(
        this Task<Result<TValue, TFailure>> resultTask,
        Func<TFailure, CancellationToken, Task<TValue>> recovery,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(resultTask, nameof(resultTask));
        ResultValidation.ThrowIfNull(recovery, nameof(recovery));
        return RecoverTaskCoreAsync(resultTask, recovery, cancellationToken);
    }

    /// <summary>
    /// Recovers a failed Result through an asynchronous Result callback.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="recovery">The asynchronous recovery callback.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the recovered Result.</returns>
    public static Task<Result<TValue, TFailure>> RecoverWithAsync<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        Func<TFailure, CancellationToken, Task<Result<TValue, TFailure>>> recovery,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(recovery, nameof(recovery));
        ValidateInitialized(result.IsInitialized);
        return RecoverWithCoreAsync(result, recovery, cancellationToken);
    }

    /// <summary>
    /// Recovers a Task-wrapped failed Result through an asynchronous Result callback.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="resultTask">The source Result Task.</param>
    /// <param name="recovery">The asynchronous recovery callback.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the recovered Result.</returns>
    public static Task<Result<TValue, TFailure>> RecoverWithAsync<TValue, TFailure>(
        this Task<Result<TValue, TFailure>> resultTask,
        Func<TFailure, CancellationToken, Task<Result<TValue, TFailure>>> recovery,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(resultTask, nameof(resultTask));
        ResultValidation.ThrowIfNull(recovery, nameof(recovery));
        return RecoverWithTaskCoreAsync(resultTask, recovery, cancellationToken);
    }

    /// <summary>
    /// Binds a value-less success through an asynchronous value-less callback.
    /// </summary>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="binder">The asynchronous binder.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the bound Result.</returns>
    public static Task<Result<TFailure>> BindAsync<TFailure>(
        this Result<TFailure> result,
        Func<CancellationToken, Task<Result<TFailure>>> binder,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(binder, nameof(binder));
        ValidateInitialized(result.IsInitialized);
        return BindValueLessCoreAsync(result, binder, cancellationToken);
    }

    /// <summary>
    /// Binds a Task-wrapped value-less success through an asynchronous callback.
    /// </summary>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="resultTask">The source Result Task.</param>
    /// <param name="binder">The asynchronous binder.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the bound Result.</returns>
    public static Task<Result<TFailure>> BindAsync<TFailure>(
        this Task<Result<TFailure>> resultTask,
        Func<CancellationToken, Task<Result<TFailure>>> binder,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(resultTask, nameof(resultTask));
        ResultValidation.ThrowIfNull(binder, nameof(binder));
        return BindValueLessTaskCoreAsync(resultTask, binder, cancellationToken);
    }

    /// <summary>
    /// Binds a value-less success through an asynchronous valued callback.
    /// </summary>
    /// <typeparam name="TValue">The bound success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="binder">The asynchronous binder.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the bound valued Result.</returns>
    public static Task<Result<TValue, TFailure>> BindAsync<TValue, TFailure>(
        this Result<TFailure> result,
        Func<CancellationToken, Task<Result<TValue, TFailure>>> binder,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(binder, nameof(binder));
        ValidateInitialized(result.IsInitialized);
        return BindValueLessToValueCoreAsync(result, binder, cancellationToken);
    }

    /// <summary>
    /// Binds a Task-wrapped value-less success through an asynchronous valued callback.
    /// </summary>
    /// <typeparam name="TValue">The bound success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="resultTask">The source Result Task.</param>
    /// <param name="binder">The asynchronous binder.</param>
    /// <param name="cancellationToken">The token passed to the callback.</param>
    /// <returns>A Task producing the bound valued Result.</returns>
    public static Task<Result<TValue, TFailure>> BindAsync<TValue, TFailure>(
        this Task<Result<TFailure>> resultTask,
        Func<CancellationToken, Task<Result<TValue, TFailure>>> binder,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(resultTask, nameof(resultTask));
        ResultValidation.ThrowIfNull(binder, nameof(binder));
        return BindValueLessToValueTaskCoreAsync(resultTask, binder, cancellationToken);
    }

    /// <summary>
    /// Matches a value-less Result through asynchronous branch callbacks.
    /// </summary>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TResult">The callback result type.</typeparam>
    /// <param name="result">The source Result.</param>
    /// <param name="onSuccess">The success callback.</param>
    /// <param name="onFailure">The failure callback.</param>
    /// <param name="cancellationToken">The token passed to the active callback.</param>
    /// <returns>A Task producing the active callback value.</returns>
    public static Task<TResult> MatchAsync<TFailure, TResult>(
        this Result<TFailure> result,
        Func<CancellationToken, Task<TResult>> onSuccess,
        Func<TFailure, CancellationToken, Task<TResult>> onFailure,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(onSuccess, nameof(onSuccess));
        ResultValidation.ThrowIfNull(onFailure, nameof(onFailure));
        ValidateInitialized(result.IsInitialized);
        return MatchValueLessCoreAsync(result, onSuccess, onFailure, cancellationToken);
    }

    /// <summary>
    /// Matches a Task-wrapped value-less Result through asynchronous branch callbacks.
    /// </summary>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <typeparam name="TResult">The callback result type.</typeparam>
    /// <param name="resultTask">The source Result Task.</param>
    /// <param name="onSuccess">The success callback.</param>
    /// <param name="onFailure">The failure callback.</param>
    /// <param name="cancellationToken">The token passed to the active callback.</param>
    /// <returns>A Task producing the active callback value.</returns>
    public static Task<TResult> MatchAsync<TFailure, TResult>(
        this Task<Result<TFailure>> resultTask,
        Func<CancellationToken, Task<TResult>> onSuccess,
        Func<TFailure, CancellationToken, Task<TResult>> onFailure,
        CancellationToken cancellationToken = default)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(resultTask, nameof(resultTask));
        ResultValidation.ThrowIfNull(onSuccess, nameof(onSuccess));
        ResultValidation.ThrowIfNull(onFailure, nameof(onFailure));
        return MatchValueLessTaskCoreAsync(resultTask, onSuccess, onFailure, cancellationToken);
    }

#pragma warning restore RCS1231

    private static async Task<Result<TNext, TFailure>> MapCoreAsync<TValue, TFailure, TNext>(
        Result<TValue, TFailure> result,
        Func<TValue, CancellationToken, Task<TNext>> mapper,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        if (result.IsFailure)
        {
            return Result.Fail<TNext, TFailure>(result.FailureReason);
        }

        var value = await RequireCallbackTaskAsync(mapper(result.Value, token)).ConfigureAwait(false);
        return Result.Succeed<TNext, TFailure>(value);
    }

    private static async Task<Result<TNext, TFailure>> MapTaskCoreAsync<TValue, TFailure, TNext>(
        Task<Result<TValue, TFailure>> task,
        Func<TValue, CancellationToken, Task<TNext>> mapper,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var result = await task.ConfigureAwait(false);
        ValidateInitialized(result.IsInitialized);
        return await MapCoreAsync(result, mapper, token).ConfigureAwait(false);
    }

    private static async Task<Result<TNext, TFailure>> BindCoreAsync<TValue, TFailure, TNext>(
        Result<TValue, TFailure> result,
        Func<TValue, CancellationToken, Task<Result<TNext, TFailure>>> binder,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        if (result.IsFailure)
        {
            return Result.Fail<TNext, TFailure>(result.FailureReason);
        }

        var bound = await RequireCallbackTaskAsync(binder(result.Value, token)).ConfigureAwait(false);
        ValidateInitialized(bound.IsInitialized);
        return bound;
    }

    private static async Task<Result<TNext, TFailure>> BindTaskCoreAsync<TValue, TFailure, TNext>(
        Task<Result<TValue, TFailure>> task,
        Func<TValue, CancellationToken, Task<Result<TNext, TFailure>>> binder,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var result = await task.ConfigureAwait(false);
        ValidateInitialized(result.IsInitialized);
        return await BindCoreAsync(result, binder, token).ConfigureAwait(false);
    }

#pragma warning disable RCS1174 // Await keeps callback exceptions faulted and avoids capturing a synchronization context.
    private static async Task<TResult> MatchCoreAsync<TValue, TFailure, TResult>(
        Result<TValue, TFailure> result,
        Func<TValue, CancellationToken, Task<TResult>> onSuccess,
        Func<TFailure, CancellationToken, Task<TResult>> onFailure,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var task = result.IsSuccess ? onSuccess(result.Value, token) : onFailure(result.FailureReason, token);
        return await RequireCallbackTaskAsync(task).ConfigureAwait(false);
    }
#pragma warning restore RCS1174

    private static async Task<TResult> MatchTaskCoreAsync<TValue, TFailure, TResult>(
        Task<Result<TValue, TFailure>> task,
        Func<TValue, CancellationToken, Task<TResult>> onSuccess,
        Func<TFailure, CancellationToken, Task<TResult>> onFailure,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var result = await task.ConfigureAwait(false);
        ValidateInitialized(result.IsInitialized);
        return await MatchCoreAsync(result, onSuccess, onFailure, token).ConfigureAwait(false);
    }

    private static async Task<Result<TValue, TFailure>> EnsureCoreAsync<TValue, TFailure>(
        Result<TValue, TFailure> result,
        Func<TValue, CancellationToken, Task<bool>> predicate,
        TFailure failureReason,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        if (result.IsFailure)
        {
            return result;
        }

        var accepted = await RequireCallbackTaskAsync(predicate(result.Value, token)).ConfigureAwait(false);
        return accepted ? result : Result.Fail<TValue, TFailure>(failureReason);
    }

    private static async Task<Result<TValue, TFailure>> EnsureTaskCoreAsync<TValue, TFailure>(
        Task<Result<TValue, TFailure>> task,
        Func<TValue, CancellationToken, Task<bool>> predicate,
        TFailure failureReason,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var result = await task.ConfigureAwait(false);
        ValidateInitialized(result.IsInitialized);
        return await EnsureCoreAsync(result, predicate, failureReason, token).ConfigureAwait(false);
    }

    private static async Task<Result<TValue, TFailure>> RecoverCoreAsync<TValue, TFailure>(
        Result<TValue, TFailure> result,
        Func<TFailure, CancellationToken, Task<TValue>> recovery,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        if (result.IsSuccess)
        {
            return result;
        }

        var value = await RequireCallbackTaskAsync(recovery(result.FailureReason, token)).ConfigureAwait(false);
        return Result.Succeed<TValue, TFailure>(value);
    }

    private static async Task<Result<TValue, TFailure>> RecoverTaskCoreAsync<TValue, TFailure>(
        Task<Result<TValue, TFailure>> task,
        Func<TFailure, CancellationToken, Task<TValue>> recovery,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var result = await task.ConfigureAwait(false);
        ValidateInitialized(result.IsInitialized);
        return await RecoverCoreAsync(result, recovery, token).ConfigureAwait(false);
    }

    private static async Task<Result<TValue, TFailure>> RecoverWithCoreAsync<TValue, TFailure>(
        Result<TValue, TFailure> result,
        Func<TFailure, CancellationToken, Task<Result<TValue, TFailure>>> recovery,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        if (result.IsSuccess)
        {
            return result;
        }

        var recovered = await RequireCallbackTaskAsync(recovery(result.FailureReason, token)).ConfigureAwait(false);
        ValidateInitialized(recovered.IsInitialized);
        return recovered;
    }

    private static async Task<Result<TValue, TFailure>> RecoverWithTaskCoreAsync<TValue, TFailure>(
        Task<Result<TValue, TFailure>> task,
        Func<TFailure, CancellationToken, Task<Result<TValue, TFailure>>> recovery,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var result = await task.ConfigureAwait(false);
        ValidateInitialized(result.IsInitialized);
        return await RecoverWithCoreAsync(result, recovery, token).ConfigureAwait(false);
    }

    private static async Task<Result<TFailure>> BindValueLessCoreAsync<TFailure>(
        Result<TFailure> result,
        Func<CancellationToken, Task<Result<TFailure>>> binder,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        if (result.IsFailure)
        {
            return Result.Fail(result.FailureReason);
        }

        var bound = await RequireCallbackTaskAsync(binder(token)).ConfigureAwait(false);
        ValidateInitialized(bound.IsInitialized);
        return bound;
    }

    private static async Task<Result<TFailure>> BindValueLessTaskCoreAsync<TFailure>(
        Task<Result<TFailure>> task,
        Func<CancellationToken, Task<Result<TFailure>>> binder,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var result = await task.ConfigureAwait(false);
        ValidateInitialized(result.IsInitialized);
        return await BindValueLessCoreAsync(result, binder, token).ConfigureAwait(false);
    }

    private static async Task<Result<TValue, TFailure>> BindValueLessToValueCoreAsync<TValue, TFailure>(
        Result<TFailure> result,
        Func<CancellationToken, Task<Result<TValue, TFailure>>> binder,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        if (result.IsFailure)
        {
            return Result.Fail<TValue, TFailure>(result.FailureReason);
        }

        var bound = await RequireCallbackTaskAsync(binder(token)).ConfigureAwait(false);
        ValidateInitialized(bound.IsInitialized);
        return bound;
    }

    private static async Task<Result<TValue, TFailure>> BindValueLessToValueTaskCoreAsync<TValue, TFailure>(
        Task<Result<TFailure>> task,
        Func<CancellationToken, Task<Result<TValue, TFailure>>> binder,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var result = await task.ConfigureAwait(false);
        ValidateInitialized(result.IsInitialized);
        return await BindValueLessToValueCoreAsync(result, binder, token).ConfigureAwait(false);
    }

#pragma warning disable RCS1174 // Await keeps callback exceptions faulted and avoids capturing a synchronization context.
    private static async Task<TResult> MatchValueLessCoreAsync<TFailure, TResult>(
        Result<TFailure> result,
        Func<CancellationToken, Task<TResult>> onSuccess,
        Func<TFailure, CancellationToken, Task<TResult>> onFailure,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var task = result.IsSuccess ? onSuccess(token) : onFailure(result.FailureReason, token);
        return await RequireCallbackTaskAsync(task).ConfigureAwait(false);
    }
#pragma warning restore RCS1174

    private static async Task<TResult> MatchValueLessTaskCoreAsync<TFailure, TResult>(
        Task<Result<TFailure>> task,
        Func<CancellationToken, Task<TResult>> onSuccess,
        Func<TFailure, CancellationToken, Task<TResult>> onFailure,
        CancellationToken token)
        where TFailure : struct, Enum
    {
        var result = await task.ConfigureAwait(false);
        ValidateInitialized(result.IsInitialized);
        return await MatchValueLessCoreAsync(result, onSuccess, onFailure, token).ConfigureAwait(false);
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static Task<T> RequireCallbackTaskAsync<T>(Task<T>? task)
    {
        return task ?? throw new InvalidOperationException("The callback returned a null Task.");
    }
}