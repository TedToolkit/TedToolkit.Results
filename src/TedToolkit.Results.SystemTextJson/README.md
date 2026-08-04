# TedToolkit.Results.SystemTextJson

`TedToolkit.Results.SystemTextJson` provides an explicit JSON union contract for both Result
arities without adding serializer dependencies to the core package.

## Install

```shell
dotnet add package TedToolkit.Results.SystemTextJson
```

Register the converter factory on the serializer options used by the application:

```csharp
using System.Text.Json;
using TedToolkit.Results.SystemTextJson;

var options = new JsonSerializerOptions();
options.Converters.Add(new ResultJsonConverterFactory());
```

The default wire shapes are:

```json
{ "IsSuccess": true, "Value": "document.json" }
{ "IsSuccess": false, "FailureReason": 0 }
{ "IsSuccess": true }
```

The converter honors the configured property naming policy, case sensitivity, payload converters,
and enum converters. It rejects uninitialized Results, contradictory branch properties, duplicate
union properties, and failure values that are not declared enum members.

The application's serializer options are part of its wire contract. Changing naming or enum
conversion settings can change stored JSON.

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

The package uses the platform-provided System.Text.Json reference on `net6.0` and `net7.0`. Other
targets use the centrally managed System.Text.Json package version.

## License

Licensed under LGPL-3.0.
