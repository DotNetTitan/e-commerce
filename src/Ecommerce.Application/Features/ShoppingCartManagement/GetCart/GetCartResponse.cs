namespace Ecommerce.Application.Features.ShoppingCartManagement.GetCart
{
    public class GetCartResponse
    {
        public required Guid CartId { get; init; }
        public required Guid CustomerId { get; init; }
        public required int TotalItems { get; init; }
        public required decimal TotalPrice { get; init; }
        public required List<CartItem> Items { get; init; }
    }

    public class CartItem
    {
        public required Guid ProductId { get; init; }
        public required string ProductName { get; init; }
        public required int Quantity { get; init; }
        public required decimal Price { get; init; }
        public required decimal TotalPrice { get; init; }
    }
}