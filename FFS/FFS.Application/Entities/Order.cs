﻿using System.ComponentModel.DataAnnotations.Schema;
using FFS.Application.Entities.Common;
using FFS.Application.Entities.Enum;

namespace FFS.Application.Entities
{
	[Table("Order")]
	public class Order : BaseEntity<int>
	{
		public int? PaymentId { get; set; }
		public string CustomerId { get; set; }
		[ForeignKey(nameof(CustomerId))]
		public ApplicationUser Customer { get; set; }
		public string? ShipperId { get; set; }
		[ForeignKey(nameof(ShipperId))]
		public ApplicationUser? Shipper { get; set; }
		[ForeignKey(nameof(PaymentId))]
		public Payment? Payment { get; set; }
		public OrderStatus OrderStatus { get; set; }
		public string? Location { get; set; }
		public string? PhoneNumber { get; set; }
		public string? Note { get; set; }
		public string? CancelReason { get; set; }
		public decimal TotalPrice { get; set; }
		public decimal? ShipFee { get; set; }
		public string? Distance { get; set; }
		public string? TimeShip { get; set; }

		public DateTime? ShipDate { get; set; }
		public ICollection<OrderDetail> OrderDetails { get; set; }
	}
}
