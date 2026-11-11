# TedToolkit.Results

[![License: LGPL-3.0](https://img.shields.io/badge/License-LGPL--3.0-blue.svg)](COPYING.LESSER)

`TedToolkit.Results` provides immutable result records for APIs that model expected success and
failure outcomes explicitly.

The package family keeps the Result contract independent from optional composition and framework
dependencies:

- [TedToolkit.Results](src/TedToolkit.Results/README.md) contains the Result value contract and factories.
- [TedToolkit.Results.Composition](src/TedToolkit.Results.Composition/README.md) adds branch-consumption,
  synchronous, Task, and collection extensions.
- [TedToolkit.Results.SystemTextJson](src/TedToolkit.Results.SystemTextJson/README.md) adds JSON converters.
- [TedToolkit.Results.AspNetCore](src/TedToolkit.Results.AspNetCore/README.md) adds MVC and minimal API adapters.

The package is internationalization-ready: it does not embed locale-specific presentation text,
while success values remain ordinary Unicode-capable .NET values.

## Repository Layout

```text
src/
  TedToolkit.Results/
  TedToolkit.Results.Composition/
  TedToolkit.Results.SystemTextJson/
  TedToolkit.Results.AspNetCore/
tests/
  TedToolkit.Results.Tests/
  TedToolkit.Results.Composition.Tests/
  TedToolkit.Results.SystemTextJson.Tests/
  TedToolkit.Results.AspNetCore.Tests/
Build/
  Build.csproj
externals/
  TedToolkit/
```

- Each directory under `src` is an independently consumable NuGet package boundary.
- Each matching project under `tests` contains fast, deterministic tests for that package.
- `Build` contains the repository pipeline entry point.
- `externals/TedToolkit` is a submodule that provides shared props and build infrastructure.

## Prerequisites

- .NET 10 SDK
- Git with submodule support

Clone with submodules:

```shell
git clone --recursive https://github.com/TedToolkit/TedToolkit.Results.git
cd TedToolkit.Results
```

If the repository is already cloned without submodules:

```shell
git submodule update --init --recursive
```

## Build And Test

Build the solution:

```shell
dotnet build TedToolkit.Results.slnx -c Release
```

Run all package test projects:

```shell
dotnet run --project tests/TedToolkit.Results.Tests/TedToolkit.Results.Tests.csproj -c Release
dotnet run --project tests/TedToolkit.Results.Composition.Tests/TedToolkit.Results.Composition.Tests.csproj -c Release
dotnet run --project tests/TedToolkit.Results.SystemTextJson.Tests/TedToolkit.Results.SystemTextJson.Tests.csproj -c Release
dotnet run --project tests/TedToolkit.Results.AspNetCore.Tests/TedToolkit.Results.AspNetCore.Tests.csproj -c Release
```

GitHub Actions runs the shared TedToolkit build workflow. Set `BUILD_SOLUTION` to `TedToolkit.Results.slnx`, `BUILD_TEST_PROJECTS` to the four test project paths above, and `BUILD_PACK_PROJECTS` to the four package projects under `src/`. List one test or package project per line.

## Target Frameworks

The core, Composition, and System.Text.Json packages target:

- `net6.0`
- `net7.0`
- `net8.0`
- `net9.0`
- `net10.0`
- `net472`
- `net48`
- `netstandard2.0`
- `netstandard2.1`

The ASP.NET Core package targets `net8.0` and `net10.0`.

## License

Licensed under [LGPL-3.0](COPYING.LESSER). See [COPYING](COPYING) and [COPYING.LESSER](COPYING.LESSER) for the full license text.
