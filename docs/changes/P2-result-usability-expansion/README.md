# Expand Result Usability and Integrations

## 📌 Status

Completed

- Change owner: TedToolkit maintainers
- Approval owner: User

## 🚦 Change priority

- Priority: P2
- Rationale: This is planned, valuable API and interoperability work with no active outage, hard deadline, or security exposure.
- Directory: `docs/changes/P2-result-usability-expansion/`

## 🎯 Intended outcome and scope

Make the closed-enum Result model convenient for Boolean conditional syntax, branch-safe procedural
code, exhaustive switch expressions, synchronous and asynchronous pipelines, value-less commands,
collections, JSON, and ASP.NET Core boundaries.

Scope includes declared failure-value enforcement, `if (result)` and `if (!result)` conditional
syntax, branch consumption and deconstruction, synchronous transformation and chaining, validation
and recovery, a value-less Result form, Task-based composition, fail-fast collection composition,
and separate System.Text.Json and ASP.NET Core integration packages.

The change is additive relative to the approved explicit-state value representation and preserves
its construction, conversion, initialization, equality, exception, nullable-success, formatting,
namespace, and target-framework contracts. Existing public APIs are not removed or renamed.

Non-goals are arbitrary failure types, multiple accumulated failures, embedded messages or
exceptions, localization, exception capture, ValueTask or asynchronous-stream combinators, LINQ
query syntax, global HTTP mappings, Newtonsoft.Json, and changing the existing diagnostic format.

## 🧾 Source intent and hard constraints

- Source request: Record and plan the complete Result usability expansion approved on 2026-08-03,
  including `public void Deconstruct` for switch patterns and Boolean conditional operators for
  `if (result)` and `if (!result)`.
- User or business outcome: Consumers can exhaustively handle a closed enum failure set and compose
  Result-returning operations without repetitive manual branching.
- External hard constraints: `TFailure` must remain `where TFailure : struct, Enum`; every stored
  failure value must satisfy `Enum.IsDefined`; repository and public documentation
  remain English; the core package keeps its existing target-framework matrix.

## 🧩 Governing principles and decisions

- Applicable principles and revision: none.
- Related ADRs and status: `docs/adr/ADR-0001-result-value-representation/README.md` (Accepted with
  a typed int-backed state enum, not yet committed), `docs/adr/ADR-0002-isolate-framework-integrations.md`
  (Accepted, not yet committed), and `docs/adr/ADR-0003-isolate-optional-composition.md`
  (Accepted, not yet committed).
- Related architecture records and revision: `docs/architecture/result-package-boundaries.md`
  (Active, not yet committed).
- Resulting constraints: Task/collection composition and framework integrations are separate leaf
  packages; the core exposes only the Result contract and intrinsic synchronous operations; enum
  failures remain closed; HTTP failure policy stays consumer-owned.
- Reapproval trigger: any public signature, failure constraint, JSON schema, integration boundary,
  target-framework strategy, or compatibility expectation changes.

The separately approved `P2-result-value-type-migration` is complete and its implemented behavior is
the current baseline. The user approved this change for implementation on 2026-08-03. Because the
repository has no Git commit yet, the current uncommitted ADR and architecture content is the
approved implementation baseline and must be pinned after the initial repository commit.

## 🧭 Planned approach

Deliver nine dependency-ordered work packages. First enforce declared failure values, then add
branch-safe consumption and synchronous composition. Add
validation/recovery and the value-less form next. Build Task and collection composition on those
stable contracts in an optional Composition package. Finally add JSON and ASP.NET Core as isolated
leaf packages.

Use explicit branch propagation throughout: a callback executes only for its active branch, failure
values are preserved exactly unless `MapFailure` or recovery explicitly changes them, and callback
exceptions or cancellation propagate without being converted into Result failures.

`Deconstruct` exposes `(isSuccess, value, failureReason)` for positional switch patterns. The
inactive payload is `default`; consumers must use `isSuccess` as the discriminator. Every
branch-dependent API, including Deconstruct, rejects an Uninitialized Result.

Define paired `operator true` and `operator false` plus `operator !` on both Result arities.
`if (result)` follows `IsSuccess`; `if (!result)` follows `IsFailure`. Do not define an implicit
conversion to `bool`, `operator &`, or `operator |`.

## ✅ Completion criteria

- Every public API and integration contract in RES-003 through RES-011 is implemented and documented.
- Every behavior case has deterministic automated coverage through the repository's documented
  `dotnet run` test entry points.
- Core, Composition, and integration packages build with zero warnings and errors for their approved target matrices.
- The core package has no Task/collection extension types, serializer dependency, or ASP.NET Core dependency.
- The JSON package round-trips both Result arities and rejects malformed or contradictory union data.
- The ASP.NET Core package produces conventional success responses and invokes only the explicit
  consumer failure mapping for failures.
- Package READMEs show exhaustive switch, pipeline, JSON, and web-boundary examples in English.
- Package READMEs show `if (result)` and `if (!result)` without implying that Result is generally
  convertible to `bool`.

## ⏱️ Workload estimate

- Person-month basis: one developer working for one complete team-capacity month.
- Work-package range total: 0.34–0.63 person-months.
- Coordination, verification, migration, and rollout: 0.05–0.10 person-months.
- Contingency: 0.05–0.10 person-months for generic overload resolution, legacy-target nullable
  annotations, serializer contract edge cases, and ASP.NET Core cross-version differences.
