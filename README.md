# Workshop: Using AI for Application Development with GitHub Copilot (.NET Edition)

This repository contains materials for a 3-hour workshop. Participants will use **Visual Studio Code**, **.NET 8**, and **GitHub Copilot** to experience how AI can support application development with modern practices including Clean Architecture and OpenTelemetry observability.

## Prerequisites

Before attending this workshop, participants should have:

- **GitHub Copilot**: Active subscription and extension installed in VS Code
- **.NET 8 SDK**: Installed and verified with `dotnet --version`
- **Visual Studio Code**: Latest version with C# Dev Kit extension
- **Git**: Basic familiarity with git commands
- **C# Experience**: Comfortable with basic C# syntax and concepts
- **GitHub Account**: For cloning repositories and accessing Copilot

### Environment Check
Run these commands to verify your setup:
```bash
dotnet --version          # Should show 8.x.x
git --version            # Any recent version
code --version           # VS Code version
```

---

## Learning Objectives

By the end of this workshop, participants will be able to:

- **Leverage repository-level Copilot Instructions** (`.github/copilot-instructions.md`) for team-wide consistent code generation
- **Transform requirements** into backlog items, acceptance criteria, and working code using AI assistance
- **Generate and refactor .NET code** following Clean Architecture and DDD principles
- **Create comprehensive tests** and documentation with AI support
- **Apply conventional commits** and generate professional PR descriptions
- **Identify anti-patterns** and best practices when working with AI coding assistants

## Schedule

### 0. Kickoff & Setup (15 min)
- Goals and environment check
- Clone starter repo and checkout `starter-projects` branch
- Copilot instructions automatically configured via `.github/copilot-instructions.md`

### 0.5. GitHub Copilot Features Tour (15 min)
- Inline completions, Chat panel, and Inline Chat
- Slash commands: `/explain`, `/fix`, `/tests`, `/doc`, `/refactor`
- Chat participants: `@workspace`, `@vscode`, `@terminal`
- Context variables: `#file`, `#selection`, `#editor`
- Quick hands-on practice with each feature

### 1. Controlling Context with Copilot Instructions (30 min)
- Understand repository-level Copilot Instructions (`.github/copilot-instructions.md`)
- Lab 1: Review instructions and generate a sample service class

### 2. Requirements → Backlog → Code (45 min)
- Turn requirements into backlog items, tests, and code
- Lab 2: Backlog items → acceptance criteria → TaskService.AddTask

### 3. Code Generation & Refactoring (45 min)
- Scaffold APIs, refactor legacy methods with slash commands
- Lab 3: Minimal API with `@workspace`, refactor with `/refactor`, generate tests with `/tests`

### 4. Testing, Documentation, Workflow (15 min)
- Generate tests, docs, commit/PR messages using Copilot features
- Lab 4: `/tests` for unit tests, `/doc` for documentation, conventional commits

### 5. Wrap-Up & Discussion (15 min)
- Lessons learned
- Anti-patterns to avoid
- Next steps and Q&A

---

## Workshop Materials

### Core Files
- **[Copilot Instructions](.github/copilot-instructions.md)**: Repository-level Copilot configuration (automatically applied)
- **[Facilitator's Guide](docs/FACILITATOR_GUIDE.md)**: Detailed timing and talking points for instructors
- **[Starter Projects README](starter-projects/README.md)**: Complete architecture documentation and lab instructions

### Starter Projects ✅
Available in the `starter-projects` branch:
- **Complete Solution**: Clean Architecture with Domain/Application/Infrastructure/API layers
- **Console Application**: .NET 8 console app with DI and logging for initial exercises
- **Web API**: Minimal API with extension methods and OpenTelemetry integration
- **Legacy Code Sample**: `LegacyTaskProcessor` for refactoring exercises
- **Test Infrastructure**: xUnit test stubs with FakeItEasy ready for implementation

## Getting Started

1. **Clone this repository**:
   ```bash
   git clone https://github.com/centricconsulting/ai-coding-workshop.git
   cd ai-coding-workshop
   ```

2. **Checkout the starter projects branch**:
   ```bash
   git checkout starter-projects
   ```

3. **Open in VS Code**:
   ```bash
   code .
   ```
   
   **That's it!** Copilot instructions are automatically configured via `.github/copilot-instructions.md` - no manual setup needed.

4. **Verify your environment**:
   ```bash
   dotnet --version    # Should show 8.x.x or later
   dotnet build        # Verify solution builds
   dotnet test         # Verify tests run
   ```

5. **Ready to start!** Follow along with your facilitator or work through the labs independently
