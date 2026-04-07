namespace ShelfLife.Api.Dtos;

public record SavedRecipeDto(
    int UserId,
    int RecipeId,
    DateTime SavedAt
);

public record CreateSavedRecipeDto(
    int RecipeId
);