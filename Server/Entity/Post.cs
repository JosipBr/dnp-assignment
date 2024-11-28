namespace Entity;

public class Post
{
    public int Id { get; set; }
    public string Title { get; set; }
    public string Body { get; set; }
    public User User { get; set; }
    public int UserId { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public List<Comment> Comments { get; set; }

    public Post()
    {
        
    }
}