- Total planning range: 0.44–0.83 person-months.
- Confidence: Medium.
- Assumptions and excluded work: no downstream source migration or package publication; TUnit and
  the existing build infrastructure remain available; platform integrations use only Microsoft
  platform dependencies; exclusions match the non-goals above.
- Re-estimation trigger and approval threshold: reapprove if scope exceeds 0.83 person-months, a
  package boundary changes, an integration needs a core dependency, or public compatibility changes.

## 🚧 Plan blockers

None. Pin the approved records to a full Git revision after the repository receives its initial
commit; this tracking action does not block the explicitly authorized implementation.

## 🗺️ Delivery map

| ID | Work package | Outcome | Priority and rationale | Estimate | Prerequisites | Status | Document |
| --- | --- | --- | --- | --- | --- | --- | --- |
| RES-011 | Enforce declared failure values | Every active failure is a declared enum member | P2 — makes the closed failure-set guarantee true at runtime | 0.02–0.04 person-months | Completed RES-002 | Implemented | `work-items/RES-011-enforce-declared-failure-values.md` |
| RES-003 | Add exhaustive branch consumption | Safe procedural, Match, and switch-pattern consumption | P2 — removes repeated unsafe branch access | 0.02–0.04 person-months | RES-011 | Implemented | `work-items/RES-003-add-exhaustive-branch-consumption.md` |
| RES-004 | Add synchronous composition | Values and failures transform and chain without manual branching | P2 — core usability outcome | 0.03–0.06 person-months | RES-003 | Implemented | `work-items/RES-004-add-synchronous-composition.md` |
| RES-005 | Add validation and recovery | Successful values can be checked and failures explicitly recovered | P2 — common pipeline control flow | 0.02–0.04 person-months | RES-004 | Implemented | `work-items/RES-005-add-validation-and-recovery.md` |
| RES-006 | Add value-less Result | Commands model success without dummy values | P2 — closes a common modeling gap | 0.04–0.07 person-months | RES-003 | Implemented | `work-items/RES-006-add-value-less-result.md` |
| RES-007 | Add Task composition | Async operations preserve the synchronous Result semantics | P2 — enables application service pipelines | 0.05–0.09 person-months | RES-004, RES-005, RES-006 | Implemented | `work-items/RES-007-add-task-composition.md` |
| RES-008 | Add collection composition | Ordered collections traverse and combine with deterministic fail-fast behavior | P3 — useful but deferrable convenience | 0.04–0.08 person-months | RES-004, RES-006 | Implemented | `work-items/RES-008-add-collection-composition.md` |
| RES-009 | Add System.Text.Json integration | Both Result arities have an explicit round-trip JSON contract | P3 — optional boundary integration | 0.06–0.10 person-months | RES-003, RES-006, ADR-0002 | Implemented | `work-items/RES-009-add-system-text-json-integration.md` |
| RES-010 | Add ASP.NET Core integration | MVC and minimal APIs convert Results without universal failure mappings | P3 — optional boundary integration | 0.06–0.11 person-months | RES-003, RES-006, ADR-0002 | Implemented | `work-items/RES-010-add-aspnet-core-integration.md` |

## 📋 Completion evidence

- The full solution builds in Release with zero warnings and errors.
- Core tests: 83 passed, including the exact three-type public contract surface and typed
  `ResultState` behavior.
- Composition tests: 15 passed; the package builds all nine core target frameworks.
- System.Text.Json tests: 13 passed; the package builds all nine target frameworks without support
  warning suppression.
- ASP.NET Core tests: 11 passed; the package builds `net8.0` and `net10.0`.
- All four generated NuGet packages contain their package README and the optional packages depend on
  the core package without reverse dependencies.

## ⚠️ Risks and coordination

| Item | Impact | Next action |
| --- | --- | --- |
| `TValue` can be the same enum as `TFailure` | Existing constructors and implicit conversions can be ambiguous | Keep existing APIs for compatibility, make named factories canonical in documentation, and do not add more implicit conversions |
| Generic callback overloads can become ambiguous | Consumers may need casts or explicit type arguments | Prefer distinct names such as `RecoverWith`; compile representative edge cases before approval |
| Inactive deconstruction payloads use `default` | Payloads cannot independently identify the branch | Document and test `isSuccess` as the only discriminator |
| The value representation has an Uninitialized default state | New APIs could accidentally treat default as a branch | Require every branch-dependent API and integration to reject Uninitialized values |
| Boolean-like syntax can hide a three-state representation | Consumers may assume default Result is simply false | Make both positive and negated conditions reject Uninitialized and omit implicit bool conversion |
| A generic enum constraint permits undefined numeric values | Consumers cannot truly enumerate runtime failures | Centralize `Enum.IsDefined` validation and reject undefined values at every creation boundary |
| JSON shape becomes durable public data | Silent schema edits could break stored or exchanged data | Treat converter contract tests as compatibility tests and version breaking changes |
| HTTP meaning varies by application | Built-in status mappings could be incorrect | Require a consumer failure mapping on every conversion call |
| Large API surface across sync and Task forms | Overload count can reduce discoverability | Keep ValueTask, async streams, LINQ aliases, and side-effect helpers out of scope |

Durable behavior belongs in public XML documentation, compatibility tests, package READMEs, the
architecture boundary, ADR-0002, and ADR-0003. The enum-failure rule may later warrant a repository design
principle if more packages adopt it. The change design remains useful through delivery and may be
removed after completion if the durable contracts fully replace it.
