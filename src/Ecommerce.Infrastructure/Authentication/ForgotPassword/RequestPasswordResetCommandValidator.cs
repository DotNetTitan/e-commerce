using FluentValidation;

namespace Ecommerce.Infrastructure.Authentication.ForgotPassword
{
    public class RequestPasswordResetCommandValidator : AbstractValidator<RequestPasswordResetCommand>
    {
        public RequestPasswordResetCommandValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Email is required.")
                .EmailAddress().WithMessage("A valid email is required.");
        }
    }
}