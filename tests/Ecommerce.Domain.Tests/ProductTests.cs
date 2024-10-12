using Ecommerce.Domain.Entities;
using FluentAssertions;

namespace Ecommerce.Domain.Tests
{
    public class ProductTests
    {
        private static Product CreateValidProduct()
        {
            return new Product
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                CategoryId = Guid.NewGuid(),
                StockQuantity = 10,
                LowStockThreshold = 5
            };
        }

        [Fact]
        public void IsInStock_ShouldReturnTrue_WhenStockQuantityIsGreaterThanRequestedQuantity()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            var result = product.IsInStock(5);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsInStock_ShouldReturnFalse_WhenStockQuantityIsLessThanRequestedQuantity()
        {
            // Arrange
            var product = CreateValidProduct();
            product.StockQuantity = 5;

            // Act
            var result = product.IsInStock(10);

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void DecreaseStock_ShouldDecreaseStockQuantity_WhenEnoughStockAvailable()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            product.DecreaseStock(5);

            // Assert
            product.StockQuantity.Should().Be(5);
        }

        [Fact]
        public void DecreaseStock_ShouldThrowException_WhenNotEnoughStock()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => product.DecreaseStock(15));
        }

        [Fact]
        public void IncreaseStock_ShouldIncreaseStockQuantity()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            product.IncreaseStock(5);

            // Assert
            product.StockQuantity.Should().Be(15);
        }

        [Fact]
        public void IsLowStock_ShouldReturnTrue_WhenStockQuantityIsLessThanOrEqualToLowStockThreshold()
        {
            // Arrange
            var product = CreateValidProduct();
            product.StockQuantity = 5;

            // Act
            var result = product.IsLowStock();

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void IsLowStock_ShouldReturnFalse_WhenStockQuantityIsGreaterThanLowStockThreshold()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            var result = product.IsLowStock();

            // Assert
            result.Should().BeFalse();
        }

        [Fact]
        public void UpdateStock_ShouldIncreaseStock_WhenNewQuantityIsGreaterThanCurrentStock()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            product.UpdateStock(15);

            // Assert
            product.StockQuantity.Should().Be(15);
        }

        [Fact]
        public void UpdateStock_ShouldDecreaseStock_WhenNewQuantityIsLessThanCurrentStock()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            product.UpdateStock(5);

            // Assert
            product.StockQuantity.Should().Be(5);
        }

        [Fact]
        public void CanFulfillOrder_ShouldReturnTrue_WhenEnoughStockAvailable()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            var result = product.CanFulfillOrder(5);

            // Assert
            result.Should().BeTrue();
        }

        [Fact]
        public void CanFulfillOrder_ShouldReturnFalse_WhenNotEnoughStockAvailable()
        {
            // Arrange
            var product = CreateValidProduct();

            // Act
            var result = product.CanFulfillOrder(15);

            // Assert
            result.Should().BeFalse();
        }
    }
}