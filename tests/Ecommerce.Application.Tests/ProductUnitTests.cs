using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.DeleteProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateProduct;
using Ecommerce.Application.Features.Products.Queries.GetProduct;
using Ecommerce.Application.Features.Products.Queries.ListProducts;
using Ecommerce.Application.Interfaces;
using NSubstitute;

namespace Ecommerce.Application.Tests
{
    public class ProductUnitTests
    {
        private readonly IProductRepository _productRepository = Substitute.For<IProductRepository>();
        private readonly IAzureBlobStorageService _blobStorageService = Substitute.For<IAzureBlobStorageService>();
        private readonly IUnitOfWork _unitOfWork = Substitute.For<IUnitOfWork>();

        [Fact]
        public async Task CreateProductHandler_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var handler = new CreateProductCommandHandler(_productRepository, _blobStorageService, _unitOfWork);
            var command = new CreateProductCommand
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                StockQuantity = 100,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 10
            };

            var createdProduct = new Domain.Entities.Product
            {
                ProductId = Guid.NewGuid(),
                Name = command.Name,
                Description = command.Description,
                Price = command.Price,
                StockQuantity = command.StockQuantity,
                CategoryId = command.CategoryId,
                LowStockThreshold = command.LowStockThreshold
            };

            _productRepository.GetByNameAsync(command.Name).Returns(Task.FromResult((Domain.Entities.Product?)null));
            _productRepository.CreateAsync(Arg.Any<Domain.Entities.Product>()).Returns(Task.FromResult(createdProduct));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(createdProduct.ProductId, result.Value.Id);
            Assert.Equal(createdProduct.Name, result.Value.Name);
            Assert.Equal(createdProduct.Sku, result.Value.Sku);
        }

        [Fact]
        public async Task UpdateProductHandler_ValidCommand_ReturnsSuccessResult()
        {
            // Arrange
            var handler = new UpdateProductCommandHandler(_productRepository,_blobStorageService, _unitOfWork);
            var productId = Guid.NewGuid();
            var command = new UpdateProductCommand
            {
                ProductId = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 19.99m,
                StockQuantity = 200,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 20
            };

            var existingProduct = new Domain.Entities.Product
            {
                ProductId = productId,
                Name = "Original Product",
                Description = "Original Description",
                Price = 9.99m,
                StockQuantity = 100,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 10
            };

            _productRepository.GetByIdAsync(productId).Returns(Task.FromResult((Domain.Entities.Product?)existingProduct));
            _productRepository.UpdateAsync(Arg.Any<Domain.Entities.Product>()).Returns(Task.FromResult(existingProduct));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(command.ProductId, result.Value.Id);
            Assert.Equal(command.Name, result.Value.Name);
            Assert.Equal(command.Description, result.Value.Description);
            Assert.Equal(command.Price, result.Value.Price);
            Assert.Equal(command.StockQuantity, result.Value.StockQuantity);
            Assert.Equal(command.CategoryId, result.Value.CategoryId);
        }

        [Fact]
        public async Task DeleteProductHandler_ExistingProduct_ReturnsSuccessResult()
        {
            // Arrange
            var handler = new DeleteProductCommandHandler(_productRepository,_blobStorageService, _unitOfWork);
            var productId = Guid.NewGuid();
            var command = new DeleteProductCommand { ProductId = productId };

            var existingProduct = new Domain.Entities.Product
            {
                ProductId = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                StockQuantity = 100,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 10
            };

            _productRepository.GetByIdAsync(productId).Returns(Task.FromResult((Domain.Entities.Product?)existingProduct));
            _productRepository.DeleteAsync(Arg.Any<Domain.Entities.Product>()).Returns(Task.FromResult(true));

            // Act
            var result = await handler.Handle(command, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(productId, result.Value.ProductId);
            Assert.True(result.Value.IsDeleted);
        }

        [Fact]
        public async Task GetProductHandler_ExistingProduct_ReturnsSuccessResult()
        {
            // Arrange
            var handler = new GetProductQueryHandler(_productRepository);
            var productId = Guid.NewGuid();
            var query = new GetProductQuery { ProductId = productId };

            var product = new Domain.Entities.Product
            {
                ProductId = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                StockQuantity = 100,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 10,
                Category = new Domain.Entities.Category { Name = "Test Category" }
            };

            _productRepository.GetByIdAsync(productId).Returns(Task.FromResult((Domain.Entities.Product?)product));

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(product.ProductId, result.Value.Id);
            Assert.Equal(product.Name, result.Value.Name);
            Assert.Equal(product.Sku, result.Value.Sku);
            Assert.Equal(product.Description, result.Value.Description);
            Assert.Equal(product.Price, result.Value.Price);
            Assert.Equal(product.StockQuantity, result.Value.StockQuantity);
            Assert.Equal(product.CategoryId, result.Value.CategoryId);
            Assert.Equal(product.Category.Name, result.Value.CategoryName);
        }

        [Fact]
        public async Task ListProductsHandler_ReturnsSuccessResult()
        {
            // Arrange
            var handler = new ListProductsQueryHandler(_productRepository);
            var query = new ListProductsQuery { PageNumber = 1, PageSize = 10 };

            var products = new List<Domain.Entities.Product>
            {
                new Domain.Entities.Product { ProductId = Guid.NewGuid(), Name = "Product 1", Description = "Description 1", Price = 9.99m, StockQuantity = 100, CategoryId = Guid.NewGuid(), Category = new Domain.Entities.Category { Name = "Category 1" } },
                new Domain.Entities.Product { ProductId = Guid.NewGuid(), Name = "Product 2", Description = "Description 2", Price = 19.99m, StockQuantity = 200, CategoryId = Guid.NewGuid(), Category = new Domain.Entities.Category { Name = "Category 2" } }
            };

            _productRepository.GetProductsAsync(query.PageNumber, query.PageSize, query.SearchTerm, query.CategoryId)
                .Returns(Task.FromResult((products, 2)));

            // Act
            var result = await handler.Handle(query, CancellationToken.None);

            // Assert
            Assert.True(result.IsSuccess);
            Assert.NotNull(result.Value);
            Assert.Equal(products.Count, result.Value.Products.Count);
            Assert.Equal(2, result.Value.TotalCount);
            Assert.Equal(query.PageNumber, result.Value.PageNumber);
            Assert.Equal(query.PageSize, result.Value.PageSize);
            Assert.All(result.Value.Products, (productDetails, index) =>
            {
                Assert.Equal(products[index].ProductId, productDetails.Id);
                Assert.Equal(products[index].Name, productDetails.Name);
                Assert.Equal(products[index].Sku, productDetails.Sku);
                Assert.Equal(products[index].Description, productDetails.Description);
                Assert.Equal(products[index].Price, productDetails.Price);
                Assert.Equal(products[index].StockQuantity, productDetails.StockQuantity);
                Assert.Equal(products[index].CategoryId, productDetails.CategoryId);
                Assert.Equal(products[index].Category.Name, productDetails.CategoryName);
            });
        }
    }
}