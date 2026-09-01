# Lab 1: Test-Driven Development with GitHub Copilot (Swift)

> **💡 Also available**: [shared .NET / Spring Boot / Python version](lab-01-tdd-with-copilot.md) · [Angular version](lab-01-tdd-with-copilot-angular.md) · [JavaScript version](lab-01-tdd-with-copilot-javascript.md) · [Kotlin version](lab-01-tdd-with-copilot-kotlin.md)

**Duration**: 30-40 minutes  
**Learning Objectives**:

- Practice the Red-Green-Refactor cycle in a Swift package
- Use Copilot to create XCTest cases before implementation in `TaskManagerApplication`
- Apply protocol-driven design without pulling UI logic into SwiftUI views
- Keep orchestration in the Application layer instead of leaking it into the app shell or infrastructure
- Review AI-generated Swift for optionals, access control, naming, and path accuracy

---

## Overview

In this lab, you will create a small `NotificationService` for the Swift track.

Unlike the shared .NET / Spring Boot / Python Lab 1, this walkthrough is separate because the Swift track uses:

- a standalone Swift Package under `src-swift/`
- XCTest instead of xUnit, JUnit, or pytest
- a thin SwiftUI shell that should stay outside the business-logic exercise
- macOS + Xcode local setup instead of a devcontainer

You will work mainly in:

- `src-swift/Sources/TaskManagerApplication/`
- `src-swift/Tests/TaskManagerApplicationTests/`

The SwiftUI app shell already exists so you can see the package in action, but this lab stays focused on the Application layer.

---

## Prerequisites

- ✅ Repository cloned and your workshop branch created from `main`
- ✅ VS Code open with GitHub Copilot enabled
- ✅ Xcode + Command Line Tools installed locally (see [`LOCAL_SETUP_SWIFT.md`](../LOCAL_SETUP_SWIFT.md))
- ✅ Baseline verification completed:

```bash
cd src-swift
swift build
swift test
```

Expected starting point: the Swift package builds successfully and the starter XCTest suites pass.

---

## Part 1: Design the Interface First (5-7 minutes)

### Scenario: Send Task Notifications from a Mobile Workflow

Imagine your future iOS app needs a thin Swift service that can send:

- email notifications
- SMS notifications
- a combined "notify both channels" workflow

For this workshop, keep SwiftUI out of scope. Focus on an Application-layer contract that the app shell could call later.

### 1.1 Ask Copilot for the Interface

Prompt Copilot:

```text
Create a NotificationService contract for the Swift track in
src-swift/Sources/TaskManagerApplication/.

Requirements:
- Swift only, no UIKit or SwiftUI types
- methods for email, SMS, and combined task notifications
- workshop-friendly names
- keep it in the Application layer
- use protocols, not framework annotations
```

### 1.2 Expected Output

Copilot should guide you toward something like:

```swift
public protocol NotificationService {
    func sendEmail(to recipient: String, subject: String, message: String) throws

    func sendSMS(to phoneNumber: String, message: String) throws

    func sendTaskNotification(
        recipient: String,
        phoneNumber: String,
        subject: String,
        message: String
    ) throws
}
```

The exact names can vary. The important part is that the Application layer owns the contract.

---

## Part 2: Write the Tests First (RED Phase) (12-15 minutes)

> **TDD rule**: do not implement the service yet. Add the failing tests first.

### 2.1 Decide on a Small Ruleset

For this lab, use a ruleset that is easy to explain and easy to test:

- recipient email must not be blank
- phone number must not be blank
- message must not be blank
- `sendTaskNotification(...)` should call both channel methods

### 2.2 Ask Copilot for XCTest Cases

Prompt Copilot:

```text
Create XCTest tests for a Swift NotificationService implementation in
src-swift/Tests/TaskManagerApplicationTests/.

Requirements:
- use XCTestCase and descriptive test method names
- avoid third-party mocking libraries
- cover blank recipient, blank phone number, blank message
- verify the combined notification path sends both email and SMS
- keep the code beginner-friendly
```

### 2.3 Expected Output

A representative test suite might look like:

```swift
final class DefaultNotificationServiceTests: XCTestCase {
    func testSendTaskNotificationSendsBothChannels() throws {
        let emailGateway = RecordingEmailGateway()
        let smsGateway = RecordingSMSGateway()
        let service = DefaultNotificationService(
            emailGateway: emailGateway,
            smsGateway: smsGateway
        )

        try service.sendTaskNotification(
            recipient: "person@example.com",
            phoneNumber: "555-0100",
            subject: "Task updated",
            message: "Review the Swift lab draft."
        )

        XCTAssertEqual(emailGateway.messages.count, 1)
        XCTAssertEqual(smsGateway.messages.count, 1)
    }

    func testSendEmailRejectsBlankRecipient() {
        XCTAssertThrowsError(
            try service.sendEmail(to: "   ", subject: "Task updated", message: "Review the Swift lab draft.")
        )
    }
}
```

