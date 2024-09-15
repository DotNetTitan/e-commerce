
namespace Ecommerce.Application.Features.OrderManagement.PlaceOrder
{
    public class PlaceOrderResponse
    {
        public Guid OrderId { get; internal set; }
        public decimal TotalAmount { get; internal set; }
    }
}