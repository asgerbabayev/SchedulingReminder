namespace ShedulingReminders.Infrastructure.Persistance.Interceptors;

/// <summary>
/// Represents an interceptor for automatically updating auditable entities during save changes operation.
/// </summary>
public class AuditableEntitySaveChangesInterceptor : SaveChangesInterceptor
{
    private readonly ICurrentUserService _currentUserService;
    private readonly IDateTime _dateTime;

    /// <summary>
    /// Initializes a new instance of the <see cref="AuditableEntitySaveChangesInterceptor"/> class.
    /// </summary>
    /// <param name="currentUserService">The current user service used to retrieve the current user's information.</param>
    /// <param name="dateTime">The date time service used to retrieve the current date and time.</param>
    public AuditableEntitySaveChangesInterceptor(
        IDateTime dateTime,
        ICurrentUserService currentUserService
        )
    {
        _dateTime = dateTime;
        _currentUserService = currentUserService;
    }

    /// <summary>
    /// Intercepts the saving changes operation before it is applied to the database.
    /// </summary>
    /// <param name="eventData">The event data for the saving changes operation.</param>
    /// <param name="result">The interception result for the saving changes operation.</param>
    /// <returns>The interception result.</returns>
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChanges(eventData, result);
    }

    /// <summary>
    /// Intercepts the saving changes operation before it is applied to the database asynchronously.
    /// </summary>
    /// <param name="eventData">The event data for the saving changes operation.</param>
    /// <param name="result">The interception result for the saving changes operation.</param>
    /// <param name="cancellationToken">The cancellation token for the asynchronous operation.</param>
    /// <returns>A task representing the interception result.</returns>
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData, InterceptionResult<int> result, CancellationToken cancellationToken = default)
    {
        UpdateEntities(eventData.Context);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    /// <summary>
    /// Updates the auditable entities with the current user and date/time information.
    /// </summary>
    /// <param name="context">The database context.</param>
    public void UpdateEntities(DbContext? context)
    {
        if (context == null) return;

        foreach (var entry in context.ChangeTracker.Entries<BaseAuditableEntity>())
        {
            if (entry.State == EntityState.Added)
            {
                entry.Entity.CreatedBy = _currentUserService.UserName != null ? _currentUserService.UserName : "User";
                entry.Entity.CreatedDate = _dateTime.Now;
            }

            if (entry.State == EntityState.Added || entry.State == EntityState.Modified)
            {
                entry.Entity.ModifiedBy = _currentUserService.UserName != null ? _currentUserService.UserName : "User";
                entry.Entity.ModifiedDate = _dateTime.Now;
            }
        }
    }
}
