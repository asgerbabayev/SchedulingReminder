namespace ShedulingReminders.Application.Common.Interfaces;

/// <summary>
/// Represents the application database context interface that includes a DbSet for reminders and the SaveChangesAsync method.
/// </summary>
public interface IApplicationDbContext
{
    /// <summary>
    /// The DbSet for reminders.
    /// </summary>
    DbSet<Reminder> Reminders { get; }

    /// <summary>
    /// Asynchronously saves all changes made in this context to the database.
    /// </summary>
    /// <param name="cancellationToken">A token to cancel the save operation.</param>
    /// <returns>A task that represents the asynchronous save operation. The task result contains the number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
