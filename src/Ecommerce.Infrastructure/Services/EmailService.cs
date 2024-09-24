using Azure.Communication.Email;
using Azure;
using Ecommerce.Infrastructure.Settings;
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

        /// <inheritdoc />
        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var emailMessage = new EmailMessage(
                senderAddress: _senderAddress,
                recipients: new EmailRecipients(new List<EmailAddress> { new EmailAddress(to) }),
                content: new EmailContent(subject)
                {
                    Html = body
                }
            );

            await _emailClient.SendAsync(WaitUntil.Started, emailMessage);
        }

        /// <inheritdoc />
        public async Task SendOrderConfirmationEmail(Guid orderId, string email)
        {
            var subject = "Order Confirmation";
            var body = $@"
                <html>
                <body>
                    <p>Dear Customer,</p>
                    <p>Thank you for your order. Your order ID is {orderId}.</p>
                    <p>We will notify you once your order is shipped.</p>
                    <p>Best regards,<br/>The Ecommerce Team</p>
                </body>
                </html>";

            // Assuming you have a method to get the customer's email by their ID
            await SendEmailAsync(email, subject, body);
        }

        /// <inheritdoc />
        public async Task SendOrderCancellationEmail(Guid orderId, string email)
        {
            var subject = "Order Cancellation";
            var body = $@"
                <html>
                <body>
                    <p>Dear Customer,</p>
                    <p>Your order with ID {orderId} has been cancelled.</p>
                    <p>If you have any questions, please contact our support team.</p>
                    <p>Best regards,<br/>The Ecommerce Team</p>
                </body>
                </html>";

            // Assuming you have a method to get the customer's email by their ID
            await SendEmailAsync(email, subject, body);
        }
    }
}