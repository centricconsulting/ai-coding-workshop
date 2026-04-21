# Lab 4: Testing, Documentation & Workflow with GitHub Copilot (Java/Spring Boot)

**Duration**: 15 minutes  
**Learning Objectives**:

- Generate comprehensive test suites using `/tests` command
- Create documentation with `/doc` command
- Write Conventional Commit messages with AI assistance
- Draft PR descriptions using `@workspace` for full context
- Integrate Copilot into complete development workflow

---

## Overview

This lab brings together everything you've learned by focusing on the "glue" activities that complete the development lifecycle:

1. **Testing** - Generate comprehensive test coverage
2. **Documentation** - Create clear, maintainable docs
3. **Version Control** - Write meaningful commit messages
4. **Code Review** - Prepare thorough PR descriptions

These activities are often rushed or skipped, but Copilot makes them fast and consistent.

---

## Prerequisites

- ✅ Completed Labs 1, 2, and 3 (Java versions)
- ✅ Working Task Manager API with CRUD operations
- ✅ Git initialized with commits from previous labs
- ✅ Familiar with all Copilot features (chat, inline chat, slash commands, context variables)

---

## Part 1: Comprehensive Test Generation (5 minutes)

### Scenario: Increase Test Coverage

You have basic tests from TDD, but need comprehensive coverage including edge cases, integration tests, and error scenarios.

### 1.1 Generate Unit Tests for a Service

#### Step 1: Select Target Method

Open `src-springboot/taskmanager-application/src/main/java/com/example/taskmanager/application/service/CreateTaskService.java` and select the `createTask` method.

#### Step 2: Use /tests Command

With the method selected, open Copilot Chat (`Ctrl+Alt+I` / `Cmd+Shift+I`) and enter:

``` text
/tests
```

Or use Inline Chat (`Ctrl+I` / `Cmd+I`):

``` text
/tests
```

#### Step 3: Review Generated Tests

Copilot should generate comprehensive tests covering:

