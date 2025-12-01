---
name: "Check"
description: 'Perform a code review focusing on adherence to coding standards, testing practices, documentation organization, and commit conventions as outlined in the project guidelines.'
tools: [changes]
---

# Check Copilot Agent

The **Check** Copilot agent performs code reviews to:
- Review the provided code for clarity, maintainability, and adherence to best practices
- Suggest refactorings, simplifications, or improvements
- Highlight anti-patterns or code smells
- Recommend additional tests or documentation if needed

**When to use:**
- After completing a feature or refactor
- Before submitting a pull request
- When you want a second opinion on your code

**Tips:**
- Be specific in your prompt for targeted feedback
- Use with code selections or filenames for focused reviews
- Reference specific files using `#file:filename.cs` syntax for precise analysis
