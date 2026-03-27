namespace ShelfLife.Api.Models;

public class FoodProduct
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Brand { get; set; }
    public string? Barcode { get; set; }
    public int CategoryId { get; set; }
    public int? DefaultShelfLifeDays { get; set; } // Typical shelf life in days
    public string? ImageUrl { get; set; }
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Category Category { get; set; } = null!;
    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    public ICollection<ShoppingListItem> ShoppingListItems { get; set; } = new List<ShoppingListItem>();
}