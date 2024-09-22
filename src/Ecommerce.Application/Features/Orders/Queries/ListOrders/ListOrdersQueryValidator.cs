using FluentValidation;

namespace Ecommerce.Application.Features.Orders.Queries.ListOrders
{
    public class ListOrdersQueryValidator : AbstractValidator<ListOrdersQuery>
    {
        public ListOrdersQueryValidator()
        {
            RuleFor(x => x.CustomerId).NotEmpty().WithMessage("Customer ID is required.");
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0.");
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
        }
    }
}