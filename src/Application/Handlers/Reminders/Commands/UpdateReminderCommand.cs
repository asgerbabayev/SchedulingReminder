using System.Text.Json.Serialization;

namespace ShedulingReminders.Application.Handlers.Reminders.Commands;

/// <summary>
/// Represents a command for updating a reminder.
/// </summary>
public class UpdateReminderCommand : IRequest<IDataResult<UpdateReminderCommand>>
{
    public Guid Id { get; set; }
    public string To { get; set; } = null!;       // Recipient of the reminder
    public string Content { get; set; } = null!;  // Content of the reminder
    public DateTime SendAt { get; set; }          // Date and time to send the reminder
    public string Method { get; set; } = null!;   // Method of sending the reminder
    [JsonIgnore]
    public string? JobId { get; set; }

    /// <summary>
    /// Represents the handler for the UpdateReminderCommand.
    /// </summary>
    public class UpdateReminderCommandHandler : IRequestHandler<UpdateReminderCommand, IDataResult<UpdateReminderCommand>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IEmailService _emailService;
        private readonly ITelegramService _telegramService;
        private readonly ICurrentUserService _currentUserService;

        #region Constructor
        public UpdateReminderCommandHandler(
            IApplicationDbContext context,
            IEmailService emailService,
            ITelegramService telegramService,
            ICurrentUserService currentUserService)
        {
            _context = context;
            _emailService = emailService;
            _telegramService = telegramService;
            _currentUserService = currentUserService;
        }
        #endregion

        /// <summary>
        /// Handles the UpdateReminderCommand and updates the corresponding reminder in the database.
        /// </summary>
        /// <param name="request">The UpdateReminderCommand to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An IDataResult containing the updated UpdateReminderCommand and the result of the operation.</returns>
        public async Task<IDataResult<UpdateReminderCommand>> Handle(UpdateReminderCommand request, CancellationToken cancellationToken)
        {
            // Retrieve the reminder to update from the database based on the provided ID
            Reminder? reminderToUpdate = await _context.Reminders.
                FirstOrDefaultAsync(x =>
                x.AppUser.UserName == _currentUserService.UserName && x.Id == request.Id);

            if (reminderToUpdate is null)
            {
                // Return an error result if the reminder is not found
                return new ErrorDataResult<UpdateReminderCommand>(Messages.ReminderNotFound);
            }
            if (!request.Method.IsValidMethod())
            {
                // If the method specified in the request is not valid, return an error result with a specific message.
                return new ErrorDataResult<UpdateReminderCommand>(Messages.InvalidMethod);
            }

            if (!request.To.IsValidTelegramId() && !request.To.IsValidEmail())
            {
                // If the recipient specified in the request is neither a valid Telegram ID nor a valid email address, return an error result with a specific message.
                return new ErrorDataResult<UpdateReminderCommand>(Messages.InvalidTo);
            }

            if (request.SendAt <= DateTime.UtcNow)
            {
                // If the specified send date is in the past or equal to the current UTC time, return an error result with a specific message.
                return new ErrorDataResult<UpdateReminderCommand>(Messages.InvalidDate);
            }

            if (reminderToUpdate.SendAt != request.SendAt)
            {
                BackgroundJob.Reschedule(reminderToUpdate.JobId,
                enqueueAt: request.SendAt);
            }

            if (reminderToUpdate.Method != request.Method)
            {
                switch (Enum.Parse(typeof(Methods), request.Method.GetNormalizedValue()))
                {
                    // If the method is Telegram, schedule a background job to send the Telegram message using the TelegramService.
                    case Methods.telegram:
                        BackgroundJob.Delete(reminderToUpdate.JobId);
                        request.JobId = BackgroundJob.Schedule(() =>
                        _telegramService.SendMessage(request.To, request.Content),
                        enqueueAt: request.SendAt);
                        break;

                    // If the method is Email, schedule a background job to send the email using the EmailService.
                    case Methods.email:
                        BackgroundJob.Delete(reminderToUpdate.JobId);
                        request.JobId = BackgroundJob.Schedule(() =>
                        _emailService.SendEmail(request.To, request.Content),
                        enqueueAt: request.SendAt);
                        break;
                }
            }

            // Map the properties from the request to the existing reminder
            reminderToUpdate.Id = request.Id;
            reminderToUpdate.To = request.To;
            reminderToUpdate.Content = request.Content;
            reminderToUpdate.Method = request.Method;
            reminderToUpdate.SendAt = request.SendAt;

            // Update the reminder in the database
            _context.Reminders.Update(reminderToUpdate);

            await _context.SaveChangesAsync(cancellationToken);

            // Return a successful result indicating the reminder has been updated
            return new SuccessDataResult<UpdateReminderCommand>(request, Messages.ReminderUpdated);
        }
    }
}

