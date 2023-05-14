using Newtonsoft.Json;

namespace ShedulingReminders.Application.Handlers.Authorizations.Commands;

/// <summary>
/// Represents a command to register a user.
/// </summary>
public record RegisterUserCommand(string FullName, string Email, string Password) : IRequest<IResult>
{
    /// <summary>
    /// Represents the command handler for registering a user.
    /// </summary>
    public class RegisterUserCommandHandler : IRequestHandler<RegisterUserCommand, IResult>
    {
        private readonly UserManager<AppUser> _userManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserCommandHandler"/> class.
        /// </summary>
        /// <param name="userManager">The user manager for handling user-related operations.</param>
        public RegisterUserCommandHandler(UserManager<AppUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        /// Handles the registration of a user.
        /// </summary>
        /// <param name="request">The command containing user registration data.</param>
        /// <param name="cancellationToken">The cancellation token.</param>
        /// <returns>A task representing the result of the registration operation.</returns>
        public async Task<IResult> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
        {
            var isThereAnyUser = await _userManager.FindByEmailAsync(request.Email);
            if (isThereAnyUser is not null)
            {
                return new ErrorResult(Messages.EmailAlreadyExist);
            }

            AppUser newUser = new()
            {
                FullName = request.FullName,
                Email = request.Email,
                UserName = request.Email,
            };
            IdentityResult identityResult = await _userManager.CreateAsync(newUser, request.Password);
            if (!identityResult.Succeeded)
            {
                return new ErrorResult(message: JsonConvert.SerializeObject(identityResult.Errors.Select(x => x.Description)));
            }
            return new Result(true, Messages.RegisterSuccessfully);
        }
    }
}

