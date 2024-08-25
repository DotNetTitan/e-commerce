using MediatR;

namespace Ecommerce.Application.Features.Authentication.ForgotPassword
{
    public class RequestPasswordResetNotification : INotification
    {
        public string Username { get; }
        public string Email { get; }
        public string PasswordResetToken { get; }

        public RequestPasswordResetNotification(string username, string email, string passwordResetToken)
        {
            Username = username;
            Email = email;
            PasswordResetToken = passwordResetToken;
        }
    }
}
