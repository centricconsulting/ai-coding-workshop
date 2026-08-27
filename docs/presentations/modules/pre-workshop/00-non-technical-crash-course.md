---
marp: true
theme: default
paginate: true
backgroundColor: #fff
---

# Non-Technical Crash Course

## Terminal, VS Code, Git & GitHub in 1 Hour

**Duration:** 60 Minutes
**Format:** Instructor-led, hands-on
**Audience:** BAs, PMs, and other non-engineering participants

---

# Why This Session Exists

The main workshop assumes you can already:

- Open a terminal
- Navigate VS Code
- Clone a repo, commit, push, pull
- Understand branches, pull requests, and issues

**Goal today:** working familiarity — not mastery — so you're ready for Lab 1.

---

# Today's Journey

```
1. Welcome & why this matters    (3 min)
2. Terminal basics               (10 min)
3. VS Code basics                (15 min)
4. Git basics                    (15 min)
5. GitHub basics + repo setup    (10 min)
6. Finish machine setup (track)  (5-15 min)
7. Wrap-up & readiness check     (7 min)
```

**Total:** ~60-70 minutes, fully hands-on

---

<!-- _class: lead -->

# Module 1: Why This Matters

**Duration:** 3 minutes

---

# The Four Tools, in Plain English

- **Terminal** — how you type instructions instead of clicking
- **VS Code** — the editor where you'll write code with Copilot
- **Git** — "track changes" for an entire folder of files, with history
- **GitHub** — the website that hosts the shared code and enables collaboration

You don't need to become an engineer today.

---

<!-- _class: lead -->

# Module 2: Terminal Basics

**Duration:** 10 minutes

---

# Open a Terminal

- **Windows:** Search "Terminal" in the Start menu
- **Mac:** `Cmd+Space`, type "Terminal"

```bash
pwd          # Print Working Directory — "where am I?"
ls           # List files (Mac/Linux)
dir          # List files (Windows)
cd Desktop   # Move into a folder
cd ..        # Move up one level
```

---

# Terminal Key Concepts

- There's always a **current location**, like Explorer/Finder but text-based
- Type a command, press `Enter` to run it
- Spelling and case matter
- `↑` (up arrow) repeats your last command

**Try it:** Move into a folder, then back out, using `cd`.

---

<!-- _class: lead -->

# Module 3: VS Code Basics

**Duration:** 15 minutes

---

# Open a Folder

- Launch VS Code
- **File → Open Folder...**
- VS Code always works on a **folder** ("workspace"), not a single file

---

# Tour the Sidebar

| Icon | Panel | Purpose |
|---|---|---|
| 📄 | Explorer | Browse files and folders |
| 🔍 | Search | Find text across all files |
| 🔀 | Source Control | Git changes |
| 🧩 | Extensions | Install add-ons like Copilot |
| 🤖 | Copilot Chat | Ask Copilot questions |

---

# Integrated Terminal & Command Palette

- **Terminal → New Terminal** (or `` Ctrl+` ``) — same terminal, inside VS Code
- `Ctrl+Shift+P` / `Cmd+Shift+P` opens the **Command Palette**
- Use it to install the **GitHub Copilot** extension and confirm sign-in

**Checkpoint:** Copilot status bar icon shows signed in, no errors.

---

<!-- _class: lead -->

# Module 4: Git Basics

**Duration:** 15 minutes

---

# Git Vocabulary

| Term | Plain-English Meaning |
|---|---|
| Repository | A folder that Git is tracking |
| Commit | A saved snapshot, with a message |
| Branch | An independent, safe line of work |
| Clone | Download a copy of a repo |
| Push / Pull | Upload / download commits |

---

# The Core Loop

```bash
git status              # What has changed?
git add <file>           # Stage a file for the next commit
git commit -m "message"  # Save a snapshot
git push                  # Upload commits to GitHub
git pull                  # Download latest commits
```

---

# Branches in 60 Seconds

```bash
git checkout main            # Switch to the main line of work
git pull                       # Make sure it's up to date
git checkout -b my-branch      # Create and switch to a new branch
```

Working on your own branch means you can't break anyone else's work.

---

<!-- _class: lead -->

# Module 5: GitHub Basics + Repo Setup

**Duration:** 10 minutes

---

# GitHub.com in 3 Concepts

- **Repository page** — the web view of a repo: code, files, history
- **Pull Request (PR)** — a request to merge your branch, reviewed by teammates
- **Issue** — a tracked task, bug, or question

---

# Clone & Branch the Workshop Repo

```bash
git clone https://github.com/centricconsulting/ai-coding-workshop.git
cd ai-coding-workshop

git checkout main
git pull
git checkout -b your-name-workshop
code .
```

---

# Verify Your Environment

**🔷 .NET Track:**
```bash
dotnet --version   # Should show 9.x.x
dotnet build
```

**🟩 Spring Boot Track:**
```bash
java -version        # Should show 21.x.x
cd src-springboot && mvn clean install
```

**Expected:** Build succeeds.

---

<!-- _class: lead -->

# Module 6: Finish Machine Setup for Your Track

**Duration:** 5-15 minutes (varies)

---

# Choose Your Track

Everyone now installs the SDK for their track, using the workshop's **minimal setup guides** — just CLI tools + Copilot, no extra IDE extensions:

- 🔷 **.NET:** [`docs/MINIMAL_SETUP_DOTNET.md`](../../../MINIMAL_SETUP_DOTNET.md)
  Installs .NET 9 SDK → `dotnet build`/`dotnet test` → Copilot check
- 🟩 **Spring Boot:** [`docs/MINIMAL_SETUP_JAVA.md`](../../../MINIMAL_SETUP_JAVA.md)
  Installs Java 21 + Maven 3.9 → `mvn clean install` → Copilot check

**Blocked from installing locally?** Use the repo's [Dev Containers](../../../../.devcontainer/README.md) instead.

---

<!-- _class: lead -->

# Module 7: Wrap-Up & Readiness Check

**Duration:** 7 minutes

---

# Readiness Checklist

- [ ] I can open a terminal and use `cd`
- [ ] I can open a folder in VS Code and find the integrated terminal
- [ ] GitHub Copilot is installed and signed in
- [ ] I know what commit, push, pull, and branch mean
- [ ] I've cloned the repo and created my own branch
- [ ] My track's SDK is installed and the build succeeded
- [ ] Copilot ghost-text suggestions appeared in a source file

---

<!-- _class: lead -->

# Ready to Begin

**Next:** [Lab 1: TDD with GitHub Copilot](../../../labs/lab-01-tdd-with-copilot.md)

**Or continue to:** [Module 00: Kickoff & Setup](../part1/00-kickoff-and-setup.md)
