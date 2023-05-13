namespace ShedulingReminders.Domain.Common;

/// <summary>
/// Base auditable entity class that extends the `BaseEntity` class and adds auditing properties.
/// </summary>
public class BaseAuditableEntity : BaseEntity
{
    /// <summary>
    /// The name of the user who created the entity.
    /// </summary>
    public string CreatedBy { get; set; } = null!;

    /// <summary>
    /// The date and time when the entity was created.
    /// </summary>
    public DateTime CreatedDate { get; set; }

    /// <summary>
    /// The name of the user who last modified the entity.
    /// </summary>
    public string? ModifiedBy { get; set; }

    /// <summary>
    /// The date and time when the entity was last modified.
    /// </summary>
    public DateTime? ModifiedDate { get; set; }
}
