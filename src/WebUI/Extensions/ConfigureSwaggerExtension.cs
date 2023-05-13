using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.Options;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using Swashbuckle.AspNetCore.SwaggerUI;
using System.Reflection;

namespace ShedulingReminders.WebUI.Extensions;

/// <summary>
/// Extension class for configuring Swagger in the application.
/// </summary>
public static class ConfigureSwaggerExtension
{
    /// <summary>
    /// Configures Swagger in the application.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the Swagger services to.</param>
    /// <returns>The updated <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            // Resolves conflicting actions by selecting the first one
            c.ResolveConflictingActions(apiDescriptions => apiDescriptions.First());

            // Includes XML comments for Swagger documentation
            c.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory,
                $"{Assembly.GetExecutingAssembly().GetName().Name}.xml"));
        });

        services.ConfigureOptions<ConfigureSwaggerOptions>();

        return services;
    }

    /// <summary>
    /// Uses Swagger and SwaggerUI in the application.
    /// </summary>
    /// <param name="app">The <see cref="IApplicationBuilder"/> to configure the middleware pipeline.</param>
    /// <param name="provider">The <see cref="IApiVersionDescriptionProvider"/> to provide API version descriptions.</param>
    /// <returns>The updated <see cref="IApplicationBuilder"/>.</returns>
    public static IApplicationBuilder UseSwagger(this IApplicationBuilder app, IApiVersionDescriptionProvider provider)
    {
        app.UseSwagger();

        // Enable middleware to serve swagger-ui (HTML, JS, CSS, etc.)
        app.UseSwaggerUI(options =>
        {
            // Configures Swagger endpoint for each API version
            foreach (var description in provider.ApiVersionDescriptions)
                options.SwaggerEndpoint($"/swagger/{description.GroupName}/swagger.json", $"WebApi {description.GroupName.ToUpperInvariant()}");

            // Configures additional SwaggerUI options
            options.DefaultModelExpandDepth(2);
            options.DefaultModelRendering(ModelRendering.Model);
            options.DefaultModelsExpandDepth(-1);
            options.DisplayOperationId();
            options.DisplayRequestDuration();
            options.DocExpansion(DocExpansion.List);
            options.EnableDeepLinking();
            options.EnableFilter();
            options.MaxDisplayedTags(5);
            options.ShowExtensions();
            options.ShowCommonExtensions();
            options.EnableValidator();
        });

        return app;
    }

    /// <summary>
    /// Configures Swagger options based on API version descriptions.
    /// </summary>
    public class ConfigureSwaggerOptions : IConfigureNamedOptions<SwaggerGenOptions>
    {
        private readonly IApiVersionDescriptionProvider provider;

        public ConfigureSwaggerOptions(IApiVersionDescriptionProvider provider)
        {
            this.provider = provider;
        }

        public void Configure(SwaggerGenOptions options)
        {
            // Adds Swagger documents for every API version discovered
            foreach (var description in provider.ApiVersionDescriptions)
            {
                options.SwaggerDoc(description.GroupName, CreateVersionInfo(description));
            }
        }

        public void Configure(string name, SwaggerGenOptions options)
        {
            Configure(options);
        }

        private OpenApiInfo CreateVersionInfo(ApiVersionDescription description)
        {
            var info = new OpenApiInfo()
            {
                Title = $"Web Api Documentation [{Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT")}] {description.ApiVersion}",
                Version = description.ApiVersion.ToString()
            };

            if (description.IsDeprecated)
            {
                info.Description += " This API version has been deprecated.";
            }

            return info;
        }
    }
}
