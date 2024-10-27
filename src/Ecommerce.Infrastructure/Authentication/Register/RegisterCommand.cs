using FluentResults;
using MediatR;

namespace Ecommerce.Infrastructure.Authentication.Register
{
    public class RegisterCommand : IRequest<Result<RegisterCommandResponse>>
    {
        public required string UserName { get; set; }
        public required string Email { get; set; }
        public required string Password { get; set; }
    }
}