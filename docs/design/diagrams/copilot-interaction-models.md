# Copilot Interaction Models

This diagram illustrates the three agent modes in GitHub Copilot (VS Code) and when to use each. Select a mode from the **Agent mode** picker below the chat box.

> **Separate controls:** *Agent mode* decides **how** Copilot works, *Permissions* decides **what** it may do without asking, and the *Agent picker* decides **who** does the work (custom agents). The former Ask and Edit modes are gone; a question is simply a prompt in Interactive mode.

## Interaction Models Overview

```mermaid
graph TB
    User[Developer] --> Decision{What do I need?}

    Decision -->|Learn, explore, steer each step| Interactive[Interactive]
    Decision -->|Design approach first| Plan[Plan]
    Decision -->|Delegate a defined task| Autopilot[Autopilot]

    Interactive --> InteractiveResult[Answers and changes<br/>You approve each step]
    Plan --> PlanResult[Multi-step approach<br/>Requirements gathered<br/>No code yet]
    Autopilot --> AutopilotResult[Autonomous execution<br/>Within Permissions<br/>Review afterward]

    style Interactive fill:#e1f5e1
    style Plan fill:#fff4e1
    style Autopilot fill:#e1e8ff
```

## Mode Comparison

```mermaid
graph LR
    subgraph "Interactive"
        I1[Request] --> I2[Analyze Context]
        I2 --> I3[Propose Step]
        I3 --> I4{You Review}
        I4 -->|Approve| I5[Apply Step]
        I4 -->|Redirect| I3
        I5 --> I6{More Steps?}
        I6 -->|Yes| I3
        I6 -->|No| I7[Done]
    end

    subgraph "Plan"
        P1[Request] --> P2[Gather Requirements]
        P2 --> P3[Design Approach]
        P3 --> P4[Structured Plan]
        P4 -.No Code Yet.-> P5[Awaiting Approval]
    end

    subgraph "Autopilot"
        A1[Request or Approved Plan] --> A2[Analyze Context]
        A2 --> A3[Execute Steps Autonomously]
        A3 --> A4{Needs Permission?}
        A4 -->|Yes| A5[Ask You]
        A5 --> A3
        A4 -->|No| A3
        A3 --> A6[Complete]
        A6 --> A7[You Review the Diff]
    end
```

## Decision Tree: Which Mode to Use?

```mermaid
flowchart TD
    Start([Need Copilot Help]) --> Q1{Is the approach<br/>and scope clear?}

    Q1 -->|No| UsePlan[Use Plan]
    Q1 -->|Yes| Q2{Is the task well defined,<br/>verifiable, and easy to undo?}

    Q2 -->|Yes| UseAutopilot[Use Autopilot]
    Q2 -->|No / want control| UseInteractive[Use Interactive]

    UseInteractive --> InteractiveEx[Examples:<br/>- Explain pattern<br/>- How does X work?<br/>- Small, focused edits]
    UsePlan --> PlanEx[Examples:<br/>- Gather requirements<br/>- Validate strategy<br/>- Clarify scope before coding]
    UseAutopilot --> AutopilotEx[Examples:<br/>- Execute an approved plan<br/>- Multi-file refactor with tests<br/>- Repeatable, verifiable changes]

    style UseInteractive fill:#e1f5e1
    style UsePlan fill:#fff4e1
    style UseAutopilot fill:#e1e8ff
```

## Interaction Characteristics

| Characteristic | Interactive | Plan | Autopilot |
|----------------|-------------|------|-----------|
| **Primary Purpose** | Collaborate step by step | Design & Clarify | Execute autonomously |
| **Output Type** | Answers + approved changes | Structured Plan | Completed changes |
| **Code Generated** | Yes, as you approve | None (plan only) | Yes, within Permissions |
| **Scope** | Focused to multi-file | Requirements gathering | Multi-file, repository-level |
| **Control Level** | Highest (every step) | High (approve plan) | Lowest (review afterward) |
| **Speed** | Deliberate | Fast | Fastest end to end |
| **Best For** | Learning, focused edits | Strategy validation | Well-defined implementation |

## Usage Patterns

### Interactive Flow

```mermaid
sequenceDiagram
    participant D as Developer
    participant C as Copilot

    D->>C: Question or request
    C->>C: Analyze context
    C->>D: Explanation or proposed step
    D->>C: Approve / redirect
    C->>D: Apply step and show result
    Note over D: You steer every step
```

### Plan Flow

```mermaid
sequenceDiagram
    participant D as Developer
    participant C as Copilot

    D->>C: Request with intent
    C->>D: Clarifying questions
    D->>C: Answers / requirements
    C->>C: Design multi-step approach
    C->>D: Structured plan
    D->>D: Review & approve strategy
    Note over D: No code written yet
```

### Autopilot Flow

```mermaid
sequenceDiagram
    participant D as Developer
    participant A as Copilot (Autopilot)
    participant R as Repository

    D->>A: Defined task or approved plan
    A->>R: Analyze codebase

    loop Until complete
        A->>R: Execute step
        opt Action exceeds Permissions
            A->>D: Request approval
            D->>A: Approve / deny
        end
    end

    A->>D: Summary of changes
    D->>R: Review diff, run tests, keep or discard
```

## Key Principle

> **Autonomy is a dial, not a switch**
> Choose the least autonomy that fits the job. Plan first when unsure, then hand off to Autopilot with Permissions you are comfortable with.

## When Plan Shines

✅ Requirements are unclear and need clarifying before coding  
✅ You want to validate the approach before committing  
✅ Complex feature with multiple design options  
✅ Onboarding — understanding what needs to change before changing it  

## When Autopilot Shines

✅ Executing an approved plan  
✅ Multi-file refactoring covered by tests  
✅ Repeatable, easy-to-verify changes on a branch  
✅ Work where review afterward is cheaper than approving each step  

---

## See Also

- [Lab 05: Interaction Models](../../labs/lab-05-interaction-models.md)
- [Agent Workflow Patterns](agent-workflow-patterns.md)
- [Agent vs Instructions vs Prompts](agent-vs-instructions-vs-prompts.md)
