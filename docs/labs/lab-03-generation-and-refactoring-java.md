# Lab 3: Code Generation & Refactoring with GitHub Copilot (Java/Spring Boot)

> **💡 Also available**: [.NET version](lab-03-generation-and-refactoring.md) using C# and ASP.NET Core

**Duration**: 45 minutes  
**Learning Objectives**:

- Generate complete REST API endpoints using Copilot and context variables
- Refactor legacy code using `/refactor` command and Inline Chat
- Apply Object Calisthenics principles with AI assistance
- Use `@workspace` for understanding and modifying existing code
- Leverage Copilot Edits for multi-file refactoring

---

## 📝 Plan First with Agents: Safer, Smarter Refactoring

Before making major changes, try using Copilot (in Agent Mode) to generate a plan first. This helps you:
- Understand the scope and impact of your changes
- Catch misunderstandings or missing steps early
- Collaborate and iterate on the approach before any code is changed

**How to try it:**
- In Copilot Chat (Agent Mode), ask: "Propose a step-by-step plan to refactor LegacyTaskProcessor to add proper exception handling, logging, and follow Object Calisthenics."
- Review the plan. Edit or reorder steps as needed.
- Only then, ask Copilot (or a custom agent like `@engineer`) to implement the plan, one step at a time or all at once.

**Custom Agents Demo:**
- Use `@planner` to generate/refine the plan
- Use `@engineer` to execute the approved plan

**Reflection:**
- Did planning first catch any issues you would have missed?
- Was the implementation smoother or more predictable?

**Facilitator Tip:**
Model this workflow live, and encourage participants to always ask for a plan before executing large or multi-file changes.

---

## Overview

In this lab, you'll work with both new and existing code:

- **Part 1**: Generate new API endpoints efficiently using Copilot's context awareness
- **Part 2**: Refactor legacy code (`LegacyTaskProcessor`) to modern standards
- **Part 3**: Apply advanced refactoring patterns (Object Calisthenics)

---

## 🚀 Agent Mode Challenge: Go Beyond Ask/Plan

For this lab, try using **Agent Mode** for at least one major task (such as refactoring `LegacyTaskProcessor` or generating all CRUD endpoints at once). Agent Mode lets Copilot plan and execute multi-step, multi-file changes, and can invoke advanced tools automatically.

**How to try it:**
- Switch Copilot Chat to "Agent" mode (dropdown in chat panel)
- Describe your goal in natural language (e.g., "Refactor LegacyTaskProcessor to add proper exception handling, logging, and follow clean code principles")
- Review the plan and results, iterate as needed

**Compare:**
- What did Agent Mode do differently than Ask/Plan?
- Did it propose a plan, use multiple tools, or make changes across files?
- Was the result more complete or did it need more review?

**Facilitator Tip:**
Encourage participants to share their Agent Mode results and discuss when this approach is most effective.

---

## Prerequisites

- ✅ Completed Lab 1 (TDD) and Lab 2 (Requirements to Code)
- ✅ Familiar with Copilot Chat, Inline Chat, and slash commands
- ✅ Understanding of Clean Architecture layers
- ✅ Repository at clean state
- ✅ Java 21 and Maven installed
- ✅ Spring Boot 3.x project configured

---

## Part 1: Generate REST API Endpoints (20 minutes)

### Scenario: Complete CRUD Operations

You have the POST /api/tasks endpoint from Lab 2. Now complete the REST API with GET, PUT, and DELETE operations.

### 1.1 Understand Existing Structure with @workspace

Before generating new code, understand what exists:

```text
@workspace Show me the API controller structure in the Spring Boot project. Where are endpoints defined and how are they organized?
```

Copilot should identify:

- `src-springboot/taskmanager-api/src/main/java/com/example/taskmanager/api/controllers/TaskController.java` - Controller definitions
- `@RestController` and `@RequestMapping` patterns
- Existing POST /api/tasks endpoint
- Service layer injection pattern
- DTO classes for requests/responses

### 1.2 Generate Endpoint: GET /api/tasks (List All)

#### Step 1: Design Service Method

Ask Copilot Chat:

```text
Create a service method in TaskService to retrieve all tasks with optional filtering:
- Method: findAllTasks(TaskStatus status) where status is optional
- Return List<Task>
- Support optional filtering by TaskStatus (enum: TODO, IN_PROGRESS, DONE)
- Order results by createdAt descending
Include JUnit 5 tests using Mockito for TaskRepository
```

