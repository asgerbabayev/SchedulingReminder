namespace ShedulingReminders.Application.Handlers.Reminders.Commands;

/// <summary>
/// Represents a command for deleting a reminder.
/// </summary>
public class DeleteRemindersCommand : IRequest<IResult>
{
    public List<Guid> ReminderIds { get; set; } = null!;

    /// <summary>
    /// Represents the handler for the DeleteRemindersCommand.
    /// </summary>
    public class DeleteRemindersCommandHandler : IRequestHandler<DeleteRemindersCommand, IResult>
    {
        private readonly IApplicationDbContext _context;

        public DeleteRemindersCommandHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Handles the DeleteRemindersCommand and deletes the corresponding reminders from the database.
        /// </summary>
        /// <param name="request">The DeleteRemindersCommand to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An IResult indicating the result of the operation.</returns>
        public async Task<IResult> Handle(DeleteRemindersCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the reminders to delete from the database based on the provided IDs
            List<Reminder> remindersToDelete = await _context.Reminders
                .Where(x => request.ReminderIds.Contains(x.Id))
                .ToListAsync();

            if (!remindersToDelete.Any())
            {
                // Return an error result if no reminders are found
                return new ErrorResult(Messages.ReminderNotFound);
            }

            remindersToDelete.ForEach(x => BackgroundJob.Delete(x.JobId));

            // Remove the reminders from the database
            _context.Reminders.RemoveRange(remindersToDelete);
            await _context.SaveChangesAsync(cancellationToken);

            // Return a successful result indicating the reminders have been deleted
            return new Result(true, Messages.RemindersDeleted);
        }
    }
}

