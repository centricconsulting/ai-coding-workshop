# Local Setup Guide — Angular Track

Use this guide if you're **not using the devcontainer** for the Angular track. Unlike the .NET
and Spring Boot tracks, there is **no minimal/lightweight local-setup alternative** for Angular —
your environment must match the specification below exactly, or you should use the
[Angular devcontainer](../.devcontainer/angular-participant/) instead.

> **Prefer Dev Containers?** See [`.devcontainer/README.md`](../.devcontainer/README.md) for the
> faster, pre-configured path (recommended).

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

## 4. Required VS Code Extensions

| Extension | ID | Purpose |
|---|---|---|
| GitHub Copilot | `GitHub.copilot` | Inline AI completions |
| GitHub Copilot Chat | `GitHub.copilot-chat` | Chat interface, agents, instructions |
| Angular Language Service | `Angular.ng-template` | Template IntelliSense, diagnostics |

```bash
code --install-extension GitHub.copilot
code --install-extension GitHub.copilot-chat
code --install-extension Angular.ng-template
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

## 6. Node.js — Exact Version Requirement

The Angular track requires **Node.js 24.15.0 or later** (or 22.22.3+, or 26.0.0+) — this is a hard
requirement of Angular CLI 22, not just a recommendation. Older Node 22.x builds (e.g. 22.16)
**will fail** with an `Angular CLI requires a minimum Node.js version` error.

- **Option A — nvm (macOS/Linux)**:
  ```bash
  curl -o- https://raw.githubusercontent.com/nvm-sh/nvm/v0.40.1/install.sh | bash
  nvm install 24
  nvm use 24
  ```
- **Option B — Direct install**: [nodejs.org](https://nodejs.org/) — download the "24.x LTS" or later installer.
- **Option C — Homebrew (macOS)**: `brew install node@24`

- [ ] **Verify**:
  ```bash
  node --version
  ```
  Expected: `v24.15.0` or later (or `v22.22.3`+, or `v26.0.0`+)

---

## 7. Install Dependencies & Verify the Build

```bash
# From the repository root
cd src-angular/task-manager
npm install
npx ng build
npx ng test --watch=false
```

Expected: build completes with "Application bundle generation complete", and all tests pass
(`Test Files  2 passed`, `Tests  6 passed`).

---

## ✅ Verify GitHub Copilot Is Working

1. Open any file in `src-angular/task-manager/src/app/` in VS Code.
2. Check the Copilot icon in the status bar is active (not red/crossed out).
3. Add a line and type a comment, e.g., `// Validate that title is not empty`, press Enter — you
   should see gray ghost-text suggestions.
4. Press **Tab** to accept, **Esc** to dismiss, then delete the test line.
5. Open Copilot Chat (`Cmd/Ctrl+Shift+I`) and ask
   `@workspace What testing framework does the Angular track use?`

**Not working?** Click the Copilot status bar icon → "Sign in to GitHub", verify your
subscription, or reload VS Code (Command Palette → "Developer: Reload Window").

---

## 🆘 Quick Troubleshooting

| Problem | Solution |
|---|---|
| `Angular CLI requires a minimum Node.js version` | Upgrade Node to 24.15+/22.22.3+/26.0+ (see Section 6); a devcontainer avoids this entirely |
| `ng: command not found` | Use `npx ng ...` (no global install needed), or `npm install -g @angular/cli` |
| Copilot suggestions not appearing | Wait 1-2 seconds; check status bar icon; reload window |
| Vitest fails to start / `EBADENGINE` warnings | Non-fatal for optional packages, but confirm Node version first (Section 6) |

---

## 📋 Quick-Check Before the Workshop

```bash
node --version           # v24.15.0+ (or 22.22.3+ / 26.0.0+)
cd ai-coding-workshop
git pull origin main
cd src-angular/task-manager
npm install
npx ng build
npx ng test --watch=false
```

Then open VS Code (`code .`) and confirm the Copilot status bar icon is active.

---

## 📚 Related Documentation

- [DevContainer README](../.devcontainer/README.md) — the recommended, pre-configured alternative
- [Angular Track Plan](./requirements/new-language-tracks/plan-angular.md) — scope and design notes
- [Workshop README](../README.md) — overview and lab links
