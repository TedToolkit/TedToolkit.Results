// -----------------------------------------------------------------------
// <copyright file="ResultBranchConsumptionTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultBranchConsumptionTests
{
    /// <summary>
    /// Verifies that IsFailure is the complement of IsSuccess for initialized results.
    /// </summary>
    [Test]
    public async Task Should_report_failure_state_for_initialized_results()
    {
        var success = Result.Succeed<string, TestFailure>("value");
        var failure = Result.Fail<string, TestFailure>(TestFailure.FIRST);

        await Assert.That(success.IsFailure).IsFalse();
        await Assert.That(failure.IsFailure).IsTrue();
    }

    /// <summary>
    /// Verifies that Match invokes only the active success callback.
    /// </summary>
    [Test]
    public async Task Should_invoke_only_success_callback_when_matching_success()
    {
        var successCalls = 0;
        var failureCalls = 0;
        var result = Result.Succeed<string, TestFailure>("value");

        var matched = result.Match(
            value =>
            {
                successCalls++;
                return value.Length;
            },
            _ =>
            {
                failureCalls++;
                return -1;
            });

        await Assert.That(matched).IsEqualTo(5);
        await Assert.That(successCalls).IsEqualTo(1);
        await Assert.That(failureCalls).IsEqualTo(0);
    }

    /// <summary>
    /// Verifies that Match invokes only the active failure callback.
    /// </summary>
    [Test]
    public async Task Should_invoke_only_failure_callback_when_matching_failure()
    {
        var successCalls = 0;
        var failureCalls = 0;
        var result = Result.Fail<string, TestFailure>(TestFailure.SECOND);

        var matched = result.Match(
            _ =>
            {
                successCalls++;
                return -1;
            },
            failure =>
            {
                failureCalls++;
                return (int)failure;
            });

        await Assert.That(matched).IsEqualTo(1);
        await Assert.That(successCalls).IsEqualTo(0);
        await Assert.That(failureCalls).IsEqualTo(1);
    }

    /// <summary>
    /// Verifies that Match validates both callbacks before inspecting the active branch.
    /// </summary>
    [Test]
    public async Task Should_reject_null_callback_when_matching_either_branch()
    {
        var success = Result.Succeed<string, TestFailure>("value");
        var failure = Result.Fail<string, TestFailure>(TestFailure.FIRST);

        await Assert.That(() => success.Match((Func<string, int>)null!, _ => 0)).Throws<ArgumentNullException>();
        await Assert.That(() => success.Match(_ => 0, null!)).Throws<ArgumentNullException>();
        await Assert.That(() => failure.Match((Func<string, int>)null!, _ => 0)).Throws<ArgumentNullException>();
        await Assert.That(() => failure.Match(_ => 0, null!)).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that Try methods return only the active branch payload.
    /// </summary>
    [Test]
    public async Task Should_try_get_only_active_branch_payload()
    {
        var success = Result.Succeed<string?, TestFailure>(null);
        var failure = Result.Fail<string?, TestFailure>(TestFailure.SECOND);

        var successHasValue = success.TryGetValue(out var successValue);
        var successHasFailure = success.TryGetFailure(out var successFailure);
        var failureHasValue = failure.TryGetValue(out var failureValue);
        var failureHasFailure = failure.TryGetFailure(out var failureReason);

        await Assert.That(successHasValue).IsTrue();
        await Assert.That(successValue).IsNull();
        await Assert.That(successHasFailure).IsFalse();
        await Assert.That(successFailure).IsEqualTo(default(TestFailure));
        await Assert.That(failureHasValue).IsFalse();
        await Assert.That(failureValue).IsNull();
        await Assert.That(failureHasFailure).IsTrue();
        await Assert.That(failureReason).IsEqualTo(TestFailure.SECOND);
    }

    /// <summary>
    /// Verifies that every branch-consumption API rejects an uninitialized result.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_result_when_consuming_branch()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.IsFailure).Throws<InvalidOperationException>();
        await Assert.That(() => result.Match(_ => 0, _ => 0)).Throws<InvalidOperationException>();
        await Assert.That(() => result.TryGetValue(out _)).Throws<InvalidOperationException>();
        await Assert.That(() => result.TryGetFailure(out _)).Throws<InvalidOperationException>();
    }
}