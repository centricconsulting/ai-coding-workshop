---
applyTo: 'src-angular/**'
---

# GitHub Copilot Instructions for Angular Workshop Track

> These instructions are automatically applied to all GitHub Copilot suggestions when working with `src-angular/`.

## 0) Workshop Mode
- Assume **Angular 22 (standalone components, no NgModules)**, **TypeScript**, **signals**, **RxJS 7**, **Vitest** (via `ng test`), and **npm**.
- Prefer **Clean Architecture** folder layout and **DDD** patterns, adapted to a single Angular app project.
- Always generate examples and code in **English**.

---

## 1) Workflow (TDD + Build Hygiene)
- **TDD first**: when asked to implement a feature, propose/emit tests before code.
- After you output code, assume we run `ng test` and `ng build` and fix warnings/errors before committing.
- When referencing rule sets, state what you followed (e.g., "Used: Clean Architecture, DDD, Tests").

---

## 2) Project Architecture (Clean Architecture, single app)
`src-angular/task-manager` is a single Angular application; layer by **folder** instead of by project:
- `src/app/domain/` — entities, value objects, repository interface (port). No Angular imports.
- `src/app/application/` — application services orchestrating use cases (`@Injectable`). Depends on `domain/` only.
- `src/app/data/` — infrastructure adapters (e.g., `InMemoryTaskRepository`, future `HttpTaskRepository`) implementing the domain repository port.
- `src/app/features/<feature>/` — standalone components (presentation layer). Thin views that delegate to `application/` services; no business logic in components.

Enforce dependency direction: `features` → `application` → `domain`; `data` → `domain`. Never let `domain/` import from Angular (`@angular/*`).

---

## 3) TypeScript/Angular Coding Style
- `PascalCase` for classes/components/interfaces, `camelCase` for variables/functions/methods, `UPPER_SNAKE_CASE` for constants and injection tokens' backing values.
- Standalone components only (`standalone: true` or default in Angular 22); no NgModules.
- Use `inject()` function for DI in components; constructor injection with `@Inject(TOKEN)` in services is acceptable (see `TaskApplicationService`).
- Use **signals** (`signal()`, `computed()`) for component state instead of manual change detection or plain fields.
- Keep domain classes framework-free: private constructors + static `create()` factory methods, private backing fields (`_title`) with public getters, no public setters — mutate only through named business methods (`updateStatus`, `updateDetails`).
- Async repository calls return `Promise<T>`; prefer `async/await` over manual `.then()` chains.
- Format with `prettier` (already a devDependency); do not hand-format against its rules.

---

## 4) DDD Modeling Rules
- Model aggregates with a private constructor and a static `create()` factory; encapsulate invariants inside the class.
- Value objects (`TaskId`, `TaskStatus`) are small, immutable classes/enums with no setters.
- Repositories are interfaces in `domain/` (e.g., `TaskRepository`), exposed via an Angular `InjectionToken` (e.g., `TASK_REPOSITORY`) and implemented in `data/`.
- Favor **business-intent method names** (`completeTask`, `updateStatus`) over generic CRUD verbs.

---

## 5) Testing Rules
- **Test runner**: Vitest via `ng test` (jsdom environment).
- Use Angular `TestBed` for component tests; provide fakes/in-memory adapters via the repository injection token instead of real HTTP calls.
- Unit-test `domain/` and `application/` in isolation (no `TestBed` needed there — plain TypeScript classes).
- Name test files `*.spec.ts` colocated with the file under test, matching existing layout (`app.spec.ts` next to `app.ts`).
- Use descriptive `describe`/`it` blocks; assert one behavior per `it`.

**Example — component test with a fake repository:**
```ts
await TestBed.configureTestingModule({
  imports: [App],
  providers: [{ provide: TASK_REPOSITORY, useClass: InMemoryTaskRepository }],
}).compileComponents();
```

---

## 6) Conventional Commits
- Use `<type>([optional scope]): <description>` with 72-char subject limit.
- Types: `feat|fix|docs|style|refactor|perf|test|build|ci|chore|revert`.
- Keep one logical change per commit; use scope to denote layer/feature (e.g., `feat(tasks): add task completion`).

---

## 7) Documentation Organization
- All documentation lives in `docs/` at the repository root (e.g., `docs/labs/lab-01-tdd-with-copilot-angular.md`).
- Keep `src-angular/task-manager/README.md` (if present) focused on setup/run instructions; put deeper guidance in `docs/`.

---

## 8) Guardrails (Workshop)
- Do **not** invent external dependencies (state libraries, UI kits) without being asked.
- Keep domain logic **out of** `features/` components and `data/` adapters.
- Prefer small, focused components and services; avoid deep template logic — push it into `application/` or `domain/`.
- If a rule conflicts, **Clean Architecture boundaries win** (then DDD, then Angular style conventions).
