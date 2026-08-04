// -----------------------------------------------------------------------
// <copyright file="ResultConstructionTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultConstructionTests
{
    /// <summary>
    /// Verifies that construction with a value creates only the success branch.
    /// </summary>
    [Test]
    public async Task Should_create_success_when_constructed_with_value()
    {
        var value = new object();

        var result = new Result<object, TestFailure>(value);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.IsInitialized).IsTrue();
        await Assert.That(result.Value).IsSameReferenceAs(value);
        await Assert.That(() => result.FailureReason).Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that construction with a failure reason creates only the failure branch.
    /// </summary>
    [Test]
    public async Task Should_create_failure_when_constructed_with_reason()
    {
        var result = new Result<object, TestFailure>(TestFailure.FIRST);

        await Assert.That(result.IsSuccess).IsFalse();
        await Assert.That(result.IsInitialized).IsTrue();
        await Assert.That(result.FailureReason).IsEqualTo(TestFailure.FIRST);
        await Assert.That(() => result.Value).Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that a nullable success branch can contain null.
    /// </summary>
    [Test]
    public async Task Should_create_success_when_value_is_null()
    {
        var result = new Result<string?, TestFailure>(null);

        await Assert.That(result.IsSuccess).IsTrue();
        await Assert.That(result.Value).IsNull();
    }

    /// <summary>
    /// Verifies that a default result reports an uninitialized state without throwing.
    /// </summary>
    [Test]
    public async Task Should_report_uninitialized_when_result_is_default()
    {
        Result<TestValue, TestFailure> result = default;

        await Assert.That(result.IsInitialized).IsFalse();
    }

    /// <summary>
    /// Verifies that a default result cannot be observed as either active branch.
    /// </summary>
    [Test]
    public async Task Should_throw_when_branch_members_are_read_from_default_result()
    {
        Result<TestValue, TestFailure> result = default;

        await Assert.That(() => result.IsSuccess).Throws<InvalidOperationException>();
        await Assert.That(() => result.Value).Throws<InvalidOperationException>();
        await Assert.That(() => result.FailureReason).Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that Result is exposed as a readonly value type.
    /// </summary>
    [Test]
    public async Task Should_be_readonly_value_type()
    {
        var resultType = typeof(Result<TestValue, TestFailure>);

        await Assert.That(resultType.IsValueType).IsTrue();
        await Assert.That(resultType.IsDefined(typeof(System.Runtime.CompilerServices.IsReadOnlyAttribute), false)).IsTrue();
    }
}