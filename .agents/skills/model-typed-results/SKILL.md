---
name: model-typed-results
description: >-
  Use TedToolkit.Results to model expected outcomes, compose results, or adapt them to HTTP and System.Text.Json.
---

# Model Typed Results

Choose an enum failure reason for expected outcomes. Decide whether success carries a value before choosing the `Result` arity.

```csharp
using TedToolkit.Results;

var success = Result.Succeed<string, SaveFailure>("document.json");
var failure = Result.Fail<string, SaveFailure>(SaveFailure.InvalidDocument);
```

Check `IsSuccess`, `IsFailure`, or the documented branch patterns before extracting a branch value. Handle unexpected exceptions separately and keep failure enums independent of HTTP and JSON.

## Choose the consumer operation

- Use [composition](references/composition.md) when sequencing work or combining multiple results.
- Use [HTTP mapping](references/http.md) when an endpoint must translate a result to a response.
- Use [JSON serialization](references/json.md) when a result crosses a serialized boundary.

Verify both branches at the relevant boundary. For a pipeline check where execution stops, for HTTP check status and body, and for JSON round-trip the discriminator and payload.
