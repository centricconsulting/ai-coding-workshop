# Workshop Presentation Modules

## 🎯 Purpose

This directory contains modular presentation files for the GitHub Copilot workshop. Each module is a standalone Marp presentation that can be:
- Delivered independently
- Combined with other modules for custom workshops
- Used for self-paced learning

---

## 📚 Module Catalog

### Pre-Workshop: Non-Technical Crash Course (1 hour)

| Module | Topic | Duration | File |
|--------|-------|----------|------|
| 00 | Terminal, VS Code, Git & GitHub Basics | 60 min | [00-non-technical-crash-course.md](modules/pre-workshop/00-non-technical-crash-course.md) |

**For:** BAs, PMs, and other non-engineering participants with no prior tooling experience. Pairs with [Lab 0](../labs/lab-00-non-technical-crash-course.md). Run immediately before Part 1.

---

### Part 1: Fundamentals (3 hours)

| Module | Topic | Duration | File |
|--------|-------|----------|------|
| 00 | Kickoff & Setup | 15 min | [00-kickoff-and-setup.md](modules/part1/00-kickoff-and-setup.md) |
| 01 | Copilot Features Tour | 15 min | [01-copilot-features-tour.md](modules/part1/01-copilot-features-tour.md) |
| 02 | Copilot Instructions & TDD | 30 min | [02-copilot-instructions-tdd.md](modules/part1/02-copilot-instructions-tdd.md) |
| 03 | Requirements to Code | 45 min | [03-requirements-to-code.md](modules/part1/03-requirements-to-code.md) |
| 04 | Generation & Refactoring | 45 min | [04-generation-refactoring.md](modules/part1/04-generation-refactoring.md) |
| 05 | Testing & Documentation | 15 min | [05-testing-documentation.md](modules/part1/05-testing-documentation.md) |
| 06 | Wrap-Up & Discussion | 15 min | [06-wrapup-discussion.md](modules/part1/06-wrapup-discussion.md) |

**Total:** ~3 hours

---

### Part 2: Advanced Copilot (3 hours)

| Module | Topic | Duration | File |
|--------|-------|----------|------|
| 00 | Welcome & Recap | 10 min | [00-welcome-recap.md](modules/part2/00-welcome-recap.md) |
| 01 | Interaction Models | 25 min | [01-interaction-models.md](modules/part2/01-interaction-models.md) |
| 02 | Skills & Customization | 30 min | [02-skills-customization.md](modules/part2/02-skills-customization.md) ⭐ NEW |
| 03 | Custom Agents Intro | 25 min | [03-custom-agents-intro.md](modules/part2/03-custom-agents-intro.md) |
| 04 | Workflow Agents | 30 min | [04-workflow-agents.md](modules/part2/04-workflow-agents.md) |
| 05 | Agent Design | 30 min | [05-agent-design.md](modules/part2/05-agent-design.md) |
| 06 | Capstone Lab | 35 min | [06-capstone-lab.md](modules/part2/06-capstone-lab.md) |
| 07 | Wrap-Up & Next Steps | 10 min | [07-wrapup-next-steps.md](modules/part2/07-wrapup-next-steps.md) |

**Total:** ~3 hours

---

### Role-Specific Modules (Coming Soon)

| Module | Target Role | Duration | Status |
|--------|-------------|----------|--------|
| BA: Requirements Modeling | Business Analysts | 45 min | 🚧 Planned |
| QA: Test Strategy Agents | QA Engineers | 45 min | 🚧 Planned |
| Infra: DevOps Automation | Infrastructure/DevOps | 45 min | 🚧 Planned |

---

## 🎨 Workshop Delivery Patterns

### Full 6-Hour Workshop (Parts 1 + 2)
**Day 1 (AM):** Part 1 modules 00-06  
**Day 1 (PM):** Part 2 modules 00-07

### Developer Fast Track (4 hours)
- Part 1: Modules 02, 03, 04 (skip setup if experienced)
- Part 2: Modules 01, 02, 03, 05

