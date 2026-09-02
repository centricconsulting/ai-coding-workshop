# Lab 2: From Requirements to Code with GitHub Copilot (Kotlin)

> **💡 Also available**: [shared .NET version](lab-02-requirements-to-code.md) · [JavaScript version](lab-02-requirements-to-code-javascript.md) · [Java/Spring Boot version](lab-02-requirements-to-code-java.md) · [Angular version](lab-02-requirements-to-code-angular.md) · [Python version](lab-02-requirements-to-code-python.md) · [Swift version](lab-02-requirements-to-code-swift.md)

**Duration**: 40-45 minutes  
**Learning Objectives**:

- Turn a plain-language request into small Kotlin code changes
- Extend the Domain layer with a `Priority` enum without breaking layer boundaries
- Add a Kotlin Application-layer create-task workflow with JUnit 5 and MockK tests
- Keep the Kotlin track JVM-only and Gradle-based
- Practice reviewing Copilot output for path accuracy, immutability, and null-safety

---

## Overview

In Lab 1 you practiced TDD in the Kotlin Application layer. In this lab, you will turn a broader feature request into changes across the Domain and Application modules.

For the Kotlin track, the architecture stays close to the existing .NET and Spring Boot examples while remaining idiomatic Kotlin:

- `src-kotlin/task-manager-domain/` holds the `Task` aggregate and value types
- `src-kotlin/task-manager-application/` holds use cases and orchestration
- `src-kotlin/task-manager-infrastructure/` holds the in-memory repository adapter
- there is **no Android module and no API module** in Labs 1-4

The goal is not to transliterate Java into Kotlin. The goal is to preserve the architectural intent while taking advantage of Kotlin's language features.

---

## Prerequisites

- ✅ Lab 1 is complete, or you have an equivalent Kotlin `NotificationService`
- ✅ Starter Domain and Infrastructure code exists under `src-kotlin/`
- ✅ VS Code is open with GitHub Copilot enabled
- ✅ Java 21 and Gradle 8+ are available, or you are using `.devcontainer/kotlin-participant`
- ✅ Baseline verification completed:

```bash
cd src-kotlin
gradle build
```

---

## Part 1: Turn the Request into Clear Rules (8-10 minutes)

### Scenario: Add Priority to a Task

A stakeholder says:

> **User Story**: As a workshop participant, I want each task to have a priority so I can quickly see what matters most.

That statement still needs concrete rules before it becomes code.

### 1.1 Ask Copilot to Break the Story Down

Prompt Copilot:

```text
I have a Kotlin/JVM task manager in src-kotlin/ with Clean Architecture modules:
- task-manager-domain
- task-manager-application
- task-manager-infrastructure

Turn this user story into 4-6 small backlog items:
"As a workshop participant, I want each task to have a priority so I can quickly see what matters most."

Keep the answer beginner-friendly and align it to the existing Kotlin modules.
```

A helpful answer should suggest work like:

- define valid priority values
- add priority to the Domain `Task`
- update task creation rules
- add a create-task use case in the Application layer
- add tests for valid and invalid inputs

### 1.2 Choose a Small, Concrete Ruleset

For this workshop, use these rules:

- valid priorities are `LOW`, `MEDIUM`, and `HIGH`
- new tasks must include a priority
- the Domain `Task` stores the priority
- the Application layer creates and saves the task
- the repository still lives behind the Domain port

---

## Part 2: Add the Domain Concept First (RED -> GREEN) (10-12 minutes)

### 2.1 Ask Copilot for a Priority Enum

Prompt Copilot:

```text
Create a Priority enum for the Kotlin track in src-kotlin/task-manager-domain/.

Requirements:
- Kotlin enum class
- values: LOW, MEDIUM, HIGH
- update the Task aggregate so priority becomes part of task creation and storage
- keep the Domain layer free of framework dependencies
- keep the code beginner-friendly and immutable where practical
```

### 2.2 Expected Output

Copilot should guide you toward something like:

```kotlin
enum class Priority {
    LOW,
    MEDIUM,
    HIGH,
}
```

And the `Task` aggregate should grow to include `priority`:

```kotlin
data class Task private constructor(
    val id: TaskId,
    val title: String,
    val description: String?,
    val priority: Priority,
    val status: TaskStatus,
    val createdAt: Instant,
    val updatedAt: Instant,
    val completedAt: Instant?,
)
```

### 2.3 Write the Domain Test First

Prompt Copilot:

```text
Update the Kotlin Domain tests in
src-kotlin/task-manager-domain/src/test/kotlin/com/example/taskmanager/domain/tasks/.

Add JUnit 5 tests that verify:
- Task.create stores the provided priority
- the default status is still TODO
- updateDetails keeps the existing priority unchanged
```

A representative test might look like:

```kotlin
@Test
@DisplayName("create stores the provided priority")
fun createStoresPriority() {
    val task = Task.create(
        title = "Prepare workshop",
        description = "Add the Kotlin lab track",
        priority = Priority.HIGH,
    )

    assertEquals(Priority.HIGH, task.priority)
    assertEquals(TaskStatus.TODO, task.status)
}
```

### 2.4 Why Start in the Domain Layer?

This keeps the dependency direction clean:

- Domain owns the business concept of priority
- Application orchestrates the workflow
- Infrastructure persists the aggregate

That is the same architectural lesson taught in the other backend-oriented tracks.

