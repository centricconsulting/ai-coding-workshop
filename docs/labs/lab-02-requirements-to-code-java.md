# Lab 2: From Requirements to Code with GitHub Copilot (Java/Spring Boot)

> **💡 Also available**: [.NET version](lab-02-requirements-to-code.md) using C# and ASP.NET Core · [Kotlin version](lab-02-requirements-to-code-kotlin.md) · [Swift version](lab-02-requirements-to-code-swift.md)

**Duration**: 45 minutes  
**Learning Objectives**:

- Transform user stories into actionable backlog items using Copilot
- Generate acceptance criteria with AI assistance
- Create test-driven implementations from requirements
- Practice the full software development workflow with AI

---

## Overview

In this lab, you'll experience the complete journey from a vague user requirement to working, tested code. You'll use GitHub Copilot to:

1. **Decompose** a user story into specific backlog items
2. **Generate** acceptance criteria for each item
3. **Create** test cases from acceptance criteria
4. **Implement** features using TDD principles from Lab 1

This simulates real-world Agile development where requirements are refined into actionable work items.

---

## Prerequisites

- ✅ Completed Lab 1 (TDD with NotificationService)
- ✅ Understand Red-Green-Refactor cycle
- ✅ Familiar with Copilot Chat and slash commands
- ✅ Lab 1 work committed or stashed (`git status` should not show unrelated changes; Lab 1 does not commit for you, so run `git add -A && git commit -m "feat: add notification service"` or `git stash`)
- ✅ Java 21 and Maven installed (the `mvn` command, not `mvnw`; the repo has no wrapper)
- ✅ Baseline is green: `cd src-springboot && mvn clean test` (note the per-module test counts; Part 3.3 expresses expectations as deltas)
- ✅ Familiar with the four Maven modules in `src-springboot/`: `taskmanager-domain`, `taskmanager-application`, `taskmanager-infrastructure`, `taskmanager-api`

> **📁 Where things live** (all paths relative to `src-springboot/`, root package `com.example.taskmanager`):
>
> | Concern | Module | Package |
> |---|---|---|
> | `Task`, `TaskId`, `TaskStatus`, `TaskRepository` (port), new `Priority` | `taskmanager-domain` | `domain.tasks` |
> | `TaskService` | `taskmanager-application` | `application.services` |
> | `TaskEntity`, `TaskMapper`, `JpaTaskRepositoryAdapter` | `taskmanager-infrastructure` | `infrastructure.persistence.*` |
> | `TaskController`, DTOs, `GlobalExceptionHandler` | `taskmanager-api` | `api.*` |
>
> Source lives in `<module>/src/main/java/com/example/taskmanager/...` and tests in `<module>/src/test/java/com/example/taskmanager/...`.

---

## Part 1: User Story Analysis (15 minutes)

### Scenario: Task Manager Enhancement

Your product owner provides this user story:

> **User Story**: As a user, I want to manage a list of tasks with priorities and due dates so I can track my work and focus on what's most important.

This is intentionally vague - typical of early requirements. Your job is to break it down into concrete, implementable features.

### 1.1 Generate Backlog Items

Open Copilot Chat (`Ctrl+Alt+I` / `Cmd+Shift+I`) and enter:

```text
I have this user story: "As a user, I want to manage a list of tasks with priorities and due dates so I can track my work and focus on what's most important."

Generate 5 backlog items with acceptance criteria for implementing this feature in our Spring Boot Task Manager application. Consider the existing Domain/Application/Infrastructure/API architecture using Clean Architecture patterns.
```

### 1.2 Review Generated Backlog

Copilot should generate something like:

#### **Backlog Item 1: Add Priority to Task Entity**

**Description**: Extend the Task domain entity to support priority levels (Low, Medium, High, Critical).

**Acceptance Criteria**:

- Task entity has a Priority property (enum)
- Priority is required when creating a task
- Priority can be updated after creation
- Priority is persisted to the database
- Invalid priority values are rejected

**Technical Notes**:

- Update the pure-POJO `Task` aggregate in the domain module
- Add a `Priority` enum in `domain.tasks`, next to `TaskId` and `TaskStatus`
- Persist via `TaskEntity` and `TaskMapper` in the infrastructure module (the domain stays free of JPA)
- Respect DDD aggregate rules

---

#### **Backlog Item 2: Add Due Date to Task Entity**

**Description**: Extend the Task domain entity to support due dates for deadline tracking.

**Acceptance Criteria**:

- Task entity has a dueDate property (nullable LocalDateTime)
- Due date is optional when creating a task
- Due date can be set/updated/cleared after creation
- Due date must be in the future when set
- Due date is persisted to the database

**Technical Notes**:

- Update the `Task` aggregate in the Domain layer and map the column in `TaskEntity`/`TaskMapper`
- Add validation for future dates only
- Handle null case for tasks without deadlines
- Consider timezone handling with LocalDateTime

---

#### **Backlog Item 3: Create Task with Priority and Due Date**

**Description**: Update task creation logic to accept priority and optional due date.

**Acceptance Criteria**:

