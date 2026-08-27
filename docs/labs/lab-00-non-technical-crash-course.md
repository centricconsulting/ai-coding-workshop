# Lab 0: Non-Technical Crash Course (Terminal, VS Code, Git, GitHub)

**Duration**: 60 minutes
**Audience**: Business Analysts, Product Managers, and other non-engineering participants who need just enough tooling literacy to take the main workshop
**Learning Objectives**:

- Run basic commands in a terminal with confidence
- Navigate VS Code: open a folder, use the sidebar, install an extension, use the integrated terminal
- Understand and perform the core Git workflow: clone, status, add, commit, push, pull, branch
- Understand GitHub basics: repositories, branches, pull requests, issues
- Clone this workshop repository, create a personal branch, and verify the environment builds

> **Facilitator note**: This lab is designed to run **immediately before Lab 1**. It assumes zero prior command-line or Git experience. Keep the pace brisk — the goal is working familiarity, not mastery. Participants should leave with the repo cloned, on their own branch, and building successfully.

---

## Overview

The main workshop assumes participants can already open a terminal, navigate VS Code, and perform basic Git/GitHub operations. This lab closes that gap in one hour so BAs, PMs, and other non-engineering participants arrive at Lab 1 ready to focus on GitHub Copilot itself, not on tooling.

**Time Budget**:

| Segment | Duration |
|---|---|
| 1. Welcome & why this matters | 3 min |
| 2. Terminal basics | 10 min |
| 3. VS Code basics | 15 min |
| 4. Git basics | 15 min |
| 5. GitHub basics + repo setup | 10 min |
| 6. Finish machine setup for your track | 5-15 min (varies by install speed) |
| 7. Wrap-up & readiness check | 7 min |

> **Note**: Step 6 duration depends on download speeds and whether the SDK is already installed. Budget extra buffer time, or have participants start SDK downloads during Step 4/5 if bandwidth allows.

---

## Prerequisites

