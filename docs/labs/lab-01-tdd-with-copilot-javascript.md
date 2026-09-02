# Lab 1: Test-Driven Development with GitHub Copilot (JavaScript)

> **💡 Also available**: [shared .NET / Spring Boot version](lab-01-tdd-with-copilot.md) · [Angular version](lab-01-tdd-with-copilot-angular.md) · [Swift version](lab-01-tdd-with-copilot-swift.md)

**Duration**: 30-40 minutes  
**Learning Objectives**:

- Practice the Red-Green-Refactor cycle in plain JavaScript
- Use Copilot to write a failing `node:test` test before changing code
- Add one small business rule to an existing function without extra tooling
- Review AI-generated code carefully instead of accepting it blindly
- Build confidence with a simple, low-ceremony testing workflow

---

## Overview

In this track, everything is intentionally small and direct:

- `src-javascript/task-manager/task.js`
- `src-javascript/task-manager/task.test.js`
- `src-javascript/task-manager/README.md`

There are no classes, no framework files, and no build step. The point of this lab is to show that **good engineering habits still matter even when the code is simple**.

The scaffold already gives you three functions:

- `createTask(title, description)`
- `updateStatus(task, newStatus)`
- `updateDetails(task, title, description)`

Each function includes a `TODO` comment. In this lab, you will pick one missing rule, write a failing test for it first, make the test pass, and then do a small cleanup.

---

## Prerequisites

- ✅ VS Code open with GitHub Copilot enabled
- ✅ Node.js installed (any current LTS release is fine)
- ✅ The JavaScript track opens successfully in your editor
- ✅ Baseline verification completed:

```bash
cd src-javascript/task-manager
node --test
```

Expected starting point: `3` passing tests.

---

## Part 1: Review the Starting Point (5 minutes)

### 1.1 Open the Existing Files

Start by reading:

- `src-javascript/task-manager/task.js`
- `src-javascript/task-manager/task.test.js`

Notice a few important things:

- the code uses plain functions, not classes
- tasks are plain JavaScript objects
- the current tests only cover the happy path
- the `TODO` comments mark the missing behavior you will add during the workshop

### 1.2 Ask Copilot to Explain the Gaps

In Copilot Chat, try:

```text
Explain the missing validation and business rules in src-javascript/task-manager/task.js.
Focus on createTask, updateStatus, and updateDetails.
Use plain language.
```

A helpful answer should mention gaps like:

- `createTask` accepts a blank title today
- `updateStatus` accepts any status change today
- `updateDetails` accepts anything without validation today

### 1.3 Pick One Small Rule

For Lab 1, keep the scope small. A good first rule is:

- **A task title cannot be blank**

That rule is easy to explain, easy to test, and easy to implement.

---

## Part 2: Write the Test First (RED Phase) (10-12 minutes)

> **TDD rule**: do not change `task.js` yet. Make the test fail first.

### 2.1 Ask Copilot to Add One New Test

Open `src-javascript/task-manager/task.test.js` and prompt Copilot:

```text
Add a node:test test to src-javascript/task-manager/task.test.js that proves createTask rejects a blank title.
Use node:assert/strict and match the existing test style.
```

### 2.2 Review the Suggested Test

A solid result should look roughly like this:

```js
test('createTask throws when the title is blank', () => {
  assert.throws(
    () => createTask('   ', 'Draft the quarterly report'),
    /title is required/i,
  );
});
```

What matters here is not the exact wording. What matters is that the test clearly says:

- what input is being used
- what behavior is expected
- why the current code should fail

### 2.3 Run the Tests

```bash
cd src-javascript/task-manager
node --test
```

**Expected result**: ❌ the new test fails.

That failure is good news. It proves your test is checking behavior that does not exist yet.

---

## Part 3: Implement the Smallest Fix (GREEN Phase) (10-12 minutes)

Now switch to `src-javascript/task-manager/task.js`.

### 3.1 Ask Copilot for a Minimal Change

Use a focused prompt like this:

```text
Update src-javascript/task-manager/task.js so createTask rejects a blank title.
Keep the file plain JavaScript.
Do not add classes, extra files, or frameworks.
Return the same task object shape when the input is valid.
```

