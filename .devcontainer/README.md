# DevContainer Selection Guide

This repository contains **7 devcontainer configurations** to support different workshop roles and technology stacks, plus **one Swift/iOS track that is local-setup-only**.

## 🎯 Which Container Should I Use?

When you open this repository in VS Code, you'll be prompted to select a devcontainer configuration. Choose based on your role:

---

### 1️⃣ **Workshop Maintainer** (Full Toolchain)

**Location:** `.devcontainer/maintainer/`

**Who should use this:**
- Workshop authors and content creators
- Facilitators preparing for multi-stack delivery
- Contributors working across the .NET, Spring Boot, Angular, JavaScript, Python, Kotlin, and Swift tracks
- Anyone needing to build/test multiple implementations at once

**What's included:**
- ✅ .NET 9 SDK
- ✅ Java 21 JDK (LTS)
- ✅ Maven 3.9+ and Gradle 8+ (covers Spring Boot and Kotlin)
- ✅ Node.js 20 (covers Angular and JavaScript) + Marp CLI (for presentations)
- ✅ Python 3.12
- ✅ All VS Code extensions (C#, Java, Spring Boot, Kotlin, Angular, Python, Markdown, Marp)
- ✅ GitHub CLI

**Builds:**
- `TaskManager.sln` (.NET), `src-springboot/` (Java), `src-kotlin/` (Kotlin/Gradle)
- `src-angular/task-manager` (Angular), `src-javascript/task-manager` (JavaScript)
- `src-python` (Python/pytest)
- All presentations and documentation
- Full workshop materials

---

### 2️⃣ **.NET Participant** (Streamlined)

**Location:** `.devcontainer/dotnet-participant/`

**Who should use this:**
- Workshop participants following the .NET track
- Self-paced learners using C# examples
- Developers working in .NET 9 environment

**What's included:**
- ✅ .NET 9 SDK
- ✅ C# Dev Kit extension
- ✅ GitHub Copilot extensions
- ✅ Minimal overhead for fast startup

**Auto-activates:**
- `.github/instructions/dotnet.instructions.md` (automatically loads when editing C# files)

**Builds:**
- `src-dotnet/TaskManager.sln` (.NET solution only)

---

### 3️⃣ **Spring Boot Participant** (Streamlined)

**Location:** `.devcontainer/springboot-participant/`

**Who should use this:**
- Workshop participants following the Spring Boot track
- Enterprise Java teams modernizing from Mule ESB
- Self-paced learners using Java examples
- Developers working in Spring Boot 3.x environment

**What's included:**
- ✅ Java 21 JDK (LTS)
- ✅ Maven 3.9+ and Gradle 8+
- ✅ Extension Pack for Java
- ✅ Spring Boot Extension Pack
- ✅ GitHub Copilot extensions
- ✅ Minimal overhead for fast startup

**Auto-activates:**
- `.github/instructions/springboot.instructions.md` (automatically loads when editing files in src-springboot/)

**Builds:**
- `src-springboot/` (Spring Boot solution only)

---

### 4️⃣ **Angular Participant** (Streamlined)

**Location:** `.devcontainer/angular-participant/`

**Who should use this:**
- Workshop participants following the Angular track
- Frontend developers building the self-contained Angular TaskManager SPA

**What's included:**
- ✅ Node.js 22 + Angular CLI (via `npx`)
- ✅ Angular Language Service extension
- ✅ GitHub Copilot extensions
- ✅ Minimal overhead for fast startup

**Builds:**
- `src-angular/task-manager/` (Angular SPA only — no backend required)

**Note:** There is no minimal/lightweight local-setup alternative for this track (unlike the .NET/Java tracks' `MINIMAL_SETUP_*.md` guides). Angular participants must either use this devcontainer or set up a local environment matching the [Angular section of `LOCAL_SETUP.md`](../docs/LOCAL_SETUP.md#-angular-track-setup) exactly.

---

### 5️⃣ **JavaScript Participant** (Streamlined)

**Location:** `.devcontainer/javascript-participant/`

**Who should use this:**
- Workshop participants following the plain-JavaScript, non-technical track
- Non-engineering participants (BAs, PMs, etc.) doing a lighter-weight version of the workshop

**What's included:**
- ✅ Node.js 22 LTS (no framework, no TypeScript, no bundler)
- ✅ GitHub Copilot extensions
- ✅ Minimal overhead — smallest of the participant containers

**Builds:**
- `src-javascript/task-manager/` (plain JavaScript, zero dependencies — tested with Node's built-in `node:test`)

**Note:** Same no-minimal-setup policy as the Angular track: use this devcontainer or match the [JavaScript section of `LOCAL_SETUP.md`](../docs/LOCAL_SETUP.md#-javascript-track-setup) exactly. In practice this track's local setup is already minimal (just Node + VS Code + Copilot).

---

### 6️⃣ **Python Participant** (Streamlined)

**Location:** `.devcontainer/python-participant/`

**Who should use this:**
- Workshop participants following the Python track
- Backend developers building the FastAPI TaskManager port
- Self-paced learners using Python examples

**What's included:**
- ✅ Python 3.12
- ✅ Python + Pylance extensions
- ✅ Python Test Adapter (pytest integration)
- ✅ GitHub Copilot extensions
- ✅ Minimal overhead for fast startup

**Builds:**
- `src-python/` (Python/FastAPI solution — installs `requirements.txt` and runs `pytest` as a sanity check)

**Note:** Same no-minimal-setup policy as the Angular/JavaScript tracks: use this devcontainer or match the [Python section of `LOCAL_SETUP.md`](../docs/LOCAL_SETUP.md#-python-track-setup) exactly.

---

### 7️⃣ **Kotlin Participant** (Streamlined)

**Location:** `.devcontainer/kotlin-participant/`

**Who should use this:**
- Workshop participants following the Kotlin track
- Mobile-oriented developers working on Kotlin/JVM domain and application logic
- Self-paced learners using the JVM-only Kotlin starter

**What's included:**
- ✅ Java 21 JDK (LTS)
- ✅ Gradle 8+
- ✅ Kotlin language extension
- ✅ GitHub Copilot extensions
- ✅ Minimal overhead for fast startup

**Builds:**
- `src-kotlin/` (Kotlin/JVM multi-module starter — runs `gradle build` as a sanity check)

**Note:** Same no-minimal-setup policy as the Angular/JavaScript/Python tracks: use this devcontainer or match the [Kotlin section of `LOCAL_SETUP.md`](../docs/LOCAL_SETUP.md#-kotlin-track-setup) exactly.

---

### 8️⃣ **Swift/iOS Track** (Local Setup Only)

**Location:** `N/A — see ../docs/LOCAL_SETUP_SWIFT.md`

**Who should use this:**
- Workshop participants following the Swift/iOS track
- Mobile developers working on the Swift Package and SwiftUI shell
- Self-paced learners using the macOS + Xcode local workflow

**What's included:**
- ✅ Swift Package under `src-swift/`
- ✅ Thin SwiftUI app shell under `src-swift/TaskManagerApp/`
- ✅ XCTest-based package tests
- ✅ GitHub Copilot-friendly local editing in VS Code
- ❌ No devcontainer (Xcode, the iOS SDK, and Simulator require local macOS tooling)

**Builds:**
- `src-swift/` with `swift build` / `swift test`
- `src-swift/TaskManagerApp/TaskManagerApp.xcodeproj` in Xcode or `xcodebuild`

**Note:** Follow [`LOCAL_SETUP_SWIFT.md`](../docs/LOCAL_SETUP_SWIFT.md) exactly. This track has **no devcontainer** and **no lightweight/minimal alternative**.

---

## 🚀 How to Select

### First Time Opening the Repository

1. Open the repository in VS Code
2. VS Code will detect multiple devcontainer configurations
3. You'll see a prompt: **"Select a Dev Container configuration to use"**
4. Choose based on the guidance above

### Switching Between Containers

1. Press `F1` or `Ctrl+Shift+P` (Windows/Linux) / `Cmd+Shift+P` (Mac)
2. Type: **"Dev Containers: Rebuild and Reopen in Container"**
3. Select the new container configuration
4. Wait for container rebuild (1-5 minutes depending on container)

---

## 📋 Container Comparison

| Feature | Maintainer | .NET Participant | Spring Boot Participant | Angular Participant | JavaScript Participant | Python Participant | Kotlin Participant | Swift/iOS (Local Only) |
|---------|-----------|------------------|------------------------|----------------------|--------------------------|---------------------|--------------------|------------------------|
| **.NET 9 SDK** | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ | N/A |
| **Java 21 JDK** | ✅ | ❌ | ✅ | ❌ | ❌ | ❌ | ✅ | N/A |
| **Maven/Gradle** | ✅ | ❌ | ✅ | ❌ | ❌ | ❌ | Gradle only | N/A |
| **Node.js / Angular CLI** | ✅ | ❌ | ❌ | ✅ | Node.js only (no CLI) | ❌ | ❌ | N/A |
| **Python 3.12** | ✅ | ❌ | ❌ | ❌ | ❌ | ✅ | ❌ | N/A |
| **Kotlin/JVM Tooling** | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ | N/A |
| **Xcode / iOS SDK** | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | macOS local only |
| **Marp CLI** | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ | ❌ | N/A |
| **All Extensions** | ✅ | .NET only | Java only | Angular only | JavaScript only | Python only | Kotlin only | Swift + Copilot locally |
| **Build Time** | ~6-8 min | ~1-2 min | ~2-3 min | ~1-2 min | <1 min | ~1-2 min | ~1-2 min | N/A — local machine dependent |
| **Container Size** | ~6 GB | ~2 GB | ~3 GB | ~1.5 GB | ~1 GB | ~1.5 GB | ~2 GB | N/A — see Local Setup Guide |
| **Copilot Instructions** | Both | .NET | Spring Boot | N/A (Angular track has no dedicated instructions file yet) | N/A (JavaScript track has no dedicated instructions file yet) | N/A (Python track has no dedicated instructions file yet) | N/A (Kotlin track has no dedicated instructions file yet) | N/A (Swift track has no dedicated instructions file yet) |

---

## 🛠️ Troubleshooting

### "Which Container Should I Use?" Decision Tree

```
Are you creating workshop content or facilitating?
├─ YES → Use Maintainer
└─ NO → Which track are you following?
    ├─ .NET → Use .NET Participant
    ├─ Spring Boot → Use Spring Boot Participant
    ├─ Python → Use Python Participant
    ├─ Kotlin → Use Kotlin Participant
    ├─ Swift/iOS → Use LOCAL_SETUP_SWIFT.md (no devcontainer)
    ├─ Angular → Use Angular Participant
    └─ JavaScript → Use JavaScript Participant
```

### Container Won't Build

**Error:** "Failed to build devcontainer"

**Solutions:**
1. Ensure Docker Desktop is running
2. Check available disk space (need 5+ GB free)
3. Try rebuilding: `F1` → "Dev Containers: Rebuild Container"
4. Clear Docker cache: `docker system prune -a`

### Copilot Instructions Not Loading

**Instructions now auto-load based on file context:**
- `.github/instructions/dotnet.instructions.md` loads for `**/*.cs` files
- `.github/instructions/springboot.instructions.md` loads for `src-springboot/**` files
- No manual configuration needed - instructions activate automatically when you open files

**If instructions aren't applying:**
- Verify `.github/instructions/` directory exists
- Check file has correct `applyTo:` frontmatter
- Reload VS Code window: `F1` → "Developer: Reload Window"

### Wrong Container Selected

**To switch containers:**
1. `F1` → "Dev Containers: Rebuild and Reopen in Container"
2. Select correct configuration
3. Wait for rebuild

---

## 📚 Next Steps

### For .NET Participants
1. Verify build: `dotnet build src-dotnet/TaskManager.sln`
2. Run tests: `dotnet test`
3. Start with [Lab 1 (.NET)](../docs/labs/lab-01-tdd-with-copilot/dotnet.md)

### For Spring Boot Participants
1. Verify build: `mvn clean install -f src-springboot/pom.xml`
2. Run tests: `mvn test -f src-springboot/pom.xml`
3. Start with [Lab 1 (Spring Boot)](../docs/labs/lab-01-tdd-with-copilot/springboot.md)

### For Python Participants
1. Verify install: `cd src-python && pip install -r requirements.txt`
2. Run tests: `pytest`
3. Start with [Lab 1](../docs/labs/lab-01-tdd-with-copilot.md)

### For Kotlin Participants
1. Verify build: `cd src-kotlin && gradle build`
2. Run tests: `gradle test`
3. Start with [Lab 1 (Kotlin)](../docs/labs/lab-01-tdd-with-copilot-kotlin.md)

### For Swift Participants
1. Verify package build: `cd src-swift && swift build`
2. Run tests: `swift test`
3. Open `src-swift/TaskManagerApp/TaskManagerApp.xcodeproj` in Xcode and start with [Lab 1 (Swift)](../docs/labs/lab-01-tdd-with-copilot-swift.md)

### For Maintainers
1. Verify all track builds work (.NET, Spring Boot, Kotlin, Angular, JavaScript, Python, Swift)
2. Test presentations: `marp --version`
3. Review [Facilitator Guide](../docs/FACILITATOR_GUIDE.md)

---

## 🔗 Related Documentation

- [Workshop README](../README.md) - Main workshop overview
- [Local Setup Guide](../docs/LOCAL_SETUP.md) - **No Dev Container?** Manual setup for .NET, Spring Boot, Angular, JavaScript, Python, and Kotlin
- [Swift Local Setup Guide](../docs/LOCAL_SETUP_SWIFT.md) - Swift/iOS track setup on macOS with Xcode (no devcontainer)
- [Pre-Workshop Checklist](../docs/PRE_WORKSHOP_CHECKLIST.md) - Setup requirements
- [Pattern Translation Guide](../docs/guides/pattern-translation.md) - .NET ↔ Java mappings
- [Facilitator Guide](../docs/FACILITATOR_GUIDE.md) - Delivery instructions

---

**Questions or issues?** Open an issue in the workshop repository.