**Expected Output**:

- Method added to `TaskService` interface
- Implementation in `TaskServiceImpl`
- Unit test: `tests/.../ services/TaskServiceImplTest.java` (updated with new test methods)

**Note**: The domain model uses `TaskStatus` enum (TODO/IN_PROGRESS/DONE) rather than a boolean `completed` field.

#### Step 2: Implement Controller Endpoint

Use `#file` context variable:

```text
Add a GET /api/tasks endpoint in #file:src-springboot/presentation/controllers/TaskController.java that:
- Accepts optional query parameter: status (string: "TODO", "IN_PROGRESS", or "DONE")
- Calls TaskService.findAllTasks()
- Returns 200 OK with array of TaskResponse
- Handles invalid status values with 400 Bad Request
Follow the existing endpoint pattern with proper exception handling
```

**Expected Addition**:

```java
@GetMapping
public ResponseEntity<List<TaskResponse>> getAllTasks(
        @RequestParam(required = false) String status) {
    
    log.info("Received GET /api/tasks with status filter: {}", status);

    // Parse status string to TaskStatus enum if provided
    TaskStatus taskStatus = null;
    if (status != null && !status.isBlank()) {
        try {
            taskStatus = TaskStatus.valueOf(status.toUpperCase());
        } catch (IllegalArgumentException e) {
            log.warn("Invalid status value: {}", status);
            throw new IllegalArgumentException(
                "Invalid status: " + status + 
                ". Valid values: TODO, IN_PROGRESS, DONE");
        }
    }

    List<Task> tasks = taskService.findAllTasks(taskStatus);
    
    List<TaskResponse> response = tasks.stream()
        .map(task -> new TaskResponse(
            task.getId(),
            task.getTitle(),
            task.getDescription(),
            task.getPriority(),
            task.getDueDate(),
            task.isCompleted(),
            task.getCreatedAt(),
            task.getCompletedAt()
        ))
        .toList();

    log.info("Returned {} tasks", response.size());
    return ResponseEntity.ok(response);
}
```

### 1.3 Generate Endpoint: GET /api/tasks/{id} (Get by ID)

Ask Copilot:

```text
Create a service method TaskService.findTaskById(UUID id) that:
- Returns Optional<Task>
- Queries repository by ID
Include JUnit 5 unit tests

Then add GET /api/tasks/{id} endpoint in TaskController that:
- Returns 200 OK with TaskResponse if found
- Returns 404 Not Found if task doesn't exist
- Handles validation errors
```

**Key Learning**: Notice how Copilot reuses patterns from existing code (error handling, response mapping, validation).

**Expected Service Method**:

```java
@Override
public Optional<Task> findTaskById(UUID id) {
    if (id == null) {
        throw new IllegalArgumentException("Task ID cannot be null");
    }
    
    log.info("Finding task by ID: {}", id);
    return taskRepository.findById(id);
}
```

**Expected Controller Method**:

```java
@GetMapping("/{id}")
public ResponseEntity<TaskResponse> getTaskById(@PathVariable UUID id) {
    log.info("Received GET /api/tasks/{}", id);
    
    Optional<Task> taskOpt = taskService.findTaskById(id);
    
    if (taskOpt.isEmpty()) {
        log.warn("Task not found: {}", id);
        return ResponseEntity.notFound().build();
    }
    
    Task task = taskOpt.get();
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
    
    return ResponseEntity.ok(response);
}
```

### 1.4 Generate Endpoint: PUT /api/tasks/{id} (Update)

Use Inline Chat (`Ctrl+I` / `Cmd+I`):

1. Open `TaskController.java`
2. Position cursor after the GET endpoints
3. Press `Ctrl+I` / `Cmd+I`
4. Enter:

```text
Add PUT /api/tasks/{id} endpoint that:
- Accepts UpdateTaskRequest (title, description, priority, dueDate)
- Calls TaskService.updateTask(UUID id, UpdateTaskRequest request)
- Returns 200 OK with updated TaskResponse if found
- Returns 404 Not Found if task doesn't exist
- Validates all inputs
Include service method in TaskServiceImpl with unit tests
```

**Expected Service Method**:

