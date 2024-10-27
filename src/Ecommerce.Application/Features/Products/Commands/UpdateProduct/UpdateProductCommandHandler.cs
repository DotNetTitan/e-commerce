using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Features.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand, Result<UpdateProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAzureBlobStorageService _blobStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateProductCommandHandler(IProductRepository productRepository, IAzureBlobStorageService blobStorageService, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateProductResponse>> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var existingProduct = await _productRepository.GetByIdAsync(request.ProductId);

            if (existingProduct == null)
            {
                return Result.Fail<UpdateProductResponse>($"Product with ID {request.ProductId} not found.");
            }

            string? newThumbnailUrl = null;
            List<string> newImageUrls = new List<string>();
            string? oldThumbnailUrl = existingProduct.ThumbnailUrl;
            var oldImageUrls = existingProduct.Images.Select(i => i.ImageUrl).ToList();

            try
            {
                existingProduct.Name = request.Name;
                existingProduct.Description = request.Description;
                existingProduct.Price = request.Price;
                existingProduct.CategoryId = request.CategoryId;
                existingProduct.LowStockThreshold = request.LowStockThreshold;

                if (request.Thumbnail != null)
                {
                    newThumbnailUrl = await _blobStorageService.UploadFileAsync(request.Thumbnail);
                    existingProduct.ThumbnailUrl = newThumbnailUrl;
                }

                if (request.Images != null && request.Images.Any())
                {
                    newImageUrls = await _blobStorageService.UploadFilesAsync(request.Images);
                    existingProduct.Images = newImageUrls.Select(url => new ProductImage { ImageUrl = url }).ToList();
                }

                existingProduct.UpdateStock(request.StockQuantity);

                await _unitOfWork.BeginTransactionAsync();
                var updatedProduct = await _productRepository.UpdateAsync(existingProduct);
                await _unitOfWork.CommitAsync();

                if (updatedProduct != null)
                {
                    // Delete old images after successful update
                    if (oldThumbnailUrl != null && oldThumbnailUrl != updatedProduct.ThumbnailUrl)
                    {
                        await _blobStorageService.DeleteFileAsync(oldThumbnailUrl);
                    }

                    foreach (var oldImageUrl in oldImageUrls)
                    {
                        if (!updatedProduct.Images.Any(i => i.ImageUrl == oldImageUrl))
                        {
                            await _blobStorageService.DeleteFileAsync(oldImageUrl);
                        }
                    }

                    return Result.Ok(new UpdateProductResponse
                    {
                        Id = updatedProduct.ProductId,
                        Name = updatedProduct.Name,
                        Sku = updatedProduct.Sku,
                        Description = updatedProduct.Description,
                        Price = updatedProduct.Price,
                        StockQuantity = updatedProduct.StockQuantity,
                        CategoryId = updatedProduct.CategoryId,
                        ThumbnailUrl = updatedProduct.ThumbnailUrl,
                        ImageUrls = updatedProduct.Images.Select(i => i.ImageUrl).ToList()
                    });
                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();

                // Delete newly uploaded images from blob storage
                if (newThumbnailUrl != null)
                {
                    await _blobStorageService.DeleteFileAsync(newThumbnailUrl);
                }

                foreach (var imageUrl in newImageUrls)
                {
                    _blobStorageService.DeleteFileAsync(imageUrl);
                }
                
                throw;
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
        public required string Sku { get; init; }
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }
        public required Guid CategoryId { get; set; }
        public string? ThumbnailUrl { get; init; }
        public List<string> ImageUrls { get; init; } = new List<string>();
    }
}
