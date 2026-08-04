// -----------------------------------------------------------------------
// <copyright file="ResultConditionalOperatorTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Reflection;

namespace TedToolkit.Results.Tests;

internal sealed class ResultConditionalOperatorTests
{
    /// <summary>
    /// Verifies that positive and negated conditions follow the successful branch state.
    /// </summary>
    [Test]
    public async Task Should_evaluate_conditions_as_success_when_result_succeeds()
    {
        var result = Result.Succeed<string, TestFailure>("value");
        var positiveBody = false;
        var negativeBody = false;

        if (result)
        {
            positiveBody = true;
        }

        if (!result)
        {
            negativeBody = true;
        }

        await Assert.That(positiveBody).IsTrue();
        await Assert.That(negativeBody).IsFalse();
    }

    /// <summary>
    /// Verifies that positive and negated conditions follow the failed branch state.
    /// </summary>
    [Test]
    public async Task Should_evaluate_conditions_as_failure_when_result_fails()
    {
        var result = Result.Fail<string, TestFailure>(TestFailure.FIRST);
        var positiveBody = false;
        var negativeBody = false;

        if (result)
        {
            positiveBody = true;
        }

        if (!result)
        {
            negativeBody = true;
        }

        await Assert.That(positiveBody).IsFalse();
        await Assert.That(negativeBody).IsTrue();
    }

    /// <summary>
    /// Verifies that neither conditional form accepts an uninitialized result.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_result_when_evaluating_condition()
    {
        Result<string, TestFailure> result = default;

        await Assert.That(() => EvaluatePositive(result)).Throws<InvalidOperationException>();
        await Assert.That(() => EvaluateNegative(result)).Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that Result exposes only the approved Boolean-like operators.
    /// </summary>
    [Test]
    public async Task Should_expose_only_approved_conditional_operators()
    {
        var operators = typeof(Result<string, TestFailure>)
            .GetMethods(BindingFlags.Public | BindingFlags.Static)
            .Where(method => method.IsSpecialName)
            .Select(method => method.Name)
            .ToArray();

        await Assert.That(operators).Contains("op_True");
        await Assert.That(operators).Contains("op_False");
        await Assert.That(operators).Contains("op_LogicalNot");
        await Assert.That(operators).DoesNotContain("op_BitwiseAnd");
        await Assert.That(operators).DoesNotContain("op_BitwiseOr");
    }

    [SuppressMessage(
        "Style",
        "RCS1073",
        Justification = "The explicit conditional is the behavior under test.")]
    private static bool EvaluatePositive(in Result<string, TestFailure> result)
    {
        if (result)
        {
            return true;
        }

        return false;
    }

    private static bool EvaluateNegative(in Result<string, TestFailure> result)
    {
        return !result;
    }
}