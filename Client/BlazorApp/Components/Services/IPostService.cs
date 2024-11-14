using ApiContracts.Post;
using ApiContracts.Comment;

namespace BlazorApp.Components.Services;

public interface IPostService
{
    Task<PostDto> AddPostAsync(CreatePostDto request);
    Task<List<PostDto>> GetPostsAsync();
    Task<PostDto> GetPostByIdAsync(int postId);
    Task<List<CommentDto>> GetCommentsByPostIdAsync(int postId);
    Task<CommentDto> AddCommentToPostAsync(int postId, CommentDto comment);
}