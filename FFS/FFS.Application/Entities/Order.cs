using FFS.Application.Entities.Common;
using FFS.Application.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Order")]
    public class Order : BaseEntity<int>
    {
        public int PaymentId { get; set; }
        public int StoreId { get; set; }
        [ForeignKey(nameof(StoreId))]
        public Store Store { get; set; }
        public string CustomerId { get; set; }
        [ForeignKey(nameof(CustomerId))]
        public ApplicationUser Customer { get; set; }   
        [ForeignKey(nameof(PaymentId))]
        public string ShipperId { get; set; }
        [ForeignKey(nameof(ShipperId))]
        public ApplicationUser Shipper { get; set; }
        public Payment Payment { get; set; }
        public OrderStatus OrderStatus { get;set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
