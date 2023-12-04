using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("OrderDetail")]
    public class OrderDetail : BaseEntity<int>
    {
        public int OrderId { get; set; }
        [ForeignKey(nameof(OrderId))]
        public Order Order { get; set; }
        public int StoreId { get; set; }
        [ForeignKey(nameof(StoreId))]
        public Store Store { get; set; }
        public int? FoodId { get; set; }
        [ForeignKey(nameof(FoodId))]
        public Food? Food { get; set; }
		public int? ComboId { get; set; }
		[ForeignKey(nameof(ComboId))]
		public Combo? Combo { get; set; }
        public int Quantity { get; set; }
        public decimal UnitPrice { get; set; }

		

	}
}
