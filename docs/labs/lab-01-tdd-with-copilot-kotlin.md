# Lab 1: Test-Driven Development with GitHub Copilot (Kotlin)

> **💡 Also available**: [shared .NET / Spring Boot / Python version](lab-01-tdd-with-copilot.md) · [Angular version](lab-01-tdd-with-copilot-angular.md) · [JavaScript version](lab-01-tdd-with-copilot-javascript.md)

**Duration**: 30-40 minutes  
**Learning Objectives**:

- Practice the Red-Green-Refactor cycle in a Kotlin/JVM codebase
- Use Copilot to create tests before implementation in `task-manager-application`
- Apply JUnit 5 and MockK in a workshop-friendly way
- Keep orchestration in the Application layer instead of leaking it into the UI or infrastructure
- Review AI-generated Kotlin for null-safety, naming, and path accuracy

---

## Overview

In this lab, you will create a small `NotificationService` for the Kotlin track.

Unlike the shared .NET / Spring Boot / Python Lab 1, this walkthrough is separate because the Kotlin track uses:

- a standalone Gradle multi-module build under `src-kotlin/`
- JUnit 5 and MockK instead of xUnit, Mockito, or pytest
- a JVM-only, mobile-oriented setup with **no Android SDK, emulator, or web server**

You will work mainly in:

- `src-kotlin/task-manager-application/src/main/kotlin/com/example/taskmanager/application/`
- `src-kotlin/task-manager-application/src/test/kotlin/com/example/taskmanager/application/`

The Application module currently contains only a placeholder marker. That is intentional. Lab 1 is where you start shaping real Kotlin code with tests first.

---

## Prerequisites

- ✅ Repository cloned and your workshop branch created from `main`
- ✅ VS Code open with GitHub Copilot enabled
- ✅ Java 21 and Gradle 8+ installed locally, or use `.devcontainer/kotlin-participant`
- ✅ Baseline verification completed:

```bash
cd src-kotlin
gradle build
```

Expected starting point: the Kotlin starter modules build successfully and the placeholder tests pass.

---

## Part 1: Design the Interface First (5-7 minutes)

### Scenario: Send Task Notifications from a Mobile-Oriented Workflow

Imagine your future Android UI needs a thin Kotlin service that can send:

- email notifications
- SMS notifications
- a combined "notify both channels" workflow

For this workshop, keep the UI out of scope. Focus on an Application-layer contract that a mobile shell could call later.

### 1.1 Ask Copilot for the Interface

Prompt Copilot:

```text
Create a NotificationService contract for the Kotlin track in
src-kotlin/task-manager-application/src/main/kotlin/com/example/taskmanager/application/.

Requirements:
- Kotlin/JVM only, no Android SDK APIs
- methods for email, SMS, and combined task notifications
- workshop-friendly names
- keep it in the Application layer
- use interfaces, not framework annotations
```

### 1.2 Expected Output

Copilot should guide you toward something like:

```kotlin
package com.example.taskmanager.application

interface NotificationService {
    fun sendEmail(recipient: String, subject: String, message: String)

    fun sendSms(phoneNumber: String, message: String)

    fun sendTaskNotification(
        recipient: String,
        phoneNumber: String,
        subject: String,
        message: String,
    )
}
```

The exact method names can vary. The important part is that the Application layer owns the behavior contract.

---

## Part 2: Write the Tests First (RED Phase) (12-15 minutes)

> **TDD rule**: do not implement the service yet. Add the failing tests first.

### 2.1 Decide on a Small Ruleset

For this lab, use a ruleset that is easy to explain and easy to test:

- recipient email must not be blank
- phone number must not be blank
- message must not be blank
- `sendTaskNotification(...)` should call both channel methods

### 2.2 Ask Copilot for JUnit 5 + MockK Tests

Prompt Copilot:

```text
Create JUnit 5 tests for a Kotlin NotificationService implementation in
src-kotlin/task-manager-application/src/test/kotlin/com/example/taskmanager/application/.

Requirements:
- use @Test and @DisplayName
- use MockK for dependencies or collaborators
- cover blank recipient, blank phone number, blank message
- verify the combined notification path sends both email and SMS
- keep the code beginner-friendly
```

### 2.3 Expected Output

A representative test suite might look like:

```kotlin
import io.mockk.mockk
import io.mockk.verify
import org.junit.jupiter.api.Assertions.assertThrows
import org.junit.jupiter.api.DisplayName
import org.junit.jupiter.api.Test

@DisplayName("DefaultNotificationService")
class DefaultNotificationServiceTest {
    private val emailGateway = mockk<EmailGateway>(relaxed = true)
    private val smsGateway = mockk<SmsGateway>(relaxed = true)
    private val service = DefaultNotificationService(emailGateway, smsGateway)

    @Test
    @DisplayName("sendTaskNotification sends both channels")
    fun sendTaskNotificationSendsBothChannels() {
        service.sendTaskNotification(
            recipient = "person@example.com",
            phoneNumber = "555-0100",
            subject = "Task updated",
            message = "Review the Kotlin lab draft.",
        )

        verify(exactly = 1) { emailGateway.send("person@example.com", "Task updated", "Review the Kotlin lab draft.") }
        verify(exactly = 1) { smsGateway.send("555-0100", "Review the Kotlin lab draft.") }
    }

    @Test
    @DisplayName("sendEmail rejects a blank recipient")
    fun sendEmailRejectsBlankRecipient() {
        assertThrows(IllegalArgumentException::class.java) {
            service.sendEmail("   ", "Task updated", "Review the Kotlin lab draft.")
        }
    }
}
```

