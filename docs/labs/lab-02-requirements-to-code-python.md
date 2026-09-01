# Lab 2: From Requirements to Code with GitHub Copilot (Python)

> **💡 Also available**: [shared .NET version](lab-02-requirements-to-code.md) · [JavaScript version](lab-02-requirements-to-code-javascript.md) · [Java/Spring Boot version](lab-02-requirements-to-code-java.md) · [Angular version](lab-02-requirements-to-code-angular.md)

**Duration**: 40-45 minutes  
**Learning Objectives**:

- Turn a plain-language request into a small set of Python/FastAPI code changes
- Extend the Domain layer with a `Priority` enum while keeping Clean Architecture boundaries intact
- Add an Application-layer create-task workflow without introducing unnecessary CQRS ceremony
- Generate pytest unit tests and a FastAPI integration test with Copilot
- Practice reviewing Copilot output for path accuracy, async behavior, and scope

---

## Overview

In Lab 1 you used TDD to add a small service in the Application layer. In this lab, you will take a broader feature from a user story and turn it into working code.

For the Python track, the design stays close to the .NET reference while still feeling Pythonic:

- `src-python/task_manager_domain/` holds the `Task` aggregate and value types
- `src-python/task_manager_application/` holds small orchestration functions or handlers
- `src-python/task_manager_api/` holds FastAPI routes and Pydantic models
- `src-python/task_manager_infrastructure/` holds the in-memory repository adapter
- tests stay under `src-python/tests/`

The goal is **not** to recreate every .NET pattern literally. The goal is to preserve the same architectural intent with Python-friendly tools.

---

## Prerequisites

- ✅ Lab 1 is complete, or you have an equivalent Python `NotificationService` implementation
- ✅ Starter Domain and Infrastructure code exists under `src-python/`
- ✅ VS Code is open with GitHub Copilot enabled
- ✅ Python dependencies installed from `src-python/requirements.txt`
- ✅ Baseline verification completed:

```bash
cd src-python
pytest
```

---

## Part 1: Turn the Request into Clear Rules (8-10 minutes)

### Scenario: Add Priority to a Task

A stakeholder says:

> **User Story**: As a workshop participant, I want each task to have a priority so I can quickly see what matters most.

That sentence is still too vague to implement directly.

### 1.1 Ask Copilot to Break the Story Down

Try this prompt in Copilot Chat:

```text
I have a Python/FastAPI task manager in src-python/ with Clean Architecture folders:
- task_manager_domain
- task_manager_application
- task_manager_api
- task_manager_infrastructure

Turn this user story into 4-6 small backlog items:
"As a workshop participant, I want each task to have a priority so I can quickly see what matters most."
Keep the answer beginner-friendly and align it to the existing layers.
```

A helpful answer should suggest work like:

- define valid priority values
- add priority to the Domain model
- update task creation logic to require priority
- expose priority in the API request and response models
- add tests for valid and invalid inputs

### 1.2 Choose a Small, Concrete Ruleset

For this workshop, use these rules:

- valid priorities are `LOW`, `MEDIUM`, and `HIGH`
- new tasks must include a priority
- the Domain `Task` should store the priority
- the Application layer should orchestrate creation and saving
- the FastAPI endpoint should return the created task

---

## Part 2: Add the Domain Concept First (RED → GREEN) (10-12 minutes)

### 2.1 Ask Copilot for a Priority Enum

Prompt Copilot:

```text
Create a Priority enum for the Python track in src-python/task_manager_domain/.
Requirements:
- use enum.Enum
- values: LOW, MEDIUM, HIGH
- keep naming explicit and workshop-friendly
- update the Task aggregate so priority becomes part of task creation and storage
- keep the Domain layer free of FastAPI or Pydantic dependencies
```

### 2.2 Expected Output

Copilot should guide you toward something like:

```python
from enum import Enum


class Priority(str, Enum):
    LOW = "LOW"
    MEDIUM = "MEDIUM"
    HIGH = "HIGH"
```

And the `Task` aggregate should grow to include `priority`:

```python
@dataclass(slots=True)
class Task:
    id: TaskId
    title: str
    description: str
    priority: Priority
    status: TaskStatus
    created_at: datetime
    updated_at: datetime

    @classmethod
    def create(cls, title: str, description: str, priority: Priority) -> "Task":
        now = datetime.now(timezone.utc)
        return cls(
            id=TaskId.new(),
            title=title,
            description=description,
            priority=priority,
            status=TaskStatus.TODO,
            created_at=now,
            updated_at=now,
        )
```

### 2.3 Write the Domain Test First

Before changing the implementation, ask Copilot for a small failing test:

```text
Update src-python/tests/task_manager_unit_tests/ with pytest coverage for adding Priority to Task.
Create tests that verify:
- Task.create stores the provided priority
- only valid Priority enum values are used by the factory
Keep the tests lightweight and consistent with the existing Python track style.
```

A representative test might look like:

```python
from task_manager_domain import Priority, Task


def test_task_create_stores_priority() -> None:
    task = Task.create("Prepare workshop", "Add the Python lab track", Priority.HIGH)

    assert task.priority is Priority.HIGH
```

