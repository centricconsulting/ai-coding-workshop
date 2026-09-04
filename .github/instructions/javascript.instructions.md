---
applyTo: 'src-javascript/**'
---

# GitHub Copilot Instructions for Plain JavaScript Workshop Track

> These instructions are automatically applied to all GitHub Copilot suggestions when working with `src-javascript/`.

## 0) Workshop Mode
- Assume **plain Node.js** (no framework, no bundler), **CommonJS** (`require`/`module.exports`), and the built-in **`node:test`** runner with **`node:assert/strict`**.
- This track is **intentionally layer-free**: there is no Clean Architecture split here. Everything about a concept (e.g., a "Task") lives in one file as plain functions and object literals, to keep the mental model simple for a non-technical/beginner audience.
- Always generate examples and code in **English**.

---

## 1) Workflow (TDD + Build Hygiene)
- **TDD first**: when asked to implement a feature, propose/emit tests before code.
- After you output code, assume we run `node --test` and fix failures before committing.
- Do not introduce classes, modules, DI containers, or a domain/application/infrastructure split — that defeats the purpose of this track. If the user asks for layering, point them at the `src-angular` or `src-dotnet` tracks instead.

---

## 2) Project Structure
- Single flat file per concept (e.g., `task.js`) with a matching `*.test.js` next to it (e.g., `task.test.js`).
- Export plain functions and constants via `module.exports = { ... }`; no classes, no `this`.
- Represent entities as plain object literals (e.g., `{ id, title, description, status, createdAt, updatedAt }`), not classes.

---

## 3) JavaScript Coding Style
- `'use strict';` at the top of every file.
- `camelCase` for functions/variables, `UPPER_SNAKE_CASE` or `SCREAMING_CASE` array/const collections (e.g., `TASK_STATUSES`).
- Use JSDoc comments (`@param`, `@returns`) above exported functions instead of TypeScript types.
- Favor **pure, immutable-style functions**: return a new object (`{ ...task, status: newStatus }`) rather than mutating the input in place.
- Use `crypto.randomUUID()` for IDs; use `new Date()` for timestamps.
- Leave workshop `// TODO:` comments in place when scaffolding starter code for participants to complete during labs — do not silently implement the validation/business rules those TODOs describe unless explicitly asked.

---

## 4) Testing Rules
- **Test framework**: Node's built-in `node:test` + `node:assert/strict`. Do not add Jest, Mocha, or other test frameworks.
- One `test(...)` block per behavior; use a clear, descriptive sentence as the test name (e.g., `'updateStatus returns a copy of the task with the new status'`).
- Prefer `assert.equal`/`assert.ok`/`assert.deepEqual` with a descriptive message as the last argument when the assertion isn't self-explanatory.
- Always assert immutability where relevant (e.g., confirm the original object is unchanged after calling an "update" function).

**Example:**
```js
test('updateStatus returns a copy of the task with the new status', () => {
  const task = createTask('Write report', 'Draft the quarterly report');
  const updated = updateStatus(task, 'InProgress');

  assert.equal(updated.status, 'InProgress');
  assert.equal(task.status, 'Todo', 'original task should be unchanged');
});
```

---

## 5) Conventional Commits
- Use `<type>([optional scope]): <description>` with 72-char subject limit.
- Types: `feat|fix|docs|style|refactor|perf|test|build|ci|chore|revert`.

---

## 6) Documentation Organization
- All documentation lives in `docs/` at the repository root (e.g., `docs/labs/lab-01-tdd-with-copilot-javascript.md`).
- Keep any `src-javascript/task-manager/README.md` focused on setup/run instructions.

---

## 7) Guardrails (Workshop)
- Do **not** add npm dependencies, bundlers, TypeScript, or a test framework beyond `node:test` unless explicitly asked.
- Do **not** introduce classes or a layered architecture — this track's whole point is showing Copilot's help with simple, flat, functional code.
- Keep functions small and side-effect-free where possible; prefer returning new objects over mutation.
