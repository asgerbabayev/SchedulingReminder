namespace ShedulingReminders.Application.Common.Interfaces;

public interface IEmailService
{
    void SendEmail(string to, string content);
}