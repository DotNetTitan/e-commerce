using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Inventory.Queries.GetInventory
{
    public class GetInventoryQueryHandler : IRequestHandler<GetInventoryQuery, Result<GetInventoryResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetInventoryQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<GetInventoryResponse>> Handle(GetInventoryQuery request, CancellationToken cancellationToken)
        {
            var (products, totalCount) = await _productRepository.GetProductsAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.CategoryId
            );

            var inventoryItems = products.ConvertAll(p => new InventoryItem
            {
                ProductId = p.ProductId,
                ProductName = p.Name,
                StockQuantity = p.StockQuantity,
                LowStockThreshold = p.LowStockThreshold,
                CategoryName = p.Category?.Name ?? "Unknown Category"
            });

            return Result.Ok(new GetInventoryResponse
            {
                InventoryItems = inventoryItems,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            });
        }
    }

    public class GetInventoryQuery : IRequest<Result<GetInventoryResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public bool? LowStockOnly { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class GetInventoryResponse
    {
        public required List<InventoryItem> InventoryItems { get; init; }
        public int TotalCount { get; internal set; }
        public int PageNumber { get; internal set; }
        public int PageSize { get; internal set; }
    }

    public class InventoryItem
    {
        public required Guid ProductId { get; init; }
        public required string ProductName { get; init; }
        public required int StockQuantity { get; init; }
        public required int LowStockThreshold { get; init; }
        public required string CategoryName { get; init; }
    }
}