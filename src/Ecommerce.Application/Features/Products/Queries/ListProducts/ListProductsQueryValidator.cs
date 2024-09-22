using FluentValidation;

namespace Ecommerce.Application.Features.Products.Queries.ListProducts
{
    public class ListProductsQueryValidator : AbstractValidator<ListProductsQuery>
    {
        public ListProductsQueryValidator()
        {
            RuleFor(x => x.PageNumber).GreaterThan(0).WithMessage("Page number must be greater than 0.");
            RuleFor(x => x.PageSize).InclusiveBetween(1, 100).WithMessage("Page size must be between 1 and 100.");
            When(x => x.SearchTerm != null, () => RuleFor(x => x.SearchTerm).MinimumLength(2).WithMessage("Search term must be at least 2 characters long."));
            When(x => x.CategoryId.HasValue, () => RuleFor(x => x.CategoryId!.Value).NotEmpty().WithMessage("Category ID must not be empty."));
        }
    }
}