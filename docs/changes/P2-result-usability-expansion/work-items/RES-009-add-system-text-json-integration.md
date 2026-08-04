# RES-009: Add System.Text.Json Integration

## 📌 Status

Implemented

## 🚦 Delivery priority

- Priority: P3
- Rationale: A maintained JSON contract is valuable at application boundaries but remains optional core-adjacent functionality.
- Blocking prerequisite: RES-003, RES-006, ADR-0002, PB-01, and PB-02.

## 🔗 Delivery context

- Parent change: `docs/changes/P2-result-usability-expansion/README.md`
- Prerequisites: RES-003, RES-006, `docs/adr/ADR-0002-isolate-framework-integrations.md`.
- Applicable principles: none.

## 🧩 Explicit governing constraints

- Implement in `TedToolkit.Results.SystemTextJson`, which references the core; the core never
  references System.Text.Json or this package.
- Preserve the enum failure constraint and serialize exactly one active branch.
- JSON is a versioned public contract of the integration package, not of the core package.
- Converter behavior follows the caller's serializer options for payloads, enum representation,
  property naming, case sensitivity, and maximum depth where applicable.
- The integration target matrix must not reduce or alter the core target matrix.
- Serializing an Uninitialized Result is invalid and never emits a branch discriminator.
- Undefined numeric enum values are invalid external data and never create a failure Result.

## 🎯 Outcome, scope, and non-goals

Consumers can register one converter factory and round-trip both Result arities through an explicit
discriminated JSON union without triggering inactive property getters.

Add the integration package, converter factory, contract tests, and package README. Do not add
serialization attributes to core types, Newtonsoft.Json support, source generation metadata,
polymorphic arbitrary failures, schema migration tooling, or a wire-format guarantee from
`ToString()`.

## 🔍 Current behavior and impact boundary

The core Result has public `Value` and `FailureReason` getters that throw on their inactive branch.
Default property-based serialization can therefore fail and does not define a stable union shape.
The approved architecture requires an optional leaf package. This work affects new integration and
test projects plus solution/package documentation; core source and dependencies remain unchanged.

## 🧪 Behavior cases

| ID | Preconditions and input | Action | Expected observable behavior |
| --- | --- | --- | --- |
| BC-1 | Valued success | Serialize | Writes discriminator true and only the `Value` branch property |
| BC-2 | Valued failure | Serialize | Writes discriminator false and only the `FailureReason` branch property |
| BC-3 | Value-less success | Serialize | Writes only discriminator true |
| BC-4 | Value-less failure | Serialize | Writes discriminator false and the failure property |
| BC-5 | Any valid JSON branch | Deserialize | Constructs the exact Result arity, branch, and payload |
| BC-6 | Nullable success value | Round-trip | Preserves success containing null |
| BC-7 | Naming and enum converters configured | Round-trip | Honors property naming and enum representation options |
| BC-8 | Missing or non-Boolean discriminator | Deserialize | Throws `JsonException` |
| BC-9 | Active required payload is missing | Deserialize | Throws `JsonException`; valued success requires `Value`, failure requires `FailureReason` |
| BC-10 | Inactive branch payload or duplicate union property is present | Deserialize | Throws `JsonException` as contradictory or ambiguous data |
| BC-11 | Unknown unrelated property is present | Deserialize | Skips it using normal forward-compatible JSON behavior |
| BC-12 | Converter factory is not registered | Serialize using the core package only | Core has no implicit serializer dependency or converter registration |
| BC-13 | Either Result arity is Uninitialized | Serialize | Throws `JsonException` and writes no valid Result object |
| BC-14 | Failure payload is an undefined numeric or string enum value | Deserialize | Throws `JsonException` and produces no Result |
| BC-15 | JSON properties appear in any order | Deserialize | Produces the same valid Result as canonical write order |
| BC-16 | Naming policy maps two logical union names to one physical name | Create or use the converter | Throws `InvalidOperationException` before ambiguous JSON is written or read |

## 🛠️ Implementation contract

Create `src/TedToolkit.Results.SystemTextJson` and
`tests/TedToolkit.Results.SystemTextJson.Tests`. Add centrally managed package version
`System.Text.Json` 10.0.10. The product project imports the same `AlmostAllFrameworks.props` as the
core and therefore targets exactly `net6.0`, `net7.0`, `net8.0`, `net9.0`, `net10.0`, `net472`,
`net48`, `netstandard2.0`, and `netstandard2.1`. `net6.0` and `net7.0` use the platform-provided
System.Text.Json reference; the remaining targets use package 10.0.10. The test project targets
`net10.0` and uses TUnit.

The integration project references `TedToolkit.Results`; the core project and package metadata gain
no reference to System.Text.Json or the integration project.

