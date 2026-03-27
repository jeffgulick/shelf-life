namespace ShelfLife.Api.Models;

public class UserHousehold
{
    public int UserId { get; set; }
    public int HouseholdId { get; set; }
    public required string Role { get; set; } // "Owner", "Member", "Guest"
    public DateTime JoinedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Household Household { get; set; } = null!;
}