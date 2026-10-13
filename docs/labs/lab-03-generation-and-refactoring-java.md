# Lab 3: Code Generation & Refactoring with GitHub Copilot (Java/Spring Boot)

> **💡 Also available**: [.NET version](lab-03-generation-and-refactoring.md) using C# and ASP.NET Core · [Kotlin version](lab-03-generation-and-refactoring-kotlin.md) · [Swift version](lab-03-generation-and-refactoring-swift.md)

**Duration**: 45 minutes  
**Learning Objectives**:

- Generate a missing REST API feature using Copilot and context variables
- Pin legacy behavior with characterization tests *before* refactoring
- Refactor legacy code using `/refactor` command and Inline Chat
- Apply Object Calisthenics principles with AI assistance
- Use `@workspace` for understanding and modifying existing code
- Understand how Copilot Edits handles multi-file changes (safely, on a scratch branch)

---

## 📝 Plan First with Agents: Safer, Smarter Refactoring

Before making major changes, try using Copilot's **Plan** mode to generate a plan first. This helps you:
- Understand the scope and impact of your changes
- Catch misunderstandings or missing steps early
- Collaborate and iterate on the approach before any code is changed

**How to try it:**
- In Copilot Chat (**Plan** mode), ask: "Propose a step-by-step plan to refactor LegacyTaskProcessor to add proper exception handling, logging, and follow Object Calisthenics. Start with characterization tests."
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

- **Part 1**: Add the one genuinely missing API feature: a `status` filter on `GET /api/tasks`
- **Part 2**: Pin the behavior of `LegacyTaskProcessor` with characterization tests, then refactor it
- **Part 3**: Apply Object Calisthenics (primitive obsession in the legacy class, plus a check of the domain)
- **Part 4** *(optional demo)*: Multi-file refactoring with Copilot Edits, on a throwaway branch

---

## 🚀 Interactive Challenge: Implement the Plan Step by Step

For this lab, take the plan you created and implement it in **Interactive** mode, one step at a time (such as adding the status filter, then writing characterization tests, then refactoring `LegacyTaskProcessor`). Interactive mode lets Copilot make multi-file changes while you approve each step, and it can invoke advanced tools (like MCP evaluation or tracing) when you ask.

**How to try it:**
- Select **Interactive** in the Agent mode picker below the chat box (keep Permissions on Manual and work on a branch)
- Describe your goal in natural language (e.g., "Add an optional status filter to GET /api/tasks following the existing controller patterns")
- Review the plan and results, iterate as needed

**Compare:**
- How did implementing from a plan differ from a single direct prompt?
- Did it follow the plan, use multiple tools, or make changes across files?
- Was the result more complete or did it need more review?

**Facilitator Tip:**
Encourage participants to share their results and discuss when this approach is most effective.

---

## Prerequisites

- ✅ Completed Lab 1 (TDD) and Lab 2 (Requirements to Code)
- ✅ Familiar with Copilot Chat, Inline Chat, and slash commands
- ✅ Understanding of Clean Architecture layers
- ✅ Repository at clean state
- ✅ Java 21 and Maven installed
- ✅ Spring Boot 3.x project configured

---

## Part 1: Fill the API Gap, a Status Filter (15 minutes)

### Scenario: Find What Is Missing Before Generating Anything

Earlier labs already built most of the Task API. Generating endpoints that exist is wasted effort, so the first skill in this lab is **discovering the gap**.

### 1.1 Understand Existing Structure with @workspace

```text
@workspace List every endpoint in TaskController and the TaskService method each one calls. Which common query capabilities are missing?
```

`TaskController` (`src-springboot/taskmanager-api/src/main/java/com/example/taskmanager/api/controllers/TaskController.java`) already exposes:

| Method | Path | Purpose |
|---|---|---|
| POST | `/api/tasks` | Create a task |
| GET | `/api/tasks` | List all tasks (**no filters**) |
| GET | `/api/tasks/{id}` | Get one task |
| PATCH | `/api/tasks/{id}` | Partial update (title, description) |
| PUT | `/api/tasks/{id}/start` | Start a task |
| PUT | `/api/tasks/{id}/complete` | Complete a task |
| PUT | `/api/tasks/{id}/cancel` | Cancel a task |
| DELETE | `/api/tasks/{id}` | Delete a task |

