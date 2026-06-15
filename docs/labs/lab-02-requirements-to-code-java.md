# Lab 2: From Requirements to Code with GitHub Copilot (Java/Spring Boot)

> **💡 Also available**: [.NET version](lab-02-requirements-to-code.md) using C# and ASP.NET Core

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
- ✅ Repository at clean state: `git status` shows no uncommitted changes
- ✅ Java 21 and Maven installed
- ✅ JUnit 5 and Mockito test dependencies configured
- ✅ Spring Boot 3.x dependencies (spring-boot-starter-web, spring-boot-starter-data-jpa)

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

- Update Task entity in Domain layer
- Add Priority enum as value object
- Update database schema with JPA annotations
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

- Update Task entity in Domain layer
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

- Update CreateTaskRequest DTO in presentation layer
- Add validation logic in service layer
- Update API endpoint and response DTOs
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
- Implement filtering in repository using JPA Specifications
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

Before we can create tasks with priority and due date, we need to add these properties to the Task entity (Items 1 & 2).

### 2.1 Add Priority Enum (Item 1)

Ask Copilot Chat:

```text
Create a Priority enum in the Domain layer following DDD patterns. Include values: LOW, MEDIUM, HIGH, CRITICAL. Add methods to parse from string safely. Follow Spring Boot conventions.
```

**Expected Output** - `src-springboot/domain/valueobjects/Priority.java`:

**Simple Enum Approach**:

```java
package com.taskmanager.domain.valueobjects;

public enum Priority {
    LOW,
    MEDIUM,
    HIGH,
    CRITICAL
}
```

**DDD Enum with Parsing** (Recommended):

```java
package com.taskmanager.domain.valueobjects;

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
            return Priority.valueOf(name.toUpperCase());
        } catch (IllegalArgumentException e) {
            throw new IllegalArgumentException(
                "Invalid priority name: " + name + ". Valid values: " + 
                Arrays.toString(values()), e);
        }
    }
}
```

> **Note**: Our implementation uses four priority levels. You can simplify to three (LOW, MEDIUM, HIGH) if preferred.

### 2.2 Write Tests for Task Entity (RED Phase)

**Following TDD**: Write tests FIRST before implementing!

Use the `/tests` command or ask Copilot Chat:

```text
Generate JUnit 5 tests for the Task entity in src-springboot/test/java/com/taskmanager/domain/entities/TaskTest.java that verify:
- Task.create with valid title and priority succeeds
- Task.create with valid title, priority, and future due date succeeds
- Task.create with null/empty/blank title throws IllegalArgumentException
- Task.create with past due date throws IllegalArgumentException
- Task.create with null due date is allowed
- updatePriority updates the priority correctly
- updateDueDate with future date succeeds
- updateDueDate with past date throws IllegalArgumentException
- markAsCompleted sets isCompleted and completedAt

Use JUnit 5 assertions and test lifecycle.
```

Run tests - they should **FAIL** because the Task entity doesn't exist yet or doesn't have these properties:

```bash
mvn test
```

Expected result: Tests fail with compilation errors. This is the **RED** phase! ✅

### 2.3 Implement Task Entity (GREEN Phase)

Now that we have failing tests, implement the code to make them pass.

Use `@workspace` to find the Task entity:

```text
@workspace Where is the Task entity defined in the Spring Boot project?
```

Then ask Copilot to update it:

```text
Update the Task entity in the Spring Boot project to add:
1. Priority property (required, using Priority enum)
2. dueDate property (LocalDateTime, nullable)
3. Validation: dueDate must be in future if provided
4. Methods to update priority and due date
Follow DDD patterns: proper encapsulation, invariant enforcement, factory method for creation
```

**Example Updated Entity** - `src-springboot/domain/entities/Task.java`:

```java
package com.taskmanager.domain.entities;

import com.taskmanager.domain.valueobjects.Priority;
import jakarta.persistence.*;
import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@Table(name = "tasks")
public class Task {
    
    @Id
    private UUID id;
    
    @Column(nullable = false)
    private String title;
    
    @Column(length = 1000)
    private String description;
    
    @Enumerated(EnumType.STRING)
    @Column(nullable = false)
    private Priority priority;
    
    @Column(name = "due_date")
    private LocalDateTime dueDate;
    
    @Column(nullable = false)
    private boolean completed;
    
    @Column(nullable = false)
    private LocalDateTime createdAt;
    
    @Column(name = "completed_at")
    private LocalDateTime completedAt;

    // JPA requires default constructor
    protected Task() {
    }

    // Private constructor - use factory method
    private Task(UUID id, String title, String description, 
                 Priority priority, LocalDateTime dueDate) {
        this.id = id;
        this.title = title;
        this.description = description;
        this.priority = priority;
        this.dueDate = dueDate;
        this.completed = false;
        this.createdAt = LocalDateTime.now();
    }

    public static Task create(String title, String description, 
                             Priority priority, LocalDateTime dueDate) {
        if (title == null || title.isBlank()) {
            throw new IllegalArgumentException("Title cannot be null or blank");
        }
        
        if (priority == null) {
            throw new IllegalArgumentException("Priority cannot be null");
        }

        if (dueDate != null && dueDate.isBefore(LocalDateTime.now())) {
            throw new IllegalArgumentException("Due date must be in the future");
        }

        return new Task(UUID.randomUUID(), title, description, priority, dueDate);
    }

    public void updatePriority(Priority priority) {
        if (priority == null) {
            throw new IllegalArgumentException("Priority cannot be null");
        }
        this.priority = priority;
    }

    public void updateDueDate(LocalDateTime dueDate) {
        if (dueDate != null && dueDate.isBefore(LocalDateTime.now())) {
            throw new IllegalArgumentException("Due date must be in the future");
        }
        this.dueDate = dueDate;
    }

    public void markAsCompleted() {
        if (this.completed) {
            throw new IllegalStateException("Task is already completed");
        }
        this.completed = true;
        this.completedAt = LocalDateTime.now();
    }

    // Getters
    public UUID getId() { return id; }
    public String getTitle() { return title; }
    public String getDescription() { return description; }
    public Priority getPriority() { return priority; }
    public LocalDateTime getDueDate() { return dueDate; }
    public boolean isCompleted() { return completed; }
    public LocalDateTime getCreatedAt() { return createdAt; }
    public LocalDateTime getCompletedAt() { return completedAt; }
}
```

Run tests again:

```bash
mvn test
```

Expected result: All tests pass! This is the **GREEN** phase! ✅

### 2.4 Refactor (If Needed)

Review the code and tests:

- Are there any code smells?
- Can validation logic be extracted to a validator class?
- Are error messages clear?
- Is the code following DDD patterns?

If you make changes, re-run tests to ensure they still pass:

```bash
mvn test
```

**Part 2 Complete!** You've successfully added Priority and DueDate to the Task entity using proper TDD.

---

## Part 3: Implement Backlog Item 3 (TDD) (15 minutes)

Now implement the full feature: Create Task with Priority and Due Date through the API.

### 3.1 Design the Application Layer

#### Step 1: Create the Service Method

Ask Copilot Chat:

```text
Create a TaskService interface and implementation in the Application layer with a method to create tasks:
- Method: createTask(CreateTaskRequest request)
- Parameters: title (required), description (optional), priority (required as string), dueDate (optional)
- Returns: created Task entity
- Validates priority string and due date
- Uses TaskRepository to save

Follow Spring Boot service patterns with @Service annotation.
```

**Expected Output** - `src-springboot/application/services/TaskService.java`:

```java
package com.taskmanager.application.services;

import com.taskmanager.application.dto.CreateTaskRequest;
import com.taskmanager.domain.entities.Task;

public interface TaskService {
    Task createTask(CreateTaskRequest request);
}
```

**Expected Output** - `src-springboot/application/services/TaskServiceImpl.java`:

```java
package com.taskmanager.application.services;

import com.taskmanager.application.dto.CreateTaskRequest;
import com.taskmanager.domain.entities.Task;
import com.taskmanager.domain.repositories.TaskRepository;
import com.taskmanager.domain.valueobjects.Priority;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Slf4j
@RequiredArgsConstructor
public class TaskServiceImpl implements TaskService {
    
    private final TaskRepository taskRepository;

    @Override
    @Transactional
    public Task createTask(CreateTaskRequest request) {
        if (request == null) {
            throw new IllegalArgumentException("Request cannot be null");
        }

        log.info("Creating task with title: {} and priority: {}", 
                 request.title(), request.priority());

        // Parse priority from string
        Priority priority = Priority.fromString(request.priority());

        // Create task entity (validates due date)
        Task task = Task.create(
            request.title(),
            request.description(),
            priority,
            request.dueDate()
        );

        // Save via repository
        Task savedTask = taskRepository.save(task);

        log.info("Task created successfully with ID: {}", savedTask.getId());

        return savedTask;
    }
}
```

