using Entity;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class CreatePostView
{
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public CreatePostView(IPostRepository postRepository, IUserRepository userRepository)
    {
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task CreateNewPostAsync()
    {
        Console.WriteLine("\n=== Create New Post ===");
        
        Console.Write("Enter User ID: ");
        if (!int.TryParse(Console.ReadLine(), out int userId))
        {
            Console.WriteLine("Invalid User ID. Please enter a valid number.");
            return;
        }

        User user = await _userRepository.GetSingleAsync(userId);
  
        
        Console.WriteLine("Enter Title: ");
        string? title = Console.ReadLine();
        
        Console.WriteLine("Enter Description: ");
        string? description = Console.ReadLine();
        
        // Validate input
        if (string.IsNullOrWhiteSpace(title) || string.IsNullOrWhiteSpace(description))
        {
            Console.WriteLine("Title and body cannot be empty.");
            return;
        }

        Post newPost = new Post();
        
        var result = await _postRepository.AddAsync(newPost);
        
        if (result != null)
        {
            Console.WriteLine($"Post created successfully with ID: {result.Id}");
        }
        else
        {
            Console.WriteLine("Error creating post.");
        }
    }
}