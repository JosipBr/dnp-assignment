using System;
using System.Threading.Tasks;
using CLI.UI.ManageUsers;
using RepositoryContracts;

public class ManageUsersView
{
    private readonly CreateUserView _createUserView;
    private readonly ListUsersView _listUsersView;

    public ManageUsersView(IUserRepository userRepository)
    {
        // Instantiate the CreateUserView and ListUsersView
        _createUserView = new CreateUserView(userRepository);
        _listUsersView = new ListUsersView(userRepository);
    }

    // Method to display the user management menu
    public async Task DisplayUserManagementMenuAsync()
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("\n=== Manage Users ===");
            Console.WriteLine("1. Create User");
            Console.WriteLine("2. List Users");
            Console.WriteLine("3. Exit");

            Console.Write("Choose an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    // Delegate to CreateUserView
                    await _createUserView.CreateNewUserAsync();
                    break;
                case "2":
                    // Delegate to ListUsersView
                    await _listUsersView.ListAllUsersAsync();
                    break;
                case "3":
                    exit = true;
                    break;
                default:
                    Console.WriteLine("Invalid option. Please try again.");
                    break;
            }
        }
    }
}