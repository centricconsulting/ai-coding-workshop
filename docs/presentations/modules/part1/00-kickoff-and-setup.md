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

<!-- _class: small -->
<style scoped>
section { font-size: 0.95em; }
h1 { font-size: 1.8em; }
</style>

# Prerequisites Check

✅ **GitHub Copilot** - Active subscription  
✅ **VS Code** - Latest stable version  
✅ **Git** - Basic familiarity  

**🔷 .NET Track:**  
✅ **.NET 9 SDK** - `dotnet --version`  
✅ **C# Experience** - Comfortable with syntax

**🟩 Spring Boot Track:**  
✅ **Java 21** - `java -version`  
✅ **Maven 3.9+** - `mvn --version`  
✅ **Java/Spring Experience** - Comfortable with syntax

**Environment Check:**
```bash
# Common
git --version
code --version

# .NET Track
dotnet --version    # Should show 9.x.x

# Spring Boot Track
java -version       # Should show 21.x.x
mvn --version       # Should show 3.9+
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

# Repository Structure (Spring Boot)

```
src-springboot/
├── taskmanager-domain/            # Business logic & entities
├── taskmanager-application/       # Use cases & services
├── taskmanager-infrastructure/    # Data access & persistence
└── taskmanager-api/               # Spring Boot REST API
    └── src/main/java/.../api/
        ├── controllers/           # REST controllers
        ├── dto/                   # Request/Response models
        └── config/                # App configuration
```

**Architecture:** Clean Architecture + DDD  
**Testing:** JUnit 5 + Mockito  
**API:** Spring Boot 3 + Java 21

---

# Today's Journey

```
0. Kickoff & Setup (15 min)
1. Copilot Features Tour (15 min)
2. Copilot Instructions & TDD (30 min)
3. Requirements → Code (45 min)
4. Code Generation & Refactoring (45 min)
5. Testing & Documentation (15 min)
6. Wrap-Up & Discussion (15 min)
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

**🔷 .NET Track:**
```bash
dotnet --version
dotnet build
dotnet test
```

**🟩 Spring Boot Track:**
```bash
java -version
cd src-springboot && mvn clean install
mvn test -f src-springboot/pom.xml
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
