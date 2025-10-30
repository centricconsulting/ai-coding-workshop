mode: "agent"
description: 'Perform a code review focusing on adherence to coding standards, testing practices, documentation organization, and commit conventions as outlined in the project guidelines.'
tools: [changes]
Do a review of all of the code and the current change set and see if anything can be refactored or improved. Focus on adherence to the coding standards, testing practices, documentation organization, and commit conventions as outlined in the project guidelines.

---
name: "/check"
description: "Review code and suggest improvements"
usage: |
	Use `/check` in Copilot Chat to request a review of the current file, selection, or implementation. Copilot will analyze the code and suggest improvements, refactorings, or highlight potential issues.

	**Example:**
	```
	/check Review the NotificationService implementation and tests. Are there any improvements we could make while keeping the same behavior?
	```

	You can use `/check` on any file, method, or code block to get actionable feedback and best practice suggestions.

---

# /check Chat Mode

The `/check` command triggers Copilot to:
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