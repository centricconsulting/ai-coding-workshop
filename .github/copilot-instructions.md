# Repository-Wide GitHub Copilot Instructions

This repository is a bilingual GitHub Copilot workshop with shared docs, labs, and presentations plus two implementation tracks: .NET 9 under `src/` and Spring Boot 3.x under `src-springboot/`.

This file defines repo-wide defaults. More specific instruction files take precedence.

- `**/*.cs`: also follow `.github/instructions/csharp.instructions.md` and `.github/instructions/dotnet.instructions.md`
- `src-springboot/**`: also follow `.github/instructions/springboot.instructions.md

## 1. Repository Purpose

- Optimize for workshop clarity and educational value.
- Keep examples practical, explicit, and easy to explain.
- Demonstrate maintainable, test-driven use of GitHub Copilot.

## 2. Coding Style And Naming Conventions

- Follow the active language's standard conventions.
- Use clear business names from the feature's ubiquitous language.
- Prefer feature-oriented organization and focused files.
- Favor descriptive names over abbreviations.
- Use standard casing for the language: `PascalCase` for types/public members, `camelCase` for locals and parameters, and uppercase constants where customary.
- Keep methods small and explicit. Prefer guard clauses over deep nesting.
- Make classes sealed or final by default unless inheritance is intentional.

## 3. Technology Stack And Preferred Libraries

- .NET track: .NET 9, ASP.NET Core Minimal API, xUnit v3, FakeItEasy, `ILogger`, OpenTelemetry, and Testcontainers.
- Spring Boot track: Java 21, Spring Boot 3.x, Spring Web, Spring Data JPA when needed, JUnit 5, Mockito, SLF4J, and Testcontainers.
- Docs and workshop content: Markdown in `docs/`, Marp-compatible slides, and VS Code Dev Container workflows.
- Prefer existing repo libraries before introducing new ones.
- Avoid mediator frameworks, heavy abstractions, alternate test frameworks, or extra dependencies unless explicitly requested.

## 4. Architecture Patterns To Follow And Avoid

- Follow Clean Architecture boundaries and use DDD where business behavior is modeled.
- Keep business rules in Domain, orchestration in Application, and transport concerns in Api.
- Keep domain logic out of controllers, endpoints, and infrastructure adapters.
- Use ports, adapters, dependency injection, and explicit boundary interfaces.
- Avoid circular dependencies, generic CRUD-style domain APIs, persistence leakage into the domain, and exposing domain entities directly as API contracts.
- Prefer feature-first structure over broad shared utility layers.

## 5. Testing, Security, And Error Handling

- Use TDD by default for new features and meaningful changes.
- Keep unit tests on Domain and Application behavior, and integration tests on Infrastructure and Api wiring.
- Validate external input at the boundary and fail fast with specific errors.
- Catch broad exceptions only when translating them into a clear boundary-level outcome or telemetry.
- Use consistent API error handling: `ProblemDetails` in .NET and centralized exception handling such as `@ControllerAdvice` in Spring Boot.
- Never hardcode or log secrets, tokens, credentials, or sensitive data.
- Use configuration and environment-based settings for infrastructure and secrets.
- Keep examples safe by default and avoid insecure shortcuts.

## 6. Documentation Standards

- Keep the root `README.md` focused on overview, setup, and navigation.
- Put detailed docs in `docs/` using the existing folders.
- Update docs when behavior, architecture, workflows, or contributor expectations change.
- Use descriptive file names, working relative links, and concise comments for public APIs or non-obvious behavior when useful.
- Explain the why, not just the how, in architecture and workshop guidance.
- Keep labs realistic, with prerequisites, expected outcomes, and runnable commands.
- Keep documentation aligned with the repository's actual code and workflows.

## 7. Working Style For Copilot Contributions

- Prefer minimal, focused changes that fit the existing structure.
- Fix root causes instead of layering on workarounds.
- Choose the clearer approach when a clever one hurts workshop readability.
- Let stack-specific instruction files refine implementation details when they are more specific.
