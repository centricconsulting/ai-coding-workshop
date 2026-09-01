# Lab 2: From Requirements to Code with GitHub Copilot (Swift)

> **💡 Also available**: [shared .NET version](lab-02-requirements-to-code.md) · [JavaScript version](lab-02-requirements-to-code-javascript.md) · [Java/Spring Boot version](lab-02-requirements-to-code-java.md) · [Angular version](lab-02-requirements-to-code-angular.md) · [Python version](lab-02-requirements-to-code-python.md) · [Kotlin version](lab-02-requirements-to-code-kotlin.md)

**Duration**: 40-45 minutes  
**Learning Objectives**:

- Turn a plain-language request into small Swift code changes
- Extend the Domain layer with a `Priority` enum without breaking layer boundaries
- Add a Swift Application-layer create-task workflow with XCTest coverage
- Keep the Swift track package-first while letting the SwiftUI shell stay thin
- Practice reviewing Copilot output for path accuracy, value semantics, and error handling

---

## Overview

In Lab 1 you practiced TDD in the Swift Application layer. In this lab, you will turn a broader feature request into changes across the Domain and Application modules.

For the Swift track, the architecture stays close to the existing backend and Kotlin examples while remaining idiomatic Swift:

- `src-swift/Sources/TaskManagerDomain/` holds the `Task` aggregate and value types
- `src-swift/Sources/TaskManagerApplication/` holds use cases and orchestration
- `src-swift/Sources/TaskManagerInfrastructure/` holds the in-memory repository adapter
- `src-swift/TaskManagerApp/` holds the thin SwiftUI shell

The goal is not to transliterate Java or C# into Swift. The goal is to preserve the architectural intent while taking advantage of Swift structs, enums, protocols, and `throws`.

---

## Prerequisites

- ✅ Lab 1 is complete, or you have an equivalent Swift `NotificationService`
- ✅ Starter Domain and Infrastructure code exists under `src-swift/`
- ✅ VS Code is open with GitHub Copilot enabled
- ✅ Xcode + Command Line Tools are available locally
- ✅ Baseline verification completed:

```bash
cd src-swift
swift build
swift test
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
I have a Swift task manager in src-swift/ with Clean Architecture modules:
- TaskManagerDomain
- TaskManagerApplication
- TaskManagerInfrastructure

Turn this user story into 4-6 small backlog items:
"As a workshop participant, I want each task to have a priority so I can quickly see what matters most."

Keep the answer beginner-friendly and align it to the existing Swift modules.
```

A helpful answer should suggest work like:

- define valid priority values
- add priority to the Domain `Task`
- update task creation rules
- add a create-task use case in the Application layer
- add tests for valid and invalid inputs

### 1.2 Choose a Small, Concrete Ruleset

For this workshop, use these rules:

- valid priorities are `low`, `medium`, and `high`
- new tasks must include a priority
- the Domain `Task` stores the priority
- the Application layer creates and saves the task
- the repository still lives behind the Domain port

---

## Part 2: Add the Domain Concept First (RED -> GREEN) (10-12 minutes)

### 2.1 Ask Copilot for a Priority Enum

Prompt Copilot:

```text
Create a Priority enum for the Swift track in src-swift/Sources/TaskManagerDomain/.

Requirements:
- Swift enum
- cases: low, medium, high
- update the Task aggregate so priority becomes part of task creation and storage
- keep the Domain layer free of UI or persistence dependencies
- keep the code beginner-friendly and immutable where practical
```

### 2.2 Expected Output

Copilot should guide you toward something like:

```swift
public enum Priority: String, Codable, CaseIterable {
    case low
    case medium
    case high
}
```

And the `Task` aggregate should grow to include `priority`:

```swift
public struct Task {
    public let id: TaskID
    public let title: String
    public let description: String?
    public let priority: Priority
    public let status: TaskStatus
    public let createdAt: Date
    public let updatedAt: Date
    public let completedAt: Date?
}
```

### 2.3 Write the Domain Test First

Prompt Copilot:

```text
Update the Swift Domain tests in src-swift/Tests/TaskManagerDomainTests/.

Add XCTest cases that verify:
- Task.create stores the provided priority
- the default status is still todo
- updatingDetails keeps the existing priority unchanged
```

A representative test might look like:

```swift
func testCreateStoresPriority() throws {
    let task = try Task.create(
        title: "Prepare workshop",
        description: "Add the Swift lab track",
        priority: .high
    )

    XCTAssertEqual(task.priority, .high)
    XCTAssertEqual(task.status, .todo)
}
```

### 2.4 Why Start in the Domain Layer?

This keeps the dependency direction clean:

- Domain owns the business concept of priority
- Application orchestrates the workflow
- Infrastructure persists the aggregate

That is the same architectural lesson taught in the other tracks.

---

## Part 3: Create an Application-Layer Workflow (10-12 minutes)

### Scenario: Add a Swift Create Task Use Case

The Swift track does not need a full CQRS framework. A good workshop compromise is:

- a small `CreateTaskCommand` struct
- a `CreateTaskUseCase` type that depends on `TaskRepository`

### 3.1 Ask Copilot for the Use Case and Tests

Prompt Copilot:

```text
Create a Swift application-layer create-task workflow under src-swift/Sources/TaskManagerApplication/.

Requirements:
- add a CreateTaskCommand value type with title, description, and priority
- add a CreateTaskUseCase type that depends on TaskRepository
- create the Task aggregate and save it with repository.save(...)
- add XCTest unit tests under src-swift/Tests/TaskManagerApplicationTests/
- keep the solution lightweight and workshop-friendly
```

### 3.2 Expected Output

Copilot should generate code along these lines.

**`CreateTask.swift`**

```swift
public struct CreateTaskCommand {
    public let title: String
    public let description: String?
    public let priority: Priority
}

public final class CreateTaskUseCase {
    private let repository: any TaskRepository

    public init(repository: any TaskRepository) {
        self.repository = repository
    }

    public func execute(_ command: CreateTaskCommand) throws -> Task {
        let task = try Task.create(
            title: command.title,
            description: command.description,
            priority: command.priority
        )

        return repository.save(task)
    }
}
```

### 3.3 Add the Use-Case Tests First

Prompt Copilot:

```text
Create XCTest cases for CreateTaskUseCase in the Swift Application module.

Requirements:
- do not use third-party mocking libraries
- verify repository.save(...) is called effectively by checking repository state
- verify the returned task contains the requested title and priority
- keep the tests readable for workshop participants
```

A representative test might look like:

```swift
func testExecuteCreatesAndSavesTask() throws {
    let repository = InMemoryTaskRepository()
    let useCase = CreateTaskUseCase(repository: repository)

    let task = try useCase.execute(
        CreateTaskCommand(
            title: "Prepare demo",
            description: "Walk through Swift optionals",
            priority: .medium
        )
    )

    XCTAssertEqual(task.priority, .medium)
    XCTAssertEqual(repository.count(), 1)
}
```

---

## Part 4: Keep the SwiftUI Shell Thin (6-8 minutes)

### Scenario: Surface Priority Without Moving Business Logic into Views

Once the package supports `Priority`, you can ask Copilot to wire it through the app shell.

Prompt Copilot:

```text
Review src-swift/TaskManagerApp/ for the SwiftUI shell.
Suggest the smallest changes needed so the UI can create tasks with a priority selection,
while keeping business rules inside the Swift package.
```

Good answers should keep the responsibilities split this way:

- **Domain**: owns the `Priority` enum and task rules
- **Application**: owns the create-task workflow
- **SwiftUI shell**: collects input and displays state

If Copilot suggests moving validation into the `View`, push back and restate the boundary.

---

## Verify Your Work

Run the Swift package tests:

```bash
cd src-swift
swift test
```

If you also updated the UI shell, run the app in Xcode and confirm the new input appears and still creates tasks successfully.

---

## Key Learning Points

### ✅ What This Lab Teaches

1. plain-language requirements still need explicit rules
2. `Priority` belongs in the Domain layer
3. Swift can mirror Clean Architecture without copying backend ceremony line-for-line
4. protocol-driven Application code stays easy to test with plain XCTest
5. immutable value types remain easy to evolve when the rules are clear

### ✅ What to Watch for in Copilot Output

- correct `src-swift/` paths
- `Priority` living in Domain, not Infrastructure
- use cases depending on `TaskRepository`, not `InMemoryTaskRepository`
- readable structs and enums instead of unnecessary framework scaffolding
- Swift error handling staying explicit and beginner-friendly

---

## Troubleshooting

### Copilot Put `Priority` in the Application Module

**Problem**: the enum was generated next to the use case  
**Solution**: move it to `TaskManagerDomain` and remind Copilot that priority is a business concept, not an orchestration concern.

### The Use Case Depends on `InMemoryTaskRepository`

**Problem**: Application now imports a concrete Infrastructure class  
**Solution**: refine the prompt with: _"Depend on the Domain `TaskRepository` protocol only."_

### Copilot Added SwiftUI State into the Package

**Problem**: suggestions use `ObservableObject`, `@Published`, or `View` in Domain/Application  
**Solution**: restate the boundary: _"The Swift package is UI-independent. Keep SwiftUI in `src-swift/TaskManagerApp/` only."_

### The Tests Started Looking Like Another Language Track

**Problem**: generated examples reference xUnit, JUnit, or pytest  
**Solution**: restate the toolchain: _"Use XCTest only for the Swift track."_

---

## Next Steps

Continue to [**Lab 3: Code Generation & Refactoring (Swift)**](lab-03-generation-and-refactoring-swift.md), where you'll generate an active-task listing workflow and refactor a legacy optional-handling smell into more idiomatic Swift.

---

## Additional Resources

- [Swift.org Documentation](https://www.swift.org/documentation/)
- [XCTest Documentation](https://developer.apple.com/documentation/xctest)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
