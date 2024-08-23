using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.Authentication.Login
{
    public class LoginCommand : IRequest<Result<LoginCommandResponse>>
    {
        public required string Username { get; init; }
        public required string Password { get; init; }
    }
}