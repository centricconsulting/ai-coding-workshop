# Lab 10: Capstone - Build a Production-Ready Agent

**Module:** 5  
**Duration:** 35 minutes  
**Part:** Advanced GitHub Copilot (Part 2)

## Objectives

By the end of this lab, you will:
- Create a functional custom agent from scratch
- Apply all design principles learned in previous labs
- Test and iterate on agent behavior
- Prepare an agent for team use with clear documentation
- **[Optional Extension]** Create a complementary skill to enhance your agent's capabilities

## Prerequisites

- Completion of [Lab 09: Agent Design](lab-09-agent-design.md)
- VS Code with GitHub Copilot extension
- Access to the TaskManager workshop repository (🔷 .NET or 🟩 Spring Boot version)
- Understanding of agent components and design patterns
- Familiarity with your chosen technology stack (.NET or Java/Spring Boot)
- **[For Optional Extension]** Completion of [Lab 06: Skills & Customization](lab-06-skills-and-customization.md)

> **Note**: This lab uses 🔷 for .NET examples and 🟩 for Spring Boot examples where stack-specific context is helpful. Most of the lab is technology-agnostic, focusing on agent design principles.

## Overview

In this capstone lab, you'll **build your own custom agent** aligned to a real workflow. You'll define success criteria, create the agent definition, test it, iterate, and prepare it for team use.

---

## Step 1: Select Your Agent Role (5 minutes)

Choose **one** of these agent roles (or propose your own):

### Option A: Code Reviewer
**Role:** Reviews code for quality, standards, and best practices  
**Workflow:** Pull request reviews, pre-commit checks  
**Value:** Consistent code quality across team members

**Example Focus Areas:**
- 🔷 .NET: Sealed classes, async/await patterns, ILogger usage, guard clauses
- 🟩 Spring Boot: @Service annotations, exception handling, SLF4J logging, proper DI

### Option B: Documentation Writer
**Role:** Generates or reviews documentation for clarity and completeness  
**Workflow:** API docs, README updates, architectural decision records  
**Value:** Consistent documentation standards

### Option C: Performance Auditor
**Role:** Identifies performance issues and optimization opportunities  
**Workflow:** Reviewing slow endpoints, database queries, algorithms  
**Value:** Proactive performance monitoring

**Example Focus Areas:**
- 🔷 .NET: EF Core query optimization, async/await overhead, collection allocations
- 🟩 Spring Boot: N+1 queries, lazy loading, connection pool tuning, JPA optimization

### Option D: Security Reviewer
**Role:** Reviews code for security vulnerabilities and best practices  
**Workflow:** Pre-deployment security checks, sensitive data handling  
**Value:** Reduced security risks

**Example Focus Areas:**
- 🔷 .NET: SQL injection, XSS, CSRF, authentication/authorization patterns
- 🟩 Spring Boot: SQL injection, Spring Security configuration, Jackson deserialization, CORS

### Option E: Your Custom Agent
**Role:** [Define your own based on team needs]  
**Workflow:** [Describe the workflow]  
**Value:** [What problem does it solve?]

### Decision

**I will create:** [Agent name and role]

---

## Step 2: Define Success Criteria (5 minutes)

Before writing instructions, define what "good" looks like for your agent.

### Success Criteria Template

**My agent is successful when:**
1. [Criterion 1 - e.g., "It identifies all security vulnerabilities in sample code"]
2. [Criterion 2 - e.g., "It provides actionable remediation steps"]
3. [Criterion 3 - e.g., "It doesn't flag false positives for safe patterns"]
4. [Criterion 4 - e.g., "Output is structured and consistent"]
5. [Criterion 5 - e.g., "It can be used by any team member with the same results"]

### Test Scenarios

**I will test my agent with:**
1. [Test scenario 1 - e.g., "Code with SQL injection vulnerability"]
2. [Test scenario 2 - e.g., "Code with proper input validation"]
3. [Test scenario 3 - e.g., "Edge case or boundary condition"]

---

## Step 3: Create the Agent Definition (15 minutes)

Create a new file: `.github/agents/[your-agent-name].agent.md`

Use this template and customize for your agent:

