using Ecommerce.Api.Controllers;
using Ecommerce.Api.DTOs.Products;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.DeleteProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateProduct;
using Ecommerce.Application.Features.Products.Queries.GetProduct;
using Ecommerce.Application.Features.Products.Queries.ListProducts;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Persistence.Repositories;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Ecommerce.Api.Tests.Product.Integration
{
    public class ProductIntegrationTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IProductRepository _productRepository;
        private readonly IMediator _mediator;
        private readonly ProductController _controller;

        public ProductIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _productRepository = new ProductRepository(_dbContext);
            _mediator = Substitute.For<IMediator>();
            _controller = new ProductController(_mediator);
        }

        [Fact]
        public async Task CreateProduct_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var createProductDto = new CreateProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                StockQuantity = 100,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 10
            };
            var createProductCommand = new CreateProductCommand
            {
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                CategoryId = createProductDto.CategoryId,
                LowStockThreshold = createProductDto.LowStockThreshold
            };
            var createProductResponse = new CreateProductResponse
            {
                Id = Guid.NewGuid(),
                Name = createProductDto.Name,
                Description = createProductDto.Description,
                Price = createProductDto.Price,
                StockQuantity = createProductDto.StockQuantity,
                CategoryId = createProductDto.CategoryId,
                Sku = "Sku-1234"
            };
            _mediator.Send(Arg.Any<CreateProductCommand>()).Returns(Result.Ok(createProductResponse));

            // Act
            var result = await _controller.CreateProduct(createProductDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<CreateProductResponse>(createdAtActionResult.Value);
            Assert.Equal(createProductResponse.Id, returnValue.Id);
            Assert.Equal(createProductResponse.Name, returnValue.Name);
            Assert.Equal(createProductResponse.Description, returnValue.Description);
            Assert.Equal(createProductResponse.Price, returnValue.Price);
            Assert.Equal(createProductResponse.StockQuantity, returnValue.StockQuantity);
            Assert.Equal(createProductResponse.CategoryId, returnValue.CategoryId);
            Assert.Equal(createProductResponse.Sku, returnValue.Sku);
        }

        [Fact]
        public async Task GetProduct_ExistingProduct_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var getProductQuery = new GetProductQuery { ProductId = productId };
            var getProductResponse = new GetProductResponse
            {
                Id = productId,
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                StockQuantity = 100,
                CategoryId = Guid.NewGuid(),
                CategoryName = "Test Category",
                Sku = "Sku-1234"
            };
            _mediator.Send(Arg.Any<GetProductQuery>()).Returns(Result.Ok(getProductResponse));

            // Act
            var result = await _controller.GetProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetProductResponse>(okResult.Value);
            Assert.Equal(getProductResponse.Id, returnValue.Id);
            Assert.Equal(getProductResponse.Name, returnValue.Name);
            Assert.Equal(getProductResponse.Description, returnValue.Description);
            Assert.Equal(getProductResponse.Price, returnValue.Price);
            Assert.Equal(getProductResponse.StockQuantity, returnValue.StockQuantity);
            Assert.Equal(getProductResponse.CategoryId, returnValue.CategoryId);
            Assert.Equal(getProductResponse.CategoryName, returnValue.CategoryName);
            Assert.Equal(getProductResponse.Sku, returnValue.Sku);
        }

        [Fact]
        public async Task UpdateProduct_ValidData_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var updateProductDto = new UpdateProductDto
            {
                ProductId = productId,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 19.99m,
                StockQuantity = 200,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 20
            };
            var updateProductCommand = new UpdateProductCommand
            {
                ProductId = productId,
                Name = updateProductDto.Name,
                Description = updateProductDto.Description,
                Price = updateProductDto.Price,
                StockQuantity = updateProductDto.StockQuantity,
                CategoryId = updateProductDto.CategoryId,
                LowStockThreshold = updateProductDto.LowStockThreshold
            };
            var updateProductResponse = new UpdateProductResponse
            {
                Id = productId,
                Name = updateProductDto.Name,
                Description = updateProductDto.Description,
                Price = updateProductDto.Price,
                StockQuantity = updateProductDto.StockQuantity,
                CategoryId = updateProductDto.CategoryId,
                Sku = "Sku-5678"
            };
            _mediator.Send(Arg.Any<UpdateProductCommand>()).Returns(Result.Ok(updateProductResponse));

            // Act
            var result = await _controller.UpdateProduct(productId, updateProductDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UpdateProductResponse>(okResult.Value);
            Assert.Equal(updateProductResponse.Id, returnValue.Id);
            Assert.Equal(updateProductResponse.Name, returnValue.Name);
            Assert.Equal(updateProductResponse.Description, returnValue.Description);
            Assert.Equal(updateProductResponse.Price, returnValue.Price);
            Assert.Equal(updateProductResponse.StockQuantity, returnValue.StockQuantity);
            Assert.Equal(updateProductResponse.CategoryId, returnValue.CategoryId);
            Assert.Equal(updateProductResponse.Sku, returnValue.Sku);
        }

        [Fact]
        public async Task DeleteProduct_ExistingProduct_ReturnsOkResult()
        {
            // Arrange
            var productId = Guid.NewGuid();
            var deleteProductCommand = new DeleteProductCommand { ProductId = productId };
            var deleteProductResponse = new DeleteProductResponse { ProductId = productId, IsDeleted = true };
            _mediator.Send(Arg.Any<DeleteProductCommand>()).Returns(Result.Ok(deleteProductResponse));

            // Act
            var result = await _controller.DeleteProduct(productId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<DeleteProductResponse>(okResult.Value);
            Assert.Equal(deleteProductResponse.ProductId, returnValue.ProductId);
            Assert.True(returnValue.IsDeleted);
        }

        [Fact]
        public async Task ListProducts_ReturnsOkResult()
        {
            // Arrange
            var listProductsDto = new ListProductsDto { PageNumber = 1, PageSize = 10 };
            var listProductsQuery = new ListProductsQuery
            {
                PageNumber = listProductsDto.PageNumber,
                PageSize = listProductsDto.PageSize,
                SearchTerm = listProductsDto.SearchTerm,
                CategoryId = listProductsDto.CategoryId
            };
            var listProductsResponse = new ListProductsResponse
            {
                Products = new List<ProductDetails>
                {
                    new ProductDetails
                    {
                        Id = Guid.NewGuid(),
                        Name = "Test Product",
                        Description = "Test Description",
                        Price = 9.99m,
                        StockQuantity = 100,
                        CategoryId = Guid.NewGuid(),
                        CategoryName = "Test Category",
                        Sku = "Sku-1234"
                    }
                },
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };
            _mediator.Send(Arg.Any<ListProductsQuery>()).Returns(Result.Ok(listProductsResponse));

            // Act
            var result = await _controller.ListProducts(listProductsDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ListProductsResponse>(okResult.Value);
            Assert.Single(returnValue.Products);
            Assert.Equal(listProductsResponse.TotalCount, returnValue.TotalCount);
            Assert.Equal(listProductsResponse.PageNumber, returnValue.PageNumber);
            Assert.Equal(listProductsResponse.PageSize, returnValue.PageSize);
        }
    }
}