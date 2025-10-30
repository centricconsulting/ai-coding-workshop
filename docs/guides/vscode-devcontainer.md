# VS Code & Devcontainer Guide

## Why Use a Devcontainer?
- Ensures everyone has the same tools, SDKs, and extensions
- No need to install .NET, Node, or other dependencies locally
- Works on any OS (Windows, Mac, Linux)
- Great for workshops, onboarding, and consistent builds

## Getting Started
1. **Open this repo in VS Code**
2. If prompted, "Reopen in Container" (or use Command Palette: `Dev Containers: Reopen in Container`)
3. Wait for the container to build and dependencies to install
4. You're ready! All tools, SDKs, and extensions are pre-installed

## Copilot & AI Custom Instructions
- For best results, set your Copilot Chat custom instructions to:
  - "Assume .NET 9, xUnit, FakeItEasy, Minimal API, ILogger, async/await everywhere. Prefer Clean Architecture and DDD."
  - "Use English for all code and comments."
- You can find the full Copilot instructions in `.github/copilot-instructions.md` (linked from the repo root)
- These instructions are auto-applied in this repo, but you can copy/paste them into your Copilot Chat settings for extra clarity

## Tips for Success
- Use the built-in terminals (they run inside the devcontainer)
- All code, builds, and tests run in the containerized environment
- If you hit issues, try `Dev Containers: Rebuild and Reopen in Container`
- For more on devcontainers: [VS Code Dev Containers Docs](https://code.visualstudio.com/docs/devcontainers/containers)

---

**Questions?** Ask in your workshop channel or check the `docs/` folder for more guides.
