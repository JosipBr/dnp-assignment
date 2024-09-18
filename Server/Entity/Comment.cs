namespace Entity;

public class Comment
{
    public int Id { get; set; }
    public string Body { get; set; }
    public int PostId { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public int UserId { get; set; }

    public Comment(string body, int postId, int userId)
    {
        Body = body;
        PostId = postId;
        UserId = userId;
        Likes = 0;
        Dislikes = 0;
    }
}