- POST /api/tasks endpoint accepts priority and dueDate fields
- Priority is required in the request
- DueDate is optional in the request
- Invalid priority values return 400 Bad Request
- Past due dates return 400 Bad Request with clear error message
- Created task returns 201 with full task details including priority and due date

**Technical Notes**:

- Extend the existing `CreateTaskRequest` and `TaskResponse` DTOs in the api module
- Keep business validation in the domain (`Task.create`) and let `GlobalExceptionHandler` translate failures
- Extend the existing `POST /api/tasks` endpoint (it already exists)
- Add unit tests for all validation rules

---

#### **Backlog Item 4: Filter Tasks by Priority**

**Description**: Allow users to retrieve tasks filtered by priority level.

**Acceptance Criteria**:

- GET /api/tasks?priority={level} endpoint supports filtering
- Valid priority values: LOW, MEDIUM, HIGH, CRITICAL
- Multiple priority values can be specified: ?priority=HIGH&priority=CRITICAL
- Invalid priority values return 400 Bad Request
- Returns empty array if no tasks match
- Results are ordered by due date (earliest first, nulls last)

**Technical Notes**:

- Add query parameters to controller
- Add a query method to the `TaskRepository` port and implement it in `JpaTaskRepositoryAdapter`
- Add sorting by due date
- Unit test filtering logic
- Integration test endpoint behavior

---

#### **Backlog Item 5: Get Tasks Due Soon**

**Description**: Provide an endpoint to retrieve tasks with approaching deadlines.

**Acceptance Criteria**:

- GET /api/tasks/due-soon endpoint returns tasks due in next 7 days
- Tasks without due dates are excluded
- Results ordered by due date (soonest first)
- Configurable time window (default 7 days, max 30 days)
- Returns 200 with array of tasks
- Query parameter: ?days={number} to customize window

**Technical Notes**:

- Add new endpoint in controller
- Create service method in application layer
- Calculate date range based on current time
- Add unit tests for date calculations
- Inject a `java.time.Clock` so "now" is testable
- Consider timezone implications with ZonedDateTime

---

### 1.3 Select Your Backlog Item

For this lab, we'll implement **Backlog Item 3: Create Task with Priority and Due Date**.

Why this item?

- ✅ Touches all layers (Domain, Application, Infrastructure, API)
- ✅ Demonstrates validation logic
- ✅ Requires TDD approach
- ✅ Foundation for other items (Items 1 & 2 are prerequisites)

---

## Part 2: Implement Prerequisites (15 minutes)

> **⚠️ TDD REMINDER**: In this section, we'll follow Red-Green-Refactor:

> 1. **RED**: Write tests FIRST that fail
> 2. **GREEN**: Implement code to make tests pass
> 3. **REFACTOR**: Improve code quality while keeping tests green

Before we can create tasks with priority and due date, we need to add these properties to the domain `Task` aggregate and make sure they survive a round trip to the database (Items 1 & 2).

> **ℹ️ This lab extends existing code.** `Task`, `TaskService`, `TaskController`, `TaskEntity` and `TaskMapper` already exist and work. You are widening them, not building them from scratch.

### 2.1 Add Priority Enum (Item 1)

Ask Copilot Chat:

```text
Create a Priority enum in the taskmanager-domain module, package com.example.taskmanager.domain.tasks (next to TaskId and TaskStatus). Include values: LOW, MEDIUM, HIGH, CRITICAL. Add a fromString method that parses case-insensitively and throws IllegalArgumentException with the valid values listed. Domain code must be pure Java with no Spring or JPA imports.
```

**Expected Output** - `src-springboot/taskmanager-domain/src/main/java/com/example/taskmanager/domain/tasks/Priority.java`:

**Simple Enum Approach**:

```java
package com.example.taskmanager.domain.tasks;

public enum Priority {
    LOW,
    MEDIUM,
    HIGH,
    CRITICAL
}
```

**Enum with Parsing** (Recommended):

```java
package com.example.taskmanager.domain.tasks;

import java.util.Arrays;

public enum Priority {
    LOW(0),
    MEDIUM(1),
    HIGH(2),
    CRITICAL(3);

    private final int value;

    Priority(int value) {
        this.value = value;
    }

    public int getValue() {
        return value;
    }

    public static Priority fromValue(int value) {
        return Arrays.stream(values())
            .filter(p -> p.value == value)
            .findFirst()
            .orElseThrow(() -> new IllegalArgumentException(
                "Invalid priority value: " + value));
    }

    public static Priority fromString(String name) {
        if (name == null || name.isBlank()) {
            throw new IllegalArgumentException("Priority name cannot be null or blank");
        }

        try {
            return Priority.valueOf(name.trim().toUpperCase());
        } catch (IllegalArgumentException e) {
            throw new IllegalArgumentException(
                "Invalid priority: " + name + ". Valid values: " +
                Arrays.toString(values()), e);
        }
    }
}
```

> **Why an explicit `value`?** It duplicates `ordinal()`, but an explicit value stays stable if someone reorders the constants, whereas `ordinal()` silently changes. You can also simplify to three levels (LOW, MEDIUM, HIGH) if preferred.

### 2.2 Write Tests for Task (RED Phase)

**Following TDD**: Write tests FIRST before implementing!

