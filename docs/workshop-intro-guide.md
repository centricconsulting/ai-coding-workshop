# Workshop COI Intro Guide (<20 Minutes)

## Workshop Title
**Using AI for Application Development with GitHub Copilot (.NET Edition)**

---

## Slide Outline (For Deck Creator)

### Slide 1: Hook
**Title:** "From 2 Days to 2 Hours: Systematic AI-Assisted Development"

**Visual:** Bold stat callout or before/after comparison

**Content:**
- Opening poll question: "Who's currently using GitHub Copilot or AI coding assistants daily?"
- Follow-up: "Keep your hand up if you're using it with systematic processes vs. ad-hoc prompting"

---

### Slide 2: The Problem & Opportunity
**Title:** "Where Are You on the AI Maturity Journey?"

**Visual:** 5-Level Maturity Model diagram

**Content:**
- **Level 1**: Manual Processes Without AI
- **Level 2**: Agentic AI With Human Input for Every Prompt ← *Most teams are here*
- **Level 3**: AI-Assisted Generation With Oversight ← *Workshop target*
- **Level 4**: AI Embedded in Pipelines With Manual Oversight
- **Level 5**: Fully Autonomous Self-Healing (Aspirational)

**Key Stats:**
- iOS team: 70-80% productivity gains
- QE team: 70% test coverage @ 95%+ reliability
- Digital Wealth: 3-5x speed improvements

**Discussion Prompt:** "What's stopping your team from moving to Level 3?"

---

### Slide 3: Workshop Overview
**Title:** "A Systematic Path to Level 3 Maturity"

**Content:**

**Format:**
- 6-hour hands-on workshop (can split into 2 parts)
- Part 1 (3h): Fundamentals—TDD, requirements → code, refactoring, testing
- Part 2 (3h): Advanced—custom agents, workflow automation, agent design

**What Makes It Different:**
- Zero setup friction (Dev Containers with .NET 9)
- Real codebase with Clean Architecture patterns
- Repository-level Copilot instructions (team-wide standards)
- Complete reference implementation included
- Based on proven internal success stories

---

### Slide 4: Learning Outcomes
**Title:** "What You'll Gain"

**Visual:** 3-column table or icon-based layout

| **Level 2 → Level 3 Skills** | **Systematic Practices** | **Measurable Outcomes** |
|------------------------------|--------------------------|-------------------------|
| AI-assisted TDD | Repository-level instructions | 70-80% productivity gains |
| Requirements → working code | SDLC gates with oversight | 70% test coverage @ 95% reliability |
| Code generation & refactoring | Standardized agent frameworks | 3-5x speed improvements |
| Custom agent design | Quality guardrails | Reduced fragmentation |

---

### Slide 5: Discussion & Feedback
**Title:** "Your Input Matters"

**Content:**
Discussion questions (pick 3-4 based on time):

1. **Where would your team benefit most?**
   - Code generation & refactoring
   - Testing & quality
   - Documentation & workflow
   - Custom agents for team-specific needs

2. **What's your biggest concern about AI-assisted development?**
   - Quality, security, skill atrophy, job displacement?

3. **Preferred format:**
   - One 6-hour intensive session?
   - Two 3-hour sessions spread over time?
   - Self-paced with office hours?

4. **What use cases are you struggling with that AI could help solve?**

5. **What would make this a 'must attend' vs. 'nice to have'?**

---

### Slide 6: Call to Action & Next Steps
**Title:** "Two Ways to Get Started"

**Content:**

**Option 1: Internal Workshop**
- Delivered internally over 2-3 days (flexible scheduling)
- For your development teams
- No cost—just time investment
- **Sign up:** [Slack channel / email / link]
- **First cohort:** [proposed dates]

**Option 2: Client Delivery**
- Turn-key workshop for your clients
- Customizable to client tech stack
- Revenue-generating engagement
- **Next step:** Work with your account manager to discuss pricing and client fit

**The Ask:**
- Internal teams: Join the pilot cohort?
- Client-facing roles: See opportunities with your accounts?
- Questions?

---

## Facilitator Talking Points

### Slide 1: Hook (2 min)

**Opening:**
"Quick poll—who here is using GitHub Copilot or another AI coding assistant in their day-to-day work? Show of hands."

*[Pause for hands]*

"Great. Now keep your hand up if you're using it with systematic processes—like test-first development, repository-level standards, or integrated into your CI/CD—versus just typing prompts ad-hoc when you remember to."

*[Most hands will drop]*

"That gap—between ad-hoc usage and systematic adoption—is exactly why we built this workshop. And that gap represents a 70-80% productivity opportunity."

---

### Slide 2: The Problem & Opportunity (3 min)

**Talking Points:**
"Let me show you where most organizations sit on the AI maturity curve."

*[Point to the 5-level model]*

