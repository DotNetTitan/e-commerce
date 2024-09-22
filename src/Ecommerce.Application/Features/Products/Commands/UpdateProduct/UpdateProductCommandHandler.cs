using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<UpdateProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public UpdateProductCommandHandler(IProductRepository productRepository)
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

            existingProduct.Name = request.Name;
            existingProduct.Description = request.Description;
            existingProduct.Price = request.Price;
            existingProduct.CategoryId = request.CategoryId;
            existingProduct.LowStockThreshold = request.LowStockThreshold;

            try
            {
                existingProduct.UpdateStock(request.StockQuantity);
            }
            catch (ArgumentException ex)
            {
                return Result.Fail<UpdateProductResponse>(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return Result.Fail<UpdateProductResponse>(ex.Message);
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

    public class UpdateProductCommand : IRequest<Result<UpdateProductResponse>>
    {
        public required Guid ProductId { get; init; }
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required decimal Price { get; init; }
        public required int StockQuantity { get; init; }
        public required Guid CategoryId { get; init; }
        public required int LowStockThreshold { get; init; }
    }

    public class UpdateProductResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }
        public required Guid CategoryId { get; set; }
    }
}