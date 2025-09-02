using TaskManager.Application.Services;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Api;

/// <summary>
/// Task Manager API for Lab 3: Code Generation & Refactoring
/// 
/// This API is configured with:
/// - Minimal API endpoints
/// - Clean Architecture dependencies
/// - OpenAPI documentation
/// - Dependency Injection
/// 
/// LAB 3 INSTRUCTIONS:
/// Use Copilot to scaffold minimal API endpoints:
/// 1. GET /tasks/{id} - Get task by ID
/// 2. POST /tasks - Create new task  
/// 3. PUT /tasks/{id}/status - Update task status
/// 4. GET /tasks - Get all active tasks
/// 
/// Example prompt: "Create minimal API endpoints for task management with proper async/await"
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container
        builder.Services.AddOpenApi();
        
        // Register Clean Architecture dependencies
        builder.Services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
        builder.Services.AddScoped<TaskService>();

        // TODO: Lab 3 - Add any additional services Copilot generates

        var app = builder.Build();

        // Configure the HTTP request pipeline
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseHttpsRedirection();

        // TODO: Lab 3 - Participants will use Copilot to generate these endpoints:
        
        // app.MapGet("/tasks/{id:guid}", async (Guid id, TaskService taskService) => {
        //     // Copilot will implement this
        // });
        
        // app.MapPost("/tasks", async (CreateTaskRequest request, TaskService taskService) => {
        //     // Copilot will implement this  
        // });
        
        // app.MapPut("/tasks/{id:guid}/status", async (Guid id, UpdateStatusRequest request, TaskService taskService) => {
        //     // Copilot will implement this
        // });
        
        // app.MapGet("/tasks", async (TaskService taskService) => {
        //     // Copilot will implement this
        // });

        // Placeholder endpoint to verify API is working
        app.MapGet("/health", () => new { Status = "Healthy", Message = "Task Manager API is ready for Lab 3!" })
            .WithName("HealthCheck")
            .WithOpenApi();

        app.Run();
    }
}

// TODO: Lab 3 - Participants will use Copilot to generate these request/response models:
// public record CreateTaskRequest(string Title, string Description);
// public record UpdateStatusRequest(int Status);
// public record TaskResponse(Guid Id, string Title, string Description, int Status, DateTime CreatedAt, DateTime UpdatedAt);