Add the new tests to the existing `taskmanager-domain/src/test/java/com/example/taskmanager/domain/tasks/TaskTest.java`. Use the `/tests` command or ask Copilot Chat:

```text
Add JUnit 5 tests to the existing TaskTest in taskmanager-domain (package com.example.taskmanager.domain.tasks) for a new Task.create(title, description, priority, dueDate) factory. Verify:
- create with valid title and priority succeeds, status is PENDING
- create with a future due date succeeds
- create with null priority throws IllegalArgumentException
- create with null/blank title still throws
- create with a past due date throws IllegalArgumentException
- create with null due date is allowed
- updatePriority changes the priority; null throws IllegalArgumentException
- updateDueDate with a future date succeeds, with a past date throws, with null clears it
- reconstitute with a past due date does NOT throw and preserves createdAt

Compute due dates relative to LocalDateTime.now() (for example plusDays(1)), never hardcode calendar dates.
```

Run the domain tests. They should **FAIL** with compilation errors because `Priority` usage and the new factory do not exist on `Task` yet:

```bash
cd src-springboot
mvn test -Dtest=TaskTest -Dsurefire.failIfNoSpecifiedTests=false
```

```powershell
# Windows PowerShell: quotes are required, otherwise the dot splits the argument
cd src-springboot
mvn test "-Dtest=TaskTest" "-Dsurefire.failIfNoSpecifiedTests=false"
```

Expected result: compilation errors. This is the **RED** phase! ✅

> **Why `-Dsurefire.failIfNoSpecifiedTests=false`?** In a multi-module build, Surefire aborts on the first module that contains no test matching `-Dtest=...`. This flag lets the build continue to the module that does. Prefer running from the reactor root (or add `-am`) rather than `-pl <module>` alone, because `-pl` can resolve stale dependency jars from `~/.m2`.

### 2.3 Implement Priority and Due Date on Task (GREEN Phase)

Use `@workspace` to find the aggregate:

```text
@workspace Where is the Task aggregate defined in the Spring Boot project?
```

It lives at `taskmanager-domain/src/main/java/com/example/taskmanager/domain/tasks/Task.java` and is a **pure POJO**: no `@Entity`, no `jakarta.persistence` imports. Keep it that way. Persistence is the infrastructure module's job (see 2.4).

Then ask Copilot to update it:

```text
Update the pure-POJO Task in taskmanager-domain to add:
1. priority (required, Priority enum)
2. dueDate (LocalDateTime, nullable, must be in the future when set via create or updateDueDate)
3. A factory Task.create(title, description, priority, dueDate)
4. updatePriority(Priority) and updateDueDate(LocalDateTime) behavior methods
5. A widened reconstitute(...) that accepts priority, dueDate and createdAt and assigns them directly WITHOUT re-validating the due date
No JPA annotations. Keep the existing title validation and status transitions.
```

**Example change** - the relevant parts of `Task.java`:

```java
package com.example.taskmanager.domain.tasks;

import java.time.LocalDateTime;
import java.util.Objects;

public final class Task {

    private final TaskId id;
    private String title;
    private String description;
    private TaskStatus status;
    private Priority priority;
    private LocalDateTime dueDate;
    private final LocalDateTime createdAt;
    private LocalDateTime completedAt;

    private Task(TaskId id, String title, String description,
                 Priority priority, LocalDateTime dueDate, LocalDateTime createdAt) {
        this.id = Objects.requireNonNull(id, "Task ID cannot be null");
        this.title = validateTitle(title);
        this.description = description;
        this.status = TaskStatus.PENDING;
        this.priority = requirePriority(priority);
        this.dueDate = dueDate;
        this.createdAt = createdAt;
        this.completedAt = null;
    }

    public static Task create(String title, String description,
                              Priority priority, LocalDateTime dueDate) {
        requireFutureOrNull(dueDate);
        return new Task(TaskId.newId(), title, description,
                        priority, dueDate, LocalDateTime.now());
    }

    // Persistence calls this: a deadline that has since passed must still load
    public static Task reconstitute(
            TaskId id, String title, String description, TaskStatus status,
            Priority priority, LocalDateTime dueDate,
            LocalDateTime createdAt, LocalDateTime completedAt) {
        Task task = new Task(id, title, description, priority, dueDate, createdAt);
        task.status = status;
        task.completedAt = completedAt;
        return task;
    }

    public void updatePriority(Priority newPriority) {
        this.priority = requirePriority(newPriority);
    }

    public void updateDueDate(LocalDateTime newDueDate) {
        requireFutureOrNull(newDueDate);
        this.dueDate = newDueDate;
    }

    private static Priority requirePriority(Priority priority) {
        if (priority == null) {
            throw new IllegalArgumentException("Task priority is required");
        }
        return priority;
    }

    private static void requireFutureOrNull(LocalDateTime dueDate) {
        if (dueDate != null && !dueDate.isAfter(LocalDateTime.now())) {
            throw new IllegalArgumentException("Task due date must be in the future");
        }
    }

    public Priority getPriority() { return priority; }
    public LocalDateTime getDueDate() { return dueDate; }

    // ...existing title/description/status behavior and getters unchanged
}
```

Two subtleties worth discussing with your group:

