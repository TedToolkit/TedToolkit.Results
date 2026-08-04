# TedToolkit.Results.AspNetCore

`TedToolkit.Results.AspNetCore` converts Results into MVC and minimal API responses while leaving
failure-to-HTTP policy in the consuming application.

## Install

```shell
dotnet add package TedToolkit.Results.AspNetCore
```

MVC controllers can use an exhaustive enum mapping:

```csharp
using TedToolkit.Results.AspNetCore;

return result.ToActionResult(failure => failure switch
{
    SaveFailure.InvalidDocument => BadRequest(),
    SaveFailure.NotFound => NotFound(),
    _ => throw new ArgumentOutOfRangeException(nameof(failure), failure, null),
});
```

Minimal API endpoints use the same policy:

```csharp
return result.ToHttpResult(failure => failure switch
{
    SaveFailure.InvalidDocument => Results.BadRequest(),
    SaveFailure.NotFound => Results.NotFound(),
    _ => throw new ArgumentOutOfRangeException(nameof(failure), failure, null),
});
```

Valued successes produce HTTP 200 with the exact value. Value-less successes produce HTTP 204.
Failure mappers run exactly once for failures and are never invoked for successes. Use `Match` when
an endpoint requires a different success response such as Created or Accepted.

## Target Frameworks

- `net8.0`
- `net10.0`

## License

Licensed under LGPL-3.0.
