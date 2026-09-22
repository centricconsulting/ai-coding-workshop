# Lab 09: Designing Effective Custom Agents

**Module:** 4  
**Duration:** 25 minutes  
**Part:** Advanced GitHub Copilot (Part 2)

## Objectives

By the end of this lab, you will:
- Understand the key components of agent instructions
- Learn how to design agents around roles, not tasks
- Practice iterating on agent behavior through instruction refinement
- Recognize patterns for creating reliable, trustworthy agents

## Prerequisites

- Completion of [Lab 08: Workflow Agents](lab-08-workflow-agents.md)
- VS Code with GitHub Copilot extension
- Access to the TaskManager workshop repository

## Background

### Agents Are Products, Not Prompts

Creating a custom agent isn't just writing a prompt. It's designing a **reusable product** that your team will rely on.

**Key Principle:** Design for roles (specialists), not tasks (one-off actions)

❌ **Task-based (Bad):** "Generate unit tests for a method"  
✅ **Role-based (Good):** "Test Strategist - Proposes comprehensive test strategies"

---

## Core Components of Agent Instructions

Every effective agent definition has:

### 1. **Identity & Role**
- Who is this agent?
- What expertise does it embody?

```markdown
You are an expert software architect specializing in Clean Architecture...
```

### 2. **Responsibilities**
- What does this agent do?
- What is in scope vs out of scope?

```markdown
## Responsibilities
- Analyze code structure for architectural boundary violations
- Identify dependency direction issues
- Review domain model design for DDD patterns
```

### 3. **Context**
- What does the agent need to know about the project?
- What standards, patterns, or constraints apply?

```markdown
## Context
This project follows Clean Architecture with these layers:
- Domain: Business logic (no external dependencies)
- Application: Use cases (depends on Domain only)
...
```

### 4. **Constraints**
- What should the agent ALWAYS do?
- What should it NEVER do?

```markdown
## Constraints
- ALWAYS check for circular dependencies
- NEVER recommend breaking Clean Architecture boundaries
```

### 5. **Process/Approach**
- How should the agent work through the task?
- What steps should it follow?

```markdown
## Analysis Process
1. Identify which layer(s) the code belongs to
2. Check dependencies against allowed directions
3. Review domain modeling
```

### 6. **Output Format**
- How should results be structured?
- What sections or headings should appear?

```markdown
## Output Format
Provide your review in this structured format:

### Architecture Review Summary
- **Scope:** [what was reviewed]
- **Overall Assessment:** [Pass/Needs Attention/Refactor Required]
...
```

### 7. **Tone & Approach**
- How should the agent communicate?
- What's the personality or style?

```markdown
## Tone
- Be direct and constructive
- Explain WHY something is a concern (educational)
- Acknowledge good practices when present
```

---

## Exercise 1: Analyze Existing Agents (10 minutes)

### Instructions

Open each of the three custom agents and map their components:

#### Architecture Reviewer
- **File:** `.github/agents/architecture-reviewer.agent.md`
- **Role:** [Identify the role]
- **Key Responsibilities:** [List 3]
- **Critical Constraints:** [List 2-3]
- **Output Structure:** [Describe the format]

#### Backlog Generator
- **File:** `.github/agents/backlog-generator.agent.md`
- **Role:** [Identify the role]
- **Key Responsibilities:** [List 3]
- **Critical Constraints:** [List 2-3]
- **Output Structure:** [Describe the format]

#### Test Strategist
- **File:** `.github/agents/test-strategist.agent.md`
- **Role:** [Identify the role]
- **Key Responsibilities:** [List 3]
- **Critical Constraints:** [List 2-3]
- **Output Structure:** [Describe the format]

### Questions
- What patterns do you notice across all three agents?
- Which component seems most critical for consistency?
- Are there any missing components you'd add?

---

## Advanced Agent Configuration

Now that you understand the core components, let's explore **advanced agent capabilities** that give you fine-grained control over agent behavior.

### Agent Frontmatter Properties

The YAML frontmatter at the top of `.agent.md` files controls how agents appear and behave. Beyond the basic `name` and `description`, several optional properties provide powerful customization.

#### Basic Properties (Review)

```yaml
---
name: "agent-name"
description: "What the agent does"
tools: ['read', 'search', 'write']  # Tools available to agent
model: Claude Sonnet 4.5  # AI model to use
---
```

