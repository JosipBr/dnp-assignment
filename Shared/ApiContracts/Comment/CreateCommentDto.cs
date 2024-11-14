namespace ApiContracts.Comment
{
    public class CreateCommentDto
    {
        public string Body { get; set; }
        public int UserId { get; set; }
    }
}