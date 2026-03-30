namespace ShelfLife.Api.Dtos;

public record RecipeDto(
    int Id,
    int HouseholdId,
    string Title,
    string Instructions,
    int PrepTimeMinutes,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateRecipeDto(
    int HouseholdId,
    string Title,
    string Instructions,
    int PrepTimeMinutes
);

public record UpdateRecipeDto(
    string Title,
    string Instructions,
    int PrepTimeMinutes
);