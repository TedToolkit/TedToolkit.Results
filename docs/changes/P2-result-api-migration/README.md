# Port the Result API

## 📌 Status

Completed

- Change owner: TedToolkit maintainers
- Approval owner: User

## 🚦 Change priority

- Priority: P2
- Rationale: This is planned, valuable package functionality with no active outage or external deadline.
- Directory: `docs/changes/P2-result-api-migration/`

## 🎯 Intended outcome and scope

Provide a public `TedToolkit.Results` package API for explicit success and failure outcomes. Port
the observable behavior of the current uncommitted
`Roboticplus.BevelCutting.Contracts/Results` source into the new namespace while supporting every
target framework already configured by the package.

Scope includes the generic result record, non-generic factory, constructors, branch accessors,
conversions, record equality, diagnostic formatting, focused TUnit tests, and package documentation.

Non-goals are consumer migration in Roboticplus.BevelCutting, serialization guarantees, localized
runtime resources, and additional APIs such as `Map`, `Bind`, `Match`, or asynchronous variants.

## 🧾 Source intent and hard constraints

- Source request: Port the functionality currently under
  `C:/Work/Codes/Roboticplus.BevelCutting/src/Roboticplus.BevelCutting.Contracts/Results`.
- User outcome: Publish the behavior as a reusable TedToolkit package.
- External hard constraints: TedToolkit is internationalized software. Authored code, tests,
  runtime messages, identifiers, comments, XML documentation, examples, and repository
  documentation must not contain Chinese text.

## 🧩 Governing principles and decisions

- Applicable principles and revision: none; this repository has no committed governing records yet.
- Related ADRs and status: none.
- Related architecture records and revision: none.
- Approved constraints: use C#, English-authored text, the existing package boundary, explicit
  acyclic test dependency, TUnit via `dotnet run`, and all configured target frameworks.
- Reapproval trigger: public API shape, enum failure constraint, formatting contract, target
  frameworks, or internationalization policy changes before completion.

## 🧭 Planned approach

Deliver one work package. Re-express the source behavior under `TedToolkit.Results`, replace APIs
that are unavailable on older target frameworks with equivalent portable code, and prove each
observable behavior through focused deterministic unit tests. Preserve the success/failure
invariant and diagnostic Emoji output, but use English exception messages and documentation.

Direct file copying was rejected because it would retain the source company's copyright header,
Chinese-authored text, the old namespace, and a null-check API unavailable on older targets.

## ✅ Completion criteria

- The approved public API and behavior cases are implemented without Chinese-authored text.
- Focused TUnit tests pass through the documented `dotnet run` command.
- The full Release solution and Build project compile with zero warnings and errors.
- The NuGet package is generated for all configured target frameworks.
- README and XML documentation describe the implemented API in English.

## ⏱️ Workload estimate

- Person-month basis: one developer working for one complete team capacity month.
- Work-package range total: 0.03–0.06 person-months.
- Coordination, verification, migration, and rollout: 0.01–0.02 person-months.
- Contingency: 0.01 person-months for multi-target analyzer or compatibility findings.
- Total planning range: 0.05–0.09 person-months.
- Confidence: High.
- Assumptions and excluded work: no downstream consumer migration, package publication, or new API
  families; the uncommitted source snapshot remains the behavioral reference.
- Re-estimation trigger and approval threshold: reapprove if scope exceeds the upper bound or changes
  the public API, compatibility, or internationalization constraints.

## 🚧 Plan blockers

None.

## 🗺️ Delivery map

| ID | Work package | Outcome | Priority and rationale | Estimate | Prerequisites | Status | Document |
| --- | --- | --- | --- | --- | --- | --- | --- |
| RES-001 | Port Result API | A tested, documented, multi-target Result package | P2 — planned reusable functionality | 0.03–0.06 person-months | None | Implemented | `work-items/RES-001-port-result-api.md` |

## ⚠️ Risks and coordination

| Item | Impact | Next action |
| --- | --- | --- |
| Source files are uncommitted | The source snapshot can change without a revision identifier | Treat the inspected files and tests as the approved snapshot for this work package |
| New namespace | Existing Roboticplus consumers are not source-compatible without edits | Keep consumer migration out of scope and document the package namespace |
| Multi-target package | A direct implementation may use APIs unavailable on older targets | Build all configured frameworks in Release |

The durable contract belongs in public XML documentation, focused tests, and the package README.
Human review may delete this process-oriented design after implementation; no ADR or architecture
record is expected.