#### Step 2: Create Request DTO

Create a record for the request:

**Expected Output** - `src-springboot/application/dto/CreateTaskRequest.java`:

```java
package com.taskmanager.application.dto;

import java.time.LocalDateTime;

public record CreateTaskRequest(
    String title,
    String description,
    String priority,
    LocalDateTime dueDate
) {}
```

#### Step 3: Write Service Tests (RED)

Ask Copilot:

```text
Create JUnit 5 tests for TaskServiceImpl in src-springboot/test/java/com/taskmanager/application/services/TaskServiceImplTest.java. Test:
- Valid request creates task with correct properties
- Invalid priority string throws IllegalArgumentException
- Past due date throws IllegalArgumentException
- Null title throws IllegalArgumentException
- Null request throws IllegalArgumentException

Use Mockito for TaskRepository and verify save was called.
```

Run tests - they should **FAIL** if not implemented yet:

```bash
mvn test -Dtest=TaskServiceImplTest
```

#### Step 4: Implement Service (GREEN)

If you've already implemented the service above, run tests:

```bash
mvn test -Dtest=TaskServiceImplTest
```

Expected result: All service tests pass! ✅

### 3.2 Update the API Layer (Following TDD)

#### Step 1: Create Response DTO

Ask Copilot:

```text
Create a TaskResponse DTO record in src-springboot/presentation/dto/ for the POST /api/tasks endpoint response. Include all task properties with appropriate JSON property names.
```

**Expected Output** - `src-springboot/presentation/dto/TaskResponse.java`:

```java
package com.taskmanager.presentation.dto;

import com.taskmanager.domain.valueobjects.Priority;
import com.fasterxml.jackson.annotation.JsonFormat;
import java.time.LocalDateTime;
import java.util.UUID;

public record TaskResponse(
    UUID id,
    String title,
    String description,
    Priority priority,
    @JsonFormat(pattern = "yyyy-MM-dd'T'HH:mm:ss")
    LocalDateTime dueDate,
    boolean completed,
    @JsonFormat(pattern = "yyyy-MM-dd'T'HH:mm:ss")
    LocalDateTime createdAt,
    @JsonFormat(pattern = "yyyy-MM-dd'T'HH:mm:ss")
    LocalDateTime completedAt
) {}
```

#### Step 2: Write Integration Tests FIRST (RED Phase)

**Following TDD**: Write integration tests BEFORE implementing the endpoint!

Ask Copilot:

```text
Create integration tests for POST /api/tasks endpoint in src-springboot/test/java/com/taskmanager/presentation/controllers/TaskControllerIntegrationTest.java that verify:
- Valid request with all fields returns 201 Created with task details and Location header
- Valid request with only required fields returns 201 Created
- Invalid priority returns 400 Bad Request
- Past due date returns 400 Bad Request
- Missing/empty/blank title returns 400 Bad Request
- Optional fields (description, dueDate) handled correctly

Use @SpringBootTest with WebEnvironment.RANDOM_PORT and TestRestTemplate.
```