- ✅ Laptop with admin rights to install software
- ✅ [VS Code](https://code.visualstudio.com/) installed
- ✅ [Git](https://git-scm.com/downloads) installed
- ✅ A [GitHub.com](https://github.com/) account, signed in
- ✅ GitHub Copilot access (Individual, Business, or Enterprise subscription)

If anyone is missing these, install them now — the facilitator should pause here until everyone is ready.

---

## Step 1: Why This Matters (3 min)

Before typing anything, set expectations:

- **Terminal** = how you type instructions to your computer instead of clicking
- **VS Code** = the editor where you'll write and review code with Copilot's help
- **Git** = the tool that tracks changes to files over time (like "track changes" for code)
- **GitHub** = the website that hosts the shared copy of the code and enables collaboration (pull requests, reviews)

You don't need to become an engineer today — just comfortable enough to follow along in the main workshop.

---

## Step 2: Terminal Basics (10 min)

### 2.1 Open a Terminal

- **Windows**: Search for "Terminal" in the Start menu
- **Mac**: Open "Terminal" from Spotlight (`Cmd+Space`, type "Terminal")

### 2.2 Try These Commands

```bash
pwd          # Print Working Directory — "where am I?"
ls           # List files in the current folder (Mac/Linux)
dir          # List files in the current folder (Windows)
cd Desktop   # Change Directory — move into a folder
cd ..        # Move up one folder level
```

### 2.3 Key Concepts

- The terminal always has a **current location** (like Windows Explorer or Finder, but text-based)
- Commands are typed, then run with `Enter`
- Case matters; spelling matters — typos are the #1 source of "it doesn't work"
- `↑` (up arrow) repeats your last command — huge time-saver

**Expected outcome**: Everyone can open a terminal and move between two folders using `cd`.

---

## Step 3: VS Code Basics (15 min)

### 3.1 Open a Folder

- Launch VS Code
- **File → Open Folder...** and pick your Desktop (or any folder)
- Point out: VS Code always works on a **folder** ("workspace"), not a single file

### 3.2 Tour the Sidebar

| Icon | Panel | Purpose |
|---|---|---|
| 📄 | Explorer | Browse files and folders |
| 🔍 | Search | Find text across all files |
| 🔀 | Source Control | Git changes (we'll use this in Step 4) |
| 🧩 | Extensions | Install add-ons like GitHub Copilot |
| 🤖 | Copilot Chat | Ask Copilot questions (used heavily in Lab 1+) |

### 3.3 Open the Integrated Terminal

- Menu: **Terminal → New Terminal** (or `` Ctrl+` ``)
- This is the **same terminal** from Step 2, just inside VS Code — no need to switch windows

### 3.4 Use the Command Palette

- Press `Ctrl+Shift+P` (Windows/Linux) or `Cmd+Shift+P` (Mac)
- Type "extensions" and select **Extensions: Install Extensions**
- Search for **GitHub Copilot** and confirm it's installed and signed in (green checkmark / status bar icon)

**Expected outcome**: Everyone has opened a folder, found the integrated terminal, and confirmed Copilot is installed.

---

## Step 4: Git Basics (15 min)

> **Analogy**: Git is "track changes" for an entire folder of files, with a permanent history you can rewind.

### 4.1 Core Vocabulary

| Term | Plain-English Meaning |
|---|---|
| Repository ("repo") | A folder that Git is tracking |
| Commit | A saved snapshot of changes, with a message describing why |
| Branch | An independent line of work — like a copy you can experiment on safely |
| Clone | Download a copy of a repo from GitHub to your computer |
| Push / Pull | Upload / download commits between your computer and GitHub |

### 4.2 Hands-On: The Core Loop

Run each command one at a time and discuss what happened:

```bash
git status              # "What has changed since my last save?"
git add <file>          # Stage a file to be included in the next commit
git commit -m "message" # Save a snapshot with a description
git push                # Upload your commits to GitHub
git pull                # Download the latest commits from GitHub
```

### 4.3 Branches in 60 Seconds

```bash
git checkout main            # Switch to the main line of work
git pull                      # Make sure it's up to date
git checkout -b my-branch     # Create and switch to a new branch
```

- Working on your own branch means you can't break anyone else's work
- This is exactly what you'll do in Step 5 for the real workshop repo

**Expected outcome**: Everyone can explain `status`, `add`, `commit`, `push`, `pull`, and `branch` in plain language.

---

## Step 5: GitHub Basics + Repo Setup (10 min)

### 5.1 GitHub.com in 3 Concepts

- **Repository page**: the web view of a repo — code, files, history
- **Pull Request (PR)**: a request to merge your branch into `main`, where teammates review your changes
- **Issue**: a tracked task, bug, or question — how teams plan and discuss work

Briefly show a real PR and Issue in a browser if possible.

### 5.2 Clone the Workshop Repository

```bash
git clone https://github.com/centricconsulting/ai-coding-workshop.git
cd ai-coding-workshop
```

### 5.3 Create Your Personal Branch

```bash
git checkout main
git pull
git checkout -b your-name-workshop
```

_Replace `your-name-workshop` with your name or a unique identifier._

### 5.4 Open It in VS Code

```bash
code .
```

### 5.5 Verify the Environment (pick your track)

```bash
# .NET track
dotnet --version   # Should show 9.x.x
dotnet build        # Should succeed

# Spring Boot track
java -version        # Should show 21.x.x
cd src-springboot && mvn clean install
```

**Expected outcome**: Repo cloned, personal branch created, build succeeds.

---

## Step 6: Finish Machine Setup for Your Track (varies)

Steps 2–5 covered the shared tools (terminal, VS Code, Git, GitHub). Now each participant finishes installing the language-specific SDK for their track using the workshop's **minimal setup guides** — no extra IDE extensions required, just the CLI tools and Copilot:

- 🔷 **.NET track**: [`docs/MINIMAL_SETUP_DOTNET.md`](../MINIMAL_SETUP_DOTNET.md) — installs the .NET 9 SDK, verifies `dotnet build`/`dotnet test`, and confirms Copilot works in a `.cs` file
- 🟩 **Spring Boot track**: [`docs/MINIMAL_SETUP_JAVA.md`](../MINIMAL_SETUP_JAVA.md) — installs Java 21 + Maven 3.9, verifies `mvn clean install`, and confirms Copilot works in a Java file

> **Prefer a zero-install option?** Both guides link to the repo's [Dev Containers](../../.devcontainer/README.md) as a fallback if local installs are blocked (e.g., locked-down corporate laptops).

**Facilitator note**: Have participants pick a track now (or assign one) and work through their guide's steps 6–8 (SDK install → build verification → Copilot check). This can run in parallel with the facilitator circulating to help with install issues.

**Expected outcome**: `dotnet build` or `mvn clean install` succeeds, and Copilot ghost-text suggestions appear in a source file.

---

## Step 7: Wrap-Up & Readiness Check (7 min)

### Readiness Checklist

- [ ] I can open a terminal and move between folders with `cd`
- [ ] I can open a folder in VS Code and find the integrated terminal
- [ ] GitHub Copilot extension is installed and signed in
- [ ] I know what `commit`, `push`, `pull`, and `branch` mean
- [ ] I have cloned `ai-coding-workshop` and created my own branch
- [ ] My track's SDK is installed (.NET 9 or Java 21 + Maven) and the build succeeds
- [ ] I saw Copilot ghost-text suggestions in a source file for my track

### What's Next

You're ready for **[Lab 1: Test-Driven Development with GitHub Copilot](lab-01-tdd-with-copilot.md)**.

---

## Troubleshooting

### "git: command not found"

Git isn't installed or isn't on PATH. Reinstall from [git-scm.com](https://git-scm.com/downloads) and restart the terminal.

### "Permission denied" or asked for a password on `git push`/`git clone`

Use the HTTPS URL and sign in when prompted, or ask the facilitator about setting up a [personal access token](https://docs.github.com/en/authentication/keeping-your-account-and-data-secure/managing-your-personal-access-tokens).

### VS Code doesn't recognize `code` command

Open VS Code, press `Cmd+Shift+P` / `Ctrl+Shift+P`, run **Shell Command: Install 'code' command in PATH**, then restart the terminal.

### Build fails (`dotnet build` / `mvn clean install`)

Confirm the correct SDK version is installed (`dotnet --version` should show 9.x, `java -version` should show 21.x) and that you're running the command from the repository root (or `src-springboot/` for Java).

---

## Facilitator Tips

- Walk the room during Steps 2–5; non-technical learners get stuck on typos and window focus, not concepts
- Pair up confident and less-confident participants
- It's fine to skip deep explanations of `git add`/staging vs. `git commit` — the goal is muscle memory, not theory
- If running short on time, Step 5.5 (build verification) can be deferred to the start of Lab 1
