# Minimal Local Setup Guide — Spring Boot (Java) Track

Use this guide if you want the **fastest possible setup** for the Spring Boot track: no Dev Container, and only the extensions strictly required to use GitHub Copilot. No language-specific VS Code extensions (like Extension Pack for Java) are installed here — you'll rely on the standard `mvn`/`java` CLI for build/test.

> **Want IntelliSense, debugging, and test explorer in VS Code?** Use the full [`LOCAL_SETUP.md`](./LOCAL_SETUP.md) guide instead, which installs the Java and Spring Boot extensions.

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

## 6. Java 21 JDK

The workshop uses **Java 21 LTS**. Any distribution works; Microsoft Build of OpenJDK is recommended.

- **Option A — Microsoft Build of OpenJDK**: [aka.ms/download-jdk](https://aka.ms/download-jdk) — select Java 21
- **Option B — Temurin**: [adoptium.net](https://adoptium.net/) — select **Temurin 21 (LTS)**
- **Option C — Homebrew (macOS)**: `brew install --cask microsoft-openjdk21`
- **Option D — SDKMAN (macOS/Linux)**:
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

## 7. Maven 3.9

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

## 8. Verify the Build

```bash
# From the repository root
mvn clean install -f src-springboot/pom.xml -DskipTests
mvn test -f src-springboot/pom.xml
```

Expected output ends with `BUILD SUCCESS`.

---

## ✅ Verify GitHub Copilot Is Working

1. Open any file in `src-springboot/` in VS Code.
2. Check the Copilot icon in the status bar is active (not red/crossed out).
3. Add a line and type a comment, e.g., `// Method to validate task title`, press Enter — you should see gray ghost-text suggestions.
4. Press **Tab** to accept, **Esc** to dismiss, then delete the test line.
5. Open Copilot Chat (`Cmd/Ctrl+Shift+I`) and ask `@workspace What testing frameworks are used in this project?`

**Not working?** Click the Copilot status bar icon → "Sign in to GitHub", verify your subscription, or reload VS Code (Command Palette → "Developer: Reload Window").

---

## 🆘 Quick Troubleshooting

| Problem | Solution |
|---|---|
| `mvn: command not found` | Ensure Maven `bin/` dir is on PATH; restart terminal |
| `java -version` shows wrong version | Set `JAVA_HOME` to your Java 21 path; restart terminal |
| Copilot suggestions not appearing | Wait 1-2 seconds; check status bar icon; reload window |
| Build fails with "SDK not found" | Confirm version with `java -version` |

---

## 📋 Quick-Check Before the Workshop

```bash
java -version           # 21.x
mvn --version           # 3.9.x
cd ai-coding-workshop
git pull origin main
mvn clean install -f src-springboot/pom.xml -DskipTests
```

Then open VS Code (`code .`) and confirm the Copilot status bar icon is active.

---

## 📚 Related Documentation

- [`LOCAL_SETUP.md`](./LOCAL_SETUP.md) — full setup with Java/Spring Boot extensions
- [DevContainer README](../.devcontainer/README.md) — a pre-configured containerized alternative
- [Workshop README](../README.md) — overview and lab links
