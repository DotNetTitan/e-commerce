using Ecommerce.Api.Controllers;
using Ecommerce.Api.DTOs.Categories;
using Ecommerce.Application.Features.Categories.Commands.CreateCategory;
using Ecommerce.Application.Features.Categories.Commands.DeleteCategory;
using Ecommerce.Application.Features.Categories.Commands.UpdateCategory;
using Ecommerce.Application.Features.Categories.Queries.GetCategory;
using Ecommerce.Application.Features.Categories.Queries.ListCategories;
using Ecommerce.Application.Interfaces;
using Ecommerce.Infrastructure.Persistence;
using Ecommerce.Infrastructure.Persistence.Repositories;
using FluentResults;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NSubstitute;

namespace Ecommerce.Api.Tests.Category.Integration
{
    public class CategoryIntegrationTests
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMediator _mediator;
        private readonly CategoryController _controller;

        public CategoryIntegrationTests()
        {
            var options = new DbContextOptionsBuilder<ApplicationDbContext>()
                .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
                .Options;

            _dbContext = new ApplicationDbContext(options);
            _categoryRepository = new CategoryRepository(_dbContext);
            _mediator = Substitute.For<IMediator>();
            _controller = new CategoryController(_mediator);
        }

        [Fact]
        public async Task CreateCategory_ValidData_ReturnsCreatedResult()
        {
            // Arrange
            var createCategoryDto = new CreateCategoryDto { Name = "Test Category", Description = "Test Description" };
            var createCategoryCommand = new CreateCategoryCommand { Name = createCategoryDto.Name, Description = createCategoryDto.Description };
            var createCategoryResponse = new CreateCategoryResponse { CategoryId = Guid.NewGuid(), Name = createCategoryDto.Name, Description = createCategoryDto.Description };
            _mediator.Send(Arg.Any<CreateCategoryCommand>()).Returns(Result.Ok(createCategoryResponse));

            // Act
            var result = await _controller.CreateCategory(createCategoryDto);

            // Assert
            var createdAtActionResult = Assert.IsType<CreatedAtActionResult>(result);
            var returnValue = Assert.IsType<CreateCategoryResponse>(createdAtActionResult.Value);
            Assert.Equal(createCategoryResponse.CategoryId, returnValue.CategoryId);
            Assert.Equal(createCategoryResponse.Name, returnValue.Name);
            Assert.Equal(createCategoryResponse.Description, returnValue.Description);
        }

        [Fact]
        public async Task GetCategory_ExistingCategory_ReturnsOkResult()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var getCategoryQuery = new GetCategoryQuery { CategoryId = categoryId };
            var getCategoryResponse = new GetCategoryResponse { CategoryId = categoryId, Name = "Test Category", Description = "Test Description" };
            _mediator.Send(Arg.Any<GetCategoryQuery>()).Returns(Result.Ok(getCategoryResponse));

            // Act
            var result = await _controller.GetCategory(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<GetCategoryResponse>(okResult.Value);
            Assert.Equal(getCategoryResponse.CategoryId, returnValue.CategoryId);
            Assert.Equal(getCategoryResponse.Name, returnValue.Name);
            Assert.Equal(getCategoryResponse.Description, returnValue.Description);
        }

        [Fact]
        public async Task UpdateCategory_ValidData_ReturnsOkResult()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var updateCategoryDto = new UpdateCategoryDto { CategoryId = categoryId, Name = "Updated Category", Description = "Updated Description" };
            var updateCategoryCommand = new UpdateCategoryCommand { CategoryId = categoryId, Name = updateCategoryDto.Name, Description = updateCategoryDto.Description };
            var updateCategoryResponse = new UpdateCategoryResponse { Id = categoryId, Name = updateCategoryDto.Name, Description = updateCategoryDto.Description };
            _mediator.Send(Arg.Any<UpdateCategoryCommand>()).Returns(Result.Ok(updateCategoryResponse));

            // Act
            var result = await _controller.UpdateCategory(categoryId, updateCategoryDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<UpdateCategoryResponse>(okResult.Value);
            Assert.Equal(updateCategoryResponse.Id, returnValue.Id);
            Assert.Equal(updateCategoryResponse.Name, returnValue.Name);
            Assert.Equal(updateCategoryResponse.Description, returnValue.Description);
        }

        [Fact]
        public async Task DeleteCategory_ExistingCategory_ReturnsOkResult()
        {
            // Arrange
            var categoryId = Guid.NewGuid();
            var deleteCategoryCommand = new DeleteCategoryCommand { CategoryId = categoryId };
            var deleteCategoryResponse = new DeleteCategoryResponse { CategoryId = categoryId, IsDeleted = true };
            _mediator.Send(Arg.Any<DeleteCategoryCommand>()).Returns(Result.Ok(deleteCategoryResponse));

            // Act
            var result = await _controller.DeleteCategory(categoryId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<DeleteCategoryResponse>(okResult.Value);
            Assert.Equal(deleteCategoryResponse.CategoryId, returnValue.CategoryId);
            Assert.True(returnValue.IsDeleted);
        }

        [Fact]
        public async Task ListCategories_ReturnsOkResult()
        {
            // Arrange
            var listCategoriesDto = new ListCategoriesDto { PageNumber = 1, PageSize = 10 };
            var listCategoriesQuery = new ListCategoriesQuery { PageNumber = listCategoriesDto.PageNumber, PageSize = listCategoriesDto.PageSize };
            var listCategoriesResponse = new ListCategoriesResponse
            {
                Categories = new List<CategoryDetails> { new CategoryDetails { Id = Guid.NewGuid(), Name = "Test Category", Description = "Test Description" } },
                TotalCount = 1,
                PageNumber = 1,
                PageSize = 10
            };
            _mediator.Send(Arg.Any<ListCategoriesQuery>()).Returns(Result.Ok(listCategoriesResponse));

            // Act
            var result = await _controller.ListCategories(listCategoriesDto);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnValue = Assert.IsType<ListCategoriesResponse>(okResult.Value);
            Assert.Single(returnValue.Categories);
            Assert.Equal(listCategoriesResponse.TotalCount, returnValue.TotalCount);
            Assert.Equal(listCategoriesResponse.PageNumber, returnValue.PageNumber);
            Assert.Equal(listCategoriesResponse.PageSize, returnValue.PageSize);
        }
    }
}