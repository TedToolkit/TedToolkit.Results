// -----------------------------------------------------------------------
// <copyright file="ResultRecoveryTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultRecoveryTests
{
    /// <summary>
    /// Verifies that Recover converts a failure into a successful value.
    /// </summary>
    [Test]
    public async Task Should_recover_failure_to_success_value()
    {
        var result = Result.Fail<string, TestFailure>(TestFailure.SECOND);

        var recovered = result.Recover(reason => reason.ToString());

        await Assert.That(recovered.Value).IsEqualTo("SECOND");
    }

    /// <summary>
    /// Verifies that recovery callbacks do not execute for an existing success.
    /// </summary>
    [Test]
    public async Task Should_preserve_success_without_recovery()
    {
        var calls = 0;
        var result = Result.Succeed<string, TestFailure>("value");

        var recovered = result.Recover(_ =>
        {
            calls++;
            return "recovered";
        });
        var recoveredWith = result.RecoverWith(_ =>
        {
            calls++;
            return Result.Succeed<string, TestFailure>("recovered");
        });

        await Assert.That(recovered.Value).IsEqualTo("value");
        await Assert.That(recoveredWith.Value).IsEqualTo("value");
        await Assert.That(calls).IsEqualTo(0);
    }

    /// <summary>
    /// Verifies that RecoverWith returns either initialized branch produced by its callback.
    /// </summary>
    [Test]
    public async Task Should_return_initialized_result_from_recovery_callback()
    {
        var result = Result.Fail<string, TestFailure>(TestFailure.FIRST);

        var success = result.RecoverWith(_ => Result.Succeed<string, TestFailure>("recovered"));
        var failure = result.RecoverWith(_ => Result.Fail<string, TestFailure>(TestFailure.SECOND));

        await Assert.That(success.Value).IsEqualTo("recovered");
        await Assert.That(failure.FailureReason).IsEqualTo(TestFailure.SECOND);
    }

    /// <summary>
    /// Verifies that RecoverWith rejects an uninitialized callback Result.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_result_from_recovery_callback()
    {
        var result = Result.Fail<string, TestFailure>(TestFailure.FIRST);

        await Assert.That(() => result.RecoverWith(_ => default)).Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that recovery validates callbacks before reading the source branch.
    /// </summary>
    [Test]
    public async Task Should_reject_null_recovery_before_reading_source()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.Recover(null!)).Throws<ArgumentNullException>();
        await Assert.That(() => result.RecoverWith(null!)).Throws<ArgumentNullException>();
    }
}