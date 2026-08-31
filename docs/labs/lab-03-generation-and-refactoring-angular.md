# Lab 3: Code Generation & Refactoring with GitHub Copilot (Angular)

> **💡 Also available**: [.NET version](lab-03-generation-and-refactoring.md) · [Java/Spring Boot version](lab-03-generation-and-refactoring-java.md)

**Duration**: 45 minutes  
**Learning Objectives**:

- Generate the remaining CRUD-style application service methods with Copilot
- Extend the repository contract intentionally when new behavior requires it
- Use `@workspace`, `#file`, and `/refactor` effectively in an Angular project
- Refactor business logic out of a component into an injectable application service
- Apply Angular-friendly clean code principles while preserving behavior

---

## 📝 Plan First Before You Refactor

Before making a multi-file change, ask Copilot for a plan first.

Example prompt:

```text
Propose a step-by-step plan for extending TaskApplicationService with update, delete, and list behavior in the Angular track, then refactoring component-level task logic into the service.
```

A good plan should mention:

- repository contract changes
- new service methods
- the tests that should be written first
- which component behavior should stay in the presentation layer
- how to verify the refactor with Vitest and `ng build`

---

## Overview

In Lab 2 you built a working create flow. In this lab you'll do two things:

1. **Finish the use-case surface area** for the SPA by adding update, delete, and richer list behavior
2. **Refactor an Angular-specific code smell**: business logic inlined directly inside a component

This is intentionally different from the .NET and Spring Boot Lab 3 walkthroughs. The Angular track should reinforce the same Clean Architecture principle using a pattern Angular developers actually encounter.

---

## Prerequisites

- ✅ Completed [Lab 2 (Angular)](lab-02-requirements-to-code-angular.md)
- ✅ `Priority`, `TaskApplicationService`, and `InMemoryTaskRepository` are working
- ✅ `npm test -- --watch=false` and `npx ng build` are green before you start

---

## Part 1: Generate the Remaining CRUD-Style Use Cases (20 minutes)

### Scenario: The SPA Needs More Than Create + Complete

Your current application service is minimal:

- `createTask(...)`
- `completeTask(task)`
- `listActiveTasks()`

Now expand it so the Angular SPA can support task details, editing, deletion, and richer listing.

### 1.1 Understand the Existing Flow with @workspace

Ask Copilot:

```text
@workspace Show me how the Angular task manager currently flows from TaskListComponent to TaskApplicationService to InMemoryTaskRepository.
Identify which files would need to change to support update and delete use cases.
```

You should see Copilot reference:

- `src-angular/task-manager/src/app/features/tasks/task-list.ts`
- `src-angular/task-manager/src/app/application/task-application.service.ts`
- `src-angular/task-manager/src/app/domain/task-repository.ts`
- `src-angular/task-manager/src/app/data/in-memory-task-repository.ts`

### 1.2 Extend the Repository Contract Intentionally

The current `TaskRepository` interface has no delete method and only exposes `getActiveTasks()`.

That is a good reminder: **new use cases sometimes require explicit contract changes**.

Ask Copilot:

```text
Update src-angular/task-manager/src/app/domain/task-repository.ts for the next Angular lab.
Add only the methods needed to support richer CRUD behavior:
- getAllTasks()
- removeTask(taskId: TaskId)
Keep business-intent names and the existing InjectionToken.
```

A reasonable interface now looks like:

```ts
export interface TaskRepository {
  findById(taskId: TaskId): Promise<Task | undefined>;
  getActiveTasks(): Promise<Task[]>;
  getAllTasks(): Promise<Task[]>;
  addTask(task: Task): Promise<void>;
  saveChanges(task: Task): Promise<void>;
  removeTask(taskId: TaskId): Promise<void>;
}
```

### 1.3 Write Application Service Tests First

Create or expand:

- `src-angular/task-manager/src/app/application/task-application.service.spec.ts`

Prompt Copilot:

```text
Add Vitest tests for TaskApplicationService covering these new use cases:
- getTask returns a task by id string
- updateTask updates title, description, and priority, then saves changes
- deleteTask removes an existing task by id string
- listTasks('all') returns getAllTasks results
- listTasks('active') returns getActiveTasks results
- updateTask throws when the task does not exist
Mock the repository via TASK_REPOSITORY.
```

### 1.4 Generate the Service Methods

Now update `src-angular/task-manager/src/app/application/task-application.service.ts`.

Use `#file` context and be explicit:

```text
Update #file:src-angular/task-manager/src/app/application/task-application.service.ts.
Add these methods:
- getTask(taskId: string)
- updateTask(taskId: string, title: string, description: string, priority: Priority)
- deleteTask(taskId: string)
- listTasks(filter: 'all' | 'active')
Use TaskId.from(taskId), keep orchestration in the service, and throw a clear error when a task is missing.
Assume Task has updateDetails and updatePriority.
```

A solid result should look similar to:

```ts
import { Priority } from '../domain/priority';
import { TaskId } from '../domain/task-id';

async getTask(taskId: string): Promise<Task | undefined> {
  return this.taskRepository.findById(TaskId.from(taskId));
}

async updateTask(
  taskId: string,
  title: string,
  description: string,
  priority: Priority,
): Promise<Task> {
  const task = await this.getRequiredTask(taskId);

  task.updateDetails(title, description);
  task.updatePriority(priority);
  await this.taskRepository.saveChanges(task);

  return task;
}

async deleteTask(taskId: string): Promise<void> {
  const task = await this.getRequiredTask(taskId);
  await this.taskRepository.removeTask(task.id);
}

async listTasks(filter: 'all' | 'active' = 'active'): Promise<Task[]> {
  return filter === 'all'
    ? this.taskRepository.getAllTasks()
    : this.taskRepository.getActiveTasks();
}

private async getRequiredTask(taskId: string): Promise<Task> {
  const task = await this.taskRepository.findById(TaskId.from(taskId));

  if (!task) {
    throw new Error(`Task ${taskId} was not found`);
  }

  return task;
}
```

### 1.5 Update the In-Memory Repository

Prompt Copilot:

```text
Update src-angular/task-manager/src/app/data/in-memory-task-repository.ts to match the expanded TaskRepository interface.
Implement getAllTasks and removeTask.
Keep the behavior deterministic for testing.
```

A straightforward implementation is:

```ts
async getAllTasks(): Promise<Task[]> {
  return [...this.tasks.values()].sort(
    (left, right) => right.updatedAt.getTime() - left.updatedAt.getTime(),
  );
}

async removeTask(taskId: TaskId): Promise<void> {
  if (!this.tasks.delete(taskId.value)) {
    throw new Error(`Task ${taskId.value} does not exist in the repository`);
  }
}
```

### 1.6 Validate the Use Cases

Run:

```bash
cd src-angular/task-manager
npm test -- --watch=false
npx ng build
```

At this point the Angular track should support a fuller set of use cases without putting domain logic in the UI.

---

## Part 2: Refactor Business Logic Out of a Component (15 minutes)

### Scenario: An Angular-Smelling Component

For this exercise, create a **before** example that intentionally violates separation of concerns.

You can paste the following into a scratch component such as `src-angular/task-manager/src/app/features/tasks/task-board.ts`, or just use it as a review/refactor exercise in the doc.

### 2.1 The “Before” Component