```java
@Override
@Transactional
public Task updateTask(UUID id, UpdateTaskRequest request) {
    if (id == null) {
        throw new IllegalArgumentException("Task ID cannot be null");
    }
    if (request == null) {
        throw new IllegalArgumentException("Request cannot be null");
    }

    log.info("Updating task: {}", id);

    Task task = taskRepository.findById(id)
        .orElseThrow(() -> new EntityNotFoundException("Task not found: " + id));

    // Update fields if provided
    if (request.title() != null && !request.title().isBlank()) {
        // Assuming Task has a setTitle method or update method
        task.updateTitle(request.title());
    }
    if (request.description() != null) {
        task.updateDescription(request.description());
    }
    if (request.priority() != null) {
        Priority priority = Priority.fromString(request.priority());
        task.updatePriority(priority);
    }
    if (request.dueDate() != null) {
        task.updateDueDate(request.dueDate());
    }

    Task updatedTask = taskRepository.save(task);
    log.info("Task updated successfully: {}", id);
    
    return updatedTask;
}
```

**Expected DTO**:

```java
public record UpdateTaskRequest(
    String title,
    String description,
    String priority,
    LocalDateTime dueDate
) {}
```

### 1.5 Generate Endpoint: DELETE /api/tasks/{id}

Ask Copilot Chat:

```text
Create a service method TaskService.deleteTask(UUID id) that:
- Deletes task from repository
- Throws EntityNotFoundException if task not found
- Returns void

Add DELETE /api/tasks/{id} endpoint that:
- Returns 204 No Content on success
- Returns 404 Not Found if task doesn't exist
Include JUnit 5 unit tests for service method
```

**Expected Service Method**:

```java
@Override
@Transactional
public void deleteTask(UUID id) {
    if (id == null) {
        throw new IllegalArgumentException("Task ID cannot be null");
    }

    log.info("Deleting task: {}", id);

    if (!taskRepository.existsById(id)) {
        throw new EntityNotFoundException("Task not found: " + id);
    }

    taskRepository.deleteById(id);
    log.info("Task deleted successfully: {}", id);
}
```

**Expected Controller Method**:

```java
@DeleteMapping("/{id}")
public ResponseEntity<Void> deleteTask(@PathVariable UUID id) {
    log.info("Received DELETE /api/tasks/{}", id);
    
    try {
        taskService.deleteTask(id);
        return ResponseEntity.noContent().build();
    } catch (EntityNotFoundException e) {
        log.warn("Task not found for deletion: {}", id);
        return ResponseEntity.notFound().build();
    }
}
```

### 1.6 Run and Test

```bash
mvn clean test
cd src-springboot
mvn spring-boot:run
```

Test the full API:

```bash
# Create a task
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title": "Test Task", "priority": "MEDIUM", "dueDate": "2026-04-30T12:00:00"}'

# List all tasks
curl http://localhost:8080/api/tasks

# Get specific task (use ID from create response)
curl http://localhost:8080/api/tasks/{id}

# Update task
curl -X PUT http://localhost:8080/api/tasks/{id} \
  -H "Content-Type: application/json" \
  -d '{"title": "Updated Task", "priority": "HIGH", "dueDate": "2026-05-01T12:00:00"}'

# Delete task
curl -X DELETE http://localhost:8080/api/tasks/{id}
```

---

## Part 2: Refactor Legacy Code (15 minutes)

### Scenario: Legacy Task Processor

The repository contains `LegacyTaskProcessor` - poorly written code that needs refactoring.

### 2.1 Find the Legacy Code

Use `@workspace`:

```text
@workspace Find the LegacyTaskProcessor class in the Spring Boot project
```

**Location**: `src-springboot/taskmanager-infrastructure/src/main/java/com/example/taskmanager/infrastructure/legacy/LegacyTaskProcessor.java`

### 2.2 Analyze Current Issues

Use `/explain` on the problematic method:

1. Navigate to the `processTask` method
2. Select the entire method
3. Use Inline Chat (`Ctrl+I` or `Cmd+I`): `/explain`

Copilot should identify issues:

