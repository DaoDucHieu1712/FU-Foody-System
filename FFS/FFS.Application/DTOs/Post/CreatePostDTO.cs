namespace FFS.Application.DTOs.Post
{
    public class CreatePostDTO
    {
        public string Title { get; set; }
        public string Content { get; set; }
        public string? Image { get; set; }
        public string UserId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool IsDelete { get; set; }
    }
}