```ts
import { Component, inject, signal } from '@angular/core';
import { TASK_REPOSITORY } from '../../domain/task-repository';
import { Priority } from '../../domain/priority';
import { Task } from '../../domain/task';
import { TaskStatus } from '../../domain/task-status';

@Component({
  selector: 'app-task-board',
  standalone: true,
  template: '',
})
export class TaskBoardComponent {
  private readonly repository = inject(TASK_REPOSITORY);

  protected readonly tasks = signal<Task[]>([]);
  protected readonly filter = signal<'all' | 'active' | 'done'>('active');
  protected readonly sortBy = signal<'priority' | 'title'>('priority');
  protected readonly errorMessage = signal('');

  async loadTasks(): Promise<void> {
    const allTasks = await this.repository.getAllTasks();

    const filteredTasks = allTasks.filter((task) => {
      if (this.filter() === 'done') {
        return task.status === TaskStatus.Done;
      }

      if (this.filter() === 'active') {
        return task.status !== TaskStatus.Done && task.status !== TaskStatus.Cancelled;
      }

      return true;
    });

    filteredTasks.sort((left, right) => {
      if (this.sortBy() === 'title') {
        return left.title.localeCompare(right.title);
      }

      const priorityWeight = {
        [Priority.High]: 0,
        [Priority.Medium]: 1,
        [Priority.Low]: 2,
      };

      return priorityWeight[left.priority] - priorityWeight[right.priority];
    });

    this.tasks.set(filteredTasks);
  }

  async completeTask(task: Task): Promise<void> {
    if (task.status === TaskStatus.Cancelled) {
      this.errorMessage.set('Cancelled tasks cannot be completed');
      return;
    }

    task.updateStatus(TaskStatus.Done);
    await this.repository.saveChanges(task);
    await this.loadTasks();
  }
}
```

### 2.2 Identify the Smells

Ask Copilot:

```text
/explain Identify the architecture and maintainability problems in this Angular component. Focus on business rules, sorting/filtering logic, and repository usage.
```

You want Copilot to call out issues like:

- business rules living directly in the component
- direct repository dependency in the presentation layer
- filtering/sorting logic mixed with UI state handling
- harder unit testing because concerns are combined
- duplicated logic risk as more components appear

### 2.3 Refactor to an Injectable Application Service

Now ask Copilot to refactor the code:

```text
/refactor Move the business logic from this Angular component into TaskApplicationService.
Requirements:
- component should depend on TaskApplicationService instead of TaskRepository
- filtering and sorting should move into service methods
- completion rule should stay out of the component
- keep the component focused on UI state and event handling
- preserve observable behavior
```

A good refactor usually produces service methods like:

```ts
async listVisibleTasks(
  filter: 'all' | 'active' | 'done',
  sortBy: 'priority' | 'title',
): Promise<Task[]> {
  const tasks = await this.taskRepository.getAllTasks();

  return tasks
    .filter((task) => {
      if (filter === 'done') {
        return task.status === TaskStatus.Done;
      }

      if (filter === 'active') {
        return task.status !== TaskStatus.Done && task.status !== TaskStatus.Cancelled;
      }

      return true;
    })
    .sort((left, right) => {
      if (sortBy === 'title') {
        return left.title.localeCompare(right.title);
      }

      return this.priorityWeight(left.priority) - this.priorityWeight(right.priority);
    });
}

private priorityWeight(priority: Priority): number {
  switch (priority) {
    case Priority.High:
      return 0;
    case Priority.Medium:
      return 1;
    case Priority.Low:
      return 2;
  }
}
```

And the component becomes thinner:

```ts
export class TaskBoardComponent {
  private readonly taskApplicationService = inject(TaskApplicationService);

  protected readonly tasks = signal<Task[]>([]);
  protected readonly filter = signal<'all' | 'active' | 'done'>('active');
  protected readonly sortBy = signal<'priority' | 'title'>('priority');
  protected readonly errorMessage = signal('');

  async loadTasks(): Promise<void> {
    this.tasks.set(
      await this.taskApplicationService.listVisibleTasks(this.filter(), this.sortBy()),
    );
  }

  async completeTask(task: Task): Promise<void> {
    try {
      await this.taskApplicationService.completeTask(task);
      await this.loadTasks();
    } catch (error) {
      this.errorMessage.set(error instanceof Error ? error.message : 'Unable to complete task');
    }
  }
}
```

### 2.4 Add Regression Tests

Use `/tests` on the new service methods so the refactor is protected.

Suggested prompt:

```text
/tests for TaskApplicationService.listVisibleTasks and completeTask including filtering, priority sorting, and cancelled-task validation
```

Then rerun the suite.

---

## Part 3: Apply Angular-Friendly Clean Code Checks (10 minutes)

This section mirrors the intent of the shared Lab 3 refactoring guidance, but adapts it to TypeScript and Angular.

