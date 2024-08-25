using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.Authentication.ResendEmailConfirmation
{
    public class ResendEmailConfirmationCommand : IRequest<Result<ResendEmailConfirmationCommandResponse>>
    {
        public required string Email { get; set; }
    }
}