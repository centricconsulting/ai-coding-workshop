# Lab 2: From Requirements to Code with GitHub Copilot (JavaScript)

> **💡 Also available**: [shared .NET version](lab-02-requirements-to-code.md) · [Java/Spring Boot version](lab-02-requirements-to-code-java.md) · [Angular version](lab-02-requirements-to-code-angular.md)

**Duration**: 40-45 minutes  
**Learning Objectives**:

- Turn a plain-language request into a small set of code changes
- Add a `priority` concept without introducing classes, layers, or new tooling
- Write tests first with Node's built-in test runner
- Keep all task behavior in one JavaScript file on purpose
- Practice reviewing Copilot suggestions for scope and clarity

---

## Overview

In Lab 1 you added one small rule with test-first development. In this lab, you will take a slightly bigger step: turning a plain-language requirement into working code.

For this JavaScript track, the design stays intentionally simple:

- all task behavior stays in `src-javascript/task-manager/task.js`
- tests stay in `src-javascript/task-manager/task.test.js`
- you run everything with `node --test`

That simplicity is a feature, not a limitation. It keeps the focus on how to think, not on framework setup.

---

## Prerequisites

- ✅ `src-javascript/task-manager/task.js` and `task.test.js` are open in VS Code
- ✅ You understand the basic RED → GREEN → REFACTOR cycle
- ✅ Baseline verification completed:

```bash
cd src-javascript/task-manager
node --test
```

---

## Part 1: Turn the Request into Clear Rules (8-10 minutes)

### Scenario: Add Priority to a Task

A stakeholder says:

> **User Story**: As a workshop participant, I want each task to have a priority so I can quickly see what matters most.

That sounds simple, but it still needs to be translated into clear decisions.

### 1.1 Ask Copilot to Break the Story Down

Try this prompt:

```text
I have a plain JavaScript task manager in src-javascript/task-manager/task.js.
There are only plain functions and object literals.
Turn this user story into 4-6 small backlog items:
"As a workshop participant, I want each task to have a priority so I can quickly see what matters most."
Keep the answer beginner-friendly.
```

A helpful answer should suggest work like:

- decide what valid priorities are
- store `priority` on each task
- reject missing or invalid priorities
- allow task details to update priority later
- add tests for both valid and invalid input

### 1.2 Choose a Small, Concrete Ruleset

For this workshop, use these simple rules:

- valid priorities are `Low`, `Medium`, and `High`
- `createTask` should store `priority`
- `createTask` should reject missing or invalid `priority`
- `updateDetails` should also be able to update `priority`

You are still working in one file. No repositories, services, DTOs, or extra folders.

---

## Part 2: Write the Tests First (RED Phase) (12-15 minutes)

### 2.1 Expand the Test File Before the Implementation

Open `src-javascript/task-manager/task.test.js` and prompt Copilot:

```text
Update src-javascript/task-manager/task.test.js for a new priority feature.
Assume we are extending the plain JavaScript API.
Add node:test tests that verify:
- createTask stores a priority value
- createTask rejects a missing priority
- createTask rejects an invalid priority
- updateDetails returns a copy with a new priority
Keep the existing node:test and assert style.
```

### 2.2 Expected Test Shape

Your generated tests might look something like this:

```js
test('createTask stores a priority value', () => {
  const task = createTask('Write report', 'Draft the quarterly report', 'High');

  assert.equal(task.priority, 'High');
});

test('createTask throws when priority is missing', () => {
  assert.throws(
    () => createTask('Write report', 'Draft the quarterly report'),
    /priority is required/i,
  );
});

test('createTask throws when priority is invalid', () => {
  assert.throws(
    () => createTask('Write report', 'Draft the quarterly report', 'Urgent'),
    /priority must be low, medium, or high/i,
  );
});

test('updateDetails returns a copy with a new priority', () => {
  const task = createTask('Write report', 'Draft the quarterly report', 'Low');

  const updated = updateDetails(
    task,
    'Write final report',
    'Final version for the board',
    'High',
  );

  assert.equal(updated.priority, 'High');
  assert.equal(task.priority, 'Low');
});
```

The important part is the thinking:

- define the valid inputs
- define the invalid inputs
- protect the existing copy-returning behavior

### 2.3 Run the Tests

```bash
cd src-javascript/task-manager
node --test
```

**Expected result**: ❌ the new tests fail.

That is the signal to move into implementation.

---

## Part 3: Implement the Feature in One File (GREEN Phase) (12-15 minutes)

Now update `src-javascript/task-manager/task.js`.

### 3.1 Ask Copilot for a Single-File Solution

Use a prompt like this:

```text
Update src-javascript/task-manager/task.js for a new priority feature.
Requirements:
- keep everything in this one file
- no classes, no layers, no extra files
- valid priorities are Low, Medium, and High
- createTask should accept and store priority
- updateDetails should accept and store priority
- throw clear errors for missing or invalid priority
- preserve the current pattern of returning a copy from updateDetails
```

