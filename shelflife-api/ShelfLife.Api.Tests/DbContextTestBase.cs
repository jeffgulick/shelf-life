using Microsoft.EntityFrameworkCore;
using ShelfLife.Api.Data;
using ShelfLife.Api.Models;

namespace ShelfLife.Api.Tests;

public abstract class DbContextTestBase : IDisposable
{
    protected readonly ShelfLifeDbContext Context;
    private bool _disposed = false;

    protected DbContextTestBase()
    {
        var options = new DbContextOptionsBuilder<ShelfLifeDbContext>()
            .UseSqlite($"DataSource=:memory:")
            .Options;

        Context = new ShelfLifeDbContext(options);
        Context.Database.OpenConnection(); // Keep the connection open
        Context.Database.EnsureCreated();
    }

    protected async Task<User> CreateTestUserAsync(string email = "test@example.com", string firstName = "Test", string lastName = "User")
    {
        var user = new User
        {
            Email = email,
            PasswordHash = "hashedpassword123",
            FirstName = firstName,
            LastName = lastName,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Users.Add(user);
        await Context.SaveChangesAsync();
        return user;
    }

    protected async Task<Category> CreateTestCategoryAsync(string name = "Test Category", string? description = null)
    {
        var category = new Category
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Categories.Add(category);
        await Context.SaveChangesAsync();
        return category;
    }

    protected async Task<Household> CreateTestHouseholdAsync(string name = "Test Household", string? description = null)
    {
        var household = new Household
        {
            Name = name,
            Description = description,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.Households.Add(household);
        await Context.SaveChangesAsync();
        return household;
    }

    protected async Task<FoodProduct> CreateTestFoodProductAsync(Category category, string name = "Test Product")
    {
        var product = new FoodProduct
        {
            Name = name,
            Brand = "Test Brand",
            CategoryId = category.Id,
            DefaultShelfLifeDays = 7,
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };

        Context.FoodProducts.Add(product);
        await Context.SaveChangesAsync();
        return product;
    }

    public void Dispose()
    {
        if (!_disposed)
        {
            Context.Database.CloseConnection();
            Context.Dispose();
            _disposed = true;
        }
    }
}