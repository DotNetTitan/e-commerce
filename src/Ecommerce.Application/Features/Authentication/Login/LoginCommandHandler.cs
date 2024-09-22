using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Exceptions;
using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.Authentication.Login
{
    public class LoginCommandHandler : IRequestHandler<LoginCommand, Result<LoginCommandResponse>>
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationService _authenticationService;

        public LoginCommandHandler(ITokenService tokenService, IAuthenticationService authenticationService)
        {
            _tokenService = tokenService;
            _authenticationService = authenticationService;
        }

        public async Task<Result<LoginCommandResponse>> Handle(LoginCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.LoginUserAsync(request.UserName, request.Password);

            if (result.IsSuccess)
            {
                var user = await _authenticationService.GetUserByUsernameAsync(request.UserName) ?? throw UserNotFoundException.FromUserName(request.UserName);

                var tokens = await _tokenService.GenerateTokensAsync(user.UserName, user.Id);

                return Result.Ok(new LoginCommandResponse
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken
                });
            }

            return Result.Fail(result.Errors);
        }
    }
}