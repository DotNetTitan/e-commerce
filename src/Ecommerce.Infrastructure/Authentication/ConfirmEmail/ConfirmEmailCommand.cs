using FluentResults;
using MediatR;

namespace Ecommerce.Infrastructure.Authentication.ConfirmEmail
{
    public class ConfirmEmailCommand : IRequest<Result<ConfirmEmailCommandResponse>>
    {
        public required string Email { get; init; }
        public required string Token { get; init; }
    }
}