> **Check the real controller**: the exact endpoint list can evolve between labs. Trust what `@workspace` finds in your checkout over this table.

The gap: `GET /api/tasks` returns everything. There is no way to ask for only `IN_PROGRESS` tasks.

Real domain facts the generated code must respect:

- `TaskStatus` has four values: `PENDING`, `IN_PROGRESS`, `COMPLETED`, `CANCELLED` (it is not a boolean `completed` flag, and there is no `TODO` or `DONE`)
- `Task.getId()` returns a `TaskId` record wrapping a `UUID`; the controller maps with `id.value()`/`new TaskId(uuid)`
- `TaskResponse` carries a `status` string, and is built with `TaskResponse.from(task)`; never construct it positionally
- A missing task throws `TaskService.TaskNotFoundException`; `GlobalExceptionHandler` turns `IllegalArgumentException` into a 400 `ProblemDetail`

### 1.2 Check What the Service Already Offers

```text
@workspace Does TaskService already have a method that filters tasks by status? Which repository port method does it use?
```

`TaskService` already has `findAllTasks()` and `findTasksByStatus(TaskStatus)`, backed by `TaskRepository.findByStatus(TaskStatus)`. So the **service and domain port need no change**; the work is in the API layer. If the service lacked it, you would add it test-first (Lab 1).

### 1.3 Write the Failing Test First

Open `src-springboot/taskmanager-api/src/test/java/com/example/taskmanager/api/TaskApiIntegrationTest.java` and ask:

```text
#file:src-springboot/taskmanager-api/src/test/java/com/example/taskmanager/api/TaskApiIntegrationTest.java
Add tests for GET /api/tasks?status=:
1. status=IN_PROGRESS returns only in-progress tasks
2. status is case-insensitive ("in_progress" works)
3. no status returns all tasks (existing behavior unchanged)
4. status=DONE returns 400 and the ProblemDetail message names the valid values PENDING, IN_PROGRESS, COMPLETED, CANCELLED
Follow the existing test style.
```

Run them and confirm they fail for the right reason.

### 1.4 Implement the Controller Change

Use `#file` so Copilot follows the existing pattern:

```text
Update listTasks in #file:src-springboot/taskmanager-api/src/main/java/com/example/taskmanager/api/controllers/TaskController.java
to accept an optional `status` query parameter.
- Parse it case-insensitively into TaskStatus
- Invalid values throw IllegalArgumentException listing the valid values (GlobalExceptionHandler maps it to 400)
- Use taskService.findTasksByStatus when present, findAllTasks otherwise
- Map with TaskResponse::from
- Keep the existing OpenAPI annotations and logging style
Design it so another optional filter can be combined later.
```

**Expected shape** (yours will differ in detail):

```java
@GetMapping
public ResponseEntity<List<TaskResponse>> listTasks(
        @RequestParam(required = false) String status) {
    logger.debug("Listing tasks with status filter: {}", status);

    List<Task> tasks = (status == null || status.isBlank())
            ? taskService.findAllTasks()
            : taskService.findTasksByStatus(parseStatus(status));

    return ResponseEntity.ok(tasks.stream().map(TaskResponse::from).toList());
}

private static TaskStatus parseStatus(String status) {
    try {
        return TaskStatus.valueOf(status.trim().toUpperCase());
    } catch (IllegalArgumentException e) {
        throw new IllegalArgumentException(
            "Invalid status: " + status + ". Valid values: PENDING, IN_PROGRESS, COMPLETED, CANCELLED");
    }
}
```

**Review prompts**: Is the valid-value list hard-coded? (Better: build it from `TaskStatus.values()` so it cannot drift.) Does the parsing belong in the controller, or at the boundary in a converter? Discuss.

> **Combining filters**: The current `Task` model has no priority or due date. If your branch has gained a `priority` filter, ask Copilot to combine it with `status` rather than adding a parallel endpoint. Both parameters optional, applied together (AND).

### 1.5 Run and Test

From the repo root:

```bash
cd src-springboot
mvn clean install -DskipTests
mvn spring-boot:run -pl taskmanager-api -Dspring-boot.run.profiles=dev
```

