using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Ecommerce.Infrastructure.Authentication.ResendEmailConfirmation
{
    public class ResendEmailConfirmationCommandNotificationHandler : INotificationHandler<ResendEmailConfirmationCommandNotification>
    {
        private readonly IEmailService _emailService;
        private readonly string _resendConfirmationEmailUrl;

        public ResendEmailConfirmationCommandNotificationHandler(IEmailService emailService, IOptions<AppSettings> options)
        {
            _emailService = emailService;
            _resendConfirmationEmailUrl = options.Value.ResendConfirmationEmailUrl;
        }

        public async Task Handle(ResendEmailConfirmationCommandNotification notification, CancellationToken cancellationToken)
        {
            const string emailSubject = "Action Required: Confirm Your Email Address";
            var emailBody = $@"
                <html>
                <body>
                    <p>Dear {notification.Email},</p>
                    <p>Thank you for registering with us. To complete your registration, please confirm your email address by clicking the link below:</p>
                    <p><a href='{_resendConfirmationEmailUrl}?email={notification.Email}&token={notification.Token}'>Confirm Email Address</a></p>
                    <p>If you did not request this email, please ignore it.</p>
                    <p>Best regards,<br/>The Ecommerce Team</p>
                </body>
                </html>";

            await _emailService.SendEmailAsync(notification.Email, emailSubject, emailBody);
        }
    }
}