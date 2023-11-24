using FFS.Application.Entities.Enum;
using System.Reflection.Metadata.Ecma335;

namespace FFS.Application.DTOs.Store
{
    public class DiscountDTO
    {
        public int? Id { get; set; }
        public int StoreId { get; set; }
        public string Code { get; set; }
        public string Description { get; set; }
        public int Percent { get; set; }
        public decimal ConditionPrice { get; set; }
        public Rank? Rank { get; set; }
        public int Quantity { get; set; }
        public DateTime Expired { get; set; }
        public bool? IsExpired { get; set; }
    }
}
