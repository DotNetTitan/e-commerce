using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Inventory.Commands.UpdateInventory
{
    public class UpdateInventoryCommandHandler : IRequestHandler<UpdateInventoryCommand, Result<UpdateInventoryResponse>>
    {
        private readonly IProductRepository _productRepository;

        public UpdateInventoryCommandHandler(IProductRepository productRepository)
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

    public class UpdateInventoryCommand : IRequest<Result<UpdateInventoryResponse>>
    {
        public required Guid ProductId { get; init; }
        public required int NewQuantity { get; init; }
        public required int LowStockThreshold { get; init; }
    }

    public class UpdateInventoryResponse
    {
        public required Guid ProductId { get; init; }
        public required int NewStockQuantity { get; init; }
        public required int LowStockThreshold { get; init; }
    }
}