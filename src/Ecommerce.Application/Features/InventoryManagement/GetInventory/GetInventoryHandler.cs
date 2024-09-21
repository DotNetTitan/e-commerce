using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.InventoryManagement.GetInventory
{
    public class GetInventoryHandler : IRequestHandler<GetInventoryQuery, Result<GetInventoryResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetInventoryHandler(IProductRepository productRepository)
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
}