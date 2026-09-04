---
applyTo: 'src-python/**'
---

# GitHub Copilot Instructions for Python Workshop Track

> These instructions are automatically applied to all GitHub Copilot suggestions when working with `src-python/`.

## 0) Workshop Mode
- Assume **Python 3.12+**, **FastAPI** (Api layer), **pytest** (`pyproject.toml` configures `testpaths = ["tests"]`), and standard-library `dataclasses`/`typing`/`Protocol` for the domain.
- Prefer **Clean Architecture** package layout and **DDD** patterns.
- Always generate examples and code in **English**.

---

## 1) Workflow (TDD + Build Hygiene)
- **TDD first**: when asked to implement a feature, propose/emit tests before code.
- After you output code, assume we run `pytest` and fix failures/warnings before committing.
- When referencing rule sets, state what you followed (e.g., "Used: Clean Architecture, DDD, Tests").

---

## 2) Project Architecture (Clean Architecture)
`src-python/` uses separate top-level packages instead of framework modules:
- `task_manager_domain/` — entities, value objects, repository `Protocol`. No third-party deps (mirrors `TaskManager.Domain` / `taskmanager-domain`).
- `task_manager_application/` — use cases orchestrating the domain. Depends on `task_manager_domain` only.
- `task_manager_infrastructure/` — adapters (e.g., `InMemoryTaskRepository`) implementing the domain repository `Protocol`.
- `task_manager_api/` — FastAPI routes and request/response models. Mirrors `TaskManager.Api` (.NET) and `taskmanager-api` (Spring Boot); no business logic here.
- `tests/task_manager_unit_tests/` — unit tests for domain + application. `tests/task_manager_integration_tests/` — integration tests for infrastructure + api.

Enforce dependency direction: `api` → `application` → `domain`; `infrastructure` → `domain`. Never import FastAPI or infrastructure adapters from `task_manager_domain`.

---

## 3) Python Coding Style
- Follow PEP 8; 4-space indentation; `snake_case` for functions/variables/modules, `PascalCase` for classes, `UPPER_SNAKE_CASE` for constants.
- Use `from __future__ import annotations` and modern typing (`str | None`, `list[Task]`) instead of `Optional`/`List` from `typing` where possible.
- Model entities as `@dataclass(slots=True)` (see `Task`, `TaskId`); keep them framework-free.
- Use `Protocol` (structural typing) for repository ports, not ABCs, matching `task_manager_domain/repository.py`.
- Prefer factory **classmethods** (`Task.create(...)`) over public constructors when invariants must be enforced.
- Use module-level docstrings and function/class docstrings (triple-quoted) as done throughout the domain package.
- Leave workshop `# TODO:` comments in place for validation/business-rule scaffolding meant for participants to complete during labs — don't silently implement what those TODOs describe unless explicitly asked.
- Async repository methods (`async def find_by_id`, `async def add_task`) — keep the `Protocol` and adapters async-first.

---

## 4) DDD Modeling Rules
- Aggregates (`Task`) use `@classmethod create(...)` factories and instance methods (`update_status`, `update_details`) for state transitions — never expose raw setters.
- Value objects (`TaskId`, `TaskStatus`) are immutable and small; prefer an enum (`TaskStatus`) or a thin wrapper dataclass (`TaskId`).
- Repositories are `Protocol`s in `task_manager_domain/` with **business-intent method names** (`find_by_id`, `get_active_tasks`, `add_task`, `save_changes`) — no generic CRUD verbs.

---

## 5) Testing Rules
- **Test framework**: `pytest`, configured via `[tool.pytest.ini_options]` in `pyproject.toml` (`testpaths = ["tests"]`).
- Use plain `assert` statements (pytest style), not `unittest.TestCase` assertions.
- Name test files `test_*.py`; name test functions `test_<behavior>` in plain English describing the expected outcome (e.g., `test_update_status_changes_status_and_timestamp`).
- Put domain/application tests under `tests/task_manager_unit_tests/`; infrastructure/api tests under `tests/task_manager_integration_tests/`.
- Import from the package under test the same way production code does (e.g., `from task_manager_domain import Task, TaskId, TaskStatus`).

---

## 6) Conventional Commits
- Use `<type>([optional scope]): <description>` with 72-char subject limit.
- Types: `feat|fix|docs|style|refactor|perf|test|build|ci|chore|revert`.

---

## 7) Documentation Organization
- All documentation lives in `docs/` at the repository root (e.g., `docs/labs/lab-01-tdd-with-copilot-python.md`).
- Keep any `src-python/README.md` focused on setup/run instructions (`pip install`, `pytest`).

---

## 8) Guardrails (Workshop)
- Do **not** invent external dependencies (ORMs, DI frameworks) without being asked.
- Keep domain logic **out of** `task_manager_api/` and `task_manager_infrastructure/`.
- `task_manager_infrastructure/legacy.py` is intentionally poor-quality legacy code used for the Lab 3 refactoring exercise — do not "fix" it proactively; only refactor it when a lab task explicitly asks.
- If a rule conflicts, **Clean Architecture boundaries win** (then DDD, then PEP 8 style).
