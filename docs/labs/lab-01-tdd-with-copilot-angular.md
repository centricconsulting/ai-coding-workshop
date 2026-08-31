# Lab 1: Test-Driven Development with GitHub Copilot (Angular)

> **💡 Also available**: [shared .NET / Spring Boot version](lab-01-tdd-with-copilot.md)

**Duration**: 30-40 minutes  
**Learning Objectives**:

- Practice the Red-Green-Refactor cycle in an Angular + TypeScript codebase
- Use Copilot to write Vitest specs before changing implementation
- Add validation and business rules to an existing domain model
- Keep business rules in the Domain layer instead of the component layer
- Build confidence reviewing and refining AI-generated tests and code

---

## Overview

In this lab, you'll work inside the Angular track's existing `Task` aggregate:

- `src-angular/task-manager/src/app/domain/task.ts`
- `src-angular/task-manager/src/app/domain/task.spec.ts`

The workshop scaffold intentionally leaves three `TODO` markers in `Task`:

1. `Task.create(title, description)`
2. `updateStatus(newStatus)`
3. `updateDetails(title, description)`

Your job is to use GitHub Copilot to drive those rules in with tests first, then implementation, then light refactoring.

> **Angular track reminder**: this project is a **self-contained SPA with an in-memory data layer**. There is no API or server in this track, so the TDD focus in Lab 1 is pure domain logic.

---

## Prerequisites

- ✅ Repository cloned and your workshop branch created from `main`
- ✅ VS Code open with GitHub Copilot and Copilot Chat enabled
- ✅ Node.js **24.15+** (or 22.22.3+ / 26+) installed
- ✅ Dependencies installed in `src-angular/task-manager`
- ✅ Baseline verification completed:

```bash
cd src-angular/task-manager
npm install
npx ng build
npx ng test --watch=false
```

Expected starting point: the Angular track builds successfully and the starter suite passes (`2` test files, `6` tests).

---

## Part 1: Review the Starting Point (5 minutes)

### 1.1 Open the Existing Domain Files

Start by reviewing:

- `src-angular/task-manager/src/app/domain/task.ts`
- `src-angular/task-manager/src/app/domain/task-status.ts`
- `src-angular/task-manager/src/app/domain/task.spec.ts`

Notice that the domain model already has a good shape:

- `TaskId` is a dedicated value type
- `TaskStatus` models lifecycle explicitly
- `Task` uses a factory method instead of a public constructor
- The implementation intentionally omits validation and lifecycle rules

### 1.2 Ask Copilot to Summarize the Gaps

In Copilot Chat, try:

```text
Explain what validation and business rules are still missing from src-angular/task-manager/src/app/domain/task.ts. Focus on Task.create, updateStatus, and updateDetails.
```

A good answer should point out gaps such as:

- Missing title validation
- No length checks
- No invalid status-transition rules
- No shared validation helper methods
- No protection against editing completed or cancelled work (if you choose that rule)

### 1.3 Pick a Reasonable Ruleset

For the workshop, use a ruleset that is easy to explain and easy to test. One solid option is:

- **Title is required** and cannot be blank after trimming
- **Title max length** is `100`
- **Description max length** is `500`
- A task cannot transition **from `Done` back to `Todo` or `InProgress`**
- A task cannot transition **out of `Cancelled`**
- `updateDetails()` reuses the same validation as `create()`
- Optional workshop rule: once a task is `Done` or `Cancelled`, it cannot be edited

> If your facilitator prefers a slightly different ruleset, that's fine. The important part is that your **tests define the behavior first**.

---

## Part 2: Write Tests First (RED Phase) (12 minutes)

> **TDD rule**: do not implement the production code yet. Make the tests fail first.

### 2.1 Request Spec Generation

Open Copilot Chat and enter:

```text
Update src-angular/task-manager/src/app/domain/task.spec.ts with Vitest tests for Task that cover:
- Task.create rejects blank titles
- Task.create rejects titles longer than 100 characters
- Task.create rejects descriptions longer than 500 characters
- updateStatus rejects invalid transitions from Done back to Todo
- updateStatus rejects transitions out of Cancelled
- updateDetails reuses title and description validation
- updateDetails throws when called on a completed task
Keep the current describe structure and TypeScript style.
```

### 2.2 Review the Generated Tests Carefully

Copilot will often get you 80-90% of the way there, but you still need to review naming, assertions, and intent.

A good Vitest addition should look roughly like this:

```ts
import { describe, expect, it } from 'vitest';
import { Task } from './task';
import { TaskStatus } from './task-status';

describe('Task', () => {
  describe('create', () => {
    it('should throw when the title is blank', () => {
      expect(() => Task.create('   ', 'Write release notes')).toThrowError(
        'Task title is required',
      );
    });

    it('should throw when the title exceeds 100 characters', () => {
      const title = 'A'.repeat(101);

      expect(() => Task.create(title, 'Write release notes')).toThrowError(
        'Task title must be 100 characters or fewer',
      );
    });

    it('should throw when the description exceeds 500 characters', () => {
      const description = 'D'.repeat(501);

      expect(() => Task.create('Release notes', description)).toThrowError(
        'Task description must be 500 characters or fewer',
      );
    });
  });

  describe('updateStatus', () => {
    it('should reject moving a completed task back to todo', () => {
      const task = Task.create('Review PR', 'Check the workshop branch');
      task.updateStatus(TaskStatus.Done);

      expect(() => task.updateStatus(TaskStatus.Todo)).toThrowError(
        'Completed tasks cannot be reopened in this lab',
      );
    });

    it('should reject transitions from cancelled', () => {
      const task = Task.create('Review PR', 'Check the workshop branch');
      task.updateStatus(TaskStatus.Cancelled);

      expect(() => task.updateStatus(TaskStatus.InProgress)).toThrowError(
        'Cancelled tasks cannot change status',
      );
    });
  });

  describe('updateDetails', () => {
    it('should reuse title validation', () => {
      const task = Task.create('Original', 'Original description');

      expect(() => task.updateDetails('', 'Updated description')).toThrowError(
        'Task title is required',
      );
    });

    it('should prevent editing completed tasks', () => {
      const task = Task.create('Original', 'Original description');
      task.updateStatus(TaskStatus.Done);

      expect(() => task.updateDetails('Updated', 'Updated description')).toThrowError(
        'Completed or cancelled tasks cannot be edited',
      );
    });
  });
});
```

### 2.3 Run the Tests and Confirm Failure

```bash
cd src-angular/task-manager
npm test -- --watch=false
```

**Expected Result**: ❌ Tests fail.

That failure is valuable. It proves your new specs are actually checking behavior that does not exist yet.

### 2.4 Reflect Before Coding

Before switching to implementation, ask yourself:

- ✅ Do test names describe behavior clearly?
- ✅ Are happy-path and guard-clause cases both covered?
- ✅ Are the failure messages specific enough to guide implementation?
- ✅ Are you testing domain rules, not UI behavior?

---

## Part 3: Implement the Minimum Code (GREEN Phase) (12 minutes)

Now use Copilot to update `src-angular/task-manager/src/app/domain/task.ts`.

### 3.1 Prompt Copilot with Explicit Constraints

```text
Update src-angular/task-manager/src/app/domain/task.ts so the new Vitest tests pass.
Implement:
- title required validation
- title max length 100
- description max length 500
- invalid status transition rules for Done and Cancelled
- updateDetails should reuse shared validation helpers
- updateDetails should reject edits when a task is Done or Cancelled
Keep the class shape intact and use small private helper methods instead of duplicating validation logic.
```

### 3.2 Expected Implementation Shape

A clean solution will usually introduce a couple of constants and helper methods:

```ts
const MAX_TITLE_LENGTH = 100;
const MAX_DESCRIPTION_LENGTH = 500;

export class Task {
  // existing fields omitted for brevity

  static create(title: string, description: string): Task {
    const validatedTitle = Task.validateTitle(title);
    const validatedDescription = Task.validateDescription(description);

    return new Task(
      TaskId.new(),
      validatedTitle,
      validatedDescription,
      TaskStatus.Todo,
      new Date(),
    );
  }

  updateStatus(newStatus: TaskStatus): void {
    if (this._status === TaskStatus.Cancelled) {
      throw new Error('Cancelled tasks cannot change status');
    }

    if (this._status === TaskStatus.Done && newStatus !== TaskStatus.Done) {
      throw new Error('Completed tasks cannot be reopened in this lab');
    }

    this._status = newStatus;
    this._updatedAt = new Date();
  }

  updateDetails(title: string, description: string): void {
    if (this._status === TaskStatus.Done || this._status === TaskStatus.Cancelled) {
      throw new Error('Completed or cancelled tasks cannot be edited');
    }

    this._title = Task.validateTitle(title);
    this._description = Task.validateDescription(description);
    this._updatedAt = new Date();
  }

  private static validateTitle(title: string): string {
    const trimmedTitle = title.trim();

    if (!trimmedTitle) {
      throw new Error('Task title is required');
    }

    if (trimmedTitle.length > MAX_TITLE_LENGTH) {
      throw new Error('Task title must be 100 characters or fewer');
    }

    return trimmedTitle;
  }

  private static validateDescription(description: string): string {
    if (description.length > MAX_DESCRIPTION_LENGTH) {
      throw new Error('Task description must be 500 characters or fewer');
    }

    return description.trim();
  }
}
```

