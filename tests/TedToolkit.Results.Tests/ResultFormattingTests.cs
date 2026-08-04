// -----------------------------------------------------------------------
// <copyright file="ResultFormattingTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultFormattingTests
{
    /// <summary>
    /// Verifies that a successful result formats its value for diagnostics.
    /// </summary>
    /// <param name="value">The success value.</param>
    /// <param name="expected">The expected diagnostic text.</param>
    [Test]
    [Arguments("value", "✅ value")]
    [Arguments(null, "✅ null")]
    public async Task Should_format_success_with_value(string? value, string expected)
    {
        var result = Result.Succeed<string?, TestFailure>(value);

        await Assert.That(result.ToString()).IsEqualTo(expected);
    }

    /// <summary>
    /// Verifies that a null value representation uses the null diagnostic fallback.
    /// </summary>
    [Test]
    public async Task Should_format_success_with_null_fallback_when_value_returns_null()
    {
        var result = Result.Succeed<NullTextValue, TestFailure>(new());

        await Assert.That(result.ToString()).IsEqualTo("✅ null");
    }

    /// <summary>
    /// Verifies that a failed result formats its reason for diagnostics.
    /// </summary>
    [Test]
    public async Task Should_format_failure_with_reason()
    {
        var result = Result.Fail<string?, TestFailure>(TestFailure.SECOND);

        await Assert.That(result.ToString()).IsEqualTo("❌ SECOND");
    }

    /// <summary>
    /// Verifies that a default result has no diagnostic branch representation.
    /// </summary>
    [Test]
    public async Task Should_throw_when_default_result_is_formatted()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => result.ToString()).Throws<InvalidOperationException>();
    }

    private sealed class NullTextValue
    {
        public override string ToString()
        {
            return null!;
        }
    }
}