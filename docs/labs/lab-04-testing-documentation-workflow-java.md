# Lab 4: Testing, Documentation & Workflow with GitHub Copilot (Java/Spring Boot)

> **💡 Also available**: [.NET version](lab-04-testing-documentation-workflow.md) · [Kotlin version](lab-04-testing-documentation-workflow-kotlin.md) · [Swift version](lab-04-testing-documentation-workflow-swift.md)

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

> **⚠️ Golden rule for this lab**: Copilot will happily invent class names, test counts, coverage percentages and "no breaking changes" claims. Every fact in a test, doc, commit message or PR description must be checked against the real code, the real test run, and the real `git diff`.

### Reference values used in this lab

Keep these in mind when reviewing anything Copilot generates:

| Item | Real value |
|------|-----------|
| Task statuses (`TaskStatus`) | `PENDING`, `IN_PROGRESS`, `COMPLETED`, `CANCELLED` |
| Build tool | Maven (`mvn`) — there is no `mvnw` and no Gradle build |
| Domain package | `com.example.taskmanager.domain.tasks` (`Task`, `TaskId`, `TaskStatus`, `TaskRepository`) |
| Application package | `com.example.taskmanager.application.services` (`TaskService`) |
| API DTO package | `com.example.taskmanager.api.dto` (`CreateTaskRequest`, `UpdateTaskRequest`, `TaskResponse` records) |

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

Open `src-springboot/taskmanager-application/src/main/java/com/example/taskmanager/application/services/TaskService.java` and select the `createTask(String title, String description)` method.

`TaskService` is a single concrete class holding all task use cases. It already has a test class (`TaskServiceTest`), so `/tests` is asked to find what is *missing*, not to start from zero.

#### Step 2: Use /tests Command

With the method selected, open Copilot Chat (`Ctrl+Alt+I` / `Cmd+Shift+I`) and enter:

``` text
/tests Add missing edge cases for createTask(String, String): title trimming, title longer than 200 characters, null description. Use JUnit 5 and Mockito, follow the style of the existing TaskServiceTest. Do not duplicate existing tests.
```

#### Step 3: Review Generated Tests

Copilot should generate tests like these (inside the existing `CreateTaskOperations` nested class of `TaskServiceTest`, which already declares the `@Mock TaskRepository taskRepository` and `@InjectMocks TaskService taskService`):

```java
@Test
@DisplayName("should trim title before saving")
void shouldTrimTitleBeforeSaving() {
    // Arrange
    when(taskRepository.save(any(Task.class)))
        .thenAnswer(invocation -> invocation.getArgument(0));

    // Act
    Task result = taskService.createTask("  Write docs  ", "Lab 4");

    // Assert
    assertEquals("Write docs", result.getTitle());
    assertEquals(TaskStatus.PENDING, result.getStatus());
}

@Test
@DisplayName("should allow a null description")
void shouldAllowNullDescription() {
    when(taskRepository.save(any(Task.class)))
        .thenAnswer(invocation -> invocation.getArgument(0));

    Task result = taskService.createTask("Write docs", null);

    assertNull(result.getDescription());
}

@Test
@DisplayName("should reject a title longer than 200 characters")
void shouldRejectTitleLongerThan200Characters() {
    String tooLong = "x".repeat(201);

    assertThrows(IllegalArgumentException.class,
        () -> taskService.createTask(tooLong, null));
    verify(taskRepository, never()).save(any(Task.class));
}

@Test
@DisplayName("should not touch the repository when the title is null")
void shouldNotSaveWhenTitleIsNull() {
    assertThrows(NullPointerException.class,
        () -> taskService.createTask(null, "description"));
    verify(taskRepository, never()).save(any(Task.class));
}
```

**What to check in anything Copilot generates here:**