### Lunch & Learn: Custom Agents (1 hour)
- Part 2: Modules 02, 03, 04 (condensed)

### Executive Overview (30 min)
- Part 1: Module 00 (10 min)
- Part 2: Modules 02, 03 (20 min, overview slides only)

### Self-Paced Learning
- Follow modules in order, complete associated labs
- Each module links to next/previous for easy navigation

---

## 🚀 Quick Start

### For Facilitators

**Full Workshop (Sequential):**
1. Start with Part 1, Module 00
2. Follow navigation links at bottom of each module
3. Each module references the corresponding lab guide

**Custom Workshop:**
1. Review catalog above
2. Select modules based on audience needs
3. Create custom index slide linking your selected modules
4. Test flow before delivery

### For Self-Paced Learners

1. Install [Marp extension for VS Code](https://marketplace.visualstudio.com/items?itemName=marp-team.marp-vscode)
2. Open any module file
3. Click "Preview Marp Slide Deck" in VS Code
4. Use navigation links to move between modules
5. Complete lab exercises referenced in each module

---

##Export Options

### Export Individual Modules

```bash
# PDF
npx @marp-team/marp-cli modules/part1/00-kickoff-and-setup.md --pdf

# HTML
npx @marp-team/marp-cli modules/part1/00-kickoff-and-setup.md --html
```

### Export Full Part (All Modules)

```bash
# Part 1 as single PDF
npx @marp-team/marp-cli modules/part1/*.md --pdf --pdf-notes --output part1-fundamentals.pdf

# Part 2 as single PDF
npx @marp-team/marp-cli modules/part2/*.md --pdf --pdf-notes --output part2-advanced.pdf
```

### Export Custom Selection

```bash
# Create custom presentation from selected modules
npx @marp-team/marp-cli \
  modules/part1/02-copilot-instructions-tdd.md \
  modules/part2/02-skills-customization.md \
  modules/part2/03-custom-agents-intro.md \
  --pdf --output custom-workshop.pdf
```

---

## 📁 File Organization

```
presentations/
  modules/
    part1/              # Fundamentals (7 modules)
    part2/              # Advanced (8 modules)
    roles/              # Role-specific (future)
  index.md              # This file
  README.md             # Usage guide
```

**Note:** Legacy monolithic files moved to [archive/presentations/](../../archive/presentations/) for reference.

---

## 🔧 Customization

### Creating Your Own Module

1. Copy an existing module as template
2. Update Marp frontmatter
3. Update navigation links at bottom
4. Add to this index
5. Test with Marp preview

### Module Template Structure

```markdown
---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

# Module Title

[Content slides...]

---

<!-- Navigation -->
**Next Module:** [Next Topic](next-module.md)
**Previous Module:** [Previous Topic](previous-module.md)
```

---

## 📖 Related Documentation

- **Lab Guides:** [docs/labs/](../labs/)
- **Facilitator Guides:** 
  - [Part 1](../FACILITATOR_GUIDE.md)
  - [Part 2](../FACILITATOR_GUIDE_PART2.md)
- **Custom Agents:** [.github/agents/](../../.github/agents/)
- **Architecture:** [docs/design/architecture.md](../design/architecture.md)

---

## 🤝 Contributing

When adding new modules:

1. Follow existing naming convention: `##-topic-name.md`
2. Include Marp frontmatter
3. Add navigation links
4. Update this index
5. Update facilitator guides
6. Submit PR with module + test delivery

---

## 📝 Version History

| Version | Date | Changes |
|---------|------|---------|
| 2.0.0 | 2026-03-30 | Modular structure, role-specific support |
| 1.5.0 | 2026-03-15 | Added Skills module (Part 2) |
| 1.0.0 | 2025-12-01 | Initial monolithic presentations |

---

**Questions?** See [README.md](README.md) for detailed usage instructions or open an issue.
