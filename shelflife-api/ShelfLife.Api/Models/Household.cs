namespace ShelfLife.Api.Models;

public class Household
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<UserHousehold> UserHouseholds { get; set; } = new List<UserHousehold>();
    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    public ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
}