### 3.2 Expected Implementation Shape

A clean result often looks like this:

```js
const TASK_STATUSES = ['Todo', 'InProgress', 'Done', 'Cancelled'];
const TASK_PRIORITIES = ['Low', 'Medium', 'High'];

function validatePriority(priority) {
  if (!priority) {
    throw new Error('Task priority is required');
  }

  if (!TASK_PRIORITIES.includes(priority)) {
    throw new Error('Task priority must be Low, Medium, or High');
  }

  return priority;
}

function createTask(title, description, priority) {
  const now = new Date();
  return {
    id: crypto.randomUUID(),
    title,
    description,
    priority: validatePriority(priority),
    status: 'Todo',
    createdAt: now,
    updatedAt: now,
  };
}

function updateDetails(task, title, description, priority) {
  return {
    ...task,
    title,
    description,
    priority: validatePriority(priority),
    updatedAt: new Date(),
  };
}
```

This keeps the design aligned with the track goals:

- plain JavaScript
- plain objects
- one file
- one small feature at a time

### 3.3 Re-run the Tests

```bash
cd src-javascript/task-manager
node --test
```

**Expected result**: ✅ all tests pass.

---

## Part 4: Review the Requirement Against the Code (REFACTOR Phase) (8-10 minutes)

### 4.1 Ask Copilot to Compare the Story and the Implementation

Try this:

```text
Review src-javascript/task-manager/task.js and task.test.js.
Check whether the priority feature matches the user story and whether the code stayed simple.
Suggest only small improvements.
```

Good suggestions may include:

- reusing the same validation helper in more than one place
- tightening test names so they read like plain English
- making error messages easier for a beginner to understand

### 4.2 Keep the Design Intentionally Small

This track is not trying to look like the .NET, Java, or Angular versions. Avoid suggestions such as:

- splitting files into multiple layers
- introducing a class for `Task`
- adding third-party test tools
- building a data-access abstraction

### 4.3 Re-run the Tests After Cleanup

```bash
cd src-javascript/task-manager
node --test
```

---

## Key Learning Points

### ✅ What This Lab Teaches

1. plain-language requests still need clear decisions
2. tests help you turn vague ideas into exact behavior
3. simple code can still be well-structured
4. Copilot helps more when your prompt names the rules clearly

### ✅ Why the Single-File Rule Helps Here

For this audience, one file is easier to follow than four folders. You can still practice:

- validation
- naming
- test-first thinking
- safe refactoring

without the overhead of a bigger architecture.

---

## Extension Exercises (If Time Permits)

### Exercise 1: Pick a Default Priority

Instead of throwing when priority is missing, decide whether new tasks should default to `Medium`. Write the test first.

### Exercise 2: Add Priority to Sorting Language

Ask Copilot to propose how a future version could sort `High` tasks before `Low` tasks, without implementing it yet.

### Exercise 3: Reuse Title Validation Too

If you added title validation in Lab 1, make sure both `createTask` and `updateDetails` reuse it consistently.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ `task.test.js` includes tests for valid and invalid priority behavior
- ✅ `task.js` stores `priority` on the task object
- ✅ invalid priority input is rejected clearly
- ✅ `updateDetails(...)` can update priority while returning a copy
- ✅ `node --test` passes after the feature is implemented
- ✅ the solution still stays in one JavaScript file

---

## Troubleshooting

### Copilot Suggested a New File

**Problem**: Copilot proposed a separate `priority.js` or another folder  
**Solution**: restate the constraint clearly: _"Keep everything in src-javascript/task-manager/task.js for this track."_

### The Tests Broke Because the Function Signature Changed

**Problem**: older tests still call `createTask(title, description)`  
**Solution**: update the relevant tests intentionally and check that the new parameter is passed everywhere it is needed.

### An Invalid Priority Still Slips Through

**Problem**: the code stores any string  
**Solution**: confirm you are checking the value against a fixed allowed list such as `['Low', 'Medium', 'High']`.

### The Original Object Changed Too

**Problem**: `updateDetails` updated the original task instead of returning a copy  
**Solution**: keep the spread pattern `return { ...task, ...changes }` and assert against both the new and original object in your test.

---

## Next Steps

Continue to [**Lab 3: Code Generation & Refactoring (JavaScript)**](lab-03-generation-and-refactoring-javascript.md), where you'll use Copilot to clean up a function that has started doing too many jobs at once.

---

## Additional Resources

- [Node.js test runner (`node:test`)](https://nodejs.org/api/test.html)
- [JavaScript MDN Guide](https://developer.mozilla.org/en-US/docs/Web/JavaScript/Guide)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Angular version of Lab 2](lab-02-requirements-to-code-angular.md)
- [Java/Spring Boot version of Lab 2](lab-02-requirements-to-code-java.md)
