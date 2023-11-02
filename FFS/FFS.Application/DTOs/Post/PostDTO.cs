namespace FFS.Application.DTOs.Post
{
    public class PostDTO
    {
        public int Id { get; set; }
        public string? Avatar { get; set; }
        public string Username { get; set; }
        public string? Image { get; set; }
        public string Title { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDelete { get; set; }
    }
}
