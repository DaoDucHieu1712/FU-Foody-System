using FFS.Application.Entities;

namespace FFS.Application.DTOs.Food
{
    public class FoodRatingDTO
    {
        public string? UserId { get; set; }
        public List<FoodRatingItem>? FoodRatings { get; set; }
        public string? ShipperId { get; set; }
        public string? NoteForShipper { get; set; }
        public string? Content { get; set; }
        public ICollection<ImageFoodRatingDTO>? Images { get; set; }
    }

    public class FoodRatingItem
    {
        public int? FoodId { get; set; }
        public int? Rate { get; set; }
    }
}
