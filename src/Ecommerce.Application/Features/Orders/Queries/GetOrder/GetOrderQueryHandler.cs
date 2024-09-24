using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Features.Orders.Queries.GetOrder
{
    public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Result<GetOrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public GetOrderQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<GetOrderResponse>> Handle(GetOrderQuery request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(request.OrderId);

            if (order == null)
            {
                return Result.Fail<GetOrderResponse>($"Order with ID {request.OrderId} not found.");
            }

            if (order.CustomerId != request.CustomerId)
            {
                return Result.Fail<GetOrderResponse>("You are not authorized to view this order.");
            }

            var response = new GetOrderResponse
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                OrderDate = order.OrderDate,
                Status = order.Status,
                OrderNumber = order.OrderNumber,
                TotalAmount = order.TotalAmount,
                Items = order.OrderItems.Select(item => new OrderItemDetails
                {
                    ProductId = item.ProductId,
                    ProductName = item.Product?.Name ?? "Unknown Product",
                    Quantity = item.Quantity,
                    UnitPrice = item.UnitPrice,
                    TotalPrice = item.TotalPrice
                }).ToList()
            };

            return Result.Ok(response);
        }
    }

    public class GetOrderQuery : IRequest<Result<GetOrderResponse>>
    {
        public required Guid OrderId { get; init; }
        public required Guid CustomerId { get; init; }
    }

    public class GetOrderResponse
    {
        public required Guid OrderId { get; init; }
        public required Guid CustomerId { get; init; }
        public required DateTime OrderDate { get; init; }
        public required OrderStatus Status { get; init; }
        public required string OrderNumber { get; init; }
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