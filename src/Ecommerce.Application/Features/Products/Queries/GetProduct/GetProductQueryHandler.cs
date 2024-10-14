using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Products.Queries.GetProduct
{
    public class GetProductQueryHandler : IRequestHandler<GetProductQuery, Result<GetProductResponse>>
    {
        private readonly IProductRepository _productRepository;

        public GetProductQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<GetProductResponse>> Handle(GetProductQuery request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.ProductId);

            if (product == null)
            {
                return Result.Fail<GetProductResponse>($"Product with ID {request.ProductId} not found.");
            }

            var response = new GetProductResponse
            {
                Id = product.ProductId,
                Name = product.Name,
                SKU = product.SKU,
                Description = product.Description,
                Price = product.Price,
                StockQuantity = product.StockQuantity,
                CategoryId = product.CategoryId,
                CategoryName = product.Category?.Name ?? "Unknown Category",
                ThumbnailUrl = product.ThumbnailUrl,
                ImageUrls = product.Images.Select(i => i.ImageUrl).ToList()
            };

            return Result.Ok(response);
        }
    }

    public class GetProductQuery : IRequest<Result<GetProductResponse>>
    {
        public required Guid ProductId { get; init; }
    }

    public class GetProductResponse
    {
        public required Guid Id { get; set; }
        public required string Name { get; set; }
        public required string SKU { get; init; }
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }
        public required Guid CategoryId { get; set; }
        public required string CategoryName { get; set; }
        public string? ThumbnailUrl { get; init; }
        public List<string> ImageUrls { get; init; } = new List<string>();
    }
}
