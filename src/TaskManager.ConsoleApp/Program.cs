using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using TaskManager.Application.Services;
using TaskManager.Domain.Repositories;
using TaskManager.Infrastructure.Repositories;

namespace TaskManager.ConsoleApp;

/// <summary>
/// Console application for Lab 1: Controlling Context with Copilot Instructions
/// 
/// This console app is configured with:
/// - Dependency Injection
/// - Logging with ILogger
/// - Clean Architecture dependencies
/// 
/// LAB 1 INSTRUCTIONS:
/// Use Copilot to generate a C# service class that:
/// 1. Uses async/await patterns
/// 2. Implements ILogger for logging
/// 3. Follows the workshop coding conventions
/// 4. Demonstrates Clean Architecture patterns
/// 
/// Example prompt: "Generate a C# service class that logs with ILogger and uses async methods"
/// </summary>
public class Program
{
    public static async Task Main(string[] args)
    {
        // Configure host with DI and logging
        var host = Host.CreateDefaultBuilder(args)
            .ConfigureServices((context, services) =>
            {
                // Register repositories
                services.AddSingleton<ITaskRepository, InMemoryTaskRepository>();
                
                // Register application services
                services.AddScoped<TaskService>();
                
                // TODO: Lab 1 - Participants will use Copilot to generate additional services here
                // Example: services.AddScoped<IYourServiceInterface, YourServiceImplementation>();
            })
            .Build();

        // Get logger for demonstration
        var logger = host.Services.GetRequiredService<ILogger<Program>>();
        
        logger.LogInformation("🚀 Task Manager Console App Started");
        logger.LogInformation("📚 This is Lab 1: Controlling Context with Copilot Instructions");
        
        // Get TaskService to demonstrate DI working
        var taskService = host.Services.GetRequiredService<TaskService>();
        logger.LogInformation("✅ TaskService successfully resolved from DI container");

        // TODO: Lab 1 - Participants will use Copilot to:
        // 1. Generate a new service class (e.g., ReportingService, NotificationService)
        // 2. Register it in DI above
        // 3. Resolve and use it here
        // 4. Observe how Copilot respects the workshop instructions for coding style
        
        logger.LogInformation("🎯 Ready for Lab 1 exercises!");
        logger.LogInformation("💡 Use Copilot to generate service classes with async/await and ILogger");
        
        await host.RunAsync();
    }
}
