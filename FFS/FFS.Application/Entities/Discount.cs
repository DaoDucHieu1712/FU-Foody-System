using FFS.Application.Entities.Common;
using FFS.Application.Entities.Enum;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Discount")]
    public class Discount : BaseEntity<int>
    { 
        public string Code { get; set; }
        public string? Description { get; set; }
        public int Percent { get; set; }
        public decimal ConditionPrice { get; set; }
        public Rank? Rank { get; set; }  
        public int Quantity { get; set; }
        public DateTime Expired { get; set; }
        public int StoreId { get; set; }
        [ForeignKey(nameof(StoreId))]
        public Store Store { get; set; }
    }
}
