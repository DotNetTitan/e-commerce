using Ecommerce.Application.Features.ShoppingCarts.Commands.AddItemToCart;
using Ecommerce.Application.Features.ShoppingCarts.Commands.UpdateCartItem;
using Ecommerce.Application.Features.ShoppingCarts.Commands.RemoveItemFromCart;
using Ecommerce.Application.Features.ShoppingCarts.Commands.ClearCart;
using Ecommerce.Application.Features.ShoppingCarts.Queries.GetCart;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using NSubstitute;


namespace Ecommerce.Application.Tests
{
    public class ShoppingCartUnitTests
    {
        private readonly IShoppingCartRepository _shoppingCartRepository;
        private readonly IProductRepository _productRepository;
        private readonly AddItemToCartCommandHandler _addItemHandler;
        private readonly UpdateCartItemCommandHandler _updateCartItemHandler;
        private readonly RemoveItemFromCartCommandHandler _removeItemHandler;
        private readonly ClearCartCommandHandler _clearCartHandler;
        private readonly GetCartQueryHandler _getCartHandler;

        public ShoppingCartUnitTests()
        {
            _shoppingCartRepository = Substitute.For<IShoppingCartRepository>();
            _productRepository = Substitute.For<IProductRepository>();
            _addItemHandler = new AddItemToCartCommandHandler(_shoppingCartRepository, _productRepository);
            _updateCartItemHandler = new UpdateCartItemCommandHandler(_shoppingCartRepository, _productRepository);
            _removeItemHandler = new RemoveItemFromCartCommandHandler(_shoppingCartRepository);
            _clearCartHandler = new ClearCartCommandHandler(_shoppingCartRepository);
            _getCartHandler = new GetCartQueryHandler(_shoppingCartRepository);
        }

       [Fact]
        public async Task AddItemToCart_NewCart_ReturnsSuccessResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var command = new AddItemToCartCommand
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = 2
            };

            var product = new Product
            {
                ProductId = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                StockQuantity = 5,
                CategoryId = Guid.NewGuid()
            };

            _productRepository.GetByIdAsync(productId).Returns(product);
            _shoppingCartRepository.GetByCustomerIdAsync(customerId).Returns((ShoppingCart)null);
            _shoppingCartRepository.CreateAsync(Arg.Any<ShoppingCart>()).Returns(new ShoppingCart { ShoppingCartId = Guid.NewGuid(), CustomerId = customerId });

