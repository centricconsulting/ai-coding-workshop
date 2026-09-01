# Local Setup Guide (No Dev Container)

Use this guide if you **cannot run Dev Containers** (Docker not available, corporate policy, resource constraints, etc.). Follow the section for your technology track: **.NET**, **Spring Boot (Java)**, **Angular**, **JavaScript**, or **Python**.

> **Prefer Dev Containers?** See [`.devcontainer/README.md`](../.devcontainer/README.md) for the faster, pre-configured path.
>
> **Prefer a minimal setup?** See [`MINIMAL_SETUP_DOTNET.md`](./MINIMAL_SETUP_DOTNET.md) or [`MINIMAL_SETUP_JAVA.md`](./MINIMAL_SETUP_JAVA.md) for Copilot-only setups without extra VS Code extensions.
> **There is no minimal/lightweight alternative for the Angular, JavaScript, or Python tracks** — your environment must match the relevant section below exactly, or use the [Angular devcontainer](../.devcontainer/angular-participant/) / [JavaScript devcontainer](../.devcontainer/javascript-participant/) / [Python devcontainer](../.devcontainer/python-participant/) instead.

---

## 📋 Required for All Tracks

### 1. GitHub Account & Copilot Subscription

- [ ] **GitHub account** — [github.com](https://github.com)
- [ ] **GitHub Copilot subscription active**
  - Individual: $10/month or $100/year
  - Business: Assigned by your organization admin
  - Students/Teachers: Free via [GitHub Education](https://education.github.com/)
- [ ] **Verify subscription** at [github.com/settings/copilot](https://github.com/settings/copilot) — should show "GitHub Copilot is active"

---

### 2. Git

- [ ] **Install Git**
  - **macOS**: `xcode-select --install` or via [Homebrew](https://brew.sh/): `brew install git`
  - **Windows**: [git-scm.com/download/win](https://git-scm.com/download/win)
  - **Linux**: `sudo apt install git` / `sudo dnf install git`
- [ ] **Verify**: `git --version` → `git version 2.30` or later
- [ ] **Configure identity**:
  ```bash
  git config --global user.name "Your Name"
  git config --global user.email "your.email@example.com"
  ```

---

### 3. Visual Studio Code

- [ ] **Install VS Code** (version 1.95 or later) — [code.visualstudio.com](https://code.visualstudio.com/)
- [ ] **Verify**: `code --version`
- [ ] **macOS extra step**: Open Command Palette (`Cmd+Shift+P`) → "Shell Command: Install 'code' command in PATH"

---

### 4. Required VS Code Extensions (All Tracks)

Install these regardless of which technology track you choose:

| Extension | ID | Purpose |
|---|---|---|
| GitHub Copilot | `GitHub.copilot` | Inline AI completions |
| GitHub Copilot Chat | `GitHub.copilot-chat` | Chat interface, agents, instructions |
| REST Client | `humao.rest-client` | Test HTTP endpoints from `.http` files |
| Markdown Mermaid | `bierner.markdown-mermaid` | Diagram previews in Markdown |
| Marp for VS Code | `marp-team.marp-vscode` | Workshop slide previews |
| Markdown All in One | `yzhang.markdown-all-in-one` | Markdown editing helpers |

**Install via command line** (copy/paste all at once):
```bash
code --install-extension GitHub.copilot
code --install-extension GitHub.copilot-chat
code --install-extension humao.rest-client
code --install-extension bierner.markdown-mermaid
code --install-extension marp-team.marp-vscode
code --install-extension yzhang.markdown-all-in-one
```

After installing, sign in to GitHub when VS Code prompts you to activate Copilot.

---

### 5. Clone the Repository

```bash
git clone https://github.com/centricconsulting/ai-coding-workshop.git
cd ai-coding-workshop
```

Create your personal branch:
```bash
git checkout main
git pull
git checkout -b your-name-workshop
```

Open in VS Code:
```bash
code .
```

---

## 🔷 .NET Track Setup

### .NET 9 SDK

- [ ] **Download .NET 9 SDK** (not just the Runtime) — [dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
- [ ] **Verify**:
  ```bash
  dotnet --version
  ```
  Expected: `9.0.x`

**Common issues:**
- `command not found` → Restart terminal or reboot after install
- Old version showing → Restart terminal; the installer updates PATH

### .NET VS Code Extensions

| Extension | ID | Purpose |
|---|---|---|
| C# Dev Kit | `ms-dotnettools.csdevkit` | C# IntelliSense, debugging, test explorer |

```bash
code --install-extension ms-dotnettools.csdevkit
```

> C# Dev Kit automatically pulls in the base **C#** extension (`ms-dotnettools.csharp`) as a dependency.

### Verify the .NET Build

```bash
# From the repository root
dotnet restore
dotnet build src-dotnet/TaskManager.sln
dotnet test
```

Expected build output:
```
Build succeeded.
  TaskManager.Domain succeeded
  TaskManager.Application succeeded
  TaskManager.Infrastructure succeeded
  TaskManager.Api succeeded
  TaskManager.UnitTests succeeded
  TaskManager.IntegrationTests succeeded
```

Expected test output:
```
Test summary: total: 11, failed: 11, succeeded: 0
```

> ✅ **11 failing tests is correct!** These are placeholders you will implement during the workshop.

### .NET HTTPS Dev Certificate

```bash
dotnet dev-certs https --trust
```

Accept any OS prompt to trust the certificate.

---

## 🟩 Spring Boot (Java) Track Setup

### Java 21 JDK

The workshop uses **Java 21 LTS**. Any distribution works; Microsoft Build of OpenJDK is recommended.

**Option A — Microsoft Build of OpenJDK (recommended):**
- [aka.ms/download-jdk](https://aka.ms/download-jdk) — select Java 21

**Option B — Temurin (Eclipse Adoptium):**
- [adoptium.net](https://adoptium.net/) — select **Temurin 21 (LTS)**

**Option C — Homebrew (macOS):**
```bash
brew install --cask microsoft-openjdk21
```

**Option D — SDKMAN (macOS/Linux):**
```bash
curl -s "https://get.sdkman.io" | bash
source "$HOME/.sdkman/bin/sdkman-init.sh"
sdk install java 21-ms
```

- [ ] **Verify**:
  ```bash
  java -version
  ```
  Expected: `openjdk version "21.x.x"` (or similar 21.x output)

---

### Maven 3.9

- [ ] **Install Maven 3.9+**
  - **macOS Homebrew**: `brew install maven`
  - **Windows**: [maven.apache.org/download.cgi](https://maven.apache.org/download.cgi) — add `bin/` to PATH
  - **Linux**: `sudo apt install maven` / `sudo dnf install maven`
  - **SDKMAN**: `sdk install maven 3.9.9`
- [ ] **Verify**:
  ```bash
  mvn --version
  ```
  Expected: `Apache Maven 3.9.x`

---

### Spring Boot VS Code Extensions

| Extension | ID | Purpose |
|---|---|---|
| Extension Pack for Java | `vscjava.vscode-java-pack` | Core Java support (Language Server, debugger, test runner, Maven, Gradle) |
| Spring Boot Dashboard | `vscjava.vscode-spring-boot-dashboard` | Run/debug Spring Boot apps from VS Code |
| Spring Boot Tools | `vmware.vscode-spring-boot` | Spring-specific IntelliSense and live data |
| Maven for Java | `vscjava.vscode-maven` | Maven project management |
| Gradle for Java | `vscjava.vscode-gradle` | Gradle support |

```bash
code --install-extension vscjava.vscode-java-pack
code --install-extension vscjava.vscode-spring-boot-dashboard
code --install-extension vmware.vscode-spring-boot
code --install-extension vscjava.vscode-maven
code --install-extension vscjava.vscode-gradle
```

> After installation, VS Code may prompt you to configure your JDK. Point it at your Java 21 installation.

### Configure Java Runtime in VS Code

Open VS Code Settings (`Cmd/Ctrl+,`) and add to your `settings.json` (Command Palette → "Open User Settings (JSON)"):

```json
{
  "java.configuration.runtimes": [
    {
      "name": "JavaSE-21",
      "path": "/path/to/your/java21",
      "default": true
    }
  ]
}
```

Find your Java home path:
- **macOS**: `/usr/libexec/java_home -v 21`
- **Windows**: Check `C:\Program Files\Microsoft\jdk-21.*`
- **Linux**: `dirname $(dirname $(readlink -f $(which java)))`

### Verify the Spring Boot Build

```bash
# From the repository root
mvn clean install -f src-springboot/pom.xml -DskipTests
```

Expected output ends with:
```
BUILD SUCCESS
```

Run the tests:
```bash
mvn test -f src-springboot/pom.xml
```

---

## 🅰️ Angular Track Setup

### Node.js — Exact Version Requirement

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

### Angular VS Code Extensions

| Extension | ID | Purpose |
|---|---|---|
| Angular Language Service | `Angular.ng-template` | Template IntelliSense, diagnostics |

```bash
code --install-extension Angular.ng-template
```

### Install Dependencies & Verify the Build

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

## 🟨 JavaScript Track Setup

This track is plain JavaScript with **zero dependencies** — no framework, no TypeScript, no
bundler. Node.js is the only requirement, and any current LTS release works (unlike the Angular
track, there is no minimum-version pitfall to worry about here).

### Node.js (Current LTS)

- [ ] **Verify**:
  ```bash
  node --version
  ```
  Expected: any current LTS release (e.g. `v22.x` or later)

If you don't have Node.js installed:
- **Option A — nvm (macOS/Linux)**: `nvm install --lts && nvm use --lts`
- **Option B — Direct install**: [nodejs.org](https://nodejs.org/) — download the "LTS" installer
- **Option C — Homebrew (macOS)**: `brew install node`

### Run the Tests

```bash
# From the repository root
cd src-javascript/task-manager
node --test
```

Expected: all 3 starter tests pass (`pass 3`, `fail 0`). There is nothing to install —
`node --test` uses Node's built-in test runner.

---

## 🐍 Python Track Setup

The Python track requires **Python 3.12 or later**.

- **Option A — Direct install**: [python.org/downloads](https://www.python.org/downloads/) —
  download the latest 3.12.x (or later) installer.
- **Option B — Homebrew (macOS)**: `brew install python@3.12`
- **Option C — pyenv (macOS/Linux)**:
  ```bash
  pyenv install 3.12.7
  pyenv local 3.12.7
  ```

- [ ] **Verify**:
  ```bash
  python3 --version
  ```
  Expected: `Python 3.12.x` or later

### Python VS Code Extensions

| Extension | ID | Purpose |
|---|---|---|
| Python | `ms-python.python` | Language support, debugging, environment management |
| Pylance | `ms-python.vscode-pylance` | Fast IntelliSense and type checking |
| Python Test Adapter | `littlefoxteam.vscode-python-test-adapter` | Run/debug pytest from the Test Explorer |

```bash
code --install-extension ms-python.python
code --install-extension ms-python.vscode-pylance
code --install-extension littlefoxteam.vscode-python-test-adapter
```

### Create a Virtual Environment & Verify the Tests

```bash
# From the repository root
cd src-python
python3 -m venv .venv
source .venv/bin/activate      # macOS/Linux
# .venv\Scripts\activate       # Windows (PowerShell/cmd)
pip install -r requirements.txt
pytest
```

Expected: all starter tests pass (currently 2 placeholder tests — these will be replaced with
real Domain/Application tests as you work through the labs).

---

## ✅ Verify GitHub Copilot Is Working

After setup, confirm Copilot is active:

1. Open a source file in VS Code:
   - **.NET**: `src-dotnet/src/TaskManager.Domain/Tasks/Task.cs`
   - **Java**: any file in `src-springboot/`
   - **Angular**: any file in `src-angular/task-manager/src/app/`
   - **JavaScript**: `src-javascript/task-manager/task.js`
   - **Python**: any file in `src-python/`
2. Check the **status bar** (bottom-right of VS Code window) — the Copilot icon should be active (not red/crossed out)
3. Add a new line and type a comment, e.g., `// Method to validate task title`
4. Press Enter — you should see gray "ghost text" suggestions
5. Press **Tab** to accept, **Esc** to dismiss
6. Delete the test line

**Copilot Chat test:**
1. Open Copilot Chat: `Cmd/Ctrl+Shift+I` (or click the chat icon in the sidebar)
2. Type: `@workspace What testing frameworks are used in this project?`
3. You should get a relevant response — ✅ Copilot is working!

**Not working?**
- Click the Copilot status bar icon → "Sign in to GitHub"
- Check your subscription: [github.com/settings/copilot](https://github.com/settings/copilot)
- Reload VS Code: Command Palette → "Developer: Reload Window"

---

## 🆘 Troubleshooting

| Problem | Solution |
|---|---|
| `dotnet: command not found` | Restart terminal after install; check PATH |
| `mvn: command not found` | Ensure Maven `bin/` dir is on PATH; restart terminal |
| `java -version` shows wrong version | Set `JAVA_HOME` to your Java 21 path; restart terminal |
| Copilot suggestions not appearing | Wait 1-2 seconds; check status bar icon; reload window |
| Java Language Server not starting | Reload VS Code window; check `java.configuration.runtimes` setting |
| Build fails with "SDK not found" | Confirm SDK version with `dotnet --list-sdks` / `java -version` |
| Extensions not loading | Ensure VS Code 1.95+; reload window; reinstall extensions |
| `Angular CLI requires a minimum Node.js version` | Upgrade Node to 24.15+/22.22.3+/26.0+ (see Angular Track Setup); a devcontainer avoids this entirely |
| `ng: command not found` | Use `npx ng ...` (no global install needed), or `npm install -g @angular/cli` |
| Vitest fails to start / `EBADENGINE` warnings | Non-fatal for optional packages, but confirm Node version first |
| `node: command not found` | Restart terminal after install; check PATH |
| `node --test` reports 0 tests | Confirm you're inside `src-javascript/task-manager` and using Node 18+ (test runner requires it) |
| `python: command not found` (macOS/Linux) | Use `python3` instead, or add an alias; see [`LOCAL_SETUP_PYTHON.md`](./LOCAL_SETUP_PYTHON.md) |
| `pytest: command not found` | Ensure your virtual environment is activated and `pip install -r requirements.txt` succeeded |

---

## 📋 Quick-Check Before the Workshop

Run these commands the morning of the workshop to confirm everything is still set up:

**For .NET participants:**
```bash
dotnet --version        # 9.0.x
cd ai-coding-workshop
git pull origin main
dotnet build src-dotnet/TaskManager.sln
```

**For Spring Boot participants:**
```bash
java -version           # 21.x
mvn --version           # 3.9.x
cd ai-coding-workshop
git pull origin main
mvn clean install -f src-springboot/pom.xml -DskipTests
```

**For Angular participants:**
```bash
node --version           # v24.15.0+ (or 22.22.3+ / 26.0.0+)
cd ai-coding-workshop
git pull origin main
cd src-angular/task-manager
npm install
npx ng build
npx ng test --watch=false
```

**For JavaScript participants:**
```bash
node --version           # any current LTS
cd ai-coding-workshop
git pull origin main
cd src-javascript/task-manager
node --test
```

**For Python participants:**
```bash
python3 --version        # 3.12.x or later
cd ai-coding-workshop
git pull origin main
cd src-python
pip install -r requirements.txt
pytest
```

Then open VS Code (`code .`) and confirm the Copilot status bar icon is active.

---

## 📚 Related Documentation

- [DevContainer README](../.devcontainer/README.md) — if you'd like to switch to containers later
- [Pre-Workshop Checklist](./PRE_WORKSHOP_CHECKLIST.md) — .NET-focused detailed checklist
- [Angular Track Plan](./requirements/new-language-tracks/plan-angular.md) — scope and design notes
- [JavaScript Track Plan](./requirements/new-language-tracks/plan-javascript.md) — scope and design notes
- [Python Track Plan](./requirements/new-language-tracks/plan-python.md) — scope and design notes
- [Workshop README](../README.md) — overview and lab links
- [Facilitator Guide](./FACILITATOR_GUIDE.md) — for workshop facilitators

---

**Still stuck?** Arrive 15 minutes early on workshop day — facilitators will help. As a last resort, [GitHub Codespaces](https://github.com/features/codespaces) provides a browser-based VS Code environment that requires no local setup.
