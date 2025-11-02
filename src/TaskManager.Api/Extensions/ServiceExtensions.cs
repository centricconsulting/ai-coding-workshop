using TaskManager.Application.Commands;
using TaskManager.Application.Queries;
using TaskManager.Application.Services;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.Api.Extensions;

/// <summary>
/// Extension methods for configuring application services
/// </summary>
public static class ServiceExtensions
{
    /// <summary>
    /// Add Clean Architecture services to the DI container
    /// </summary>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        // Register repositories
        services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
        
        // Register application services
        services.AddScoped<TaskService>();
        
        // Register command handlers
        services.AddScoped<CreateTaskCommandHandler>();
        services.AddScoped<UpdateTaskCommandHandler>();
        services.AddScoped<DeleteTaskCommandHandler>();
        
        // Register query handlers
        services.AddScoped<GetTasksQueryHandler>();
        
        // Add Problem Details with proper content type
        services.AddProblemDetails(options =>
        {
            options.CustomizeProblemDetails = context =>
            {
                // Ensure ProblemDetails uses application/problem+json content type
                context.HttpContext.Response.ContentType = "application/problem+json";
            };
        });
        
        // TODO: Lab 3 - Add any additional services Copilot generates
        
        return services;
    }
}
