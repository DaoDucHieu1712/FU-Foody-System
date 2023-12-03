using FFS.Application.Entities.Enum;
using FFS.Application.Entities;
using Newtonsoft.Json;

namespace FFS.Application.DTOs.Order
{
    public class OrderRequestDTO
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
        public OrderStatus OrderStatus { get; set; }



	}
}
