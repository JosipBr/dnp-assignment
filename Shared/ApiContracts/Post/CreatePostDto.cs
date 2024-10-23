namespace ApiContracts.Post;

public class CreatePostDto
{
    public required string Title { get; set; }
    public required string Body { get; set; }
    public required int UserId { get; set; } // ID of the user creating the post
}