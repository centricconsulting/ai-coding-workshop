# Workshop Terminology

A plain-language glossary of terms used throughout this workshop's labs and presentations. It's written for non-technical attendees — if you're new to software development or GitHub Copilot, start here whenever you hit an unfamiliar word.

Terms are grouped by category and listed alphabetically within each group.

## Development Practices & Architecture

- **Aggregate** — A cluster of related data and rules that are treated as a single unit and always kept consistent together. For example, an "Order" and its "Order Items" form one aggregate: you never change an item's quantity without also updating the order's total, so the two are saved and validated together as a group. Aggregates have one designated entry point (called the "aggregate root") that all changes must go through, which prevents the data from ending up in a contradictory or invalid state. This matters in a workshop context because it's a core building block of Domain-Driven Design, and you'll see it reflected directly in how the TaskManager reference app's Domain layer is structured.

- **Bounded Context** — A clear boundary around a specific business area where terms and rules have one consistent meaning. The same word can mean different things in different parts of a business — "Customer" in a Sales system might track leads and quotes, while "Customer" in a Support system tracks tickets and satisfaction scores. Rather than forcing one shared definition everywhere, Domain-Driven Design encourages drawing an explicit boundary around each area so each team's model stays simple and accurate for its own purpose. Translation happens deliberately at the edges where contexts need to talk to each other, instead of quietly causing confusion inside the code.

- **CI/CD (Continuous Integration / Continuous Deployment)** — Automated processes that build, test, and release code changes frequently and reliably, instead of doing it all manually. Continuous Integration means every change is automatically compiled and tested as soon as it's pushed, so problems are caught within minutes rather than discovered weeks later. Continuous Deployment (or delivery) takes that further by automatically packaging and releasing changes that pass all checks, often straight to production. Together they let teams ship small, frequent, low-risk changes instead of large, infrequent, high-risk ones — and they're part of why automated tests (unit and integration tests) matter so much in modern development.

- **Clean Architecture** — A way of organizing code so that core business logic doesn't depend on technical details like databases, web frameworks, or external services, making the system easier to change and test. The idea is to arrange code in layers — typically Domain (business rules), Application (orchestration of those rules), and Infrastructure/API (technical plumbing) — where inner layers never know anything about outer layers. That means you can swap your database, your web framework, or even your UI without touching the business logic at all, and you can test that business logic without spinning up a real database. In this workshop's reference app, you'll see this layering applied consistently across every language track.

- **DDD (Domain-Driven Design)** — An approach to building software that models the system around the real business concepts and language that experts in that field actually use, rather than around technical or database-centric thinking. The goal is to close the gap between "how the business describes the problem" and "how the code describes the problem," so a developer and a business expert can look at the same model and recognize it. Supporting ideas like Entities, Value Objects, Aggregates, and Bounded Contexts all exist to help capture business rules and language directly and explicitly in code, instead of letting them get scattered across the system or buried in infrastructure concerns.

- **Dependency Injection** — A technique where a piece of code is given the tools it needs from the outside, rather than creating them itself, making it easier to test and swap out parts. For example, instead of a class creating its own database connection internally, that connection is "injected" into it when it's created — often automatically, by a framework. This matters for testing because it lets you substitute a real dependency (like a database) with a fake or mock one in a test, without changing the code being tested at all. It's also what makes Clean Architecture's strict layering practical: outer layers can supply implementations to inner layers without the inner layers needing to know the concrete details.

- **Entity** — A business object that has its own identity and can change over time while still being "the same thing." For example, a specific Task is an entity: its title, status, or due date might change throughout its life, but it's still recognized as that same task because of its unique identity (usually an ID), not because its current data happens to match. This is the key distinction from a Value Object, which has no identity of its own and is instead defined purely by its values — if any value changes, it's considered a different value object entirely, not an updated version of the same one.

- **Integration Test** — A test that checks whether multiple parts of a system work correctly together, such as the application code and a real (or realistic) database, message queue, or external API. Unlike a unit test, which isolates one small piece of logic, an integration test verifies that the "seams" between components — wiring, configuration, data persistence, network calls — actually work as expected when combined. These tests are typically slower and more complex to set up than unit tests (often using tools like Testcontainers to spin up real dependencies), so most teams write many unit tests and a smaller, more targeted set of integration tests.

- **Red-Green-Refactor** — The step-by-step cycle at the heart of Test-Driven Development: write a small failing test first (red), write just enough code to make that test pass (green), and then clean up and improve the code without changing its behavior (refactor), relying on the test to confirm nothing broke. Repeating this short cycle over and over — rather than writing a large batch of code and testing it all at once — keeps each step small, verifiable, and low-risk. It also means the test suite grows alongside the code, so by the time a feature is "done," it already has test coverage protecting it from future regressions.

- **Refactoring** — Improving the internal structure, clarity, or organization of code without changing what it does from the outside. This might mean renaming a confusing variable, breaking a long method into smaller ones, or removing duplicated logic — all while the software's observable behavior and existing tests continue to pass exactly as before. Refactoring is the deliberate "cleanup" step that keeps a codebase healthy and easy to work with over time, and it's only safe to do confidently when there's a solid set of automated tests in place to catch any accidental behavior changes.

