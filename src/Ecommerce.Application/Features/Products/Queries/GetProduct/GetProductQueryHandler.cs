using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Products.Queries.GetProduct
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

    public class GetProductQuery : IRequest<Result<GetProductQueryResponse>>
    {
        public required Guid ProductId { get; init; }
    }

    public class GetProductQueryResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }
        public required Guid CategoryId { get; set; }
        public required string CategoryName { get; set; }
    }
}