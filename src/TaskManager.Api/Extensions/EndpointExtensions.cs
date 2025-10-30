using TaskManager.Api.Models;
using TaskManager.Application.Commands;
using TaskManager.Application.Services;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        app.MapPost("/tasks", async (
            CreateTaskRequest request,
            CreateTaskCommandHandler handler,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken) =>
        {
            var logger = loggerFactory.CreateLogger("TaskManager.Api.Endpoints.CreateTask");
            
            try
            {
                // Validate request DTO
                var validationContext = new ValidationContext(request);
                var validationResults = new List<ValidationResult>();
                if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
                {
                    var errors = validationResults.Select(v => v.ErrorMessage).ToList();
                    logger.LogWarning("Request validation failed: {Errors}", string.Join(", ", errors));
                    
                    var problemDetails = new ProblemDetails
                    {
                        Status = 400,
                        Title = "Validation Error",
                        Detail = string.Join(" ", errors),
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                    };
                    
                    return Results.Problem(problemDetails);
                }
                
                logger.LogInformation(
                    "Creating task with title: {Title}, priority: {Priority}",
                    request.Title,
                    request.Priority);

                var command = new CreateTaskCommand
                {
                    Title = request.Title,
                    Description = request.Description,
                    Priority = request.Priority,
                    DueDate = request.DueDate
                };

                var task = await handler.HandleAsync(command, cancellationToken);

                var response = TaskResponse.FromDomain(task);

                logger.LogInformation(
                    "Successfully created task with ID: {TaskId}",
                    response.Id);

                return Results.Created($"/tasks/{response.Id}", response);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning(
                    ex,
                    "Validation error creating task: {Message}",
                    ex.Message);

                var problemDetails = new ProblemDetails
                {
                    Status = 400,
                    Title = "Validation Error",
                    Detail = ex.Message,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                };
                
                return Results.Problem(problemDetails);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Unexpected error creating task with title: {Title}",
                    request.Title);

                return Results.Problem(new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred while creating the task.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                });
            }
        })
            .WithName("CreateTask")
            .WithOpenApi();

        // TODO: Lab 3 - Participants will use Copilot to generate these task endpoints:
        
        // Get task by ID
        // app.MapGet("/tasks/{id:guid}", GetTaskByIdAsync)
        //     .WithName("GetTask")
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

    // TODO: Lab 3 - Participants will use Copilot to generate these endpoint handlers:
    
    // private static async Task<IResult> GetTaskByIdAsync(Guid id, TaskService taskService)
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
