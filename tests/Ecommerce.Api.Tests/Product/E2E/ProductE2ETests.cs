using Ecommerce.Api.DTOs.Categories;
using Ecommerce.Api.DTOs.Products;
using Ecommerce.Application.Features.Categories.Commands.CreateCategory;
using Ecommerce.Application.Features.Products.Commands.CreateProduct;
using Ecommerce.Application.Features.Products.Commands.DeleteProduct;
using Ecommerce.Application.Features.Products.Commands.UpdateProduct;
using Ecommerce.Application.Features.Products.Queries.GetProduct;
using Ecommerce.Application.Features.Products.Queries.ListProducts;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;

namespace Ecommerce.Api.Tests.Product.E2E
{
    public class ProductE2ETests : IDisposable
    {
        private readonly WebApplicationFactory<Program> _factory;

        public ProductE2ETests()
        {
            _factory = new WebApplicationFactory<Program>();
        }

        public void Dispose()
        {
            _factory.Dispose();
        }

        private HttpClient CreateClient()
        {
            string dbName = Guid.NewGuid().ToString();
            return _factory.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });

                    var emailClientDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(Azure.Communication.Email.EmailClient));
                    if (emailClientDescriptor != null)
                    {
                        services.Remove(emailClientDescriptor);
                    }

                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddDebug();
                    });
                });
            }).CreateClient();
        }

        [Fact]
        public async Task CreateProduct_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            using var client = CreateClient();

            // Create a category first
            var createCategoryDto = new CreateCategoryDto
            {
                Name = "Test Category",
                Description = "Test Category Description"
            };
            var categoryResponse = await client.PostAsJsonAsync("/api/v1/categories", createCategoryDto);
            var createdCategory = await categoryResponse.Content.ReadFromJsonAsync<CreateCategoryResponse>();
            Assert.NotNull(createdCategory);

            var createProductDto = new CreateProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                StockQuantity = 100,
                CategoryId = createdCategory.CategoryId,
                LowStockThreshold = 10
            };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/products", createProductDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<CreateProductResponse>();
            Assert.NotNull(result);
            Assert.Equal(createProductDto.Name, result.Name);
            Assert.Equal(createProductDto.Description, result.Description);
            Assert.Equal(createProductDto.Price, result.Price);
            Assert.Equal(createProductDto.StockQuantity, result.StockQuantity);
            Assert.Equal(createProductDto.CategoryId, result.CategoryId);
            Assert.NotNull(result.SKU);
        }

        [Fact]
        public async Task GetProduct_ExistingProduct_ReturnsOkResult()
        {
            // Arrange
            using var client = CreateClient();
            var createProductDto = new CreateProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                StockQuantity = 100,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 10
            };
            var createResponse = await client.PostAsJsonAsync("/api/v1/products", createProductDto);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();

            // Act
            var response = await client.GetAsync($"/api/v1/products/{createdProduct.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<GetProductResponse>();
            Assert.NotNull(result);
            Assert.Equal(createdProduct.Id, result.Id);
            Assert.Equal(createdProduct.Name, result.Name);
            Assert.Equal(createdProduct.Description, result.Description);
            Assert.Equal(createdProduct.Price, result.Price);
            Assert.Equal(createdProduct.StockQuantity, result.StockQuantity);
            Assert.Equal(createdProduct.CategoryId, result.CategoryId);
            Assert.Equal(createdProduct.SKU, result.SKU);
        }

        [Fact]
        public async Task UpdateProduct_ValidData_ReturnsOkResult()
        {
            // Arrange
            using var client = CreateClient();
            var createProductDto = new CreateProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                StockQuantity = 100,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 10
            };
            var createResponse = await client.PostAsJsonAsync("/api/v1/products", createProductDto);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();

            var updateProductDto = new UpdateProductDto
            {
                ProductId = createdProduct.Id,
                Name = "Updated Product",
                Description = "Updated Description",
                Price = 19.99m,
                StockQuantity = 200,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 20
            };

            // Act
            var response = await client.PutAsJsonAsync($"/api/v1/products/{createdProduct.Id}", updateProductDto);

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<UpdateProductResponse>();
            Assert.NotNull(result);
            Assert.Equal(updateProductDto.ProductId, result.Id);
            Assert.Equal(updateProductDto.Name, result.Name);
            Assert.Equal(updateProductDto.Description, result.Description);
            Assert.Equal(updateProductDto.Price, result.Price);
            Assert.Equal(updateProductDto.StockQuantity, result.StockQuantity);
            Assert.Equal(updateProductDto.CategoryId, result.CategoryId);
            Assert.Equal(createdProduct.SKU, result.SKU);
        }

        [Fact]
        public async Task DeleteProduct_ExistingProduct_ReturnsOkResult()
        {
            // Arrange
            using var client = CreateClient();
            var createProductDto = new CreateProductDto
            {
                Name = "Test Product",
                Description = "Test Description",
                Price = 9.99m,
                StockQuantity = 100,
                CategoryId = Guid.NewGuid(),
                LowStockThreshold = 10
            };
            var createResponse = await client.PostAsJsonAsync("/api/v1/products", createProductDto);
            var createdProduct = await createResponse.Content.ReadFromJsonAsync<CreateProductResponse>();

            // Act
            var response = await client.DeleteAsync($"/api/v1/products/{createdProduct.Id}");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<DeleteProductResponse>();
            Assert.NotNull(result);
            Assert.Equal(createdProduct.Id, result.ProductId);
            Assert.True(result.IsDeleted);
        }

        [Fact]
        public async Task ListProducts_ReturnsOkResult()
        {
            // Arrange
            using var client = CreateClient();

            // Create a category first
            var createCategoryDto = new CreateCategoryDto
            {
                Name = "Test Category",
                Description = "Test Category Description"
            };
            var categoryResponse = await client.PostAsJsonAsync("/api/v1/categories", createCategoryDto);
            var createdCategory = await categoryResponse.Content.ReadFromJsonAsync<CreateCategoryResponse>();
            Assert.NotNull(createdCategory);

            var products = new[]
            {
                new CreateProductDto
                {
                    Name = "Test Product 1",
                    Description = "Test Description 1",
                    Price = 9.99m,
                    StockQuantity = 100,
                    CategoryId = createdCategory.CategoryId,
                    LowStockThreshold = 10
                },
                new CreateProductDto
                {
                    Name = "Test Product 2",
                    Description = "Test Description 2",
                    Price = 19.99m,
                    StockQuantity = 200,
                    CategoryId = createdCategory.CategoryId,
                    LowStockThreshold = 20
                }
            };

            foreach (var product in products)
            {
                var createResponse = await client.PostAsJsonAsync("/api/v1/products", product);
                Console.WriteLine($"Create Product Response: {await createResponse.Content.ReadAsStringAsync()}");
            }

            // Add a small delay to ensure products are saved
            await Task.Delay(1000);

            // Act
            var response = await client.GetAsync("/api/v1/products?PageNumber=1&PageSize=10");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var responseContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"List Products Response: {responseContent}");
            var result = await response.Content.ReadFromJsonAsync<ListProductsResponse>();
            Assert.NotNull(result);
            Assert.NotEmpty(result.Products);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
            Assert.Equal(2, result.Products.Count);

            // Verify that both products are in the result
            Assert.Contains(result.Products, p => p.Name == "Test Product 1");
            Assert.Contains(result.Products, p => p.Name == "Test Product 2");
        }
    }
}