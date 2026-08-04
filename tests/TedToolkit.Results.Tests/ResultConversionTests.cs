// -----------------------------------------------------------------------
// <copyright file="ResultConversionTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultConversionTests
{
    /// <summary>
    /// Verifies that a value implicitly converts to a successful result.
    /// </summary>
    [Test]
    public async Task Should_create_success_when_value_is_implicitly_converted()
    {
        var value = new TestValue();

        Result<TestValue, TestFailure> result = value;

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value).IsSameReferenceAs(value);
    }

    /// <summary>
    /// Verifies that a nullable value implicitly converts to a successful result containing null.
    /// </summary>
    [Test]
    public async Task Should_create_success_when_null_is_implicitly_converted()
    {
        Result<string?, TestFailure> result = CreateNullValue();

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value).IsNull();
    }

    /// <summary>
    /// Verifies that a failure reason implicitly converts to a failed result.
    /// </summary>
    [Test]
    public async Task Should_create_failure_when_reason_is_implicitly_converted()
    {
        Result<TestValue, TestFailure> result = TestFailure.SECOND;

        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.FailureReason).IsEqualTo(TestFailure.SECOND);
    }

    /// <summary>
    /// Verifies that a successful result explicitly converts to its contained value.
    /// </summary>
    [Test]
    public async Task Should_return_value_when_success_is_explicitly_converted()
    {
        var value = new TestValue();
        var result = new Result<TestValue, TestFailure>(value);

        var converted = (TestValue)result;

        await Assert.That(converted).IsSameReferenceAs(value);
    }

    /// <summary>
    /// Verifies that a failed result cannot explicitly convert to a success value.
    /// </summary>
    [Test]
    public async Task Should_throw_when_failure_is_explicitly_converted()
    {
        var result = new Result<TestValue, TestFailure>(TestFailure.FIRST);

        await Assert.That(() => (TestValue)result).Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that a default result cannot explicitly convert to a success value.
    /// </summary>
    [Test]
    public async Task Should_throw_when_default_result_is_explicitly_converted()
    {
        Result<TestValue, TestFailure> result = default;

        await Assert.That(() => (TestValue)result).Throws<InvalidOperationException>();
    }

    private static string? CreateNullValue()
    {
        return null;
    }
}