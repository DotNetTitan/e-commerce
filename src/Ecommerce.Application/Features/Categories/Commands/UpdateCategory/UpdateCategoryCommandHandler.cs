using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Categories.Commands.UpdateCategory
{
    public class UpdateCategoryCommandHandler : IRequestHandler<UpdateCategoryCommand, Result<UpdateCategoryCommandResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public UpdateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<UpdateCategoryCommandResponse>> Handle(UpdateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
            {
                return Result.Fail<UpdateCategoryCommandResponse>($"Category with ID {request.CategoryId} not found.");
            }

            category.Name = request.Name;
            category.Description = request.Description;

            var updatedCategory = await _categoryRepository.UpdateAsync(category);

            if (updatedCategory != null)
            {
                return Result.Ok(new UpdateCategoryCommandResponse
                {
                    Id = updatedCategory.CategoryId,
                    Name = updatedCategory.Name,
                    Description = updatedCategory.Description
                });
            }

            return Result.Fail<UpdateCategoryCommandResponse>("Failed to update category");
        }
    }

    public class UpdateCategoryCommand : IRequest<Result<UpdateCategoryCommandResponse>>
    {
        public required Guid CategoryId { get; init; }
        public required string Name { get; init; }
        public string? Description { get; init; }
    }

    public class UpdateCategoryCommandResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}