> **Why this sequence?** The default profile targets PostgreSQL, so the `dev` profile is needed for a local run. `-pl taskmanager-api` resolves sibling modules from your local Maven repository, so they must be installed first or you get stale jars.
>
> **Windows PowerShell**: quote the `-D` argument: `mvn spring-boot:run -pl taskmanager-api "-Dspring-boot.run.profiles=dev"`

Then run the tests in a second terminal:

```bash
cd src-springboot
mvn test
```

Exercise the API (task creation takes only `title` and `description`, so there are no dates to go stale):

```bash
# Create two tasks
curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title": "Write report", "description": "Quarterly numbers"}'

curl -X POST http://localhost:8080/api/tasks \
  -H "Content-Type: application/json" \
  -d '{"title": "Review PR"}'

# Start one of them (use an ID from the create response)
curl -X PUT http://localhost:8080/api/tasks/{id}/start

# Filtered and unfiltered lists
curl "http://localhost:8080/api/tasks?status=IN_PROGRESS"
curl http://localhost:8080/api/tasks

# Invalid status -> 400 naming PENDING, IN_PROGRESS, COMPLETED, CANCELLED
curl -i "http://localhost:8080/api/tasks?status=DONE"

# Partial update uses PATCH, not PUT
curl -X PATCH http://localhost:8080/api/tasks/{id} \
  -H "Content-Type: application/json" \
  -d '{"title": "Updated title"}'
```

> **Dates**: if your task model gains a due date, API validation will require it to be in the future. Use a far-future value (for example `2099-12-31T12:00:00`) or compute one relative to today, never a fixed date that will go stale.

---

## Part 2: Characterize, Then Refactor Legacy Code (20 minutes)

### Scenario: The Legacy Task Processor

The repository contains `LegacyTaskProcessor`: intentionally poor code. Despite the name, it is a **string transformer**, not something that processes `Task` aggregates.

### 2.1 Find the Legacy Code

```text
@workspace Find the LegacyTaskProcessor class in the Spring Boot project
```

**Location**: `src-springboot/taskmanager-infrastructure/src/main/java/com/example/taskmanager/infrastructure/legacy/LegacyTaskProcessor.java`

### 2.2 Analyze Current Issues

Select the `processTask` method and use Inline Chat (`Ctrl+I` / `Cmd+I`): `/explain`

The signature is `public String processTask(int id, String data, int type, boolean flag)`. What it actually does:

| `type` | `flag` | Behavior |
|---|---|---|
| `1` | `true` | Invert letter case, replace each space with `_`, truncate to 50 chars, sleep 100 ms, write `task_<id>.txt` |
| `1` | `false` | Upper-case the input |
| `2` | any | Keep the first word as-is, lower-case the remaining words (split on `" "`) |
| other | any | Return input unchanged |
| any | any | `null` or empty `data` returns `""` |

Copilot should identify:

- ❌ Nesting 6 levels deep
- ❌ Magic numbers (`1`, `2`, `50`, `100`) and the `' '` literal
- ❌ `int type` and `boolean flag` are opaque (primitive obsession)
- ❌ `result +=` in a loop (string concatenation)
- ❌ File I/O and `Thread.sleep` inline with the transformation (mixed concerns)
- ❌ Two empty `catch` blocks that swallow `IOException` and `InterruptedException`
- ❌ No logging
- ❌ Poor naming (`data`, `flag`, `type`, `i`)

### 2.3 Characterization Tests First

> **Rule for legacy code**: never refactor without a safety net, and write the net **against the unmodified code**. Tests written *after* a refactor describe what the new code does; if the refactor changed behavior they would enshrine the bug. Characterization tests record what the code **does** today, not what it *should* do, and must pass both before and after.

Ask Copilot:

```text
Write JUnit 5 characterization tests for LegacyTaskProcessor.processTask. Do NOT change the class.
Assert the current return values for: null data, empty data, type 1 + flag true (case inversion,
spaces to underscores), the 50-character truncation boundary (49, 50, 51 chars), type 1 + flag false,
type 2 (first word kept, rest lower-cased), an unknown type, and the input " " (a single space) with type 2.
Delete any task_<id>.txt file the tests create.
```

Put them in `taskmanager-infrastructure/src/test/java/com/example/taskmanager/infrastructure/legacy/LegacyTaskProcessorCharacterizationTest.java`. Expected flavor:

```java
class LegacyTaskProcessorCharacterizationTest {

    private final LegacyTaskProcessor processor = new LegacyTaskProcessor();

    @AfterEach
    void deleteOutputFile() throws IOException {
        Files.deleteIfExists(Path.of("task_7.txt"));
    }

    @Test
    void nullOrEmptyDataReturnsEmptyString() {
        assertEquals("", processor.processTask(7, null, 1, true));
        assertEquals("", processor.processTask(7, "", 2, false));
    }

    @Test
    void typeOneWithFlagInvertsCaseAndReplacesSpaces() {
        assertEquals("hELLO_wORLD", processor.processTask(7, "Hello World", 1, true));
    }

    @Test
    void typeOneWithFlagTruncatesToFiftyCharacters() {
        assertEquals(50, processor.processTask(7, "a".repeat(51), 1, true).length());
    }

    @Test
    void typeOneWithoutFlagUpperCases() {
        assertEquals("HELLO WORLD", processor.processTask(7, "Hello World", 1, false));
    }

    @Test
    void typeTwoKeepsFirstWordAndLowerCasesTheRest() {
        assertEquals("Hello world", processor.processTask(7, "Hello WORLD", 2, false));
    }

    @Test
    void unknownTypeReturnsInputUnchanged() {
        assertEquals("Hello", processor.processTask(7, "Hello", 3, false));
    }

    // Surprising: " ".split(" ") yields an EMPTY array, so there is no first word.
    @Test
    void typeTwoWithOnlyASpaceReturnsEmptyString() {
        assertEquals("", processor.processTask(7, " ", 2, false));
    }
}
```

**Why the `" "` case matters**: the obvious refactor, "take `words[0]`, then loop over the rest", throws `ArrayIndexOutOfBoundsException` on that input. Only a test written *before* the refactor catches it.

Run them and make sure they **pass against the untouched class**:

```bash
cd src-springboot
mvn test -pl taskmanager-infrastructure -am
```

If a test fails, your expectation was wrong, not the code. Fix the test; it is documenting, not judging.

### 2.4 Refactor with /refactor Command

Select the entire `processTask` method and use Copilot Chat:

```text
/refactor this method to follow Clean Code and Spring Boot best practices. Keep the public signature
processTask(int id, String data, int type, boolean flag) and the observable return values identical:
1. Use guard clauses (fail fast, no nested ifs)
2. Replace `int type` internally with a ProcessingType enum (INVERT_CASE, CAPITALIZE_FIRST_WORD, PASSTHROUGH)
3. Replace magic numbers with named constants (max length 50, space, delay 100 ms)
4. Use StringBuilder instead of += in loops
5. Extract the file write behind a TaskOutputWriter interface (port) with a FileTaskOutputWriter adapter
6. Add SLF4J logging via LoggerFactory (the project does not use Lombok)
7. Do not swallow exceptions: log and throw a TaskProcessingException; restore the interrupt flag on InterruptedException
8. Max 2 levels of indentation per method, small focused methods
9. Constructor injection; keep it a Spring @Component
Do not change behavior for any input covered by the characterization tests.
```

**Expected shape** (abridged; yours will differ):

