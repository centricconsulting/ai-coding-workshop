---
applyTo: 'src-swift/**'
---

# GitHub Copilot Instructions for Swift Workshop Track

> These instructions are automatically applied to all GitHub Copilot suggestions when working with `src-swift/`.

## 0) Workshop Mode
- Assume **Swift 5.9+ (Swift Package Manager)**, targeting **iOS 17+ / macOS 14+**, **XCTest**, and **SwiftUI** for the app target (`TaskManagerApp`).
- Prefer **Clean Architecture** target layout and **DDD** patterns.
- Always generate examples and code in **English**.

---

## 1) Workflow (TDD + Build Hygiene)
- **TDD first**: when asked to implement a feature, propose/emit tests before code.
- After you output code, assume we run `swift test` (package) and the Xcode build for `TaskManagerApp` and fix warnings/errors before committing.
- When referencing rule sets, state what you followed (e.g., "Used: Clean Architecture, DDD, Tests").

---

## 2) Project Architecture (Clean Architecture, SwiftPM)
`Package.swift` declares three library targets plus matching test targets:
- `TaskManagerDomain` — entities, value objects, `TaskRepository` protocol, `TaskError`. No dependencies.
- `TaskManagerApplication` — use cases (`CreateTask`, `CompleteTask`, `ListTasks`) orchestrating the domain. Depends on `TaskManagerDomain` only.
- `TaskManagerInfrastructure` — adapters (e.g., `InMemoryTaskRepository`, `LegacyTaskProcessor`) implementing domain protocols. Depends on `TaskManagerDomain`.
- `TaskManagerApp` (separate Xcode project) — SwiftUI views/view models (e.g., `TaskListView`, `TaskListViewModel`); presentation layer only, delegates to the application/use-case targets.

Enforce dependency direction: `TaskManagerApp` → `TaskManagerApplication` → `TaskManagerDomain`; `TaskManagerInfrastructure` → `TaskManagerDomain`. Never import Foundation-only conveniences into invariant logic if it couples the domain to a specific runtime unnecessarily; keep `TaskManagerDomain` dependency-free beyond `Foundation`.

---

## 3) Swift Coding Style
- `PascalCase` for types (`struct`, `class`, `enum`, `protocol`), `camelCase` for properties/functions/parameters/local variables.
- Model aggregates as `struct` conforming to `Identifiable, Equatable, Sendable` (see `Task`), with a **private `init`** plus static `create(...)`/`reconstitute(...)` factory functions that `throw` on invariant violations.
- Represent invalid states as a dedicated `Error`-conforming `enum` (e.g., `TaskError`), and throw it from factories/business methods rather than using `fatalError` or optionals for error signaling.
- Business methods return a **new value** (`updatingStatus(to:now:)`, `updatingDetails(...)`) instead of mutating `self`, matching the immutable `struct` + `let` properties pattern already in `Task.swift`.
- Provide default parameter values for injectable seams useful in tests (e.g., `now: Date = Date()`, `id: TaskID = .new()`).
- Use `guard` for early-exit validation; prefer `guard let` over force-unwrapping.

---

## 4) DDD Modeling Rules
- Aggregates (`Task`) expose only factory functions (`create`, `reconstitute`) and named business methods (`updatingStatus`, `updatingDetails`) — no public mutators; all stored properties are `let`.
- Value objects (`TaskID`, `TaskStatus`) are small, immutable, `Equatable`/`Sendable` types.
- Repositories are protocols in `TaskManagerDomain` (e.g., `TaskRepository`) with **business-intent method names**, implemented by adapters in `TaskManagerInfrastructure`.
- Use cases in `TaskManagerApplication` are small, single-purpose types (`CreateTask`, `CompleteTask`, `ListTasks`) rather than one large "service" god-object.

---

## 5) Testing Rules
- **Test framework**: `XCTest`, run via `swift test`.
- Test classes are `final class <Type>Tests: XCTestCase` under `Tests/<Target>Tests/`, mirroring the target under test (`TaskManagerDomainTests`, `TaskManagerApplicationTests`, `TaskManagerInfrastructureTests`).
- Name test methods `test<Behavior>` describing the expected outcome (e.g., `testCompletedTasksCannotBeReopened`).
- Use `XCTAssertThrowsError(...) { error in XCTAssertEqual(error as? TaskError, .expectedCase) }` for invariant/error-path tests instead of only asserting that *some* error was thrown.
- Inject fixed `Date`/`id` values via the factories' default parameters to keep tests deterministic (avoid relying on real `Date()` in assertions).

---

## 6) Conventional Commits
- Use `<type>([optional scope]): <description>` with 72-char subject limit.
- Types: `feat|fix|docs|style|refactor|perf|test|build|ci|chore|revert`.

---

## 7) Documentation Organization
- All documentation lives in `docs/` at the repository root (e.g., `docs/labs/lab-01-tdd-with-copilot-swift.md`).
- Keep any `src-swift/README.md` focused on setup/run instructions (`swift build`, `swift test`, opening `TaskManagerApp.xcodeproj`).

---

## 8) Guardrails (Workshop)
- Do **not** invent external dependencies (third-party packages) without being asked — this track is plain SwiftPM + XCTest + SwiftUI.
- Keep domain logic **out of** `TaskManagerApp` (SwiftUI views/view models) and `TaskManagerInfrastructure`.
- If a rule conflicts, **Clean Architecture boundaries win** (then DDD, then Swift API design guidelines).
