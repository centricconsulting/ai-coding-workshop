# Starter Projects - GitHub Copilot Workshop

This directory contains the **starting point** for the GitHub Copilot workshop. Participants begin here with a partially-implemented solution and use GitHub Copilot to complete the implementation through 4 hands-on labs.

## 📦 What's Included (Pre-Built)

The starter projects provide a foundation following Clean Architecture principles:

### ✅ **Complete Infrastructure**
- **Clean Architecture Solution**: Domain, Application, Infrastructure, API layers properly structured
- **Dependency Injection**: Fully configured with `Microsoft.Extensions.DependencyInjection`
- **Logging**: ILogger integration throughout
- **OpenTelemetry**: Distributed tracing configured with console exporter
- **Testing Framework**: xUnit + FakeItEasy setup with test projects

### ✅ **Domain Layer** (`src/TaskManager.Domain/`)
- `Task` entity with factory method (partial implementation)
- `TaskId` strongly-typed ID (value object)
- `TaskStatus` enum (Todo, InProgress, Done, Cancelled)
- Basic structure ready for enhancement

### ✅ **Application Layer** (`src/TaskManager.Application/`)
- `ITaskRepository` interface (contract defined)
- `TaskService` skeleton with TODO markers
- Service interfaces ready for implementation

### ✅ **Infrastructure Layer** (`src/TaskManager.Infrastructure/`)
- `InMemoryTaskRepository` basic implementation
- Repository pattern structure
- Legacy code sample for refactoring exercises

### ✅ **API Layer** (`src/TaskManager.Api/`)
- Minimal API host configured
- Extension methods pattern (ServiceExtensions, EndpointExtensions, OpenApiExtensions)
- OpenTelemetry integration
- Health check endpoint
- Swagger/OpenAPI ready

### ✅ **Console App** (`src/TaskManager.ConsoleApp/`)
- DI container configured
- Logging setup
- Ready for Lab 1 exercises

### ✅ **Test Projects**
- **Unit Tests**: Test stubs with `NotImplementedException` placeholders
- **Integration Tests**: Test stubs ready for Copilot generation
- Test organization by feature and method

---

## 🚧 What Participants Will Build (Labs 1-4)

### **Lab 1: TDD with Copilot** (30 min)
Participants use Copilot to implement:
- `INotificationService` interface
- `NotificationService` implementation (email/SMS notifications)
- Complete test suite following TDD Red-Green-Refactor

**Starting Point**: Console app with DI configured  
**Output**: Fully-tested notification service

---

### **Lab 2: Requirements to Code** (45 min)
Participants use Copilot to implement:
- `CreateTaskCommand` and `CreateTaskCommandHandler`
- `Priority` value object (Low, Medium, High, Critical)
- Enhanced `Task` entity with Priority and DueDate
- Business validation rules
- Complete unit test suite

**Starting Point**: Domain entity skeleton  
**Output**: Complete task creation feature with CQRS pattern

---

### **Lab 3: Code Generation & Refactoring** (45 min)
Participants use Copilot to:
- Generate `GetTasksQuery` and handler
- Implement POST `/tasks` endpoint with DTOs
- Refactor `LegacyTaskProcessor` to modern async code
- Add integration tests
- Create `.http` file for manual testing

**Starting Point**: API with extension method structure  
**Output**: Working CRUD API with refactored legacy code

---

### **Lab 4: Testing, Documentation & Workflow** (15 min)
Participants use Copilot to:
- Generate remaining unit and integration tests
- Add XML documentation comments
- Write conventional commit messages
- Create PR descriptions

**Starting Point**: Working code from Labs 1-3  
**Output**: Production-ready code with tests and documentation

---

## 🏗️ Architecture Overview

