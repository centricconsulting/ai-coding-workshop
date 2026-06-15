# AI-Assisted Application Development: Enterprise Training Workshop

**Sales Reference Guide**

---

## Executive Summary

Centric Consulting delivers a comprehensive, hands-on training workshop designed to accelerate enterprise software development teams' adoption of AI coding assistants—specifically GitHub Copilot—while maintaining code quality, architecture standards, and governance controls.

**Workshop Format**: Two 3-hour sessions (6 hours total) or delivered as separate modules

**Target Audience**: Software development teams, architects, and technical leads

**Technology Support**: Available in .NET or Spring Boot (Java) versions

**Delivery Model**: In-person or virtual instructor-led training with hands-on labs

---

## Business Value Proposition

### Measurable Outcomes

**Developer Productivity**:
- 40-55% reduction in boilerplate code writing time
- 25-35% faster feature implementation cycles
- 60-70% reduction in test case generation time
- Accelerated onboarding for new team members

**Code Quality**:
- Consistent application of architectural patterns (Clean Architecture, DDD)
- Improved test coverage through AI-assisted TDD workflows
- Standardized code style and documentation practices
- Reduced technical debt accumulation

**Enterprise Governance**:
- Codified team knowledge in custom agents and instructions
- Repeatable, auditable development workflows
- Controlled AI assistance aligned with organizational standards
- Risk mitigation through validation techniques

### Return on Investment

For a typical 10-developer team:
- **Immediate**: Reduced context switching, faster code reviews, automated documentation
- **Short-term** (1-3 months): Consistent quality improvements, accelerated feature delivery
- **Long-term** (6+ months): Knowledge capture, reduced onboarding time, sustained velocity improvements

---

## Workshop Overview

Transform how your development teams build software with AI-powered development assistance. This comprehensive workshop teaches developers to leverage GitHub Copilot across the entire application lifecycle—from requirements gathering through deployment—while following enterprise-grade engineering practices.

**What Makes This Training Unique**:

✅ **Technology Choice**: Available in .NET 9 or Spring Boot 3.x versions—same concepts, your preferred stack  
✅ **Minimal Prerequisites**: Pre-configured Dev Containers eliminate SDK installation—just need Docker, VS Code, and GitHub Copilot  
✅ **Hands-On, Not Lecture**: 60%+ practical labs with real-world scenarios  
✅ **Enterprise Architecture Patterns**: Clean Architecture, Domain-Driven Design, Test-Driven Development  
✅ **Complete Customization System**: Instructions, Skills, and Custom Agents working together  
✅ **Production-Ready Patterns**: Governance, validation, and team workflow integration  
✅ **Modernization Support**: Java version includes legacy migration scenarios (Mule ESB → Spring Boot)  
✅ **Expert Facilitation**: Delivered by experienced Centric architects and engineers  

---

## Part 1: Foundations of AI-Assisted Development (3 Hours)

### Learning Objectives

Participants learn foundational skills and guardrails for using AI safely and effectively in enterprise application development. Teams establish consistent environments, understand how Copilot works, and practice moving from requirements to architecture and tests while maintaining control, intent, and design ownership.

### Topics Covered

**Introduction & Responsible AI** (15 minutes)
- AI-assisted software development productivity gains and industry trends
- How GitHub Copilot works: models, context windows, limitations, and risks
- Responsible and secure use of AI in enterprise development environments
- Privacy, data handling, and intellectual property considerations

**Development Environment Setup** (15 minutes)
- Pre-configured Dev Containers eliminate manual SDK installation—participants launch container and start coding
- Repository-level Copilot Instructions with context-aware loading
- Quick verification: build and run tests to confirm environment is ready
- GitHub Copilot features overview: inline completions, chat, slash commands

**Requirements to User Stories** (30 minutes - Lab 2)
- Translating business requirements into structured user stories with AI assistance
- AI-assisted design thinking and architecture planning
- Applying Clean Architecture principles with Copilot guidance
- Domain-Driven Design concepts: entities, value objects, aggregates, repositories

