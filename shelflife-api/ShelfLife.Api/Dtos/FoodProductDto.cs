namespace ShelfLife.Api.Dtos;

public record FoodProductDto(
    int Id,
    string Name,
    string? Brand,
    string? Barcode,
    int CategoryId,
    int? DefaultShelfLifeDays,
    string? ImageUrl,
    string? Description,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateFoodProductDto(
    string Name,
    string? Brand,
    string? Barcode,
    int CategoryId,
    int? DefaultShelfLifeDays,
    string? ImageUrl,
    string? Description
);

public record UpdateFoodProductDto(
    string Name,
    string? Brand,
    string? Barcode,
    int CategoryId,
    int? DefaultShelfLifeDays,
    string? ImageUrl,
    string? Description
);