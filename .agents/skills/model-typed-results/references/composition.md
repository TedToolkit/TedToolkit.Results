# Compose Typed Results

- Import `TedToolkit.Results` and use `Match` when converting both branches to a common output.
- Choose the composition overload whose success and failure types match the next operation. Keep an existing failure from reaching a success-only callback.
- Test a successful pipeline and a failed intermediate operation, including the intended short-circuit or aggregation behavior for a collection.

```csharp
using TedToolkit.Results;

var result = Result.Succeed<string, SaveFailure>("document.json");
var message = result.Match(
    value => $"Saved {value}",
    failure => $"Failed: {failure}");

var branch = result switch
{
    (true, var value, _) => value,
    (false, _, var failure) => failure.ToString(),
};

var path = result
    .Ensure(value => value.Length > 0, SaveFailure.InvalidDocument)
    .Map(value => value.ToUpperInvariant())
    .Recover(_ => "fallback.json");

enum SaveFailure
{
    InvalidDocument,
}
```

```csharp
using TedToolkit.Results;

Result<Document, SaveFailure> loaded = await LoadAsync().ConfigureAwait(false);
Result<string, SaveFailure> path = await loaded
    .MapAsync((document, cancellationToken) => SaveAsync(document, cancellationToken))
    .ConfigureAwait(false);
```
