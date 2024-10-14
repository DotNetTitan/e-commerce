using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Products.Queries.ListProducts
{
    public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, Result<ListProductsResponse>>
    {
        private readonly IProductRepository _productRepository;

        public ListProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ListProductsResponse>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
        {
            var (products, totalCount) = await _productRepository.GetProductsAsync(
                request.PageNumber,
                request.PageSize,
                request.SearchTerm,
                request.CategoryId
            );

            var productDetails = products.ConvertAll(p => new ProductDetails
            {
                Id = p.ProductId,
                Name = p.Name,
                SKU = p.SKU,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "Unknown Category",
                ThumbnailUrl = p.ThumbnailUrl,
                ImageUrls = p.Images.Select(i => i.ImageUrl).ToList()
            });

            var response = new ListProductsResponse
            {
                Products = productDetails,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return Result.Ok(response);
        }
    }

    public class ListProductsQuery : IRequest<Result<ListProductsResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class ListProductsResponse
    {
        public required IReadOnlyCollection<ProductDetails> Products { get; set; }
        public required int TotalCount { get; set; }
        public required int PageNumber { get; set; }
        public required int PageSize { get; set; }
    }

    public class ProductDetails
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