### 3.3 Re-run the Suite

```bash
cd src-angular/task-manager
npm test -- --watch=false
```

**Expected Result**: ✅ Tests pass.

At this point you've completed the **Green** step: the smallest change that satisfies the tests.

---

## Part 4: Refactor and Review (REFACTOR Phase) (8 minutes)

### 4.1 Ask Copilot for a Focused Review

Use Copilot Chat:

```text
/check Review src-angular/task-manager/src/app/domain/task.ts and task.spec.ts. Suggest small refactorings that improve readability without changing behavior.
```

Likely suggestions:

- Extract constants for repeated limits
- Keep error messages consistent
- Trim inputs once, not multiple times
- Add one or two additional edge-case tests
- Rename variables for clarity

### 4.2 Light Refactors Worth Accepting

Good refactors in this lab are small and behavior-preserving:

- Rename `validatedTitle` / `validatedDescription` if Copilot proposes clearer names
- Reuse helpers instead of repeating guard clauses
- Keep methods short and intention-revealing
- Ensure timestamps are updated in only one place per state change

### 4.3 Re-run After Every Refactor

```bash
cd src-angular/task-manager
npm test -- --watch=false
npx ng build
```

Passing tests after refactoring are the proof that you improved structure without breaking behavior.

---

## Key Learning Points

### ✅ What TDD Gave You

1. **Better design pressure** - the spec file forced you to decide the domain rules explicitly
2. **Safer refactoring** - you could clean up helpers after behavior was locked in
3. **Faster review of AI output** - Copilot generated a starting point, but your tests kept it honest
4. **Framework-independent business rules** - these rules live in the domain model, not in Angular templates

### ✅ What Good Angular Architecture Looks Like Here

- `Task` owns task invariants
- `TaskListComponent` should remain a thin presentation layer in later labs
- Application services orchestrate use cases
- Repositories hide storage details, even when that storage is only in-memory

---

## Extension Exercises (If Time Permits)

### Exercise 1: Reject Duplicate Status Changes

Add a test proving `updateStatus(TaskStatus.Todo)` throws if the task is already `Todo`.

### Exercise 2: Normalize Whitespace

Update the implementation so titles and descriptions are trimmed before storage, then add tests proving it.

### Exercise 3: Add More Lifecycle Rules

For example:

- `Todo -> Done` might require going through `InProgress`
- `Cancelled -> Done` is invalid
- `Done -> Cancelled` is invalid

If you add a rule, add the failing spec first.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ `task.spec.ts` contains meaningful new failing tests first
- ✅ `Task.create()` enforces title and length validation
- ✅ `updateStatus()` enforces explicit lifecycle rules
- ✅ `updateDetails()` reuses validation logic instead of duplicating it
- ✅ All Vitest specs pass after implementation
- ✅ `npx ng build` still succeeds
- ✅ You can explain the Red-Green-Refactor cycle using your own changes

---

## Troubleshooting

### Tests Passed Immediately

**Problem**: your new tests passed before you changed production code  
**Solution**: the assertions may be too weak. Verify you're using `toThrowError(...)` with meaningful expectations.

### Copilot Suggested UI Changes

**Problem**: Copilot started modifying components or templates  
**Solution**: restate the scope: _"Only change src-angular/task-manager/src/app/domain/task.ts and task.spec.ts."_

### Error Messages Don't Match

**Problem**: the implementation throws, but the tests still fail  
**Solution**: align on one clear message per rule, then update either the test or implementation intentionally.

### Build Fails After Passing Tests

**Problem**: TypeScript or formatting issues remain  
**Solution**: run `npx ng build` and fix any compiler errors before moving on.

---

## Next Steps

Continue to [**Lab 2: From Requirements to Code (Angular)**](lab-02-requirements-to-code-angular.md), where you'll extend the in-memory repository, add `Priority`, and wire the create-task flow through the Angular application.

---

## Additional Resources

- [Angular Testing Guide](https://angular.dev/guide/testing)
- [Vitest Documentation](https://vitest.dev/guide/)
- [GitHub Copilot Documentation](https://docs.github.com/en/copilot)
- [Shared Lab 1 walkthrough (.NET / Spring Boot)](lab-01-tdd-with-copilot.md)
