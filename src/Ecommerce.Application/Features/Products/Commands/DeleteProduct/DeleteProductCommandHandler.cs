using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Products.Commands.DeleteProduct
{
    public class DeleteProductCommandHandler : IRequestHandler<DeleteProductCommand, Result<DeleteProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductCommandHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<DeleteProductResponse>> Handle(DeleteProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                return Result.Fail<DeleteProductResponse>($"Product with ID {request.ProductId} not found.");
            }

            var isDeleted = await _productRepository.DeleteAsync(product);

            if (isDeleted)
            {
                return Result.Ok(new DeleteProductResponse
                {
                    ProductId = request.ProductId,
                    IsDeleted = true
                });
            }

            return Result.Fail<DeleteProductResponse>($"Failed to delete product  with ID {request.ProductId}");
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