// -----------------------------------------------------------------------
// <copyright file="ValueLessResultAsyncCompositionTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

namespace TedToolkit.Results.Tests;

internal sealed class ValueLessResultAsyncCompositionTests
{
    /// <summary>
    /// Verifies that a value-less success binds asynchronously into either Result arity.
    /// </summary>
    [Test]
    public async Task Should_bind_value_less_success_asynchronously_into_either_arity()
    {
        var result = Result.Succeed<TestFailure>();

        var valueLess = await result.BindAsync(_ => Task.FromResult(Result.Fail(TestFailure.FIRST)))
            .ConfigureAwait(false);
        var valued = await result.BindAsync(_ => Task.FromResult(Result.Succeed<string, TestFailure>("value")))
            .ConfigureAwait(false);

        await Assert.That(valueLess.FailureReason).IsEqualTo(TestFailure.FIRST);
        await Assert.That(valued.Value).IsEqualTo("value");
    }

    /// <summary>
    /// Verifies that a value-less failure short-circuits asynchronous bind callbacks.
    /// </summary>
    [Test]
    public async Task Should_short_circuit_value_less_failure_asynchronously()
    {
        var calls = 0;
        var result = Result.Fail(TestFailure.SECOND);

        var bound = await result.BindAsync(_ =>
            {
                calls++;
                return Task.FromResult(Result.Succeed<TestFailure>());
            })
            .ConfigureAwait(false);

        await Assert.That(bound.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(calls).IsEqualTo(0);
    }

    /// <summary>
    /// Verifies that value-less asynchronous Match invokes only the active callback.
    /// </summary>
    [Test]
    public async Task Should_match_value_less_result_asynchronously()
    {
        var result = Result.Fail(TestFailure.SECOND);

        var matched = await Task.FromResult(result).MatchAsync(
            _ => Task.FromResult("success"),
            (reason, _) => Task.FromResult(reason.ToString())).ConfigureAwait(false);

        await Assert.That(matched).IsEqualTo("SECOND");
    }
}