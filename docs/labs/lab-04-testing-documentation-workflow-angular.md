# Lab 4: Testing, Documentation & Workflow with GitHub Copilot (Angular)

> **💡 Also available**: [.NET version](lab-04-testing-documentation-workflow.md) · [Java/Spring Boot version](lab-04-testing-documentation-workflow-java.md) · [Kotlin version](lab-04-testing-documentation-workflow-kotlin.md) · [Swift version](lab-04-testing-documentation-workflow-swift.md)

**Duration**: 15-20 minutes  
**Learning Objectives**:

- Use `/tests` to expand Vitest coverage quickly and safely
- Generate TSDoc comments with `/doc` for Angular and TypeScript code
- Document the Angular SPA workflow clearly for future contributors
- Write Conventional Commit messages for Angular feature and refactor work
- Draft a useful PR description with `@workspace`

---

## Overview

This lab focuses on the finishing work that often gets rushed:

1. **Testing** - round out unit and component coverage
2. **Documentation** - add TSDoc and contributor-facing notes
3. **Version Control** - write meaningful commits
4. **Pull Requests** - summarize work clearly for reviewers

For the Angular track, that means documenting a **standalone Angular SPA with an in-memory repository** rather than a server-side API.

---

## Prerequisites

- ✅ Completed [Labs 1-3 (Angular)](lab-03-generation-and-refactoring-angular.md)
- ✅ The Angular app builds and tests successfully
- ✅ Git is initialized and you have at least one commit on your branch
- ✅ You know how to open Copilot Chat, Inline Chat, and use slash commands

---

## Part 1: Generate Comprehensive Vitest Coverage (5-6 minutes)

### Scenario: Move Beyond the Starter Tests

By now you should have more than the original `task.spec.ts` and `app.spec.ts`. This is the perfect moment to ask Copilot for broader coverage instead of hand-writing every case from scratch.

### 1.1 Use /tests on the Application Service

Open `src-angular/task-manager/src/app/application/task-application.service.ts`, select `updateTask()` or `createTask()`, and run:

```text
/tests
```

A strong result should cover behavior such as:

- creating a task calls `addTask`
- updating a task loads by id, updates details, updates priority, and saves
- deleting a task calls `removeTask`
- missing tasks produce a clear error
- list filtering delegates to the correct repository method

A representative service test looks like this:

```ts
import { TestBed } from '@angular/core/testing';
import { beforeEach, describe, expect, it, vi } from 'vitest';
import { TaskApplicationService } from './task-application.service';
import { Priority } from '../domain/priority';
import { TASK_REPOSITORY } from '../domain/task-repository';

describe('TaskApplicationService', () => {
  const repository = {
    findById: vi.fn(),
    getActiveTasks: vi.fn(),
    getAllTasks: vi.fn(),
    addTask: vi.fn(),
    saveChanges: vi.fn(),
    removeTask: vi.fn(),
  };

  let service: TaskApplicationService;

  beforeEach(() => {
    Object.values(repository).forEach((member) => member.mockReset());

    TestBed.configureTestingModule({
      providers: [
        TaskApplicationService,
        { provide: TASK_REPOSITORY, useValue: repository },
      ],
    });

    service = TestBed.inject(TaskApplicationService);
  });

  it('creates a task and stores it in the repository', async () => {
    const task = await service.createTask('Plan demo', 'Prepare Angular lab', Priority.High);

    expect(task.priority).toBe(Priority.High);
    expect(repository.addTask).toHaveBeenCalledOnce();
  });
});
```

### 1.2 Use /tests on the Component

Now open `src-angular/task-manager/src/app/features/tasks/task-list.ts` and ask Copilot for component tests:

```text
/tests for TaskListComponent using Angular TestBed and a mocked TaskApplicationService. Cover ngOnInit loading, createTask success, and createTask failure.
```

Good component tests should verify outcomes like:

- `ngOnInit()` loads active tasks
- `createTask()` calls the service with the current form state
- a successful create clears the form and prepends the task
- a rejected promise surfaces an error message

