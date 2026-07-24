# Agent Orchestration Patterns

This guide explains how to coordinate multiple agents using handoffs and subagents in VS Code GitHub Copilot custom agents.

## Table of Contents

- [Overview](#overview)
- [Handoffs: User-Controlled Sequential Workflows](#handoffs-user-controlled-sequential-workflows)
- [Subagents: Programmatic Agent Invocation](#subagents-programmatic-agent-invocation)
- [Comparison: Handoffs vs Subagents](#comparison-handoffs-vs-subagents)
- [When to Use Each Pattern](#when-to-use-each-pattern)
- [Examples](#examples)

## Overview

VS Code Copilot custom agents support two patterns for multi-agent workflows:

1. **Handoffs** - Sequential, user-controlled transitions between agents with interactive buttons
2. **Subagents** - Programmatic invocation of agents from within another agent

Both patterns enable complex workflows, but serve different purposes and use cases.

## Handoffs: User-Controlled Sequential Workflows

### What Are Handoffs?

Handoffs create guided sequential workflows where users transition between agents with suggested next steps. After a chat response completes, handoff buttons appear that let users move to the next agent with relevant context and a pre-filled prompt.

### Key Characteristics

- **User-initiated**: User clicks a button to proceed
- **Sequential**: One agent at a time, linear workflow
- **Visible**: Handoff buttons appear in the UI
- **Reviewable**: User can modify the prompt before sending
- **Context transfer**: Previous agent's context flows to next agent

### Configuration Format

Handoffs are defined in the agent file frontmatter:

```yaml
---
name: "architecture-reviewer"
description: 'Reviews code and design for architectural concerns'
tools: ['read', 'search/changes']
handoffs:
  - label: Create User Stories
    agent: backlog-generator
    prompt: |-
      Based on the architectural review above, create user stories for:
      - Identified violations and fixes
      - Refactoring approach
      - Component boundaries
    send: false
  - label: Implement Changes
    agent: default
    prompt: |-
      Implement the architectural changes recommended above.
    send: true
---
```

### Handoff Properties

| Property | Required | Description |
|----------|----------|-------------|
| `label` | Yes | Button text displayed to user |
| `agent` | Yes | Target agent identifier (use `default` for implementation agent) |
| `prompt` | Yes | Pre-filled prompt sent to target agent |
| `send` | No | Auto-submit prompt (default: `false`) |
| `model` | No | Specific model to use (format: `"Model Name (vendor)"`) |

### Send Behavior

**`send: false` (default)**
- Prompt is pre-filled in chat input
- User can review, edit, or add details
- User manually sends when ready
- **Use for**: Workflows requiring user review or input

**`send: true`**
- Prompt automatically submits on button click
- Agent starts processing immediately
- No user intervention needed
- **Use for**: Well-defined, repeatable workflows

### Example Workflow

```
Architecture Review Agent
         ↓ (User clicks "Create User Stories")
Backlog Generator Agent
         ↓ (User clicks "Plan Tests")
Test Strategist Agent
         ↓ (User clicks "Implement")
Default Implementation Agent
```

## Subagents: Programmatic Agent Invocation

### What Are Subagents?

Subagents allow an agent to programmatically invoke other agents during its execution. The parent agent calls subagents, waits for their responses, and incorporates results into its own output.

### Key Characteristics

- **Agent-initiated**: Parent agent decides when to invoke subagents
- **Programmatic**: Called via code/instructions, not user clicks
- **Hidden execution**: User doesn't see individual subagent calls
- **Automatic**: No user intervention required
- **Result aggregation**: Parent agent synthesizes subagent outputs

### Configuration Format

#### 1. Frontmatter Configuration

```yaml
---
name: "orchestrator"
description: 'Orchestrates multiple specialized agents'
tools: ['read', 'search', 'agent']  # Must include 'agent' tool
agents:  # List of allowed subagents
  - backlog-generator
  - test-strategist
  - plan
  # OR use '*' to allow all agents
  # OR use [] to prevent any subagent invocation
model: Claude Sonnet 4.5
---
```

**Critical requirements:**
- `tools` must include `'agent'` or `'subagent'`
- `agents` specifies which agents can be invoked
  - Array of agent names: `['agent1', 'agent2']`
  - All agents: `'*'`
  - No subagents: `[]`

#### 2. Instructions for Invocation

In the agent body, provide instructions on when and how to use subagents:

```markdown
# Orchestrator Agent

## Workflow

When analyzing architecture:

1. Review code for violations
2. Invoke @backlog-generator to create implementation stories
3. Invoke @test-strategist to plan test coverage
4. Invoke @plan for complex refactoring strategies
5. Synthesize all findings into comprehensive report

## Using Subagents

- **@backlog-generator**: Pass architectural violations and required fixes
- **@test-strategist**: Provide components needing test coverage
- **@plan**: Share complex structural issues requiring detailed planning

Wait for each subagent to complete before proceeding to the next step.
```

### Subagent Invocation Syntax

In agent instructions, reference subagents using `@agent-name`:

```markdown
When architectural issues are found:
1. Analyze the codebase
2. Invoke @backlog-generator with context about violations
3. Invoke @test-strategist with testing requirements
4. Combine results into final report
```

### Example Workflow

```
User: "Review the architecture and create a plan"
         ↓
Orchestrator Agent (running)
    ├─→ Calls @architecture-reviewer subagent
    │       ↓ (returns violations)
    ├─→ Calls @backlog-generator subagent
    │       ↓ (returns user stories)
    ├─→ Calls @test-strategist subagent
    │       ↓ (returns test strategy)
    └─→ Synthesizes all results
         ↓
Returns comprehensive report to user
```

## Comparison: Handoffs vs Subagents

| Aspect | Handoffs | Subagents |
|--------|----------|-----------|
| **Initiation** | User clicks button | Agent calls programmatically |
| **Control** | User-controlled | Agent-controlled |
| **Execution** | Sequential, one at a time | Can be parallel or sequential |
| **Visibility** | Handoff buttons in UI | Hidden from user |
| **Context** | User can modify prompt | Agent passes context directly |
| **Review** | User reviews each step | No user review between steps |
| **Interruption** | User can stop between steps | Runs until completion |
| **Output** | Separate outputs per agent | Single aggregated output |
| **Parallelism** | Not supported | Can run multiple subagents |
| **Use case** | Human-in-the-loop workflows | Automated orchestration |
| **Configuration** | `handoffs:` in frontmatter | `agents:` + `'agent'` tool |

## When to Use Each Pattern

### Use Handoffs When:

✅ You want **human-in-the-loop** workflows
✅ Each step requires **user review or approval**
✅ Users need to **modify context** between steps
✅ The workflow is **exploratory** (user decides next step)
✅ You want to **teach** users a multi-step process
✅ **Sequential** execution is required (A → B → C)
✅ Users need **visibility** into each agent's work

**Examples:**
- Planning → Review → Implementation
- Architecture Review → Create Stories → Estimate Work
- Generate Tests → Review Tests → Implement Features
- Security Scan → Fix Issues → Re-scan

### Use Subagents When:

✅ You need **automated orchestration** without user intervention
✅ One agent requires **specialized analysis** from others
✅ You want to **aggregate** multiple perspectives into one report
✅ **Parallel execution** could improve efficiency
✅ The workflow is **well-defined** and repeatable
✅ Users want **one comprehensive result**, not separate outputs
✅ You're building a **meta-agent** that coordinates specialists

**Examples:**
- Code review that checks security, performance, and architecture
- Comprehensive analysis combining multiple specialized agents
- Feature implementation that auto-generates tests and docs
- Multi-perspective assessment (technical, business, security)

### Use Both Together

You can combine patterns:

```yaml
---
name: "comprehensive-reviewer"
tools: ['read', 'search', 'agent']
agents:
  - security-reviewer
  - architecture-reviewer
handoffs:
  - label: Create Backlog
    agent: backlog-generator
    prompt: "Based on the comprehensive review above, create implementation stories"
---

# Comprehensive Reviewer

First, I will invoke specialized review subagents:
- @security-reviewer for security analysis
- @architecture-reviewer for design review

Then I'll synthesize findings into a comprehensive report.

Users can then use the handoff to create a backlog from my findings.
```

## Examples

### Example 1: Architecture Reviewer with Handoffs

```yaml
---
name: "architecture-reviewer"
description: 'Reviews architecture and suggests next steps'
tools: ['read', 'search/changes']
handoffs:
  - label: Create User Stories
    agent: backlog-generator
    prompt: |-
      Based on the architectural review above, create user stories for implementing:
      - Architectural violations and required fixes
      - Recommended refactoring approach
      - Component boundaries and responsibilities
      Include priority and risk assessment.
    send: false
  - label: Implement Changes
    agent: default
    prompt: |-
      Implement the architectural changes recommended above.
      Follow Clean Architecture principles and include tests.
    send: false
  - label: Plan Test Strategy
    agent: test-strategist
    prompt: |-
      Create a test strategy for the architectural changes above.
    send: false
---

# Architecture Reviewer

I review code for Clean Architecture and DDD compliance.

After my review, you can:
- Create user stories for fixes
- Implement changes immediately
- Plan test strategy
```

### Example 2: Orchestrator with Subagents

```yaml
---
name: "comprehensive-planner"
description: 'Creates comprehensive implementation plan using multiple perspectives'
tools: ['read', 'search', 'agent']
agents:
  - architecture-reviewer
  - test-strategist
  - backlog-generator
model: Claude Sonnet 4.5
---

# Comprehensive Planner

I coordinate multiple specialized agents to create a complete implementation plan.

## My Process

1. **Architecture Analysis**: Invoke @architecture-reviewer to identify design concerns
2. **Test Planning**: Invoke @test-strategist to define test requirements
3. **Work Breakdown**: Invoke @backlog-generator to create user stories
4. **Synthesis**: Combine all analyses into a prioritized roadmap

## Output

I provide a single comprehensive report containing:
- Architecture assessment and recommendations
- Test strategy and coverage requirements
- Prioritized backlog with story estimates
- Implementation roadmap with dependencies
- Risk assessment and mitigation strategies

I run all analyses automatically - you receive one complete plan.
```

### Example 3: Combined Pattern

```yaml
---
name: "feature-planner"
description: 'Plans feature implementation with option to start coding'
tools: ['read', 'search', 'agent']
agents:
  - test-strategist
  - architecture-reviewer
handoffs:
  - label: Start Implementation
    agent: default
    prompt: "Implement the feature plan above, starting with tests."
    send: true
---

# Feature Planner

I create detailed feature implementation plans by coordinating specialists.

## My Workflow

First, I automatically:
1. Invoke @architecture-reviewer to assess design impact
2. Invoke @test-strategist to plan test approach
3. Synthesize findings into implementation plan

Then you can:
- Review the complete plan
- Click "Start Implementation" to begin coding immediately
- Ask questions or request modifications
```

## Best Practices

### For Handoffs

1. **Keep prompts contextual**: Reference "above" to carry context forward
2. **Use `send: false` by default**: Let users review before proceeding
3. **Provide clear labels**: Button text should indicate the action
4. **Limit handoff options**: 2-4 handoffs max to avoid overwhelming users
5. **Order by likelihood**: Most common next step first

### For Subagents

1. **Specify allowed agents**: Don't use `'*'` unless necessary
2. **Include 'agent' tool**: Required in `tools` array
3. **Clear instructions**: Explain when and how to invoke each subagent
4. **Wait for completion**: Don't invoke multiple subagents simultaneously unless needed
5. **Synthesize results**: Combine subagent outputs into coherent response
6. **Handle failures**: Provide guidance if subagent unavailable

### For Both

1. **Document the workflow**: Explain the orchestration pattern in agent description
2. **Test thoroughly**: Verify context passes correctly between agents
3. **Consider cost**: Multiple agent invocations increase token usage
4. **Maintain boundaries**: Each agent should have clear, distinct responsibilities
5. **Version control**: Track agent file changes in repository

## Troubleshooting

### Handoffs Not Appearing

- Check YAML frontmatter syntax
- Verify target agent exists
- Ensure `label`, `agent`, and `prompt` are specified
- Check agent file is in correct location (`.github/agents/`)

### Subagent Invocation Fails

- Verify `'agent'` tool included in `tools` array
- Check target agent listed in `agents` array
- Ensure target agent file exists and is valid
- Verify no circular dependencies (A calls B, B calls A)

### Context Not Flowing

- For handoffs: Use "above" or "previous review" in prompt
- For subagents: Explicitly pass context in invocation instructions
- Check prompt length limits

## References

- [VS Code Custom Agents Documentation](https://code.visualstudio.com/docs/agent-customization/custom-agents)
- [VS Code Handoffs Documentation](https://code.visualstudio.com/docs/agent-customization/custom-agents#handoffs)
- [VS Code Subagents Documentation](https://code.visualstudio.com/docs/agents/subagents)
- [Agent Design Guide](./agent-design-guide.md)
- [Custom Agent Catalog](./custom-agent-catalog.md)

---

**Last Updated:** 2026-07-23
