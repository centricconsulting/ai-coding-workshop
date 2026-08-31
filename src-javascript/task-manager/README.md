# JavaScript Task Manager (Non-Technical Track)

A tiny, dependency-free JavaScript version of the workshop's Task Manager exercise. No
framework, no build step, no TypeScript — just plain JavaScript functions you run directly with
Node.js.

## Prerequisites

- Node.js (any current LTS release) — verify with `node --version`

## Running the tests

```bash
node --test
```

This uses Node's built-in [`node:test`](https://nodejs.org/api/test.html) module, so there is
nothing to install.

## Files

- `task.js` — the `createTask`, `updateStatus`, and `updateDetails` functions. Several `TODO`
  comments are left in on purpose — implementing them (with Copilot's help) is the point of
  Lab 1.
- `task.test.js` — the starter test suite.

## Where this fits in the workshop

See [`docs/labs/lab-01-tdd-with-copilot-javascript.md`](../../docs/labs/lab-01-tdd-with-copilot-javascript.md)
and the other JavaScript-track lab docs in `docs/labs/`.
