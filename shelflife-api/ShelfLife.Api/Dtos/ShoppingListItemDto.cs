namespace ShelfLife.Api.Dtos;

public record ShoppingListItemDto(
    int Id,
    int ShoppingListId,
    int? FoodProductId,
    string? CustomItemName,
    decimal Quantity,
    string Unit,
    bool IsCompleted,
    decimal? EstimatedPrice,
    string? Notes,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateShoppingListItemDto(
    int ShoppingListId,
    int? FoodProductId,
    string? CustomItemName,
    decimal Quantity,
    string Unit,
    decimal? EstimatedPrice,
    string? Notes
);

public record UpdateShoppingListItemDto(
    int? FoodProductId,
    string? CustomItemName,
    decimal Quantity,
    string Unit,
    bool IsCompleted,
    decimal? EstimatedPrice,
    string? Notes
);