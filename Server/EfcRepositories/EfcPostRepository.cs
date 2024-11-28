using System.Globalization;
using Entity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using RepositoryContracts;

namespace EfcRepositories;

public class EfcPostRepository: IPostRepository
{
    private readonly AppContext ctx;

    public EfcPostRepository(AppContext ctx)
    {
        this.ctx = ctx;
    }

    public async Task<Post> AddAsync(Post post)
    {
        EntityEntry<Post> entityEntry = await ctx.Posts.AddAsync(post);
        await ctx.SaveChangesAsync();
        return entityEntry.Entity;
    }

    public async Task UpdateAsync(Post post)
    {
        if (!(await ctx.Posts.AnyAsync(p => p.Id == post.Id)))
        {
            throw new KeyNotFoundException("Post with id {post.Id} not found");
        }
        
        ctx.Posts.Update(post); 
        await ctx.SaveChangesAsync();
    }

    public async Task DeleteAsync(int id)
    {
        Post? existing = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (existing == null)
        {
            throw new KeyNotFoundException("Post with id {id} not found");
        }
        ctx.Posts.Remove(existing); 
        await ctx.SaveChangesAsync();
    }
    
    public async Task<Post> GetSingleAsync(int id)
    {
        Post? post = await ctx.Posts.SingleOrDefaultAsync(p => p.Id == id);
        if (post == null)
        {
            throw new KeyNotFoundException($"Post with id {id} not found");
        }
        return post;
    }
    
    
    public IQueryable<Post> GetManyAsync()
    {
        return ctx.Posts.AsQueryable();
    }
    
}