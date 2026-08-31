# Execution Plan: New Language Tracks (2–3 Days Each)

## Assumptions

- Each track is built by a developer (or pair) fluent in that language, working in parallel.
- AI-assisted (Copilot) porting of the existing .NET/Spring Boot Domain + Application layers and
  lab content, same method used for the original Java/Spring Boot port.
- Scope per the approved plans in this folder: Labs 1–4 only, self-contained (no cross-track
  backend dependencies), Kotlin/Swift as logic-only modules (UI shell deferred).
- Day estimates are working days; a "day" assumes focused solo effort, not calendar elapsed time.

## Day-by-Day Breakdown (applies per track, adjusted per stack below)

| Day | Focus | Output |
|---|---|---|
| **Day 1** | Scaffold project structure (Domain/Application/Infrastructure layers or SPA equivalent); AI-assisted port of `Task`, `TaskId`, `TaskStatus`, `TaskRepository`; write/port unit tests | Buildable project, passing test suite, supports Lab 1 (TDD with Copilot) |
| **Day 2** | Port Lab 2 (Requirements-to-Code) and Lab 3 (Generation & Refactoring) content; design and validate the track-specific refactor exercise (see per-track notes below, not a literal translation of the OO smell) | `lab-02-*.md` and `lab-03-*.md` drafts, working "before" code with a genuine smell to refactor |
| **Day 3** | Port Lab 4 (Testing/Documentation Workflow); add devcontainer or local-setup doc; full dry run of Labs 1–4 end-to-end; fix gaps found during dry run | `lab-04-*.md`, `.devcontainer/<track>-participant/` or `docs/MINIMAL_SETUP_<TRACK>.md`, validated lab set ready for review |

## Per-Track Notes

### Python (2 days realistic, 3 with buffer)
- Day 1: FastAPI + Domain/Application scaffold, pytest.
- Day 2: Requirements-to-code + refactor exercise ("fat function" → guard clauses/small functions).
- Day 3: Testing/doc workflow (pytest fixtures, docstrings), devcontainer, dry run.

### Angular (2–3 days)
- Day 1: Standalone-components scaffold, in-memory `TaskRepository`, Vitest setup.
- Day 2: Requirements-to-code + refactor exercise (business logic inline in component → extracted
  application service).
- Day 3: Testing/doc workflow (TSDoc, Vitest specs), devcontainer, dry run — likely needs the
  full 3rd day since UI components add more surface area than a pure backend/logic module.

### Kotlin (2 days realistic, 3 with buffer)
- Day 1: Plain Kotlin module scaffold (Gradle), JUnit5/MockK setup.
- Day 2: Requirements-to-code + refactor exercise (nullable/manual null-checks → sealed
  classes/idiomatic null-safety).
- Day 3: Testing/doc workflow (KDoc), devcontainer (JDK + Gradle, no Android SDK), dry run.

### Swift (3 days — least buffer, most risk)
- Day 1: SPM package scaffold, XCTest setup.
- Day 2: Requirements-to-code + refactor exercise (force-unwraps/untyped errors → `Result`/`throws`
  + safe optional binding).
- Day 3: Testing/doc workflow (DocC comments), **plus** resolving the macOS-only tooling
  constraint (local setup doc vs. devcontainer), dry run. Recommend confirming Mac availability
  for all Swift-track participants before this day.

### JavaScript — non-technical audience (1–2 days, smallest scope)
- Day 1: Single-file `task.js` scaffold (no layering), `node:test` setup, TDD exercise for
  `createTask`/`updateStatus`-equivalent functions in plain language.
- Day 2: Requirements-to-code (add a field with validation) + beginner-friendly refactor exercise
  ("does too much" function → smaller named functions), testing/doc workflow, devcontainer
  (Node LTS only), dry run — likely needs less time than the other tracks since there's no
  framework or layering to scaffold, but lab-doc language/pacing needs extra care for the
  non-technical audience.

## Timeline

- **If run in parallel** (recommended): **3 working days total elapsed**, assuming each track has
  its own developer and no shared blockers.
- **If run sequentially by one person**: **9–14 working days total** (2–3 days × 4 tracks, plus
  1–2 days for the JavaScript track).

## Dependencies / Sequencing Risks

- None across tracks (all self-contained) — they can start simultaneously.
- Swift's macOS/tooling question should be resolved before Day 3 of that track to avoid rework on
  the devcontainer/setup deliverable.
- Recommend a shared review checkpoint at the end of Day 2 across all four tracks so the Lab 3
  refactor exercises can be sanity-checked together for consistency of difficulty/pacing.

## Next Steps Pending Approval

1. Confirm the day-by-day plan and parallel timeline above.
2. Confirm developer/owner assignment per track.
3. Confirm start date.
