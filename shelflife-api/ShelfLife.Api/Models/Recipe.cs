namespace ShelfLife.Api.Models;

public class Recipe
{
    public int Id { get; set; }
    public int CreatorId { get; set; }
    public required string Title { get; set; }
    public required string Instructions { get; set; }
    public int PrepTimeMinutes { get; set; }
    public bool IsPublic { get; set; } = false;
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public User Creator { get; set; } = null!;
    public ICollection<RecipeIngredient> RecipeIngredients { get; set; } = new List<RecipeIngredient>();
    public ICollection<SavedRecipe> SavedRecipes { get; set; } = new List<SavedRecipe>();
}