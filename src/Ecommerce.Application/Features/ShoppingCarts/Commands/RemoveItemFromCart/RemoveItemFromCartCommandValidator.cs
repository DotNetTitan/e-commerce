using FluentValidation;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.RemoveItemFromCart
{
    public class RemoveItemFromCartCommandValidator : AbstractValidator<RemoveItemFromCartCommand>
    {
        public RemoveItemFromCartCommandValidator()
        {
            RuleFor(p => p.CustomerId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();

            RuleFor(p => p.ProductId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}