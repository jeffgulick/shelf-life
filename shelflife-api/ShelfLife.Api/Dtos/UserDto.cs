namespace ShelfLife.Api.Dtos;

public record UserDto(
    int Id,
    string Email,
    string FirstName,
    string LastName,
    DateTime CreatedAt,
    DateTime UpdatedAt
);

public record CreateUserDto(
    string Email,
    string Password,
    string FirstName,
    string LastName
);

public record UpdateUserDto(
    string Email,
    string FirstName,
    string LastName
);