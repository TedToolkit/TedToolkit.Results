// -----------------------------------------------------------------------
// <copyright file="ValueLessResultConstructionTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ValueLessResultConstructionTests
{
    /// <summary>
    /// Verifies that the value-less success factory creates an initialized success.
    /// </summary>
    [Test]
    public async Task Should_create_value_less_success_with_factory()
    {
        var result = Result.Succeed<TestFailure>();

        await Assert.That(result.IsInitialized).IsTrue();
        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsFailure).IsFalse();
        await Assert.That(() => result.FailureReason).Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that construction, factory creation, and conversion create the same failure.
    /// </summary>
    [Test]
    public async Task Should_create_value_less_failure_from_declared_reason()
    {
        var constructed = new Result<TestFailure>(TestFailure.SECOND);
        var factory = Result.Fail(TestFailure.SECOND);
        Result<TestFailure> converted = TestFailure.SECOND;

        await Assert.That(constructed.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(factory).IsEqualTo(constructed);
        await Assert.That(converted).IsEqualTo(constructed);
    }

    /// <summary>
    /// Verifies that value-less failures reject an undefined reason.
    /// </summary>
    [Test]
    public async Task Should_reject_undefined_value_less_failure_reason()
    {
        await Assert.That(() => _ = CreateFailure((TestFailure)999))
            .Throws<ArgumentOutOfRangeException>();
    }

    /// <summary>
    /// Verifies that a default value-less Result remains uninitialized and branch-inaccessible.
    /// </summary>
    [Test]
    public async Task Should_reject_branch_access_when_value_less_result_is_default()
    {
        Result<TestFailure> result = default;

        await Assert.That(result.IsInitialized).IsFalse();
        await Assert.That(() => result.IsSuccess).Throws<InvalidOperationException>();
        await Assert.That(() => result.IsFailure).Throws<InvalidOperationException>();
        await Assert.That(() => result.FailureReason).Throws<InvalidOperationException>();
        await Assert.That(() => result.ToString()).Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that value-less record equality includes initialization state and failure reason.
    /// </summary>
    [Test]
    public async Task Should_compare_value_less_results_by_state_and_reason()
    {
        Result<TestFailure> firstDefault = default;
        Result<TestFailure> secondDefault = default;
        var success = Result.Succeed<TestFailure>();
        var firstFailure = Result.Fail(TestFailure.FIRST);
        var secondFailure = Result.Fail(TestFailure.FIRST);

        await Assert.That(firstDefault).IsEqualTo(secondDefault);
        await Assert.That(firstDefault).IsNotEqualTo(success);
        await Assert.That(firstFailure).IsEqualTo(secondFailure);
        await Assert.That(firstFailure).IsNotEqualTo(success);
    }

    private static Result<TestFailure> CreateFailure(TestFailure reason)
    {
        return new(reason);
    }
}