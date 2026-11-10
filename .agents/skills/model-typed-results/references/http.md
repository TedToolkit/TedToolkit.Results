# Map Results to HTTP

- Use the MVC or minimal API adapter matching the endpoint shape. With MVC, `ToActionResult` maps failure reasons through a caller-supplied function.
- Define the response for each failure reason explicitly. Keep service failure enums free of framework status codes.
- Test the status and response body for a successful result and each externally visible failure mapping.

```csharp
using TedToolkit.Results.AspNetCore;

return result.ToActionResult(failure => failure switch
{
    SaveFailure.InvalidDocument => BadRequest(),
    SaveFailure.NotFound => NotFound(),
    _ => throw new ArgumentOutOfRangeException(nameof(failure), failure, null),
});
```

```csharp
return result.ToHttpResult(failure => failure switch
{
    SaveFailure.InvalidDocument => Results.BadRequest(),
    SaveFailure.NotFound => Results.NotFound(),
    _ => throw new ArgumentOutOfRangeException(nameof(failure), failure, null),
});
```
