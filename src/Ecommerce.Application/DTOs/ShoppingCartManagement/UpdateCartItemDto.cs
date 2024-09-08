namespace Ecommerce.Application.DTOs.ShoppingCartManagement
{
    public class UpdateCartItemDto
    {
        public required Guid CustomerId { get; init; }
        public required Guid ProductId { get; init; }
        public required int NewQuantity { get; init; }
    }
}