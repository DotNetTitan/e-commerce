namespace Ecommerce.Api.DTOs.Orders
{
    public class PlaceOrderDto
    {
        public Guid CustomerId { get; set; }
        public required List<OrderItemDto> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime OrderDate { get; set; }
    }
}