```java
package com.example.taskmanager.application.service;

import com.example.taskmanager.application.dto.CreateTaskRequest;
import com.example.taskmanager.domain.entity.Task;
import com.example.taskmanager.domain.repository.TaskRepository;
import com.example.taskmanager.domain.valueobject.Priority;
import com.example.taskmanager.domain.valueobject.TaskStatus;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.NullAndEmptySource;
import org.junit.jupiter.params.provider.ValueSource;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.slf4j.Logger;

import java.time.LocalDateTime;
import java.util.UUID;

import static org.assertj.core.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
class CreateTaskServiceTest {

    @Mock
    private TaskRepository taskRepository;

    @Mock
    private Logger logger;

    @InjectMocks
    private CreateTaskService sut;

    private CreateTaskRequest validRequest;

    @BeforeEach
    void setUp() {
        validRequest = CreateTaskRequest.builder()
                .title("Test Task")
                .description("Test Description")
                .priority("HIGH")
                .dueDate(LocalDateTime.now().plusDays(7))
                .build();
    }

    @Test
    void createTask_WithValidRequest_CreatesTask() {
        // Arrange
        Task expectedTask = Task.builder()
                .id(UUID.randomUUID())
                .title("Test Task")
                .description("Test Description")
                .priority(Priority.HIGH)
                .status(TaskStatus.TODO)
                .dueDate(validRequest.getDueDate())
                .createdAt(LocalDateTime.now())
                .build();

        when(taskRepository.save(any(Task.class))).thenReturn(expectedTask);

        // Act
        Task result = sut.createTask(validRequest);

        // Assert
        assertThat(result).isNotNull();
        assertThat(result.getTitle()).isEqualTo("Test Task");
        assertThat(result.getPriority()).isEqualTo(Priority.HIGH);
        assertThat(result.getStatus()).isEqualTo(TaskStatus.TODO);
        verify(taskRepository, times(1)).save(any(Task.class));
    }

    @Test
    void createTask_WithNullRequest_ThrowsIllegalArgumentException() {
        // Act & Assert
        assertThatThrownBy(() -> sut.createTask(null))
                .isInstanceOf(IllegalArgumentException.class)
                .hasMessageContaining("request cannot be null");
    }

    @ParameterizedTest
    @NullAndEmptySource
    @ValueSource(strings = {"   ", "\t", "\n"})
    void createTask_WithInvalidTitle_ThrowsIllegalArgumentException(String invalidTitle) {
        // Arrange
        validRequest.setTitle(invalidTitle);

        // Act & Assert
        assertThatThrownBy(() -> sut.createTask(validRequest))
                .isInstanceOf(IllegalArgumentException.class)
                .hasMessageContaining("title");
    }

    @ParameterizedTest
    @ValueSource(strings = {"INVALID", "SUPER_URGENT", "NONE", ""})
    void createTask_WithInvalidPriority_ThrowsIllegalArgumentException(String invalidPriority) {
        // Arrange
        validRequest.setPriority(invalidPriority);

        // Act & Assert
        assertThatThrownBy(() -> sut.createTask(validRequest))
                .isInstanceOf(IllegalArgumentException.class)
                .hasMessageContaining("priority");
    }

    @Test
    void createTask_WithPastDueDate_ThrowsIllegalArgumentException() {
        // Arrange
        validRequest.setDueDate(LocalDateTime.now().minusDays(1));

        // Act & Assert
        assertThatThrownBy(() -> sut.createTask(validRequest))
                .isInstanceOf(IllegalArgumentException.class)
                .hasMessageContaining("due date cannot be in the past");
    }

    @Test
    void createTask_WithNullDueDate_CreatesTaskSuccessfully() {
        // Arrange
        validRequest.setDueDate(null);
        Task expectedTask = Task.builder()
                .id(UUID.randomUUID())
                .title("Test Task")
                .priority(Priority.HIGH)
                .status(TaskStatus.TODO)
                .dueDate(null)
                .createdAt(LocalDateTime.now())
                .build();

        when(taskRepository.save(any(Task.class))).thenReturn(expectedTask);

        // Act
        Task result = sut.createTask(validRequest);

        // Assert
        assertThat(result).isNotNull();
        assertThat(result.getDueDate()).isNull();
    }

    @Test
    void createTask_LogsInformationMessages() {
        // Arrange
        when(taskRepository.save(any(Task.class))).thenReturn(any(Task.class));

        // Act
        sut.createTask(validRequest);

        // Assert
        verify(logger, atLeast(2)).info(anyString(), any());
    }

    @Test
    void createTask_WhenRepositoryThrowsException_PropagatesException() {
        // Arrange
        when(taskRepository.save(any(Task.class)))
                .thenThrow(new RuntimeException("Database error"));

        // Act & Assert
        assertThatThrownBy(() -> sut.createTask(validRequest))
                .isInstanceOf(RuntimeException.class)
                .hasMessageContaining("Database error");
    }

    @Test
    void createTask_SetsCorrectDefaultStatus() {
        // Arrange
        Task capturedTask = Task.builder().build();
        when(taskRepository.save(any(Task.class))).thenAnswer(invocation -> {
            Task task = invocation.getArgument(0);
            return task;
        });

        // Act
        Task result = sut.createTask(validRequest);

        // Assert
        assertThat(result.getStatus()).isEqualTo(TaskStatus.TODO);
    }
}
```

### 1.2 Generate Integration Tests

For REST API endpoints, generate integration tests:

``` text
Create integration tests for the POST /api/tasks endpoint using @SpringBootTest and MockMvc. Test:
- Valid request returns 201 Created
- Invalid priority returns 400 Bad Request with ProblemDetail
- Past due date returns 400 Bad Request
- Missing required fields returns 400 Bad Request
- Response body contains all expected fields
Use JUnit 5, AssertJ, and realistic test data
```

**Expected Output** - `src-springboot/taskmanager-api/src/test/java/com/example/taskmanager/api/TaskControllerIntegrationTest.java`:

