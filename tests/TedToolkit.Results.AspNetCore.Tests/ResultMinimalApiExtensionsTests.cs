// -----------------------------------------------------------------------
// <copyright file="ResultMinimalApiExtensionsTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Http;

namespace TedToolkit.Results.AspNetCore.Tests;

internal sealed class ResultMinimalApiExtensionsTests
{
    [Test]
    public async Task Should_convert_valued_success_to_ok_without_mapping_failure()
    {
        var calls = 0;
        var response = Result.Succeed<string, TestFailure>("document.json").ToHttpResult(_ =>
        {
            calls++;
            return Microsoft.AspNetCore.Http.Results.BadRequest();
        });

        await Assert.That(((IStatusCodeHttpResult)response).StatusCode).IsEqualTo(200);
        await Assert.That(((IValueHttpResult)response).Value).IsEqualTo("document.json");
        await Assert.That(calls).IsEqualTo(0);
    }

    [Test]
    public async Task Should_return_exact_valued_failure_mapping_once()
    {
        var calls = 0;
        var mapped = Microsoft.AspNetCore.Http.Results.NotFound();
        var response = Result.Fail<string, TestFailure>(TestFailure.MISSING).ToHttpResult(failure =>
        {
            calls++;
            return failure == TestFailure.MISSING ? mapped : Microsoft.AspNetCore.Http.Results.BadRequest();
        });

        await Assert.That(ReferenceEquals(response, mapped)).IsTrue();
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    public async Task Should_convert_value_less_branches_to_minimal_api_results()
    {
        var calls = 0;
        var success = Result.Succeed<TestFailure>().ToHttpResult(_ =>
        {
            calls++;
            return Microsoft.AspNetCore.Http.Results.BadRequest();
        });
        var mapped = Microsoft.AspNetCore.Http.Results.NotFound();
        var failure = Result.Fail(TestFailure.MISSING).ToHttpResult(reason =>
        {
            calls++;
            return reason == TestFailure.MISSING ? mapped : Microsoft.AspNetCore.Http.Results.BadRequest();
        });

        await Assert.That(((IStatusCodeHttpResult)success).StatusCode).IsEqualTo(204);
        await Assert.That(ReferenceEquals(failure, mapped)).IsTrue();
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    public async Task Should_validate_minimal_api_failure_mappers_and_results()
    {
        Func<TestFailure, IResult> mapper = null!;

        await Assert.That(() => Result.Succeed<string, TestFailure>("value").ToHttpResult(mapper))
            .Throws<ArgumentNullException>();
        await Assert.That(() => Result.Succeed<TestFailure>().ToHttpResult(mapper))
            .Throws<ArgumentNullException>();
        await Assert.That(() => Result.Fail<string, TestFailure>(TestFailure.INVALID).ToHttpResult(_ => null!))
            .Throws<InvalidOperationException>();
        await Assert.That(() => Result.Fail(TestFailure.INVALID).ToHttpResult(_ => null!))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Should_propagate_failure_mapper_exception()
    {
        var expected = new InvalidDataException("Mapping failed.");
        Exception? actual = null;

        try
        {
            _ = Result.Fail<string, TestFailure>(TestFailure.INVALID).ToHttpResult(_ => throw expected);
        }
        catch (InvalidDataException exception)
        {
            actual = exception;
        }

        await Assert.That(ReferenceEquals(actual, expected)).IsTrue();
    }

    [Test]
    public async Task Should_reject_uninitialized_minimal_api_results_without_invoking_mapper()
    {
        var calls = 0;

        await Assert.That(() => default(Result<string, TestFailure>).ToHttpResult(_ =>
            {
                calls++;
                return Microsoft.AspNetCore.Http.Results.BadRequest();
            }))
            .Throws<InvalidOperationException>();
        await Assert.That(() => default(Result<TestFailure>).ToHttpResult(_ =>
            {
                calls++;
                return Microsoft.AspNetCore.Http.Results.BadRequest();
            }))
            .Throws<InvalidOperationException>();
        await Assert.That(calls).IsEqualTo(0);
    }
}