#### Advanced Properties (New)

##### 1. `user-invocable` (boolean, default: true)

Controls whether the agent appears in the agents dropdown.

```yaml
user-invocable: true   # Visible in dropdown (default)
user-invocable: false  # Hidden from dropdown
```

**Use Cases:**
- **true (default)**: General-purpose agents users select explicitly
- **false**: Internal agents only called by other agents as subagents

**Example:** A "validation" agent that only runs as part of other workflows:
```yaml
---
name: "internal-validator"
description: "Validates code quality metrics"
user-invocable: false  # Users don't directly invoke this
---
```

---

##### 2. `disable-model-invocation` (boolean, default: false)

Prevents other agents from calling this agent as a subagent.

```yaml
disable-model-invocation: false  # Can be called as subagent (default)
disable-model-invocation: true   # Prevents subagent invocation
```

**Use Cases:**
- **false (default)**: Agent can be used both directly and as subagent
- **true**: Agent requires direct user interaction (UI agents, approval workflows)

**Example:** An agent that requires human confirmation:
```yaml
---
name: "deployment-approver"
description: "Reviews and approves deployments"
disable-model-invocation: true  # Must be invoked manually
---
```

---

##### 3. `agents` (array)

Specifies which agents this agent can call as subagents.

```yaml
agents: ['*']                    # Can call any agent (default behavior)
agents: ['architect', 'planner'] # Can only call specific agents
agents: []                       # Cannot call any agents
```

**Use Cases:**
- **`['*']`**: Orchestrator agents that coordinate multiple specialists
- **Specific list**: Controlled workflows with defined handoff paths
- **`[]`**: Terminal agents that don't delegate (e.g., implementers)

**Example:** A planner that can only handoff to specific implementers:
```yaml
---
name: "feature-planner"
description: "Plans feature implementation"
agents: ['implementer', 'test-strategist']  # Controlled handoffs
---
```

---

##### 4. `argument-hint` (string)

Provides hint text shown in chat input when agent is selected as slash command.

```yaml
argument-hint: "[file path] [options]"
```

**Use Cases:**
- Guide users on what information to provide
- Document expected parameters or context

**Example:**
```yaml
---
name: "code-reviewer"
description: "Reviews code for quality issues"
argument-hint: "[file or directory to review]"
---
```

When user types `/code-reviewer`, they see hint: `[file or directory to review]`

---

### Property Combinations

You can combine these properties for sophisticated agent control:

#### Example 1: Public Orchestrator Agent
```yaml
---
name: "workflow-orchestrator"
user-invocable: true              # Users can select it
disable-model-invocation: false   # Other agents can use it
agents: ['planner', 'implementer', 'reviewer']  # Can call these agents
---
```

#### Example 2: Internal Helper Agent
```yaml
---
name: "code-formatter"
user-invocable: false             # Hidden from dropdown
disable-model-invocation: false   # But other agents can call it
agents: []                        # Doesn't call other agents
---
```

#### Example 3: Manual-Only Terminal Agent
```yaml
---
name: "security-auditor"
user-invocable: true              # Users can select it
disable-model-invocation: true    # But agents cannot auto-invoke it
agents: []                        # Doesn't delegate to other agents
---
```

---

## Handoffs: Orchestrating Multi-Agent Workflows

**Handoffs** enable you to create guided sequential workflows that transition between agents with suggested next steps. After an agent completes its response, handoff buttons appear, allowing users to move to the next agent with relevant context and a pre-filled prompt.

### Why Use Handoffs?

✅ **Guided workflows**: Lead users through multi-step processes  
✅ **Context preservation**: Carry context between agent transitions  
✅ **Human-in-the-loop**: Users review and approve each step before proceeding  
✅ **Workflow standardization**: Encode best practices as agent chains

### Handoff Definition

Handoffs are defined in the agent's YAML frontmatter:

```yaml
---
name: "planner"
description: "Creates implementation plans"
tools: ['read', 'search']
handoffs:
  - label: "Start Implementation"
    agent: "implementer"
    prompt: "Implement the plan outlined above"
    send: false
    model: Claude Sonnet 4.5
---
```

#### Handoff Properties

| Property | Required | Description |
|----------|----------|-------------|
| `label` | Yes | Button text shown to user (e.g., "Start Implementation") |
| `agent` | Yes | Target agent identifier to switch to |
| `prompt` | Yes | Pre-filled prompt sent to target agent |
| `send` | No | Auto-submit prompt (default: false). If true, workflow continues automatically |
| `model` | No | Override model for this handoff (optional) |