1. **`reconstitute()` must not re-validate the due date.** Creation rejects past due dates, but a stored task whose deadline has since passed must still be loadable. Applying the rule on load would make overdue tasks permanently unreadable.
2. **`reconstitute()` must keep `createdAt`.** The original code accepted the parameter but overwrote it with `LocalDateTime.now()` (with a comment admitting the limitation). Since the mapper round trip now asserts on it, fix it while widening the signature, as shown above.

> **⚠️ Expect a wall of compilation errors.** Making `priority` required is a **breaking change**: `Task.create(String)` and `Task.create(String, String)` no longer exist as callers know them. Roughly 80 call sites across five test classes in four modules stop compiling:
>
> | Test class | Module | Approx. call sites |
> |---|---|---|
> | `TaskTest` | domain | ~36 |
> | `TaskServiceTest` | application | ~27 |
> | `JpaTaskRepositoryAdapterIntegrationTest` | infrastructure | ~20 |
> | `TaskMapperTest` | infrastructure | ~8 |
> | `TaskApiIntegrationTest` | api | ~8 |
>
> This is expected, not a mistake. Work through it module by module, domain first. Ask Copilot:
>
> ```text
> Update every Task.create(...) call in this test class to the new 4-argument form using Priority.MEDIUM and a null dueDate, unless the test is about priority or due date. Do not change assertions.
> ```
>
> **Fork in the road (non-breaking variant):** keep the old factories and have them delegate with `Priority.MEDIUM` and no due date. Nothing breaks, but "priority is required" becomes "priority defaults to MEDIUM", which weakens the acceptance criteria. This lab takes the breaking path on purpose, because repairing a suite after a deliberate change is exactly what TDD makes safe.

Run the domain tests again from `src-springboot`:

```bash
mvn test -Dtest=TaskTest -Dsurefire.failIfNoSpecifiedTests=false
```

```powershell
mvn test "-Dtest=TaskTest" "-Dsurefire.failIfNoSpecifiedTests=false"
```

Expected result: all `TaskTest` tests pass! This is the **GREEN** phase! ✅

### 2.4 Persist the New Fields (Infrastructure Round Trip)

The domain `Task` knows nothing about the database, so the new fields **will silently fail to persist** until the infrastructure module learns about them. Three files change:

1. **`TaskEntity`**: add the columns (`taskmanager-infrastructure/.../persistence/entities/TaskEntity.java`)
2. **`TaskMapper`**: map both fields in `toEntity()` and `toDomain()`
3. **`TaskMapperTest`**: extend the round-trip assertions

Write the test first. Ask Copilot:

```text
Extend TaskMapperTest in taskmanager-infrastructure so the round trip Task -> TaskEntity -> Task asserts priority, dueDate and createdAt are preserved, including a task whose dueDate is in the past (reconstitute must not throw).
```

Then implement:

```java
// TaskEntity.java (infrastructure): JPA annotations belong here, not on Task
@Column(name = "priority", nullable = false, length = 20)
private String priority;

@Column(name = "due_date")
private LocalDateTime dueDate;
```

```java
// TaskMapper.toEntity
entity.setPriority(task.getPriority().name());
entity.setDueDate(task.getDueDate());

// TaskMapper.toDomain
return Task.reconstitute(
    taskId,
    entity.getTitle(),
    entity.getDescription(),
    status,
    Priority.valueOf(entity.getPriority()),
    entity.getDueDate(),
    entity.getCreatedAt(),
    entity.getCompletedAt()
);
```

Run the infrastructure tests from the reactor root:

```bash
mvn test -Dtest='TaskMapperTest,JpaTaskRepositoryAdapterIntegrationTest' -Dsurefire.failIfNoSpecifiedTests=false
```

```powershell
mvn test "-Dtest=TaskMapperTest,JpaTaskRepositoryAdapterIntegrationTest" "-Dsurefire.failIfNoSpecifiedTests=false"
```

> **📝 Production note: schema migration.** Production uses `ddl-auto: validate`, while `dev` and `test` use `create-drop`, which creates the new columns automatically and hides the gap. Before this reaches a real database you need a migration adding `priority` and `due_date`. This lab does not cover migrations.

### 2.5 Refactor (If Needed)

Review the code and tests:

- Are there any code smells?
- Can the validation helpers be shared?
- Are error messages clear?
- Does the domain module still have zero Spring/JPA imports?

If you make changes, re-run the tests to ensure they still pass:

```bash
mvn test
```

**Part 2 Complete!** You've added Priority and DueDate to the domain model and its persistence mapping using TDD.

---

## Part 3: Implement Backlog Item 3 (TDD) (15 minutes)

Now extend the existing feature: Create Task with Priority and Due Date through the API.

### 3.1 Update the Application Layer

`TaskService` (`taskmanager-application/src/main/java/com/example/taskmanager/application/services/TaskService.java`) is a single concrete class with no interface. With one implementation, an interface adds a file and no value, so we do not introduce `TaskServiceImpl`.

#### Step 1: Write Service Tests (RED)

Add to the existing `TaskServiceTest` (same module, package `com.example.taskmanager.application.services`). Ask Copilot:

```text
Add Mockito tests to TaskServiceTest for a new createTask(String title, String description, Priority priority, LocalDateTime dueDate) method on TaskService:
- Valid input saves a task with the given priority and dueDate (use ArgumentCaptor on TaskRepository.save)
- Null priority throws IllegalArgumentException and save is never called
- Past due date throws IllegalArgumentException and save is never called
- Null title throws and save is never called
Use dates relative to LocalDateTime.now().
```

Run (the tests fail to compile until the method exists):

```bash
mvn test -Dtest=TaskServiceTest -Dsurefire.failIfNoSpecifiedTests=false
```

```powershell
mvn test "-Dtest=TaskServiceTest" "-Dsurefire.failIfNoSpecifiedTests=false"
```

#### Step 2: Implement the Service Method (GREEN)

Replace the two existing `createTask` overloads (they can no longer build a `Task`) with:

```java
@Transactional
public Task createTask(String title, String description,
                       Priority priority, LocalDateTime dueDate) {
    Task task = Task.create(title, description, priority, dueDate);
    return taskRepository.save(task);
}
```

The service stays an orchestrator: all validation lives in `Task.create`. Run the tests again:

```bash
mvn test -Dtest=TaskServiceTest -Dsurefire.failIfNoSpecifiedTests=false
```

Expected result: all service tests pass! ✅

> **Stale jars:** if a downstream module fails with `NoClassDefFoundError` for `Priority`, it resolved an old domain jar from `~/.m2`. Run from the reactor root, add `-am`, or run `mvn clean install -DskipTests` first.

### 3.2 Extend the API Layer (Following TDD)

`POST /api/tasks` **already exists** in `TaskController`, alongside `GET /{id}`, `GET /`, `PATCH /{id}`, `PUT /{id}/start`, `PUT /{id}/complete`, `PUT /{id}/cancel` and `DELETE /{id}`. We are extending it, not creating it.

#### Step 1: Write Integration Tests FIRST (RED Phase)

Add to the existing `TaskApiIntegrationTest` in `taskmanager-api/src/test/java/com/example/taskmanager/api/`. Ask Copilot:

```text
Add tests to TaskApiIntegrationTest for POST /api/tasks with the new priority and dueDate fields:
- Valid request with all fields returns 201 and the body contains priority and dueDate
- Valid request with priority only returns 201 and dueDate is null
- Lowercase priority ("high") is accepted and returned as HIGH
- Invalid priority returns 400 with an RFC 7807 body (check $.detail)
- Past due date returns 400
- Missing priority returns 400 with $.errors.priority
- Missing/blank title still returns 400

Follow the style of the existing tests in the file. Build due dates from LocalDateTime.now().plusDays(7), never a literal date.
```

Run them:

```bash
mvn test -Dtest=TaskApiIntegrationTest -Dsurefire.failIfNoSpecifiedTests=false
```

```powershell
mvn test "-Dtest=TaskApiIntegrationTest" "-Dsurefire.failIfNoSpecifiedTests=false"
```

Expected result: the new tests **FAIL**. Note the honest RED signal: the endpoint exists, so you will **not** see a 404. Instead the request is accepted with `201` but the response has no `priority` field, so the JSON assertions fail. A test that fails on a missing field is a perfectly good TDD red. ✅

If the whole class fails to compile first, that is the Part 2 breaking change showing up in this module; fix those `Task.create(...)` calls before continuing.

#### Step 2: Extend the DTOs

Widen the existing records rather than creating new ones, so existing validation and the `TaskResponse.from(Task)` factory are preserved.

`taskmanager-api/src/main/java/com/example/taskmanager/api/dto/CreateTaskRequest.java`:

```java
package com.example.taskmanager.api.dto;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Size;

import java.time.LocalDateTime;

public record CreateTaskRequest(
    @NotBlank(message = "Task title is required")
    @Size(max = 200, message = "Task title cannot exceed 200 characters")
    String title,

    @Size(max = 2000, message = "Task description cannot exceed 2000 characters")
    String description,

    @NotBlank(message = "Task priority is required")
    String priority,

    LocalDateTime dueDate
) {}
```

`TaskResponse.java` gains two components and maps them in `from(Task)`:

```java
public record TaskResponse(
    UUID id,
    String title,
    String description,
    String status,
    String priority,
    LocalDateTime dueDate,
    LocalDateTime createdAt,
    LocalDateTime completedAt
) {
    public static TaskResponse from(Task task) {
        return new TaskResponse(
            task.getId().value(),
            task.getTitle(),
            task.getDescription(),
            task.getStatus().name(),
            task.getPriority().name(),
            task.getDueDate(),
            task.getCreatedAt(),
            task.getCompletedAt()
        );
    }
}
```

> The old `CreateTaskRequest.of(...)` convenience factories no longer fit a four-field record; update or remove them and fix their callers.

#### Step 3: Update the Controller (GREEN Phase)

Ask Copilot:

```text
In TaskController.createTask, parse request.priority() with Priority.fromString and pass priority and dueDate to the new TaskService.createTask overload. Do NOT add any @ExceptionHandler to the controller: GlobalExceptionHandler already maps IllegalArgumentException to a 400 ProblemDetail.
```

The change is two lines inside the existing method:

