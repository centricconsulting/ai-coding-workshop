---
applyTo: 'src-kotlin/**'
---

# GitHub Copilot Instructions for Kotlin Workshop Track

> These instructions are automatically applied to all GitHub Copilot suggestions when working with `src-kotlin/`.

## 0) Workshop Mode
- Assume **Kotlin/JVM (toolchain 21)**, **Gradle Kotlin DSL** (multi-module), **JUnit 5 (Jupiter)**, and **MockK** (not Mockito).
- Prefer **Clean Architecture** module layout and **DDD** patterns.
- Always generate examples and code in **English**.

---

## 1) Workflow (TDD + Build Hygiene)
- **TDD first**: when asked to implement a feature, propose/emit tests before code.
- After you output code, assume we run `./gradlew test` and fix warnings/errors before committing.
- When referencing rule sets, state what you followed (e.g., "Used: Clean Architecture, DDD, Tests").

---

## 2) Project Architecture (Clean Architecture, multi-module Gradle)
Maintain the existing module structure declared in `settings.gradle.kts`:
- `task-manager-domain` — entities, value objects, repository interfaces. No Spring/framework deps.
- `task-manager-application` — use cases orchestrating the domain. Depends on `task-manager-domain` only.
- `task-manager-infrastructure` — adapters (e.g., `InMemoryTaskRepository`) implementing domain repository interfaces.

Root `build.gradle.kts` centrally configures the Kotlin JVM toolchain (21), JUnit 5 + MockK test dependencies, and `useJUnitPlatform()` for all subprojects — do not duplicate that per-module. Package root is `com.example.taskmanager.<layer>.<feature>` (e.g., `com.example.taskmanager.domain.tasks`).

---

## 3) Kotlin Coding Style
- `PascalCase` for classes/objects, `camelCase` for functions/properties/variables, `UPPER_SNAKE_CASE` for top-level/companion constants.
- Model aggregates as `data class` with a **private constructor** (`@ConsistentCopyVisibility data class Task private constructor(...)`) plus a `companion object` with a `create(...)` factory and, where useful, a `reconstitute(...)` factory for rehydration.
- Use `copy()` inside business methods to produce new instances instead of mutating properties (aggregates are immutable `val`s).
- Prefer `require(...)` for invariant validation inside factory/companion functions; throw `IllegalStateException` for illegal state transitions (see `updateStatus`).
- Use nullable types (`String?`) instead of sentinel values; use `Instant` for timestamps.
- Favor expression-bodied functions (`= ...`) for simple derivations (e.g., `normalizeDescription`).

---

## 4) DDD Modeling Rules
- Aggregates (`Task`) expose only factory functions (`create`, `reconstitute`) and named business methods (`updateStatus`, `updateDetails`) — no public setters, matching the immutable `data class` + `copy()` pattern already in `Task.kt`.
- Value objects (`TaskId`, `TaskStatus`) are small, immutable types; prefer Kotlin enums or value classes over primitives.
- Repositories are interfaces in `task-manager-domain` with **business-intent method names**, implemented by adapters in `task-manager-infrastructure`.

---

## 5) Testing Rules
- **Test framework**: JUnit 5 (Jupiter), run via Gradle's `useJUnitPlatform()`.
- **Mocks**: `MockK` (not Mockito/MockitoKotlin).
- Use `@DisplayName` on the test class and each test method with a plain-English description (see `TaskTest.kt`), and `@Test` for each case.
- Name test classes `<ClassName>Test` in the mirrored package under `src/test/kotlin/...`.
- Use `assertThrows<...>` / `Assertions.assertThrows` for invariant/error-path tests, and assert on the exception message where the production code encodes intent in it (e.g., `.contains("reopened")`).

---

## 6) Conventional Commits
- Use `<type>([optional scope]): <description>` with 72-char subject limit.
- Types: `feat|fix|docs|style|refactor|perf|test|build|ci|chore|revert`.

---

## 7) Documentation Organization
- All documentation lives in `docs/` at the repository root (e.g., `docs/labs/lab-01-tdd-with-copilot-kotlin.md`).
- Keep any `src-kotlin/README.md` focused on setup/run instructions (`./gradlew build`, `./gradlew test`).

---

## 8) Guardrails (Workshop)
- Do **not** invent external dependencies (Spring, Ktor, Ktorm) without being asked — this track is plain Kotlin/JVM with Gradle.
- Keep domain logic **out of** `task-manager-infrastructure`.
- If a rule conflicts, **Clean Architecture boundaries win** (then DDD, then Kotlin style conventions).
