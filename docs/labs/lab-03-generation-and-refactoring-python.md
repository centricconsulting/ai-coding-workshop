# Lab 3: Code Generation & Refactoring with GitHub Copilot (Python)

> **💡 Also available**: [shared .NET version](lab-03-generation-and-refactoring.md) · [JavaScript version](lab-03-generation-and-refactoring-javascript.md) · [Java/Spring Boot version](lab-03-generation-and-refactoring-java.md) · [Angular version](lab-03-generation-and-refactoring-angular.md)

**Duration**: 35-45 minutes  
**Learning Objectives**:

- Use Copilot to generate a FastAPI GET endpoint from existing architecture
- Add an Application-layer query workflow for listing active tasks
- Refactor a Python-flavored “fat function” into small, typed helpers
- Preserve behavior by leaning on pytest before and after refactoring
- Practice asking Copilot for focused changes instead of broad rewrites

---

## 📝 Plan First Before You Refactor

Before changing code across multiple files, ask Copilot for a plan first.

Example prompt:

```text
Propose a step-by-step plan for the Python track.
I need to:
- add a GET /tasks FastAPI endpoint in src-python/
- create any small application-layer function needed to list active tasks
- refactor the legacy fat function in src-python/task_manager_infrastructure/
Keep the architecture aligned with the existing Domain/Application/Api/Infrastructure folders.
```

A good plan should mention:

- what existing repository methods can be reused
- which tests should protect the current behavior first
- which helper functions could be extracted from the legacy function
- how to keep the refactor Pythonic rather than copying the .NET class-based example

---

## Overview

In this lab, you work on two different Copilot strengths:

1. **Generation** - creating the next slice of the API from existing patterns
2. **Refactoring** - cleaning up code that works but has become difficult to read

For the Python track, the refactoring target is intentionally different from the .NET and Spring Boot versions.

Instead of a large legacy class, you start with a **fat function** in `src-python/task_manager_infrastructure/legacy.py`. That is a more natural Python smell:

- deeply nested conditionals
- unclear parameter names
- string manipulation mixed with side effects
- sleep calls and file writes in the same function
- no type hints or clear boundaries

---

## Prerequisites

- ✅ Labs 1 and 2 are complete, or you have equivalent Python-track code
- ✅ `TaskRepository` and `InMemoryTaskRepository` already exist under `src-python/`
- ✅ The POST `/tasks` route is already working
- ✅ Baseline verification completed:

```bash
cd src-python
pytest
```

---

## Part 1: Generate a GET `/tasks` Endpoint (12-15 minutes)

### Scenario: Return Active Tasks

Your repository already exposes a business-intent method:

- `get_active_tasks()`

Now you want to expose that through the Application and API layers.

### 1.1 Ask Copilot to Explain the Existing Path

Start with context:

```text
@workspace Show me how the Python track currently creates tasks.
Explain which files live in task_manager_domain, task_manager_application, task_manager_api, and task_manager_infrastructure.
Then suggest the smallest clean way to add GET /tasks.
```

A helpful answer should point you toward:

- `TaskRepository.get_active_tasks()` in the Domain contract
- a small application function or handler that delegates to the repository
- a FastAPI route that maps Domain objects to response models

### 1.2 Ask Copilot for the Application-Layer Query

Prompt Copilot:

```text
Create a Python application-layer workflow for listing active tasks.
Requirements:
- use the existing TaskRepository protocol
- call get_active_tasks()
- keep it async
- return tasks ordered by created_at descending
- keep the code lightweight and Pythonic
- place the code under src-python/task_manager_application/
- add pytest unit tests if a new function or handler is introduced
```

### 1.3 Expected Output

A clean result might look like this.

**`src-python/task_manager_application/list_tasks.py`**

```python
from task_manager_domain import Task, TaskRepository


async def list_tasks(repository: TaskRepository) -> list[Task]:
    tasks = await repository.get_active_tasks()
    return sorted(tasks, key=lambda task: task.created_at, reverse=True)
```

