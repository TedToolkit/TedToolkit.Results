# Minimize the Result Contract Surface

## 📌 Status

Completed

- Change owner: TedToolkit maintainers
- Approval owner: User

## 🚦 Change priority

- Priority: P2
- Rationale: This is a planned public package-boundary correction that materially improves DDD contract-layer usage without addressing an active outage.
- Directory: `docs/changes/P2-minimize-result-contract-surface/`

## 🎯 Intended outcome and scope

Make `TedToolkit.Results` a minimal contract package that owns Result construction, representation,
state and payload access, and C# language members that cannot be moved to extensions. Move every
transferable branch-consumption and pipeline method to `TedToolkit.Results.Composition`.

Scope includes moving synchronous branch consumption, deconstruction, transformation, binding,
validation, and recovery APIs; relocating their XML documentation and tests; protecting the reduced
core member surface; updating package guidance; and preserving ordinary inferred fluent source
syntax for consumers that reference Composition.

Non-goals are introducing `IResult`, changing state representation, changing exception behavior,
removing factories, removing conditional or conversion operators, changing JSON/HTTP contracts,
adding new combinators, or changing target frameworks.

## 🧾 Source intent and hard constraints

- Source request: Keep the core package primarily as a contract and move all functions that C# can
  express as extensions into Composition.
- User or business outcome: DDD contract projects reference a smaller Result API while implementation
  projects opt into consumption and pipeline conveniences.
- External hard constraints: factories remain in core; non-transferable operators, conversions,
  record equality, and safe `ToString` remain supported; `TFailure` remains a declared enum value.

## 🧩 Governing principles and decisions

- Applicable principles and revision: none.
- Related ADRs and status: `docs/adr/ADR-0004-minimize-result-contract-surface.md` (Accepted, not yet committed), superseding ADR-0003.
- Related architecture records and revision: `docs/architecture/result-package-boundaries.md` (Active, not yet committed).
- Resulting constraints: core has no public transferable methods; Composition owns the complete
  extension surface in namespace `TedToolkit.Results`; optional packages continue to depend only on core.
- Reapproval trigger: a moved method must remain in core, an operator/factory is removed, a public
  interface is introduced, exception behavior changes, or the target matrix changes.

The repository has no Git commit, so no governing revision can be pinned. The user approved this
boundary on 2026-08-04; pin the records after the initial repository commit.

## 🧭 Planned approach

Deliver one coherent work package because the core removal and Composition replacement must land
together to preserve fluent source syntax. Add a synchronous `ResultExtensions` class to
Composition, remove the corresponding instance members from both core Result arities, move behavior
tests to the Composition test project, and add contract tests that reject future transferable core
members. Keep static factories and non-transferable language members untouched. Preserve ordinary
inferred fluent calls; document the C# limitation for calls that previously supplied only an
instance method's own generic arguments.

## ✅ Completion criteria

- Core declares none of Match, TryGet, Deconstruct, Map, Bind, MapFailure, Ensure, Recover, or RecoverWith.
- Composition exposes the same call shapes and preserves branch, callback, nullable, exception, and Uninitialized behavior.
- Deconstruction syntax compiles and behaves identically through the extension method.
- Core-only tests compile without referencing Composition; Composition owns all moved behavior tests.
- Existing async, collection, JSON, ASP.NET Core, operator, conversion, formatting, equality, and factory behavior remains green.
- All package targets build with zero warnings and errors, and package READMEs explain the new dependency boundary.

## ⏱️ Workload estimate

- Person-month basis: one developer working for one complete team-capacity month.
- Work-package range total: 0.05–0.10 person-months.
- Coordination, verification, migration, and rollout: 0.01–0.03 person-months.
- Contingency: 0.02–0.04 person-months for extension deconstruction, nullable metadata, analyzer, and binary-compatibility issues.
- Total planning range: 0.08–0.17 person-months.
- Confidence: Medium.
- Assumptions and excluded work: packages are not yet published from this uncommitted repository;
  downstream application migration and package publication are excluded.
- Re-estimation trigger and approval threshold: reapprove above 0.17 person-months or if approved
  language syntax cannot be preserved without another core member.

## 🚧 Plan blockers

None.

## 🗺️ Delivery map

| ID | Work package | Outcome | Priority and rationale | Estimate | Prerequisites | Status | Document |
| --- | --- | --- | --- | --- | --- | --- | --- |
| RES-012 | Move transferable APIs to Composition | Core becomes a minimal contract while Composition preserves fluent behavior | P2 — corrects the public package boundary | 0.05–0.10 person-months | ADR-0004 | Completed | `work-items/RES-012-move-transferable-apis-to-composition.md` |

## 📋 Completion evidence

- Core declares none of the transferable methods; the reflected public-surface contract test passes.
- Composition owns all 16 approved extension signatures and 59 behavior tests, including both
  deconstruction forms, positional switching, null validation order, and uninitialized callback Results.
- Core, Composition, System.Text.Json, and ASP.NET Core test projects pass 42, 59, 13, and 11 tests.
- `dotnet build TedToolkit.Results.slnx -c Release` succeeds for the complete target matrix with zero
  warnings and zero errors.
- Package READMEs document the dependency boundary, recompilation requirement, and the C# limitation
  around partially explicit generic arguments on extension methods.

## ⚠️ Risks and coordination

| Item | Impact | Next action |
| --- | --- | --- |
| Removing instance members is binary breaking | Existing binaries can fail to bind | Require recompilation and a major-version publication decision |
| Core-only source loses moved APIs | Consumers must add Composition | Document installation and keep the same namespace/call syntax |
| Extension deconstruction may differ in discovery | Switch/deconstruction syntax could regress | Add compile/runtime tests for both Result arities |
| Core and Composition versions can drift | Extensions may depend on unavailable core behavior | Release compatible versions together and keep only public-core dependencies |

Durable ownership remains in ADR-0004 and the architecture record. Public behavior remains in XML
documentation, contract tests, and package READMEs. This implementation design can be removed after
delivery once those durable records fully describe the current package family.
