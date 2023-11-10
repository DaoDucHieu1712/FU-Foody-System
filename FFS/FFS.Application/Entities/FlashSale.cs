using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    public class FlashSale : BaseEntity<int>
    {
        public int FoodId { get; set; }
        public int StoreId { get; set; }
        public int Percent { get; set; }
        public DateTime Start { get; set; }
        public DateTime End { get; set; }
        [ForeignKey(nameof(FoodId))]
        public Food Food { get; set; }
        [ForeignKey(nameof(StoreId))]
        public Store Store { get; set; }
    }
}
