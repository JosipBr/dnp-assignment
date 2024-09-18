using Entity;
using RepositoryContracts;

namespace CLI.UI.ManagePosts;

public class ListPostsView
{
    private readonly IPostRepository _postRepository;

    public ListPostsView(IPostRepository postRepository)
    {
        _postRepository = postRepository;
    }

    public async Task DisplayPostsAsync()
    {
        Console.WriteLine("=== List of all posts ===");

        IQueryable<Post> posts =  _postRepository.GetManyAsync();
        
        foreach (var post in posts)
        {
            Console.WriteLine($"ID: {post.Id}, Title: {post.Title}, User ID: {post.UserId}");
        }
    }
}