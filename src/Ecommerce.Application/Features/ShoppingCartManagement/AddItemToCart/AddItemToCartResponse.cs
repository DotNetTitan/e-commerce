namespace Ecommerce.Application.Features.ShoppingCartManagement.AddItemToCart
{
    public class AddItemToCartResponse
    {
        public required Guid CartId { get; init; }
        public required Guid ProductId { get; init; }
        public required int Quantity { get; init; }
        public required int TotalItems { get; init; }
        public required decimal TotalPrice { get; init; }
    }
}