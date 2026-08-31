# Plan: Kotlin Track (Mobile — Android)

## Summary

Self-contained **plain Kotlin module** (no Android framework dependency, no emulator required)
covering Domain + Application layers, for use by mobile (Android) developers in the workshop.
A runnable Android app shell (installable on an emulator) is a **deferred** follow-on — Labs 1–4
ship as a logic-only module with tests first.

## Stack

- Kotlin (JVM target, no Android SDK dependency for Labs 1–4)
- **Testing**: JUnit5, MockK for mocking
- Build tool: Gradle (Kotlin DSL), matching common Android tooling conventions so this can later
  be dropped into an Android app module without restructuring

## Proposed Structure

```
src-kotlin/
  task-manager-domain/       # Task, TaskId, TaskStatus, TaskRepository interface
  task-manager-application/  # use cases orchestrating domain rules
  task-manager-infrastructure/ # in-memory repository implementation
  # (future) task-manager-android/  — thin UI shell, deferred
```

## Labs to Port

- `lab-01-tdd-with-copilot.md` — Kotlin/Gradle test commands (`./gradlew test`).
- `lab-02-requirements-to-code-kotlin.md` — new variant.
- `lab-03-generation-and-refactoring-kotlin.md` — new variant. **Refactor exercise note**: avoid a
  literal translation of the .NET/Java smell. A natural Kotlin smell: heavy use of nullable types
  and manual null-checks instead of Kotlin idioms (sealed classes / `Result`-style error handling,
  data classes with `copy()`), refactored to idiomatic null-safety patterns — highlights a
  Kotlin-specific best practice rather than reusing the OO refactor from other tracks.
- `lab-04-testing-documentation-workflow-kotlin.md` — new variant, using KDoc and JUnit5
  `@DisplayName`.

## Devcontainer / Setup

- New `.devcontainer/kotlin-participant/` (JDK 21, Kotlin plugin, Gradle) — no Android SDK/emulator
  needed for Labs 1–4, keeping the devcontainer lightweight and consistent with existing containers.
- Minimal setup doc: `docs/MINIMAL_SETUP_KOTLIN.md`.

## Estimate

**1–2 days**, AI-assisted port of Domain/Application layers plus lab doc translation.

## Open Questions

- Confirm the deferred Android UI shell should be tracked as a distinct future task once Labs 1–4
  are validated, rather than bundled into this initial estimate.
