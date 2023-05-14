namespace ShedulingReminders.Application.Common.Security.Jwt;

public interface IAccessToken
{
    DateTime Expiration { get; set; }
    string Token { get; set; }
}
