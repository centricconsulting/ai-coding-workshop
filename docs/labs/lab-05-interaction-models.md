# Lab 05: Copilot Interaction Models (Ask, Plan, Agent)

**Module:** 1  
**Duration:** 25 minutes  
**Part:** Advanced GitHub Copilot (Part 2)

## Objectives

By the end of this lab, you will:
- Understand the three primary interaction models in GitHub Copilot
- Know when to use Ask, Plan, and Agent modes
- Experience the differences through hands-on exercises
- Recognize Agent Mode as a distinct execution model

## Prerequisites

- Completion of Part 1 labs (or equivalent Copilot experience)
- VS Code with GitHub Copilot extension
- Access to the TaskManager workshop repository

## Background

GitHub Copilot in VS Code offers three distinct interaction models, each optimized for different workflows:

### 1. **Ask Mode** (Informational)
- **Purpose:** Learning, exploration, explanation
- **Behavior:** Provides answers without making changes
- **Use when:** You need to understand code, patterns, or concepts

### 2. **Plan Mode** (Design Before Code)
- **Purpose:** Design a multi-step approach and gather requirements before code generation begins
- **Behavior:** Asks clarifying questions, proposes a structured plan, and waits for your approval before any code is written
- **Use when:** You want to validate the strategy or requirements before committing to implementation

### 3. **Agent Mode** (Multi-Step Workflows)
- **Purpose:** Complex, repository-level tasks
- **Behavior:** Plan → execute → review with human checkpoints
- **Use when:** Work spans multiple files or requires analysis
- **Key trait:** Human-in-the-loop by design

## Discovering Copilot Capabilities

Before diving into the exercises, let's explore **slash commands** — quick ways to discover and access Copilot's features.

### Slash Commands for Discovery

Slash commands start with `/` and help you find agents, skills, and other capabilities:

| Command | Purpose | Example Usage |
|---------|---------|---------------|
| `/help` | Show all available commands | Type `/help` in chat |
| `/agents` | List all available custom agents | `/agents` to see @architect, @planner, etc. |
| `/skills` | List all available skills | `/skills` to see #test-data-generator, etc. |
| `/init` | Start a new project or workspace | `/init dotnet webapi` |
| `/create-workspace` | Create new workspace | `/create-workspace my-project` |
| `/create-notebook` | Create new Jupyter notebook | `/create-notebook data-analysis` |
| `/create-file` | Create new file with AI assistance | `/create-file readme.md` |
| `/install-extension` | Install VS Code extension | `/install-extension ms-python.python` |

### Agent Discovery with @-mentions

Once you know which agents exist (from `/agents`), you can invoke them with `@`:

```
@architect Review the domain model for this feature
@planner Create an implementation plan for user authentication
```

### Skills Discovery with #-mentions

Skills are specialized knowledge modules invoked with `#`:

```
#test-data-generator Create sample order data for integration tests
```

> **Note:** Skills and their differences from agents will be covered in [Lab 06: Skills & Customization](lab-06-skills-and-customization.md).

### Try It Now (2 minutes)

1. Open Copilot Chat
2. Type `/help` to see all available commands
3. Type `/agents` to see available custom agents
4. Type `/skills` to see available skills

**Observation:** What agents and skills are available in this workshop?

---

## Lab Structure

You'll perform **the same task** using all three modes to understand their strengths and limitations.

### The Task

**Scenario:** You need to add a new property `Priority` to the `Task` entity in the Domain layer and ensure it's properly handled throughout the codebase.

---

## Exercise 1: Ask Mode (5 minutes)

### Instructions

1. Open **Copilot Chat** in VS Code
2. Ensure you're in **Ask mode** (default chat behavior)
3. Enter this prompt:

```
I want to add a Priority property (Low, Medium, High) to the Task entity. 
How should I implement this following Clean Architecture and DDD patterns?
```

4. Review the response

### Expected Outcome

