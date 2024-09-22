using FluentValidation;

namespace Ecommerce.Application.Features.ShoppingCarts.Commands.AddItemToCart
{
    public class AddItemToCartCommandValidator : AbstractValidator<AddItemToCartCommand>
    {
        public AddItemToCartCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required.");
            RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero.");
        }
    }
}