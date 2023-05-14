using ShedulingReminders.Application.Common.Security.Jwt;

namespace ShedulingReminders.Application;

/// <summary>
/// Provides extension methods for configuring application services.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Adds application services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Configure Hangfire with SQL Server storage
        services.AddHangfire(x => x.UseSqlServerStorage(configuration.GetConnectionString("DefaultConnection")));

        // Add Hangfire server
        services.AddHangfireServer();

        // Adds MediatR for handling commands and queries
        services.AddMediatR(Assembly.GetExecutingAssembly());

        // Adds FluentValidation for automatic validation using attributes
        services.AddFluentValidationAutoValidation();

        // Adds AutoMapper for object mapping
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        // Adds validators from the executing assembly
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());

        services.AddScoped<ITokenHelper, JwtHelper>();

        return services;
    }

}

