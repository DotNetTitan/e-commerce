using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.CategoryManagement.UpdateCategory
{
    public class UpdateCategoryHandler : IRequestHandler<UpdateCategoryCommand, Result<UpdateCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<UpdateCategoryResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
            {
                return Result.Fail<UpdateCategoryResponse>($"Category with ID {request.CategoryId} not found.");
            }

            category.Name = request.Name;
            category.Description = request.Description;

            var updatedCategory = await _categoryRepository.UpdateAsync(category);

            if (updatedCategory != null)
            {
                return Result.Ok(new UpdateCategoryResponse
                {
                    Id = updatedCategory.CategoryId,
                    Name = updatedCategory.Name,
                    Description = updatedCategory.Description
                });
            }

            return Result.Fail<UpdateCategoryResponse>("Failed to update category");
        }
    }
}