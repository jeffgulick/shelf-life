namespace ShelfLife.Api.Dtos;

public record UserHouseholdDto(
    int UserId,
    int HouseholdId,
    string Role,
    DateTime JoinedAt
);

public record CreateUserHouseholdDto(
    int UserId,
    int HouseholdId,
    string Role
);

public record UpdateUserHouseholdDto(
    string Role
);