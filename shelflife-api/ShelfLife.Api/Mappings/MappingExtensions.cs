namespace ShelfLife.Api.Mappings;

using ShelfLife.Api.Dtos;
using ShelfLife.Api.Models;

public static class MappingExtensions
{
    /// <summary>
    /// Maps a User entity to UserDto, excluding sensitive data like PasswordHash
    /// </summary>
    /// <param name="user">The User entity to map</param>
    /// <returns>UserDto with mapped properties</returns>
    /// <exception cref="ArgumentNullException">Thrown when user is null</exception>
    public static UserDto ToDto(this User user)
    {
        ArgumentNullException.ThrowIfNull(user);
        
        return new UserDto(
            Id: user.Id,
            Email: user.Email,
            FirstName: user.FirstName,
            LastName: user.LastName,
            CreatedAt: user.CreatedAt,
            UpdatedAt: user.UpdatedAt
        );
    }
}