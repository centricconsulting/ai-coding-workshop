# Lab 3: Code Generation & Refactoring with GitHub Copilot (Kotlin)

> **💡 Also available**: [shared .NET version](lab-03-generation-and-refactoring.md) · [JavaScript version](lab-03-generation-and-refactoring-javascript.md) · [Java/Spring Boot version](lab-03-generation-and-refactoring-java.md) · [Angular version](lab-03-generation-and-refactoring-angular.md) · [Python version](lab-03-generation-and-refactoring-python.md) · [Swift version](lab-03-generation-and-refactoring-swift.md)

**Duration**: 35-45 minutes  
**Learning Objectives**:

- Use Copilot to generate the next Kotlin Application-layer workflow from existing context
- Add a query for listing active tasks without introducing a web or Android UI layer
- Refactor a Kotlin-specific null-safety smell instead of reusing another track's legacy-class exercise
- Protect behavior with JUnit 5 characterization tests before cleanup
- Practice asking Copilot for focused Kotlin refactors instead of broad rewrites

---

## 📝 Plan First Before You Refactor

Before changing code across multiple files, ask Copilot for a plan first.

Example prompt:

```text
Propose a step-by-step plan for the Kotlin track.
I need to:
- add an application-layer workflow for listing active tasks in src-kotlin/
- write characterization tests for the legacy processor in task-manager-infrastructure
- refactor the null-check-heavy Kotlin code into idiomatic null-safe Kotlin

Keep the architecture aligned with the existing Domain/Application/Infrastructure modules.
```

A good plan should mention:

- which existing repository methods can be reused
- which tests should protect the current behavior first
- where Kotlin safe calls, elvis operators, or a sealed result type would help
- how to keep the refactor Kotlin-focused rather than copying the .NET or Python versions

---

## Overview

In this lab, you work on two different Copilot strengths:

1. **Generation** - creating the next slice of Application-layer behavior
2. **Refactoring** - cleaning up code that works but ignores Kotlin's best features

For the Kotlin track, the refactoring target is intentionally different from the other tracks.

Instead of a fat function or a large legacy class, you start with a **null-safety smell** in:

- `src-kotlin/task-manager-infrastructure/src/main/kotlin/com/example/taskmanager/infrastructure/LegacyTaskProcessor.kt`

The legacy code behaves more like nullable Java than idiomatic Kotlin:

- many `String?` and `Int?` inputs
- repeated `if (x != null)` checks
- blank-string handling repeated inline
- no use of safe calls (`?.`) or the elvis operator (`?:`)
- no meaningful result type for update processing

That makes this a great Kotlin-specific lesson.

---

## Prerequisites

- ✅ Labs 1 and 2 are complete, or you have equivalent Kotlin-track code
- ✅ `TaskRepository` and `InMemoryTaskRepository` already exist under `src-kotlin/`
- ✅ Baseline verification completed:

```bash
cd src-kotlin
gradle build
```

---

## Part 1: Generate a "List Active Tasks" Workflow (10-12 minutes)

### Scenario: Return Active Tasks from the Application Layer

The Kotlin track has no API or Android UI shell in Labs 1-4, so the generation exercise happens in the Application layer.

Your repository already exposes a business-intent method:

- `findActiveTasks()`

Now you want an Application-layer workflow that returns active tasks ordered from newest to oldest.

### 1.1 Ask Copilot to Explain the Existing Path

Prompt Copilot:

```text
@workspace Show me how the Kotlin track currently stores and retrieves tasks.
Explain which files live in task-manager-domain, task-manager-application, and task-manager-infrastructure.
Then suggest the smallest clean way to add a list-active-tasks use case.
```

A helpful answer should point you toward:

- `TaskRepository.findActiveTasks()` in the Domain contract
- a small Application-layer query class or function
- tests that sort tasks by `createdAt` descending

### 1.2 Ask Copilot for the Application-Layer Query

Prompt Copilot:

```text
Create a Kotlin application-layer workflow for listing active tasks.

Requirements:
- use the existing TaskRepository
- call findActiveTasks()
- return tasks ordered by createdAt descending
- keep the code lightweight and JVM-only
- place the code under src-kotlin/task-manager-application/
- add JUnit 5 tests using MockK
```

### 1.3 Expected Output

A clean result might look like this:

```kotlin
class ListActiveTasksUseCase(
    private val repository: TaskRepository,
) {
    fun handle(): List<Task> =
        repository
            .findActiveTasks()
            .sortedByDescending(Task::createdAt)
}
```

This is intentionally smaller than a full query bus or controller. It serves the same purpose with less ceremony.

---

## Part 2: Add Characterization Tests Before the Refactor (8-10 minutes)

### Scenario: Protect the Legacy Processor First

Before you clean up `LegacyTaskProcessor.kt`, make sure JUnit captures the current behavior.

### 2.1 Ask Copilot for Test Ideas

Prompt Copilot:

```text
Review the Kotlin file
src-kotlin/task-manager-infrastructure/src/main/kotlin/com/example/taskmanager/infrastructure/LegacyTaskProcessor.kt

Suggest characterization tests I should add before refactoring.
Focus on the current string output for missing, blank, and present nullable fields.
Keep the tests lightweight and deterministic.
```

Good test ideas usually cover:

- `null` request returns the current fallback value
- blank title becomes `(blank)` while missing title becomes `(missing)`
- positive estimates append `Xm`
- `DONE` maps to `state=complete`
- notification output is skipped when the channel or recipient is missing

### 2.2 Create a Baseline Test File

A representative test file might include:

```kotlin
@Test
@DisplayName("processUpdate uses missing markers for absent nullable fields")
fun processUpdateUsesMissingMarkers() {
    val processor = LegacyTaskProcessor()

    val result = processor.processUpdate(
        LegacyTaskUpdateRequest(
            taskId = null,
            title = null,
            description = null,
            estimateMinutes = null,
            assignee = null,
            status = null,
            notifyChannel = null,
            notifyRecipient = null,
        ),
    )

    assertEquals(
        "Task unknown | title=(missing) | description=(missing) | estimate=unknown | assignee=unassigned | state=open | notify=skipped",
        result,
    )
}
```

### 2.3 Run the Tests

```bash
cd src-kotlin
gradle test
```

**Expected result**: ✅ tests pass before you refactor.

That passing baseline is your safety net.

---

## Part 3: Refactor Toward Idiomatic Kotlin Null-Safety (12-15 minutes)

### 3.1 Why This Smell Matters in Kotlin

The original code is not just verbose. It misses one of Kotlin's biggest advantages:

- null-safety as a language feature

If Kotlin code is filled with manual null checks everywhere, you lose readability and much of the value of the language. The refactor in this lab should move the code toward:

- safe calls (`?.`)
- elvis operators (`?:`)
- `takeIf { ... }`
- small helper functions
- optionally, a sealed class or `Result`-style outcome if the logic becomes more explicit

### 3.2 Ask Copilot for a Focused Refactor

Prompt Copilot:

```text
/refactor Refactor the Kotlin LegacyTaskProcessor.

Requirements:
- keep the external behavior of processUpdate the same
- replace repeated manual null checks with idiomatic Kotlin null-safety
- use safe calls, elvis operators, and small helper functions where they improve clarity
- keep the code workshop-friendly, not overly clever
- do not introduce Android APIs or coroutines
```

### 3.3 What a Good Refactor Usually Produces

A clean Kotlin result often introduces helpers such as:

- `normalizeText(...)`
- `formatTitle(...)`
- `formatDescription(...)`
- `formatEstimate(...)`
- `formatNotification(...)`

The main function should start to read more like a short recipe:

```kotlin
fun processUpdate(request: LegacyTaskUpdateRequest?): String {
    val safeRequest = request ?: return ""

    return listOf(
        formatTaskId(safeRequest.taskId),
        formatTitle(safeRequest.title),
        formatDescription(safeRequest.description),
        formatEstimate(safeRequest.estimateMinutes),
        formatAssignee(safeRequest.assignee),
        formatStatus(safeRequest.status),
        formatNotification(safeRequest.notifyChannel, safeRequest.notifyRecipient),
    ).joinToString(" | ")
}
```

That is the core idea of the refactor:

- fewer scattered null checks
- clearer intent
- Kotlin features doing the work the language was designed for

### 3.4 Keep the Refactor Beginner-Friendly

This lab is **not** asking for the cleverest possible Kotlin.

Avoid:

- advanced functional libraries
- dense scope-function chains that are hard to explain
- coroutine rewrites
- changing the observable behavior during the cleanup

---

## Part 4: Re-run, Review, and Explain the Improvement (6-8 minutes)

### 4.1 Run the Tests Again

```bash
cd src-kotlin
gradle test
```

**Expected result**: ✅ tests still pass.

### 4.2 Ask Copilot for a Final Review

Prompt Copilot:

```text
/check Review the refactored Kotlin legacy module.
Explain whether the null-safety improvements made the code more idiomatic and readable.
Suggest only small improvements.
```

Good suggestions may include:

- turning repeated fallback strings into named constants
- renaming a helper to better match its behavior
- deciding whether a sealed result type would improve clarity
- trimming one more repeated `String` operation

### 4.3 Explain the Improvement in Plain Language

A good workshop explanation sounds like this:

> The code still returns the same summary, but now Kotlin's null-safety features express the defaults directly. That makes the logic easier to read, easier to test, and more obviously Kotlin.

---

## Verify Your Work

Run the Kotlin track tests:

```bash
cd src-kotlin
gradle test
```

**Expected result**: ✅ tests pass after both the new Application-layer query and the legacy refactor.

---

## Key Learning Points

### ✅ What You Practiced

1. generating the next Application-layer slice from existing context
2. reusing a Domain repository method through a small use case
3. protecting a refactor with tests before cleanup
4. replacing manual null checks with idiomatic Kotlin null-safety
5. keeping Kotlin refactors readable instead of clever

### ✅ What This Lab Is Not About

This lab is **not** about turning every refactor into a framework exercise. In the Kotlin track, better structure usually means:

- smaller helpers
- better names
- clearer defaults
- fewer manual null checks

---

## Troubleshooting

### Copilot Tried to Add an API or Android Screen

**Problem**: the generated solution introduced a controller, route, or Activity  
**Solution**: restate the track boundary: _"Labs 1-4 are JVM-only. Keep this in Domain/Application/Infrastructure only."_

### The Refactor Changed Behavior

**Problem**: tests started failing after the cleanup  
**Solution**: use the characterization tests as the contract. Restore the original output first, then continue improving the internals.

### Copilot Replaced the Code with Clever Scope-Function Chains

**Problem**: the result became hard to explain to beginners  
**Solution**: narrow the request: _"Prefer small named helper functions over dense `let`/`run`/`also` chains."_

### The Code Still Has Repeated `if (x != null)` Checks

**Problem**: the refactor only rearranged the old structure  
**Solution**: explicitly ask Copilot to use `?.`, `?:`, and `takeIf { ... }` so the language features remove the repetition.

---

## Next Steps

Continue to [**Lab 4: Testing, Documentation & Workflow (Kotlin)**](lab-04-testing-documentation-workflow-kotlin.md), where you'll expand JUnit coverage, add KDoc, and finish the Kotlin track with a clean review workflow.

---

## Additional Resources

- [Kotlin Null Safety](https://kotlinlang.org/docs/null-safety.html)
- [MockK Documentation](https://mockk.io/)
- [JUnit 5 User Guide](https://junit.org/junit5/docs/current/user-guide/)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
