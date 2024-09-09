using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.InventoryManagement.UpdateInventory
{
    public class UpdateInventoryHandler : IRequestHandler<UpdateInventoryCommand, Result<UpdateInventoryResponse>>
    {
        private readonly IProductRepository _productRepository;

        public UpdateInventoryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<UpdateInventoryResponse>> Handle(UpdateInventoryCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                return Result.Fail<UpdateInventoryResponse>($"Product with ID {request.ProductId} not found.");
            }

            try
            {
                if (request.NewQuantity > product.StockQuantity)
                {
                    product.IncreaseStock(request.NewQuantity - product.StockQuantity);
                }
                else if (request.NewQuantity < product.StockQuantity)
                {
                    product.DecreaseStock(product.StockQuantity - request.NewQuantity);
                }

                product.LowStockThreshold = request.LowStockThreshold;

                await _productRepository.UpdateAsync(product);

                return Result.Ok(new UpdateInventoryResponse
                {
                    ProductId = product.ProductId,
                    NewStockQuantity = product.StockQuantity,
                    LowStockThreshold = product.LowStockThreshold
                });
            }
            catch (Exception ex)
            {
                return Result.Fail<UpdateInventoryResponse>(ex.Message);
            }
        }
    }
}