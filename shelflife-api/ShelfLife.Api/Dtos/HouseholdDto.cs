namespace ShelfLife.Api.Dtos;

public record HouseholdDto(
    int Id,
    string Name,
    string? Description,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateHouseholdDto(
    string Name,
    string? Description
);

public record UpdateHouseholdDto(
    string Name,
    string? Description
);