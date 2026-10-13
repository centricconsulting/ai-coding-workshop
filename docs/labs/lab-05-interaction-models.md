# Lab 05: Copilot Interaction Models (Interactive, Plan, Autopilot)

**Module:** 1  
**Duration:** 25 minutes  
**Part:** Advanced GitHub Copilot (Part 2)

## Objectives

By the end of this lab, you will:
- Understand the Interactive and Plan agent modes in GitHub Copilot (Autopilot is discussed, not practiced)
- Know when to use each mode and how it differs from Permissions and the Agent picker
- Experience the differences through hands-on exercises
- Recognize autonomy as a dial: choose the least autonomy that fits the job

## Prerequisites

- Completion of Part 1 labs (or equivalent Copilot experience)
- VS Code with GitHub Copilot extension
- Access to the TaskManager workshop repository

## Background

GitHub Copilot in VS Code offers three **agent modes**, selected from the **Agent mode** picker in the control bar below the chat text box:

### 1. **Interactive** (Step-by-Step Collaboration)
- **Purpose:** Questions, exploration, and focused changes with you steering each step
- **Behavior:** Answers questions and makes changes while you review and approve along the way
- **Use when:** You want to learn, explore, or keep tight control

> **Note:** The former separate *Ask* and *Edit* modes are gone. Asking a question is simply a prompt in Interactive mode.

### 2. **Plan** (Plan First, Execute When Ready)
- **Purpose:** Design a multi-step approach and gather requirements before code generation begins
- **Behavior:** Asks clarifying questions, proposes a structured plan, and waits for your approval before any code is written
- **Use when:** You want to validate the strategy or requirements before committing to implementation

### 3. **Autopilot** (Autonomous Within Permissions) — *discussion only*
- **Purpose:** Complex, multi-file tasks completed with minimal interruption
- **Behavior:** Works autonomously within the limits set by **Permissions**; you review the result afterward
- **Use when:** The task is well defined (ideally from an approved plan) and easy to verify
- **Key trait:** Less supervision, so scope and permissions matter more

> **Note:** Your facilitator will discuss Autopilot, but it is **not used in the labs**. You'll build the foundations (Interactive and Plan) first.

### Three Separate Controls

| Control | Answers the question | Examples |
|---------|----------------------|----------|
| **Agent mode** | *How* does Copilot work? | Interactive, Plan, Autopilot |
| **Permissions** | *What* may it do without asking? | Manual (the default in this lab) |
| **Agent picker** | *Who* does the work? | Custom agents such as `plan`, `architecture-reviewer` |

## Discovering Copilot Capabilities

Before diving into the exercises, let's explore **slash commands** — quick ways to discover and access Copilot's features.

### Slash Commands for Discovery

Slash commands start with `/` and help you find agents, skills, and other capabilities:

| Command | Purpose | Example Usage |
|---------|---------|---------------|
| `/` (type slash alone) | Show the slash commands available in your current chat session | Type `/` in chat to see the list |
| `/agents` | Configure and browse your custom agents | `/agents` to see architecture-reviewer, plan, etc. |
| `/skills` | Configure and browse your agent skills | `/skills` to see #test-data-generator, etc. |
| `/init` | Generate or update workspace instructions (`copilot-instructions.md`/`AGENTS.md`) | `/init` in an agent session |
| `/new` | Scaffold a new workspace or file from a natural-language description | `/new dotnet web api project` |
| `/newNotebook` | Scaffold a new Jupyter notebook from a natural-language description | `/newNotebook data analysis starter` |
| `/plan` | Create a detailed implementation plan before coding | `/plan Add Priority property to Task entity` |

> **Note:** Available slash commands depend on your chat surface (VS Code Copilot Chat panel, Agent sessions, or the Copilot CLI) and enabled features — if a command listed here isn't recognized, type `/` alone to see what's actually available in your environment.

### Agent Discovery via Mode Selection

Once you know which agents exist (from `/agents`), you invoke them by **selecting them from the Agent picker** next to the **+** button in VS Code Copilot Chat, or with `/agent <name>` in the Copilot CLI:

```
Select "architecture-reviewer" from the Agent picker → 
Review the domain model for this feature
```

> **Note:** In the VS Code and Copilot CLI flows used in this workshop, custom agents (`.github/agents/*.agent.md`) are selected via the Agent picker (VS Code) or `/agent` (CLI). This is separate from the **Agent mode** picker (Interactive, Plan, Autopilot). `@` is still used for mentioning files and chat participants (for example, `@workspace`), and older docs or chat surfaces may still show `@agentName`.

### Skills Discovery with #-mentions

Skills are specialized knowledge modules invoked with `#`:

```
#test-data-generator Create sample order data for integration tests
```

> **Note:** Skills and their differences from agents will be covered in [Lab 06: Skills & Customization](lab-06-skills-and-customization.md).

### Try It Now (2 minutes)

1. Open Copilot Chat
2. Type `/` to see all available commands for this chat session
3. Type `/agents` to see available custom agents
4. Type `/skills` to see available skills

**Observation:** What agents and skills are available in this workshop?

---

## Lab Structure

You'll perform **the same task** in Interactive and Plan modes to understand their strengths and limitations, then carry the plan through in Interactive mode.

### The Task

**Scenario:** You need to add a new property `Priority` to the `Task` entity in the Domain layer and ensure it's properly handled throughout the codebase.

---

## Exercise 1: Interactive Mode (5 minutes)

### Instructions

1. Open **Copilot Chat** in VS Code
2. In the **Agent mode** picker below the chat box, select **Interactive**
3. Confirm **Permissions** is set to **Manual**
4. Enter this prompt:

```
I want to add a Priority property (Low, Medium, High) to the Task entity. 
How should I implement this following Clean Architecture and DDD patterns?
Explain only; do not change any files.
```

5. Review the response

### Expected Outcome

Copilot will:
- Explain how to add the property
- Suggest using a Value Object for Priority
- Describe the pattern, but **not make any changes** (you asked a question, and Manual permissions would prompt you before any edit)

### Questions to Consider

- Did Copilot provide enough detail to implement this yourself?
- What follow-up questions would you ask?
- If you had said "go ahead" instead, how would Interactive mode have involved you in each step?

---

## Exercise 2: Plan Mode (10 minutes)

### Instructions

1. Open **Copilot Chat** in VS Code
2. In the **Agent mode** picker, select **Plan**
3. Use this prompt:

```
I want to add a Priority property (Low, Medium, High) to the Task entity.
```

4. **Observe Plan's behavior:**
   - Copilot will ask clarifying questions about requirements
   - It will design a multi-step approach before writing any code
   - Review the proposed plan before approving

5. Respond to any clarifying questions
6. Review the structured plan that is produced

### Expected Outcome

Plan will:
- Ask clarifying questions (e.g., "Should Priority be a value object or an enum?")
- Propose a structured, multi-step implementation approach
- **Not write any code yet** — waiting for your approval of the plan

### Questions to Consider

- What requirements did Plan Mode surface that you hadn't considered?
- How does seeing the plan first change how you approach the implementation?
- When is Plan Mode the right choice over jumping straight to implementation?

---

## Exercise 3: Carry Out the Plan in Interactive Mode (10 minutes)

### Instructions

1. Create a throwaway branch so the result is easy to review or discard:

```bash
git switch -c lab-05-priority
```

2. Start a new chat and select **Interactive** in the **Agent mode** picker
3. Leave **Permissions** on **Manual**
4. Paste the plan from Exercise 2 (or use this prompt) and ask Copilot to implement it **one step at a time**:

```
Add a Priority property (Low, Medium, High) to the Task entity following DDD patterns.
Ensure the change is properly integrated across Domain, Application, and Api layers.
Implement one layer at a time and wait for my approval before moving on.
```

**Alternative:** If you want to use a **custom agent** (discovered via `/agents`):

```
Select "plan" from the Agent picker → 
Add Priority property to Task entity with full integration
```

This runs the `plan` agent, which specializes in creating implementation plans.

5. **Observe the process:**
   - File analysis
   - Proposed changes
   - Approval prompts for edits and commands

6. Review each step before proceeding
7. Accept or reject individual changes
8. When finished, review the full diff in the Source Control view, then keep or discard the changes (`git switch -` and delete the branch to discard)

### Expected Outcome

Interactive mode will:
1. **Analyze** the codebase structure
2. **Propose** changes in stages:
   - Domain layer (value object + entity)
   - Application layer (if needed)
   - Api layer (request/response mapping)
3. **Wait for your approval** at each step

### Questions to Consider

- How did implementing from a plan differ from the Exercise 1 prompt?
- What visibility did you have into Copilot's reasoning at each step?
- Did the result follow the plan from Exercise 2?

---

## Discussion: Where Does Autopilot Fit? (facilitator-led, no exercise)

**Autopilot** works autonomously within your **Permissions** and leaves review until the end. We won't use it in the labs, but consider:

- Which tasks would you trust to run with minimal interruption?
- What would you want in place first (a branch, tests, narrower Permissions)?
- Why do the Interactive and Plan skills you just practiced still matter?

---

## Comparison Table

Create your own comparison based on the exercises:

| Aspect | Interactive | Plan |
|--------|-------------|------|
| **Speed** | [Your observation] | [Your observation] |
| **Scope** | [Your observation] | [Your observation] |
| **Control** | [Your observation] | [Your observation] |
| **Best For** | [Your observation] | [Your observation] |

---

## Key Takeaways

### Interactive
✅ Use for: Learning, exploration, focused changes with step-by-step control  
❌ Don't use for: Large, well-defined tasks where approving every step is wasteful

### Plan
✅ Use for: Designing approach and clarifying requirements before code generation  
❌ Don't use for: When you're already clear on the approach and ready to execute

### Autopilot (discussion only)
✅ Fits: Well-defined, verifiable, multi-file work (ideally from an approved plan)  
❌ Doesn't fit: Unclear requirements, risky or hard-to-undo changes  
⚠️ Remember: Autonomy is a dial. Choose the **least autonomy** that fits the job, and let **Permissions** set the guardrails

---

## Reflection Questions

1. **Which mode felt most natural for this task? Why?**
2. **When would you deliberately choose Interactive over a more autonomous mode like Autopilot?**
3. **What are the risks of giving an agent more autonomy than the task needs?**
4. **How do Permissions and the Agent mode picker work together to keep a human in the loop?**

---

## Next Steps

In [Lab 06: Skills & Customization](lab-06-skills-and-customization.md), you'll learn about:
- The **Skills system** and how it differs from agents
- When to use **instructions**, **agents**, or **skills**
- How to leverage the customization hierarchy effectively

Then in [Lab 07: Custom Agents Intro](lab-07-custom-agents-intro.md), you'll learn how to create specialized agents for specific workflows.

---

## Additional Resources

- [GitHub Copilot Modes Documentation](https://docs.github.com/copilot)
- [Diagram: Copilot Interaction Models](../design/diagrams/copilot-interaction-models.md)
- [When to Use Each Mode (Decision Tree)](../design/diagrams/agent-vs-instructions-vs-prompts.md)