```
TaskManager.sln
├── src/
│   ├── TaskManager.Domain/          # Domain entities, value objects, enums
│   │   ├── Tasks/
│   │   │   ├── Task.cs             # Aggregate root (partial)
│   │   │   ├── TaskId.cs           # Strongly-typed ID
│   │   │   └── TaskStatus.cs       # Status enum
│   │   └── Repositories/
│   │       └── ITaskRepository.cs   # Repository interface
│   │
│   ├── TaskManager.Application/     # Business logic and use cases
│   │   ├── Services/
│   │   │   ├── TaskService.cs      # TODO: Implement methods
│   │   │   └── INotificationService.cs # TODO: Lab 1
│   │   ├── Commands/                # TODO: Lab 2 (CQRS)
│   │   ├── Queries/                 # TODO: Lab 3 (CQRS)
│   │   └── Handlers/                # TODO: Labs 2 & 3
│   │
│   ├── TaskManager.Infrastructure/  # Data access and external services
│   │   ├── Repositories/
│   │   │   └── InMemoryTaskRepository.cs # Simple implementation
│   │   └── Legacy/
│   │       └── LegacyTaskProcessor.cs    # TODO: Lab 3 refactoring
│   │
│   ├── TaskManager.Api/             # REST API with Minimal APIs
│   │   ├── Program.cs               # Host configuration
│   │   ├── Extensions/
│   │   │   ├── ServiceExtensions.cs       # DI registration
│   │   │   ├── EndpointExtensions.cs      # TODO: Lab 3 endpoints
│   │   │   ├── OpenApiExtensions.cs       # Swagger config
│   │   │   ├── LoggingExtensions.cs       # Logging setup
│   │   │   └── OpenTelemetryExtensions.cs # Tracing
│   │   └── Models/                  # TODO: Lab 3 DTOs
│   │
│   └── TaskManager.ConsoleApp/      # Lab 1 starting point
│       └── Program.cs               # DI and logging configured
│
└── tests/
    ├── TaskManager.UnitTests/       # TODO: Implement tests (Labs 1-4)
    │   ├── UnitTest1.cs            # Test stubs with NotImplementedException
    │   ├── Services/               # TODO: Lab 1 tests
    │   ├── Domain/                 # TODO: Lab 2 tests
    │   └── Application/            # TODO: Labs 2 & 3 tests
    │
    └── TaskManager.IntegrationTests/ # TODO: Implement tests (Labs 3-4)
        └── UnitTest1.cs            # Test stubs with NotImplementedException
```

---

## 🚀 Getting Started

### Prerequisites
- **.NET 9 SDK** (or later) - `dotnet --version` should show 9.x.x
- **Visual Studio Code** with:
  - GitHub Copilot extension
  - C# Dev Kit extension
- **Git** and **GitHub account** with Copilot subscription
- **Basic C# knowledge**: Async/await, LINQ, dependency injection

### Initial Setup

1. **Clone the repository**:
   ```bash
   git clone https://github.com/centricconsulting/ai-coding-workshop.git
   cd ai-coding-workshop
   ```

2. **Checkout starter-projects branch**:
   ```bash
   git checkout starter-projects
   ```

3. **Open in VS Code**:
   ```bash
   code .
   ```

4. **Verify GitHub Copilot is active**:
   - Check bottom-right status bar for Copilot icon
   - Should show ✅ (enabled) or 🔴 (disabled)
   - Copilot instructions automatically loaded from `.github/copilot-instructions.md`

5. **Build and test**:
   ```bash
   dotnet build
   dotnet test   # Tests will fail - this is expected!
   ```

   Expected output:
   ```
   Build succeeded
   Test summary: total: 11, failed: 11, succeeded: 0
   ```

   ✅ **This is correct!** The failing tests have placeholders for participants to implement.

---

## 🎯 Expected Test Failures (By Design)

The starter projects intentionally include **11 failing tests** with `NotImplementedException`:

### Unit Tests (6 failures)
- `AddTaskAsync_WithValidData_ShouldReturnTaskId` - Lab 2
- `AddTaskAsync_WithNullTitle_ShouldThrowArgumentException` - Lab 2
- `AddTaskAsync_WithEmptyDescription_ShouldThrowArgumentException` - Lab 4
- `GetTaskAsync_WithExistingId_ShouldReturnTask` - Lab 4
- `GetTaskAsync_WithNonExistentId_ShouldReturnNull` - Lab 4
- `UpdateTaskStatusAsync_WithValidData_ShouldUpdateSuccessfully` - Lab 4

### Integration Tests (5 failures)
- `CreateTask_WithValidData_ShouldReturn201` - Lab 4
- `GetTask_WithExistingId_ShouldReturn200` - Lab 4
- `GetTask_WithNonExistentId_ShouldReturn404` - Lab 4
- `UpdateTaskStatus_WithValidData_ShouldReturn200` - Lab 4
- `GetActiveTasks_ShouldReturn200WithTaskList` - Lab 4

**These tests serve as:**
- 🎯 **Learning objectives** for each lab
- 📝 **Implementation guides** (test names describe what to build)
- ✅ **Success criteria** (when tests pass, lab is complete)

---

## 🔍 Key Features Demonstrated

### Clean Architecture
- **Domain Layer**: No dependencies, pure business logic
- **Application Layer**: Depends on Domain only, defines interfaces
- **Infrastructure Layer**: Implements Application interfaces
- **API Layer**: Depends on Infrastructure, orchestrates requests

### Modern .NET Patterns
- **Minimal APIs**: Extension methods for organization
- **Dependency Injection**: `IServiceCollection` extensions
- **Async/await**: Throughout codebase
- **Repository Pattern**: Interface-based data access
- **CQRS Lite**: Commands and Queries separation

### OpenTelemetry Integration
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

**To see traces**:
```bash
dotnet run --project src/TaskManager.Api/TaskManager.Api.csproj
curl http://localhost:5215/health
```

Traces appear in console showing request lifecycle.

---

## 📚 GitHub Copilot Instructions

The repository includes **automatic Copilot configuration** via `.github/copilot-instructions.md`:

### What's Configured
- ✅ **TDD Workflow**: "Propose tests before code"
- ✅ **Coding Style**: File-scoped namespaces, sealed classes by default, async/await
- ✅ **Clean Architecture**: Project structure and dependency rules
- ✅ **DDD Patterns**: Aggregates, value objects, strongly-typed IDs
- ✅ **Testing Rules**: xUnit + FakeItEasy, organize by feature/method
- ✅ **Conventional Commits**: Format and examples
- ✅ **Object Calisthenics**: Refactoring guidelines

### How It Works
When you open this repository in VS Code with GitHub Copilot:
1. Copilot automatically loads instructions from `.github/copilot-instructions.md`
2. All suggestions follow these conventions
3. No manual configuration needed!

### Testing Instructions Work
Try asking Copilot:
```
"Create a DDD aggregate with factory method and invariants"
```

Copilot will generate code following the workshop patterns:
- Sealed class
- Private constructor
- Static factory method
- File-scoped namespace
- Guard clauses with `nameof()`

---

## 🛠️ Development Workflow

### Running the API
```bash
cd src/TaskManager.Api
dotnet run

# Or with watch mode
dotnet watch run
```

API available at: `http://localhost:5215`  
Swagger UI: `http://localhost:5215/swagger`

### Running Tests
```bash
# All tests
dotnet test

# With detailed output
dotnet test --logger "console;verbosity=detailed"

# Watch mode (re-run on file changes)
dotnet watch test

# Specific project
dotnet test tests/TaskManager.UnitTests/
```

### Building
```bash
# Full solution
dotnet build

# Specific project
dotnet build src/TaskManager.Domain/

# Release configuration
dotnet build --configuration Release
```

---

## 🧪 Testing Strategy

### Unit Tests
- **Location**: `tests/TaskManager.UnitTests/`
- **Framework**: xUnit 2.8+
- **Mocking**: FakeItEasy
- **Pattern**: Arrange-Act-Assert
- **Organization**: Folders by feature, classes per method

