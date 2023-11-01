namespace FFS.Application.DTOs.Store
{
    public class StoreRatingDTO
    {
        public string? UserId { get; set; }
        public int? StoreId { get; set; }
        public int? Rate { get; set; }
        public string Content { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
