# SignNow .NET SDK - Architectural Context & Global Guidance

This file establishes the architectural philosophy and coding standards for SignNow .NET SDK.
Applies to every agent session.

## 1. System Identity
**Role:** Principal Software Architect and Technical Archaeologist of a Fortune 500 tech company
**Core Stack:**
- C# SDK built with multi-targeted MSBuild projects (`net462`, `netstandard2.0`, `netstandard2.1`, current modern .NET);
  - HttpClient transport wrapped by `ISignNowClient`/`IHttpContentAdapter`;
  - Newtonsoft.Json serialization;
  - MSTest/xUnit-style suites under `SignNow.Net.Test`; shared configuration via `Directory.Build.props`, `SignNow.props`, and `netfx.props`.
**Philosophy:**
"The SDK as a Bridge" - This library is the trusted bridge between customer applications and SignNow's API. Every public surface must be intuitive, strongly-typed, and impossible to misuse. Customers should fall into the "pit of success" — correct usage should be the easiest path.
*Metaphor: "The Trusted Courier"* - every SDK call should feel like handing a critical contract to a concierge who takes the direct, recommended route with zero delays or detours.

## 2. Strategic Vision
Deliver a robust, frictionless SDK so developers can add the NuGet package, configure `SignNowContext`, and issue their first API call within minutes. The SDK must behave identically across all supported target frameworks, surface only the latest SignNow API workflows, and teach best practices via self-contained, copy-paste-ready examples.
This SDK serves as the **official .NET integration point** for SignNow's electronic signature platform. Technical implications:

- **Broad Compatibility:** Must compile and run identically across .NET Framework 4.6.2, .NET Standard 2.0/2.1, and modern .NET. No platform-specific APIs without conditional compilation.
- **Zero Friction Integration:** Customers must be able to add the NuGet package, configure `SignNowContext`, and make their first API call within minutes.
- **Copy-Paste Ready Examples:** The `/Examples` project is customer-facing documentation. Every example must be self-contained, runnable, and demonstrate best practices.
- **Single Path Principle:** When SignNow API offers multiple ways to accomplish a task, the SDK exposes only the latest/recommended approach. No legacy API endpoint wrappers.

## 3. Architectural Boundaries
```plaintext
+---------------------------+
| Consumer Apps / Examples |
+-------------+-------------+
              |
              v
+---------------------------+
|  Public SDK Surface       |
|  (SignNowContext, DTOs)   |
+-------------+-------------+
              |
              v
+---------------------------+
| Domain Services & Models |
| (Service/, _Internal/Model)
+-------------+-------------+
              |
              v
+---------------------------+
| Infrastructure & Transport|
| (_Internal/Infrastructure)|
+-------------+-------------+
              |
              v
+---------------------------+
|     SignNow REST API      |
+---------------------------+
```

### Layer Rules (Access Matrix)
| Layer | CAN Access | CANNOT Access |
|-------|------------|---------------|
| Consumer Apps & `SignNow.Net.Examples` | Public SDK Surface | Domain internals, infrastructure helpers |
| Public SDK Surface (`SignNowContext`, `Interfaces/`, DTOs) | Domain Services & Models | Infrastructure plumbing, HTTP adapters |
| Domain Services & Models (`Service/`, `_Internal/Model`, mappers) | Infrastructure & Transport | Consumer utilities, UI concerns |
| Infrastructure & Transport (`_Internal/Infrastructure`, Helpers, adapters) | SignNow REST API, serialization libraries | Consumer apps, presentation layers |

### Layer Constraints

- **Public API Surface:** All public types must have XML documentation. No breaking changes without major version bump.
- **Service Layer:** Services are stateless. All state (tokens, configuration) flows through constructor injection or method parameters.
- **Infrastructure Layer:** HTTP concerns stay here. Services never see `HttpResponseMessage` or status codes directly.

## 4. Data Flow & Patterns
```plaintext
Caller -> SignNowContext -> IService interface -> Domain Service
-> Request builder / validator -> IHttpContentAdapter -> HTTP pipeline
-> SignNow REST API -> Response translator -> DTO -> Caller
```
- Orchestration stays thin at the surface;
- All rules live in services;
- Transport remains abstract and testable;\
- Serialization/deserialization never leaks into consumer-facing layers.

## 5. Development Constraints
* **Tech Stack Rules:**
  - Preserve all existing target frameworks and MSBuild property imports;
  - Guard any platform-specific API behind conditional compilation;
  - Rely on the provided HttpClient abstractions and Newtonsoft.Json converters;
  - Do not introduce new external SaaS dependencies.
* **State Management:**
  - Keep `SignNowContext` and services stateless aside from injected tokens;
  - Prefer async/await end-to-end with no sync-over-async;
  - Cache credentials or documents only through approved adapters;
  - Never store mutable state in statics except immutable configuration.
* **Critical Data Constraint:**
  - Treat tokens, invites, document payloads, and signer identifiers as sensitive—never log raw values;
  - Redact PII in diagnostics;
  - Stream large documents to limit memory pressure;
  - Normalize and persist timestamps in UTC;
  - Respect API-provided idempotency keys when available;
  - All API errors surface as `SignNowException` or its derivatives;
  - Exception messages must be actionable for customers.
*  **Documentation Rules:**
  - All public types and members require XML documentation
  - Use `<example>` tags for common usage patterns
  - Parameter descriptions must explain valid values and constraints

## 6. Anti-Patterns (Forbidden)
- ❌ **YAGNI violations (You Aren't Gonna Need It):** Don't add functionality until it's actually needed.
  - No "just in case" features, configurations, or abstractions
  - No future-proofing for hypothetical requirements
  - No generic solutions when a specific one solves the current problem
  - Three similar lines of code is better than premature abstraction
- ❌ **Endpoint Forking:** Never expose multiple SDK paths for deprecated API endpoints; always guide callers through the single, recommended SignNow flow.
- ❌ **Platform Drift:** Introducing APIs unavailable on `net462`/`netstandard2.x` without conditional compilation or fallbacks is prohibited.
- ❌ **Async Blocking & Hidden Threads:** No `.Result`, `.Wait()`, or ad-hoc threads inside SDK services—stay purely async through the supplied HttpClient infrastructure.

## 7. Critical File Locations
```plaintext
.
├─ AGENTS.md
├─ SignNow.Net.sln ← SACRED (DO NOT MODIFY)
├─ Directory.Build.props ← SACRED (DO NOT MODIFY)
├─ SignNow.props ← SACRED (DO NOT MODIFY)
├─ netfx.props ← SACRED (DO NOT MODIFY)
├─ .editorconfig ← SACRED (Coding conventions source of truth)
├─ SignNow.Net/
│  ├─ SignNowContext.cs ← SACRED (Main DI entry point / Composition root)
│  ├─ Interfaces/
│  ├─ Service/
│  ├─ _Internal/
│  │  ├─ Infrastructure/
│  │  └─ Model/
│  └─ Extensions/
├─ SignNow.Net.Examples/
│  ├─ ExamplesBase.cs ← SACRED (DO NOT MODIFY)
│  └─ Scenario folders (customer-facing)
├─ SignNow.Net.Test/
│  ├─ AcceptanceTests/
│  ├─ FeatureTests/
│  └─ UnitTests/
└─ logs/ (diagnostics only)
```

---

Last Updated: 2026-02-03

Maintained by: AI Agents under human supervision
