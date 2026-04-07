using Microsoft.EntityFrameworkCore;
using ShelfLife.Api.Models;
using Xunit;

namespace ShelfLife.Api.Tests.Models;

public class CategoryModelTests : DbContextTestBase
{
    [Fact]
    public async Task CreateCategory_WithValidData_ShouldSucceed()
    {
        // Arrange
        var name = "Fruits";
        var description = "Fresh fruits category";
        var color = "#FF5733";

        // Act
        var category = new Category
        {
            Name = name,
            Description = description,
            Color = color,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Categories.Add(category);
        await Context.SaveChangesAsync();

        // Assert
        Assert.NotNull(category);
        Assert.True(category.Id > 0);
        Assert.Equal(name, category.Name);
        Assert.Equal(description, category.Description);
        Assert.Equal(color, category.Color);
    }

    [Fact]
    public async Task CreateCategory_WithDuplicateName_ShouldThrowException()
    {
        // Arrange
        const string categoryName = "Dairy";
        await CreateTestCategoryAsync(categoryName);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            var duplicateCategory = new Category
            {
                Name = categoryName,
                Description = "Another dairy category",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            Context.Categories.Add(duplicateCategory);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateCategory_WithLongName_ShouldSucceed()
    {
        // Note: SQLite doesn't enforce length constraints by default
        // This test verifies the model accepts longer strings
        // Length validation should be handled at the API/validation layer
        
        // Arrange
        var longName = new string('a', 101);

        // Act
        var category = new Category
        {
            Name = longName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        Context.Categories.Add(category);
        await Context.SaveChangesAsync();

        // Assert
        Assert.True(category.Id > 0);
        Assert.Equal(longName, category.Name);
    }

    [Fact]
    public async Task CreateCategory_WithNullOptionalFields_ShouldSucceed()
    {
        // Arrange
        var name = "Test Category";

        // Act
        var category = new Category
        {
            Name = name,
            Description = null,
            Color = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Categories.Add(category);
        await Context.SaveChangesAsync();

        // Assert
        Assert.NotNull(category);
        Assert.True(category.Id > 0);
        Assert.Equal(name, category.Name);
        Assert.Null(category.Description);
        Assert.Null(category.Color);
    }
}