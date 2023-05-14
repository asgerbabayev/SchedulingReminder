using ShedulingReminders.Application.Common.Security.Jwt;

namespace ShedulingReminders.Application.Handlers.Authorizations.Commands;

public record LoginUserCommand(string Email, string Password) : IRequest<IDataResult<AccessToken>>
{
    public class LoginUserCommandHandler : IRequestHandler<LoginUserCommand, IDataResult<AccessToken>>
    {
        private readonly ITokenHelper _tokenHelper;
        private readonly UserManager<AppUser> _userManager;
        public LoginUserCommandHandler(UserManager<AppUser> userManager,
            ITokenHelper tokenHelper)
        {
            _userManager = userManager;
            _tokenHelper = tokenHelper;
        }
        public async Task<IDataResult<AccessToken>> Handle(LoginUserCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);
            if (user is null)
            {
                return new ErrorDataResult<AccessToken>(Messages.UserNotFound);
            }

            var result = await _userManager.CheckPasswordAsync(user, request.Password);
            if (!result) return new ErrorDataResult<AccessToken>(Messages.InvalidCredentials);

            var accessToken = _tokenHelper.CreateToken<AccessToken>(user);

            return new SuccessDataResult<AccessToken>(accessToken, Messages.LoginSuccessfully);
        }
    }
}
