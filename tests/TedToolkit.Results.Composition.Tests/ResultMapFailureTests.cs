// -----------------------------------------------------------------------
// <copyright file="ResultMapFailureTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultMapFailureTests
{
    /// <summary>
    /// Verifies that MapFailure transforms a failure reason exactly once.
    /// </summary>
    [Test]
    public async Task Should_map_reason_when_result_fails()
    {
        var calls = 0;
        var result = Result.Fail<string, TestFailure>(TestFailure.SECOND);

        var mapped = result.MapFailure(reason =>
        {
            calls++;
            return reason == TestFailure.SECOND ? OtherFailure.MAPPED : OtherFailure.OTHER;
        });

        await Assert.That(mapped.FailureReason).IsEqualTo(OtherFailure.MAPPED);
        await Assert.That(calls).IsEqualTo(1);
    }

    /// <summary>
    /// Verifies that MapFailure preserves a success without invoking the mapper.
    /// </summary>
    [Test]
    public async Task Should_preserve_success_without_mapping_failure()
    {
        var calls = 0;
        var result = Result.Succeed<string, TestFailure>("value");

        var mapped = result.MapFailure(_ =>
        {
            calls++;
            return OtherFailure.OTHER;
        });

        await Assert.That(mapped.Value).IsEqualTo("value");
        await Assert.That(calls).IsEqualTo(0);
    }

    /// <summary>
    /// Verifies that MapFailure validates a null mapper before reading the source branch.
    /// </summary>
    [Test]
    public async Task Should_reject_null_failure_mapper_before_reading_source()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.MapFailure((Func<TestFailure, OtherFailure>)null!))
            .Throws<ArgumentNullException>();
    }

    /// <summary>
    /// Verifies that MapFailure rejects an uninitialized source.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_source_when_mapping_failure()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.MapFailure(_ => OtherFailure.MAPPED)).Throws<InvalidOperationException>();
    }

    private enum OtherFailure
    {
        OTHER = 0,

        MAPPED = 1,
    }
}