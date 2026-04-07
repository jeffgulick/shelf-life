namespace ShelfLife.Api.Models;

public class SavedRecipe
{
    public int UserId { get; set; }
    public int RecipeId { get; set; }
    public DateTime SavedAt { get; set; }

    // Navigation properties
    public User User { get; set; } = null!;
    public Recipe Recipe { get; set; } = null!;
}