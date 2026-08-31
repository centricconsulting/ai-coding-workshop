namespace TaskManager.Api.Extensions;

/// <summary>
/// Extension methods for configuring logging
/// </summary>
public static class LoggingExtensions
{
    /// <summary>
    /// Configure application logging
    /// </summary>
    public static IServiceCollection AddApplicationLogging(this IServiceCollection services)
    {
        // Logging is already configured by default in WebApplication.CreateBuilder()
        // This extension is here for consistency and future enhancements
        
        // TODO: Lab 3 - Participants can use Copilot to enhance logging configuration
        // Example: Add custom log levels, structured logging, or external providers
        
        return services;
    }
    
    /// <summary>
    /// Configure logging middleware
    /// </summary>
    public static WebApplication UseApplicationLogging(this WebApplication app)
    {
        // TODO: Lab 3 - Participants can add request/response logging middleware
        // Example: app.UseHttpLogging();
        
        return app;
    }
}
