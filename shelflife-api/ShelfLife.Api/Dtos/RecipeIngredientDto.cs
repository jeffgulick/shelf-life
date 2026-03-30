namespace ShelfLife.Api.Dtos;

public record RecipeIngredientDto(
    int Id,
    int RecipeId,
    int FoodProductId,
    decimal Quantity,
    string UnitOfMeasure,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateRecipeIngredientDto(
    int RecipeId,
    int FoodProductId,
    decimal Quantity,
    string UnitOfMeasure
);

public record UpdateRecipeIngredientDto(
    int FoodProductId,
    decimal Quantity,
    string UnitOfMeasure
);