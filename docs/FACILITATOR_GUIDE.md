# This document provides a detailed facilitator's guide for running the 3-hour workshop.

## Timing and Flow Tips

- **Keep labs hands-on**: Minimize lecture time, maximize coding time
- **Walking around**: Circulate during labs to help with issues
- **Backup plans**: Have pre-built examples ready if participants struggle
- **Flex time**: Each section has 5-10 minutes of flex time built in
- **Energy management**: Take 5-minute breaks between major sections
- **Advanced participants**: Have optional extension exercises ready

---ilitator's Guide: Using AI for Application Development with GitHub Copilot (.NET Edition)

This document provides a detailed facilitator’s guide for running the 3-hour workshop.

---

## 0. Kickoff & Setup (0:00 – 0:15, 15 min)
**You do**:
- Welcome participants, introduce goals: *“We’ll learn how to use AI (Copilot) to help with requirements, code, tests, docs, and workflow in .NET projects.”*
- Explain **Copilot Instructions** concept.
- Quick demo: show your prepared `WORKSHOP-COPILOT-INSTRUCTIONS.md`.

**Participants do**:
- Confirm environment:
  - VS Code open
  - GitHub Copilot enabled
  - .NET 8 SDK installed
  - Clone starter repo (console app + Web API skeleton + messy legacy code)
- Paste **Workshop Copilot Instructions** into VS Code → Settings → Copilot Instructions.

---

## 1. Controlling Context with Copilot Instructions (0:15 – 0:45, 30 min)
**You do**:
- Explain why *context matters* for Copilot output.
- Show difference with/without instructions (e.g., generate a class, note coding style vs messy defaults).
- Highlight key instructions:
  - Coding style (file-scoped namespaces, `nameof`)
  - Clean Architecture project layout
  - DDD aggregates
  - Test rules (xUnit + FakeItEasy)
  - Conventional commits

**Participants do (Lab 1)**:
1. Configure Copilot Instructions with:
   - Use async/await
   - ILogger for logging
   - xUnit for tests
2. Ask Copilot: *“Generate a C# service class that logs with ILogger and uses async methods.”*
3. Observe: Code respects style and rules.

---

## 2. Requirements → Backlog → Code (0:45 – 1:30, 45 min)
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

## 3. Code Generation & Refactoring in .NET (1:30 – 2:15, 45 min)
**You do**:
- Show Copilot scaffolding: create a `TasksController` with minimal API.
- Show refactor of messy method (provided in repo): 
  - Before: long function, nested ifs, poor naming
  - After: Copilot helps split into smaller methods, add async, ILogger logging.

**Participants do (Lab 3)**:
1. Scaffold minimal Web API with endpoints:
   - `GET /tasks/{id}`
   - `POST /tasks`
2. Refactor provided “spaghetti” method using Copilot:
   - Enforce guard clauses (no `else`)
   - Add async
   - Add logging
3. Re-run `dotnet build && dotnet test`.

---

## 4. Testing, Documentation, Workflow (2:15 – 2:45, 30 min)
**You do**:
- Show Copilot generating:
  - xUnit tests (happy path + invalid input)
  - README docs for an API
  - Commit message + PR summary (Conventional Commit format)

**Participants do (Lab 4)**:
1. Generate 2 unit tests for their `TaskService`.
2. Stage changes → ask Copilot: *“Write a Conventional Commit message for this change.”*
3. Ask Copilot: *“Draft a PR description including intent, scope, risks, next steps.”*
4. Generate a README section explaining their API.

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
- **Instructions not applied**: Restart VS Code after pasting instructions

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
- **Repo**: Starter projects (console app, Web API, messy legacy code).
- **Instructions file**: `WORKSHOP-COPILOT-INSTRUCTIONS.md`.
- **README**: Workshop outline and labs.
- **Reference sheet**: Key prompts, Conventional Commits guide.
