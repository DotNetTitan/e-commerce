using Azure.Communication.Email;
using Azure;
using Ecommerce.Domain.Settings;
using Microsoft.Extensions.Options;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Infrastructure.Services
{
    /// <summary>
    /// Represents a class for sending emails.
    /// </summary>
    public class EmailService : IEmailService
    {
        private readonly EmailClient _emailClient;
        private readonly string _senderAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailService"/> class.
        /// </summary>
        /// <param name="emailClient">The email client.</param>
        /// <param name="appSettings">The application settings.</param>
        public EmailService(EmailClient emailClient, IOptions<AppSettings> appSettings)
        {
            _emailClient = emailClient;
            _senderAddress = appSettings.Value.EmailSenderAddress;
        }

        /// <summary>
        /// Sends an email asynchronously.
        /// </summary>
        /// <param name="to">The recipient email address.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = new EmailMessage(
                senderAddress: _senderAddress,
                recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress(to) }),
                content: new EmailContent(subject)
                {
                    PlainText = body
                }
            );

            await _emailClient.SendAsync(WaitUntil.Started, emailMessage);
        }
    }
}