# DevContainer Test Plan

**Purpose**: Validate .NET and Spring Boot participant devcontainers work correctly  
**Date**: March 31, 2026  
**Epic**: #16 - Spring Boot Adaptation  

---

## Prerequisites

- Docker Desktop running
- VS Code with Dev Containers extension
- Repository cloned: `git clone https://github.com/centricconsulting/ai-coding-workshop.git`

---

## Test 1: .NET Participant DevContainer

### 1.1 Open DevContainer

```bash
# From VS Code Command Palette (Cmd/Ctrl+Shift+P)
> Dev Containers: Open Folder in Container...
# Select: .devcontainer/dotnet-participant/devcontainer.json
```

**Expected**: Container builds and starts within 2-3 minutes

### 1.2 Verify Post-Create Command

Check terminal output shows:
- ✅ `dotnet build src-dotnet/TaskManager.sln` completed successfully
- ✅ `dotnet dev-certs https --trust` executed
- ✅ No build errors

### 1.3 Verify .NET Environment

```bash
# Test .NET SDK
dotnet --version
# Expected: 9.0.x

# Test .NET CLI works
dotnet --list-sdks
# Expected: Shows .NET 9.0 SDK

# Check solution builds
cd /workspaces/ai-code-workshop
dotnet build src-dotnet/TaskManager.sln
# Expected: Build succeeded, 0 errors

# Run .NET tests
dotnet test src-dotnet/TaskManager.sln
# Expected: All tests pass
```

### 1.4 Verify Extensions Installed

Check VS Code extensions panel for:
- ✅ GitHub Copilot
- ✅ GitHub Copilot Chat
- ✅ C# DevKit
- ✅ REST Client
- ✅ Markdown Mermaid
- ✅ Marp for VS Code
- ✅ Markdown All in One

### 1.5 Verify GitHub CLI

```bash
gh --version
# Expected: gh version 2.x or higher

gh auth status
# Expected: Shows authentication status (may need login)
```

### 1.6 Verify Port Forwarding

```bash
# Start .NET API
cd src-dotnet/src/TaskManager.Api
dotnet run
```

**Expected**:
- Port 5000 (HTTP) auto-forwarded
- Port 5001 (HTTPS) auto-forwarded
- Can access http://localhost:5000/api/tasks (or via forwarded port)
- Press Ctrl+C to stop

### 1.7 Verify Copilot Instructions

```bash
# Check .NET instructions exist
ls -la .github/instructions/
cat .github/instructions/csharp.instructions.md
cat .github/instructions/dotnet.instructions.md
```

**Expected**: Files exist and contain .NET-specific guidance

---

## Test 2: Spring Boot Participant DevContainer

### 2.1 Open DevContainer

```bash
# From VS Code Command Palette (Cmd/Ctrl+Shift+P)
> Dev Containers: Reopen Folder in Container...
# Select: .devcontainer/springboot-participant/devcontainer.json
```

**Expected**: Container builds and starts within 2-3 minutes

### 2.2 Verify Post-Create Command

Check terminal output shows:
- ✅ `mvn clean install -f src-springboot/pom.xml -DskipTests` completed successfully
- ✅ No build errors
- ✅ All modules compiled

### 2.3 Verify Java Environment

```bash
# Test Java version
java -version
# Expected: openjdk version "21.0.x" (Java 21 LTS)

# Test Maven
mvn --version
# Expected: Apache Maven 3.9.x, Java version: 21.0.x

# Test Gradle
gradle --version
# Expected: Gradle 8.5.x

# Check JAVA_HOME
echo $JAVA_HOME
# Expected: /usr/local/sdkman/candidates/java/current
```

### 2.4 Verify Spring Boot Build

```bash
# Navigate to Spring Boot project
cd /workspaces/ai-code-workshop/src-springboot

# Build all modules
mvn clean install
# Expected: BUILD SUCCESS, all 102 tests pass

# Run tests only
mvn test
# Expected: 
# - taskmanager-domain: 36 tests pass
# - taskmanager-application: 33 tests pass
# - taskmanager-infrastructure: 25 tests pass
# - taskmanager-api: 8 tests pass
# - Total: 102 tests, 0 failures

# Check code coverage
mvn test jacoco:report
# Expected: 72.55% overall coverage
```

### 2.5 Verify Extensions Installed

Check VS Code extensions panel for:
- ✅ GitHub Copilot
- ✅ GitHub Copilot Chat
- ✅ Extension Pack for Java
- ✅ Spring Boot Dashboard
- ✅ Spring Boot Tools
- ✅ Maven for Java
- ✅ Gradle for Java
- ✅ REST Client
- ✅ Markdown Mermaid
- ✅ Marp for VS Code

### 2.6 Verify GitHub CLI

```bash
gh --version
# Expected: gh version 2.x or higher
```

### 2.7 Verify Port Forwarding

```bash
# Start Spring Boot application with dev profile (uses H2 in-memory database)
cd /workspaces/ai-code-workshop/src-springboot
mvn spring-boot:run -pl taskmanager-api -Dspring-boot.run.profiles=dev
```

**Expected**:
- Port 8080 auto-forwarded
- Application starts successfully with H2 database
- Can access http://localhost:8080/actuator/health
- Can access http://localhost:8080/swagger-ui.html
- Can access http://localhost:8080/h2-console (H2 database console)
- Press Ctrl+C to stop

### 2.8 Verify Copilot Instructions

```bash
# Check Spring Boot instructions exist
ls -la .github/instructions/
cat .github/instructions/springboot.instructions.md
```

**Expected**: File exists and contains Spring Boot-specific guidance

### 2.9 Verify Spring Boot Samples

