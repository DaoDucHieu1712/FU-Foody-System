using FFS.Application.Entities;

namespace FFS.Application.DTOs.Food
{
    public class FoodRatingDTO
    {
        public string UserId { get; set; }
        public int FoodId { get; set; }
        public int? Rate { get; set; }
        public string? Content { get; set; }
        public ICollection<ImageCommentDTO>? Images { get; set; }
    }
}
