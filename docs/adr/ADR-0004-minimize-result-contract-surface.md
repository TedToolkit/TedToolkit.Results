# ADR-0004: Minimize the Result Contract Surface

- Status: Superseded
- Date: 2026-08-04
- Decision owner: User
- Decision scope: Public API and package ownership for core Result behavior and optional consumer conveniences.
- Applicable principles: None.
- Related change or work packages: `docs/changes/P2-minimize-result-contract-surface/README.md`, RES-012
- Supersedes: ADR-0003
- Superseded by: ADR-0005

## 📌 Decision at a glance

Keep only Result construction, state, payload access, and non-transferable C# language members in
`TedToolkit.Results`; move every transferable branch-consumption and composition method to
`TedToolkit.Results.Composition`.

## 🧭 Context and decision question

DDD-style contract projects need to declare and exchange Result values without acquiring a broad
behavioral API. ADR-0003 separated Task and collection extensions but retained synchronous
composition as instance members in the core assembly. The remaining question is whether the core
should continue owning transferable convenience methods or become a minimal value contract.

Some language features cannot be expressed as extension methods: conditional operators, conversion
operators, and the `ToString` override must be declared by the Result type. Static factories also
belong to construction and are required for a value-less successful Result. Those constraints set
the lower practical boundary of the core package.

## 🎯 Decision drivers and constraints

| Type | Driver or constraint | Evidence or source | Priority |
| --- | --- | --- | --- |
| Hard constraint | Contract projects must construct both Result arities without Composition | Value-less success requires `Result.Succeed<TFailure>()` | Must |
| Hard constraint | `if (result)`, `if (!result)`, conversions, and safe diagnostic formatting remain supported | User-approved language behavior and C# operator rules | Must |
| Hard constraint | Core retains the nine-target framework matrix and no optional package dependency | Current package architecture | Must |
| Decision driver | Transferable consumer conveniences should not enlarge the contract assembly | User-approved DDD contract boundary | High |
| Decision driver | Ordinary inferred fluent calls should remain available after installing Composition | Extension methods in the existing namespace | High |
| Compatibility constraint | Removing instance methods from core is a binary breaking change | CLR member ownership | Must document |

## 🔎 Options and evidence

| Option | Evidence and confidence | Meets drivers | Decisive trade-off | Outcome |
| --- | --- | --- | --- | --- |
| Keep synchronous instance methods in core | Current implementation; High | Fails the minimal-contract driver | Core-only consumers receive behavior they do not need | Rejected |
| Move only Map and Bind families | Direct structural alternative; High | Partially meets the driver | Leaves an arbitrary split for Match, TryGet, Ensure, and Recover | Rejected |
| Move every transferable method to Composition | C# extension-method rules and current package structure; High | Meets every hard constraint and driver | Requires a Composition dependency for those APIs and breaks old binaries | Selected |
| Remove every method from core | C# language rules; High | Cannot preserve required operators, conversions, or safe formatting | Required language behavior would be lost | Rejected |

## ✅ Decision

`TedToolkit.Results` owns:

- `Result<TValue, TFailure>`, `Result<TFailure>`, and the static `Result` factory;
- public construction and the `IsInitialized`, `IsSuccess`, `IsFailure`, `Value`, and
  `FailureReason` properties;
- conditional operators, conversion operators, record equality, and the safe `ToString` override;
- internal state and invariant validation required to keep those contracts valid.

`TedToolkit.Results.Composition` owns extension methods for:

- `Match`, `TryGetValue`, `TryGetFailure`, and `Deconstruct`;
- `Map`, `Bind`, `MapFailure`, `Ensure`, `Recover`, and `RecoverWith`;
- the existing Task and collection composition families.

All Composition extensions remain in namespace `TedToolkit.Results`, preserving ordinary inferred
fluent source syntax when the optional package is referenced. No public `IResult` interface is
introduced by this decision.

## 💡 Why this decision now

The package family already has a dedicated Composition boundary, so keeping transferable
synchronous behavior in the core no longer serves dependency or discoverability needs. Moving the
complete transferable surface gives contract projects a coherent minimal package, while retaining
language features that C# cannot relocate and construction needed to produce Result values.

Reconsider this decision only if a transferable method becomes necessary to uphold a core value
invariant, extension syntax cannot preserve an approved language scenario, or measured package
fragmentation creates greater consumer harm than the smaller contract surface prevents.

## 🔗 Evidence and links

- Current architecture: `docs/architecture/result-package-boundaries.md`
- Superseded boundary: `docs/adr/ADR-0003-isolate-optional-composition.md`
- Core types: `src/TedToolkit.Results/Result{TValue,TFailure}.cs`, `src/TedToolkit.Results/Result{TFailure}.cs`
- Composition package: `src/TedToolkit.Results.Composition/TedToolkit.Results.Composition.csproj`

No benchmark is required because the decision is governed by package responsibility and language
constraints rather than a performance comparison.

## ⚖️ Consequences and accepted trade-offs

- Contract projects reference only the small core package and can still construct and inspect Results.
- Consumers need Composition for Match, TryGet, deconstruction, and synchronous or asynchronous pipelines.
- Ordinary inferred fluent calls remain unchanged after adding the package because extensions keep
  the core namespace. Calls that supplied only an instance method's own generic arguments must use
  inference or a typed delegate because C# extension methods cannot partially specify type arguments.
- Existing binaries compiled against removed instance methods are incompatible and require recompilation.
- Published packages require a major-version compatibility decision and coordinated core/Composition versions.
- XML documentation and behavior tests move with the APIs that now own them.

## 🛠️ Implementation handoff

RES-012 must move the complete transferable API as one coherent boundary, preserve exception and
nullable-flow behavior, protect the remaining core public surface with contract tests, and verify
extension deconstruction and fluent calls through a Composition-only consumer reference.

## 🔄 Rollout, rollback, and exit

Before publication, rollback restores the instance methods and their tests to core. After
publication, consumers add `TedToolkit.Results.Composition` and recompile; package documentation
must identify the dependency change. Restoring the methods later would duplicate public APIs and
requires another architecture decision.

## 📅 Follow-ups and review triggers

| Item | Owner | Due date or objective trigger | Status |
| --- | --- | --- | --- |
| Pin this ADR to a full Git revision | TedToolkit maintainers | First repository commit containing the decision | Open |
| Reassess binary compatibility and major version | TedToolkit maintainers | Before first affected package publication | Open |
| Reassess core surface | TedToolkit maintainers | A new transferable public method is proposed for core | Open |
