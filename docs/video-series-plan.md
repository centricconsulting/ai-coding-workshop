# GitHub Copilot for .NET Developers — Video Series Plan

**Audience:** Mixed .NET developers (all experience levels)  
**Format:** Talking head + screen share in VS Code  
**Style:** Concept → Why it matters → Live demo (no lab walkthroughs)  
**Target duration:** 10–15 minutes per video  
**Total runtime:** ~125 minutes across 10 videos  
**Codebase:** [TaskManager](../TaskManager.sln) — Clean Architecture .NET 9 solution

---

## Track 1 — Getting Started

### Video 1: Introduction to AI-Assisted .NET Development
**Duration:** ~10 min

**Topics:**
- The mindset shift from autocomplete to AI pair programming
- What GitHub Copilot is and isn't (and where it fits in the workflow)
- Overview of the three interaction modes (inline, chat, agent)
- Series roadmap — what's covered and how the videos connect

**Demo:** Brief inline completion teaser — show Copilot completing a C# method from a comment

---

### Video 2: Copilot Features Tour + Encoding Team Standards
**Duration:** ~15 min

**Topics:**
- Three interaction modes: inline completions, chat panel, inline chat
- Key slash commands: `/explain`, `/fix`, `/tests`, `/doc`
- Chat participants and context: `@workspace`, `#file`, `#selection`
- Copilot Instructions (`.github/instructions/`) — encoding C# conventions and architecture standards so the whole team gets consistent suggestions

**Demo:**
- `/explain` on `Task.cs` with `@workspace` context
- Show how `.github/instructions/csharp.instructions.md` shapes Copilot's suggestions vs. without it

---

## Track 2 — Core Coding Workflows

### Video 3: TDD in C# — Red, Green, Refactor with Copilot
**Duration:** ~15 min

**Topics:**
- The Red-Green-Refactor cycle and why it matters with AI-generated code
- Interface-first design as a Copilot prompt strategy
- Testing stack: xUnit + FakeItEasy
- Why TDD is especially important when using AI — validating output, not just accepting it
- Arrange-Act-Assert pattern in practice

**Demo:** Write a failing test for a new service interface, let Copilot suggest the implementation, then refactor with Copilot assistance

---

### Video 4: From User Story to C# Feature (DDD)
**Duration:** ~15 min

**Topics:**
- Decomposing vague user stories into actionable acceptance criteria with Copilot
- Domain-Driven Design building blocks: Value Objects, Entities, Aggregates
- How DDD fits the Clean Architecture layers in this codebase
- Using `#file` context for precise, consistent prompts

**Demo:** Prompt Copilot to build a `Priority` value object from a raw user story — show the decomposition conversation and the resulting C# code

---

### Video 5: Generating APIs & Refactoring Legacy Code
**Duration:** ~15 min

**Topics:**
- CQRS with MediatR — generating command/query handlers and Minimal API endpoints
- Using `@workspace` to keep generated code consistent with existing patterns
- Object Calisthenics principles as a refactoring target
- Guard clauses and reducing nesting in legacy code

**Demo:**
- Generate a `DeleteTask` MediatR handler and endpoint
- Before/after refactor of `LegacyTaskProcessor` using Object Calisthenics prompts

---

### Video 6: Testing, Documentation & Git Workflow
**Duration:** ~12 min

**Topics:**
- Generating test suites from method selections with `/tests`
- XML documentation comments with `/doc`
- Writing conventional commit messages with Copilot
- Generating PR descriptions using `@workspace` context
- The complete AI-assisted development cycle end-to-end

**Demo:** Take a recently implemented method through the full workflow — generate tests, add XML docs, write a commit message, and draft a PR description

---

## Track 3 — Interaction Models & Customization

### Video 7: Ask vs Edit vs Agent Mode — Choosing the Right Tool
**Duration:** ~12 min

**Topics:**
- Ask mode: exploration, learning, understanding code — no changes made
- Edit mode: targeted, scoped changes with diff preview
- Agent mode: multi-step orchestration across files — not "better chat"
- Decision framework: scope, control, and risk level
- Common mistakes: using Agent mode for simple edits, using Ask mode when you need action

**Demo:** Deliver the same prompt ("add a Priority property to the Task entity") in all three modes — show how scope, output, and control differ

---

### Video 8: Skills & the Customization Hierarchy
**Duration:** ~12 min

**Topics:**
- The four customization levels: Prompts → Instructions → Skills → Agents
- What Skills are: domain knowledge packages without tool access
- Skills vs Agents: knowledge vs action
- The decision tree: does it need to read/write files? → Agent. Does it need consistent domain context? → Skill.
- How to discover available skills (`/skills`) and agents (`/agents`)

**Demo:** Walk through the `test-data-generator` SKILL.md in this repo — show how it shapes test data suggestions vs. a plain prompt

---

## Track 4 — Custom Agents

### Video 9: Introduction to Custom Agents
**Duration:** ~12 min

**Topics:**
- Agents as specialist AI consultants — role-based, not task-based
- YAML frontmatter properties: `user-invocable`, `disable-model-invocation`, `handoffs`
- The agent catalog in this repo and how to explore it
- When to use a custom agent vs. standard chat or a skill

**Demo:** Run the `architecture-reviewer` agent on the Task domain model, then ask the same question in plain chat — highlight the quality and consistency gap

---

### Video 10: Workflow Agents in Action
**Duration:** ~15 min

**Topics:**
- Real-world agent workflows: backlog generation, architecture review, test strategy
- Comparing agent output to standard chat for structure and actionability
- What agents excel at: consistency, format enforcement, first-pass automation
- Agent limitations: they still need human judgment and verification

**Demo:**
- `backlog-generator` agent live — turn a feature description into sprint-ready stories
- Discuss what to trust, what to verify, and when to iterate

---

## Production Notes

### Recording Setup
- VS Code with TaskManager solution open
- `.github/instructions/` files active
- Custom agents from repo loaded
- Theme/font size readable on screen share

### Suggested Recording Order
Record Track 1 first to establish context, then Track 2 in order (each builds on prior concepts), then Tracks 3–4 which are largely self-contained.

| Order | Video | Reason |
|-------|-------|--------|
| 1st | Video 1 | Sets the stage for everything |
| 2nd | Video 2 | Establishes tools and instructions used in all demos |
| 3rd | Video 3 | First deep .NET demo |
| 4th | Video 4 | Builds on TDD / domain concepts |
| 5th | Video 5 | Depends on understanding of project structure |
| 6th | Video 6 | Caps the core workflow track |
| 7th | Video 7 | Reusable across teams — minimal .NET dependency |
| 8th | Video 8 | Reusable across teams |
| 9th | Video 9 | Intro before workflow demo |
| 10th | Video 10 | Culmination of agent content |

### Reusability
- **Videos 1–6:** .NET-specific, use C# and TaskManager codebase
- **Videos 7–10:** Largely language-agnostic — can be reused or lightly re-dubbed for Java/other audiences

---

## Related Resources

- [Lab Guides](labs/) — corresponding hands-on exercises for each topic
- [Presentation Modules](presentations/modules/) — Marp slide decks used in live workshops
- [C# Instructions](../.github/instructions/csharp.instructions.md) — team standards referenced in Video 2
- [Architecture Overview](design/architecture.md) — background for Videos 4 and 5
- [Custom Agent Catalog](guides/custom-agent-catalog.md) — agent reference for Videos 9–10
