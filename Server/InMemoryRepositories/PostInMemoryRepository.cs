using Entity;
using RepositoryContracts;

namespace InMemoryRepositories;

public class PostInMemoryRepository: IPostRepository
{
    private readonly List<Post> posts = new();

    public PostInMemoryRepository()
    {
        _ = AddAsync(new Post("I love pizza", "Pizza is SOOO goood trust me bro", 1)).Result;
        _ = AddAsync(new Post( "I love anime", "I am a weeb", 2)).Result;
        _ = AddAsync(new Post("I am depressed", "I feel down 24/7", 3)).Result;
        _ = AddAsync(new Post("Lets go party", "I NEED TO DRINK BROOOO", 4)).Result;
        _ = AddAsync(new Post("How to make money", "This post will teach you how to make money", 1)).Result;
        _ = AddAsync(new Post("I lost my cat", "My black and white cat ran away from my apartment yesterday. Has anyone seen it? ", 3)).Result;
    }

    public Task<Post> AddAsync(Post post)
    {
        post.Id = posts.Any()
            ? posts.Max(p => p.Id) + 1
            : 1;
        posts.Add(post);
        return Task.FromResult(post);
    }

    public Task UpdateAsync(Post post)
    {
        Post? existingPost = posts.SingleOrDefault(p=>p.Id == post.Id);
        if (existingPost == null)
        {
            throw new InvalidOperationException($"Post {post.Id} was not found.");
        }

        posts.Remove(existingPost);
        posts.Add(post);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int Id)
    {
        Post? postToRemove = posts.SingleOrDefault(p => p.Id == Id);
        if (postToRemove == null)
        {
            throw new InvalidOperationException($"Post {Id} was not found.");
        }
        posts.Remove(postToRemove);
        return Task.CompletedTask;
    }

    public Task<Post> GetSingleAsync(int Id)
    {
        Post? post = posts.SingleOrDefault(p => p.Id == Id);
        if (post == null)
        {
            throw new InvalidOperationException($"Post {Id} was not found.");
        }
        return Task.FromResult(post);
    }

    public IQueryable<Post> GetManyAsync()
    {
        return posts.AsQueryable();
    }
}