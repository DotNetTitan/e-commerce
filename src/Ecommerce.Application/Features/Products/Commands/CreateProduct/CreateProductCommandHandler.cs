using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Microsoft.AspNetCore.Http;

namespace Ecommerce.Application.Features.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Result<CreateProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAzureBlobStorageService _blobStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public CreateProductCommandHandler(IProductRepository productRepository, IAzureBlobStorageService blobStorageService, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<CreateProductResponse>> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            // Check if a product with the same name already exists
            var existingProduct = await _productRepository.GetByNameAsync(request.Name);
            if (existingProduct != null)
            {
                return Result.Fail<CreateProductResponse>($"A product with the name '{request.Name}' already exists.");
            }

            var product = new Product
            {
                Name = request.Name,
                Description = request.Description,
                Price = request.Price,
                StockQuantity = request.StockQuantity,
                CategoryId = request.CategoryId,
                LowStockThreshold = request.LowStockThreshold
            };

            string? thumbnailUrl = null;
            List<string> imageUrls = new List<string>();

            try
            {
                if (request.Thumbnail != null)
                {
                    thumbnailUrl = await _blobStorageService.UploadFileAsync(request.Thumbnail);
                    product.ThumbnailUrl = thumbnailUrl;
                }

                if (request.Images != null && request.Images.Any())
                {
                    imageUrls = await _blobStorageService.UploadFilesAsync(request.Images);
                    product.Images = imageUrls.Select(url => new ProductImage { ImageUrl = url }).ToList();
                }

                await _unitOfWork.BeginTransactionAsync();
                var createdProduct = await _productRepository.CreateAsync(product);
                await _unitOfWork.CommitAsync();

                if (createdProduct != null)
                {
                    return Result.Ok(new CreateProductResponse
                    {
                        Id = createdProduct.ProductId,
                        Name = createdProduct.Name,
                        Sku = createdProduct.Sku,
                        Description = createdProduct.Description,
                        Price = createdProduct.Price,
                        StockQuantity = createdProduct.StockQuantity,
                        CategoryId = createdProduct.CategoryId,
                        ThumbnailUrl = createdProduct.ThumbnailUrl,
                        ImageUrls = createdProduct.Images.Select(i => i.ImageUrl).ToList()
                    });
                }
            }
            catch (Exception)
            {
                await _unitOfWork.RollbackAsync();

                // Delete uploaded images from blob storage
                if (thumbnailUrl != null)
                {
                    await _blobStorageService.DeleteFileAsync(thumbnailUrl);
                }

                foreach (var imageUrl in imageUrls)
                {
                    _blobStorageService.DeleteFileAsync(imageUrl);
                }

                throw;
            }

            return Result.Fail<CreateProductResponse>("Failed to create product");
        }
    }

    public class CreateProductCommand : IRequest<Result<CreateProductResponse>>
    {
        public required string Name { get; init; }
        public required string Description { get; init; }
        public required decimal Price { get; init; }
        public required int StockQuantity { get; init; }
        public required Guid CategoryId { get; init; }
        public required int LowStockThreshold { get; init; }
        public IFormFile? Thumbnail { get; init; }
        public List<IFormFile>? Images { get; init; }
    }

    public class CreateProductResponse
    {
        public required Guid Id { get; init; }
        public required string Name { get; init; }
        public required string Sku { get; init; }
        public required string Description { get; init; }
        public required decimal Price { get; init; }
        public required int StockQuantity { get; init; }
        public required Guid CategoryId { get; init; }
        public string? ThumbnailUrl { get; init; }
        public List<string> ImageUrls { get; init; } = new List<string>();
    }
}
