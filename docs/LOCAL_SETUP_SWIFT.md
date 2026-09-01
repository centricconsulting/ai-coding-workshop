# Swift/iOS Local Setup Guide

Use this guide for the **Swift/iOS track**. Unlike the other workshop tracks, Swift depends on **macOS, Xcode, the iOS SDK, and Simulator support**, so there is **no devcontainer** for this path.

> **Using another track?** See [`LOCAL_SETUP.md`](./LOCAL_SETUP.md) for .NET, Spring Boot, Angular, JavaScript, Python, and Kotlin local setup.
>
> **Swift/iOS is local-only.** Open the Swift package in VS Code if you want Copilot-assisted package editing, then use Xcode to run the iOS app shell.

---

## 📋 Required Software

### 1. GitHub Account & Copilot Access

- [ ] **GitHub account** — [github.com](https://github.com)
- [ ] **GitHub Copilot subscription active**
- [ ] **Verify subscription** at [github.com/settings/copilot](https://github.com/settings/copilot)

### 2. Git

- [ ] Install Git
  - Easiest macOS path: `xcode-select --install`
  - Or Homebrew: `brew install git`
- [ ] Verify:
  ```bash
  git --version
  ```
- [ ] Configure identity:
  ```bash
  git config --global user.name "Your Name"
  git config --global user.email "your.email@example.com"
  ```

### 3. Visual Studio Code

- [ ] Install VS Code (1.95 or later) — [code.visualstudio.com](https://code.visualstudio.com/)
- [ ] Verify:
  ```bash
  code --version
  ```
- [ ] In VS Code, run: **Shell Command: Install 'code' command in PATH**

### 4. Xcode + Command Line Tools

The Swift track assumes every participant has a Mac and will use **Xcode** locally.

- [ ] Install **Xcode** from the Mac App Store
- [ ] Launch Xcode once and accept the license / first-run prompts
- [ ] Install Command Line Tools:
  ```bash
  xcode-select --install
  ```
- [ ] If multiple Xcode versions are installed, point command-line tools at the version you want:
  ```bash
  sudo xcode-select -s /Applications/Xcode.app
  ```

Verify both Xcode and Swift:

```bash
xcodebuild -version
swift --version
```

Expected:
- `Xcode 15.x` or later
- `Swift version 5.9` or later

---

## 🧰 Recommended VS Code Extensions

Install the workshop's common extensions plus Swift support.

| Extension | ID | Purpose |
|---|---|---|
| GitHub Copilot | `GitHub.copilot` | Inline AI completions |
| GitHub Copilot Chat | `GitHub.copilot-chat` | Chat, agents, instructions |
| Swift for Visual Studio Code | `swiftlang.swift-vscode` | Swift Package Manager language support |
| REST Client | `humao.rest-client` | Useful for non-Swift workshop materials |
| Markdown Mermaid | `bierner.markdown-mermaid` | Diagram previews |
| Marp for VS Code | `marp-team.marp-vscode` | Slide previews |
| Markdown All in One | `yzhang.markdown-all-in-one` | Markdown editing helpers |

Install from the terminal:

```bash
code --install-extension GitHub.copilot
code --install-extension GitHub.copilot-chat
code --install-extension swiftlang.swift-vscode
code --install-extension humao.rest-client
code --install-extension bierner.markdown-mermaid
code --install-extension marp-team.marp-vscode
code --install-extension yzhang.markdown-all-in-one
```

> The Swift VS Code extension works best with **Swift Package Manager** projects such as `src-swift/`. Support for `.xcodeproj` files is more limited, so use **Xcode** for the `TaskManagerApp` UI shell.

---

## 📦 Clone the Repository

```bash
git clone https://github.com/centricconsulting/ai-coding-workshop.git
cd ai-coding-workshop
```

Create your branch:

```bash
git checkout main
git pull
git checkout -b your-name-workshop
```

Open the repo in VS Code:

```bash
code .
```

---

## 🍎 Verify the Swift Package

The business-logic track lives in `src-swift/` as a Swift Package.

### Build the Package

```bash
cd src-swift
swift build
```

### Run the XCTest Suite

```bash
swift test
```

Expected result: the `TaskManagerDomain`, `TaskManagerApplication`, and `TaskManagerInfrastructure` targets build, and the XCTest suites pass.

### Helpful VS Code Workflow

Open `src-swift/Package.swift` or any file under `src-swift/Sources/` and confirm you get:
- syntax highlighting
- completions
- Go to Definition / Find References
- inline Copilot suggestions

If Swift language features do not appear immediately, run `swift build` once first so SourceKit-LSP has fresh package metadata.

---

## 📱 Open and Run the iOS UI Shell

The minimal SwiftUI shell lives in:

- `src-swift/TaskManagerApp/TaskManagerApp.xcodeproj`

### Open the App in Xcode

From the repository root:

```bash
open src-swift/TaskManagerApp/TaskManagerApp.xcodeproj
```

Or open it manually from Xcode: **File → Open...** → `src-swift/TaskManagerApp/TaskManagerApp.xcodeproj`

### Run It in Simulator

1. Select the **TaskManagerApp** scheme
2. Choose any iPhone Simulator (for example, an iPhone 16 simulator)
3. Press **Run** (`⌘R`)

What you should see:
- a task list seeded with a few sample tasks
- a simple add-task form
- a **Complete** button for active tasks
- completed tasks shown with their final status

### Optional CLI Build

If you prefer `xcodebuild`, run something like:

```bash
xcodebuild \
  -project src-swift/TaskManagerApp/TaskManagerApp.xcodeproj \
  -scheme TaskManagerApp \
  -destination 'platform=iOS Simulator,name=iPhone 16' \
  build
```

If your machine does not have that exact simulator name installed, substitute any available iPhone simulator from Xcode.

---

## ✅ Verify GitHub Copilot Is Working

After setup:

1. Open a Swift file such as:
   - `src-swift/Sources/TaskManagerDomain/Task.swift`
   - `src-swift/Sources/TaskManagerApplication/CreateTask.swift`
2. Start typing a small helper or test name
3. Confirm Copilot suggestions appear inline
4. Open Copilot Chat and ask a Swift-specific question about the package

A good first prompt is:

```text
Explain how the Swift task manager package separates Domain, Application, Infrastructure, and the SwiftUI shell.
```

---

## 🛠️ Troubleshooting

### `swift: command not found`

- Re-run `xcode-select --install`
- Ensure Xcode is installed and launched once
- If needed, set the active developer directory explicitly:
  ```bash
  sudo xcode-select -s /Applications/Xcode.app
  ```

### Xcode opens but the package does not resolve

- From `src-swift/`, run:
  ```bash
  swift build
  ```
- Reopen the project after the package finishes resolving

### VS Code shows limited Swift support

- Confirm `swiftlang.swift-vscode` is installed
- Open the repository root or `src-swift/` so VS Code can see `Package.swift`
- Build once with `swift build` so SourceKit-LSP can index the package

### The app scheme does not appear in Xcode

- Open `src-swift/TaskManagerApp/TaskManagerApp.xcodeproj`, not just the repository folder
- In Xcode, choose **Product → Scheme → TaskManagerApp**
- If package resolution is still pending, wait for Xcode to finish before running

### Simulator build complains about a missing device name

- Open **Xcode → Window → Devices and Simulators**
- Pick any installed iPhone simulator and use that name in your `xcodebuild -destination ...` command

---

## 📚 Related Documentation

- [Workshop README](../README.md) — overview, labs, and track selection
- [General Local Setup Guide](./LOCAL_SETUP.md) — other non-Swift tracks
- [DevContainer Guide](../.devcontainer/README.md) — available containers for the other tracks
- [Lab Walkthrough Index](./labs/README.md) — all lab guides

---

**Still stuck?** Arrive a few minutes early on workshop day and validate both `swift test` and the Simulator run before the live lab begins.