```java
package com.example.taskmanager.api;

import com.example.taskmanager.application.dto.CreateTaskRequest;
import com.example.taskmanager.application.dto.TaskResponse;
import com.fasterxml.jackson.databind.ObjectMapper;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.params.ParameterizedTest;
import org.junit.jupiter.params.provider.ValueSource;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.http.MediaType;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.MvcResult;

import java.time.LocalDateTime;

import static org.assertj.core.api.Assertions.assertThat;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.post;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.*;

@SpringBootTest
@AutoConfigureMockMvc
class TaskControllerIntegrationTest {

    @Autowired
    private MockMvc mockMvc;

    @Autowired
    private ObjectMapper objectMapper;

    @Test
    void postTask_WithValidRequest_Returns201Created() throws Exception {
        // Arrange
        CreateTaskRequest request = CreateTaskRequest.builder()
                .title("Integration Test Task")
                .description("Testing POST endpoint")
                .priority("HIGH")
                .dueDate(LocalDateTime.now().plusDays(7))
                .build();

        // Act
        MvcResult result = mockMvc.perform(post("/api/tasks")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isCreated())
                .andExpect(jsonPath("$.title").value("Integration Test Task"))
                .andExpect(jsonPath("$.priority").value("HIGH"))
                .andExpect(jsonPath("$.status").value("TODO"))
                .andExpect(jsonPath("$.id").exists())
                .andExpect(jsonPath("$.createdAt").exists())
                .andReturn();

        // Assert
        String responseBody = result.getResponse().getContentAsString();
        TaskResponse taskResponse = objectMapper.readValue(responseBody, TaskResponse.class);
        assertThat(taskResponse).isNotNull();
        assertThat(taskResponse.getTitle()).isEqualTo("Integration Test Task");
        assertThat(taskResponse.getPriority()).isEqualTo("HIGH");
    }

    @ParameterizedTest
    @ValueSource(strings = {"INVALID", "SUPER_URGENT", "NONE"})
    void postTask_WithInvalidPriority_Returns400BadRequest(String invalidPriority) throws Exception {
        // Arrange
        CreateTaskRequest request = CreateTaskRequest.builder()
                .title("Test Task")
                .priority(invalidPriority)
                .dueDate(LocalDateTime.now().plusDays(1))
                .build();

        // Act & Assert
        mockMvc.perform(post("/api/tasks")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isBadRequest())
                .andExpect(jsonPath("$.type").exists())
                .andExpect(jsonPath("$.title").value("Bad Request"))
                .andExpect(jsonPath("$.status").value(400))
                .andExpect(jsonPath("$.detail").value(org.hamcrest.Matchers.containsStringIgnoringCase("priority")));
    }

    @Test
    void postTask_WithPastDueDate_Returns400BadRequest() throws Exception {
        // Arrange
        CreateTaskRequest request = CreateTaskRequest.builder()
                .title("Test Task")
                .priority("MEDIUM")
                .dueDate(LocalDateTime.now().minusDays(7))
                .build();

        // Act & Assert
        mockMvc.perform(post("/api/tasks")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isBadRequest())
                .andExpect(jsonPath("$.detail").value(org.hamcrest.Matchers.containsStringIgnoringCase("due date")));
    }

    @Test
    void postTask_WithMissingTitle_Returns400BadRequest() throws Exception {
        // Arrange
        CreateTaskRequest request = CreateTaskRequest.builder()
                .priority("LOW")
                .dueDate(LocalDateTime.now().plusDays(1))
                .build();

        // Act & Assert
        mockMvc.perform(post("/api/tasks")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content(objectMapper.writeValueAsString(request)))
                .andExpect(status().isBadRequest())
                .andExpect(jsonPath("$.detail").value(org.hamcrest.Matchers.containsStringIgnoringCase("title")));
    }

    @Test
    void postTask_WithEmptyBody_Returns400BadRequest() throws Exception {
        // Act & Assert
        mockMvc.perform(post("/api/tasks")
                        .contentType(MediaType.APPLICATION_JSON)
                        .content("{}"))
                .andExpect(status().isBadRequest());
    }
}
```

### 1.3 Run Complete Test Suite

```bash
./mvnw test
```

Or if using Gradle:

```bash
./gradlew test
```

Verify coverage includes:

- ✅ Happy path tests
- ✅ Guard clause tests
- ✅ Edge case tests (nulls, empty strings, boundaries)
- ✅ Validation tests
- ✅ Error handling tests
- ✅ Logging verification tests
- ✅ Integration tests

---

## Part 2: Generate Documentation (3 minutes)

### Scenario: Document Your API

Create comprehensive documentation for the Task Manager API.

### 2.1 Add Javadoc to Classes

#### Step 1: Select a Class

Open `src-springboot/taskmanager-application/src/main/java/com/example/taskmanager/application/service/CreateTaskService.java` and select the class declaration.

#### Step 2: Use /doc Command

With the class selected, use Inline Chat (`Ctrl+I`):

``` text
/doc
```

**Expected Output**:

