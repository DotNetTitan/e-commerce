using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;

namespace Ecommerce.Application.Features.OrderManagement.ListUserOrders
{
    public class ListUserOrdersHandler : IRequestHandler<ListUserOrdersQuery, Result<ListUserOrdersResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public ListUserOrdersHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository ?? throw new ArgumentNullException(nameof(orderRepository));
        }

        public async Task<Result<ListUserOrdersResponse>> Handle(ListUserOrdersQuery request, CancellationToken cancellationToken)
        {
            if (request == null)
            {
                return Result.Fail<ListUserOrdersResponse>("Request cannot be null");
            }

            try
            {
                var orders = await _orderRepository.GetOrdersByCustomerIdAsync(request.CustomerId);
                var orderSummaries = MapOrdersToSummaries(orders);

                return Result.Ok(new ListUserOrdersResponse { Orders = orderSummaries });
            }
            catch (Exception ex)
            {
                return Result.Fail<ListUserOrdersResponse>($"An error occurred while processing the request: {ex.Message}");
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
}