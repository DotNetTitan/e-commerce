﻿using Ecommerce.Api.DTOs.Categories;
using Ecommerce.Application.Features.Categories.Commands.CreateCategory;
using Ecommerce.Application.Features.Categories.Commands.DeleteCategory;
using Ecommerce.Application.Features.Categories.Commands.UpdateCategory;
using Ecommerce.Application.Features.Categories.Queries.GetCategory;
using Ecommerce.Application.Features.Categories.Queries.ListCategories;
using Ecommerce.Infrastructure.Persistence;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Net;
using System.Net.Http.Json;
using Xunit.Abstractions;

namespace Ecommerce.Api.Tests.Category.E2E
{
    public class CategoryE2ETests : IAsyncLifetime
    {
        private readonly ITestOutputHelper _testOutputHelper;
        private WebApplicationFactory<Program>? _factory;

        public CategoryE2ETests(ITestOutputHelper testOutputHelper)
        {
            _testOutputHelper = testOutputHelper;
        }

        public Task InitializeAsync()
        {
            _factory = new WebApplicationFactory<Program>();
            return Task.CompletedTask;
        }

        public async Task DisposeAsync()
        {
            if (_factory != null)
            {
                await _factory.DisposeAsync();
            }
        }

        private HttpClient CreateClient()
        {
            string dbName = Guid.NewGuid().ToString();

            return _factory!.WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    // Remove existing DbContext
                    var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));
                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // Add in-memory database
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        options.UseInMemoryDatabase(dbName);
                    });
                    
                    // Add minimal logging
                    services.AddLogging(loggingBuilder =>
                    {
                        loggingBuilder.ClearProviders();
                        loggingBuilder.AddConsole();
                        loggingBuilder.AddDebug();
                    });
                });
            }).CreateClient();
        }
        
        [Fact]
        public async Task CreateCategory_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            using var client = CreateClient();
            var createCategoryDto = new CreateCategoryDto { Name = "Test Category", Description = "Test Description" };

            // Act
            var response = await client.PostAsJsonAsync("/api/v1/categories", createCategoryDto);

            // Assert
            Assert.Equal(HttpStatusCode.Created, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<CreateCategoryResponse>();
            Assert.NotNull(result);
            Assert.Equal(createCategoryDto.Name, result.Name);
            Assert.Equal(createCategoryDto.Description, result.Description);
        }

        [Fact]
        public async Task GetCategory_ExistingCategory_ReturnsOkResult()
        {
            // Arrange
            using var client = CreateClient();
            var createCategoryDto = new CreateCategoryDto { Name = "Test Category", Description = "Test Description" };
            var createResponse = await client.PostAsJsonAsync("/api/v1/categories", createCategoryDto);
            var createdCategory = await createResponse.Content.ReadFromJsonAsync<CreateCategoryResponse>();

            // Act
            if (createdCategory != null)
            {
                var response = await client.GetAsync($"/api/v1/categories/{createdCategory.CategoryId}");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var result = await response.Content.ReadFromJsonAsync<GetCategoryResponse>();
                Assert.NotNull(result);
                Assert.Equal(createdCategory.CategoryId, result.CategoryId);
                Assert.Equal(createdCategory.Name, result.Name);
                Assert.Equal(createdCategory.Description, result.Description);
            }
        }

        [Fact]
        public async Task UpdateCategory_ValidData_ReturnsOkResult()
        {
            // Arrange
            using var client = CreateClient();
            var createCategoryDto = new CreateCategoryDto { Name = "Test Category", Description = "Test Description" };
            var createResponse = await client.PostAsJsonAsync("/api/v1/categories", createCategoryDto);
            var createdCategory = await createResponse.Content.ReadFromJsonAsync<CreateCategoryResponse>();

            if (createdCategory != null)
            {
                var updateCategoryDto = new UpdateCategoryDto
                {
                    CategoryId = createdCategory.CategoryId,
                    Name = "Updated Category",
                    Description = "Updated Description"
                };

                // Act
                var response = await client.PutAsJsonAsync($"/api/v1/categories/{createdCategory.CategoryId}", updateCategoryDto);

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var result = await response.Content.ReadFromJsonAsync<UpdateCategoryResponse>();
                Assert.NotNull(result);
                Assert.Equal(updateCategoryDto.CategoryId, result.Id);
                Assert.Equal(updateCategoryDto.Name, result.Name);
                Assert.Equal(updateCategoryDto.Description, result.Description);
            }
        }

        [Fact]
        public async Task DeleteCategory_ExistingCategory_ReturnsOkResult()
        {
            // Arrange
            using var client = CreateClient();
            var createCategoryDto = new CreateCategoryDto { Name = "Test Category", Description = "Test Description" };
            var createResponse = await client.PostAsJsonAsync("/api/v1/categories", createCategoryDto);
            var createdCategory = await createResponse.Content.ReadFromJsonAsync<CreateCategoryResponse>();

            // Act
            if (createdCategory != null)
            {
                var response = await client.DeleteAsync($"/api/v1/categories/{createdCategory.CategoryId}");

                // Assert
                Assert.Equal(HttpStatusCode.OK, response.StatusCode);
                var result = await response.Content.ReadFromJsonAsync<DeleteCategoryResponse>();
                Assert.NotNull(result);
                Assert.Equal(createdCategory.CategoryId, result.CategoryId);
                Assert.True(result.IsDeleted);
            }
        }

        [Fact]
        public async Task ListCategories_ReturnsOkResult()
        {
            // Arrange
            using var client = CreateClient();
            var createCategoryDto1 = new CreateCategoryDto { Name = "Test Category 1", Description = "Test Description 1" };
            var createCategoryDto2 = new CreateCategoryDto { Name = "Test Category 2", Description = "Test Description 2" };
            await client.PostAsJsonAsync("/api/v1/categories", createCategoryDto1);
            await client.PostAsJsonAsync("/api/v1/categories", createCategoryDto2);

            // Act
            var response = await client.GetAsync("/api/v1/categories?PageNumber=1&PageSize=10");

            // Assert
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            var result = await response.Content.ReadFromJsonAsync<ListCategoriesResponse>();
            Assert.NotNull(result);
            Assert.Equal(2, result.Categories.Count);
            Assert.Equal(2, result.TotalCount);
            Assert.Equal(1, result.PageNumber);
            Assert.Equal(10, result.PageSize);
        }
    }
}