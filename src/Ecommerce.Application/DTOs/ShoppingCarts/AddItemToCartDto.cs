namespace Ecommerce.Application.DTOs.ShoppingCarts
{
    public class AddItemToCartDto
    {
        public required Guid CustomerId { get; set; }
        public required Guid ProductId { get; set; }
        public required int Quantity { get; set; }
    }
}