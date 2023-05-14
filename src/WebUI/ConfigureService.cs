using Microsoft.AspNetCore.RateLimiting;
using System.Threading.RateLimiting;

namespace ShedulingReminders.WebUI;

/// <summary>
/// Extension class for configuring web UI services in the application.
/// </summary>
public static class ConfigureService
{
    /// <summary>
    /// Adds and configures web UI services.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddWebUIService(this IServiceCollection services)
    {
        // Adds and configures API versioning
        services.AddAndConfigureApiVersioning();

        // Configures Swagger
        services.ConfigureSwagger();

        services.AddRateLimiter(options =>
        {
            options.AddSlidingWindowLimiter("Sliding", config =>
            {
                config.Window = TimeSpan.FromSeconds(8);
                config.PermitLimit = 4;
                config.QueueLimit = 2;
                config.QueueProcessingOrder = QueueProcessingOrder.OldestFirst;
                config.SegmentsPerWindow = 2;
            });
        });

        return services;
    }
}


