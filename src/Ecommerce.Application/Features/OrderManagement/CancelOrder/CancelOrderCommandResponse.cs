using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Features.OrderManagement.CancelOrder
{
    public class CancelOrderCommandResponse
    {
        public required Guid OrderId { get; init; }
        public required OrderStatus Status { get; init; }
    }
}