```java
package com.example.taskmanager.application.service;

import com.example.taskmanager.application.dto.CreateTaskRequest;
import com.example.taskmanager.domain.entity.Task;
import com.example.taskmanager.domain.repository.TaskRepository;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

/**
 * Service for handling task creation in the task management system.
 * Validates input, creates task entities, and persists them to the repository.
 * 
 * <p>This service ensures all business rules are enforced including:
 * <ul>
 *   <li>Title validation (non-empty)</li>
 *   <li>Priority validation (must be valid enum value)</li>
 *   <li>Due date validation (must be in the future if provided)</li>
 *   <li>Default status assignment (TODO)</li>
 * </ul>
 * 
 * @author Task Manager Team
 * @version 1.0
 * @since 1.0
 */
@Service
public class CreateTaskService {

    private static final Logger logger = LoggerFactory.getLogger(CreateTaskService.class);

    private final TaskRepository taskRepository;

    /**
     * Constructs a new CreateTaskService with the specified repository.
     *
     * @param taskRepository the repository for persisting task entities
     * @throws IllegalArgumentException if taskRepository is null
     */
    public CreateTaskService(TaskRepository taskRepository) {
        if (taskRepository == null) {
            throw new IllegalArgumentException("taskRepository cannot be null");
        }
        this.taskRepository = taskRepository;
    }

    /**
     * Creates a new task based on the provided request.
     * 
     * <p>This method validates the request, constructs a task entity,
     * persists it to the repository, and returns the created task.
     *
     * @param request the request containing task creation details
     * @return the newly created task entity with generated ID and timestamps
     * @throws IllegalArgumentException if request is null, contains invalid data
     *         (empty title, invalid priority, or past due date)
     * @throws RuntimeException if repository operation fails
     */
    public Task createTask(CreateTaskRequest request) {
        if (request == null) {
            logger.error("Attempted to create task with null request");
            throw new IllegalArgumentException("request cannot be null");
        }

        logger.info("Creating task with title: {}", request.getTitle());

        // Validate title
        if (request.getTitle() == null || request.getTitle().trim().isEmpty()) {
            logger.error("Task creation failed: invalid title");
            throw new IllegalArgumentException("title cannot be null or empty");
        }

        // ... rest of implementation
    }
}
```

### 2.2 Generate API Documentation (README)

Ask Copilot Chat:

```
Create an API documentation section for #file:README.md that documents all the Task Manager API endpoints (POST, GET, GET by ID, PUT, DELETE). Include:
- Endpoint URL
- HTTP method
- Request body examples
- Response examples
- Status codes
- Error responses
Format as Markdown
```

**Expected Output** - Added to `README.md`:

````markdown
## API Documentation

### Base URL

`http://localhost:8080`

### Endpoints

#### 1. Create Task
Creates a new task with priority and optional due date.

**Endpoint**: `POST /api/tasks`

**Request Body:**
```json
{
    "title": "Complete project documentation",
    "description": "Write comprehensive API documentation",
    "priority": "HIGH",
    "dueDate": "2025-10-30T17:00:00"
}
```

**Success Response** (201 Created):
```json
{
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Complete project documentation",
    "description": "Write comprehensive API documentation",
    "priority": "HIGH",
    "status": "TODO",
    "dueDate": "2025-10-30T17:00:00",
    "createdAt": "2025-10-20T10:30:00"
}
```

**Note**: The response uses `status` field (enum: TODO, IN_PROGRESS, DONE, CANCELLED).

**Error Responses:**
```json
{
    "type": "about:blank",
    "title": "Bad Request",
    "status": 400,
    "detail": "Invalid priority: SUPER_URGENT",
    "instance": "/api/tasks"
}
```
Other possible errors:
- `400 Bad Request` - Invalid priority or past due date
- `500 Internal Server Error` - Server error

---

#### 2. List All Tasks

Retrieves all tasks, optionally filtered by completion status.

**Endpoint**: `GET /api/tasks`

**Query Parameters** (optional):
- `completed` (boolean): Filter by completion status

**Examples:**
```
GET /api/tasks
GET /api/tasks?completed=true
GET /api/tasks?completed=false
```