```java
@Component
public class LegacyTaskProcessor {

    private static final Logger logger = LoggerFactory.getLogger(LegacyTaskProcessor.class);

    private static final int MAX_RESULT_LENGTH = 50;
    private static final char WORD_SEPARATOR = ' ';
    private static final String WORD_SEPARATOR_REPLACEMENT = "_";
    private static final long SIMULATED_PROCESSING_MILLIS = 100;

    private final TaskOutputWriter outputWriter;

    public LegacyTaskProcessor(TaskOutputWriter outputWriter) {
        this.outputWriter = outputWriter;
    }

    public String processTask(int id, String data, int type, boolean flag) {
        if (data == null || data.isEmpty()) {
            return "";
        }
        return switch (ProcessingType.fromCode(type)) {
            case INVERT_CASE -> flag ? invertCaseAndPersist(id, data) : data.toUpperCase();
            case CAPITALIZE_FIRST_WORD -> keepFirstWordLowerCaseRest(data);
            case PASSTHROUGH -> data;
        };
    }

    private String invertCaseAndPersist(int id, String data) {
        String inverted = truncate(invertCase(data));
        simulateProcessing();
        outputWriter.write(id, inverted);
        return inverted;
    }

    private String invertCase(String data) {
        StringBuilder builder = new StringBuilder(data.length());
        for (char character : data.toCharArray()) {
            builder.append(invertCharacter(character));
        }
        return builder.toString();
    }

    private String invertCharacter(char character) {
        if (character == WORD_SEPARATOR) {
            return WORD_SEPARATOR_REPLACEMENT;
        }
        return String.valueOf(Character.isUpperCase(character)
                ? Character.toLowerCase(character)
                : Character.toUpperCase(character));
    }

    private String truncate(String value) {
        return value.length() > MAX_RESULT_LENGTH ? value.substring(0, MAX_RESULT_LENGTH) : value;
    }

    private String keepFirstWordLowerCaseRest(String data) {
        String[] words = data.split(String.valueOf(WORD_SEPARATOR));
        if (words.length == 0) {          // " ".split(" ") is an empty array
            return "";
        }
        StringBuilder builder = new StringBuilder(words[0]);
        for (int index = 1; index < words.length; index++) {
            builder.append(WORD_SEPARATOR).append(words[index].toLowerCase());
        }
        return builder.toString();
    }

    private void simulateProcessing() {
        try {
            Thread.sleep(SIMULATED_PROCESSING_MILLIS);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            logger.error("Task processing interrupted", e);
            throw new TaskProcessingException("Task processing interrupted", e);
        }
    }
}

public enum ProcessingType {
    INVERT_CASE, CAPITALIZE_FIRST_WORD, PASSTHROUGH;

    public static ProcessingType fromCode(int code) {
        return switch (code) {
            case 1 -> INVERT_CASE;
            case 2 -> CAPITALIZE_FIRST_WORD;
            default -> PASSTHROUGH;
        };
    }
}

public interface TaskOutputWriter {            // port
    void write(int taskId, String content);
}

@Component
public class FileTaskOutputWriter implements TaskOutputWriter {   // adapter
    private static final Logger logger = LoggerFactory.getLogger(FileTaskOutputWriter.class);

    @Override
    public void write(int taskId, String content) {
        try {
            Files.writeString(Path.of("task_" + taskId + ".txt"), content);
        } catch (IOException e) {
            logger.error("Failed to write output for task {}", taskId, e);
            throw new TaskProcessingException("Failed to write output for task " + taskId, e);
        }
    }
}

public class TaskProcessingException extends RuntimeException {
    public TaskProcessingException(String message, Throwable cause) {
        super(message, cause);
    }
}
```

**Review prompts**:

- Which behavior changed on purpose? (The two swallowed exceptions now surface as `TaskProcessingException`. This is the only observable change and is intentional.)
- Does the `" "` characterization test still pass? If Copilot dropped the `words.length == 0` guard, it caught you.
- Constructor injection now requires a `TaskOutputWriter` bean. Update the characterization test setup accordingly (for example `new LegacyTaskProcessor(new FileTaskOutputWriter())`, or a stub writer).
- Optional goal: `CompletableFuture`. Skip it unless you are willing to change the public signature other code and labs depend on.

### 2.5 Verify and Extend the Tests

Re-run the characterization tests. They must all still pass:

```bash
cd src-springboot
mvn test -pl taskmanager-infrastructure -am
```

Only now use `/tests` to cover **new** seams, such as:

- `TaskOutputWriter` is called with the id and the truncated result (use a hand-written stub or Mockito)
- `IOException` from the writer surfaces as `TaskProcessingException`
- Interrupting the thread yields `TaskProcessingException` with the interrupt flag restored
- `ProcessingType.fromCode` maps `1`, `2`, and anything else

---

## Part 3: Apply Object Calisthenics (5 minutes)

### Scenario: Further Code Quality Improvements

### 3.1 Review Object Calisthenics Rules

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
9. No getters/setters (for domain entities, use methods that express behavior)

### 3.2 Apply: Wrap Primitives (Primitive Obsession in the Legacy Class)

First, check whether the domain already complies:

```text
@workspace Are there primitive types in the domain module that should be wrapped in value objects?
```

It largely does: `TaskId` is a record wrapping `UUID`, and `TaskStatus` is an enum, not a string or int. The domain stays pure Java (no JPA annotations); persistence mapping lives in `TaskEntity`/`TaskMapper` in the infrastructure layer.