---

## Part 3: Create an Application-Layer Workflow (10-12 minutes)

### Scenario: Add a Kotlin Create Task Use Case

The Kotlin track does not need a full CQRS framework. A good workshop compromise is:

- a small `CreateTaskCommand` data class
- a `CreateTaskUseCase` class that depends on `TaskRepository`

### 3.1 Ask Copilot for the Use Case and Tests

Prompt Copilot:

```text
Create a Kotlin application-layer create-task workflow under
src-kotlin/task-manager-application/.

Requirements:
- add a CreateTaskCommand data class with title, description, and priority
- add a CreateTaskUseCase class that depends on TaskRepository
- create the Task aggregate and save it with repository.save(...)
- add JUnit 5 unit tests using MockK
- keep the solution lightweight and workshop-friendly
```

### 3.2 Expected Output

Copilot should generate code along these lines.

**`CreateTask.kt`**

```kotlin
data class CreateTaskCommand(
    val title: String,
    val description: String?,
    val priority: Priority,
)

class CreateTaskUseCase(
    private val repository: TaskRepository,
) {
    fun handle(command: CreateTaskCommand): Task {
        val task = Task.create(
            title = command.title,
            description = command.description,
            priority = command.priority,
        )

        return repository.save(task)
    }
}
```

### 3.3 Add the Use-Case Tests First

Prompt Copilot:

```text
Create JUnit 5 tests for CreateTaskUseCase in the Kotlin Application module.

Requirements:
- use MockK for the TaskRepository
- verify repository.save(...) is called once
- verify the returned task contains the requested title and priority
- keep the tests readable for workshop participants
```

A representative test might look like:

```kotlin
@Test
@DisplayName("handle creates and saves a task")
fun handleCreatesAndSavesTask() {
    val repository = mockk<TaskRepository>()
    every { repository.save(any()) } answers { firstArg() }
    val useCase = CreateTaskUseCase(repository)

    val task = useCase.handle(
        CreateTaskCommand(
            title = "Prepare demo",
            description = "Walk through Kotlin null-safety",
            priority = Priority.MEDIUM,
        ),
    )

    assertEquals(Priority.MEDIUM, task.priority)
    verify(exactly = 1) { repository.save(any()) }
}
```

### 3.4 Why Not Add More Infrastructure Yet?

Because this lab is teaching:

- requirement translation
- Domain-first design
- Application-layer orchestration
- test-first thinking with Kotlin tools

A single command plus use case gives you those lessons without extra ceremony.

---

## Part 4: Extend the In-Memory Repository (8-10 minutes)

### 4.1 Ask Copilot for the Smallest Required Repository Change

Prompt Copilot:

```text
Update the Kotlin InMemoryTaskRepository so it supports the Priority changes.

Requirements:
- preserve existing behavior
- keep storage in a mutable map
- do not move business rules into Infrastructure
- update or add JUnit 5 tests if needed
```

### 4.2 Expected Output

If your repository already stores whole `Task` aggregates, the code change should be very small.

That is a good architectural sign: when the Domain model grows, Infrastructure should often need only minimal updates.

---

## Verify Your Work

Run the Kotlin track tests:

```bash
cd src-kotlin
gradle test
```

**Expected result**: ✅ tests pass.

---

## Key Learning Points

### ✅ What This Lab Teaches

1. plain-language requirements still need explicit rules
2. `Priority` belongs in the Domain layer
3. Kotlin can mirror Clean Architecture without copying Java ceremony line-for-line
4. MockK keeps Application-layer tests focused and readable
5. immutable aggregates remain easy to evolve when the rules are clear

### ✅ What to Watch for in Copilot Output

- correct `src-kotlin/` paths
- `Priority` living in Domain, not Infrastructure
- use cases depending on `TaskRepository`, not concrete implementations
- readable data classes instead of unnecessary framework scaffolding
- Kotlin null-safety and trimming logic staying explicit

---

## Troubleshooting

### Copilot Put `Priority` in the Application Module

**Problem**: the enum was generated next to the use case  
**Solution**: move it to `task-manager-domain` and remind Copilot that priority is a business concept, not an orchestration concern.

### The Use Case Depends on `InMemoryTaskRepository`

**Problem**: Application now imports a concrete Infrastructure class  
**Solution**: refine the prompt with: _"Depend on the Domain `TaskRepository` interface only."_

### MockK Syntax Became Mockito Syntax

**Problem**: Copilot generated `when(...).thenReturn(...)`  
**Solution**: restate the library choice: _"Use MockK only, with `every { ... }` and `verify { ... }`."_

### Copilot Added Android Types

**Problem**: suggestions reference `Parcelable`, `ViewModel`, or `Context`  
**Solution**: restate the track boundary: _"This is a JVM-only workshop track. Do not use Android SDK classes."_

---

## Next Steps

Continue to [**Lab 3: Code Generation & Refactoring (Kotlin)**](lab-03-generation-and-refactoring-kotlin.md), where you'll generate a task-listing workflow and refactor the legacy null-check-heavy processor into idiomatic Kotlin.

---

## Additional Resources

- [Kotlin Data Classes](https://kotlinlang.org/docs/data-classes.html)
- [MockK Documentation](https://mockk.io/)
- [JUnit 5 User Guide](https://junit.org/junit5/docs/current/user-guide/)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