**Success Response** (200 OK):
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "title": "Complete project documentation",
    "priority": "HIGH",
    "status": "TODO",
    "createdAt": "2025-10-20T10:30:00"
  },
  {
    "id": "8bd42f99-1234-5678-abcd-ef0123456789",
    "title": "Review pull requests",
    "priority": "MEDIUM",
    "status": "IN_PROGRESS",
    "createdAt": "2025-10-20T11:15:00"
  }
]
```

---

#### 3. Get Task by ID

Retrieves a specific task by its unique identifier.

**Endpoint**: `GET /api/tasks/{id}`

**Path Parameters**:

- `id` (required): Task unique identifier (UUID)

**Success Response** (200 OK):

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Complete project documentation",
  "description": "Write comprehensive API documentation",
  "priority": "HIGH",
  "status": "TODO",
  "dueDate": "2025-10-30T17:00:00",
  "createdAt": "2025-10-20T10:30:00"
}
```

**Error Responses**:

- `404 Not Found` - Task with specified ID does not exist

---

#### 4. Update Task

Updates an existing task's properties.

**Endpoint**: `PUT /api/tasks/{id}`

**Request Body**:

```json
{
  "title": "Updated task title",
  "description": "Updated description",
  "priority": "CRITICAL",
  "status": "DONE",
  "dueDate": "2025-11-01T17:00:00"
}
```

**Success Response** (200 OK):

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "title": "Updated task title",
  "description": "Updated description",
  "priority": "CRITICAL",
  "status": "DONE",
  "dueDate": "2025-11-01T17:00:00",
  "createdAt": "2025-10-20T10:30:00"
}
```

**Error Responses**:

- `400 Bad Request` - Invalid data
- `404 Not Found` - Task not found

---

#### 5. Delete Task

Permanently deletes a task.

**Endpoint**: `DELETE /api/tasks/{id}`

**Success Response** (204 No Content)

**Error Responses**:

- `404 Not Found` - Task not found

---

### Priority Values

Valid priority values for task creation and updates:

- `LOW` (0)
- `MEDIUM` (1)
- `HIGH` (2)
- `CRITICAL` (3)

### Status Values

Valid status values:

- `TODO` - Task not started
- `IN_PROGRESS` - Task currently being worked on
- `DONE` - Task completed
- `CANCELLED` - Task cancelled

### Error Response Format

All error responses follow [RFC 7807 Problem Details](https://datatracker.ietf.org/doc/html/rfc7807) format using Spring's ProblemDetail:

```json
{
    "type": "about:blank",
    "title": "Bad Request",
    "status": 400,
    "detail": "Invalid priority: SUPER_URGENT",
    "instance": "/api/tasks"
}
```
````

### 2.3 Generate Architecture Documentation (ADR)

Ask Copilot:

```text
Create an Architecture Decision Record (ADR) in docs/adr/0001-use-clean-architecture.md documenting why we chose Clean Architecture for this Task Manager application. Include:
- Context
- Decision
- Consequences (positive and negative)
- Alternatives considered
Follow the ADR template format
```

---

## Part 3: Write Conventional Commit Messages (3 minutes)

### Scenario: Commit Your Changes

You've made significant changes across multiple files. Write meaningful commit messages following Conventional Commits format.

### 3.1 Stage Changes

```bash
git add src-springboot/taskmanager-api/src/main/java/com/example/taskmanager/api/
git add src-springboot/taskmanager-application/src/main/java/com/example/taskmanager/application/query/
git add src-springboot/taskmanager-api/src/test/java/com/example/taskmanager/api/
```

### 3.2 Generate Commit Message

In Copilot Chat:

```text
Write a Conventional Commit message for the staged changes. Include a concise subject line (<=72 chars) and a detailed body explaining what was added.
```

**Expected Output**:

```text
feat(api): add GET endpoints for task retrieval

- Add GET /api/tasks endpoint with optional completed filter
- Add GET /api/tasks/{id} endpoint for single task retrieval
- Implement GetTasksQuery and GetTaskByIdQuery handlers
- Add integration tests using @SpringBootTest and MockMvc
- Update endpoint documentation in README

These endpoints complete the CRUD API for task management,
allowing clients to retrieve tasks individually or as a list
with optional filtering by completion status.
```

### 3.3 Commit with Generated Message

```bash
git commit -m "feat(api): add GET endpoints for task retrieval

- Add GET /api/tasks endpoint with optional completed filter
- Add GET /api/tasks/{id} endpoint for single task retrieval
- Implement GetTasksQuery and GetTaskByIdQuery handlers
- Add integration tests using @SpringBootTest and MockMvc
- Update endpoint documentation in README

