using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.Categories.Commands.CreateCategory
{
    public class CreateCategoryCommandHandler : IRequestHandler<CreateCategoryCommand, Result<CreateCategoryCommandResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryCommandHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<CreateCategoryCommandResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            // Check if a category with the same name already exists
            var existingCategory = await _categoryRepository.GetByNameAsync(request.Name);
            if (existingCategory != null)
            {
                return Result.Fail<CreateCategoryCommandResponse>($"A category with the name '{request.Name}' already exists.");
            }

            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            var createdCategory = await _categoryRepository.CreateAsync(category);

            if (createdCategory != null)
            {
                return Result.Ok(new CreateCategoryCommandResponse
                {
                    CategoryId = createdCategory.CategoryId,
                    Name = createdCategory.Name,
                    Description = createdCategory.Description
                });
            }

            return Result.Fail<CreateCategoryCommandResponse>("Failed to create category");
        }
    }

    public class CreateCategoryCommand : IRequest<Result<CreateCategoryCommandResponse>>
    {
        public required string Name { get; init; }
        public string? Description { get; init; }
    }

    public class CreateCategoryCommandResponse
    {
        public required Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}