Copilot will:
- Explain how to add the property
- Suggest using a Value Object for Priority
- Describe the pattern, but **not make any changes**

### Questions to Consider

- Did Copilot provide enough detail to implement this yourself?
- What follow-up questions would you ask?
- When is this mode most valuable?

---

## Exercise 2: Plan Mode (10 minutes)

### Instructions

1. Open **Copilot Chat** in VS Code
2. Switch to **Plan Mode** (select from the mode dropdown)
3. Use this prompt:

```
I want to add a Priority property (Low, Medium, High) to the Task entity.
```

4. **Observe Plan Mode's behavior:**
   - Copilot will ask clarifying questions about requirements
   - It will design a multi-step approach before writing any code
   - Review the proposed plan before approving

5. Respond to any clarifying questions
6. Review the structured plan that is produced

### Expected Outcome

Plan Mode will:
- Ask clarifying questions (e.g., "Should Priority be a value object or an enum?")
- Propose a structured, multi-step implementation approach
- **Not write any code yet** — waiting for your approval of the plan

### Questions to Consider

- What requirements did Plan Mode surface that you hadn't considered?
- How does seeing the plan first change how you approach the implementation?
- When is Plan Mode the right choice over jumping straight to Agent Mode?

### Challenge

After reviewing the plan, switch to **Agent Mode** and use the plan output as your prompt. Does Agent Mode follow the proposed approach?

---

## Exercise 3: Agent Mode (10 minutes)

### Instructions

1. Open **Copilot Chat**
2. Switch to **Agent Mode** (look for the Agent mode toggle/button)
3. Use this prompt:

```
Add a Priority property (Low, Medium, High) to the Task entity following DDD patterns.
Ensure the change is properly integrated across Domain, Application, and Api layers.
```

**Alternative:** If you want to use a **custom agent** (discovered via `/agents`):

```
@plan Add Priority property to Task entity with full integration
```

This invokes the `@plan` agent, which specializes in creating implementation plans.

4. **Observe the Agent's process:**
   - Planning phase
   - File analysis
   - Proposed changes
   - Checkpoints for your review

5. Review each step before proceeding
6. Accept or reject individual changes

### Expected Outcome

Agent Mode will:
1. **Analyze** the codebase structure
2. **Plan** the changes across multiple files
3. **Propose** changes in stages:
   - Domain layer (value object + entity)
   - Application layer (if needed)
   - Api layer (request/response mapping)
4. **Wait for your approval** at key checkpoints

### Questions to Consider

- How did Agent Mode's approach differ from Plan Mode?
- What visibility did you have into the Agent's reasoning?
- When would you use Plan Mode first, then hand off to Agent Mode?

---

## Comparison Table

Create your own comparison based on the exercises:

| Aspect | Ask Mode | Plan Mode | Agent Mode |
|--------|----------|-----------|------------|
| **Speed** | [Your observation] | [Your observation] | [Your observation] |
| **Scope** | [Your observation] | [Your observation] | [Your observation] |
| **Control** | [Your observation] | [Your observation] | [Your observation] |
| **Best For** | [Your observation] | [Your observation] | [Your observation] |

---

## Key Takeaways

### Ask Mode
✅ Use for: Learning, exploration, gathering context  
❌ Don't use for: Making changes, implementing features

### Plan Mode
✅ Use for: Designing approach and clarifying requirements before code generation  
❌ Don't use for: When you're already clear on the approach and ready to execute

### Agent Mode
✅ Use for: Complex workflows, repository-level analysis, staged changes  
❌ Don't use for: Simple edits, quick fixes  
⚠️ Remember: Agent Mode is **not just "better chat"** — it's a different execution model

---

## Reflection Questions

1. **Which mode felt most natural for this task? Why?**
2. **When would you deliberately choose Ask Mode over Agent Mode?**
3. **What are the risks of using Agent Mode for everything?**
4. **How does Agent Mode enforce "human-in-the-loop"?**

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
