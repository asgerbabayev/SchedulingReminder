using Microsoft.AspNetCore.Http;

namespace ShedulingReminders.Application.Handlers.Reminders.Commands;

/// <summary>
/// Represents a command to create a reminder.
/// </summary>
public record CreateReminderCommand(string To, string Content, DateTime SendAt, string Method)
    : IRequest<IDataResult<CreateReminderCommand>>
{
    /// <summary>
    /// Handler for the CreateReminderCommand.
    /// </summary>
    public class CreateReminderHandler : IRequestHandler<CreateReminderCommand, IDataResult<CreateReminderCommand>>
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly IApplicationDbContext _context;
        private readonly IHttpContextAccessor _accessor;
        private readonly IMapper _mapper;
        private readonly IEmailService _emailService;
        private readonly ITelegramService _telegramService;

        #region Constructor
        public CreateReminderHandler(
            IApplicationDbContext context,
            IMapper mapper,
            IEmailService emailService,
            ITelegramService telegramService,
            IHttpContextAccessor accessor,
            UserManager<AppUser> userManager)
        {
            _context = context;
            _mapper = mapper;
            _emailService = emailService;
            _telegramService = telegramService;
            _accessor = accessor;
            _userManager = userManager;
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
            bool isAuthenticated = _accessor.HttpContext.User.Identity.IsAuthenticated;
            string username = _accessor.HttpContext.User.Identity.Name;
            if (isAuthenticated)
            {
                var user = await _userManager.FindByNameAsync(username);
                reminder.AppUser = user;
            }

            switch (Enum.Parse(typeof(Methods), request.Method.GetNormalizedValue()))
            {
                // If the method is Telegram, schedule a background job to send the Telegram message using the TelegramService.
                case Methods.telegram when request.To.IsValidTelegramId():
                    reminder.JobId = BackgroundJob.Schedule(() =>
                    _telegramService.SendMessage(request.To, request.Content),
                    enqueueAt: request.SendAt);
                    break;

                // If the method is Email, schedule a background job to send the email using the EmailService.
                case Methods.email when request.To.IsValidEmail():
                    reminder.JobId = BackgroundJob.Schedule(() =>
                    _emailService.SendEmail(request.To, request.Content),
                    enqueueAt: request.SendAt);
                    break;
                default:
                    return new ErrorDataResult<CreateReminderCommand>(Messages.InvalidTo);
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

