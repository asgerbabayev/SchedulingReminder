namespace ShedulingReminders.Infrastructure.Persistance;

/// <summary>
/// Represents a class for initializing the application's database.
/// </summary>
public class AppDbContextInitialiser
{
    private readonly AppDbContext _dbContext;
    private readonly ILogger<AppDbContextInitialiser> _logger;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContextInitialiser"/> class.
    /// </summary>
    /// <param name="dbContext">The application's database context.</param>
    /// <param name="logger">The logger for logging initialization errors.</param>
    public AppDbContextInitialiser(AppDbContext dbContext,
                                   ILogger<AppDbContextInitialiser> logger)
    {
        _dbContext = dbContext;
        _logger = logger;
    }

    /// <summary>
    /// Initializes the application's database asynchronously.
    /// </summary>
    /// <returns>A task representing the asynchronous initialization operation.</returns>
    public async System.Threading.Tasks.Task InitializeAsync()
    {
        try
        {
            if (_dbContext.Database.IsSqlServer())
            {
                await _dbContext.Database.MigrateAsync();
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An error occurred while initializing the database.");
            throw;
        }
    }
}

