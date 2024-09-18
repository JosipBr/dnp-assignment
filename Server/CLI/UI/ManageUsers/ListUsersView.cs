using Entity;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class ListUsersView
{
    private readonly IUserRepository _userRepository;

    public ListUsersView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task ListAllUsersAsync()
    {
        Console.WriteLine("=== List of all users ===");

        IQueryable<User> users = _userRepository.GetMany();

        foreach (var user in users)
        {
            Console.WriteLine($"ID: {user.Id}, Username: {user.Username}");
        }
    }
}