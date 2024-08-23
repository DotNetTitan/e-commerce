using MediatR;
using Ecommerce.Application.Interfaces;
using Microsoft.Extensions.Options;
using Ecommerce.Domain.Settings;

namespace Ecommerce.Application.Features.Authentication.Notifications.UserRegistered
{
    public class UserRegisteredNotificationHandler : INotificationHandler<UserRegisteredNotification>
    {
        private readonly IEmailService _emailService;
        private readonly string _confirmationEmailUrl;

        public UserRegisteredNotificationHandler(IEmailService emailService, IOptions<AppSettings> appSettings)
        {
            _emailService = emailService;
            _confirmationEmailUrl = appSettings.Value.ConfirmationEmailUrl;
        }

        public async Task Handle(UserRegisteredNotification notification, CancellationToken cancellationToken)
        {
            var emailSubject = "Confirm your registration";
            var emailBody = $"Hi {notification.Username}, please confirm your registration by clicking the following link: " +
                            $"{_confirmationEmailUrl}?token={notification.EmailConfirmationToken}&email={notification.Email}";

            await _emailService.SendEmailAsync(notification.Email, emailSubject, emailBody);
        }
    }
}