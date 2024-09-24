using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Exceptions;
using FluentResults;
using MediatR;

namespace Ecommerce.Infrastructure.Authentication.Login
{
    public class LoginQueryHandler : IRequestHandler<LoginQuery, Result<LoginQueryResponse>>
    {
        private readonly ITokenService _tokenService;
        private readonly IAuthenticationService _authenticationService;

        public LoginQueryHandler(ITokenService tokenService, IAuthenticationService authenticationService)
        {
            _tokenService = tokenService;
            _authenticationService = authenticationService;
        }

        public async Task<Result<LoginQueryResponse>> Handle(LoginQuery request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.LoginUserAsync(request.UserName, request.Password);

            if (result.IsSuccess)
            {
                var user = await _authenticationService.GetUserByUsernameAsync(request.UserName) ?? throw UserNotFoundException.FromUserName(request.UserName);

                var tokens = await _tokenService.GenerateTokensAsync(user.UserName!, user.Id);

                return Result.Ok(new LoginQueryResponse
                {
                    AccessToken = tokens.AccessToken,
                    RefreshToken = tokens.RefreshToken
                });
            }

            return Result.Fail(result.Errors);
        }
    }
}