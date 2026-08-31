# .NET to Spring Boot Pattern Translation Guide

**Version**: 1.0  
**Date**: March 31, 2026  
**Purpose**: Help workshop participants translate .NET patterns to Spring Boot equivalents

---

## Table of Contents

1. [Introduction](#introduction)
2. [Quick Reference](#quick-reference)
3. [Clean Architecture](#clean-architecture)
4. [Domain Layer Patterns](#domain-layer-patterns)
5. [Application Layer Patterns](#application-layer-patterns)
6. [Infrastructure Layer Patterns](#infrastructure-layer-patterns)
7. [API Layer Patterns](#api-layer-patterns)
8. [Testing Patterns](#testing-patterns)
9. [Configuration Management](#configuration-management)
10. [Dependency Injection](#dependency-injection)
11. [Logging](#logging)
12. [Common Patterns Comparison](#common-patterns-comparison)

---

## Introduction

This guide provides side-by-side comparisons of patterns between .NET and Spring Boot implementations of the TaskManager application. Both implementations follow Clean Architecture and Domain-Driven Design principles, making them structurally similar with different syntax and frameworks.

**Key Principle**: The architectural patterns remain the same; only the implementation details change.

---

## Quick Reference

| Pattern/Concept | .NET | Spring Boot |
|----------------|------|-------------|
| **Language** | C# 12+ | Java 21+ |
| **Build Tool** | dotnet CLI / MSBuild | Maven 3.9+ or Gradle 8.5 |
| **Package Management** | NuGet | Maven Central |
| **Web Framework** | ASP.NET Core Minimal APIs | Spring MVC / Spring Boot |
| **DI Container** | Microsoft.Extensions.DependencyInjection | Spring IoC Container |
| **ORM** | Entity Framework Core | Hibernate / Spring Data JPA |
| **Configuration** | appsettings.json | application.yml |
| **Logging** | ILogger (Microsoft.Extensions.Logging) | SLF4J + Logback |
| **Test Framework** | xUnit | JUnit 5 (Jupiter) |
| **Mocking** | FakeItEasy / Moq | Mockito |
| **Validation** | Data Annotations | Jakarta Validation (Bean Validation) |
| **HTTP Client** | HttpClient | RestTemplate / WebClient |
| **API Docs** | Swagger/OpenAPI (Swashbuckle) | SpringDoc OpenAPI |
| **Records** | `public record` | `public record` (Java 16+) |

---

## Clean Architecture

Both implementations follow the same Clean Architecture layers with identical dependency rules:

```
┌─────────────────────────────────────┐
│         API / Presentation          │  ← REST controllers, DTOs
├─────────────────────────────────────┤
│        Infrastructure               │  ← JPA/EF, repositories
├─────────────────────────────────────┤
│          Application                │  ← Use cases, services
├─────────────────────────────────────┤
│            Domain                   │  ← Business logic, entities
└─────────────────────────────────────┘
     ↑ Dependencies flow inward ↑
```

### Project Structure

**.NET**:
```
src-dotnet/
├── TaskManager.sln
├── src/
│   ├── TaskManager.Domain/        # Pure C#, no dependencies
│   ├── TaskManager.Application/   # Depends on Domain
│   ├── TaskManager.Infrastructure/ # Depends on Application + Domain
│   └── TaskManager.Api/           # Depends on all layers
└── tests/
    ├── TaskManager.UnitTests/
    └── TaskManager.IntegrationTests/
```

**Spring Boot**:
```
pom.xml (parent)
├── taskmanager-domain/            # Pure Java, no dependencies
├── taskmanager-application/       # Depends on Domain
├── taskmanager-infrastructure/    # Depends on Application + Domain
└── taskmanager-api/              # Depends on all layers
    └── src/test/java/            # Tests integrated per module
```

**Key Difference**: .NET uses separate test projects; Spring Boot typically co-locates tests with source code in `src/test/java`.

---

## Domain Layer Patterns

The domain layer is framework-agnostic in both stacks - pure business logic with no external dependencies.

### Aggregate Root

**C# (.NET)**:
```csharp
namespace TaskManager.Domain.Tasks;

/// <summary>
/// Task aggregate root
/// </summary>
public sealed class Task
{
    // Private constructor - forces use of factory method
    private Task(TaskId id, string title, string description, 
                 TaskStatus status, DateTime createdAt)
    {
        Id = id;
        Title = title;
        Description = description;
        Status = status;
        CreatedAt = createdAt;
    }

    public TaskId Id { get; }
    public string Title { get; private set; }
    public string Description { get; private set; }
    public TaskStatus Status { get; private set; }
    public DateTime CreatedAt { get; }
    
    // Factory method
    public static Task Create(string title, string description)
    {
        return new Task(
            TaskId.New(),
            title,
            description,
            TaskStatus.Todo,
            DateTime.UtcNow);
    }
    
    // Behavior method
    public void UpdateStatus(TaskStatus newStatus)
    {
        Status = newStatus;
        UpdatedAt = DateTime.UtcNow;
    }
}
```

**Java (Spring Boot)**:
```java
package com.example.taskmanager.domain.tasks;

import java.time.LocalDateTime;

/**
 * Task aggregate root
 */
public final class Task {
    
    // Private constructor - forces use of factory method
    private Task(TaskId id, String title, String description,
                 TaskStatus status, LocalDateTime createdAt) {
        this.id = id;
        this.title = title;
        this.description = description;
        this.status = status;
        this.createdAt = createdAt;
    }
    
    private final TaskId id;
    private String title;
    private String description;
    private TaskStatus status;
    private final LocalDateTime createdAt;
    private LocalDateTime updatedAt;
    
    // Getters
    public TaskId getId() { return id; }
    public String getTitle() { return title; }
    public String getDescription() { return description; }
    public TaskStatus getStatus() { return status; }
    public LocalDateTime getCreatedAt() { return createdAt; }
    
    // Factory method
    public static Task create(String title, String description) {
        return new Task(
            TaskId.generate(),
            title,
            description,
            TaskStatus.PENDING,
            LocalDateTime.now());
    }
    
    // Behavior method
    public void updateStatus(TaskStatus newStatus) {
        this.status = newStatus;
        this.updatedAt = LocalDateTime.now();
    }
}
```

**Key Patterns**:
- ✅ Private constructor in both
- ✅ Factory methods for creation
- ✅ Behavior methods (not just setters)
- ✅ `final`/`sealed` classes

**Differences**:
- C# properties vs Java getter methods
- C# `DateTime.UtcNow` vs Java `LocalDateTime.now()`
- C# `sealed` vs Java `final` for classes
- C# auto-properties vs Java explicit fields

### Value Objects

**C# Record**:
```csharp
namespace TaskManager.Domain.Tasks;

/// <summary>
/// Strongly-typed identifier for Task entities
/// </summary>
public sealed record TaskId(Guid Value)
{
    public static TaskId New() => new(Guid.NewGuid());
    public static TaskId From(Guid value) => new(value);
    
    public override string ToString() => Value.ToString();
}
```

**Java Record (Java 16+)**:
```java
package com.example.taskmanager.domain.tasks;

import java.util.UUID;

/**
 * Strongly-typed identifier for Task entities
 */
public record TaskId(UUID value) {
    
    public static TaskId generate() {
        return new TaskId(UUID.randomUUID());
    }
    
    public static TaskId of(UUID value) {
        return new TaskId(value);
    }
    
    @Override
    public String toString() {
        return value.toString();
    }
}
```

**Key Patterns**:
- ✅ Both use records (immutable value types)
- ✅ Factory methods for creation
- ✅ Structural equality (automatic in records)

**Differences**:
- C# `Guid` vs Java `UUID`
- C# capitalizes property names; Java uses lowercase method names

### Enums

**C#**:
```csharp
namespace TaskManager.Domain.Tasks;

public enum TaskStatus
{
    Todo,
    InProgress,
    Done,
    Cancelled
}
```

**Java**:
```java
package com.example.taskmanager.domain.tasks;

public enum TaskStatus {
    PENDING,
    IN_PROGRESS,
    COMPLETED,
    CANCELLED
}
```

**Key Differences**:
- C# uses PascalCase; Java uses SCREAMING_SNAKE_CASE for enum values
- Otherwise functionally identical

### Repository Interface (Port)

**C# Interface**:
```csharp
namespace TaskManager.Domain.Repositories;

/// <summary>
/// Repository interface for Task aggregate
/// Uses business-intent method names (not generic CRUD)
/// </summary>
public interface ITaskRepository
{
    Task<Task?> FindByIdAsync(TaskId taskId, 
        CancellationToken cancellationToken = default);
    
    Task<IEnumerable<Task>> GetActiveTasksAsync(
        CancellationToken cancellationToken = default);
    
    Task AddTaskAsync(Task task, 
        CancellationToken cancellationToken = default);
    
    Task SaveChangesAsync(Task task, 
        CancellationToken cancellationToken = default);
}
```

**Java Interface**:
```java
package com.example.taskmanager.domain.tasks;

import java.util.List;
import java.util.Optional;

/**
 * Repository interface for Task aggregate
 * Uses business-intent method names (not generic CRUD)
 */
public interface TaskRepository {
    
    Optional<Task> findById(TaskId id);
    
    List<Task> findActiveUser();
    
    Task save(Task task);
    
    void delete(TaskId id);
    
    List<Task> findByStatus(TaskStatus status);
}
```

**Key Patterns**:
- ✅ Business-intent method names
- ✅ Domain-level interface (port)
- ✅ Works with domain objects only

**Differences**:
- C# uses `async`/`await` extensively; Java traditionally synchronous (or uses `CompletableFuture`)
- C# can return `null`; Java prefers `Optional<T>`
- C# `IEnumerable<T>` vs Java `List<T>` or `Collection<T>`
- C# `Task<T>` (async type) vs Java void/return types

---

## Application Layer Patterns

### Application Service

**C# Service**:
```csharp
using Microsoft.Extensions.Logging;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;

namespace TaskManager.Application.Services;

public sealed class TaskService
{
    private readonly ITaskRepository _taskRepository;
    private readonly ILogger<TaskService> _logger;

    public TaskService(
        ITaskRepository taskRepository, 
        ILogger<TaskService> logger)
    {
        _taskRepository = taskRepository;
        _logger = logger;
    }

    public async Task<TaskId> AddTaskAsync(
        string title, 
        string description, 
        CancellationToken cancellationToken = default)
    {
        _logger.LogInformation(
            "Adding new task with title: {Title}", title);
        
        var task = Domain.Tasks.Task.Create(title, description);
        await _taskRepository.AddTaskAsync(task, cancellationToken);
        
        return task.Id;
    }
}
```

**Java Service**:
```java
package com.example.taskmanager.application.services;

import com.example.taskmanager.domain.tasks.*;
import lombok.RequiredArgsConstructor;
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.stereotype.Service;
import org.springframework.transaction.annotation.Transactional;

@Service
@Transactional(readOnly = true)
@RequiredArgsConstructor
public class TaskService {
    
    private static final Logger log = 
        LoggerFactory.getLogger(TaskService.class);
    
    private final TaskRepository taskRepository;
    
    @Transactional
    public TaskId addTask(String title, String description) {
        log.info("Adding new task with title: {}", title);
        
        Task task = Task.create(title, description);
        Task saved = taskRepository.save(task);
        
        return saved.getId();
    }
}
```

**Key Patterns**:
- ✅ Constructor injection (DI)
- ✅ Delegates to domain layer
- ✅ Orchestrates transactions
- ✅ Logging

**Differences**:
- C# suffix with `Async`; Java doesn't require this
- C# `async`/`await` vs Java synchronous (transactions managed by Spring)
- C# `ILogger<T>` vs Java `Logger` from SLF4J
- Java uses `@Service` annotation; C# registers manually in DI container
- Java `@Transactional` annotation vs C# manual transaction management
- Java `@RequiredArgsConstructor` (Lombok) generates constructor; C# writes it explicitly

---

## Infrastructure Layer Patterns

### JPA Entity vs EF Entity

**C# (Entity Framework - if used)**:
```csharp
namespace TaskManager.Infrastructure.Persistence;

public class TaskEntity
{
    public Guid Id { get; set; }
    public string Title { get; set; } = string.Empty;
    public string? Description { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public DateTime? CompletedAt { get; set; }
}
```

**Java (JPA Entity)**:
```java
package com.example.taskmanager.infrastructure.persistence.entities;

import jakarta.persistence.*;
import lombok.Data;
import java.time.LocalDateTime;
import java.util.UUID;

@Entity
@Table(name = "tasks")
@Data
public class TaskEntity {
    
    @Id
    @Column(name = "id", nullable = false, updatable = false)
    private UUID id;
    
    @Column(name = "title", nullable = false, length = 200)
    private String title;
    
    @Column(name = "description", columnDefinition = "TEXT")
    private String description;
    
    @Column(name = "status", nullable = false, length = 50)
    private String status;
    
    @Column(name = "created_at", nullable = false, updatable = false)
    private LocalDateTime createdAt;
    
    @Column(name = "completed_at")
    private LocalDateTime completedAt;
}
```

**Key Differences**:
- Java requires explicit `@Entity`, `@Table`, `@Column` annotations
- C# EF uses conventions (can override with Fluent API)
- Java Lombok `@Data` generates getters/setters; C# uses properties
- Both separate persistence model from domain model

### Repository Implementation (Adapter)

**C# In-Memory Repository**:
```csharp
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;

namespace TaskManager.Infrastructure.Repositories;

public sealed class InMemoryTaskRepository : ITaskRepository
{
    private readonly Dictionary<TaskId, Task> _tasks = new();

    public Task<Task?> FindByIdAsync(
        TaskId taskId, 
        CancellationToken cancellationToken = default)
    {
        _tasks.TryGetValue(taskId, out var task);
        return Task.FromResult(task);
    }

    public Task<IEnumerable<Task>> GetActiveTasksAsync(
        CancellationToken cancellationToken = default)
    {
        var activeTasks = _tasks.Values
            .Where(t => t.Status != TaskStatus.Done && 
                       t.Status != TaskStatus.Cancelled);
        return Task.FromResult(activeTasks);
    }

    public Task AddTaskAsync(
        Task task, 
        CancellationToken cancellationToken = default)
    {
        _tasks[task.Id] = task;
        return Task.CompletedTask;
    }
}
```

**Java JPA Repository Adapter**:
```java
package com.example.taskmanager.infrastructure.persistence.repositories;

import com.example.taskmanager.domain.tasks.*;
import com.example.taskmanager.infrastructure.persistence.entities.TaskEntity;
import com.example.taskmanager.infrastructure.persistence.mappers.TaskMapper;
import lombok.RequiredArgsConstructor;
import org.springframework.stereotype.Repository;

import java.util.List;
import java.util.Optional;
import java.util.stream.Collectors;

@Repository
@RequiredArgsConstructor
public class JpaTaskRepositoryAdapter implements TaskRepository {
    
    private final SpringDataTaskRepository jpaRepository;
    private final TaskMapper mapper;
    
    @Override
    public Task save(Task task) {
        TaskEntity entity = mapper.toEntity(task);
        TaskEntity saved = jpaRepository.save(entity);
        return mapper.toDomain(saved);
    }
    
    @Override
    public Optional<Task> findById(TaskId id) {
        return jpaRepository.findById(id.value())
            .map(mapper::toDomain);
    }
    
    @Override
    public List<Task> findByStatus(TaskStatus status) {
        return jpaRepository.findByStatus(status.name())
            .stream()
            .map(mapper::toDomain)
            .collect(Collectors.toList());
    }
}
```

**Spring Data JPA Interface**:
```java
package com.example.taskmanager.infrastructure.persistence.repositories;

import com.example.taskmanager.infrastructure.persistence.entities.TaskEntity;
import org.springframework.data.jpa.repository.JpaRepository;
import java.util.List;
import java.util.UUID;

public interface SpringDataTaskRepository 
    extends JpaRepository<TaskEntity, UUID> {
    
    List<TaskEntity> findByStatus(String status);
}
```

**Key Patterns**:
- ✅ Adapter pattern (implements domain interface)
- ✅ Mapper to translate domain ↔ entity
- ✅ Spring Data eliminates boilerplate (no C# equivalent without libraries)

**Differences**:
- Spring Data JPA auto-implements CRUD methods based on interface
- C# requires manual implementation (unless using a library like MediatR + EF extensions)
- Java uses `@Repository` annotation; C# registers in DI container

### Mapper (Anti-Corruption Layer)

**Java Mapper**:
```java
package com.example.taskmanager.infrastructure.persistence.mappers;

import com.example.taskmanager.domain.tasks.*;
import com.example.taskmanager.infrastructure.persistence.entities.TaskEntity;
import org.springframework.stereotype.Component;

@Component
public final class TaskMapper {
    
    public TaskEntity toEntity(Task task) {
        if (task == null) return null;
        
        TaskEntity entity = new TaskEntity();
        entity.setId(task.getId().value());
        entity.setTitle(task.getTitle());
        entity.setDescription(task.getDescription());
        entity.setStatus(task.getStatus().name());
        entity.setCreatedAt(task.getCreatedAt());
        entity.setCompletedAt(task.getCompletedAt());
        
        return entity;
    }
    
    public Task toDomain(TaskEntity entity) {
        if (entity == null) return null;
        
        TaskId taskId = TaskId.of(entity.getId());
        TaskStatus status = TaskStatus.valueOf(entity.getStatus());
        
        return Task.reconstitute(
            taskId,
            entity.getTitle(),
            entity.getDescription(),
            status,
            entity.getCreatedAt(),
            entity.getCompletedAt()
        );
    }
}
```

**C# Mapper (manual or AutoMapper)**:
```csharp
// Manual mapping example
namespace TaskManager.Infrastructure.Mappers;

public static class TaskMapper
{
    public static TaskEntity ToEntity(Task task)
    {
        return new TaskEntity
        {
            Id = task.Id.Value,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status.ToString(),
            CreatedAt = task.CreatedAt,
            CompletedAt = task.CompletedAt
        };
    }
    
    public static Task ToDomain(TaskEntity entity)
    {
        var taskId = TaskId.From(entity.Id);
        var status = Enum.Parse<TaskStatus>(entity.Status);
        
        // Use reconstitute method on aggregate
        return Task.Reconstitute(
            taskId,
            entity.Title,
            entity.Description,
            status,
            entity.CreatedAt,
            entity.CompletedAt);
    }
}
```

**Key Pattern**: Both implement anti-corruption layer to prevent ORM concerns from leaking into domain

---

## API Layer Patterns

### REST Controllers

**C# Minimal API**:
```csharp
// Program.cs
using TaskManager.Api.Extensions;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddApplicationServices();
builder.Services.AddOpenApiDocumentation();

var app = builder.Build();
app.MapTaskEndpoints(); // Extension method with endpoints
app.Run();

// EndpointExtensions.cs
public static class EndpointExtensions
{
    public static WebApplication MapTaskEndpoints(this WebApplication app)
    {
        var group = app.MapGroup("/api/tasks")
            .WithTags("Tasks")
            .WithOpenApi();

        group.MapPost("/", async (
            CreateTaskRequest request,
            TaskService service) =>
        {
            var taskId = await service.AddTaskAsync(
                request.Title, 
                request.Description);
            return Results.Created($"/api/tasks/{taskId}", taskId);
        })
        .Produces<TaskId>(StatusCodes.Status201Created)
        .WithName("CreateTask");

        group.MapGet("/{id:guid}", async (
            Guid id, 
            TaskService service) =>
        {
            var task = await service.GetTaskByIdAsync(TaskId.From(id));
            return task is not null 
                ? Results.Ok(task) 
                : Results.NotFound();
        })
        .WithName("GetTask");

        return app;
    }
}
```

**Java Spring MVC Controller**:
```java
package com.example.taskmanager.api.controllers;

import com.example.taskmanager.api.dto.*;
import com.example.taskmanager.application.services.TaskService;
import com.example.taskmanager.domain.tasks.*;
import io.swagger.v3.oas.annotations.Operation;
import io.swagger.v3.oas.annotations.tags.Tag;
import jakarta.validation.Valid;
import lombok.RequiredArgsConstructor;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;
import org.springframework.web.bind.annotation.*;

import java.net.URI;
import java.util.List;
import java.util.UUID;

@RestController
@RequestMapping("/api/tasks")
@RequiredArgsConstructor
@Tag(name = "Tasks", description = "Task management endpoints")
public class TaskController {
    
    private final TaskService taskService;
    
    @PostMapping
    @Operation(summary = "Create a new task")
    public ResponseEntity<TaskResponse> createTask(
            @Valid @RequestBody CreateTaskRequest request) {
        
        Task task = taskService.createTask(
            request.title(), 
            request.description());
        
        TaskResponse response = TaskResponse.from(task);
        URI location = URI.create("/api/tasks/" + task.getId().value());
        
        return ResponseEntity.created(location).body(response);
    }
    
    @GetMapping("/{id}")
    @Operation(summary = "Get task by ID")
    public ResponseEntity<TaskResponse> getTask(
            @PathVariable UUID id) {
        
        Task task = taskService.getTaskById(TaskId.of(id));
        return ResponseEntity.ok(TaskResponse.from(task));
    }
    
    @GetMapping
    @Operation(summary = "List all tasks")
    public ResponseEntity<List<TaskResponse>> listTasks() {
        List<Task> tasks = taskService.getAllTasks();
        List<TaskResponse> response = tasks.stream()
            .map(TaskResponse::from)
            .toList();
        return ResponseEntity.ok(response);
    }
}
```

**Key Patterns**:
- ✅ Thin controllers - delegate to service layer
- ✅ Use DTOs (never expose domain entities)
- ✅ Return proper HTTP status codes
- ✅ OpenAPI documentation

**Differences**:
- C# uses Minimal APIs (functional style); Java uses class-based controllers
- C# `Results` helper vs Java `ResponseEntity`
- C# uses `MapPost`, `MapGet` methods; Java uses `@PostMapping`, `@GetMapping` annotations
- Java `@RestController` combines `@Controller` + `@ResponseBody`

### DTOs (Data Transfer Objects)

**C# Records**:
```csharp
namespace TaskManager.Api.Contracts;

public sealed record CreateTaskRequest(
    string Title, 
    string Description);

public sealed record TaskResponse(
    Guid Id,
    string Title,
    string Description,
    string Status,
    DateTime CreatedAt);
```

**Java Records**:
```java
package com.example.taskmanager.api.dto;

import jakarta.validation.constraints.NotBlank;
import jakarta.validation.constraints.Size;

public record CreateTaskRequest(
    @NotBlank(message = "Title is required")
    @Size(max = 200, message = "Title must be 200 characters or less")
    String title,
    
    @Size(max = 2000, message = "Description too long")
    String description
) {}

public record TaskResponse(
    UUID id,
    String title,
    String description,
    String status,
    LocalDateTime createdAt
) {
    public static TaskResponse from(Task task) {
        return new TaskResponse(
            task.getId().value(),
            task.getTitle(),
            task.getDescription(),
            task.getStatus().name(),
            task.getCreatedAt()
        );
    }
}
```

**Key Patterns**:
- ✅ Both use records for immutability
- ✅ Separate DTOs from domain models
- ✅ Factory methods for conversions

**Differences**:
- Java uses Jakarta validation annotations on record fields
- C# validation typically uses FluentValidation library or Data Annotations on separate validator classes
- Java static factory pattern common on DTOs; C# uses extension methods or mapper classes

### Exception Handling

**C# Middleware**:
```csharp
app.UseExceptionHandler(exceptionHandlerApp =>
{
    exceptionHandlerApp.Run(async context =>
    {
        context.Response.StatusCode = StatusCodes.Status500InternalServerError;
        context.Response.ContentType = "application/problem+json";

        var exceptionFeature = context.Features
            .Get<IExceptionHandlerFeature>();

        if (exceptionFeature?.Error is not null)
        {
            var problem = new ProblemDetails
            {
                Status = StatusCodes.Status500InternalServerError,
                Title = "An error occurred",
                Detail = exceptionFeature.Error.Message
            };

            await context.Response.WriteAsJsonAsync(problem);
        }
    });
});
```

**Java Global Exception Handler**:
```java
package com.example.taskmanager.api.exception;

import com.example.taskmanager.application.services.TaskNotFoundException;
import org.springframework.http.HttpStatus;
import org.springframework.http.ProblemDetail;
import org.springframework.web.bind.annotation.ExceptionHandler;
import org.springframework.web.bind.annotation.RestControllerAdvice;

@RestControllerAdvice
public class GlobalExceptionHandler {
    
    @ExceptionHandler(TaskNotFoundException.class)
    public ProblemDetail handleTaskNotFound(TaskNotFoundException ex) {
        ProblemDetail problem = ProblemDetail.forStatusAndDetail(
            HttpStatus.NOT_FOUND,
            ex.getMessage()
        );
        problem.setTitle("Task Not Found");
        return problem;
    }
    
    @ExceptionHandler(IllegalArgumentException.class)
    public ProblemDetail handleValidation(IllegalArgumentException ex) {
        ProblemDetail problem = ProblemDetail.forStatusAndDetail(
            HttpStatus.BAD_REQUEST,
            ex.getMessage()
        );
        problem.setTitle("Validation Error");
        return problem;
    }
    
    @ExceptionHandler(Exception.class)
    public ProblemDetail handleGeneral(Exception ex) {
        ProblemDetail problem = ProblemDetail.forStatusAndDetail(
            HttpStatus.INTERNAL_SERVER_ERROR,
            "An unexpected error occurred"
        );
        problem.setTitle("Internal Server Error");
        return problem;
    }
}
```

**Key Patterns**:
- ✅ Centralized exception handling
- ✅ RFC 7807 Problem Details format
- ✅ Maps exceptions to HTTP status codes

**Differences**:
- Java uses `@RestControllerAdvice` + `@ExceptionHandler` annotations
- C# uses middleware pipeline with `UseExceptionHandler`
- Java more declarative; C# more imperative

---

## Testing Patterns

### Unit Tests

**C# with xUnit + FakeItEasy**:
```csharp
using Xunit;
using FakeItEasy;
using TaskManager.Application.Services;
using TaskManager.Domain.Repositories;
using TaskManager.Domain.Tasks;

namespace TaskManager.UnitTests.Services;

public sealed class TaskServiceTests
{
    [Fact]
    public async System.Threading.Tasks.Task AddTaskAsync_ValidInput_CreatesTask()
    {
        // Arrange
        var fakeRepository = A.Fake<ITaskRepository>();
        var service = new TaskService(
            fakeRepository, 
            A.Fake<ILogger<TaskService>>());

        // Act
        var result = await service.AddTaskAsync("Test Task", "Description");

        // Assert
        A.CallTo(() => fakeRepository.AddTaskAsync(
            A<Task>.That.Matches(t => t.Title == "Test Task"), 
            A<CancellationToken>._))
        .MustHaveHappenedOnceExactly();
    }
}
```

**Java with JUnit 5 + Mockito**:
```java
package com.example.taskmanager.application.services;

import com.example.taskmanager.domain.tasks.*;
import org.junit.jupiter.api.BeforeEach;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.junit.jupiter.api.extension.ExtendWith;
import org.mockito.InjectMocks;
import org.mockito.Mock;
import org.mockito.junit.jupiter.MockitoExtension;

import static org.junit.jupiter.api.Assertions.*;
import static org.mockito.ArgumentMatchers.any;
import static org.mockito.Mockito.*;

@ExtendWith(MockitoExtension.class)
@DisplayName("TaskService Unit Tests")
class TaskServiceTest {
    
    @Mock
    private TaskRepository taskRepository;
    
    @InjectMocks
    private TaskService taskService;
    
    @Test
    @DisplayName("createTask with valid input creates task")
    void createTask_ValidInput_CreatesTask() {
        // Arrange
        Task task = Task.create("Test Task", "Description");
        when(taskRepository.save(any(Task.class))).thenReturn(task);
        
        // Act
        TaskId result = taskService.createTask("Test Task", "Description");
        
        // Assert
        assertNotNull(result);
        verify(taskRepository, times(1)).save(any(Task.class));
    }
}
```

**Key Patterns**:
- ✅ Arrange-Act-Assert structure
- ✅ Mock dependencies
- ✅ Verify interactions

**Differences**:
- C# `[Fact]` vs Java `@Test`
- C# FakeItEasy `A.Fake<T>()` vs Java Mockito `@Mock` annotation
- C# `A.CallTo().MustHaveHappened()` vs Java `verify()`
- Java uses `@ExtendWith(MockitoExtension.class)` for mock injection
- Java `@DisplayName` for readable test names; C# uses method name

### Integration Tests

**C# Integration Test (WebApplicationFactory)**:
```csharp
using Microsoft.AspNetCore.Mvc.Testing;
using Xunit;

namespace TaskManager.IntegrationTests;

public sealed class TaskApiTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public TaskApiTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async System.Threading.Tasks.Task CreateTask_ReturnsCreated()
    {
        // Arrange
        var request = new { Title = "Test", Description = "Test Desc" };

        // Act
        var response = await _client.PostAsJsonAsync("/api/tasks", request);

        // Assert
        response.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Created, response.StatusCode);
    }
}
```

**Java Integration Test (@SpringBootTest)**:
```java
package com.example.taskmanager.api;

import com.example.taskmanager.api.dto.CreateTaskRequest;
import com.example.taskmanager.api.dto.TaskResponse;
import org.junit.jupiter.api.DisplayName;
import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.boot.test.web.client.TestRestTemplate;
import org.springframework.http.HttpStatus;
import org.springframework.http.ResponseEntity;

import static org.junit.jupiter.api.Assertions.*;

@SpringBootTest(webEnvironment = SpringBootTest.WebEnvironment.RANDOM_PORT)
@DisplayName("Task API Integration Tests")
class TaskApiIntegrationTest {
    
    @Autowired
    private TestRestTemplate restTemplate;
    
    @Test
    @DisplayName("POST /api/tasks creates new task")
    void createTask_ValidRequest_ReturnsCreated() {
        // Arrange
        CreateTaskRequest request = new CreateTaskRequest(
            "Test Task",
            "Test Description"
        );
        
        // Act
        ResponseEntity<TaskResponse> response = restTemplate.postForEntity(
            "/api/tasks",
            request,
            TaskResponse.class
        );
        
        // Assert
        assertEquals(HttpStatus.CREATED, response.getStatusCode());
        assertNotNull(response.getBody());
        assertEquals("Test Task", response.getBody().title());
    }
}
```

**Key Patterns**:
- ✅ Full application context
- ✅ Real HTTP requests
- ✅ Test database (H2 for Java, InMemory for .NET)

**Differences**:
- C# `WebApplicationFactory` vs Java `@SpringBootTest`
- C# `HttpClient` vs Java `TestRestTemplate`
- Java `@Autowired` for dependency injection in tests
- Java `webEnvironment = RANDOM_PORT` to avoid port conflicts

---

## Configuration Management

### Configuration Files

**.NET (appsettings.json)**:
```json
{
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=TaskManager;..."
  },
  "AllowedHosts": "*"
}
```

**Spring Boot (application.yml)**:
```yaml
spring:
  application:
    name: taskmanager-api
  datasource:
    url: jdbc:postgresql://localhost:5432/taskmanager
    username: ${DATABASE_USERNAME:postgres}
    password: ${DATABASE_PASSWORD:postgres}
  jpa:
    hibernate:
      ddl-auto: validate
    show-sql: true

logging:
  level:
    root: INFO
    com.example.taskmanager: DEBUG

server:
  port: 8080
```

**Key Patterns**:
- ✅ Hierarchical configuration
- ✅ Environment-specific overrides
- ✅ External configuration

**Differences**:
- .NET uses JSON; Spring Boot commonly uses YAML (can use properties files too)
- .NET `appsettings.{Environment}.json` vs Spring Boot `application-{profile}.yml`
- Spring Boot `${VAR:default}` syntax for environment variables
- .NET accessed via `IConfiguration`; Spring Boot via `@Value` or `@ConfigurationProperties`

### Reading Configuration

**C#**:
```csharp
public class MyService
{
    private readonly string _connectionString;
    
    public MyService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection");
    }
}
```

**Java**:
```java
@Service
public class MyService {
    @Value("${spring.datasource.url}")
    private String connectionString;
    
    // Or use @ConfigurationProperties for type-safe config
}

@ConfigurationProperties(prefix = "app")
@Data
public class AppConfig {
    private String name;
    private Database database;
    
    @Data
    public static class Database {
        private String url;
        private int maxConnections;
    }
}
```

**Differences**:
- C# uses `IConfiguration` interface
- Java uses `@Value` annotation or `@ConfigurationProperties` classes
- Java approach more declarative with annotations

---

## Dependency Injection

### Registration

**C# (Program.cs)**:
```csharp
var builder = WebApplication.CreateBuilder(args);

// Register services
builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
builder.Services.AddScoped<TaskService>();
builder.Services.AddTransient<IEmailService, EmailService>();

var app = builder.Build();
```

**Java (Configuration Class)**:
```java
@Configuration
public class AppConfig {
    
    @Bean
    public TaskRepository taskRepository() {
        return new InMemoryTaskRepository();
    }
    
    // Or use component scanning:
    // @Component, @Service, @Repository auto-detected
}

// Or use annotations directly on classes:
@Service  // Automatically registered as singleton
public class TaskService { ... }

@Repository  // Automatically registered
public class JpaTaskRepositoryAdapter implements TaskRepository { ... }
```

**Lifetimes**:

| .NET | Spring Boot | Description |
|------|-------------|-------------|
| `AddSingleton` | `@Scope("singleton")` (default) | One instance per application |
| `AddScoped` | `@Scope("request")` | One instance per HTTP request |
| `AddTransient` | `@Scope("prototype")` | New instance every time |

**Key Differences**:
- C# registers explicitly; Java often uses component scanning with annotations
- C# extension methods; Java `@Bean` methods or stereotype annotations
- Spring default is singleton; .NET requires explicit choice

### Constructor Injection

**C#**:
```csharp
public sealed class TaskService
{
    private readonly ITaskRepository _repository;
    private readonly ILogger<TaskService> _logger;
    
    public TaskService(ITaskRepository repository, ILogger<TaskService> logger)
    {
        _repository = repository;
        _logger = logger;
    }
}
```

**Java (with Lombok)**:
```java
@Service
@RequiredArgsConstructor  // Lombok generates constructor
public class TaskService {
    
    private final TaskRepository repository;
    private final Logger logger;
    
    // Constructor generated by Lombok
}

// Without Lombok:
@Service
public class TaskService {
    
    private final TaskRepository repository;
    private final Logger logger;
    
    public TaskService(TaskRepository repository, Logger logger) {
        this.repository = repository;
        this.logger = logger;
    }
}
```

**Key Pattern**: Both prefer constructor injection over field or setter injection

---

## Logging

### Basic Logging

**C#**:
```csharp
using Microsoft.Extensions.Logging;

public class TaskService
{
    private readonly ILogger<TaskService> _logger;
    
    public TaskService(ILogger<TaskService> logger)
    {
        _logger = logger;
    }
    
    public async Task DoSomething()
    {
        _logger.LogInformation("Doing something");
        _logger.LogWarning("Warning message");
        _logger.LogError(ex, "Error occurred");
    }
}
```

**Java**:
```java
import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

@Service
public class TaskService {
    
    private static final Logger log = 
        LoggerFactory.getLogger(TaskService.class);
    
    public void doSomething() {
        log.info("Doing something");
        log.warn("Warning message");
        log.error("Error occurred", ex);
    }
}
```

**Structured Logging**:

**C#**:
```csharp
_logger.LogInformation(
    "Task {TaskId} created by {UserId}", 
    taskId, userId);
```

**Java**:
```java
log.info("Task {} created by {}", taskId, userId);
```

**Key Patterns**:
- ✅ Structured logging with placeholders
- ✅ Log levels (Trace, Debug, Info, Warning, Error, Critical)

**Differences**:
- C# uses `ILogger<T>` injected via DI
- Java uses static `Logger` instance (common pattern)
- C# uses named parameters `{TaskId}`; Java uses positional `{}`

---

## Common Patterns Comparison

### Async/Await vs Synchronous

**C# Async Pattern**:
```csharp
public async Task<Task?> GetTaskByIdAsync(TaskId id)
{
    return await _repository.FindByIdAsync(id);
}

// Calling code
var task = await service.GetTaskByIdAsync(taskId);
```

**Java Traditional Pattern** (Spring Boot handles threading):
```java
public Optional<Task> getTaskById(TaskId id) {
    return repository.findById(id);
}

// Calling code
Optional<Task> task = service.getTaskById(taskId);
```

**Java CompletableFuture** (if async needed):
```java
public CompletableFuture<Optional<Task>> getTaskByIdAsync(TaskId id) {
    return CompletableFuture.supplyAsync(
        () -> repository.findById(id)
    );
}
```

**Key Difference**: C# async/await is pervasive; Java Spring Boot applications traditionally synchronous (framework handles threading)

### Null Handling

**C#**:
```csharp
// Nullable reference types (C# 8+)
public Task? FindTask(TaskId id)  // May return null
{
    return _repository.FindById(id);
}

// Null-conditional operator
var title = task?.Title ?? "Unknown";

// Null-forgiving operator
var task = FindTask(id)!;  // Assert non-null
```

**Java**:
```java
// Optional<T>
public Optional<Task> findTask(TaskId id) {
    return repository.findById(id);
}

// Usage
Optional<Task> taskOpt = findTask(id);
String title = taskOpt.map(Task::getTitle).orElse("Unknown");

// Or throw if not found
Task task = taskOpt.orElseThrow(
    () -> new TaskNotFoundException(id)
);
```

**Key Difference**: C# has nullable reference types with `?`; Java uses `Optional<T>` wrapper

### LINQ vs Streams

**C# LINQ**:
```csharp
var activeTasks = tasks
    .Where(t => t.Status != TaskStatus.Done)
    .OrderBy(t => t.CreatedAt)
    .Take(10)
    .ToList();
```

**Java Streams**:
```java
List<Task> activeTasks = tasks.stream()
    .filter(t -> t.getStatus() != TaskStatus.COMPLETED)
    .sorted(Comparator.comparing(Task::getCreatedAt))
    .limit(10)
    .collect(Collectors.toList());
```

**Key Pattern**: Both provide functional-style collection operations

---

## Summary

### When to Use Which?

**Use .NET When**:
- Microsoft ecosystem (Azure,SQL Server, Active Directory)
- Windows-first environment
- Existing C# expertise on team
- Strong async/await requirements
- Enterprise Windows applications

**Use Spring Boot When**:
- JVM ecosystem (Kafka, Hadoop, Cassandra)
- Linux/container-first environment
- Existing Java expertise on team
- Need for extensive Java library ecosystem
- Enterprise cross-platform applications
- Microservices architecture

### Learning Path

**For .NET developers learning Spring Boot**:
1. Learn Java  syntax and idioms
2. Understand Spring Framework basics (@Component, @Service, @Autowired)
3. Learn Spring Boot conventions and auto-configuration
4. Study Spring Data JPA (vs Entity Framework)
5. Practice with JUnit 5 + Mockito (vs xUnit + FakeItEasy)

**For Java developers learning .NET**:
1. Learn C# syntax and idioms
2. Understand .NET DI container
3. Learn async/await pattern
4. Study Entity Framework Core
5. Practice with xUnit + FakeItEasy

---

## Additional Resources

### .NET Resources
- [.NET Documentation](https://docs.microsoft.com/dotnet/)
- [ASP.NET Core Documentation](https://docs.microsoft.com/aspnet/core/)
- [Entity Framework Core](https://docs.microsoft.com/ef/core/)

### Spring Boot Resources
- [Spring Boot Documentation](https://spring.io/projects/spring-boot)
- [Spring Data JPA Documentation](https://spring.io/projects/spring-data-jpa)
- [Baeldung Spring Tutorials](https://www.baeldung.com/)

### Clean Architecture
- [Clean Architecture by Robert C. Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [DDD Reference by Eric Evans](https://www.domainlanguage.com/ddd/reference/)

---

**Document Version**: 1.0  
**Last Updated**: March 31, 2026  
**Maintained By**: AI Coding Workshop Team