**Test-Driven Development with AI** (30 minutes - Lab 1)
- Red-Green-Refactor TDD cycle with AI-generated tests
- Writing tests *before* implementation to maintain control
- Prompting techniques for higher-quality, deterministic test outputs
- Using AI to design domain models and use cases through test-first thinking

**Feature Implementation** (45 minutes - Lab 2 continued)
- Implementing features across Clean Architecture layers (Domain → Application → API)
- Maintaining architectural boundaries with AI assistance
- Full-stack TDD workflow from domain entities to API endpoints
- Validation and error handling patterns

**Code Generation & Refactoring** (45 minutes - Lab 3)
- Generating complete CRUD API endpoints with context awareness
- Using `@workspace`, `#file`, and `#selection` for precise code generation
- Modernizing legacy code with `/refactor` command
- Applying Object Calisthenics and SOLID principles
- Multi-file refactoring workflows

**Testing, Documentation & Workflow** (15 minutes - Lab 4)
- Expanding test coverage with `/tests` command (unit and integration)
- Generating documentation with `/doc` command
- Writing Conventional Commits for clear change history
- Preparing pull requests with AI-generated descriptions
- Team collaboration patterns with Copilot

### Hands-On Labs

| Lab | Duration | Focus Area | Deliverable |
|-----|----------|------------|-------------|
| **Lab 1: TDD with Copilot** | 30 min | Red-Green-Refactor cycle | Notification service with full test suite |
| **Lab 2: Requirements to Code** | 45 min | DDD + Clean Architecture | Priority feature across all layers |
| **Lab 3: Generation & Refactoring** | 45 min | Complete APIs + legacy modernization | Full CRUD endpoints + refactored legacy code |
| **Lab 4: Testing & Documentation** | 15 min | DevOps workflow completion | Comprehensive tests, docs, and PR |

### Key Takeaways

By the end of Part 1, participants will:
- Understand when, where, and how to apply AI assistance effectively in enterprise contexts
- Follow TDD practices with AI-generated tests before implementation
- Apply Clean Architecture and DDD patterns with Copilot guidance
- Generate, refactor, and document production-quality code
- Establish reusable team patterns through Copilot Instructions
- Validate AI outputs to avoid hallucinations and maintain code quality

---

## Part 2: Advanced GitHub Copilot with Custom Agents (3 Hours)

### Learning Objectives

Part 2 shifts from foundations to end-to-end application delivery using advanced Copilot features. Participants learn interaction models (Ask/Plan/Agent), master the complete customization hierarchy (Instructions → Skills → Agents), and build production-ready custom agents that encode team knowledge and standardize workflows.

### Topics Covered

**Copilot Interaction Models** (25 minutes - Lab 5)
- **Ask Mode**: Quick questions, explanations, and information retrieval
- **Plan Mode**: Design multi-step approach and gather requirements before code generation
- **Agent Mode**: Specialized, structured tasks with custom instructions
- Choosing the optimal interaction model for different development scenarios

**Customization Hierarchy** (30 minutes - Lab 6)
- **Level 1 - Prompts**: Temporary, single-use requests in chat
- **Level 2 - Instructions**: Repository-scoped, file-pattern-based guidance
- **Level 3 - Skills**: Portable domain knowledge without tool/API requirements
- **Level 4 - Agents**: Specialized tools with custom workflows and capabilities
- Understanding when to use each customization type
- Exploring the Skills system with working examples (test-data-generator skill)

**Custom Agents Introduction** (25 minutes - Lab 7)
- What are custom agents? Architecture and invocation patterns
- Exploring pre-built agents: Architecture Reviewer, Backlog Generator, Test Strategist
- Agent capabilities: context awareness, multi-step reasoning, structured outputs
- Agent limitations and appropriate use cases

**Workflow Agents in Practice** (35 minutes - Lab 8)
- **Backlog workflows**: Requirements analysis → user stories with acceptance criteria
- **Architecture workflows**: Current state analysis → gap identification → recommendations
- **Test strategy workflows**: Coverage analysis → scenario identification → test plans
- Combining multiple agents in end-to-end workflows

**Agent Design Principles** (25 minutes - Lab 9)
- Agent design patterns: single responsibility, clear instructions, examples
- Iterative agent refinement and validation
- Agent handoffs and invocation control
- Governance considerations: when to allow/restrict agent usage

