namespace Ecommerce.Api.DTOs.Orders
{
    public class UpdateOrderStatusDto
    {
        public required Ecommerce.Domain.Enums.OrderStatus OrderStatus { get; set; }
        public required Ecommerce.Domain.Enums.PaymentStatus PaymentStatus { get; set; }
        public string? TrackingNumber { get; set; }
    }
}