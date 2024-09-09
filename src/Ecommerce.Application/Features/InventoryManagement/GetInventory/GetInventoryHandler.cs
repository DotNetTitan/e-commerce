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
            var products = await _productRepository.GetAllAsync();

            var inventoryItems = products.Select(p => new InventoryItem
            {
                ProductId = p.ProductId,
                ProductName = p.Name,
                StockQuantity = p.StockQuantity,
                LowStockThreshold = p.LowStockThreshold
            }).ToList();

            return Result.Ok(new GetInventoryResponse
            {
                InventoryItems = inventoryItems
            });
        }
    }
}