using ShedulingReminders.Infrastructure.Persistance.Configurations.Common;

namespace ShedulingReminders.Infrastructure.Persistance.Configurations;

/// <summary>
/// Represents the entity type configuration for the `Reminder` entity.
/// </summary>
public class ReminderConfiguration : IEntityTypeConfiguration<Reminder>
{
    /// <summary>
    /// Configures the entity type properties and relationships for the `Reminder` entity.
    /// </summary>
    /// <param name="builder">The entity type builder used to configure the entity type.</param>
    public void Configure(EntityTypeBuilder<Reminder> builder)
    {
        builder.ConfigureAuditableBaseEntity();
        builder.Property(t => t.To).IsRequired();
        builder.Property(t => t.Content).IsRequired();
        builder.Property(t => t.SendAt).IsRequired();
        builder.Property(t => t.Method).IsRequired();
    }
}