- ✅ **No builders or setters.** `Task` has a private constructor and is created with `Task.create(title)` / `Task.create(title, description)`. Its status is assigned by the aggregate (`PENDING` on creation) and changes only through `start()`, `complete()` and `cancel()`.
- ✅ **Stubbing returns a real value.** `thenAnswer(invocation -> invocation.getArgument(0))` echoes the saved task. Never write `thenReturn(any(Task.class))` — a matcher is not a value, and Mockito throws `InvalidUseOfMatchersException` (often reported on a *different*, later stubbing).
- ✅ **No unused variables** left over from generation.
- ✅ **Status names exist.** Only `PENDING`, `IN_PROGRESS`, `COMPLETED`, `CANCELLED`.
- ✅ **Imports resolve** to `com.example.taskmanager.domain.tasks.*` and `com.example.taskmanager.application.services.TaskService`.

#### Optional: Let Coverage Pick the Target

Instead of picking a method by hand, measure first, then write the tests the measurement justifies:

```bash
cd src-springboot
mvn clean test jacoco:report
```

Open `taskmanager-<module>/target/site/jacoco/index.html` for each module, find a class with low coverage (for example in `taskmanager-infrastructure`), select it, and run `/tests` on it. Compare with `src-springboot/coverage-summary.txt`, which records an earlier snapshot — numbers go stale, so always re-measure.

### 1.2 Generate Integration Tests

Open `src-springboot/taskmanager-api/src/test/java/com/example/taskmanager/api/TaskApiIntegrationTest.java` (it already uses `@SpringBootTest`, `@AutoConfigureMockMvc` and H2) and ask Copilot Chat:

``` text
Add integration tests to TaskApiIntegrationTest for the POST /api/tasks validation rules: blank title returns 400, title over 200 characters returns 400, and a valid request returns 201 with status PENDING. Match the existing test style (MockMvc, ObjectMapper).
```

**Expected shape** (build requests with the `CreateTaskRequest` record — records are immutable, so create a new request per case instead of mutating a shared fixture):

```java
@Test
@DisplayName("POST /api/tasks creates a PENDING task")
void shouldCreateTaskWithPendingStatus() throws Exception {
    CreateTaskRequest request = CreateTaskRequest.of("Write docs", "Lab 4");

    mockMvc.perform(post("/api/tasks")
            .contentType(MediaType.APPLICATION_JSON)
            .content(objectMapper.writeValueAsString(request)))
        .andExpect(status().isCreated())
        .andExpect(jsonPath("$.title").value("Write docs"))
        .andExpect(jsonPath("$.status").value("PENDING"));
}

@Test
@DisplayName("POST /api/tasks rejects a blank title")
void shouldRejectBlankTitle() throws Exception {
    CreateTaskRequest request = CreateTaskRequest.of("   ");

    mockMvc.perform(post("/api/tasks")
            .contentType(MediaType.APPLICATION_JSON)
            .content(objectMapper.writeValueAsString(request)))
        .andExpect(status().isBadRequest());
}

@Test
@DisplayName("POST /api/tasks rejects a title over 200 characters")
void shouldRejectTitleOver200Characters() throws Exception {
    CreateTaskRequest request = CreateTaskRequest.of("x".repeat(201));

    mockMvc.perform(post("/api/tasks")
            .contentType(MediaType.APPLICATION_JSON)
            .content(objectMapper.writeValueAsString(request)))
        .andExpect(status().isBadRequest());
}
```

Review checklist: record accessors are `title()` / `status()` (not `getTitle()`); `TaskResponse.status` is a `String` such as `"PENDING"`; there are no builders or setters on the DTOs. The existing test class already contains some of these cases — delete any generated duplicates.

### 1.3 Run Complete Test Suite

The Spring Boot track is Maven-only and has no wrapper, so use plain `mvn` from `src-springboot/`:

```bash
cd src-springboot
mvn clean test
```

Verify coverage includes:

- ✅ Happy path tests
- ✅ Guard clause tests
- ✅ Edge case tests (nulls, empty strings, boundaries)
- ✅ Validation tests
- ✅ Error handling tests
- ✅ Integration tests

---

## Part 2: Generate Documentation (3 minutes)

### Scenario: Document Your API

Create accurate documentation for the Task Manager API.

### 2.1 Add Javadoc to Classes

#### Step 1: Select a Class

Open `src-springboot/taskmanager-application/src/main/java/com/example/taskmanager/application/services/TaskService.java` and select the `createTask(String title, String description)` method (or the class declaration).

#### Step 2: Use /doc Command

