// -----------------------------------------------------------------------
// <copyright file="ValueLessResultBindTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ValueLessResultBindTests
{
    /// <summary>
    /// Verifies that a value-less success can bind into either Result arity.
    /// </summary>
    [Test]
    public async Task Should_bind_value_less_success_into_either_result_arity()
    {
        var result = Result.Succeed<TestFailure>();

        var valueLess = result.Bind(() => Result.Fail(TestFailure.SECOND));
        var valued = result.Bind(() => Result.Succeed<string, TestFailure>("value"));

        await Assert.That(valueLess.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(valued.Value).IsEqualTo("value");
    }

    /// <summary>
    /// Verifies that a value-less failure propagates without invoking either binder.
    /// </summary>
    [Test]
    public async Task Should_propagate_value_less_failure_without_binding()
    {
        var calls = 0;
        var result = Result.Fail(TestFailure.FIRST);

        var valueLess = result.Bind(() =>
        {
            calls++;
            return Result.Succeed<TestFailure>();
        });
        var valued = result.Bind(() =>
        {
            calls++;
            return Result.Succeed<string, TestFailure>("value");
        });

        await Assert.That(valueLess.FailureReason).IsEqualTo(TestFailure.FIRST);
        await Assert.That(valued.FailureReason).IsEqualTo(TestFailure.FIRST);
        await Assert.That(calls).IsEqualTo(0);
    }

    /// <summary>
    /// Verifies that value-less bind rejects an uninitialized callback Result.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_result_from_value_less_binder()
    {
        var result = Result.Succeed<TestFailure>();

        await Assert.That(() => result.Bind(() => default)).Throws<InvalidOperationException>();
        await Assert.That(() => result.Bind(() => default(Result<string, TestFailure>)))
            .Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that value-less Bind validates either binder before reading the source branch.
    /// </summary>
    [Test]
    public async Task Should_reject_null_binder_before_reading_value_less_source()
    {
        Result<TestFailure> result = default;

        await Assert.That(() => result.Bind((Func<Result<TestFailure>>)null!))
            .Throws<ArgumentNullException>();
        await Assert.That(() => result.Bind((Func<Result<string, TestFailure>>)null!))
            .Throws<ArgumentNullException>();
    }
}