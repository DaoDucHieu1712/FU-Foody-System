namespace FFS.Application.DTOs.Comment
{
    public class CommentDTO
    {
        public string? UserId { get; set; }
        public string Username { get; set; }
        public string Avatar { get; set; }
        public string? ShipperId { get; set; }
        public string? NoteForShipper { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