This is intentionally smaller than the .NET `GetTasksQueryHandler`, but it serves the same purpose: one application use case for retrieving task data.

### 1.4 Ask Copilot for the FastAPI Route

Use a focused prompt:

```text
Add a Python GET /tasks endpoint under src-python/task_manager_api/.
Requirements:
- reuse the existing response model shape from the POST endpoint
- call the application-layer list_tasks function
- return active tasks only
- keep HTTP concerns in the API layer
- do not move repository logic into the route
```

### 1.5 Expected Output

A representative route might look like:

```python
from fastapi import FastAPI

from task_manager_api.models import TaskResponse
from task_manager_api.dependencies import get_task_repository
from task_manager_application.list_tasks import list_tasks


@app.get("/tasks", response_model=list[TaskResponse])
async def get_tasks() -> list[TaskResponse]:
    tasks = await list_tasks(get_task_repository())
    return [
        TaskResponse(
            id=str(task.id),
            title=task.title,
            description=task.description,
            priority=task.priority.value,
            status=task.status.value,
            created_at=task.created_at,
            updated_at=task.updated_at,
        )
        for task in tasks
    ]
```

The exact file split can vary, but the responsibilities should stay the same.

---

## Part 2: Add Tests Before the Refactor (8-10 minutes)

### Scenario: Protect the Legacy Function First

Before you clean up `src-python/task_manager_infrastructure/legacy.py`, make sure pytest captures the current behavior.

### 2.1 Ask Copilot for Test Ideas

Try this prompt:

```text
Review src-python/task_manager_infrastructure/legacy.py.
Suggest pytest cases I should add before refactoring the fat function.
Focus on current string processing behavior, branching, and return values.
Keep the tests lightweight.
```

Good test ideas usually cover:

- type `1` with `flag=True` toggles letter casing and replaces spaces
- long output is trimmed to the current maximum length
- type `1` with `flag=False` uppercases the input
- type `2` keeps the first word and lowercases later words
- unknown `type` returns the input unchanged
- `None` or empty input returns an empty string

### 2.2 Create a Baseline Test File

A representative starter test file might include:

```python
from task_manager_infrastructure.legacy import process_task


def test_process_task_type_one_with_flag_true_transforms_text() -> None:
    result = process_task(7, "Hello World", 1, True)

    assert result == "hELLO_wORLD"


def test_process_task_type_two_lowercases_words_after_first() -> None:
    result = process_task(7, "HELLO WORLD AGAIN", 2, False)

    assert result == "HELLO world again"
```

### 2.3 Run the Tests

```bash
cd src-python
pytest
```

**Expected result**: ✅ tests pass before you refactor.

That passing baseline is your safety net.

---

## Part 3: Refactor the Fat Function into Small Helpers (12-15 minutes)

### 3.1 Ask Copilot for a Focused Refactor

Use a prompt like this:

```text
/refactor Refactor src-python/task_manager_infrastructure/legacy.py.
Requirements:
- keep the external behavior of process_task the same
- convert nested if statements to guard clauses where possible
- extract small single-purpose helper functions
- add type hints
- improve parameter names if appropriate
- use the standard logging module instead of print statements where it helps
- separate formatting logic from persistence side effects
- keep this as an idiomatic Python refactor, not a class-based rewrite
```

### 3.2 What a Good Refactor Usually Produces

A clean Python result often introduces helpers such as:

- `normalize_input(...)`
- `toggle_case_and_replace_spaces(...)`
- `limit_output_length(...)`
- `persist_processed_task(...)`
- `format_type_two_text(...)`

The main function should start to read more like a short recipe:

```python
def process_task(task_id: int, raw_text: str | None, task_type: int, should_persist: bool) -> str:
    if not raw_text:
        return ""

    if task_type == 1:
        return _process_type_one(task_id, raw_text, should_persist)

    if task_type == 2:
        return _process_type_two(raw_text)

    if task_type == 3:
        return _process_type_three(task_id, raw_text)

    return raw_text
```

That is the core idea of the refactor:

- early exits instead of deep nesting
- named helpers instead of inline branches
- clearer parameter names instead of `id`, `data`, `type`, and `flag`

