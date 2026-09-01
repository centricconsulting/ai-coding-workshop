# DevContainer Selection Guide

This repository contains **6 devcontainer configurations** to support different workshop roles and technology stacks.

## 🎯 Which Container Should I Use?

When you open this repository in VS Code, you'll be prompted to select a devcontainer configuration. Choose based on your role:

---

### 1️⃣ **Workshop Maintainer** (Full Toolchain)

**Location:** `.devcontainer/maintainer/`

**Who should use this:**
- Workshop authors and content creators
- Facilitators preparing for dual-stack delivery
- Contributors working on both .NET and Spring Boot examples
- Anyone needing to build/test both implementations

**What's included:**
- ✅ .NET 9 SDK
- ✅ Java 21 JDK (LTS)
- ✅ Maven 3.9+ and Gradle 8+
- ✅ Node.js 20 + Marp CLI (for presentations)
- ✅ All VS Code extensions (C#, Java, Spring Boot, Markdown, Marp)
- ✅ GitHub CLI

**Builds:**
- Both `TaskManager.sln` (.NET) and `src-springboot/` (Java)
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
- ✅ Minimal overhead — smallest of the six containers

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

| Feature | Maintainer | .NET Participant | Spring Boot Participant | Angular Participant | JavaScript Participant | Python Participant |
|---------|-----------|------------------|------------------------|----------------------|--------------------------|---------------------|
| **.NET 9 SDK** | ✅ | ✅ | ❌ | ❌ | ❌ | ❌ |
| **Java 21 JDK** | ✅ | ❌ | ✅ | ❌ | ❌ | ❌ |
| **Maven/Gradle** | ✅ | ❌ | ✅ | ❌ | ❌ | ❌ |
| **Node.js / Angular CLI** | ✅ | ❌ | ❌ | ✅ | Node.js only (no CLI) | ❌ |
| **Python 3.12** | ❌ | ❌ | ❌ | ❌ | ❌ | ✅ |
| **Marp CLI** | ✅ | ❌ | ❌ | ❌ | ❌ | ❌ |
| **All Extensions** | ✅ | .NET only | Java only | Angular only | JavaScript only | Python only |
| **Build Time** | ~3-5 min | ~1-2 min | ~2-3 min | ~1-2 min | <1 min | ~1-2 min |
| **Container Size** | ~5 GB | ~2 GB | ~3 GB | ~1.5 GB | ~1 GB | ~1.5 GB |
| **Copilot Instructions** | Both | .NET | Spring Boot | N/A (Angular track has no dedicated instructions file yet) | N/A (JavaScript track has no dedicated instructions file yet) | N/A (Python track has no dedicated instructions file yet) |

---

## 🛠️ Troubleshooting

### "Which Container Should I Use?" Decision Tree

```
Are you creating workshop content or facilitating?
├─ YES → Use Maintainer
└─ NO → Are you following .NET or Spring Boot track?
    ├─ .NET → Use .NET Participant
    └─ Spring Boot → Use Spring Boot Participant
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

### For Maintainers
1. Verify both builds work
2. Test presentations: `marp --version`
3. Review [Facilitator Guide](../docs/FACILITATOR_GUIDE.md)

---

## 🔗 Related Documentation

- [Workshop README](../README.md) - Main workshop overview
- [Local Setup Guide](../docs/LOCAL_SETUP.md) - **No Dev Container?** Manual setup for .NET and Spring Boot
- [Pre-Workshop Checklist](../docs/PRE_WORKSHOP_CHECKLIST.md) - Setup requirements
- [Pattern Translation Guide](../docs/guides/pattern-translation.md) - .NET ↔ Java mappings
- [Facilitator Guide](../docs/FACILITATOR_GUIDE.md) - Delivery instructions

---

**Questions or issues?** Open an issue in the workshop repository.
