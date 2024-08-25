using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.Authentication.ResetPassword
{
    public class ResetPasswordCommand : IRequest<Result<ResetPasswordCommandResponse>>
    {
        public required string Email { get; set; }
        public required string Token { get; set; }
        public required string NewPassword { get; set; }
    }
}