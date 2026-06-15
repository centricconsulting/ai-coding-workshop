# This document provides a detailed facilitator's guide for running the 3-hour workshop.

## Timing and Flow Tips

- **Keep labs hands-on**: Minimize lecture time, maximize coding time
- **Walking around**: Circulate during labs to help with issues
- **Backup plans**: Have pre-built examples ready if participants struggle
- **Flex time**: Each section has 5-10 minutes of flex time built in
- **Energy management**: Take 5-minute breaks between major sections
- **Advanced participants**: Have optional extension exercises ready
- **Copilot Instructions**: Repository uses `.github/instructions/` with context-aware `applyTo` patterns - no manual setup required by participants!
- **Dual-Stack Delivery**:
  - **Homogeneous groups** (.NET only or Java only): Timing estimates remain unchanged
  - **Mixed groups** (.NET and Java together): Add 5-10 minutes to Labs 1-3 for cross-stack comparison discussions
  - Labs 4-10 are mostly stack-agnostic - no timing adjustments needed

Facilitator's Guide: Using AI for Application Development with GitHub Copilot (Bilingual Edition: .NET & Spring Boot)

This document provides a detailed facilitator's guide for running the 3-hour workshop with support for both .NET and Spring Boot technology stacks.


## 0. Kickoff & Setup (0:00 – 0:15, 15 min)

**You do**:

- Welcome participants, introduce goals: *"We'll learn how to use AI (Copilot) to help with requirements, code, tests, docs, and workflow in software projects using either .NET or Spring Boot."*
- **Poll the room**: Ask participants to raise hands for .NET vs Spring Boot (helps gauge group composition)
- Explain **Copilot Instructions** concept and context-aware instruction loading (`.github/instructions/` with `applyTo` patterns).
- Quick demo: show that instructions automatically load based on file context:
  - 🔷 **.NET**: Files matching `**/*.cs` → `csharp.instructions.md` and `dotnet.instructions.md`
  - 🟩 **Spring Boot**: Files matching `src-springboot/**` → `springboot.instructions.md`

