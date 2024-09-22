using FluentValidation;

namespace Ecommerce.Application.Features.Inventory.Commands.UpdateInventory
{
    public class UpdateInventoryCommandValidator : AbstractValidator<UpdateInventoryCommand>
    {
        public UpdateInventoryCommandValidator()
        {
            RuleFor(x => x.ProductId).NotEmpty().WithMessage("Product ID must not be empty.");
            RuleFor(x => x.NewQuantity).GreaterThan(0).WithMessage("New quantity must be greater than 0.");
            RuleFor(x => x.LowStockThreshold).GreaterThan(0).WithMessage("Low stock threshold must be greater than 0.");
        }
    }
}