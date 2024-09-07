using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.ProductManagement.GetProductDetails
{
    public class GetProductDetailsHandler : IRequestHandler<GetProductDetailsQuery, Result<GetProductDetailsResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductDetailsHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<GetProductDetailsResponse>> Handle(GetProductDetailsQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                return Result.Fail<GetProductDetailsResponse>($"Product with ID {request.ProductId} not found.");
            }

            var response = new GetProductDetailsResponse
            {
                Id = product.ProductId,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "Unknown Category"
            };

            return Result.Ok(response);
        }
    }
}