```bash
# Check source code exists
ls -la src-springboot/
find src-springboot -name "*.java" | wc -l
# Expected: 22 Java files

# Check README
cat src-springboot/README.md
# Expected: Comprehensive documentation with quick start guide

# Check coverage report
cat src-springboot/coverage-summary.txt
# Expected: Shows 72.55% overall coverage
```

---

## Test 3: Maintainer DevContainer (Optional)

### 3.1 Open DevContainer

```bash
# From VS Code Command Palette
> Dev Containers: Reopen Folder in Container...
# Select: .devcontainer/maintainer/devcontainer.json
```

**Expected**: Container builds with BOTH .NET and Java support

### 3.2 Verify Post-Create Command

Check terminal output shows:
- ✅ `dotnet build src-dotnet/TaskManager.sln` completed
- ✅ `mvn clean install -f src-springboot/pom.xml -DskipTests` completed
- ✅ `dotnet dev-certs https --trust` executed
- ✅ `npm install -g @marp-team/marp-cli` completed

### 3.3 Verify Both Stacks

```bash
# Test .NET
dotnet --version
# Expected: 9.0.x

dotnet test src-dotnet/TaskManager.sln
# Expected: All .NET tests pass

# Test Java/Spring Boot
java -version
# Expected: openjdk 21.0.x

cd src-springboot && mvn test
# Expected: All 102 Spring Boot tests pass
```

### 3.4 Verify Node.js (for Marp)

```bash
node --version
# Expected: v20.x

npm --version
# Expected: 10.x or higher

marp --version
# Expected: @marp-team/marp-cli v3.x
```

### 3.5 Verify Extensions

Check VS Code has BOTH:
- .NET extensions (C# DevKit, etc.)
- Java extensions (Extension Pack for Java, Spring Boot, etc.)

---

## Success Criteria

For **Issue #28** to be considered complete, all three devcontainers must:

- ✅ Build successfully without errors
- ✅ Execute postCreateCommand correctly
- ✅ Install all required VS Code extensions
- ✅ Provide correct runtime environment (Java 21, .NET 9, Node 20)
- ✅ Build and test respective projects successfully
- ✅ Forward required ports automatically
- ✅ Provide GitHub CLI access
- ✅ Load repository-specific Copilot instructions

---

## Common Issues & Troubleshooting

### Issue: Container Won't Build

**Solution**: 
```bash
# Rebuild container without cache
> Dev Containers: Rebuild Container Without Cache
```

### Issue: postCreateCommand Fails

**Solution**:
```bash
# Run manually after container starts
# For .NET:
dotnet build src-dotnet/TaskManager.sln

# For Spring Boot:
cd src-springboot && mvn clean install -DskipTests
```

### Issue: Ports Not Forwarding

**Solution**:
- Check VS Code Ports panel (View → Ports)
- Manually add port forwarding if needed
- Ensure application is running

### Issue: Extensions Not Installing

**Solution**:
```bash
# Reload window
> Developer: Reload Window

# Or reinstall extensions manually from Extensions panel
```

### Issue: GitHub CLI Not Authenticated

**Solution**:
```bash
gh auth login
# Follow prompts to authenticate
```

---

## Quick Smoke Test (5 minutes per container)

### .NET Container Quick Test
```bash
# Build, test, run
cd /workspaces/ai-code-workshop
dotnet build src-dotnet/TaskManager.sln && \
dotnet test src-dotnet/TaskManager.sln && \
echo "✅ .NET container working"
```

### Spring Boot Container Quick Test
```bash
# Build, test
cd /workspaces/ai-code-workshop/src-springboot
mvn clean test && \
echo "✅ Spring Boot container working"
```

---

## Test Results Template

Copy and fill out after testing:

```markdown
## DevContainer Test Results - [Date]

### .NET Participant Container
- [ ] Container builds successfully
- [ ] postCreateCommand completes
- [ ] .NET 9.0 SDK available
- [ ] Solution builds without errors
- [ ] All tests pass
- [ ] Extensions installed
- [ ] GitHub CLI works
- [ ] Ports forward correctly
- [ ] Copilot instructions load

**Status**: ✅ PASS / ❌ FAIL  
**Notes**: 

---

### Spring Boot Participant Container
- [ ] Container builds successfully
- [ ] postCreateCommand completes
- [ ] Java 21 available
- [ ] Maven 3.9+ available
- [ ] Spring Boot builds (102 tests pass)
- [ ] 72%+ coverage achieved
- [ ] Extensions installed
- [ ] GitHub CLI works
- [ ] Ports forward correctly
- [ ] Copilot instructions load
- [ ] Swagger UI accessible

**Status**: ✅ PASS / ❌ FAIL  
**Notes**:

---

### Maintainer Container (Optional)
- [ ] Container builds successfully
- [ ] Both .NET and Spring Boot build
- [ ] Node.js and Marp CLI available
- [ ] All extensions installed

**Status**: ✅ PASS / ❌ FAIL  
**Notes**:

---

**Overall Assessment**: ✅ All containers working / ⚠️ Minor issues / ❌ Major issues

**Tested By**: [Your Name]  
**Date**: [Date]
```

---

## Issue #28 Status Verification

Based on testing, update GitHub Issue #28 with results:

```markdown
**DevContainer Test Results - March 31, 2026**

Tested all three devcontainers:

✅ **dotnet-participant**: Builds .NET 9 solution, all tests pass
✅ **springboot-participant**: Builds Spring Boot project, 102 tests pass, 72% coverage
✅ **maintainer**: Builds both stacks, Marp CLI installed

All acceptance criteria met. Issue #28 confirmed complete.
```

---

## Next Steps After Testing

1. If all tests pass → Issue #28 is verified complete ✅
2. If issues found → Document and fix before proceeding
3. Move to next Epic #16 issue (recommend #19 - Pattern Translation Guide)
