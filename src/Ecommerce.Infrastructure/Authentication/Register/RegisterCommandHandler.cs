﻿using MediatR;
using Ecommerce.Application.Interfaces;
using FluentResults;

namespace Ecommerce.Infrastructure.Authentication.Register
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
            var result = await _authenticationService.RegisterUserAsync(request.UserName, request.Email, request.Password);

            if(result.IsSuccess)
            {
                await _mediator.Publish(new RegisterCommandNotification(request.UserName, request.Email, result.Value), cancellationToken);

                var response = new RegisterCommandResponse
                {
                    Message = "Registration successful! Please check your email to confirm your account and complete the registration process."
                };

                return Result.Ok(response);
            }

            return Result.Fail(result.Errors);
        }
    }
}