### Common Handoff Patterns

#### Pattern 1: Plan → Implement → Review

**Planner Agent:**
```yaml
handoffs:
  - label: "Start Implementation"
    agent: "implementer"
    prompt: "Implement the feature plan outlined above"
    send: false
```

**Implementer Agent:**
```yaml
handoffs:
  - label: "Request Code Review"
    agent: "code-reviewer"
    prompt: "Review the implementation for quality and standards"
    send: false
```

**Usage Flow:**
1. User selects the `planner` agent from the Agent Mode dropdown: "Plan a user authentication feature"
2. Planner provides plan
3. User clicks "Start Implementation" → hands off to the `implementer` agent
4. Implementer generates code
5. User clicks "Request Code Review" → hands off to the `code-reviewer` agent

---

#### Pattern 2: Write Failing Tests → Make Tests Pass

**Test-First Agent:**
```yaml
handoffs:
  - label: "Implement to Pass Tests"
    agent: "implementer"
    prompt: "Implement the code to make the tests above pass"
    send: false
```

**Usage Flow:**
1. User selects the `test-first` agent from the Agent Mode dropdown: "Create tests for order validation"
2. Agent generates failing tests (easier to review than big implementations)
3. User reviews tests, clicks "Implement to Pass Tests"
4. Implementer writes code to satisfy tests

---

#### Pattern 3: Architecture → Documentation

**Architect Agent:**
```yaml
handoffs:
  - label: "Generate Documentation"
    agent: "documenter"
    prompt: "Document the architectural decisions made above"
    send: false
```

---

### Multiple Handoffs

Agents can define multiple handoff options:

```yaml
handoffs:
  - label: "Implement Full Feature"
    agent: "implementer"
    prompt: "Implement the complete feature as planned"
    send: false
  
  - label: "Prototype Only"
    agent: "prototyper"
    prompt: "Create a quick prototype to validate the approach"
    send: false
  
  - label: "Create Architecture Doc"
    agent: "architect"
    prompt: "Document architectural decisions for this plan"
    send: false
```

Users see all three handoff buttons and choose the appropriate next step.

---

### Auto-Send Handoffs

Setting `send: true` makes the workflow continue automatically:

```yaml
handoffs:
  - label: "Auto-Validate"
    agent: "validator"
    prompt: "Validate the implementation above"
    send: true  # Automatically submits when clicked
```

⚠️ **Use cautiously**: Auto-send removes human review checkpoint. Best for:
- Simple validation steps
- Non-destructive operations
- Established workflows where auto-proceed is safe

---

### Handoff Best Practices

#### ✅ Do:
- **Keep handoff chains short** (3-4 agents max)
- **Use descriptive labels** ("Request Security Review" not just "Next")
- **Pre-fill useful prompts** with specific context
- **Design for human review** (prefer `send: false`)
- **Document handoff paths** in agent descriptions

#### ❌ Don't:
- Create circular handoffs (A → B → A)
- Auto-send without clear justification
- Skip human review for destructive operations
- Make handoff chains too complex

---

### Exercise Addition: Design a Handoff Workflow

**Scenario:** You want to create a workflow for adding a new feature.

**Agents Involved:**
1. **Planner** - Creates feature plan
2. **Implementer** - Writes code
3. **Test Strategist** - Proposes test scenarios

**Your Task:**
Design handoffs for this workflow. Answer:

1. What should each agent's handoff button say?
2. Should any handoffs use `send: true`? Why or why not?
3. In what order should agents be chained?

<details>
<summary>Example Solution</summary>

**Planner:**
```yaml
handoffs:
  - label: "Start Implementation"
    agent: "implementer"
    prompt: "Implement the feature plan above, starting with domain layer"
    send: false  # Human reviews plan first
```

**Implementer:**
```yaml
handoffs:
  - label: "Generate Test Strategy"
    agent: "test-strategist"
    prompt: "Propose comprehensive test scenarios for the implementation above"
    send: false  # Human reviews implementation first
```

**Flow:** Plan → Review → Implement → Review → Test Strategy

**Why `send: false` everywhere?**  
Human reviews ensure each step is correct before proceeding. No auto-proceed for code changes.

