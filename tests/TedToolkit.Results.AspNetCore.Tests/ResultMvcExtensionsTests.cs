// -----------------------------------------------------------------------
// <copyright file="ResultMvcExtensionsTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using Microsoft.AspNetCore.Mvc;

namespace TedToolkit.Results.AspNetCore.Tests;

internal sealed class ResultMvcExtensionsTests
{
    [Test]
    public async Task Should_convert_valued_success_to_ok_without_mapping_failure()
    {
        var calls = 0;
        var result = Result.Succeed<string, TestFailure>("document.json");

        var action = result.ToActionResult(_ =>
        {
            calls++;
            return new BadRequestResult();
        });

        var response = action.Result as OkObjectResult;
        await Assert.That(response).IsNotNull();
        await Assert.That(response!.StatusCode).IsEqualTo(200);
        await Assert.That(response.Value).IsEqualTo("document.json");
        await Assert.That(calls).IsEqualTo(0);
    }

    [Test]
    public async Task Should_return_exact_valued_failure_mapping_once()
    {
        var calls = 0;
        ActionResult<string> mapped = new NotFoundResult();
        var result = Result.Fail<string, TestFailure>(TestFailure.MISSING);

        var action = result.ToActionResult(failure =>
        {
            calls++;
            return failure == TestFailure.MISSING ? mapped : new BadRequestResult();
        });

        await Assert.That(ReferenceEquals(action, mapped)).IsTrue();
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    public async Task Should_convert_value_less_branches_to_mvc_results()
    {
        var calls = 0;
        var success = Result.Succeed<TestFailure>().ToActionResult(_ =>
        {
            calls++;
            return new BadRequestResult();
        });
        IActionResult mapped = new NotFoundResult();
        var failure = Result.Fail(TestFailure.MISSING).ToActionResult(reason =>
        {
            calls++;
            return reason == TestFailure.MISSING ? mapped : new BadRequestResult();
        });

        await Assert.That(success).IsTypeOf<NoContentResult>();
        await Assert.That(ReferenceEquals(failure, mapped)).IsTrue();
        await Assert.That(calls).IsEqualTo(1);
    }

    [Test]
    public async Task Should_validate_mvc_failure_mappers_and_results()
    {
        Func<TestFailure, ActionResult<string>> valuedMapper = null!;
        Func<TestFailure, IActionResult> valueLessMapper = null!;
        var valued = Result.Succeed<string, TestFailure>("value");
        var valueLess = Result.Succeed<TestFailure>();

        await Assert.That(() => valued.ToActionResult(valuedMapper)).Throws<ArgumentNullException>();
        await Assert.That(() => valueLess.ToActionResult(valueLessMapper)).Throws<ArgumentNullException>();
        await Assert.That(() => Result.Fail<string, TestFailure>(TestFailure.INVALID).ToActionResult(_ => null!))
            .Throws<InvalidOperationException>();
        await Assert.That(() => Result.Fail(TestFailure.INVALID).ToActionResult(_ => null!))
            .Throws<InvalidOperationException>();
    }

    [Test]
    public async Task Should_reject_uninitialized_mvc_results_without_invoking_mapper()
    {
        var calls = 0;

        await Assert.That(() => default(Result<string, TestFailure>).ToActionResult(_ =>
            {
                calls++;
                return new BadRequestResult();
            }))
            .Throws<InvalidOperationException>();
        await Assert.That(() => default(Result<TestFailure>).ToActionResult(_ =>
            {
                calls++;
                return new BadRequestResult();
            }))
            .Throws<InvalidOperationException>();
        await Assert.That(calls).IsEqualTo(0);
    }
}