> **Do not** make `TaskId` an `@Embeddable` or put `@Entity` on domain classes. That pulls JPA into the domain module and violates the Clean Architecture rule that Domain has no framework dependencies.

The real primitive obsession is in `LegacyTaskProcessor`'s signature:

**Before**:

```java
public String processTask(int id, String data, int type, boolean flag)
//                        ^ what is 7?    ^ 1? 2? 3?   ^ true means... what?
processor.processTask(7, "Hello World", 1, true);
```

Ask Copilot:

```text
In LegacyTaskProcessor, `int type` and `boolean flag` are primitive obsession. Propose a typed
alternative that keeps the existing method working. Should the flag become part of the ProcessingType?
```

**After** (one possible direction):

```java
public enum ProcessingType { INVERT_CASE, INVERT_CASE_AND_PERSIST, CAPITALIZE_FIRST_WORD, PASSTHROUGH }

public String process(int id, String data, ProcessingType type) { ... }

// Legacy signature kept as a thin adapter so existing callers and Lab 4 still work
public String processTask(int id, String data, int type, boolean flag) {
    return process(id, data, ProcessingType.fromLegacy(type, flag));
}
```

Discuss: a boolean that only matters for *one* value of `type` is really a hidden fourth `type`. Pulling it into the enum makes the invalid combination unrepresentable. Is wrapping `int id` in a value object also warranted here, or is that over-engineering for a legacy shim? (Note the domain already has a real `TaskId`.)

### 3.3 Apply: First-Class Collections

Ask Copilot:

```text
If we have a class with a List<Task> field, how should we wrap it following Object Calisthenics and DDD patterns?
```

**Before**:

```java
public class TaskList {
    private List<Task> tasks;

    public List<Task> getTasks() {
        return tasks;
    }
}
```

**After**:

```java
public final class TaskCollection {
    private final List<Task> tasks;

    public TaskCollection(List<Task> tasks) {
        this.tasks = tasks != null ? new ArrayList<>(tasks) : new ArrayList<>();
    }

    public int size() {
        return tasks.size();
    }

    public List<Task> asList() {
        return List.copyOf(tasks);
    }

    public void add(Task task) {
        if (task == null) {
            throw new IllegalArgumentException("Task cannot be null");
        }
        tasks.add(task);
    }

    public Optional<Task> findById(TaskId id) {
        return tasks.stream()
                .filter(task -> task.getId().equals(id))
                .findFirst();
    }

    public TaskCollection filterByStatus(TaskStatus status) {
        return new TaskCollection(tasks.stream()
                .filter(task -> task.getStatus() == status)
                .toList());
    }
}
```

Discuss whether the repo needs this now (it passes `List<Task>` around). A wrapper earns its place when it gains behavior, not just because a rule says so.

### 3.4 Apply: No Abbreviations

**Before**:

```java
var res = repo.findById(id);
if (res.isPresent()) {
    var t = res.get();
    // ...
}
```

**After**:

```java
Optional<Task> result = repository.findById(taskId);
if (result.isPresent()) {
    Task task = result.get();
    // ...
}
```

### 3.5 Apply: Tell, Don't Ask

The domain already does this: `Task` exposes `isCompleted()`, `isCancelled()` and `isInProgress()` and behavior methods such as `start()`, `complete()`, `cancel()` instead of setters.

**Before** (asking):

```java
if (task.getStatus() == TaskStatus.COMPLETED) { ... }
```

**After** (telling):

```java
if (task.isCompleted()) { ... }
```

---

## Part 4: Multi-File Refactoring with Copilot Edits (Optional Demo: Throwaway Branch Only)

> **⚠️ Demonstration only, do not merge.** Renaming a field across a layered codebase is destructive. `title` appears in the domain aggregate, `TaskEntity`, the mapper, `CreateTaskRequest`, `UpdateTaskRequest`, `TaskResponse`, the controller, the database migrations and most tests, and renaming it changes the published API contract for no functional gain.
>
> Do this on a scratch branch, review the diff, then **discard it**:
>
> ```bash
> git switch -c scratch/copilot-edits-demo
> # ... run the demo ...
> git switch -    # return to your work
> git branch -D scratch/copilot-edits-demo
> ```

### Scenario: See How Copilot Edits Handles a Cross-Cutting Change

