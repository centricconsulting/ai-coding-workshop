# Lab 4: Testing, Documentation & Workflow with GitHub Copilot (Python)

> **💡 Also available**: [shared .NET version](lab-04-testing-documentation-workflow.md) · [JavaScript version](lab-04-testing-documentation-workflow-javascript.md) · [Java/Spring Boot version](lab-04-testing-documentation-workflow-java.md) · [Angular version](lab-04-testing-documentation-workflow-angular.md)

**Duration**: 15-20 minutes  
**Learning Objectives**:

- Expand FastAPI endpoint coverage with pytest fixtures and `TestClient`
- Use Copilot to add clear Python docstrings without over-documenting
- Keep documentation aligned with the actual `src-python/` files and commands
- Write Conventional Commit drafts and a small review checklist for the Python track
- Treat testing, documentation, and workflow as part of the feature, not cleanup work

---

## Overview

This lab focuses on the finishing work that often gets skipped when people are in a hurry:

1. **Testing** - strengthen confidence in the POST and GET endpoints
2. **Documentation** - add useful Python docstrings and concise usage notes
3. **Workflow** - stage the right files and prepare clear commit or PR text

For the Python track, the workflow stays close to the code participants already built:

- FastAPI code lives under `src-python/task_manager_api/`
- Application logic lives under `src-python/task_manager_application/`
- starter persistence lives under `src-python/task_manager_infrastructure/`
- tests live under `src-python/tests/`
- verification runs with `pytest`

---

## Prerequisites

- ✅ Labs 1-3 are complete, or you have equivalent Python-track code
- ✅ POST `/tasks` and GET `/tasks` both work in the FastAPI app
- ✅ `pytest` already runs successfully from `src-python/`
- ✅ You are comfortable asking Copilot for tests, docs, and workflow text

Baseline verification:

```bash
cd src-python
pytest
```

---

## Part 1: Expand Integration Coverage with Fixtures (6-7 minutes)

### Scenario: Test the API Like a Small Real System

By now, the Python track should have at least two endpoints:

- `POST /tasks`
- `GET /tasks`

This is a good moment to stop testing one request at a time and introduce reusable pytest fixtures.

### 1.1 Ask Copilot for Fixture-Based Test Setup

Prompt Copilot:

```text
Add pytest fixture-based integration tests for the Python FastAPI track.
Requirements:
- files live under src-python/tests/task_manager_integration_tests/
- create a fixture that provides a fresh InMemoryTaskRepository
- create a fixture that provides a FastAPI TestClient wired to that repository
- test POST /tasks and GET /tasks
- keep the setup simple and workshop-friendly
```

### 1.2 Expected Output

A representative test file might look like this:

```python
import pytest
from fastapi.testclient import TestClient

from task_manager_api.app import create_app
from task_manager_infrastructure import InMemoryTaskRepository


@pytest.fixture
def repository() -> InMemoryTaskRepository:
    return InMemoryTaskRepository()


@pytest.fixture
def client(repository: InMemoryTaskRepository) -> TestClient:
    app = create_app(repository)
    return TestClient(app)


def test_create_task_returns_201(client: TestClient) -> None:
    response = client.post(
        "/tasks",
        json={
            "title": "Draft lab notes",
            "description": "Prepare Python examples",
            "priority": "HIGH",
        },
    )

    assert response.status_code == 201


def test_get_tasks_returns_created_task(client: TestClient) -> None:
    client.post(
        "/tasks",
        json={
            "title": "Draft lab notes",
            "description": "Prepare Python examples",
            "priority": "HIGH",
        },
    )

    response = client.get("/tasks")

    assert response.status_code == 200
    payload = response.json()
    assert len(payload) == 1
    assert payload[0]["title"] == "Draft lab notes"
```

### 1.3 Good Test Ideas for This Track

Strong additions usually cover things like:

- a valid POST returns `201 Created`
- invalid priority input returns a `4xx` response
- GET returns only active tasks
- GET returns tasks in the expected order
- each test gets a fresh in-memory repository via a fixture

### 1.4 Run the Tests

```bash
cd src-python
pytest
```

If the tests pass, the API now has a stronger safety net.

---

## Part 2: Add Useful Python Docstrings (4-5 minutes)

### Scenario: Document the Why, Not the Obvious

In the .NET and Java tracks, this lab leans on XML doc comments or Javadoc. For Python, a better fit is short, accurate docstrings.

Good places to document include:

- the `CreateTaskHandler` or `list_tasks(...)` application workflow
- any mapping helper that turns Domain objects into response models
- the FastAPI app factory if you introduced one
- refactored helpers that encapsulate non-obvious legacy behavior

### 2.1 Ask Copilot for Focused Docstrings

Try this prompt:

```text
Review the Python track under src-python/ and suggest Google-style or reST-style docstrings for the public application functions, handlers, and API helpers.
Only add docstrings where they explain intent, parameters, or return values.
Do not add noisy comments that just repeat the code.
```

