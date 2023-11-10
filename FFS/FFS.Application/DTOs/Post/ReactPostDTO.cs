namespace FFS.Application.DTOs.Post
{
    public class ReactPostDTO
    {
        public string UserId { get; set; }
        public int? PostId { get; set; }
        public bool IsLike { get; set; }
    }
}