- ❌ Nested if statements (6+ indentation levels)
- ❌ Synchronous blocking code (`Thread.sleep()`)
- ❌ Poor error handling (exceptions swallowed with empty catch)
- ❌ No logging
- ❌ Magic numbers (1, 2, 50) and strings
- ❌ Long method (80+ lines with multiple responsibilities)
- ❌ Poor naming (`data`, `flag`, `type`, `i`)
- ❌ String concatenation in loops (inefficient)
- ❌ Mixed concerns (file I/O in processing logic)
- ❌ Not following guard clause pattern
- ❌ No use of Spring's features (@Service, dependency injection)

### 2.3 Refactor with /refactor Command

Select the entire `processTask` method and use Copilot Chat:

```text
/refactor this method to follow Clean Code and Spring Boot best practices:
1. Use guard clauses (fail fast, no nested ifs)
2. Add proper exception handling (don't swallow exceptions)
3. Add structured logging with SLF4J (@Slf4j annotation)
4. Extract smaller methods for single responsibilities
5. Replace magic numbers with enums or constants
6. Use meaningful parameter and variable names
7. Use StringBuilder for string operations in loops
8. Separate concerns: extract file I/O to a separate component
9. Follow Object Calisthenics: max 2 levels of indentation per method
10. Add @Service annotation and use dependency injection
11. Consider using CompletableFuture for async operations (optional)
Follow Spring Boot conventions and best practices
```

**Expected Improvements**:

- `@Service` annotation with `@Slf4j`
- Strongly-typed `ProcessingType` enum instead of `int type`
- Guard clauses for null/empty input (fail fast)
- Private helper methods: `processFormatting()`, `processCapitalization()`, `truncateIfNeeded()`
- Constructor injection for dependencies
- Proper exception handling with logging
- `StringBuilder` for efficient string building
- Meaningful names: `taskIdentifier`, `inputText`, `processingType`, `shouldInvertCase`

**Expected Refactored Code**:

```java
package com.example.taskmanager.infrastructure.legacy;

import lombok.RequiredArgsConstructor;
import lombok.extern.slf4j.Slf4j;
import org.springframework.stereotype.Service;

import java.util.List;

@Service
@Slf4j
@RequiredArgsConstructor
public class RefactoredTaskProcessor {

    private final TaskRepository taskRepository;
    private final TaskOutputWriter taskOutputWriter;

    public ProcessingResult processTaskBatch(List<TaskItem> tasks) {
        if (tasks == null || tasks.isEmpty()) {
            log.info("No tasks to process");
            return ProcessingResult.empty();
        }

        log.info("Starting batch processing of {} tasks", tasks.size());
        
        ProcessingResult result = new ProcessingResult();

        for (TaskItem task : tasks) {
            processSingleTask(task, result);
        }

        log.info("Batch processing completed: {} succeeded, {} failed",
                 result.getSuccessCount(), result.getFailureCount());

        return result;
    }

    private void processSingleTask(TaskItem task, ProcessingResult result) {
        if (!isTaskValid(task)) {
            log.warn("Invalid task {} skipped", task.getId());
            result.addFailure(task.getId(), "Invalid task data");
            return;
        }

        try {
            executeTaskProcessing(task);
            result.addSuccess(task.getId());
            log.info("Task {} processed successfully", task.getId());
        } catch (Exception ex) {
            log.error("Failed to process task {}", task.getId(), ex);
            result.addFailure(task.getId(), ex.getMessage());
        }
    }

    private boolean isTaskValid(TaskItem task) {
        if (task == null) {
            return false;
        }
        if (task.getTitle() == null || task.getTitle().isBlank()) {
            return false;
        }
        if (task.getPriority() < 0 || task.getPriority() > 3) {
            return false;
        }
        return true;
    }

    private void executeTaskProcessing(TaskItem task) {
        // Update task status
        task.setStatus(TaskStatus.PROCESSING);
        taskRepository.save(task);

        // Simulate processing (in real code, replace with actual business logic)
        try {
            Thread.sleep(100);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            throw new ProcessingException("Task processing interrupted", e);
        }

        // Complete task
        task.setStatus(TaskStatus.COMPLETED);
        task.setCompletedAt(LocalDateTime.now());
        taskRepository.save(task);

        // Write output if writer is configured
        if (taskOutputWriter != null) {
            taskOutputWriter.write(task);
        }
    }
}

// Supporting classes
enum ProcessingType {
    FORMAT,
    CAPITALIZE,
    TRUNCATE
}

class ProcessingResult {
    private final List<UUID> successfulIds = new ArrayList<>();
    private final Map<UUID, String> failures = new HashMap<>();

    public static ProcessingResult empty() {
        return new ProcessingResult();
    }

    public void addSuccess(UUID id) {
        successfulIds.add(id);
    }

    public void addFailure(UUID id, String reason) {
        failures.put(id, reason);
    }

    public int getSuccessCount() {
        return successfulIds.size();
    }

    public int getFailureCount() {
        return failures.size();
    }

    public List<UUID> getSuccessfulIds() {
        return List.copyOf(successfulIds);
    }

    public Map<UUID, String> getFailures() {
        return Map.copyOf(failures);
    }
}

class ProcessingException extends RuntimeException {
    public ProcessingException(String message, Throwable cause) {
        super(message, cause);
    }
}
```

