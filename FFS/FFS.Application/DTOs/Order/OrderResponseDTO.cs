﻿using FFS.Application.Entities.Enum;

namespace FFS.Application.DTOs.Order
{
    public class OrderResponseDTO
    {
        public int Id { get; set; }
        public int? PaymentId { get; set; }
        public string? CustomerId { get; set; }
        public string? ShipperId { get; set; }
        public string? CustomerName { get; set; }
        public string? ShipperName { get; set; }
        public string? Location { get; set; }
        public string? CancelReason { get; set; }
        public string? Note { get; set; }
        public string? PhoneNumber { get; set; }
        public decimal TotalPrice { get; set; }
        public OrderStatus OrderStatus { get; set; }
    }
}
