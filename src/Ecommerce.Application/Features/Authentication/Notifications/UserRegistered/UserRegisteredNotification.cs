using MediatR;

namespace Ecommerce.Application.Features.Authentication.Notifications.UserRegistered
{
    public class UserRegisteredNotification : INotification
    {
        public string Username { get; }
        public string Email { get; }
        public string EmailConfirmationToken { get; }

        public UserRegisteredNotification(string username, string email, string emailConfirmationToken)
        {
            Username = username;
            Email = email;
            EmailConfirmationToken = emailConfirmationToken;
        }
    }
}