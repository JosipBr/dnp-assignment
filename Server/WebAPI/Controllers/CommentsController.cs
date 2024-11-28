using ApiContracts.Comment;
using Entity;
using Microsoft.AspNetCore.Mvc;
using RepositoryContracts;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WebAPI.Controllers
{
    [ApiController]
    [Route("posts/{postId}/comments")]
    public class CommentsController : ControllerBase
    {
        private readonly ICommentRepository commentRepo;
        private readonly IUserRepository userRepo;

        public CommentsController(ICommentRepository commentRepo, IUserRepository userRepo)
        {
            this.commentRepo = commentRepo;
            this.userRepo  = userRepo;
        }

        // POST: /posts/{postId}/comments
        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateCommentAsync(int postId, [FromBody] CreateCommentDto request)
        {
            try
            {
                Comment comment = new Comment()
                {
                    PostId = postId,
                    Body = request.Body,
                    UserId = request.UserId,
                };

                Comment createdComment = await commentRepo.AddAsync(comment);

                User user = await userRepo.GetSingleAsync(request.UserId);

                if (user == null)
                {
                    return NotFound("User not found");
                }

                CommentDto commentDto = new CommentDto
                {
                    Id = createdComment.Id,
                    Body = createdComment.Body,
                    UserId = createdComment.UserId,
                    PostId = createdComment.PostId,
                    AuthorName = user.Username,
                };
                Console.WriteLine($"Created comment - web api: {commentDto.Body} by {commentDto.AuthorName}");
                return Created($"/posts/{postId}/comments/{commentDto.Id}", commentDto);
            }
            catch (Exception e)
            {
                Console.WriteLine("koji kurac");
                return StatusCode(500, e.Message); // Internal Server Error
            }
        }


        // GET: /posts/{postId}/comments/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<CommentDto>> GetComment(int postId, int id)
        {
            try
            {
                var comment = await commentRepo.GetSingleAsync(id);
                if (comment == null || comment.PostId != postId)
                {
                    return NotFound(); // 404 Not Found
                }

                // Get the user to fetch AuthorName
                var user = await userRepo.GetSingleAsync(comment.UserId);

                // Map the comment to DTO including AuthorName
                CommentDto commentDto = new CommentDto
                {
                    Id = comment.Id,
                    Body = comment.Body,
                    UserId = comment.UserId,
                    PostId = comment.PostId,
                    AuthorName = user.Username  
                };

                return Ok(commentDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }


        // GET: /posts/{postId}/comments
        [HttpGet]
        public async Task<IResult> GetMany(int postId, [FromQuery] int? userId)
        {
            try
            {
                IQueryable<Comment> queryableComments = commentRepo.GetMany();

                // Filter by postId
                queryableComments = queryableComments.Where(c => c.PostId == postId);

                // Filter by userId if provided
                if (userId.HasValue)
                {
                    queryableComments = queryableComments.Where(c => c.UserId == userId.Value);
                }

                // Fetch the AuthorName for each comment
                List<CommentDto> comments = queryableComments.Select(comment => new CommentDto
                {
                    Id = comment.Id,
                    Body = comment.Body,
                    UserId = comment.UserId,
                    PostId = comment.PostId,
                    AuthorName = userRepo.GetSingleAsync(comment.UserId).Result.Username
                }).ToList();
                return Results.Ok(comments);
                
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return Results.NotFound(e.Message);
            }
        }


        // PUT: /posts/{postId}/comments/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateComment(int postId, int id, [FromBody] UpdateCommentDto request)
        {
            try
            {
                var comment = await commentRepo.GetSingleAsync(id);
                if (comment == null || comment.PostId != postId)
                {
                    return NotFound(); 
                }

                comment.Body = request.Body;
                await commentRepo.UpdateAsync(comment);

                return NoContent(); 
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }

        // DELETE: /posts/{postId}/comments/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteComment(int postId, int id)
        {
            try
            {
                var comment = await commentRepo.GetSingleAsync(id);
                if (comment == null || comment.PostId != postId)
                {
                    return NotFound(); // 404 Not Found
                }

                await commentRepo.DeleteAsync(id);

                return NoContent(); // 204 No Content
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
            }
        }
    }
}
