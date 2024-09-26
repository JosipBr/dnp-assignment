using Entity;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class SinglePostView
{
    private readonly IPostRepository _postRepository;
    private readonly ICommentRepository _commentRepository;

    public SinglePostView(IPostRepository postRepository, ICommentRepository commentRepository)
    {
        _postRepository = postRepository;
        _commentRepository = commentRepository;
    }

    public async Task DisplayPostAsync()
    {
        Console.WriteLine("\n=== View Post ===");
        Post foundPost = null;
        int postId = 0;
        
        while (foundPost == null)
        {
            Console.WriteLine("Enter the postId you want to view:");
        
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
        
        Console.WriteLine($"Title: {foundPost.Title}");
        Console.WriteLine($"Body: {foundPost.Body}");
        Console.WriteLine($"Posted by User: {foundPost.UserId}");

        IQueryable<Comment> allComments =  _commentRepository.GetMany();
        IQueryable<Comment> postComments = allComments.Where(c => c.PostId == postId);
        
     
            Console.WriteLine("\n=== Comments ===");
            foreach (var comment in postComments)
            {
                Console.WriteLine($"\nUser ID: {comment.UserId}, Comment: {comment.Body}");
            }
        
    }
}