using Microsoft.EntityFrameworkCore;
using ShelfLife.Api.Models;
using Xunit;

namespace ShelfLife.Api.Tests.Models;

public class FoodProductModelTests : DbContextTestBase
{
    [Fact]
    public async Task CreateFoodProduct_WithValidData_ShouldSucceed()
    {
        // Arrange
        var category = await CreateTestCategoryAsync("Fruits");
        var name = "Apple";
        var brand = "Fresh Farm";
        var barcode = "123456789";

        // Act
        var product = new FoodProduct
        {
            Name = name,
            Brand = brand,
            Barcode = barcode,
            CategoryId = category.Id,
            DefaultShelfLifeDays = 14,
            ImageUrl = "https://example.com/apple.jpg",
            Description = "Fresh red apple",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.FoodProducts.Add(product);
        await Context.SaveChangesAsync();

        // Assert
        Assert.NotNull(product);
        Assert.True(product.Id > 0);
        Assert.Equal(name, product.Name);
        Assert.Equal(brand, product.Brand);
        Assert.Equal(barcode, product.Barcode);
        Assert.Equal(category.Id, product.CategoryId);
    }

    [Fact]
    public async Task CreateFoodProduct_WithInvalidCategoryId_ShouldThrowException()
    {
        // Act & Assert
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            var product = new FoodProduct
            {
                Name = "Invalid Product",
                CategoryId = 999, // Non-existent category
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            Context.FoodProducts.Add(product);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateFoodProduct_WithLongName_ShouldSucceed()
    {
        // Note: SQLite doesn't enforce length constraints by default
        // This test verifies the model accepts longer strings
        // Length validation should be handled at the API/validation layer
        
        // Arrange
        var category = await CreateTestCategoryAsync();
        var longName = new string('a', 201);

        // Act
        var product = new FoodProduct
        {
            Name = longName,
            CategoryId = category.Id,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        Context.FoodProducts.Add(product);
        await Context.SaveChangesAsync();

        // Assert
        Assert.True(product.Id > 0);
        Assert.Equal(longName, product.Name);
    }

    [Fact]
    public async Task FoodProduct_NavigationProperty_ShouldLoadCategory()
    {
        // Arrange
        var category = await CreateTestCategoryAsync("Vegetables");
        var product = await CreateTestFoodProductAsync(category, "Carrot");

        // Act
        var loadedProduct = await Context.FoodProducts
            .Include(p => p.Category)
            .FirstAsync(p => p.Id == product.Id);

        // Assert
        Assert.NotNull(loadedProduct.Category);
        Assert.Equal(category.Id, loadedProduct.Category.Id);
        Assert.Equal("Vegetables", loadedProduct.Category.Name);
    }

    [Fact]
    public async Task FoodProduct_WithNullOptionalFields_ShouldSucceed()
    {
        // Arrange
        var category = await CreateTestCategoryAsync();

        // Act
        var product = new FoodProduct
        {
            Name = "Basic Product",
            CategoryId = category.Id,
            Brand = null,
            Barcode = null,
            DefaultShelfLifeDays = null,
            ImageUrl = null,
            Description = null,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.FoodProducts.Add(product);
        await Context.SaveChangesAsync();

        // Assert
        Assert.NotNull(product);
        Assert.True(product.Id > 0);
        Assert.Null(product.Brand);
        Assert.Null(product.Barcode);
    }
}