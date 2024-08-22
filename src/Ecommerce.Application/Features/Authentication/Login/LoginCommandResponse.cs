namespace Ecommerce.Application.Features.Authentication.Login
{
    public class LoginCommandResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}