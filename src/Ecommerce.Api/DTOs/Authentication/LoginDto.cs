namespace Ecommerce.Api.DTOs.Authentication
{
    public class LoginDto
    {
        public required string UserName { get; init; }
        public required string Password { get; init; }
    }
}