# GitHub Copilot Workshop - Starter Projects

This branch contains the complete starter project for the GitHub Copilot workshop. The solution demonstrates Clean Architecture principles with .NET 8 and includes modern observability practices through OpenTelemetry integration.

## 🏗️ Architecture Overview

```
TaskManager.sln
├── src/
│   ├── TaskManager.Domain/        # Domain entities and value objects
│   ├── TaskManager.Application/   # Business logic and interfaces
│   ├── TaskManager.Infrastructure/# Data access and external services
│   ├── TaskManager.Api/           # REST API with minimal APIs
│   └── TaskManager.ConsoleApp/    # Simple console application
└── tests/
    ├── TaskManager.UnitTests/     # Unit tests with xUnit + FakeItEasy
    └── TaskManager.IntegrationTests/ # Integration tests
```

## 🚀 Getting Started

### Prerequisites
- .NET 8.0 SDK or later
- Visual Studio Code with GitHub Copilot extension
- Git

### Quick Start
```bash
# Clone and navigate to the project
git clone <your-repo-url>
cd ai-code-workshop
git checkout starter-projects

# Build the solution
dotnet build

# Run tests
dotnet test

# Start the API
dotnet run --project src/TaskManager.Api/TaskManager.Api.csproj
```

The API will start at `http://localhost:5215` with OpenAPI documentation available at `/swagger`.

## 📚 Workshop Labs

### Lab 1: Test-Driven Development
**Location:** `tests/TaskManager.UnitTests/`
**Goal:** Use GitHub Copilot to implement failing tests and then generate the implementation.

**Key Files:**
- `TaskServiceTests.cs` - Service layer tests (stubs provided)
- `TaskRepositoryTests.cs` - Repository interface tests

**Instructions:**
1. Review the failing test stubs
2. Use Copilot to generate meaningful test implementations
3. Generate corresponding implementation code

### Lab 2: Clean Architecture & Refactoring
**Location:** `src/TaskManager.Application/`
**Goal:** Improve code quality and implement missing functionality using Copilot.

**Key Files:**
- `TaskService.cs` - Contains TODO methods to implement
- `LegacyTaskProcessor.cs` - Legacy code to refactor using Clean Architecture

**Instructions:**
1. Implement the TODO methods in TaskService
2. Refactor LegacyTaskProcessor to follow SOLID principles
3. Use Copilot to suggest better abstractions

### Lab 3: API Development with Modern Patterns
**Location:** `src/TaskManager.Api/Extensions/`
**Goal:** Scaffold minimal API endpoints using extension methods pattern.

**Key Files:**
- `EndpointExtensions.cs` - Map API endpoints here
- `ServiceExtensions.cs` - Dependency injection registration
- `OpenApiExtensions.cs` - OpenAPI/Swagger configuration

**Example Copilot Prompts:**
```
"Implement GET /tasks/{id} endpoint with proper error handling"
"Create POST /tasks endpoint with validation"
"Add PUT /tasks/{id}/status endpoint for status updates"
```

## 🔍 OpenTelemetry Integration

The starter project includes OpenTelemetry for modern observability practices:

### Features
- **Console Exporter:** Traces output to console for learning
- **ASP.NET Core Instrumentation:** Automatic HTTP request tracing
- **Custom Activity Source:** Manual instrumentation in TaskService

### Configuration
Located in `src/TaskManager.Api/Extensions/OpenTelemetryExtensions.cs`:

```csharp
services.AddOpenTelemetry()
    .WithTracing(builder =>
    {
        builder
            .AddSource("TaskManager.Application")
            .SetSampler(new AlwaysOnSampler())
            .AddAspNetCoreInstrumentation()
            .AddConsoleExporter();
    });
```

### Viewing Traces
Run the API and make requests to see traces in the console:
```bash
dotnet run --project src/TaskManager.Api/TaskManager.Api.csproj
curl http://localhost:5215/health
```

## 🧪 Testing Strategy

### Unit Tests
- **Framework:** xUnit with FakeItEasy for mocking
- **Coverage:** Service layer, repository interfaces, domain logic
- **Pattern:** Arrange-Act-Assert with descriptive test names

### Integration Tests
- **Scope:** API endpoints with in-memory database
- **Coverage:** Complete request/response cycles
- **Pattern:** Given-When-Then scenarios

### Test Discovery
All tests are discoverable in VS Code's Test Explorer:
```bash
dotnet test --list-tests
```

## 📁 Key Files Reference

### Domain Layer (`TaskManager.Domain/`)
```csharp
// Core business entities
Task.cs              // Main domain entity
TaskStatus.cs        // Value object enumeration
TaskPriority.cs      // Value object enumeration
```

### Application Layer (`TaskManager.Application/`)
```csharp
// Business logic interfaces
ITaskService.cs      // Primary service interface
ITaskRepository.cs   // Repository abstraction

// Implementations
TaskService.cs       // Service with TODO methods
LegacyTaskProcessor.cs // Legacy code for refactoring
```

### Infrastructure Layer (`TaskManager.Infrastructure/`)
```csharp
InMemoryTaskRepository.cs // Simple in-memory implementation
```

### API Layer (`TaskManager.Api/`)
```csharp
Program.cs           // Application entry point
Extensions/          // Organized extension methods:
  ├── ServiceExtensions.cs     // DI configuration
  ├── EndpointExtensions.cs    // API route mapping
  ├── OpenApiExtensions.cs     // Swagger setup
  ├── LoggingExtensions.cs     # Logging config
  └── OpenTelemetryExtensions.cs # Observability
```

## 🎯 Learning Objectives

By completing this workshop, developers will:

1. **Master GitHub Copilot:** Learn effective prompting techniques for code generation
2. **Apply Clean Architecture:** Understand dependency inversion and separation of concerns
3. **Practice TDD:** Use failing tests to drive implementation
4. **Modern API Patterns:** Implement minimal APIs with proper organization
5. **Observability Basics:** Understand distributed tracing with OpenTelemetry

## 🔧 GitHub Copilot Tips

### Effective Prompting
```
✅ Good: "Implement GetTaskByIdAsync with null checking and NotFound result"
❌ Poor: "Make this work"

✅ Good: "Create xUnit test for TaskService.CreateTaskAsync with invalid input"
❌ Poor: "Add test"
```

### Context Awareness
- Keep related files open for better context
- Use descriptive variable and method names
- Comment your intent before asking for implementation

### Custom Instructions
The project includes comprehensive GitHub Copilot instructions in `WORKSHOP-COPILOT-INSTRUCTIONS.md` that configure Copilot for:
- Clean Architecture patterns
- Domain-Driven Design
- Test-Driven Development
- Conventional commits
- .NET best practices

## 🚦 Troubleshooting

### Build Issues
```bash
# Clean and restore packages
dotnet clean
dotnet restore
dotnet build
```

### Test Issues
```bash
# Run tests with verbose output
dotnet test --logger "console;verbosity=detailed"
```

### API Issues
```bash
# Check if port is in use
lsof -i :5215

# Run with specific environment
ASPNETCORE_ENVIRONMENT=Development dotnet run --project src/TaskManager.Api
```

## 📖 Additional Resources

- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Clean Architecture Guide](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- [ASP.NET Core Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- [OpenTelemetry .NET](https://opentelemetry.io/docs/instrumentation/net/)
- [xUnit Documentation](https://xunit.net/docs/getting-started/netcore/cmdline)

---

Happy coding with GitHub Copilot! 🤖✨
