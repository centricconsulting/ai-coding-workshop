# Lab 4: Testing, Documentation & Workflow with GitHub Copilot (JavaScript)

> **💡 Also available**: [shared .NET version](lab-04-testing-documentation-workflow.md) · [Java/Spring Boot version](lab-04-testing-documentation-workflow-java.md) · [Angular version](lab-04-testing-documentation-workflow-angular.md) · [Kotlin version](lab-04-testing-documentation-workflow-kotlin.md) · [Swift version](lab-04-testing-documentation-workflow-swift.md)

**Duration**: 15-20 minutes  
**Learning Objectives**:

- Use Copilot to expand plain JavaScript test coverage
- Add helpful plain-language comments without over-documenting
- Update a small README accurately
- Write simple Conventional Commit messages
- Draft a clear pull request description for a lightweight change set

---

## Overview

This lab focuses on the finishing work that often gets skipped when people are in a hurry:

1. **Testing** - add coverage around the code you changed
2. **Documentation** - make the code and README easier for the next person to understand
3. **Workflow** - describe your changes clearly in commits and pull requests

For this track, the workflow stays lightweight:

- code lives in `src-javascript/task-manager/task.js`
- tests live in `src-javascript/task-manager/task.test.js`
- the track README lives in `src-javascript/task-manager/README.md`
- validation runs with `node --test`

---

## Prerequisites

- ✅ Labs 1-3 are complete, or you have made equivalent JavaScript-track changes
- ✅ `task.js`, `task.test.js`, and `README.md` exist in `src-javascript/task-manager/`
- ✅ Baseline verification completed:

```bash
cd src-javascript/task-manager
node --test
```

---

## Part 1: Expand Test Coverage with Copilot (5-6 minutes)

### Scenario: Cover the Rules You Added

By now your task manager probably has more logic than the original three happy-path tests covered.

This is a good moment to let Copilot help you find missing cases.

### 1.1 Use `/tests` or a Direct Chat Prompt

Open `src-javascript/task-manager/task.js`, select one function, and try:

```text
/tests
```

If you prefer regular chat, use a prompt like:

```text
Add node:test coverage for src-javascript/task-manager/task.js.
Focus on validation errors, copy-returning behavior, and allowed status or priority rules.
Use node:assert/strict.
```

### 1.2 Good Test Ideas for This Track

Strong additions usually cover things like:

- blank titles are rejected
- missing descriptions are rejected if you added that rule
- invalid priorities are rejected
- invalid status changes are rejected
- `updateStatus` returns a new object instead of changing the original
- `updateDetails` returns a new object instead of changing the original

### 1.3 Run the Tests

```bash
cd src-javascript/task-manager
node --test
```

If the tests pass, your code has a stronger safety net.

---

## Part 2: Add Plain-Language Comments and README Help (4-5 minutes)

### Scenario: Make the Code Easier to Read for the Next Person

For this track, good documentation does **not** mean formal JSDoc everywhere. It means short, useful explanations in plain language.

### 2.1 Ask Copilot for Helpful Comments

Try this prompt:

```text
Review src-javascript/task-manager/task.js and suggest a few short plain-language comments that explain the non-obvious business rules.
Do not add noisy comments that only repeat the code.
```

Good comment locations might include:

- why tasks are kept as plain objects in this track
- why certain status changes are rejected
- why helper functions exist after refactoring

### 2.2 Keep Comments Human and Short

A useful comment might look like:

```js
// Keep this list small and explicit so invalid status values fail fast.
const TASK_STATUSES = ['Todo', 'InProgress', 'Done', 'Cancelled'];
```

Avoid comments like this:

```js
// This function updates the task.
function updateDetails(...) {
```

That kind of comment does not add any real help.

### 2.3 Update the Track README

Open `src-javascript/task-manager/README.md` and ask Copilot:

```text
Suggest a small README update for src-javascript/task-manager/README.md.
Explain what the task manager demonstrates, how to run the tests with node --test, and which file contains the main task functions.
Keep it concise and accurate.
```

This is especially useful after Labs 1-3 changed the behavior beyond the original scaffold.

---

## Part 3: Write Conventional Commit Messages (3-4 minutes)

### 3.1 Stage the Relevant Files

A typical JavaScript-track change set might include:

```bash
git add src-javascript/task-manager/task.js
git add src-javascript/task-manager/task.test.js
git add src-javascript/task-manager/README.md
```

### 3.2 Ask Copilot for a Commit Message

Try:

```text
Write a Conventional Commit message for the plain JavaScript workshop track.
The change added task validation, priority support, and more node:test coverage.
Include a short subject and a helpful body.
```

A good result might look like this:

```text
feat(javascript): add task validation and priority rules

- validate task titles and priority values
- keep task logic in one plain JavaScript file
- expand node:test coverage for happy-path and error cases
- update the task-manager README for workshop participants
```

### 3.3 Ask for a Refactor Commit Too

If Lab 3 created a separate refactor commit, try:

```text
Write a Conventional Commit message for a plain JavaScript refactor that split one large function into smaller helper functions without changing behavior.
```

Example result:

```text
refactor(javascript): split task update logic into helpers

- extract validation helpers for clearer intent
- separate validation from update behavior
- preserve existing behavior with node:test coverage
```

---

## Part 4: Draft a Pull Request Description (4-5 minutes)

### 4.1 Use `@workspace` for Context

Prompt Copilot Chat:

```text
@workspace Draft a pull request description for the plain JavaScript task-manager track.
Include:
- summary of behavior changes
- testing performed
- documentation updates
- a short reviewer checklist
Use Markdown.
```

### 4.2 What a Good PR Description Sounds Like

````md
## Summary

This PR expands the plain JavaScript task-manager workshop track by adding validation,
priority support, and a small refactor that keeps the code readable without introducing
extra layers or tooling.

## Testing Performed

```bash
cd src-javascript/task-manager
node --test
```

## Reviewer Checklist

- [ ] task behavior still lives in one JavaScript file
- [ ] tests cover both success and error paths
- [ ] comments explain why, not just what
- [ ] README matches the actual commands and files
````

Review the output before using it. Copilot is a drafting partner, not the final approver.

---

## Key Learning Points

### ✅ Testing Workflow

1. Copilot can generate a strong first draft of tests
2. you still need to review the edge cases and assertions
3. `node --test` gives you a fast, dependency-free way to validate changes

### ✅ Documentation Workflow

1. comments should explain the reason behind a rule
2. README updates should stay short and accurate
3. plain-language documentation fits this track better than formal JSDoc everywhere

### ✅ Collaboration Workflow

1. Conventional Commits make change history easier to scan
2. a short PR summary helps reviewers understand the goal quickly
3. a reviewer checklist reduces back-and-forth

---

## Extension Exercises (If Time Permits)

### Exercise 1: Ask Copilot for Missing Edge Cases

Prompt Copilot to look for one validation case your tests still do not cover.

### Exercise 2: Tighten a Comment

Take one generated comment and rewrite it so it explains more with fewer words.

### Exercise 3: Draft Release Notes

Ask Copilot to summarize the JavaScript track changes in 3-5 bullets for workshop release notes.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ `task.test.js` has stronger coverage than the starter scaffold
- ✅ `task.js` has a few helpful plain-language comments where they add value
- ✅ `src-javascript/task-manager/README.md` accurately reflects the track
- ✅ you can produce a Conventional Commit message for your work
- ✅ you have a clear PR description draft with testing notes and a checklist
- ✅ `node --test` still passes

---

## Troubleshooting

### `/tests` Generated Weak Assertions

**Problem**: the tests only check that something exists  
**Solution**: ask for specific behavior, error cases, and checks that the original object stays unchanged.

### Copilot Added Too Many Comments

**Problem**: the file is full of comments that repeat the code  
**Solution**: keep only comments that explain a rule, decision, or non-obvious reason.

### The README Drifted from Reality

**Problem**: the generated text mentions files or commands that do not exist  
**Solution**: compare every line against the actual track: `task.js`, `task.test.js`, `README.md`, and `node --test`.

### The Commit Message Is Too Vague

**Problem**: Copilot suggested something like `update javascript files`  
**Solution**: tell Copilot exactly what changed and ask for a Conventional Commit format.

---

## Workshop Wrap-Up

After Lab 4, the JavaScript track should leave you with a full lightweight workflow:

- write tests first
- turn plain-language requirements into code
- refactor for readability
- document the important decisions
- prepare clear commit and PR text

That is the same overall workshop journey as the other tracks, translated into the simplest possible technical setup.

---

## Additional Resources

- [Conventional Commits](https://www.conventionalcommits.org/)
- [Node.js test runner (`node:test`)](https://nodejs.org/api/test.html)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Angular version of Lab 4](lab-04-testing-documentation-workflow-angular.md)
- [Java/Spring Boot version of Lab 4](lab-04-testing-documentation-workflow-java.md)