### 3.2 Expected Implementation Shape

A clean answer often looks like this:

```js
function createTask(title, description) {
  const trimmedTitle = title.trim();

  if (!trimmedTitle) {
    throw new Error('Task title is required');
  }

  const now = new Date();
  return {
    id: crypto.randomUUID(),
    title: trimmedTitle,
    description,
    status: 'Todo',
    createdAt: now,
    updatedAt: now,
  };
}
```

This is a good Lab 1 solution because it is:

- small
- readable
- easy to explain
- directly driven by the test

### 3.3 Re-run the Tests

```bash
cd src-javascript/task-manager
node --test
```

**Expected result**: ✅ all tests pass again.

---

## Part 4: Light Cleanup (REFACTOR Phase) (6-8 minutes)

Passing tests mean the behavior works. Now you can do a small cleanup with confidence.

### 4.1 Ask Copilot for a Small Refactor

Try this prompt:

```text
Review src-javascript/task-manager/task.js and suggest one or two small refactorings that improve readability without changing behavior.
Keep everything in one file and use plain functions.
```

Good refactors for this lab might include:

- keeping the trimmed title in a clearly named variable
- making the error message more consistent
- avoiding repeated work if you later reuse the same validation idea elsewhere

### 4.2 Keep the Refactor Small

This is not the moment for a big redesign. Avoid:

- creating classes
- creating folders or layers
- rewriting all three functions at once
- adding rules you did not test first

### 4.3 Re-run the Tests Again

```bash
cd src-javascript/task-manager
node --test
```

The tests are your safety net. If they still pass, your cleanup preserved behavior.

---

## Key Learning Points

### ✅ Why This Workflow Helps

1. the test made the requirement concrete
2. the failing test proved the rule was missing
3. the smallest possible fix reduced risk
4. the passing test made refactoring safer

### ✅ Why This Simpler Track Still Matters

You do not need a large application to practice good habits. Even a tiny file like `task.js` benefits from:

- clear rules
- focused tests
- careful review of AI suggestions

---

## Extension Exercises (If Time Permits)

### Exercise 1: Protect the Description

Add a failing test that proves `createTask` rejects `null` or `undefined` descriptions.

### Exercise 2: Add a Status Rule

Add a failing test proving `updateStatus(task, 'NotARealStatus')` should throw.

### Exercise 3: Reuse the Same Validation

Add a failing test proving `updateDetails(task, '   ', 'Updated text')` should also reject a blank title.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ you added a new failing test to `src-javascript/task-manager/task.test.js`
- ✅ you updated `src-javascript/task-manager/task.js` to make that test pass
- ✅ `node --test` passes after your change
- ✅ you can explain what RED, GREEN, and REFACTOR each mean
- ✅ you reviewed Copilot's output instead of accepting it without checking

---

## Troubleshooting

### The New Test Passed Immediately

**Problem**: your test did not fail first  
**Solution**: make sure you are actually checking for an error with `assert.throws(...)` and using an input like `'   '`.

### Copilot Changed Too Much Code

**Problem**: Copilot started rewriting unrelated parts of the file  
**Solution**: restate the scope clearly: _"Only update createTask in src-javascript/task-manager/task.js."_

### The Error Message Does Not Match

**Problem**: the function throws, but the test still fails  
**Solution**: check whether your test expects a specific error message or pattern and make them match intentionally.

### You Forgot the RED Step

**Problem**: you changed the code before writing the test  
**Solution**: undo the change, write the test first, then repeat the cycle.

---

## Next Steps

Continue to [**Lab 2: From Requirements to Code (JavaScript)**](lab-02-requirements-to-code-javascript.md), where you'll turn a plain-language request into a slightly richer task model without adding any extra layers or tooling.

---

## Additional Resources

- [Node.js test runner (`node:test`)](https://nodejs.org/api/test.html)
- [Node.js strict assertions](https://nodejs.org/api/assert.html)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Angular version of Lab 1](lab-01-tdd-with-copilot-angular.md)
- [Shared .NET / Spring Boot walkthrough](lab-01-tdd-with-copilot.md)
