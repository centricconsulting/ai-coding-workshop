🧠 Advanced GitHub Copilot Workshop (Part 2)

Custom Agents & AI-Driven Development Workflows

Duration: 3 Hours
Format: Instructor-led, hands-on (remote, in-person, or hybrid)
Audience: Developers, Architects, Technical Leads
Prerequisite: Completion of Using AI for Application Development with GitHub Copilot (Part 1)

⸻

🎯 Workshop Purpose

This advanced workshop extends foundational GitHub Copilot usage into workflow-level AI by introducing Custom Copilot Agents, Agent Mode, and role-based AI patterns in VS Code.

Participants move beyond ad-hoc prompting to:
	•	reusable AI workflows,
	•	consistent team behaviors,
	•	and governed AI usage aligned with modern SDLC practices.

The workshop is aligned with the current VS Code Copilot experience (December update) and reflects real-world constraints and best practices.

⸻

🎓 Learning Objectives

By the end of this workshop, participants will be able to:
	•	Explain and intentionally use Ask, Edit, and Agent modes
	•	Understand Custom Copilot Agents as chat participants
	•	Apply agents to common engineering workflows (beyond code generation)
	•	Design effective, role-based custom agents
	•	Iterate on agent instructions to improve reliability
	•	Establish patterns for agent reuse and governance

⸻

🧩 Workshop Structure

The workshop follows a consistent rhythm:

Concept → Demo → Hands-On Lab → Reflection

This mirrors how teams should adopt Copilot in practice.

⸻

🟦 Module 0 — Kickoff & Context Reset

Duration: 10 minutes

Objectives
	•	Re-anchor participants in Part 1 concepts
	•	Set expectations for advanced usage
	•	Establish shared mental models

Topics
	•	Quick recap:
	•	Copilot Instructions
	•	TDD, refactoring, documentation automation
	•	Positioning:
“In Part 1, Copilot helped you write better code.
In Part 2, we’ll focus on shaping how teams work using AI.”
	•	Overview of the agenda and labs
	•	Environment readiness check (VS Code, Copilot, repo access)

⸻

🟦 Module 1 — Copilot Interaction Models (Ask, Edit, Agent)

Duration: 25 minutes

Objectives
	•	Understand how Copilot behaves in VS Code today
	•	Learn when to use each interaction mode

Topics
	•	Ask Mode
	•	Exploration, explanation, learning
	•	Edit Mode
	•	Local, scoped code changes
	•	Agent Mode
	•	Plan → execute → review
	•	Multi-file, multi-step workflows
	•	Human-in-the-loop by design

Demo

The same task performed three ways:
	1.	Ask → explanation only
	2.	Edit → localized refactor
	3.	Agent → repository-level analysis and plan

Key Takeaway

Agent Mode is not “better chat” — it is a different execution model.

⸻

🟦 Module 2 — Custom Copilot Agents as Chat Participants

Duration: 30 minutes

Objectives
	•	Build the correct mental model for agents
	•	Understand why agents improve consistency

Topics
	•	What Custom Agents are in practice
	•	Named chat participants (@ArchitectureReviewer)
	•	Role-based AI personas
	•	How agents differ from:
	•	Prompts (one-off)
	•	Copilot Instructions (always-on guidance)
	•	When to use an agent:
	•	Repeated workflows
	•	Reviews and validation
	•	Structured outputs
	•	Team-level consistency

Guided Exercise

Participants interact with a pre-created agent using Agent Mode and compare the result to standard Copilot Chat.

Key Takeaway

If Copilot Instructions are guardrails,
agents are specialists you consult.

⸻

🟦 Module 3 — Workflow Agents in Action (Hands-On Lab)

Duration: 45 minutes

Objectives
	•	Apply agents to real development workflows
	•	Observe the difference between agents and ad-hoc prompting

Lab Scenarios (facilitator selects 2–3)
	•	Backlog Generation
	•	User story → backlog items + acceptance criteria
	•	Architecture Review
	•	Scan repo for structural or boundary issues
	•	Test Strategy
	•	Propose unit vs integration tests and edge cases
	•	Compare to /tests

Deliverables
	•	Side-by-side comparison of:
	•	Copilot Chat output
	•	Agent output
	•	Group discussion on reliability and consistency

⸻

🟦 Module 4 — Designing Effective Custom Agents

Duration: 25 minutes

Objectives
	•	Learn how to design agents teams can trust
	•	Understand instruction quality and constraints

Topics
	•	Designing agents around roles, not tasks
	•	Instruction components:
	•	Responsibilities
	•	Constraints
	•	Output structure
	•	Iteration loop:
	•	Run → refine → re-run
	•	Governance considerations:
	•	Versioning
	•	Review via pull requests
	•	Sharing across teams

Demo

Live refinement of an agent and observation of behavior changes.

Key Takeaway

Agents are products, not prompts.

⸻

🟦 Module 5 — Capstone Lab: Build a Production-Ready Agent

Duration: 35 minutes

Objectives
	•	Create a reusable agent aligned to a real workflow
	•	Apply all concepts learned so far

Steps
	1.	Select a workflow (reviewer, strategist, writer, etc.)
	2.	Define success criteria
	3.	Create or customize an agent definition
	4.	Test using Agent Mode
	5.	Iterate based on results

Deliverable
	•	A functional custom agent
	•	Clear intended use case and scope

⸻

🟦 Module 6 — Wrap-Up, Governance & Next Steps

Duration: 10 minutes

Discussion Topics
	•	Which workflows benefit most from agents?
	•	What should be standardized at team vs org level?
	•	How do we prevent “prompt sprawl”?
	•	How should agents be reviewed and evolved?

Closing Guidance
	•	Start with reviewer agents, not executors
	•	Maintain a shared agent catalog
	•	Treat agents as governed assets
	•	Keep humans accountable