- **Present:** Use the **[Modular Presentations](./presentations/modules/part1/)** (Marp format) to guide Part 1
  - [Part 1 Module Catalog](./presentations/index.md#part-1-fundamentals-3-hours) - 7 standalone modules
  - View with Marp VS Code extension or export to PDF
  - Modules cover all labs with talking points
  - **Archived:** [Legacy monolithic presentation](../archive/presentations/fundamentals-github-copilot.md) available for reference

**Participants do**:

- Confirm environment:
  - VS Code open
  - GitHub Copilot enabled
  - **🔷 .NET**: .NET 9 SDK installed (`dotnet --version`)
  - **🟩 Spring Boot**: Java 21 JDK installed (`java -version`), Maven/Gradle

- Clone the repository and checkout the `main` branch:

  ```bash
  git clone https://github.com/centricconsulting/ai-coding-workshop.git
  cd ai-coding-workshop
  git checkout main
  ```

- **Select DevContainer** based on your technology stack:
  - 🔷 **.NET Participants**: "Reopen in Container" → Choose `.devcontainer/dotnet/devcontainer.json`
  - 🟩 **Spring Boot Participants**: "Reopen in Container" → Choose `.devcontainer/springboot/devcontainer.json`
  - 🔷🟩 **Both Stacks** (facilitators/explorers): Choose `.devcontainer/bilingual/devcontainer.json`

- **Copilot Instructions automatically load** based on file context via `.github/instructions/` (no manual setup needed!)

- **Verify build**:
  - 🔷 **.NET**: `dotnet build` and `dotnet test` in project root
  - 🟩 **Spring Boot**: `cd src-springboot && mvn clean test`

---

## 0.1. Tech Stack Selection & Group Composition (After Setup, 5 min)

**You do**:

- **For Homogeneous Groups (.NET only OR Java only)**:
  - Simplify delivery: focus on one technology stack throughout
  - Skip "dual-stack" explanations and comparisons
  - Lab materials support both tracks independently
  - Emphasize stack-specific Copilot Instructions file (`.github/instructions/dotnet.instructions.md` or `springboot.instructions.md`)

- **For Mixed Groups (.NET AND Java together)**:
  - Explain **bilingual format** with 🔷 .NET and 🟩 Spring Boot markers
  - Encourage cross-pollination: ".NET devs can learn from Java patterns and vice versa"
  - Pair mixed-stack participants during labs for knowledge sharing
  - **Labs 1-3**: Significant stack-specific content (separate files or bilingual sections)
  - **Labs 4-10**: Mostly stack-agnostic (agent concepts, testing philosophy)

- **For Enterprise Java Teams Modernizing from Mule ESB**:
  - Reference [Pattern Translation Guide](guides/dotnet-to-springboot-patterns.md) for Mule → Spring Boot mappings
  - Highlight **Lab 3** refactoring scenarios (includes Mule ESB legacy code examples)
  - Mention modernization agent (#20 - in development) for Mule → Spring Boot transformation assistance
  - Emphasize Cost of Delay for legacy integration platforms

**Participants do**:

- Confirm their chosen stack and verify devcontainer selection
- Open a sample file in their stack to see Copilot Instructions load:
  - 🔷 **.NET**: Open any `.cs` file → see csharp.instructions.md and dotnet.instructions.md activate
  - 🟩 **Spring Boot**: Open any file in `src-springboot/` → see springboot.instructions.md activate

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

**Code Operations:**
- `/explain` - Understand code functionality
- `/fix` - Suggest fixes for errors or bugs
- `/tests` - Generate unit tests for selected code
- `/doc` - Create documentation comments
- `/refactor` - Improve code structure
- `/new` - Scaffold new files or projects

**Customization Commands:**
- `/init` - Initialize project with custom instructions
- `/create-agent` - Generate custom agent with AI
- `/create-skill` - Generate agent skill with AI
- `/create-instruction` - Generate instruction file with AI
- `/create-prompt` - Generate prompt file with AI
- `/create-hook` - Generate hook configuration with AI

**Navigation:**
- `/agents` - Open Configure Custom Agents menu
- `/skills` - Open Configure Skills menu
- `/clear` - Clear chat history


**Demo**: Show `/tests` on a method, `/explain` on complex code, `/init` for project setup

---
### 📝 Plan First with Agents (Demo)

**Goal:** Demonstrate how Copilot (in Agent Mode) or a custom agent like `@planner` can propose a step-by-step plan before any code is changed.

**How to show it:**
- In Copilot Chat (Agent Mode), ask: "Propose a step-by-step plan to refactor LegacyTaskProcessor to use async/await, add logging, and follow Object Calisthenics."
- Review the plan with the group. Edit or reorder steps as needed.
- Only then, ask Copilot (or `@engineer`) to implement the plan, step by step.

**Discussion:**
- Why is planning first valuable?
- Did the plan catch any issues or clarify the approach?
- How does this workflow compare to direct code generation?

**Tip:** Encourage participants to always ask for a plan before executing large or multi-file changes.


### **Agent Mode & MCP Tools**

**Agent Mode** allows Copilot to operate more autonomously, chaining together multiple steps, tools, and reasoning to solve complex tasks. This is especially powerful for:
- Multi-file or cross-cutting changes
- Automated codebase analysis or refactoring
- Running evaluation or test suites with minimal manual intervention
- Integrating with Model Context Protocol (MCP) tools for advanced workflows

**MCP Tools** provide specialized capabilities (e.g., evaluation, tracing, agent orchestration) that can be invoked directly in Agent Mode. These tools enable:
- Automated evaluation of code or models
- Tracing and observability for AI workflows
- Agent-driven code generation and review

**How to use Agent Mode:**
1. Select "Agent" from the Copilot agents dropdown (or use the command palette).
2. Describe your goal in natural language (e.g., "Refactor all service classes to use async/await and add logging").
3. For advanced scenarios, reference MCP tools directly (e.g., "Evaluate this model using the aitk-evaluation_planner tool").
4. Review the proposed plan and results; iterate as needed.

**When to use Agent Mode:**
- When a task spans multiple files or requires orchestration
- For codebase-wide refactoring or analysis
- To automate repetitive or evaluation-heavy workflows
- When you want Copilot to propose and execute a plan, not just a single edit

**Facilitator Tips:**
- Encourage participants to try Agent Mode for at least one lab (e.g., Lab 3 or 4)
- Compare results from Ask/Plan vs. Agent Mode
- Demonstrate invoking an MCP tool (e.g., evaluation or tracing) and discuss the output

**Example prompt:**
> "Use Agent Mode to generate unit tests for all public methods in the Application layer, then run the tests and summarize the results."

---
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
- **Show stack-specific instructions**:
  - 🔷 **.NET**: `.github/instructions/dotnet.instructions.md` (loaded for `.cs` files)
  - 🟩 **Spring Boot**: `.github/instructions/springboot.instructions.md` (loaded for `src-springboot/**`)
- Explain that instruction files automatically load based on file patterns (no manual setup needed).
- **Emphasize Section 1: TDD Workflow** - "When asked to implement a feature, propose/emit tests before code"
- Show difference with/without instructions (e.g., generate a class, note coding style vs messy defaults).
- **Highlight key instructions** common to both stacks:
  - **TDD first**: Write tests before implementation
  - Clean Architecture project layout (Domain/Application/Infrastructure/API)
  - DDD aggregates and value objects
  - Conventional commits
- **Stack-specific patterns**:
  - 🔷 **.NET**: File-scoped namespaces, `nameof`, async/await, sealed classes, xUnit + FakeItEasy, OpenTelemetry
  - 🟩 **Spring Boot**: @Service/@Repository, @Slf4j, Optional types, JUnit 5 + Mockito, Spring Boot Actuator

**Participants do (Lab 1) - Following TDD Red-Green-Refactor**:

> **Note for Facilitators**: [Lab 01](labs/lab-01-tdd-with-copilot.md) is **bilingual** with 🔷 .NET and 🟩 Spring Boot sections throughout. Participants work through their chosen stack's examples.

**Scenario**: Create a `NotificationService` that sends task notifications via email and SMS.

**High-Level Steps** (details in lab file):

1. **Create Interface First** (Design)
   - 🔷 **.NET**: `INotificationService` in `src/TaskManager.Application/Services/`
   - 🟩 **Spring Boot**: `NotificationService` interface in `notification` package

2. **Write Tests FIRST** (Red)
   - 🔷 **.NET**: xUnit tests with FakeItEasy mocking, organized by method in class-per-method folders
   - 🟩 **Spring Boot**: JUnit 5 tests with Mockito, organized by feature in test classes
   - Run tests → Should FAIL (Red phase)

3. **Implement Code** (Green)
   - 🔷 **.NET**: Sealed class, file-scoped namespace, async/await, guard clauses, structured logging
   - 🟩 **Spring Boot**: @Service annotation, constructor injection, @Slf4j, Optional handling
   - Run tests → Should PASS (Green phase)

4. **Observe & Reflect** (Refactor)
   - Review code quality and adherence to instructions
   - Ask Copilot for improvement suggestions

**Key Learning Points to Emphasize** (universal across stacks):

- ✅ **TDD enforces design thinking** - interface and tests force you to think about API before implementation
- ✅ **Copilot respects instructions** - consistent style across all generated code
- ✅ **Tests document behavior** - reading tests tells you exactly what the service does
- ✅ **Red-Green-Refactor cycle** - see tests fail, then pass, then improve
- ⚠️ **Don't skip the "Red" step** - if you write implementation first, you miss design feedback from tests

**Common Mistakes to Call Out**:

- ❌ Asking for implementation before tests (violates TDD)
- ❌ Not organizing tests by feature/method (makes tests hard to navigate)
- ❌ Accepting code without verifying it follows instructions
- ❌ Not running tests after each step

---

## 2. Requirements → Backlog → Code (1:00 – 1:45, 45 min)

**You do**:

- Introduce the idea: AI can turn **requirements → backlog items → tests → code**.
- Demo:  
  *User story:* *“As a user, I want to manage a list of tasks so I can track progress.”*  
  → Copilot generates backlog items (stories), acceptance criteria, test stubs.

**Participants do (Lab 2)**:

> **Note for Facilitators**: Lab 02 has **separate files** for each stack:
> - 🔷 **.NET**: [lab-02-requirements-to-code.md](labs/lab-02-requirements-to-code.md)
> - 🟩 **Spring Boot**: [lab-02-requirements-to-code-java.md](labs/lab-02-requirements-to-code-java.md)

**High-Level Steps** (participants follow their stack's lab file):

1. Generate backlog items from user story: *"As a user, I want to manage a list of tasks so I can track progress."*
2. Use **Backlog Generator agent** (optional) or manual prompting
3. Pick one backlog item (e.g., Add Task, Update Task status)
4. Generate unit test skeleton following TDD
   - 🔷 **.NET**: xUnit test for `TaskService.AddTask`
   - 🟩 **Spring Boot**: JUnit 5 test for `TaskService.createTask`
5. Implement the service method with Copilot
6. Run tests and verify
7. Implement API endpoint (Minimal API vs @RestController)

**Key Point**: Both stacks follow CQRS pattern - emphasize separation of commands and queries

---

## 3. Code Generation & Refactoring (1:45 – 2:30, 45 min)

**You do**:

- Show Copilot scaffolding for REST API endpoints
  - 🔷 **.NET**: Create `TasksController` with minimal API style
  - 🟩 **Spring Boot**: Create `TaskController` with @RestController
- Show refactor of messy legacy code (provided in repo):
  - Before: long function, nested ifs, poor naming, synchronous operations
  - After: Copilot helps split into smaller methods, add async, structured logging

**Participants do (Lab 3)**:

> **Note for Facilitators**: Lab 03 has **separate files** for each stack:
> - 🔷 **.NET**: [lab-03-generation-and-refactoring.md](labs/lab-03-generation-and-refactoring.md)
> - 🟩 **Spring Boot**: [lab-03-generation-and-refactoring-java.md](labs/lab-03-generation-and-refactoring-java.md)

**High-Level Steps** (participants follow their stack's lab file):

**Part A: REST API Generation**
1. Generate REST API endpoints for Task CRUD operations
2. Follow stack patterns:
   - 🔷 **.NET**: Minimal APIs with route groups, async/await, CancellationToken
   - 🟩 **Spring Boot**: @RestController with @GetMapping/@PostMapping, ResponseEntity
3. Generate OpenAPI/Swagger documentation

**Part B: Legacy Code Refactoring**
1. Refactor provided legacy code (LegacyTaskProcessor)
2. Apply Object Calisthenics rules:
   - One level of indentation
   - No else keyword
   - Wrap primitive types
   - First-class collections
3. **🟩 Spring Boot Bonus**: Mule ESB → Spring Boot refactoring example (see lab file)

**Key Point for Enterprise Java Teams**: Lab 3 includes Mule ESB modernization scenarios - legacy DataWeave transformations → Spring Boot services

---

## 4. Testing, Documentation, Workflow (2:30 – 2:45, 15 min)

**You do**:

- Show Copilot generating:
  - Unit tests using `/tests` command
    - 🔷 **.NET**: xUnit tests with FakeItEasy
    - 🟩 **Spring Boot**: JUnit 5 tests with Mockito
  - Documentation using `/doc` command (language-agnostic)
  - Commit message using Chat with staged changes context
  - PR summary with `@workspace` for full context

**Participants do (Lab 4)**:

> **Note for Facilitators**: Lab 04 is currently **.NET-focused** ([lab-04-testing-documentation-workflow.md](labs/lab-04-testing-documentation-workflow.md)). Spring Boot version is planned.\n> Core concepts (testing philosophy, documentation, workflow) apply to both stacks.

**High-Level Steps** (mostly stack-agnostic):

1. **Generate tests**: Select a method, use `/tests` command
   - 🔷 **.NET**: Generates xUnit + FakeItEasy tests
   - 🟩 **Spring Boot**: Generates JUnit 5 + Mockito tests (manual prompting if lab not available)
2. **Generate documentation**: Use `/doc` to create inline docs (XML docs / JavaDoc)
3. **Conventional commits**: Stage changes, ask Chat for commit message following Conventional Commits
4. **PR descriptions**: Use `@workspace` to draft PR summary with intent, scope, and risks
5. **README generation**: Ask for Getting Started section in README.md

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
  - Verify `.github/instructions/` directory contains instruction files with `applyTo:` frontmatter
  - Reload VS Code window: `F1` → "Developer: Reload Window"
  - Try Command Palette → "GitHub Copilot: Restart Language Server"

### .NET Build Issues

- **Wrong version**: Ensure .NET 9 SDK is installed (`dotnet --version`)
- **Missing dependencies**: Run `dotnet restore` in project directory
- **Path issues**: Use absolute paths or ensure correct working directory

### Spring Boot Build Issues

- **Wrong Java version**: Ensure Java 21 JDK is installed (`java -version`)
- **Missing dependencies**: Run `mvn clean install` or `./gradlew build` in `src-springboot/` directory
- **Port conflicts**: Application may fail to start if port 8080 is in use - check `application.properties`
- **DevContainer issues**: Ensure you selected the correct devcontainer (`.devcontainer/springboot/` or `.devcontainer/bilingual/`)
- **Maven wrapper**: Use `./mvnw` instead of `mvn` if Maven is not globally installed

### Copilot Generating Wrong Code

- **Check instructions**: Verify workshop instructions are properly configured
  - 🔷 **.NET**: Verify `csharp.instructions.md` and `dotnet.instructions.md` load for `.cs` files
  - 🟩 **Spring Boot**: Verify `springboot.instructions.md` loads for files in `src-springboot/`
- **Context matters**: Include relevant files in VS Code workspace
- **Prompt clarity**: Be specific about requirements and constraints
  - Include stack in prompt: "Generate a Spring Boot @RestController" vs "Generate a .NET Minimal API"
- **Restart Copilot**: Command Palette → "GitHub Copilot: Restart Language Server"
- **Wrong patterns**: If Copilot generates .NET code when you want Java (or vice versa):
  - Open a file in the correct stack's directory (`src-springboot/` for Java, `src/` for .NET)
  - Include stack-specific keywords in prompts (@Service, JUnit 5, Mockito vs sealed, xUnit, FakeItEasy)

---

## Deliverables Recap

- **Repo**: Clean Architecture solution with Domain/Application/Infrastructure/API layers in the `main` branch
- **Copilot Instructions**: `.github/instructions/` (context-aware files with `applyTo` patterns, automatically loaded)
- **Documentation**:
  - Main README with workshop outline
  - Facilitator's Guide (this document)
  - Detailed Lab Walkthroughs in `docs/labs/`
  - Starter Projects README with architecture details
- **Code Examples**: Console app, Web API with OpenTelemetry, legacy code for refactoring (LegacyTaskProcessor)
- **Test Infrastructure**: xUnit test stubs with FakeItEasy ready for participants

---

## Appendix: GitHub Copilot Chat Participants & Custom Agents

GitHub Copilot offers both built-in chat participants and custom Copilot agents that provide specialized assistance for various development tasks. Understanding the difference and when to use each helps participants get the most relevant and accurate responses.

### Built-in Chat Participants vs. Custom Copilot Agents

**Built-in Chat Participants** are VS Code's native Copilot features accessed with `@` syntax (e.g., `@workspace`, `@vscode`). These provide general-purpose assistance.

**Custom Copilot Agents** are specialized, configurable agents defined in `.agent.md` files in the `.github/agents/` directory. These provide role-specific guidance and can be selected from the agents dropdown in Copilot Chat.

---

### Built-in Chat Participants

These are accessed using `@` syntax in Copilot Chat.

#### 1. General Chat (Default)

**When to use**:

- General coding questions
- Conceptual explanations
- Architecture discussions
- Best practices inquiries

**How to use**:

- Simply type your question in chat without any prefix
- Or use `@copilot` explicitly

**Examples**:

```text
What is the repository pattern in DDD?
How should I structure a .NET Web API project?
Explain the benefits of async/await in C#
```

**Best for**:

- ✅ Learning concepts
- ✅ Getting design advice
- ✅ Understanding patterns
- ✅ General programming questions

**Limitations**:

- ⚠️ No direct access to your codebase context (use @workspace for that)
- ⚠️ May give generic answers without project-specific context

---

#### 2. @workspace

**When to use**:

- Questions about YOUR specific codebase
- Finding code across the project
- Understanding project structure
- Locating implementations or definitions

**How to use**:

- Type `@workspace` followed by your question
- Copilot will search and analyze your entire workspace

**Examples**:

```text
@workspace Where is the Task entity defined?
@workspace How is logging configured in this project?
@workspace Find all implementations of IRepository
@workspace Show me how authentication is handled
@workspace What testing frameworks are used?
```

**Best for**:

- ✅ Code navigation and discovery
- ✅ Understanding existing implementations
- ✅ Finding patterns used in your project
- ✅ Locating specific classes, methods, or files
- ✅ Understanding project conventions

**Workshop Tips**:

- Emphasize `@workspace` in Labs 2-4 when participants need to understand existing code
- Show how it finds code across all layers (Domain, Application, Infrastructure, API)
- Demonstrate finding repository interfaces, endpoint patterns, test structures

---

#### 3. @vscode

**When to use**:

- Questions about VS Code functionality
- Setting up extensions
- Configuring workspace settings
- Keyboard shortcuts
- Editor features

**How to use**:

- Type `@vscode` followed by your question

**Examples**:

```text
@vscode How do I change the editor theme?
@vscode What's the keyboard shortcut for formatting code?
@vscode How do I configure auto-save?
@vscode How do I debug a .NET application?
@vscode How do I set up a launch configuration?
```

**Best for**:

- ✅ VS Code configuration
- ✅ Editor productivity tips
- ✅ Extension recommendations
- ✅ Debugging setup
- ✅ Workspace customization

**Workshop Tips**:

- Use when participants struggle with VS Code features
- Helpful for debugging configuration questions
- Good for keyboard shortcut discovery

---

#### 4. @terminal

**When to use**:

- Shell command questions
- Terminal operations
- Command-line tool usage
- Script writing

**How to use**:

- Type `@terminal` followed by your question
- Ask about bash, zsh, PowerShell, or cmd commands

**Examples**:

```text
@terminal How do I run tests in watch mode?
@terminal What command finds all .cs files?
@terminal How do I check git commit history?
@terminal Create a command to list all NuGet packages
@terminal How do I build in release mode?
```

**Best for**:

- ✅ Shell commands
- ✅ Git operations
- ✅ .NET CLI commands
- ✅ Build scripts
- ✅ Terminal productivity

**Workshop Tips**:

- Useful for participants unfamiliar with .NET CLI
- Help with git commands during Labs 4
- Good for showing batch operations

---

#### 5. @azure (If Available)

**When to use**:

- Azure-specific questions
- Cloud deployment guidance
- Azure service configuration
- Azure CLI commands

**How to use**:

- Type `@azure` followed by your question

**Examples**:

```text
@azure How do I deploy a .NET Web API to Azure App Service?
@azure What's the best Azure service for hosting this application?
@azure How do I configure Application Insights?
@azure Generate an Azure Bicep template for this project
```

**Best for**:

- ✅ Azure deployment strategies
- ✅ Service recommendations
- ✅ Configuration guidance
- ✅ Azure CLI commands
- ✅ Infrastructure as Code

**Workshop Notes**:

- Not core to this workshop but useful for deployment discussions
- Mention in wrap-up as next steps
- Good for "how would I deploy this?" questions

---

### Custom Copilot Agents

Custom Copilot agents are specialized assistants defined in `.github/agents/*.agent.md` files. They are accessed by selecting them from the **agents dropdown** in Copilot Chat.

#### Available Custom Agents in This Workshop

1. **Check** - Code review and improvement suggestions
   - Reviews code for best practices, refactoring opportunities, and code smells
   - Use before submitting PRs
   - Located at `.github/agents/Check.agent.md`

2. **Architect** - Architecture and planning for documentation
   - Limited to Markdown files only
   - Creates detailed architectural plans and ADRs
   - Integrates with GitHub issue management
   - Located at `.github/agents/architect.agent.md`

3. **Plan** - Strategic planning and analysis
   - Read-only tools for research and planning
   - Develops comprehensive implementation strategies
   - Analyzes codebases before making changes
   - Located at `.github/agents/plan.agent.md`

4. **API Architect** - API design and implementation
   - Generates three-layer API designs (service, manager, resilience)
   - Creates fully implemented code
   - Located at `.github/agents/api-architect.agent.md`

5. **Expert .NET Software Engineer** - .NET best practices
   - Expert guidance on C#, SOLID, testing, performance
   - Located at `.github/agents/expert-dotnet-software-engineer.agent.md`

**How to use custom agents:**
1. Open Copilot Chat in VS Code
2. Click the **agents dropdown** at the top of the chat panel
3. Select the desired agent (e.g., Check, Plan, Architect)
4. Type your prompt - the agent's specialized instructions are automatically applied

---

### Comparison Table

| Type | Name | Access Method | Best Use Case | Example |
|------|------|---------------|---------------|---------|
| **Built-in** | Default | General chat | Concepts, patterns, best practices | "What is Clean Architecture?" |
| **Built-in** | @workspace | `@workspace` | Find code, understand structure | "@workspace Where is ITaskRepository?" |
| **Built-in** | @vscode | `@vscode` | Settings, shortcuts, configuration | "@vscode How do I format on save?" |
| **Built-in** | @terminal | `@terminal` | Shell commands, scripts | "@terminal Run tests in watch mode" |
| **Built-in** | @azure | `@azure` | Deployment, Azure services | "@azure Deploy to App Service" |
| **Custom Agent** | Check | Agents dropdown | Code review before PR | Select Check, then "Review this code" |
| **Custom Agent** | Plan | Agents dropdown | Strategic planning | Select Plan, then "Plan implementation for feature X" |
| **Custom Agent** | Architect | Agents dropdown | Architecture docs | Select Architect, then "Design ADR for decision" |

---

### Teaching Chat Participants & Agents in the Workshop

#### **Section 0.5 (Copilot Features Tour)**

**Demo Strategy** (5-7 minutes on participants and agents):

1. **Show Default Chat**:

   ```text
   What is the repository pattern?
   ```

   → Generic explanation

2. **Compare with @workspace**:

   ```text
   @workspace Where is the repository pattern implemented?
   ```

   → Specific file locations in YOUR project

3. **Show the Difference**:
   - Default: Generic, educational
   - @workspace: Specific, actionable

4. **Quick @terminal Demo**:

   ```text
   @terminal How do I run tests with detailed output?
   ```

   → Shows actual commands for their environment

#### **Throughout Labs - Usage Guide**

| Lab | Recommended Tools | When to Use |
|-----|------------------|-------------|
| **Lab 1** | Default, @workspace, Plan agent | Concepts, finding test patterns, planning tests |
| **Lab 2** | @workspace, Default, Architect agent | Finding entities, understanding layers, planning features |
| **Lab 3** | @workspace, @terminal, Check agent | Finding endpoints, refactoring commands, code review |
| **Lab 4** | @workspace, @terminal, Check agent | Git commands, understanding test structure, PR review |

#### **Common Participant Mistakes**

1. **Using Default Instead of @workspace**:
   - **Mistake**: "Where is the Task entity?"
   - **Better**: "@workspace Where is the Task entity?"
   - **Result**: Specific file path vs. generic explanation

2. **Using @workspace for Concepts**:
   - **Mistake**: "@workspace What is dependency injection?"
   - **Better**: "What is dependency injection?" (default)
   - **Result**: Educational explanation vs. code search

3. **Not Using @terminal for Commands**:
   - **Mistake**: "How do I run tests?"
   - **Better**: "@terminal How do I run tests?"
   - **Result**: Generic vs. environment-specific commands

4. **Not Using Custom Agents for Specialized Tasks**:
   - **Mistake**: Asking default chat to review code
   - **Better**: Select **Check** agent from dropdown for code review
   - **Result**: Generic feedback vs. structured review following project standards

5. **Not Using Plan Agent Before Big Changes**:
   - **Mistake**: Diving into implementation without planning
   - **Better**: Select **Plan** agent to develop strategy first
   - **Result**: Ad-hoc changes vs. comprehensive, thought-out approach

---

### Advanced Tips for Facilitators

#### **Combining Chat Participants and Custom Agents**

You can use chat participants and custom agents in sequence:

1. **Understand concept** (Default):

   ```text
   Explain the CQRS pattern
   ```

2. **Find in code** (@workspace):

   ```text
   @workspace Show me examples of commands and queries in this project
   ```

3. **Review code** (Check agent):

   Select **Check** agent from dropdown:
   ```text
   Review the command handlers for best practices
   ```

4. **Run tests** (@terminal):

   ```text
   @terminal Run tests for the command handlers
   ```

#### **Context Variables with Participants and Agents**

Combine chat participants and agents with context variables for precision:

```text
@workspace What tests exist for #file:CreateTaskCommandHandler.cs?
```

```text
@terminal How do I run tests in #file:CreateTaskCommandHandlerTests.cs?
```

With custom agents:
```text
[Select Check agent] Review #file:TaskService.cs for improvements
```

#### **When Participants and Agents Don't Help**

Sometimes participants need to:

- **Read documentation**: Point them to official docs
- **Debug interactively**: Use VS Code debugger
- **Review logs**: Look at actual error messages
- **Ask human experts**: Some questions need human judgment

---

### Selection Flowchart for Participants

```text
START: I have a task
    ↓
Do I need specialized assistance?
    ↓ YES → Select custom agent from dropdown
    │       ├─ Code review? → Check
    │       ├─ Planning? → Plan
    │       ├─ Architecture docs? → Architect
    │       └─ API design? → API Architect
    ↓ NO
    ↓
Is it about MY code?
    ↓ YES → Use @workspace
    ↓ NO
    ↓
Is it about VS Code?
    ↓ YES → Use @vscode
    ↓ NO
    ↓
Is it about terminal/commands?
    ↓ YES → Use @terminal
    ↓ NO
    ↓
Is it about Azure/deployment?
    ↓ YES → Use @azure
    ↓ NO
    ↓
Use Default Chat
```

---

### Troubleshooting

#### **@workspace Not Finding Code**

**Symptoms**: Says "I couldn't find..." for code that exists

**Solutions**:

1. Ensure all relevant files are in workspace (not excluded)
2. Wait for indexing to complete (check bottom status bar)
3. Reload window: `Cmd/Ctrl+Shift+P` → "Reload Window"
4. Try being more specific: include file names or paths

#### **Custom Agents Not Appearing**

**Symptoms**: Custom agents not showing in dropdown

**Solutions**:

1. Ensure `.agent.md` files are in `.github/agents/` directory
2. Files must have proper `chatagent` frontmatter
3. Reload VS Code window: `Cmd/Ctrl+Shift+P` → "Reload Window"
4. Check VS Code is version 1.106 or later

#### **Chat Participants Not Available**

**Symptoms**: @azure or other participants don't work

**Solutions**:

1. Check Copilot extension version (update if needed)
2. Verify subscription includes advanced features
3. Some modes require specific extensions installed
4. Try restarting VS Code

#### **Wrong Mode Selected**

**Symptoms**: Generic answers when you wanted specific ones

**Solutions**:

1. Check which mode you're using (shown in chat)
2. Rephrase with explicit mode: "@workspace [question]"
3. Clear chat and start over with correct mode

---

### Practice Exercise for Participants

**5-Minute Hands-On** (during Section 0.5):

Ask participants to try each mode:

1. **Default Chat**:

   ```text
   What is Test-Driven Development?
   ```

   *Expected*: Conceptual explanation of TDD

2. **@workspace**:

   ```text
   @workspace Where are the xUnit tests located?
   ```

   *Expected*: Actual file paths in their project

3. **@terminal**:

   ```text
   @terminal Show me the command to build and test
   ```

   *Expected*: `dotnet build && dotnet test` or similar

4. **@vscode**:

   ```text
   @vscode How do I toggle the terminal?
   ```

   *Expected*: Keyboard shortcut (Ctrl+\` or Cmd+\`)

**Debrief**:

- Which mode gave the most useful answer for each question?
- When would you use each mode during the labs?

---

### Key Takeaways for Facilitators

✅ **@workspace is most important** for this workshop - emphasize it!  
✅ **Default chat** is good for learning concepts  
✅ **@terminal** helps with .NET CLI commands  
✅ **Modes are contextual** - teach when to use which  
✅ **Practice makes perfect** - participants learn by using them  

**Don't Overwhelm**:

- Focus on @workspace and default chat primarily
- Introduce other modes as needed during labs
- Reference this appendix for detailed explanations
- Let participants discover advanced usage naturally

---

### Additional Resources

- [GitHub Copilot Chat Documentation](https://docs.github.com/en/copilot/using-github-copilot/asking-github-copilot-questions-in-your-ide)
- [Using Chat Participants](https://docs.github.com/en/copilot/using-github-copilot/getting-started-with-chat-participants-in-github-copilot)
- [Copilot Context Variables](https://code.visualstudio.com/docs/copilot/copilot-chat#_chat-context)
