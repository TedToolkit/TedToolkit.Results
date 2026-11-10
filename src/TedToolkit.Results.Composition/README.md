# TedToolkit.Results.Composition

`TedToolkit.Results.Composition` adds optional branch-consumption and synchronous, Task, and
collection composition extensions to the dependency-light `TedToolkit.Results` contract package.

## Install

```shell
dotnet add package TedToolkit.Results.Composition
```

The extensions use the core `TedToolkit.Results` namespace, so the same import enables branch-safe
consumption, deconstruction, and fluent pipelines:

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

The inactive deconstructed payload is `default`; always use the Boolean discriminator.

Task composition uses the same fluent surface:

```csharp
using TedToolkit.Results;

Result<Document, SaveFailure> loaded = await LoadAsync().ConfigureAwait(false);
Result<string, SaveFailure> path = await loaded
    .MapAsync((document, cancellationToken) => SaveAsync(document, cancellationToken))
    .ConfigureAwait(false);
```

Collection composition is ordered and fail-fast:

```csharp
Result<IReadOnlyList<Document>, SaveFailure> documents = results.Sequence();
```

`Sequence`, `Traverse`, and `Combine` stop at the first failure. Task callbacks run only for the
active branch, receive the supplied cancellation token, and propagate exceptions and cancellation.

## Migration From Core Instance Methods

Installing this package preserves ordinary fluent calls such as `result.Map(value => value.Length)`.
C# does not support partially explicit generic arguments on extension methods. A call such as
`result.Map<int>(null!)` must instead provide a typed delegate, for example:

```csharp
result.Map((Func<string, int>)null!);
```

Removing the former core instance members is also a binary breaking change; existing consumers must
reference Composition and recompile.

## Target Frameworks

- `net6.0`
- `net7.0`
- `net8.0`
- `net9.0`
- `net10.0`
- `net472`
- `net48`
- `netstandard2.0`
- `netstandard2.1`

## Agent skill

The `model-typed-results` skill ships in `TedToolkit.Results`, which this package restores as a dependency. If your project references only this package, get the resolved `TedToolkit.Results` version from `dotnet package list --include-transitive` and import the skill with `dotnet-package-skills install --package TedToolkit.Results@<resolved-version>`.

## License

Licensed under LGPL-3.0.
