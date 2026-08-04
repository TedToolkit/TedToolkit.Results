// -----------------------------------------------------------------------
// <copyright file="ResultFailureValidationTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ResultFailureValidationTests
{
    /// <summary>
    /// Verifies that every valued failure creation path preserves a declared enum member.
    /// </summary>
    [Test]
    public async Task Should_create_failure_when_reason_is_declared()
    {
        var constructed = new Result<string, TestFailure>(TestFailure.SECOND);
        var factory = Result.Fail<string, TestFailure>(TestFailure.SECOND);
        Result<string, TestFailure> converted = TestFailure.SECOND;

        await Assert.That(constructed.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(factory.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(converted.FailureReason).IsEqualTo(TestFailure.SECOND);
    }

    /// <summary>
    /// Verifies that every valued failure creation path rejects an undefined numeric value.
    /// </summary>
    [Test]
    public async Task Should_reject_failure_when_reason_is_undefined()
    {
        const TestFailure reason = (TestFailure)999;

        var constructed = CaptureUndefinedFailure(() => _ = CreateFailure(reason));
        var factory = CaptureUndefinedFailure(() => _ = Result.Fail<string, TestFailure>(reason));
        var converted = CaptureUndefinedFailure(() => _ = ConvertFailure(reason));

        await Assert.That(constructed.ParamName).IsEqualTo("failureReason");
        await Assert.That(constructed.ActualValue).IsEqualTo(reason);
        await Assert.That(factory.ParamName).IsEqualTo("failureReason");
        await Assert.That(factory.ActualValue).IsEqualTo(reason);
        await Assert.That(converted.ParamName).IsEqualTo("failureReason");
        await Assert.That(converted.ActualValue).IsEqualTo(reason);
    }

    /// <summary>
    /// Verifies that zero is accepted only when the enum declares a zero-valued member.
    /// </summary>
    [Test]
    public async Task Should_validate_zero_against_declared_members()
    {
        var declared = new Result<string, TestFailure>(TestFailure.FIRST);

        await Assert.That(declared.FailureReason).IsEqualTo(TestFailure.FIRST);
        var exception = CaptureUndefinedFailure(
            () => _ = CreateFailure((NoZeroFailure)0));

        await Assert.That(exception.ParamName).IsEqualTo("failureReason");
        await Assert.That(exception.ActualValue).IsEqualTo((NoZeroFailure)0);
    }

    /// <summary>
    /// Verifies that a named flags composite is accepted while an unnamed combination is rejected.
    /// </summary>
    [Test]
    public async Task Should_accept_only_declared_flags_values()
    {
        var named = new Result<string, FlagsFailure>(FlagsFailure.BOTH);

        await Assert.That(named.FailureReason).IsEqualTo(FlagsFailure.BOTH);
        var exception = CaptureUndefinedFailure(
            () => _ = CreateFailure(FlagsFailure.FIRST | FlagsFailure.THIRD));

        await Assert.That(exception.ParamName).IsEqualTo("failureReason");
        await Assert.That(exception.ActualValue).IsEqualTo(FlagsFailure.FIRST | FlagsFailure.THIRD);
    }

    /// <summary>
    /// Verifies that duplicate enum names remain valid when their shared value is declared.
    /// </summary>
    [Test]
    public async Task Should_accept_failure_when_underlying_value_has_duplicate_names()
    {
        var result = new Result<string, DuplicateFailure>(DuplicateFailure.ALIAS);

        await Assert.That(result.FailureReason).IsEqualTo(DuplicateFailure.PRIMARY);
    }

    private static Result<string, TestFailure> ConvertFailure(TestFailure reason)
    {
        return reason;
    }

    private static Result<string, TFailure> CreateFailure<TFailure>(TFailure reason)
        where TFailure : struct, Enum
    {
        return new(reason);
    }

    private static ArgumentOutOfRangeException CaptureUndefinedFailure(Action action)
    {
        try
        {
            action();
        }
        catch (ArgumentOutOfRangeException caught)
        {
            return caught;
        }

        throw new InvalidOperationException("The failure value was expected to be rejected.");
    }

    private enum NoZeroFailure
    {
        FIRST = 1,
    }

    [Flags]
    private enum FlagsFailure
    {
        NONE = 0,

        FIRST = 1 << 0,

        SECOND = 1 << 1,

        BOTH = FIRST | SECOND,

        THIRD = 1 << 2,
    }

#pragma warning disable CA1069, RCS1234 // Duplicate values are the invalid input under test.
    private enum DuplicateFailure
    {
        PRIMARY = 1,

        ALIAS = 1,
    }
#pragma warning restore CA1069, RCS1234
}