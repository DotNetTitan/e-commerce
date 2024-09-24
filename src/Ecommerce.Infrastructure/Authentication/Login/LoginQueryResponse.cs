namespace Ecommerce.Infrastructure.Authentication.Login
{
    public class LoginQueryResponse
    {
        public string? AccessToken { get; set; }
        public string? RefreshToken { get; set; }
    }
}