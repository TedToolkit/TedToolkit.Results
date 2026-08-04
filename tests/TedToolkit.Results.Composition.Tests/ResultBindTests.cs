// -----------------------------------------------------------------------
// <copyright file="ResultBindTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultBindTests
{
    /// <summary>
    /// Verifies that Bind returns the initialized Result produced for a success.
    /// </summary>
    [Test]
    public async Task Should_return_bound_result_when_source_succeeds()
    {
        var result = Result.Succeed<string, TestFailure>("value");

        var boundSuccess = result.Bind(value => Result.Succeed<int, TestFailure>(value.Length));
        var boundFailure = result.Bind(_ => Result.Fail<int, TestFailure>(TestFailure.SECOND));

        await Assert.That(boundSuccess.Value).IsEqualTo(5);
        await Assert.That(boundFailure.FailureReason).IsEqualTo(TestFailure.SECOND);
    }

    /// <summary>
    /// Verifies that Bind propagates a source failure without invoking the binder.
    /// </summary>
    [Test]
    public async Task Should_propagate_failure_without_binding()
    {
        var calls = 0;
        var result = Result.Fail<string, TestFailure>(TestFailure.FIRST);

        var bound = result.Bind(value =>
        {
            calls++;
            return Result.Succeed<int, TestFailure>(value.Length);
        });

        await Assert.That(bound.FailureReason).IsEqualTo(TestFailure.FIRST);
        await Assert.That(calls).IsEqualTo(0);
    }

    /// <summary>
    /// Verifies that Bind rejects an uninitialized Result returned by the binder.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_result_returned_by_binder()
    {
        var result = Result.Succeed<string, TestFailure>("value");

        await Assert.That(() => result.Bind(_ => default(Result<int, TestFailure>)))
            .Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that Bind validates a null binder before reading the source branch.
    /// </summary>
    [Test]
    public async Task Should_reject_null_binder_before_reading_source()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.Bind((Func<string, Result<int, TestFailure>>)null!))
            .Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that Bind rejects an uninitialized source without invoking the binder.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_source_without_binding()
    {
        var calls = 0;
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.Bind(value =>
        {
            calls++;
            return Result.Succeed<int, TestFailure>(value.Length);
        })).Throws<InvalidOperationException>();
        await Assert.That(calls).IsEqualTo(0);
    }
}