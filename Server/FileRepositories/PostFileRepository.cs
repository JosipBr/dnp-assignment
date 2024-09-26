using System.Text.Json;
using Entity;
using RepositoryContracts;

namespace FileRepositories;

public class PostFileRepository: IPostRepository
{
    private readonly string filePath = "posts.json";

    public PostFileRepository()
    {
        if (!File.Exists(filePath))
        {
            Console.WriteLine("Creating users.json");
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Post> AddAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        int maxId = posts.Count > 0 ? posts.Max(x => x.Id)  : 1;
        post.Id = maxId ;
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
        return post;
    }

    public async Task UpdateAsync(Post post)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        Post? Post = posts.SingleOrDefault(c=>c.Id == post.Id);
        if (Post == null)
        {
            throw new InvalidOperationException($"Post {post.Id} was not found.");
        }
        posts.Remove(Post);
        posts.Add(post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public async Task DeleteAsync(int Id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        Post? Post = posts.SingleOrDefault(c=>c.Id == Id);
        if (Post == null)
        {
            throw new InvalidOperationException($"Post {Id} was not found.");
        }
        posts.Remove(Post);
        postsAsJson = JsonSerializer.Serialize(posts);
        await File.WriteAllTextAsync(filePath, postsAsJson);
    }

    public async Task<Post> GetSingleAsync(int Id)
    {
        string postsAsJson = await File.ReadAllTextAsync(filePath);
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        Post? post = posts.SingleOrDefault(p => p.Id == Id);
        if (post == null)
        {
            throw new InvalidOperationException($"Post {Id} was not found.");
        }
        return post;
    }

    public  IQueryable<Post> GetManyAsync()
    {
        string postsAsJson =  File.ReadAllTextAsync(filePath).Result;
        List<Post> posts = JsonSerializer.Deserialize<List<Post>>(postsAsJson);
        return posts.AsQueryable();
    }
}