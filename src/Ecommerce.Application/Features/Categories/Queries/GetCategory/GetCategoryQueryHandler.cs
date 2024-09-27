using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Categories.Queries.GetCategory
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Result<GetCategoryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<GetCategoryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
            {
                return Result.Fail<GetCategoryResponse>($"Category with ID {request.CategoryId} not found.");
            }

            return Result.Ok(new GetCategoryResponse
            {
                CategoryId = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            });
        }
    }

    public class GetCategoryQuery : IRequest<Result<GetCategoryResponse>>
    {
        public required Guid CategoryId { get; init; }
    }

    public class GetCategoryResponse
    {
        public required Guid CategoryId { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}