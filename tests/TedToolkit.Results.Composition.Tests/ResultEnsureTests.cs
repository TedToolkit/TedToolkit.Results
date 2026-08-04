// -----------------------------------------------------------------------
// <copyright file="ResultEnsureTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultEnsureTests
{
    /// <summary>
    /// Verifies that Ensure preserves a success that satisfies its predicate.
    /// </summary>
    [Test]
    public async Task Should_preserve_success_when_predicate_is_satisfied()
    {
        var value = new TestValue();
        var result = Result.Succeed<TestValue, TestFailure>(value);

        var ensured = result.Ensure(candidate => ReferenceEquals(candidate, value), TestFailure.FIRST);

        await Assert.That(ensured.Value).IsSameReferenceAs(value);
    }

    /// <summary>
    /// Verifies that Ensure returns either a fixed or computed reason when validation fails.
    /// </summary>
    [Test]
    public async Task Should_fail_success_when_predicate_is_not_satisfied()
    {
        var factoryCalls = 0;
        var result = Result.Succeed<string, TestFailure>("value");

        var fixedFailure = result.Ensure(_ => false, TestFailure.FIRST);
        var computedFailure = result.Ensure(
            _ => false,
            _ =>
            {
                factoryCalls++;
                return TestFailure.SECOND;
            });

        await Assert.That(fixedFailure.FailureReason).IsEqualTo(TestFailure.FIRST);
        await Assert.That(computedFailure.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(factoryCalls).IsEqualTo(1);
    }

    /// <summary>
    /// Verifies that Ensure propagates an existing failure without invoking callbacks.
    /// </summary>
    [Test]
    public async Task Should_propagate_existing_failure_without_validation()
    {
        var predicateCalls = 0;
        var factoryCalls = 0;
        var result = Result.Fail<string, TestFailure>(TestFailure.FIRST);

        var ensured = result.Ensure(
            _ =>
            {
                predicateCalls++;
                return false;
            },
            _ =>
            {
                factoryCalls++;
                return TestFailure.SECOND;
            });

        await Assert.That(ensured.FailureReason).IsEqualTo(TestFailure.FIRST);
        await Assert.That(predicateCalls).IsEqualTo(0);
        await Assert.That(factoryCalls).IsEqualTo(0);
    }

    /// <summary>
    /// Verifies that Ensure validates delegates before reading the source branch.
    /// </summary>
    [Test]
    public async Task Should_reject_null_ensure_delegate_before_reading_source()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.Ensure(null!, TestFailure.FIRST)).Throws<ArgumentNullException>();
        await Assert.That(() => result.Ensure(_ => true, null!)).Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that Ensure rejects an uninitialized source after validating delegates.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_source_when_ensuring()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.Ensure(_ => true, TestFailure.FIRST)).Throws<InvalidOperationException>();
    }
}