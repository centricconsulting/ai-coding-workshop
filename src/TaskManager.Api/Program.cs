using TaskManager.Api.Extensions;

namespace TaskManager.Api;

/// <summary>
/// Task Manager API for Lab 3: Code Generation & Refactoring
/// 
/// This API is organized using extension methods for:
/// - Service registration (Clean Architecture dependencies)
/// - OpenAPI documentation configuration
/// - Endpoint mapping and handlers
/// - Logging configuration
/// 
/// LAB 3 INSTRUCTIONS:
/// Use Copilot to scaffold minimal API endpoints in the EndpointExtensions class:
/// 1. GET /tasks/{id} - Get task by ID
/// 2. POST /tasks - Create new task  
/// 3. PUT /tasks/{id}/status - Update task status
/// 4. GET /tasks - Get all active tasks
/// 
/// Example prompt: "Implement the GetTaskByIdAsync endpoint handler with proper error handling"
/// </summary>
public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Configure services using extension methods
        builder.Services
            .AddApplicationServices()      // Clean Architecture dependencies
            .AddApplicationLogging()       // Logging configuration
            .AddOpenApiDocumentation();    // OpenAPI/Swagger setup

        var app = builder.Build();

        // Configure middleware pipeline using extension methods
        app.UseHttpsRedirection();
        app.UseApplicationLogging();       // Request/response logging
        app.UseOpenApiDocumentation();     // OpenAPI middleware
        app.MapTaskEndpoints();            // API endpoints

        app.Run();
    }
}