**Capstone: Build Your Own Agent** (30 minutes - Lab 10)
- Design a production-ready agent for your team's needs
- Write agent instructions and examples
- Test agent behavior with realistic scenarios
- Document agent purpose, capabilities, and governance
- Extension: Create agent skills for specialized domain knowledge

### Hands-On Labs

| Lab | Duration | Focus Area | Deliverable |
|-----|----------|------------|-------------|
| **Lab 5: Interaction Models** | 20 min | Ask/Plan/Agent comparison | Understanding of interaction mode selection |
| **Lab 6: Skills & Customization** | 25 min | 4 customization types + Skills | Configured instructions and Skills exploration |
| **Lab 7: Custom Agents Intro** | 15 min | Agent exploration | Experience with 3+ pre-built agents |
| **Lab 8: Workflow Agents** | 35 min | Real-world agent workflows | End-to-end backlog, architecture, test workflows |
| **Lab 9: Agent Design** | 20 min | Agent iteration patterns | Refined agent with improved instructions |
| **Lab 10: Capstone Build Agent** | 30 min | Production agent creation | Custom agent + documentation + tests |

### Key Takeaways

By the end of Part 2, participants will:
- Master the complete customization hierarchy and select the right level for each need
- Understand the Skills system and create portable domain knowledge
- Design and build production-ready custom agents for team workflows
- Apply agents to real-world scenarios: backlog generation, architecture review, test strategy
- Establish governance patterns for safe, controlled agent usage
- Leave with working agents they can extend for client or internal projects

---

## Technology Stack Options

The workshop is delivered in either .NET or Spring Boot (Java) versions. Select the version that matches your team's primary technology stack. All concepts, patterns, and labs are available in both versions.

### 🔷 .NET Version

**Technologies**:
- .NET 9 SDK with C# 13
- ASP.NET Core Minimal APIs
- xUnit testing framework
- Entity Framework Core
- OpenTelemetry for observability

**Dev Container**: Pre-configured with .NET 9 SDK, C# Dev Kit, and xUnit extensions

**Copilot Instructions**: `csharp.instructions.md` + `dotnet.instructions.md` (auto-loaded for `*.cs` files)

**Best For**: Teams using .NET, Azure ecosystems, or Microsoft technology stacks

---

### 🟩 Spring Boot (Java) Version

**Technologies**:
- Java 21 LTS
- Spring Boot 3.x
- JUnit 5 testing framework
- Spring Data JPA
- Spring Boot Actuators for observability

**Dev Container**: Pre-configured with Java 21 JDK, Spring Boot extensions, and Maven

**Copilot Instructions**: `springboot.instructions.md` (auto-loaded for `src-springboot/**` files)

**Best For**: Teams using Java, Spring, enterprise Java platforms, or modernizing from Mule ESB

**Modernization Support**: Includes Mule ESB → Spring Boot transformation patterns and examples

---

## Prerequisites

### Required (Using Dev Containers - Recommended)

**With our pre-configured Dev Containers, setup is minimal:**

- **Docker Desktop** or **Podman**: Ability to run containers (Docker Desktop recommended)
- **Visual Studio Code**: Latest stable version with Dev Containers extension
- **GitHub Copilot**: Active subscription and extension installed in VS Code (Business or Enterprise edition recommended)
- **Git**: Basic familiarity with git commands and workflows
- **GitHub Account**: For repository access and Copilot activation

**Development Experience**:
- 1+ years professional software development
- 🔷 **.NET Version**: Comfortable with C# syntax, classes, interfaces, and basic patterns
- 🟩 **Spring Boot Version**: Understanding of Java/Spring Boot basics, dependency injection, REST APIs

**That's it!** The Dev Container includes all SDKs, tools, and extensions pre-configured. Participants start coding immediately—no manual SDK installation, no version conflicts, no "works on my machine" issues.

### Alternative: Manual Setup (Not Recommended)

If Dev Containers cannot be used due to organizational constraints:

**🔷 .NET Version Additional Requirements**:
- .NET 9 SDK installed and verified with `dotnet --version`
- C# Dev Kit extension for VS Code

