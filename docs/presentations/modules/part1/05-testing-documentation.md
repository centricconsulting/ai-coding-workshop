---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

<!-- _class: lead -->

# Module 5

## Testing, Documentation & Workflow
### Completing the Lifecycle

**Duration:** 15 minutes

---

# Lab 04 Overview

**Complete the development lifecycle:**

1. Generate comprehensive tests (`/tests`)
2. Add XML documentation (`/doc`)
3. Update API documentation
4. Write conventional commit messages
5. Generate PR description

**Time:** 15 minutes  
**Goal:** AI-assisted workflow end-to-end

---

# Generate Tests with /tests

```
# In Copilot Chat
/tests for CreateTaskCommand
```

**Copilot generates:**
- Happy path tests
- Edge cases
- Boundary conditions
- Error scenarios

**Your job:** Review, adjust, run

---

# Documentation with /doc

```
# In Copilot Chat
/doc for ITaskService
```

**Generates:**
- XML documentation comments
- Parameter descriptions
- Return value docs
- Example usage

**Result:** IntelliSense-ready documentation

---

# Conventional Commits

**Format:** `<type>(<scope>): <description>`

**Types:**
- `feat` - New feature
- `fix` - Bug fix
- `docs` - Documentation
- `test` - Tests
- `refactor` - Code restructuring
- `chore` - Maintenance

**Example:** `feat(domain): add Priority value object`

---

# PR Description with @workspace

```
@workspace Generate a PR description for my changes
```

**Copilot provides:**
- Summary of changes
- Files modified
- Testing notes
- Breaking changes (if any)

**Then:** Review and adjust before creating PR

---

<!-- _class: lead -->

# Hands-On Time

**Lab Guide:** [Lab 04: Testing & Documentation](../../../labs/lab-04-testing-documentation-workflow.md)

**Next Module:** [Wrap-Up & Discussion](06-wrapup-discussion.md)

**Previous Module:** [Generation & Refactoring](04-generation-refactoring.md)