With the selection active, open **Inline Chat** (`Ctrl+I` / `⌘I`). The `/doc` command only works here, not in the Chat view:

``` text
/doc
```

**Fallback** (if `/doc` isn't offered, or you're in the Chat view): attach the file and ask in plain language:

``` text
Add Javadoc to the selected method. Explain behavior and failure modes, not just the signature.
```

**What good output looks like** — it explains behavior and failure modes, not just the signature:

```java
/**
 * Creates a new task in PENDING status and persists it.
 *
 * @param title the task title; trimmed, must not be blank or exceed 200 characters
 * @param description optional task description, may be null
 * @return the persisted task
 * @throws NullPointerException if title is null
 * @throws IllegalArgumentException if title is blank or too long
 */
```

The Javadoc in `TaskService` already covers most public methods. If Copilot only restates the signature, add business context to the prompt or skip the method — a comment that repeats the code is noise.

Records have accessors, not getters: when documenting DTOs, refer to `request.title()` and `response.status()`.

### 2.2 Generate API Documentation (README)

`src-springboot/README.md` **already has** an "API Endpoints" section. Update it rather than adding a second one. Ask:

```text
@workspace Compare the API Endpoints section in src-springboot/README.md with TaskController, CreateTaskRequest, UpdateTaskRequest and TaskResponse. List anything that is missing or wrong, then propose corrections. Only document what exists in the code.
```

For reference, here is the real API that the README must match (verify against `TaskController`):

````markdown
### Endpoints

Base URL: `http://localhost:8080/api/tasks`

| Method | Endpoint | Description |
|--------|----------|-------------|
| `POST` | `/api/tasks` | Create a task (starts as `PENDING`) |
| `GET` | `/api/tasks` | List all tasks (plain JSON array, no filtering or paging) |
| `GET` | `/api/tasks/{id}` | Get a task by ID |
| `PATCH` | `/api/tasks/{id}` | Partially update title and/or description |
| `PUT` | `/api/tasks/{id}/start` | `PENDING` → `IN_PROGRESS` |
| `PUT` | `/api/tasks/{id}/complete` | → `COMPLETED` |
| `PUT` | `/api/tasks/{id}/cancel` | → `CANCELLED` |
| `DELETE` | `/api/tasks/{id}` | Delete a task |

**Create a task**

```bash
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title": "Write docs", "description": "Finish Lab 4"}'
```

Request fields: `title` (required, max 200 characters), `description` (optional, max 2000 characters).

Response `201 Created`:

```json
{
  "id": "123e4567-e89b-12d3-a456-426614174000",
  "title": "Write docs",
  "description": "Finish Lab 4",
  "status": "PENDING",
  "createdAt": "2026-01-15T10:30:00",
  "completedAt": null
}
```

**Status values**: `PENDING`, `IN_PROGRESS`, `COMPLETED`, `CANCELLED`

**Errors** use RFC 7807 Problem Details, for example `404 Not Found`:

```json
{
  "type": "https://api.taskmanager.example.com/problems/task-not-found",
  "title": "Task Not Found",
  "status": 404,
  "detail": "Task not found with ID: 123e4567-e89b-12d3-a456-426614174000"
}
```
````

If Copilot documents query filters, a paged envelope, a `completed` flag, a `PUT` update, or status values like `TODO`/`DONE`, it is describing a different API. Correct it.

### 2.3 Generate Architecture Documentation (ADR)

[ADR 0001 – Use Clean Architecture](../adr/0001-use-clean-architecture.md) already exists. **Do not recreate or overwrite it.** Check `docs/adr/` for the next free number (currently `0002`) and document a decision you actually made during the labs. A good candidate: *`TaskService` is a concrete class rather than an interface-per-use-case*. Ask Copilot:

```text
Create an Architecture Decision Record at docs/adr/0002-<short-decision-title>.md, following the format of docs/adr/0001-use-clean-architecture.md. The decision: TaskService is a single concrete application service (no interface, no CQRS, no mediator library). Base the context on src-springboot/taskmanager-application/.../services/TaskService.java and .github/instructions/springboot.instructions.md. Include Context, Decision, Consequences (positive and negative) and Alternatives considered. Do not invent facts that are not in the code.
```

