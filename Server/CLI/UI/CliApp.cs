using System;
using System.Threading.Tasks;
using CLI.UI.ManagePosts;
using RepositoryContracts;

public class CliApp
{
    private readonly ManageUsersView _manageUsersView;
    private readonly ManagePostsView _managePostsView;

    public CliApp(IUserRepository userRepository, IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _manageUsersView = new ManageUsersView(userRepository);
        _managePostsView = new ManagePostsView(postRepository, commentRepository, userRepository);
    }

    public async Task StartAsync()
    {
        bool exit = false;

        while (!exit)
        {
            Console.WriteLine("=== Main Menu ===");
            Console.WriteLine("1. Manage Users");
            Console.WriteLine("2. Manage Posts");
            Console.WriteLine("3. Exit");

            Console.Write("Choose an option: ");
            string option = Console.ReadLine();

            switch (option)
            {
                case "1":
                    // Delegate to ManageUsersView for user management tasks
                    await _manageUsersView.DisplayUserManagementMenuAsync();
                    break;
                case "2":
                    // Delegate to ManagePostsView for post management tasks
                    await _managePostsView.DisplayPostManagementMenuAsync();
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