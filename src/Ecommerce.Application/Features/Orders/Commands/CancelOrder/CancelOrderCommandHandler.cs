using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Enums;
using FluentResults;

namespace Ecommerce.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result<CancelOrderCommandResponse>>
    {
        private readonly IOrderRepository _orderRepository;

        public CancelOrderCommandHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        public async Task<Result<CancelOrderCommandResponse>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(request.OrderId);

            if (order == null)
            {
                return Result.Fail<CancelOrderCommandResponse>($"Order with ID {request.OrderId} not found.");
            }

            if (order.CustomerId != request.CustomerId)
            {
                return Result.Fail<CancelOrderCommandResponse>("You are not authorized to cancel this order.");
            }

            if (order.Status != OrderStatus.InProgress)
            {
                return Result.Fail<CancelOrderCommandResponse>($"Cannot cancel order with status {order.Status}.");
            }

            order.UpdateStatus(OrderStatus.Cancelled);
            await _orderRepository.UpdateOrderAsync(order);

            return Result.Ok(new CancelOrderCommandResponse
            {
                OrderId = order.OrderId,
                Status = order.Status
            });
        }
    }

    public class CancelOrderCommand : IRequest<Result<CancelOrderCommandResponse>>
    {
        public required Guid OrderId { get; init; }
        public required Guid CustomerId { get; init; }
    }

    public class CancelOrderCommandResponse
    {
        public required Guid OrderId { get; init; }
        public required OrderStatus Status { get; init; }
    }
}