using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.Events;
using Ecommerce.Domain.Exceptions;
using MassTransit;
using FluentResults;

namespace Ecommerce.Application.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, Result<PlaceOrderResponse>>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IBus _bus;

        public PlaceOrderCommandHandler(
            IOrderRepository orderRepository, 
            ICustomerRepository customerRepository, 
            IUnitOfWork unitOfWork,
            IBus bus)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
            _bus = bus;
        }

        public async Task<Result<PlaceOrderResponse>> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var customer = await _customerRepository.GetByIdAsync(request.CustomerId) ?? throw CustomerNotFoundException.FromId(request.CustomerId);

                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    ShippingAddress = customer.CustomerAddress!,
                    PaymentMethod = request.PaymentMethod
                };

                foreach (var item in request.Items)
                {
                    var orderItem = new OrderItem
                    {
                        OrderId = order.OrderId,
                        ProductId = item.ProductId,
                        Quantity = item.Quantity,
                        UnitPrice = item.UnitPrice
                    };
                    order.AddOrderItem(orderItem);
                }

                if (order.IsEmpty())
                {
                    return Result.Fail<PlaceOrderResponse>("Cannot place an empty order.");
                }

                if (!order.ValidateTotalAmount(request.TotalAmount))
                {
                    return Result.Fail<PlaceOrderResponse>("Calculation mismatch.");
                }

                order.GenerateTrackingNumber(); // Generate tracking number

                await _orderRepository.CreateOrderAsync(order);

                await _unitOfWork.CommitAsync();

                // Publish event to Azure Service Bus
                await _bus.Publish(new OrderPlacedEvent
                {
                    OrderId = order.OrderId,
                    CustomerId = order.CustomerId,
                    Email = customer.Email
                }, cancellationToken);

                return Result.Ok(new PlaceOrderResponse
                {
                    OrderId = order.OrderId,
                    TotalAmount = order.TotalAmount,
                    OrderNumber = order.OrderNumber
                });
            }
            catch (Exception ex)
            {
                await _unitOfWork.RollbackAsync();
                return Result.Fail<PlaceOrderResponse>(ex.Message);
            }
        }
    }

    public class PlaceOrderCommand : IRequest<Result<PlaceOrderResponse>>
    {
        public Guid CustomerId { get; set; }
        public required List<Item> Items { get; set; }
        public decimal TotalAmount { get; set; }
        public PaymentMethod PaymentMethod { get; set; }
    }

    public class Item
    {
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }
    }

    public class PlaceOrderResponse
    {
        public Guid OrderId { get; internal set; }
        public decimal TotalAmount { get; internal set; }
        public required string OrderNumber { get; init; }
    }
}