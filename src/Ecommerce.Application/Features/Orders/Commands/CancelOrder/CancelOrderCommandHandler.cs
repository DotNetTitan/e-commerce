using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Enums;
using FluentResults;
using Ecommerce.Domain.Events;
using Ecommerce.Domain.Exceptions;
using MassTransit;

namespace Ecommerce.Application.Features.Orders.Commands.CancelOrder
{
    public class CancelOrderCommandHandler : IRequestHandler<CancelOrderCommand, Result<CancelOrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IBus _bus;

        public CancelOrderCommandHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository, IBus bus)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _bus = bus;
        }

        public async Task<Result<CancelOrderResponse>> Handle(CancelOrderCommand request, CancellationToken cancellationToken)
        {
            var customer = await _customerRepository.GetByIdAsync(request.CustomerId) 
                ?? throw CustomerNotFoundException.FromId(request.CustomerId);

            var order = await _orderRepository.GetOrderByOrderIdAsync(request.OrderId);

            if (order == null)
            {
                return Result.Fail<CancelOrderResponse>($"Order with ID {request.OrderId} not found.");
            }

            if (order.CustomerId != request.CustomerId)
            {
                return Result.Fail<CancelOrderResponse>("You are not authorized to cancel this order.");
            }

            if (order.Status != OrderStatus.Processing)
            {
                return Result.Fail<CancelOrderResponse>($"Cannot cancel order with status {order.Status}.");
            }

            order.CancelOrder();

            await _orderRepository.UpdateOrderAsync(order);

            // Publish event
            await _bus.Publish(new OrderCancelledEvent
            {
                OrderId = order.OrderId,
                CustomerId = order.CustomerId,
                Email = customer.Email
            }, cancellationToken);

            return Result.Ok(new CancelOrderResponse
            {
                OrderId = order.OrderId,
                Status = order.Status,
                OrderNumber = order.OrderNumber
            });
        }
    }

    public class CancelOrderCommand : IRequest<Result<CancelOrderResponse>>
    {
        public required Guid OrderId { get; init; }
        public required Guid CustomerId { get; init; }
    }

    public class CancelOrderResponse
    {
        public required Guid OrderId { get; init; }
        public required OrderStatus Status { get; init; }
        public required string OrderNumber { get; init; }
    }
}