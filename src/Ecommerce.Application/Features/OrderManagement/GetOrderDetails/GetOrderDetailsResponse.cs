using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Features.OrderManagement.GetOrderDetails
{
    public class GetOrderDetailsResponse
    {
        public required Guid OrderId { get; init; }
        public required Guid CustomerId { get; init; }
        public required DateTime OrderDate { get; init; }
        public required OrderStatus Status { get; init; }
        public required decimal TotalAmount { get; init; }
        public required List<OrderItemDetails> Items { get; init; }
    }

    public class OrderItemDetails
    {
        public required Guid ProductId { get; init; }
        public required string ProductName { get; init; }
        public required int Quantity { get; init; }
        public required decimal UnitPrice { get; init; }
        public required decimal TotalPrice { get; init; }
    }
}