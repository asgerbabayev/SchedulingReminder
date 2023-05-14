namespace ShedulingReminders.Domain.Identity;

/// <summary>
/// Represents an application user entity that extends the `IdentityUser` class.
/// </summary>
public class AppUser : IdentityUser
{
    public string FullName { get; set; } = null!;
    public virtual ICollection<Reminder> Reminders { get; set; }
    public AppUser() => Reminders = new HashSet<Reminder>();
}
