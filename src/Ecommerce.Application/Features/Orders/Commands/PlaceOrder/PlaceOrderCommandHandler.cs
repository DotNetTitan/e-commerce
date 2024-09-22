using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Enums;
using Ecommerce.Application.DTOs.Orders;

namespace Ecommerce.Application.Features.Orders.Commands.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderCommandResponse>
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly IUnitOfWork _unitOfWork;

        public PlaceOrderCommandHandler(IOrderRepository orderRepository, ICustomerRepository customerRepository, IUnitOfWork unitOfWork)
        {
            _orderRepository = orderRepository;
            _customerRepository = customerRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<PlaceOrderCommandResponse> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var customer = await _customerRepository.GetByIdAsync(request.OrderDetails.CustomerId) ?? throw CustomerNotFoundException.FromId(request.OrderDetails.CustomerId);

                var order = new Order
                {
                    CustomerId = request.OrderDetails.CustomerId,
                    OrderDate = DateTime.UtcNow,
                    ShippingAddress = customer.CustomerAddress!
                };

                foreach (var item in request.OrderDetails.OrderItems)
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
                    throw new InvalidOperationException("Cannot place an empty order.");
                }

                if (!order.ValidateTotalAmount(request.OrderDetails.TotalAmount))
                {
                    throw new InvalidOperationException("Calculation mismatch.");
                }

                order.UpdateStatus(OrderStatus.InProgress);

                await _orderRepository.CreateOrderAsync(order);

                await _unitOfWork.CommitAsync(); // Commit the transaction if everything is successful

                return new PlaceOrderCommandResponse { OrderId = order.OrderId, TotalAmount = order.TotalAmount };
            }
            catch
            {
                await _unitOfWork.RollbackAsync(); // Rollback the transaction in case of an exception
                throw;
            }
        }
    }

    public class PlaceOrderCommand : IRequest<PlaceOrderCommandResponse>
    {
        public required PlaceOrderDto OrderDetails { get; set; }
    }

    public class PlaceOrderCommandResponse
    {
        public Guid OrderId { get; internal set; }
        public decimal TotalAmount { get; internal set; }
    }
}