namespace ShelfLife.Api.Models;

public class User
{
    public int Id { get; set; }
    public required string Email { get; set; }
    public required string PasswordHash { get; set; }
    public required string FirstName { get; set; }
    public required string LastName { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    // Navigation properties
    public ICollection<InventoryItem> InventoryItems { get; set; } = new List<InventoryItem>();
    public ICollection<ShoppingList> ShoppingLists { get; set; } = new List<ShoppingList>();
    public ICollection<UserHousehold> UserHouseholds { get; set; } = new List<UserHousehold>();
    public ICollection<Recipe> CreatedRecipes { get; set; } = new List<Recipe>();
    public ICollection<SavedRecipe> SavedRecipes { get; set; } = new List<SavedRecipe>();
}