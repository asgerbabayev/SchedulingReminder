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
        builder.Property(t => t.Content).IsRequired();

        builder.HasOne(b => b.AppUser)
            .WithMany(c => c.Reminders)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
