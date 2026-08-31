# Lab 2: From Requirements to Code with GitHub Copilot (Angular)

> **💡 Also available**: [.NET version](lab-02-requirements-to-code.md) · [Java/Spring Boot version](lab-02-requirements-to-code-java.md)

**Duration**: 45-50 minutes  
**Learning Objectives**:

- Turn a vague feature request into concrete Angular implementation work
- Add a small but meaningful domain concept (`Priority`) using TDD
- Implement the in-memory repository used by the Angular track
- Wire an end-to-end create-task flow inside the SPA
- Generate and review Vitest coverage across domain, application, data, and UI layers

---

## Overview

In the Angular track, "full stack" means **Domain → Application → Data → Presentation** inside the SPA.

There is no backend server in this lab. Instead, you'll complete the missing in-memory plumbing and deliver a real feature that users can exercise in the browser.

The scaffold already includes these files:

- `src-angular/task-manager/src/app/domain/task.ts`
- `src-angular/task-manager/src/app/domain/task-repository.ts`
- `src-angular/task-manager/src/app/data/in-memory-task-repository.ts`
- `src-angular/task-manager/src/app/application/task-application.service.ts`
- `src-angular/task-manager/src/app/features/tasks/task-list.ts`
- `src-angular/task-manager/src/app/features/tasks/task-list.html`

In this lab you'll add a **Priority** concept, implement the repository, then wire a create flow from the component to the application service and back to the view.

---

## Prerequisites

- ✅ Completed [Lab 1 (Angular)](lab-01-tdd-with-copilot-angular.md) or already understand Red-Green-Refactor
- ✅ Angular track dependencies installed
- ✅ Baseline commands work:

```bash
cd src-angular/task-manager
npx ng build
npx ng test --watch=false
```

- ✅ Repository is in a clean state: `git status`

---

## Part 1: Analyze the Requirement (10 minutes)

### Scenario: Make Task Creation Useful in the Angular SPA

Your product owner says:

> **User Story**: As a workshop participant, I want to create tasks with a priority so I can track what matters most without leaving the Angular application.

That is enough to start, but not enough to code directly.

### 1.1 Generate Backlog Items

Ask Copilot Chat:

```text
I have this user story for our Angular track:
"As a workshop participant, I want to create tasks with a priority so I can track what matters most without leaving the Angular application."

Generate 5 backlog items with acceptance criteria for the existing Angular standalone-component SPA.
Assume there is no backend and data stays in an in-memory repository.
```

A useful answer should break the story into items like:

1. Add a `Priority` domain type
2. Extend the `Task` aggregate to store priority
3. Implement `InMemoryTaskRepository`
4. Update `TaskApplicationService` to create prioritized tasks
5. Add a create-task form to `TaskListComponent`

### 1.2 Choose a Thin Slice

For this workshop, implement a thin but complete slice:

- Add `Priority`
- Update `Task`
- Implement the repository
- Wire `TaskApplicationService.createTask()`
- Add a form and display priority in the component
- Cover the feature with Vitest specs

---

## Part 2: Add the Priority Domain Concept (RED → GREEN) (12 minutes)

### 2.1 Create the New Domain Type

Ask Copilot:

```text
Create src-angular/task-manager/src/app/domain/priority.ts as a simple TypeScript enum for the Angular task manager. Use values Low, Medium, and High.
```

**Expected Output**:

```ts
export enum Priority {
  Low = 'Low',
  Medium = 'Medium',
  High = 'High',
}
```

### 2.2 Update Task Specs First

Now extend `src-angular/task-manager/src/app/domain/task.spec.ts` before changing the implementation:

```text
Update src-angular/task-manager/src/app/domain/task.spec.ts for the new Priority concept.
Add tests that verify:
- Task.create stores the provided priority
- creating a task without a priority throws an error
- updatePriority changes the priority and updates updatedAt
Keep the existing Vitest style.
```

A likely spec addition:

```ts
import { Priority } from './priority';

describe('create', () => {
  it('should create a task with priority', () => {
    const task = Task.create('Plan demo', 'Prepare Angular walkthrough', Priority.High);

    expect(task.priority).toBe(Priority.High);
  });

  it('should throw when priority is missing', () => {
    expect(() => Task.create('Plan demo', 'Prepare Angular walkthrough', undefined as never))
      .toThrowError('Task priority is required');
  });
});

describe('updatePriority', () => {
  it('should update the priority and updatedAt timestamp', () => {
    const task = Task.create('Plan demo', 'Prepare Angular walkthrough', Priority.Low);
    const originalUpdatedAt = task.updatedAt;

    task.updatePriority(Priority.High);

    expect(task.priority).toBe(Priority.High);
    expect(task.updatedAt.getTime()).toBeGreaterThanOrEqual(originalUpdatedAt.getTime());
  });
});
```

### 2.3 Update the Task Aggregate

Now prompt Copilot to update `src-angular/task-manager/src/app/domain/task.ts`:

```text
Update src-angular/task-manager/src/app/domain/task.ts to support Priority.
Requirements:
- add a readonly priority getter backed by a private field
- update the constructor and Task.create factory to require Priority
- add updatePriority(newPriority: Priority)
- validate that priority is provided
Keep the existing class style and reuse the validation approach from Lab 1.
```

**Expected Shape**:

```ts
import { Priority } from './priority';

export class Task {
  private _priority: Priority;

  private constructor(
    id: TaskId,
    title: string,
    description: string,
    priority: Priority,
    status: TaskStatus,
    createdAt: Date,
  ) {
    this.id = id;
    this._title = title;
    this._description = description;
    this._priority = priority;
    this._status = status;
    this.createdAt = createdAt;
    this._updatedAt = createdAt;
  }

  get priority(): Priority {
    return this._priority;
  }

  static create(title: string, description: string, priority: Priority): Task {
    if (!priority) {
      throw new Error('Task priority is required');
    }

    return new Task(
      TaskId.new(),
      Task.validateTitle(title),
      Task.validateDescription(description),
      priority,
      TaskStatus.Todo,
      new Date(),
    );
  }

  updatePriority(newPriority: Priority): void {
    if (!newPriority) {
      throw new Error('Task priority is required');
    }

    this._priority = newPriority;
    this._updatedAt = new Date();
  }
}
```

### 2.4 Re-run the Tests

```bash
cd src-angular/task-manager
npm test -- --watch=false
```

At this point your domain tests should be green again.

---

## Part 3: Implement the In-Memory Repository (RED → GREEN) (12 minutes)

### 3.1 Write Repository Tests First

Create a new spec file:

- `src-angular/task-manager/src/app/data/in-memory-task-repository.spec.ts`

Prompt Copilot:

```text
Create Vitest tests for src-angular/task-manager/src/app/data/in-memory-task-repository.ts.
Cover:
- addTask stores a task by id
- findById returns the stored task
- getActiveTasks excludes Done and Cancelled tasks
- getActiveTasks sorts by updatedAt descending
- saveChanges updates an existing task
- saveChanges throws if the task was never added
Use the real Task aggregate and Priority enum.
```

A representative test looks like this:

```ts
import { describe, expect, it } from 'vitest';
import { InMemoryTaskRepository } from './in-memory-task-repository';
import { Priority } from '../domain/priority';
import { Task } from '../domain/task';
import { TaskStatus } from '../domain/task-status';

describe('InMemoryTaskRepository', () => {
  it('should omit done and cancelled tasks from getActiveTasks', async () => {
    const repository = new InMemoryTaskRepository();
    const todoTask = Task.create('Todo', 'Visible task', Priority.Medium);
    const doneTask = Task.create('Done', 'Hidden task', Priority.Low);
    doneTask.updateStatus(TaskStatus.Done);

    await repository.addTask(todoTask);
    await repository.addTask(doneTask);

    const activeTasks = await repository.getActiveTasks();

    expect(activeTasks).toEqual([todoTask]);
  });
});
```

### 3.2 Implement the Repository

Now update `src-angular/task-manager/src/app/data/in-memory-task-repository.ts`:

```text
Implement InMemoryTaskRepository so it satisfies the TaskRepository contract.
Use the internal Map for storage.
Behavior:
- findById returns a task or undefined
- getActiveTasks returns tasks not in Done or Cancelled status, sorted by updatedAt descending
- addTask stores the task by id
- saveChanges replaces an existing task and throws if the id does not exist
```

**Expected Implementation**:

