# This document provides a detailed facilitator's guide for running the 3-hour workshop.

## Timing and Flow Tips

- **Keep labs hands-on**: Minimize lecture time, maximize coding time
- **Walking around**: Circulate during labs to help with issues
- **Backup plans**: Have pre-built examples ready if participants struggle
- **Flex time**: Each section has 5-10 minutes of flex time built in
- **Energy management**: Take 5-minute breaks between major sections
- **Advanced participants**: Have optional extension exercises ready
- **Copilot Instructions**: Repository uses `.github/copilot-instructions.md` for automatic configuration - no manual setup required by participants!

---ilitator's Guide: Using AI for Application Development with GitHub Copilot (.NET Edition)

This document provides a detailed facilitator’s guide for running the 3-hour workshop.

---

## 0. Kickoff & Setup (0:00 – 0:15, 15 min)
**You do**:
- Welcome participants, introduce goals: *"We'll learn how to use AI (Copilot) to help with requirements, code, tests, docs, and workflow in .NET projects."*
- Explain **Copilot Instructions** concept and `.github/copilot-instructions.md` approach.
- Quick demo: show that the repository already has instructions configured automatically.

**Participants do**:
- Confirm environment:
  - VS Code open
  - GitHub Copilot enabled
  - .NET 8 SDK installed (`dotnet --version`)
  - Clone starter repo and checkout `starter-projects` branch:
    ```bash
    git clone https://github.com/centricconsulting/ai-coding-workshop.git
    cd ai-coding-workshop
    git checkout starter-projects
    ```
- Open the repository in VS Code
- **Copilot Instructions are automatically active** via `.github/copilot-instructions.md` (no manual setup needed!)
- Verify build: `dotnet build` and `dotnet test`

---

## 0.5. GitHub Copilot Features Overview (0:15 – 0:30, 15 min)
**You do** - Quick tour of Copilot capabilities:

### **Inline Completions (Ghost Text)**
- As you type, Copilot suggests code in gray text
- Press `Tab` to accept, `Esc` to dismiss
- `Alt+]` / `Alt+[` to cycle through suggestions
- Works in comments, code, and tests

### **Copilot Chat Panel** (`Ctrl+Alt+I` or `Cmd+Shift+I`)
- Open chat interface for conversational coding
- Ask questions: *"How do I configure logging in .NET?"*
- Request code: *"Create a repository interface for Task entity"*
- Iterate on solutions without leaving VS Code

### **Inline Chat** (`Ctrl+I` or `Cmd+I`)
- Quick chat directly in your editor at cursor position
- Perfect for small edits: *"Add error handling"* or *"Make this method async"*
- Less context switching than full chat panel

### **Slash Commands** - Shortcuts for common tasks:
- `/explain` - Understand code functionality
- `/fix` - Suggest fixes for errors or bugs
- `/tests` - Generate unit tests for selected code
- `/doc` - Create documentation comments
- `/refactor` - Improve code structure
- `/new` - Scaffold new files or projects
- `/clear` - Clear chat history

**Demo**: Show `/tests` on a method, `/explain` on complex code

### **Chat Participants (Agents)** - Specialized assistants:
- `@workspace` - Answers about your entire codebase
  - *"@workspace Where is the Task entity defined?"*
  - *"@workspace How is logging configured?"*
- `@vscode` - VS Code settings and commands
  - *"@vscode How do I change theme?"*
- `@terminal` - Terminal commands and shell help
  - *"@terminal How do I run tests in watch mode?"*
- `@azure` - Azure-specific guidance (if available)

**Demo**: Show `@workspace` finding code across solution

### **Context Variables** - Provide specific context:
- `#file` - Reference specific files
  - *"Refactor #file:TaskService.cs to use dependency injection"*
- `#selection` - Reference selected code
  - *"Add unit tests for #selection"*
- `#editor` - Current file context
- `#terminalSelection` - Selected terminal output

**Demo**: Select code, use `#selection` in chat

### **Copilot Edits** (Multi-file editing)
- Edit multiple files at once with AI guidance
- Add files to working set, describe changes
- Copilot proposes changes across all files
- Review and accept/reject changes

**Demo**: Add multiple files, request cross-cutting change

**Participants do (Quick practice)**:
1. Try inline completion by typing a method comment
2. Open Copilot Chat (`Ctrl+Alt+I`), ask: *"What testing frameworks are used in this project?"*
3. Select a method, use Inline Chat (`Ctrl+I`): *"Add XML documentation"*
4. Try a slash command: `/explain` on any method
5. Use `@workspace`: *"@workspace Where is ITaskRepository implemented?"*

---

## 1. Controlling Context with Copilot Instructions (0:30 – 1:00, 30 min)
**You do**:
- Explain why *context matters* for Copilot output.
- Show the `.github/copilot-instructions.md` file in the repository.
- Explain that this file automatically configures Copilot for everyone working in this repo (no manual setup needed).
- Show difference with/without instructions (e.g., generate a class, note coding style vs messy defaults).
- Highlight key instructions in the file:
  - Coding style (file-scoped namespaces, `nameof`, async/await)
  - Clean Architecture project layout (Domain/Application/Infrastructure/API)
  - DDD aggregates and value objects
  - Test rules (xUnit + FakeItEasy)
  - Conventional commits
  - OpenTelemetry for observability

