using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Categories.Queries.ListCategories
{
    public class ListCategoriesQueryHandler : IRequestHandler<ListCategoriesQuery, Result<ListCategoriesResponse>>
    {
        private readonly ICategoryRepository _categoryRepository;

        public ListCategoriesQueryHandler(ICategoryRepository categoryRepository)
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

    public class ListCategoriesQuery : IRequest<Result<ListCategoriesResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
    }

    public class ListCategoriesResponse
    {
        public required IReadOnlyCollection<CategoryDetails> Categories { get; set; }
        public required int TotalCount { get; set; }
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }

    public class CategoryDetails
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public string? Description { get; set; }
    }
}