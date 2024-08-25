using Ecommerce.Application.Interfaces;
using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.Authentication.ResetPassword
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, Result<ResetPasswordCommandResponse>>
    {

        private readonly IAuthenticationService _authenticationService;

        public ResetPasswordCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result<ResetPasswordCommandResponse>> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ResetPasswordAsync(request.Email, request.Token, request.NewPassword);

            if (result.IsSuccess)
            {
                return Result.Ok(new ResetPasswordCommandResponse { Message = "Password reset successful." });
            }

            return Result.Fail<ResetPasswordCommandResponse>(result.Errors);
        }
    }
}