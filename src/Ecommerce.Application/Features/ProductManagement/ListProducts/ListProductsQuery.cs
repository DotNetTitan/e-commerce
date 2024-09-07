using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.ProductManagement.ListProducts
{
    public class ListProductsQuery : IRequest<Result<ListProductsResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
    }
}