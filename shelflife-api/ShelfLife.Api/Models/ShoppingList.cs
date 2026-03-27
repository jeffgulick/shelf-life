namespace ShelfLife.Api.Models;

public class ShoppingList
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int? HouseholdId { get; set; } // Null for personal lists
    public required string Name { get; set; }
    public string? Description { get; set; }
    public bool IsCompleted { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Household? Household { get; set; }
    public ICollection<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();
}