### 2.4 Why Start in the Domain Layer?

This keeps the dependency direction clean:

- Domain owns the concept of priority
- Application orchestrates use cases with that concept
- API translates HTTP payloads into Domain-friendly values

That matches the existing workshop architecture instead of letting the endpoint define the business rule.

---

## Part 3: Create an Application-Layer Workflow (10-12 minutes)

### Scenario: Add a Pythonic Create Task Use Case

The .NET track uses a command plus handler. Python can mirror that intent with less ceremony.

A good workshop compromise is:

- a small `CreateTaskCommand` dataclass for structured input
- a `CreateTaskHandler` class or `create_task(...)` async function for orchestration

Either approach is acceptable. For parity with the .NET lab, this document uses a small handler class.

### 3.1 Ask Copilot for a Handler and Unit Tests

Try this prompt:

```text
Create a Python application-layer create-task workflow under src-python/task_manager_application/.
Requirements:
- add a CreateTaskCommand dataclass with title, description, and priority
- add a CreateTaskHandler that depends on TaskRepository
- use async def and await repository methods
- create the Task aggregate and save it with add_task and save_changes
- add pytest unit tests using unittest.mock.AsyncMock
- keep the solution Pythonic and lightweight rather than full CQRS infrastructure
```

### 3.2 Expected Output

Copilot should generate code along these lines.

**`src-python/task_manager_application/create_task.py`**

```python
from dataclasses import dataclass

from task_manager_domain import Priority, Task, TaskRepository


@dataclass(slots=True)
class CreateTaskCommand:
    title: str
    description: str
    priority: Priority


class CreateTaskHandler:
    def __init__(self, repository: TaskRepository) -> None:
        self._repository = repository

    async def handle(self, command: CreateTaskCommand) -> Task:
        task = Task.create(
            title=command.title,
            description=command.description,
            priority=command.priority,
        )
        await self._repository.add_task(task)
        await self._repository.save_changes(task)
        return task
```

### 3.3 Add Handler Tests First

Prompt Copilot:

```text
Create pytest unit tests for CreateTaskHandler in src-python/tests/task_manager_unit_tests/.
Use unittest.mock.AsyncMock for the TaskRepository.
Verify:
- a task is created and returned
- add_task is awaited once
- save_changes is awaited once
- the created task contains the requested priority
Avoid extra dependencies beyond pytest and the standard library.
```

A representative test might look like:

```python
import asyncio
from unittest.mock import AsyncMock

from task_manager_application.create_task import CreateTaskCommand, CreateTaskHandler
from task_manager_domain import Priority


def test_create_task_handler_saves_new_task() -> None:
    repository = AsyncMock()
    handler = CreateTaskHandler(repository)
    command = CreateTaskCommand(
        title="Prepare demo",
        description="Walk through FastAPI route generation",
        priority=Priority.MEDIUM,
    )

    task = asyncio.run(handler.handle(command))

    assert task.priority is Priority.MEDIUM
    repository.add_task.assert_awaited_once()
    repository.save_changes.assert_awaited_once_with(task)
```

### 3.4 Why Not Build a Bigger CQRS Framework?

Because this workshop is teaching:

- requirement translation
- layer boundaries
- async repository usage
- test-first thinking

A single dataclass plus handler gives you those lessons without burying beginners in framework scaffolding.

---

## Part 4: Add FastAPI Models and a POST Endpoint (10-12 minutes)

### 4.1 Ask Copilot for the API Layer

Use a focused prompt:

```text
Add a FastAPI POST /tasks endpoint for the Python track.
Requirements:
- files should live under src-python/task_manager_api/
- create CreateTaskRequest and TaskResponse Pydantic models
- map the request to CreateTaskCommand
- call CreateTaskHandler
- return a TaskResponse
- keep HTTP concerns in the API layer and business rules in Domain/Application
- use src-python/task_manager_infrastructure/InMemoryTaskRepository for a simple workshop setup
```

### 4.2 Expected Output

Copilot should generate code similar to this.

**`src-python/task_manager_api/models.py`**

```python
from datetime import datetime

from pydantic import BaseModel


class CreateTaskRequest(BaseModel):
    title: str
    description: str
    priority: str


class TaskResponse(BaseModel):
    id: str
    title: str
    description: str
    priority: str
    status: str
    created_at: datetime
    updated_at: datetime
```

**`src-python/task_manager_api/app.py`**

```python
from fastapi import FastAPI

from task_manager_api.models import CreateTaskRequest, TaskResponse
from task_manager_application.create_task import CreateTaskCommand, CreateTaskHandler
from task_manager_domain import Priority
from task_manager_infrastructure import InMemoryTaskRepository

app = FastAPI()
repository = InMemoryTaskRepository()
handler = CreateTaskHandler(repository)


@app.post("/tasks", response_model=TaskResponse, status_code=201)
async def create_task(request: CreateTaskRequest) -> TaskResponse:
    task = await handler.handle(
        CreateTaskCommand(
            title=request.title,
            description=request.description,
            priority=Priority(request.priority),
        )
    )

    return TaskResponse(
        id=str(task.id),
        title=task.title,
        description=task.description,
        priority=task.priority.value,
        status=task.status.value,
        created_at=task.created_at,
        updated_at=task.updated_at,
    )
```

