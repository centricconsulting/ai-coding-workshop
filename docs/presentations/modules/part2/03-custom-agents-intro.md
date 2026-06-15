---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

<!-- _class: lead -->

# Module 3

## Custom Copilot Agents
### Workflows with Tool Access

**Duration:** 25 minutes

---

# What Are Custom Agents?

**Named chat participants** with specific roles

- Selectable from agent dropdown
- Role-based AI personas (e.g., Architecture Reviewer)
- Defined scope and constraints
- Structured, consistent outputs
- Encode team knowledge

---

# Mental Model: The Specialist

```
Standard Copilot Chat = General AI Assistant

Custom Agent = Domain Expert Consultant
```

You wouldn't ask a general assistant to:
- Review architecture → Ask an architect
- Plan testing → Ask a QA specialist  
- Generate backlog → Ask a product analyst

**Custom agents ARE those specialists**

---

# The Complete Hierarchy

| Feature | Prompts | Instructions | Skills | Agents |
|---------|---------|--------------|--------|--------|
| **Scope** | One-off | Always-on | On-demand | On-demand |
| **Invocation** | Chat | Automatic | `#name` | `@name` |
| **Tool Access** | ❌ | ❌ | ❌ | ✅ |
| **Purpose** | Question | Guardrails | Knowledge | Workflow |
| **Discovery** | N/A | N/A | `/skills` | `/agents` |

---

# When to Use Custom Agents

✅ **Use agents for:**
- Repeated **workflows** (reviews, planning)
- Tasks requiring **file/codebase access**
- **Multi-step orchestration**
- Validation and review tasks

❌ **Don't use agents for:**
- Template generation (use Skills)
- Simple questions (use Prompts)
- Always-on rules (use Instructions)
- Knowledge without actions

---

# Workshop Agents

## Architecture Reviewer
Reviews code for Clean Architecture & DDD compliance

## Backlog Generator
Creates user stories with acceptance criteria

## Test Strategist
Proposes comprehensive test strategies

---

# Guided Exercise

**Try the Architecture Reviewer agent:**

1. Open Copilot Chat in Agent Mode
2. Select "Architecture Reviewer" from dropdown
3. Prompt: "Review the Task domain model"
4. Compare to standard Copilot Chat output

**Observe:**
- Structured format
- Consistency
- Depth of analysis

---

# Key Insight

> **Instructions** = Guardrails  
> **Skills** = Knowledge base  
> **Agents** = Specialists you consult  

All three work together!

---

<!-- _class: lead -->

# Hands-On Time

**Lab Guide:** [Lab 07: Custom Agents Intro](../../labs/lab-07-custom-agents-intro.md)

**Next Module:** [Workflow Agents](04-workflow-agents.md)

**Previous Module:** [Skills & Customization](02-skills-customization.md)