### 2.4 Run the Tests and Confirm Failure

```bash
cd src-kotlin
gradle test
```

**Expected result**: ❌ your new tests fail because the implementation does not exist yet.

That failing build is the correct TDD checkpoint.

---

## Part 3: Implement the Smallest Working Version (GREEN Phase) (10-12 minutes)

### 3.1 Ask Copilot for a Minimal Implementation

Prompt Copilot:

```text
Implement the Kotlin NotificationService in task-manager-application.

Requirements:
- keep it JVM-only, no Android classes
- validate blank recipient, phone number, subject, and message
- delegate to small gateway interfaces for email and SMS
- keep the implementation straightforward and readable
- match the JUnit 5 and MockK tests
```

### 3.2 Expected Output

A clean result often looks like:

```kotlin
class DefaultNotificationService(
    private val emailGateway: EmailGateway,
    private val smsGateway: SmsGateway,
) : NotificationService {
    override fun sendEmail(recipient: String, subject: String, message: String) {
        require(recipient.isNotBlank()) { "Recipient is required." }
        require(subject.isNotBlank()) { "Subject is required." }
        require(message.isNotBlank()) { "Message is required." }

        emailGateway.send(recipient.trim(), subject.trim(), message.trim())
    }

    override fun sendSms(phoneNumber: String, message: String) {
        require(phoneNumber.isNotBlank()) { "Phone number is required." }
        require(message.isNotBlank()) { "Message is required." }

        smsGateway.send(phoneNumber.trim(), message.trim())
    }

    override fun sendTaskNotification(
        recipient: String,
        phoneNumber: String,
        subject: String,
        message: String,
    ) {
        sendEmail(recipient, subject, message)
        sendSms(phoneNumber, message)
    }
}
```

This is a good Lab 1 solution because it is:

- small
- easy to test
- free of framework coupling
- ready for a future Android shell to call

---

## Part 4: Light Refactoring (REFACTOR Phase) (5-7 minutes)

### 4.1 Ask Copilot for a Small Cleanup

Prompt Copilot:

```text
/refactor Review the Kotlin NotificationService implementation.
Suggest one or two small refactorings that improve readability without changing behavior.
Prefer Kotlin guard clauses and small private helpers over clever abstractions.
```

Good refactors for this lab might include:

- extracting a tiny `requireNotBlank(...)` helper
- trimming values once before delegation
- improving `@DisplayName` wording in the tests

### 4.2 Keep the Refactor Small

Avoid turning this into:

- a framework-heavy architecture exercise
- a coroutine example
- an Android-specific service
- a class hierarchy with unnecessary abstractions

Lab 1 is about the TDD loop, not maximizing complexity.

---

## Verify Your Work

Run the Kotlin track tests:

```bash
cd src-kotlin
gradle test
```

**Expected result**: ✅ tests pass after the implementation is complete.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ the Application layer contains a Kotlin `NotificationService` contract
- ✅ JUnit 5 tests were written before the implementation
- ✅ MockK verifies the combined notification workflow
- ✅ blank input is rejected with clear exceptions
- ✅ the final implementation stays JVM-only and framework-light
- ✅ `gradle test` passes from `src-kotlin/`

---

## Troubleshooting

### Copilot Generated Android APIs

**Problem**: suggestions use `Context`, `Log`, or other Android-only types  
**Solution**: restate the scope: _"This track is JVM-only for Labs 1-4. Do not use Android SDK classes."_

### Copilot Suggested Coroutines or `suspend` Everywhere

**Problem**: the generated service became async for no reason  
**Solution**: keep the contract synchronous. The starter repository is synchronous, and the workshop is teaching boundaries first, not concurrency.

### MockK Imports Are Wrong

**Problem**: Copilot mixes Mockito syntax into the Kotlin test  
**Solution**: refine the prompt with: _"Use MockK only. No Mockito imports or APIs."_

### The Generated Files Landed in the Wrong Module

**Problem**: Copilot placed Application files in Domain or Infrastructure  
**Solution**: move the files under `src-kotlin/task-manager-application/` and remind Copilot that NotificationService is an Application concern.

---

## Next Steps

Continue to [**Lab 2: From Requirements to Code (Kotlin)**](lab-02-requirements-to-code-kotlin.md), where you'll extend the Task domain with `Priority` and build the Kotlin track's first real task-creation workflow.

---

## Additional Resources

- [Kotlin Language Documentation](https://kotlinlang.org/docs/home.html)
- [JUnit 5 User Guide](https://junit.org/junit5/docs/current/user-guide/)
- [MockK Documentation](https://mockk.io/)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