### 4.1 Open Copilot Edits

1. Open Command Palette (`Ctrl+Shift+P` / `Cmd+Shift+P`)
2. Search for "Copilot Edits: Open"
3. Or use the dedicated Copilot Edits panel in the sidebar

### 4.2 Add Files to the Working Set

Start with these, **knowing the set is incomplete** (the point of the demo):

- `src-springboot/taskmanager-domain/src/main/java/com/example/taskmanager/domain/tasks/Task.java`
- `src-springboot/taskmanager-api/src/main/java/com/example/taskmanager/api/dto/CreateTaskRequest.java`
- `src-springboot/taskmanager-application/src/main/java/com/example/taskmanager/application/services/TaskService.java`
- `src-springboot/taskmanager-api/src/main/java/com/example/taskmanager/api/controllers/TaskController.java`
- `src-springboot/taskmanager-application/src/test/java/com/example/taskmanager/application/services/TaskServiceTest.java`

### 4.3 Describe the Change

```text
Rename the "title" field to "name" across all files in the working set.
```

### 4.4 Review the Proposed Changes

- Which files does Copilot touch? Which ones does it **miss** (persistence entity, mapper, Flyway migrations, other test classes)?
- Run `mvn clean test`. Does the build compile? Count the failures.
- Discuss: why is a partial rename worse than none? When would an IDE rename (`Shift+F6` in IntelliJ) beat Copilot Edits?

Then discard the branch (see the warning above).

---

## Key Learning Points

### ✅ Context-Aware Code Generation

1. **@workspace**: Discover what already exists *and* what is missing before generating
2. **#file**: Reference specific files for consistent patterns
3. **#selection**: Refactor specific code sections
4. **Real types**: Generated code must use `TaskId`, `TaskStatus`, and `TaskResponse.from(task)`; always compile what Copilot gives you

### ✅ Effective Refactoring Workflow

1. **/explain**: Understand code before changing it
2. **Characterization tests first**: Pin what the legacy code *does*, including surprising edges like the `" "` split
3. **/refactor**: Automated refactoring with specific goals and a "keep behavior identical" constraint
4. **Re-run the same tests**: They prove behavior was preserved
5. **Small steps**: Refactor in small steps, run tests frequently

### ✅ Code Quality Improvements

1. **Guard clauses**: Early returns reduce indentation
2. **Enums over magic ints and flags**: Make invalid states unrepresentable
3. **Ports and adapters**: Put I/O behind an interface
4. **Proper exception handling**: Don't swallow exceptions; log and throw, restore interrupt flags
5. **Logging**: SLF4J via `LoggerFactory`
6. **Architecture**: Keep Spring and JPA out of the domain

### ✅ Multi-File Editing

1. **Copilot Edits**: Consistent changes across multiple files
2. **Working set**: An incomplete working set yields a broken partial change
3. **Review**: Always review the diff; use scratch branches for experiments

---

## Extension Exercises (If Time Permits)

### Exercise 1: Add Pagination

Add `page` and `size` parameters to `GET /api/tasks`.

> **Architecture constraint**: do **not** put Spring Data's `Pageable` or `Page<T>` on the `TaskRepository` domain port. That would pull Spring Data into the domain module. Keep pagination in the application/API layers, returning a plain response type such as a `PagedResponse<T>` record (`content`, `page`, `size`, `totalElements`, `totalPages`, `first`, `last`).

Things to discuss and test:

1. Changing the list response from a bare array to an object is a **breaking API change**; which existing tests must be updated?
2. Slicing in memory after loading all rows is fine for a workshop and wrong at scale. Record it as a known gap. How would you push paging into the persistence adapter without leaking `Pageable` into the port (for example a domain-level `PageRequest` value object mapped inside the adapter)?

### Exercise 2: Add Sorting

Add sorting to `GET /api/tasks` over the fields that exist on `Task` today (for example `title` and `createdAt`), ascending and descending. Validate the sort field at the boundary and return 400 for unknown fields.

### Discussion 3: Is a `TaskResponseMapper` Warranted?

A teammate suggests extracting a dedicated `TaskResponseMapper` `@Component` for converting `Task` to `TaskResponse`.

Discuss, using the existing code:

