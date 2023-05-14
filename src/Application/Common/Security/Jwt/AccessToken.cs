namespace ShedulingReminders.Application.Common.Security.Jwt;

public class AccessToken : IAccessToken
{
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
}
