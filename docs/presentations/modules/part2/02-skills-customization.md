---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

<!-- _class: lead -->

# Module 2

## Skills & Customization Hierarchy
### Four Ways to Customize Copilot

**Duration:** 30 minutes ⭐ NEW

---

# The Customization Hierarchy

```
4 Types (Specific → General)

1. Prompts      → One-off, in-chat
2. Instructions → Always-on guardrails
3. Skills       → Domain knowledge (#skill-name)
4. Agents       → Workflows with tools (@agent-name)
```

**Key principle:** Use the simplest level that solves your problem

---

# What Are Skills?

**Skills = Domain Expertise Without Tool Access**

- Portable knowledge modules
- Invoked with `#skill-name`
- Provide templates, patterns, examples
- **No file access** - pure knowledge
- Discovered via `/skills` command

**Example:** `#test-data-generator`

---

# Skills vs Agents

| Aspect | Skills | Agents |
|--------|--------|--------|
| **Purpose** | Knowledge & templates | Workflows & actions |
| **Tool Access** | ❌ None | ✅ Read/write files |
| **Invocation** | `#skill-name` | `@agent-name` |
| **Best For** | Patterns, examples | Multi-step tasks |
| **Discovery** | `/skills` | `/agents` |

---

# When to Use Each Type

**Prompts:** One-time question  
**Instructions:** Team coding standards  
**Skills:** Generate templates/patterns  
**Agents:** Orchestrate multi-step workflows

**Decision tree:**
1. Need to make changes? → Agent
2. Need templates/knowledge? → Skill
3. Always-on rule? → Instructions
4. One-off question? → Prompt

---

# Slash Commands for Discovery

```bash
/help              # Show all commands
/agents            # List available agents
/skills            # List available skills
/init              # Start new project
/create-workspace  # Create workspace
/create-notebook   # Create Jupyter notebook
/create-file       # Create file with AI
```

**Try it:** Type `/skills` in Copilot Chat now

---

# Key Takeaway

> Skills provide **knowledge**  
> Agents provide **action**  
> Choose based on what you need

**Most confusion:** Skills vs Agents  
**Remember:** Does it need to read/write files?
- Yes → Agent
- No → Maybe a Skill

---

<!-- _class: lead -->

# Hands-On Time

**Lab Guide:** [Lab 06: Skills & Customization](../../labs/lab-06-skills-and-customization.md)

**Next Module:** [Custom Agents Intro](03-custom-agents-intro.md)

**Previous Module:** [Interaction Models](01-interaction-models.md)