### 2.2 Expected Output

A useful result might look like:

```python
class CreateTaskHandler:
    """Create and persist a new task using the configured repository."""

    async def handle(self, command: CreateTaskCommand) -> Task:
        """Build a task from validated application input and save it."""
```

Or for an application function:

```python
async def list_tasks(repository: TaskRepository) -> list[Task]:
    """Return active tasks ordered from newest to oldest."""
```

### 2.3 Keep Docstrings Short and Honest

Useful docstrings explain:

- what the function is for
- what the main inputs mean
- what gets returned
- any business intent that is not obvious from the name alone

Avoid docstrings that just restate the function name.

---

## Part 3: Review the Track README or Module Docs Carefully (2-3 minutes)

For this track, documentation can also mean checking module-level docstrings or a small track README if one exists.

Try a prompt like:

```text
Review the Python track documentation and module docstrings under src-python/.
Point out any mismatch between the documented commands, file paths, and the actual code layout.
Keep suggestions concise.
```

What you want to catch:

- paths that forgot the `src-python/` prefix
- commands that mention tools not used in this track
- docstrings that refer to Java or .NET concepts too literally

---

## Part 4: Stage Files and Draft Workflow Text (3-4 minutes)

### 4.1 Stage the Relevant Files

A typical Python-track change set might include:

```bash
git add src-python/task_manager_application/
git add src-python/task_manager_api/
git add src-python/task_manager_infrastructure/
git add src-python/tests/task_manager_integration_tests/
git add src-python/tests/task_manager_unit_tests/
```

### 4.2 Ask Copilot for a Commit Message

Try:

```text
Write a Conventional Commit message for the Python workshop track.
The change expanded FastAPI integration tests, added Python docstrings, and cleaned up workflow documentation.
Include a short subject and a helpful body.
```

A good result might look like:

```text
test(python): expand FastAPI coverage and docs

- add fixture-based pytest coverage for POST and GET task endpoints
- document application handlers and API helpers with concise docstrings
- align workflow guidance with the Python track file layout
```

### 4.3 Ask for a PR Description Draft

Prompt Copilot Chat:

```text
@workspace Draft a pull request description for the Python workshop track.
Include:
- summary of testing improvements
- documentation updates
- validation performed with pytest
- a short reviewer checklist
Use Markdown.
```

A representative result might look like:

````md
## Summary

This change strengthens the Python workshop track by expanding FastAPI endpoint coverage,
adding concise docstrings to the application and API layers, and aligning workflow guidance
with the `src-python/` structure.

## Testing Performed

```bash
cd src-python
pytest
```

## Reviewer Checklist

- [ ] tests cover both POST and GET task flows
- [ ] fixtures provide isolated repository state per test
- [ ] docstrings explain intent rather than repeating code
- [ ] documented file paths start with `src-python/`
````

Review the output before using it. Copilot is a drafting partner, not the final approver.

---

## Verify Your Work

Run the Python track tests from `src-python/`:

```bash
cd src-python
pytest
```

**Expected result**: ✅ tests pass.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ endpoint tests use pytest fixtures instead of copy-pasted setup
- ✅ POST and GET routes both have realistic integration coverage
- ✅ public Python functions or handlers have concise, useful docstrings
- ✅ documented commands and paths match the real `src-python/` layout
- ✅ you can generate a Conventional Commit and PR draft for the Python track
- ✅ `pytest` still passes

---

## Troubleshooting

### The Fixtures Share State Between Tests

**Problem**: one test leaves data behind for the next test  
**Solution**: make sure each fixture returns a new `InMemoryTaskRepository` instance and that the `TestClient` is created from that fresh instance.

### Copilot Added Too Many Docstrings

**Problem**: every private helper now has a long block of text  
**Solution**: keep docstrings for public or non-obvious code paths. Remove anything that only repeats the implementation.

### The Suggested Tests Need Extra Plugins

**Problem**: Copilot introduced `pytest-asyncio` or other packages  
**Solution**: stay within the workshop dependencies. Use FastAPI `TestClient` and regular pytest fixtures unless you truly need more.

### The Workflow Text Mentions the Wrong Paths

**Problem**: the generated text says `src/` or references another track  
**Solution**: correct every command to use `src-python/` and compare the draft against the actual repository structure.

---

## Workshop Wrap-Up

After Lab 4, the Python track should leave you with the same overall workflow as the other tracks:

- write tests first
- translate requirements into small code changes
- refactor for readability
- document the important intent
- prepare clear commit and review text

That is the workshop journey, translated into Python and FastAPI.

---

## Additional Resources

- [Conventional Commits](https://www.conventionalcommits.org/)
- [FastAPI Testing](https://fastapi.tiangolo.com/tutorial/testing/)
- [pytest Fixtures](https://docs.pytest.org/en/stable/how-to/fixtures.html)
- [Python Docstring Conventions](https://peps.python.org/pep-0257/)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
