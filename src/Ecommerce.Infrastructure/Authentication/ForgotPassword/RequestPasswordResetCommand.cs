using FluentResults;
using MediatR;

namespace Ecommerce.Infrastructure.Authentication.ForgotPassword
{
    public class RequestPasswordResetCommand : IRequest<Result<RequestPasswordResetCommandResponse>>
    {
        public required string Email { get; set; }
    }
}