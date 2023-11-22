namespace FFS.Application.DTOs.Food
{
    public class FoodFlashSaleDTO
    {
        public int Id { get; set; }
        public string FoodName { get; set; }
        public string ImageURL { get; set; }
        public int Quantity { get; set; }
        public decimal Price { get; set; }
        public string CategoryName { get; set; }
    }
}
