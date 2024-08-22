using MediatR;
using Ecommerce.Application.Interfaces;
using FluentResults;

namespace Ecommerce.Application.Features.Authentication.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterCommandResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        public RegisterCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result<RegisterCommandResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.RegisterUserAsync(request.Username, request.Email, request.Password);

            if(result.IsSuccess)
            {
                return Result.Ok();
            }

            return Result.Fail(result.Errors);
        }
    }
}