# Plan: Angular Track

## Summary

Self-contained Angular SPA — **no backend dependency of any kind** (not on the .NET/Spring Boot
API, and no new server component either). Built entirely by the workshop's Angular developers, who
are frontend-focused and should not need backend skills to complete the labs.

## Stack

- Angular (latest LTS) with standalone components
- **State/data layer**: in-memory service (e.g., an injectable `TaskStore`/`TaskRepository` backed
  by a `Map` or array, mirroring the same "in-memory repository" concept used in the other tracks'
  `Infrastructure` layer) — no HTTP calls, no server
- **Testing**: Vitest (Angular CLI 22 default; Karma/Jasmine are deprecated by the Angular team)

## Proposed Structure

```
src-angular/
  task-manager/
    src/app/
      domain/           # Task model, TaskId, TaskStatus, TaskRepository interface (TS)
      application/       # services orchestrating domain rules (create/update/complete task)
      data/              # in-memory repository implementation
      features/tasks/    # components/pages consuming application services
```

Layering intentionally mirrors Domain/Application/Infrastructure even though it's client-side only,
so the TDD labs still exercise real business-rule tests independent of UI rendering.

## Labs to Port

- `lab-01-tdd-with-copilot.md` — Angular-specific test commands (`ng test`, running on Vitest).
- `lab-02-requirements-to-code-angular.md` — new variant, building the domain/application layer
  and a component to exercise it.
- `lab-03-generation-and-refactoring-angular.md` — new variant. **Refactor exercise note**: avoid
  a literal translation of the .NET/Java smell. A natural Angular/TS smell: business logic
  inlined directly inside a component (violating separation of concerns) refactored out into an
  injectable application service — demonstrates the same Clean Architecture principle the other
  tracks reinforce, in an idiom Angular devs will recognize.
- `lab-04-testing-documentation-workflow-angular.md` — new variant, using Vitest spec docs and
  TSDoc comments.

## Devcontainer / Setup

- New `.devcontainer/angular-participant/` (Node 24+, Angular CLI via `npx`, Angular Language
  Service extension) — **verified**: `npm install && npx ng build && npx ng test --watch=false`
  all pass inside the actual `mcr.microsoft.com/devcontainers/typescript-node:24` image. Note:
  Angular CLI 22 requires Node 24.15+/22.22.3+/26.0+; the widely-used `:1-22-bookworm` devcontainer
  tag ships Node 22.16 and fails this requirement — use the `:24` tag.
- Full local setup: Angular section of `docs/LOCAL_SETUP.md`. **Policy**: unlike the .NET/Java tracks,
  there is no lightweight "Copilot-only" `MINIMAL_SETUP_*.md` alternative for this track —
  participants must either use the devcontainer or match the local setup doc's spec exactly
  (particularly the Node version requirement).

## Estimate

**1–2 days**, AI-assisted port of Domain/Application layers into TypeScript plus lab doc
translation.

## Open Questions

- Confirm Angular version/style guide (standalone components vs. NgModules) — recommend
  standalone components since that's the modern default and reduces boilerplate for a workshop
  setting.
