using FluentValidation;

namespace Ecommerce.Application.Features.Authentication.ConfirmEmail
{
    public class ConfirmEmailCommandValidator : AbstractValidator<ConfirmEmailCommand>
    {
        public ConfirmEmailCommandValidator()
        {
            RuleFor(x => x.Email).NotEmpty().WithMessage("Email is required.");
            RuleFor(x => x.Token).NotEmpty().WithMessage("Token is required.");
        }
    }
}