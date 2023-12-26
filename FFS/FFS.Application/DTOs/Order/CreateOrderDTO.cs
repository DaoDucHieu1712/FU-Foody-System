using System.Text.Json.Serialization;
using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.Order
{
	public class CreateOrderDTO

	{
		[JsonIgnore]
		public int Id { get; set; }
		public int? PaymentId { get; set; }
		public string CustomerId { get; set; }
		public string? ShipperId { get; set; }
		public string? Location { get; set; }
		public string? PhoneNumber { get; set; }
		public string? CancelReason { get; set; }
		public string Note { get; set; }
		public decimal TotalPrice { get; set; }
		public decimal? ShipFee { get; set; }
		public string? Distance { get; set; }
		public string? TimeShip { get; set; }

		public OrderStatus OrderStatus { get; set; }
		public List<OrderDetailDTO> OrderDetails { get; set; }
	}
}
