using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("Food")]
    public class Food : BaseEntity<int>
    {
        public string FoodName { get; set; }
        public string ImageURL { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public decimal RateAverage { get; set; } = 0;
        public int TotalRate { get; set; } = 0;
        public int RatingCount { get; set; } = 0;
        public int CategoryId { get; set; }
        [ForeignKey(nameof(CategoryId))]
        public Category? Category { get; set; }
        public int StoreId { get; set; }
        [ForeignKey(nameof(StoreId))]   
        public Store? Store { get; set; }
        public ICollection<Comment>? Comments { get; set; }
        public ICollection<OrderDetail>? OrderDetails { get; set; }
        public ICollection<Inventory>? Inventories { get; set; }
        public ICollection<FlashSaleDetail>? FlashSaleDetails { get; set; }
    }
}
