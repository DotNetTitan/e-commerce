using Ecommerce.Application.Interfaces;
using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.Authentication.ForgotPassword
{
    public class RequestPasswordResetCommandHandler : IRequestHandler<RequestPasswordResetCommand, Result<RequestPasswordResetCommandResponse>>
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly IMediator _mediator;

        public RequestPasswordResetCommandHandler(IAuthenticationService authenticationService, IMediator mediator)
        {
            _authenticationService = authenticationService;
            _mediator = mediator;
        }

        public async Task<Result<RequestPasswordResetCommandResponse>> Handle(RequestPasswordResetCommand request, CancellationToken cancellationToken)
        {
            var result = await _authenticationService.GetPasswordResetTokenAsync(request.Email);

            if (result.IsSuccess)
            {
                await _mediator.Publish(new RequestPasswordResetNotification(request.Email, request.Email, result.Value), cancellationToken);

                var response = new RequestPasswordResetCommandResponse
                {
                    Message = "A password reset link has been sent to your email address. Please check your inbox."
                };

                return Result.Ok(response);
            }

            return Result.Fail(result.Errors);
        }
    }
}