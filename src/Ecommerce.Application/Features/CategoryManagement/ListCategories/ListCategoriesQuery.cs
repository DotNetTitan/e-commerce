using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.CategoryManagement.ListCategories
{
    public class ListCategoriesQuery : IRequest<Result<ListCategoriesQueryResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
    }
}