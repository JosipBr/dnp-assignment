using System.Text.Json;
using Entity;
using RepositoryContracts;

namespace FileRepositories;

public class CommentFileRepository: ICommentRepository
{
    private readonly string filePath = "comments.json";

    public CommentFileRepository()
    {
        if (!File.Exists(filePath))
        {
            File.WriteAllText(filePath, "[]");
        }
    }

    public async Task<Comment> AddAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment>? comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        int maxId = comments.Count > 0 ? comments.Max(x => x.Id)+1  : 1;
        comment.Id = maxId;
        comments.Add(comment);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
        return comment;
    }

    public async Task UpdateAsync(Comment comment)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        Comment? Comment = comments.SingleOrDefault(c=>c.Id == comment.Id);
        if (Comment == null)
        {
            throw new InvalidOperationException($"Comment {comment.Id} was not found.");
        }
        comments.Remove(Comment);
        comments.Add(comment);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }

    public async Task DeleteAsync(int Id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        Comment? Comment = comments.SingleOrDefault(c=>c.Id == Id);
        if (Comment == null)
        {
            throw new InvalidOperationException($"Comment {Id} was not found.");
        }
        comments.Remove(Comment);
        commentsAsJson = JsonSerializer.Serialize(comments);
        await File.WriteAllTextAsync(filePath, commentsAsJson);
    }

    public async Task<Comment> GetSingleAsync(int Id)
    {
        string commentsAsJson = await File.ReadAllTextAsync(filePath);
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        Comment? comment = comments.SingleOrDefault(p => p.Id == Id);
        if (comment == null)
        {
            throw new InvalidOperationException($"Comment {Id} was not found.");
        }
        return comment;
    }

    public  IQueryable<Comment> GetMany()
    {
        string commentsAsJson =  File.ReadAllTextAsync(filePath).Result;
        List<Comment> comments = JsonSerializer.Deserialize<List<Comment>>(commentsAsJson);
        return comments.AsQueryable();
    }
}