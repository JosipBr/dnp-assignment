using Entity;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository: ICommentRepository
{
    private readonly List<Comment> comments = new();

    
    
    public Task<Comment> AddAsync(Comment comment)
    {
        comment.Id = comments.Any()
            ? comments.Max(p => p.Id) + 1
            : 1;
        comments.Add(comment);
        return Task.FromResult(comment);
    }

    public Task UpdateAsync(Comment comment)
    {
        Comment? Comment = comments.SingleOrDefault(p=>p.Id == comment.Id);
        if (Comment == null)
        {
            throw new InvalidOperationException($"Comment {comment.Id} was not found.");
        }

        comments.Remove(Comment);
        comments.Add(comment);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(int Id)
    {
        Comment? commentToRemove = comments.SingleOrDefault(p => p.Id == Id);
        if (commentToRemove == null)
        {
            throw new InvalidOperationException($"Comment {Id} was not found.");
        }
        comments.Remove(commentToRemove);
        return Task.CompletedTask;
    }

    public Task<Comment> GetSingleAsync(int Id)
    {
        Comment? comment = comments.SingleOrDefault(p => p.Id == Id);
        if (comment == null)
        {
            throw new InvalidOperationException($"Comment {Id} was not found.");
        }
        return Task.FromResult(comment);
    }

    public IQueryable<Comment> GetMany()
    {
        return comments.AsQueryable();
    }
}