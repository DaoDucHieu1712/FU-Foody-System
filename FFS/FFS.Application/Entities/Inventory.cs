using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Inventory")]
    public class Inventory : BaseEntity<int>
    {
        public int FoodId { get; set; }
        public int StoreId { get; set; }
        [ForeignKey(nameof(FoodId))]
        public Food Food { get; set; }
        [ForeignKey(nameof(StoreId))]
        public Store Store { get; set; }
        public int quantity { get; set; }
    }
}
