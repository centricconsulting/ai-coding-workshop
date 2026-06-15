---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

<!-- _class: lead -->

# Module 2

## Copilot Instructions & TDD
### Red-Green-Refactor with AI

**Duration:** 30 minutes

---

# What Are Copilot Instructions?

**🔷 .NET:** `.github/instructions/dotnet.instructions.md` (auto-loads for C# files)  
**🟩 Spring Boot:** `.github/instructions/springboot.instructions.md` (auto-loads for `src-springboot/**`)

**Purpose:**
- Repository-wide AI behavior
- Always active (no manual activation)
- Team standards enforcement
- Consistent across all team members

**Think of it as:** A senior developer reviewing every suggestion

---

# Our Copilot Instructions

Key rules encoded:

| Rule | 🔷 .NET | 🟩 Spring Boot |
|------|---------|----------------|
| **TDD first** | ✅ Tests before code | ✅ Tests before code |
| **Clean Architecture** | ✅ Domain has no deps | ✅ Domain has no deps |
| **DDD patterns** | ✅ Aggregates, VOs | ✅ Aggregates, VOs |
| **Stack** | ✅ .NET 9, Minimal APIs | ✅ Spring Boot 3, Java 21 |
| **Testing** | ✅ xUnit + FakeItEasy | ✅ JUnit 5 + Mockito |
| **Naming** | ✅ PascalCase / camelCase | ✅ camelCase / PascalCase |

---

# Test-Driven Development (TDD)

```
🔴 RED → ✅ GREEN → ♻️ REFACTOR
```

## Red Phase
Write a **failing test** that defines desired behavior

## Green Phase
Write **minimal code** to make the test pass

## Refactor Phase
**Improve code quality** while keeping tests green

---

# Why TDD with Copilot?

**Traditional concern:** "AI writes code without tests"

**Our approach:**
- Copilot Instructions **enforce tests first**
- AI generates test cases from requirements
- Tests are easier to review than implementations
- Validates AI-generated code immediately

**Result:** Higher quality, verified code

---

# Lab 01 Overview

**🔷 .NET Track — Build:** `NotificationService` with TDD

1. Define `INotificationService` interface (RED)
2. Generate comprehensive test suite (RED)
3. Implement `NotificationService` (GREEN)
4. Refactor for quality (REFACTOR)

**🟩 Spring Boot Track — Build:** `NotificationService` with TDD

1. Define `NotificationService` interface (RED)
2. Generate comprehensive JUnit 5 test suite (RED)
3. Implement `NotificationServiceImpl` (GREEN)
4. Refactor for quality (REFACTOR)

**Time:** 25 minutes  
**Key learning:** AI accelerates TDD, doesn't bypass it

---

# Demo: TDD Workflow

**Watch for:**
- How Copilot suggests test scenarios
- Interface-first design
- Test structure (Arrange-Act-Assert)
- Implementation simplicity
- Refactoring suggestions

**Then:** You'll do it hands-on in Lab 01

---

<!-- _class: lead -->

# Hands-On Time

**Lab Guide:** [Lab 01: TDD with Copilot](../../../labs/lab-01-tdd-with-copilot.md)

**Next Module:** [Requirements to Code](02-requirements-to-code.md)

**Previous Module:** [Copilot Features Tour](01-copilot-features-tour.md)
