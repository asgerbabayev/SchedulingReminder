namespace ShedulingReminders.Infrastructure.Services;

/// <summary>
/// Service for sending emails.
/// </summary>
public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    /// <summary>
    /// Sends an email to the specified recipient with the given content.
    /// </summary>
    /// <param name="to">Recipient's email address.</param>
    /// <param name="content">Content of the email.</param>
    public void SendEmail(string to, string content)
    {
        var apiKey = _configuration["SendinBlue:ApiKey"];
        Configuration.Default.ApiKey.Add("api-key", apiKey);
        var apiInstance = new TransactionalEmailsApi();

        SendSmtpEmailSender email = new SendSmtpEmailSender(
            _configuration["SendinBlue:User"],
            _configuration["SendinBlue:Email"]
        );


        SendSmtpEmailTo smtpEmailTo = new SendSmtpEmailTo(to, "User");
        List<SendSmtpEmailTo> tos = new List<SendSmtpEmailTo>() { smtpEmailTo };

        var sendSmtpEmail = new SendSmtpEmail(email, to: tos, textContent: content, subject: "Reminder");

        apiInstance.SendTransacEmail(sendSmtpEmail);
    }
}