Any decision from your own work is fine — the point is a fresh decision with real trade-offs.

---

## Part 3: Write Conventional Commit Messages (3 minutes)

### Scenario: Commit Your Changes

You've made changes across several files. Write meaningful commit messages following the Conventional Commits format.

### 3.1 Stage Changes

First see what you actually changed, then stage those real paths:

```bash
git status
git diff --stat
```

For example, if you completed Parts 1 and 2:

```bash
git add src-springboot/taskmanager-application/src/test/java/com/example/taskmanager/application/services/TaskServiceTest.java
git add src-springboot/taskmanager-api/src/test/java/com/example/taskmanager/api/TaskApiIntegrationTest.java
git add src-springboot/README.md docs/adr/
```

`git add` on a path that does not exist fails with `pathspec ... did not match any files` — if you see that, you are staging something you did not create.

### 3.2 Generate Commit Message

In Copilot Chat:

```text
Write a Conventional Commit message for the staged changes. Use only what appears in `git diff --staged`. Include a concise subject line (<=72 chars) and a short body explaining what was added and why.
```

**Example of a faithful result** (yours will differ — it must describe *your* diff):

```text
test(application): cover createTask edge cases

- Add tests for title trimming, null description and 200-character limit
- Add POST /api/tasks validation tests to TaskApiIntegrationTest
- Align README API section with TaskController

The existing tests only covered the happy path and null/blank titles.
```

**Verify before committing**: every bullet maps to a file in the diff. Do not accept class names, handlers, test counts or metrics (such as "reduces complexity from 15 to 4") that Copilot cannot point to in the diff — inventing them is the failure mode this step is meant to teach you to catch.

### 3.3 Commit with Generated Message

```bash
git commit -m "test(application): cover createTask edge cases" \
  -m "Add tests for title trimming, null description and the 200-character limit. Add POST /api/tasks validation tests. Align the README API section with TaskController."
```

### 3.4 Practice: Stage More Changes

If you refactored `LegacyTaskProcessor` in Lab 3, stage it and its tests (use `git status` to find the real test path):

```bash
git add src-springboot/taskmanager-infrastructure/src/main/java/com/example/taskmanager/infrastructure/legacy/LegacyTaskProcessor.java
git status
```

Ask Copilot:

```text
Write a Conventional Commit message for the staged LegacyTaskProcessor refactoring. Describe only the changes visible in `git diff --staged`.
```

Check the result against the diff: if the message says "Stream API" but the code uses a `StringBuilder` loop, or quotes complexity figures nobody measured, fix the message. If your change alters behavior, mark it with `!` and a `BREAKING CHANGE:` footer.

---

## Part 4: Draft Pull Request Descriptions (4 minutes)

### Scenario: Prepare for Code Review

You've completed your changes. Create an honest PR description for review.

### 4.1 Generate PR Description with @workspace

In Copilot Chat:

```text
@workspace Draft a Pull Request description for the changes on this branch compared with main. Base it on `git diff main...HEAD` only. Include:
- Summary of changes
- What was implemented (features, refactoring, tests)
- Intent and motivation
- Testing performed (list the exact commands and their results)
- Breaking changes (list them, or state how you verified there are none)
- Checklist for reviewers
Do not state test counts, coverage percentages or performance figures unless they come from a command output I provide. Use Markdown formatting.
```

**Example of an honest result** (fill in the numbers from your own run):

````markdown
# Add createTask edge-case tests and align README (Spring Boot)

## Summary
Adds unit and integration tests for task creation validation and corrects the
API section of `src-springboot/README.md` to match `TaskController`.

## Changes
- `TaskServiceTest`: trimming, null description, 200-character title limit
- `TaskApiIntegrationTest`: POST /api/tasks validation cases
- `README.md`: endpoint table and examples now match the code
- `docs/adr/0002-...`: records the "concrete TaskService" decision

## Testing Performed
```bash
cd src-springboot
mvn clean test
```
Result: <paste BUILD SUCCESS/FAILURE and the "Tests run" total from your output>

Coverage: <paste figures from `mvn test jacoco:report`, or write "not measured">

