using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ProductManagement.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<GetProductQueryResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<GetProductQueryResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                return Result.Fail<GetProductQueryResponse>($"Product with ID {request.ProductId} not found.");
            }

            var response = new GetProductQueryResponse
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