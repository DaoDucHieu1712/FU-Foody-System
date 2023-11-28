namespace FFS.Application.DTOs.Admin
{
	public class FoodDetailStatistic
	{
		public string FoodName { get; set; }
		public decimal? RateAverage { get; set; }
		public int? RatingCount { get; set; }
		public int QuantityOfSell { get; set; }
	}
}
