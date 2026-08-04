// -----------------------------------------------------------------------
// <copyright file="ResultFactoryTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultFactoryTests
{
    /// <summary>
    /// Verifies that the success factory preserves the supplied value.
    /// </summary>
    [Test]
    public async Task Should_create_success_when_using_factory()
    {
        var value = new TestValue();

        var result = Result.Succeed<TestValue, TestFailure>(value);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value).IsSameReferenceAs(value);
    }

    /// <summary>
    /// Verifies that the success factory accepts a nullable value.
    /// </summary>
    [Test]
    public async Task Should_create_success_when_factory_value_is_null()
    {
        var result = Result.Succeed<string?, TestFailure>(null);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value).IsNull();
    }

    /// <summary>
    /// Verifies that the failure factory preserves the supplied reason.
    /// </summary>
    [Test]
    public async Task Should_create_failure_when_using_factory()
    {
        var result = Result.Fail<TestValue, TestFailure>(TestFailure.SECOND);

        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.FailureReason).IsEqualTo(TestFailure.SECOND);
    }
}