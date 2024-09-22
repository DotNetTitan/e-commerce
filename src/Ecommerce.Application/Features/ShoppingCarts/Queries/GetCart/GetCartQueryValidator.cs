using FluentValidation;

namespace Ecommerce.Application.Features.ShoppingCarts.Queries.GetCart
{
    public class GetCartQueryValidator : AbstractValidator<GetCartQuery>
    {
        public GetCartQueryValidator()
        {
            RuleFor(p => p.CustomerId)
                .NotEmpty().WithMessage("{PropertyName} is required.")
                .NotNull();
        }
    }
}