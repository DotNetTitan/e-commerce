using MediatR;
using Ecommerce.Application.Interfaces;
using FluentResults;
using Ecommerce.Application.Features.Authentication.Notifications.UserRegistered;

namespace Ecommerce.Application.Features.Authentication.Register
{
    public class RegisterCommandHandler : IRequestHandler<RegisterCommand, Result<RegisterCommandResponse>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMediator _mediator;

        public RegisterCommandHandler(IAuthenticationService authenticationService, IMediator mediator)
        {
            _authenticationService = authenticationService;
            _mediator = mediator;
        }

        public async Task<Result<RegisterCommandResponse>> Handle(RegisterCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.RegisterUserAsync(request.Username, request.Email, request.Password);

            if(result.IsSuccess)
            {
                await _mediator.Publish(new UserRegisteredNotification(request.Username, request.Email, result.Value), cancellationToken);

                return Result.Ok();
            }

            return Result.Fail(result.Errors);
        }
    }
}