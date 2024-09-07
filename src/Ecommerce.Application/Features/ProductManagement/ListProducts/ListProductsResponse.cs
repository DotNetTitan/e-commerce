namespace Ecommerce.Application.Features.ProductManagement.ListProducts
{
    public class ListProductsResponse
    {
        public required List<ProductDetails> Products { get; set; }
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