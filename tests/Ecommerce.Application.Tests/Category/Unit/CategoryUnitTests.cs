using Ecommerce.Application.Features.Categories.Commands.CreateCategory;
using Ecommerce.Application.Features.Categories.Commands.DeleteCategory;
using Ecommerce.Application.Features.Categories.Commands.UpdateCategory;
using Ecommerce.Application.Features.Categories.Queries.GetCategory;
using Ecommerce.Application.Features.Categories.Queries.ListCategories;
using Ecommerce.Application.Interfaces;
using NSubstitute;

namespace Ecommerce.Application.Tests.Category.Unit;

public class CategoryUnitTests
{
    private readonly ICategoryRepository _categoryRepository;

    public CategoryUnitTests()
    {
        _categoryRepository = Substitute.For<ICategoryRepository>();
    }
    
    [Fact]
    public async Task CreateCategoryHandler_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var handler = new CreateCategoryCommandHandler(_categoryRepository);
        var command = new CreateCategoryCommand
        {
            Name = "Test Category",
            Description = "Test Description"
        };

        var createdCategory = new Domain.Entities.Category
        {
            CategoryId = Guid.NewGuid(),
            Name = command.Name,
            Description = command.Description
        };

        _categoryRepository.GetByNameAsync(command.Name).Returns(Task.FromResult((Domain.Entities.Category?)null));
        _categoryRepository.CreateAsync(Arg.Any<Domain.Entities.Category>()).Returns(Task.FromResult((Domain.Entities.Category?)createdCategory));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(createdCategory.CategoryId, result.Value.CategoryId);
        Assert.Equal(createdCategory.Name, result.Value.Name);
        Assert.Equal(createdCategory.Description, result.Value.Description);
    }

    [Fact]
    public async Task CreateCategoryHandler_DuplicateCategoryName_ReturnsFailureResult()
    {
        // Arrange
        var handler = new CreateCategoryCommandHandler(_categoryRepository);
        var command = new CreateCategoryCommand
        {
            Name = "Existing Category",
            Description = "Test Description"
        };

        var existingCategory = new Domain.Entities.Category
        {
            CategoryId = Guid.NewGuid(),
            Name = command.Name,
            Description = "Existing Description"
        };

        _categoryRepository.GetByNameAsync(command.Name).Returns(Task.FromResult((Domain.Entities.Category?)existingCategory));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, error => error.Message.Contains("already exists"));
    }

    [Fact]
    public async Task GetCategoryHandler_ExistingCategory_ReturnsSuccessResult()
    {
        // Arrange
        var handler = new GetCategoryQueryHandler(_categoryRepository);
        var categoryId = Guid.NewGuid();
        var query = new GetCategoryQuery { CategoryId = categoryId };

        var category = new Domain.Entities.Category
        {
            CategoryId = categoryId,
            Name = "Test Category",
            Description = "Test Description"
        };

        _categoryRepository.GetByIdAsync(categoryId).Returns(Task.FromResult((Domain.Entities.Category?)category));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(category.CategoryId, result.Value.CategoryId);
        Assert.Equal(category.Name, result.Value.Name);
        Assert.Equal(category.Description, result.Value.Description);
    }

    [Fact]
    public async Task GetCategoryHandler_NonExistentCategory_ReturnsFailureResult()
    {
        // Arrange
        var handler = new GetCategoryQueryHandler(_categoryRepository);
        var categoryId = Guid.NewGuid();
        var query = new GetCategoryQuery { CategoryId = categoryId };

        _categoryRepository.GetByIdAsync(categoryId).Returns(Task.FromResult((Domain.Entities.Category?)null));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsFailed);
        Assert.Contains(result.Errors, error => error.Message.Contains("not found"));
    }

    [Fact]
    public async Task UpdateCategoryHandler_ValidCommand_ReturnsSuccessResult()
    {
        // Arrange
        var handler = new UpdateCategoryCommandHandler(_categoryRepository);
        var categoryId = Guid.NewGuid();
        var command = new UpdateCategoryCommand
        {
            CategoryId = categoryId,
            Name = "Updated Category",
            Description = "Updated Description"
        };

        var existingCategory = new Domain.Entities.Category
        {
            CategoryId = categoryId,
            Name = "Original Category",
            Description = "Original Description"
        };

        _categoryRepository.GetByIdAsync(categoryId).Returns(Task.FromResult((Domain.Entities.Category?)existingCategory));
        _categoryRepository.UpdateAsync(Arg.Any<Domain.Entities.Category>()).Returns(Task.FromResult((Domain.Entities.Category?)existingCategory));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await _categoryRepository.Received(1).UpdateAsync(Arg.Is<Domain.Entities.Category>(c =>
            c.CategoryId == categoryId &&
            c.Name == command.Name &&
            c.Description == command.Description));
    }

    [Fact]
    public async Task DeleteCategoryHandler_ExistingCategory_ReturnsSuccessResult()
    {
        // Arrange
        var handler = new DeleteCategoryCommandHandler(_categoryRepository);
        var categoryId = Guid.NewGuid();
        var command = new DeleteCategoryCommand { CategoryId = categoryId };

        var existingCategory = new Domain.Entities.Category
        {
            CategoryId = categoryId,
            Name = "Test Category",
            Description = "Test Description"
        };

        _categoryRepository.GetByIdAsync(categoryId).Returns(Task.FromResult((Domain.Entities.Category?)existingCategory));
        _categoryRepository.DeleteAsync(Arg.Any<Domain.Entities.Category>()).Returns(Task.FromResult(true));

        // Act
        var result = await handler.Handle(command, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        await _categoryRepository.Received(1).DeleteAsync(Arg.Is<Domain.Entities.Category>(c => c.CategoryId == categoryId));
    }

    [Fact]
    public async Task ListCategoriesHandler_ReturnsSuccessResult()
    {
        // Arrange
        var handler = new ListCategoriesQueryHandler(_categoryRepository);
        var query = new ListCategoriesQuery { PageNumber = 1, PageSize = 10 };

        var categories = new List<Domain.Entities.Category>
        {
            new Domain.Entities.Category { CategoryId = Guid.NewGuid(), Name = "Category 1", Description = "Description 1" },
            new Domain.Entities.Category { CategoryId = Guid.NewGuid(), Name = "Category 2", Description = "Description 2" }
        };

        _categoryRepository.GetAllAsync(query.PageNumber, query.PageSize, null)
            .Returns(Task.FromResult((categories, 2)));

        // Act
        var result = await handler.Handle(query, CancellationToken.None);

        // Assert
        Assert.True(result.IsSuccess);
        Assert.NotNull(result.Value);
        Assert.Equal(categories.Count, result.Value.Categories.Count);
        Assert.Equal(2, result.Value.TotalCount);
        Assert.All(result.Value.Categories, (categoryDto, index) =>
        {
            Assert.Equal(categories[index].CategoryId, categoryDto.CategoryId);
            Assert.Equal(categories[index].Name, categoryDto.Name);
            Assert.Equal(categories[index].Description, categoryDto.Description);
        });
    }
}