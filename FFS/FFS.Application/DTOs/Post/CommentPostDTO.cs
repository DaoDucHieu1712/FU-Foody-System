using FFS.Application.DTOs.Food;
using FFS.Application.Entities;

namespace FFS.Application.DTOs.Post
{
    public class CommentPostDTO
    {
        public string? UserId { get; set; }
        public int? PostId { get; set; }
        public string? Content { get; set; }
        public ICollection<ImageCommentDTO>? Images { get; set; }

    }
}
