namespace ShelfLife.Api.Dtos;

public record ShoppingListDto(
    int Id,
    int UserId,
    int? HouseholdId,
    string Name,
    string? Description,
    bool IsCompleted,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateShoppingListDto(
    int UserId,
    int? HouseholdId,
    string Name,
    string? Description
);

public record UpdateShoppingListDto(
    string Name,
    string? Description,
    bool IsCompleted
);