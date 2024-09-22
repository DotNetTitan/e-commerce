using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.Products.Queries.ListProducts
{
    public class ListProductsQueryHandler : IRequestHandler<ListProductsQuery, Result<ListProductsQueryResponse>>
    {
        private readonly IProductRepository _productRepository;

        public ListProductsQueryHandler(IProductRepository productRepository)
        {
            _productRepository = productRepository;
        }

        public async Task<Result<ListProductsQueryResponse>> Handle(ListProductsQuery request, CancellationToken cancellationToken)
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
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "Unknown Category"
            });

            var response = new ListProductsQueryResponse
            {
                Products = productDetails,
                TotalCount = totalCount,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize
            };

            return Result.Ok(response);
        }
    }

    public class ListProductsQuery : IRequest<Result<ListProductsQueryResponse>>
    {
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
        public string? SearchTerm { get; set; }
        public Guid? CategoryId { get; set; }
    }

    public class ListProductsQueryResponse
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
        public required string Description { get; set; }
        public required decimal Price { get; set; }
        public required int StockQuantity { get; set; }
        public required Guid CategoryId { get; set; }
        public required string CategoryName { get; set; }
    }
}