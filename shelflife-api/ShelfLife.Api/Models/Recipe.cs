namespace ShelfLife.Api.Models;

public class Recipe
{
    public int Id { get; set; }
    public int HouseholdId { get; set; }
    public required string Title { get; set; }
    public required string Instructions { get; set; }
    public int PrepTimeMinutes { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public Household Household { get; set; } = null!;
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
}