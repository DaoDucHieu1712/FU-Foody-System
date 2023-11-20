namespace FFS.Application.DTOs.Post
{
    public class CreateReactPostDTO
    {
        public string UserId { get; set; }
        public int? PostId { get; set; }
        public bool IsLike { get; set; }
    }
}
