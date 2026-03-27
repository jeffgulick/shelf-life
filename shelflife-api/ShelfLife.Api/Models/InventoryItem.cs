namespace ShelfLife.Api.Models;

public class InventoryItem
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public int FoodProductId { get; set; }
    public int? HouseholdId { get; set; } // Null for personal items
    public decimal Quantity { get; set; }
    public required string Unit { get; set; } // "pieces", "lbs", "oz", "liters", etc.
    public DateOnly PurchaseDate { get; set; }
    public DateOnly ExpirationDate { get; set; }
    public string? Location { get; set; } // "Fridge", "Pantry", "Freezer", etc.
    public string? Notes { get; set; }
    public bool IsConsumed { get; set; } = false; // Soft delete when item is used up
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public FoodProduct FoodProduct { get; set; } = null!;
    public Household? Household { get; set; }
}