namespace ShelfLife.Api.Models;

public class RecipeIngredient
{
    public int Id { get; set; }
    public int RecipeId { get; set; }
    public int FoodProductId { get; set; }
    public decimal Quantity { get; set; }
    public required string UnitOfMeasure { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Recipe Recipe { get; set; } = null!;
    public FoodProduct FoodProduct { get; set; } = null!;
}