### 2.4 Generate Tests for Refactored Code

Select the refactored class and use `/tests`:

```text
/tests
```

Verify generated tests cover:

- ✅ Null input returns empty result or throws exception
- ✅ Empty collection returns empty result
- ✅ Valid tasks are processed successfully
- ✅ Invalid tasks are logged and skipped
- ✅ Processing exceptions are caught and logged
- ✅ Result contains correct success/failure counts
- ✅ TaskRepository is called with correct parameters
- ✅ TaskOutputWriter is invoked for successful tasks

Run tests:

```bash
mvn test
```

---

## Part 3: Apply Object Calisthenics (10 minutes)

### Scenario: Further Code Quality Improvements

Apply Object Calisthenics rules to improve code quality.

### 3.1 Review Object Calisthenics Rules

Ask Copilot:

```text
What are the Object Calisthenics rules and how do they apply to Java/Spring Boot code?
```

Key rules:

1. Only one level of indentation per method
2. Don't use 'else' keyword (guard clauses)
3. Wrap all primitives and strings
4. First-class collections
5. One dot per line (avoid call chains)
6. Don't abbreviate names
7. Keep all entities small
8. No classes with more than two instance variables
9. No getters/setters (for domain entities - use methods that express behavior)

### 3.2 Apply: Wrap Primitives

Find places where primitive types are used directly for domain concepts.

Ask Copilot:

```text
Review the TaskItem class in the Spring Boot project. Are there primitive types that should be wrapped in value objects following DDD patterns? For example, should task status be an enum or value object?
```

**Before**:

```java
@Entity
public class TaskItem {
    private UUID id;
    private String status; // Primitive obsession
    private int priority; // Magic numbers
}
```

**After** (with Copilot assistance):

```java
@Entity
public class TaskItem {
    @EmbeddedId
    private TaskId id;
    
    @Enumerated(EnumType.STRING)
    private TaskStatus status;
    
    @Enumerated(EnumType.STRING)
    private Priority priority;
}

// Value object for TaskId
@Embeddable
public class TaskId {
    private UUID value;
    
    protected TaskId() {}
    
    public TaskId(UUID value) {
        if (value == null) {
            throw new IllegalArgumentException("TaskId cannot be null");
        }
        this.value = value;
    }
    
    public static TaskId generate() {
        return new TaskId(UUID.randomUUID());
    }
    
    public UUID getValue() {
        return value;
    }
    
    @Override
    public boolean equals(Object o) {
        if (this == o) return true;
        if (!(o instanceof TaskId taskId)) return false;
        return Objects.equals(value, taskId.value);
    }
    
    @Override
    public int hashCode() {
        return Objects.hash(value);
    }
}
```

### 3.3 Apply: First-Class Collections

Find collections that are exposed directly and wrap them.

Ask Copilot:

```text
If we have a class with a List<Task> field, how should we wrap it following Object Calisthenics and DDD patterns in Spring Boot?
```

**Before**:

```java
public class TaskList {
    private List<TaskItem> tasks;
    
    public List<TaskItem> getTasks() {
        return tasks;
    }
}
```

**After**:

