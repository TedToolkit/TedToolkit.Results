# Result Package Boundaries

- Status: Active
- Owner: TedToolkit maintainers
- Scope and system boundary: The core Result abstraction and its optional composition, serialization, and web-framework integrations for the lifetime of the public package family.
- Governing principles: None
- Related ADRs: `docs/adr/ADR-0001-result-value-representation/README.md`, `docs/adr/ADR-0002-isolate-framework-integrations.md`, `docs/adr/ADR-0006-keep-concrete-result-contracts.md`
- Last approved revision: Unavailable; the repository has no committed revision yet.

## Current architecture

`TedToolkit.Results` is the dependency-light core package. Its approved representation is a
`readonly record struct` with explicit Uninitialized, Success, and Failure states. It owns the closed
success/failure representation, construction, state and payload properties, required C# language
operators and conversions, safe diagnostic formatting, and value-less results. A failure remains a
value of an enum type so consumers can enumerate the closed failure set.

The public contract exposes concrete Result types rather than common state interfaces. Package-local
guards receive the already-public `IsInitialized` value and preserve boundary-specific exception
context. This keeps the core contract small and avoids implying an interface-oriented polymorphic
usage model that would box the built-in Result structs.

Composition and framework integrations are optional leaf packages:

```text
TedToolkit.Results.Composition    ──> TedToolkit.Results
TedToolkit.Results.SystemTextJson ──> TedToolkit.Results
TedToolkit.Results.AspNetCore     ──> TedToolkit.Results
```

The core package never references an optional package. Optional packages do not reference one
another. Consumers install only the behavior required by their implementation layer.

`TedToolkit.Results.Composition` owns all transferable branch-consumption and composition methods,
including Match, TryGet, Deconstruct, synchronous pipelines, Task pipelines, and ordered fail-fast
collection extensions. It keeps them in the `TedToolkit.Results` namespace for consumer source compatibility.
`TedToolkit.Results.SystemTextJson` owns the public JSON union schema and converter registration.
`TedToolkit.Results.AspNetCore` owns conversion to ASP.NET Core MVC and minimal-API response types,
while the consuming application owns the mapping from each failure enum value to an HTTP response.

## Constraints for change design

- Every Result failure type remains constrained by `where TFailure : struct, Enum`; an arbitrary
  error object, exception, message, or metadata dictionary is not a supported failure branch.
- Every stored failure value must be declared by its enum type. Numeric values and unnamed flag
  combinations for which `Enum.IsDefined` is false are rejected at Result creation boundaries.
- The all-zero default of either Result arity is Uninitialized. It never masquerades as success or
  failure, and branch-dependent APIs reject it with `InvalidOperationException`.
- The core package remains independent of `System.Text.Json`, ASP.NET Core, dependency injection,
  logging, transferable branch-consumption/composition surfaces, and application frameworks.
- The core owns static factories because construction is part of the contract and value-less
  success must be constructible without Composition.
- Public composition signatures continue to accept and return concrete Result structs.
- Initialization guards remain internal implementation details. Public consumers inspect
  `IsInitialized` or use branch-dependent members that already reject the default value.
- Operators, conversions, record equality, and safe `ToString` remain on the core types because C#
  cannot relocate them to extension methods without removing approved behavior.
- Every other transferable public method belongs to Composition, even when it has no external dependency.
- Optional integration dependencies must not reduce the target-framework matrix of the core package.
- Each integration package targets only frameworks supported by its platform dependency and records
  that target matrix in its package documentation.
- JSON serialization must use an explicit branch discriminator and must never read an inactive
  Result property.
- ASP.NET Core integration must not assign universal HTTP semantics to consumer-defined enum values.
- Public API additions require deterministic unit or contract tests on every affected target family.
- Authored source, runtime messages, code documentation, tests, and repository documentation remain
  English-only; consumer values remain ordinary Unicode data.

## Decision links and exceptions

`docs/adr/ADR-0001-result-value-representation/README.md` governs the explicit-state value
representation. `docs/adr/ADR-0002-isolate-framework-integrations.md` selects separate optional
framework packages. `docs/adr/ADR-0006-keep-concrete-result-contracts.md` defines the minimal concrete
core contract and complete Composition surface. ADR-0006 supersedes ADR-0005. There are no approved
exceptions.

## Review triggers

- A proposed core API requires a framework or serializer dependency.
- A proposed public Result interface or initialization guard lacks a concrete consumer capability
  that cannot be expressed through the existing Result properties.
- A public Composition signature converts either Result struct to an interface reference.
- A proposed transferable branch-consumption or composition method is added to the core types.
- A proposed API permits default Result values to enter a success or failure flow unnoticed.
- An integration requires another integration package.
- A platform dependency can no longer support its documented target matrix.
- A consumer requirement proposes non-enum failures, multiple simultaneous failures, or embedded
  localized messages.
- A consumer requires undefined numeric enum values or unnamed flag combinations as failure reasons.
- A new serialization format or HTTP framework requires another public integration boundary.
