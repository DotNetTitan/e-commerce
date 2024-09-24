using MediatR;
using Ecommerce.Application.Interfaces;
using Microsoft.Extensions.Options;
using Ecommerce.Infrastructure.Settings;

namespace Ecommerce.Infrastructure.Authentication.Register
{
    public class RegisterCommandNotificationHandler : INotificationHandler<RegisterCommandNotification>
    {
        private readonly IEmailService _emailService;
        private readonly string _confirmationEmailUrl;

        public RegisterCommandNotificationHandler(IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _emailService = emailService;
            _confirmationEmailUrl = appSettings.Value.ConfirmationEmailUrl;
        }

        public async Task Handle(RegisterCommandNotification notification, CancellationToken cancellationToken)
        {
            const string emailSubject = "Confirm your registration";
            var emailBody = $@"
              <html>
              <body>
                  <p>Dear {notification.Username},</p>
                  <p>Thank you for registering with us. To complete your registration, please confirm your email address by clicking the link below:</p>
                  <p><a href='{_confirmationEmailUrl}?token={notification.EmailConfirmationToken}&email={notification.Email}'>Confirm Email</a></p>
                  <p>If you did not register for an account, please ignore this email.</p>
                  <br>
                  <p>Best regards,</p>
                  <p>The Ecommerce Team</p>
              </body>
              </html>";
            await _emailService.SendEmailAsync(notification.Email, emailSubject, emailBody);
        }
    }
}