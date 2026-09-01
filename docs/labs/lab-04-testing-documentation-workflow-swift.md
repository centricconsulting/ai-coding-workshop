# Lab 4: Testing, Documentation & Workflow with GitHub Copilot (Swift)

> **💡 Also available**: [shared .NET version](lab-04-testing-documentation-workflow.md) · [JavaScript version](lab-04-testing-documentation-workflow-javascript.md) · [Java/Spring Boot version](lab-04-testing-documentation-workflow-java.md) · [Angular version](lab-04-testing-documentation-workflow-angular.md) · [Python version](lab-04-testing-documentation-workflow-python.md) · [Kotlin version](lab-04-testing-documentation-workflow-kotlin.md)

**Duration**: 15-20 minutes  
**Learning Objectives**:

- Expand Swift test coverage with small shared XCTest setup
- Use Copilot to add clear DocC-style `///` comments without over-documenting
- Keep documentation aligned with the real `src-swift/` modules and Xcode / SwiftPM commands
- Draft Conventional Commit and PR text for the Swift track
- Treat testing, documentation, and workflow as part of the feature, not cleanup work

---

## Overview

This lab focuses on the finishing work that often gets skipped when people are in a hurry:

1. **Testing** - strengthen confidence in your Domain, Application, and Infrastructure code
2. **Documentation** - add useful DocC-style comments to public Swift types
3. **Workflow** - stage the right files and prepare clear commit or PR text

For the Swift track, the workflow stays close to the code you built in earlier labs:

- Domain code lives under `src-swift/Sources/TaskManagerDomain/`
- Application code lives under `src-swift/Sources/TaskManagerApplication/`
- Infrastructure code lives under `src-swift/Sources/TaskManagerInfrastructure/`
- the app shell lives under `src-swift/TaskManagerApp/`
- verification runs with `swift test`, plus Xcode for the iOS shell

---

## Prerequisites

- ✅ Labs 1-3 are complete, or you have equivalent Swift-track code
- ✅ `swift test` already runs successfully from `src-swift/`
- ✅ You are comfortable asking Copilot for tests, docs, and workflow text

Baseline verification:

```bash
cd src-swift
swift test
```

---

## Part 1: Expand Test Coverage with Shared Setup (6-7 minutes)

### Scenario: Reduce Test Repetition Without Hiding Intent

By now, the Swift track should include:

- Domain tests around `Task`
- Application tests for one or more use cases
- Infrastructure tests for `InMemoryTaskRepository`

This is a good moment to reduce repeated setup while keeping the tests explicit.

### 1.1 Ask Copilot for XCTest Fixture Ideas

Prompt Copilot:

```text
Review the Swift tests under src-swift/.
Suggest a small XCTest cleanup using setUp(), helper builders, or shared sample data where it reduces duplication.

Requirements:
- keep the tests workshop-friendly
- do not hide the important assertions
- stay within XCTest only
```

### 1.2 Expected Output

A representative test class might move repeated setup into `setUp()`:

```swift
final class CreateTaskUseCaseTests: XCTestCase {
    private var repository: InMemoryTaskRepository!
    private var useCase: CreateTaskUseCase!

    override func setUp() {
        super.setUp()
        repository = InMemoryTaskRepository()
        useCase = CreateTaskUseCase(repository: repository)
    }

    func testExecuteCreatesAndSavesTask() throws {
        let task = try useCase.execute(
            CreateTaskCommand(
                title: "Draft Swift guide",
                description: "Add screenshots"
            )
        )

        XCTAssertEqual(task.title, "Draft Swift guide")
    }
}
```

### 1.3 Good Test Ideas for This Track

Strong additions usually cover things like:

- `ListActiveTasksUseCase` returns tasks in descending `createdAt` order
- `Task.updatingStatus(...)` keeps `completedAt` stable after completion
- `LegacyTaskProcessor` preserves its public output after the optional-safety refactor
- repository tests prove completed and cancelled tasks are excluded from active results

---

## Part 2: Add Useful DocC-Style Comments (4-5 minutes)

### Scenario: Document the Why, Not the Obvious

In the Swift track, a better fit than long prose comments is short, accurate `///` documentation on:

- public use cases
- Domain types with business rules
- Infrastructure helpers with non-obvious legacy behavior

### 2.1 Ask Copilot for Focused Documentation

Prompt Copilot:

```text
Review the Swift track under src-swift/ and suggest concise DocC-style comments for the public
application types, domain types, and any non-obvious infrastructure helpers.

Only add documentation where it explains intent or business rules.
Do not add noisy comments that just repeat the code.
```

### 2.2 Expected Output

A useful result might look like:

```swift
/// Creates and persists a new task using the configured repository.
public final class CreateTaskUseCase {
    /// Builds a task from application input and stores it through the domain port.
    public func execute(_ command: CreateTaskCommand) throws -> Task {
        ...
    }
}
```