- `TaskResponse.from(Task)` already exists, lives on the DTO that owns the mapping, and every endpoint uses it.
- What would a mapper component add: a new class, an injected dependency, and either duplication of the factory or a redundant delegate?
- When *would* it be justified (several response shapes, mapping needing injected collaborators, versioned DTOs)?

The honest answer for this codebase is **no**. Ask Copilot to argue both sides, then decide. Choosing not to build something is a valid outcome.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ You identified that most of the REST API already exists (POST, GET list, GET by ID, `PATCH` update, state-change `PUT`s, DELETE) and did **not** rebuild it
- ✅ `GET /api/tasks?status=` works for `PENDING`, `IN_PROGRESS`, `COMPLETED`, `CANCELLED`, and returns 400 for invalid values
- ✅ Characterization tests were written and passing **before** refactoring `LegacyTaskProcessor`, including the `" "` edge case
- ✅ `LegacyTaskProcessor` refactored: guard clauses, `ProcessingType` enum, named constants, `StringBuilder`, I/O behind a `TaskOutputWriter` port
- ✅ No swallowed exceptions (log and throw)
- ✅ The same characterization tests still pass after the refactor
- ✅ All tests passing (`mvn clean test`)
- ✅ Domain module still free of Spring/JPA dependencies
- ✅ Code is clean, readable, and maintainable

---

## Troubleshooting

### Copilot Generates Inconsistent Patterns

**Problem**: New code doesn't match existing style  
**Solution**: Use `#file` to reference the existing controller and state "Follow the existing Spring Boot pattern"

### Copilot Generates Code That Doesn't Compile

**Problem**: References `UUID` ids, `TaskStatus.TODO`/`DONE`, `new TaskResponse(...)` or other APIs that don't exist  
**Solution**: Point Copilot at the real types (`#file` on `Task.java`, `TaskStatus.java`, `TaskResponse.java`, `TaskService.java`)

### Refactoring Breaks the Characterization Tests

**Problem**: A characterization test fails after refactoring  
**Solution**: That is the safety net working. Fix the *refactored code* so behavior matches, unless you are deliberately changing behavior (such as surfacing swallowed exceptions); in that case change the test consciously and record why.

### Too Many Changes at Once

**Problem**: Copilot suggests massive refactoring  
**Solution**: Break into smaller steps. Refactor one method at a time. Run tests after each change.

### Maven Build or Run Fails

**Problem**: Compilation errors, stale-jar errors, or the app cannot reach PostgreSQL  
**Solution**: Run `mvn clean install -DskipTests` from `src-springboot`, then `mvn spring-boot:run -pl taskmanager-api -Dspring-boot.run.profiles=dev` (PowerShell: quote the `-D` argument).

### Injecting a `Path` with `@Value` Fails

**Problem**: If you externalize the output directory and inject a `Path` via `@Value`, a web application context may resolve it as a ServletContext resource  
**Solution**: Inject a `String` and convert to `Path` inside the adapter.

---

## Next Steps

Move on to [**Lab 4: Testing, Documentation & Workflow**](lab-04-testing-documentation-workflow-java.md) where you'll:

- Generate comprehensive test suites with `/tests`
- Create documentation with `/doc`
- Write Conventional Commit messages
- Draft PR descriptions with `@workspace`

---

## Additional Resources

### Spring Boot & Java
- [Spring Boot Reference Documentation](https://docs.spring.io/spring-boot/reference/)
- [Spring Data JPA Query Methods](https://docs.spring.io/spring-data/jpa/docs/current/reference/html/#jpa.query-methods)
- [SLF4J Documentation](http://www.slf4j.org/manual.html)

### Clean Code & Patterns
- [Object Calisthenics](https://williamdurand.fr/2013/06/03/object-calisthenics/)
- [Refactoring Techniques](https://refactoring.guru/refactoring/techniques)
- [Characterization Tests](https://michaelfeathers.silvrback.com/characterization-testing)
- [Clean Code Principles](https://www.baeldung.com/java-clean-code)
- [DDD in Spring Boot](https://reflectoring.io/spring-boot-clean-architecture/)

### Cross-Reference
- [.NET version of this lab](lab-03-generation-and-refactoring.md) - Compare patterns and approaches
- [Pattern Translation Guide](../guides/dotnet-to-springboot-patterns.md) - .NET ↔ Spring Boot equivalencies
