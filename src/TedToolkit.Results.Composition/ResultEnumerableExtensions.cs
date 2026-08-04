// -----------------------------------------------------------------------
// <copyright file="ResultEnumerableExtensions.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;

using static TedToolkit.Results.ResultValidation;

namespace TedToolkit.Results;

/// <summary>
/// Provides ordered fail-fast composition for Result collections.
/// </summary>
public static class ResultEnumerableExtensions
{
    /// <summary>
    /// Sequences valued Results into one Result containing ordered success values.
    /// </summary>
    /// <typeparam name="TValue">The success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="results">The Results to sequence.</param>
    /// <returns>An ordered read-only value list, or the first failure.</returns>
    public static Result<IReadOnlyList<TValue>, TFailure> Sequence<TValue, TFailure>(
        this IEnumerable<Result<TValue, TFailure>> results)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(results, nameof(results));
        var values = new List<TValue>();
        var position = 0;

        foreach (var result in results)
        {
            ValidateInitialized(result.IsInitialized, position);
            if (result.IsFailure)
            {
                return Result.Fail<IReadOnlyList<TValue>, TFailure>(result.FailureReason);
            }

            values.Add(result.Value);
            position++;
        }

        IReadOnlyList<TValue> output = new ReadOnlyCollection<TValue>(values);
        return Result.Succeed<IReadOnlyList<TValue>, TFailure>(output);
    }

    /// <summary>
    /// Traverses source items through a Result-producing selector in enumeration order.
    /// </summary>
    /// <typeparam name="TSource">The source item type.</typeparam>
    /// <typeparam name="TValue">The selected success value type.</typeparam>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="source">The source items.</param>
    /// <param name="selector">The Result-producing selector.</param>
    /// <returns>An ordered read-only value list, or the first failure.</returns>
    public static Result<IReadOnlyList<TValue>, TFailure> Traverse<TSource, TValue, TFailure>(
        this IEnumerable<TSource> source,
        Func<TSource, Result<TValue, TFailure>> selector)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(source, nameof(source));
        ResultValidation.ThrowIfNull(selector, nameof(selector));
        var values = new List<TValue>();
        var position = 0;

        foreach (var item in source)
        {
            var result = selector(item);
            ValidateInitialized(result.IsInitialized, position);
            if (result.IsFailure)
            {
                return Result.Fail<IReadOnlyList<TValue>, TFailure>(result.FailureReason);
            }

            values.Add(result.Value);
            position++;
        }

        IReadOnlyList<TValue> output = new ReadOnlyCollection<TValue>(values);
        return Result.Succeed<IReadOnlyList<TValue>, TFailure>(output);
    }

    /// <summary>
    /// Combines value-less Results and returns the first failure in enumeration order.
    /// </summary>
    /// <typeparam name="TFailure">The failure enum type.</typeparam>
    /// <param name="results">The value-less Results to combine.</param>
    /// <returns>Success when every input succeeds; otherwise, the first failure.</returns>
    public static Result<TFailure> Combine<TFailure>(this IEnumerable<Result<TFailure>> results)
        where TFailure : struct, Enum
    {
        ResultValidation.ThrowIfNull(results, nameof(results));
        var position = 0;

        foreach (var result in results)
        {
            ValidateInitialized(result.IsInitialized, position);
            if (result.IsFailure)
            {
                return Result.Fail(result.FailureReason);
            }

            position++;
        }

        return Result.Succeed<TFailure>();
    }
}