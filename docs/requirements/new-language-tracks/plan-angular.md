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
- **Testing**: Jasmine + Karma (Angular CLI default)

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

- `lab-01-tdd-with-copilot.md` — Angular-specific test commands (`ng test`).
- `lab-02-requirements-to-code-angular.md` — new variant, building the domain/application layer
  and a component to exercise it.
- `lab-03-generation-and-refactoring-angular.md` — new variant. **Refactor exercise note**: avoid
  a literal translation of the .NET/Java smell. A natural Angular/TS smell: business logic
  inlined directly inside a component (violating separation of concerns) refactored out into an
  injectable application service — demonstrates the same Clean Architecture principle the other
  tracks reinforce, in an idiom Angular devs will recognize.
- `lab-04-testing-documentation-workflow-angular.md` — new variant, using Jasmine spec docs and
  TSDoc comments.

## Devcontainer / Setup

- New `.devcontainer/angular-participant/` (Node LTS, Angular CLI, Angular Language Service
  extension).
- Minimal setup doc: `docs/MINIMAL_SETUP_ANGULAR.md`.

## Estimate

**1–2 days**, AI-assisted port of Domain/Application layers into TypeScript plus lab doc
translation.

## Open Questions

- Confirm Angular version/style guide (standalone components vs. NgModules) — recommend
  standalone components since that's the modern default and reduces boilerplate for a workshop
  setting.
