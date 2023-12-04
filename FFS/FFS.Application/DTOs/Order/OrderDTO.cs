using FFS.Application.Entities.Enum;
using FFS.Application.Entities;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.DTOs.Order
{
    public class OrderDTO
    {
        public int Id { get; set; }
        public int? PaymentId { get; set; }
        public int StoreName { get; set; }
        public string CustomerName { get; set; }
        public string ShipperName { get; set; }
        public string Location { get; set; }
        public string? CancelReason { get; set; }
        public string? Note { get; set; }
        public string PhoneNumber { get; set; }
        public decimal TotalPrice { get; set; }
		public decimal ShipFee { get; set; }
        public OrderStatus OrderStatus { get; set; }
		public List<OrderDetailResponseDTO> orderDetails { get; set; }
	}
}
