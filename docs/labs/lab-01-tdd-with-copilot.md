# Lab 1: Test-Driven Development with GitHub Copilot

**Duration**: 30-40 minutes  
**Learning Objectives**:

- Master the Red-Green-Refactor TDD cycle with AI assistance
- Use Copilot to generate tests before implementation
- Apply repository Copilot Instructions for consistent code quality
- Understand how TDD enforces better design decisions
- Compare TDD patterns across .NET and Spring Boot

---

## Tech Stack Selection

> **Choose Your Path**: This lab supports both **.NET** and **Spring Boot** implementations.  
> 
> - **For .NET**: Follow sections marked with 🔷 or "(.NET)"
> - **For Spring Boot**: Follow sections marked with 🟩 or "(Spring Boot)"
> - **Mixed Groups**: Facilitators can demonstrate both approaches side-by-side
>
> The TDD principles and workflow are identical across both stacks—only syntax and frameworks differ.

---

## Overview

In this lab, you'll create a `NotificationService` that sends task notifications via email and SMS. You'll follow strict Test-Driven Development (TDD) practices:

1. **Design** - Create the interface first
2. **Red** - Write failing tests
3. **Green** - Implement code to pass tests
4. **Refactor** - Improve and reflect

> **Why TDD?** Writing tests first forces you to think about your API design, ensures testability, and provides living documentation of behavior. This principle applies equally to .NET and Spring Boot development.

---

## Prerequisites

### 🔷 .NET Prerequisites

- ✅ Repository cloned and `main` branch checked out
- ✅ VS Code open with GitHub Copilot enabled
- ✅ `.github/instructions/dotnet.instructions.md` and `.github/instructions/csharp.instructions.md` exist
- ✅ Initial build successful: `dotnet build && dotnet test`
- ✅ DevContainer: Use `.devcontainer/dotnet-participant` or `.devcontainer/maintainer`

### 🟩 Spring Boot Prerequisites

- ✅ Repository cloned and `main` branch checked out
- ✅ VS Code open with GitHub Copilot enabled
- ✅ `.github/instructions/springboot.instructions.md` exists
- ✅ Java 21 and Maven 3.9+ installed
- ✅ Initial build successful: `cd src-springboot && mvn clean test`
- ✅ DevContainer: Use `.devcontainer/springboot-participant` or `.devcontainer/maintainer`

---

## Step 1: Create Interface First (Design Phase)

> **Goal**: Define the API before writing any tests or implementation. This forces you to think about what clients of your service will need.

### 1.1 Open Copilot Chat

- Press `Ctrl+Alt+I` (Windows/Linux) or `Cmd+Shift+I` (Mac)
- This opens the Copilot Chat panel

### 1.2 Request Interface Generation

#### 🔷 .NET Prompt

In the chat panel, enter:

```text
Create an INotificationService interface in the Application layer for sending email and SMS notifications about tasks. Include methods for both individual and combined notifications.
```

#### 🟩 Spring Boot Prompt

In the chat panel, enter:

```text
Create a NotificationService interface in the application layer for sending email and SMS notifications about tasks. Use Java naming conventions and CompletableFuture for async operations.
```

### 1.3 Review Generated Interface

#### 🔷 .NET Interface

Copilot should generate something like:

```csharp
namespace TaskManager.Application.Services;

public interface INotificationService
{
    Task SendEmailNotificationAsync(string recipient, string subject, string message, CancellationToken cancellationToken = default);
    
    Task SendSmsNotificationAsync(string phoneNumber, string message, CancellationToken cancellationToken = default);
    
    Task SendNotificationAsync(string recipient, string phoneNumber, string subject, string message, CancellationToken cancellationToken = default);
}
```

**Expected Location**: `src/TaskManager.Application/Services/INotificationService.cs`

#### 🟩 Spring Boot Interface

Copilot should generate something like:

```java
package com.example.taskmanager.application.services;

/**
 * Service interface for sending task notifications via multiple channels.
 * Follows Clean Architecture principles - defined in application layer,
 * implemented by infrastructure adapters.
 */
public interface NotificationService {
    
    /**
     * Send email notification.
     *
     * @param recipient email address
     * @param subject email subject
     * @param message email body
     */
    void sendEmailNotification(String recipient, String subject, String message);
    
    /**
     * Send SMS notification.
     *
     * @param phoneNumber recipient phone number
     * @param message SMS message body
     */
    void sendSmsNotification(String phoneNumber, String message);
    
    /**
     * Send both email and SMS notification.
     *
     * @param recipient email address
     * @param phoneNumber phone number
     * @param subject email subject
     * @param message notification message
     */
    void sendNotification(String recipient, String phoneNumber, String subject, String message);
}
```

