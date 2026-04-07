using ShelfLife.Api.Dtos;
using ShelfLife.Api.Models;

namespace ShelfLife.Api.Services.Interfaces;

public interface IUserService
{
    Task<UserDto?> GetByIdAsync(int id);
    Task<UserDto?> GetByEmailAsync(string email);
    Task<IEnumerable<UserDto>> GetAllAsync();
    Task<UserDto> CreateAsync(CreateUserDto createUserDto);
    Task<UserDto?> UpdateAsync(int id, UpdateUserDto updateUserDto);
    Task<bool> DeleteAsync(int id);
    Task<User?> ValidateCredentialsAsync(string email, string password);
    Task<bool> EmailExistsAsync(string email);
}