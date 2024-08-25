using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Settings;
using MediatR;
using Microsoft.Extensions.Options;

namespace Ecommerce.Application.Features.Authentication.ForgotPassword
{
    public class RequestPasswordResetNotificationHandler : INotificationHandler<RequestPasswordResetNotification>
    {
        private readonly IEmailService _emailService;
        private readonly string _passwordResetUrl;

        public RequestPasswordResetNotificationHandler(IEmailService emailService, IOptions<AppSettings> options)
        {
            _emailService = emailService;
            _passwordResetUrl = options.Value.PasswordResetUrl;
        }

        public async Task Handle(RequestPasswordResetNotification notification, CancellationToken cancellationToken)
        {
            const string emailSubject = "Password Reset Request";

            var emailBody = $@"
                <html>
                <body>
                    <p>Dear {notification.Username},</p>
                    <p>We received a request to reset your password. Please click the link below to reset your password:</p>
                    <p><a href='{_passwordResetUrl}?email={notification.Email}&token={notification.PasswordResetToken}'>Reset Password</a></p>
                    <p>If you did not request a password reset, please ignore this email or contact our support team.</p>
                    <br>
                    <p>Best regards,</p>
                    <p>The Ecommerce Team</p>
                    <p><small>For your security, this link will expire in 24 hours.</small></p>
                </body>
                </html>";

            await _emailService.SendEmailAsync(notification.Email, emailSubject, emailBody);
        }
    }
}