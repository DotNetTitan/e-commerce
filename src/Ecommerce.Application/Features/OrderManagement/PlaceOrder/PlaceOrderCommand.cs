using MediatR;
using Ecommerce.Application.DTOs.OrderManagement;

namespace Ecommerce.Application.Features.OrderManagement.PlaceOrder
{
    public class PlaceOrderCommand : IRequest<PlaceOrderResponse>
    {
        public required PlaceOrderDto OrderDetails { get; set; }
    }
}
