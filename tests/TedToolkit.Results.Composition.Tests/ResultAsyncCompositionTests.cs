// -----------------------------------------------------------------------
// <copyright file="ResultAsyncCompositionTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Globalization;

namespace TedToolkit.Results.Tests;

internal sealed class ResultAsyncCompositionTests
{
    /// <summary>
    /// Verifies that async mapping invokes one successful callback with the supplied token.
    /// </summary>
    [Test]
    public async Task Should_map_success_asynchronously_with_supplied_token()
    {
        using var source = new CancellationTokenSource();
        var result = Result.Succeed<string, TestFailure>("value");
        CancellationToken observed = default;

        var mapped = await result.MapAsync((value, token) =>
            {
                observed = token;
                return Task.FromResult(value.Length);
            }, source.Token)
            .ConfigureAwait(false);

        await Assert.That(mapped.Value).IsEqualTo(5);
        await Assert.That(observed).IsEqualTo(source.Token);
    }

    /// <summary>
    /// Verifies that a failed async stage short-circuits without observing token cancellation.
    /// </summary>
    [Test]
    public async Task Should_short_circuit_failed_async_stage_without_callback()
    {
        using var source = new CancellationTokenSource();
        await source.CancelAsync().ConfigureAwait(false);
        var calls = 0;
        var result = Result.Fail<string, TestFailure>(TestFailure.FIRST);

        var mapped = await result.MapAsync((value, _) =>
            {
                calls++;
                return Task.FromResult(value.Length);
            }, source.Token)
            .ConfigureAwait(false);

        await Assert.That(mapped.FailureReason).IsEqualTo(TestFailure.FIRST);
        await Assert.That(calls).IsEqualTo(0);
    }

    /// <summary>
    /// Verifies representative async match, ensure, and recovery behavior.
    /// </summary>
    [Test]
    public async Task Should_compose_async_branch_operations()
    {
        var success = Result.Succeed<string, TestFailure>("value");
        var failure = Result.Fail<string, TestFailure>(TestFailure.FIRST);

        var matched = await success.MatchAsync(
            (value, _) => Task.FromResult(value.Length),
            (_, _) => Task.FromResult(-1)).ConfigureAwait(false);
        var ensured = await success.EnsureAsync((_, _) => Task.FromResult(false), TestFailure.SECOND)
            .ConfigureAwait(false);
        var recovered = await failure.RecoverAsync((reason, _) => Task.FromResult(reason.ToString()))
            .ConfigureAwait(false);
        var recoveredWith = await failure.RecoverWithAsync(
            (_, _) => Task.FromResult(Result.Succeed<string, TestFailure>("recovered")))
            .ConfigureAwait(false);

        await Assert.That(matched).IsEqualTo(5);
        await Assert.That(ensured.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(recovered.Value).IsEqualTo("FIRST");
        await Assert.That(recoveredWith.Value).IsEqualTo("recovered");
    }

    /// <summary>
    /// Verifies that Result-producing async callbacks reject null Tasks and default Results.
    /// </summary>
    [Test]
    public async Task Should_reject_invalid_async_callback_results()
    {
        var success = Result.Succeed<string, TestFailure>("value");
        var failure = Result.Fail<string, TestFailure>(TestFailure.FIRST);
        Func<string, CancellationToken, Task<Result<int, TestFailure>>> nullTaskBinder = (_, _) => null!;

        await Assert.That(() => success.BindAsync(nullTaskBinder))
            .Throws<InvalidOperationException>();
        await Assert.That(() => success.BindAsync((_, _) => Task.FromResult(default(Result<int, TestFailure>))))
            .Throws<InvalidOperationException>();
        await Assert.That(() => failure.RecoverWithAsync((_, _) => Task.FromResult(default(Result<string, TestFailure>))))
            .Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that direct-source async argument validation occurs before returning a Task.
    /// </summary>
    [Test]
    public async Task Should_validate_direct_async_arguments_synchronously()
    {
        Result<string, TestFailure> result = default;
        Func<string, CancellationToken, Task<int>> mapper = null!;

        await Assert.That(() => result.MapAsync(mapper)).Throws<ArgumentNullException>();
        await Assert.That(() => result.MapAsync((value, _) => Task.FromResult(value.Length)))
            .Throws<InvalidOperationException>();
    }

    /// <summary>
    /// Verifies that Task sources chain stages and preserve a middle failure.
    /// </summary>
    [Test]
    public async Task Should_chain_task_result_pipeline_with_short_circuit()
    {
        var laterCalls = 0;

        var result = await Task.FromResult(Result.Succeed<string, TestFailure>("value"))
            .MapAsync((value, _) => Task.FromResult(value.Length))
            .BindAsync((_, _) => Task.FromResult(Result.Fail<int, TestFailure>(TestFailure.SECOND)))
            .MapAsync((value, _) =>
            {
                laterCalls++;
                return Task.FromResult(value.ToString(CultureInfo.InvariantCulture));
            }).ConfigureAwait(false);

        await Assert.That(result.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(laterCalls).IsEqualTo(0);
    }
}