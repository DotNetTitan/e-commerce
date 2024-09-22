namespace Ecommerce.Application.DTOs.Orders
{
    public class CancelOrderDto
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
    }
}