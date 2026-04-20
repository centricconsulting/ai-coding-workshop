---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

<!-- _class: lead -->

# Module 3

## Code Generation & Refactoring
### Scaffolding and Modernization

**Duration:** 45 minutes

---

# Two Workflows

## 1. Generate from Scratch
- Complete CRUD endpoints
- Following Clean Architecture
- With query handlers (CQRS)

## 2. Refactor Legacy Code
- Modernize old patterns
- Apply Object Calisthenics
- Improve testability

---

# Lab 03: Part A - Generation

**Generate:**
- `GET /tasks/{id}` - Retrieve single task
- `PUT /tasks/{id}` - Update task
- `DELETE /tasks/{id}` - Delete task

**Using:**
- `@workspace` for context
- Minimal API patterns
- CQRS queries
- Integration tests

**Time:** 20 minutes

---

# CQRS Pattern

**Command-Query Responsibility Segregation**

## Commands
- Change state
- Example: `CreateTaskCommand`, `UpdateTaskCommand`

## Queries
- Read state
- Example: `GetTaskByIdQuery`, `GetAllTasksQuery`

**Benefit:** Clear separation of read/write concerns

---

# Lab 03: Part B - Refactoring

**Legacy code:**
- 🔷 .NET: `LegacyTaskProcessor.cs`
- 🟩 Java: `LegacyTaskProcessor.java`

**Problems:**
- Nested if statements
- Abbreviations
- No guard clauses
- Poor testability

**Your task:** Use `/refactor` to modernize

**Principles:** Object Calisthenics

---

# Object Calisthenics (Light)

**Key rules:**
- One level of indentation per method
- Don't use `else` - use guard clauses
- No abbreviations
- Wrap primitives in meaningful types
- Small methods with clear names

**Example:**
```csharp
// ❌ Before
if (task != null) {
    if (task.Status == "Active") {
        // logic
    }
}

// ✅ After
if (task == null) return;
if (task.Status != "Active") return;
// logic
```

---

# Using @workspace

**Why it matters:**
- Copilot sees your entire codebase
- Suggests patterns already in use
- Maintains consistency

**Example prompts:**
- `@workspace Show me all domain entities`
- `@workspace How are minimal APIs structured in this project?`
- `@workspace Generate GET endpoint following existing pattern`

---

<!-- _class: lead -->

# Hands-On Time

**Lab Guide:**
- 🔷 [Lab 03: Generation & Refactoring (.NET)](../../labs/lab-03-generation-and-refactoring.md)
- 🟩 [Lab 03: Generation & Refactoring (Java)](../../labs/lab-03-generation-and-refactoring-java.md)

**Next Module:** [Testing & Documentation](04-testing-documentation.md)

**Previous Module:** [Requirements to Code](03-requirements-to-code.md)
