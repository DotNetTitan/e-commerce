using FluentValidation;

namespace Ecommerce.Application.Features.Authentication.ChangePassword
{
    public class ChangePasswordCommandValidator : AbstractValidator<ChangePasswordCommand>
    {
        public ChangePasswordCommandValidator()
        {
            RuleFor(p => p.Username)
                .NotEmpty().WithMessage("{Username} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{Username} must not exceed 50 characters.");

            RuleFor(p => p.CurrentPassword)
                .NotEmpty().WithMessage("{CurrentPassword} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{CurrentPassword} must not exceed 50 characters.");

            RuleFor(p => p.NewPassword)
                .NotEmpty().WithMessage("{NewPassword} is required.")
                .NotNull()
                .MaximumLength(50).WithMessage("{NewPassword} must not exceed 50 characters.");
        }
    }
}