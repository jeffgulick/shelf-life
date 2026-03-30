namespace ShelfLife.Api.Dtos;

public record CategoryDto(
    int Id,
    string Name,
    string? Description,
    string? Color,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateCategoryDto(
    string Name,
    string? Description,
    string? Color
);

public record UpdateCategoryDto(
    string Name,
    string? Description,
    string? Color
);