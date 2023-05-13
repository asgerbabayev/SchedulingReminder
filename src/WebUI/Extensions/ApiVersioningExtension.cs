namespace ShedulingReminders.WebUI.Extensions;

/// <summary>
/// Extension class for adding and configuring API versioning in the application.
/// </summary>
public static class ApiVersioningExtension
{
    /// <summary>
    /// Adds and configures API versioning options.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddAndConfigureApiVersioning(this IServiceCollection services)
    {
        // Adds and configures API versioning options
        services.AddApiVersioning(opt =>
        {
            // Sets the default API version to 1.0
            opt.DefaultApiVersion = new Microsoft.AspNetCore.Mvc.ApiVersion(1, 0);

            // Assumes the default version when the version is not specified
            opt.AssumeDefaultVersionWhenUnspecified = true;

            // Enables reporting of API versions in the response headers
            opt.ReportApiVersions = true;

            // Configures the API version readers to determine the version from different sources
            opt.ApiVersionReader = ApiVersionReader.Combine(
                new UrlSegmentApiVersionReader(),
                new HeaderApiVersionReader("x-api-version"),
                new MediaTypeApiVersionReader("x-api-version")
            );
        });

        // Configures versioned API exploration
        services.AddVersionedApiExplorer(config =>
        {
            // Sets the group name format to 'v'VVV (e.g., 'v1')
            config.GroupNameFormat = "'v'VVV";

            // Enables substitution of API versions in URLs
            config.SubstituteApiVersionInUrl = true;
        });

        return services;
    }
}


