namespace ShedulingReminders.Infrastructure.Persistance;

/// <summary>
/// Represents the application's database context.
/// </summary>
public class AppDbContext : IdentityDbContext<AppUser>, IApplicationDbContext
{
    private readonly IMediator _mediator;
    private readonly AuditableEntitySaveChangesInterceptor _interceptor;

    /// <summary>
    /// Initializes a new instance of the <see cref="AppDbContext"/> class.
    /// </summary>
    /// <param name="options">The options for configuring the database context.</param>
    /// <param name="interceptor">The interceptor for automatically updating auditable entities.</param>
    /// <param name="mediator">The mediator for handling events and commands.</param>
    public AppDbContext(
                        DbContextOptions<AppDbContext> options,
                        AuditableEntitySaveChangesInterceptor interceptor,
                        IMediator mediator)
                        : base(options)
    {
        _interceptor = interceptor;
        _mediator = mediator;
    }

    /// <summary>
    /// Gets or sets the reminders entity set.
    /// </summary>
    public DbSet<Reminder> Reminders => Set<Reminder>();

    /// <summary>
    /// Configures the model for the database context.
    /// </summary>
    /// <param name="builder">The model builder instance to be configured.</param>
    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        base.OnModelCreating(builder);
    }

    /// <summary>
    /// Configures the options for the database context.
    /// </summary>
    /// <param name="optionsBuilder">The options builder used to configure the options.</param>
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.AddInterceptors(_interceptor);
    }

    /// <summary>
    /// Saves all changes made in this context to the database asynchronously.
    /// </summary>
    /// <param name="cancellationToken">A <see cref="CancellationToken"/> to observe while waiting for the task to complete.</param>
    /// <returns>A task representing the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
