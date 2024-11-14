namespace ApiContracts.Comment
{
    public class CommentDto
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Body { get; set; }
        public int UserId { get; set; }
        public string AuthorName { get; set; }  
    }
}