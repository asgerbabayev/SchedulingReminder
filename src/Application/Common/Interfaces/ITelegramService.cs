namespace ShedulingReminders.Application.Common.Interfaces;

public interface ITelegramService
{
    void SendMessage(string to, string content);
}
