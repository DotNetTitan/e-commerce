using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ProductManagement.DeleteProduct
{
    public class DeleteProductHandler : IRequestHandler<DeleteProductCommand, Result<DeleteProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public DeleteProductHandler(IProductRepository productRepository)
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

            var isDeleted = await _productRepository.DeleteAsync(request.ProductId);

            if (isDeleted)
            {
                return Result.Ok(new DeleteProductResponse
                {
                    ProductId = request.ProductId,
                    IsDeleted = true
                });
            }

            return Result.Fail<DeleteProductResponse>("Failed to delete product");
        }
    }
}