namespace FFS.Application.DTOs.Store
{
    public class ExportFoodDTO
    {
        public int Id { get; set; }
        public string? FoodName { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string CategoryName { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
