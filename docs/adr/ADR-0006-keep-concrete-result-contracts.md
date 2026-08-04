# ADR-0006: Keep Concrete Result Contracts

- Status: Accepted
- Date: 2026-08-04
- Decision owner: User
- Decision scope: The public abstraction and initialization-validation boundary for both Result arities across the package lifetime.
- Applicable principles: None.
- Related change or work packages: `docs/changes/P2-remove-result-state-contracts/README.md`, RES-014
- Supersedes: ADR-0005
- Superseded by: None

## 📌 Decision at a glance

Expose only the concrete Result factory and value types, keep initialization guards internal, and retain the measured `readonly record struct` representation.

## 🧭 Context and decision question

ADR-0005 introduced `IResultState` and `IResultState<TFailure>` to share state-only infrastructure.
Implementation showed that the interfaces removed a few arity-specific guards but added public types,
documentation, compatibility obligations, and a boxing-prone polymorphic model without an identified
consumer API that needs to accept both Result arities. The decision is whether those interfaces and a
public `ThrowIfUninitialized` extension provide enough consumer value to justify that surface.

## 🎯 Decision drivers and constraints

| Type | Driver or constraint | Evidence or source | Priority |
| --- | --- | --- | --- |
| Hard constraint | Default Results remain uninitialized and branch-dependent members reject them | ADR-0001 and existing tests | Must |
| Hard constraint | Concrete fluent calls retain inference, return shapes, and exception behavior | Current Composition API | Must |
| Hard constraint | The nine-target core and Composition matrix remains | `AlmostAllFrameworks.props` | Must |
| Decision driver | Public API exists for consumer capabilities, not internal source deduplication | User-approved simplification | High |
| Decision driver | Ordinary Result flows remain allocation-free | ADR-0001 measured evidence | High |
| Decision driver | Boundary-specific guards preserve useful diagnostic context | Existing JSON, collection, callback, and HTTP behavior | Medium |

## 🔎 Options and evidence

| Option | Evidence and confidence | Meets drivers | Decisive trade-off | Outcome |
| --- | --- | --- | --- | --- |
| Keep both interfaces | Implemented API and tests; High | Provides nominal common inspection | No identified polymorphic consumer; adds surface and invites boxing | Rejected |
| Publish `ThrowIfUninitialized` as an interface extension | C# receiver rules and existing guards; High | Adds concise syntax | Does not replace context-specific guards and may box interface receivers | Rejected |
| Remove interfaces and keep package-local Boolean guards | Existing `IsInitialized` property; High | Preserves every observable behavior with the smallest public surface | Internal helpers cannot use a common Result type constraint | Selected |
| Change Result to class for interface-oriented use | ADR-0001 measurements; High | Avoids separate interface boxing after construction | Allocates ordinary Results and changes default, source, and binary semantics without an interface-heavy workload | Rejected |

## ✅ Decision

Core exports only `Result`, `Result<TFailure>`, and `Result<TValue, TFailure>`. No common public Result
interface or public initialization-guard extension is provided. Both structs retain `IsInitialized`,
and every branch-dependent property and operator continues rejecting the default value.

Core and optional packages consolidate equivalent validation by passing `IsInitialized` to internal
helpers. Specialized helpers retain their exception type, message, position, or callback context.
Composition continues exposing concrete Result receivers and return values.

ADR-0001 remains active without modification: both Result arities remain `readonly record struct`.
Representation is reconsidered only when a representative interface-heavy consumer workload exists
and new BenchmarkDotNet evidence changes the measured allocation and throughput trade-off.

## 💡 Why this decision now

The interface implementation supplied evidence about maintenance shape: it increased the durable
public contract to remove internal overloads whose shared input is already a Boolean property. The
user-approved outcome is lower total conceptual cost rather than the smallest count of private helper
methods. The existing class baseline allocates approximately 32 bytes per constructed Result while the
struct path allocates no managed memory, so no current evidence supports changing representation.

Reconsider interfaces only when a concrete public API must accept both arities through one parameter.
Reconsider a public guard only when consumers need explicit prevalidation beyond existing branch access.

## 🔗 Evidence and links

- Representation decision and benchmarks: `docs/adr/ADR-0001-result-value-representation/README.md`
- Superseded interface decision: `docs/adr/ADR-0005-share-result-state-contracts.md`
- Current architecture: `docs/architecture/result-package-boundaries.md`
- Core types: `src/TedToolkit.Results/Result{TValue,TFailure}.cs`, `src/TedToolkit.Results/Result{TFailure}.cs`

No new benchmark is required because the selected direction retains the benchmarked representation
and removes interface conversions rather than adding a new performance claim.

## ⚖️ Consequences and accepted trade-offs

- The core public surface returns to three concrete contract types.
- Consumers use concrete Result types and existing properties rather than a common interface.
- Internal helpers accept Boolean initialization state and cannot access other Result properties.
- No ordinary or interface-oriented call is added that could box a Result struct.
- A future genuine polymorphic consumer requires a new public API decision.

## 🛠️ Implementation handoff

RES-014 removes both interfaces and their focused tests and documentation, restores exact public-
surface tests, removes interface implementations from both structs, and changes shared guards to
Boolean inputs without altering exception behavior.

## 🔄 Rollout, rollback, and exit

The packages have not been published from this uncommitted repository, so removal requires no consumer
migration. If an interface-bearing package is published first, removal requires a breaking release.
Rollback restores ADR-0005 and its interface contracts.

## 📅 Follow-ups and review triggers

| Item | Owner | Due date or objective trigger | Status |
| --- | --- | --- | --- |
| Pin this ADR to a full Git revision | TedToolkit maintainers | First repository commit containing the decision | Open |
| Reassess common abstraction | TedToolkit maintainers | A concrete public consumer must accept both Result arities | Open |
| Reassess struct representation | TedToolkit maintainers | Representative interface-heavy evidence contradicts ADR-0001 | Open |
