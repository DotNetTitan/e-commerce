using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.CategoryManagement.GetCategory
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
}