using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<UpdateProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAzureBlobStorageService _blobStorageService;

        public UpdateProductCommandHandler(IProductRepository productRepository, IAzureBlobStorageService blobStorageService)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
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

            if (request.Thumbnail != null)
            {
                if (!string.IsNullOrEmpty(existingProduct.ThumbnailUrl))
                {
                    await _blobStorageService.DeleteFileAsync(existingProduct.ThumbnailUrl);
                }
                existingProduct.ThumbnailUrl = await _blobStorageService.UploadFileAsync(request.Thumbnail);
            }

            if (request.Images != null && request.Images.Any())
            {
                foreach (var imageUrl in existingProduct.ImageUrls)
                {
                    await _blobStorageService.DeleteFileAsync(imageUrl);
                }
                existingProduct.ImageUrls = await _blobStorageService.UploadFilesAsync(request.Images);
            }

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
                    SKU = updatedProduct.SKU,
                    Description = updatedProduct.Description,
                    Price = updatedProduct.Price,
                    StockQuantity = updatedProduct.StockQuantity,
                    CategoryId = updatedProduct.CategoryId,
                    ThumbnailUrl = updatedProduct.ThumbnailUrl,
                    ImageUrls = updatedProduct.ImageUrls
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
        public IFormFile? Thumbnail { get; init; }
        public List<IFormFile>? Images { get; init; }
    }

    public class UpdateProductResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required string SKU { get; init; }
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }
        public required Guid CategoryId { get; set; }
        public string? ThumbnailUrl { get; init; }
        public List<string> ImageUrls { get; init; } = new List<string>();
    }
}