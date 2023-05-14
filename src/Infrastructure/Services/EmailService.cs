using System.Net;
using System.Net.Mail;

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
        var smtpClient = new SmtpClient("smtp-relay.sendinblue.com")
        {
            Port = 587,
            Credentials = new NetworkCredential("asgerbabayev@hotmail.com", "QaDqfOymIR1B6nHW"),
            EnableSsl = true,
        };

        smtpClient.Send(_configuration["SendinBlue:Email"], to, "Reminder", content);
    }
}
