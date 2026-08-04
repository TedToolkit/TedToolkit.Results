// -----------------------------------------------------------------------
// <copyright file="ValueLessResultConsumptionTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;

namespace TedToolkit.Results.Tests;

internal sealed class ValueLessResultConsumptionTests
{
    /// <summary>
    /// Verifies that Match invokes only the active value-less branch callback.
    /// </summary>
    [Test]
    public async Task Should_match_only_active_value_less_branch()
    {
        var success = Result.Succeed<TestFailure>();
        var failure = Result.Fail(TestFailure.SECOND);

        var successText = success.Match(() => "success", reason => reason.ToString());
        var failureText = failure.Match(() => "success", reason => reason.ToString());

        await Assert.That(successText).IsEqualTo("success");
        await Assert.That(failureText).IsEqualTo("SECOND");
    }

    /// <summary>
    /// Verifies that value-less Match validates both callbacks before reading the source branch.
    /// </summary>
    [Test]
    public async Task Should_reject_null_callback_before_matching_value_less_result()
    {
        Result<TestFailure> result = default;

        await Assert.That(() => result.Match((Func<int>)null!, _ => 0)).Throws<ArgumentNullException>();
        await Assert.That(() => result.Match(() => 0, (Func<TestFailure, int>)null!))
            .Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that TryGetFailure and deconstruction expose only the active failure.
    /// </summary>
    [Test]
    public async Task Should_consume_value_less_branches_without_inactive_access()
    {
        var success = Result.Succeed<TestFailure>();
        var failure = Result.Fail(TestFailure.SECOND);

        var successHasFailure = success.TryGetFailure(out var successReason);
        var failureHasFailure = failure.TryGetFailure(out var failureReason);
        var (isSuccess, deconstructedSuccessReason) = success;
        var (isFailureSuccess, deconstructedFailureReason) = failure;

        await Assert.That(successHasFailure).IsFalse();
        await Assert.That(successReason).IsEqualTo(default(TestFailure));
        await Assert.That(failureHasFailure).IsTrue();
        await Assert.That(failureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(isSuccess).IsTrue();
        await Assert.That(deconstructedSuccessReason).IsEqualTo(default(TestFailure));
        await Assert.That(isFailureSuccess).IsFalse();
        await Assert.That(deconstructedFailureReason).IsEqualTo(TestFailure.SECOND);
    }

    /// <summary>
    /// Verifies that value-less conditional syntax follows the active branch.
    /// </summary>
    [Test]
    public async Task Should_evaluate_value_less_conditional_syntax()
    {
        var success = Result.Succeed<TestFailure>();
        var failure = Result.Fail(TestFailure.FIRST);

        await Assert.That(EvaluatePositive(success)).IsTrue();
        await Assert.That(EvaluateNegative(success)).IsFalse();
        await Assert.That(EvaluatePositive(failure)).IsFalse();
        await Assert.That(EvaluateNegative(failure)).IsTrue();
    }

    /// <summary>
    /// Verifies that value-less branch consumers reject an uninitialized Result.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_value_less_branch_consumption()
    {
        Result<TestFailure> result = default;

        await Assert.That(() => result.Match(() => 0, _ => 0)).Throws<InvalidOperationException>();
        await Assert.That(() => result.TryGetFailure(out _)).Throws<InvalidOperationException>();
        await Assert.That(() => result.Deconstruct(out _, out _)).Throws<InvalidOperationException>();
        await Assert.That(() => EvaluatePositive(result)).Throws<InvalidOperationException>();
        await Assert.That(() => EvaluateNegative(result)).Throws<InvalidOperationException>();
    }

    [SuppressMessage(
        "Style",
        "RCS1073",
        Justification = "The explicit conditional is the behavior under test.")]
    private static bool EvaluatePositive(in Result<TestFailure> result)
    {
        if (result)
        {
            return true;
        }

        return false;
    }

    private static bool EvaluateNegative(in Result<TestFailure> result)
    {
        return !result;
    }
}