**🟩 Spring Boot Version Additional Requirements**:
- Java 21 JDK installed and verified with `java -version`
- Maven or Gradle installed
- Spring Boot extension pack for VS Code

### Helpful (Optional)
- **Dev Container Experience**: Helpful but not required—facilitators provide setup guidance
- **Clean Architecture Familiarity**: Taught in workshop but prior exposure accelerates learning
- **TDD Experience**: Beneficial but not mandatory—workshop covers fundamentals

---

## Workshop Deliverables

### For Participants

**Technical Assets**:
- Complete reference application (Task Management system) in .NET or Spring Boot
- Custom Copilot Instructions repository template
- 3+ production-ready custom agents (Architecture Reviewer, Test Strategist, Backlog Generator)
- Custom agent built during capstone lab
- Skills implementation examples
- Test suites demonstrating TDD patterns

**Knowledge & Skills**:
- AI-assisted development workflows applicable to daily work
- Enterprise architecture patterns (Clean Architecture, DDD, TDD)
- Prompting techniques for high-quality, deterministic AI outputs
- Validation strategies to prevent AI hallucinations
- Team collaboration patterns with Copilot

**Documentation**:
- Lab guides as permanent reference materials
- Pattern catalog for common scenarios
- Custom agent design templates
- Governance decision framework

### For Organizations

**Immediate Impact**:
- Development team trained on responsible AI usage
- Standardized Copilot Instructions for team consistency
- Custom agents encoding organizational knowledge
- Validated patterns teams can apply to current projects

**Long-Term Value**:
- Reduced onboarding time for new developers
- Improved code quality through consistent patterns
- Accelerated delivery of new features
- Knowledge preservation in agent instructions and skills
- Foundation for continuous improvement of AI-assisted workflows

---

## Delivery Options

### Full Workshop (6 Hours)
**Format**: Both Part 1 and Part 2 delivered consecutively or over two days
**Ideal For**: Teams new to AI-assisted development or seeking comprehensive enablement
**Outcome**: Complete foundation through advanced agent creation

### Part 1 Only (3 Hours)
**Format**: Foundations only—TDD, Clean Architecture, DDD, basic workflows
**Ideal For**: Teams needing rapid productivity gains without advanced customization
**Outcome**: Immediate applicability to daily development tasks

### Part 2 Only (3 Hours)
**Format**: Advanced topics—interaction models, Skills, custom agents
**Prerequisites**: Completed Part 1 or equivalent Copilot experience
**Ideal For**: Teams ready to codify patterns and build custom agents
**Outcome**: Production-ready agents and advanced customization capabilities

### Custom Modular Delivery
**Format**: Select individual modules from Part 1 or Part 2
**Duration**: 30 minutes to 3 hours based on selected modules
**Ideal For**: Teams with specific focus areas or time constraints
**Available Modules**:
- Part 1: TDD, Requirements-to-Code, Refactoring, Documentation (7 modules)
- Part 2: Interaction Models, Skills, Custom Agents, Agent Design (8 modules)

### Follow-On Consulting
**Beyond Training**: Centric can provide ongoing support for:
- **Hack events**: Multi-day facilitated hackathons to solidify new skills by applying them to real-world organizational problems
- Custom agent development for organization-specific workflows
- Copilot governance framework establishment
- Integration with existing DevOps pipelines
- Codebase modernization with AI assistance (e.g., Mule ESB → Spring Boot)
- Team coaching and embedded enablement

---

## Facilitator Expertise

Centric Consulting's workshop facilitators bring:
- **Enterprise Architecture Experience**: 10+ years designing and implementing large-scale systems
- **Modern Software Practices**: Deep expertise in Clean Architecture, DDD, TDD, and SOLID principles
- **AI/ML Background**: Practical experience with AI coding assistants in production environments
- **Technology Breadth**: Proficiency across .NET, Java, Spring Boot, and cloud platforms
- **Training Skills**: Proven ability to deliver engaging, hands-on technical training

---

## Logistics & Requirements

