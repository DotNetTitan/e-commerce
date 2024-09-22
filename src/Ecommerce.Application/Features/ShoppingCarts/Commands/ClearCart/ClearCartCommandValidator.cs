using FluentValidation;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.ClearCart
{
    public class ClearCartCommandValidator : AbstractValidator<ClearCartCommand>
    {
        public ClearCartCommandValidator()
        {
            RuleFor(p => p.CustomerId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}