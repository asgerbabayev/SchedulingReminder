using Telegram.Bot;
namespace ShedulingReminders.Infrastructure.Services;

public class TelegramService : ITelegramService
{
    private readonly IConfiguration _configuration;

    public TelegramService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void SendMessage(string to, string content)
    {
        TelegramBotClient client = new(_configuration["TelegramBotToken"]);
        client.SendTextMessageAsync(
        chatId: to,
        text: content,
        allowSendingWithoutReply: true).GetAwaiter();
    }
}
