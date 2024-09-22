using MediatR;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Exceptions;
using Ecommerce.Domain.Enums;

namespace Ecommerce.Application.Features.Orders.Commands.PlaceOrder
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
                var customer = await _customerRepository.GetByIdAsync(request.CustomerId) ?? throw CustomerNotFoundException.FromId(request.CustomerId);

                var order = new Order
                {
                    CustomerId = request.CustomerId,
                    ShippingAddress = customer.CustomerAddress!
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
                    throw new InvalidOperationException("Cannot place an empty order.");
                }

                if (!order.ValidateTotalAmount(request.TotalAmount))
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

    public class PlaceOrderCommand : IRequest<PlaceOrderResponse>
    {
        public Guid CustomerId { get; set; }
        public required List<Item> Items { get; set; }
        public decimal TotalAmount { get; set; }
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
    }
}