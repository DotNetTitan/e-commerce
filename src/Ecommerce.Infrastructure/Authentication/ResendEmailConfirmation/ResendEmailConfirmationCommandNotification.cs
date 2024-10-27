using MediatR;

namespace Ecommerce.Infrastructure.Authentication.ResendEmailConfirmation
{
    public class ResendEmailConfirmationCommandNotification : INotification
    {
        public string Email { get; set; }
        public string Token { get; set; }

        public ResendEmailConfirmationCommandNotification(string email, string token)
        {
            Email = email;
            Token = token;
        }
    }
}