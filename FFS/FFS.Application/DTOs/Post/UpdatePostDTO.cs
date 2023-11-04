namespace FFS.Application.DTOs.Post
{
    public class UpdatePostDTO
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string? Image { get; set; }
        public string Content { get; set; }
        public string UserId { get; set; }
        public DateTime UpdatedAt { get; set; }
    }
}
