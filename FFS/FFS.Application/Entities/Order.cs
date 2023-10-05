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
        public string UserId { get; set; }
        [ForeignKey(nameof(UserId))]
        public ApplicationUser User { get; set; }   
        [ForeignKey(nameof(PaymentId))]
        public Payment Payment { get; set; }
        public OrderStatus OrderStatus { get;set; }
        public ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
