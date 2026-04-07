using Microsoft.EntityFrameworkCore;
using ShelfLife.Api.Models;
using Xunit;

namespace ShelfLife.Api.Tests.Models;

public class InventoryItemModelTests : DbContextTestBase
{
    [Fact]
    public async Task CreateInventoryItem_WithValidData_ShouldSucceed()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var category = await CreateTestCategoryAsync();
        var product = await CreateTestFoodProductAsync(category);
        var household = await CreateTestHouseholdAsync();

        // Act
        var inventoryItem = new InventoryItem
        {
            UserId = user.Id,
            FoodProductId = product.Id,
            HouseholdId = household.Id,
            Quantity = 2.5m,
            Unit = "pounds",
            PurchaseDate = DateOnly.FromDateTime(DateTime.Today),
            ExpirationDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7)),
            Location = "Refrigerator",
            Notes = "Organic apples",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.InventoryItems.Add(inventoryItem);
        await Context.SaveChangesAsync();

        // Assert
        Assert.NotNull(inventoryItem);
        Assert.True(inventoryItem.Id > 0);
        Assert.Equal(user.Id, inventoryItem.UserId);
        Assert.Equal(product.Id, inventoryItem.FoodProductId);
        Assert.Equal(2.5m, inventoryItem.Quantity);
        Assert.Equal("pounds", inventoryItem.Unit);
    }

    [Fact]
    public async Task CreateInventoryItem_WithInvalidUserId_ShouldThrowException()
    {
        // Arrange
        var category = await CreateTestCategoryAsync();
        var product = await CreateTestFoodProductAsync(category);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            var inventoryItem = new InventoryItem
            {
                UserId = 999, // Non-existent user
                FoodProductId = product.Id,
                Quantity = 1,
                Unit = "piece",
                PurchaseDate = DateOnly.FromDateTime(DateTime.Today),
                ExpirationDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            Context.InventoryItems.Add(inventoryItem);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateInventoryItem_WithInvalidFoodProductId_ShouldThrowException()
    {
        // Arrange
        var user = await CreateTestUserAsync();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            var inventoryItem = new InventoryItem
            {
                UserId = user.Id,
                FoodProductId = 999, // Non-existent product
                Quantity = 1,
                Unit = "piece",
                PurchaseDate = DateOnly.FromDateTime(DateTime.Today),
                ExpirationDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            Context.InventoryItems.Add(inventoryItem);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task InventoryItem_ShouldSupportNullHousehold()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var category = await CreateTestCategoryAsync();
        var product = await CreateTestFoodProductAsync(category);

        // Act
        var inventoryItem = new InventoryItem
        {
            UserId = user.Id,
            FoodProductId = product.Id,
            HouseholdId = null, // Personal item
            Quantity = 1,
            Unit = "piece",
            PurchaseDate = DateOnly.FromDateTime(DateTime.Today),
            ExpirationDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.InventoryItems.Add(inventoryItem);
        await Context.SaveChangesAsync();

        // Assert
        Assert.NotNull(inventoryItem);
        Assert.True(inventoryItem.Id > 0);
        Assert.Null(inventoryItem.HouseholdId);
    }

    [Fact]
    public async Task InventoryItem_NavigationProperties_ShouldLoad()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var category = await CreateTestCategoryAsync("Fruits");
        var product = await CreateTestFoodProductAsync(category, "Apple");
        var household = await CreateTestHouseholdAsync("Family");

        var inventoryItem = new InventoryItem
        {
            UserId = user.Id,
            FoodProductId = product.Id,
            HouseholdId = household.Id,
            Quantity = 1,
            Unit = "piece",
            PurchaseDate = DateOnly.FromDateTime(DateTime.Today),
            ExpirationDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1)),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.InventoryItems.Add(inventoryItem);
        await Context.SaveChangesAsync();

        // Act
        var loadedItem = await Context.InventoryItems
            .Include(i => i.User)
            .Include(i => i.FoodProduct)
            .ThenInclude(p => p.Category)
            .Include(i => i.Household)
            .FirstAsync(i => i.Id == inventoryItem.Id);

        // Assert
        Assert.NotNull(loadedItem.User);
        Assert.NotNull(loadedItem.FoodProduct);
        Assert.NotNull(loadedItem.Household);
        Assert.Equal("test@example.com", loadedItem.User.Email);
        Assert.Equal("Apple", loadedItem.FoodProduct.Name);
        Assert.Equal("Family", loadedItem.Household.Name);
    }
}