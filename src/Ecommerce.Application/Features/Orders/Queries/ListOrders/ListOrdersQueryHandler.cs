using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Features.Orders.Queries.ListOrders
{
    public class ListOrdersQueryHandler : IRequestHandler<ListOrdersQuery, Result<ListOrdersQueryResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public ListOrdersQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Result<ListOrdersQueryResponse>> Handle(ListOrdersQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return Result.Fail<ListOrdersQueryResponse>("Request cannot be null");
            }

            try
            {
                var orders = await _orderRepository.GetOrdersByCustomerIdAsync(request.CustomerId);
                var orderSummaries = MapOrdersToSummaries(orders);

                return Result.Ok(new ListOrdersQueryResponse { Orders = orderSummaries });
            }
            catch (Exception ex)
            {
                return Result.Fail<ListOrdersQueryResponse>($"An error occurred while processing the request: {ex.Message}");
            }
        }

        private static List<OrderSummary> MapOrdersToSummaries(List<Order> orders)
        {
            return orders.ConvertAll(order => new OrderSummary
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalAmount = order.TotalAmount,
                ItemCount = order.OrderItems.Count
            });
        }
    }

    public class ListOrdersQuery : IRequest<Result<ListOrdersQueryResponse>>
    {
        public required Guid CustomerId { get; init; }
        public int PageNumber { get; set; } = 1;
        public int PageSize { get; set; } = 10;
    }

    public class ListOrdersQueryResponse
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