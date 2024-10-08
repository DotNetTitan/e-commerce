using FluentResults;
using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Features.Orders.Commands.UpdateOrderStatus
{
    public class UpdateOrderStatusCommandHandler : IRequestHandler<UpdateOrderStatusCommand, Result<UpdateOrderStatusResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateOrderStatusCommandHandler(IOrderRepository orderRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Result<UpdateOrderStatusResponse>> Handle(UpdateOrderStatusCommand request, CancellationToken cancellationToken)
        {
            var order = await _orderRepository.GetOrderByOrderIdAsync(request.OrderId);

            if (order == null)
            {
                return Result.Fail<UpdateOrderStatusResponse>($"Order with ID {request.OrderId} not found.");
            }

            if (request.OrderStatus == OrderStatus.Shipped && string.IsNullOrEmpty(request.TrackingNumber))
            {
                return Result.Fail<UpdateOrderStatusResponse>("Tracking number is required when setting status to Shipped.");
            }

            order.UpdateOrderStatus(request.OrderStatus);
            
            order.UpdatePaymentStatus(request.PaymentStatus);

            await _orderRepository.UpdateOrderAsync(order);
            await _unitOfWork.CommitAsync();

            return Result.Ok(new UpdateOrderStatusResponse
            {
                OrderId = order.OrderId,
                OrderStatus = order.OrderStatus,
                TrackingNumber = order.TrackingNumber
            });
        }
    }
    
    public class UpdateOrderStatusCommand : IRequest<Result<UpdateOrderStatusResponse>>
    {
        public required Guid OrderId { get; init; }
        public required OrderStatus OrderStatus { get; init; }
        public required PaymentStatus PaymentStatus { get; init; }
        public string? TrackingNumber { get; init; }
    }

    public class UpdateOrderStatusResponse
    {
        public required Guid OrderId { get; init; }
        public required OrderStatus OrderStatus { get; init; }
        public string? TrackingNumber { get; init; }
    }
}