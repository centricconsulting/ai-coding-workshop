# Lab 3: Code Generation & Refactoring with GitHub Copilot (JavaScript)

> **💡 Also available**: [shared .NET version](lab-03-generation-and-refactoring.md) · [Java/Spring Boot version](lab-03-generation-and-refactoring-java.md) · [Angular version](lab-03-generation-and-refactoring-angular.md)

**Duration**: 35-45 minutes  
**Learning Objectives**:

- Use Copilot to improve code that has become harder to read
- Spot a beginner-friendly code smell: one function doing too many jobs
- Refactor plain JavaScript into smaller, named helper functions
- Preserve behavior by leaning on `node:test`
- Practice asking Copilot for focused refactors instead of broad rewrites

---

## 📝 Plan First Before You Refactor

Before changing code across a file, ask Copilot for a plan first.

Example prompt:

```text
Propose a step-by-step plan for refactoring src-javascript/task-manager/task.js so the logic is easier to read.
Assume updateDetails has grown to validate input, normalize values, and update the task all in one place.
Keep everything in one file and use plain functions.
```

A good plan should mention:

- which behavior must stay the same
- which tests protect the refactor
- which helper functions could be extracted
- how to keep the file simple for a non-technical audience

---

## Overview

By now, your JavaScript task manager may still be small, but small files can get messy too.

This lab focuses on a very common problem:

> **One function starts doing too many things at once.**

That is a useful refactoring moment because code becomes harder to:

- read
- explain
- test
- change safely

In this track, the fix is not “add more architecture.” The fix is usually much simpler:

- give each piece of logic one clear job
- use small helper functions with clear names
- keep everything in `src-javascript/task-manager/task.js`

---

## Prerequisites

- ✅ Completed Labs 1 and 2, or already have extra validation logic in `task.js`
- ✅ `src-javascript/task-manager/task.js` and `task.test.js` are working
- ✅ Baseline verification completed:

```bash
cd src-javascript/task-manager
node --test
```

---

## Part 1: Look for the “Too Many Jobs” Smell (8-10 minutes)

### Scenario: `updateDetails` Has Grown Too Much

After Lab 2, a version of `updateDetails` might look something like this:

```js
function updateDetails(task, title, description, priority) {
  if (!title || !title.trim()) {
    throw new Error('Task title is required');
  }

  if (description == null) {
    throw new Error('Task description is required');
  }

  if (!priority) {
    throw new Error('Task priority is required');
  }

  if (!['Low', 'Medium', 'High'].includes(priority)) {
    throw new Error('Task priority must be Low, Medium, or High');
  }

  return {
    ...task,
    title: title.trim(),
    description: description.trim(),
    priority,
    updatedAt: new Date(),
  };
}
```

This works, but it is doing several jobs at once:

- validating title
- validating description
- validating priority
- building the updated object
- normalizing trimmed values before returning the updated task

### 1.1 Ask Copilot to Explain the Problem

Try this prompt:

```text
/explain Why is this JavaScript function hard to maintain?
Focus on the idea that one function is doing too many jobs.
Use simple language.
```

A helpful answer should mention:

- it is harder to scan quickly
- small changes are riskier
- repeated validation becomes harder to reuse
- the function mixes validation and object-building work in one crowded block

---

## Part 2: Protect the Refactor with Tests First (8-10 minutes)

Before cleaning anything up, make sure `task.test.js` protects the current behavior.

### 2.1 Ask Copilot for Test Gaps

Prompt:

```text
Review src-javascript/task-manager/task.test.js and suggest which tests should exist before refactoring updateDetails.
Focus on validation, normalization, and copy behavior.
```

You want tests that prove things like:

- blank titles are rejected
- invalid priorities are rejected
- a new object is returned
- the original task is unchanged

### 2.2 Add Missing Tests

A representative test might look like:

```js
test('updateDetails returns a new object and keeps the original unchanged', () => {
  const task = createTask('Write report', 'Draft the quarterly report', 'Low');

  const updated = updateDetails(task, 'Write final report', 'Board version', 'High');

  assert.notEqual(updated, task);
  assert.equal(updated.title, 'Write final report');
  assert.equal(updated.priority, 'High');
  assert.equal(task.title, 'Write report');
  assert.equal(task.priority, 'Low');
});
```

### 2.3 Run the Tests

```bash
cd src-javascript/task-manager
node --test
```

If the tests pass, you are ready to refactor safely.

---

## Part 3: Refactor into Small, Named Helpers (12-15 minutes)

### 3.1 Ask Copilot for a Focused Refactor

Use a prompt like this:

```text
/refactor Refactor src-javascript/task-manager/task.js so updateDetails no longer does too many things at once.
Requirements:
- keep everything in one file
- use plain helper functions
- keep the same external behavior
- separate validation, normalization, and object updating into smaller named functions
- do not introduce classes or extra files
```

### 3.2 Expected Refactoring Shape

A clean refactor might produce helpers like these:

```js
function validateTitle(title) {
  const trimmedTitle = title.trim();

  if (!trimmedTitle) {
    throw new Error('Task title is required');
  }

  return trimmedTitle;
}

function validatePriority(priority) {
  if (!priority) {
    throw new Error('Task priority is required');
  }

  if (!['Low', 'Medium', 'High'].includes(priority)) {
    throw new Error('Task priority must be Low, Medium, or High');
  }

  return priority;
}

function validateDescription(description) {
  if (description == null) {
    throw new Error('Task description is required');
  }

  return description.trim();
}

function updateDetails(task, title, description, priority) {
  return {
    ...task,
    title: validateTitle(title),
    description: validateDescription(description),
    priority: validatePriority(priority),
    updatedAt: new Date(),
  };
}
```

This version is easier to read because each helper has one clear job.

### 3.3 Why This Refactor Is a Good Fit for This Track

It keeps the lesson simple:

- no new architecture diagram needed
- no framework knowledge needed
- no extra files to hunt through
- just clearer code in the same file

---

## Part 4: Re-run, Review, and Explain the Improvement (8-10 minutes)

### 4.1 Run the Tests Again

```bash
cd src-javascript/task-manager
node --test
```

**Expected result**: ✅ tests still pass.

That tells you the behavior stayed the same while the structure improved.

### 4.2 Ask Copilot for a Final Review

Try this prompt:

```text
/check Review the refactored src-javascript/task-manager/task.js.
Explain whether the helper function names are clear and whether each function now has one main job.
```

### 4.3 What “Better” Should Feel Like

After the refactor, someone new to the file should be able to understand it faster because:

- validation has names
- normalization has a clear place to live
- the main function reads more like a short recipe than a long wall of logic

---

## Key Learning Points

### ✅ What You Practiced

1. using tests as protection before refactoring
2. spotting when one function has grown too busy
3. extracting small helpers with clear names
4. improving readability without changing the outside behavior

### ✅ What This Lab Is Not About

This lab is **not** about adding layers, patterns, or ceremony. In this track, better structure usually means:

- smaller functions
- clearer names
- less mixed responsibility

---

## Extension Exercises (If Time Permits)

### Exercise 1: Reuse Helpers in `createTask`

If you extracted `validateTitle` or `validatePriority`, use them in `createTask` too.

### Exercise 2: Remove Repeated Allowed Values

Extract the priority list into a constant so it is not repeated in multiple places.

### Exercise 3: Reuse Description Validation

If you created `validateDescription`, reuse it anywhere description text is accepted so the rules stay consistent.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ your tests covered the current behavior before refactoring
- ✅ `updateDetails` no longer mixes too many concerns in one block of code
- ✅ helper functions have clear, plain names
- ✅ everything still lives in `src-javascript/task-manager/task.js`
- ✅ `node --test` still passes after the refactor
- ✅ you can explain the improvement as “one function, one job”

---

## Troubleshooting

### Copilot Tried to Redesign the Whole App

**Problem**: the suggestion introduced classes, folders, or a bigger architecture  
**Solution**: restate the scope: _"Keep the plain JavaScript track in one file. I only want a small readability refactor."_

### The Refactor Changed Behavior

**Problem**: tests started failing after the cleanup  
**Solution**: compare the before and after behavior carefully. Restore the tests to green before doing more cleanup.

### Helper Names Still Feel Vague

**Problem**: Copilot suggested names like `processTaskData`  
**Solution**: prefer names that say exactly what the function does, such as `validatePriority` or `formatTaskSummary`.

### The Main Function Is Still Too Long

**Problem**: `updateDetails` still feels crowded  
**Solution**: ask Copilot one more focused question: _"What logic here still has a second job that could be named and extracted?"_

---

## Next Steps

Continue to [**Lab 4: Testing, Documentation & Workflow (JavaScript)**](lab-04-testing-documentation-workflow-javascript.md), where you'll use Copilot to expand tests, improve comments, and prepare simple commit and pull request text.

---

## Additional Resources

- [Refactoring Guru](https://refactoring.guru/refactoring)
- [Node.js test runner (`node:test`)](https://nodejs.org/api/test.html)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Angular version of Lab 3](lab-03-generation-and-refactoring-angular.md)
- [Java/Spring Boot version of Lab 3](lab-03-generation-and-refactoring-java.md)
