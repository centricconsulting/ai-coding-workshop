# Modular Workshop Presentations

## Overview

This directory contains modular Marp presentations for the GitHub Copilot workshop. Each module is a standalone presentation file that can be used independently or combined with others for custom workshop delivery.

## Why Modular?

**Benefits:**
- ✅ **Self-paced learning** - Students pick and choose modules
- ✅ **Custom workshops** - Mix modules for different audiences (developers, BAs, QA, infra)
- ✅ **Role-specific content** - Specialized modules for different roles
- ✅ **Easy maintenance** - Update individual topics without affecting others
- ✅ **Flexible delivery** - 30-minute lunch & learn or full-day workshop

---

## Quick Start

### Option 1: View in VS Code (Recommended)

1. **Install Marp Extension**
   - Open VS Code
   - Install [Marp for VS Code](https://marketplace.visualstudio.com/items?itemName=marp-team.marp-vscode)
   
2. **Preview a Module**
   - Open any `.md` file in `modules/` folder
   - Click "Preview Marp Slide Deck" icon (top right)
   - Or use command: `Ctrl+Shift+P` → "Marp: Open Preview to the Side"

3. **Navigate Between Modules**
   - Use navigation links at bottom of each module
   - Or open modules sequentially from file explorer

### Option 2: Export to PDF/HTML

```bash
# Export single module
npx @marp-team/marp-cli modules/part1/00-kickoff-and-setup.md --pdf

# Export all Part 1 modules as one PDF
npx @marp-team/marp-cli modules/part1/*.md --pdf --output part1-complete.pdf

# Export with presenter notes
npx @marp-team/marp-cli modules/part1/*.md --pdf --pdf-notes --output part1-with-notes.pdf

# Export to HTML (interactive)
npx @marp-team/marp-cli modules/part2/*.md --html --output part2-interactive.html
```

---

## Module Catalog

See [index.md](index.md) for complete catalog of available modules.

**Quick Links:**
- [Part 1: Fundamentals](modules/part1/) (7 modules, ~3 hours)
- [Part 2: Advanced](modules/part2/) (8 modules, ~3 hours)
- [Role-Specific](modules/roles/) (Coming soon)

---

## Common Use Cases

### 1. Full Workshop (6 hours)

**Delivery:** Instructor-led, 2 x 3-hour sessions

```
Day 1 Morning (Part 1):
  → modules/part1/00 through 06

Day 1 Afternoon (Part 2):
  → modules/part2/00 through 07
```

### 2. Developer Fast Track (4 hours)

**Delivery:** Experienced developers, skip basics

```
Session 1 (2 hours):
  → part1/02-copilot-instructions-tdd
  → part1/03-requirements-to-code
  → part1/04-generation-refactoring

Session 2 (2 hours):
  → part2/01-interaction-models
  → part2/02-skills-customization
  → part2/03-custom-agents-intro
  → part2/05-agent-design
```

### 3. Lunch & Learn: Custom Agents (1 hour)

**Delivery:** Quick intro for decision-makers

```
→ part2/02-skills-customization (15 min, overview)
→ part2/03-custom-agents-intro (20 min, demo)
→ part2/04-workflow-agents (15 min, real examples)
→ Q&A (10 min)
```

### 4. Role-Specific Workshop: Business Analysts (future)

**Delivery:** BA-focused content

```
→ part1/00-kickoff-and-setup
→ part1/03-requirements-to-code
→ part2/02-skills-customization
→ roles/ba-requirements-modeling
→ part2/04-workflow-agents (backlog focus)
```

### 5. Self-Paced Learning

**Delivery:** Individual learners

```
1. Start with Part 1, Module 00
2. Follow "Next Module" links at bottom of each deck
3. Complete lab exercises referenced in modules
4. Progress at your own pace
5. Jump to Part 2 when ready
```

---

## For Facilitators

### Pre-Workshop Setup

1. **Environment Check**
   - Marp extension installed
   - Test preview for all modules you'll deliver
   - Review facilitator guides: [Part 1](../FACILITATOR_GUIDE.md), [Part 2](../FACILITATOR_GUIDE_PART2.md)

2. **Customize Module Selection**
   - Review [index.md](index.md) delivery patterns
   - Select modules for your audience
   - Create custom navigation flow if needed
   - Test timing with dry run

3. **Materials Preparation**
   - Export PDFs for distribution (optional)
   - Prepare lab environment
   - Review related lab guides in [docs/labs/](../labs/)

### During Delivery

**Module Structure:**
- Each module is 10-45 minutes
- Includes references to corresponding lab guides
- Navigation links at bottom for easy flow
- Can pause between modules for Q&A

**Tips:**
- **Modular = Flexible**: Adjust timing based on discussion
- **Lab Integration**: Switch to lab guide when module references hands-on time
- **Mix & Match**: Skip or reorder modules as needed
- **Interactive**: Use Marp's presentation mode or export to HTML for polls/quizzes

### Post-Workshop

- **Feedback**: Track which modules resonated
- **Iteration**: Update individual modules based on feedback
- **Sharing**: Export custom selections for different teams

---

## For Self-Paced Learners

### Getting Started

1. **Install Prerequisites**
   ```bash
   # Marp Extension for VS Code
   code --install-extension marp-team.marp-vscode
   ```

2. **Navigate to Part 1**
   - Open `modules/part1/00-kickoff-and-setup.md`
   - Preview in VS Code
   - Follow "Next Module" link at end

3. **Complete Labs**
   - Each module references a lab guide
   - Labs are in [docs/labs/](../labs/)
   - Complete lab before moving to next module

4. **Track Progress**
   - Use [index.md](index.md) as checklist
   - Mark completed modules
   - Part 1 completion = ready for Part 2

### Learning Path

```
Part 1: Fundamentals
├─ 00: Setup (15 min) → Clone repo, verify environment→ 01: Features Tour (15 min) → Learn Copilot capabilities
├─ 02: TDD (30 min) → Build NotificationService with TDD
├─ 03: Requirements (45 min) → Transform user story to code
├─ 04: Generation (45 min) → CRUD endpoints + refactoring
├─ 05: Testing (15 min) → Tests, docs, PRs
└─ 06: Wrap-Up (15 min) → Review and discuss

Part 2: Advanced
├─ 00: Recap (10 min) → Review Part 1, preview Part 2
├─ 01: Interaction Models (25 min) → Ask vs Plan vs Agent
├─ 02: Skills (30 min) → Customization hierarchy
├─ 03: Agents Intro (25 min) → Meet the workshop agents
├─ 04: Workflows (30 min) → Apply agents to real scenarios
├─ 05: Design (30 min) → Agent design principles
├─ 06: Capstone (35 min) → Build your own agent
└─ 07: Wrap-Up (10 min) → Governance and next steps
```

---

## Technical Details

### Marp Frontmatter

Each module uses this Marp configuration:

```yaml
---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---
```

**To customize:**
- `theme`: Change to custom theme (see Marp docs)
- `paginate`: Add slide numbers
- `backgroundColor`: Set background color

### Export Commands Reference

```bash
# Single module to PDF
npx @marp-team/marp-cli modules/part1/00-kickoff-and-setup.md --pdf

# Single module to HTML
npx @marp-team/marp-cli modules/part1/00-kickoff-and-setup.md --html

# All Part 1 modules to single PDF
npx @marp-team/marp-cli modules/part1/*.md --pdf --output part1-fundamentals.pdf

# All Part 2 modules to single PDF with notes
npx @marp-team/marp-cli modules/part2/*.md --pdf --pdf-notes --output part2-advanced.pdf

# Custom selection (specific modules only)
npx @marp-team/marp-cli \
  modules/part1/02-copilot-instructions-tdd.md \
  modules/part1/03-requirements-to-code.md \
  modules/part2/02-skills-customization.md \
  --pdf --output custom-selection.pdf

# Export to PPT (requires conversion tool)
npx @marp-team/marp-cli modules/part1/*.md --pptx
```

### CI/CD Integration

```yaml
# Example GitHub Actions workflow
name: Export Presentations
on: [push]
jobs:
  export:
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v3
      - name: Export Part 1
        run: |
          npx @marp-team/marp-cli docs/presentations/modules/part1/*.md \
            --pdf --output part1.pdf
      - name: Upload artifact
        uses: actions/upload-artifact@v3
        with:
          name: presentations
          path: '*.pdf'
```

---

## Creating New Modules

### Module Template

```markdown
---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

<!-- _class: lead -->

# Module X

## Your Topic Title
### Optional Subtitle

**Duration:** XX minutes

---

# Your Content Slide

- Bullet point 1
- Bullet point 2
- Bullet point 3

---

# Another Slide

Content here...

---

<!-- _class: lead -->

# Hands-On Time

**Lab Guide:** [Lab XX: Topic](../../labs/lab-XX-topic.md)

**Next Module:** [Next Topic](XX-next-topic.md)

**Previous Module:** [Previous Topic](XX-previous-topic.md)
```

### Naming Convention

- `##-descriptive-name.md`
- Two-digit prefix for ordering
- Lowercase with hyphens
- Descriptive but concise

**Examples:**
- `00-kickoff-and-setup.md`
- `02-skills-customization.md`
- `05-agent-design.md`

### Guidelines

1. **Modular & Standalone**: Each module should make sense on its own
2. **Navigation Links**: Always include at bottom of module
3. **Duration**: Estimate realistic time on title slide
4. **Lab References**: Link to corresponding lab guides
5. **Consistent Style**: Use same Marp frontmatter
6. **Update Index**: Add new module to [index.md](index.md)

---

## Migrating from Monolithic Presentations

**Legacy files (archived March 30, 2026):**
- `fundamentals-github-copilot.md` (834 lines) - [Archived](../../archive/presentations/fundamentals-github-copilot.md)
- `advanced-github-copilot.md` (862 lines) - [Archived](../../archive/presentations/advanced-github-copilot.md)

**Migration complete:**
1. ✅ Modular structure created
2. ✅ All documentation references updated
3. ✅ Facilitators transitioned to modular delivery
4. ✅ Monolithic files archived

**Current structure:** Modular presentations are the canonical version. See [index.md](index.md) for complete catalog.

---

## Troubleshooting

### "Preview doesn't work"
- **Fix**: Install Marp extension, reload VS Code
- **Command**: `code --install-extension marp-team.marp-vscode`

### "Export command fails"
- **Fix**: Install Marp CLI globally
- **Command**: `npm install -g @marp-team/marp-cli`

### "Slides look different in preview vs export"
- **Fix**: Use same Marp version for consistency
- **Check versions**: `npx @marp-team/marp-cli --version`

### "Navigation links don't work in PDF"
- **Expected**: PDFs don't support cross-file links
- **Workaround**: Export all modules as single PDF

---

## Contributing

### Adding Role-Specific Modules (Encouraged!)

We welcome contributions for role-specific content:

1. **Create module** in `modules/roles/`
2. **Follow template** (see "Creating New Modules" above)
3. **Test delivery** with target audience
4. **Add to index** with clear role tag
5. **Submit PR** with facilitator notes

**Planned roles:**
- Business Analysts
- QA Engineers
- Infrastructure/DevOps
- Product Managers
- Technical Writers

### Improving Existing Modules

1. Fork repository
2. Make changes to specific module(s)
3. Test with Marp preview
4. Submit PR with rationale
5. Include feedback from delivery (if applicable)

---

## Additional Resources

- **Marp Documentation**: https://marp.app/
- **VS Code Extension**: https://marketplace.visualstudio.com/items?itemName=marp-team.marp-vscode
- **Lab Guides**: [docs/labs/](../labs/)
- **Facilitator Guides**: [docs/FACILITATOR_GUIDE.md](../FACILITATOR_GUIDE.md)
- **Workshop Index**: [index.md](index.md)

---

## Support

**Questions or issues?**
- Review [index.md](index.md) for catalog
- Check facilitator guides
- Open GitHub issue
- Contact workshop maintainers

**Found a bug?**
- Open issue with module name
- Include steps to reproduce
- Suggest fix if possible

---

## License

See [LICENSE.md](../../LICENSE.md) at repository root.