```markdown
---
name: "[agent-name]"
description: '[Brief description of what this agent does]'
tools: [changes]  # or [] if no tools needed
model: Claude Sonnet 4
---

# [Agent Name]

You are an expert [role/specialty] with deep knowledge of [domain expertise].

## Responsibilities

- [Responsibility 1]
- [Responsibility 2]
- [Responsibility 3]
- [Responsibility 4]

## Context

This project follows [relevant context about the codebase, standards, patterns]:
- [Context point 1]
- [Context point 2]
- [Context point 3]

**For stack-specific agents, include:**
- 🔷 .NET: Clean Architecture, CQRS pattern, sealed classes, async/await, ILogger, xUnit + FakeItE asy
- 🟩 Spring Boot: Clean Architecture, Service layer pattern, @Service/@Repository, SLF4J, JUnit 5 + Mockito

## Constraints

- ALWAYS [critical rule the agent must follow]
- NEVER [something the agent must avoid]
- [Additional constraint]
- [Additional constraint]

## Analysis Process

1. [Step 1 of how the agent should approach the task]
2. [Step 2]
3. [Step 3]
4. [Step 4]

## Output Format

Provide your [review/analysis/report] in this structured format:

### [Section 1 Title]
- **[Field]:** [description]
- **[Field]:** [description]

### [Section 2 Title]

[Description of what goes in this section]

### [Section 3 Title]

[Description of what goes in this section]

## Tone

- [Tone guideline 1 - e.g., "Be direct and constructive"]
- [Tone guideline 2 - e.g., "Explain WHY, not just WHAT"]
- [Tone guideline 3 - e.g., "Acknowledge good practices"]

## Examples of What to Flag

**Adjust based on your agent role. Here are examples for a Code Reviewer:**

### 🔷 .NET Examples
- Missing `sealed` keyword on classes that shouldn't be inherited
- Synchronous I/O in async methods (blocking calls)
- Missing guard clauses or using nested `if` statements instead
- String interpolation in logging statements (should use structured logging)
- Not using `nameof()` operator in exceptions
- Missing `CancellationToken` parameters in async methods

### 🟩 Spring Boot Examples
- Missing `@Service` or `@Repository` annotations
- Not using `@Transactional` on service methods that modify data
- Empty catch blocks that swallow exceptions
- Not using `@Slf4j` or structured logging
- Missing `@RequiredArgsConstructor` for constructor injection
- Using `Optional` as method parameters (anti-pattern)
- Not handling `EntityNotFoundException` in service layer
```

### Instructions

1. Create the file `.github/agents/[your-agent-name].agent.md`
2. Fill in all sections based on your chosen role
3. Be specific in constraints and output format
4. Include examples where helpful

---

## Step 4: Test Your Agent (10 minutes)

### Test Procedure

1. **Open Copilot Chat** in Agent Mode
2. **Select your custom agent** from the dropdown
3. **Run each test scenario** you defined in Step 2
4. **Record the results:**
   - Did the agent follow the output format?
   - Did it meet your success criteria?
   - Were there unexpected behaviors?
   - What worked well? What didn't?

### Example Test Scenarios

**For Code Reviewer Agent:**

#### 🔷 .NET Test Scenario
```csharp
// Test file with intentional issues
public class TaskService  // Missing 'sealed'
{
    public async Task<Task> GetTaskAsync(Guid id)  // Missing CancellationToken
    {
        if (id == Guid.Empty)
            throw new ArgumentException("Invalid ID");  // Missing nameof()
        
        var result = _repository.GetAsync(id).Result;  // Blocking in async!
        _logger.LogInformation($"Found task {id}");  // String interpolation in logging
        return result;
    }
}
```

#### 🟩 Spring Boot Test Scenario
```java
// Test file with intentional issues
public class TaskServiceImpl {  // Missing @Service annotation
    
    private TaskRepository taskRepository;  // Should use @RequiredArgsConstructor
    
    public Task getTaskById(UUID id) {  // Missing Optional return type
        if (id == null) {
            throw new IllegalArgumentException("ID cannot be null");
        }
        
        try {
            return taskRepository.findById(id).get();  // Unsafe get()
        } catch (Exception e) {
            // Empty catch block - swallowed exception!
            return null;
        }
    }
}
```

###Test Results Template

**Test 1:** [Scenario description]  
- **Agent Output:** [Summary or excerpt]  
- **Success Criteria Met:** [Yes/No - which ones?]  
- **Issues Found:** [List any problems]

**Test 2:** [Scenario description]  
- **Agent Output:** [Summary or excerpt]  
- **Success Criteria Met:** [Yes/No - which ones?]  
- **Issues Found:** [List any problems]

**Test 3:** [Scenario description]  
- **Agent Output:** [Summary or excerpt]  
- **Success Criteria Met:** [Yes/No - which ones?]  
- **Issues Found:** [List any problems]

---

## Step 5: Iterate and Refine (5 minutes)

