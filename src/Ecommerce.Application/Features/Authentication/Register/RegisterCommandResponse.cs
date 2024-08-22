namespace Ecommerce.Application.Features.Authentication.Register
{
    public class RegisterCommandResponse
    {
        public bool Succeeded { get; set; }
        public IEnumerable<string>? Errors { get; set; }
    }
}