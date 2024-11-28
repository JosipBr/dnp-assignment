using Entity;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class AddCommentView
{
    private readonly ICommentRepository _commentRepository;
    private readonly IPostRepository _postRepository;
    private readonly IUserRepository _userRepository;

    public AddCommentView(ICommentRepository commentRepository, IPostRepository postRepository, IUserRepository userRepository)
    {
        _commentRepository = commentRepository;
        _postRepository = postRepository;
        _userRepository = userRepository;
    }

    public async Task AddCommentAsync()
    {
        Console.WriteLine("\n=== Add a Comment ===");

        int postId = 0;
        Post foundPost=null;

        while (foundPost == null)
        {
            Console.WriteLine("Enter the postId you want to comment on:");

            if (!int.TryParse(Console.ReadLine(), out postId))
            {
                Console.WriteLine("Invalid input. Please enter a valid post ID.");
                continue;
            }

            IQueryable<Post> allPosts = _postRepository.GetManyAsync();
            foreach (var post in allPosts)
            {
                if (post.Id == postId)
                {
                    foundPost = await _postRepository.GetSingleAsync(postId);
                    break;
                }
            }

            if (foundPost == null)
            {
                Console.WriteLine("Post not found. Please try again.");
            }
        }

        int userId=0;
        User foundUser = null;
        while (foundUser == null)
        {
            Console.WriteLine("Enter the userId that would leave a comment:");
        
            if (!int.TryParse(Console.ReadLine(), out userId))
            {
                Console.WriteLine("Invalid input. Please enter a valid user ID.");
                continue;
            }

            IQueryable<User> allUsers = _userRepository.GetMany();
            foreach (var user in allUsers)
            {
                if (user.Id == userId)
                {
                    foundUser = await _userRepository.GetSingleAsync(userId);
                    break; 
                }
            }

            if (foundUser == null)
            {
                Console.WriteLine("User not found. Please try again.");
            }
        }

        string commentBody;
        while (true)
        {
            Console.Write("Enter your comment: ");
            commentBody = Console.ReadLine();
            if (!string.IsNullOrWhiteSpace(commentBody))
            {
                break;
            }
            else
            {
                Console.WriteLine("Comment cannot be empty. Please try again.");
            }
        }

        Comment comment = new Comment();

        var result = await _commentRepository.AddAsync(comment);

        if (result != null)
        {
            Console.WriteLine("Comment added successfully.");
        }
        else
        {
            Console.WriteLine("Error adding comment.");
        }
    }
}
