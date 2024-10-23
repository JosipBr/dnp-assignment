using ApiContracts.Post;
using Entity;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;

namespace WebAPI.Controllers;

[ApiController]
[Route("[controller]")]
public class PostsController: ControllerBase
{
    private readonly IPostRepository postRepo;
    private readonly IUserRepository userRepo;

    public PostsController(IPostRepository postRepo, IUserRepository userRepo)
    {
        this.postRepo = postRepo;
        this.userRepo = userRepo;
    }

    [HttpPost]
    public async Task<ActionResult<Post>> CreatePostAsync([FromBody] CreatePostDto request)
    {
        try
        {
            Post post = new(request.Body, request.Title, request.UserId);
            Post created = await postRepo.AddAsync(post);
            PostDto postDto = new PostDto
            {
                Id = created.Id,
                Title = created.Title,
                Body = created.Body,
                UserId = created.UserId,
                Likes = created.Likes,
                Dislikes = created.Dislikes
            };
            
            return Created($"/Posts/{postDto.Id}", postDto);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
    //GET: /Posts/{id}
    [HttpGet("{id}")]
    public async Task<IResult> GetSinglePost([FromRoute]int id)
    {
        try
        {
            Post post = await postRepo.GetSingleAsync(id);
            if (post == null)
            {
                return Results.NotFound("post not found");
            }
            
            return Results.Ok(post);

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }
    
    //GET: /Posts?title=sample&userId=1
    [HttpGet]
    public async Task<IResult> GetAllPosts([FromQuery] string? title, [FromQuery] int? userId)
    {
        try
        {
            var posts = postRepo.GetManyAsync();
            if (!string.IsNullOrWhiteSpace(title))
            {
                posts = posts.Where(p => p.Title.Contains(title));
            }

            if (userId.HasValue)
            {
                posts = posts.Where(p => p.UserId == userId.Value);
            }
            return Results.Ok(posts);
            
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return Results.NotFound(e.Message);
        }
    }
    
    // PUT: /Posts/{id}
    [HttpPut("{id}")]
    public async Task<IActionResult> UpdatePost(int id, [FromBody] UpdatePostDto request)
    {
        try
        {
            var post = await postRepo.GetSingleAsync(id);
            if (post == null)
            {
                return NotFound(); // 404 Not Found
            }

            // Update the post fields
            post.Title = request.Title;
            post.Body = request.Body;

            await postRepo.UpdateAsync(post);

            return NoContent(); // 204 No Content
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message); // Internal Server Error
        }
    }
    
    // DELETE: /Posts/{id}
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeletePost(int id)
    {
        try
        {
            var post = await postRepo.GetSingleAsync(id);
            if (post == null)
            {
                return NotFound(); // 404 Not Found
            }

            await postRepo.DeleteAsync(id);

            return NoContent(); // 204 No Content
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message); // Internal Server Error
        }
    }

}