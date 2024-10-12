using Ecommerce.Application.Features.Orders.Commands.CancelOrder;
using Ecommerce.Application.Features.Orders.Commands.PlaceOrder;
using Ecommerce.Application.Features.Orders.Commands.UpdateOrderStatus;
using Ecommerce.Application.Features.Orders.Queries.GetOrder;
using Ecommerce.Application.Features.Orders.Queries.ListOrders;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;
using MassTransit;
using NSubstitute;

namespace Ecommerce.Application.Tests
{
    public class OrderUnitTests
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICustomerRepository _customerRepository;
        private readonly PlaceOrderCommandHandler _placeOrderHandler;
        private readonly UpdateOrderStatusCommandHandler _updateOrderStatusHandler;
        private readonly CancelOrderCommandHandler _cancelOrderHandler;
        private readonly GetOrderQueryHandler _getOrderHandler;
        private readonly ListOrdersQueryHandler _listOrdersHandler;

        public OrderUnitTests()
        {
            _orderRepository = Substitute.For<IOrderRepository>();
            _customerRepository = Substitute.For<ICustomerRepository>();
            var unitOfWork = Substitute.For<IUnitOfWork>();
            var bus = Substitute.For<IBus>();
            _placeOrderHandler = new PlaceOrderCommandHandler(_orderRepository, _customerRepository, unitOfWork, bus);
            _updateOrderStatusHandler = new UpdateOrderStatusCommandHandler(_orderRepository, unitOfWork);
            _cancelOrderHandler = new CancelOrderCommandHandler(_orderRepository, _customerRepository, bus);
            _getOrderHandler = new GetOrderQueryHandler(_orderRepository);
            _listOrdersHandler = new ListOrdersQueryHandler(_orderRepository);
        }

        [Fact]
        public async Task PlaceOrder_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var command = new PlaceOrderCommand
            {
                CustomerId = customerId,
                Items = new List<Item>
                {
                    new Item { ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 10.00m },
                    new Item { ProductId = Guid.NewGuid(), Quantity = 1, UnitPrice = 15.00m }
                },
                TotalAmount = 35.00m,
                PaymentMethod = PaymentMethod.CashOnDelivery
            };

            var customer = new Customer
            {
                CustomerId = customerId,
                IdentityId = Guid.NewGuid().ToString(),
                Email = "test@example.com",
                CustomerAddress = new Address("123 Main St", "Apt 4", "City", "State", "12345", "Country")
            };
            _customerRepository.GetByIdAsync(customerId).Returns(customer);

            // Act
            var result = await _placeOrderHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(35.00m, result.Value.TotalAmount);
            Assert.NotEmpty(result.Value.OrderNumber);
        }

        [Fact]
        public async Task UpdateOrderStatus_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var command = new UpdateOrderStatusCommand
            {
                OrderId = orderId,
                OrderStatus = OrderStatus.Processing,
                PaymentStatus = PaymentStatus.Paid
            };

            var order = new Order
            {
                CustomerId = Guid.NewGuid(),
                ShippingAddress = new Address("123 Main St", "Apt 4", "City", "State", "12345", "Country")
            };
            _orderRepository.GetOrderByOrderIdAsync(orderId).Returns(order);

            // Act
            var result = await _updateOrderStatusHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(OrderStatus.Processing, result.Value.OrderStatus);
        }

        [Fact]
        public async Task CancelOrder_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var customerId = Guid.NewGuid();
            var command = new CancelOrderCommand
            {
                OrderId = orderId,
                CustomerId = customerId
            };

            var customer = new Customer
            {
                CustomerId = customerId,
                IdentityId = Guid.NewGuid().ToString(),
                Email = "test@example.com",
                CustomerAddress = new Address("123 Main St", "Apt 4", "City", "State", "12345", "Country")
            };
            _customerRepository.GetByIdAsync(customerId).Returns(customer);

            var order = new Order
            {
                CustomerId = customerId,
                ShippingAddress = new Address("123 Main St", "Apt 4", "City", "State", "12345", "Country")
            };
            
            _orderRepository.GetOrderByOrderIdAsync(orderId).Returns(order);

            // Act
            var result = await _cancelOrderHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(OrderStatus.Cancelled, result.Value.OrderStatus);
        }

        [Fact]
        public async Task GetOrder_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            
            var order = new Order
            {
                CustomerId = customerId,
                ShippingAddress = new Address("123 Main St", "Apt 4", "City", "State", "12345", "Country")
            };
            
            var query = new GetOrderQuery { OrderId = order.OrderId, CustomerId = customerId };
            
            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 10.00m
            };
            order.AddOrderItem(orderItem);

            _orderRepository.GetOrderByOrderIdAsync(order.OrderId).Returns(Task.FromResult(order));

            // Act
            var result = await _getOrderHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(order.OrderId, result.Value.OrderId);
            Assert.Equal(customerId, result.Value.CustomerId);
            Assert.Equal(1, result.Value.Items.Count);
            Assert.Equal(20.00m, result.Value.TotalAmount);
        }

        [Fact]
        public async Task ListOrders_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var query = new ListOrdersQuery { CustomerId = customerId };

            var orders = new List<Order>
            {
                new Order
                {
                    CustomerId = customerId,
                    ShippingAddress = new Address("123 Main St", "Apt 4", "City", "State", "12345", "Country")
                },
                new Order
                {
                    CustomerId = customerId,
                    ShippingAddress = new Address("456 Elm St", "Unit 2", "Town", "State", "67890", "Country")
                }
            };
            _orderRepository.GetOrdersByCustomerIdAsync(customerId).Returns(orders);

            // Act
            var result = await _listOrdersHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(2, result.Value.Orders.Count);
        }
    }
}