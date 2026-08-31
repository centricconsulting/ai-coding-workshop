using TaskManager.Application.Services;

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
