namespace Ecommerce.Application.DTOs.Orders
{
    public class PlaceOrderDto
    {
        public Guid CustomerId { get; set; }
        public required List<OrderItemDto> OrderItems { get; set; }
        public decimal TotalAmount { get; set; }
    }
}