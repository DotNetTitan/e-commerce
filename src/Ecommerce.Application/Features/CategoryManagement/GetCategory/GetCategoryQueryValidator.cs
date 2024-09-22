using FluentValidation;

namespace Ecommerce.Application.Features.CategoryManagement.GetCategory
{
    public class GetCategoryQueryValidator : AbstractValidator<GetCategoryQuery>
    {
        public GetCategoryQueryValidator()
        {
            RuleFor(x => x.CategoryId).NotEmpty().WithMessage("Category ID is required.");
        }
    }
}