Based on your test results, refine your agent:

### Common Issues and Fixes

| Issue | Likely Cause | Fix |
|-------|--------------|-----|
| Output format inconsistent | Vague instructions | Add explicit structure with headings |
| Agent goes out of scope | No constraints | Add "NEVER" constraints |
| Too verbose | No guidance on brevity | Add tone guidance: "Be concise" |
| Misses important checks | Missing from responsibilities | Add to responsibilities list |
| False positives | Overly broad rules | Add examples of acceptable patterns |

### Refinement Process

1. Identify the top 2-3 issues from testing
2. Update your agent's instructions
3. **Re-test** with the same scenarios
4. Repeat until success criteria are met

---

## Step 6: Document Your Agent (5 minutes)

Create a brief usage guide for your agent:

### Agent Documentation Template

**Agent Name:** [Your agent name]

**Purpose:** [One-sentence description]

**When to Use:**
- [Use case 1]
- [Use case 2]
- [Use case 3]

**When NOT to Use:**
- [Anti-pattern 1]
- [Anti-pattern 2]

**Example Prompts:**
```
[Example prompt 1]
[Example prompt 2]
[Example prompt 3]
```

**Expected Output:**
- [What users should expect]
- [Output structure/format]

**Limitations:**
- [Limitation 1]
- [Limitation 2]

**Maintenance:**
- **Owner:** [Your name or team]
- **Last Updated:** [Date]
- **Review Frequency:** [e.g., Quarterly]

---

## 🌟 Optional Extension: Add a Complementary Skill (10 minutes)

**Note:** This is **bonus work** for participants who finish the core agent exercise early or want to explore the Skills system further.

### When to Create a Skill vs. Agent

Refer back to [Lab 06: Skills & Customization](lab-06-skills-and-customization.md) decision guide:

- **Agent**: Workflows, orchestration, delegation, runtime tool access
- **Skill**: Specialized knowledge, templates, reusable patterns, documentation

### Skill Ideas for Your Agent

Depending on which agent you built, consider these complementary skills:

| Agent Type | Complementary Skill |
|------------|---------------------|
| **Code Reviewer** | Code review checklist generator (produces context-specific review templates) |
| **Documentation Writer** | Documentation template library (README, ADR, API doc templates) |
| **Performance Auditor** | Performance test data generator (creates load test scenarios) |
| **Security Reviewer** | Security test case generator (OWASP Top 10 test scenarios) |

### Steps to Create Your Skill

#### 1. Define Skill Purpose

Your skill should:
- **Complement** your agent (not duplicate it)
- Provide **reusable knowledge or templates**
- Be **domain-specific** (not general-purpose)

**Example:** If you built a Security Reviewer agent, a complementary skill might generate security test cases based on vulnerability patterns.

---

#### 2. Create the Skill File

Create: `.github/skills/[skill-name]/SKILL.md`

**Template:**

```markdown
---
name: "[skill-name]"
description: '[Brief description of what this skill provides]'
argument-hint: '[What input does the skill expect?]'
user-invocable: true  # Make it appear in skills list
---

# [Skill Name]

You are a specialized skill that provides [domain expertise or capability].

## Purpose

This skill helps developers by:
- [Value proposition 1]
- [Value proposition 2]
- [Value proposition 3]

## Scope

**IN SCOPE:**
- [What this skill covers]
- [What this skill covers]

**OUT OF SCOPE:**
- [What this skill does NOT do - refer to agents for this]
- [What this skill does NOT do]

## Instructions

When invoked, you will:

1. [Step 1 - e.g., "Analyze the provided code/context"]
2. [Step 2 - e.g., "Identify applicable patterns from your knowledge base"]
3. [Step 3 - e.g., "Generate output in specified format"]
4. [Step 4 - e.g., "Provide usage guidance"]

## Knowledge Base

### [Category 1 - e.g., "Common Vulnerability Patterns"]

**Pattern:** [Pattern name]
- **Description:** [What it is]
- **Detection:** [How to identify it]
- **Test Case:** [How to test for it]

**Pattern:** [Pattern name]
- **Description:** [What it is]
- **Detection:** [How to identify it]
- **Test Case:** [How to test for it]

#### Example: Security Patterns by Stack

**🔷 .NET Security Patterns**

**Pattern:** Injection Vulnerabilities in LINQ/EF Core
- **Description:** Raw SQL or unsanitized string concatenation in queries
- **Detection:** Look for `FromSqlRaw()` with string interpolation, or concatenated SQL
- **Test Case:** Pass malicious input like `'; DROP TABLE Tasks--` to query parameters

