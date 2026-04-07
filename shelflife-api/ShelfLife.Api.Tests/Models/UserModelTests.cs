using Microsoft.EntityFrameworkCore;
using ShelfLife.Api.Models;
using Xunit;

namespace ShelfLife.Api.Tests.Models;

public class UserModelTests : DbContextTestBase
{
    [Fact]
    public async Task CreateUser_WithValidData_ShouldSucceed()
    {
        // Arrange
        var email = "user@example.com";
        var firstName = "John";
        var lastName = "Doe";

        // Act
        var user = await CreateTestUserAsync(email, firstName, lastName);

        // Assert
        Assert.NotNull(user);
        Assert.True(user.Id > 0);
        Assert.Equal(email, user.Email);
        Assert.Equal(firstName, user.FirstName);
        Assert.Equal(lastName, user.LastName);
    }

    [Fact]
    public async Task CreateUser_WithDuplicateEmail_ShouldThrowException()
    {
        // Arrange
        const string email = "duplicate@example.com";
        await CreateTestUserAsync(email);

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            var duplicateUser = new User
            {
                Email = email,
                PasswordHash = "hash",
                FirstName = "Another",
                LastName = "User",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow
            };
            Context.Users.Add(duplicateUser);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateUser_WithValidLongEmail_ShouldSucceed()
    {
        // Note: SQLite doesn't enforce length constraints by default
        // This test verifies the model accepts longer strings
        // Length validation should be handled at the API/validation layer
        
        // Arrange
        var longEmail = new string('a', 250) + "@example.com";

        // Act
        var user = new User
        {
            Email = longEmail,
            PasswordHash = "hash",
            FirstName = "Test",
            LastName = "User",
            CreatedAt = DateTime.UtcNow,
            UpdatedAt = DateTime.UtcNow
        };
        Context.Users.Add(user);
        await Context.SaveChangesAsync();

        // Assert
        Assert.True(user.Id > 0);
        Assert.Equal(longEmail, user.Email);
    }
}