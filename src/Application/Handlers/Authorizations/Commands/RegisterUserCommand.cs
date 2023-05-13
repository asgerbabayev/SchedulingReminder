namespace ShedulingReminders.Application.Handlers.Authorizations.Commands;

public class RegisterUserCommand : IRequest<IResult>
{
    public int? TelegramId { get; set; }
    public string Email { get; set; } = null!;
    public string Password { get; set; } = null!;

}
