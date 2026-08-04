// -----------------------------------------------------------------------
// <copyright file="ValueLessResultFormattingTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ValueLessResultFormattingTests
{
    /// <summary>
    /// Verifies the exact value-less diagnostic format for both branches.
    /// </summary>
    [Test]
    public async Task Should_format_value_less_result_for_diagnostics()
    {
        var success = Result.Succeed<TestFailure>();
        var failure = Result.Fail(TestFailure.SECOND);

        await Assert.That(success.ToString()).IsEqualTo("✅");
        await Assert.That(failure.ToString()).IsEqualTo("❌ SECOND");
    }
}