using Ecommerce.Domain.Entities;
using FluentAssertions;

namespace Ecommerce.Domain.Tests
{
     public class ShoppingCartItemTests
    {
        private static ShoppingCartItem CreateValidShoppingCartItem()
        {
            return new ShoppingCartItem
            {
                ShoppingCartId = Guid.NewGuid(),
                ProductId = Guid.NewGuid(),
                Quantity = 1,
                Price = 10.00m
            };
        }

        private static Product CreateValidProduct()
        {
            return new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                CategoryId = Guid.NewGuid()
            };
        }
        
        [Fact]
        public void Constructor_ShouldInitializePropertiesCorrectly()
        {
            // Act
            var item = CreateValidShoppingCartItem();

            // Assert
            item.ShoppingCartItemId.Should().NotBe(Guid.Empty);
            item.Quantity.Should().Be(1);
            item.Price.Should().Be(10.00m);
        }

        [Fact]
        public void TotalPrice_ShouldCalculateCorrectly()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();
            item.Quantity = 3;

            // Act
            var totalPrice = item.TotalPrice;

            // Assert
            totalPrice.Should().Be(30.00m);
        }

        [Fact]
        public void IncreaseQuantity_ShouldIncreaseQuantityCorrectly()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();

            // Act
            item.IncreaseQuantity(2);

            // Assert
            item.Quantity.Should().Be(3);
        }

        [Fact]
        public void IncreaseQuantity_ShouldThrowException_WhenAmountIsNegative()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => item.IncreaseQuantity(-1));
        }

        [Fact]
        public void DecreaseQuantity_ShouldDecreaseQuantityCorrectly()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();
            item.Quantity = 5;

            // Act
            item.DecreaseQuantity(2);

            // Assert
            item.Quantity.Should().Be(3);
        }

        [Fact]
        public void DecreaseQuantity_ShouldNotGoBelowZero()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();
            item.Quantity = 2;

            // Act
            item.DecreaseQuantity(3);

            // Assert
            item.Quantity.Should().Be(0);
        }

        [Fact]
        public void DecreaseQuantity_ShouldThrowException_WhenAmountIsNegative()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => item.DecreaseQuantity(-1));
        }

        [Fact]
        public void UpdateQuantity_ShouldUpdateQuantityCorrectly()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();

            // Act
            item.UpdateQuantity(5);

            // Assert
            item.Quantity.Should().Be(5);
        }

        [Fact]
        public void UpdateQuantity_ShouldThrowException_WhenNewQuantityIsNegative()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();

            // Act & Assert
            Assert.Throws<ArgumentException>(() => item.UpdateQuantity(-1));
        }

        [Fact]
        public void IsAvailable_ShouldReturnTrue_WhenProductHasStock()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();
            item.Product = CreateValidProduct();
            item.Product.StockQuantity = 5;

            // Act & Assert
            item.IsAvailable.Should().BeTrue();
        }

        [Fact]
        public void IsAvailable_ShouldReturnFalse_WhenProductHasNoStock()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();
            item.Product = CreateValidProduct();
            item.Product.StockQuantity = 0;

            // Act & Assert
            item.IsAvailable.Should().BeFalse();
        }

        [Fact]
        public void IsQuantityAvailable_ShouldReturnTrue_WhenRequestedQuantityIsAvailable()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();
            item.Product = CreateValidProduct();
            item.Product.StockQuantity = 5;

            // Act & Assert
            item.IsQuantityAvailable(3).Should().BeTrue();
        }

        [Fact]
        public void IsQuantityAvailable_ShouldReturnFalse_WhenRequestedQuantityIsNotAvailable()
        {
            // Arrange
            var item = CreateValidShoppingCartItem();
            item.Product = CreateValidProduct();
            item.Product.StockQuantity = 2;

            // Act & Assert
            item.IsQuantityAvailable(3).Should().BeFalse();
        }
    }
}