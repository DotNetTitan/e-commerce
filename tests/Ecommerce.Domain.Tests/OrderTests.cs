using Ecommerce.Domain.Entities;
using Ecommerce.Domain.Enums;
using Ecommerce.Domain.ValueObjects;
using FluentAssertions;

namespace Ecommerce.Domain.Tests
{
    public class OrderTests
    {
        private Order CreateValidOrder()
        {
            return new Order
            {
                CustomerId = Guid.NewGuid(),
                ShippingAddress = new Address("Test Building", "123 Test St", "12345", "Test City", "TS", "Test Country")
            };
        }

        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Act
            var order = CreateValidOrder();

            // Assert
            order.OrderId.Should().NotBe(Guid.Empty);
            order.OrderDate.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(1));
            order.OrderNumber.Should().NotBeNullOrEmpty();
            order.TrackingNumber.Should().NotBeNullOrEmpty();
            order.OrderStatus.Should().Be(OrderStatus.Pending);
            order.PaymentStatus.Should().Be(PaymentStatus.Pending);
            order.PaymentMethod.Should().Be(PaymentMethod.CashOnDelivery);
            order.OrderItems.Should().BeEmpty();
        }

        [Fact]
        public void AddOrderItem_ShouldAddItemAndRecalculateTotalAmount()
        {
            // Arrange
            var order = CreateValidOrder();
            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 10.00m
            };

            // Act
            order.AddOrderItem(orderItem);

            // Assert
            order.OrderItems.Should().ContainSingle();
            order.TotalAmount.Should().Be(20.00m);
        }

        [Fact]
        public void RemoveOrderItem_ShouldRemoveItemAndRecalculateTotalAmount()
        {
            // Arrange
            var order = CreateValidOrder();
            var orderItem = new OrderItem
            {
                OrderId = order.OrderId,
                ProductId = Guid.NewGuid(),
                Quantity = 2,
                UnitPrice = 10.00m
            };
            order.AddOrderItem(orderItem);

            // Act
            order.RemoveOrderItem(orderItem);

            // Assert
            order.OrderItems.Should().BeEmpty();
            order.TotalAmount.Should().Be(0);
        }

        [Fact]
        public void UpdateOrderStatus_ShouldUpdateStatusWhenTransitionIsValid()
        {
            // Arrange
            var order = CreateValidOrder();

            // Act
            order.UpdateOrderStatus(OrderStatus.Processing);

            // Assert
            order.OrderStatus.Should().Be(OrderStatus.Processing);
        }

        [Fact]
        public void UpdateOrderStatus_ShouldThrowExceptionWhenTransitionIsInvalid()
        {
            // Arrange
            var order = CreateValidOrder();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.UpdateOrderStatus(OrderStatus.Delivered));
        }

        [Fact]
        public void UpdatePaymentStatus_ShouldUpdateStatusWhenTransitionIsValid()
        {
            // Arrange
            var order = CreateValidOrder();

            // Act
            order.UpdatePaymentStatus(PaymentStatus.Paid);

            // Assert
            order.PaymentStatus.Should().Be(PaymentStatus.Paid);
        }

        [Fact]
        public void UpdatePaymentStatus_ShouldThrowExceptionWhenTransitionIsInvalid()
        {
            // Arrange
            var order = CreateValidOrder();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.UpdatePaymentStatus(PaymentStatus.Refunded));
        }

        [Fact]
        public void CancelOrder_ShouldCancelOrderWhenStatusIsPendingOrProcessing()
        {
            // Arrange
            var order = CreateValidOrder();

            // Act
            order.CancelOrder();

            // Assert
            order.OrderStatus.Should().Be(OrderStatus.Cancelled);
            order.PaymentStatus.Should().Be(PaymentStatus.Pending);
        }

        [Fact]
        public void CancelOrder_ShouldThrowExceptionWhenOrderIsShipped()
        {
            // Arrange
            var order = CreateValidOrder();
            order.UpdateOrderStatus(OrderStatus.Processing);
            order.UpdateOrderStatus(OrderStatus.Shipped);

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => order.CancelOrder());
        }

        [Fact]
        public void IsEmpty_ShouldReturnTrueWhenOrderHasNoItems()
        {
            // Arrange
            var order = CreateValidOrder();

            // Act & Assert
            order.IsEmpty().Should().BeTrue();
        }

        [Fact]
        public void ItemCount_ShouldReturnCorrectTotalQuantity()
        {
            // Arrange
            var order = CreateValidOrder();
            order.AddOrderItem(new OrderItem { OrderId = order.OrderId, ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 10.00m });
            order.AddOrderItem(new OrderItem { OrderId = order.OrderId, ProductId = Guid.NewGuid(), Quantity = 3, UnitPrice = 15.00m });

            // Act & Assert
            order.ItemCount.Should().Be(5);
        }

        [Fact]
        public void UniqueItemCount_ShouldReturnCorrectUniqueItemCount()
        {
            // Arrange
            var order = CreateValidOrder();
            order.AddOrderItem(new OrderItem { OrderId = order.OrderId, ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 10.00m });
            order.AddOrderItem(new OrderItem { OrderId = order.OrderId, ProductId = Guid.NewGuid(), Quantity = 3, UnitPrice = 15.00m });

            // Act & Assert
            order.UniqueItemCount.Should().Be(2);
        }

        [Fact]
        public void ValidateTotalAmount_ShouldReturnTrueWhenTotalMatchesExpected()
        {
            // Arrange
            var order = CreateValidOrder();
            order.AddOrderItem(new OrderItem { OrderId = order.OrderId, ProductId = Guid.NewGuid(), Quantity = 2, UnitPrice = 10.00m });
            order.AddOrderItem(new OrderItem { OrderId = order.OrderId, ProductId = Guid.NewGuid(), Quantity = 3, UnitPrice = 15.00m });

            // Act & Assert
            order.ValidateTotalAmount(65.00m).Should().BeTrue();
        }
    }
}
