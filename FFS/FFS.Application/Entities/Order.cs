using FFS.Application.Entities.Common;
using FFS.Application.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Order")]
    public class Order : BaseEntity<int>
    {
        public int? PaymentId { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public ApplicationUser Customer { get; set; }   
        [ForeignKey(nameof(PaymentId))]
        public string? ShipperId { get; set; }
        [ForeignKey(nameof(ShipperId))]
        public ApplicationUser? Shipper { get; set; }
        public Payment? Payment { get; set; }
        public OrderStatus OrderStatus { get;set; }
        public string Location { get; set; }
        public string? Note { get; set; }
        public string? CancelReason { get; set; }
        public decimal TotalPrice { get; set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
