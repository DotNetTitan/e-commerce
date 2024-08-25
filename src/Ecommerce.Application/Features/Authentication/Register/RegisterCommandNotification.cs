using MediatR;

namespace Ecommerce.Application.Features.Authentication.Register
{
    public class RegisterCommandNotification : INotification
    {
        public string Username { get; }
        public string Email { get; }
        public string EmailConfirmationToken { get; }

        public RegisterCommandNotification(string username, string email, string emailConfirmationToken)
        {
            Username = username;
            Email = email;
            EmailConfirmationToken = emailConfirmationToken;
        }
    }
}