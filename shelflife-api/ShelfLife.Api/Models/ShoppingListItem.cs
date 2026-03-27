namespace ShelfLife.Api.Models;

public class ShoppingListItem
{
    public int Id { get; set; }
    public int ShoppingListId { get; set; }
    public int? FoodProductId { get; set; } // Null for custom items
    public string? CustomItemName { get; set; } // For items not in the product database
    public decimal Quantity { get; set; }
    public required string Unit { get; set; }
    public bool IsCompleted { get; set; } = false;
    public decimal? EstimatedPrice { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ShoppingList ShoppingList { get; set; } = null!;
    public FoodProduct? FoodProduct { get; set; }
}