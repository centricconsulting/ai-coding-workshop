# Workshop: Using AI for Application Development with GitHub Copilot (.NET Edition)

This repository contains materials for a 3-hour workshop. Participants will use **Visual Studio Code**, **.NET 8**, and **GitHub Copilot** to experience how AI can support application development.

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

- **Configure GitHub Copilot** with custom instructions for consistent, context-aware code generation
- **Transform requirements** into backlog items, acceptance criteria, and working code using AI assistance
- **Generate and refactor .NET code** following Clean Architecture and DDD principles
- **Create comprehensive tests** and documentation with AI support
- **Apply conventional commits** and generate professional PR descriptions
- **Identify anti-patterns** and best practices when working with AI coding assistants

## Schedule

### 0. Kickoff & Setup (15 min)
- Goals and environment check
- Clone starter repo, enable Copilot, configure workshop instructions

### 1. Controlling Context with Copilot Instructions (30 min)
- Understand Copilot Instructions
- Lab 1: Configure instructions and generate a sample service class

### 2. Requirements → Backlog → Code (45 min)
- Turn requirements into backlog items, tests, and code
- Lab 2: Backlog items → acceptance criteria → TaskService.AddTask

### 3. Code Generation & Refactoring (45 min)
- Scaffold APIs, refactor legacy methods
- Lab 3: Minimal API, refactor spaghetti code, add logging

### 4. Testing, Documentation, Workflow (30 min)
- Generate tests, docs, commit/PR messages
- Lab 4: Unit tests, commit message, PR description, README section

### 5. Wrap-Up & Discussion (15 min)
- Lessons learned
- Anti-patterns to avoid
- Next steps and Q&A

---

## Workshop Materials

### Core Files
- **[Workshop Instructions](WORKSHOP-COPILOT-INSTRUCTIONS.md)**: Comprehensive Copilot configuration for .NET development
- **[Facilitator's Guide](docs/FACILITATOR_GUIDE.md)**: Detailed timing and talking points for instructors

### Starter Projects (Coming Soon)
- **Console Application**: Basic .NET 8 console app for initial exercises
- **Web API Skeleton**: Minimal API structure ready for enhancement  
- **Legacy Code Sample**: "Spaghetti code" for refactoring exercises

### Reference Materials
- **Prompt Library**: Collection of proven prompts for common development tasks
- **Conventional Commits Guide**: Template and examples for consistent commit messages
- **Clean Architecture Cheat Sheet**: Quick reference for project structure and dependencies

## Getting Started

1. **Clone this repository**:
   ```bash
   git clone https://github.com/centricconsulting/ai-coding-workshop.git
   cd ai-coding-workshop
   ```

2. **Copy the workshop instructions**:
   - Open `WORKSHOP-COPILOT-INSTRUCTIONS.md`
   - Copy the entire contents
   - In VS Code: Settings → GitHub Copilot → Instructions
   - Paste the instructions

3. **Verify your setup** using the environment check above

4. **Ready to start!** Follow along with your facilitator or work through the labs independently
