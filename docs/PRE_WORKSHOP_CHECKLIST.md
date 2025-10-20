# Pre-Workshop Environment Checklist

Use this checklist to verify your environment is ready **before** attending the GitHub Copilot workshop. Completing these steps in advance ensures you can focus on learning during the workshop without setup delays.

## ✅ Required Setup (Complete Before Workshop)

### 1. GitHub Account & Copilot Subscription

- [ ] **GitHub account created**
  - Visit [github.com](https://github.com) to sign up if needed
  
- [ ] **GitHub Copilot subscription active**
  - Individual: $10/month or $100/year
  - Business: Through your organization
  - Students/Teachers: Free through [GitHub Education](https://education.github.com/)
  
- [ ] **Verify subscription status**
  1. Go to [github.com/settings/copilot](https://github.com/settings/copilot)
  2. Should show "GitHub Copilot is active"
  3. Note your subscription type (Individual/Business/Free)

**Troubleshooting:**
- If no subscription: Visit [github.com/features/copilot](https://github.com/features/copilot) to subscribe
- If using Business/Organization: Contact your admin to be added

---

### 2. Visual Studio Code Installation

- [ ] **VS Code installed** (version 1.80 or later)
  - Download: [code.visualstudio.com](https://code.visualstudio.com/)
  - Verify: Run `code --version` in terminal
  
Expected output:
```
1.95.0 (or later)
```

**Platform-specific notes:**
- **macOS**: Install VS Code, then open Command Palette (Cmd+Shift+P) → "Shell Command: Install 'code' command in PATH"
- **Windows**: Add to PATH during installation
- **Linux**: Follow distribution-specific instructions

---

### 3. VS Code Extensions

Install these required extensions:

- [ ] **GitHub Copilot** (GitHub.copilot)
  1. Open VS Code
  2. Click Extensions icon (left sidebar) or press `Cmd/Ctrl+Shift+X`
  3. Search for "GitHub Copilot"
  4. Click "Install"
  5. Sign in with your GitHub account when prompted
  
- [ ] **GitHub Copilot Chat** (GitHub.copilot-chat)
  - Search for "GitHub Copilot Chat"
  - Click "Install"
  
- [ ] **C# Dev Kit** (ms-dotnettools.csdevkit)
  - Search for "C# Dev Kit"
  - Click "Install"
  - Includes C# language support, IntelliSense, and debugging

**Verify extensions installed:**
1. Open Command Palette (`Cmd/Ctrl+Shift+P`)
2. Type "Extensions: Show Installed Extensions"
3. Confirm all three are listed

**Verify Copilot is working:**
1. Create a new file: `test.cs`
2. Type: `// Function to calculate fibonacci`
3. Press Enter - you should see gray "ghost text" suggestions
4. Press Tab to accept or Esc to dismiss
5. ✅ If you see suggestions, Copilot is working!
6. Delete test file

---

### 4. .NET 9 SDK Installation

- [ ] **.NET 9 SDK installed**
  - Download: [dotnet.microsoft.com/download/dotnet/9.0](https://dotnet.microsoft.com/download/dotnet/9.0)
  - Choose ".NET 9 SDK" (not Runtime)
  - Run installer for your platform

- [ ] **Verify installation**
  
Run in terminal:
```bash
dotnet --version
```

Expected output:
```
9.0.x (any 9.x.x version is fine)
```

**Common issues:**
- **Command not found**: Restart terminal or reboot computer
- **Old version showing**: Uninstall old versions, reinstall .NET 9, restart terminal
- **Multiple versions**: That's OK! The workshop uses .NET 9, but having 6/7/8 won't hurt

---

### 5. Git Installation & Configuration

- [ ] **Git installed**
  - macOS: Install via [Xcode Command Line Tools](https://developer.apple.com/xcode/) or [Homebrew](https://brew.sh/)
  - Windows: Download from [git-scm.com](https://git-scm.com/download/win)
  - Linux: Use package manager (`apt`, `yum`, `dnf`)

- [ ] **Verify Git version**

```bash
git --version
```

Expected: `git version 2.30` or later

- [ ] **Configure Git identity** (if not already done)

```bash
git config --global user.name "Your Name"
git config --global user.email "your.email@example.com"
```

- [ ] **Verify configuration**

```bash
git config --global user.name
git config --global user.email
```

Should display your name and email.

---

### 6. Clone Workshop Repository

- [ ] **Clone the repository**

```bash
git clone https://github.com/centricconsulting/ai-coding-workshop.git
cd ai-coding-workshop
```

- [ ] **Checkout starter-projects branch**

```bash
git checkout starter-projects
```

Expected output:
```
Switched to branch 'starter-projects'
```

- [ ] **Open in VS Code**

```bash
code .
```

VS Code should open with the workshop repository.

---

### 7. Verify Solution Builds

- [ ] **Restore dependencies**

```bash
dotnet restore
```

Expected: "Restore succeeded" with no errors

- [ ] **Build solution**

```bash
dotnet build
```

Expected output:
```
Build succeeded in X.Xs
TaskManager.Domain succeeded
TaskManager.Application succeeded
TaskManager.Infrastructure succeeded
TaskManager.Api succeeded
TaskManager.ConsoleApp succeeded
TaskManager.UnitTests succeeded
TaskManager.IntegrationTests succeeded
```

- [ ] **Run tests** (should have failures - this is expected!)

```bash
dotnet test
```

Expected output:
```
Test summary: total: 11, failed: 11, succeeded: 0
```

✅ **This is correct!** The 11 failing tests are placeholders you'll implement during the workshop.

**Build issues?**
- Run `dotnet clean` then try again
- Check .NET version is 9.x
- Ensure all extensions are installed

---

### 8. Verify GitHub Copilot Integration

- [ ] **Open a C# file**
  - Navigate to `src/TaskManager.Domain/Tasks/Task.cs`
  
- [ ] **Check Copilot status bar**
  - Look at bottom-right of VS Code window
  - Should see GitHub Copilot icon
  - Icon should show checkmark (✅) or be blue/white (active)
  - If red/crossed out, click it and sign in

- [ ] **Test inline suggestions**
  1. At the end of the file, add a new line
  2. Type: `// Method to validate task title`
  3. Press Enter
  4. Start typing: `public static bool`
  5. You should see gray "ghost text" completing the method
  6. Press Tab to accept or Esc to dismiss
  7. Delete the test code

- [ ] **Test Copilot Chat**
  1. Open Copilot Chat: `Cmd/Ctrl+Shift+I` (or click chat icon in left sidebar)
  2. Type: `What testing frameworks are used in this project?`
  3. Press Enter
  4. Should get a response mentioning xUnit and FakeItEasy
  5. ✅ Chat is working!

- [ ] **Test @workspace participant**
  1. In Copilot Chat, type: `@workspace Where is the Task entity defined?`
  2. Should respond with file path: `src/TaskManager.Domain/Tasks/Task.cs`
  3. ✅ Workspace context is working!

**Copilot not working?**
- Click Copilot icon in status bar → "Sign in to GitHub"
- Check subscription at [github.com/settings/copilot](https://github.com/settings/copilot)
- Reload window: Command Palette → "Developer: Reload Window"
- Check internet connection (Copilot requires online access)

---

## 📋 Workshop Day Quick Check (5 minutes before)

Run these commands to verify everything still works:

```bash
# 1. Check .NET
dotnet --version
# Should show: 9.x.x

# 2. Navigate to workshop directory
cd path/to/ai-coding-workshop
git checkout starter-projects

# 3. Pull latest changes
git pull origin starter-projects

# 4. Verify build
dotnet build
# Should show: Build succeeded

# 5. Check Copilot status in VS Code
code .
# Check status bar icon is active (✅)
```

---

## 🆘 Common Issues & Solutions

### Issue: "GitHub Copilot is not available"

**Solution:**
1. Check subscription: [github.com/settings/copilot](https://github.com/settings/copilot)
2. Sign out and back in: Click Copilot status bar icon → Sign out → Sign in
3. Reload VS Code window: Command Palette → "Developer: Reload Window"

### Issue: "dotnet: command not found"

**Solution:**
1. Ensure .NET 9 SDK is installed (not just Runtime)
2. Restart terminal/computer after installation
3. Check PATH environment variable includes .NET

### Issue: Build fails with "SDK not found"

**Solution:**
1. Run `dotnet --list-sdks` to see installed SDKs
2. Should include `9.0.xxx`
3. If missing, reinstall .NET 9 SDK
4. If multiple SDKs, ensure global.json (if present) doesn't pin to old version

### Issue: Copilot suggestions not appearing

**Solution:**
1. Ensure you're typing in a supported file (`.cs`, `.md`, etc.)
2. Wait 1-2 seconds after typing
3. Check Copilot isn't disabled for the file type
4. Try closing and reopening the file
5. Check Copilot status bar icon isn't showing error

### Issue: Extensions not loading

**Solution:**
1. Ensure VS Code is up to date (Help → Check for Updates)
2. Disable other AI/autocomplete extensions that might conflict
3. Reload window: Command Palette → "Developer: Reload Window"
4. Reinstall extensions if needed

### Issue: Git clone fails

**Solution:**
1. Check internet connection
2. If using SSH: Ensure SSH keys are configured on GitHub
3. Try HTTPS instead: `git clone https://github.com/centricconsulting/ai-coding-workshop.git`
4. Check firewall/proxy settings

---

## 📧 Getting Help

If you encounter issues completing this checklist:

1. **Check workshop documentation**:
   - Main README: `README.md`
   - Facilitator Guide: `docs/FACILITATOR_GUIDE.md`
   
2. **Official documentation**:
   - [GitHub Copilot Docs](https://docs.github.com/en/copilot)
   - [.NET Installation Guide](https://learn.microsoft.com/en-us/dotnet/core/install/)
   - [VS Code Setup](https://code.visualstudio.com/docs/setup/setup-overview)

3. **Contact workshop facilitator**:
   - Reach out via email/Slack before the workshop
   - Arrive 15 minutes early for help

4. **Backup plan**:
   - If all else fails, we can use GitHub Codespaces (cloud-based VS Code)
   - Requires only a browser and GitHub account

---

## ✅ Final Checklist

Before the workshop, confirm:

- [ ] GitHub Copilot subscription is active
- [ ] VS Code with all 3 extensions installed (Copilot, Copilot Chat, C# Dev Kit)
- [ ] .NET 9 SDK installed (`dotnet --version` shows 9.x.x)
- [ ] Git installed and configured
- [ ] Workshop repository cloned and on `starter-projects` branch
- [ ] Solution builds successfully (`dotnet build`)
- [ ] Tests run and show 11 expected failures (`dotnet test`)
- [ ] Copilot inline suggestions work
- [ ] Copilot Chat responds to queries
- [ ] Workspace context works with `@workspace`

---

## 🎯 You're Ready!

If all items above are checked, you're fully prepared for the workshop! 🎉

See you at the workshop! Bring:
- ✅ Your laptop with the environment set up
- ✅ Power adapter (3-hour workshop)
- ✅ Curiosity and willingness to experiment
- ✅ Questions about AI-assisted development

**Note**: If you couldn't complete all checklist items, still attend! Facilitators will help during the setup period, and we have backup options available.