In namespace `TedToolkit.Results.SystemTextJson`, expose
`public sealed class ResultJsonConverterFactory : JsonConverterFactory`. Consumers register it
through `JsonSerializerOptions.Converters`. The factory supports both `Result<TFailure>` and
`Result<TValue, TFailure>` only when the failure generic argument is an enum value type.

The logical JSON properties are:

| Result branch | Logical properties |
| --- | --- |
| Valued success | `IsSuccess: true`, `Value: <TValue>` |
| Valued failure | `IsSuccess: false`, `FailureReason: <TFailure>` |
| Value-less success | `IsSuccess: true` |
| Value-less failure | `IsSuccess: false`, `FailureReason: <TFailure>` |

Apply `JsonSerializerOptions.PropertyNamingPolicy` to the logical property names when writing and
matching. Respect `PropertyNameCaseInsensitive` when reading. Delegate payload and enum
serialization to `JsonSerializer` with the caller's options so registered converters apply.

Compute the three physical property names when creating each closed converter. If the active naming
policy maps any two logical names to the same physical name under the configured case comparison,
throw `InvalidOperationException` rather than accepting an ambiguous schema.

Write `IsSuccess` first and then the active payload property. Reading is property-order independent:
read one complete object, reject duplicate logical union properties, require the active payload,
reject the inactive payload, skip unrelated unknown properties, and throw `JsonException` for
malformed union state. Reject Uninitialized values during serialization. Never obtain payloads
through the inactive public property.

After deserializing `TFailure` with the caller's configured enum converter, require
`Enum.IsDefined(typeof(TFailure), failureReason)`. Translate either an undefined value or the core
constructor's `ArgumentOutOfRangeException` into `JsonException`. This rule overrides a caller enum
converter configured to allow integer values because the Result wire contract permits declared
failure members only.

## ✅ Verification plan

| Behavior case | Test level and location | Setup and action | Observable assertion | Command |
| --- | --- | --- | --- | --- |
| BC-1–BC-6 | Contract, integration test project | Serialize and deserialize both arities and branches | Exact logical shape and branch round-trip | `dotnet run --project tests/TedToolkit.Results.SystemTextJson.Tests/TedToolkit.Results.SystemTextJson.Tests.csproj -c Release` |
| BC-7 | Contract, same project | Configure camel case and string-enum converter | Options determine names and enum representation | Same command |
| BC-8–BC-11 | Negative contract, same project | Read malformed, contradictory, duplicate, and extended JSON | Exact accept/reject classification and `JsonException` | Same command |
| BC-12 | Dependency/build inspection | Build core alone and inspect project/package dependencies | No System.Text.Json package dependency in core | `dotnet build src/TedToolkit.Results/TedToolkit.Results.csproj -c Release` |
| BC-13, BC-14 | Negative contract, integration test project | Serialize defaults and deserialize undefined enum values | `JsonException` and no Result | Integration test command |
| BC-15, BC-16 | Contract, integration test project | Reorder properties and configure a colliding naming policy | Order-independent success and collision rejection | Integration test command |
| All | Multi-target build/package | Build integration targets and pack Release | Zero warnings/errors and documented assets | `dotnet build TedToolkit.Results.slnx -c Release` |

## ⏱️ Workload estimate

- Planning range: 0.06–0.10 person-months.
- Confidence: Medium.
- Assumptions: the platform references on net6.0/net7.0 and System.Text.Json 10.0.10 on the remaining
  targets remain compatible with the fixed target matrix and repository licensing requirements.
- Excluded work: Newtonsoft.Json, source generation, schema version negotiation, and benchmarks.
- Actual effort on completion: Completed in the current implementation.
- Material variance and reason: net6.0/net7.0 use their platform reference because package 10.0.10 explicitly rejects those TFMs.

## 📋 Completion evidence

All nine targets build with zero warnings. Thirteen contract tests pass for both arities, valid
round trips, serializer options, malformed unions, undefined failures, and naming collisions.

## 🔄 Migration and rollback

This creates an optional package and a new wire contract. Before publication, rollback removes the
new projects. After publication, incompatible schema changes require a major version or an explicit
migration path; removing the converter package does not change the core binary contract.

## ⚠️ Risks and open questions

| Item | Impact | Owner or next decision |
| --- | --- | --- |
| Naming policy can make physical property names vary | Stored documents depend on serializer options | Document options as part of each consumer's wire contract, reject collisions, and test default/web profiles |
| Re-entering JsonSerializer from a converter can recurse | Converter implementation may stack overflow | Request converters for payload types, never for the enclosing Result type |
| Legacy target dependency graph is larger | Old-framework consumers acquire transitive BCL support packages | Inspect the produced NuGet dependency groups before approval |
