# Serialize Typed Results

- Register the package converter factory on the same `JsonSerializerOptions` used to read and write results.
- Preserve the branch discriminator. Successful value results carry `Value` and failures carry `FailureReason` under the configured naming policy.
- Round-trip each arity and branch, including a success without a value. Verify malformed or contradictory branch payloads against the package’s current converter behavior.

```csharp
using System.Text.Json;
using TedToolkit.Results.SystemTextJson;

var options = new JsonSerializerOptions();
options.Converters.Add(new ResultJsonConverterFactory());
```

The documented JSON uses `IsSuccess` plus either `Value` or `FailureReason`. A non-value success only needs `IsSuccess`. Follow the configured naming policy and avoid accepting contradictory branch fields.
