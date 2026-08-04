# RES-010: Add ASP.NET Core Integration

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P3
- Rationale: Web response conversion improves ASP.NET Core endpoints but is optional and application-boundary specific.
- Blocking prerequisite: RES-003, RES-006, ADR-0002, PB-01, and PB-02.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-usability-expansion/README.md`
- Prerequisites: RES-003, RES-006, `docs/adr/ADR-0002-isolate-framework-integrations.md`.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- Implement in `TedToolkit.Results.AspNetCore`, which references the core and
  `Microsoft.AspNetCore.App`; the core never references ASP.NET Core.
- The package does not assign universal status codes to consumer-defined failure enum values.
- Every conversion call receives an explicit failure mapping delegate.
- Valued success defaults to HTTP 200 with the value; value-less success defaults to HTTP 204.
- Target exactly `net8.0` and `net10.0` as the approved ASP.NET Core LTS baselines without obligating
  the core to drop its legacy or .NET Standard targets.
- Conversion rejects an Uninitialized Result before selecting a response or invoking a mapper.

## 🎯 Outcome, scope, and non-goals

MVC controllers and minimal-API endpoints can convert either Result arity into conventional success
responses while explicitly and exhaustively mapping failure enum values to application-owned HTTP
responses.

Add an optional integration package, MVC and minimal-API extension families, contract tests, and a
package README. Do not add global mutable profiles, reflection-based enum conventions, ProblemDetails
schemas, exception middleware, automatic logging, JSON converters, endpoint filters, or dependency-
injection registration.

## 🔍 Current behavior and impact boundary

Core consumers can manually branch and construct ASP.NET Core responses, but there is no maintained
conversion boundary. The failure enum has no universal status meaning. ADR-0002 requires a separate
leaf package and consumer-owned failure mapping. This work affects new integration and test projects
plus solution/package documentation; it does not change core source or dependencies.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Valued success | Convert to MVC `ActionResult<TValue>` | Returns HTTP 200 carrying the exact value; failure mapper is not invoked |
| BC-2 | Valued failure | Convert to MVC result | Invokes the supplied mapper once with the exact enum and returns its response |
| BC-3 | Value-less success | Convert to MVC `IActionResult` | Returns HTTP 204; failure mapper is not invoked |
| BC-4 | Value-less failure | Convert to MVC result | Invokes the supplied mapper once and returns its response |
| BC-5 | Valued success | Convert to minimal-API `IResult` | Executes as HTTP 200 with the value |
| BC-6 | Valued failure | Convert to minimal-API result | Invokes and returns the supplied failure mapping |
| BC-7 | Value-less success | Convert to minimal-API `IResult` | Executes as HTTP 204 |
| BC-8 | Value-less failure | Convert to minimal-API result | Invokes and returns the supplied failure mapping |
| BC-9 | Failure mapper is null | Invoke any conversion on either branch | Throws `ArgumentNullException` before branch evaluation |
| BC-10 | Failure mapper returns null | Convert a failure | Throws `InvalidOperationException` |
| BC-11 | Failure mapper throws | Convert a failure | Propagates the same exception instance |
| BC-12 | Enum switch in failure mapper omits a value at runtime | Convert that failure | Consumer mapping behavior remains visible; the package supplies no fallback mapping |
| BC-13 | Either Result arity is Uninitialized | Invoke any conversion | Throws `InvalidOperationException` and invokes no mapper |

## 🛠️ Implementation contract

Create `src/TedToolkit.Results.AspNetCore` and `tests/TedToolkit.Results.AspNetCore.Tests`. Use
`Microsoft.NET.Sdk`, target exactly `net8.0;net10.0`, and add
`<FrameworkReference Include="Microsoft.AspNetCore.App" />`. The TUnit test project targets
`net10.0`. A future LTS change is a review trigger and does not alter this implementation contract
without design reapproval.

In namespace `TedToolkit.Results.AspNetCore`, add exactly these extension classes and methods:

```csharp
public static class ResultMvcExtensions
{
    public static ActionResult<TValue> ToActionResult<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        Func<TFailure, ActionResult<TValue>> failureMapper)
        where TFailure : struct, Enum;

    public static IActionResult ToActionResult<TFailure>(
        this Result<TFailure> result,
        Func<TFailure, IActionResult> failureMapper)
        where TFailure : struct, Enum;
}

