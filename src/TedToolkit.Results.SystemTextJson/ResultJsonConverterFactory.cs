// -----------------------------------------------------------------------
// <copyright file="ResultJsonConverterFactory.cs" company="TedToolkit">
// Copyright (c) TedToolkit. All rights reserved.
// Licensed under the LGPL-3.0 license. See COPYING, COPYING.LESSER file in the project root for full license information.
// </copyright>
// -----------------------------------------------------------------------

using System.Diagnostics.CodeAnalysis;
using System.Runtime.CompilerServices;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace TedToolkit.Results.SystemTextJson;

/// <summary>
/// Creates System.Text.Json converters for valued and value-less Result types.
/// </summary>
public sealed class ResultJsonConverterFactory : JsonConverterFactory
{
    /// <inheritdoc />
    public override bool CanConvert(Type typeToConvert)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(typeToConvert);
#else
        if (typeToConvert is null)
        {
            throw new ArgumentNullException(nameof(typeToConvert));
        }
#endif

        if (!typeToConvert.IsGenericType)
        {
            return false;
        }

        var definition = typeToConvert.GetGenericTypeDefinition();
        var arguments = typeToConvert.GetGenericArguments();
        return (definition == typeof(Result<>) && arguments[0].IsEnum)
            || (definition == typeof(Result<,>) && arguments[1].IsEnum);
    }

    /// <inheritdoc />
    public override JsonConverter CreateConverter(Type typeToConvert, JsonSerializerOptions options)
    {
#if NET6_0_OR_GREATER
        ArgumentNullException.ThrowIfNull(typeToConvert);
        ArgumentNullException.ThrowIfNull(options);
#else
        if (typeToConvert is null)
        {
            throw new ArgumentNullException(nameof(typeToConvert));
        }

        if (options is null)
        {
            throw new ArgumentNullException(nameof(options));
        }
#endif

        var definition = typeToConvert.GetGenericTypeDefinition();
        var arguments = typeToConvert.GetGenericArguments();
        var converterType = definition == typeof(Result<>)
            ? typeof(ValueLessResultConverter<>).MakeGenericType(arguments)
            : typeof(ValuedResultConverter<,>).MakeGenericType(arguments);

        return (JsonConverter)Activator.CreateInstance(converterType, new PropertyNames(options))!;
    }

    [SuppressMessage(
        "Performance",
        "CA1812",
        Justification = "Closed converter instances are created through reflection.")]
    private sealed class ValuedResultConverter<TValue, TFailure>(PropertyNames names)
        : JsonConverter<Result<TValue, TFailure>>
        where TFailure : struct, Enum
    {
        public override Result<TValue, TFailure> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var union = ReadUnion(document.RootElement, names);
            if (union.IsSuccess)
            {
                if (!union.HasValue || union.HasFailure)
                {
                    throw new JsonException("A successful valued Result requires only the Value payload.");
                }

                var value = union.Value.Deserialize<TValue>(options);
                return Result.Succeed<TValue, TFailure>(value!);
            }

            if (!union.HasFailure || union.HasValue)
            {
                throw new JsonException("A failed Result requires only the FailureReason payload.");
            }

            return Result.Fail<TValue, TFailure>(ReadFailure<TFailure>(union.Failure, options));
        }

        public override void Write(
            Utf8JsonWriter writer,
            Result<TValue, TFailure> value,
            JsonSerializerOptions options)
        {
            EnsureInitialized(value.IsInitialized);
            writer.WriteStartObject();
            writer.WriteBoolean(names.IsSuccess, value.IsSuccess);
            if (value.IsSuccess)
            {
                writer.WritePropertyName(names.Value);
                JsonSerializer.Serialize(writer, value.Value, options);
            }
            else
            {
                writer.WritePropertyName(names.FailureReason);
                JsonSerializer.Serialize(writer, value.FailureReason, options);
            }

            writer.WriteEndObject();
        }
    }

    [SuppressMessage(
        "Performance",
        "CA1812",
        Justification = "Closed converter instances are created through reflection.")]
    private sealed class ValueLessResultConverter<TFailure>(PropertyNames names)
        : JsonConverter<Result<TFailure>>
        where TFailure : struct, Enum
    {
        public override Result<TFailure> Read(
            ref Utf8JsonReader reader,
            Type typeToConvert,
            JsonSerializerOptions options)
        {
            using var document = JsonDocument.ParseValue(ref reader);
            var union = ReadUnion(document.RootElement, names);
            if (union.HasValue)
            {
                throw new JsonException("A value-less Result cannot contain a Value payload.");
            }

            if (union.IsSuccess)
            {
                if (union.HasFailure)
                {
                    throw new JsonException("A successful Result cannot contain a FailureReason payload.");
                }

                return Result.Succeed<TFailure>();
            }

            if (!union.HasFailure)
            {
                throw new JsonException("A failed Result requires a FailureReason payload.");
            }

            return Result.Fail(ReadFailure<TFailure>(union.Failure, options));
        }

        public override void Write(Utf8JsonWriter writer, Result<TFailure> value, JsonSerializerOptions options)
        {
            EnsureInitialized(value.IsInitialized);
            writer.WriteStartObject();
            writer.WriteBoolean(names.IsSuccess, value.IsSuccess);
            if (value.IsFailure)
            {
                writer.WritePropertyName(names.FailureReason);
                JsonSerializer.Serialize(writer, value.FailureReason, options);
            }

            writer.WriteEndObject();
        }
    }

    private static UnionData ReadUnion(in JsonElement root, PropertyNames names)
    {
        if (root.ValueKind != JsonValueKind.Object)
        {
            throw new JsonException("A Result must be represented by a JSON object.");
        }

        bool? isSuccess = null;
        JsonElement value = default;
        JsonElement failure = default;
        var hasValue = false;
        var hasFailure = false;

        foreach (var property in root.EnumerateObject())
        {
            if (names.Equals(property.Name, names.IsSuccess))
            {
                if (isSuccess.HasValue || property.Value.ValueKind is not JsonValueKind.True and not JsonValueKind.False)
                {
                    throw new JsonException("IsSuccess must appear once as a Boolean value.");
                }

                isSuccess = property.Value.GetBoolean();
            }
            else if (names.Equals(property.Name, names.Value))
            {
                if (hasValue)
                {
                    throw new JsonException("Value must not appear more than once.");
                }

                hasValue = true;
                value = property.Value;
            }
            else if (names.Equals(property.Name, names.FailureReason))
            {
                if (hasFailure)
                {
                    throw new JsonException("FailureReason must not appear more than once.");
                }

                hasFailure = true;
                failure = property.Value;
            }
        }

        if (!isSuccess.HasValue)
        {
            throw new JsonException("A Result requires a Boolean IsSuccess discriminator.");
        }

        return new(isSuccess.Value, hasValue, value, hasFailure, failure);
    }

    [SuppressMessage(
        "Usage",
        "CA2263",
        Justification = "The generic overload is unavailable on legacy target frameworks.")]
    private static TFailure ReadFailure<TFailure>(in JsonElement element, JsonSerializerOptions options)
        where TFailure : struct, Enum
    {
        try
        {
            var failure = element.Deserialize<TFailure>(options);
            if (!Enum.IsDefined(typeof(TFailure), failure))
            {
                throw new JsonException("FailureReason must be a declared enum value.");
            }

            return failure;
        }
        catch (ArgumentOutOfRangeException exception)
        {
            throw new JsonException("FailureReason must be a declared enum value.", exception);
        }
    }

    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    private static void EnsureInitialized(bool isInitialized)
    {
        if (isInitialized)
        {
            return;
        }

        throw new JsonException("An uninitialized Result cannot be serialized.");
    }

    private sealed class PropertyNames
    {
        private readonly StringComparison _comparison;

        internal PropertyNames(JsonSerializerOptions options)
        {
            var policy = options.PropertyNamingPolicy;
            IsSuccess = policy?.ConvertName("IsSuccess") ?? "IsSuccess";
            Value = policy?.ConvertName("Value") ?? "Value";
            FailureReason = policy?.ConvertName("FailureReason") ?? "FailureReason";
            _comparison = options.PropertyNameCaseInsensitive
                ? StringComparison.OrdinalIgnoreCase
                : StringComparison.Ordinal;

            var comparer = options.PropertyNameCaseInsensitive
                ? StringComparer.OrdinalIgnoreCase
                : StringComparer.Ordinal;
            var names = new HashSet<string>(comparer)
            {
                IsSuccess,
                Value,
                FailureReason,
            };
            if (names.Count == 3)
            {
                return;
            }

            throw new InvalidOperationException("The JSON naming policy produces ambiguous Result properties.");
        }

        internal string FailureReason { get; }

        internal string IsSuccess { get; }

        internal string Value { get; }

        internal bool Equals(string left, string right)
        {
            return string.Equals(left, right, _comparison);
        }
    }

    private readonly struct UnionData
    {
        internal UnionData(
            bool isSuccess,
            bool hasValue,
            in JsonElement value,
            bool hasFailure,
            in JsonElement failure)
        {
            IsSuccess = isSuccess;
            HasValue = hasValue;
            Value = value;
            HasFailure = hasFailure;
            Failure = failure;
        }

        internal JsonElement Failure { get; }

        internal bool HasFailure { get; }

        internal bool HasValue { get; }

        internal bool IsSuccess { get; }

        internal JsonElement Value { get; }
    }
}