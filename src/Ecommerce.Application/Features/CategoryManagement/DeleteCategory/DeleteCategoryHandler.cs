using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Application.Features.ProductManagement.DeleteProduct;

namespace Ecommerce.Application.Features.CategoryManagement.DeleteCategory
{
    public class DeleteCategoryHandler : IRequestHandler<DeleteCategoryCommand, Result<DeleteCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<DeleteCategoryResponse>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category == null)
            {
                return Result.Fail<DeleteCategoryResponse>($"Category with ID {request.Id} not found.");
            }

            var isDeleted = await _categoryRepository.DeleteAsync(category);

            if (isDeleted)
            {
                return Result.Ok(new DeleteCategoryResponse
                {
                    Id = request.Id,
                    IsDeleted = true
                });
            }

            return Result.Fail<DeleteCategoryResponse>($"Failed to delete category with ID {request.Id}");
        }
    }
}