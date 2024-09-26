namespace Ecommerce.Api.DTOs.Orders
{
    public class UpdateOrderStatusDto
    {
        public required Ecommerce.Domain.Enums.OrderStatus Status { get; set; }
        public string? TrackingNumber { get; set; }
    }
}