# ADR-0003: Isolate Optional Composition Extensions

- Status: Superseded
- Date: 2026-08-03
- Decision owner: User
- Decision scope: Package boundary for Task and collection Result composition.
- Applicable principles: None.
- Related change or work packages: `docs/changes/P2-result-usability-expansion/README.md`, RES-007, RES-008
- Supersedes: The original RES-007 and RES-008 placement of extension types in the core assembly.
- Superseded by: ADR-0004

## Decision

Keep `TedToolkit.Results` as the dependency-light contract package containing the two Result types,
the static factory, and intrinsic synchronous instance operations. Publish Task and collection
extension types in the optional `TedToolkit.Results.Composition` package.

The Composition package references only the core package and mirrors its nine target frameworks.
Its public extension types remain in the `TedToolkit.Results` namespace so adding the package does
not require another using directive. The core assembly must not expose `ResultAsyncExtensions` or
`ResultEnumerableExtensions`.

## Context

DDD-style contract projects often need only the Result value types. Putting every convenience
extension in the same assembly expands the contract surface even when an implementation does not
use Task pipelines or collection traversal. The extensions form an independently meaningful NuGet
and API boundary, while synchronous methods remain intrinsic Result behavior with no optional
dependency.

## Consequences

- Contract projects can reference only `TedToolkit.Results`.
- Implementation projects opt into Task and collection composition explicitly.
- Existing extension call syntax and namespace imports remain unchanged after adding the package.
- The repository maintains one additional product and test project.
- Package versions must remain compatible across the core and Composition packages.

## Verification

Core contract tests assert that the optional extension types are absent from the core assembly.
Composition tests own all Task and collection behavior cases, and its project builds against the
same nine target frameworks as the core package.
