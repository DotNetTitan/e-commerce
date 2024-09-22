using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Categories.Commands.DeleteCategory
{
    public class DeleteCategoryCommandHandler : IRequestHandler<DeleteCategoryCommand, Result<DeleteCategoryCommandResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public DeleteCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<DeleteCategoryCommandResponse>> Handle(DeleteCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
            {
                return Result.Fail<DeleteCategoryCommandResponse>($"Category with ID {request.CategoryId} not found.");
            }

            var isDeleted = await _categoryRepository.DeleteAsync(category);

            if (isDeleted)
            {
                return Result.Ok(new DeleteCategoryCommandResponse
                {
                    CategoryId = request.CategoryId,
                    IsDeleted = true
                });
            }

            return Result.Fail<DeleteCategoryCommandResponse>($"Failed to delete category with ID {request.CategoryId}");
        }
    }

    public class DeleteCategoryCommand : IRequest<Result<DeleteCategoryCommandResponse>>
    {
        public required Guid CategoryId { get; init; }
    }

    public class DeleteCategoryCommandResponse
    {
        public required Guid CategoryId { get; set; }
        public required bool IsDeleted { get; set; }
    }
}