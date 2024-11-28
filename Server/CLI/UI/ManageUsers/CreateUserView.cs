using Entity;
using RepositoryContracts;

namespace CLI.UI.ManageUsers;

public class CreateUserView
{
    private readonly IUserRepository _userRepository;

    public CreateUserView(IUserRepository userRepository)
    {
        _userRepository = userRepository;
    }

    public async Task CreateNewUserAsync()
    {
        Console.WriteLine("\n=== Create New User ===");

        Console.Write("Enter username: ");
        string userName = Console.ReadLine();

        Console.Write("Enter password: ");
        string password = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(userName) || string.IsNullOrWhiteSpace(password))
        {
            Console.WriteLine("Invalid input. Username and password cannot be empty.");
            return;
        }

        User newUser = new User();
        
        User result = await _userRepository.AddAsync(newUser);

        if (result != null)
        {
            Console.WriteLine($"User created successfully with ID: {result.Id}");
        }
        else
        {
            Console.WriteLine("Error creating user.");
        }
    }
}