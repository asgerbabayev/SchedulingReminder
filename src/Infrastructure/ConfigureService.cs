namespace ShedulingReminders.Infrastructure;

/// <summary>
/// Provides extension methods for configuring infrastructure services.
/// </summary>
public static class ConfigureServices
{
    /// <summary>
    /// Adds infrastructure services to the specified <see cref="IServiceCollection"/>.
    /// </summary>
    /// <param name="services">The <see cref="IServiceCollection"/> to add the services to.</param>
    /// <param name="configuration">The <see cref="IConfiguration"/> containing the configuration settings.</param>
    /// <returns>The modified <see cref="IServiceCollection"/>.</returns>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        // Adds the AuditableEntitySaveChangesInterceptor as a scoped service
        services.AddScoped<AuditableEntitySaveChangesInterceptor>();

        // Registers the AppDbContext as a service with the specified connection string
        services.AddDbContext<AppDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // Registers IApplicationDbContext as a scoped service, using the AppDbContext implementation
        services.AddScoped<IApplicationDbContext>(provider => provider.GetRequiredService<AppDbContext>());

        // Configures identity services using AppUser and AppRole, with AppDbContext as the store
        services.AddIdentity<AppUser, AppRole>().AddEntityFrameworkStores<AppDbContext>();

        // Adds the AppDbContextInitialiser as a scoped service
        services.AddScoped<AppDbContextInitialiser>();

        // Adds DateTimeService as a transient service for providing current date and time
        services.AddTransient<IDateTime, DateTimeService>();

        // Register EmailService and TelegramService as scoped services
        services.AddScoped<IEmailService, EmailService>();
        services.AddScoped<ITelegramService, TelegramService>();


        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(option =>
        {
            option.TokenValidationParameters = new TokenValidationParameters
            {
                ValidAudience = configuration.GetSection("Jwt:Audience").Value,
                ValidIssuer = configuration.GetSection("Jwt:Issuer").Value,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration.GetSection("Jwt:SecurityKey").Value)),
                ClockSkew = TimeSpan.Zero,
                ValidateIssuerSigningKey = true,
            };
        });

        return services;
    }
}