### 1.3 Run the Angular Validation Commands

```bash
cd src-angular/task-manager
npm test -- --watch=false
npx ng build
```

If both pass, you now have confidence across domain, service, repository, and component layers.

---

## Part 2: Generate TSDoc and Contributor Documentation (4-5 minutes)

### Scenario: Make the Angular Track Self-Explaining

### 2.1 Use /doc on the Domain and Application Layers

Good Angular candidates for `/doc`:

- `src-angular/task-manager/src/app/domain/task.ts`
- `src-angular/task-manager/src/app/domain/task-id.ts`
- `src-angular/task-manager/src/app/application/task-application.service.ts`

Example Copilot usage in Inline Chat:

```text
/doc
```

A strong TSDoc result for `TaskApplicationService` should look something like:

```ts
/**
 * Application service that coordinates task-related use cases for the Angular workshop track.
 *
 * This service keeps orchestration, validation flow, and repository coordination out of
 * standalone components so the presentation layer remains focused on UI behavior.
 */
@Injectable({ providedIn: 'root' })
export class TaskApplicationService {
  /**
   * Creates a new task and persists it through the configured repository.
   *
   * @param title Human-readable task title entered by the user.
   * @param description Optional details that provide more context.
   * @param priority Relative importance selected in the UI.
   * @returns The newly created task aggregate.
   */
  async createTask(title: string, description: string, priority: Priority): Promise<Task> {
    // ...
  }
}
```

### 2.2 Update the Angular Track README

Unlike the server-side tracks, the Angular track benefits more from **feature and workflow documentation** than from endpoint docs.

Open `src-angular/task-manager/README.md` and ask Copilot:

```text
Create a README section for the Angular task manager that explains:
- this app is a standalone Angular SPA with no backend
- the Domain, Application, Data, and Features folders
- how to run build and test commands
- where the in-memory repository is implemented
Format as concise Markdown.
```

A helpful addition might look like:

````md
## Workshop Architecture

This task manager is a standalone Angular SPA used for the workshop's Angular lab track.
It has no backend dependency; data is stored in memory through `InMemoryTaskRepository`.

### Layers

- `src/app/domain/` - task aggregate, ids, status, priority, repository contract
- `src/app/application/` - use-case orchestration via `TaskApplicationService`
- `src/app/data/` - in-memory repository implementation
- `src/app/features/tasks/` - standalone presentation components

### Common Commands

```bash
npm test -- --watch=false
npx ng build
npm start
```
````

Review the generated Markdown for accuracy before accepting it.

---

## Part 3: Write Conventional Commit Messages (3 minutes)

### 3.1 Stage Related Changes Together

Examples:

```bash
git add src-angular/task-manager/src/app/domain/
git add src-angular/task-manager/src/app/application/
git add src-angular/task-manager/src/app/data/
git add src-angular/task-manager/src/app/features/tasks/
```

### 3.2 Ask Copilot for a Feature Commit

```text
Write a Conventional Commit message for the Angular track changes that added Priority, implemented the in-memory repository, and wired the create-task flow.
Include a short subject and a helpful body.
```

**Expected Output**:

```text
feat(angular): add in-memory task creation workflow

- add Priority support to the Task aggregate
- implement InMemoryTaskRepository for Angular labs
- update TaskApplicationService to create prioritized tasks
- wire TaskListComponent form to the application service
- add Vitest coverage for domain, repository, service, and component
```

### 3.3 Ask Copilot for a Refactor Commit

```text
Write a Conventional Commit message for the Angular refactor that moved business logic out of a component and into TaskApplicationService.
```

**Expected Output**:

```text
refactor(angular): move task rules from component into service

- remove repository usage from the presentation layer
- centralize filtering, sorting, and completion rules
- keep standalone components focused on UI state and events
- add regression tests for service-driven behavior
```

---

## Part 4: Draft a Pull Request Description (4-5 minutes)

### 4.1 Use @workspace for Full Context

Ask Copilot Chat:

