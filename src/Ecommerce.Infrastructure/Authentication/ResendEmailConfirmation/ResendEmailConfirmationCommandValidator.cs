﻿using FluentValidation;

namespace Ecommerce.Infrastructure.Authentication.ResendEmailConfirmation
{
    public class ResendEmailConfirmationCommandValidator : AbstractValidator<ResendEmailConfirmationCommand>
    {
        public ResendEmailConfirmationCommandValidator()
        {
            RuleFor(p => p.Email)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .EmailAddress().WithMessage("{PropertyName} is not a valid email address.");
        }
    }
}