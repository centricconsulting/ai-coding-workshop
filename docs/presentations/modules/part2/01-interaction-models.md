---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

<!-- _class: lead -->

# Module 1

## Copilot Interaction Models
### Interactive, Plan, Autopilot

**Duration:** 25 minutes

---

# Three Agent Modes

Select from the **Agent mode** picker below the chat box.

## Interactive
**Purpose:** Step-by-step collaboration  
**Result:** Answers and changes, with you steering each step

## Plan
**Purpose:** Plan first, execute when ready  
**Result:** Structured plan awaiting your approval

## Autopilot
**Purpose:** Works autonomously within permissions  
**Result:** Completed work for you to review afterward

---

# Three Separate Controls

| Control | Answers the question | Examples |
|---------|----------------------|----------|
| **Agent mode** | *How* does Copilot work? | Interactive, Plan, Autopilot |
| **Permissions** | *What* may it do without asking? | Manual (default) and others |
| **Agent picker** | *Who* is doing the work? | Custom agents such as `plan`, `architecture-reviewer` |

> The old Ask / Edit / Agent split is gone. A question is simply a prompt in Interactive mode.

---

# Interactive Mode: When to Use

✅ **Use when:**
- Asking questions and exploring code
- Learning patterns and getting explanations
- Small or focused changes
- You want to approve each step

❌ **Don't use when:**
- A large task needs little supervision
- You already have an approved plan to execute

---

# Plan Mode: When to Use

✅ **Use when:**
- You want to understand the approach before any code is written
- Requirements need clarifying before implementation
- Complex changes spanning multiple files
- You want to validate the strategy first

❌ **Don't use when:**
- You need a quick direct answer
- You're ready to execute immediately

---

# Autopilot Mode: Discussion

*We will discuss Autopilot, but we won't use it in the labs.*

✅ **Fits when:**
- The task is well defined, ideally from an approved plan
- Work is repeatable and easy to verify (tests, builds)
- Work is on a branch you can review and revert

❌ **Don't use when:**
- Requirements are unclear
- Changes are risky or hard to undo
- You are still learning the codebase

**Key:** Autopilot acts *within permissions*. Review the result like a pull request.

---

# Demo: Same Task, Two Ways

**Task:** Add Priority property to Task entity

1. **Interactive** → Ask, then approve each step
2. **Plan** → Requirement gathering + multi-step design, then implement the plan step by step

**Observe the differences in:**
- Autonomy
- Control
- Visibility
- Review effort

---

# Key Takeaway

> Autonomy is a dial, not a switch  
> Choose the **least autonomy** that fits the job

Plan first, then hand off to Autopilot with **permissions** you are comfortable with

---

<!-- _class: lead -->

# Hands-On Time

**Lab Guide:** [Lab 05: Interaction Models](../../../labs/lab-05-interaction-models.md)

**Next Module:** [Skills & Customization](02-skills-customization.md)

**Previous Module:** [Welcome & Recap](00-welcome-recap.md)
