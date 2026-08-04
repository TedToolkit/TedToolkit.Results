// -----------------------------------------------------------------------
// <copyright file="ResultJsonConverterFactoryTests.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Text.Json;
using System.Text.Json.Serialization;

namespace TedToolkit.Results.SystemTextJson.Tests;

internal sealed class ResultJsonConverterFactoryTests
{
    /// <summary>
    /// Verifies the canonical JSON shape for both valued branches.
    /// </summary>
    [Test]
    public async Task Should_write_canonical_valued_union_shapes()
    {
        var options = CreateOptions();
        var success = Result.Succeed<string, TestFailure>("value");
        var failure = Result.Fail<string, TestFailure>(TestFailure.SECOND);

        var successJson = JsonSerializer.Serialize(success, options);
        var failureJson = JsonSerializer.Serialize(failure, options);

        await Assert.That(successJson).IsEqualTo("{\"IsSuccess\":true,\"Value\":\"value\"}");
        await Assert.That(failureJson).IsEqualTo("{\"IsSuccess\":false,\"FailureReason\":1}");
    }

    /// <summary>
    /// Verifies the canonical JSON shape for both value-less branches.
    /// </summary>
    [Test]
    public async Task Should_write_canonical_value_less_union_shapes()
    {
        var options = CreateOptions();

        var successJson = JsonSerializer.Serialize(Result.Succeed<TestFailure>(), options);
        var failureJson = JsonSerializer.Serialize(Result.Fail(TestFailure.FIRST), options);

        await Assert.That(successJson).IsEqualTo("{\"IsSuccess\":true}");
        await Assert.That(failureJson).IsEqualTo("{\"IsSuccess\":false,\"FailureReason\":0}");
    }

    /// <summary>
    /// Verifies that nullable values and every branch round-trip through the converter.
    /// </summary>
    [Test]
    public async Task Should_round_trip_both_result_arities()
    {
        var options = CreateOptions();
        var nullableSuccess = Result.Succeed<string?, TestFailure>(null);
        var valuedFailure = Result.Fail<string?, TestFailure>(TestFailure.SECOND);
        var valueLessSuccess = Result.Succeed<TestFailure>();

        var nullableRoundTrip = JsonSerializer.Deserialize<Result<string?, TestFailure>>(
            JsonSerializer.Serialize(nullableSuccess, options),
            options);
        var failureRoundTrip = JsonSerializer.Deserialize<Result<string?, TestFailure>>(
            JsonSerializer.Serialize(valuedFailure, options),
            options);
        var valueLessRoundTrip = JsonSerializer.Deserialize<Result<TestFailure>>(
            JsonSerializer.Serialize(valueLessSuccess, options),
            options);

        await Assert.That(nullableRoundTrip.IsSuccess).IsTrue();
        await Assert.That(nullableRoundTrip.Value).IsNull();
        await Assert.That(failureRoundTrip.FailureReason).IsEqualTo(TestFailure.SECOND);
        await Assert.That(valueLessRoundTrip.IsSuccess).IsTrue();
    }

    /// <summary>
    /// Verifies that property naming and enum converters remain caller-controlled.
    /// </summary>
    [Test]
    public async Task Should_respect_caller_naming_and_enum_options()
    {
        var options = CreateOptions();
        options.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        options.Converters.Add(new JsonStringEnumConverter());
        var result = Result.Fail<string, TestFailure>(TestFailure.SECOND);

        var json = JsonSerializer.Serialize(result, options);
        var roundTrip = JsonSerializer.Deserialize<Result<string, TestFailure>>(json, options);

        await Assert.That(json).IsEqualTo("{\"isSuccess\":false,\"failureReason\":\"SECOND\"}");
        await Assert.That(roundTrip.FailureReason).IsEqualTo(TestFailure.SECOND);
    }

    /// <summary>
    /// Verifies that property order and unrelated properties do not affect valid input.
    /// </summary>
    [Test]
    public async Task Should_read_valid_union_in_any_order_with_unknown_properties()
    {
        var options = CreateOptions();
        const string json = "{\"Unknown\":42,\"Value\":\"value\",\"IsSuccess\":true}";

        var result = JsonSerializer.Deserialize<Result<string, TestFailure>>(json, options);

        await Assert.That(result.Value).IsEqualTo("value");
    }

    /// <summary>
    /// Verifies that malformed, contradictory, and undefined failure data is rejected.
    /// </summary>
    [Test]
    [Arguments("{}")]
    [Arguments("{\"IsSuccess\":true}")]
    [Arguments("{\"IsSuccess\":false}")]
    [Arguments("{\"IsSuccess\":true,\"Value\":\"value\",\"FailureReason\":0}")]
    [Arguments("{\"IsSuccess\":false,\"FailureReason\":999}")]
    [Arguments("{\"IsSuccess\":true,\"IsSuccess\":true,\"Value\":\"value\"}")]
    public async Task Should_reject_invalid_union_data(string json)
    {
        var options = CreateOptions();

        await Assert.That(() => JsonSerializer.Deserialize<Result<string, TestFailure>>(json, options))
            .Throws<JsonException>();
    }

    /// <summary>
    /// Verifies that uninitialized Results cannot be serialized.
    /// </summary>
    [Test]
    public async Task Should_reject_uninitialized_result_serialization()
    {
        var options = CreateOptions();
        Result<string, TestFailure> valued = default;
        Result<TestFailure> valueLess = default;

        await Assert.That(() => JsonSerializer.Serialize(valued, options)).Throws<JsonException>();
        await Assert.That(() => JsonSerializer.Serialize(valueLess, options)).Throws<JsonException>();
    }

    /// <summary>
    /// Verifies that ambiguous physical property names are rejected.
    /// </summary>
    [Test]
    public async Task Should_reject_colliding_naming_policy()
    {
        var options = CreateOptions();
        options.PropertyNamingPolicy = new CollidingNamingPolicy();

        await Assert.That(() => JsonSerializer.Serialize(Result.Succeed<TestFailure>(), options))
            .Throws<InvalidOperationException>();
    }

    private static JsonSerializerOptions CreateOptions()
    {
        var options = new JsonSerializerOptions();
        options.Converters.Add(new ResultJsonConverterFactory());
        return options;
    }

    private sealed class CollidingNamingPolicy : JsonNamingPolicy
    {
        public override string ConvertName(string name)
        {
            return "result";
        }
    }
}