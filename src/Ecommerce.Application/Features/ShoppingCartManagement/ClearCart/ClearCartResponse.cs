namespace Ecommerce.Application.Features.ShoppingCartManagement.ClearCart
{
    public class ClearCartResponse
    {
        public required Guid CustomerId { get; init; }
        public required bool Success { get; init; }
    }
}