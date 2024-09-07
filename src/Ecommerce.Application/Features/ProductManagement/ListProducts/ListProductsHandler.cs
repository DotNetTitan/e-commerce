using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;

namespace Ecommerce.Application.Features.ProductManagement.ListProducts
{
    public class ListProductsHandler : IRequestHandler<ListProductsQuery, Result<ListProductsResponse>>
    {
        private readonly IProductRepository _productRepository;

        public ListProductsHandler(IProductRepository productRepository)
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

            var productDetails = products.Select(p => new ProductDetails
            {
                Id = p.ProductId,
                Name = p.Name,
                Description = p.Description,
                Price = p.Price,
                StockQuantity = p.StockQuantity,
                CategoryId = p.CategoryId,
                CategoryName = p.Category?.Name ?? "Unknown Category"
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
}