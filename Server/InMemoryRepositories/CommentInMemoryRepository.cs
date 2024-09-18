using Entity;
using RepositoryContracts;

namespace InMemoryRepositories;

public class CommentInMemoryRepository: ICommentRepository
{
    private readonly List<Comment> comments = new();

    public CommentInMemoryRepository()
    {
        // I add a bunch of dummy data.
        // The underscore is a discard, which means I don't care about the result. AddAsync returns the added comment, but I don't need it here.
        // I call .Result on the Task, because I can't make the constructor async.
        _ = AddAsync(new Comment("I love it too!", 1, 2)).Result;
        _ = AddAsync(new Comment("So true!", 1, 3)).Result;
        _ = AddAsync(new Comment("They're just too good", 1, 3)).Result;
        _ = AddAsync(new Comment("That stuff makes you fat", 1, 4)).Result;
        _ = AddAsync(new Comment("+ ratiod", 1, 4)).Result;
        
        _ = AddAsync(new Comment("WEEEEEEB!", 2, 1)).Result;
        _ = AddAsync(new Comment("Do you need a friend?", 2, 3)).Result;
        _ = AddAsync(new Comment("ME TOO!",2, 4)).Result;
        _ = AddAsync(new Comment("I only watch Pokemon, does that make me a weeb?", 2, 3)).Result;

        _ = AddAsync(new Comment("#FIRST", 3, 1)).Result;
        _ = AddAsync(new Comment("Go to a psychologist.", 3, 2)).Result;
        _ = AddAsync(new Comment("Didn't ask", 3, 4)).Result;
        _ = AddAsync(new Comment("Sounds like a you problem", 3, 4)).Result;
        
        _ = AddAsync(new Comment("I'm busy this weekend", 4, 1)).Result;
        _ = AddAsync(new Comment("Not today! It's raining :(", 4, 3)).Result;
        _ = AddAsync(new Comment("LETS GOOOOO!", 4, 2)).Result;
        _ = AddAsync(new Comment("Maybe next week.", 4, 1)).Result;
        
        _ = AddAsync(new Comment("Sooo where is the guide?", 5, 2)).Result;
        _ = AddAsync(new Comment("Pfff poor people problems.", 5, 3)).Result;
        _ = AddAsync(new Comment("Scam.", 5, 4)).Result;
        _ = AddAsync(new Comment("Kys", 5, 2)).Result;
        
        _ = AddAsync(new Comment("Orange cats are just way better.", 6, 2)).Result;
        _ = AddAsync(new Comment("I hope you find it soon", 6, 4)).Result;
        _ = AddAsync(new Comment("HOLY S#I% IT'S IN MY GARDEN", 6, 2)).Result;
        _ = AddAsync(new Comment("What color is your Buggati", 6, 1)).Result;
    }

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