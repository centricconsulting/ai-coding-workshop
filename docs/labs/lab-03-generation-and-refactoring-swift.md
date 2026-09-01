# Lab 3: Code Generation & Refactoring with GitHub Copilot (Swift)

> **💡 Also available**: [shared .NET version](lab-03-generation-and-refactoring.md) · [JavaScript version](lab-03-generation-and-refactoring-javascript.md) · [Java/Spring Boot version](lab-03-generation-and-refactoring-java.md) · [Angular version](lab-03-generation-and-refactoring-angular.md) · [Python version](lab-03-generation-and-refactoring-python.md) · [Kotlin version](lab-03-generation-and-refactoring-kotlin.md)

**Duration**: 35-45 minutes  
**Learning Objectives**:

- Use Copilot to generate the next Swift Application-layer workflow from existing context
- Add a query for listing active tasks without introducing extra UI logic
- Refactor a Swift-specific optional and error-handling smell instead of copying another track's legacy-class exercise
- Protect behavior with XCTest characterization tests before cleanup
- Practice asking Copilot for focused Swift refactors instead of broad rewrites

---

## 📝 Plan First Before You Refactor

Before changing code across multiple files, ask Copilot for a plan first.

Example prompt:

```text
Propose a step-by-step plan for the Swift track.
I need to:
- add an application-layer workflow for listing active tasks in src-swift/
- write characterization tests for the legacy processor in TaskManagerInfrastructure
- refactor the force-unwrapped Swift code into safer optional handling and typed errors

Keep the architecture aligned with the existing Domain/Application/Infrastructure modules.
```

A good plan should mention:

- which existing repository methods can be reused
- which tests should protect the current behavior first
- where `guard let`, `if let`, or small helpers would help
- whether `throws` or `Result` would clarify failure paths
- how to keep the refactor Swift-focused rather than copying the .NET or Kotlin versions

---

## Overview

In this lab, you work on two different Copilot strengths:

1. **Generation** - creating the next slice of Application-layer behavior
2. **Refactoring** - cleaning up code that works but ignores Swift's best features

For the Swift track, the refactoring target is intentionally different from the other tracks.

Instead of a fat controller or server endpoint, you start with a **Swift-specific legacy smell** in:

- `src-swift/Sources/TaskManagerInfrastructure/LegacyTaskProcessor.swift`

The legacy code behaves more like unsafe, pre-Swift-idiom code than modern Swift:

- many `String?` and `Int?` inputs
- repeated `request!` force unwraps
- blank-string handling repeated inline
- stringly typed status handling
- no meaningful error type for bad input or missing data

That makes this a good Swift-specific lesson.

---

## Prerequisites

- ✅ Labs 1 and 2 are complete, or you have equivalent Swift-track code
- ✅ `TaskRepository` and `InMemoryTaskRepository` already exist under `src-swift/`
- ✅ Baseline verification completed:

```bash
cd src-swift
swift build
swift test
```

---

## Part 1: Generate a "List Active Tasks" Workflow (10-12 minutes)

### Scenario: Return Active Tasks from the Application Layer

The Swift track already includes a minimal app shell, but this generation exercise still belongs in the Application layer.

Your repository already exposes a business-intent method:

- `findActiveTasks()`

Now you want an Application-layer workflow that returns active tasks ordered from newest to oldest.

### 1.1 Ask Copilot to Explain the Existing Path

Prompt Copilot:

```text
@workspace Show me how the Swift track currently stores and retrieves tasks.
Explain which files live in TaskManagerDomain, TaskManagerApplication, TaskManagerInfrastructure,
and TaskManagerApp.
Then suggest the smallest clean way to add a list-active-tasks use case.
```

A helpful answer should point you toward:

- `TaskRepository.findActiveTasks()` in the Domain contract
- a small Application-layer query type
- tests that sort tasks by `createdAt` descending

### 1.2 Ask Copilot for the Application-Layer Query

Prompt Copilot:

```text
Create a Swift application-layer workflow for listing active tasks.

Requirements:
- use the existing TaskRepository
- call findActiveTasks()
- return tasks ordered by createdAt descending
- keep the code lightweight and package-friendly
- place the code under src-swift/Sources/TaskManagerApplication/
- add XCTest cases
```

### 1.3 Expected Output

A clean result might look like this:

```swift
public final class ListActiveTasksUseCase {
    private let repository: any TaskRepository

    public init(repository: any TaskRepository) {
        self.repository = repository
    }

    public func execute() -> [Task] {
        repository
            .findActiveTasks()
            .sorted { $0.createdAt > $1.createdAt }
    }
}
```

This is intentionally smaller than a full query bus or view model. It serves the same purpose with less ceremony.

---

## Part 2: Add Characterization Tests Before the Refactor (8-10 minutes)

### Scenario: Protect the Legacy Processor First

Before you clean up `LegacyTaskProcessor.swift`, make sure XCTest captures the current behavior.

### 2.1 Ask Copilot for Test Ideas

Prompt Copilot:

```text
Review the Swift file
src-swift/Sources/TaskManagerInfrastructure/LegacyTaskProcessor.swift

Suggest characterization tests I should add before refactoring.
Focus on the current string output for missing, blank, and present optional fields.
Keep the tests lightweight and deterministic.
```

Good test ideas usually cover:

- `nil` request returns the current fallback value
- blank title becomes `(blank)` while missing title becomes `(missing)`
- positive estimates append `Xm`
- `DONE` maps to `state=complete`
- notification output is skipped when the channel or recipient is missing

### 2.2 Create a Baseline Test File

A representative test file might include:

```swift
func testProcessUpdateUsesMissingMarkers() {
    let processor = LegacyTaskProcessor()

    let result = processor.processUpdate(
        LegacyTaskUpdateRequest(
            taskID: nil,
            title: nil,
            description: nil,
            estimateMinutes: nil,
            assignee: nil,
            status: nil,
            notifyChannel: nil,
            notifyRecipient: nil
        )
    )

    XCTAssertEqual(
        result,
        "Task unknown | title=(missing) | description=(missing) | estimate=unknown | assignee=unassigned | state=open | notify=skipped"
    )
}
```

### 2.3 Run the Tests

```bash
cd src-swift
swift test
```

**Expected result**: ✅ tests pass before you refactor.

That passing baseline is your safety net.

---

## Part 3: Refactor Toward Safer Swift (12-15 minutes)

### 3.1 Why This Smell Matters in Swift

The original code is not just verbose. It misses several of Swift's biggest advantages:

- safer optional handling
- guard clauses
- typed errors
- clear value-focused helper functions

If Swift code is filled with `request!` and stringly typed fallback rules everywhere, you lose readability and much of the value of the language. The refactor in this lab should move the code toward:

- `guard let` and `if let`
- small formatting helpers
- normalized optional text handling
- `throws` or `Result<String, LegacyTaskProcessorError>` where appropriate
- a proper Swift error type instead of ad-hoc failure strings

### 3.2 Ask Copilot for a Focused Refactor

Prompt Copilot:

```text
/refactor Refactor the Swift LegacyTaskProcessor.

Requirements:
- keep the public behavior of processUpdate stable unless a test-driven error path is intentionally introduced
- replace repeated force unwraps with safe optional binding
- use guard statements and small helper functions where they improve clarity
- introduce a typed Swift error if it makes invalid input handling clearer
- keep the code workshop-friendly, not overly clever
```

### 3.3 What a Good Refactor Usually Produces

A clean Swift result often introduces helpers such as:

- `normalizeText(_:)`
- `formatTitle(_:)`
- `formatDescription(_:)`
- `formatEstimate(_:)`
- `formatNotification(channel:recipient:)`

The main function should start to read more like a short recipe:

```swift
func processUpdate(_ request: LegacyTaskUpdateRequest?) -> String {
    guard let request else {
        return ""
    }

    return [
        formatTaskID(request.taskID),
        formatTitle(request.title),
        formatDescription(request.description),
        formatEstimate(request.estimateMinutes),
        formatAssignee(request.assignee),
        formatState(request.status),
        formatNotification(channel: request.notifyChannel, recipient: request.notifyRecipient)
    ]
    .joined(separator: " | ")
}
```

Or, if you decide invalid requests should become errors:

```swift
func processUpdate(_ request: LegacyTaskUpdateRequest?) throws -> String
```

That can be a good direction too, as long as the tests drive it and the API remains easy to explain.

### 3.4 Keep the Refactor Explainable

This lab is **not** asking for the cleverest possible Swift.

For workshop participants, a better answer is:

- fewer force unwraps
- clearer defaults
- typed failures when needed
- helpers with business names
- code you can explain in a live session

---

## Part 4: Ask Copilot for a Quick Review (3-4 minutes)

Prompt Copilot:

```text
/check Review the refactored Swift legacy module.
Focus on:
- any remaining force unwraps
- places where typed errors would be clearer than strings
- whether the code still fits the workshop's Domain/Application/Infrastructure boundaries
```

You want feedback that is:

- specific
- small
- tied to actual code
- focused on risk or clarity, not style noise

---

## Verify Your Work

Run the Swift track tests:

```bash
cd src-swift
swift test
```

**Expected result**: ✅ tests pass after both the new Application-layer query and the legacy refactor.

---

## Key Learning Points

### ✅ What You Practiced

1. generating the next Application-layer slice from existing context
2. reusing a Domain repository method through a small use case
3. protecting a refactor with tests before cleanup
4. replacing force unwraps with safer Swift optional handling
5. keeping Swift refactors readable instead of clever

### ✅ What This Lab Is Not About

This lab is **not** about turning every refactor into an architecture rewrite. In the Swift track, better structure usually means:

- smaller helpers
- better names
- clearer defaults
- fewer force unwraps
- safer error handling

---

## Troubleshooting

### Copilot Tried to Move the Logic into SwiftUI

**Problem**: the generated solution pushed formatting or business rules into `src-swift/TaskManagerApp/`  
**Solution**: restate the boundary: _"Keep this in Domain/Application/Infrastructure only. The SwiftUI shell should remain thin."_

### The Refactor Changed Behavior

**Problem**: tests started failing after cleanup  
**Solution**: use the characterization tests as the contract. Restore the original output first, then continue improving the internals.

### Copilot Replaced the Code with Dense Functional Chains

**Problem**: the result became hard to explain to beginners  
**Solution**: narrow the request: _"Prefer small named helper functions and guard clauses over dense chains."_

### The Code Still Has `!` Everywhere

**Problem**: the refactor only rearranged the old structure  
**Solution**: explicitly ask Copilot to use `guard let`, `if let`, and helper functions so the force unwraps disappear.

---

## Next Steps

Continue to [**Lab 4: Testing, Documentation & Workflow (Swift)**](lab-04-testing-documentation-workflow-swift.md), where you'll expand XCTest coverage, add DocC-style comments, and finish the Swift track with a clean review workflow.

---

## Additional Resources

- [The Swift Programming Language](https://docs.swift.org/swift-book/)
- [XCTest Documentation](https://developer.apple.com/documentation/xctest)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
