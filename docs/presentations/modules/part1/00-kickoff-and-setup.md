---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

# GitHub Copilot for .NET Development

## AI-Assisted Development Fundamentals

**Duration:** 3 Hours  
**Format:** Instructor-led, hands-on  
**Part:** 1 of 2

---

# Welcome to AI-Assisted Development

## What You'll Learn Today

- Use **Copilot Instructions** for team-wide consistency
- Apply **Test-Driven Development** with AI assistance
- Transform **requirements into working code**
- Generate and **refactor APIs** following Clean Architecture
- Create **comprehensive tests and documentation**
- Follow **conventional commits** and AI-generated PR descriptions

---

## The only constant is change

- Copilot is available in multiple IDEs, GitHub.com, CLI, and mobile — features vary by tool
- What you see in Visual Studio 2022 or 2026 may differ from VS Code or other tools
  - See [Copilot feature matrix](https://docs.github.com/en/copilot/reference/copilot-feature-matrix) for version details
- AI-assisted software engineering tools and practices are evolving quickly — expect changes and differences
- Training content reflects current features; some may vary by tool
- Embrace a growth mindset and have fun!

---

# Why This Workshop Matters

**Traditional Development:**
- Manual test writing
- Repetitive CRUD code
- Documentation debt
- Inconsistent patterns across team

**With GitHub Copilot:**
- AI generates tests from specifications
- Accelerated implementation
- Documentation alongside code
- Encoded team standards via Copilot Instructions

---

# Prerequisites Check

✅ **GitHub Copilot** - Active subscription  
✅ **.NET 9 SDK** - `dotnet --version`  
✅ **VS Code** - With C# Dev Kit extension  
✅ **Git** - Basic familiarity  
✅ **C# Experience** - Comfortable with syntax

**Environment Check:**
```bash
dotnet --version    # Should show 9.x.x
git --version
code --version
```

---

# Repository Structure

```
TaskManager.sln
├── src/
│   ├── TaskManager.Domain/        # Business logic
│   ├── TaskManager.Application/   # Use cases
│   ├── TaskManager.Infrastructure/ # Data access
│   └── TaskManager.Api/           # Minimal API
└── tests/
    ├── TaskManager.UnitTests/
    └── TaskManager.IntegrationTests/
```

**Architecture:** Clean Architecture + DDD  
**Testing:** xUnit + FakeItEasy  
**API:** .NET 9 Minimal APIs

---

# Today's Journey

```
0. Kickoff & Setup (15 min)
0.5. Copilot Features Tour (15 min)
1. Copilot Instructions & TDD (30 min)
2. Requirements → Code (45 min)
3. Code Generation & Refactoring (45 min)
4. Testing & Documentation (15 min)
5. Wrap-Up & Discussion (15 min)
```

**Total:** 3 hours with hands-on labs

---

<!-- _class: lead -->

# Module 0: Setup & Environment

## Getting Ready

**Duration:** 15 minutes

---

# Clone and Branch

```bash
git clone https://github.com/centricconsulting/ai-coding-workshop.git
cd ai-coding-workshop

# Create your personal branch
git checkout main
git pull
git checkout -b your-name-workshop
```

**Important:** Work on your own branch to avoid conflicts

---

# Verify Environment

```bash
# Check .NET
dotnet --version

# Build solution
dotnet build

# Run tests
dotnet test
```

**Expected:** All should succeed

---

# Copilot Instructions Preview

This repository includes **`.github/instructions/`** with context-aware instruction files

**What it does:**
- Automatically applied to all Copilot interactions
- Encodes Clean Architecture rules
- Enforces DDD patterns
- Specifies .NET 9 conventions

**No setup needed** - It just works!

---

<!-- _class: lead -->

# Ready to Begin

**Next Module:** [Copilot Features Tour](01-copilot-features-tour.md)

Use the **Marp preview** to navigate between modules
