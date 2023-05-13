using ShedulingReminders.Application.Common.Extensions;

namespace ShedulingReminders.Application.Handlers.Reminders.Commands;

/// <summary>
/// Represents a command for creating a reminder.
/// </summary>
public class CreateReminderCommand : IRequest<IDataResult<CreateReminderCommand>>
{
    public string To { get; set; } = null!;       // Recipient of the reminder
    public string Content { get; set; } = null!;  // Content of the reminder
    public DateTime SendAt { get; set; }          // Date and time to send the reminder
    public string Method { get; set; } = null!;   // Method of sending the reminder

    /// <summary>
    /// Handler for the CreateReminderCommand.
    /// </summary>
    public class CreateReminderHandler : IRequestHandler<CreateReminderCommand, IDataResult<CreateReminderCommand>>
    {
        private readonly IApplicationDbContext _context;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ITelegramService _telegramService;

        #region Constructor
        public CreateReminderHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IEmailService emailService,
            ITelegramService telegramService)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _telegramService = telegramService;
        }
        #endregion

        /// <summary>
        /// Handles the CreateReminderCommand.
        /// </summary>
        /// <param name="request">The CreateReminderCommand to handle.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>An instance of IResult indicating the result of the command handling.</returns>
        public async Task<IDataResult<CreateReminderCommand>> Handle(CreateReminderCommand request, CancellationToken cancellationToken)
        {
            if (!request.Method.IsValidMethod())
            {
                // If the method specified in the request is not valid, return an error result with a specific message.
                return new ErrorDataResult<CreateReminderCommand>(Messages.InvalidMethod);
            }

            if (!request.To.IsValidTelegramId() && !request.To.IsValidEmail())
            {
                // If the recipient specified in the request is neither a valid Telegram ID nor a valid email address, return an error result with a specific message.
                return new ErrorDataResult<CreateReminderCommand>(Messages.InvalidTo);
            }

            if (request.SendAt <= DateTime.UtcNow)
            {
                // If the specified send date is in the past or equal to the current UTC time, return an error result with a specific message.
                return new ErrorDataResult<CreateReminderCommand>(Messages.InvalidDate);
            }

            // Map the CreateReminderCommand to a Reminder entity.
            Reminder reminder = _mapper.Map<Reminder>(request);

            switch (Enum.Parse(typeof(Methods), request.Method.GetNormalizedValue()))
            {
                // If the method is Telegram, schedule a background job to send the Telegram message using the TelegramService.
                case Methods.telegram:
                    reminder.JobId = BackgroundJob.Schedule(() =>
                    _telegramService.SendMessage(request.To, request.Content),
                    enqueueAt: request.SendAt);
                    break;

                // If the method is Email, schedule a background job to send the email using the EmailService.
                case Methods.email:
                    reminder.JobId = BackgroundJob.Schedule(() =>
                    _emailService.SendEmail(request.To, request.Content),
                    enqueueAt: request.SendAt);
                    break;
            }

            // Add the reminder to the DbContext.
            await _context.Reminders.AddAsync(reminder);

            // Save the changes to the database.
            await _context.SaveChangesAsync(cancellationToken);

            // Return a success result with the processed CreateReminderCommand and a success message.
            return new SuccessDataResult<CreateReminderCommand>(request, Messages.Added);
        }
    }
}

