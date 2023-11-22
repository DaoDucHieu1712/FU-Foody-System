namespace FFS.Application.DTOs.QueryParametter
{
    public class CheckFoodFlashSaleParameters : QueryStringParameters
    {
        public int StoreId { get; set; }
		public string? FoodName { get; set; }
		public DateTime Start { get; set; } = DateTime.Now;
        public DateTime End { get; set; } = DateTime.Now.AddDays(1);
    }
}
