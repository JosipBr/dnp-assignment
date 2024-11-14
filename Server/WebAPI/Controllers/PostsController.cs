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
    public async Task<ActionResult<PostDto>> CreatePostAsync([FromBody] CreatePostDto request)
    {
        try
        {
            // Create the post using the provided data
            Post post = new(request.Body, request.Title, request.UserId);
        
            // Add the post to the repository
            Post created = await postRepo.AddAsync(post);
        
            // Get the user associated with the post using UserId
            User user = await userRepo.GetSingleAsync(request.UserId);
        
            if (user == null)
            {
                return NotFound("User not found"); 
            }


            PostDto postDto = new PostDto
            {
                Id = created.Id,
                Title = created.Title,
                Body = created.Body,
                UserId = created.UserId,
                Likes = created.Likes,
                Dislikes = created.Dislikes,
                UserName = user.Username 
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
            User user = await userRepo.GetSingleAsync(post.UserId);
            if (post == null)
            {
                return Results.NotFound("post not found");
            }
            
            PostDto dto = new()
            {
                Id = post.Id,
                Title = post.Title,
                Body = post.Body,
                UserId = post.UserId,
                UserName = user.Username
            };
            
            return Results.Ok(dto);

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
            IQueryable<Post> queryablePosts = postRepo.GetManyAsync();
            if (!string.IsNullOrWhiteSpace(title))
            {
                queryablePosts = queryablePosts.Where(p => p.Title.Contains(title));
            }

            if (userId.HasValue)
            {
                queryablePosts = queryablePosts.Where(p => p.UserId == userId.Value);
            }

            List<PostDto> posts = queryablePosts.Select(post => new PostDto
                {
                    Id = post.Id,
                    Title = post.Title,
                    Body = post.Body,
                    UserId = post.UserId,
                    UserName = userRepo.GetSingleAsync(post.UserId).Result.Username
                })
                .ToList();
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