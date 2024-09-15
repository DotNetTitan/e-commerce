using FluentValidation;

namespace Ecommerce.Application.Features.OrderManagement.PlaceOrder
{
    public class PlaceOrderCommandValidator : AbstractValidator<PlaceOrderCommand>
    {
        public PlaceOrderCommandValidator()
        {
            RuleFor(x => x.OrderDetails.CustomerId).NotEmpty().WithMessage("Customer ID is required");
            RuleFor(x => x.OrderDetails.TotalAmount).GreaterThan(0).WithMessage("Total amount must be greater than zero");
            RuleFor(x => x.OrderDetails.OrderItems).NotEmpty().WithMessage("Order must contain at least one item");
            RuleForEach(x => x.OrderDetails.OrderItems).ChildRules(item =>
            {
                item.RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID is required");
                item.RuleFor(x => x.Quantity).GreaterThan(0).WithMessage("Quantity must be greater than zero");
                item.RuleFor(x => x.UnitPrice).GreaterThan(0).WithMessage("Unit price must be greater than zero");
            });
        }
    }
}