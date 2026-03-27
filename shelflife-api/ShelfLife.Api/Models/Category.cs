namespace ShelfLife.Api.Models;

public class Category
{
    public int Id { get; set; }
    public required string Name { get; set; }
    public string? Description { get; set; }
    public string? Color { get; set; } // For UI theming
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<FoodProduct> FoodProducts { get; set; } = new List<FoodProduct>();
}