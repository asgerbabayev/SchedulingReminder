using ShedulingReminders.Application.Common.Security.Jwt;

public interface ITokenHelper
{
    TAccessToken CreateToken<TAccessToken>(AppUser user)
      where TAccessToken : IAccessToken, new();
}