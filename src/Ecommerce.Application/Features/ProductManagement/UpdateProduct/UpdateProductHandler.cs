using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ProductManagement.UpdateProduct
{
    public class UpdateProductHandler : IRequestHandler<UpdateProductCommand, Result<UpdateProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<UpdateProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetByIdAsync(request.ProductId);

            if (existingProduct == null)
            {
                return Result.Fail<UpdateProductResponse>($"Product with ID {request.ProductId} not found.");
            }

            // Update basic properties
            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.Price = request.Price;
            existingProduct.CategoryId = request.CategoryId;

            // Handle stock quantity update
            int stockDifference = request.StockQuantity - existingProduct.StockQuantity;
            if (stockDifference != 0)
            {
                try
                {
                    if (stockDifference > 0)
                    {
                        existingProduct.IncreaseStock(stockDifference);
                    }
                    else
                    {
                        existingProduct.DecreaseStock(-stockDifference);
                    }
                }
                catch (ArgumentException ex)
                {
                    return Result.Fail<UpdateProductResponse>(ex.Message);
                }
                catch (InvalidOperationException ex)
                {
                    return Result.Fail<UpdateProductResponse>(ex.Message);
                }
            }

            var updatedProduct = await _productRepository.UpdateAsync(existingProduct);

            if (updatedProduct != null)
            {
                return Result.Ok(new UpdateProductResponse
                {
                    Id = updatedProduct.ProductId,
                    Name = updatedProduct.Name,
                    Description = updatedProduct.Description,
                    Price = updatedProduct.Price,
                    StockQuantity = updatedProduct.StockQuantity,
                    CategoryId = updatedProduct.CategoryId
                });
            }

            return Result.Fail<UpdateProductResponse>("Failed to update product");
        }
    }
}