```java
public class TaskCollection {
    private final List<TaskItem> tasks;

    public TaskCollection(List<TaskItem> tasks) {
        this.tasks = tasks != null ? new ArrayList<>(tasks) : new ArrayList<>();
    }

    public int size() {
        return tasks.size();
    }
    
    public List<TaskItem> asList() {
        return List.copyOf(tasks); // Immutable view
    }

    public void add(TaskItem task) {
        if (task == null) {
            throw new IllegalArgumentException("Task cannot be null");
        }
        tasks.add(task);
    }

    public Optional<TaskItem> findById(TaskId id) {
        return tasks.stream()
                .filter(t -> t.getId().equals(id))
                .findFirst();
    }

    public List<TaskItem> filterByStatus(TaskStatus status) {
        return tasks.stream()
                .filter(t -> t.getStatus() == status)
                .toList();
    }
}
```

### 3.4 Apply: No Abbreviations

Use Inline Chat to expand abbreviated names:

1. Find abbreviated variable names (e.g., `var t`, `var res`, `int cnt`)
2. Select the code
3. Inline Chat: "Expand all abbreviated variable names to be fully descriptive"

**Before**:

```java
var res = repo.findById(id);
if (res.isPresent()) {
    var t = res.get();
    var cnt = t.getItems().size();
    // ...
}
```

**After**:

```java
Optional<Task> result = repository.findById(id);
if (result.isPresent()) {
    Task task = result.get();
    int itemCount = task.getItems().size();
    // ...
}
```

### 3.5 Apply: Tell, Don't Ask

Replace getters with behavior-expressing methods.

**Before**:

```java
if (task.getStatus() == TaskStatus.COMPLETED && 
    task.getCompletedAt() != null) {
    // do something
}
```

**After**:

```java
if (task.isCompleted()) {
    // do something
}

// In Task class:
public boolean isCompleted() {
    return status == TaskStatus.COMPLETED && completedAt != null;
}
```

---

## Part 4: Multi-File Refactoring with Copilot Edits (Optional, if time)

### Scenario: Rename Across Multiple Files

Use Copilot Edits for cross-cutting changes.

### 4.1 Open Copilot Edits

1. Open Command Palette (`Ctrl+Shift+P` / `Cmd+Shift+P`)
2. Search for "Copilot Edits: Open"
3. Or use dedicated Copilot Edits panel in sidebar

### 4.2 Add Files to Working Set

Add related files:

- `src-springboot/taskmanager-domain/src/main/java/com/example/taskmanager/domain/tasks/Task.java`
- `src-springboot/taskmanager-api/src/main/java/com/example/taskmanager/api/dto/CreateTaskRequest.java`
- `src-springboot/taskmanager-application/src/main/java/com/example/taskmanager/application/services/TaskService.java`
- `src-springboot/taskmanager-api/src/main/java/com/example/taskmanager/api/controllers/TaskController.java`
- `src-springboot/taskmanager-application/src/test/java/com/example/taskmanager/application/services/TaskServiceTest.java`

### 4.3 Describe Change

In the Copilot Edits panel:

```text
Rename the "title" field to "name" across all files in the working set. Update:
- Entity field
- DTO field
- All references in services
- All references in controllers
- All test assertions
Ensure consistency across the entire codebase
```

### 4.4 Review Proposed Changes

Copilot will show:

- All files that will be modified
- Exact changes in each file
- Side-by-side diff view

### 4.5 Accept or Reject

- Review each change carefully
- Accept all if changes look correct
- Or accept/reject individual file changes
- Run tests after applying: `mvn test`

---

## Key Learning Points

### ✅ Context-Aware Code Generation

1. **@workspace**: Understanding existing structure before generating
2. **#file**: Referencing specific files for consistent patterns
3. **#selection**: Refactoring specific code sections
4. **Pattern Reuse**: Copilot learned patterns from existing endpoints

### ✅ Effective Refactoring Workflow

1. **/explain**: Understand code before changing it
2. **/refactor**: Automated refactoring with specific goals
3. **/tests**: Generate tests for refactored code
4. **Iterative**: Refactor in small steps, run tests frequently

### ✅ Code Quality Improvements

1. **Guard Clauses**: Early returns reduce indentation
2. **Proper Exception Handling**: Don't swallow exceptions, log them
3. **Logging**: SLF4J with @Slf4j provides observability
4. **Single Responsibility**: Extracted methods with clear purposes
5. **Object Calisthenics**: Advanced quality constraints
6. **Spring Boot Patterns**: @Service, dependency injection, @Transactional

### ✅ Multi-File Editing

