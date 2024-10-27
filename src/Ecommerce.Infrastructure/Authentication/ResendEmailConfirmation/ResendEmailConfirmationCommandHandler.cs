using Ecommerce.Application.Interfaces;
using FluentResults;
using MediatR;

namespace Ecommerce.Infrastructure.Authentication.ResendEmailConfirmation
{
    public class ResendEmailConfirmationCommandHandler : IRequestHandler<ResendEmailConfirmationCommand, Result<ResendEmailConfirmationCommandResponse>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMediator _mediator;

        public ResendEmailConfirmationCommandHandler(IAuthenticationService authenticationService, IMediator mediator)
        {
            _authenticationService = authenticationService;
            _mediator = mediator;
        }

        public async Task<Result<ResendEmailConfirmationCommandResponse>> Handle(ResendEmailConfirmationCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.GetEmailConfirmationTokenAsync(request.Email);

            if (result.IsSuccess)
            {
                await _mediator.Publish(new ResendEmailConfirmationCommandNotification(request.Email, result.Value), cancellationToken);

                var response = new ResendEmailConfirmationCommandResponse
                {
                    Message = "Email confirmation link sent successfully."
                };

                return Result.Ok(response);
            }

            return Result.Fail<ResendEmailConfirmationCommandResponse>(result.Errors);
        }
    }
}