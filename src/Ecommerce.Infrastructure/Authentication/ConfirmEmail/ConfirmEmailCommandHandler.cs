using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Infrastructure.Authentication.ConfirmEmail
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, Result<ConfirmEmailCommandResponse>>
    {
        private readonly IAuthenticationService _authenticationService;

        public ConfirmEmailCommandHandler(IAuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        public async Task<Result<ConfirmEmailCommandResponse>> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.ConfirmEmailAsync(request.Email, request.Token);

            if (result.IsSuccess)
            {
                var response = new ConfirmEmailCommandResponse
                {
                    Message = "Email confirmation successful."
                };

                return Result.Ok(response);
            }

            return Result.Fail<ConfirmEmailCommandResponse>(result.Errors);
        }
    }
}