**Participants do (Lab 1)**:
1. Open `.github/copilot-instructions.md` and review the configured instructions.
2. Ask Copilot: *"Generate a C# service class that logs with ILogger and uses async methods."*
3. Observe: Code respects style and rules defined in the instructions file.
4. Try modifying the instructions file to see how it affects Copilot's suggestions (optional).

---

## 2. Requirements → Backlog → Code (1:00 – 1:45, 45 min)
**You do**:
- Introduce the idea: AI can turn **requirements → backlog items → tests → code**.
- Demo:  
  *User story:* *“As a user, I want to manage a list of tasks so I can track progress.”*  
  → Copilot generates backlog items (stories), acceptance criteria, test stubs.

**Participants do (Lab 2)**:
1. Write prompt: *“Generate 3 backlog items for a task manager, with acceptance criteria.”*
2. Pick one (e.g., Add Task).
3. Generate a unit test skeleton in xUnit for `AddTask`.
4. Implement `TaskService.AddTask` with Copilot.
5. Run `dotnet test` → verify.

---

## 3. Code Generation & Refactoring in .NET (1:45 – 2:30, 45 min)
**You do**:
- Show Copilot scaffolding: create a `TasksController` with minimal API.
- Show refactor of messy method (provided in repo): 
  - Before: long function, nested ifs, poor naming
  - After: Copilot helps split into smaller methods, add async, ILogger logging.

**Participants do (Lab 3)**:
1. Use `@workspace` to understand the API structure: *"@workspace Show me the API endpoint extensions"*
2. Scaffold minimal Web API endpoints using Chat:
   - `GET /tasks/{id}` - *"Implement the GetTaskByIdAsync endpoint in EndpointExtensions"*
   - `POST /tasks` - Use inline chat (`Ctrl+I`) with `#file:EndpointExtensions.cs`
3. Use `/refactor` on the `LegacyTaskProcessor.ProcessTaskBatch` method:
   - Select the method, Chat: `/refactor enforce guard clauses and add async`
   - Or use Inline Chat: *"Refactor this to use async/await and add logging"*
4. Use `/tests` on refactored code to generate unit tests
5. Re-run `dotnet build && dotnet test`.

---

## 4. Testing, Documentation, Workflow (2:30 – 2:45, 15 min)
**You do**:
- Show Copilot generating:
  - xUnit tests using `/tests` command
  - README docs using `/doc` command
  - Commit message using Chat with staged changes context
  - PR summary with `@workspace` for full context

**Participants do (Lab 4)**:
1. Select a method in `TaskService`, use `/tests` to generate xUnit tests
2. Use `/doc` to generate XML documentation for a class or method
3. Stage changes (`git add`), then use Chat: *"Write a Conventional Commit message for these staged changes"*
4. Ask Chat: *"@workspace Draft a PR description including intent, scope, and risks for the changes I made"*
5. Generate a README section: *"Create a Getting Started section for the API in #file:README.md"*

---

## 5. Wrap-Up & Discussion (2:45 – 3:00, 15 min)
**You do**:
- Recap: where Copilot helped (backlog shaping, scaffolding, refactoring, testing, docs, workflow).
- Call out **anti-patterns**:
  - Prompt roulette (unversioned prompts, inconsistent results)
  - Over-trusting Copilot without tests
  - Letting AI sneak domain logic into API layer
- Next steps:
  - Standardize Copilot Instructions in team repos
  - Build shared prompt/playbook library
  - Apply to real legacy code modernization

**Participants do**:
- Share takeaways.
- Ask Q&A: where would they use this tomorrow?

---

## Troubleshooting Common Issues

### Copilot Not Working
- **Check subscription**: Verify active GitHub Copilot subscription
- **Extension enabled**: Ensure Copilot extension is installed and enabled in VS Code
- **Authentication**: Sign out and back in to GitHub in VS Code
- **Instructions not loading**: 
  - Ensure you're working in the repository root (where `.github/` folder exists)
  - Restart VS Code to reload repository-level instructions
  - Check that `.github/copilot-instructions.md` exists in the repo
  - Try Command Palette → "GitHub Copilot: Restart Language Server"

### .NET Build Issues  
- **Wrong version**: Ensure .NET 8 SDK is installed (`dotnet --version`)
- **Missing dependencies**: Run `dotnet restore` in project directory
- **Path issues**: Use absolute paths or ensure correct working directory

### Copilot Generating Wrong Code
- **Check instructions**: Verify workshop instructions are properly configured
- **Context matters**: Include relevant files in VS Code workspace
- **Prompt clarity**: Be specific about requirements and constraints
- **Restart Copilot**: Command Palette → "GitHub Copilot: Restart Language Server"

---

## Deliverables Recap
- **Repo**: Starter projects in `starter-projects` branch (Clean Architecture solution with Domain/Application/Infrastructure/API layers)
- **Copilot Instructions**: `.github/copilot-instructions.md` (automatically applied, repository-level configuration)
- **Documentation**: 
  - Main README with workshop outline
  - Facilitator's Guide (this document)
  - Starter Projects README with architecture details
- **Code Examples**: Console app, Web API with OpenTelemetry, legacy code for refactoring (LegacyTaskProcessor)
- **Test Infrastructure**: xUnit test stubs with FakeItEasy ready for participants