```text
@workspace Draft a pull request description for the Angular task manager labs. Include:
- summary of the user-facing behavior
- architectural changes across domain/application/data/features
- tests performed
- any follow-up ideas
- a reviewer checklist
Use Markdown.
```

A strong PR description will usually include sections like:

- Summary
- Changes
- Testing Performed
- Reviewer Checklist
- Future Work

### 4.2 What a Good Angular PR Summary Sounds Like

````md
## Summary

This PR completes the Angular task manager workshop flow by adding priority-aware task creation,
implementing the in-memory repository, expanding the application service, and keeping business
logic out of standalone components.

## Testing Performed

```bash
npm test -- --watch=false
npx ng build
```

## Reviewer Checklist

- [ ] Components remain thin and delegate use cases to TaskApplicationService
- [ ] Domain rules stay in Task, not templates
- [ ] Repository behavior is covered by Vitest
- [ ] No backend dependencies were introduced
````

Review and refine it before posting.

---

## Key Learning Points

### ✅ Testing Workflow

1. `/tests` is fastest when you give it a specific file or selected method
2. generated tests still need review for clarity and correctness
3. Angular component tests should assert UI behavior, not private implementation details

### ✅ Documentation Workflow

1. `/doc` works well for TSDoc on public classes and methods
2. workshop README content should explain architecture and commands, not just generated Angular CLI defaults
3. concise documentation is more useful than long generic comments

### ✅ Collaboration Workflow

1. Conventional Commits make workshop history easier to read
2. PR descriptions are better when `@workspace` can see the whole change set
3. a reviewer checklist helps keep architecture boundaries intact

---

## Extension Exercises (If Time Permits)

### Exercise 1: Add Coverage for Sorting or Filtering

Use `/tests` on `listVisibleTasks()` and ask for priority-sorting and filter-edge-case coverage.

### Exercise 2: Add TSDoc to the Priority Enum and Repository Contract

Use `/doc` on `priority.ts` and `task-repository.ts`, then edit the comments to keep them concise.

### Exercise 3: Draft a Release Note

Ask Copilot to summarize the Angular track additions in 5 bullet points suitable for workshop release notes.

---

## Success Criteria

You've completed this lab successfully when:

- ✅ Vitest coverage exists for the important Angular use cases
- ✅ public TypeScript classes and methods have helpful TSDoc comments
- ✅ the Angular track README explains the SPA architecture and commands clearly
- ✅ commit messages follow Conventional Commits
- ✅ you have a solid PR description generated from `@workspace`
- ✅ `npm test -- --watch=false` and `npx ng build` both succeed

---

## Troubleshooting

### /tests Generated Weak Assertions

**Problem**: Copilot produced tests that only check truthiness  
**Solution**: ask for specific behaviors, error messages, and repository interactions.

### /doc Added Generic Noise

**Problem**: the TSDoc repeats the method name without adding meaning  
**Solution**: rewrite comments to explain intent, constraints, and why the code exists.

### Commit Message Is Too Vague

**Problem**: Copilot suggests `update angular files`  
**Solution**: stage a smaller logical change set and tell Copilot exactly what changed.

### PR Description Mentions a Backend

**Problem**: Copilot drifted toward the .NET or Java tracks  
**Solution**: restate that this is a **standalone Angular SPA with an in-memory repository and no server**.

---

## Workshop Wrap-Up

After Lab 4, the Angular track should leave participants with a complete workflow:

- TDD against domain rules
- requirements-to-code across the SPA stack
- generation + refactoring with architectural boundaries intact
- testing, documentation, commit hygiene, and PR preparation

That is the same workshop arc as the .NET and Spring Boot tracks, translated into Angular-idiomatic practices.

---

## Additional Resources

- [Angular Testing](https://angular.dev/guide/testing)
- [TypeScript TSDoc](https://tsdoc.org/)
- [Conventional Commits](https://www.conventionalcommits.org/)
- [Vitest Guide](https://vitest.dev/guide/)
- [Shared .NET walkthrough](lab-04-testing-documentation-workflow.md)
- [Java/Spring Boot walkthrough](lab-04-testing-documentation-workflow-java.md)
