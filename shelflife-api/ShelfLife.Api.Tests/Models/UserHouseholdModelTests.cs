using Microsoft.EntityFrameworkCore;
using ShelfLife.Api.Models;
using Xunit;

namespace ShelfLife.Api.Tests.Models;

public class UserHouseholdModelTests : DbContextTestBase
{
    [Fact]
    public async Task CreateUserHousehold_WithValidData_ShouldSucceed()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var household = await CreateTestHouseholdAsync();

        // Act
        var userHousehold = new UserHousehold
        {
            UserId = user.Id,
            HouseholdId = household.Id,
            Role = "Owner",
            JoinedAt = DateTime.UtcNow
        };

        Context.UserHouseholds.Add(userHousehold);
        await Context.SaveChangesAsync();

        // Assert
        Assert.Equal(user.Id, userHousehold.UserId);
        Assert.Equal(household.Id, userHousehold.HouseholdId);
        Assert.Equal("Owner", userHousehold.Role);
    }

    [Theory]
    [InlineData("Owner")]
    [InlineData("Member")]
    [InlineData("Guest")]
    public async Task CreateUserHousehold_WithValidRoles_ShouldSucceed(string role)
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var household = await CreateTestHouseholdAsync();

        // Act
        var userHousehold = new UserHousehold
        {
            UserId = user.Id,
            HouseholdId = household.Id,
            Role = role,
            JoinedAt = DateTime.UtcNow
        };

        Context.UserHouseholds.Add(userHousehold);
        await Context.SaveChangesAsync();

        // Assert
        Assert.Equal(role, userHousehold.Role);
    }

    [Fact]
    public async Task CreateUserHousehold_WithDuplicateUserAndHousehold_ShouldThrowException()
    {
        // Arrange
        var user = await CreateTestUserAsync();
        var household = await CreateTestHouseholdAsync();

        var firstUserHousehold = new UserHousehold
        {
            UserId = user.Id,
            HouseholdId = household.Id,
            Role = "Owner",
            JoinedAt = DateTime.UtcNow
        };

        Context.UserHouseholds.Add(firstUserHousehold);
        await Context.SaveChangesAsync();

        // Clear tracking to avoid EF Core tracking conflict
        Context.ChangeTracker.Clear();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            var duplicateUserHousehold = new UserHousehold
            {
                UserId = user.Id,
                HouseholdId = household.Id,
                Role = "Member", // Different role, but same user-household combination
                JoinedAt = DateTime.UtcNow
            };
            Context.UserHouseholds.Add(duplicateUserHousehold);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateUserHousehold_WithInvalidUserId_ShouldThrowException()
    {
        // Arrange
        var household = await CreateTestHouseholdAsync();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            var userHousehold = new UserHousehold
            {
                UserId = 999, // Non-existent user
                HouseholdId = household.Id,
                Role = "Owner",
                JoinedAt = DateTime.UtcNow
            };
            Context.UserHouseholds.Add(userHousehold);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task CreateUserHousehold_WithInvalidHouseholdId_ShouldThrowException()
    {
        // Arrange
        var user = await CreateTestUserAsync();

        // Act & Assert
        var exception = await Assert.ThrowsAsync<DbUpdateException>(async () =>
        {
            var userHousehold = new UserHousehold
            {
                UserId = user.Id,
                HouseholdId = 999, // Non-existent household
                Role = "Owner",
                JoinedAt = DateTime.UtcNow
            };
            Context.UserHouseholds.Add(userHousehold);
            await Context.SaveChangesAsync();
        });

        Assert.NotNull(exception);
    }

    [Fact]
    public async Task UserHousehold_NavigationProperties_ShouldLoad()
    {
        // Arrange
        var user = await CreateTestUserAsync("owner@example.com", "John", "Doe");
        var household = await CreateTestHouseholdAsync("The Doe Family");

        var userHousehold = new UserHousehold
        {
            UserId = user.Id,
            HouseholdId = household.Id,
            Role = "Owner",
            JoinedAt = DateTime.UtcNow
        };

        Context.UserHouseholds.Add(userHousehold);
        await Context.SaveChangesAsync();

        // Act
        var loadedUserHousehold = await Context.UserHouseholds
            .Include(uh => uh.User)
            .Include(uh => uh.Household)
            .FirstAsync(uh => uh.UserId == user.Id && uh.HouseholdId == household.Id);

        // Assert
        Assert.NotNull(loadedUserHousehold.User);
        Assert.NotNull(loadedUserHousehold.Household);
        Assert.Equal("owner@example.com", loadedUserHousehold.User.Email);
        Assert.Equal("The Doe Family", loadedUserHousehold.Household.Name);
    }
}