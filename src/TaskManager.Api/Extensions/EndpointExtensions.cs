using TaskManager.Api.Models;
using TaskManager.Application.Commands;
using TaskManager.Application.Queries;
using TaskManager.Application.Services;
using TaskManager.Domain.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;
using DomainTaskStatus = TaskManager.Domain.Tasks.TaskStatus;

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

        // Get all tasks with optional status filter
        app.MapGet("/tasks", async (
            [FromQuery] string? status,
            GetTasksQueryHandler handler,
            ILoggerFactory loggerFactory,
            CancellationToken cancellationToken) =>
        {
            var logger = loggerFactory.CreateLogger("TaskManager.Api.Endpoints.GetTasks");
            
            try
            {
                DomainTaskStatus? statusFilter = null;
                
                // Parse status parameter if provided
                if (!string.IsNullOrWhiteSpace(status))
                {
                    if (!Enum.TryParse<DomainTaskStatus>(status, ignoreCase: true, out var parsedStatus))
                    {
                        logger.LogWarning("Invalid status parameter: {Status}", status);
                        
                        return Results.BadRequest(new ProblemDetails
                        {
                            Status = 400,
                            Title = "Invalid Status Parameter",
                            Detail = $"Status must be one of: {string.Join(", ", Enum.GetNames<DomainTaskStatus>())}",
                            Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                        });
                    }
                    
                    statusFilter = parsedStatus;
                    logger.LogInformation("Retrieving tasks with status filter: {Status}", status);
                }
                else
                {
                    logger.LogInformation("Retrieving all tasks (no status filter)");
                }

                var query = new GetTasksQuery { Status = statusFilter };
                var tasks = await handler.HandleAsync(query, cancellationToken);
                
                var response = tasks.Select(TaskResponse.FromDomain).ToList();
                
                logger.LogInformation(
                    "Successfully retrieved {Count} tasks",
                    response.Count);

                return Results.Ok(response);
            }
            catch (Exception ex)
            {
                logger.LogError(
                    ex,
                    "Unexpected error retrieving tasks");

                return Results.Problem(new ProblemDetails
                {
                    Status = 500,
                    Title = "Internal Server Error",
                    Detail = "An unexpected error occurred while retrieving tasks.",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
                });
            }
        })
            .WithName("GetTasks")
            .WithOpenApi();

        // Update existing task
        app.MapPut("/tasks/{id:guid}", UpdateTaskAsync)
            .WithName("UpdateTask")
            .WithOpenApi();

        // Delete task
        app.MapDelete("/tasks/{id:guid}", DeleteTaskAsync)
            .WithName("DeleteTask")
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

        return app;
    }

    // PUT /tasks/{id} - Update existing task
    private static async Task<IResult> UpdateTaskAsync(
        Guid id,
        UpdateTaskRequest request,
        UpdateTaskCommandHandler handler,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger("TaskManager.Api.Endpoints.UpdateTask");
        
        try
        {
            // Validate request DTO
            var validationContext = new ValidationContext(request);
            var validationResults = new List<ValidationResult>();
            if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
            {
                var errors = validationResults.Select(v => v.ErrorMessage).ToList();
                logger.LogWarning("Request validation failed for task {TaskId}: {Errors}", id, string.Join(", ", errors));
                
                var problemDetails = new ProblemDetails
                {
                    Status = 400,
                    Title = "Validation Error",
                    Detail = string.Join(" ", errors),
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                };
                
                return Results.Problem(problemDetails);
            }

            logger.LogInformation("Updating task {TaskId} with title: {Title}, priority: {Priority}", id, request.Title, request.Priority);

            // Parse priority using TaskPriority value object
            TaskPriority priority;
            try
            {
                priority = TaskPriority.FromName(request.Priority);
            }
            catch (ArgumentException ex)
            {
                logger.LogWarning("Invalid priority value for task {TaskId}: {Priority}", id, request.Priority);
                
                var problemDetails = new ProblemDetails
                {
                    Status = 400,
                    Title = "Validation Error",
                    Detail = ex.Message,
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1"
                };
                
                return Results.Problem(problemDetails);
            }

            // Create command and invoke handler
            var command = new UpdateTaskCommand(
                new TaskId(id),
                request.Title,
                request.Description ?? string.Empty,
                priority,
                request.DueDate);

            var updatedTask = await handler.HandleAsync(command, cancellationToken);
            
            if (updatedTask == null)
            {
                logger.LogWarning("Task {TaskId} not found", id);
                
                var problemDetails = new ProblemDetails
                {
                    Status = 404,
                    Title = "Not Found",
                    Detail = $"Task with ID {id} was not found",
                    Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
                };
                
                return Results.Problem(problemDetails);
            }

            logger.LogInformation("Successfully updated task {TaskId}", id);

            var response = TaskResponse.FromDomain(updatedTask);
            return Results.Ok(response);
        }
        catch (ArgumentException ex)
        {
            logger.LogWarning(ex, "Validation error updating task {TaskId}: {Message}", id, ex.Message);
            
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
            logger.LogError(ex, "Error updating task {TaskId}", id);
            
            var problemDetails = new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred while updating the task",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            };
            
            return Results.Problem(problemDetails);
        }
    }

    // DELETE /tasks/{id} - Delete existing task
    private static async Task<IResult> DeleteTaskAsync(
        Guid id,
        DeleteTaskCommandHandler handler,
        ILoggerFactory loggerFactory,
        CancellationToken cancellationToken)
    {
        var logger = loggerFactory.CreateLogger("TaskManager.Api.Endpoints.DeleteTask");
        
        try
        {
            logger.LogInformation("Deleting task with ID {TaskId}", id);

            var taskId = new TaskId(id);
            var command = new DeleteTaskCommand(taskId);

            await handler.HandleAsync(command, cancellationToken);

            logger.LogInformation("Task with ID {TaskId} deleted successfully", id);

            return Results.NoContent();
        }
        catch (InvalidOperationException ex)
        {
            logger.LogWarning("Task with ID {TaskId} not found for deletion: {Message}", id, ex.Message);

            return Results.Problem(new ProblemDetails
            {
                Status = 404,
                Title = "Task Not Found",
                Detail = $"Task with ID {id} was not found.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.5.4"
            });
        }
        catch (Exception ex)
        {
            logger.LogError(
                ex,
                "Unexpected error deleting task with ID {TaskId}",
                id);

            return Results.Problem(new ProblemDetails
            {
                Status = 500,
                Title = "Internal Server Error",
                Detail = "An unexpected error occurred while deleting the task.",
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1"
            });
        }
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