These endpoints complete the CRUD API for task management,
allowing clients to retrieve tasks individually or as a list
with optional filtering by completion status."
```

### 3.4 Practice: Stage More Changes

Stage the refactored legacy code:

```bash
git add src-springboot/taskmanager-infrastructure/src/main/java/com/example/taskmanager/infrastructure/legacy/LegacyTaskProcessor.java
git add src-springboot/taskmanager-infrastructure/src/test/java/com/example/taskmanager/infrastructure/legacy/
```

Ask Copilot:

```text
Write a Conventional Commit message for refactoring LegacyTaskProcessor. Include details about what was improved (extracted methods, guard clauses, logging, removed nested loops).
```

**Expected Output**:

```text
refactor(legacy): modernize LegacyTaskProcessor with clean code principles

- Extract processTask and validateTask methods for single responsibility
- Replace nested if statements with guard clauses
- Add structured logging with SLF4J throughout
- Remove nested for loops and replace with Stream API
- Improve error handling with specific exception types
- Add comprehensive unit tests with Mockito

Reduces cyclomatic complexity from 15 to 4 per method.
Follows Object Calisthenics: one level of indentation per method.
```

Commit:

```bash
git commit -F- <<'EOF'
refactor(legacy): modernize LegacyTaskProcessor with clean code principles

- Extract processTask and validateTask methods for single responsibility
- Replace nested if statements with guard clauses
- Add structured logging with SLF4J throughout
- Remove nested for loops and replace with Stream API
- Improve error handling with specific exception types
- Add comprehensive unit tests with Mockito

