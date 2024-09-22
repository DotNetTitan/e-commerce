using FluentValidation;

namespace Ecommerce.Application.Features.CategoryManagement.DeleteCategory
{
    public class DeleteCategoryCommandValidator : AbstractValidator<DeleteCategoryCommand>
    {
        public DeleteCategoryCommandValidator()
        {
            RuleFor(x => x.CategoryId)
                .NotEmpty().WithMessage("Category ID is required.");
        }
    }
}