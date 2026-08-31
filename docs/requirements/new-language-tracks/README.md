# New Language Tracks — Planning Overview

## Background

Aaron requested feedback on incorporating additional languages into the workshop, beyond the
existing bilingual .NET / Spring Boot tracks: **Angular, Swift, Kotlin, and Python**. Developers
from each of these communities are already part of the workshop audience.

A fifth track, **plain JavaScript**, was added afterward for a genuinely non-technical audience
(similar profile to Lab 0's BA/PM participants) — see [plan-javascript.md](./plan-javascript.md).
Unlike the other four, it is intentionally lightweight (no framework, no layering) rather than a
Clean Architecture port.

This folder contains one plan per track, scoped for Aaron's approval before any scaffolding work
begins.

## Scope Decisions (confirmed)

- **Labs 1–4 only** per new track (Lab 1: TDD, Lab 2: Requirements-to-Code, Lab 3: Generation &
  Refactoring, Lab 4: Testing/Documentation Workflow). Labs 5–10 are already stack-agnostic
  (Copilot interaction models, skills, agents, capstone) and require no per-language variant —
  matching the existing pattern where only Labs 2–4 have `-java.md` counterparts.
- **All four tracks are self-contained** — no track depends on another track's backend:
  - **Python**: full FastAPI backend (Clean Architecture: Domain/Application/Api/Infrastructure).
  - **Angular**: self-contained SPA with an in-memory/local domain and data layer — no server
    component. Built entirely by the Angular developers as part of the workshop.
  - **Kotlin**: plain Kotlin module (JVM-only, no Android framework dependency), covering
    Domain/Application layers. No Android emulator required for Labs 1–4.
  - **Swift**: Swift Package Manager (SPM) module (no iOS framework dependency), covering
    Domain/Application layers. No iOS simulator required for Labs 1–4. Note: Swift/Xcode tooling
    requires macOS — cannot run in a Linux devcontainer, so this track needs a local-setup guide
    instead of (or in addition to) a devcontainer.
- **Runnable UI shell for Kotlin/Swift (installable on Android/iOS emulators/simulators):
  deferred.** Labs 1–4 will ship as logic-only modules with tests first; a thin app shell can be
  added later as a follow-on enhancement once the core labs are validated.
- **Refactor exercise (Lab 3) requires care per track** — a literal line-by-line translation of
  the .NET/Java refactor exercise risks producing no genuine "code smell" to refactor in the new
  language's idioms. Each track's Lab 3 plan calls out its own smell/refactor target.
- **Porting approach**: AI-assisted translation of the existing .NET/Spring Boot Domain +
  Application layers and lab content (same method used for the original Java/Spring Boot port,
  which took ~1–2 days).
- **Setup policy**: unlike the existing .NET/Java tracks (which offer a lightweight
  `MINIMAL_SETUP_*.md` Copilot-only path alongside the full `LOCAL_SETUP.md`), none of these four
  new tracks have a minimal-setup alternative. Participants must either use the track's devcontainer
  or set up a local environment that matches the track's `LOCAL_SETUP_<TRACK>.md` spec exactly.

## Estimates Summary

| Track | Shape | Test framework | Estimate |
|---|---|---|---|
| Python | FastAPI backend | pytest | 1–2 days |
| Angular | Self-contained SPA, in-memory data layer | Vitest | 1–2 days |
| Kotlin | Plain Kotlin module (no emulator) | JUnit5 + MockK | 1–2 days |
| Swift | SPM package (no simulator) | XCTest | 1–2 days |
| JavaScript (non-technical) | Single-file, no framework/layering | `node:test` | 1–2 days |

**Total: ~1 week sequentially by one person, or 2–3 days if each language's developers port their
own track in parallel** (recommended).

## Per-Track Plans

- [Python](./plan-python.md)
- [Angular](./plan-angular.md)
- [Kotlin](./plan-kotlin.md)
- [Swift](./plan-swift.md)
- [JavaScript (non-technical audience)](./plan-javascript.md)
- [Execution Plan (day-by-day, 2–3 days per track)](./execution-plan.md)

## Open Questions For Aaron

1. Approve scope (Labs 1–4 only, no full app for mobile tracks) — yes/no?
2. Priority order to build these in?
3. Confirm Kotlin/Swift UI shell stays deferred, or should it be included from the start?
4. Any devcontainer requirement for Swift given the macOS-only Xcode toolchain, or is a local
   setup guide acceptable?
5. Confirm the JavaScript track's audience/prerequisites (see plan-javascript.md's open questions).
