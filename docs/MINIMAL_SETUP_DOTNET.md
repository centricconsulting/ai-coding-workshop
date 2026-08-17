# Minimal Local Setup Guide — .NET Track

Use this guide if you want the **fastest possible setup** for the .NET track: no Dev Container, and only the extensions strictly required to use GitHub Copilot. No language-specific VS Code extension (like C# Dev Kit) is installed here — you'll rely on the standard `dotnet` CLI for build/test.

> **Want IntelliSense, debugging, and test explorer in VS Code?** Use the full [`LOCAL_SETUP.md`](./LOCAL_SETUP.md) guide instead, which installs C# Dev Kit.

---

## 1. GitHub Account & Copilot Subscription

- [ ] **GitHub account** — [github.com](https://github.com)
- [ ] **GitHub Copilot subscription active** — verify at [github.com/settings/copilot](https://github.com/settings/copilot)

---

## 2. Git

- [ ] **Install Git**
  - **macOS**: `xcode-select --install` or `brew install git`
  - **Windows**: [git-scm.com/download/win](https://git-scm.com/download/win)
  - **Linux**: `sudo apt install git` / `sudo dnf install git`
- [ ] **Verify**: `git --version` → `2.30` or later
- [ ] **Configure identity**:
  ```bash
  git config --global user.name "Your Name"
  git config --global user.email "your.email@example.com"
  ```

---

## 3. Visual Studio Code

- [ ] **Install VS Code** (1.95+) — [code.visualstudio.com](https://code.visualstudio.com/)
- [ ] **Verify**: `code --version`
- [ ] **macOS extra step**: Command Palette (`Cmd+Shift+P`) → "Shell Command: Install 'code' command in PATH"

---

## 4. Minimal VS Code Extensions

Only the two extensions required to use Copilot:

| Extension | ID | Purpose |
|---|---|---|
| GitHub Copilot | `GitHub.copilot` | Inline AI completions |
| GitHub Copilot Chat | `GitHub.copilot-chat` | Chat interface, agents, instructions |

```bash
code --install-extension GitHub.copilot
code --install-extension GitHub.copilot-chat
```

After installing, sign in to GitHub when VS Code prompts you to activate Copilot.

---

## 5. Clone the Repository

```bash
git clone https://github.com/centricconsulting/ai-coding-workshop.git
cd ai-coding-workshop
git checkout main
git pull
git checkout -b your-name-workshop
code .
```

---

## 6. .NET 9 SDK

- [ ] **Download .NET 9 SDK** (not just the Runtime) — [dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [ ] **Verify**:
  ```bash
  dotnet --version
  ```
  Expected: `9.0.x`

---

## 7. Verify the Build

```bash
# From the repository root
dotnet restore
dotnet build TaskManager.sln
dotnet test
```

Expected: build succeeds for all projects, and tests report `total: 11, failed: 11, succeeded: 0` — **11 failing tests is correct**; they're workshop placeholders.

---

## 8. .NET HTTPS Dev Certificate

```bash
dotnet dev-certs https --trust
```

Accept any OS prompt to trust the certificate.

---

## ✅ Verify GitHub Copilot Is Working

1. Open `src/TaskManager.Domain/Tasks/Task.cs` in VS Code.
2. Check the Copilot icon in the status bar is active (not red/crossed out).
3. Add a line and type a comment, e.g., `// Method to validate task title`, press Enter — you should see gray ghost-text suggestions.
4. Press **Tab** to accept, **Esc** to dismiss, then delete the test line.
5. Open Copilot Chat (`Cmd/Ctrl+Shift+I`) and ask `@workspace What testing frameworks are used in this project?`

**Not working?** Click the Copilot status bar icon → "Sign in to GitHub", verify your subscription, or reload VS Code (Command Palette → "Developer: Reload Window").

---

## 🆘 Quick Troubleshooting

| Problem | Solution |
|---|---|
| `dotnet: command not found` | Restart terminal after install; check PATH |
| Old SDK version showing | Restart terminal; run `dotnet --list-sdks` |
| Copilot suggestions not appearing | Wait 1-2 seconds; check status bar icon; reload window |
| Build fails with "SDK not found" | Confirm SDK version with `dotnet --list-sdks` |

---

## 📋 Quick-Check Before the Workshop

```bash
dotnet --version        # 9.0.x
cd ai-coding-workshop
git pull origin main
dotnet build TaskManager.sln
```

Then open VS Code (`code .`) and confirm the Copilot status bar icon is active.

---

## 📚 Related Documentation

- [`LOCAL_SETUP.md`](./LOCAL_SETUP.md) — full setup with C# Dev Kit and other productivity extensions
- [DevContainer README](../.devcontainer/README.md) — a pre-configured containerized alternative
- [Workshop README](../README.md) — overview and lab links
