namespace ShedulingReminders.Domain.Entities;

/// <summary>
/// Represents a reminder entity that includes properties for reminder details and extends the `BaseAuditableEntity` class.
/// </summary>
public class Reminder : BaseAuditableEntity
{
    /// <summary>
    /// The recipient of the reminder.
    /// </summary>
    public string To { get; set; } = null!;

    /// <summary>
    /// The content or message of the reminder.
    /// </summary>
    public string Content { get; set; } = null!;

    /// <summary>
    /// The date and time when the reminder should be sent.
    /// </summary>
    public DateTime SendAt { get; set; }

    /// <summary>
    /// The method of sending the reminder (e.g., email, telegram).
    /// </summary>
    public string Method { get; set; } = null!;

    public string? JobId { get; set; }
    public virtual AppUser? AppUser { get; set; }
}
