using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<DeleteCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<DeleteCategoryResponse>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
            {
                return Result.Fail<DeleteCategoryResponse>($"Category with ID {request.CategoryId} not found.");
            }

            var isDeleted = await _categoryRepository.DeleteAsync(category);

            if (isDeleted)
            {
                return Result.Ok(new DeleteCategoryResponse
                {
                    CategoryId = request.CategoryId,
                    IsDeleted = true
                });
            }

            return Result.Fail<DeleteCategoryResponse>($"Failed to delete category with ID {request.CategoryId}");
        }
    }

    public class DeleteCategoryCommand : IRequest<Result<DeleteCategoryResponse>>
    {
        public required Guid CategoryId { get; init; }
    }

    public class DeleteCategoryResponse
    {
        public required Guid CategoryId { get; set; }
        public required bool IsDeleted { get; set; }
    }
}