using TaskManager.Application.Handlers;
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
        
        // TODO: Lab 3 - Add any additional services Copilot generates
        
        return services;
    }
}
