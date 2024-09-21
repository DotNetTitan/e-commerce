using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Features.OrderManagement.PlaceOrder
{
    public class PlaceOrderCommandHandler : IRequestHandler<PlaceOrderCommand, PlaceOrderResponse>
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

        public async Task<PlaceOrderResponse> Handle(PlaceOrderCommand request, CancellationToken cancellationToken)
        {
            await _unitOfWork.BeginTransactionAsync();

            try
            {
                var customer = await _customerRepository.GetByIdAsync(request.OrderDetails.CustomerId) ?? throw new CustomerNotFoundException();

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

                if(order.TotalAmount!= request.OrderDetails.TotalAmount)
                {
                    throw new InvalidOperationException("Calculation mismatch.");
                }

                order.UpdateStatus(OrderStatus.InProgress);

                await _orderRepository.CreateOrderAsync(order);

                await _unitOfWork.CommitAsync(); // Commit the transaction if everything is successful

                return new PlaceOrderResponse { OrderId = order.OrderId, TotalAmount = order.TotalAmount };
            }
            catch
            {
                await _unitOfWork.RollbackAsync(); // Rollback the transaction in case of an exception
                throw;
            }
        }
    }
}