```java
Priority priority = Priority.fromString(request.priority());
Task task = taskService.createTask(
    request.title(), request.description(), priority, request.dueDate());
```

> **🚫 Do not add a controller-local `@ExceptionHandler`.** A handler declared in a controller **takes precedence** over the `@RestControllerAdvice`. Adding one silently changes this one controller's error contract from RFC 7807 `ProblemDetail` to a bespoke shape, leaving the API inconsistent and breaking existing assertions on `$.title` and `$.errors.*`.
>
> Instead, read `taskmanager-api/.../exception/GlobalExceptionHandler.java`. It already maps `TaskNotFoundException` → 404, `IllegalArgumentException` and `IllegalStateException` → 400, `MethodArgumentNotValidException` → 400 with a field-keyed `errors` map, and a catch-all → 500. Throwing the right exception from the domain is all the controller needs to do.

Run the integration tests again:

```bash
mvn test -Dtest=TaskApiIntegrationTest -Dsurefire.failIfNoSpecifiedTests=false
```

Expected result: all integration tests pass! This is the **GREEN** phase! ✅

#### Step 4: Create Manual Testing File

Create a `tasks.http` file in `src-springboot/` for manual testing with the REST Client extension:

```text
Create a tasks.http file in src-springboot/ with test scenarios for POST /api/tasks including: all fields, required fields only, all priority levels, invalid priority, missing priority, blank title, past due date, and future due date. Use REST Client format with @baseUrl = http://localhost:8080 and compute the future due date dynamically rather than hardcoding it.
```

Example `tasks.http`:

```http
### Variables
@baseUrl = http://localhost:8080
@contentType = application/json
# Due dates must be in the future. REST Client computes one 7 days ahead,
# so this file does not expire. If your version lacks $datetime, type any
# date at least a day ahead, e.g. 2099-01-01T17:00:00.
@dueDate = {{$datetime "YYYY-MM-DDTHH:mm:ss" 7 d}}

### Create task with all fields
POST {{baseUrl}}/api/tasks
Content-Type: {{contentType}}

{
  "title": "Complete Lab 2",
  "description": "Finish requirements to code lab",
  "priority": "HIGH",
  "dueDate": "{{dueDate}}"
}

### Create task with required fields only
POST {{baseUrl}}/api/tasks
Content-Type: {{contentType}}

{
  "title": "Review documentation",
  "priority": "LOW"
}

### Invalid priority (expect 400)
POST {{baseUrl}}/api/tasks
Content-Type: {{contentType}}

{
  "title": "Test task",
  "priority": "SUPER_URGENT"
}

### Missing priority (expect 400 with errors.priority)
POST {{baseUrl}}/api/tasks
Content-Type: {{contentType}}

{
  "title": "No priority"
}

### Past due date (expect 400)
POST {{baseUrl}}/api/tasks
Content-Type: {{contentType}}

{
  "title": "Past task",
  "priority": "MEDIUM",
  "dueDate": "2020-01-01T00:00:00"
}
```

### 3.3 Run Full Test Suite

From `src-springboot`:

```bash
mvn clean test
```

All tests should pass! ✅

**Expected output** (counts are deltas against your starting baseline, which you recorded in the prerequisites):

- `taskmanager-domain`: add at least ~25 tests covering `Priority`, `Task.create`, `updatePriority`, `updateDueDate` and `reconstitute`
- `taskmanager-application`: add at least 3 tests for the new `createTask` validation paths
- `taskmanager-infrastructure`: add at least 2 tests for the priority/dueDate/createdAt round trip
- `taskmanager-api`: add at least ~10 tests for the extended `POST /api/tasks`
- Build: SUCCESS

Exact totals drift as the reference implementation evolves, so compare against your own baseline rather than an absolute number.

---

## Part 4: Manual Testing & Validation (5 minutes)

### 4.1 Run the API

The default profile targets PostgreSQL at `localhost:5432` with `ddl-auto: validate`, so the app will not start without a database. Use the `dev` profile instead: it runs on in-memory H2 with `ddl-auto: create-drop`.

Build and install all modules first so the API runs against your changed domain jar, not a stale one in `~/.m2`:

```bash
cd src-springboot
mvn clean install -DskipTests
mvn spring-boot:run -pl taskmanager-api -Dspring-boot.run.profiles=dev
```

```powershell
cd src-springboot
mvn clean install -DskipTests
mvn spring-boot:run -pl taskmanager-api "-Dspring-boot.run.profiles=dev"
```

The API starts on `http://localhost:8080`. The H2 console is available at `http://localhost:8080/h2-console`, handy for inspecting the new `priority` and `due_date` columns.

### 4.2 Test with REST Client Extension (Recommended)

If you created the `tasks.http` file:

1. Install the **REST Client** extension in VS Code (by Huachao Mao)
2. Open `src-springboot/tasks.http`
3. Click **"Send Request"** above any test scenario
4. View the response in a split pane

This is the easiest way to test your API!

### 4.3 Test with curl (Alternative)

Due dates must be in the future, so compute one instead of copying a fixed date:

```bash
# macOS
DUE=$(date -v+7d +"%Y-%m-%dT%H:%M:%S")
# Linux
DUE=$(date -d '+7 days' +"%Y-%m-%dT%H:%M:%S")
```