</details>

---

##

## Exercise 2: Iterating on Agent Instructions (15 minutes)

### Scenario

The **Test Strategist** agent sometimes provides too many tests, including low-value scenarios. You want to refine it to focus on **high-value tests only**.

### Part A: Baseline Behavior

1. Open Copilot Chat in Agent Mode
2. Select **Test Strategist**
3. Prompt: `Propose test scenarios for a simple getter method that returns a task's title`

**Observe:** Does it over-test? Does it recommend unnecessary tests?

---

### Part B: Refine the Agent

1. Open `.github/agents/test-strategist.agent.md`
2. Add this constraint to the **Constraints** section:

```markdown
- Focus on HIGH-VALUE tests only (avoid trivial getters/setters)
- Skip tests for auto-implemented properties or simple pass-through methods
- Prioritize tests that verify business logic, invariants, and edge cases
```

3. **Save the file**

---

### Part C: Re-test Behavior

1. Return to Copilot Chat (Agent Mode)
2. Select **Test Strategist** again
3. Use the same prompt: `Propose test scenarios for a simple getter method that returns a task's title`

**Observe:** Did the agent's behavior change? Did it decline or simplify the recommendation?

---

### Reflection Questions

1. **Did the constraint reduce over-testing?**
2. **What other refinements would improve this agent?**
3. **How many iterations would you expect before an agent is "production-ready"?**

---

## Design Patterns for Reliable Agents

### Pattern 1: Role-Based Scope
✅ **Do:** "You are a code reviewer specializing in security"  
❌ **Don't:** "Generate code for feature X"

### Pattern 2: Explicit Constraints
✅ **Do:** "NEVER recommend breaking layer boundaries"  
❌ **Don't:** Leave implicit assumptions unstated

### Pattern 3: Structured Outputs
✅ **Do:** Define sections, headings, and formats  
❌ **Don't:** Allow free-form, unpredictable responses

### Pattern 4: Educational Tone
✅ **Do:** "Explain WHY this is a concern"  
❌ **Don't:** Just list issues without context

### Pattern 5: Boundaries & Disclaimers
✅ **Do:** "This agent reviews; humans decide"  
❌ **Don't:** Imply the agent is authoritative

---

## Governance Considerations

### Versioning
- Agents should be versioned (like code)
- Track changes in git commit history
- Consider semantic versioning for major changes

### Review Process
- Agent changes require **pull request review**
- Test agent behavior before merging
- Document breaking changes

### Team Alignment
- Agents encode **team decisions**, not individual preferences
- Discuss agent behavior in retrospectives
- Update agents as practices evolve

### Documentation
- Maintain a **catalog of agents** ([docs/guides/custom-agent-catalog.md](../guides/custom-agent-catalog.md))
- Document when to use each agent
- Provide examples of good vs bad usage

---

## Common Pitfalls

### ❌ Pitfall 1: Task-Based Agents
Creating agents for single, one-off tasks instead of repeatable roles.

**Fix:** Design for workflows, not individual actions.

### ❌ Pitfall 2: Vague Instructions
Leaving agent behavior open to interpretation.

**Fix:** Use explicit constraints and structured outputs.

### ❌ Pitfall 3: Over-Scoping
Making agents do too much.

**Fix:** Keep agents focused on one role or domain.

### ❌ Pitfall 4: Under-Testing
Deploying agents without validating behavior.

**Fix:** Test agents with real scenarios before sharing.

### ❌ Pitfall 5: No Iteration Loop
Treating agents as "set and forget."

**Fix:** Continuously refine based on usage and feedback.

---

## Key Takeaways

✅ **Agents are products** - Design, test, and maintain them like code  
✅ **Role-based design** - Specialists, not task executors  
✅ **Explicit constraints** - State what the agent must/must not do  
✅ **Structured outputs** - Consistency requires format  
✅ **Iterate continuously** - Refine based on real usage  
✅ **Govern as team assets** - Version, review, and document

---

## Next Steps

In [Lab 10: Capstone - Build Your Own Agent](lab-10-capstone-build-agent.md), you'll **create a production-ready custom agent** from scratch, applying everything you've learned.

---

## Additional Resources

- [Agent Design Guide](../guides/agent-design-guide.md)
- [Agent Governance](../guides/agent-governance.md)
- [Custom Agent Catalog](../guides/custom-agent-catalog.md)
