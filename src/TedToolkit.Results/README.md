# TedToolkit.Results

`TedToolkit.Results` provides immutable result value records for representing an operation as either a
success value or an enum failure reason.

## Install

```shell
dotnet add package TedToolkit.Results
```

## Use

```csharp
using TedToolkit.Results;

var success = Result.Succeed<string, SaveFailure>("document.json");

if (success)
{
    Console.WriteLine(success.Value);
}

Result<string, SaveFailure> failure = SaveFailure.InvalidDocument;
Console.WriteLine(failure.FailureReason);

enum SaveFailure
{
    InvalidDocument,
}
```

A result has exactly one active branch. Reading `Value` from a failed result or
`FailureReason` from a successful result throws `InvalidOperationException`.

`Result<TValue, TFailure>` is a readonly value type. Its default value is uninitialized:

```csharp
Result<string, SaveFailure> result = default;
Console.WriteLine(result.IsInitialized); // false
```

Reading branch-dependent members or formatting an uninitialized result throws
`InvalidOperationException`.

## Consume Branches

Use `if (result)` for success and `if (!result)` for failure. These operators have the same branch
semantics as `IsSuccess` and `IsFailure`; Result is not generally convertible to `bool`.

```csharp
if (!result)
{
    Console.WriteLine(result.FailureReason);
}
```

Install [TedToolkit.Results.Composition](../TedToolkit.Results.Composition/README.md) for `Match`,
`TryGetValue`, `TryGetFailure`, deconstruction, and synchronous, Task, or collection pipelines. The
core package intentionally keeps these transferable conveniences outside the value contract. JSON
and ASP.NET Core adapters are also separate optional packages.

## Create Results

Use the non-generic factory when the result type is explicit at the call site:

```csharp
var success = Result.Succeed<string, SaveFailure>("document.json");
var failure = Result.Fail<string, SaveFailure>(SaveFailure.InvalidDocument);
```

Constructors and implicit conversions provide equivalent behavior:

```csharp
Result<string, SaveFailure> success = "document.json";
Result<string, SaveFailure> failure = SaveFailure.InvalidDocument;
```

Success values may be `null` when `TValue` is nullable. `TFailure` must be an enum value type.

## Read Results

- `IsInitialized` safely identifies whether either branch is active.
- `IsSuccess` identifies the active branch.
- `Value` returns the success value.
- `FailureReason` returns the failure enum.
- An explicit conversion returns the success value and follows the same branch validation as
  `Value`.

```csharp
Result<string, SaveFailure> result = "document.json";
var fileName = (string)result;
```

Because `Result<TValue, TFailure>` is a record struct, equality compares the active branch and its
contained value.

## Diagnostic Text

`ToString()` prefixes successes with `✅` and failures with `❌`:

```text
✅ document.json
❌ InvalidDocument
```

This text is for logs and debugging only. It is not a serialization, persistence, localization, or
user-interface contract.

## Internationalization

The package does not hard-code localized presentation text. Source, runtime messages, documentation,
and examples use English, while consumer-provided success values remain normal Unicode-capable .NET
values. Applications own any locale-specific user-interface text.

## Target Frameworks

- `net6.0`
- `net7.0`
- `net8.0`
- `net9.0`
- `net10.0`
- `net11.0`
- `net472`
- `net48`
- `netstandard2.0`
- `netstandard2.1`

## License

Licensed under LGPL-3.0.

## Related Documentation

- [Repository overview](../../README.md)
