# ADR-0002: Isolate Framework Integrations

- Status: Accepted
- Date: 2026-08-03
- Decision owner: User
- Decision scope: Package and dependency boundaries for JSON and ASP.NET Core integration.
- Applicable principles: None.
- Related change or work packages: `docs/changes/P2-result-usability-expansion/README.md`, RES-009, RES-010
- Supersedes: None
- Superseded by: None

## 📌 Decision at a glance

Keep `TedToolkit.Results` framework-independent and provide System.Text.Json and ASP.NET Core support
through two separate optional leaf packages that depend only on the core package.

## 🧭 Context and decision question

The core package currently contains one public Result abstraction and no direct package dependencies.
Planned usability work includes a stable JSON representation and HTTP-response conversion. Adding
those capabilities raises an enduring boundary question: whether integrations belong in the core
package, one combined integration package, or separate optional packages.

## 🎯 Decision drivers and constraints

| Type | Driver or constraint | Evidence or source | Priority |
| --- | --- | --- | --- |
| Hard constraint | `TFailure` remains an enum and stored failure values must be declared members so the failure set is closed and enumerable | User decision and approved change constraint on 2026-08-03 | Must |
| Hard constraint | Integration dependencies must not shrink the core target matrix | `CLAUDE.md` and `docs/architecture/result-package-boundaries.md` | Must |
| Decision driver | Consumers should not acquire unused web or serialization dependencies | Package-boundary objective approved by the user | High |
| Decision driver | JSON must not access the inactive throwing property | Current `Value` and `FailureReason` behavior | High |
| Decision driver | HTTP status semantics belong to the consuming application | Failure enums are consumer-defined and have no universal HTTP meaning | High |
| Documented evidence | ASP.NET Core class libraries use `Microsoft.AspNetCore.App` as a framework reference and should prioritize supported LTS versions | Microsoft Learn, "Use ASP.NET Core APIs in a class library" | Medium |
| Documented evidence | System.Text.Json is platform-provided on net6.0 and net7.0, while package 10.0.10 provides compatible assets for the remaining targets | Target-matrix build evidence and NuGet package metadata | Medium |

## 🔎 Options and evidence

| Option | Evidence and confidence | Meets drivers | Decisive trade-off | Outcome |
| --- | --- | --- | --- | --- |
| Keep the core only and require every consumer to write adapters | Current status; High | Preserves boundaries but duplicates non-trivial converters | No maintained interoperability contract | Rejected |
| Add JSON and ASP.NET Core APIs to `TedToolkit.Results` | Direct implementation; High | Fails dependency and target-isolation drivers | Every consumer acquires framework coupling | Rejected |
| Create one combined integrations package | Structural alternative; High | Keeps core clean but couples unrelated integrations | JSON-only consumers acquire ASP.NET Core coupling | Rejected |
| Create separate System.Text.Json and ASP.NET Core packages | Structural alternative plus platform documentation; High | Meets every hard constraint and driver | Adds two projects and package surfaces to maintain | Selected |

## ✅ Decision

Create `TedToolkit.Results.SystemTextJson` and `TedToolkit.Results.AspNetCore` as separate optional
packages. Each package references `TedToolkit.Results`; neither integration references the other,
and the core references neither.

The JSON package mirrors all nine core target frameworks. It uses the platform-provided
System.Text.Json reference on `net6.0` and `net7.0`, and centrally managed package version 10.0.10
on the remaining targets. The ASP.NET Core package targets `net8.0` and `net10.0` with a
`Microsoft.AspNetCore.App` framework reference.

The JSON package owns an explicit discriminated-union schema and converter factory, and rejects
undefined failure enum values as invalid JSON. The ASP.NET Core
package may provide conventional success responses, but it requires the consumer to map every
failure enum value to an HTTP response. No global mutable mapping registry is introduced.

## 💡 Why this decision now

Serialization and web conversion create framework dependencies and public contracts that are harder
to reverse than local combinator methods. Separate leaf packages preserve the existing core target
matrix, prevent unrelated transitive dependencies, and keep consumer-specific HTTP semantics outside
the reusable Result model.

Reconsider the decision only if package proliferation causes demonstrated maintenance harm, the
platforms provide a common dependency-free interoperability contract, or an integration cannot be
implemented without changing the core representation.

## 🔗 Evidence and links

- Microsoft Learn: https://learn.microsoft.com/aspnet/core/fundamentals/target-aspnetcore
- System.Text.Json package: https://www.nuget.org/packages/System.Text.Json
- Current core project: `src/TedToolkit.Results/TedToolkit.Results.csproj`
- Architecture boundary: `docs/architecture/result-package-boundaries.md`

No benchmark or proof of concept is required because performance does not decide the package
boundary.

## ⚖️ Consequences and accepted trade-offs

- The core remains usable without serializer or web-framework dependencies.
- Each integration can choose platform-compatible target frameworks without changing the core.
- The repository gains two projects, test projects, package READMEs, and release artifacts.
- JSON shape becomes a versioned public compatibility contract of the JSON package.
- ASP.NET Core consumers must provide an explicit failure mapping; this is deliberate duplication of
  application policy rather than hidden global behavior.
- Cross-package versions must remain compatible and be released coherently.

## 🛠️ Implementation handoff

RES-009 must implement the JSON package without attributes or references in the core type. RES-010
must implement the ASP.NET Core package with a `Microsoft.AspNetCore.App` framework reference and
consumer-supplied failure mappings. Both work packages must verify that the core project remains
unchanged by their platform dependencies.

## 🔄 Rollout, rollback, and exit

The integrations are additive packages. Rollback stops publishing and removes the affected leaf
package without changing the core binary contract. A published JSON schema must be versioned or
migrated rather than silently changed.

## 📅 Follow-ups and review triggers

| Item | Owner | Due date or objective trigger | Status |
| --- | --- | --- | --- |
| Pin this ADR and the architecture record to a full Git revision | TedToolkit maintainers | First repository commit containing the approved records | Open |
| Reassess supported ASP.NET Core TFMs | TedToolkit maintainers | An LTS framework enters or leaves support | Open |
| Reassess System.Text.Json 10.0.10 | TedToolkit maintainers | A supported target, compatibility, or security requirement changes | Open |
