using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Categories.Queries.GetCategory
{
    public class GetCategoryQueryHandler : IRequestHandler<GetCategoryQuery, Result<GetCategoryQueryResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryQueryHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<GetCategoryQueryResponse>> Handle(GetCategoryQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.CategoryId);

            if (category == null)
            {
                return Result.Fail<GetCategoryQueryResponse>($"Category with ID {request.CategoryId} not found.");
            }

            return Result.Ok(new GetCategoryQueryResponse
            {
                Id = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            });
        }
    }

    public class GetCategoryQuery : IRequest<Result<GetCategoryQueryResponse>>
    {
        public required Guid CategoryId { get; init; }
    }

    public class GetCategoryQueryResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}