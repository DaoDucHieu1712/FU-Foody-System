namespace FFS.Application.DTOs.Store
{
    public class StoreRatingDTO
    {
        public string? UserId { get; set; }
        public int? StoreId { get; set; }
        public int? FoodId { get; set; }
        public int? Rate { get; set; }
        public string? Content { get; set; }
    }
}
