using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("FoodCombo")]
    public class FoodCombo : BaseEntity<int>
    {
        public int StoreId { get; set; }
        public int FoodId { get; set; }

        [ForeignKey(nameof(StoreId))]
        public Store Store { get; set; }
        [ForeignKey(nameof(FoodId))]
        public Food Food { get; set; }
    }
}
