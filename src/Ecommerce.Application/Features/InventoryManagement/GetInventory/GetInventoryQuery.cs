using FluentResults;
using MediatR;

namespace Ecommerce.Application.Features.InventoryManagement.GetInventory
{
    public class GetInventoryQuery : IRequest<Result<GetInventoryResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public bool? LowStockOnly { get; set; }
        public Guid? CategoryId { get; set; }
    }
}