**Pattern:** Missing Input Validation in Minimal APIs
- **Description:** Route handlers accepting DTOs without validation attributes or FluentValidation
- **Detection:** Check for missing `[Required]`, `[StringLength]`, or validator registration
- **Test Case:** Send requests with null, oversized, or malformed data

**Pattern:** Exposed Sensitive Data in Logging
- **Description:** Logging passwords, tokens, or PII in structured logs
- **Detection:** Search for `ILogger.LogInformation()` calls with sensitive parameters
- **Test Case:** Trigger log statements and verify sensitive data doesn't appear

**🟩 Spring Boot Security Patterns**

**Pattern:** SQL Injection in JPA Queries
- **Description:** Using string concatenation in `@Query` annotations or native queries
- **Detection:** Look for `@Query(value = "SELECT * FROM tasks WHERE id = " + id)`
- **Test Case:** Pass `1 OR 1=1--` as ID parameter and verify it's parameterized safely

**Pattern:** Missing @Valid on Request Bodies
- **Description:** REST controllers accepting `@RequestBody` without `@Valid` annotation
- **Detection:** Check controller methods for `@RequestBody` without `@Valid`
- **Test Case:** POST invalid data (missing required fields) and verify 400 Bad Request

**Pattern:** Exposed Stack Traces in Production
- **Description:** Exception handlers returning full stack traces to clients
- **Detection:** Look for `@ExceptionHandler` methods that include `e.printStackTrace()` or `e.getStackTrace()`
- **Test Case:** Trigger an exception and verify response doesn't leak internal details

#### Example: Performance Patterns by Stack

**🔷 .NET Performance Patterns**

**Pattern:** N+1 Query Problem in EF Core
- **Description:** Loading related entities in loops instead of using `.Include()`
- **Detection:** Look for foreach loops with individual repository/DbContext calls
- **Test Case:** Enable SQL logging, execute query, count database round-trips

**Pattern:** Missing Async in I/O Operations
- **Description:** Synchronous database/HTTP calls blocking thread pool
- **Detection:** Search for `.Result`, `.Wait()`, `.GetAwaiter().GetResult()` in async contexts
- **Test Case:** Load test endpoint, monitor thread pool starvation

**🟩 Spring Boot Performance Patterns**

**Pattern:** N+1 Query Problem in JPA
- **Description:** Lazy loading causing multiple queries instead of JOIN FETCH
- **Detection:** Look for `@OneToMany` without `@EntityGraph` or `JOIN FETCH` in JPQL
- **Test Case:** Enable Hibernate SQL logging, count SELECT statements for a collection

**Pattern:** Missing @Transactional(readOnly = true)
- **Description:** Read-only queries acquiring unnecessary write locks
- **Detection:** Service methods with only SELECT queries lacking `readOnly = true`
- **Test Case:** Monitor database lock contention under load

### [Category 2]

[Similar structure]

## Output Format

Provide results in this format:

```markdown
### [Section 1]
[Content structure]

### [Section 2]
[Content structure]
```

## Examples

### Example Input
```
[Sample user request]
```

### Example Output
```
[What the skill would produce]
```

## Resource Files

This skill references the following resources:
- `template.md` - [Description]
- `patterns.json` - [Description]

## Relationship to Agents

This skill is designed to work with:
- **@[your-agent-name]**: [How they work together]

Typical workflow:
1. User invokes `#[skill-name]` to generate [artifact]
2. User reviews and refines the output
3. User invokes `@[agent-name]` to apply it to codebase
```

---

#### 3. Test Your Skill

1. Open Copilot Chat
2. Invoke your skill with `#[skill-name] [input]`
3. Verify:
   - Does it stay in scope?
   - Is output format consistent?
   - Is knowledge base being applied correctly?
   - Does it avoid doing agent work (tool invocations, orchestration)?

---

#### 4. Create Agent-Skill Workflow

Document how your agent and skill work together:

**Example Workflow (Security Reviewer + Security Test Generator):**

```
Step 1: Generate test cases
> #security-test-generator authentication endpoint
[Skill produces OWASP-based test scenarios]

Step 2: Review code against test scenarios
> @security-reviewer Review authentication endpoint against the test cases above
[Agent performs code review with runtime tool access]
```

**Your Workflow:**

```
Step 1: [Skill invocation]
> #[your-skill-name] [input]
[What the skill produces]

Step 2: [Agent invocation]
> @[your-agent-name] [prompt referencing skill output]
[What the agent does]
```

---

#### 5. Add Skill Documentation