```powershell
# Windows PowerShell
$DUE = (Get-Date).AddDays(7).ToString("yyyy-MM-ddTHH:mm:ss")
```

**Valid Request** (bash):

```bash
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d "{
    \"title\": \"Complete Lab 2\",
    \"description\": \"Finish requirements to code lab\",
    \"priority\": \"HIGH\",
    \"dueDate\": \"$DUE\"
  }"
```

**Expected Response**: 201 Created (values below are illustrative; yours will differ)

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Complete Lab 2",
  "description": "Finish requirements to code lab",
  "status": "PENDING",
  "priority": "HIGH",
  "dueDate": "<the value of $DUE>",
  "createdAt": "<now>",
  "completedAt": null
}
```

**Invalid Priority**:

```bash
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Task",
    "priority": "SuperUrgent"
  }'
```

**Expected Response**: 400 Bad Request with an RFC 7807 `ProblemDetail` from `GlobalExceptionHandler` (abridged):

```json
{
  "title": "Invalid Input",
  "status": 400,
  "detail": "Invalid priority: SuperUrgent. Valid values: [LOW, MEDIUM, HIGH, CRITICAL]"
}
```

**Past Due Date**:

```bash
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Test Task",
    "priority": "LOW",
    "dueDate": "2020-01-01T00:00:00"
  }'