### 4.3 Review the Boundary Decisions

Check that the generated code keeps responsibilities separate:

- `CreateTaskRequest` is an API concern
- `CreateTaskCommand` is an Application concern
- `Priority` and `Task` are Domain concerns
- `InMemoryTaskRepository` is an Infrastructure concern

That separation is the point of the exercise.

---

## Part 5: Add an Endpoint Integration Test (8-10 minutes)

### 5.1 Ask Copilot for a FastAPI TestClient Test

Prompt Copilot:

```text
Create a pytest integration test for the Python POST /tasks endpoint.
Requirements:
- place it under src-python/tests/task_manager_integration_tests/
- use FastAPI TestClient
- verify a valid request returns 201 Created
- verify the JSON response contains title, description, priority, and status
- keep the test realistic for the in-memory repository setup
```

### 5.2 Expected Output

A representative test looks like this:

```python
from fastapi.testclient import TestClient

from task_manager_api.app import app


client = TestClient(app)


def test_create_task_returns_created_task() -> None:
    response = client.post(
        "/tasks",
        json={
            "title": "Ship lab draft",
            "description": "Prepare the Python workshop handout",
            "priority": "HIGH",
        },
    )

    assert response.status_code == 201

    payload = response.json()
    assert payload["title"] == "Ship lab draft"
    assert payload["priority"] == "HIGH"
    assert payload["status"] == "TODO"
```

### 5.3 Optional Refinement Prompt

After the happy path works, try:

```text
/check Review the Python POST /tasks endpoint and tests.
Suggest small improvements to validation, naming, or response mapping without changing the architecture.
```

Good suggestions might include:

- converting the request priority to a `Priority` enum earlier
- tightening error messages for invalid enum values
- moving response mapping into a helper if it starts repeating

---

## Verify Your Work

Run the Python track tests from `src-python/`:

```bash
cd src-python
pytest
```

**Expected result**: ✅ tests pass.

If you implemented both unit and integration tests, you should see the new Python test files collected alongside any earlier lab tests.

---

## Prepare Your Git Changes

When you are ready to review your changes, stage the files touched in this lab.

A typical Python-track change set might include:

```bash
git add src-python/task_manager_domain/
git add src-python/task_manager_application/
git add src-python/task_manager_api/
git add src-python/tests/task_manager_unit_tests/
git add src-python/tests/task_manager_integration_tests/
```

Before committing, check:

- ✅ paths use `src-python/` consistently
- ✅ Domain code has no FastAPI imports
- ✅ Application code depends on Domain abstractions, not HTTP models
- ✅ API code maps requests and responses cleanly
- ✅ `pytest` passes from `src-python/`

---

## Key Learning Points

### ✅ What This Lab Teaches

1. plain-language requirements still need concrete rules
2. the Domain layer should own business concepts like priority
3. Python can mirror Clean Architecture without copying .NET ceremony line-for-line
4. async repository workflows are easy to test with `AsyncMock`
5. FastAPI endpoints stay cleaner when mapping is separated from business logic

### ✅ What to Watch for in Copilot Output

- correct `src-python/` paths
- `async` repository calls being awaited
- type hints on public functions and classes
- clear separation between Pydantic models and Domain types
- small, focused handlers instead of overly clever abstractions

---

## Troubleshooting

### Copilot Put `Priority` in the API Layer

**Problem**: the enum was generated next to FastAPI request models  
**Solution**: move it back to `src-python/task_manager_domain/` and remind Copilot that priority is a business concept, not an HTTP concern.

### The Handler Uses Raw Strings Instead of a Domain Enum

**Problem**: `CreateTaskHandler` accepts `"HIGH"` instead of `Priority.HIGH`  
**Solution**: refine the prompt with: _"Use the Domain `Priority` enum in Application code. Only the API layer should translate strings from HTTP."_

### The Tests Need Extra Async Plugins

**Problem**: Copilot suggested `pytest-asyncio` or `pytest-anyio`  
**Solution**: keep the workshop lightweight. Use `asyncio.run(...)` in unit tests instead of adding more packages.

### The Endpoint Started Containing Business Logic

**Problem**: validation and task creation are happening directly in the route  
**Solution**: move orchestration back into `CreateTaskHandler` so the FastAPI function only translates and delegates.

---

## Next Steps

Continue to [**Lab 3: Code Generation & Refactoring (Python)**](lab-03-generation-and-refactoring-python.md), where you'll add a GET endpoint and refactor the intentionally messy legacy processing function into smaller helpers.

---

## Additional Resources

- [FastAPI Tutorial](https://fastapi.tiangolo.com/tutorial/)
- [pytest Documentation](https://docs.pytest.org/)
- [Python `unittest.mock` Documentation](https://docs.python.org/3/library/unittest.mock.html)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [JavaScript version of Lab 2](lab-02-requirements-to-code-javascript.md)