Create: `.github/skills/[skill-name]/README.md`

```markdown
# [Skill Name]

## Purpose
[One-sentence description]

## When to Use
- [Use case 1]
- [Use case 2]

## Usage

```bash
#[skill-name] [input description]
```

## Example

```
#[skill-name] user authentication
```

## Works Well With

- **Agents:** `@[your-agent-name]`
- **Other Skills:** `#[related-skill]`

## Maintenance

- **Owner:** [Your name]
- **Last Updated:** [Date]
```

---

### Extension Deliverables

If you complete the optional extension, you should have:

✅ A skill definition file (`.github/skills/[name]/SKILL.md`)  
✅ Skill documentation (`.github/skills/[name]/README.md`)  
✅ Documented agent-skill workflow showing how they complement each other  
✅ Test results showing skill stays in scope and produces correct output format

---

## Deliverables

### Core Deliverables (Required)

At the end of this lab, you should have:

✅ A custom agent definition file (`.github/agents/[name].agent.md`)  
✅ Test results showing the agent meets success criteria  
✅ Usage documentation for team members  
✅ At least one iteration/refinement based on testing

### Extension Deliverables (Optional)

If you completed the optional skill extension:

✅ A skill definition file (`.github/skills/[name]/SKILL.md`)  
✅ Skill documentation (`.github/skills/[name]/README.md`)  
✅ Documented agent-skill workflow

---

## Group Share (If in Workshop Setting)

**Demonstrate your agent:**
1. Show the agent name and purpose
2. Run a live demo with a test scenario
3. Show the structured output
4. Share one key design decision you made
5. Share one challenge you encountered
6. **[If applicable]** Demo your agent-skill workflow

**Learn from others:**
- What agents did others create?
- What patterns emerged across different agents?
- What would you borrow for your next agent?
- Who created complementary skills? How do they work together?

---

## Key Takeaways

✅ **Start with success criteria** - Define "done" before building  
✅ **Test early and often** - Don't wait until it's "perfect"  
✅ **Iterate based on real usage** - Agents improve over time  
✅ **Document for others** - Agents are team assets, not personal tools  
✅ **Keep scope focused** - Better to excel at one thing than be mediocre at many

---

## Next Steps: Taking Agents to Production

### Before sharing your agent with the team:

1. **Test with real scenarios** (not just examples)
2. **Get peer review** (have a colleague test it)
3. **Document edge cases** and limitations
4. **Add to the team catalog** ([docs/guides/custom-agent-catalog.md](../guides/custom-agent-catalog.md))
5. **Set up governance** (review process, versioning)

### Continuous Improvement

- Collect feedback from team usage
- Track common issues or misunderstandings
- Update instructions based on lessons learned
- Retire or merge agents that become obsolete

---

## Bonus Challenge (Optional)

**Advanced Exercise:**

Create a **second agent** that complements your first one. For example:
- If you built a Security Reviewer, build a Security Remediation Guide
- If you built a Documentation Writer, build a Documentation Reviewer
- If you built a Performance Auditor, build a Performance Optimizer

**Workflow:** Use both agents in sequence to demonstrate a complete workflow.

---

## Workshop Conclusion (Module 6)

Congratulations! You've completed the Advanced GitHub Copilot workshop.

### What You've Learned

- ✅ Ask, Plan, and Agent interaction models
- ✅ When and how to use custom agents
- ✅ How to design reliable, role-based agents
- ✅ Iteration and governance for agent maintenance
- ✅ Hands-on experience building a production-ready agent

### Putting It Into Practice

**Week 1:** Use existing agents (Architecture Reviewer, Backlog Generator, Test Strategist) in your daily work  
**Week 2:** Identify one repetitive workflow and draft an agent for it  
**Week 3:** Test and refine your agent with real scenarios  
**Week 4:** Share with your team and gather feedback

### Final Reflection

1. **Which workflows will benefit most from agents in your work?**
2. **What agents should be standardized across your team?**
3. **How will you prevent "prompt sprawl" as agents proliferate?**
4. **Who will own agent governance on your team?**

---

## Additional Resources

- [Agent Design Guide](../guides/agent-design-guide.md)
- [Agent Governance](../guides/agent-governance.md)
- [Custom Agent Catalog](../guides/custom-agent-catalog.md)
- [GitHub Documentation: Custom Agents](https://docs.github.com/copilot)

---

**Thank you for participating in Advanced GitHub Copilot!**

Questions or feedback? [Open an issue](../../CONTRIBUTING.md) or reach out to the workshop facilitators.
