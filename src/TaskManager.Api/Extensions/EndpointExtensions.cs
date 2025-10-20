using Microsoft.AspNetCore.Mvc;
using TaskManager.Api.Models;
using TaskManager.Application.Commands;
using TaskManager.Application.Handlers;
using TaskManager.Application.Services;
using DomainTask = TaskManager.Domain.Tasks.Task;

namespace TaskManager.Api.Extensions;

/// <summary>
/// Extension methods for configuring API endpoints
/// </summary>
public static class EndpointExtensions
{
    /// <summary>
    /// Map all Task Manager API endpoints
    /// </summary>
    public static WebApplication MapTaskEndpoints(this WebApplication app)
    {
        // Health check endpoint
        app.MapGet("/health", () => new { Status = "Healthy", Message = "Task Manager API is ready for Lab 3!" })
            .WithName("HealthCheck")
            .WithOpenApi();

        // Create new task
        app.MapPost("/tasks", CreateTaskAsync)
            .WithName("CreateTask")
            .WithOpenApi();

        // TODO: Lab 3 - Participants will use Copilot to generate these task endpoints:
        
        // Get task by ID
        // app.MapGet("/tasks/{id:guid}", GetTaskByIdAsync)
        //     .WithName("GetTask")
        //     .WithOpenApi();
        
        // Create new task
        // app.MapPost("/tasks", CreateTaskAsync)
        //     .WithName("CreateTask")
        //     .WithOpenApi();
        
        // Update task status
        // app.MapPut("/tasks/{id:guid}/status", UpdateTaskStatusAsync)
        //     .WithName("UpdateTaskStatus")
        //     .WithOpenApi();
        
        // Get all active tasks
        // app.MapGet("/tasks", GetActiveTasksAsync)
        //     .WithName("GetActiveTasks")
        //     .WithOpenApi();

        return app;
    }

    /// <summary>
    /// Handles POST /tasks endpoint - creates a new task
    /// </summary>
    private static async Task<IResult> CreateTaskAsync(
        [FromBody] CreateTaskRequest request,
        CreateTaskCommandHandler handler,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger("TaskEndpoints");
        
        try
        {
            // Map DTO to command
            var command = new CreateTaskCommand
            {
                Title = request.Title,
                Description = request.Description,
                Priority = request.Priority,
                DueDate = request.DueDate
            };

            // Execute command
            var task = await handler.HandleAsync(command, cancellationToken);

            // Map domain entity to response DTO
            var response = MapToResponse(task);

            // Return 201 Created with Location header
            return Results.Created($"/tasks/{response.Id}", response);
        }
        catch (ArgumentException ex)
        {
            // Handle validation errors (invalid priority, title, due date)
            logger.LogWarning(ex, "Validation error creating task: {Message}", ex.Message);
            return Results.BadRequest(new ProblemDetails
            {
                Status = 400,
                Title = "Validation Error",
                Detail = ex.Message,
                Type = "https://tools.ietf.org/html/rfc9110#section-15.5.1"
            });
        }
        catch (Exception ex)
        {
            // Handle unexpected errors
            logger.LogError(ex, "Unexpected error creating task");
            return Results.Problem(
                title: "Internal Server Error",
                detail: "An unexpected error occurred while creating the task",
                statusCode: 500
            );
        }
    }

    /// <summary>
    /// Maps domain Task entity to API TaskResponse DTO
    /// </summary>
    private static TaskResponse MapToResponse(DomainTask task)
    {
        return new TaskResponse
        {
            Id = task.Id.Value.ToString(),
            Title = task.Title,
            Description = task.Description,
            Priority = task.Priority.Name,
            Status = task.Status.ToString(),
            DueDate = task.DueDate,
            CreatedAt = task.CreatedAt,
            UpdatedAt = task.UpdatedAt
        };
    }

    // TODO: Lab 3 - Participants will use Copilot to generate these endpoint handlers:
    
    // private static async Task<IResult> GetTaskByIdAsync(Guid id, TaskService taskService)
    // {
    //     // Copilot will implement this
    // }
    
    // private static async Task<IResult> CreateTaskAsync(CreateTaskRequest request, TaskService taskService)
    // {
    //     // Copilot will implement this
    // }
    
    // private static async Task<IResult> UpdateTaskStatusAsync(Guid id, UpdateStatusRequest request, TaskService taskService)
    // {
    //     // Copilot will implement this
    // }
    
    // private static async Task<IResult> GetActiveTasksAsync(TaskService taskService)
    // {
    //     // Copilot will implement this
    // }
}

// TODO: Lab 3 - Participants will use Copilot to generate these request/response models:
// public record CreateTaskRequest(string Title, string Description);
// public record UpdateStatusRequest(int Status);
// public record TaskResponse(Guid Id, string Title, string Description, int Status, DateTime CreatedAt, DateTime UpdatedAt);
