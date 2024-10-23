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

        public CommentsController(ICommentRepository commentRepo)
        {
            this.commentRepo = commentRepo;
        }

        // POST: /posts/{postId}/comments
        [HttpPost]
        public async Task<ActionResult<CommentDto>> CreateCommentAsync(int postId, [FromBody] CreateCommentDto request)
        {
            try
            {
                Comment comment = new Comment(request.Body, request.UserId, postId);
                Comment createdComment = await commentRepo.AddAsync(comment);

                CommentDto commentDto = new CommentDto
                {
                    Id = createdComment.Id,
                    Body = createdComment.Body,
                    UserId = createdComment.UserId,
                    PostId = createdComment.PostId
                };

                return Created($"/posts/{postId}/comments/{commentDto.Id}", commentDto);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
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

                // Map to DTO
                CommentDto commentDto = new CommentDto
                {
                    Id = comment.Id,
                    Body = comment.Body,
                    UserId = comment.UserId,
                    PostId = comment.PostId
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
        public async Task<ActionResult<IEnumerable<CommentDto>>> GetMany(int postId, [FromQuery] int? userId)
        {
            try
            {
                var comments =  commentRepo.GetMany();

                // Filter by post ID
                comments = comments.Where(c => c.PostId == postId);

                // Filter by user ID if provided
                if (userId.HasValue)
                {
                    comments = comments.Where(c => c.UserId == userId.Value);
                }

                var commentDtos = comments.Select(c => new CommentDto
                {
                    Id = c.Id,
                    Body = c.Body,
                    UserId = c.UserId,
                    PostId = c.PostId
                }).ToList();

                return Ok(commentDtos);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                return StatusCode(500, e.Message);
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
                    return NotFound(); // 404 Not Found
                }

                // Update comment body
                comment.Body = request.Body;
                await commentRepo.UpdateAsync(comment);

                return NoContent(); // 204 No Content
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
