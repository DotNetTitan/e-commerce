using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Features.OrderManagement.ListUserOrders
{
    public class ListUserOrdersResponse
    {
        public required List<OrderSummary> Orders { get; init; }
    }

    public class OrderSummary
    {
        public required Guid OrderId { get; init; }
        public required DateTime OrderDate { get; init; }
        public required OrderStatus Status { get; init; }
        public required decimal TotalAmount { get; init; }
        public required int ItemCount { get; init; }
    }
}