- **TDD (Test-Driven Development)** — A practice where you write a test for a small piece of behavior before writing the code that implements it, rather than writing all the code first and testing it afterward (or not at all). By starting with a failing test that describes exactly what the code should do, developers are forced to think clearly about requirements and edge cases up front, and they get immediate, automatic confirmation once that behavior is correctly implemented. Over time, this produces both working code and a comprehensive, reliable test suite as a natural byproduct of how the code was written, which is why it's emphasized throughout this workshop's labs.

- **Unit Test** — A small, fast, automated test that checks a single piece of logic in isolation, without relying on external systems like databases, file systems, or network calls. Because they're small and self-contained, unit tests run in milliseconds and can be executed constantly during development, giving near-instant feedback when something breaks. A healthy codebase typically has a large number of unit tests covering individual business rules and functions, complemented by a smaller number of broader integration tests that check how those pieces work together.

- **Value Object** — A piece of data that is defined entirely by its values, with no unique identity of its own — unlike an Entity, which retains the same identity even as its data changes. For example, a date range or a monetary amount is typically a value object: two "$50 USD" amounts are considered completely interchangeable and equal, regardless of where they came from, because there's nothing distinguishing one from another beyond the value itself. Value objects are often made immutable (impossible to change after creation) specifically because this removes a whole category of bugs where one part of the code accidentally changes a value that another part of the code still depends on.

## AI / GitHub Copilot

- **Agent Mode** — A GitHub Copilot mode where it can autonomously plan and carry out multi-step tasks, such as editing several files or running commands, rather than just suggesting one line at a time.
- **Context Window** — The amount of text (instructions, code, conversation history) an AI model can "see" and consider at once when generating a response.
- **Custom Agent** — A specialized, pre-configured version of Copilot set up for a particular role or task, defined in this repo under `.github/agents/`.
- **GitHub Copilot / Copilot Chat** — GitHub's AI pair-programming tool that suggests code and answers questions, either inline in the editor or through a chat conversation.
- **Hallucination** — When an AI confidently produces information or code that is incorrect or made up, rather than accurate.
- **Harness** — The surrounding application that runs an AI model and gives it the ability to act, such as reading files, running commands, or calling tools — GitHub Copilot CLI and Copilot Chat in VS Code are both examples of harnesses. The underlying AI model itself only generates text; it's the harness that decides what tools the model is allowed to use, feeds it relevant context, and carries out the actions it requests. This distinction matters because the same model can behave very differently depending on which harness it's running in, since each harness controls what capabilities, safety checks, and context the model has access to.
- **MCP (Model Context Protocol)** — An open standard that lets AI tools like Copilot connect to external systems and data sources (such as GitHub or Azure) in a consistent way.
- **Prompt** — The instructions or question you give to an AI assistant to get the response or action you want.
- **Skill** — A packaged set of instructions or capabilities that extends what Copilot can do for a specific kind of task.
- **Slash Command** — A shortcut typed with a leading `/` (like `/tests` or `/agent`) that tells Copilot to perform a specific predefined action.

## Tools & Environment

- **CLI (Command-Line Interface)** — A way of interacting with software by typing text commands instead of clicking buttons, such as the GitHub Copilot CLI used in this workshop.
- **Dev Container** — A pre-configured, ready-to-use development environment (including tools and dependencies) that runs the same way for everyone, usually inside a container.
- **Git** — A version control system that tracks changes to files over time, letting multiple people collaborate on the same codebase safely.
- **GitHub** — A web-based platform for hosting Git repositories and collaborating on code, including code review, issue tracking, and automation.
- **IDE (Integrated Development Environment)** — An application that combines a code editor, tools, and helpers in one place to make writing software easier (for example, VS Code).
- **Terminal** — A text-based window used to type and run commands directly to the computer.
- **VS Code** — Visual Studio Code, the free code editor used throughout this workshop, with built-in support for GitHub Copilot.

## Software/Web Concepts

- **API (Application Programming Interface)** — A defined way for different pieces of software to talk to each other, like a menu of requests one program can make to another.
- **Branch** — A separate, parallel line of work in a Git repository, used to make changes without affecting the main codebase until they're ready.
- **Clone** — Making a local copy of a Git repository on your own computer.
- **Commit** — A saved snapshot of changes in a Git repository, along with a message describing what changed.
- **Endpoint** — A specific address or URL that an API exposes, which other software can call to request or send data.
- **Fork** — Creating your own independent copy of someone else's GitHub repository, so you can make changes without affecting the original.
- **Framework vs. Library** — A library is a set of reusable code you call when you need it; a framework provides the overall structure and calls your code for you.
- **Minimal API** — A simplified style (used in .NET) for building APIs with less boilerplate code than traditional setups.
- **Pull Request (PR)** — A request to merge one set of code changes into another branch or codebase, typically reviewed by others before being accepted.
- **Repository (Repo)** — A storage location for a project's code and its full history of changes.
