using Ecommerce.Application.Features.Inventory.Commands.UpdateInventory;
using Ecommerce.Application.Features.Inventory.Queries.GetInventory;
using Ecommerce.Application.Interfaces;
using Ecommerce.Domain.Entities;
using NSubstitute;

namespace Ecommerce.Application.Tests
{
    public class InventoryUnitTests
    {
        private readonly IProductRepository _productRepository;
        private readonly UpdateInventoryCommandHandler _updateHandler;
        private readonly GetInventoryQueryHandler _getHandler;

        public InventoryUnitTests()
        {
            _productRepository = Substitute.For<IProductRepository>();
            _updateHandler = new UpdateInventoryCommandHandler(_productRepository);
            _getHandler = new GetInventoryQueryHandler(_productRepository);
        }
        
        [Fact]
        public async Task UpdateInventoryHandler_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new UpdateInventoryCommand
            {
                ProductId = productId,
                NewQuantity = 50,
                LowStockThreshold = 10
            };

            var existingProduct = new Product
            {
                ProductId = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                CategoryId = Guid.NewGuid(),
                StockQuantity = 30,
                LowStockThreshold = 5
            };

            _productRepository.GetByIdAsync(productId).Returns(existingProduct);
            _productRepository.UpdateAsync(Arg.Any<Product>()).Returns(existingProduct);

            // Act
            var result = await _updateHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(productId, result.Value.ProductId);
            Assert.Equal(command.NewQuantity, result.Value.NewStockQuantity);
            Assert.Equal(command.LowStockThreshold, result.Value.LowStockThreshold);
        }

        [Fact]
        public async Task UpdateInventoryHandler_ProductNotFound_ReturnsFailureResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var command = new UpdateInventoryCommand
            {
                ProductId = productId,
                NewQuantity = 50,
                LowStockThreshold = 10
            };

            _productRepository.GetByIdAsync(productId).Returns((Product?)null);

            // Act
            var result = await _updateHandler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsFailed);
            Assert.Contains(result.Errors, error => error.Message.Contains($"Product with ID {productId} not found"));
        }

        [Fact]
        public async Task GetInventoryHandler_ReturnsSuccessResult()
        {
            // Arrange
            var query = new GetInventoryQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = "test",
                CategoryId = Guid.NewGuid()
            };

            var products = new List<Product>
            {
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Test Product 1",
                    Description = "Test Description 1",
                    Price = 9.99m,
                    CategoryId = Guid.NewGuid(),
                    StockQuantity = 10,
                    LowStockThreshold = 5,
                    Category = new Category { Name = "Test Category" }
                },
                new Product
                {
                    ProductId = Guid.NewGuid(),
                    Name = "Test Product 2",
                    Description = "Test Description 2",
                    Price = 19.99m,
                    CategoryId = Guid.NewGuid(),
                    StockQuantity = 20,
                    LowStockThreshold = 8,
                    Category = new Category { Name = "Test Category" }
                }
            };

            _productRepository.GetProductsAsync(query.PageNumber, query.PageSize, query.SearchTerm, query.CategoryId)
                .Returns((products, products.Count));

            // Act
            var result = await _getHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(products.Count, result.Value.InventoryItems.Count);
            Assert.Equal(products.Count, result.Value.TotalCount);
            Assert.Equal(query.PageNumber, result.Value.PageNumber);
            Assert.Equal(query.PageSize, result.Value.PageSize);
            Assert.All(result.Value.InventoryItems, item =>
            {
                Assert.NotEqual(Guid.Empty, item.ProductId);
                Assert.NotEmpty(item.ProductName);
                Assert.True(item.StockQuantity >= 0);
                Assert.True(item.LowStockThreshold >= 0);
                Assert.NotEmpty(item.CategoryName);
            });
        }

        [Fact]
        public async Task GetInventoryHandler_EmptyResult_ReturnsSuccessResultWithEmptyList()
        {
            // Arrange
            var query = new GetInventoryQuery
            {
                PageNumber = 1,
                PageSize = 10,
                SearchTerm = "nonexistent",
                CategoryId = Guid.NewGuid()
            };

            _productRepository.GetProductsAsync(query.PageNumber, query.PageSize, query.SearchTerm, query.CategoryId)
                .Returns((new List<Product>(), 0));

            // Act
            var result = await _getHandler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Empty(result.Value.InventoryItems);
            Assert.Equal(0, result.Value.TotalCount);
            Assert.Equal(query.PageNumber, result.Value.PageNumber);
            Assert.Equal(query.PageSize, result.Value.PageSize);
        }
    }
}