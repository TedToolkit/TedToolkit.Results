// -----------------------------------------------------------------------
// <copyright file="ResultMapTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultMapTests
{
    /// <summary>
    /// Verifies that Map transforms a successful value exactly once.
    /// </summary>
    [Test]
    public async Task Should_map_value_when_result_succeeds()
    {
        var calls = 0;
        var result = Result.Succeed<string, TestFailure>("value");

        var mapped = result.Map(value =>
        {
            calls++;
            return value.Length;
        });

        await Assert.That(mapped.Value).IsEqualTo(5);
        await Assert.That(calls).IsEqualTo(1);
    }

    /// <summary>
    /// Verifies that Map propagates a failure without invoking the mapper.
    /// </summary>
    [Test]
    public async Task Should_propagate_failure_without_mapping()
    {
        var calls = 0;
        var result = Result.Fail<string, TestFailure>(TestFailure.SECOND);

        var mapped = result.Map(value =>
        {
            calls++;
            return value.Length;
        });

        await Assert.That(mapped.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(calls).IsEqualTo(0);
    }

    /// <summary>
    /// Verifies that Map permits a nullable mapped success value.
    /// </summary>
    [Test]
    public async Task Should_map_success_to_null_when_mapper_returns_null()
    {
        var result = Result.Succeed<string, TestFailure>("value");

        var mapped = result.Map(_ => (string?)null);

        await Assert.That(mapped.Value).IsNull();
    }

    /// <summary>
    /// Verifies that Map validates its delegate before reading the source branch.
    /// </summary>
    [Test]
    public async Task Should_reject_null_mapper_before_reading_source()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.Map((Func<string, int>)null!)).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that Map propagates the exact exception thrown by the mapper.
    /// </summary>
    [Test]
    public async Task Should_propagate_same_exception_when_mapper_throws()
    {
        var expected = new InvalidOperationException("mapper");
        var result = Result.Succeed<string, TestFailure>("value");
        InvalidOperationException? actual = null;

        try
        {
            _ = result.Map((Func<string, int>)(_ => throw expected));
        }
        catch (InvalidOperationException caught)
        {
            actual = caught;
        }

        await Assert.That(actual).IsSameReferenceAs(expected);
    }

    /// <summary>
    /// Verifies that Map rejects an uninitialized source without invoking the mapper.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_source_without_mapping()
    {
        var calls = 0;
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.Map(value =>
        {
            calls++;
            return value.Length;
        })).Throws<InvalidOperationException>();
        await Assert.That(calls).IsEqualTo(0);
    }
}