            // Act
            var result = await _addItemHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(command.ProductId, result.Value.ProductId);
            Assert.Equal(command.Quantity, result.Value.Quantity);
            Assert.Equal(2, result.Value.TotalItems);
            Assert.Equal(20.00m, result.Value.TotalPrice);
        }

        [Fact]
        public async Task AddItemToCart_ExistingCart_ReturnsSuccessResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cartId = Guid.NewGuid();
            var command = new AddItemToCartCommand
            {
                CustomerId = customerId,
                ProductId = productId,
                Quantity = 2
            };

            var product = new Product
            {
                ProductId = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                StockQuantity = 5,
                CategoryId = Guid.NewGuid()
            };

            var existingCart = new ShoppingCart
            {
                ShoppingCartId = cartId,
                CustomerId = customerId,
                ShoppingCartItems = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem
                    {
                        ShoppingCartId = cartId,
                        ProductId = Guid.NewGuid(),
                        Quantity = 1,
                        Price = 15.00m
                    }
                }
            };

            _productRepository.GetByIdAsync(productId).Returns(product);
            _shoppingCartRepository.GetByCustomerIdAsync(customerId).Returns(existingCart);

            // Act
            var result = await _addItemHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(command.ProductId, result.Value.ProductId);
            Assert.Equal(command.Quantity, result.Value.Quantity);
            Assert.Equal(3, result.Value.TotalItems);
            Assert.Equal(35.00m, result.Value.TotalPrice);
        }

        [Fact]
        public async Task UpdateCartItem_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cartId = Guid.NewGuid();
            var command = new UpdateCartItemCommand
            {
                CustomerId = customerId,
                ProductId = productId,
                NewQuantity = 3
            };

            var product = new Product
            {
                ProductId = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 10.00m,
                StockQuantity = 5,
                CategoryId = Guid.NewGuid()
            };

            var cart = new ShoppingCart
            {
                ShoppingCartId = cartId,
                CustomerId = customerId,
                ShoppingCartItems = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem
                    {
                        ShoppingCartId = cartId,
                        ProductId = productId,
                        Quantity = 2,
                        Price = 10.00m
                    }
                }
            };

            _productRepository.GetByIdAsync(productId).Returns(product);
            _shoppingCartRepository.GetByCustomerIdAsync(customerId).Returns(cart);

            // Act
            var result = await _updateCartItemHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(cartId, result.Value.CartId);
            Assert.Equal(command.ProductId, result.Value.ProductId);
            Assert.Equal(command.NewQuantity, result.Value.NewQuantity);
            Assert.Equal(5, result.Value.TotalItems);
            Assert.Equal(50.00m, result.Value.TotalPrice);
        }

        [Fact]
        public async Task RemoveItemFromCart_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var productId = Guid.NewGuid();
            var cartId = Guid.NewGuid();
            var command = new RemoveItemFromCartCommand
            {
                CustomerId = customerId,
                ProductId = productId
            };

            var cart = new ShoppingCart
            {
                ShoppingCartId = cartId,
                CustomerId = customerId,
                ShoppingCartItems = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem
                    {
                        ShoppingCartId = cartId,
                        ProductId = productId,
                        Quantity = 2,
                        Price = 10.00m
                    },
                    new ShoppingCartItem
                    {
                        ShoppingCartId = cartId,
                        ProductId = Guid.NewGuid(),
                        Quantity = 1,
                        Price = 15.00m
                    }
                }
            };

            _shoppingCartRepository.GetByCustomerIdAsync(customerId).Returns(cart);

            // Act
            var result = await _removeItemHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(cartId, result.Value.CartId);
            Assert.Equal(productId, result.Value.RemovedProductId);
            Assert.Equal(1, result.Value.TotalItems);
            Assert.Equal(15.00m, result.Value.TotalPrice);
        }

        [Fact]
        public async Task ClearCart_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var command = new ClearCartCommand
            {
                CustomerId = customerId
            };

            _shoppingCartRepository.ClearAsync(customerId).Returns(true);

            // Act
            var result = await _clearCartHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(customerId, result.Value.CustomerId);
            Assert.True(result.Value.Success);
        }

        [Fact]
        public async Task GetCart_ValidRequest_ReturnsSuccessResult()
        {
            // Arrange
            var customerId = Guid.NewGuid();
            var cartId = Guid.NewGuid();
            var query = new GetCartQuery
            {
                CustomerId = customerId
            };

            var cart = new ShoppingCart
            {
                ShoppingCartId = cartId,
                CustomerId = customerId,
                ShoppingCartItems = new List<ShoppingCartItem>
                {
                    new ShoppingCartItem
                    {
                        ShoppingCartId = cartId,
                        ProductId = Guid.NewGuid(),
                        Quantity = 2,
                        Price = 10.00m,
                        Product = new Product
                        {
                            ProductId = Guid.NewGuid(),
                            Name = "Product 1",
                            Description = "Description 1",
                            Price = 10.00m,
                            CategoryId = Guid.NewGuid()
                        }
                    },
                    new ShoppingCartItem
                    {
                        ShoppingCartId = cartId,
                        ProductId = Guid.NewGuid(),
                        Quantity = 1,
                        Price = 15.00m,
                        Product = new Product
                        {
                            ProductId = Guid.NewGuid(),
                            Name = "Product 2",
                            Description = "Description 2",
                            Price = 15.00m,
                            CategoryId = Guid.NewGuid()
                        }
                    }
                }
            };

            _shoppingCartRepository.GetByCustomerIdAsync(customerId).Returns(cart);

            // Act
            var result = await _getCartHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(cartId, result.Value.CartId);
            Assert.Equal(customerId, result.Value.CustomerId);
            Assert.Equal(3, result.Value.TotalItems);
            Assert.Equal(35.00m, result.Value.TotalPrice);
            Assert.Equal(2, result.Value.Items.Count);
        }
    }
}