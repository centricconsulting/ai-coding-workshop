# Plan: Swift Track (Mobile — iOS)

## Summary

Self-contained **Swift Package (SPM)** module (no iOS/UIKit/SwiftUI framework dependency, no
simulator required) covering Domain + Application layers, for use by mobile (iOS) developers in
the workshop. A runnable iOS app shell (installable on a simulator) is a **deferred** follow-on —
Labs 1–4 ship as a logic-only package with tests first.

## Stack

- Swift 5.9+ (or latest stable)
- **Testing**: XCTest (built-in), or Swift Testing (Apple's newer framework) if the team prefers
  its more modern macro-based syntax — recommend XCTest for workshop familiarity unless Aaron
  prefers otherwise
- Build tool: Swift Package Manager

## Proposed Structure

```
src-swift/
  TaskManagerDomain/        # Task, TaskId, TaskStatus, TaskRepository protocol
  TaskManagerApplication/   # use cases orchestrating domain rules
  TaskManagerInfrastructure/# in-memory repository implementation
  Tests/
    TaskManagerDomainTests/
    TaskManagerApplicationTests/
  # (future) TaskManagerApp/ — thin iOS UI shell, deferred
```

## Labs to Port

- `lab-01-tdd-with-copilot.md` — Swift/SPM test commands (`swift test`).
- `lab-02-requirements-to-code-swift.md` — new variant.
- `lab-03-generation-and-refactoring-swift.md` — new variant. **Refactor exercise note**: avoid a
  literal translation of the .NET/Java smell. A natural Swift smell: force-unwrapping optionals
  (`!`) and untyped error handling, refactored to `Result<T, Error>` / `throws` and safe optional
  binding — a Swift-idiomatic refactor rather than reusing the OO refactor from other tracks.
- `lab-04-testing-documentation-workflow-swift.md` — new variant, using DocC-style comments and
  XCTest naming conventions.

## Devcontainer / Setup

- **Important constraint**: Xcode and the iOS SDK/simulator require macOS and cannot run in a
  Linux devcontainer. Recommend:
  - No devcontainer for this track (or a Linux devcontainer covering only the SPM
    package/command-line `swift build`/`swift test`, without simulator support), plus
  - `docs/MINIMAL_SETUP_SWIFT.md` — local setup guide requiring Xcode Command Line Tools on macOS.
- If any participants are on Windows/Linux with no Mac available, they'd only be able to run
  `swift test` if a Linux Swift toolchain is installed — full parity with macOS users is not
  guaranteed. Flagging this as a workshop logistics constraint, not just a technical one.

## Estimate

**1–2 days**, AI-assisted port of Domain/Application layers plus lab doc translation.

## Open Questions

- Confirm whether all Swift-track participants will have access to a Mac (required for realistic
  Swift/iOS development), since this affects whether the devcontainer-only fallback is acceptable.
- Confirm the deferred iOS UI shell should be tracked as a distinct future task once Labs 1–4 are
  validated, rather than bundled into this initial estimate.