Reduces cyclomatic complexity from 15 to 4 per method.
Follows Object Calisthenics: one level of indentation per method.
EOF
```

---

## Part 4: Draft Pull Request Descriptions (4 minutes)

### Scenario: Prepare for Code Review

You've completed the workshop implementation. Create a comprehensive PR description for review.

### 4.1 Generate PR Description with @workspace

In Copilot Chat:

```text
@workspace Draft a Pull Request description for all the changes made in this branch. Include:
- Summary of changes
- What was implemented (features, refactoring, tests)
- Intent and motivation
- Testing performed
- Breaking changes (if any)
- Checklist for reviewers
Use Markdown formatting
```

**Expected Output**:

````markdown
# Task Manager API - Complete Implementation (Spring Boot)

## Summary
This PR implements a complete CRUD API for task management following Clean Architecture and DDD patterns, with comprehensive test coverage and documentation.

## Changes

### Features Implemented
- ✅ **Create Task** (`POST /api/tasks`) - Create tasks with priority and due date
- ✅ **List Tasks** (`GET /api/tasks`) - Retrieve all tasks with optional completion filter
- ✅ **Get Task by ID** (`GET /api/tasks/{id}`) - Retrieve single task
- ✅ **Update Task** (`PUT /api/tasks/{id}`) - Update task properties
- ✅ **Delete Task** (`DELETE /api/tasks/{id}`) - Remove task

### Architecture & Design
- Implemented Clean Architecture layers (Domain, Application, Infrastructure, API)
- Applied DDD patterns: Aggregates, Value Objects, Factory Methods
- CQRS pattern for commands and queries
- Dependency injection with Spring Framework
- Proper error handling with Spring's ProblemDetail (RFC 7807)

### Code Quality Improvements
- Refactored `LegacyTaskProcessor` to modern standards:
  - Extracted methods for single responsibility
  - Guard clauses (no nested ifs)
  - Structured logging with SLF4J
  - Stream API instead of nested loops
  - Reduced cyclomatic complexity from 15 to 4
- Applied Object Calisthenics principles:
  - One level of indentation per method
  - No else keyword
  - Wrapped primitives in value objects
  - Descriptive naming (no abbreviations)

### Testing
- **Unit Tests**: 45+ tests covering all services, commands, queries, and domain logic
- **Integration Tests**: 12+ tests for all REST endpoints
- Test framework: JUnit 5 with Mockito for mocking
- Test organization: Feature-based with clear naming conventions
- Assertions: AssertJ for fluent assertions
- Coverage: ~92% code coverage (excluding infrastructure)

### Documentation
- Javadoc for all public APIs
- Comprehensive README with:
  - API endpoint documentation
  - Request/response examples
  - Error response formats
- Architecture Decision Records (ADRs) for key decisions
- Inline comments for complex business logic

## Intent & Motivation
This implementation serves as a reference for:
- AI-assisted development workflow with GitHub Copilot
- Clean Architecture in Spring Boot 3
- TDD practices (Red-Green-Refactor)
- DDD patterns in practice
- Modern Java conventions and best practices

## Testing Performed

### Automated Tests
```bash
./mvnw clean test    # ✅ Success, 0 failures
./mvnw verify        # ✅ All integration tests passed
```

### Manual API Testing
All endpoints tested with curl:
- ✅ Valid requests return correct responses
- ✅ Invalid data returns 400 Bad Request with ProblemDetail
- ✅ Not found scenarios return 404
- ✅ Server errors return 500 with ProblemDetail

### Performance
- Average response time: <50ms for CRUD operations
- No memory leaks detected in load testing
- Database connection pooling configured appropriately

## Breaking Changes
⚠️ **None** - This is initial implementation

## Migration Required
None - Uses in-memory H2 database for workshop purposes

## Reviewer Checklist
Please verify:
- [ ] All tests pass (`./mvnw test`)
- [ ] Build succeeds with no warnings (`./mvnw clean package`)
- [ ] Code follows Spring Boot conventions (automatically loaded from `.github/instructions/`)
- [ ] Clean Architecture dependencies respected (no circular references)
- [ ] Domain logic stays in Domain layer (no business logic in API/Infrastructure)
- [ ] All public APIs have Javadoc
- [ ] Error handling uses proper status codes and ProblemDetail
- [ ] Structured logging with SLF4J (no System.out.println)
- [ ] Guard clauses for parameter validation
- [ ] Tests organized by feature with clear naming
- [ ] No Spring annotations in Domain layer

## Related Issues
Closes #1 - Implement Task Manager CRUD API  
Closes #2 - Refactor legacy code  
Closes #3 - Add comprehensive test coverage  

## Screenshots
N/A - API only, no UI

## Deployment Notes
- Requires Java 21 or higher
- Uses H2 in-memory database (no external database required)
- Default port: 8080
- Spring profiles: `dev`, `test`, `prod`

## Next Steps (Future Work)
- [ ] Add database persistence (PostgreSQL or MySQL)
- [ ] Implement Spring Security for authentication/authorization
- [ ] Add pagination and advanced filtering with Spring Data JPA Specifications
- [ ] Implement task notification service with Spring Events
- [ ] Add distributed tracing with Micrometer and Zipkin
- [ ] Generate OpenAPI/Swagger documentation with SpringDoc
````

### 4.2 Review and Refine

Review the generated PR description and adjust:

- Add specific issue numbers
- Include actual test counts
- Add screenshots if you created a UI
- Highlight any specific areas needing review

---

## Key Learning Points

### ✅ Testing Best Practices

1. **/tests Command**: Generates comprehensive test suites instantly
2. **Test Coverage**: Happy path, edge cases, error conditions
3. **Test Organization**: Clear package structure, descriptive test names
4. **Integration Tests**: @SpringBootTest with MockMvc for full API testing
5. **AssertJ**: Fluent assertions for readable tests
6. **Mockito**: Clean mocking with @Mock and @InjectMocks

### ✅ Documentation Efficiency

1. **/doc Command**: Javadoc generated from code context
2. **API Docs**: Clear examples with request/response formats
3. **Architecture Docs**: ADRs document important decisions
4. **Consistency**: AI ensures consistent documentation style

### ✅ Version Control Quality

1. **Conventional Commits**: Structured, parsable commit messages
2. **Semantic Commits**: Type, scope, description format
3. **Detailed Bodies**: Explain what, why, and how
4. **Changelog Ready**: Commits can generate CHANGELOG.md automatically

### ✅ Code Review Preparation

1. **@workspace Context**: Full codebase understanding for PR description
2. **Comprehensive PRs**: All changes documented and explained
3. **Reviewer Checklist**: Clear acceptance criteria
4. **Impact Analysis**: Breaking changes, migrations, deployment notes

---

## Extension Exercises (If Time Permits)

### Exercise 1: Generate CHANGELOG.md

Ask Copilot to generate a CHANGELOG.md file from your commit history:

```text
Generate a CHANGELOG.md file based on the git commit history. Group by version, follow Keep a Changelog format.
```

### Exercise 2: Create Contributing Guidelines

Generate CONTRIBUTING.md with guidelines for contributors:

```text
Create a CONTRIBUTING.md file that explains:
- How to set up the development environment
- Coding conventions (reference .github/instructions/ for context-aware standards)
- Testing requirements
- PR process
- Commit message format
```

### Exercise 3: API Client SDK Documentation

Generate documentation for consuming the API:

```text
Create a quick start guide in docs/guides/api-quickstart.md for developers consuming our Task Manager API. Include authentication (if applicable), endpoint examples with Java RestTemplate or WebClient, and common error handling patterns.
```

---

## Success Criteria

You've completed this lab successfully when:

- ✅ Comprehensive test suite generated with `/tests` (unit + integration)
- ✅ All public APIs have Javadoc via `/doc`
- ✅ API documentation in README.md with examples
- ✅ Conventional Commit messages written for all changes
- ✅ Complete PR description drafted with `@workspace`
- ✅ All tests passing
- ✅ Documentation is clear and maintainable
- ✅ Ready for code review

---

## Workshop Wrap-Up

Congratulations! You've completed all four labs (Java version). You now know how to:

### ✅ Test-Driven Development (Lab 1 - Java)

- Follow Red-Green-Refactor cycle
- Use Copilot to generate tests before implementation
- Apply Copilot Instructions for consistent code quality

### ✅ Requirements to Code (Lab 2 - Java)

- Decompose user stories into backlog items
- Generate acceptance criteria
- Implement features using TDD
- Maintain Clean Architecture across all layers

### ✅ Code Generation & Refactoring (Lab 3 - Java)

- Generate complete REST endpoints with context
- Refactor legacy code with `/refactor`
- Apply Object Calisthenics principles
- Use Copilot Edits for multi-file changes

### ✅ Testing, Documentation & Workflow (Lab 4 - Java)

- Generate comprehensive test coverage
- Create clear documentation
- Write meaningful commit messages
- Prepare thorough PR descriptions

---

## Troubleshooting

### /tests Generates Incomplete Tests

**Problem**: Tests don't cover all edge cases  
**Solution**: Be explicit: "/tests including edge cases, validation errors, and null handling"

### /doc Generates Generic Comments

**Problem**: Javadoc comments don't add value beyond method signature  
**Solution**: Select more context (class + method), provide business context in prompt

### Commit Message Too Generic

**Problem**: Copilot generates "Update files" type messages  
**Solution**: Stage related changes only, provide context: "Write commit for adding GET endpoints"

### PR Description Missing Details

**Problem**: PR description is too high-level  
**Solution**: Use @workspace and be specific: "Include testing details, breaking changes, and reviewer checklist"

### Integration Tests Fail to Start Spring Context

**Problem**: @SpringBootTest tests fail with bean creation errors  
**Solution**: Ensure test configuration matches main configuration, check application-test.properties

---

## Next Steps Beyond Workshop

### Apply to Real Projects

1. Add context-aware instruction files (`.github/instructions/`) to your team's repositories
2. Establish Conventional Commits standard
3. Use `/tests` for all new code
4. Use `/doc` for public APIs
5. Use `@workspace` in daily work

### Advanced Copilot Usage

1. Custom instructions for team-specific patterns
2. Copilot for Business with organization policies
3. Fine-tuned models for domain-specific code
4. Integration with CI/CD pipelines

### Continue Learning

- Practice TDD with different features
- Explore advanced DDD patterns
- Learn Spring Boot Actuator for observability
- Study Clean Architecture in depth
- Explore Spring Cloud for microservices

---

## Additional Resources

- [JUnit 5 Documentation](https://junit.org/junit5/docs/current/user-guide/)
- [Mockito Documentation](https://site.mockito.org/)
- [AssertJ Documentation](https://assertj.github.io/doc/)
- [Spring Boot Testing](https://spring.io/guides/gs/testing-web/)
- [Conventional Commits Specification](https://www.conventionalcommits.org/)
- [Keep a Changelog](https://keepachangelog.com/)
- [RFC 7807 Problem Details](https://tools.ietf.org/html/rfc7807)
- [GitHub Copilot Best Practices](https://docs.github.com/en/copilot)
- [Clean Architecture by Uncle Bob](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