```

**Expected Response**: 400 Bad Request, `detail`: `Task due date must be in the future`.

**Missing Priority**: omitting `priority` returns 400 with `errors.priority` set to `Task priority is required` (a field-keyed map from bean validation).

> **Two 400 shapes:** bean-validation failures return a field-keyed `errors` map, while domain failures return a plain `detail` string. Both are valid RFC 7807, but clients must handle both. That is worth a deliberate team decision.

### 4.4 Verify Test Results

Confirm that:

- ✅ Valid requests return 201 Created with `priority` and `dueDate` in the body
- ✅ Invalid priority returns 400 Bad Request with a clear `detail`
- ✅ Past due dates return 400 Bad Request with a validation error
- ✅ Missing priority and missing/blank title return 400 Bad Request
- ✅ Optional fields (description, dueDate) can be omitted
- ✅ Values survive a database round trip (check `priority` and `due_date` in the H2 console)

---

## Key Learning Points

### ✅ AI-Assisted Requirements Analysis

1. **Decomposition**: Copilot helped break vague user story into concrete items
2. **Acceptance Criteria**: Generated testable, specific criteria for each item
3. **Technical Context**: Understood existing architecture and suggested appropriate Spring Boot patterns
4. **Comprehensive Coverage**: Identified edge cases and validation rules

### ✅ Full-Stack TDD Workflow

1. **Red-Green-Refactor Applied**: Tests written FIRST at every layer
2. **Domain Layer TDD**: `Task` tests → implementation → refactor
3. **Application Layer TDD**: Service tests → implementation → validation
4. **Infrastructure Round Trip**: Mapper tests prove new fields persist
5. **API Layer TDD**: Integration tests → controller change → manual testing
6. **Repairing a suite after a breaking change** is safe when the tests are good

### ✅ Clean Architecture Maintained

1. **Dependencies Flow Inward**: API (Controllers) → Application (Services) → Domain (Task)
2. **Domain Purity**: `Task` and `Priority` have no Spring or JPA imports; `TaskEntity` and `TaskMapper` absorb persistence concerns
3. **Application Logic**: Services orchestrate use cases and coordinate domain objects
4. **API Responsibility**: Only request/response mapping and HTTP concerns, no business logic

### ✅ Spring Boot Best Practices

1. **Dependency Injection**: Constructor injection with Lombok's `@RequiredArgsConstructor`
2. **Transaction Management**: `@Transactional` at the service layer
3. **Centralized Exception Handling**: `GlobalExceptionHandler` (`@RestControllerAdvice`) produces consistent RFC 7807 responses; controllers do not hand-roll error bodies
4. **DTOs**: Separated domain models from API contracts using records
5. **Ports and Adapters**: `TaskRepository` is a domain port implemented by `JpaTaskRepositoryAdapter`

### ⚠️ Known Gaps

1. **No schema migration** for the new columns (see 2.4)
2. **No injected `Clock`**: due-date validation reads `LocalDateTime.now()` directly, so boundary cases are hard to test. Item 5 will make this painful.

---

## Extension Exercises (If Time Permits)

### Exercise 1: Implement Item 4 (Filter by Priority)

1. Generate acceptance criteria tests
2. Add a `findByPriority` method to the `TaskRepository` port (domain), implement it in `JpaTaskRepositoryAdapter` and `SpringDataTaskRepository`
3. Add API endpoint query parameters
4. Test with multiple priority filters

### Exercise 2: Implement Item 5 (Due Soon)

1. Inject a `Clock` and write tests for date range calculations
2. Create service method in Application layer
3. Add API endpoint
4. Test edge cases (timezone boundaries)

### Exercise 3: Update Priority and Due Date via PATCH

1. `PATCH /api/tasks/{id}` already exists; generate a backlog item to extend it
2. Extend `UpdateTaskRequest` with optional `priority` and `dueDate`
3. Call `updatePriority`/`updateDueDate` through `TaskService`
4. Test validation and error cases

---

## Success Criteria

You've completed this lab successfully when:

- ✅ User story decomposed into 5 backlog items with acceptance criteria
- ✅ `Priority` enum created in `domain.tasks`
- ✅ Domain `Task` updated with priority and dueDate, with no JPA annotations
- ✅ `TaskEntity` and `TaskMapper` persist the new fields
- ✅ `TaskService.createTask` extended with tests
- ✅ `POST /api/tasks` accepts priority and dueDate with proper validation via `GlobalExceptionHandler`
- ✅ Existing tests repaired and the full reactor passes (`mvn clean test`)
- ✅ Manual testing against the `dev` profile confirms expected behavior
- ✅ Clean Architecture principles maintained throughout

---

## Troubleshooting

### Copilot Generates Generic Backlog Items

**Problem**: Backlog items don't consider existing architecture  
**Solution**: Use `@workspace` to give context: "Given our Spring Boot Clean Architecture structure..."

### Tests Don't Cover Edge Cases

**Problem**: Missing validation tests  
**Solution**: Explicitly ask: "Generate JUnit 5 tests for all guard clauses and edge cases"

### Repository Needs a New Method

**Problem**: `TaskRepository` doesn't have a method you need  
**Solution**: `TaskRepository` is a **domain port**, a plain interface with business-intent methods. Add the method there, implement it in `JpaTaskRepositoryAdapter` (infrastructure), delegating to the package-private `SpringDataTaskRepository extends JpaRepository<TaskEntity, UUID>`. Never make the domain port extend `JpaRepository`; that would pull Spring Data into the domain module and invert the dependency direction.

### Date Validation Issues

**Problem**: Due date validation fails unexpectedly  
**Solution**: Remember sample dates must be in the future; compute them relative to now. For boundary tests, consider injecting a `java.time.Clock`.

### Maven Build Fails

**Problem**: Compilation errors or dependency issues  
**Solution**: From `src-springboot`, run `mvn clean install -DskipTests`. After the Part 2 signature change, compilation errors in existing tests are expected; fix them module by module.

### `No tests matching pattern ... were executed!`

**Problem**: Surefire aborts on a module with no match for `-Dtest=`  
**Solution**: Add `-Dsurefire.failIfNoSpecifiedTests=false` (quote each `-D` argument in PowerShell).

### `NoClassDefFoundError` or HTTP 500 on every request

**Problem**: A module resolved a stale `taskmanager-domain` jar from `~/.m2`  
**Solution**: Run `mvn clean install -DskipTests` from the reactor root, or use `-am` with `-pl`.

### Spring Boot Won't Start

**Problem**: Application context fails to load  
**Solution**: Run with the `dev` profile (`-Dspring-boot.run.profiles=dev`) so it uses in-memory H2 instead of PostgreSQL. Check `application.yml` and `application-dev.yml` in `taskmanager-api/src/main/resources`.

---

## Next Steps

Move on to [**Lab 3: Code Generation & Refactoring**](lab-03-generation-and-refactoring.md) where you'll:

- Scaffold complete API endpoints with Copilot
- Refactor legacy code using `/refactor` command
- Apply Object Calisthenics principles
- Use `@workspace` for cross-file understanding

---

## Documenting Architectural Decisions (ADR)

> **Why ADRs?**
> 
> As you make key design or architectural choices (e.g., how to model priorities, validation, or API structure), it's best practice to capture your reasoning in an [Architecture Decision Record (ADR)](https://adr.github.io/). This helps your team understand why decisions were made and makes future changes easier to justify.

**Sample Copilot Prompt:**
```text
Write an Architecture Decision Record (ADR) for our approach to modeling task priorities as an enum in the Domain layer. Include:
- Context and alternatives considered
- Decision summary
- Consequences (tradeoffs, future impact)
Format as Markdown.
```

**Where to put ADRs:**
- Save ADRs in the `docs/adr/` folder (create it if it doesn't exist).
- Use a clear filename, e.g., `docs/adr/0001-task-priority-enum.md`.

**Learn more:** [ADR GitHub site](https://adr.github.io/)

---

## Additional Resources

### Spring Boot & Java
- [Spring Boot Testing Documentation](https://docs.spring.io/spring-boot/docs/current/reference/html/features.html#features.testing)
- [JUnit 5 User Guide](https://junit.org/junit5/docs/current/user-guide/)
- [Mockito Documentation](https://javadoc.io/doc/org.mockito/mockito-core/latest/org/mockito/Mockito.html)
- [Spring Data JPA Reference](https://docs.spring.io/spring-data/jpa/docs/current/reference/html/)

### Architecture & Patterns
- [User Story Best Practices](https://www.atlassian.com/agile/project-management/user-stories)
- [Clean Architecture](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [DDD in Spring Boot](https://reflectoring.io/spring-boot-clean-architecture/)

### Cross-Reference
- [.NET version of this lab](lab-02-requirements-to-code.md) - Compare patterns and approaches
- [Pattern Translation Guide](../guides/dotnet-to-springboot-patterns.md) - .NET ↔ Spring Boot equivalencies