### 3.3 Why This Refactor Fits Python Better

It keeps the lesson aligned with Python habits:

- small functions
- readable guard clauses
- type hints
- modest modules instead of heavy abstractions

This is intentionally different from the .NET lab's legacy-class cleanup, even though the learning goal is the same.

---

## Part 4: Re-run, Review, and Explain the Improvement (8-10 minutes)

### 4.1 Run the Tests Again

```bash
cd src-python
pytest
```

**Expected result**: ✅ tests still pass.

That tells you the behavior stayed stable while the structure improved.

### 4.2 Ask Copilot for a Final Review

Try this prompt:

```text
/check Review the refactored Python legacy module in src-python/task_manager_infrastructure/legacy.py.
Explain whether each helper has one clear job and whether the guard clauses improved readability.
Suggest only small improvements.
```

Good suggestions may include:

- renaming a helper to match its exact behavior
- moving magic numbers like the output limit into a named constant
- narrowing a type hint
- reducing repeated string operations

### 4.3 Explain the Improvement in Plain Language

A good workshop explanation sounds like this:

> The code still does the same job, but now each step has a name. That makes it easier to read, test, and change without getting lost in nested branches.

---

## Verify Your Work

Run the Python track tests from `src-python/`:

```bash
cd src-python
pytest
```

**Expected result**: ✅ tests pass after both the new GET endpoint and the legacy refactor.

---

## Prepare Your Git Changes

Stage the files you touched in this lab.

A typical Python-track change set might include:

```bash
git add src-python/task_manager_application/
git add src-python/task_manager_api/
git add src-python/task_manager_infrastructure/
git add src-python/tests/task_manager_unit_tests/
git add src-python/tests/task_manager_integration_tests/
```

Before committing, check:

- ✅ the GET route uses `src-python/` paths consistently
- ✅ the Application layer owns list-task orchestration
- ✅ the route maps responses without embedding business logic
- ✅ the refactor uses small helpers and clearer names
- ✅ `pytest` still passes

---

## Key Learning Points

### ✅ What You Practiced

1. generating the next API slice from existing context
2. reusing a Domain repository method through the Application layer
3. protecting a refactor with tests before cleanup
4. converting a fat function into smaller, named helpers
5. keeping Python refactors simple and intention-revealing

### ✅ What This Lab Is Not About

This lab is **not** about adding more framework machinery than the workshop needs. In the Python track, better structure usually means:

- smaller functions
- clearer names
- guard clauses
- better boundaries between pure logic and side effects

---

## Troubleshooting

### Copilot Tried to Move Logic Straight into the FastAPI Route

**Problem**: the GET endpoint directly filtered and sorted repository data  
**Solution**: restate the boundary: _"Keep orchestration in `src-python/task_manager_application/`. The route should translate and delegate."_

### The Refactor Changed Behavior

**Problem**: tests started failing after the cleanup  
**Solution**: compare one branch at a time. Restore the original observable behavior first, then continue improving names and structure.

### Copilot Suggested a Class-Based Rewrite

**Problem**: the function became a service class with several methods  
**Solution**: narrow the request: _"Keep this as a module-level Python refactor. I want small functions, not a new class hierarchy."_

### Print Statements Stayed in the Final Version

**Problem**: the refactor still mixes return values with console output  
**Solution**: ask Copilot to replace `print(...)` with `logging` where diagnostics are useful and keep side effects isolated.

---

## Next Steps

Continue to [**Lab 4: Testing, Documentation & Workflow (Python)**](lab-04-testing-documentation-workflow-python.md), where you'll expand integration tests, add docstrings, and finish the track with the same workflow habits used in the other stacks.

---

## Additional Resources

- [FastAPI Tutorial](https://fastapi.tiangolo.com/tutorial/)
- [pytest Documentation](https://docs.pytest.org/)
- [Python Logging HOWTO](https://docs.python.org/3/howto/logging.html)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [JavaScript version of Lab 3](lab-03-generation-and-refactoring-javascript.md)
