using FluentValidation;

namespace Ecommerce.Application.Features.CategoryManagement.CreateCategory
{
    public class CreateCategoryCommandValidator : AbstractValidator<CreateCategoryCommand>
    {
        public CreateCategoryCommandValidator()
        {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Category name is required.")
                .MaximumLength(100).WithMessage("Category name must not exceed 100 characters.");

            RuleFor(x => x.Description)
                .MaximumLength(500).WithMessage("Category description must not exceed 500 characters.")
                .When(x => !string.IsNullOrEmpty(x.Description));
        }
    }
}