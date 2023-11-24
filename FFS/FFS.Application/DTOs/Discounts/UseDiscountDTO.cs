namespace FFS.Application.DTOs.Discounts
{
	public class UseDiscountDTO
	{
		public string Code { get; set; }
		public string UserId { get; set; }
		public decimal? TotalPrice { get; set; }

	}
}
