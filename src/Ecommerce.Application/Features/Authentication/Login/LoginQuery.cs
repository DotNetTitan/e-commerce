using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.Authentication.Login
{
    public class LoginQuery : IRequest<Result<LoginQueryResponse>>
    {
        public required string UserName { get; init; }
        public required string Password { get; init; }
    }
}