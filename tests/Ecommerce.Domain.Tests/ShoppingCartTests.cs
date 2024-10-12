using Ecommerce.Domain.Entities;
using FluentAssertions;

namespace Ecommerce.Domain.Tests
{
    public class ShoppingCartTests
    {
        private static ShoppingCart CreateValidShoppingCart()
        {
            return new ShoppingCart
            {
                CustomerId = Guid.NewGuid()
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
            var cart = CreateValidShoppingCart();

            // Assert
            cart.ShoppingCartId.Should().NotBe(Guid.Empty);
            cart.ShoppingCartItems.Should().BeEmpty();
        }

        [Fact]
        public void AddItem_ShouldAddNewItem_WhenProductNotInCart()
        {
            // Arrange
            var cart = CreateValidShoppingCart();
            var product = CreateValidProduct();

            // Act
            cart.AddItem(product, 2);

            // Assert
            cart.ShoppingCartItems.Should().HaveCount(1);
            cart.TotalItems.Should().Be(2);
            cart.TotalPrice.Should().Be(20.00m);
        }

        [Fact]
        public void AddItem_ShouldUpdateExistingItem_WhenProductAlreadyInCart()
        {
            // Arrange
            var cart = CreateValidShoppingCart();
            var product = CreateValidProduct();
            cart.AddItem(product, 2);

            // Act
            cart.AddItem(product, 3);

            // Assert
            cart.ShoppingCartItems.Should().HaveCount(1);
            cart.TotalItems.Should().Be(5);
            cart.TotalPrice.Should().Be(50.00m);
        }

        [Fact]
        public void RemoveItem_ShouldRemoveItem_WhenItemExists()
        {
            // Arrange
            var cart = CreateValidShoppingCart();
            var product = CreateValidProduct();
            cart.AddItem(product, 2);

            // Act
            cart.RemoveItem(product.ProductId);

            // Assert
            cart.ShoppingCartItems.Should().BeEmpty();
            cart.TotalItems.Should().Be(0);
            cart.TotalPrice.Should().Be(0m);
        }

        [Fact]
        public void RemoveItem_ShouldNotThrowException_WhenItemDoesNotExist()
        {
            // Arrange
            var cart = CreateValidShoppingCart();

            // Act & Assert
            cart.Invoking(c => c.RemoveItem(Guid.NewGuid()))
                .Should().NotThrow();
        }

        [Fact]
        public void Clear_ShouldRemoveAllItems()
        {
            // Arrange
            var cart = CreateValidShoppingCart();
            var product1 = CreateValidProduct();
            var product2 = CreateValidProduct();
            cart.AddItem(product1, 2);
            cart.AddItem(product2, 1);

            // Act
            cart.Clear();

            // Assert
            cart.ShoppingCartItems.Should().BeEmpty();
            cart.TotalItems.Should().Be(0);
            cart.TotalPrice.Should().Be(0m);
        }

        [Fact]
        public void IsEmpty_ShouldReturnTrue_WhenCartHasNoItems()
        {
            // Arrange
            var cart = CreateValidShoppingCart();

            // Act & Assert
            cart.IsEmpty.Should().BeTrue();
        }

        [Fact]
        public void IsEmpty_ShouldReturnFalse_WhenCartHasItems()
        {
            // Arrange
            var cart = CreateValidShoppingCart();
            var product = CreateValidProduct();
            cart.AddItem(product, 1);

            // Act & Assert
            cart.IsEmpty.Should().BeFalse();
        }

        [Fact]
        public void UniqueItemCount_ShouldReturnCorrectCount()
        {
            // Arrange
            var cart = CreateValidShoppingCart();
            var product1 = CreateValidProduct();
            var product2 = CreateValidProduct();
            cart.AddItem(product1, 2);
            cart.AddItem(product2, 1);

            // Act & Assert
            cart.UniqueItemCount.Should().Be(2);
        }
    }
}