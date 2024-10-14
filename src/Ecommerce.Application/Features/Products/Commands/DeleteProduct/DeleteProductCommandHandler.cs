using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<DeleteProductResponse>>
    {
        private readonly IProductRepository _productRepository;
        private readonly IAzureBlobStorageService _blobStorageService;
        private readonly IUnitOfWork _unitOfWork;

        public DeleteProductCommandHandler(IProductRepository productRepository, IAzureBlobStorageService blobStorageService, IUnitOfWork unitOfWork)
        {
            _productRepository = productRepository;
            _blobStorageService = blobStorageService;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<DeleteProductResponse>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                return Result.Fail<DeleteProductResponse>($"Product with ID {request.ProductId} not found.");
            }

            try
            {
                await _unitOfWork.BeginTransactionAsync();

                var isDeleted = await _productRepository.DeleteAsync(product);

                if (isDeleted)
                {
                    // Delete thumbnail if exists
                    if (!string.IsNullOrEmpty(product.ThumbnailUrl))
                    {
                        await _blobStorageService.DeleteFileAsync(product.ThumbnailUrl);
                    }

                    // Delete all product images
                    foreach (var image in product.Images)
                    {
                        await _blobStorageService.DeleteFileAsync(image.ImageUrl);
                    }

                    await _unitOfWork.CommitAsync();

                    return Result.Ok(new DeleteProductResponse
                    {
                        ProductId = request.ProductId,
                        IsDeleted = true
                    });
                }

                await _unitOfWork.RollbackAsync();
                return Result.Fail<DeleteProductResponse>($"Failed to delete product with ID {request.ProductId}");
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail<DeleteProductResponse>($"An error occurred while deleting the product: {ex.Message}");
            }
        }
    }

    public class DeleteProductCommand : IRequest<Result<DeleteProductResponse>>
    {
        public required Guid ProductId { get; init; }
    }

    public class DeleteProductResponse
    {
        public required Guid ProductId { get; set; }
        public required bool IsDeleted { get; set; }
    }
}
