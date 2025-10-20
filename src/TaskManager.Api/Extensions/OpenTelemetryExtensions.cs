using OpenTelemetry;
using OpenTelemetry.Resources;
using OpenTelemetry.Trace;

namespace TaskManager.Api.Extensions;

/// <summary>
/// Extension methods for configuring OpenTelemetry observability
/// </summary>
public static class OpenTelemetryExtensions
{
    /// <summary>
    /// Add OpenTelemetry services with console logging
    /// This gives developers exposure to modern observability practices
    /// </summary>
    public static IServiceCollection AddOpenTelemetryObservability(this IServiceCollection services)
    {
        services.AddOpenTelemetry()
            .ConfigureResource(resource => resource
                .AddService("TaskManager.Api")
                .AddAttributes(new Dictionary<string, object>
                {
                    ["service.version"] = "1.0.0",
                    ["service.environment"] = "workshop"
                }))
            .WithTracing(tracing =>
            {
                tracing
                    .AddAspNetCoreInstrumentation(options =>
                    {
                        // Capture request and response bodies for learning purposes
                        options.RecordException = true;
                        options.EnrichWithHttpRequest = (activity, request) =>
                        {
                            activity.SetTag("http.request.method", request.Method);
                            activity.SetTag("http.request.path", request.Path);
                        };
                        options.EnrichWithHttpResponse = (activity, response) =>
                        {
                            activity.SetTag("http.response.status_code", response.StatusCode);
                        };
                    })
                    .AddSource("TaskManager.*")  // Capture our custom activity sources
                    .AddConsoleExporter();       // Export traces to console for learning
            });
        
        // TODO: Lab 3 - Participants can use Copilot to add additional instrumentation
        // Example: .AddHttpClientInstrumentation(), .AddSqlClientInstrumentation()
        
        return services;
    }
}
