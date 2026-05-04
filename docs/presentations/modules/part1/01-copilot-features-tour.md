---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

<!-- _class: lead -->

# Module 0.5

## GitHub Copilot Features Tour
### Capabilities Overview

**Duration:** 15 minutes

---

<style scoped>
section { font-size: 0.95em; }
h1, h2 { font-size: 1.8em; }
</style>

# Three Ways to Interact

## 1. Inline Completions
- Active while typing
- Suggests next lines
- **Trigger:** Start typing or use `Alt+\` (Windows/Linux) or `Option+\` (Mac)

## 2. Chat Panel
- Ask questions, explain code
- Get suggestions without editing
- **Trigger:** Click Copilot icon in sidebar

## 3. Inline Chat
- Edit code in-place
- **Trigger:** `Ctrl+I` (Windows/Linux) or `Cmd+I` (Mac)

---

# Slash Commands

Quick shortcuts for common tasks:

| Command | Purpose | Example |
|---------|---------|---------|
| `/explain` | Explain code | `/explain this function` |
| `/fix` | Suggest fixes | `/fix this error` |
| `/tests` | Generate tests | `/tests for this class` |
| `/doc` | Add documentation | `/doc this API` |
| `/refactor` | Improve code | `/refactor to use guard clauses` |

---

# More Slash Commands

| Command | Purpose | Example |
|---------|---------|---------|
| `/help` | Show all commands | `/help` |
| `/agents` | List custom agents | `/agents` |
| `/skills` | List skills | `/skills` |
| `/init` | Start new project | `/init dotnet webapi` |
| `/create-file` | Create with AI | `/create-file readme.md` |

**Try it now:** Type `/help` in Copilot Chat

---
<style scoped>
section { font-size: 0.95em; }
h1, h2 { font-size: 1.8em; }
</style>

# Chat Participants

Provide context for better results:

## @workspace
- Questions about your codebase
- "Where is the Task entity?"

## @vscode
- VS Code features and commands
- "How do I debug tests?"

## @terminal
- CLI commands and troubleshooting
- "How to restore packages?"

---
<style scoped>
section { font-size: 0.95em; }
h1, h2 { font-size: 1.8em; }
</style>

# Context Variables

Reference specific context:

## #file
- Reference a specific file
- Example: `Explain #file:Task.cs`

## #selection
- Current editor selection
- Example: `Refactor #selection`

## #editor
- Current active file
- Example: `Add tests for #editor`


---

# Quick Practice (5 min)
<style scoped>
section { font-size: 0.95em; }
h1, h2 { font-size: 1.8em; }
</style>

**🔷 .NET Track — Try these:**

1. Open `TaskManager.Domain/Tasks/Task.cs`
2. Use `/explain` on the Task class
3. Try `@workspace` with: "What's the architecture pattern?"
4. Use `#file` with: "Suggest improvements for #file:Task.cs"

**🟩 Spring Boot Track — Try these:**

1. Open `src-springboot/taskmanager-domain/src/main/java/com/example/taskmanager/domain/tasks/Task.java`
2. Use `/explain` on the Task class
3. Try `@workspace` with: "What's the architecture pattern?"
4. Use `#file` with: "Suggest improvements for #file:Task.java"

**Goal:** Get comfortable with Copilot's features

---

<!-- _class: lead -->

# Ready for TDD

**Next Module:** [Copilot Instructions & TDD](01-copilot-instructions-tdd.md)

**Previous Module:** [Kickoff & Setup](00-kickoff-and-setup.md)