**Expected Location**: `src-springboot/taskmanager-application/src/main/java/com/example/taskmanager/application/services/NotificationService.java`

> **Note**: Spring Boot applications typically use synchronous methods—Spring manages threading internally. If async is needed, use `CompletableFuture<Void>` return types.

### 1.4 Verify Design

Review the interface and ask yourself:

- ✅ Does it belong in the Application layer?  
  - **Yes** - it's a service interface (port in Clean Architecture)
- ✅ Are method names descriptive and intention-revealing?
  - **.NET**: Uses `Async` suffix following C# conventions
  - **Spring Boot**: Uses camelCase following Java conventions
- ✅ Does it follow framework conventions?
  - **.NET**: `async`/`await` with `CancellationToken`
  - **Spring Boot**: Synchronous methods (Spring handles threading)
- ✅ Is the API easy to use and understand?

If satisfied, accept the code. If not, refine your prompt.

> **Pattern Reference**: See [Pattern Translation Guide](../guides/dotnet-to-springboot-patterns.md#application-layer-patterns) for more interface comparisons.

---

## Step 2: Write Tests FIRST (Red Phase)

> **Critical TDD Principle**: Write tests BEFORE implementation. This is the "Red" phase - tests will fail because the implementation doesn't exist yet.

### 2.1 Request Test Generation

#### 🔷 .NET Prompt

In Copilot Chat, enter:

```text
Create xUnit tests for NotificationService in the pattern specified in our .NET instructions. Organize tests by method with separate test classes. Use FakeItEasy for mocking ILogger. Test happy path and all guard clauses.
```

#### 🟩 Spring Boot Prompt

In Copilot Chat, enter:

```text
Create JUnit 5 tests for NotificationServiceImpl in the pattern specified in our Spring Boot instructions. Organize tests with @Nested classes for each method. Use Mockito for mocking. Test happy path and all guard clauses. Use @DisplayName for readable test names.
```

### 2.2 Review Test Structure

#### 🔷 .NET Test Structure

Copilot should create a folder structure like:

```text
tests/TaskManager.UnitTests/Services/NotificationServiceTests/
├── SendEmailNotificationAsyncTests.cs
├── SendSmsNotificationAsyncTests.cs
└── SendNotificationAsyncTests.cs
```

Example test class (`SendEmailNotificationAsyncTests.cs`):

```csharp
using Xunit;
using FakeItEasy;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Services;

namespace TaskManager.UnitTests.Services.NotificationServiceTests;

public sealed class SendEmailNotificationAsyncTests
{
    private readonly ILogger<NotificationService> _fakeLogger;
    
    public SendEmailNotificationAsyncTests()
    {
        _fakeLogger = A.Fake<ILogger<NotificationService>>();
    }

    [Fact]
    public async Task SendEmailNotificationAsync_WithValidParams_SendsEmail()
    {
        // Arrange
        var service = new NotificationService(_fakeLogger);
        
        // Act
        await service.SendEmailNotificationAsync(
            "test@example.com", 
            "Test Subject", 
            "Test Message");
        
        // Assert
        // Will verify logging once implementation exists
    }

    [Theory]
    [InlineData(null, "subject", "message")]
    [InlineData("", "subject", "message")]
    [InlineData("  ", "subject", "message")]
    public async Task SendEmailNotificationAsync_WithInvalidRecipient_ThrowsArgumentException(
        string recipient, string subject, string message)
    {
        // Arrange
        var service = new NotificationService(_fakeLogger);
        
        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(
            () => service.SendEmailNotificationAsync(recipient, subject, message));
    }
    
    // Additional guard clause tests...
}
```

#### 🟩 Spring Boot Test Structure

Copilot should create test classes in:

```text
src-springboot/taskmanager-application/src/test/java/
    com/example/taskmanager/application/services/
        NotificationServiceImplTest.java
```

Example test class:

```java
package com.example.taskmanager.application.services;

import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Nested;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;
import org.slf4j.Logger;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
@DisplayName("NotificationService Tests")
class NotificationServiceImplTest {
    
    @Mock
    private Logger logger;
    
    private NotificationServiceImpl service;
    
    @BeforeEach
    void setUp() {
        service = new NotificationServiceImpl(logger);
    }
    
    @Nested
    @DisplayName("sendEmailNotification()")
    class SendEmailNotificationTests {
        
        @Test
        @DisplayName("should send email with valid parameters")
        void shouldSendEmailWithValidParams() {
            // Arrange
            String recipient = "test@example.com";
            String subject = "Test Subject";
            String message = "Test Message";
            
            // Act
            assertDoesNotThrow(() -> 
                service.sendEmailNotification(recipient, subject, message)
            );
            
            // Assert
            verify(logger, times(2)).info(anyString(), any());
        }
        
        @Test
        @DisplayName("should throw exception when recipient is null")
        void shouldThrowWhenRecipientIsNull() {
            // Act & Assert
            assertThrows(IllegalArgumentException.class, () ->
                service.sendEmailNotification(null, "subject", "message")
            );
        }
        
        @Test
        @DisplayName("should throw exception when recipient is empty")
        void shouldThrowWhenRecipientIsEmpty() {
            // Act & Assert
            assertThrows(IllegalArgumentException.class, () ->
                service.sendEmailNotification("", "subject", "message")
            );
        }
        
        @Test
        @DisplayName("should throw exception when recipient is blank")
        void shouldThrowWhenRecipientIsBlank() {
            // Act & Assert
            assertThrows(IllegalArgumentException.class, () ->
                service.sendEmailNotification("   ", "subject", "message")
            );
        }
        
        // Additional guard clause tests for subject and message...
    }
    
    @Nested
    @DisplayName("sendSmsNotification()")
    class SendSmsNotificationTests {
        // Similar structure for SMS tests...
    }
    
    @Nested
    @DisplayName("sendNotification()")
    class SendNotificationTests {
        // Combined notification tests...
    }
}
```

### 2.3 Key Testing Pattern Differences

| Aspect | .NET (xUnit + FakeItEasy) | Spring Boot (JUnit 5 + Mockito) |
|--------|---------------------------|----------------------------------|
| **Test Attribute** | `[Fact]` | `@Test` |
| **Parameterized** | `[Theory]` + `[InlineData]` | `@ParameterizedTest` + `@ValueSource` |
| **Test Organization** | Separate class files | `@Nested` classes in one file |
| **Mocking** | `A.Fake<T>()` | `@Mock` annotation |
| **Setup** | Constructor injection | `@BeforeEach` method |
| **Assertions** | `Assert.ThrowsAsync<T>()` | `assertThrows()` |
| **Verification** | `A.CallTo().MustHaveHappened()` | `verify(mock, times(n)).method()` |
| **Display Names** | Method name | `@DisplayName` annotation |

> **Pattern Reference**: See [Testing Patterns](../guides/dotnet-to-springboot-p atterns.md#testing-patterns) for detailed comparisons.

### 2.4 Run Tests and Verify They Fail

#### 🔷 .NET

```bash
dotnet test
```

**Expected Result**: ❌ **Tests FAIL**

You should see errors like:

```text
error CS0246: The type or namespace name 'NotificationService' could not be found
```

#### 🟩 Spring Boot

```bash
cd src-springboot
mvn test -Dtest=NotificationServiceImplTest
```

**Expected Result**: ❌ **Tests FAIL**

You should see compilation errors like:

```text
[ERROR] cannot find symbol
[ERROR]   symbol:   class NotificationServiceImpl
[ERROR]   location: package com.example.taskmanager.application.services
```

**This is GOOD!** You're in the "Red" phase of TDD. The tests define what you need to build.

### 2.5 Reflect on Test Design

Before implementing, review:

- ✅ Do test names clearly describe behavior?
  - **.NET**: Method names are the description
  - **Spring Boot**: `@DisplayName` provides readable descriptions
- ✅ Are guard clause tests comprehensive?
  - Both stacks test null, empty, and whitespace
- ✅ Is the happy path covered?
  - Both have positive test cases
- ✅ Are tests organized logically?
  - **.NET**: Multiple test class files
  - **Spring Boot**: `@Nested` classes in single file
├── SendSmsNotificationAsyncTests.cs
└── SendNotificationAsyncTests.cs
```

Each test class should contain:

- ✅ Tests for the happy path (valid inputs)
- ✅ Tests for guard clauses (null/empty parameters)
- ✅ Descriptive test method names (e.g., `SendEmailNotificationAsync_WithValidInputs_SendsEmail`)
- ✅ FakeItEasy mocks for `ILogger<NotificationService>`
- ✅ Async test methods with proper assertions

### 2.3 Example Test (SendEmailNotificationAsyncTests.cs)

```csharp
namespace TaskManager.UnitTests.Services.NotificationServiceTests;

public sealed class SendEmailNotificationAsyncTests
{
    private readonly ILogger<NotificationService> _logger;
    private readonly NotificationService _sut;

    public SendEmailNotificationAsyncTests()
    {
        _logger = A.Fake<ILogger<NotificationService>>();
        _sut = new NotificationService(_logger);
    }

    [Fact]
    public async Task SendEmailNotificationAsync_WithValidInputs_SendsEmail()
    {
        // Arrange
        const string recipient = "user@example.com";
        const string subject = "Task Update";
        const string message = "Your task has been updated";

        // Act
        await _sut.SendEmailNotificationAsync(recipient, subject, message);

        // Assert
        // Verify logging occurred (implementation detail we'll check)
        A.CallTo(_logger).Where(call => 
            call.Method.Name == "Log" && 
            call.GetArgument<LogLevel>(0) == LogLevel.Information)
            .MustHaveHappened();
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendEmailNotificationAsync_WithInvalidRecipient_ThrowsArgumentException(string invalidRecipient)
    {
        // Arrange
        const string subject = "Test";
        const string message = "Test message";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendEmailNotificationAsync(invalidRecipient, subject, message));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendEmailNotificationAsync_WithInvalidSubject_ThrowsArgumentException(string invalidSubject)
    {
        // Arrange
        const string recipient = "user@example.com";
        const string message = "Test message";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendEmailNotificationAsync(recipient, invalidSubject, message));
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public async Task SendEmailNotificationAsync_WithInvalidMessage_ThrowsArgumentException(string invalidMessage)
    {
        // Arrange
        const string recipient = "user@example.com";
        const string subject = "Test";

        // Act & Assert
        await Assert.ThrowsAsync<ArgumentException>(() => 
            _sut.SendEmailNotificationAsync(recipient, subject, invalidMessage));
    }
}
```

### 2.4 Run Tests (Expect Failure - RED)

In the terminal, run:

```bash
dotnet test
```

**Expected Result**: ❌ **Tests FAIL**

You should see errors like:

```text
error CS0246: The type or namespace name 'NotificationService' could not be found
```

**This is GOOD!** You're in the "Red" phase of TDD. The tests define what you need to build.

### 2.5 Reflect on Test Design

Before implementing, review:

- ✅ Do test names clearly describe behavior?
- ✅ Are guard clause tests comprehensive?
- ✅ Is the happy path covered?
- ✅ Are tests organized by method?

---

## Step 3: Implement Code (Green Phase)

> **Goal**: Write the MINIMUM code needed to make tests pass. No more, no less.

### 3.1 Request Implementation

#### 🔷 .NET Prompt

In Copilot Chat, enter:

```text
Implement NotificationService that passes all the tests. Follow our .NET coding style: sealed class, file-scoped namespace, ILogger dependency injection, async/await, guard clauses with nameof.
```

#### 🟩 Spring Boot Prompt

In Copilot Chat, enter:

```text
Implement NotificationServiceImpl that passes all the tests. Follow our Spring Boot coding style: @Service annotation, constructor injection with @RequiredArgsConstructor, SLF4J logging, guard clauses with IllegalArgumentException, JavaDoc comments.
```

### 3.2 Review Generated Implementation

#### 🔷 .NET Implementation

Copilot should generate `src/TaskManager.Application/Services/NotificationService.cs`:

```csharp
namespace TaskManager.Application.Services;

public sealed class NotificationService : INotificationService
{
    private readonly ILogger<NotificationService> _logger;

    public NotificationService(ILogger<NotificationService> logger)
    {
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task SendEmailNotificationAsync(
        string recipient, 
        string subject, 
        string message, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient cannot be null or empty", nameof(recipient));
        
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject cannot be null or empty", nameof(subject));
        
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty", nameof(message));

        _logger.LogInformation(
            "Sending email notification to {Recipient} with subject {Subject}", 
            recipient, 
            subject);

        // Simulate email sending
        await Task.Delay(100, cancellationToken);

        _logger.LogInformation(
            "Email notification sent successfully to {Recipient}", 
            recipient);
    }

    public async Task SendSmsNotificationAsync(
        string phoneNumber, 
        string message, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be null or empty", nameof(phoneNumber));
        
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty", nameof(message));

        _logger.LogInformation(
            "Sending SMS notification to {PhoneNumber}", 
            phoneNumber);

        // Simulate SMS sending
        await Task.Delay(100, cancellationToken);

        _logger.LogInformation(
            "SMS notification sent successfully to {PhoneNumber}", 
            phoneNumber);
    }

    public async Task SendNotificationAsync(
        string recipient, 
        string phoneNumber, 
        string subject, 
        string message, 
        CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(recipient))
            throw new ArgumentException("Recipient cannot be null or empty", nameof(recipient));
        
        if (string.IsNullOrWhiteSpace(phoneNumber))
            throw new ArgumentException("Phone number cannot be null or empty", nameof(phoneNumber));
        
        if (string.IsNullOrWhiteSpace(subject))
            throw new ArgumentException("Subject cannot be null or empty", nameof(subject));
        
        if (string.IsNullOrWhiteSpace(message))
            throw new ArgumentException("Message cannot be null or empty", nameof(message));

        _logger.LogInformation(
            "Sending combined notification to email {Recipient} and phone {PhoneNumber}", 
            recipient, 
            phoneNumber);

        await SendEmailNotificationAsync(recipient, subject, message, cancellationToken);
        await SendSmsNotificationAsync(phoneNumber, message, cancellationToken);

        _logger.LogInformation(
            "Combined notification sent successfully");
    }
}
```

#### 🟩 Spring Boot Implementation

Copilot should generate `src-springboot/taskmanager-application/src/main/java/com/example/taskmanager/application/services/NotificationServiceImpl.java`:

```java
package com.example.taskmanager.application.services;

import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;

/**
 * Implementation of NotificationService.
 * Sends task notifications via email and SMS.
 * 
 * Note: This is a simplified implementation for testing purposes.
 * In production, would integrate with actual email and SMS providers.
 */
@Service
@RequiredArgsConstructor
public class NotificationServiceImpl implements NotificationService {
    
    private static final Logger log = LoggerFactory.getLogger(NotificationServiceImpl.class);
    
    @Override
    public void sendEmailNotification(String recipient, String subject, String message) {
        validateParameter(recipient, "Recipient");
        validateParameter(subject, "Subject");
        validateParameter(message, "Message");
        
        log.info("Sending email notification to {} with subject {}", recipient, subject);
        
        // Simulate email sending
        simulateDelay(100);
        
        log.info("Email notification sent successfully to {}", recipient);
    }
    
    @Override
    public void sendSmsNotification(String phoneNumber, String message) {
        validateParameter(phoneNumber, "Phone number");
        validateParameter(message, "Message");
        
        log.info("Sending SMS notification to {}", phoneNumber);
        
        // Simulate SMS sending
        simulateDelay(100);
        
        log.info("SMS notification sent successfully to {}", phoneNumber);
    }
    
    @Override
    public void sendNotification(String recipient, String phoneNumber, String subject, String message) {
        validateParameter(recipient, "Recipient");
        validateParameter(phoneNumber, "Phone number");
        validateParameter(subject, "Subject");
        validateParameter(message, "Message");
        
        log.info("Sending combined notification to email {} and phone {}", recipient, phoneNumber);
        
        sendEmailNotification(recipient, subject, message);
        sendSmsNotification(phoneNumber, message);
        
        log.info("Combined notification sent successfully");
    }
    
    /**
     * Validate that a parameter is not null or blank.
     *
     * @param value the value to validate
     * @param parameterName the parameter name for the exception message
     * @throws IllegalArgumentException if the value is null or blank
     */
    private void validateParameter(String value, String parameterName) {
        if (value == null || value.isBlank()) {
            throw new IllegalArgumentException(parameterName + " cannot be null or empty");
        }
    }
    
    /**
     * Simulate async operation delay.
     */
    private void simulateDelay(int milliseconds) {
        try {
            Thread.sleep(milliseconds);
        } catch (InterruptedException e) {
            Thread.currentThread().interrupt();
            throw new RuntimeException("Simulation interrupted", e);
        }
    }
}
```

**Key Spring Boot Patterns**:
- `@Service` annotation registers in Spring IoC container
- `@RequiredArgsConstructor` (Lombok) generates constructor
- Static `Logger` instance (common Java pattern)
- `IllegalArgumentException` for validation (Java standard)
- Helper method `validateParameter()` to reduce duplication
- `Thread.sleep()` for simulation (not `async`/`await`)

### 3.3 Verify Code Quality

#### 🔷 .NET Code Quality Checks

Check that the implementation follows all conventions:

- ✅ **`sealed class`** - Class cannot be inherited (defensive design)
- ✅ **File-scoped namespace** - `namespace TaskManager.Application.Services;`
- ✅ **Constructor validation** - `logger ?? throw new ArgumentNullException(nameof(logger))`
- ✅ **Guard clauses** - All parameters validated at method start
- ✅ **`nameof()` operator** - Used in all exceptions for refactoring safety
- ✅ **Async/await** - All methods properly async with `CancellationToken`
- ✅ **Structured logging** - Parameters passed to logger, not string interpolation
- ✅ **No `else` statements** - Guard clauses enable "fail fast" pattern
- ✅ **Single responsibility** - Class only handles notifications

#### 🟩 Spring Boot Code Quality Checks

Check that the implementation follows all conventions:

- ✅ **`@Service` annotation** - Marks as Spring service component
- ✅ **Interface implementation** - `implements NotificationService`
- ✅ **Lombok annotations** - `@Slf4j` for logging, `@RequiredArgsConstructor` for DI
- ✅ **Guard clauses** - All parameters validated at method start with `IllegalArgumentException`
- ✅ **SLF4J structured logging** - Uses parameterized logging: `log.info("...", param1, param2)`
- ✅ **No constructor needed** - Lombok generates it for `final` fields (if any dependencies added)
- ✅ **Single responsibility** - Class only handles notifications
- ✅ **Exception messages** - Clear, descriptive error messages

### 3.4 Run Tests (Expect Success - GREEN)

#### 🔷 .NET - Run Tests

In the terminal, run:

```bash
dotnet test
```

**Expected Result**: ✅ **Tests PASS**

You should see:

```text
Passed!  - Failed:     0, Passed:    12, Skipped:     0, Total:    12
```

#### 🟩 Spring Boot - Run Tests

In the terminal, run:

```bash
mvn test
```

or

```bash
./mvnw test  # If using Maven wrapper
```

**Expected Result**: ✅ **Tests PASS**

You should see:

```text
[INFO] Tests run: 12, Failures: 0, Errors: 0, Skipped: 0
[INFO] BUILD SUCCESS
```

**Congratulations!** You've completed the Red-Green cycle.

---

## Step 4: Observe & Reflect (Refactor Phase)

> **Goal**: Improve code quality without changing behavior. Tests should still pass.

### 4.1 Review Architecture

#### 🔷 .NET Architecture Review

Ask yourself:

- ✅ **Layer Separation**: Is `NotificationService` correctly in the Application layer?
  - Yes - it's a use case/service, not domain logic or infrastructure
- ✅ **Dependencies**: Does it only depend on `ILogger` (infrastructure concern)?
  - Yes - clean dependency injection
- ✅ **Domain Logic**: Is there any domain logic here?
  - No - this is pure application service orchestration

#### 🟩 Spring Boot Architecture Review

Ask yourself:

- ✅ **Layer Separation**: Is `NotificationServiceImpl` correctly in the `application.services` package?
  - Yes - it's a use case/service, not domain logic or infrastructure
- ✅ **Dependencies**: Does it only depend on `Logger` (crosscutting concern)?
  - Yes - clean separation (no infrastructure dependencies yet)
- ✅ **Domain Logic**: Is there any domain logic here?
  - No - this is pure application service orchestration
- ✅ **Spring Best Practices**: Using `@Service` for component scanning?
  - Yes - proper Spring stereotype annotation

### 4.2 Review Test Quality

Ask yourself:

- ✅ **Test Organization**: Are tests organized by method (nested test classes)?
- ✅ **Descriptive Names**: Can you understand behavior just by reading test names?
- ✅ **Test Coverage**: Are all edge cases covered (null, empty, whitespace)?
- ✅ **Test Independence**: Does each test run independently?
- ✅ **Mocking Strategy**: Are mocks used appropriately (only for dependencies)?


### 4.3 Ask Copilot for Improvements

> **Reusable Prompt (works for both stacks):**
> 
> Use the `/check` slash command in Copilot Chat to get code review and improvement suggestions:
> 
> ```text
> /check Review the NotificationService implementation and tests. Are there any improvements we could make while keeping the same behavior?
> ```

Copilot might suggest:

- **Extract validation logic** into a helper method (reduce duplication)
- **Add more specific exception types** (e.g., `InvalidEmailException`)
- **Add integration tests** for actual email/SMS providers
- **Add telemetry/tracing** with OpenTelemetry (workshop bonus!)
- **Spring Boot specific**: Consider using Spring Validation annotations (`@NotBlank`, `@Email`)

### 4.4 Optional Refactoring Exercise

#### 🔷 .NET - Extract Validation Logic

If time permits, try extracting parameter validation:

```csharp
private static void ValidateParameter(string value, string parameterName)
{
    if (string.IsNullOrWhiteSpace(value))
        throw new ArgumentException($"{parameterName} cannot be null or empty", parameterName);
}
```

Then refactor methods to use:

```csharp
ValidateParameter(recipient, nameof(recipient));
ValidateParameter(subject, nameof(subject));
ValidateParameter(message, nameof(message));
```

**Run tests again**: `dotnet test` - Should still pass! ✅

#### 🟩 Spring Boot - Extract Validation Logic

If time permits, try extracting parameter validation:

```java
private void validateParameter(String value, String parameterName) {
    if (value == null || value.isBlank()) {
        throw new IllegalArgumentException(parameterName + " cannot be null or blank");
    }
}
```

Then refactor methods to use:

```java
validateParameter(recipient, "recipient");
validateParameter(subject, "subject");
validateParameter(message, "message");
```

**Run tests again**: `mvn test` - Should still pass! ✅

---

## Key Learning Points

### ✅ TDD Benefits You Experienced

1. **Design First**: Interface and tests forced you to think about the API before writing code
2. **Clear Requirements**: Tests document exactly what the service should do
3. **Confidence**: Every change is validated by tests
4. **Refactoring Safety**: Can improve code structure without fear of breaking behavior
5. **No Overengineering**: Only wrote code needed to pass tests

### ✅ Copilot Instructions Impact

#### 🔷 .NET Code Generation

1. **Consistency**: All generated code follows the same conventions
2. **Quality**: Guard clauses, async/await, structured logging automatically included
3. **Best Practices**: Sealed classes, `nameof()`, file-scoped namespaces enforced
4. **Test Patterns**: xUnit + FakeItEasy patterns consistently applied

#### 🟩 Spring Boot Code Generation

1. **Consistency**: All generated code follows Spring Boot conventions
2. **Quality**: Guard clauses, exception handling, SLF4J logging automatically included
3. **Best Practices**: `@Service` annotations, Lombok annotations, proper interface implementation
4. **Test Patterns**: JUnit 5 + Mockito patterns consistently applied

### ⚠️ Common TDD Mistakes (Avoid These!)

1. ❌ **Writing implementation before tests** - You lose design feedback
2. ❌ **Writing tests after implementation** - Tests tend to just verify existing code, not drive design
3. ❌ **Skipping the "Red" phase** - You don't know if tests actually test anything
4. ❌ **Making tests pass by changing tests** - Tests define requirements; don't cheat!
5. ❌ **Ignoring failing tests** - Red → Green → Refactor, always in that order

---

## Extension Exercises (If Time Permits)

### Exercise 1: Add Email Validation

#### 🔷 .NET Version

1. Write a test that verifies email format validation
2. Implement email validation in `SendEmailNotificationAsync`
3. Consider using `System.ComponentModel.DataAnnotations.EmailAddressAttribute`
4. Ensure tests pass

#### 🟩 Spring Boot Version

1. Write a test that verifies email format validation
2. Implement email validation in `sendEmail`
3. Consider using Spring's `@Email` validation or Apache Commons Validator
4. Ensure tests pass

### Exercise 2: Add OpenTelemetry Tracing

#### 🔷 .NET Version

1. Research OpenTelemetry in the workshop instructions
2. Add `ActivitySource` tracing to notification methods
3. Write tests that verify activities are created

#### 🟩 Spring Boot Version

1. Research OpenTelemetry in the workshop instructions
2. Add `@WithSpan` annotations or manual span creation
3. Write tests that verify spans are created

### Exercise 3: Add Batch Notifications

#### 🔷 .NET Version

1. Design an interface method: `Task SendBatchNotificationsAsync(IEnumerable<Notification> notifications)`
2. Write tests for batch sending (multiple recipients)
3. Implement batch notification logic

#### 🟩 Spring Boot Version

1. Design an interface method: `void sendBatchNotifications(List<Notification> notifications)`
2. Write tests for batch sending (multiple recipients)
3. Implement batch notification logic

---

## Success Criteria

You've completed this lab successfully when:

### 🔷 .NET Success Criteria

- ✅ `INotificationService` interface created in Application layer
- ✅ Test suite created with 12+ passing tests using xUnit + FakeItEasy
- ✅ `NotificationService` implementation follows all Copilot Instructions conventions
- ✅ You followed Red-Green-Refactor cycle (saw tests fail, then pass)
- ✅ Code uses: sealed classes, file-scoped namespaces, guard clauses, `nameof()`
- ✅ Tests run successfully with `dotnet test`

### 🟩 Spring Boot Success Criteria

- ✅ `NotificationService` interface created in `application.services` package
- ✅ Test suite created with 12+ passing tests using JUnit 5 + Mockito
- ✅ `NotificationServiceImpl` follows Spring Boot best practices
- ✅ You followed Red-Green-Refactor cycle (saw tests fail, then pass)
- ✅ Code uses: `@Service`, Lombok annotations, guard clauses, SLF4J logging
- ✅ Tests run successfully with `mvn test`

### 🌐 Common Success Criteria

- ✅ You understand why TDD leads to better design
- ✅ Code is clean, readable, and well-organized
- ✅ You can explain the Red-Green-Refactor cycle

---

## Troubleshooting

### 🔷 .NET Issues

#### Tests Won't Compile

**Problem**: `NotificationService` type not found  
**Solution**: This is expected in the Red phase! Implement the service in Step 3.

#### Tests Pass Immediately

**Problem**: Tests pass even though no implementation exists  
**Solution**: Your tests might be too lenient. Review test assertions.

#### Copilot Not Following Conventions

**Problem**: Generated code doesn't use sealed classes, nameof, etc.  
**Solution**:

1. Verify `.github/instructions/` directory exists with instruction files
2. Reload VS Code window: `F1` → "Developer: Reload Window"
3. Be explicit in prompts: "Follow .NET conventions"

#### FakeItEasy Not Working

**Problem**: Can't create fakes or verify calls  
**Solution**:

1. Ensure using directive: `using FakeItEasy;`
2. Check NuGet package is installed in test project
3. Review FakeItEasy syntax in existing tests

### 🟩 Spring Boot Issues

#### Tests Won't Compile

**Problem**: `NotificationServiceImpl` symbol not found  
**Solution**: This is expected in the Red phase! Implement the service in Step 3.

#### Mockito Annotations Not Working

**Problem**: `@Mock` or `@InjectMocks` not injecting correctly  
**Solution**:

1. Ensure `@ExtendWith(MockitoExtension.class)` on test class
2. Check Mockito dependency in `pom.xml` (should be in parent)
3. Use `@BeforeEach` to manually initialize mocks if needed

#### Logger Not Injecting

**Problem**: Logger is null in service  
**Solution**:

1. Ensure SLF4J dependency is included
2. Use static logger: `private static final Logger log = LoggerFactory.getLogger(ClassName.class);`
3. Or inject with constructor if using a logging facade

#### Maven Build Failing

**Problem**: `mvn test` fails with compilation errors  
**Solution**:

1. Run `mvn clean` first to clear old builds
2. Verify Java 21 is active: `java -version`
3. Check all imports are correct (use VS Code auto-import: `Ctrl+.`)
4. Ensure Lombok plugin is installed in VS Code

#### Spring Boot Application Won't Start

**Problem**: Context fails to load during tests  
**Solution**:

1. This lab doesn't require full Spring Boot context
2. Use `@ExtendWith(MockitoExtension.class)` not `@SpringBootTest`
3. Mock all dependencies—don't start the application

####  Tests Pass But Coverage is Low

**Problem**: Code coverage tool shows gaps  
**Solution**:

1. Add more edge case tests
2. Test exception paths
3. Run `mvn test jacoco:report` to see coverage details

### Common to Both Stacks

#### Copilot Generates Wrong Framework Code

**Problem**: Copilot gives .NET code when you want Java or vice versa  
**Solution**:

1. Be explicit: "using Spring Boot" or "using .NET"
2. Reference file context: "in this Java file..." or "in this C# file..."
3. Check that correct instructions file is loading (status bar shows active instructions)

#### Tests Are Flaky

**Problem**: Tests sometimes pass, sometimes fail  
**Solution**:

1. Remove timing dependencies (use mocks, not actual delays)
2. Avoid shared state between tests
3. Use deterministic test data

---

## Next Steps

Move on to [**Lab 2: Requirements → Backlog → Code**](lab-02-requirements-to-code.md) where you'll:

- Convert user stories into backlog items with Copilot
- Generate acceptance criteria
- Build features from requirements
- Practice the full development workflow

---

## Additional Resources

### 🔷 .NET Resources

- [xUnit Documentation](https://xunit.net/)
- [FakeItEasy Documentation](https://fakeiteasy.github.io/)
- [Clean Architecture in .NET](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- [.NET Dependency Injection](https://learn.microsoft.com/en-us/dotnet/core/extensions/dependency-injection)

### 🟩 Spring Boot Resources

- [JUnit 5 User Guide](https://junit.org/junit5/docs/current/user-guide/)
- [Mockito Documentation](https://javadoc.io/doc/org.mockito/mockito-core/latest/org/mockito/Mockito.html)
- [Spring Boot Testing](https://docs.spring.io/spring-boot/docs/current/reference/html/features.html#features.testing)
- [Clean Architecture in Spring Boot](https://reflectoring.io/spring-boot-clean-architecture/)
- [Spring Dependency Injection](https://docs.spring.io/spring-framework/reference/core/beans/dependencies/factory-collaborators.html)

### 🌐 General Resources

- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Test-Driven Development (TDD) Guide](https://martinfowler.com/bliki/TestDrivenDevelopment.html)
- [Pattern Translation Guide](/workspaces/ai-code-workshop/docs/guides/dotnet-to-springboot-patterns.md) - .NET ↔ Spring Boot equivalencies
