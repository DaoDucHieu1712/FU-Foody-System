namespace FFS.Application.DTOs.QueryParametter
{
	public class FlashSaleParameter : QueryStringParameters
	{
		public DateTime Start { get; set; } = DateTime.MinValue;
		public DateTime End { get; set; } = DateTime.MaxValue;
	}
}