### 2.4 Run the Tests and Confirm Failure

```bash
cd src-swift
swift test
```

**Expected result**: ❌ your new tests fail because the implementation does not exist yet.

That failing build is the correct TDD checkpoint.

---

## Part 3: Implement the Smallest Working Version (GREEN Phase) (10-12 minutes)

### 3.1 Ask Copilot for a Minimal Implementation

Prompt Copilot:

```text
Implement the Swift NotificationService in TaskManagerApplication.

Requirements:
- keep it package-friendly, no UIKit or SwiftUI imports
- validate blank recipient, phone number, subject, and message
- delegate to small gateway protocols for email and SMS
- keep the implementation straightforward and readable
- match the XCTest suite
```

### 3.2 Expected Output

A clean result often looks like:

```swift
final class DefaultNotificationService: NotificationService {
    private let emailGateway: EmailGateway
    private let smsGateway: SMSGateway

    init(emailGateway: EmailGateway, smsGateway: SMSGateway) {
        self.emailGateway = emailGateway
        self.smsGateway = smsGateway
    }

    func sendEmail(to recipient: String, subject: String, message: String) throws {
        try validate(text: recipient, fieldName: "Recipient")
        try validate(text: subject, fieldName: "Subject")
        try validate(text: message, fieldName: "Message")

        emailGateway.send(
            to: recipient.trimmingCharacters(in: .whitespacesAndNewlines),
            subject: subject.trimmingCharacters(in: .whitespacesAndNewlines),
            message: message.trimmingCharacters(in: .whitespacesAndNewlines)
        )
    }

    func sendSMS(to phoneNumber: String, message: String) throws {
        try validate(text: phoneNumber, fieldName: "Phone number")
        try validate(text: message, fieldName: "Message")

        smsGateway.send(
            to: phoneNumber.trimmingCharacters(in: .whitespacesAndNewlines),
            message: message.trimmingCharacters(in: .whitespacesAndNewlines)
        )
    }

    func sendTaskNotification(recipient: String, phoneNumber: String, subject: String, message: String) throws {
        try sendEmail(to: recipient, subject: subject, message: message)
        try sendSMS(to: phoneNumber, message: message)
    }
}
```

This is a good Lab 1 solution because it is:

- small
- easy to test
- free of framework coupling
- ready for the SwiftUI shell to call later

---

## Part 4: Light Refactoring (REFACTOR Phase) (5-7 minutes)

### 4.1 Ask Copilot for a Small Cleanup

Prompt Copilot:

```text
/refactor Review the Swift NotificationService implementation.
Suggest one or two small refactorings that improve readability without changing behavior.
Prefer guard clauses and small private helpers over clever abstractions.
```

Good candidates include:

- a reusable `validate(text:fieldName:)` helper
- trimming input once before delegating
- small test helpers for recording gateway calls

### 4.2 Keep the Refactor Boring on Purpose

This lab is not asking for Combine, async/await, actors, or dependency-injection frameworks.

For the workshop, a better result is:

- explicit protocols
- clear validation
- readable test names
- no UI dependencies in the Application layer

---

## Verify Your Work

Run the Swift package tests:

```bash
cd src-swift
swift test
```

**Expected result**: ✅ tests pass after the implementation is complete.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ the Application layer contains a Swift `NotificationService` contract
- ✅ XCTest cases were written before the implementation
- ✅ the combined notification workflow is covered by tests
- ✅ blank input is rejected with clear failures
- ✅ the final implementation stays package-friendly and framework-light
- ✅ `swift test` passes from `src-swift/`

---

## Troubleshooting

### Copilot Generated SwiftUI or UIKit Types

**Problem**: suggestions use `ObservableObject`, `View`, or `UIApplication`  
**Solution**: restate the scope: _"This lab stays inside `TaskManagerApplication`. Do not use SwiftUI or UIKit types."_

### Copilot Added Async Code Everywhere

**Problem**: the generated service became `async` for no reason  
**Solution**: keep the contract synchronous. The workshop is teaching boundaries first, not concurrency.

### The Generated Files Landed in the Wrong Module

**Problem**: Copilot placed Application files in Domain or Infrastructure  
**Solution**: move the files under `src-swift/Sources/TaskManagerApplication/` and remind Copilot that `NotificationService` is an Application concern.

### XCTest Output Is Hard to Read

**Problem**: the tests rely on one giant helper  
**Solution**: keep only the repeated setup in helpers. Leave the important assertions inside each test.

---

## Next Steps

Continue to [**Lab 2: From Requirements to Code (Swift)**](lab-02-requirements-to-code-swift.md), where you'll turn a broader feature request into Domain and Application changes for the Swift track.

---

## Additional Resources

- [Swift Language Documentation](https://www.swift.org/documentation/)
- [XCTest Documentation](https://developer.apple.com/documentation/xctest)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
