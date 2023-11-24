namespace FFS.Application.DTOs.Food
{
    public class AllFoodDTO
    {
        public virtual int Id { get; set; }
        public string? FoodName { get; set; }
        public string? ImageURL { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public int? CategoryId { get; set; }
        public int? StoreId { get; set; }
        public decimal? PriceAfterSale { get; set; }
        public int? SalePercent { get; set; }
    }
}
