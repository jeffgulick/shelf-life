namespace ShelfLife.Api.Dtos;

public record RecipeDto(
    int Id,
    int CreatorId,
    string Title,
    string Instructions,
    int PrepTimeMinutes,
    bool IsPublic,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateRecipeDto(
    string Title,
    string Instructions,
    int PrepTimeMinutes,
    bool? IsPublic = false
);

public record UpdateRecipeDto(
    string Title,
    string Instructions,
    int PrepTimeMinutes,
    bool IsPublic
);