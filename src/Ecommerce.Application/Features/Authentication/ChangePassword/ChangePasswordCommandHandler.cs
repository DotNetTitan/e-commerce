using Ecommerce.Application.Interfaces;
using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.Authentication.ChangePassword
{
    public class ChangePasswordCommandHandler : IRequestHandler<ChangePasswordCommand, Result<ChangePasswordCommandResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        public ChangePasswordCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result<ChangePasswordCommandResponse>> Handle(ChangePasswordCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ChangePasswordAsync(request.Username, request.CurrentPassword, request.NewPassword);

            if (result.IsSuccess)
            {
                var response = new ChangePasswordCommandResponse
                {
                    Message = "Password changed successfully."
                };

                return Result.Ok(response);
            }

            return Result.Fail<ChangePasswordCommandResponse>(result.Errors);
        }
    }
}