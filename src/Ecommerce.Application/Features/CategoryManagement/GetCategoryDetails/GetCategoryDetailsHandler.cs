using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.CategoryManagement.GetCategoryDetails
{
    public class GetCategoryDetailsHandler : IRequestHandler<GetCategoryDetailsQuery, Result<GetCategoryDetailsResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public GetCategoryDetailsHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<GetCategoryDetailsResponse>> Handle(GetCategoryDetailsQuery request, CancellationToken cancellationToken)
        {
            var category = await _categoryRepository.GetByIdAsync(request.Id);

            if (category == null)
            {
                return Result.Fail<GetCategoryDetailsResponse>($"Category with ID {request.Id} not found.");
            }

            return Result.Ok(new GetCategoryDetailsResponse
            {
                Id = category.CategoryId,
                Name = category.Name,
                Description = category.Description
            });
        }
    }
}