**Example Structure**:
```
tests/TaskManager.UnitTests/
├── Services/
│   └── NotificationServiceTests/          # One folder per class
│       ├── SendEmailNotificationAsyncTests.cs    # One class per method
│       ├── SendSmsNotificationAsyncTests.cs
│       └── SendNotificationAsyncTests.cs
├── Domain/
│   └── TaskTests/
│       ├── CreateTests.cs
│       ├── UpdateStatusTests.cs
│       └── UpdatePriorityTests.cs
└── Application/
    └── Commands/
        └── CreateTaskCommandHandlerTests.cs
```

### Integration Tests
- **Location**: `tests/TaskManager.IntegrationTests/`
- **Framework**: xUnit + WebApplicationFactory
- **Scope**: API endpoints end-to-end
- **Pattern**: Given-When-Then

---

## 🚦 Troubleshooting

### Copilot Not Working
```bash
# Check Copilot status
# Look for GitHub Copilot icon in VS Code status bar

# Reload Copilot instructions
# Command Palette (Cmd/Ctrl+Shift+P) → "Reload Window"

# Verify subscription
# GitHub.com → Settings → Copilot
```

### Build Errors
```bash
# Clean solution
dotnet clean

# Restore packages
dotnet restore

# Rebuild
dotnet build
```

### Test Failures Beyond Expected
```bash
# Check if you're on correct branch
git branch --show-current  # Should show: starter-projects

# Check for uncommitted changes
git status

# Reset to clean state (WARNING: loses changes)
git reset --hard origin/starter-projects
```

### Port Already in Use
```bash
# Find process using port 5215
lsof -i :5215

# Kill process (replace PID with actual)
kill -9 <PID>
```

---

## 📖 Additional Resources

### Documentation
- **Workshop README**: `../README.md` - Overall workshop structure
- **Facilitator Guide**: `../docs/FACILITATOR_GUIDE.md` - Instructor notes
- **Lab Walkthroughs**: `../docs/labs/` - Detailed step-by-step guides
- **Copilot Instructions**: `../.github/copilot-instructions.md` - Configuration reference

### External Resources
- [GitHub Copilot Docs](https://docs.github.com/en/copilot)
- [.NET 9 Documentation](https://learn.microsoft.com/en-us/dotnet/core/whats-new/dotnet-9)
- [Clean Architecture (Microsoft)](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures)
- [Minimal APIs](https://learn.microsoft.com/en-us/aspnet/core/fundamentals/minimal-apis)
- [xUnit Documentation](https://xunit.net/)
- [OpenTelemetry .NET](https://opentelemetry.io/docs/instrumentation/net/)

---

## 🎓 Workshop Flow

Participants follow this sequence:

1. **Start here** (starter-projects branch) ✅ You are here
2. **Lab 1**: Implement NotificationService with TDD
3. **Lab 2**: Create task creation feature with CQRS
4. **Lab 3**: Build CRUD API and refactor legacy code
5. **Lab 4**: Complete tests and documentation
6. **Reference** (test-lab-walkthrough branch) - Completed implementation available if stuck

---

## 💡 Pro Tips for Workshop Success

### For Participants
1. **Read TODO comments** - They guide what to implement
2. **Keep related files open** - Copilot uses context from open tabs
3. **Use descriptive names** - Better names = better Copilot suggestions
4. **Iterate prompts** - If first suggestion isn't right, refine your ask
5. **Run tests frequently** - Use TDD Red-Green-Refactor cycle

### For Facilitators
1. **Show test-lab-walkthrough** if participants get stuck
2. **Emphasize instructions** - Point out how Copilot follows `.github/copilot-instructions.md`
3. **Circulate during labs** - Common issues: Copilot disabled, wrong branch, missing context
4. **Time management** - Labs 2 & 3 take longest, adjust breaks accordingly

---

Ready to build with GitHub Copilot? **Start with Lab 1!** 🚀

See [Lab 1: TDD with Copilot](../docs/labs/lab-01-tdd-with-copilot.md) to begin.
