﻿using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.Authentication.ChangePassword
{
    public class ChangePasswordCommand : IRequest<Result<ChangePasswordCommandResponse>>
    {
        public required string Username { get; init; }
        public required string CurrentPassword { get; init; }
        public required string NewPassword { get; init; }
    }
}