public static class ResultMinimalApiExtensions
{
    public static IResult ToHttpResult<TValue, TFailure>(
        this Result<TValue, TFailure> result,
        Func<TFailure, IResult> failureMapper)
        where TFailure : struct, Enum;

    public static IResult ToHttpResult<TFailure>(
        this Result<TFailure> result,
        Func<TFailure, IResult> failureMapper)
        where TFailure : struct, Enum;
}
```

Each conversion validates `failureMapper` before inspecting the Result. The valued MVC overload
returns `OkObjectResult` wrapped as `ActionResult<TValue>`; the value-less MVC overload returns
`NoContentResult`. The valued minimal-API overload returns `Results.Ok(result.Value)`; the value-less
overload returns `Results.NoContent()`. These are respectively HTTP 200 with the exact value and HTTP
204 without a body.

For failure, invoke `failureMapper` exactly once and return its response unchanged. A null mapping
result is invalid and throws `InvalidOperationException`.
An Uninitialized source is invalid and throws `InvalidOperationException` before either branch is
converted.

Package examples must use an exhaustive switch expression over the application's failure enum:

```csharp
return result.ToActionResult(failure => failure switch
{
    SaveFailure.InvalidDocument => BadRequest(),
    SaveFailure.NotFound => NotFound(),
    _ => throw new ArgumentOutOfRangeException(nameof(failure), failure, null),
});
```

The final guard covers the CLR's representational enum remainder, while RES-011 guarantees that
ordinary Results never contain it. Every declared application failure must still have its own switch
arm. Do not add a package-provided default mapping, catch mapping exceptions, or translate enum names
into response bodies.

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1–BC-4 | Contract, MVC integration tests | Convert both Result arities and branches | Exact action-result type, status, body, mapper count, and enum | `dotnet run --project tests/TedToolkit.Results.AspNetCore.Tests/TedToolkit.Results.AspNetCore.Tests.csproj -c Release` |
| BC-5–BC-8 | Contract, minimal-API integration tests | Execute returned `IResult` in a test HTTP context | Exact status and serialized success payload; mapper count | Same command |
| BC-9–BC-11 | Unit/contract, same project | Supply null, null-returning, and throwing mappers | Exact exception behavior | Same command |
| BC-12, BC-13 | Compile/runtime example test | Use an explicit enum switch mapping and default Result | No fallback response; default is rejected | Same command |
| Dependency boundary | Build and inspect references | Build core and integration separately | Core has no ASP.NET Core framework reference | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.06–0.11 person-months.
- Confidence: Medium.
- Assumptions: MVC and minimal-API primitives remain available in the supported LTS shared frameworks.
- Excluded work: ProblemDetails policy, DI profiles, endpoint filters, OpenAPI metadata, exception
  middleware, authentication policy, and package publication.
- Actual effort on completion: Completed in the current implementation.
- Material variance and reason: None.

## 📋 Completion evidence

Both approved targets build with zero warnings. Eleven contract tests pass for valued and value-less
MVC and minimal API conversions, explicit mappings, null handling, exception propagation, and default rejection.

## 🔄 Migration and rollback

This creates an optional package and no persistent state. Before publication, rollback removes the
new projects. After publication, removing or changing conversion defaults is a package-level breaking
change but does not alter the core Result binary contract.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| HTTP 200/204 defaults may not fit every endpoint | Consumers may need Created, Accepted, or custom success metadata | Keep Match available for custom success behavior; consider explicit success-mapper overloads only from demonstrated demand |
| Minimal-API response bodies use configured JSON behavior | Runtime output depends on application serializer settings | Contract-test status and logical payload, not application-specific formatting |
| ASP.NET Core support windows change | Target list can become stale | Treat an LTS support transition as a design review trigger; do not silently change the approved TFMs |
