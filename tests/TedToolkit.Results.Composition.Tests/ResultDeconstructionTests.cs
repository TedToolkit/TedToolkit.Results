// -----------------------------------------------------------------------
// <copyright file="ResultDeconstructionTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultDeconstructionTests
{
    /// <summary>
    /// Verifies that deconstruction exposes only the successful branch payload.
    /// </summary>
    [Test]
    public async Task Should_deconstruct_success_with_inactive_failure_default()
    {
        var (isSuccess, value, failureReason) = Result.Succeed<string?, TestFailure>(null);

        await Assert.That(isSuccess).IsTrue();
        await Assert.That(value).IsNull();
        await Assert.That(failureReason).IsEqualTo(default(TestFailure));
    }

    /// <summary>
    /// Verifies that deconstruction exposes only the failed branch payload.
    /// </summary>
    [Test]
    public async Task Should_deconstruct_failure_with_inactive_value_default()
    {
        var (isSuccess, value, failureReason) = Result.Fail<string, TestFailure>(TestFailure.SECOND);

        await Assert.That(isSuccess).IsFalse();
        await Assert.That(value).IsNull();
        await Assert.That(failureReason).IsEqualTo(TestFailure.SECOND);
    }

    /// <summary>
    /// Verifies that positional switch arms select the active Result branch.
    /// </summary>
    [Test]
    public async Task Should_select_active_branch_when_using_positional_switch()
    {
        var success = Result.Succeed<string, TestFailure>("value");
        var failure = Result.Fail<string, TestFailure>(TestFailure.FIRST);

        var successText = Format(success);
        var failureText = Format(failure);

        await Assert.That(successText).IsEqualTo("value");
        await Assert.That(failureText).IsEqualTo("FIRST");
    }

    /// <summary>
    /// Verifies that an uninitialized result cannot be deconstructed.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_result_when_deconstructing()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.Deconstruct(out _, out _, out _)).Throws<InvalidOperationException>();
    }

    private static string Format(in Result<string, TestFailure> result)
    {
        return result switch
        {
            (true, var value, _) => value,
            (false, _, var failureReason) => failureReason.ToString(),
        };
    }
}