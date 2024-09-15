using MediatR;

namespace Ecommerce.Application.Features.OrderManagement.CancelOrder
{
    public class CancelOrderCommand : IRequest<bool>
    {
        public Guid OrderId { get; set; }
        public Guid CustomerId { get; set; }
    }
}
