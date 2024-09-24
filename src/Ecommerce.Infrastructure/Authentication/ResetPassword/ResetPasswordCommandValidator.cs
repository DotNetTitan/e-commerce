using FluentValidation;

namespace Ecommerce.Infrastructure.Authentication.ResetPassword
{
    public class ResetPasswordCommandValidator : AbstractValidator<ResetPasswordCommand>
    {
        public ResetPasswordCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.");

            RuleFor(p => p.Token)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull()
                .MinimumLength(6).WithMessage("{PropertyName} must not be less than 6 characters.");
        }
    }
}