```ts
@Injectable({ providedIn: 'root' })
export class InMemoryTaskRepository implements TaskRepository {
  private readonly tasks = new Map<string, Task>();

  async findById(taskId: TaskId): Promise<Task | undefined> {
    return this.tasks.get(taskId.value);
  }

  async getActiveTasks(): Promise<Task[]> {
    return [...this.tasks.values()]
      .filter((task) => task.status !== TaskStatus.Done && task.status !== TaskStatus.Cancelled)
      .sort((left, right) => right.updatedAt.getTime() - left.updatedAt.getTime());
  }

  async addTask(task: Task): Promise<void> {
    this.tasks.set(task.id.value, task);
  }

  async saveChanges(task: Task): Promise<void> {
    if (!this.tasks.has(task.id.value)) {
      throw new Error(`Task ${task.id.value} does not exist in the repository`);
    }

    this.tasks.set(task.id.value, task);
  }
}
```

### 3.3 Run the Suite

```bash
cd src-angular/task-manager
npm test -- --watch=false
```

Your domain and data-layer tests should now pass.

---

## Part 4: Wire the Application Service and Component (RED → GREEN) (14 minutes)

### 4.1 Update the Application Service

First add a focused spec:

- `src-angular/task-manager/src/app/application/task-application.service.spec.ts`

Prompt Copilot:

```text
Create Vitest tests for TaskApplicationService.
Cover:
- createTask creates a Task with the requested priority and calls addTask
- listActiveTasks returns repository results
- completeTask updates the task status to Done and calls saveChanges
Mock the repository through the TASK_REPOSITORY injection token.
```

Then update the service itself:

```text
Update src-angular/task-manager/src/app/application/task-application.service.ts so createTask accepts Priority.
Keep listActiveTasks and completeTask behavior intact.
```

**Expected Service Update**:

```ts
import { Priority } from '../domain/priority';

async createTask(title: string, description: string, priority: Priority): Promise<Task> {
  const task = Task.create(title, description, priority);
  await this.taskRepository.addTask(task);
  return task;
}
```

### 4.2 Add a Thin Create Flow in the Component

Now update the presentation layer.

Ask Copilot:

```text
Update TaskListComponent and task-list.html to support creating tasks.
Requirements:
- keep business logic in TaskApplicationService
- use signals for component state
- capture title, description, and priority in the component
- call taskApplicationService.createTask(...)
- prepend the created task to the visible list
- show a simple error message if creation fails
Use the existing standalone component style and avoid adding a backend dependency.
```

A clean component shape looks like this:

```ts
import { Component, OnInit, inject, signal } from '@angular/core';
import { TaskApplicationService } from '../../application/task-application.service';
import { Priority } from '../../domain/priority';
import { Task } from '../../domain/task';

@Component({
  selector: 'app-task-list',
  standalone: true,
  templateUrl: './task-list.html',
  styleUrl: './task-list.css',
})
export class TaskListComponent implements OnInit {
  private readonly taskApplicationService = inject(TaskApplicationService);

  protected readonly tasks = signal<Task[]>([]);
  protected readonly title = signal('');
  protected readonly description = signal('');
  protected readonly selectedPriority = signal(Priority.Medium);
  protected readonly priorities = Object.values(Priority);
  protected readonly errorMessage = signal('');

  async ngOnInit(): Promise<void> {
    this.tasks.set(await this.taskApplicationService.listActiveTasks());
  }

  async createTask(): Promise<void> {
    this.errorMessage.set('');

    try {
      const task = await this.taskApplicationService.createTask(
        this.title(),
        this.description(),
        this.selectedPriority(),
      );

      this.tasks.update((tasks) => [task, ...tasks]);
      this.title.set('');
      this.description.set('');
      this.selectedPriority.set(Priority.Medium);
    } catch (error) {
      this.errorMessage.set(error instanceof Error ? error.message : 'Unable to create task');
    }
  }

  protected updateTitle(event: Event): void {
    this.title.set((event.target as HTMLInputElement).value);
  }

  protected updateDescription(event: Event): void {
    this.description.set((event.target as HTMLTextAreaElement).value);
  }

  protected updatePriority(event: Event): void {
    this.selectedPriority.set((event.target as HTMLSelectElement).value as Priority);
  }
}
```

And the template can stay simple and explicit:

