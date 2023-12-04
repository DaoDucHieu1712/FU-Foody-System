using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.Store
{
	public class OrderStatistic
	{
		public OrderStatus OrderStatus { get; set; }
		public int? NumberOfOrder { get; set; }
	}
}
