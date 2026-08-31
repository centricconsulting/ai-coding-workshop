# Plan: Plain JavaScript Track (Non-Technical Audience)

## Summary

A fifth, separate track alongside Angular — **plain JavaScript, no framework, no build tooling,
no TypeScript** — aimed at a genuinely non-technical audience (similar profile to Lab 0's
BA/PM/non-engineering participants), not developers who simply don't know Angular.

This is **not** a Clean Architecture port like the other four tracks. Replicating
Domain/Application/Infrastructure layering would add cognitive overhead unrelated to the
learning goal for this audience. Scope is intentionally lighter.

## Stack

- Plain JavaScript (ES2022), no framework, no TypeScript, no bundler
- **Runtime**: Node.js (any current LTS) — code runs directly with `node`, no build step
- **Testing**: Node's built-in `node:test` module + `assert` — zero dependencies to install,
  which fits both the "no extra tooling" goal for a non-technical audience and this repo's
  general preference for minimal dependencies

## Proposed Structure

```
src-javascript/
  task-manager/
    task.js         # Task-related functions (create, updateStatus, updateDetails) — plain
                     # functions and object literals, not classes/layers
    task.test.js     # node:test suite
    README.md        # how to run: `node --test`
```

No Domain/Application/Data folder split — everything related to the Task concept lives in one
file to keep the mental model simple. The same TDD red-green-refactor arc from the other tracks
still applies, just without architectural ceremony.

## Labs to Port (simplified versions of Labs 1–4)

- `lab-01-tdd-with-copilot-javascript.md` — write a failing `node:test` test for a `createTask`
  function first (RED), implement it (GREEN), then refactor with Copilot's help. Emphasis on
  explaining *why* tests come first in plain language, more so than the developer-focused tracks.
- `lab-02-requirements-to-code-javascript.md` — turn a plain-language user story into a small
  set of functions (e.g., add a `priority` field with validation), with tests. Kept to a single
  file, no layering.
- `lab-03-generation-and-refactoring-javascript.md` — refactor exercise should target a beginner
  -friendly smell: a function that does too many things at once (e.g., validates input, updates
  state, and formats output all in one function) refactored into smaller, named functions —
  a low-jargon parallel to the other tracks' Object Calisthenics/DDD-flavored refactors.
- `lab-04-testing-documentation-workflow-javascript.md` — generating tests with Copilot's
  `/tests`, plain-language code comments (not TSDoc/JSDoc formality), and a simple Conventional
  Commit + PR description.

## Devcontainer / Setup

- New `.devcontainer/javascript-participant/` (Node 22 LTS only — no Angular CLI, no TypeScript
  compiler, minimal image). **Verified**: `node --test` passes inside the actual
  `mcr.microsoft.com/devcontainers/javascript-node:22` image.
- Full local setup: JavaScript section of `docs/LOCAL_SETUP.md` (merged into the shared doc
  alongside .NET/Spring Boot/Angular, rather than a standalone file). **Policy** (same as the
  other four new tracks): no lightweight "Copilot-only" minimal-setup alternative — participants
  use the devcontainer or match the local setup doc's spec exactly. In practice this track's
  local setup is already minimal (just Node + VS Code + Copilot), so the devcontainer and local
  paths are nearly identical in complexity.

## Estimate

**1–2 days** — smaller in scope than the other four tracks (no layering, no framework), but lab
docs still need care to keep language approachable for a non-technical audience.

## Open Questions

- Confirm the audience is exactly Lab 0's profile (BA/PM/non-engineering) so lab language/pacing
  can be calibrated the same way Lab 0 was written.
- Should this track's Lab 1 assume participants have already completed Lab 0 (terminal/VS
  Code/Git basics), or does it need to re-teach those basics inline? Recommend assuming Lab 0 is
  a prerequisite, consistent with how Lab 0 is positioned for other tracks.