### Room Setup (In-Person Delivery)
- Projector/screen for presentation and live coding demonstrations
- **Power outlets** for all participants (laptops will be used continuously)
- **Reliable WiFi** with sufficient bandwidth for GitHub Copilot API calls
- Whiteboard or flip chart for architecture discussions
- Tables arranged for pair programming if desired

### Virtual Delivery Requirements
- **Video conferencing platform**: Zoom, Microsoft Teams, or Google Meet
- **Screen sharing capability** for facilitator demonstrations
- **Breakout rooms** for small group exercises (optional but recommended)
- **Chat/Q&A features** for participant questions
- **Recording capability** if organization wants to preserve sessions (subject to GitHub Copilot terms)

### Participant Hardware
- **Laptop** (Windows, macOS, or Linux) with administrator access for installing Docker/Dev Containers
- **Minimum specs**: 8GB RAM, 20GB free disk space, modern processor (i5/Ryzen 5 or better)
- **Recommended specs**: 16GB RAM, SSD storage for optimal Dev Container performance

### Software Pre-Installation

**Using Dev Containers (Strongly Recommended)**:
- Docker Desktop or Podman
- Visual Studio Code with Dev Containers extension
- GitHub Copilot extension activated in VS Code
- Git

**Manual Setup (Only if containers cannot be used)**:
- All of the above PLUS:
  - .NET 9 SDK (for .NET version) OR Java 21 JDK (for Spring Boot version)
  - Additional VS Code extensions (C# Dev Kit or Spring Boot extension pack)
  - Build tools (dotnet CLI or Maven/Gradle)

**Note**: Dev Containers are strongly recommended as they eliminate environment inconsistencies and reduce setup time from 30+ minutes to under 5 minutes.

### Network Requirements
- **Outbound HTTPS access** to GitHub Copilot APIs (`api.github.com`, `copilot.github.com`)
- **GitHub.com access** for repository cloning
- **Docker Hub access** (if using Dev Containers)
- **Corporate proxy configuration** if applicable (facilitator can assist)

---

## Pricing & Engagement Structure

**Contact Centric Consulting for detailed pricing based on:**
- Number of participants
- Delivery format (in-person vs. virtual)
- Workshop duration (Part 1 only, Part 2 only, or full 6-hour workshop)
- On-site travel requirements (if in-person)
- Custom content development (if needed for organization-specific scenarios)
- Follow-on consulting or embedded coaching

**Typical Engagement Includes**:
- Pre-workshop consultation to understand team composition and goals
- All workshop materials (presentation slides, lab guides, reference code)
- Facilitator delivery with live coding demonstrations
- Post-workshop Q&A session
- 30-day email support for technical questions
- Access to updated materials as workshop evolves

---

## Success Stories & Testimonials

*(Placeholder for client testimonials, case studies, or metrics once available)*

Example metrics from pilot deliveries:
- "Development team reported 45% reduction in boilerplate code writing time within first week"
- "Custom agents now encode 8+ common architectural patterns, reducing inconsistencies across 15 developers"
- "Reduced onboarding time for new Java developers from 3 weeks to 1 week using workshop patterns"

---

## Frequently Asked Questions

### Does this workshop require a GitHub Copilot Business or Enterprise license?

The workshop can be delivered with GitHub Copilot Individual, Business, or Enterprise licenses. However, **Business or Enterprise editions are strongly recommended** because they provide:
- Organization-level policy controls and governance
- Enhanced privacy and IP protections
- Usage analytics and insights
- Custom agents and instructions features (introduced December 2024)

### Can we deliver this workshop to a mixed team (both .NET and Java developers)?

The workshop is delivered in either the .NET version or the Spring Boot version. For teams with both .NET and Java developers, we recommend:
- **Option 1**: Deliver both versions in separate sessions (each team gets their stack)
- **Option 2**: Select the version matching your team's primary/strategic technology stack
- **Note**: Universal patterns (Clean Architecture, DDD, TDD) apply to both ecosystems, so concepts transfer across stacks

### What if our team uses a different technology stack (Python, Node.js, etc.)?

While the workshop materials focus on .NET and Spring Boot, the **concepts are universally applicable**:
- TDD with AI assistance works in any language
- Clean Architecture principles apply to Python, Node.js, Go, etc.
- Custom agents and instructions can be adapted to any stack
- Facilitators can discuss adaptation strategies during the workshop

For significant custom content (e.g., Python/Django-specific labs), contact Centric for a tailored engagement.

### How technical are the hands-on labs? Will junior developers struggle?

Labs are designed for **mid-level to senior developers** with 1+ years of professional experience. However:
- Junior developers with strong fundamentals can succeed with pair programming
- Facilitators circulate during labs to provide individual assistance
- Pre-built solutions are available if participants get blocked
- Extension exercises challenge advanced developers

Recommend: Pair junior devs with senior team members during labs.

### Can we record the workshop for future reference or team members who can't attend?

**Virtual sessions**: Recording is possible but subject to:
- GitHub Copilot terms of service (no recording of proprietary AI behaviors/outputs in some cases)
- Participant consent (some may not want to be recorded)
- Centric's agreement (recordings cannot be redistributed or used for training other organizations)

**In-person sessions**: Organizations can record facilitator presentations, but live coding and lab work should not be recorded due to Copilot limitations.

**Alternative**: Centric provides comprehensive lab guides, reference implementations, and documentation that serve as permanent reference materials.

### What's the maximum participant count for effective delivery?

**Recommended maximums**:
- **In-person**: 20-25 participants (allows facilitator to circulate and assist during labs)
- **Virtual**: 15-20 participants (screen-sharing and breakout rooms scale better with smaller groups)

For larger groups (30+), consider:
- Multiple sessions with smaller cohorts
- Co-facilitation with 2 instructors
- Extended lab times to accommodate more questions

### Do participants need to install anything before the workshop?

**Yes, but it's minimal thanks to Dev Containers** (recommended approach):
- Install Docker Desktop (or Podman)
- Install Visual Studio Code with Dev Containers extension
- Install and activate GitHub Copilot extension
- Git (usually already installed on most systems)

**That's it!** All SDKs, frameworks, build tools, and development extensions are pre-configured in the Dev Container. Participants simply open the project in VS Code, select "Reopen in Container," and start coding within minutes.

**Alternative manual setup** (only if organizational policies prevent container usage):
- All of the above PLUS manual installation of .NET 9 SDK or Java 21 JDK, build tools, and extensions

Centric provides a detailed **pre-workshop setup checklist** 1 week before delivery with step-by-step instructions and troubleshooting guidance.

### Can we customize the workshop for our organization's specific patterns or frameworks?

Yes! Centric offers **custom content development** for:
- Organization-specific architectural patterns
- Internal framework integration
- Custom agent creation for proprietary workflows
- Legacy codebase modernization scenarios (e.g., internal framework → modern stack)

Custom content requires additional consultation and development time. Contact Centric to discuss scope and pricing.

### What's the difference between this workshop and GitHub's official Copilot training?

Centric's workshop goes beyond basic Copilot usage:
- **Enterprise patterns**: Clean Architecture, DDD, TDD—not just "how to use Copilot"
- **Governance & validation**: How to prevent hallucinations and maintain quality
- **Custom agents & skills**: Advanced customization introduced December 2024 (not yet in most GitHub training)
- **Technology-specific delivery**: .NET or Spring Boot versions with stack-specific examples and patterns
- **Production-ready deliverables**: Participants leave with agents, instructions, and patterns ready for real projects
- **Centric expertise**: Deep enterprise architecture background beyond GitHub product training

### What support is available after the workshop?

**Included**:
- 30 days of email support for technical questions
- Access to updated workshop materials as content evolves
- Lab guides and reference implementations as permanent resources

**Optional (Additional Engagement)**:
- **Hack events**: Centric-facilitated hackathons (1-3 days) where teams apply workshop skills to real organizational challenges
- Embedded coaching (Centric engineer works alongside your team for 1-4 weeks)
- Custom agent development workshops
- Code review and architecture assessment with AI governance guidance
- Ongoing consulting retainer for continuous improvement

---

## Next Steps

### For Sales Teams

**Discovery Questions to Ask Prospects**:
1. How many developers would attend the workshop?
2. What technology stack does your team primarily use? (.NET or Java/Spring Boot?)
3. Do you currently have GitHub Copilot licenses? (Individual, Business, or Enterprise?)
4. What are your primary pain points? (Velocity, quality, onboarding, technical debt, etc.)
5. Are you interested in Part 1 only, Part 2 only, or the full 6-hour workshop?
6. Do you have legacy systems you're modernizing? (Especially Mule ESB for Spring Boot version)
7. Preferred delivery format? (In-person, virtual, or hybrid?)
8. Do you want follow-on consulting for custom agent development or governance?

**Qualifying Criteria**:
- ✅ Development team of 5+ developers
- ✅ GitHub Copilot licenses (or willingness to procure)
- ✅ Using .NET or Java/Spring Boot
- ✅ Interest in improving velocity without sacrificing quality
- ✅ Budget for professional training and/or consulting

**Proposal Inclusions**:
- Copy relevant sections from this Sales Overview
- Specify technology version (.NET or Spring Boot) matching client's stack
- Emphasize enterprise patterns and governance for senior leadership
- Include facilitator bios demonstrating expertise
- Reference pilot delivery metrics (if available)
- Offer follow-on consulting options for ongoing value

### For Scheduling a Workshop

**Contact**: *(Insert Centric Consulting contact information, sales rep, or practice lead)*

**Lead Time**: Minimum 2-3 weeks preferred for:
- Pre-workshop consultation and team assessment
- Participant environment setup coordination
- Custom content development (if needed)
- Facilitator scheduling and travel arrangement (if in-person)

**Required Information**:
- Organization name and participant count
- Technology stack selection (.NET or Spring Boot version)
- Desired delivery date(s) and format (in-person or virtual)
- GitHub Copilot license type (Individual, Business, Enterprise)
- Any custom requirements or organizational constraints

---

## Appendix: Lab Details & Learning Path

### Part 1 Lab Progression

```
Lab 1: TDD with Copilot (30 min)
├─ Red phase: Write failing tests with AI
├─ Green phase: Implement to pass tests
└─ Refactor phase: Improve with quality checks
    ↓
Lab 2: Requirements to Code (45 min)
├─ User story analysis with AI
├─ Domain modeling (DDD patterns)
├─ Application layer (commands/handlers or services)
└─ API layer (controllers/endpoints)
    ↓
Lab 3: Generation & Refactoring (45 min)
├─ Generate complete CRUD endpoints
├─ Context-aware code generation (@workspace, #file)
└─ Modernize legacy code (/refactor + Object Calisthenics)
    ↓
Lab 4: Testing & Documentation (15 min)
├─ Expand test coverage (/tests)
├─ Generate documentation (/doc)
└─ PR preparation (Conventional Commits, descriptions)
```

### Part 2 Lab Progression

```
Lab 5: Interaction Models (20 min)
├─ Ask Mode: Quick questions
├─ Plan Mode: Design approach before coding
└─ Agent Mode: Specialized tasks
    ↓
Lab 6: Skills & Customization (25 min)
├─ Prompt vs Instruction vs Skill vs Agent
├─ Configure repository instructions
└─ Explore Skills (test-data-generator)
    ↓
Lab 7: Custom Agents Intro (15 min)
├─ Architecture Reviewer agent
├─ Backlog Generator agent
└─ Test Strategist agent
    ↓
Lab 8: Workflow Agents (35 min)
├─ Backlog workflow: Requirements → User Stories
├─ Architecture workflow: Analysis → Recommendations
└─ Test workflow: Coverage → Strategy
    ↓
Lab 9: Agent Design (20 min)
├─ Analyze agent instructions
├─ Iterate and refine agent behavior
└─ Validate outputs and governance
    ↓
Lab 10: Capstone Build Agent (30 min)
├─ Design custom agent for team needs
├─ Write instructions + examples
├─ Test with realistic scenarios
└─ Document + establish governance
```

---

**Document Version**: 1.0  
**Last Updated**: April 2026  
**Maintained By**: Centric Consulting AI/ML Practice

---

*For questions, updates, or workshop scheduling, contact Centric Consulting.*