### 3.1 Prefer Strong Types Over Stringly-Typed Logic

Ask Copilot:

```text
Review the Angular task manager for places where a string should be an enum, value type, or typed union instead.
```

Good examples:

- `TaskStatus` enum instead of status strings
- `Priority` enum instead of numeric magic values
- `TaskId` value object instead of raw strings throughout the domain layer
- typed unions like `'all' | 'active' | 'done'` for UI filter state

### 3.2 Keep Components Thin

A good rule for this track:

- components gather user input
- services orchestrate use cases
- domain objects protect invariants
- repositories manage storage details

If a component starts doing validation, sorting, lifecycle decisions, or storage lookups directly, it is a refactoring candidate.

### 3.3 Use Copilot Edits for Cross-File Consistency

Good Angular examples for Copilot Edits:

- renaming `selectedPriority` to `priorityFilter`
- replacing repeated literal unions with a shared type alias
- updating service method signatures and all call sites together

Example request:

```text
Rename listTasks(filter) to listTasksByView(filter) across the Angular task-manager app and update all references consistently.
```

Always review every diff before accepting it.

---

## Key Learning Points

### ✅ Why the Refactor Matters

1. **Angular components stay easier to test** when they do less
2. **Business rules become reusable** across more than one component
3. **Application services become the use-case boundary** for the SPA
4. **Copilot is better with clear intent** when you point it at the right layer and file context

### ✅ What Changed Technically

- repository contract expanded intentionally
- service surface area became closer to true CRUD
- filtering/sorting moved out of the component
- architectural boundaries became clearer

---

## Extension Exercises (If Time Permits)

### Exercise 1: Add a Search View

Extend `listVisibleTasks()` to support a `searchText` parameter and generate tests first.

### Exercise 2: Add Soft Delete Instead of Hard Delete

Replace `removeTask()` with a domain rule that marks tasks `Cancelled`, then update the UI accordingly.

### Exercise 3: Add an Edit Form

Use Copilot to build a small edit workflow that calls `updateTask()` without moving rules back into the component.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ `TaskApplicationService` has update, delete, and richer list behavior
- ✅ `TaskRepository` and `InMemoryTaskRepository` were extended intentionally to support the new use cases
- ✅ business logic is no longer inlined inside the example component
- ✅ the refactor moves repository usage back behind the application service boundary
- ✅ filtering, sorting, and completion rules are covered by Vitest
- ✅ `npm test -- --watch=false` and `npx ng build` still pass

---

## Troubleshooting

### Copilot Keeps Logic in the Component

**Problem**: the refactor still leaves filtering or validation in the UI  
**Solution**: restate the goal explicitly: _"Move business rules and repository usage into TaskApplicationService. Leave only UI state and event handling in the component."_

### The Repository Contract Feels Too Generic

**Problem**: Copilot proposes raw CRUD names everywhere  
**Solution**: keep business-intent names where practical (`getActiveTasks`, `saveChanges`, `removeTask`) and add only what the use case truly needs.

### Tests Became Brittle After Refactoring

**Problem**: the tests over-couple to implementation details  
**Solution**: assert behavior and public outcomes, not helper-method names or private implementation steps.

### Build Fails After Interface Changes

**Problem**: TypeScript reports missing members on `TaskRepository`  
**Solution**: update both the interface and every fake/mock used in specs to match the new contract.

---

## Next Steps

Continue to [**Lab 4: Testing, Documentation & Workflow (Angular)**](lab-04-testing-documentation-workflow-angular.md), where you'll use `/tests`, `/doc`, Conventional Commits, and a PR template to wrap up the Angular track cleanly.

---

## Additional Resources

- [Angular Style Guide](https://angular.dev/style-guide)
- [Refactoring Guru](https://refactoring.guru/refactoring)
- [Angular Dependency Injection Guide](https://angular.dev/guide/di)
- [Shared .NET walkthrough](lab-03-generation-and-refactoring.md)
- [Java/Spring Boot walkthrough](lab-03-generation-and-refactoring-java.md)
