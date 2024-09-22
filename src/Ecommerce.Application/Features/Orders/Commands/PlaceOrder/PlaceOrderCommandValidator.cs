using FluentValidation;

namespace Ecommerce.Application.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderCommandValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required");
            RuleFor(x => x.TotalAmount).GreaterThan(0).WithMessage("Total amount must be greater than zero");
            RuleFor(x => x.Items).NotEmpty().WithMessage("Order must contain at least one item");
            RuleForEach(x => x.Items).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required");
                item.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
                item.RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than zero");
            });
        }
    }
}