# Lab 4: Testing, Documentation & Workflow with GitHub Copilot (Kotlin)

> **💡 Also available**: [shared .NET version](lab-04-testing-documentation-workflow.md) · [JavaScript version](lab-04-testing-documentation-workflow-javascript.md) · [Java/Spring Boot version](lab-04-testing-documentation-workflow-java.md) · [Angular version](lab-04-testing-documentation-workflow-angular.md) · [Python version](lab-04-testing-documentation-workflow-python.md)

**Duration**: 15-20 minutes  
**Learning Objectives**:

- Expand Kotlin test coverage with reusable JUnit 5 setup
- Use Copilot to add clear KDoc without over-documenting
- Keep documentation aligned with the real `src-kotlin/` modules and Gradle commands
- Draft Conventional Commit and PR text for the Kotlin track
- Treat testing, documentation, and workflow as part of the feature, not cleanup work

---

## Overview

This lab focuses on the finishing work that often gets skipped when people are in a hurry:

1. **Testing** - strengthen confidence in your Domain, Application, and Infrastructure code
2. **Documentation** - add useful KDoc to public Kotlin types
3. **Workflow** - stage the right files and prepare clear commit or PR text

For the Kotlin track, the workflow stays close to the code you built in earlier labs:

- Domain code lives under `src-kotlin/task-manager-domain/`
- Application code lives under `src-kotlin/task-manager-application/`
- Infrastructure code lives under `src-kotlin/task-manager-infrastructure/`
- verification runs with Gradle and JUnit 5

---

## Prerequisites

- ✅ Labs 1-3 are complete, or you have equivalent Kotlin-track code
- ✅ `gradle test` already runs successfully from `src-kotlin/`
- ✅ You are comfortable asking Copilot for tests, docs, and workflow text

Baseline verification:

```bash
cd src-kotlin
gradle test
```

---

## Part 1: Expand Test Coverage with Shared Setup (6-7 minutes)

### Scenario: Reduce Test Repetition Without Hiding Intent

By now, the Kotlin track should include:

- Domain tests around `Task`
- Application tests for one or more use cases
- Infrastructure tests for `InMemoryTaskRepository`

This is a good moment to reduce repeated setup while keeping the tests explicit.

### 1.1 Ask Copilot for JUnit 5 Fixture Ideas

Prompt Copilot:

```text
Review the Kotlin tests under src-kotlin/.
Suggest a small JUnit 5 cleanup using @BeforeEach or shared builders where it reduces duplication.

Requirements:
- keep the tests workshop-friendly
- do not hide the important assertions
- stay within JUnit 5 and MockK
```

### 1.2 Expected Output

A representative test class might move repeated setup into `@BeforeEach`:

```kotlin
@DisplayName("CreateTaskUseCase")
class CreateTaskUseCaseTest {
    private lateinit var repository: TaskRepository
    private lateinit var useCase: CreateTaskUseCase

    @BeforeEach
    fun setUp() {
        repository = mockk()
        useCase = CreateTaskUseCase(repository)
    }

    @Test
    @DisplayName("handle creates and saves a task")
    fun handleCreatesAndSavesTask() {
        every { repository.save(any()) } answers { firstArg() }

        val task = useCase.handle(
            CreateTaskCommand("Draft Kotlin guide", "Add screenshots", Priority.HIGH),
        )

        assertEquals(Priority.HIGH, task.priority)
    }
}
```

### 1.3 Good Test Ideas for This Track

Strong additions usually cover things like:

- `ListActiveTasksUseCase` returns tasks in descending `createdAt` order
- `Task.updateStatus(...)` keeps `completedAt` stable after completion
- `LegacyTaskProcessor` preserves its public output after the null-safety refactor
- repository tests prove completed and cancelled tasks are excluded from active results

---

## Part 2: Add Useful KDoc (4-5 minutes)

### Scenario: Document the Why, Not the Obvious

In the Kotlin track, a better fit than long prose comments is short, accurate KDoc on:

- public use cases
- Domain types with business rules
- Infrastructure helpers with non-obvious legacy behavior

### 2.1 Ask Copilot for Focused KDoc

Prompt Copilot:

```text
Review the Kotlin track under src-kotlin/ and suggest concise KDoc for the public
application classes, domain types, and any non-obvious infrastructure helpers.

Only add documentation where it explains intent or business rules.
Do not add noisy comments that just repeat the code.
```

### 2.2 Expected Output

A useful result might look like:

```kotlin
/**
 * Creates and persists a new task using the configured repository.
 */
class CreateTaskUseCase(
    private val repository: TaskRepository,
) {
    /**
     * Builds a task from application input and stores it through the domain port.
     */
    fun handle(command: CreateTaskCommand): Task = repository.save(...)
}
```

Or for the Domain layer:

```kotlin
/**
 * Aggregate root representing work tracked in the workshop task manager.
 */
data class Task ...
```

### 2.3 Keep KDoc Short and Honest

Useful KDoc explains:

- what the type or function is for
- what the important inputs mean
- what business rule matters
- what return value the caller can expect

Avoid comments that just restate the symbol name.

---

## Part 3: Review Paths and Commands Carefully (2-3 minutes)

Try a prompt like:

```text
Review the Kotlin track documentation and KDoc under src-kotlin/.
Point out any mismatch between the documented Gradle commands, file paths, and the actual module layout.
Keep suggestions concise.
```

What you want to catch:

- paths that forgot the `src-kotlin/` prefix
- commands that mention Maven, pytest, or Android tooling by mistake
- comments that talk about an API or UI layer that this track does not include yet

---

## Part 4: Stage Files and Draft Workflow Text (3-4 minutes)

### 4.1 Stage the Relevant Files

A typical Kotlin-track change set might include:

```bash
git add src-kotlin/task-manager-domain/
git add src-kotlin/task-manager-application/
git add src-kotlin/task-manager-infrastructure/
git add docs/labs/lab-0*-kotlin.md
```

### 4.2 Ask Copilot for a Commit Message

Prompt Copilot:

```text
Write a Conventional Commit message for the Kotlin workshop track.
The change expanded JUnit coverage, added KDoc, and aligned workflow documentation.
Include a short subject and a helpful body.
```

A good result might look like:

```text
docs(kotlin): expand tests and documentation workflow

- add JUnit-focused guidance for the Kotlin track
- document key public types with concise KDoc
- align workflow notes with the Gradle multi-module layout
```

### 4.3 Ask for a PR Description Draft

Prompt Copilot Chat:

```text
@workspace Draft a pull request description for the Kotlin workshop track.
Include:
- summary of testing improvements
- documentation updates
- validation performed with gradle test
- a short reviewer checklist
Use Markdown.
```

Review the output before using it. Copilot is a drafting partner, not the final approver.

---

## Verify Your Work

Run the Kotlin track tests:

```bash
cd src-kotlin
gradle test
```

**Expected result**: ✅ tests pass.

---

## Summary

By the end of this lab, the Kotlin track should feel complete in the same way as the other tracks:

- test-first changes
- explicit boundaries
- readable documentation
- clean workflow habits

The exact syntax changed, but the engineering discipline stayed the same.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ Kotlin tests use small shared setup where it helps
- ✅ public Kotlin types or use cases have concise, useful KDoc
- ✅ documented commands and paths match the real `src-kotlin/` layout
- ✅ you can generate a Conventional Commit and PR draft for the Kotlin track
- ✅ `gradle test` still passes

---

## Troubleshooting

### The Test Setup Became Too Abstract

**Problem**: helper methods now hide what each test is doing  
**Solution**: keep only the repeated setup in `@BeforeEach`. Leave important assertions and scenario-specific data in the test body.

### Copilot Added Too Much KDoc

**Problem**: every private helper now has a long comment  
**Solution**: keep KDoc for public or non-obvious code paths. Remove anything that only repeats the implementation.

### The Suggested Commands Mention the Wrong Toolchain

**Problem**: generated text references Maven, pytest, or Android Studio  
**Solution**: correct every command to use `gradle` and compare the draft against the real `src-kotlin/` module layout.

### MockK and Mockito Got Mixed Together

**Problem**: tests compile conceptually but the examples mix libraries  
**Solution**: restate the rule: _"Use JUnit 5 and MockK only for the Kotlin track."_

---

## Next Steps

Review the full Kotlin sequence again if you want to polish the track end to end:

1. [Lab 1: TDD with GitHub Copilot (Kotlin)](lab-01-tdd-with-copilot-kotlin.md)
2. [Lab 2: From Requirements to Code (Kotlin)](lab-02-requirements-to-code-kotlin.md)
3. [Lab 3: Code Generation & Refactoring (Kotlin)](lab-03-generation-and-refactoring-kotlin.md)

After that, continue into the shared advanced labs in [Part 2](../FACILITATOR_GUIDE_PART2.md).

---

## Additional Resources

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Kotlin Coding Conventions](https://kotlinlang.org/docs/coding-conventions.html)
- [JUnit 5 User Guide](https://junit.org/junit5/docs/current/user-guide/)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
