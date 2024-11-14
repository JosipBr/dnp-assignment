using ApiContracts;

namespace BlazorApp.Components.Services;

public interface IUserService
{
    public Task<UserDto> AddUserAsync(CreateUserDto request);
    //public Task UpdateUserAsync(UpdateUserDto request, int id);
    public Task<List<UserDto>> GetUsersAsync();
}