1. **Copilot Edits**: Consistent changes across multiple files
2. **Working Set**: Explicitly define scope of changes
3. **Review Process**: Always review AI-proposed changes
4. **Safe Refactoring**: Tests validate behavior preservation

---

## Extension Exercises (If Time Permits)

### Exercise 1: Add Pagination

Refactor GET /api/tasks to support pagination using Spring Data's `Pageable`.
Use Copilot to:

1. Add `Pageable` parameter to service method
2. Update repository method to return `Page<Task>`
3. Modify controller to accept `@RequestParam` page and size
4. Update tests

Example:

```java
@GetMapping
public ResponseEntity<Page<TaskResponse>> getAllTasks(
        @RequestParam(required = false) String status,
        @RequestParam(defaultValue = "0") int page,
        @RequestParam(defaultValue = "20") int size) {
    
    Pageable pageable = PageRequest.of(page, size, Sort.by("createdAt").descending());
    // ...
}
```

### Exercise 2: Add Sorting

Add sorting support to GET /api/tasks using Spring Data's `Sort`.
Valid sort fields: title, priority, dueDate, createdAt

### Exercise 3: Extract Response Mapper

Create a dedicated `TaskResponseMapper` component for converting Task entities to TaskResponse DTOs.
Use Copilot Edits to update all endpoints to use the mapper.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ Full CRUD API endpoints implemented (POST, GET, GET by ID, PUT, DELETE)
- ✅ All endpoints follow consistent @RestController patterns
- ✅ LegacyTaskProcessor refactored to modern Spring Boot standards
- ✅ Refactored code follows Object Calisthenics principles
- ✅ Guard clauses used instead of nested ifs
- ✅ Proper exception handling (no swallowed exceptions)
- ✅ SLF4J structured logging added
- ✅ All tests passing
- ✅ Code is clean, readable, and maintainable
- ✅ Spring Boot best practices applied (@Service, @Transactional, DI)

---

## Troubleshooting

### Copilot Generates Inconsistent Patterns

**Problem**: New endpoints don't match existing style  
**Solution**: Use `#file` to reference existing controller, explicitly state "Follow the existing Spring Boot pattern"

### Refactoring Breaks Tests

**Problem**: Tests fail after refactoring  
**Solution**: This is OK! Update tests to match new behavior. Use `/tests` to regenerate tests.

### Too Many Changes at Once

**Problem**: Copilot suggests massive refactoring  
**Solution**: Break into smaller steps. Refactor one method at a time. Run tests after each change.

### Multi-File Edit Misses Files

**Problem**: Copilot Edits doesn't update all references  
**Solution**: Use IntelliJ's/VS Code's built-in "Rename" (Shift+F6 in IntelliJ) for simple renames. Use Copilot Edits for semantic changes.

### Maven Build Fails

**Problem**: Compilation errors after refactoring  
**Solution**: Run `mvn clean compile` to rebuild. Check for missing imports or incorrect annotations.

---

## Next Steps

Move on to [**Lab 4: Testing, Documentation & Workflow**](lab-04-testing-documentation-workflow.md) where you'll:

- Generate comprehensive test suites with `/tests`
- Create documentation with `/doc`
- Write Conventional Commit messages
- Draft PR descriptions with `@workspace`

---

## Additional Resources

### Spring Boot & Java
- [Spring Boot Best Practices](https://docs.spring.io/spring-boot/reference/features/developing-auto-configuration.html)
- [Spring Data JPA Query Methods](https://docs.spring.io/spring-data/jpa/docs/current/reference/html/#jpa.query-methods)
- [SLF4J Documentation](http://www.slf4j.org/manual.html)
- [Lombok Annotations](https://projectlombok.org/features/all)

### Clean Code & Patterns
- [Object Calisthenics](https://williamdurand.fr/2013/06/03/object-calisthenics/)
- [Refactoring Techniques](https://refactoring.guru/refactoring/techniques)
- [Clean Code Principles](https://www.baeldung.com/java-clean-code)
- [DDD in Spring Boot](https://reflectoring.io/spring-boot-clean-architecture/)

### Cross-Reference
- [.NET version of this lab](lab-03-generation-and-refactoring.md) - Compare patterns and approaches
- [Pattern Translation Guide](../design/pattern-translation-guide.md) - .NET ↔ Spring Boot equivalencies