```html
<section class="task-list">
  <h1>Task Manager</h1>

  <form class="task-form" (submit)="createTask(); $event.preventDefault()">
    <label>
      Title
      <input [value]="title()" (input)="updateTitle($event)" />
    </label>

    <label>
      Description
      <textarea [value]="description()" (input)="updateDescription($event)"></textarea>
    </label>

    <label>
      Priority
      <select [value]="selectedPriority()" (change)="updatePriority($event)">
        @for (priority of priorities; track priority) {
          <option [value]="priority">{{ priority }}</option>
        }
      </select>
    </label>

    <button type="submit">Create task</button>
  </form>

  @if (errorMessage()) {
    <p class="error">{{ errorMessage() }}</p>
  }

  <ul>
    @for (task of tasks(); track task.id.value) {
      <li>{{ task.title }} — {{ task.priority }} — {{ task.status }}</li>
    } @empty {
      <li>No active tasks.</li>
    }
  </ul>
</section>
```

### 4.3 Add Component Coverage

Create `src-angular/task-manager/src/app/features/tasks/task-list.spec.ts` and ask Copilot:

```text
Create Vitest tests for TaskListComponent that verify:
- active tasks load on ngOnInit
- createTask calls TaskApplicationService with title, description, and priority
- successful creation prepends the task to the list
- failed creation shows the error message
Use Angular TestBed and a mocked TaskApplicationService.
```

### 4.4 Validate the End-to-End Flow

Run:

```bash
cd src-angular/task-manager
npm test -- --watch=false
npx ng build
```

If you want a manual browser check too:

```bash
cd src-angular/task-manager
npm start
```

Then open `http://localhost:4200/`, create a few tasks, and confirm the list updates immediately.

---

## Key Learning Points

### ✅ What Changed Across the SPA

1. **Domain** - `Task` now understands `Priority`
2. **Data** - the in-memory repository is a real implementation, not a stub
3. **Application** - the service orchestrates creation instead of the component building domain objects directly
4. **Presentation** - the standalone component gathers input and delegates behavior

### ✅ Why This Still Counts as Full-Stack Work

Even without an HTTP API, you still implemented a complete vertical slice:

- business concept
- persistence abstraction
- persistence implementation
- use-case orchestration
- user-facing workflow
- automated tests at multiple layers

---

## Extension Exercises (If Time Permits)

### Exercise 1: Seed Default Tasks

Update `InMemoryTaskRepository` so the workshop starts with two or three sample tasks.

### Exercise 2: Add a Priority Badge Style

Use `task-list.css` to visually distinguish `High`, `Medium`, and `Low` tasks.

### Exercise 3: Sort by Priority First

Update `getActiveTasks()` so `High` items appear before `Medium` and `Low`, then add a test for the new ordering.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ `Priority` exists as a domain enum in `src/app/domain/priority.ts`
- ✅ `Task` stores and updates priority correctly
- ✅ `InMemoryTaskRepository` no longer throws `Not implemented`
- ✅ `TaskApplicationService.createTask()` accepts and persists priority
- ✅ `TaskListComponent` can create tasks through the application service
- ✅ Vitest coverage exists for domain, repository, service, and component behavior
- ✅ `npx ng build` and `npm test -- --watch=false` both succeed

---

## Troubleshooting

### Copilot Tries to Add HTTP Calls

**Problem**: the generated code starts using `HttpClient` or `/api/tasks`  
**Solution**: remind Copilot that the Angular track is **self-contained with an in-memory repository and no backend**.

### Injection Token Errors in Tests

**Problem**: `TASK_REPOSITORY` is not provided in TestBed  
**Solution**: register a fake repository in the spec's `providers` array.

### Template Type Errors

**Problem**: event target casts fail during build  
**Solution**: keep small helper methods like `updateTitle(event: Event)` in the component rather than embedding complex casts in the template.

### Component Test Is Flaky

**Problem**: assertions run before async state settles  
**Solution**: await `fixture.whenStable()` after triggering component async work.

---

## Next Steps

Continue to [**Lab 3: Code Generation & Refactoring (Angular)**](lab-03-generation-and-refactoring-angular.md), where you'll expand the application service, add the rest of the CRUD behavior, and refactor business logic out of a component.

---

## Additional Resources

- [Angular Signals Guide](https://angular.dev/guide/signals)
- [Angular Components Guide](https://angular.dev/guide/components)
- [Vitest Mocking Guide](https://vitest.dev/guide/mocking.html)
- [Shared .NET walkthrough](lab-02-requirements-to-code.md)
- [Java/Spring Boot walkthrough](lab-02-requirements-to-code-java.md)