## Breaking Changes
None to production code — this PR only adds tests and documentation.
(If you changed an API contract, list each change here.)

## Known Gaps
- No load or performance testing was done.

## Reviewer Checklist
- [ ] README endpoints match `TaskController`
- [ ] New tests fail when the behavior they cover is broken
- [ ] ADR reflects a decision actually made in the code
````

> **⚠️ Verify every claim.** An assistant will confidently write "45+ tests", "92% coverage", "no memory leaks detected" or "Breaking Changes: None" with nothing behind it. Before posting, check each factual statement against command output and the diff. "Breaking Changes: None" is only acceptable if you checked that no API contract, database schema or public behavior changed.

### 4.2 Review and Refine

Review the generated PR description and adjust:

- Replace placeholders with real command output (test totals, coverage)
- Add specific issue numbers
- Delete anything you cannot verify
- Highlight any specific areas needing review

---

## Key Learning Points

### ✅ Testing Best Practices

1. **/tests Command**: Generates test suites quickly — but you must verify they compile against the real API
2. **Measure, Then Test**: JaCoCo shows where real gaps are
3. **Test Organization**: Clear package structure, descriptive test names
4. **Integration Tests**: @SpringBootTest with MockMvc for full API testing
5. **Mockito**: Stub with real values (`thenAnswer`), never matchers as return values

### ✅ Documentation Efficiency

1. **/doc Command**: Javadoc generated from code context
2. **API Docs**: Update the existing README section and check it against the controller
3. **Architecture Docs**: ADRs document decisions you really made, at the next free number
4. **Consistency**: Use one status list everywhere: `PENDING`, `IN_PROGRESS`, `COMPLETED`, `CANCELLED`

### ✅ Version Control Quality

1. **Conventional Commits**: Structured, parsable commit messages
2. **Semantic Commits**: Type, scope, description format
3. **Faithful Bodies**: Describe only what is in the diff
4. **Changelog Ready**: Commits can generate CHANGELOG.md automatically

### ✅ Code Review Preparation

1. **@workspace Context**: Full codebase understanding for the PR description
2. **Honest PRs**: Real command output, no invented metrics
3. **Reviewer Checklist**: Clear acceptance criteria
4. **Impact Analysis**: Breaking changes stated only after verification

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
Create a quick start guide in docs/guides/api-quickstart.md for developers consuming our Task Manager API. Include endpoint examples with Java RestTemplate or WebClient, and common error handling patterns. Only document endpoints that exist in TaskController.
```

---

## Success Criteria

You've completed this lab successfully when:

- ✅ New tests generated with `/tests` compile and pass (unit + integration)
- ✅ Public APIs you touched have meaningful Javadoc via `/doc`
- ✅ README API section matches `TaskController`
- ✅ Conventional Commit messages describe only real changes
- ✅ PR description drafted with `@workspace` and every claim verified
- ✅ `mvn clean test` passes
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
- Prepare thorough, verified PR descriptions

---

## Troubleshooting

### /tests Generates Incomplete Tests

**Problem**: Tests don't cover all edge cases  
**Solution**: Be explicit: "/tests including edge cases, validation errors, and null handling"

### /tests Generates Code That Does Not Compile

**Problem**: Generated tests use builders, setters, `getTitle()` on records, or status values such as `TODO`/`DONE`  
**Solution**: Point Copilot at the real types (`Task.create(...)`, `CreateTaskRequest.of(...)`, record accessors like `title()`) and the real `TaskStatus` values

### /doc Generates Generic Comments

**Problem**: Javadoc comments don't add value beyond method signature  
**Solution**: Select more context (class + method), provide business context in prompt

### Commit Message Too Generic or Invented

**Problem**: Copilot generates "Update files" messages, or describes classes and metrics that are not in the diff  
**Solution**: Stage related changes only and tell Copilot to use `git diff --staged` as the only source

### PR Description Missing Details

**Problem**: PR description is too high-level  
**Solution**: Use @workspace and be specific: "Include testing details, breaking changes, and reviewer checklist"

### Integration Tests Fail to Start Spring Context

**Problem**: @SpringBootTest tests fail with bean creation errors  
**Solution**: Ensure test configuration matches main configuration, check `application-test.yml`

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
