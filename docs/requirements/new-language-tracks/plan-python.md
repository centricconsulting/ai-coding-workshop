# Plan: Python Track

## Summary

Full, self-contained port of the TaskManager reference app to Python, following the same Clean
Architecture layering already used in the .NET and Spring Boot tracks: `Domain`, `Application`,
`Api`, `Infrastructure`.

## Stack

- Python 3.12+
- **Web framework**: FastAPI (Minimal-API-equivalent ergonomics, async support, closest match to
  the .NET Minimal API style already used in this workshop)
- **Testing**: pytest, `unittest.mock` for test doubles
- **Persistence**: in-memory repository implementation for labs (mirrors the existing
  `ITaskRepository` in-memory pattern), matching `TaskManager.Domain/Repositories/ITaskRepository.cs`

## Proposed Structure

```
src-python/
  task_manager_domain/       # Task entity, TaskId, TaskStatus, TaskRepository protocol
  task_manager_application/  # use cases / services orchestrating domain rules
  task_manager_api/          # FastAPI routes, request/response models
  task_manager_infrastructure/# in-memory repository implementation
tests/
  task_manager_unit_tests/
  task_manager_integration_tests/
```

## Labs to Port

- `lab-01-tdd-with-copilot.md` — language-agnostic already; add Python-specific command snippets
  (pytest invocation) where the doc references dotnet/mvn commands.
- `lab-02-requirements-to-code-python.md` — new variant.
- `lab-03-generation-and-refactoring-python.md` — new variant. **Refactor exercise note**: avoid a
  literal translation of the .NET/Java smell. Python's idiomatic refactor target should be a
  function doing too much validation/branching inline (a "fat function" smell) refactored into
  small, single-purpose functions/guard clauses — a more natural Python code smell than the
  class-based smells used in the OO tracks.
- `lab-04-testing-documentation-workflow-python.md` — new variant, using pytest fixtures and
  docstring-driven documentation (Sphinx-style or simple README) instead of XML doc comments /
  Javadoc.

## Devcontainer / Setup

- New `.devcontainer/python-participant/` (Python 3.12, Pylance, pytest extension) — same pattern
  as `dotnet-participant` and `springboot-participant`.
- Minimal setup doc: `docs/MINIMAL_SETUP_PYTHON.md`.

## Estimate

**1–2 days**, AI-assisted port of Domain/Application layers plus lab doc translation, consistent
with the original Java/Spring Boot port timeline.

## Open Questions

- None currently — lowest-risk track, most natural fit for a backend port.
