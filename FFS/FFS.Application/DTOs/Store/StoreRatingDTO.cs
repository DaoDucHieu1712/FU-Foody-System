namespace FFS.Application.DTOs.Store
{
    public class StoreRatingDTO
    {
        public string? Email { get; set; }
        public int? StoreId { get; set; }
        public int? Rate { get; set; }
        public string? Content { get; set; }
        public string? ShipperId { get; set; }
        public string? NoteForShipper { get; set; }
        public int? ParentCommentId { get; set; }
        public DateTime UpdatedAt { get; set; } = DateTime.Now;
    }
}
