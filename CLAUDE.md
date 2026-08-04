# Project instructions

## Language policy

- The implementation language is C#.
- Write all README files, user-facing documentation, XML documentation, and code comments in English.
- Preserve code, commands, identifiers, package names, and URLs exactly as written.
- TedToolkit is internationalized software. Do not hard-code Chinese text in source code, tests,
  runtime messages, identifiers, comments, XML documentation, examples, or repository documentation.
- Public APIs must continue to accept ordinary Unicode consumer data; locale-specific presentation
  belongs outside this core library.

## Project scope

- `TedToolkit.Results` is the dependency-light contract package boundary. Optional composition,
  serialization, and web adapters live in separate leaf packages that reference it.
- Implement the approved `Result` API contract under `docs/changes/P2-result-api-migration/`.
- Production projects must not reference test or build projects.

## Test execution

- Use `dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release` to execute tests.
- Do not use `dotnet test` unless explicitly requested.
