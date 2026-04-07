using Microsoft.EntityFrameworkCore;
using ShelfLife.Api.Models;
using Xunit;

namespace ShelfLife.Api.Tests.Data;

public class ShelfLifeDbContextTests : DbContextTestBase
{
    [Fact]
    public async Task DbContext_ShouldCreateAllTables()
    {
        // Act & Assert - Verify all DbSets are accessible
        Assert.NotNull(Context.Users);
        Assert.NotNull(Context.Categories);
        Assert.NotNull(Context.FoodProducts);
        Assert.NotNull(Context.Households);
        Assert.NotNull(Context.UserHouseholds);
        Assert.NotNull(Context.InventoryItems);
        Assert.NotNull(Context.Recipes);
        Assert.NotNull(Context.RecipeIngredients);
        Assert.NotNull(Context.ShoppingLists);
        Assert.NotNull(Context.ShoppingListItems);

        // Verify we can perform basic operations on each entity
        var userCount = await Context.Users.CountAsync();
        var categoryCount = await Context.Categories.CountAsync();
        
        Assert.True(userCount >= 0);
        Assert.True(categoryCount >= 0);
    }

    [Fact]
    public async Task DbContext_CascadeDelete_UserShouldDeleteInventoryItems()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var category = await CreateTestCategoryAsync();
        var product = await CreateTestFoodProductAsync(category);

        var inventoryItem = new InventoryItem
        {
            UserId = user.Id,
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

        // Act - Delete the user
        Context.Users.Remove(user);
        await Context.SaveChangesAsync();

        // Assert - Inventory item should be deleted
        var remainingInventoryItems = await Context.InventoryItems
            .Where(i => i.UserId == user.Id)
            .CountAsync();

        Assert.Equal(0, remainingInventoryItems);
    }

    [Fact]
    public async Task DbContext_RestrictDelete_CategoryShouldNotDeleteWithProducts()
    {
        // Arrange
        var category = await CreateTestCategoryAsync();
        var product = await CreateTestFoodProductAsync(category);

        // Act & Assert - Should not be able to delete category with products
        var exception = await Assert.ThrowsAnyAsync<Exception>(async () =>
        {
            Context.Categories.Remove(category);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(exception);
        // Verify the product still exists
        var productExists = await Context.FoodProducts.AnyAsync(p => p.Id == product.Id);
        Assert.True(productExists);
    }

    [Fact]
    public async Task DbContext_SetNullDelete_HouseholdDeletionShouldSetNullOnShoppingLists()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var household = await CreateTestHouseholdAsync();

        var shoppingList = new ShoppingList
        {
            UserId = user.Id,
            HouseholdId = household.Id,
            Name = "Grocery List",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.ShoppingLists.Add(shoppingList);
        await Context.SaveChangesAsync();

        // Act - Delete the household
        Context.Households.Remove(household);
        await Context.SaveChangesAsync();

        // Assert - Shopping list should still exist with null household
        var remainingList = await Context.ShoppingLists
            .FirstOrDefaultAsync(sl => sl.Id == shoppingList.Id);

        Assert.NotNull(remainingList);
        Assert.Null(remainingList.HouseholdId);
    }

    [Fact]
    public async Task DbContext_UniqueConstraints_ShouldBeEnforced()
    {
        // Test unique email constraint
        var user1 = await CreateTestUserAsync("unique@example.com");
        
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            // Clear tracking to avoid conflicts
            Context.ChangeTracker.Clear();
            
            var user2 = new User
            {
                Email = "unique@example.com", // Duplicate email
                PasswordHash = "hash",
                FirstName = "Another",
                LastName = "User",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            Context.Users.Add(user2);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(exception);

        // Test unique category name constraint with fresh context to avoid conflicts
        Context.ChangeTracker.Clear();
        
        var category1 = new Category
        {
            Name = "Test Unique Category",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };        
        Context.Categories.Add(category1);
        await Context.SaveChangesAsync();
        
        var categoryException = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            // Clear tracking to avoid conflicts
            Context.ChangeTracker.Clear();
            
            var category2 = new Category
            {
                Name = "Test Unique Category", // Duplicate name
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            Context.Categories.Add(category2);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(categoryException);
    }

    [Fact]
    public async Task DbContext_ComplexQuery_ShouldExecuteCorrectly()
    {
        // Arrange
        var user = await CreateTestUserAsync("complex@example.com");
        var household = await CreateTestHouseholdAsync("Test House");
        var category = await CreateTestCategoryAsync("Fruits");
        var product = await CreateTestFoodProductAsync(category, "Apple");

        var inventoryItem = new InventoryItem
        {
            UserId = user.Id,
            FoodProductId = product.Id,
            HouseholdId = household.Id,
            Quantity = 5,
            Unit = "pieces",
            PurchaseDate = DateOnly.FromDateTime(DateTime.Today),
            ExpirationDate = DateOnly.FromDateTime(DateTime.Today.AddDays(7)),
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.InventoryItems.Add(inventoryItem);
        await Context.SaveChangesAsync();

        // Act - Complex query with joins
        var result = await Context.InventoryItems
            .Include(i => i.User)
            .Include(i => i.FoodProduct)
                .ThenInclude(p => p.Category)
            .Include(i => i.Household)
            .Where(i => i.User.Email == "complex@example.com" && 
                       i.FoodProduct.Category.Name == "Fruits")
            .Select(i => new
            {
                UserEmail = i.User.Email,
                ProductName = i.FoodProduct.Name,
                CategoryName = i.FoodProduct.Category.Name,
                HouseholdName = i.Household!.Name,
                Quantity = i.Quantity
            })
            .FirstOrDefaultAsync();

        // Assert
        Assert.NotNull(result);
        Assert.Equal("complex@example.com", result.UserEmail);
        Assert.Equal("Apple", result.ProductName);
        Assert.Equal("Fruits", result.CategoryName);
        Assert.Equal("Test House", result.HouseholdName);
        Assert.Equal(5, result.Quantity);
    }
}