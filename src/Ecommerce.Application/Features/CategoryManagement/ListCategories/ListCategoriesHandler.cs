using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.CategoryManagement.ListCategories
{
    public class ListCategoriesHandler : IRequestHandler<ListCategoriesQuery, Result<ListCategoriesResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public ListCategoriesHandler(ICategoryRepository categoryRepository)
        {
            _categoryRepository = categoryRepository;
        }

        public async Task<Result<ListCategoriesResponse>> Handle(ListCategoriesQuery request, CancellationToken cancellationToken)
        {
            var (categories, totalCount) = await _categoryRepository.GetAllAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm
            );

            var categoryDetails = categories.ConvertAll(c => new CategoryDetails
            {
                Id = c.CategoryId,
                Name = c.Name,
                Description = c.Description
            });

            var response = new ListCategoriesResponse
            {
                Categories = categoryDetails,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return Result.Ok(response);
        }
    }
}