- **Level 1** is fully manual—no AI at all
- **Level 2** is where most of us are: we have Copilot open, we use it, but we're driving every prompt manually. There's no systematic process.
- **Level 3** is where the magic happens. Teams at Level 3 have systematic oversight—TDD workflows, repository instructions, SDLC gates. That's where our iOS team got their 70-80% gains. Two-day tasks became two-hour tasks.
- **Level 4 and 5** are about pipeline integration and autonomy—aspirational for most teams right now.

**Open the floor:**
"So here's my question: What's stopping your team from moving to Level 3? What are the barriers?"

*[Let them talk for 30-60 seconds. Common answers: lack of standards, don't know best practices, concerns about quality, leadership buy-in]*

"Great—those are exactly the things this workshop addresses."

---

### Slide 3: Workshop Overview (3 min)

**Talking Points:**
"Here's what we built. This is a **6-hour hands-on workshop**—not a demo, not a talk track. You're actually coding."

**Part 1—Fundamentals (3 hours):**
- You'll learn test-driven development with AI—write the test first, then let Copilot generate the implementation
- Turn requirements into backlog items, into acceptance criteria, into working code
- Refactor legacy code, generate documentation, automate your workflow

**Part 2—Advanced (3 hours):**
- Build custom Copilot agents that encode your team's knowledge
- Design agents for architecture review, backlog generation, test strategy
- Learn how to govern and scale agent usage across teams

**What makes this different:**
- **Zero setup hassles**: We use Dev Containers. You open VS Code, reopen in container, and you're coding in .NET 9 with everything pre-configured.
- **Real codebase**: This isn't "hello world." It's a Clean Architecture solution with domain-driven design patterns.
- **Repository-level instructions**: You'll see how to set team-wide Copilot standards that prevent fragmentation and enforce quality.
- **Reference implementation**: If you get stuck, there's a complete solution branch you can check.

"This is based on real internal wins—the iOS team, the QE team, Digital Wealth. We took what worked for them and packaged it so you can replicate it."

---

### Slide 4: Learning Outcomes (2 min)

**Talking Points:**
"By the end of this workshop, you'll have the skills to move your team from Level 2 to Level 3. Here's what that looks like:"

*[Walk through the table columns]*

**Skills:** You'll know how to do AI-assisted TDD, turn requirements into code, refactor confidently, and design custom agents.

**Practices:** You'll leave with repository-level instructions, SDLC gate patterns, and standardized frameworks you can implement Monday morning.

**Outcomes:** These aren't hypothetical. Teams using these approaches are seeing 70-80% productivity gains, 70% test coverage at 95%+ reliability, and 3-5x speed improvements.

"The key word is **systematic**. This isn't about typing better prompts. It's about building AI into your team's DNA in a governed, repeatable way."

---

### Slide 5: Discussion & Feedback (8-10 min)

**Framing:**
"We want your input to make this workshop as valuable as possible. Let's spend the next few minutes on co-creation."

**Pick 3-4 questions based on your audience:**

**Question 1: Where would your team benefit most?**
"Show of hands—who needs help with:"
- Code generation and refactoring? *[pause for hands]*
- Testing and quality? *[pause]*
- Documentation and workflow? *[pause]*
- Custom agents for team-specific needs? *[pause]*

*[Note which gets the most interest—informs emphasis]*

**Question 2: What's your biggest concern?**
"What worries you about AI-assisted development? Quality? Security? Developers losing skills? Something else?"

*[Open floor. Listen actively. Common concerns and responses:]*
- **Quality**: "That's why Level 3 is about oversight. We teach validation gates, not blind trust."
- **Security**: "Fair point. We cover prompt hygiene and avoiding sensitive data leakage."
- **Skill atrophy**: "Think of it like calculators—we still teach math, but focus on problem-solving. AI handles boilerplate."

**Question 3: Format preference?**
"Would you rather have one 6-hour intensive, or two 3-hour sessions spread over time? Be honest—what fits your schedule?"

*[Gauge interest—helps you plan delivery]*

**Question 4: Use cases you're struggling with?**
"What tasks or workflows are painful right now that AI might help solve?"

*[Capture these—you might build new labs or examples around them]*

**Question 5: Must-attend vs. nice-to-have?**
"What would make this a 'must attend' for your team versus 'nice to have if I have time'?"

*[Helps you understand priorities and selling points]*

---

### Slide 6: Call to Action (2 min)

**Opening:**
"Alright, so we see two ways this workshop creates value."

**Internal Path:**
"First, if you want to **level up your own team**—we can run this internally over a few days. Flexible format, zero cost to you beyond your time. This is about building our collective capability."

"If that's interesting, drop your name in [Slack channel / email]. We're scheduling our first internal cohort for [dates], and we're looking for 10-15 participants."

**Client Path:**
"Second, if you see opportunities with **your clients**—especially those talking about modernization, productivity, or AI transformation—this can be a standalone engagement or part of a larger program."

"We've packaged it as a turn-key offering. You bring the client, we deliver the workshop, and it's customizable to their tech stack. If you've got a client in mind, grab your account manager and let's talk pricing and positioning."

**Close:**
"So—show of hands:"
- Who's interested in the internal cohort?
- Who sees client opportunities?
- What questions do you have?

*[Open floor for Q&A]*

---

## Timing Discipline

- **Slide 1**: 2 min (tight—just the hook)
- **Slide 2**: 3 min (maturity model + 30-60 sec discussion)
- **Slide 3**: 3 min (workshop structure)
- **Slide 4**: 2 min (outcomes—walk through quickly)
- **Slide 5**: 8-10 min (the meat—this is your feedback session)
- **Slide 6**: 2 min (CTA and logistics)

**Total: 18-20 minutes**

Set a timer for each section. If discussion gets deep, say: "Great point—let's capture that and discuss offline."

---

## Handling Tough Questions

**"Won't AI replace developers?"**
→ "This workshop is about augmentation, not replacement. The iOS team didn't shrink—they took on more ambitious work. AI handles boilerplate; you focus on architecture, design, and business value."

**"How do we ensure code quality?"**
→ "That's the whole point of Level 3—systematic oversight. We teach TDD-first, code review patterns, and validation gates. You're not blindly accepting suggestions."

**"Our company doesn't allow Copilot yet."**
→ "Fair point. The principles work with any AI coding assistant—Claude, Cursor, Codeium, etc. Let's talk about your tooling access offline."

**"I'm worried about developers losing coding skills."**
→ "Valid concern. Think of it like calculators—we still teach math fundamentals, but focus on problem-solving over manual computation. AI handles repetitive patterns; you still need to understand architecture, design, and debugging."

**"How much does client delivery cost?"**
→ "Great question—it depends on customization, duration, and travel. Your account manager has the pricing model. This is a premium workshop offering, not a loss leader."

**"Can we white-label this for clients?"**
→ "Absolutely. We can co-brand or fully white-label depending on the engagement."

**"What if my client doesn't use .NET?"**
→ "We built this with .NET because it's common in our practice, but the patterns work across any language. We can adapt to Python, Java, TypeScript, etc. with some lead time."

**"What's the ideal client for this?"**
→ "Mid-to-large development teams (10+ devs), already have or are considering Copilot/AI tools, interested in systematic adoption vs. ad-hoc usage. Maturity Level 2 trying to reach Level 3."

---

## Key Messages to Reinforce

1. **Most teams are at Level 2**—you're not behind, you're where the industry is
2. **Level 3 is the unlock**—systematic processes, not better prompts
3. **70-80% productivity gains are real**—based on internal success stories
4. **This workshop is hands-on**—not demos, not slides, actual coding
5. **Two delivery options**—internal upskilling and client revenue stream

---

## Questions You Want Answers To

Use the discussion time to gather:

1. What's the right format? (One long session vs split)
2. What's the right audience level? (Beginners vs experienced devs)
3. What concerns need addressing? (Quality, security, governance)
4. Who are the early adopters? (Names for pilot cohort)
5. What client opportunities exist? (Leads for account managers)

---

## One-Pager for Account Managers

*(Share this separately with account managers for client discussions)*

### Workshop: AI-Assisted Development with GitHub Copilot

**Client Value Proposition:**
- Accelerate development productivity by 70-80% through systematic AI adoption
- Move from ad-hoc AI usage (Level 2) to systematic, governed workflows (Level 3)
- Hands-on, outcome-driven workshop with measurable results

**Ideal Client Profile:**
- Development teams of 10-50+ developers
- Using or evaluating GitHub Copilot, Cursor, or similar AI tools
- Interested in modernization, code quality improvement, or velocity gains
- Open to new practices (TDD, Clean Architecture, AI-assisted workflows)

**Delivery Format:**
- **Duration**: 6 hours (can split into 2x 3-hour sessions)
- **Participants**: Up to 20 developers per cohort
- **Prerequisites**: GitHub Copilot access, .NET experience (adaptable to other languages)
- **Outcomes**: Developers leave with practical skills, reference implementation, and team-wide standards

**Differentiation:**
- Not just demos—hands-on labs with real codebases
- Addresses quality concerns through systematic oversight
- Based on proven internal success (70-80% productivity gains)
- Enterprise patterns: Clean Architecture, DDD, TDD
- Can be standalone or bundled with modernization programs

**Pricing & Engagement:**
- Contact: [Your Name / Workshop Lead]
- Pricing available through account manager
- Customizable to client tech stack and industry vertical

---

## Post-Session Follow-Up

**For Internal Participants:**
- Send calendar invite for pilot cohort
- Share GitHub repo link
- Create Slack channel for ongoing discussion

**For Client-Facing Roles:**
- Schedule follow-up with interested account managers
- Provide client pitch deck/one-pager
- Discuss pilot client opportunities
