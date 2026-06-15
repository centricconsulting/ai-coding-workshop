---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

<!-- _class: lead -->

# Module 5

## Designing Effective Agents
### Principles, Properties & Patterns

**Duration:** 30 minutes

---

# Core Principle

> Agents are products, not prompts

Design, test, and maintain them **like code**

---

<!-- _class: small -->
<style scoped>
section { font-size: 1.00em; }
h1 { font-size: 1.8em; }
</style>

# Agent Components

## Core Instructions:
1. **Identity & Role** - Who is this agent?
2. **Responsibilities** - What does it do?
3. **Context** - What does it need to know?
4. **Constraints** - Rules it must follow
5. **Process** - How it approaches tasks

---

6. **Output Format** - Structured results
7. **Tone** - Communication style

## YAML Frontmatter (NEW):
- `user-invocable`, `disable-model-invocation`
- `agents`, `argument-hint`, `handoffs`

---

# Design Pattern: Role-Based Scope

✅ **Do:** "You are a code reviewer specializing in security"

❌ **Don't:** "Generate code for feature X"

**Focus on WHO, not WHAT**

---

# Design Pattern: Explicit Constraints

✅ **Do:**
```markdown
## Constraints
- ALWAYS check for circular dependencies
- NEVER recommend breaking layer boundaries
```

❌ **Don't:** Leave assumptions unstated

---

# Design Pattern: Structured Outputs

✅ **Do:** Define sections and format

```markdown
## Output Format

### Review Summary
- **Scope:** [what was reviewed]
- **Assessment:** [Pass/Needs Attention/Refactor]

### Findings
...
```

❌ **Don't:** Allow free-form responses

---

# Iteration Loop

```
Define → Test → Observe → Refine → Repeat
```

**Example refinement:**
- Agent over-tests simple getters
- Add constraint: "Focus on high-value tests only"
- Re-test with same scenario
- Observe improved behavior

---

<!-- _class: small -->
<style scoped>
section { font-size: 0.95em; }
h1 { font-size: 1.8em; }
</style>

# Advanced Agent Properties

## user-invocable
- `true`: Visible in dropdown (default)
- `false`: Hidden, only for subagents

## disable-model-invocation
- `true`: Prevents auto-invocation by other agents
- `false`: Callable as subagent (default)

---

## handoffs
- Sequential workflow buttons
- Guide users through processes
- Human-in-the-loop between steps

---

# Handoffs: Orchestrated Workflows

```yaml
handoffs:
  - label: "Start Implementation"
    agent: "implementer"
    prompt: "Implement the plan above"
    send: false  # Wait for user approval
```

**Use cases:**
- Plan → Implement → Review
- Generate Tests → Make Pass
- Architecture → Documentation

**Key:** `send: false` keeps human in the loop

---

# Governance Considerations

## Versioning
- Track changes in git
- Semantic versioning for major updates

## Review Process
- Agent **and skill** changes require PR review
- Test before merging

---

## Team Alignment
- Agents/Skills encode **team decisions**
- Update as practices evolve

---

# Common Pitfalls

❌ **Task-based agents** → Use role-based  
❌ **Vague instructions** → Be explicit  
❌ **Over-scoping** → Keep focused  
❌ **No testing** → Validate before sharing  
❌ **Set-and-forget** → Iterate continuously

---

<!-- _class: lead -->

# Hands-On Time

**Lab Guide:** [Lab 09: Agent Design](../../../labs/lab-09-agent-design.md)

**Next Module:** [Capstone Lab](06-capstone-lab.md)

**Previous Module:** [Workflow Agents](04-workflow-agents.md)