Or for the Domain layer:

```swift
/// Aggregate root representing work tracked in the workshop task manager.
public struct Task {
    ...
}
```

### 2.3 Keep the Comments Short and Honest

Useful Swift documentation explains:

- what the type or function is for
- what the important inputs mean
- what business rule matters
- what a caller can expect back

Avoid comments that just restate the symbol name.

---

## Part 3: Review Paths and Commands Carefully (2-3 minutes)

Try a prompt like:

```text
Review the Swift track documentation and comments under src-swift/ (including src-swift/TaskManagerApp/).
Point out any mismatch between the documented SwiftPM commands, Xcode paths, and the actual layout.
Keep suggestions concise.
```

What you want to catch:

- paths that forgot the `src-swift/` prefix
- comments that reference UIKit or server endpoints by mistake
- commands that mention Gradle, Maven, pytest, or a devcontainer
- docs that forget the app shell lives in `src-swift/TaskManagerApp/TaskManagerApp.xcodeproj`

---

## Part 4: Stage Files and Draft Workflow Text (3-4 minutes)

### 4.1 Stage the Relevant Files

A typical Swift-track change set might include:

```bash
git add src-swift/
git add src-swift/TaskManagerApp/
git add docs/labs/lab-0*-swift.md
git add docs/LOCAL_SETUP_SWIFT.md
```

### 4.2 Ask Copilot for a Commit Message

Prompt Copilot:

```text
Write a Conventional Commit message for the Swift workshop track.
The change expanded XCTest coverage, added DocC comments, and aligned workflow documentation.
Include a short subject and a helpful body.
```

A good result might look like:

```text
docs(swift): expand tests and documentation workflow

- add XCTest-focused guidance for the Swift track
- document key public types with concise DocC comments
- align workflow notes with the Swift package and Xcode app shell
```

### 4.3 Ask for a PR Description Draft

Prompt Copilot Chat:

```text
@workspace Draft a pull request description for the Swift workshop track.
Include:
- summary of testing improvements
- documentation updates
- validation performed with swift test and an Xcode app run
- a short reviewer checklist
Use Markdown.
```

Review the output before using it. Copilot is a drafting partner, not the final approver.

---

## Verify Your Work

Run the Swift package tests:

```bash
cd src-swift
swift test
```

If you touched the app shell, run `src-swift/TaskManagerApp/TaskManagerApp.xcodeproj` in Xcode and confirm the UI still lists, adds, and completes tasks.

---

## Summary

By the end of this lab, the Swift track should feel complete in the same way as the other tracks:

- test-first changes
- explicit boundaries
- useful documentation
- clean workflow habits

The syntax changed, but the engineering discipline stayed the same.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ Swift tests use small shared setup where it helps
- ✅ public Swift types or use cases have concise, useful `///` documentation
- ✅ documented commands and paths match the real `src-swift/` (including `src-swift/TaskManagerApp/`) layout
- ✅ you can generate a Conventional Commit and PR draft for the Swift track
- ✅ `swift test` still passes

---

## Troubleshooting

### The Test Setup Became Too Abstract

**Problem**: helper methods now hide what each test is doing  
**Solution**: keep only the repeated setup in `setUp()`. Leave important assertions and scenario-specific data in the test body.

### Copilot Added Too Much Documentation

**Problem**: every private helper now has a long comment  
**Solution**: keep `///` comments for public or non-obvious code paths. Remove anything that only repeats the implementation.

### The Suggested Commands Mention the Wrong Toolchain

**Problem**: generated text references Gradle, pytest, or a devcontainer  
**Solution**: correct every command to use `swift test`, `swift build`, or Xcode, and compare the draft against the real layout.

### The UI Notes Started Explaining Business Logic

**Problem**: documentation suggests validating tasks inside SwiftUI views  
**Solution**: restate the architecture rule: _"Keep business rules in the Swift package; keep SwiftUI focused on input and display."_

---

## Next Steps

Review the full Swift sequence again if you want to polish the track end to end:

1. [Lab 1: TDD with GitHub Copilot (Swift)](lab-01-tdd-with-copilot-swift.md)
2. [Lab 2: From Requirements to Code (Swift)](lab-02-requirements-to-code-swift.md)
3. [Lab 3: Code Generation & Refactoring (Swift)](lab-03-generation-and-refactoring-swift.md)

After that, continue into the shared advanced labs in [Part 2](../FACILITATOR_GUIDE_PART2.md).

---

## Additional Resources

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Writing Symbol Documentation in Your Source Files](https://developer.apple.com/documentation/xcode/writing-symbol-documentation-in-your-source-files)
- [XCTest Documentation](https://developer.apple.com/documentation/xctest)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
