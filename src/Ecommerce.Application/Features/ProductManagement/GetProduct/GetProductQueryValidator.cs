using FluentValidation;

namespace Ecommerce.Application.Features.ProductManagement.GetProduct
{
    public class GetProductQueryValidator : AbstractValidator<GetProductQuery>
    {
        public GetProductQueryValidator()
        {
            RuleFor(x => x.ProductId)
           .NotEmpty().WithMessage("ProductId is required.");
        }
    }
}