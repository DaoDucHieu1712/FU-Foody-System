using FFS.Application.Entities.Common;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FFS.Application.Entities
{
    [Table("FlashSaleDetail")]
    public class FlashSaleDetail
    {
        public int FoodId { get; set; }
        public int FlashSaleId { get; set; }

        [ForeignKey(nameof(FoodId))]
        public Food? Food { get; set; }

        [ForeignKey(nameof(FlashSaleId))]
        public FlashSale? FlashSale { get; set; }

        public decimal? PriceAfterSale { get; set; }
        public int? SalePercent { get; set; }
        public int? NumberOfProductSale { get; set; }
    }
}