Run the integration tests - they should **FAIL** with 404 Not Found (endpoint doesn't exist yet):

```bash
mvn test -Dtest=TaskControllerIntegrationTest
```

Expected result: All integration tests fail. This is the **RED** phase! ✅

#### Step 3: Implement the Controller (GREEN Phase)

Now implement the controller to make the tests pass:

```text
Implement POST /api/tasks endpoint in a TaskController (@RestController) that:
1. Accepts CreateTaskRequest as JSON body
2. Validates request using @Valid and @NotBlank annotations
3. Calls TaskService.createTask to create the task
4. Maps the domain Task entity to TaskResponse DTO
5. Returns 201 Created with Location header and TaskResponse body
6. Handles IllegalArgumentException → 400 Bad Request
7. Handles unexpected exceptions → 500 Internal Server Error

Use @RestController, @PostMapping, @ResponseStatus, and proper exception handling with @ExceptionHandler.
```

**Expected Output** - `src-springboot/presentation/controllers/TaskController.java`:

```java
package com.taskmanager.presentation.controllers;

import com.taskmanager.application.dto.CreateTaskRequest;
import com.taskmanager.application.services.TaskService;
import com.taskmanager.domain.entities.Task;
import com.taskmanager.presentation.dto.TaskResponse;
import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;
import org.springframework.web.servlet.support.ServletUriComponentsBuilder;

import java.net.URI;

@RestController
@RequestMapping("/api/tasks")
@Slf4j
@RequiredArgsConstructor
public class TaskController {
    
    private final TaskService taskService;

    @PostMapping
    public ResponseEntity<TaskResponse> createTask(@RequestBody CreateTaskRequest request) {
        log.info("Received POST /api/tasks request");

        Task task = taskService.createTask(request);

        TaskResponse response = new TaskResponse(
            task.getId(),
            task.getTitle(),
            task.getDescription(),
            task.getPriority(),
            task.getDueDate(),
            task.isCompleted(),
            task.getCreatedAt(),
            task.getCompletedAt()
        );

        URI location = ServletUriComponentsBuilder
            .fromCurrentRequest()
            .path("/{id}")
            .buildAndExpand(task.getId())
            .toUri();

        return ResponseEntity.created(location).body(response);
    }

    @ExceptionHandler(IllegalArgumentException.class)
    public ResponseEntity<ErrorResponse> handleIllegalArgument(IllegalArgumentException ex) {
        log.warn("Validation error: {}", ex.getMessage());
        return ResponseEntity.badRequest()
            .body(new ErrorResponse("Validation Error", ex.getMessage()));
    }

    record ErrorResponse(String error, String message) {}
}
```

Run the integration tests again:

```bash
mvn test -Dtest=TaskControllerIntegrationTest
```

Expected result: All integration tests pass! This is the **GREEN** phase! ✅

#### Step 4: Create Manual Testing File

Create a `tasks.http` file in the project root for manual testing with the REST Client extension:

```text
Create a tasks.http file in src-springboot/ with test scenarios for POST /api/tasks endpoint including:
- Valid requests with all fields
- Valid requests with required fields only
- All priority levels (LOW, MEDIUM, HIGH, CRITICAL)
- Invalid priority
- Missing/empty/blank title
- Past due date
- Future due date
- Optional field combinations

Use REST Client format with @baseUrl variable set to http://localhost:8080
```

Example `tasks.http`:

```http
### Variables
@baseUrl = http://localhost:8080
@contentType = application/json

### Create task with all fields
POST {{baseUrl}}/api/tasks
Content-Type: {{contentType}}

{
  "title": "Complete Lab 2",
  "description": "Finish requirements to code lab",
  "priority": "HIGH",
  "dueDate": "2026-04-15T17:00:00"
}

### Create task with required fields only
POST {{baseUrl}}/api/tasks
Content-Type: {{contentType}}

{
  "title": "Review documentation",
  "priority": "LOW"
}

### Invalid priority
POST {{baseUrl}}/api/tasks
Content-Type: {{contentType}}

{
  "title": "Test task",
  "priority": "SUPER_URGENT"
}

### Past due date
POST {{baseUrl}}/api/tasks
Content-Type: {{contentType}}

{
  "title": "Past task",
  "priority": "MEDIUM",
  "dueDate": "2020-01-01T00:00:00"
}
```

### 3.3 Run Full Test Suite

```bash
mvn clean test
```

All tests should pass! ✅

**Expected output**:

- Unit tests: All passing (10+ for TaskServiceImpl, 11+ for Task entity)
- Integration tests: All passing (6+ for TaskControllerIntegrationTest)
- Build: SUCCESS

---

## Part 4: Manual Testing & Validation (5 minutes)

### 4.1 Run the API

```bash
cd src-springboot
mvn spring-boot:run
```

The API will start on `http://localhost:8080` (default Spring Boot port).

### 4.2 Test with REST Client Extension (Recommended)

If you created the `tasks.http` file:

1. Install the **REST Client** extension in VS Code (by Huachao Mao)
2. Open `src-springboot/tasks.http`
3. Click **"Send Request"** above any test scenario
4. View the response in a split pane

This is the easiest way to test your API!

### 4.3 Test with curl (Alternative)

**Valid Request**:

```bash
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{
    "title": "Complete Lab 2",
    "description": "Finish requirements to code lab",
    "priority": "HIGH",
    "dueDate": "2026-04-25T17:00:00"
  }'
```

**Expected Response**: 201 Created with Location header

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Complete Lab 2",
  "description": "Finish requirements to code lab",
  "priority": "HIGH",
  "dueDate": "2026-04-25T17:00:00",
  "completed": false,
  "createdAt": "2026-03-31T14:30:00",
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

**Expected Response**: 400 Bad Request

```json
{
  "error": "Validation Error",
  "message": "Invalid priority name: SuperUrgent. Valid values: [LOW, MEDIUM, HIGH, CRITICAL]"
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

**Expected Response**: 400 Bad Request

### 4.4 Verify Test Results

Confirm that:

- ✅ Valid requests return 201 Created with Location header
- ✅ Invalid priority returns 400 Bad Request with clear error message
- ✅ Past due dates return 400 Bad Request with validation error
- ✅ Missing/blank title returns 400 Bad Request
- ✅ Optional fields (description, dueDate) can be omitted
- ✅ Response includes all task properties with correct JSON formatting

---

## Key Learning Points

### ✅ AI-Assisted Requirements Analysis

1. **Decomposition**: Copilot helped break vague user story into concrete items
2. **Acceptance Criteria**: Generated testable, specific criteria for each item
3. **Technical Context**: Understood existing architecture and suggested appropriate Spring Boot patterns
4. **Comprehensive Coverage**: Identified edge cases and validation rules

### ✅ Full-Stack TDD Workflow

1. **Red-Green-Refactor Applied**: Tests written FIRST at every layer
2. **Domain Layer TDD**: Task entity tests → implementation → refactor
3. **Application Layer TDD**: Service tests → implementation → validation
4. **API Layer TDD**: Integration tests → controller implementation → manual testing
5. **Test Coverage**: Unit tests for logic, integration tests for full stack
6. **All Layers Tested**: Each layer validated independently with proper test pyramid

### ✅ Clean Architecture Maintained

1. **Dependencies Flow Inward**: API (Controllers) → Application (Services) → Domain (Entities)
2. **Domain Purity**: No Spring/infrastructure concerns in entities
3. **Application Logic**: Services orchestrate use cases and coordinate domain objects
4. **API Responsibility**: Only request/response mapping and HTTP concerns, no business logic

### ✅ Spring Boot Best Practices

1. **Dependency Injection**: Used constructor injection with Lombok's `@RequiredArgsConstructor`
2. **Transaction Management**: Applied `@Transactional` at service layer
3. **Exception Handling**: Used `@ExceptionHandler` for global error handling
4. **DTOs**: Separated internal domain models from API contracts using records
5. **Logging**: Structured logging with SLF4J and Lombok's `@Slf4j`

---

## Extension Exercises (If Time Permits)

### Exercise 1: Implement Item 4 (Filter by Priority)

1. Generate acceptance criteria tests
2. Implement repository filtering using JPA Specifications
3. Add API endpoint with query parameters
4. Test with multiple priority filters

### Exercise 2: Implement Item 5 (Due Soon)

1. Write tests for date range calculations
2. Create service method in Application layer
3. Add API endpoint
4. Test edge cases (timezone boundaries)

### Exercise 3: Add Update Task Endpoint

1. Generate backlog item with acceptance criteria
2. Create UpdateTaskRequest DTO
3. Implement PUT /api/tasks/{id} endpoint
4. Test validation and error cases

---

## Success Criteria

You've completed this lab successfully when:

- ✅ User story decomposed into 5 backlog items with acceptance criteria
- ✅ Priority enum created in Domain layer
- ✅ Task entity updated with Priority and dueDate
- ✅ TaskService and implementation created with tests
- ✅ POST /api/tasks endpoint working with proper validation
- ✅ All tests passing (unit and integration)
- ✅ Manual testing confirms expected behavior
- ✅ Clean Architecture principles maintained throughout

---

## Troubleshooting

### Copilot Generates Generic Backlog Items

**Problem**: Backlog items don't consider existing architecture  
**Solution**: Use `@workspace` to give context: "Given our Spring Boot Clean Architecture structure..."

### Tests Don't Cover Edge Cases

**Problem**: Missing validation tests  
**Solution**: Explicitly ask: "Generate JUnit 5 tests for all guard clauses and edge cases"

### Repository Pattern Not Working

**Problem**: TaskRepository doesn't have needed methods  
**Solution**: Ensure repository extends `JpaRepository<Task, UUID>` in domain layer

### Date Validation Issues

**Problem**: Due date validation fails unexpectedly  
**Solution**: Use `LocalDateTime.now()` consistently, consider using `Clock` for testability

### Maven Build Fails

**Problem**: Compilation errors or dependency issues  
**Solution**: Run `mvn clean install` to download dependencies and rebuild

### Spring Boot Won't Start

**Problem**: Application context fails to load  
**Solution**: Check `application.properties` for H2 configuration and verify all beans are properly annotated

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
