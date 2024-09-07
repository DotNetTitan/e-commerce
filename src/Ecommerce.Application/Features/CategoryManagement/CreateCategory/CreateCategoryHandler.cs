using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.CategoryManagement.CreateCategory
{
    public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Result<CreateCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public CreateCategoryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<CreateCategoryResponse>> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
        {
            var category = new Category
            {
                Name = request.Name,
                Description = request.Description
            };

            var createdCategory = await _categoryRepository.CreateAsync(category);

            if (createdCategory != null)
            {
                return Result.Ok(new CreateCategoryResponse
                {
                    Id = createdCategory.CategoryId,
                    Name = createdCategory.Name,
                    Description = createdCategory.Description
                });
            }

            return Result.Fail<CreateCategoryResponse>("Failed to create category");
        }
    }
}