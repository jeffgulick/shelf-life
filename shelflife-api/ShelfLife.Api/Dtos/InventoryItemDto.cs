namespace ShelfLife.Api.Dtos;

public record InventoryItemDto(
    int Id,
    int UserId,
    int FoodProductId,
    int? HouseholdId,
    decimal Quantity,
    string Unit,
    DateOnly PurchaseDate,
    DateOnly ExpirationDate,
    string? Location,
    string? Notes,
    bool IsConsumed,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateInventoryItemDto(
    int UserId,
    int FoodProductId,
    int? HouseholdId,
    decimal Quantity,
    string Unit,
    DateOnly PurchaseDate,
    DateOnly ExpirationDate,
    string? Location,
    string? Notes
);

public record UpdateInventoryItemDto(
    decimal Quantity,
    string Unit,
    DateOnly PurchaseDate,
    DateOnly ExpirationDate,
    string? Location,
    string? Notes,
    bool IsConsumed
);