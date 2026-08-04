# ADR-0005: Share Result State Contracts

- Status: Superseded
- Date: 2026-08-04
- Decision owner: User
- Decision scope: The public state-inspection contract and Composition API shape for both Result arities across the package lifetime.
- Applicable principles: None.
- Related change or work packages: `docs/changes/P2-result-state-contracts/README.md`, RES-013
- Supersedes: ADR-0004
- Superseded by: ADR-0006

## 📌 Decision at a glance

Expose non-generic and failure-generic state interfaces from core, while retaining concrete Result
types in public Composition signatures to preserve inference, payload shape, and allocation-free value flows.

## 🧭 Context and decision question

Both Result structs expose identical initialization and branch properties, and both expose the same
typed failure property. Optional packages currently repeat arity-specific guards because there is no
common Result constraint. The decision is whether a common interface should become the basis for all
Composition APIs or remain a focused state contract used where both arities truly share semantics.

ADR-0004 selected a minimal core surface and explicitly declined a public interface. The new consumer
requirement is a type-safe common state abstraction that callers can use without supplying explicit
type arguments and that package implementations can use to remove duplicated guards.

## 🎯 Decision drivers and constraints

| Type | Driver or constraint | Evidence or source | Priority |
| --- | --- | --- | --- |
| Hard constraint | Ordinary Composition calls continue to infer all generic types | Existing fluent public signatures and user requirement | Must |
| Hard constraint | Ordinary concrete Result composition does not acquire interface boxing | ADR-0001 allocation-free value representation and CLR value-type interface conversion | Must |
| Hard constraint | Both Result arities preserve current state, payload, and exception behavior | Current core tests and architecture record | Must |
| Hard constraint | Core retains all nine target frameworks and no optional dependency | `AlmostAllFrameworks.props` | Must |
| Decision driver | Consumers can write one state-inspection abstraction for both Result arities | User-approved requirement | High |
| Decision driver | Identical arity-specific internal guards are consolidated | Current core and Composition source | Medium |

## 🔎 Options and evidence

| Option | Evidence and confidence | Meets drivers | Decisive trade-off | Outcome |
| --- | --- | --- | --- | --- |
| Keep only concrete Result types | Current implementation; High | Preserves inference and value flows but lacks a common contract | Repeats state-only generic infrastructure | Rejected |
| Make all Composition APIs interface-based | C# and CLR type rules; High | Provides one nominal input abstraction | Boxes struct receivers or requires additional self-type parameters; cannot preserve valued return shapes | Rejected |
| Add focused state interfaces and retain concrete Composition signatures | Current API analysis; High | Meets common inspection, inference, and value-flow constraints | Public surface adds two contract types and most semantic overloads remain | Selected |
| Add one interface containing untyped Value and FailureReason payloads | Type analysis; High | Superficially unifies every member | Loses compile-time payload types and boxes enum failures | Rejected |

## ✅ Decision

Core exposes `IResultState` with `IsInitialized`, `IsSuccess`, and `IsFailure` and exposes
`IResultState<TFailure>` with `FailureReason`, constrained by `where TFailure : struct, Enum`.
`Result<TFailure>` and `Result<TValue, TFailure>` implement `IResultState<TFailure>`.

The interfaces contain inspection properties only. They do not contain success values, construction,
validation commands, callbacks, or composition methods. `FailureReason` retains the existing contract
and throws for a successful or uninitialized Result.

Public Composition APIs continue to use concrete Result parameters and return types. Internal helpers
may use `where TResult : struct, IResultState` with an `in TResult` parameter. They must not accept an
interface reference when a concrete Result is passed, and no existing fluent call gains explicit type
arguments.

## 💡 Why this decision now

The common interfaces express a real consumer capability that was absent when ADR-0004 was accepted:
infrastructure can inspect either arity through one public contract. Keeping the interface focused
preserves the minimal core boundary because it adds no transferable behavior. Retaining concrete
Composition signatures recognizes that valued and value-less success branches have different callback
and return shapes; hiding those differences would trade a smaller implementation for a less precise and
potentially allocating consumer API.

Reconsider the boundary only if C# gains a cross-target mechanism that preserves concrete associated
result shapes and type inference without boxing, or a measured consumer workload justifies a separate
interface-oriented composition API.

## 🔗 Evidence and links

- Current architecture: `docs/architecture/result-package-boundaries.md`
- Superseded decision: `docs/adr/ADR-0004-minimize-result-contract-surface.md`
- Core types: `src/TedToolkit.Results/Result{TValue,TFailure}.cs`, `src/TedToolkit.Results/Result{TFailure}.cs`
- Composition APIs: `src/TedToolkit.Results.Composition/ResultExtensions.cs`, `src/TedToolkit.Results.Composition/ResultAsyncExtensions.cs`

No benchmark is required for the selected public API boundary because public Composition signatures
remain concrete. Implementation verification must inspect generated code or use a focused allocation
check if an internal constrained helper is found to convert a Result to an interface reference.

## ⚖️ Consequences and accepted trade-offs

- Generic consumer infrastructure can inspect either Result arity through one contract.
- A consumer that stores a Result in an interface-typed variable accepts normal CLR struct boxing.
- Package hot paths use constrained generic helpers and retain concrete public signatures.
- The core assembly exports two additional public contract types.
- Most Composition overloads remain because they encode different success payload and callback shapes.
- Existing source and binaries remain compatible because the interfaces and implementations are additive.

## 🛠️ Implementation handoff

RES-013 adds the two interfaces, implements them implicitly on both Result structs, protects inferred
consumer calls with compile-time tests, consolidates only state-equivalent internal guards, and retains
specialized JSON and positional collection exception behavior.

## 🔄 Rollout, rollback, and exit

No consumer migration is required. Consumers opt into the interfaces when useful. Before publication,
rollback removes the interface declarations and restores local guards. After publication, removing either
interface or an implemented member is a public compatibility break.

## 📅 Follow-ups and review triggers

| Item | Owner | Due date or objective trigger | Status |
| --- | --- | --- | --- |
| Pin this ADR to a full Git revision | TedToolkit maintainers | First repository commit containing the decision | Open |
| Review interface-oriented composition | TedToolkit maintainers | A concrete consumer cannot express state-only infrastructure through these contracts | Open |
| Reassess boxing constraints | TedToolkit maintainers | Supported runtimes change their value-type interface behavior | Open |
