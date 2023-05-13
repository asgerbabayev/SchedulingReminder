namespace ShedulingReminders.Domain.Identity;

/// <summary>
/// Represents an application user entity that extends the `IdentityUser` class.
/// </summary>
public class AppUser : IdentityUser
{
    /// <summary>
    /// The Telegram ID associated with the user.
    /// </summary>
    public int? TelegramId { get; set; }

    /// <summary>
    /// Collection of reminders associated with the user.
    /// </summary>
    public virtual ICollection<Reminder> Reminders { get; set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="AppUser"/> class.
    /// </summary>
    public AppUser() => Reminders = new HashSet<Reminder>();
}
