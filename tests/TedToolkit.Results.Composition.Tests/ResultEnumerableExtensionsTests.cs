// -----------------------------------------------------------------------
// <copyright file="ResultEnumerableExtensionsTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Collections.ObjectModel;

namespace TedToolkit.Results.Tests;

internal sealed class ResultEnumerableExtensionsTests
{
    private static readonly ReadOnlyCollection<int> _one = new([1,]);

    private static readonly ReadOnlyCollection<int> _oneTwo = new([1, 2,]);

    private static readonly ReadOnlyCollection<int> _oneTwoThree = new([1, 2, 3,]);

    /// <summary>
    /// Verifies that Sequence materializes successful values in order into a read-only collection.
    /// </summary>
    [Test]
    public async Task Should_sequence_success_values_in_order()
    {
        Result<int, TestFailure>[] results =
        [
            Result.Succeed<int, TestFailure>(1),
            Result.Succeed<int, TestFailure>(2),
        ];

        var sequenced = results.Sequence();

        await Assert.That(sequenced.Value).IsTypeOf<ReadOnlyCollection<int>>();
        await Assert.That(sequenced.Value).IsEquivalentTo(_oneTwo);
    }

    /// <summary>
    /// Verifies that Sequence returns an empty read-only success for empty input.
    /// </summary>
    [Test]
    public async Task Should_sequence_empty_input_to_empty_success()
    {
        var sequenced = Array.Empty<Result<int, TestFailure>>().Sequence();

        await Assert.That(sequenced.Value).IsEmpty();
    }

    /// <summary>
    /// Verifies that Sequence stops at the first failure in enumeration order.
    /// </summary>
    [Test]
    public async Task Should_stop_sequence_at_first_failure()
    {
        var yielded = 0;

        var sequenced = GetResults().Sequence();

        await Assert.That(sequenced.FailureReason).IsEqualTo(TestFailure.FIRST);
        await Assert.That(yielded).IsEqualTo(2);

        IEnumerable<Result<int, TestFailure>> GetResults()
        {
            yielded++;
            yield return Result.Succeed<int, TestFailure>(1);
            yielded++;
            yield return Result.Fail<int, TestFailure>(TestFailure.FIRST);
            yielded++;
            yield return Result.Fail<int, TestFailure>(TestFailure.SECOND);
        }
    }

    /// <summary>
    /// Verifies that Traverse invokes the selector in order and stops at its first failure.
    /// </summary>
    [Test]
    public async Task Should_traverse_in_order_until_first_failure()
    {
        var selected = new List<int>();

        var traversed = _oneTwoThree.Traverse(value =>
        {
            selected.Add(value);
            return value == 2
                ? Result.Fail<string, TestFailure>(TestFailure.SECOND)
                : Result.Succeed<string, TestFailure>(value.ToString(System.Globalization.CultureInfo.InvariantCulture));
        });

        await Assert.That(traversed.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(selected).IsEquivalentTo(_oneTwo);
    }

    /// <summary>
    /// Verifies that Combine succeeds for empty input and stops at the first value-less failure.
    /// </summary>
    [Test]
    public async Task Should_combine_value_less_results_with_fail_fast_semantics()
    {
        var empty = Array.Empty<Result<TestFailure>>().Combine();
        var failure = new[]
        {
            Result.Succeed<TestFailure>(),
            Result.Fail(TestFailure.SECOND),
            Result.Fail(TestFailure.FIRST),
        }.Combine();

        await Assert.That(empty.IsSuccess).IsTrue();
        await Assert.That(failure.FailureReason).IsEqualTo(TestFailure.SECOND);
    }

    /// <summary>
    /// Verifies that collection composition rejects null arguments and uninitialized elements.
    /// </summary>
    [Test]
    public async Task Should_reject_invalid_collection_composition_input()
    {
        IEnumerable<Result<int, TestFailure>> source = null!;
        Result<int, TestFailure>[] uninitializedResults = [default,];

        await Assert.That(() => source.Sequence()).Throws<ArgumentNullException>();
        await Assert.That(() => _one.Traverse<int, int, TestFailure>(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => uninitializedResults.Sequence())
            .Throws<InvalidOperationException>();
        await Assert.That(() => _one.Traverse(_ => default(Result<int, TestFailure>)))
            .Throws<InvalidOperationException>();
    }
}