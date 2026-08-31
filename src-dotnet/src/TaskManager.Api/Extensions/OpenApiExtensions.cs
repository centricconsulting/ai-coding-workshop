namespace TaskManager.Api.Extensions;

/// <summary>
/// Extension methods for configuring OpenAPI documentation
/// </summary>
public static class OpenApiExtensions
{
    /// <summary>
    /// Add OpenAPI services and configuration
    /// </summary>
    public static IServiceCollection AddOpenApiDocumentation(this IServiceCollection services)
    {
        services.AddOpenApi();
        
        // TODO: Lab 3 - Participants can use Copilot to enhance OpenAPI configuration
        // Example: Add custom OpenAPI info, security definitions, etc.
        
        return services;
    }
    
    /// <summary>
    /// Configure OpenAPI middleware pipeline
    /// </summary>
    public static WebApplication UseOpenApiDocumentation(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
            
            // TODO: Lab 3 - Participants can add Swagger UI or other documentation tools
            // Example: app.UseSwaggerUI(options => options.SwaggerEndpoint("/openapi/v1.json", "Task Manager API"));
        }
        
        return app;
    }
}
