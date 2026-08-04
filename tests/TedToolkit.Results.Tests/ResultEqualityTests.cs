// -----------------------------------------------------------------------
// <copyright file="ResultEqualityTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultEqualityTests
{
    /// <summary>
    /// Verifies that success records compare their contained values.
    /// </summary>
    [Test]
    public async Task Should_be_equal_when_success_values_are_equal()
    {
        var first = new Result<string, TestFailure>("value");
        var second = new Result<string, TestFailure>("value");

        await Assert.That(first).IsEqualTo(second);
    }

    /// <summary>
    /// Verifies that failure records compare their contained reasons.
    /// </summary>
    [Test]
    public async Task Should_be_equal_when_failure_reasons_are_equal()
    {
        var first = new Result<string, TestFailure>(TestFailure.FIRST);
        var second = new Result<string, TestFailure>(TestFailure.FIRST);

        await Assert.That(first).IsEqualTo(second);
    }

    /// <summary>
    /// Verifies that records from different branches are not equal.
    /// </summary>
    [Test]
    public async Task Should_not_be_equal_when_branches_differ()
    {
        var success = new Result<string, TestFailure>("value");
        var failure = new Result<string, TestFailure>(TestFailure.FIRST);

        await Assert.That(success).IsNotEqualTo(failure);
    }

    /// <summary>
    /// Verifies that default results compare equal to each other.
    /// </summary>
    [Test]
    public async Task Should_be_equal_when_both_results_are_default()
    {
        Result<string, TestFailure> first = default;
        Result<string, TestFailure> second = default;

        await Assert.That(first).IsEqualTo(second);
    }

    /// <summary>
    /// Verifies that a default result differs from every initialized branch.
    /// </summary>
    [Test]
    public async Task Should_not_be_equal_when_one_result_is_default()
    {
        Result<string, TestFailure> uninitialized = default;
        var success = new Result<string, TestFailure>("");
        var failure = new Result<string, TestFailure>(TestFailure.FIRST);

        await Assert.That(uninitialized).IsNotEqualTo(success);
        await Assert.That(uninitialized).IsNotEqualTo(failure);
    }
}